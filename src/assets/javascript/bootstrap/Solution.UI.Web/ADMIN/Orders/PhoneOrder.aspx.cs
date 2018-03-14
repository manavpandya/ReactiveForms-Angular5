using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using System.Text;
using Solution.ShippingMethods;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class PhoneOrder : BasePage
    {
        Decimal SwatchQty = Decimal.Zero;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.MaintainScrollPositionOnPostBack = true;
            CommonOperations.RedirectWithSSL(true);
            if (!IsPostBack)
            {
                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                {//charge logic

                    Guid objGuid = Guid.NewGuid();
                    string stGuid = objGuid.ToString();
                    txtCOnfirmationID.Text = stGuid;
                }
                //TxtCardVerificationCode.TextMode = TextBoxMode.Password;
                // TxtCardVerificationCode.Attributes.Add("autocomplete", "off");

                aRelated.ImageUrl = "/App_Themes/" + Page.Theme + "/images/Add-to-cart.gif";
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/place-order.png";
                btnincompleteorder.ImageUrl = "/App_Themes/" + Page.Theme + "/images/in-complete-order.jpg";
                BtnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                popupContactClose.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
                btnGenerate.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/apply.png";
                btnStorecreaditapply.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/apply.png";
                txtBoxcaptureamount.Attributes.Add("readonly", "true");
                txtstorecreaditAmount.Attributes.Add("readonly", "true");
                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                {//charge logic
                    // rdoCreditCard.Attributes.Remove("AutoPostBack");
                    // rdoCreditCard.Attributes.Add("AutoPostBack", "false");
                    //rdoCreditCard.Checked = false;
                    rdoCreditCard.Checked = true;

                }
                else
                {
                    // rdoCreditCard.Attributes.Remove("AutoPostBack");
                    //  rdoCreditCard.Attributes.Add("AutoPostBack", "true");
                    rdoCreditCard.Checked = true;

                }

                if (Session["AdminID"] != null && Session["AdminID"].ToString() != "")
                {
                    Boolean IsSalesManager = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select ISNULL(isSalesManager,0) as isSalesManager from tb_Admin where AdminID = " + Session["AdminID"].ToString() + " and ISNULL(Deleted,0)=0 and ISNULL(Active,0)=1"));
                    if (IsSalesManager)
                        hdnIsSalesManager.Value = "0";
                    else
                        hdnIsSalesManager.Value = "1";
                }
                GetStoreList();
                GetCreditCardType();
                FillCountry();
                TxtEmail.Focus();
                Session["CustCouponCode"] = null;
                Session["CustCouponCodeDiscount"] = null;
                Session["CustCouponvalid"] = null;
                Session["Storecreaditamount"] = null;

                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                {
                    rdoCreditCard.Attributes.Add("onchange", "if(ValidationNotLogincc()){chkHeight();__doPostBack('ctl00$ContentPlaceHolder1$rdoCreditCard','');}else{ document.getElementById('ContentPlaceHolder1_rdoCreditCard').checked = false; return false; }");
                }
                if (Request.QueryString["CustID"] != null)
                {

                    string shippingMethod = "";
                    if (Request.QueryString["Ono"] != null)
                    {

                        try
                        {
                            OrderComponent objOrderComp = new OrderComponent();
                            tb_Order objtb_Order = new tb_Order();
                            objtb_Order = objOrderComp.GetOrderByOrderNumber(Convert.ToInt32(Request.QueryString["Ono"].ToString()));
                            TxtNotes.Text = objtb_Order.OrderNotes.ToString();
                            try
                            {
                                ddlcallref.Items.FindByText(objtb_Order.Callreference.ToString());
                            }
                            catch { }
                            // txtCouponCode.Text =Convert.ToString(objtb_Order.CouponCode.ToString());
                            if (!string.IsNullOrEmpty(objtb_Order.Storecreaditno))
                            {
                                txtstorecreaditno.Text = objtb_Order.Storecreaditno.ToString();
                            }


                            txtstorecreaditAmount.Text = String.Format("{0:0.00}", Convert.ToDecimal(objtb_Order.StorecreaditAmont.ToString()));

                            TxtShippingCost.Text = Convert.ToString(objtb_Order.OrderShippingCosts);
                            TxtDiscount.Text = Convert.ToString(objtb_Order.CustomDiscount);
                            if (Convert.ToDecimal(objtb_Order.StorecreaditAmont) != Convert.ToDecimal(objtb_Order.CustomDiscount))
                            {
                                Session["QtyDiscount"] = objtb_Order.CustomDiscount.ToString();
                            }
                            TxtTax.Text = Convert.ToString(objtb_Order.OrderTax);
                            if (!string.IsNullOrEmpty(objtb_Order.CouponCode))
                            {
                                Session["CustCouponCode"] = objtb_Order.CouponCode.ToString();
                                txtCouponCode.Text = objtb_Order.CouponCode.ToString();
                                HdnCustID.Value = objtb_Order.CustomerID.ToString();
                                btnGenerate_Click(null, null);
                                //Session["CustCouponCodeDiscount"] = objtb_Order.CouponDiscountAmount.ToString();
                            }
                            if (!string.IsNullOrEmpty(objtb_Order.StorecreaditAmont.ToString()))
                            {
                                Session["Storecreaditamount"] = objtb_Order.StorecreaditAmont.ToString();
                            }

                            TxtEmail.Text = objtb_Order.Email.ToString();
                            txtB_FName.Text = objtb_Order.BillingFirstName.ToString();
                            txtB_LName.Text = objtb_Order.BillingLastName.ToString();
                            txtB_Company.Text = objtb_Order.BillingCompany.ToString();
                            txtB_Add1.Text = objtb_Order.BillingAddress1.ToString();
                            txtB_Add2.Text = objtb_Order.BillingAddress2.ToString();
                            txtB_Suite.Text = objtb_Order.BillingSuite.ToString();
                            txtB_City.Text = objtb_Order.BillingCity.ToString();
                            shippingMethod = objtb_Order.ShippingMethod.ToString();
                            ddlB_Country.ClearSelection();
                            ddlB_Country.Items.FindByText(objtb_Order.BillingCountry.ToString()).Selected = true;
                            ddlB_Country_SelectedIndexChanged(null, null);
                            try
                            {
                                ddlB_State.ClearSelection();
                                ddlB_State.Items.FindByText(objtb_Order.BillingState.ToString()).Selected = true;
                            }
                            catch
                            {
                                txtB_OtherState.Text = objtb_Order.BillingState.ToString();
                                ddlB_State.Items.FindByText("Other").Selected = true; ;
                                txtB_OtherState.Visible = true;
                            }

                            txtB_Zip.Text = objtb_Order.BillingZip.ToString();
                            txtB_Phone.Text = objtb_Order.BillingPhone.ToString();


                            txtS_FName.Text = objtb_Order.ShippingFirstName.ToString();
                            txtS_LNAme.Text = objtb_Order.ShippingLastName.ToString();
                            txtS_Company.Text = objtb_Order.ShippingCompany.ToString();
                            txtS_Add1.Text = objtb_Order.ShippingAddress1.ToString();
                            txtS_Add2.Text = objtb_Order.ShippingAddress2.ToString();
                            txtS_Suite.Text = objtb_Order.ShippingSuite.ToString();
                            txtS_City.Text = objtb_Order.ShippingCity.ToString();
                            ddlS_Country.ClearSelection();
                            ddlS_Country.Items.FindByText(objtb_Order.ShippingCountry.ToString()).Selected = true;
                            ddlS_Country_SelectedIndexChanged(null, null);
                            try
                            {
                                ddlS_State.ClearSelection();
                                ddlS_State.Items.FindByText(objtb_Order.ShippingState.ToString()).Selected = true;
                            }
                            catch
                            {
                                txtS_OtherState.Text = objtb_Order.ShippingState.ToString();
                                ddlS_State.Items.FindByText("Other").Selected = true;
                                txtS_OtherState.Visible = true;
                            }

                            txtS_Zip.Text = objtb_Order.ShippingZip.ToString();
                            txtS_Phone.Text = objtb_Order.ShippingPhone.ToString();
                            if (!string.IsNullOrEmpty(objtb_Order.PaymentMethod.ToString()) && objtb_Order.PaymentMethod.ToString().ToString() == "other")
                            {
                                rdoCheque.Checked = true;
                                rdoCheque_CheckedChanged(null, null);
                                txtCheque.Text = objtb_Order.Referrer.ToString();
                            }
                            else
                            {
                                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && !Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                                {
                                    rdoCreditCard.Checked = true;
                                    rdoCreditCard_CheckedChanged(null, null);

                                }
                                if (!string.IsNullOrEmpty(objtb_Order.CardName.ToString()))
                                {
                                    TxtNameOnCard.Text = objtb_Order.CardName.ToString();
                                }
                                if (!string.IsNullOrEmpty(objtb_Order.CardType.ToString()))
                                {
                                    ddlCardType.ClearSelection();
                                    ddlCardType.Items.FindByText(objtb_Order.CardType.ToString()).Selected = true;
                                }
                                if (!string.IsNullOrEmpty(objtb_Order.CardVarificationCode.ToString()))
                                {
                                    TxtCardVerificationCode.Text = objtb_Order.CardVarificationCode.ToString();

                                }
                                if (!string.IsNullOrEmpty(objtb_Order.CardExpirationMonth))
                                {
                                    ddlMonth.ClearSelection();
                                    try
                                    {
                                        ddlMonth.Items.FindByText(objtb_Order.CardExpirationMonth.ToString()).Selected = true;
                                    }
                                    catch { }
                                }
                                if (!string.IsNullOrEmpty(objtb_Order.CardExpirationYear))
                                {
                                    ddlYear.ClearSelection();
                                    try
                                    {
                                        ddlYear.Items.FindByText(objtb_Order.CardExpirationYear.ToString()).Selected = true;
                                    }
                                    catch { }
                                }
                                if (!string.IsNullOrEmpty(objtb_Order.CardNumber.ToString()))
                                {
                                    //TxtCardNumber.Text = SecurityComponent.Decrypt(objtb_Order.CardNumber.ToString());
                                    string CardNumber = SecurityComponent.Decrypt(objtb_Order.CardNumber.ToString());
                                    if (CardNumber.Length > 4)
                                    {
                                        for (int i = 0; i < CardNumber.Length - 4; i++)
                                        {
                                            TxtCardNumber.Text += "*";
                                        }
                                        TxtCardNumber.Text += CardNumber.ToString().Substring(CardNumber.Length - 4);
                                        Session["CardNumber"] = CardNumber.ToString();
                                    }
                                    else
                                    {
                                        TxtCardNumber.Text = "";
                                    }


                                }
                            }
                        }
                        catch
                        {

                        }

                        //Session["CustCouponCode"]

                    }
                    if (Request.QueryString["saleorder"] == null)
                    {
                        Session["CustCouponCode"] = null;
                        Session["CustCouponCodeDiscount"] = null;
                        Session["Storecreaditamount"] = null;
                        Session["CustCouponvalid"] = null;
                        RemoveCart(Convert.ToInt32(Request.QueryString["CustID"].ToString()));
                        HdnCustID.Value = Request.QueryString["CustID"].ToString();
                        GetCustomerDetails(Convert.ToInt32(Request.QueryString["CustID"].ToString()));
                        GVShoppingCartItems.DataSource = null;
                        GVShoppingCartItems.DataBind();
                    }
                    else
                    {


                        HdnCustID.Value = Request.QueryString["CustID"].ToString();
                        if (Request.QueryString["Ono"] != null)
                        {
                            //if(Session["CustCouponCode"] != null)
                            //{

                            //}
                            //else
                            //{
                            //    Session["CustCouponCode"] = null;
                            //}
                            //if (Session["CustCouponCodeDiscount"] != null)
                            //{

                            //}
                            //else
                            //{
                            //    Session["CustCouponCodeDiscount"] = null;
                            //}
                            //if (Session["Storecreaditamount"] != null)
                            //{

                            //}
                            //else
                            //{
                            //    Session["Storecreaditamount"] = null;
                            //}
                        }
                        else
                        {
                            Session["CustCouponCode"] = null;
                            Session["CustCouponCodeDiscount"] = null;
                            Session["Storecreaditamount"] = null;
                            Session["CustCouponvalid"] = null;

                        }


                        if (Session["CustCouponCode"] != null && Session["CustCouponCode"].ToString() != "" && Session["CustCouponCode"].ToString().ToLower().Trim() != "pricematch")
                        {
                            CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET DiscountPrice =0 WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + HdnCustID.Value.ToString() + ")");
                        }
                        if (Request.QueryString["Ono"] != null)
                        {
                            GetCustomerDetailsWithorder(Convert.ToInt32(Request.QueryString["CustID"].ToString()));
                        }
                        else
                        {
                            GetCustomerDetails(Convert.ToInt32(Request.QueryString["CustID"].ToString()));
                        }
                        GetShippIngMethodByBillAddress();
                        BindCartInGrid();
                        BindShippingMethod();
                        if (!string.IsNullOrEmpty(shippingMethod))
                        {
                            for (int i = ddlShippingMethod.Items.Count - 1; i > 0; i--)
                            {
                                if (ddlShippingMethod.Items[i].Value.IndexOf(shippingMethod.ToString()) > -1)
                                {
                                    ddlShippingMethod.ClearSelection();
                                    ddlShippingMethod.Items[i].Selected = true;
                                    break;
                                }
                            }

                            if (ddlShippingMethod.Items.Count > 0) { ddlShippingMethod_SelectedIndexChanged(null, null); }
                        }
                        // BindFreightShippingMethod();
                    }
                }
                else
                {
                    HdnCustID.Value = "0";
                }
                if (Request.QueryString["searchlinksku"] != null)
                {

                    hdnsearchlinksku.Value = Request.QueryString["searchlinksku"].ToString();
                    aRelated_Click(null, null);

                }

            }
            else
            {
                BindCartInGrid();
                if (ddlB_State.SelectedValue.ToString().Trim() == "-11" && ddlS_State.SelectedValue.ToString().Trim() == "-11")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg1", "MakeBillingOtherVisible(); MakeShippingOtherVisible();", true);
                }
                else if (ddlB_State.SelectedValue.ToString() == "-11")
                {
                    if (chkAddress.Checked == true)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg1", "MakeBillingOtherVisible(); MakeShippingOtherVisible();", true);
                        txtS_OtherState.Visible = true;
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg2", "MakeBillingOtherVisible();", true);
                    }

                }
                else if (ddlS_State.SelectedValue.ToString() == "-11")
                {

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg3", "MakeShippingOtherVisible();", true);

                }
                //GetShippIngMethodByBillAddress();
                //BindShippingMethod();
            }
            if (Request.QueryString["StoreId"] != null)
            {
                ddlStore.SelectedValue = Request.QueryString["StoreId"].ToString();
                ddlStore.Enabled = false;
            }

            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            {//charge logic

                //trchargelogicdiv.Attributes.Add("style", "display:'';");
                //trchargelogiciframe.Attributes.Add("style", "display:'';");
                //trnameoncard.Attributes.Add("style", "display:none;");
                //trcardtype.Attributes.Add("style", "display:none;");
                //trcardnumber.Attributes.Add("style", "display:none;");
                //trexpirationdate.Attributes.Add("style", "display:none;");

                hdnischargelogic.Value = "1";
                //if(!IsPostBack)
                //{
                //rdoCreditCard.Attributes.Remove("AutoPostBack");
                //rdoCreditCard.Attributes.Add("AutoPostBack", "false");
                rdoCreditCard.AutoPostBack = false;
                // rdoCreditCard.Checked = false;
                //}



            }
            else
            {
                //trchargelogicdiv.Attributes.Add("style", "display:none;");
                //trchargelogiciframe.Attributes.Add("style", "display:none;");
                //trnameoncard.Attributes.Add("style", "display:'';");
                //trcardtype.Attributes.Add("style", "display:'';");
                //trcardnumber.Attributes.Add("style", "display:'';");
                //trexpirationdate.Attributes.Add("style", "display:'';");
                //if (!IsPostBack)
                //{
                rdoCreditCard.AutoPostBack = true;
                // rdoCreditCard.Attributes.Remove("AutoPostBack");
                // rdoCreditCard.Attributes.Add("AutoPostBack", "true");
                // rdoCreditCard.Checked = true;
                //}
                hdnischargelogic.Value = "0";

            }

        }

        /// <summary>
        /// Bind both Country Drop down list
        /// </summary>
        public void FillCountry()
        {
            ddlS_Country.Items.Clear();
            ddlB_Country.Items.Clear();

            CountryComponent objCountry = new CountryComponent();
            DataSet dscountry = new DataSet();
            dscountry = objCountry.GetAllCountries();

            if (dscountry != null && dscountry.Tables.Count > 0 && dscountry.Tables[0].Rows.Count > 0)
            {
                ddlB_Country.DataSource = dscountry.Tables[0];
                ddlB_Country.DataTextField = "Name";
                ddlB_Country.DataValueField = "CountryID";
                ddlB_Country.DataBind();
                ddlB_Country.Items.Insert(0, new ListItem("Select Country", "0"));
                //ddlBillcountry.Items.Insert(dscountry.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));

                ddlS_Country.DataSource = dscountry.Tables[0];
                ddlS_Country.DataTextField = "Name";
                ddlS_Country.DataValueField = "CountryID";
                ddlS_Country.DataBind();
                ddlS_Country.Items.Insert(0, new ListItem("Select Country", "0"));
                // ddlShipCounry.Items.Insert(dscountry.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));

            }
            else
            {
                ddlB_Country.DataSource = null;
                ddlB_Country.DataBind();
                ddlS_Country.DataSource = null;
                ddlS_Country.DataBind();
            }

            if (ddlB_Country.Items.FindByText("United States") != null)
            {
                ddlB_Country.Items.FindByText("United States").Selected = true;
            }
            if (ddlS_Country.Items.FindByText("United States") != null)
            {
                ddlS_Country.Items.FindByText("United States").Selected = true;
            }
            ddlB_Country_SelectedIndexChanged(null, null);
            ddlS_Country_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Removes the Cart Items
        /// </summary>
        /// <param name="CustId">int CustId</param>
        private void RemoveCart(Int32 CustId)
        {
            CommonComponent.ExecuteCommonData("DELETE FROM tb_ShoppingCartItems WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + CustId + ")");
        }

        /// <summary>
        /// Get Credit Card Type By StoreID
        /// </summary>
        private void GetCreditCardType()
        {
            CreditCardComponent objCreditcard = new CreditCardComponent();
            DataSet dsCard = new DataSet();
            ddlCardType.Items.Clear();
            dsCard = objCreditcard.GetAllCarTypeByStoreID(Convert.ToInt32(ddlStore.SelectedValue.ToString()));
            if (dsCard != null && dsCard.Tables.Count > 0 && dsCard.Tables[0].Rows.Count > 0)
            {
                ddlCardType.DataSource = dsCard;
                ddlCardType.DataTextField = "CardType";
                ddlCardType.DataValueField = "CardTypeID";
                ddlCardType.DataBind();
            }
            else
            {
                ddlCardType.DataSource = null;
                ddlCardType.DataBind();
            }
            ddlCardType.Items.Insert(0, new ListItem("Select Card Type", "0"));
            ddlCardType.SelectedIndex = 0;
            for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 10; i++)
                ddlYear.Items.Add(i.ToString());
        }

        /// <summary>
        /// Get Store List and bind into Store Drop down
        /// </summary>
        private void GetStoreList()
        {
            ddlStore.Items.Clear();
            DataSet dsStore = new DataSet();

            ddlStore.Items.Insert(0, new ListItem("Half Price Drapes", "1"));
            ddlStore.SelectedIndex = 0;

            //dsStore = StoreComponent.GetStoreList();

            //Only Selected Stores will Display not all ----------------------

            //dsStore = CommonComponent.GetCommonDataSet("Select * from tb_store where ISNULL(deleted,0)=0");
            //if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            //{
            //    ddlStore.DataSource = dsStore;
            //    ddlStore.DataValueField = "StoreID";
            //    ddlStore.DataTextField = "StoreName";
            //    ddlStore.DataBind();
            //}
            //else
            //{
            //    ddlStore.DataSource = null;
            //    ddlStore.DataBind();
            //}
            ////ddlStore.Items.Insert(0, new ListItem("All", "0"));
            //if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            //{
            //    ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            //}
            //else
            //{
            //    ddlStore.SelectedIndex = 0;
            //}
        }

        /// <summary>
        /// Get Customer Details by CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 Customer ID</param>
        private void GetCustomerDetails(Int32 CustomerID)
        {
            try
            {
                DataSet dsCustomer = new DataSet();
                CustomerComponent objCust = new CustomerComponent();
                dsCustomer = objCust.GetCustomerDetails(CustomerID);

                //Billing Address


                if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Email"].ToString()))
                    {

                        TxtEmail.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());
                    }

                    txtB_FName.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString());
                    txtB_LName.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString());
                    txtB_Company.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Company"].ToString());

                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["IsRegistered"].ToString()))
                    {

                        chkisregister.Checked = Convert.ToBoolean(dsCustomer.Tables[0].Rows[0]["IsRegistered"].ToString());
                        hdnisregestered.Value = dsCustomer.Tables[0].Rows[0]["IsRegistered"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Customertype"].ToString()))
                    {
                        ddlcustomertype.SelectedValue = dsCustomer.Tables[0].Rows[0]["Customertype"].ToString();
                    }


                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Address1"].ToString()))
                    {

                        txtB_Add1.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address1"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Address2"].ToString()))
                    {
                        txtB_Add2.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address2"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Suite"].ToString()))
                    {
                        txtB_Suite.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Suite"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["City"].ToString()))
                    {
                        txtB_City.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["City"].ToString());
                    }
                    ddlB_Country.ClearSelection();
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Country"].ToString()))
                    {
                        ddlB_Country.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Country"].ToString());
                        ddlB_Country_SelectedIndexChanged(null, null);
                    }
                    ddlB_State.ClearSelection();
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["State"].ToString()))
                    {
                        try
                        {
                            ddlB_State.Items.FindByText(Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"].ToString())).Selected = true;
                        }
                        catch
                        {
                            ddlB_State.Items.FindByText("Other").Selected = true;
                            txtB_OtherState.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"].ToString());
                        }
                    }

                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString()))
                    {
                        txtB_Zip.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Phone"].ToString()))
                    {
                        txtB_Phone.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Phone"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["NameOnCard"].ToString()))
                    {
                        TxtNameOnCard.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["NameOnCard"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardTypeID"].ToString()))
                    {
                        ddlCardType.ClearSelection();
                        ddlCardType.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardTypeID"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardVerificationCode"].ToString()))
                    {
                        TxtCardVerificationCode.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardVerificationCode"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString()))
                    {
                        string CardNumber = SecurityComponent.Decrypt(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString());
                        if (CardNumber.Length > 4)
                        {
                            for (int i = 0; i < CardNumber.Length - 4; i++)
                            {
                                TxtCardNumber.Text += "*";
                            }
                            TxtCardNumber.Text += CardNumber.ToString().Substring(CardNumber.Length - 4);
                            Session["CardNumber"] = CardNumber.ToString();
                        }
                        else
                        {
                            TxtCardNumber.Text = "";
                        }
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardExpirationMonth"].ToString()))
                    {
                        ddlMonth.ClearSelection();
                        ddlMonth.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardExpirationMonth"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardExpirationYear"].ToString()))
                    {
                        ddlYear.ClearSelection();
                        ddlYear.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardExpirationYear"].ToString());
                    }
                }


                //Shipping Address

                if (dsCustomer != null && dsCustomer.Tables.Count > 1 && dsCustomer.Tables[1].Rows.Count > 0)
                {
                    Session["ShipppCiutomer"] = dsCustomer;
                    txtS_FName.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["FirstName"].ToString());
                    txtS_LNAme.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["LastName"].ToString());
                    txtS_Company.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Company"].ToString());
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Address1"].ToString()))
                    {
                        txtS_Add1.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address1"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Address2"].ToString()))
                    {
                        txtS_Add2.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address2"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Suite"].ToString()))
                    {
                        txtS_Suite.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Suite"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["City"].ToString()))
                    {
                        txtS_City.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["City"].ToString());
                    }
                    ddlS_Country.ClearSelection();
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Country"].ToString()))
                    {
                        ddlS_Country.SelectedValue = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Country"].ToString());
                        ddlS_Country_SelectedIndexChanged(null, null);
                    }
                    ddlS_State.ClearSelection();
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["State"].ToString()))
                    {
                        try
                        {
                            ddlS_State.Items.FindByText(Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"].ToString())).Selected = true;
                        }
                        catch
                        {
                            ddlS_State.Items.FindByText("Other").Selected = true;
                            txtS_OtherState.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"].ToString());
                        }
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString()))
                    {
                        txtS_Zip.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Phone"].ToString()))
                    {
                        txtS_Phone.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Phone"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());

            }

        }
        private void GetCustomerDetailsWithorder(Int32 CustomerID)
        {
            try
            {
                DataSet dsCustomer = new DataSet();
                CustomerComponent objCust = new CustomerComponent();
                dsCustomer = objCust.GetCustomerDetails(CustomerID);

                //Billing Address


                if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
                {

                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["IsRegistered"].ToString()))
                    {

                        chkisregister.Checked = Convert.ToBoolean(dsCustomer.Tables[0].Rows[0]["IsRegistered"].ToString());
                        hdnisregestered.Value = dsCustomer.Tables[0].Rows[0]["IsRegistered"].ToString();
                    }


                }


                //Shipping Address

                if (dsCustomer != null && dsCustomer.Tables.Count > 1 && dsCustomer.Tables[1].Rows.Count > 0)
                {
                    Session["ShipppCiutomer"] = dsCustomer;

                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());

            }

        }

        /// <summary>
        /// Binds the Cart in Grid
        /// </summary>
        private void BindCartInGrid()
        {
            Int32 CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());
            Int32 CustID = CustomerID;
            Session["QtyDiscount"] = null;
            Session["QtyDiscount1"] = null;
            if (GVShoppingCartItems.Rows.Count > 0)
            {
                for (int i = 0; i < GVShoppingCartItems.Rows.Count; i++)
                {
                    Label lblCustomerCartId = (Label)GVShoppingCartItems.Rows[i].FindControl("lblCustomerCartId");
                    Label lblProductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductID");
                    Label lblVariantNames = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantNames");
                    Label lblVariantValues = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantValues");
                    Label lblOrginalDiscountPrice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblOrginalDiscountPrice");
                    TextBox txtcouponprice = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtcouponprice");
                    decimal DiscountPrice = 0;
                    if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
                    {
                        decimal.TryParse(lblOrginalDiscountPrice.Text.ToString().Trim(), out DiscountPrice);

                    }
                    if ((Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null && Session["CustCouponCode"].ToString() != "" && Session["CustCouponCode"].ToString().ToLower().Trim() == "pricematch"))
                    {

                        double dis = Convert.ToDouble(CommonComponent.GetScalarCommonData("select isnull(DiscountPrice,0) as DiscountPrice from tb_ShoppingCartItems where CustomCartID=" + lblCustomerCartId.Text + ""));

                        txtcouponprice.Text = String.Format("{0:0.00}", Convert.ToDecimal(dis));
                        lblOrginalDiscountPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(dis));
                        decimal.TryParse(lblOrginalDiscountPrice.Text.ToString().Trim(), out DiscountPrice);


                    }
                    TextBox txtNotes = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtNotes");
                    if (!string.IsNullOrEmpty(lblCustomerCartId.Text.Trim()))
                    {
                        string strnotes = "";
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["CustCouponCode"])) && !string.IsNullOrEmpty(Convert.ToString(Session["CustCouponCodeDiscount"])))
                        {

                            if (txtNotes.Text.ToString().ToLower().Contains("coupon code"))
                            {
                                //string[] strNotes = txtNotes.Text.ToString().Replace("'", "''").Split("\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                //strnotes = strNotes[0];
                                strnotes = txtNotes.Text.ToString();
                                string strnotes1 = txtNotes.Text.ToString().Substring(txtNotes.Text.ToString().IndexOf("\""), txtNotes.Text.ToString().LastIndexOf("\"") - txtNotes.Text.ToString().IndexOf("\"") + 1);
                                strnotes = strnotes.Replace(strnotes1, " \"" + "Coupon Code # " + Session["CustCouponCode"].ToString() + " By " + Session["AdminName"].ToString() + "\"");
                                txtNotes.Text = strnotes.Trim();


                            }
                            else
                            {
                                txtNotes.Text = txtNotes.Text.Trim() + " \"" + "Coupon Code # " + Session["CustCouponCode"].ToString() + " By " + Session["AdminName"].ToString() + "\"";
                            }

                        }
                        else
                        {
                            if (txtNotes.Text.ToString().ToLower().Contains("coupon code"))
                            {
                                //string[] strNotes = txtNotes.Text.ToString().Replace("'", "''").Split("\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                //strnotes = strNotes[0];
                                strnotes = txtNotes.Text.ToString();
                                string strnotes1 = txtNotes.Text.ToString().Substring(txtNotes.Text.ToString().IndexOf("\""), txtNotes.Text.ToString().LastIndexOf("\"") - txtNotes.Text.ToString().IndexOf("\"") + 1);
                                strnotes = strnotes.Replace(strnotes1, "");
                                txtNotes.Text = strnotes.Trim();

                            }

                        }

                        CommonComponent.ExecuteCommonData("Update tb_ShoppingCartItems set Notes='" + txtNotes.Text.ToString().Replace("'", "''") + "',DiscountPrice=" + DiscountPrice + " Where CustomCartID=" + lblCustomerCartId.Text.ToString() + " and ProductID=" + lblProductID.Text.Trim() + " and VariantNames='" + lblVariantNames.Text.ToString() + "' and VariantValues='" + lblVariantValues.Text.ToString() + "'");

                    }
                }
            }

            String strswatchQtyy = "";
            if (AppLogic.AppConfigs("SwatchMaxlength") != null && AppLogic.AppConfigs("SwatchMaxlength").ToString() != "")
            {
                //strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + CustomerID + " Order By ShoppingCartID DESC) "));
                //if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
                //{
                SwatchQty = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                //}
            }
            DataSet DsCItems = new DataSet();
            DsCItems = ShoppingCartComponent.GetPhoneOrderCartDetailByCustomerID(CustID);

            DataTable dt = new DataTable();
            if (DsCItems != null && DsCItems.Tables.Count > 0 && DsCItems.Tables[0].Rows.Count > 0)
            {
                string strProduct = "";
                for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                {
                    strProduct += Convert.ToString(DsCItems.Tables[0].Rows[i]["Productid"].ToString()) + ",";
                }

                ViewState["AllProductsSwatch"] = null;
                int SwatchCnt = 0;
                if (!string.IsNullOrEmpty(strProduct.Trim()) && strProduct.Length > 0)
                {
                    strProduct = strProduct.Substring(0, strProduct.Length - 1);
                    SwatchCnt = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect Count(ProductID) as Isfreefabricswatch from tb_product where ProductID in(" + strProduct + ") and ItemType='Swatch'  and StoreID = 1"));
                    if (DsCItems.Tables[0].Rows.Count == SwatchCnt)
                    {
                        ViewState["AllProductsSwatch"] = "1";
                    }
                }

                DataView dv = new DataView();
                dv = DsCItems.Tables[0].DefaultView;
                dv.Sort = " CustomCartID asc,ProductType Desc ";
                dt = dv.ToTable();
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                trOrderDetails.Style.Add("display", "block");

                GVShoppingCartItems.DataSource = dt;
                GVShoppingCartItems.DataBind();

                // strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + CustomerID + " Order By ShoppingCartID DESC) "));
                if (AppLogic.AppConfigs("SwatchMaxlength") != null && AppLogic.AppConfigs("SwatchMaxlength").ToString() != "")
                {
                    //if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
                    //{
                    SwatchQty = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                    //}
                }
                for (int i = 0; i < GVShoppingCartItems.Rows.Count; i++)
                {
                    Label lblCustomerCartId = (Label)GVShoppingCartItems.Rows[i].FindControl("lblCustomerCartId");
                    Label lblProductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductID");
                    Label lblVariantNames = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantNames");
                    Label lblVariantValues = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantValues");
                    Label lblOrginalDiscountPrice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblOrginalDiscountPrice");

                    decimal DiscountPrice = 0;
                    if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
                    {
                        decimal.TryParse(lblOrginalDiscountPrice.Text.ToString().Trim(), out DiscountPrice);
                    }
                    if (!string.IsNullOrEmpty(lblCustomerCartId.Text.Trim()))
                    {
                        CommonComponent.ExecuteCommonData("Update tb_ShoppingCartItems set DiscountPrice=" + DiscountPrice + " Where CustomCartID=" + lblCustomerCartId.Text.ToString() + " and ProductID=" + lblProductID.Text.Trim() + " and VariantNames='" + lblVariantNames.Text.ToString() + "' and VariantValues='" + lblVariantValues.Text.ToString() + "'");
                    }
                }
                try
                {
                    decimal subtotal = 0;
                    if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null) && Session["CustCouponCode"].ToString().ToLower().Trim() != "pricematch")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string StrSaleprice = "0";
                            string StrQuantity = "0";

                            if (dr["IsProductType"].ToString() == "0")
                            {
                                Int32 Qty = 0;
                                Decimal pp = 0;
                                Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + " and ItemType='Swatch' "));
                                if (SwatchQty > Decimal.Zero)
                                {

                                    if (Isorderswatch == 1)
                                    {
                                        pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + dr["Quantity"].ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + ""));

                                        if (Convert.ToDecimal(pp) >= SwatchQty)
                                        {
                                            pp = (pp - SwatchQty) / Convert.ToDecimal(dr["Quantity"].ToString());
                                            StrSaleprice = pp.ToString();
                                            StrQuantity = dr["Quantity"].ToString();
                                            SwatchQty = Decimal.Zero;
                                        }
                                        else
                                        {
                                            if (SwatchQty > Decimal.Zero)
                                            {

                                                StrSaleprice = "0.00";
                                                StrQuantity = dr["Quantity"].ToString();
                                                SwatchQty = SwatchQty - pp;
                                            }
                                            else
                                            {
                                                StrSaleprice = pp.ToString();
                                                StrQuantity = dr["Quantity"].ToString();
                                            }

                                        }
                                    }
                                    else
                                    {
                                        StrQuantity = dr["Quantity"].ToString();
                                        StrSaleprice = dr["SalePrice"].ToString().Trim();
                                    }
                                }
                                else
                                {
                                    if (Isorderswatch == 1)
                                    {
                                        pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + dr["Quantity"].ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + ""));
                                        pp = pp / Convert.ToDecimal(dr["Quantity"].ToString());
                                        StrSaleprice = pp.ToString();
                                        StrQuantity = dr["Quantity"].ToString();
                                    }
                                    else
                                    {
                                        StrQuantity = dr["Quantity"].ToString();
                                        StrSaleprice = dr["SalePrice"].ToString().Trim();
                                    }
                                }
                            }
                            else
                            {
                                StrQuantity = dr["Quantity"].ToString();
                                StrSaleprice = dr["SalePrice"].ToString().Trim();
                            }

                            dr["DiscountPercent"] = Convert.ToDecimal(Session["CustCouponCodeDiscount"].ToString());
                            decimal DiscountPrice = 0, OrgiPrice = 0;
                            decimal.TryParse(dr["DiscountPercent"].ToString().Trim(), out DiscountPrice);
                            decimal.TryParse(StrSaleprice.ToString().Trim(), out OrgiPrice);

                            if (Session["CustCouponvalid"] != null && Session["CustCouponvalid"].ToString() == "1")
                            {
                                #region CheckMembership Discount
                                if (HdnCustID.Value != null)
                                {
                                    String ProductId = dr["Productid"].ToString();
                                    decimal ProductDiscount = 0;
                                    decimal CategoryDiscount = 0;
                                    decimal NewDiscount = 0;
                                    decimal DesPrice = 0;

                                    //String CategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ""));
                                    // String ParentCategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select cast(ParentCategoryID as nvarchar(500))+',' from tb_CategoryMapping WHERE CategoryID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ") FOR XML PATH('')"));

                                    String ParentCategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select cast(ParentCategoryID as nvarchar(500))+',' from tb_CategoryMapping WHERE CategoryID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where isnull(p.IsSaleclearance,0)=0  and p.ProductID not in (select top 1 ProductID  from tb_ProductVariantValue where ProductID=" + ProductId + " and isnull(OnSale,0)=1 and cast(OnSaleFromdate as date)<=cast(GETDATE() as date) and cast(OnSaleTodate as date)>=cast(GETDATE() as date)) and pc.ProductID=" + ProductId + ") FOR XML PATH('')"));

                                    if (ParentCategoryId.Length > 0)
                                    {
                                        ParentCategoryId = ParentCategoryId.Substring(0, ParentCategoryId.Length - 1);
                                    }
                                    else
                                    {
                                        ParentCategoryId = "0";
                                    }

                                    //ProductDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT ISNULL(md.Discount,0) AS ProductDiscount "
                                    //+ " FROM dbo.tb_MembershipDiscount md Left OUTER JOIN dbo.tb_Product Prod ON Prod.ProductID =md.DiscountObjectID " +
                                    //" WHERE md.CustID='" + HdnCustID.Value.ToString() + "' AND md.DiscountType='product' AND md.StoreID= " + ddlStore.SelectedValue.ToString() + " AND md.DiscountObjectID=" + ProductId + ""));
                                    //if (ProductDiscount <= 0)
                                    //{
                                    //    CategoryDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT top 1 md.Discount AS CategoryDiscount "
                                    //        + " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                                    //        + "WHERE md.DiscountType='category' AND  md.CustID= " + HdnCustID.Value.ToString() + "  AND md.storeid=" + ddlStore.SelectedValue.ToString() + " AND (md.DiscountObjectID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ") or md.DiscountObjectID in (" + ParentCategoryId + "))"));
                                    //    //    Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT md.Discount AS CategoryDiscount "
                                    //    //+ " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                                    //    //+ "WHERE md.DiscountType='category' AND md.CustID= " + HdnCustID.Value.ToString() + "  AND md.storeid=" + ddlStore.SelectedValue.ToString() + " AND md.DiscountObjectID=" + CategoryId + " "));
                                    //    DiscountPrice = CategoryDiscount;
                                    //}

                                    ProductDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT ISNULL(md.Discount,0) AS ProductDiscount "
                                   + " FROM dbo.tb_MembershipDiscount md Left OUTER JOIN dbo.tb_Product Prod ON Prod.ProductID =md.DiscountObjectID " +
                                   " WHERE md.CustID='" + HdnCustID.Value.ToString() + "' AND md.DiscountType='product' AND md.StoreID= " + ddlStore.SelectedValue.ToString() + " AND md.DiscountObjectID=" + ProductId + " and isnull(Prod.IsSaleclearance,0)=0 and Prod.ProductID not in (select top 1 ProductID  from tb_ProductVariantValue where isnull(OnSale,0)=1 and cast(OnSaleFromdate as date)<=cast(GETDATE() as date) and cast(OnSaleTodate as date)>=cast(GETDATE() as date) and ProductID=" + ProductId + " ) "));
                                    if (ProductDiscount <= 0)
                                    {
                                        CategoryDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT top 1 md.Discount AS CategoryDiscount "
                                            + " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                                            + "WHERE md.DiscountType='category' AND  md.CustID= " + HdnCustID.Value.ToString() + "  AND md.storeid=" + ddlStore.SelectedValue.ToString() + " AND (md.DiscountObjectID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where isnull(p.IsSaleclearance,0)=0  and p.ProductID not in (select top 1 ProductID  from tb_ProductVariantValue where ProductID=" + ProductId + " and isnull(OnSale,0)=1 and cast(OnSaleFromdate as date)<=cast(GETDATE() as date) and cast(OnSaleTodate as date)>=cast(GETDATE() as date)) and pc.ProductID=" + ProductId + ") or md.DiscountObjectID in (" + ParentCategoryId + "))"));
                                        //    Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT md.Discount AS CategoryDiscount "
                                        //+ " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                                        //+ "WHERE md.DiscountType='category' AND md.CustID= " + HdnCustID.Value.ToString() + "  AND md.storeid=" + ddlStore.SelectedValue.ToString() + " AND md.DiscountObjectID=" + CategoryId + " "));
                                        DiscountPrice = CategoryDiscount;
                                    }
                                    else
                                    {
                                        DiscountPrice = ProductDiscount;
                                    }
                                    decimal DicPrice = 0, TempDis = 0;
                                    if (DiscountPrice > 0 && DiscountPrice <= 99)
                                    {
                                        TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                        DicPrice = OrgiPrice - TempDis;
                                        if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                        {
                                            subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                        }
                                    }
                                    else if (DiscountPrice >= 100)
                                    {
                                        TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                        DicPrice = TempDis;
                                        if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                        {
                                            subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                        }
                                    }
                                    else
                                    {
                                        if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                        {
                                            subtotal = subtotal + (Convert.ToDecimal(StrSaleprice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                        }
                                    }

                                }
                                #endregion
                            }
                            else
                            {


                                String strCategory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidForCategory FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CustCouponCode"].ToString() + "'"));
                                string srchildcategory = "";
                                try
                                {
                                    //if (!string.IsNullOrEmpty(strCategory))
                                    //{
                                    // srchildcategory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT cast(categoryId as nvarchar(50))+',' FROM tb_Category WHERE isnull(active,0)=1 and isnull(storeid,0)=1 and isnull(deleted,0)=0 and Categoryid in (SELECT Categoryid FROM tb_CategoryMapping WHERE ParentCategoryID in (" + strCategory + ")) FOR XML PATH('')"));
                                    //}
                                    //if (srchildcategory.Length > 1)
                                    //{
                                    //    strCategory = srchildcategory + strCategory;
                                    //}
                                }
                                catch
                                {

                                }

                                String strCategoryPercantage = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT CategoryPercentage FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CustCouponCode"].ToString() + "'"));
                                String strProduct = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidforProduct FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CustCouponCode"].ToString() + "'"));
                                if (!string.IsNullOrEmpty(strCategory))
                                {
                                    DataSet dspp = new DataSet();
                                    string strquery = "SELECT ProductId,1749 as CategoryId FROM tb_ProductVariantValue WHERE ISNULL(tb_ProductVariantValue.VarActive,0)=1 AND (CAST(OnSaleFromdate AS DATE) <=  CAST(GETDATE() AS DATE) AND	CAST(OnSaleTodate AS DATE)	>=	CAST(GETDATE() AS DATE)) AND ISNULL(OnSale,0)=1 and ProductID=" + dr["Productid"].ToString() + " UNION SELECT  ProductId,1749 as CategoryId  FROM tb_product where  isnull(IsSaleclearance,0)=1 and ProductID=" + dr["Productid"].ToString() + " UNION ";
                                    dspp = CommonComponent.GetCommonDataSet(strquery + " SELECT ProductId,CategoryId FROM tb_ProductCategory WHERE ProductId=" + dr["Productid"].ToString() + "");
                                    Decimal disccat = decimal.Zero;
                                    if (dspp != null && dspp.Tables.Count > 0 && dspp.Tables[0].Rows.Count > 0)
                                    {
                                        decimal DicPrice = 0, TempDis = 0;
                                        string[] pers = { "" };
                                        string[] cats = { "" };
                                        try
                                        {
                                            bool iadded = false;
                                            for (int icc = 0; icc < dspp.Tables[0].Rows.Count; icc++)
                                            {
                                                if (!string.IsNullOrEmpty(strCategoryPercantage))
                                                {
                                                    cats = strCategory.Split(',');
                                                    pers = strCategoryPercantage.Split(',');
                                                    for (int co = 0; co < cats.Length; co++)
                                                    {
                                                        if (dspp.Tables[0].Rows[icc]["categoryId"].ToString() == cats[co].ToString().Trim())
                                                        {

                                                            Decimal.TryParse(pers[co].ToString(), out disccat);
                                                            iadded = true;
                                                        }
                                                    }
                                                }

                                                if (iadded == false)
                                                {
                                                    DataSet dsold = new DataSet();
                                                    dsold = CommonComponent.GetCommonDataSet("WITH Emp_CTE AS (SELECT tb_CategoryMapping.CategoryID,tb_CategoryMapping.ParentCategoryID FROM tb_Category INNER JOIN tb_CategoryMapping on tb_Category.CategoryID=tb_CategoryMapping.CategoryID WHERE tb_CategoryMapping.CategoryID=" + dspp.Tables[0].Rows[icc]["categoryId"].ToString() + " and StoreID=1 and isnull(active,0)=1 and isnull(deleted,0)=0  and  Name not like '%swatch%' UNION ALL SELECT tb_CategoryMapping.CategoryID,tb_CategoryMapping.ParentCategoryID FROM tb_Category INNER JOIN tb_CategoryMapping on tb_Category.CategoryID=tb_CategoryMapping.CategoryID INNER JOIN Emp_CTE on Emp_CTE.ParentCategoryID=tb_CategoryMapping.CategoryID WHERE   StoreID=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and  Name not like '%swatch%') SELECT CategoryID,ParentCategoryID FROM Emp_CTE WHERE isnull(ParentCategoryID,0)<>0 ");

                                                    if (dsold != null && dsold.Tables.Count > 0 && dsold.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int idd = 0; idd < dsold.Tables[0].Rows.Count; idd++)
                                                        {
                                                            if (!string.IsNullOrEmpty(strCategoryPercantage))
                                                            {
                                                                cats = strCategory.Split(',');
                                                                pers = strCategoryPercantage.Split(',');
                                                                for (int co = 0; co < cats.Length; co++)
                                                                {
                                                                    if (dsold.Tables[0].Rows[idd]["ParentCategoryID"].ToString() == cats[co].ToString().Trim())
                                                                    {
                                                                        if (Convert.ToDecimal(pers[co].ToString()) > Convert.ToDecimal(disccat))
                                                                        {
                                                                            Decimal.TryParse(pers[co].ToString(), out disccat);

                                                                        }
                                                                        //break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }


                                            }
                                        }
                                        catch
                                        {

                                        }
                                        if (disccat > Decimal.Zero)
                                        {

                                        }
                                        else
                                        {
                                            disccat = DiscountPrice;
                                        }
                                        if (disccat > 1 && disccat <= 99)
                                        {
                                            TempDis = Convert.ToDecimal((OrgiPrice * disccat) / 100);
                                            DicPrice = OrgiPrice - TempDis;
                                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                            {
                                                subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                            }
                                        }
                                        else if (disccat >= 100)
                                        {
                                            TempDis = Convert.ToDecimal((OrgiPrice * disccat) / 100);
                                            DicPrice = TempDis;
                                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                            {
                                                subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                            }
                                        }
                                        else
                                        {
                                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                            {
                                                subtotal = subtotal + (Convert.ToDecimal(StrSaleprice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(strProduct))
                                        {
                                            strProduct = "," + strProduct.Replace(" ", "") + ",";
                                            if (strProduct.IndexOf("," + dr["Productid"].ToString() + ",") > -1)
                                            {

                                                decimal DicPrice = 0, TempDis = 0;
                                                if (DiscountPrice > 0 && DiscountPrice <= 99)
                                                {
                                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                                    DicPrice = OrgiPrice - TempDis;
                                                    if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                                    {
                                                        subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                                    }
                                                }
                                                else if (DiscountPrice >= 100)
                                                {
                                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                                    DicPrice = TempDis;
                                                    if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                                    {
                                                        subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                                    }
                                                }
                                                else
                                                {
                                                    if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                                    {
                                                        subtotal = subtotal + (Convert.ToDecimal(StrSaleprice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                                {
                                                    subtotal = subtotal + (Convert.ToDecimal(StrSaleprice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                            {
                                                subtotal = subtotal + (Convert.ToDecimal(StrSaleprice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                            }
                                        }
                                    }
                                }
                                else if (!string.IsNullOrEmpty(strProduct))
                                {
                                    strProduct = "," + strProduct.Replace(" ", "") + ",";

                                    if (strProduct.IndexOf("," + dr["Productid"].ToString() + ",") > -1)
                                    {
                                        decimal DicPrice = 0, TempDis = 0;
                                        if (DiscountPrice > 0 && DiscountPrice <= 99)
                                        {
                                            TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                            DicPrice = OrgiPrice - TempDis;
                                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                            {
                                                subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                            }
                                        }
                                        else if (DiscountPrice >= 100)
                                        {
                                            TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                            DicPrice = TempDis;
                                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                            {
                                                subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                            }
                                        }
                                        else
                                        {
                                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                            {
                                                subtotal = subtotal + (Convert.ToDecimal(StrSaleprice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                        {
                                            subtotal = subtotal + (Convert.ToDecimal(StrSaleprice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                        }
                                    }
                                }
                                else
                                {
                                    if (DiscountPrice > 0)
                                    {
                                        decimal DicPrice = 0, TempDis = 0;
                                        if (DiscountPrice > 0 && DiscountPrice <= 99)
                                        {
                                            TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                            DicPrice = OrgiPrice - TempDis;
                                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                            {
                                                subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                            }
                                        }
                                        else if (DiscountPrice >= 100)
                                        {
                                            TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                            DicPrice = TempDis;
                                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                            {
                                                subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                            }
                                        }
                                        else
                                        {
                                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                            {
                                                subtotal = subtotal + (Convert.ToDecimal(StrSaleprice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                                        {
                                            subtotal = subtotal + (Convert.ToDecimal(StrSaleprice.ToString()) * Convert.ToDecimal(StrQuantity.ToString()));
                                        }
                                    }
                                }

                            }

                        }
                    }
                    else if (Session["CustCouponCode"] != null && Session["CustCouponCode"].ToString() != "" && Session["CustCouponCode"].ToString().ToLower().Trim() == "pricematch")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                            {
                                if (dr["IsProductType"].ToString() == "0")
                                {
                                    Int32 Qty = 0;
                                    Decimal pp = 0;
                                    Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + " and ItemType='Swatch' "));
                                    if (SwatchQty > Decimal.Zero)
                                    {

                                        if (Isorderswatch == 1)
                                        {
                                            pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + dr["Quantity"].ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + ""));
                                            if (Convert.ToDecimal(pp) >= SwatchQty)
                                            {
                                                Qty = Convert.ToInt32(dr["Quantity"].ToString());
                                                pp = (pp - SwatchQty) / Convert.ToDecimal(dr["Quantity"].ToString());
                                                SwatchQty = Decimal.Zero;
                                            }
                                            else
                                            {
                                                Qty = Convert.ToInt32(dr["Quantity"].ToString());
                                                if (SwatchQty > Decimal.Zero)
                                                {

                                                    SwatchQty = SwatchQty - Convert.ToDecimal(pp);
                                                    pp = Decimal.Zero;
                                                }
                                                else
                                                {
                                                    pp = pp / Convert.ToDecimal(dr["Quantity"].ToString());

                                                }
                                            }
                                            subtotal = subtotal + (Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                                        }
                                        else
                                        {
                                            subtotal = subtotal + (Convert.ToDecimal(dr["DiscountPrice"].ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                                        }
                                    }
                                    else
                                    {
                                        if (Isorderswatch == 1)
                                        {
                                            pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + dr["Quantity"].ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + ""));
                                            pp = pp / Convert.ToDecimal(dr["Quantity"].ToString());
                                            subtotal = subtotal + (Convert.ToDecimal(pp) * Convert.ToDecimal(dr["Quantity"].ToString()));
                                        }
                                        else
                                        {
                                            subtotal = subtotal + (Convert.ToDecimal(dr["DiscountPrice"].ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                                        }

                                    }
                                }
                                else
                                {
                                    subtotal = subtotal + (Convert.ToDecimal(dr["DiscountPrice"].ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                                }
                            }

                        }
                    }


                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr != null && dr["ProductType"].ToString().ToLower() != "child")
                            {
                                if (dr["IsProductType"].ToString() == "0")
                                {
                                    Int32 Qty = 0;
                                    Decimal pp = 0;
                                    Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + " and ItemType='Swatch' "));
                                    if (SwatchQty > Decimal.Zero)
                                    {

                                        if (Isorderswatch == 1)
                                        {
                                            pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + dr["Quantity"].ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + ""));
                                            if (Convert.ToDecimal(pp) >= SwatchQty)
                                            {
                                                Qty = Convert.ToInt32(dr["Quantity"].ToString());
                                                pp = (pp - SwatchQty) / Convert.ToDecimal(dr["Quantity"].ToString());
                                                SwatchQty = Decimal.Zero;
                                            }
                                            else
                                            {
                                                Qty = Convert.ToInt32(dr["Quantity"].ToString());
                                                if (SwatchQty > Decimal.Zero)
                                                {

                                                    SwatchQty = SwatchQty - Convert.ToDecimal(pp);
                                                    pp = Decimal.Zero;
                                                }
                                                else
                                                {
                                                    pp = pp / Convert.ToDecimal(dr["Quantity"].ToString());

                                                }
                                            }
                                            subtotal = subtotal + (Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                                        }
                                        else
                                        {
                                            subtotal = subtotal + (Convert.ToDecimal(dr["SalePrice"].ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                                        }
                                    }
                                    else
                                    {
                                        if (Isorderswatch == 1)
                                        {
                                            pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + dr["Quantity"].ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + ""));
                                            pp = pp / Convert.ToDecimal(dr["Quantity"].ToString());
                                            subtotal = subtotal + (Convert.ToDecimal(pp) * Convert.ToDecimal(dr["Quantity"].ToString()));
                                        }
                                        else
                                        {
                                            subtotal = subtotal + (Convert.ToDecimal(dr["SalePrice"].ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                                        }

                                    }
                                }
                                else
                                {
                                    subtotal = subtotal + (Convert.ToDecimal(dr["SalePrice"].ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                                }
                            }

                        }
                    }
                    lblSubTotal.Text = subtotal.ToString("f2");

                    bool ChkNoDiscount = false; bool.TryParse(CommonComponent.GetScalarCommonData("SELECT NoDiscount FROM dbo.tb_Customer WHERE CustomerID=" + HdnCustID.Value.ToString()).ToString(), out ChkNoDiscount);
                    if (ChkNoDiscount == true)
                    {
                        if (Session["QtyDiscount1"] != null)
                        {
                            Session["QtyDiscount"] = Session["QtyDiscount1"].ToString();
                        }
                    }
                    else
                    {
                        if (Session["QtyDiscount1"] != null)
                        {
                            if (Session["QtyDiscount"] != null)
                            {
                                Session["QtyDiscount"] = Convert.ToDecimal(Session["QtyDiscount1"].ToString()) + Convert.ToDecimal(Session["QtyDiscount"].ToString());
                            }
                            else
                            {
                                Session["QtyDiscount"] = Session["QtyDiscount1"].ToString();
                            }
                        }
                    }

                    #region Display Discount
                    if (Session["QtyDiscount"] != null)
                    {
                        decimal Qty = 0;
                        decimal.TryParse(Session["QtyDiscount"].ToString(), out Qty);
                        TxtDiscount.Text = Math.Round(Qty, 2).ToString();
                    }
                    else
                    {
                        // TxtDiscount.Text = "0.00";

                    }

                    decimal Storecreaditamount = 0;
                    if (Session["Storecreaditamount"] != null)
                    {
                        decimal.TryParse(Session["Storecreaditamount"].ToString(), out Storecreaditamount);

                    }

                    #endregion
                    try
                    {
                        if (hfSubTotal.Value.ToString() != "")
                        {

                            if (Request.QueryString["CustID"] != null && !string.IsNullOrEmpty(Request.QueryString["CustID"].ToString()))
                            {
                                try
                                {
                                    string filedoc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT DocFile FROM dbo.tb_Customer WHERE CustomerID ='" + Request.QueryString["CustID"].ToString() + "'"));
                                    string CustDocPathTemp = AppLogic.AppConfigs("Customer.DocumentPath");

                                    if (File.Exists(Server.MapPath(CustDocPathTemp + Request.QueryString["CustID"].ToString() + "/" + filedoc)))
                                    {
                                        TxtTax.Text = "0";
                                    }
                                    else
                                    {
                                        TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                                    }
                                }
                                catch
                                {
                                    TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                                }

                            }
                            else
                            {
                                TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                            }


                        }
                        else
                        {
                            TxtTax.Text = "0";
                        }
                    }
                    catch
                    {

                    }


                    decimal tax = Convert.ToDecimal(TxtTax.Text.Trim());
                    decimal ship = Convert.ToDecimal(TxtShippingCost.Text.Trim());

                    decimal discount = Convert.ToDecimal(TxtDiscount.Text.Trim());



                    decimal FinalSubTotal = (subtotal + tax + ship) - discount;
                    if (FinalSubTotal < 0)
                    {
                        FinalSubTotal = 0;
                    }
                    lblTotal.Text = FinalSubTotal.ToString("f2");
                    hfSubTotal.Value = lblSubTotal.Text;
                    hfTotal.Value = lblTotal.Text;
                    decimal totalzero = Convert.ToDecimal(Convert.ToDecimal(hfTotal.Value) - Convert.ToDecimal(Storecreaditamount));
                    if (totalzero > Decimal.Zero)
                    {
                        txtBoxcaptureamount.Text = string.Format("{0:0.00}", totalzero);
                    }
                    else
                    {
                        txtBoxcaptureamount.Text = "0.00";
                    }


                }
                catch { }
            }
            else
            {
                trOrderDetails.Style.Add("display", "none");
                GVShoppingCartItems.DataSource = null;
                GVShoppingCartItems.DataBind();
                TxtDiscount.Text = "0.00";
                Session["QtyDiscount"] = null;
                Session["QtyDiscount1"] = null;
            }
        }
        protected void TxtCardVerificationCode_PreRender(object sender, EventArgs e)
        {
            TxtCardVerificationCode.Attributes["value"] = TxtCardVerificationCode.Text.ToString();
        }
        /// <summary>
        /// Bind Shipping Method
        /// </summary>
        private void BindShippingMethod()
        {
            bool FlagIsShipping = false;
            string strUSPSMessage = "";
            string strUPSMessage = "";
            string strFedexSMessage = "";
            lblMsg.Text = "";
            ddlShippingMethod.Items.Clear();
            ShippingComponent objShipping = new ShippingComponent();
            DataSet objShipServices = new DataSet();
            objShipServices = objShipping.GetShippingServices(Convert.ToInt32(ddlStore.SelectedValue.ToString()));
            ddlShippingMethod.Items.Clear();
            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(hdncountry.Value.ToString()));
            decimal Weight = decimal.Zero;
            StateComponent objState = new StateComponent();
            string stateName = Convert.ToString(objState.GetStateCodeByName(hdnState.Value.ToString()));
            if (ViewState["hdnWeightTotal"] != null)
            {
                Weight = Convert.ToDecimal(ViewState["hdnWeightTotal"].ToString());
            }
            if (ViewState["hdnWeightTotal1"] != null)
            {
                //Weight += Convert.ToDecimal(ViewState["hdnWeightTotal1"].ToString());
            }
            if (hfSubTotal.Value.ToString() != "")
            {
                if (Request.QueryString["CustID"] != null && !string.IsNullOrEmpty(Request.QueryString["CustID"].ToString()))
                {
                    string filedoc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT DocFile FROM dbo.tb_Customer WHERE CustomerID ='" + Request.QueryString["CustID"].ToString() + "'"));
                    string CustDocPathTemp = AppLogic.AppConfigs("Customer.DocumentPath");

                    if (File.Exists(Server.MapPath(CustDocPathTemp + Request.QueryString["CustID"].ToString() + "/" + filedoc)))
                    {
                        TxtTax.Text = "0";
                    }
                    else
                    {
                        TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                    }
                }
                else
                {
                    TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                }
            }
            else
            {
                TxtTax.Text = "0";
            }
            if (Weight == 0)
            {
                Weight = 1;
            }
            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));

            DataTable UPSTable = new DataTable();
            UPSTable.Columns.Add("ShippingMethodName", typeof(String));
            UPSTable.Columns.Add("Price", typeof(decimal));


            DataTable USPSTable = new DataTable();
            USPSTable.Columns.Add("ShippingMethodName", typeof(String));
            USPSTable.Columns.Add("Price", typeof(decimal));


            DataTable FedexTable = new DataTable();
            FedexTable.Columns.Add("ShippingMethodName", typeof(String));
            FedexTable.Columns.Add("Price", typeof(decimal));


            if (objShipServices != null && objShipServices.Tables.Count > 0 && objShipServices.Tables[0].Rows.Count > 0)
            {
                if (objShipServices.Tables[0].Select("ShippingService='UPS'").Length > 0)
                {
                    UPSTable = UPSMethodBind(CountryCode.ToString(), stateName, hdnZipCode.Value.ToString(), Weight, "UPS", ref strUPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='USPS'").Length > 0)
                {
                    EndiciaService objRate = new EndiciaService();

                    USPSTable = objRate.EndiciaGetRatesAdmin(hdnZipCode.Value.ToString(), CountryCode.ToString(), Convert.ToDouble(Weight), ref strUSPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='FEDEX'").Length > 0)
                {
                    FedexTable = FedexMethod(Convert.ToDecimal(Weight), stateName, hdnZipCode.Value.ToString(), CountryCode, ref strFedexSMessage);
                    if (FedexTable != null && FedexTable.Rows.Count == 0)
                    {
                        ViewState["IsFrieght"] = "1";
                    }
                    else
                    {
                        ViewState["IsFrieght"] = "0";
                    }
                }

                if (UPSTable != null && UPSTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(UPSTable);
                }
                if (USPSTable != null && USPSTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(USPSTable);
                }


                if (FedexTable != null && FedexTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(FedexTable);
                }

                if (Request.QueryString["CustID"] != null)
                {
                    bool IsFreeShipping = false;
                    IsFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasFreeShipping,0) FROM tb_CustomerLevel inner JOIN dbo.tb_Customer ON tb_Customer.CustomerLevelID=tb_CustomerLevel.CustomerLevelID WHERE tb_Customer.CustomerID=" + Convert.ToInt32(Request.QueryString["CustID"].ToString()) + ""));
                    if (IsFreeShipping)
                    {
                        if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
                        {
                            //String strFreeShipping = "Standard Shipping($0.00)";
                            //DataRow dataRow = ShippingTable.NewRow();
                            //dataRow["ShippingMethodName"] = strFreeShipping;
                            //dataRow["Price"] = 0;
                            // ShippingTable.Rows.Add(dataRow);
                        }
                    }
                }

                //if (!string.IsNullOrEmpty(CountryCode.ToString().Trim()) && (CountryCode.ToUpper() == "US" || CountryCode.ToUpper() == "CA" || CountryCode.ToUpper() == "AU" || CountryCode.ToUpper() == "GB"))
                //{

                decimal OrderTotal = 0;
                decimal SubTotal = 0;
                double Price = 0;
                if (!string.IsNullOrEmpty(lblTotal.Text.ToString()))
                {
                    OrderTotal = Convert.ToDecimal(lblTotal.Text.ToString());
                }
                if (!string.IsNullOrEmpty(lblSubTotal.Text.ToString()) && Convert.ToDecimal(lblSubTotal.Text.ToString()) > 0)
                {
                    SubTotal = Convert.ToDecimal(lblSubTotal.Text.ToString());
                }
                string strfreeshipping = Convert.ToString(CommonComponent.GetScalarCommonData("Select isnull(configvalue,'0') from tb_AppConfig WHERE Configname ='FreeShippingLimit' and isnull(Deleted,0)=0 and StoreId=1"));
                if (CountryCode.ToString().Trim().ToUpper() == "US" || CountryCode.ToString().Trim().ToUpper() == "UNITED STATES")
                {
                    if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                    {
                        for (int k = 0; k < ShippingTable.Rows.Count; k++)
                        {
                            if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ground") > -1)
                            {
                                string[] strMethodname = ShippingTable.Rows[k]["ShippingMethodName"].ToString().Split("($".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string Shippingname = "";
                                if (strMethodname.Length > 0)
                                {
                                    Shippingname = strMethodname[0].ToString().Replace("ups ", "").Replace("UPS ", "");
                                }
                                if (OrderTotal >= 0)
                                {
                                    if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(strfreeshipping))
                                    {
                                        Price = 0;

                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(0) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(29.99))
                                    {
                                        Price = 5.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);

                                    }

                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(30.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(48.99))
                                    {
                                        Price = 7.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);

                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(70.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(129.99))
                                    {
                                        Price = 12.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);

                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(130.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(199.99))
                                    {
                                        Price = 16.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);

                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(200.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(248.99))
                                    {
                                        Price = 21.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);

                                    }

                                    Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(Price)) + ")";
                                }
                                else
                                {
                                    Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(Price)) + ")";
                                }
                                ShippingTable.Rows[k]["ShippingMethodName"] = Shippingname.Replace("ups ", "").Replace("UPS ", "");
                                ShippingTable.Rows[k]["Price"] = Convert.ToDecimal(Price);
                                ShippingTable.AcceptChanges();
                            }
                            else if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups standard") > -1)
                            {
                                ShippingTable.Rows.RemoveAt(k);
                                ShippingTable.AcceptChanges();
                                k--;

                            }
                            else if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups ") > -1)
                            {
                                ShippingTable.Rows.RemoveAt(k);
                                ShippingTable.AcceptChanges();
                                k--;

                            }
                        }

                        if (SubTotal > 0)
                        {
                            //decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.18);
                            //DataRow dr;
                            //dr = ShippingTable.NewRow();
                            //dr["ShippingMethodName"] = "USA-3 DAY Express Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                            //dr["Price"] = Convert.ToDecimal(ShippingPrice);
                            //ShippingTable.Rows.Add(dr);

                            //ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.22);
                            //DataRow drnext;
                            //drnext = ShippingTable.NewRow();
                            //drnext["ShippingMethodName"] = "USA-Next Day Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                            //drnext["Price"] = Convert.ToDecimal(ShippingPrice);
                            //ShippingTable.Rows.Add(drnext);
                        }
                    }
                }
                else if (CountryCode.ToUpper() == "CA" || CountryCode.ToString().Trim().ToUpper() == "CANADA")
                {
                    //decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.18);
                    DataRow dr;
                    for (int k = 0; k < ShippingTable.Rows.Count; k++)
                    {
                        if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups standard") > -1)
                        {
                            ShippingTable.Rows.RemoveAt(k);
                            ShippingTable.AcceptChanges();
                            k--;
                        }
                        else if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups ") > -1)
                        {
                            ShippingTable.Rows.RemoveAt(k);
                            ShippingTable.AcceptChanges();
                            k--;
                        }
                    }

                    //dr = ShippingTable.NewRow();
                    //dr["ShippingMethodName"] = "UPS Standard to Canada" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(0)) + ")";
                    //dr["Price"] = 0;
                    //ShippingTable.Rows.Add(dr);

                    if (ViewState["AllProductsSwatch"] != null && ViewState["AllProductsSwatch"].ToString().Trim() != "" && ViewState["AllProductsSwatch"].ToString().Trim() == "1")
                    {
                        double SwatchRate = 5.99;
                        if (!string.IsNullOrEmpty(AppLogic.AppConfigs("InternationalSwatchRate").ToString()))
                        {
                            double.TryParse(AppLogic.AppConfigs("InternationalSwatchRate").ToString(), out SwatchRate);
                        }
                        dr = ShippingTable.NewRow();
                        dr["ShippingMethodName"] = "International Swatch Orders" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(SwatchRate)) + ")";
                        dr["Price"] = Convert.ToDecimal(SwatchRate);
                        ShippingTable.Rows.Add(dr);
                    }
                    if (SubTotal > Decimal.Zero)
                    {
                        decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.25);
                        dr = ShippingTable.NewRow();
                        dr["ShippingMethodName"] = "CANADA-International Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                        dr["Price"] = Convert.ToDecimal(ShippingPrice);
                        ShippingTable.Rows.Add(dr);
                    }

                    // ONE Pending for Swatch Product
                }
                else if (CountryCode.ToUpper() == "AU" || CountryCode.ToString().Trim().ToUpper() == "AUSTRALIA")
                {
                    if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                    {
                        for (int k = 0; k < ShippingTable.Rows.Count; k++)
                        {

                            ShippingTable.Rows.RemoveAt(k);
                            ShippingTable.AcceptChanges();
                            k--;


                        }
                    }
                    DataRow dr;
                    if (ViewState["AllProductsSwatch"] != null && ViewState["AllProductsSwatch"].ToString().Trim() != "" && ViewState["AllProductsSwatch"].ToString().Trim() == "1")
                    {
                        double SwatchRate = 5.99;
                        if (!string.IsNullOrEmpty(AppLogic.AppConfigs("InternationalSwatchRate").ToString()))
                        {
                            double.TryParse(AppLogic.AppConfigs("InternationalSwatchRate").ToString(), out SwatchRate);
                        }
                        dr = ShippingTable.NewRow();
                        dr["ShippingMethodName"] = "International Swatch Orders" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(SwatchRate)) + ")";
                        dr["Price"] = Convert.ToDecimal(SwatchRate);
                        ShippingTable.Rows.Add(dr);
                    }
                    if (SubTotal > Decimal.Zero)
                    {
                        decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.35);
                        dr = ShippingTable.NewRow();
                        dr["ShippingMethodName"] = "AUSTRALIA-International Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                        dr["Price"] = Convert.ToDecimal(ShippingPrice);
                        ShippingTable.Rows.Add(dr);
                    }


                }
                else if (CountryCode.ToUpper() == "GB" || CountryCode.ToString().Trim().ToUpper() == "UNITED KINGDOM")
                {
                    string StrShippingstate = "";
                    DataRow dr;

                    if (chkAddress.Checked == false && !string.IsNullOrEmpty(txtS_OtherState.Text.Trim().ToString()))
                    {
                        StrShippingstate = txtS_OtherState.Text.Trim();
                    }
                    else if (!string.IsNullOrEmpty(txtB_OtherState.Text.Trim().ToString()))
                    {
                        StrShippingstate = txtB_OtherState.Text.Trim();
                    }
                    if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                    {
                        for (int k = 0; k < ShippingTable.Rows.Count; k++)
                        {
                            if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups worldwide") > -1)
                            {
                                ShippingTable.Rows.RemoveAt(k);
                                ShippingTable.AcceptChanges();
                                k--;

                            }
                            else if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups ") > -1)
                            {
                                ShippingTable.Rows.RemoveAt(k);
                                ShippingTable.AcceptChanges();
                                k--;

                            }
                        }

                    }
                    if (ViewState["AllProductsSwatch"] != null && ViewState["AllProductsSwatch"].ToString().Trim() != "" && ViewState["AllProductsSwatch"].ToString().Trim() == "1")
                    {
                        double SwatchRate = 5.99;
                        if (!string.IsNullOrEmpty(AppLogic.AppConfigs("InternationalSwatchRate").ToString()))
                        {
                            double.TryParse(AppLogic.AppConfigs("InternationalSwatchRate").ToString(), out SwatchRate);
                        }
                        dr = ShippingTable.NewRow();
                        dr["ShippingMethodName"] = "International Swatch Orders" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(SwatchRate)) + ")";
                        dr["Price"] = Convert.ToDecimal(SwatchRate);
                        ShippingTable.Rows.Add(dr);
                    }
                    if (SubTotal > Decimal.Zero)
                    {

                        decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.35);
                        dr = ShippingTable.NewRow();
                        dr["ShippingMethodName"] = "UK-GB International Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                        dr["Price"] = Convert.ToDecimal(ShippingPrice);
                        ShippingTable.Rows.Add(dr);
                    }

                    //if (!string.IsNullOrEmpty(StrShippingstate) && StrShippingstate.ToString().ToLower().IndexOf("virgin islands") > -1)
                    //{
                    //    ShippingPrice = 0;
                    //    //decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.35);
                    //    dr = ShippingTable.NewRow();
                    //    dr["ShippingMethodName"] = "UPS Worldwide Express" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                    //    dr["Price"] = Convert.ToDecimal(ShippingPrice);
                    //    ShippingTable.Rows.Add(dr);
                    //}

                }
                try
                {
                    if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                    {
                        DataView dvShipping = ShippingTable.DefaultView;
                        dvShipping.Sort = "Price asc";

                        ddlShippingMethod.DataSource = dvShipping.ToTable();
                        ddlShippingMethod.DataTextField = "ShippingMethodName";
                        ddlShippingMethod.DataValueField = "ShippingMethodName";
                        ddlShippingMethod.DataBind();
                        FlagIsShipping = true;
                    }
                }
                catch { }
                ddlShippingMethod.Items.Insert(ddlShippingMethod.Items.Count, new ListItem("Free Shipping($0.00)", "Free Shipping($0.00)"));

                if (ViewState["selectedShippingmethod"] != null)
                {
                    try
                    {
                        int Index = ViewState["selectedShippingmethod"].ToString().IndexOf("($");
                        int Length = ViewState["selectedShippingmethod"].ToString().LastIndexOf(")") - Index;
                        string strShippingCost = "";
                        if (Index != -1 && Length != 0)
                        {
                            strShippingCost = ViewState["selectedShippingmethod"].ToString().Substring(Index + 2, Length - 2).Trim();

                        }
                        string strShippingMethod = ViewState["selectedShippingmethod"].ToString().Replace("($" + strShippingCost + ")", "");

                        for (int i = 0; i < ddlShippingMethod.Items.Count; i++)
                        {
                            if (ddlShippingMethod.Items[i].Text.ToString().ToLower().IndexOf(strShippingMethod.ToLower()) > -1)
                            {
                                ddlShippingMethod.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    try
                    {
                        ViewState["selectedShippingmethod"] = ddlShippingMethod.SelectedItem.Text.ToString();
                    }
                    catch
                    {

                    }
                }

                //}
                //else
                //{
                //    ddlShippingMethod.Items.Clear();
                //    ListItem li = new ListItem("Bongo International", "Bongo International");
                //    ddlShippingMethod.Items.Insert(ddlShippingMethod.Items.Count, li);
                //}

                if (strUSPSMessage != "" && strUPSMessage != "")
                {
                    lblMsg.Text = strUPSMessage + strUSPSMessage;
                    lblMsg.Visible = true;
                }
                else if (strUSPSMessage != "")
                {
                    lblMsg.Text = strUSPSMessage;
                    lblMsg.Visible = true;
                }
                else if (strUPSMessage != "")
                {
                    lblMsg.Text = strUPSMessage;
                    lblMsg.Visible = true;
                }

                if (FlagIsShipping == true)
                    ddlShippingMethod_SelectedIndexChanged(null, null);

            }
        }

        /// <summary>
        /// Fedex Method Bind
        /// </summary>
        /// <param name="Weight">Decimal Weight</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Country">String Country</param>
        /// <param name="StrMessage">Return Error Message </param>
        /// <returns>Returns th Fedex Methods as a DataTable</returns>
        private DataTable FedexMethod(decimal Weight, string State, string ZipCode, string Country, ref string StrMessage)
        {

            //   string GetFedexrate = Convert.ToString(obj.FedexGetRates(Convert.ToDecimal(Weight), "", "", "", State, ZipCode, CountryCode.ToString(), Convert.ToInt32(Session["CustID"]), true));

            if (ZipCode == "" || Country == "")
            {
                return null;
            }
            StringBuilder tmpFixedShipping = new StringBuilder(4096);
            StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            string GetFedexrate = "";
            Fedex obj = new Fedex();
            //      StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            if (Weight > decimal.Zero)
            {
                GetFedexrate = Convert.ToString(obj.FedexGetRates(Convert.ToDecimal(Weight), "", "", "", State, ZipCode, Country.ToString(), Convert.ToInt32(Session["CustID"]), true));
            }
            else
                GetFedexrate = Convert.ToString(obj.FedexGetRates(Convert.ToDecimal(1), "", "", "", State, ZipCode, Country.ToString(), Convert.ToInt32(Session["CustID"]), true));


            //FedEx Methods

            tmpRealTimeShipping.Append((string)GetFedexrate);
            string strResult = GetFedexrate;

            #region Get Fixed Shipping Methods

            try
            {
                ShippingComponent objShipping = new ShippingComponent();
                DataSet dsFixedShippingMethods = new DataSet();
                dsFixedShippingMethods = objShipping.GetFixedShippingMethods(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "FEDEX");
                if (dsFixedShippingMethods != null && dsFixedShippingMethods.Tables.Count > 0 && dsFixedShippingMethods.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsFixedShippingMethods.Tables[0].Rows.Count; i++)
                    {

                        tmpFixedShipping.Append((string)dsFixedShippingMethods.Tables[0].Rows[i]["ShippingMethod"] + ",");
                    }
                }
            }
            catch { }

            #endregion
            string[] strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strResult.ToString() == "")
            {
                //lblMsg.Visible = true;
                StrMessage = strResult + "<br />";
                // lblMsg.Text += strResult + "<br />";
                strResult = tmpFixedShipping.ToString();

                strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strMethod.Length; i++)
                {
                    string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string Shippingname = "";
                    if (strMethodname.Length > 0)
                    {
                        Shippingname = strMethodname[0].ToString();
                    }
                    if (strMethodname.Length > 1)
                    {
                        Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                    }
                    //  rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));
                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                    ShippingTable.Rows.Add(dataRow);
                }
            }
            else
            {
                strResult = tmpFixedShipping.ToString() + strResult;
                strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strMethod.Length; i++)
                {
                    string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string Shippingname = "";
                    if (strMethodname.Length > 0)
                    {
                        Shippingname = strMethodname[0].ToString();
                    }
                    if (strMethodname.Length > 1)
                    {
                        Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                    }
                    // rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));

                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                    ShippingTable.Rows.Add(dataRow);
                }
            }

            return ShippingTable;

        }

        /// <summary>
        /// UPS Method Bind
        /// </summary>
        /// <param name="Country">String CountryCode</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Weight">Decimal Weight</param>
        /// <returns>Returns the UPS  Method List as a Dataset</returns>
        private DataTable UPSMethodBind(string Country, string State, string ZipCode, decimal Weight, string ServiceName, ref string StrMessage)
        {

            if (ZipCode == "" || Country == "")
            {
                return null;
            }

            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            try
            {



                UPS obj = new UPS(AppLogic.AppConfigs("Shipping.OriginAddress"), AppLogic.AppConfigs("Shipping.OriginAddress2"), AppLogic.AppConfigs("Shipping.OriginCity"), AppLogic.AppConfigs("Shipping.OriginState"), AppLogic.AppConfigs("Shipping.OriginZip"), AppLogic.AppConfigs("Shipping.OriginCountry"));
                obj.DestinationCountryCode = Country;
                obj.DestinationStateProvince = State;
                obj.DestinationZipPostalCode = ZipCode;
                StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
                StringBuilder tmpFixedShipping = new StringBuilder(4096);
                string strResult = "";
                try
                {



                    if (Weight > decimal.Zero)
                    {
                        tmpRealTimeShipping.Append((string)obj.UPSGetRates(Convert.ToDecimal(Weight), Convert.ToDecimal(0), true));
                    }
                    else
                    {
                        tmpRealTimeShipping.Append((string)obj.UPSGetRates(Convert.ToDecimal(1), Convert.ToDecimal(0), true));
                    }
                    strResult = tmpRealTimeShipping.ToString();
                }
                catch
                {

                }
                #region Get Fixed Shipping Methods

                try
                {
                    ShippingComponent objShipping = new ShippingComponent();
                    DataSet dsFixedShippingMethods = new DataSet();
                    dsFixedShippingMethods = objShipping.GetFixedShippingMethods(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), ServiceName);
                    if (dsFixedShippingMethods != null && dsFixedShippingMethods.Tables.Count > 0 && dsFixedShippingMethods.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsFixedShippingMethods.Tables[0].Rows.Count; i++)
                        {

                            tmpFixedShipping.Append((string)dsFixedShippingMethods.Tables[0].Rows[i]["ShippingMethod"] + ",");
                        }
                    }
                }
                catch { }

                #endregion

                string[] strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (strResult.ToString().ToLower().IndexOf("error") > -1)
                {
                    //lblMsg.Visible = true;
                    StrMessage = strResult + "<br />";
                    // lblMsg.Text += strResult + "<br />";
                    strResult = tmpFixedShipping.ToString();

                    strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < strMethod.Length; i++)
                    {
                        string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string Shippingname = "";
                        if (strMethodname.Length > 0)
                        {
                            Shippingname = strMethodname[0].ToString();
                        }
                        if (strMethodname.Length > 1)
                        {
                            Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                        }
                        //  rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));
                        DataRow dataRow = ShippingTable.NewRow();
                        dataRow["ShippingMethodName"] = Shippingname;
                        dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                        ShippingTable.Rows.Add(dataRow);



                    }
                }
                else
                {
                    strResult = tmpFixedShipping.ToString() + strResult;
                    strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < strMethod.Length; i++)
                    {
                        string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string Shippingname = "";
                        if (strMethodname.Length > 0)
                        {
                            Shippingname = strMethodname[0].ToString();
                        }
                        if (strMethodname.Length > 1)
                        {
                            Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                        }
                        // rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));

                        DataRow dataRow = ShippingTable.NewRow();
                        dataRow["ShippingMethodName"] = Shippingname;
                        dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                        ShippingTable.Rows.Add(dataRow);


                    }

                }
            }
            catch
            {

            }
            return ShippingTable;
        }

        /// <summary>
        /// Gets the Shipping Method by Bill Address
        /// </summary>
        private void GetShippIngMethodByBillAddress()
        {
            if (chkAddress.Checked == true)
            {
                hdnZipCode.Value = txtB_Zip.Text.ToString();
                if (ddlB_State.SelectedValue == "-11")
                {
                    hdnState.Value = txtB_OtherState.Text.ToString();
                }
                else
                {
                    hdnState.Value = ddlB_State.SelectedItem.Text.ToString();
                }
                hdncountry.Value = ddlB_Country.SelectedValue.ToString();

            }
            else
            {
                hdnZipCode.Value = txtS_Zip.Text.ToString();
                if (ddlS_State.SelectedValue == "-11")
                {
                    hdnState.Value = txtS_OtherState.Text.ToString();
                }
                else
                {
                    hdnState.Value = ddlS_State.SelectedItem.Text.ToString();
                }
                hdncountry.Value = ddlS_Country.SelectedValue.ToString();
            }


        }

        /// <summary>
        /// Get Sales Tax By ZipCode or StateName
        /// </summary>
        /// <param name="stateName">string StateName</param>
        /// <param name="zipCode">String ZipCode</param>
        /// <param name="orderAmount">string OrderAmount</param>
        /// <returns>Returns the Sale Tax Value</returns>
        private decimal SaleTax(string stateName, string zipCode, decimal orderAmount)
        {
            ShoppingCartComponent objTax = new ShoppingCartComponent();
            decimal salesTax = decimal.Zero;
            if (TxtDiscount.Text.Trim() == "")
            {
                orderAmount = orderAmount - Convert.ToDecimal(0);
            }
            else
            {
                //if (!string.IsNullOrEmpty(TxtShippingCost.Text.ToString()) && TxtShippingCost.Text.ToString() != "" && Convert.ToDecimal(TxtShippingCost.Text) > 0)
                //{
                //    orderAmount = orderAmount + Convert.ToDecimal(TxtShippingCost.Text) - Convert.ToDecimal(TxtDiscount.Text.Trim());
                //}
                //else
                //{
                orderAmount = orderAmount - Convert.ToDecimal(TxtDiscount.Text.Trim());
                //}
            }


            if (stateName.ToString().ToLower().Trim() == "georgia" || stateName.ToString().ToLower().Trim() == "ga")
            {
                decimal shppservicechrge = decimal.Zero;

                if (!string.IsNullOrEmpty(TxtShippingCost.Text))
                {
                    Decimal.TryParse(TxtShippingCost.Text, out shppservicechrge);
                    orderAmount = orderAmount + shppservicechrge;
                }
            }

            Boolean ishastax = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT isnull(LevelHasnoTax,0) FROM tb_CustomerLevel WHERE CustomerLevelID in (SELECT isnull(CustomerLevelID,0) FROM tb_customer WHERE CustomerId=" + HdnCustID.Value.ToString() + ")"));
            if (ishastax == false)
            {
                salesTax = Convert.ToDecimal(objTax.GetSalesTax(stateName, zipCode, orderAmount, Convert.ToInt32(ddlStore.SelectedValue.ToString())));
            }
            //  hdnTaxRate.Value = Convert.ToString(objTax.GetSalesTaxDetails(stateName, zipCode, orderAmount, Convert.ToInt32(ddlStore.SelectedValue.ToString())));
            return salesTax;
        }

        /// <summary>
        /// Shopping Cart Items Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void GVShoppingCartItems_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ViewState["hdnWeightTotal"] = null;
                ViewState["hdnWeightTotal1"] = null;
                Label lblHeaderDiscount = e.Row.FindControl("lblHeaderDiscount") as Label;
                if (Session["CustCouponCodeDiscount"] != null && Session["CustCouponCode"] != null)
                {
                    decimal Discount = 0;
                    decimal.TryParse(Session["CustCouponCodeDiscount"].ToString(), out Discount);
                    // lblHeaderDiscount.Text = "(" + Discount.ToString("f2") + "%)";
                }
                Session["QtyDiscount1"] = null;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblName = e.Row.FindControl("lblName") as Label;
                Label lblItem = e.Row.FindControl("lblItem") as Label;
                Label lblProductType = e.Row.FindControl("lblProductType") as Label;
                Label lblISProductType = e.Row.FindControl("lblisProductType") as Label;

                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                Label lblVariantNames = (Label)e.Row.FindControl("lblVariantNames");
                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                Label lblSKU = (Label)e.Row.FindControl("lblSKU");

                Label lblCustomerCartId = (Label)e.Row.FindControl("lblCustomerCartId");
                Label lblParentProductID = (Label)e.Row.FindControl("lblParentProductID");
                Label lblDiscountPrice = (Label)e.Row.FindControl("lblDiscountPrice");
                Label lblOrginalDiscountPrice = (Label)e.Row.FindControl("lblOrginalDiscountPrice");
                Label lblIndiSubTotal = (Label)e.Row.FindControl("lblIndiSubTotal");
                Label lblSubTot = (Label)e.Row.FindControl("lblSubTot");

                System.Web.UI.HtmlControls.HtmlInputHidden hdnSubTotalGrid = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnSubTotalGrid");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnWeightTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnWeightTotal");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnWeightTotal1 = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnWeightTotal1");
                Label lblQty = (Label)e.Row.FindControl("lblQty");
                Label lblSalePrice = (Label)e.Row.FindControl("lblSalePrice");
                string[] variantName = lblVariantNames.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantValue = lblVariantValues.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                HiddenField hdnswatchqty = (HiddenField)e.Row.FindControl("hdnswatchqty");
                HiddenField hdnswatchtype = (HiddenField)e.Row.FindControl("hdnswatchtype");
                Label lblCustomerCartChangePriceId = (Label)e.Row.FindControl("lblCustomerCartChangePriceId");
                TextBox txtChangePrice = e.Row.FindControl("txtChangePrice") as TextBox;
                TextBox txtcouponprice = e.Row.FindControl("txtcouponprice") as TextBox;
                System.Web.UI.HtmlControls.HtmlAnchor btnSavePrice = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("btnSavePrice");
                if (lblSKU.Text.ToString().ToLower() == "hemming" || lblSKU.Text.ToString().ToLower() == "fabrication" || lblSKU.Text.ToString().ToLower() == "special-order-custom")
                {
                    txtChangePrice.Visible = true;
                    lblPrice.Visible = false;

                    btnSavePrice.Attributes.Add("onclick", "ChangeItemPrice('" + lblCustomerCartChangePriceId.ClientID.ToString() + "','" + txtChangePrice.ClientID.ToString() + "');");
                    btnSavePrice.Visible = true;
                }
                else
                {
                    txtChangePrice.Visible = false;
                    lblPrice.Visible = true;
                    btnSavePrice.Visible = false;
                }


                string strMount = "";
                string strColor = "";
                for (int j = 0; j < variantValue.Length; j++)
                {
                    if (variantName.Length > j)
                    {
                        lblName.Text += "<br />" + variantName[j].ToString() + " : " + variantValue[j].ToString();
                    }
                    if (variantName[j].ToString().ToLower().IndexOf("mount") > -1)
                    {
                        strMount = "mount";
                    }
                    else if (variantName[j].ToString().ToLower().IndexOf("color") > -1)
                    {
                        strColor = variantValue[j].ToString();
                    }
                }
                if (Session["CustCouponCode"] != null && Session["CustCouponCode"].ToString() != "" && Session["CustCouponCode"].ToString().ToLower().Trim() == "pricematch")
                {
                    if (lblName.Text.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1 && Convert.ToDecimal(lblPrice.Text) == Decimal.Zero)
                    {
                        txtcouponprice.Visible = false;
                        lblOrginalDiscountPrice.Visible = true;
                    }
                    else
                    {
                        txtcouponprice.Visible = true;
                        lblOrginalDiscountPrice.Visible = false;
                        txtcouponprice.Text = lblOrginalDiscountPrice.Text;

                        txtcouponprice.Attributes.Add("onchange", "ChangeDiscountPrice('" + lblCustomerCartChangePriceId.ClientID.ToString() + "','" + txtcouponprice.ClientID.ToString() + "');");
                    }

                }
                else
                {
                    txtcouponprice.Visible = false;
                    lblOrginalDiscountPrice.Visible = true;
                }
                try
                {
                    if (lblISProductType.Text.ToString() == "2")
                    {
                        if (lblVariantNames.Text.ToString().Trim().ToLower().IndexOf("width,length,") > -1)
                        {
                            lblSKU.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE VariantValue like '%custom%' and ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + ""));

                        }
                        else
                        {
                            DataSet dsp = new DataSet();
                            dsp = CommonComponent.GetCommonDataSet("Exec GuiGetProductFabricDetailsForPID " + Convert.ToInt32(lblProductID.Text.ToString()) + "");
                            if (dsp != null && dsp.Tables.Count > 0 && dsp.Tables[0].Rows.Count > 0)
                            {
                                lblSKU.Text = dsp.Tables[0].Rows[0]["code"].ToString();
                                lblName.Text = dsp.Tables[0].Rows[0]["name"].ToString();
                                for (int j = 0; j < variantValue.Length; j++)
                                {
                                    if (variantName.Length > j)
                                    {
                                        lblName.Text += "<br />" + variantName[j].ToString() + " : " + variantValue[j].ToString();
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        //if (!string.IsNullOrEmpty(strMount) && !string.IsNullOrEmpty(strColor))
                        //{
                        //    lblSKU.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE VariantValue='" + strColor.ToString().Trim().Replace("'", "''") + "' and ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + ""));
                        //}
                    }
                }
                catch { }
                //try
                //{
                //    if (lblProductType != null && lblProductType.Text.ToString() == "3")
                //    {
                //        string strsku = "";
                //        for (int pp = 0; pp < variantName.Length; pp++)
                //        {

                //            if (variantName[pp].ToString().ToLower().IndexOf("color") > -1)
                //            {
                //                lblSKU.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE VariantValue='" + variantValue[pp].ToString().Trim().Replace("'", "''") + "' and ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + ""));
                //                break;
                //            }

                //        }
                //    }
                //}
                //catch { }
                decimal optionweight = 0;
                if (lblISProductType.Text.ToString() == "2")
                {
                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and SKU='" + lblSKU.Text.ToString().Trim() + "'"));
                    if (lblVariantNames.Text.ToString().Trim().ToLower().IndexOf("width,length,") > -1)
                    {
                        string strskufind = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(SKU,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and VariantValue like '%custom%'"));

                        if (!string.IsNullOrEmpty(strskufind))
                        {
                            lblSKU.Text = strskufind;
                        }
                    }

                }
                else if (lblISProductType.Text.ToString() == "3")
                {
                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and SKU='" + lblSKU.Text.ToString().Trim() + "'"));


                }
                else if (lblISProductType.Text.ToString() == "1")
                {
                    for (int i = 0; i < variantValue.Length; i++)
                    {
                        if (variantValue.Length > i)
                        {
                            if (variantName[i].ToString().ToLower().IndexOf("select size") > -1)
                            {
                                if (variantValue[i].ToString().IndexOf("($") > -1)
                                {
                                    string sttrval = variantValue[i].ToString().Substring(0, variantValue[i].ToString().IndexOf("($"));
                                    sttrval = sttrval.Replace("(Buy 1 Get 1 Free)", "").Replace("(Final Sales)", "");
                                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));

                                    string strskufind = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(SKU,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));

                                    if (!string.IsNullOrEmpty(strskufind))
                                    {
                                        lblSKU.Text = strskufind;
                                    }
                                    break;
                                }
                                else
                                {
                                    string sttrval = variantValue[i].ToString();
                                    sttrval = sttrval.Replace("(Buy 1 Get 1 Free)", "").Replace("(Final Sales)", "");
                                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));

                                    string strskufind = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(SKU,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));
                                    if (!string.IsNullOrEmpty(strskufind))
                                    {
                                        lblSKU.Text = strskufind;
                                    }

                                    break;
                                }
                            }

                        }
                    }

                }
                if (hdnSubTotalGrid != null)
                {
                    ViewState["hdnSubTotal"] = hdnSubTotalGrid.Value.ToString();
                }



                if (hdnWeightTotal != null)
                {
                    if (optionweight > Decimal.Zero)
                    {
                        optionweight = optionweight * Convert.ToInt32(lblQty.Text);
                        decimal dd = decimal.Zero;
                        if (ViewState["hdnWeightTotal"] != null)
                        {
                            dd = Convert.ToDecimal(ViewState["hdnWeightTotal"].ToString());
                        }
                        dd = dd + Convert.ToDecimal(optionweight);
                        ViewState["hdnWeightTotal"] = dd.ToString();
                    }
                    else
                    {
                        decimal dd = decimal.Zero;
                        if (ViewState["hdnWeightTotal"] != null)
                        {
                            dd = Convert.ToDecimal(ViewState["hdnWeightTotal"].ToString());
                        }
                        dd = dd + Convert.ToDecimal(hdnWeightTotal.Value.ToString());
                        ViewState["hdnWeightTotal"] = dd.ToString();
                        // ViewState["hdnWeightTotal"] = hdnWeightTotal.Value.ToString();
                    }
                }
                if (hdnWeightTotal1 != null)
                {
                    decimal dd = decimal.Zero;
                    if (ViewState["hdnWeightTotal1"] != null)
                    {
                        dd = Convert.ToDecimal(ViewState["hdnWeightTotal1"].ToString());
                    }
                    dd = dd + Convert.ToDecimal(hdnWeightTotal1.Value.ToString());
                    ViewState["hdnWeightTotal1"] = dd.ToString();
                }

                if (lblProductType != null)
                {
                    if (lblProductType.Text.Trim().ToLower() == "parent")
                    {
                        lblItem.Text = "-";
                    }
                    else
                    {
                        lblName.Text = "-";
                    }
                }
                Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + lblProductID.Text.ToString() + " and ItemType='Swatch' "));
                if (SwatchQty > Decimal.Zero)
                {

                    if (Isorderswatch == 1)
                    {
                        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT  case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + lblProductID.Text.ToString() + ""));

                        lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                        lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                        pp = pp * Convert.ToDecimal(lblQty.Text.ToString());
                        if (Convert.ToDecimal(pp) >= SwatchQty)
                        {
                            hdnswatchqty.Value = lblQty.Text.ToString();
                            hdnswatchtype.Value = "0";
                            lblIndiSubTotal.Text = "$" + String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty)));
                            lblSubTot.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty)));
                            lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty)) / Convert.ToDecimal(lblQty.Text.ToString()));
                            lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty)) / Convert.ToDecimal(lblQty.Text.ToString()));
                            SwatchQty = Decimal.Zero;

                        }
                        else
                        {
                            if (SwatchQty > Decimal.Zero)
                            {
                                lblIndiSubTotal.Text = "$" + String.Format("{0:0.00}", Convert.ToDecimal(0));
                                lblSubTot.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                                hdnswatchqty.Value = lblQty.Text.ToString();
                                hdnswatchtype.Value = "0";
                                SwatchQty = SwatchQty - Convert.ToDecimal(pp);
                                lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                                lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                            }
                            else
                            {
                                lblIndiSubTotal.Text = "$" + String.Format("{0:0.00}", Convert.ToDecimal(pp.ToString()));
                                lblSubTot.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp.ToString()));
                                hdnswatchqty.Value = lblQty.Text.ToString();
                                hdnswatchtype.Value = "0";
                                lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) / Convert.ToDecimal(lblQty.Text.ToString())));
                                lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) / Convert.ToDecimal(lblQty.Text.ToString())));

                            }

                        }
                    }
                }
                else
                {
                    if (Isorderswatch == 1)
                    {
                        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT  case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + lblProductID.Text.ToString() + ""));
                        lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                        lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                        pp = pp * Convert.ToDecimal(lblQty.Text.ToString());
                        lblIndiSubTotal.Text = "$" + String.Format("{0:0.00}", Convert.ToDecimal(pp.ToString()));
                        lblSubTot.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp.ToString()));
                        hdnswatchqty.Value = lblQty.Text.ToString();
                        hdnswatchtype.Value = "0";
                    }

                }
                string Quantity = "0";
                if (hdnswatchtype.Value.ToString() != "")
                {
                    Quantity = hdnswatchqty.Value.ToString();
                }
                else
                {
                    Quantity = lblQty.Text.ToString();
                }
                decimal DiscountPrice = 0, OrgiPrice = 0;
                if (HdnCustID.Value != null && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null) && !string.IsNullOrEmpty(lblDiscountPrice.Text.ToString()) && Session["CustCouponCode"].ToString().ToLower().Trim() != "pricematch")
                {
                    decimal.TryParse(Session["CustCouponCodeDiscount"].ToString(), out DiscountPrice);
                    decimal.TryParse(lblSalePrice.Text.ToString().Trim().ToString(), out OrgiPrice);
                    if (Session["CustCouponvalid"] != null && Session["CustCouponvalid"].ToString() == "1")
                    {
                        #region CheckMembership Discount
                        if (HdnCustID.Value != null)
                        {
                            String ProductId = lblProductID.Text.ToString();
                            decimal ProductDiscount = 0;
                            decimal CategoryDiscount = 0;
                            decimal NewDiscount = 0;
                            decimal DesPrice = 0;

                            //String CategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ""));
                            // String ParentCategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select cast(ParentCategoryID as nvarchar(500))+',' from tb_CategoryMapping WHERE CategoryID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ") FOR XML PATH('')"));
                            String ParentCategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select cast(ParentCategoryID as nvarchar(500))+',' from tb_CategoryMapping WHERE CategoryID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where isnull(p.IsSaleclearance,0)=0  and p.ProductID not in (select top 1 ProductID  from tb_ProductVariantValue where ProductID=" + ProductId + " and isnull(OnSale,0)=1 and cast(OnSaleFromdate as date)<=cast(GETDATE() as date) and cast(OnSaleTodate as date)>=cast(GETDATE() as date)) and pc.ProductID=" + ProductId + ") FOR XML PATH('')"));
                            if (ParentCategoryId.Length > 0)
                            {
                                ParentCategoryId = ParentCategoryId.Substring(0, ParentCategoryId.Length - 1);
                            }
                            else
                            {
                                ParentCategoryId = "0";
                            }

                            //ProductDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT ISNULL(md.Discount,0) AS ProductDiscount "
                            //+ " FROM dbo.tb_MembershipDiscount md Left OUTER JOIN dbo.tb_Product Prod ON Prod.ProductID =md.DiscountObjectID " +
                            //" WHERE md.CustID='" + HdnCustID.Value.ToString() + "' AND md.DiscountType='product' AND md.StoreID= " + ddlStore.SelectedValue.ToString() + " AND md.DiscountObjectID=" + ProductId + ""));
                            //if (ProductDiscount <= 0)
                            //{
                            //    CategoryDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT top 1 md.Discount AS CategoryDiscount "
                            //        + " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                            //        + "WHERE md.DiscountType='category' AND  md.CustID= " + HdnCustID.Value.ToString() + "  AND md.storeid=" + ddlStore.SelectedValue.ToString() + " AND (md.DiscountObjectID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ") or md.DiscountObjectID in (" + ParentCategoryId + "))"));
                            //    //    Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT md.Discount AS CategoryDiscount "
                            //    //+ " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                            //    //+ "WHERE md.DiscountType='category' AND md.CustID= " + HdnCustID.Value.ToString() + "  AND md.storeid=" + ddlStore.SelectedValue.ToString() + " AND md.DiscountObjectID=" + CategoryId + " "));
                            //    DiscountPrice = CategoryDiscount;
                            //}
                            ProductDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT ISNULL(md.Discount,0) AS ProductDiscount "
                                    + " FROM dbo.tb_MembershipDiscount md Left OUTER JOIN dbo.tb_Product Prod ON Prod.ProductID =md.DiscountObjectID " +
                                    " WHERE md.CustID='" + HdnCustID.Value.ToString() + "' AND md.DiscountType='product' AND md.StoreID= " + ddlStore.SelectedValue.ToString() + " AND md.DiscountObjectID=" + ProductId + " and isnull(Prod.IsSaleclearance,0)=0 and Prod.ProductID not in (select top 1 ProductID  from tb_ProductVariantValue where isnull(OnSale,0)=1 and cast(OnSaleFromdate as date)<=cast(GETDATE() as date) and cast(OnSaleTodate as date)>=cast(GETDATE() as date) and ProductID=" + ProductId + " ) "));
                            if (ProductDiscount <= 0)
                            {
                                CategoryDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT top 1 md.Discount AS CategoryDiscount "
                                    + " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                                    + "WHERE md.DiscountType='category' AND  md.CustID= " + HdnCustID.Value.ToString() + "  AND md.storeid=" + ddlStore.SelectedValue.ToString() + " AND (md.DiscountObjectID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where isnull(p.IsSaleclearance,0)=0  and p.ProductID not in (select top 1 ProductID  from tb_ProductVariantValue where ProductID=" + ProductId + " and isnull(OnSale,0)=1 and cast(OnSaleFromdate as date)<=cast(GETDATE() as date) and cast(OnSaleTodate as date)>=cast(GETDATE() as date)) and pc.ProductID=" + ProductId + ") or md.DiscountObjectID in (" + ParentCategoryId + "))"));
                                //    Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT md.Discount AS CategoryDiscount "
                                //+ " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                                //+ "WHERE md.DiscountType='category' AND md.CustID= " + HdnCustID.Value.ToString() + "  AND md.storeid=" + ddlStore.SelectedValue.ToString() + " AND md.DiscountObjectID=" + CategoryId + " "));
                                DiscountPrice = CategoryDiscount;
                            }
                            else
                            {
                                DiscountPrice = ProductDiscount;
                            }
                            if (DiscountPrice > 0)
                            {
                                decimal DicPrice = 0, TempDis = 0;
                                if (DiscountPrice > 0 && DiscountPrice <= 99)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                    DicPrice = OrgiPrice - TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else if (DiscountPrice >= 100)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                    DicPrice = TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else
                                {
                                    lblOrginalDiscountPrice.Text = string.Format("{0:0.00}", Convert.ToDecimal(OrgiPrice));
                                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }

                                GVShoppingCartItems.Columns[5].Visible = true;


                            }
                            else
                            {
                                lblOrginalDiscountPrice.Text = string.Format("{0:0.00}", Convert.ToDecimal(OrgiPrice));
                                lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                            }
                        }
                        #endregion
                    }
                    else
                    {


                        String strCategory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidForCategory FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CustCouponCode"].ToString() + "'"));
                        string srchildcategory = "";
                        try
                        {
                            //if (!string.IsNullOrEmpty(strCategory))
                            //{
                            //    srchildcategory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT cast(categoryId as nvarchar(50))+',' FROM tb_Category WHERE isnull(active,0)=1 and isnull(storeid,0)=1 and isnull(deleted,0)=0 and Categoryid in (SELECT Categoryid FROM tb_CategoryMapping WHERE ParentCategoryID in (" + strCategory + ")) FOR XML PATH('')"));
                            //}
                            //if (srchildcategory.Length > 1)
                            //{
                            //    strCategory = srchildcategory + strCategory;
                            //}
                        }
                        catch
                        {

                        }
                        String strCategoryPercantage = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT CategoryPercentage FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CustCouponCode"].ToString() + "'"));
                        String strProduct = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidforProduct FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CustCouponCode"].ToString() + "'"));
                        if (!string.IsNullOrEmpty(strCategory))
                        {
                            DataSet dspp = new DataSet();
                            string strquery = "SELECT ProductId,1749 as CategoryId FROM tb_ProductVariantValue WHERE ISNULL(tb_ProductVariantValue.VarActive,0)=1 AND (CAST(OnSaleFromdate AS DATE) <=  CAST(GETDATE() AS DATE) AND	CAST(OnSaleTodate AS DATE)	>=	CAST(GETDATE() AS DATE)) AND ISNULL(OnSale,0)=1 and ProductID=" + lblProductID.Text.ToString() + " UNION SELECT  ProductId,1749 as CategoryId  FROM tb_product where  isnull(IsSaleclearance,0)=1 and ProductID=" + lblProductID.Text.ToString() + " UNION ";
                            dspp = CommonComponent.GetCommonDataSet(strquery + " SELECT ProductId,CategoryId  FROM tb_ProductCategory WHERE ProductId=" + lblProductID.Text.ToString() + "");
                            Decimal disccat = decimal.Zero;
                            if (dspp != null && dspp.Tables.Count > 0 && dspp.Tables[0].Rows.Count > 0)
                            {
                                decimal DicPrice = 0, TempDis = 0;
                                string[] pers = { "" };
                                string[] cats = { "" };
                                try
                                {
                                    bool iadded = false;
                                    for (int icc = 0; icc < dspp.Tables[0].Rows.Count; icc++)
                                    {
                                        if (!string.IsNullOrEmpty(strCategoryPercantage))
                                        {
                                            cats = strCategory.Split(',');
                                            pers = strCategoryPercantage.Split(',');
                                            for (int co = 0; co < cats.Length; co++)
                                            {
                                                if (dspp.Tables[0].Rows[icc]["categoryId"].ToString() == cats[co].ToString().Trim())
                                                {
                                                    if (Convert.ToDecimal(pers[co].ToString()) > Convert.ToDecimal(disccat))
                                                    {
                                                        Decimal.TryParse(pers[co].ToString(), out disccat);
                                                        iadded = true;
                                                    }
                                                    //break;
                                                }
                                            }
                                        }

                                        if (iadded == false)
                                        {
                                            DataSet dsold = new DataSet();
                                            dsold = CommonComponent.GetCommonDataSet("WITH Emp_CTE AS (SELECT tb_CategoryMapping.CategoryID,tb_CategoryMapping.ParentCategoryID FROM tb_Category INNER JOIN tb_CategoryMapping on tb_Category.CategoryID=tb_CategoryMapping.CategoryID WHERE tb_CategoryMapping.CategoryID=" + dspp.Tables[0].Rows[icc]["categoryId"].ToString() + " and StoreID=1 and isnull(active,0)=1 and isnull(deleted,0)=0  and  Name not like '%swatch%' UNION ALL SELECT tb_CategoryMapping.CategoryID,tb_CategoryMapping.ParentCategoryID FROM tb_Category INNER JOIN tb_CategoryMapping on tb_Category.CategoryID=tb_CategoryMapping.CategoryID INNER JOIN Emp_CTE on Emp_CTE.ParentCategoryID=tb_CategoryMapping.CategoryID WHERE   StoreID=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and  Name not like '%swatch%') SELECT CategoryID,ParentCategoryID FROM Emp_CTE WHERE isnull(ParentCategoryID,0)<>0 ");

                                            if (dsold != null && dsold.Tables.Count > 0 && dsold.Tables[0].Rows.Count > 0)
                                            {
                                                for (int idd = 0; idd < dsold.Tables[0].Rows.Count; idd++)
                                                {
                                                    if (!string.IsNullOrEmpty(strCategoryPercantage))
                                                    {
                                                        cats = strCategory.Split(',');
                                                        pers = strCategoryPercantage.Split(',');
                                                        for (int co = 0; co < cats.Length; co++)
                                                        {
                                                            if (dsold.Tables[0].Rows[idd]["ParentCategoryID"].ToString() == cats[co].ToString().Trim())
                                                            {
                                                                if (Convert.ToDecimal(pers[co].ToString()) > Convert.ToDecimal(disccat))
                                                                {
                                                                    Decimal.TryParse(pers[co].ToString(), out disccat);

                                                                }
                                                                //break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }


                                    }



                                }
                                catch
                                {

                                }
                                if (disccat > Decimal.Zero)
                                {

                                }
                                else
                                {
                                    disccat = DiscountPrice;
                                }
                                if (disccat > 1 && disccat <= 99)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * disccat) / 100);
                                    DicPrice = OrgiPrice - TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else if (disccat >= 100)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * disccat) / 100);
                                    DicPrice = TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else
                                {
                                    lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(strProduct))
                                {
                                    strProduct = "," + strProduct.Replace(" ", "") + ",";
                                    if (strProduct.IndexOf("," + lblProductID.Text.ToString() + ",") > -1)
                                    {

                                        decimal DicPrice = 0, TempDis = 0;
                                        if (DiscountPrice > 0 && DiscountPrice <= 99)
                                        {
                                            TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                            DicPrice = OrgiPrice - TempDis;
                                            lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                            GVShoppingCartItems.Columns[5].Visible = true;
                                            lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                            lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                        }
                                        else if (DiscountPrice >= 100)
                                        {
                                            TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                            DicPrice = TempDis;
                                            lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                            GVShoppingCartItems.Columns[5].Visible = true;
                                            lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                            lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));

                                        }
                                        else
                                        {
                                            lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                            GVShoppingCartItems.Columns[5].Visible = true;
                                            lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                            lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                        }
                                    }
                                    else
                                    {

                                        lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                        GVShoppingCartItems.Columns[5].Visible = true;
                                        lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                        lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    }
                                }
                                else
                                {
                                    lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(strProduct))
                        {
                            strProduct = "," + strProduct.Replace(" ", "") + ",";

                            if (strProduct.IndexOf("," + lblProductID.Text.ToString() + ",") > -1)
                            {
                                decimal DicPrice = 0, TempDis = 0;
                                if (DiscountPrice > 0 && DiscountPrice <= 99)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                    DicPrice = OrgiPrice - TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");

                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else if (DiscountPrice >= 100)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                    DicPrice = TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");

                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else
                                {
                                    lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                            }
                            else
                            {

                                lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                GVShoppingCartItems.Columns[5].Visible = true;
                                lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                            }
                        }
                        else
                        {

                            if (DiscountPrice > 0)
                            {
                                decimal DicPrice = 0, TempDis = 0;
                                if (DiscountPrice > 0 && DiscountPrice <= 99)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                    DicPrice = OrgiPrice - TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                }
                                else if (DiscountPrice >= 100)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                    DicPrice = TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                }
                                else
                                { lblOrginalDiscountPrice.Text = "0.00"; }

                                GVShoppingCartItems.Columns[5].Visible = true;
                                lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                            }
                            else
                            {
                                lblOrginalDiscountPrice.Text = "0.00";
                                GVShoppingCartItems.Columns[5].Visible = false;
                            }
                        }
                    }

                }
                else if ((Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null && Session["CustCouponCode"].ToString() != "" && Session["CustCouponCode"].ToString().ToLower().Trim() == "pricematch"))
                {
                    GVShoppingCartItems.Columns[5].Visible = true;
                    double dis = Convert.ToDouble(CommonComponent.GetScalarCommonData("select isnull(DiscountPrice,0) as DiscountPrice from tb_ShoppingCartItems where CustomCartID=" + lblCustomerCartChangePriceId.Text + ""));

                    txtcouponprice.Text = String.Format("{0:0.00}", Convert.ToDecimal(dis));
                    lblOrginalDiscountPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(dis));
                    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(dis) * Convert.ToDecimal(Quantity.ToString())));
                }
                else { GVShoppingCartItems.Columns[5].Visible = false; }

                #region Quantity Discount
                bool ChkQtyDiscount = false;
                decimal DecSubtotal = 0;
                string Price = "0";
                if (DiscountPrice > 0)
                {
                    Price = DiscountPrice.ToString();
                }
                else
                {
                    Price = lblSalePrice.Text.ToString();
                }

                Decimal QtyDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SElect ISNULL(qt.DiscountPercent,0) as DiscountPercent from tb_QuantityDiscountTable as qt " +
                                            " inner join tb_QauntityDiscount ON qt.QuantityDiscountID = dbo.tb_QauntityDiscount.QuantityDiscountID  " +
                                            " Where qt.LowQuantity <=" + Quantity.ToString() + " and qt.HighQuantity >=" + Quantity.ToString() + " and tb_QauntityDiscount.QuantityDiscountID in (Select QuantityDiscountID from  " +
                                            " tb_Product Where StoreId=" + ddlStore.SelectedValue.ToString() + " and ProductId=" + lblProductID.Text.ToString() + ") "));
                decimal.TryParse(lblSubTot.Text.ToString(), out DecSubtotal);
                if (QtyDiscount > Decimal.Zero)
                {

                    QtyDiscount = (Convert.ToDecimal(Price.ToString()) * QtyDiscount) / 100;
                    if (!string.IsNullOrEmpty(DecSubtotal.ToString().Trim()))
                    {
                        if (QtyDiscount > 0 && DecSubtotal > 0)
                        {
                            string dd = string.Format("{0:0.00}", QtyDiscount);
                            decimal QtyDis01 = (Convert.ToDecimal(Price) * Convert.ToDecimal(Quantity.ToString())) - (Convert.ToDecimal(dd) * Convert.ToDecimal(Quantity.ToString()));
                            lblSubTot.Text = "$" + DecSubtotal + "";
                            //  ltrSubTotal.Text = "<s>$" + Subtotal + "</s><br />$" + string.Format("{0:0.00}", QtyDis01) + "";
                        }
                        else
                            lblSubTot.Text = "$" + DecSubtotal + "";
                    }
                    if (Session["QtyDiscount"] != null)
                    {
                        string dd = string.Format("{0:0.00}", QtyDiscount);
                        decimal Qtydt = Convert.ToDecimal(Session["QtyDiscount"].ToString()) + (Convert.ToDecimal(dd) * Convert.ToDecimal(Quantity.ToString()));
                        Session["QtyDiscount"] = Qtydt;
                    }
                    else
                    {
                        string dd = string.Format("{0:0.00}", QtyDiscount);
                        Session["QtyDiscount"] = Convert.ToDouble(dd) * Convert.ToDouble(Quantity.ToString());
                    }
                }
                else
                {
                    lblSubTot.Text = "$" + DecSubtotal + "";
                }
                #endregion






            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));
        }

        /// <summary>
        /// Billing Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlB_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hfSubTotal.Value == "")
                hfSubTotal.Value = "0";
            if (hfTotal.Value == "")
                hfTotal.Value = "0";
            lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));

            ddlB_State.Items.Clear();
            if (ddlB_Country.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlB_Country.SelectedValue.ToString()));

                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlB_State.DataSource = dsState.Tables[0];
                    ddlB_State.DataTextField = "Name";
                    ddlB_State.DataValueField = "StateID";
                    ddlB_State.DataBind();
                    ddlB_State.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlB_State.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    ddlB_State.SelectedIndex = 0;
                }
                else
                {
                    ddlB_State.DataSource = null;
                    ddlB_State.DataBind();
                    ddlB_State.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlB_State.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlB_State.SelectedIndex = 0;
                }
            }
            else
            {
                ddlB_State.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlB_State.Items.Insert(1, new ListItem("Other", "-11"));
                ddlB_State.SelectedIndex = 0;
            }
            if (chkAddress.Checked)
            {
                ddlS_Country.SelectedIndex = ddlB_Country.SelectedIndex;
                ddlS_Country_SelectedIndexChanged(null, null);
                ddlS_State.SelectedIndex = ddlB_State.SelectedIndex;

            }
            if (ddlB_Country.SelectedValue.ToString() == "1")
            {
                txtB_Zip.Attributes.Add("onkeypress", "return keyRestrictforIntOnly(event,'0123456789');");
            }
            else
            {
                txtB_Zip.Attributes.Remove("onkeypress");
            }
            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            {
                if (rdoCreditCard.Checked)
                {
                    rdoCreditCard.Checked = true;
                }
            }
        }

        /// <summary>
        /// Shipping Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlS_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));

            ddlS_State.Items.Clear();
            if (ddlS_Country.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlS_Country.SelectedValue.ToString()));
                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlS_State.DataSource = dsState.Tables[0];
                    ddlS_State.DataTextField = "Name";
                    ddlS_State.DataValueField = "StateID";
                    ddlS_State.DataBind();
                    ddlS_State.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlS_State.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    ddlS_State.SelectedIndex = 0;
                }

                else
                {
                    ddlS_State.DataSource = null;
                    ddlS_State.DataBind();
                    ddlS_State.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlS_State.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlS_State.SelectedIndex = 0;
                }
            }
            else
            {
                ddlS_State.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlS_State.Items.Insert(1, new ListItem("Other", "-11"));
                ddlS_State.SelectedIndex = 0;


            }
            if (chkAddress.Checked == false)
            {
                hdncountry.Value = ddlS_Country.SelectedValue.ToString();
                BindShippingMethod();
            }
            if (ddlS_Country.SelectedValue.ToString() == "1")
            {
                txtS_Zip.Attributes.Add("onkeypress", "return keyRestrictforIntOnly(event,'0123456789');");
            }
            else
            {
                txtS_Zip.Attributes.Remove("onkeypress");
            }
            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            {
                if (rdoCreditCard.Checked)
                {
                    rdoCreditCard.Checked = true;
                }
            }
        }

        /// <summary>
        /// Credit Card Radio Button Checked Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void rdoCreditCard_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCreditCard.Checked)
            {
                tblCheque.Visible = false;
                tblCreditCard.Visible = true;
                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                {
                    btnSave_Click(null, null);
                }
            }
            else
            {
                tblCheque.Visible = true;
                tblCreditCard.Visible = false;
            }
        }

        /// <summary>
        ///  Shopping Cart Items Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnshoppingcartitems_click(object sender, EventArgs e)
        {
            lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));
            GetShippIngMethodByBillAddress();
            BindShippingMethod();
            //BindFreightShippingMethod();
            BindCartInGrid();
        }

        private void BindFreightShippingMethod()
        {
            decimal Weight = 0;
            Decimal ShipmentWeight = 0;

            //ShipmentWeight = Convert.ToDecimal(AppLogic.AppConfigs("FedEx.MaxWeight"));
            //if (ViewState["hdnWeightTotal"] != null)
            //{
            //    Weight = Convert.ToDecimal(ViewState["hdnWeightTotal"].ToString());
            //}
            //if (ViewState["hdnWeightTotal1"] != null)
            //{
            //    Weight += Convert.ToDecimal(ViewState["hdnWeightTotal1"].ToString());
            //}

            //if (ViewState["IsFrieght"] != null && ViewState["IsFrieght"].ToString() == "1" && Weight > 0 && ShipmentWeight > 0)
            //{
            //    if ((Weight > ShipmentWeight))
            //    {
            //        ListItem li = new ListItem("Freight", "Freight");
            //        ddlShippingMethod.Items.Insert(ddlShippingMethod.Items.Count, li);
            //    }
            //}

            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(hdncountry.Value.ToString()));
            if (!string.IsNullOrEmpty(CountryCode.ToString().Trim()) && (CountryCode.ToUpper() == "US" || CountryCode.ToUpper() == "CA" || CountryCode.ToUpper() == "AU" || CountryCode.ToUpper() == "GB"))
            {
                if (ddlShippingMethod.Items.Count == 0)
                {
                    ListItem li = new ListItem("Freight", "Freight");
                    ddlShippingMethod.Items.Insert(0, li);
                }
                else
                {
                    ListItem li = new ListItem("Freight", "Freight");
                    ddlShippingMethod.Items.Insert(ddlShippingMethod.Items.Count, li);
                }
            }
            else
            {
                ddlShippingMethod.Items.Clear();
                ListItem li = new ListItem("Bongo International", "Bongo International");
                ddlShippingMethod.Items.Insert(ddlShippingMethod.Items.Count, li);
            }
        }

        /// <summary>
        /// Shipping Methods Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlShippingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal ShippingCost = 0;
            if (ddlShippingMethod.SelectedValue.ToString() != "")
            {
                try
                {


                    if (ddlShippingMethod.SelectedItem.Text.ToString().ToLower().IndexOf("free shipping") > -1)
                    {
                        TxtShippingCost.Attributes.Add("readonly", "true");
                    }
                    else
                    {
                        TxtShippingCost.Attributes.Remove("readonly");
                    }
                }
                catch { }
                int Index = ddlShippingMethod.SelectedItem.Text.IndexOf("($");
                int Length = ddlShippingMethod.SelectedItem.Text.LastIndexOf(")") - Index;
                if (Index != -1 && Length != 0)
                {
                    string strShippingCost = ddlShippingMethod.SelectedItem.Text.Substring(Index + 2, Length - 2).Trim();
                    Decimal.TryParse(strShippingCost, out ShippingCost);
                }
                ViewState["selectedShippingmethod"] = ddlShippingMethod.SelectedItem.Text.ToString();
            }

            TxtShippingCost.Text = ShippingCost.ToString("f2");

            decimal subtotal = 0;
            decimal.TryParse(lblSubTotal.Text, out subtotal);

            decimal tax = 0;
            try
            {
                tax = Convert.ToDecimal(TxtTax.Text.Trim());
            }
            catch
            {

            }
            decimal ship = 0;
            try
            {
                ship = Convert.ToDecimal(TxtShippingCost.Text.Trim());
            }
            catch
            {

            }

            decimal discount = 0;
            try
            {
                discount = Convert.ToDecimal(TxtDiscount.Text.Trim());
            }
            catch
            {

            }
            try
            {
                if (hfSubTotal.Value.ToString() != "")
                {

                    if (Request.QueryString["CustID"] != null && !string.IsNullOrEmpty(Request.QueryString["CustID"].ToString()))
                    {
                        try
                        {
                            string filedoc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT DocFile FROM dbo.tb_Customer WHERE CustomerID ='" + Request.QueryString["CustID"].ToString() + "'"));
                            string CustDocPathTemp = AppLogic.AppConfigs("Customer.DocumentPath");

                            if (File.Exists(Server.MapPath(CustDocPathTemp + Request.QueryString["CustID"].ToString() + "/" + filedoc)))
                            {
                                TxtTax.Text = "0";
                            }
                            else
                            {
                                TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                            }
                        }
                        catch
                        {
                            TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                        }

                    }
                    else
                    {
                        TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                    }


                }
                else
                {
                    TxtTax.Text = "0";
                }
            }
            catch { }
            decimal FinalSubTotal = (subtotal + tax + ship) - discount;
            if (FinalSubTotal < 0)
            {
                FinalSubTotal = 0;
            }
            lblTotal.Text = FinalSubTotal.ToString("f2");
            hfSubTotal.Value = lblSubTotal.Text;
            hfTotal.Value = lblTotal.Text;
            BindCartInGrid();
            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            {
                if (rdoCreditCard.Checked)
                {
                    rdoCreditCard.Checked = true;
                }
            }

        }

        /// <summary>
        /// Billing State Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlB_State_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));
            if (ddlB_State.SelectedValue == "-11")
            {
                txtB_OtherState.Visible = true;

                //ddlB_State.Visible = false;
            }
            else
            {
                txtB_OtherState.Visible = false;

                //ddlB_State.Visible = true;
            } txtB_Zip.Focus();
            GetShippIngMethodByBillAddress();
            BindShippingMethod();
            // BindFreightShippingMethod();
            trMsg.Visible = false;
            lblMsg.Text = "";
            if (chkAddress.Checked)
            {
                ddlS_State.SelectedIndex = ddlB_State.SelectedIndex;

            }
        }

        /// <summary>
        /// Shipping State Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlS_State_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));
            if (ddlS_State.SelectedValue == "-11")
            {
                txtS_OtherState.Visible = true;

                ddlS_State.Visible = true;
            }
            else
            {
                txtS_OtherState.Visible = false;

                ddlS_State.Visible = true;
            }
            txtS_Zip.Focus();
            GetShippIngMethodByBillAddress();
            BindShippingMethod();
            // BindFreightShippingMethod();
            trMsg.Visible = false;
            lblMsg.Text = "";
        }

        /// <summary>
        /// Billing Zip code Text Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtB_Zip_TextChanged(object sender, EventArgs e)
        {
            lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));
            GetShippIngMethodByBillAddress();
            BindShippingMethod();
            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            {
                if (rdoCreditCard.Checked)
                {
                    rdoCreditCard.Checked = true;
                }
            }
            // BindFreightShippingMethod();
        }

        /// <summary>
        /// Address Check box Checked Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void chkAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAddress.Checked == false)
            {
                if (Session["ShipppCiutomer"] != null)
                {
                    DataSet dsCustomer = new DataSet();
                    dsCustomer = (DataSet)Session["ShipppCiutomer"];
                    if (dsCustomer != null && dsCustomer.Tables.Count > 1 && dsCustomer.Tables[0].Rows.Count > 0)
                    {
                        Session["ShipppCiutomer"] = dsCustomer;
                        txtS_FName.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString());
                        txtS_Company.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Company"].ToString());
                        txtS_LNAme.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString());
                        if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Address1"].ToString()))
                        {
                            txtS_Add1.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address1"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Address2"].ToString()))
                        {
                            txtS_Add2.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address2"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Suite"].ToString()))
                        {
                            txtS_Suite.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Suite"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["City"].ToString()))
                        {
                            txtS_City.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["City"].ToString());
                        }
                        ddlS_Country.ClearSelection();
                        if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Country"].ToString()))
                        {
                            ddlS_Country.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Country"].ToString());
                            ddlS_Country_SelectedIndexChanged(null, null);
                        }
                        ddlS_State.ClearSelection();
                        if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["State"].ToString()))
                        {
                            try
                            {
                                ddlS_State.Items.FindByText(Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"].ToString())).Selected = true;
                            }
                            catch
                            {
                                ddlS_State.Items.FindByText("Other").Selected = true;
                                txtS_OtherState.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"].ToString());
                            }
                        }
                        if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString()))
                        {
                            txtS_Zip.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString());
                        }

                        if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Phone"].ToString()))
                        {
                            txtS_Phone.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Phone"].ToString());
                        }
                    }
                }
            }
            else
            {
                txtS_FName.Text = txtB_FName.Text.ToString().Trim();
                txtS_LNAme.Text = txtB_LName.Text.ToString().Trim();
                txtS_Company.Text = txtB_Company.Text.ToString().Trim();
                txtS_Add1.Text = txtB_Add1.Text.ToString().Trim();
                txtS_Add2.Text = txtB_Add2.Text.ToString().Trim();
                txtS_Suite.Text = txtB_Suite.Text.ToString().Trim();
                txtS_City.Text = txtB_City.Text.ToString().Trim();
                ddlS_Country.ClearSelection();
                ddlS_Country.SelectedValue = ddlB_Country.SelectedValue.ToString();
                ddlS_Country_SelectedIndexChanged(null, null);
                ddlS_State.ClearSelection();
                try
                {
                    ddlS_State.SelectedValue = ddlB_State.SelectedValue.ToString();
                    txtS_OtherState.Text = txtB_OtherState.Text.ToString().Trim();
                }
                catch
                {
                    ddlS_State.Items.FindByText("Other").Selected = true;
                    txtS_OtherState.Text = txtB_OtherState.Text.ToString().Trim();
                }
                txtS_Zip.Text = txtB_Zip.Text.ToString().Trim();
                txtS_Phone.Text = txtB_Phone.Text.ToString().Trim();

            }
            GetShippIngMethodByBillAddress();
            BindShippingMethod();
            // BindFreightShippingMethod();
        }

        /// <summary>
        /// Cheque Radio Button Checked Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void rdoCheque_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoCheque.Checked)
            {
                tblCheque.Visible = true;
                tblCreditCard.Visible = false;
                hdnOrdernumber.Value = "0";
            }
            else
            {
                tblCheque.Visible = false;
                tblCreditCard.Visible = true;
            }
        }

        /// <summary>
        /// Insert New Customer Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void aRelated_Click(object sender, ImageClickEventArgs e)
        {
            //TxtEmail.ReadOnly = true;
            //RequiredFieldValidator16.Visible = false;
            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            {
                if (rdoCreditCard.Checked)
                {
                    rdoCreditCard.Checked = true;
                }
            }
            if (HdnCustID.Value == "" || HdnCustID.Value == "0")
            {
                CustomerComponent objCustomer = new CustomerComponent();
                tb_Customer objCust = new tb_Customer();
                Int32 CustID = -1;


                CustID = objCustomer.InsertCustomer(objCust, Convert.ToInt32(ddlStore.SelectedValue.ToString()));
                HdnCustID.Value = Convert.ToString(CustID);
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "openCenteredCrossSaleWindow('ContentPlaceHolder1_lblSubTotal','ContentPlaceHolder1_lblSubTotal');", true);

            //if (InsertCustomer())
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "openCenteredCrossSaleWindow('ContentPlaceHolder1_lblSubTotal','ContentPlaceHolder1_lblSubTotal');", true);
            //}
            //else
            //{
            //    if (HdnCustID.Value.ToString() == "0")
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Email Address Already Exists, Please Enter different Email Address.','Required Information','ContentPlaceHolder1_TxtEmail');", true);
            //    }
            //    else
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "openCenteredCrossSaleWindow('ContentPlaceHolder1_lblSubTotal','ContentPlaceHolder1_lblSubTotal');", true);
            //    }
            //}
        }

        /// <summary>
        /// Inserts the Customer
        /// </summary>
        /// <returns>Returns true if Inserted, false otherwise</returns>
        private bool InsertCustomer()
        {
            if (HdnCustID.Value != "" && Convert.ToInt32(HdnCustID.Value) <= 0)
            {
                OrderComponent objCust = new OrderComponent();
                tb_Order objCustomer = new tb_Order();
                objCustomer.Email = TxtEmail.Text.ToString();
                objCustomer.FirstName = txtB_FName.Text.ToString();
                objCustomer.LastName = txtB_LName.Text.ToString();
                objCustomer.BillingCompany = txtB_Company.Text.ToString();
                objCustomer.BillingFirstName = txtB_FName.Text.ToString();
                objCustomer.BillingLastName = txtB_LName.Text.ToString();
                objCustomer.BillingAddress1 = txtB_Add1.Text.ToString();
                objCustomer.BillingAddress2 = txtB_Add2.Text.ToString();
                objCustomer.BillingCity = txtB_City.Text.ToString();
                if (ddlB_State.SelectedValue.ToString() == "-11")
                {
                    objCustomer.BillingState = txtB_OtherState.Text.ToString();
                }
                else
                {
                    objCustomer.BillingState = ddlB_State.SelectedItem.Text.ToString();
                }
                objCustomer.BillingSuite = txtB_Suite.Text.ToString();
                objCustomer.BillingZip = txtB_Zip.Text.ToString();
                objCustomer.BillingCountry = ddlB_Country.SelectedItem.Text.ToString();
                objCustomer.BillingEmail = TxtEmail.Text.ToString();
                objCustomer.BillingPhone = txtB_Phone.Text.ToString();
                objCustomer.ShippingFirstName = txtS_FName.Text.ToString();
                objCustomer.ShippingCompany = txtS_Company.Text.ToString();
                objCustomer.ShippingLastName = txtS_LNAme.Text.ToString();
                objCustomer.ShippingAddress1 = txtS_Add1.Text.ToString();
                objCustomer.ShippingAddress2 = txtS_Add2.Text.ToString();
                objCustomer.ShippingCity = txtS_City.Text.ToString();
                if (ddlS_State.SelectedValue.ToString() == "-11")
                {
                    objCustomer.ShippingState = txtS_OtherState.Text.ToString();
                }
                else
                {
                    objCustomer.ShippingState = ddlS_State.SelectedItem.Text.ToString();
                }
                objCustomer.ShippingSuite = txtS_Suite.Text.ToString();
                objCustomer.ShippingZip = txtS_Zip.Text.ToString();
                objCustomer.ShippingCountry = ddlS_Country.SelectedItem.Text.ToString();
                objCustomer.ShippingEmail = TxtEmail.Text.ToString();
                objCustomer.ShippingPhone = txtS_Phone.Text.ToString();
                objCustomer.LastIPAddress = Request.UserHostAddress.ToString();
                Int32 CustomerId = objCust.PhoneOrderAddCustomer(objCustomer, Convert.ToInt32(ddlStore.SelectedValue.ToString()));

                if (CustomerId > 0)
                {
                    HdnCustID.Value = Convert.ToString(CustomerId);
                    return true;
                }
                else
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// Finally Place order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(hdncountry.Value.ToString()));
            if (CountryCode.ToString().ToLower() != "us" && CountryCode.ToString().ToLower() != "au" && CountryCode.ToString().ToLower() != "ca" && CountryCode.ToString().ToLower() != "gb" && CountryCode.ToString().Trim().ToUpper() != "CANADA" && CountryCode.ToString().Trim().ToUpper() != "AUSTRALIA" && CountryCode.ToString().Trim().ToUpper() != "UNITED KINGDOM" && CountryCode.ToString().Trim().ToUpper() != "UNITED STATES")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please order place by Bongo international setup.','Message');", true);
                return;
            }

            if (rdoCreditCard.Checked)
            {
                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                {
                    if (TxtNameOnCard.Text.ToString().Trim() == "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Name of Card.','Required Information','ContentPlaceHolder1_TxtNameOnCard');", true);
                        TxtNameOnCard.Focus();
                        return;
                    }
                    else if (ddlCardType.SelectedIndex == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Card Type.','Required Information','ContentPlaceHolder1_ddlCardType');", true);
                        ddlCardType.Focus();
                        return;
                    }
                    else if (TxtCardNumber.Text.ToString().Trim() == "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Card Number.','Required Information','ContentPlaceHolder1_TxtCardNumber');", true);
                        TxtCardNumber.Focus();
                        return;
                    }
                    //else if (TxtCardVerificationCode.Text.ToString().Trim() == "")
                    //{
                    //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Card Verification Code.','Required Information','ContentPlaceHolder1_TxtCardVerificationCode');", true);
                    //    TxtCardVerificationCode.Focus();
                    //    return;
                    //}
                    if (TxtCardNumber.Text.ToString().Trim() != "" && TxtCardNumber.Text.ToString().Trim().IndexOf("*") < -1)
                    {
                        bool chkflg = false;
                        long crdNumber = 0;
                        chkflg = long.TryParse(TxtCardNumber.Text.ToString(), out crdNumber);
                        if (!chkflg)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter valid Numeric Card Number','Required Information','ContentPlaceHolder1_TxtCardNumber');", true);
                            TxtCardNumber.Focus();
                            return;
                        }
                    }
                    if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && TxtCardNumber.Text.ToString().Trim() != "" && TxtCardNumber.Text.ToString().Trim().Length < 16)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Credit Card Number must be 16 digit long.','Required Information','ContentPlaceHolder1_TxtCardNumber');", true);
                        TxtCardNumber.Focus();
                        return;
                    }
                    if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && TxtCardNumber.Text.ToString().Trim() != "" && (TxtCardNumber.Text.ToString().Trim().Length < 15 || TxtCardNumber.Text.ToString().Trim().Length > 15))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Credit Card Number must be 15 digit long.','Required Information','ContentPlaceHolder1_TxtCardNumber');", true);
                        TxtCardNumber.Focus();
                        return;
                    }

                    if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && TxtCardVerificationCode.Text.ToString().Trim() != "" && (TxtCardVerificationCode.Text.ToString().Trim().Length < 3 || TxtCardVerificationCode.Text.ToString().Trim().Length > 3))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Card Verification Code must be 3 digit long.','Required Information','ContentPlaceHolder1_TxtCardVerificationCode');", true);
                        TxtCardVerificationCode.Focus();
                        return;
                    }
                    if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && TxtCardVerificationCode.Text.ToString().Trim() != "" && TxtCardVerificationCode.Text.ToString().Trim().Length < 4)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Card Verification Code must be 4 digit long.','Required Information','ContentPlaceHolder1_TxtCardVerificationCode');", true);
                        TxtCardVerificationCode.Focus();
                        return;
                    }
                    if (ddlMonth.SelectedIndex == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Month.','Required Information','ContentPlaceHolder1_ddlMonth');", true);
                        ddlMonth.Focus();
                        return;
                    }
                    else if (ddlYear.SelectedIndex == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Year.','Required Information','ContentPlaceHolder1_ddlYear');", true);
                        ddlYear.Focus();
                        return;
                    }
                    else if ((Convert.ToInt32(ddlYear.SelectedValue) > DateTime.Now.Year) || (Convert.ToInt32(ddlYear.SelectedValue) == DateTime.Now.Year && Convert.ToInt32(ddlMonth.SelectedValue) >= DateTime.Now.Month))
                    {

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Valid Expiration Date.','Required Information','ContentPlaceHolder1_ddlYear');", true);
                        return;
                    }
                }
                if (GVShoppingCartItems.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Your Shopping Cart is Empty.','Sorry!','ContentPlaceHolder1_aRelated');", true);
                    return;
                }
                if (ddlShippingMethod.SelectedValue.ToString().ToLower().Contains("freight") && Convert.ToDecimal(TxtShippingCost.Text) <= 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Shipping Cost for " + ddlShippingMethod.SelectedValue.ToString() + " Method.','Required Information','ContentPlaceHolder1_TxtShippingCost');", true);
                    return;
                }
                try
                {
                    if (hdnIsSalesManager.Value != null && hdnIsSalesManager.Value == "1")
                    {
                        if (!string.IsNullOrEmpty(TxtDiscount.Text.ToString()) && Convert.ToDecimal(TxtDiscount.Text.ToString()) > 0)
                        {
                            decimal SubTot = Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("%", ""));
                            decimal SubTotDispercent = (SubTot * 10) / 100;
                            decimal Discount = Convert.ToDecimal(TxtDiscount.Text.ToString());
                            if (Discount > SubTotDispercent)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Discount Should not be apply grater than 10% of SubTotal.','Required Information','ContentPlaceHolder1_TxtDiscount');", true);
                                TxtDiscount.Text = "0";
                                return;
                            }
                        }
                    }
                }
                catch { }
                try
                {
                    if (Request.QueryString["CustId"] == null)
                    {
                        if (!String.IsNullOrEmpty(TxtEmail.Text))
                        {
                            Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(customerid) from tb_Customer where Email='" + TxtEmail.Text.ToString().Trim().Replace("'", "''") + "' and storeid=" + ddlStore.SelectedValue.ToString() + " and isnull(Active,0)=1 and isnull(IsRegistered,0)=1 and isnull(Deleted,0)=0"));
                            if (Count > 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Email Address Already Exists, Please Enter different Email Address.','Required Information','ContentPlaceHolder1_TxtEmail');", true);
                                return;
                            }
                        }
                    }
                }
                catch { }

                AddOrder();
            }
            else
            {
                if (txtCheque.Text.ToString().Trim() == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter other payment detail.','Required Information','ContentPlaceHolder1_txtCheque');", true);
                    txtCheque.Focus();
                    return;
                }
                if (GVShoppingCartItems.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Your Shopping Cart is Empty.','Sorry!','ContentPlaceHolder1_aRelated');", true);
                    return;
                }
                if (ddlShippingMethod.SelectedValue.ToString().ToLower().Contains("freight") && Convert.ToDecimal(TxtShippingCost.Text) <= 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Shipping Cost for " + ddlShippingMethod.SelectedValue.ToString() + " Method.','Required Information','ContentPlaceHolder1_TxtShippingCost');", true);
                    return;
                }
                try
                {
                    if (hdnIsSalesManager.Value != null && hdnIsSalesManager.Value == "1")
                    {
                        if (!string.IsNullOrEmpty(TxtDiscount.Text.ToString()) && Convert.ToDecimal(TxtDiscount.Text.ToString()) > 0)
                        {
                            decimal SubTot = Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("%", ""));
                            decimal SubTotDispercent = (SubTot * 10) / 100;
                            decimal Discount = Convert.ToDecimal(TxtDiscount.Text.ToString());
                            if (Discount > SubTotDispercent)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Discount Should not be apply grater than 10% of SubTotal.','Required Information','ContentPlaceHolder1_TxtDiscount');", true);
                                TxtDiscount.Text = "0";
                                return;
                            }
                        }


                    }
                }
                catch { }
                try
                {
                    if (Request.QueryString["CustId"] == null)
                    {
                        if (!String.IsNullOrEmpty(TxtEmail.Text))
                        {
                            Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(customerid) from tb_Customer where Email='" + TxtEmail.Text.ToString().Trim().Replace("'", "''") + "' and storeid=" + ddlStore.SelectedValue.ToString() + " and isnull(Active,0)=1 and isnull(IsRegistered,0)=1 and isnull(Deleted,0)=0"));
                            if (Count > 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Email Address Already Exists, Please Enter different Email Address.','Required Information','ContentPlaceHolder1_TxtEmail');", true);
                                return;
                            }
                        }
                    }
                }
                catch { }
                AddOrder();
            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/Admin/Orders/CustomerPhoneOrder.aspx");
        }

        /// <summary>
        /// Gets the customer details for payment.
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns tb_Order Table Object</returns>
        private tb_Order GetCustomerDetailsForpayment(Int32 CustomerID)
        {
            DataSet dsCustomer = new DataSet();
            CustomerComponent objCust = new CustomerComponent();
            dsCustomer = objCust.GetCustomerDetails(CustomerID);

            //Billing Address
            tb_Order objorderData = new tb_Order();

            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {

                objorderData.FirstName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString());
                objorderData.LastName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString());
                objorderData.Email = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());
                objorderData.BillingFirstName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString());
                objorderData.BillingLastName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString());
                objorderData.BillingAddress1 = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address1"].ToString());
                objorderData.BillingAddress2 = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address2"].ToString());
                objorderData.BillingSuite = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Suite"].ToString());
                objorderData.BillingCity = Convert.ToString(dsCustomer.Tables[0].Rows[0]["City"].ToString());
                objorderData.BillingState = Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"].ToString());
                objorderData.BillingZip = Convert.ToString(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString());
                objorderData.BillingCountry = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CountryName"].ToString());
                objorderData.BillingPhone = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Phone"].ToString());
                objorderData.BillingEmail = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());
                objorderData.CardName = Convert.ToString(TxtNameOnCard.Text.Trim());
                objorderData.CardType = Convert.ToString(ddlCardType.SelectedItem.Text.ToString());
                objorderData.CardVarificationCode = Convert.ToString(TxtCardVerificationCode.Text.ToString());
                if (TxtCardNumber.Text.ToString().IndexOf("*") > -1)
                {
                    objorderData.CardNumber = SecurityComponent.Decrypt(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString());
                }
                else
                {
                    objorderData.CardNumber = TxtCardNumber.Text.ToString();
                }
                objorderData.CardExpirationMonth = ddlMonth.SelectedValue.ToString();
                objorderData.CardExpirationYear = ddlYear.SelectedValue.ToString();
                // Credit Card Details
            }


            //Shipping Address

            if (dsCustomer != null && dsCustomer.Tables.Count > 1 && dsCustomer.Tables[1].Rows.Count > 0)
            {
                objorderData.ShippingFirstName = Convert.ToString(dsCustomer.Tables[1].Rows[0]["FirstName"].ToString());
                objorderData.ShippingLastName = Convert.ToString(dsCustomer.Tables[1].Rows[0]["LastName"].ToString());
                objorderData.ShippingAddress1 = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address1"].ToString());
                objorderData.ShippingAddress2 = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address2"].ToString());
                objorderData.ShippingSuite = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Suite"].ToString());
                objorderData.ShippingCity = Convert.ToString(dsCustomer.Tables[1].Rows[0]["City"].ToString());
                objorderData.ShippingState = Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"].ToString());
                objorderData.ShippingZip = Convert.ToString(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString());
                objorderData.ShippingCountry = Convert.ToString(dsCustomer.Tables[1].Rows[0]["CountryName"].ToString());
                objorderData.ShippingPhone = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Phone"].ToString());
                objorderData.ShippingEmail = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Email"].ToString());
            }


            return objorderData;
        }

        /// <summary>
        /// Get Customer Billing and Shipping Details while Adding Order
        /// </summary>
        /// <returns>Returns the Object of tb_Order</returns>
        private tb_Order GetCustomerDetailsForAddOrder()
        {
            tb_Order objCustomer = new tb_Order();
            objCustomer.Email = TxtEmail.Text.ToString();
            objCustomer.FirstName = txtB_FName.Text.ToString();
            objCustomer.LastName = txtB_LName.Text.ToString();
            objCustomer.BillingCompany = txtB_Company.Text.ToString();
            objCustomer.BillingFirstName = txtB_FName.Text.ToString();
            objCustomer.BillingLastName = txtB_LName.Text.ToString();
            objCustomer.BillingAddress1 = txtB_Add1.Text.ToString();
            objCustomer.BillingAddress2 = txtB_Add2.Text.ToString();
            objCustomer.BillingCity = txtB_City.Text.ToString();
            if (ddlB_State.SelectedValue.ToString() == "-11")
            {
                objCustomer.BillingState = txtB_OtherState.Text.ToString();
            }
            else
            {
                objCustomer.BillingState = ddlB_State.SelectedItem.Text.ToString();
            }
            objCustomer.BillingSuite = txtB_Suite.Text.ToString();
            objCustomer.BillingZip = txtB_Zip.Text.ToString();
            objCustomer.BillingCountry = ddlB_Country.SelectedItem.Text.ToString();
            objCustomer.BillingEmail = TxtEmail.Text.ToString();
            objCustomer.BillingPhone = txtB_Phone.Text.ToString();
            objCustomer.ShippingFirstName = txtS_FName.Text.ToString();
            objCustomer.ShippingCompany = txtS_Company.Text.ToString();
            objCustomer.ShippingLastName = txtS_LNAme.Text.ToString();
            objCustomer.ShippingAddress1 = txtS_Add1.Text.ToString();
            objCustomer.ShippingAddress2 = txtS_Add2.Text.ToString();
            objCustomer.ShippingCity = txtS_City.Text.ToString();
            if (ddlS_State.SelectedValue.ToString() == "-11")
            {
                objCustomer.ShippingState = txtS_OtherState.Text.ToString();
            }
            else
            {
                objCustomer.ShippingState = ddlS_State.SelectedItem.Text.ToString();
            }
            objCustomer.ShippingSuite = txtS_Suite.Text.ToString();
            objCustomer.ShippingZip = txtS_Zip.Text.ToString();
            objCustomer.ShippingCountry = ddlS_Country.SelectedItem.Text.ToString();
            objCustomer.ShippingEmail = TxtEmail.Text.ToString();
            objCustomer.ShippingPhone = txtS_Phone.Text.ToString();
            objCustomer.LastIPAddress = Request.UserHostAddress.ToString();
            objCustomer.BillingEqualsShipping = Convert.ToBoolean(chkAddress.Checked);
            return objCustomer;
        }

        private string GenerateRandomCode()
        {

            Random random = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;

        }

        /// <summary>
        /// Sending Mail to Customer EmailID
        /// </summary>
        private void SendMailCreateAccount()
        {
            CustomerComponent objCustomer = new CustomerComponent();
            DataSet dsCreateAccount = new DataSet();
            dsCreateAccount = objCustomer.GetEmailTamplate("CreateAccount", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsCreateAccount != null && dsCreateAccount.Tables.Count > 0 && dsCreateAccount.Tables[0].Rows.Count > 0)
            {

                string strpassword = GenerateRandomCode().ToString();
                if (HdnCustID.Value != null && HdnCustID.Value != "")
                { CommonComponent.ExecuteCommonData("update tb_customer set password = '" + Convert.ToString(SecurityComponent.Encrypt(strpassword) + "'") + " where CustomerID =" + HdnCustID.Value); }

                String strBody = "";
                String strSubject = "";
                strBody = dsCreateAccount.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsCreateAccount.Tables[0].Rows[0]["Subject"].ToString();

                strSubject = Regex.Replace(strSubject, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###FIRSTNAME###", txtB_FName.Text.ToString() + ' ' + txtB_LName.Text.ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###USERNAME###", TxtEmail.Text.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###PASSWORD###", strpassword, RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);


                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMail(TxtEmail.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }


        /// <summary>
        /// Add New Phone Order
        /// </summary>
        private void AddOrder()
        {
            tb_Order objOrder = new tb_Order();
            objOrder = GetCustomerDetailsForAddOrder();
            objOrder.CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());

            Decimal Storecreaditamount = 0;
            decimal.TryParse(txtstorecreaditAmount.Text, out Storecreaditamount);

            if (rdoCreditCard.Checked && AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && !Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            {
                objOrder.CardType = ddlCardType.SelectedItem.Text.ToString();
                objOrder.CardVarificationCode = TxtCardVerificationCode.Text.ToString();
                objOrder.CardName = TxtNameOnCard.Text.Trim().ToString();
                objOrder.CardExpirationMonth = ddlMonth.SelectedValue.ToString();
                objOrder.CardExpirationYear = ddlYear.SelectedValue.ToString();
                if (TxtCardNumber.Text.ToString().IndexOf("*") > -1)
                {
                    if (Session["CardNumber"] != null)
                    {
                        objOrder.CardNumber = SecurityComponent.Encrypt(Session["CardNumber"].ToString());
                        try
                        {
                            objOrder.Last4 = Session["CardNumber"].ToString().Substring(Session["CardNumber"].ToString().Length - 4);
                        }
                        catch { }
                    }

                }
                else
                {
                    objOrder.CardNumber = SecurityComponent.Encrypt(TxtCardNumber.Text.ToString());
                    if (TxtCardNumber.Text.ToString().Length > 4)
                    {
                        try
                        {


                            objOrder.Last4 = TxtCardNumber.Text.ToString().Substring(TxtCardNumber.Text.ToString().Length - 4);
                        }
                        catch { }
                    }
                }
            }
            if (ddlShippingMethod.SelectedValue.ToString() != "")
            {
                int Index = ddlShippingMethod.SelectedItem.Text.IndexOf("($");
                if (Index > -1)
                {
                    objOrder.ShippingMethod = ddlShippingMethod.SelectedItem.Text.ToString().Substring(0, Index);
                }
            }
            objOrder.CouponDiscountAmount = decimal.Zero;
            objOrder.QuantityDiscountAmount = decimal.Zero;
            objOrder.CouponCode = "";


            // objOrder.Notes = TxtNotes.Text.ToString();


            objOrder.OrderNotes = TxtNotes.Text.ToString();


            if (Convert.ToDecimal(hfSubTotal.Value.ToString()) > Decimal.Zero || Convert.ToDecimal(hfSubTotal.Value.ToString()) < Decimal.Zero)
            {
                objOrder.OrderSubtotal = Convert.ToDecimal(hfSubTotal.Value.ToString());
            }
            else
            {
                objOrder.OrderSubtotal = Convert.ToDecimal(lblSubTotal.Text.ToString());

            }
            if (Convert.ToDecimal(hfTotal.Value.ToString()) > Decimal.Zero || Convert.ToDecimal(hfTotal.Value.ToString()) < Decimal.Zero)
            {
                objOrder.OrderTotal = Convert.ToDecimal(hfTotal.Value.ToString());
            }
            else
            {
                objOrder.OrderTotal = Convert.ToDecimal(lblTotal.Text.ToString());
            }
            objOrder.OrderShippingCosts = Convert.ToDecimal(TxtShippingCost.Text.ToString());
            objOrder.OrderTax = Convert.ToDecimal(TxtTax.Text.ToString());

            OrderComponent objPayment = new OrderComponent();
            DataSet dsPayment = new DataSet();
            dsPayment = objPayment.GetPaymentGateway("CREDITCARD", Convert.ToInt32(AppLogic.AppConfigs("StoreId").ToString()));
            string strPaymentgateWay = "";
            string strPaymentgateWaystatus = "";
            if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
            {
                strPaymentgateWay = Convert.ToString(dsPayment.Tables[0].Rows[0]["PaymentService"].ToString().ToUpper());
                strPaymentgateWaystatus = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString().ToUpper());
            }

            if (rdoCreditCard.Checked)
            {
                objOrder.Deleted = true;
                objOrder.PaymentMethod = "CREDITCARD";
                objOrder.PaymentGateway = strPaymentgateWay.ToString().ToUpper();
            }
            else
            {
                objOrder.Deleted = false;
                objOrder.PaymentMethod = "OTHER";
                objOrder.PaymentGateway = "OTHER";
                objOrder.TransactionStatus = "CAPTURED";
                objOrder.CapturedOn = DateTime.Now;
            }



            if (!string.IsNullOrEmpty(txtstorecreaditno.Text) && !string.IsNullOrEmpty(Storecreaditamount.ToString()) && Storecreaditamount > 0)
            {

                objOrder.Storecreaditno = txtstorecreaditno.Text;
                objOrder.StorecreaditAmont = Convert.ToDecimal(txtstorecreaditAmount.Text);

                objOrder.CustomDiscount = Convert.ToDecimal(TxtDiscount.Text.ToString());

            }
            else
            {

                objOrder.CustomDiscount = Convert.ToDecimal(TxtDiscount.Text.ToString());
            }
            objOrder.IsNew = true;
            objOrder.LastIPAddress = Request.UserHostAddress.ToString();
            OrderComponent objAddOrder = new OrderComponent();
            Int32 OrderNumber = 0;
            //if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            //{
            //    if (hdnOrdernumber.Value == "0")
            //    {
            //        OrderNumber = Convert.ToInt32(objAddOrder.AddPhoneOrder(objOrder, OrderNumber, Convert.ToInt32(ddlStore.SelectedValue.ToString())));
            //    }
            //    else
            //    {
            //        OrderNumber = Convert.ToInt32(hdnOrdernumber.Value.ToString());
            //    }
            //}
            //else
            //{
            OrderNumber = Convert.ToInt32(objAddOrder.AddPhoneOrder(objOrder, OrderNumber, Convert.ToInt32(ddlStore.SelectedValue.ToString())));
            // }
            if (OrderNumber > 0)
            {

                if (chkisregister.Checked == true && HdnCustID.Value != null && HdnCustID.Value != "" && hdnisregestered.Value != "" && hdnisregestered.Value.ToLower() == "false")
                {
                    CommonComponent.ExecuteCommonData("update tb_customer set IsRegistered=1,Active=1 where CustomerID = " + HdnCustID.Value);
                    SendMailCreateAccount();
                }
                else
                {
                    if (Request.QueryString["CustId"] == null)
                    {
                        if (chkisregister.Checked == false && HdnCustID.Value != null && HdnCustID.Value != "" && hdnisregestered.Value != "" && hdnisregestered.Value.ToLower() == "false")
                        {
                            CommonComponent.ExecuteCommonData("update tb_customer set IsRegistered=1,Active=1 where CustomerID = " + HdnCustID.Value);
                            SendMailCreateAccount();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(ddlcustomertype.SelectedItem.Text) && HdnCustID.Value != null && HdnCustID.Value != "")
                { CommonComponent.ExecuteCommonData("update tb_customer set Customertype='" + ddlcustomertype.SelectedItem.Text + "' where CustomerID = " + HdnCustID.Value); }


                if (!string.IsNullOrEmpty(ddlcallref.SelectedItem.Text))
                {
                    string strcallref = ddlcallref.SelectedItem.Text;
                    if (ddlcallref.SelectedItem.Text.ToLower() == "other")
                    { strcallref = txtrefother.Text; }

                    CommonComponent.ExecuteCommonData("update tb_order set Callreference='" + strcallref + "' where OrderNumber = " + OrderNumber);
                }

                if (GVShoppingCartItems.Rows.Count > 0)
                {
                    for (int i = 0; i < GVShoppingCartItems.Rows.Count; i++)
                    {
                        Label lblCustomerCartId = (Label)GVShoppingCartItems.Rows[i].FindControl("lblCustomerCartId");
                        Label lblProductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductID");
                        Label lblVariantNames = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantNames");
                        Label lblVariantValues = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantValues");

                        TextBox txtNotes = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtNotes");
                        if (!string.IsNullOrEmpty(lblCustomerCartId.Text.Trim()) && !string.IsNullOrEmpty(txtNotes.Text.Trim()))
                        {
                            string strnn = "";
                            if (lblVariantValues.Text.Length > 0)
                            {
                                strnn = lblVariantValues.Text.ToString().Substring(0, lblVariantValues.Text.ToString().Length - 1);
                            }
                            CommonComponent.ExecuteCommonData("Update tb_OrderedShoppingCartItems set Notes='" + txtNotes.Text.Trim().ToString().Replace("'", "''") + "' Where OrderedShoppingCartID in (SELECT ShoppingCardID FROM tb_Order WHERE Ordernumber=" + OrderNumber + ") AND RefProductID=" + lblProductID.Text.Trim() + " and VariantNames like '%" + lblVariantNames.Text.ToString() + "%' and VariantValues like '%" + strnn.ToString() + "%'");
                        }
                    }
                }

                if (rdoCreditCard.Checked)
                {
                    if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
                    {
                        string strResult = "OK";
                        if (Convert.ToDecimal(txtBoxcaptureamount.Text.ToString()) > Decimal.Zero)
                        {
                            strResult = OrderPayment(strPaymentgateWay, strPaymentgateWaystatus, OrderNumber, Convert.ToDecimal(txtBoxcaptureamount.Text.ToString()), GetCustomerDetailsForpayment(Convert.ToInt32(HdnCustID.Value.ToString())));
                        }
                        if (strResult.ToUpper() == "OK")
                        {
                            string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortal_Order_QuantityAdjust] " + OrderNumber + ""));

                            if (strPaymentgateWaystatus.ToLower().IndexOf("auth") > -1)
                            {
                                objAddOrder.InsertOrderlog(1, OrderNumber, "", Convert.ToInt32(Session["AdminID"].ToString()));
                            }
                            else if (strPaymentgateWaystatus.ToLower().IndexOf("capture") > -1 || strPaymentgateWaystatus.ToLower().IndexOf("sale") > -1)
                            {
                                objAddOrder.InsertOrderlog(2, OrderNumber, "", Convert.ToInt32(Session["AdminID"].ToString()));
                            }
                            objAddOrder.InsertOrderlog(9, OrderNumber, "", Convert.ToInt32(Session["AdminID"].ToString()));
                            CommonComponent.ExecuteCommonData("update tb_Order set IsPhoneOrder=1, SalesAgentID=" + Convert.ToInt32(Session["AdminID"].ToString()) + " where OrderNumber=" + OrderNumber.ToString());
                            OrderComponent objOrderInventory = new OrderComponent();
                            objOrderInventory.UpdateInventoryByOrderNumber(OrderNumber, -1);

                            int totalItemCount = GVShoppingCartItems.Rows.Count;
                            for (int i = 0; i < totalItemCount; i++)
                            {

                                Label lblProductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductID");
                                Label lblPrice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblPrice");
                                Label lblDiscountprice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblOrginalDiscountPrice");
                                Label lblQty = (Label)GVShoppingCartItems.Rows[i].FindControl("lblQty");

                                Label lblIndiSubTotal = (Label)GVShoppingCartItems.Rows[i].FindControl("lblIndiSubTotal");
                                Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + lblProductID.Text.ToString() + " and ItemType='Swatch'"));
                                if (Isorderswatch == 1)
                                {
                                    //String strpp = lblPrice.Text.ToString().Replace("$", "");
                                    //String strdiscount = lblDiscountprice.Text.ToString().Replace("$", "");
                                    try
                                    {


                                        Decimal strpp = Convert.ToDecimal(lblPrice.Text.ToString().Replace("$", "").Trim());
                                        Decimal strdiscount = Convert.ToDecimal(lblDiscountprice.Text.ToString().Replace("$", "").Trim());
                                        Decimal decSubTotal = Convert.ToDecimal(lblIndiSubTotal.Text.ToString().Replace("$", "").Trim());
                                        if (strpp > Decimal.Zero)
                                        {
                                            if (strpp == decSubTotal)
                                            {
                                                strpp = strpp / Convert.ToDecimal(lblQty.Text.ToString());
                                            }
                                        }
                                        if (strdiscount > Decimal.Zero)
                                        {
                                            if (strdiscount == decSubTotal)
                                            {
                                                strdiscount = strdiscount / Convert.ToDecimal(lblQty.Text.ToString());
                                            }
                                        }

                                        CommonComponent.ExecuteCommonData("UPDATE tb_OrderedShoppingCartItems SET Price='" + string.Format("{0:0.00}", strpp) + "',DiscountPrice='" + string.Format("{0:0.00}", strdiscount) + "' WHERE RefProductid=" + lblProductID.Text.ToString() + " AND OrderedShoppingCartID in (SELECT ShoppingCardID FROM tb_order WHERE OrderNumber=" + OrderNumber + ")");
                                    }
                                    catch { }
                                }
                            }


                            if (Request.QueryString["Ono"] != null)
                            {

                                CommonComponent.ExecuteCommonData("update tB_order set IsPhoneOrder =0 ,Deleted =1 where ordernumber =" + Request.QueryString["Ono"].ToString());

                            }


                            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                            {
                                if (rdoCreditCard.Checked)
                                {

                                }
                                else
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "jConfirmok", "confirmmsg(" + OrderNumber + ");", true);
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "jConfirmok", "confirmmsg(" + OrderNumber + ");", true);
                            }
                            string StrVendor1 = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortal_Order_QuantityAdjust] " + OrderNumber + ""));



                            Response.Redirect("/Admin/Orders/OrderList.aspx");


                        }
                        if ((Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null && Session["CustCouponCode"].ToString() != "" && Session["CustCouponCode"].ToString().ToLower().Trim() == "pricematch"))
                        {


                        }
                        else
                        {
                            Session["CustCouponCode"] = null;
                            Session["CustCouponCodeDiscount"] = null;
                        }

                        Session["Storecreaditamount"] = null;
                    }
                }
                else
                {
                    OrderComponent objOrderComp = new OrderComponent();
                    tb_Order objtb_Order = new tb_Order();
                    objtb_Order = objOrderComp.GetOrderByOrderNumber(OrderNumber);
                    objtb_Order.Referrer = txtCheque.Text.Trim();
                    objOrderComp.UpdateOrder(objtb_Order);
                    CommonComponent.ExecuteCommonData("update tb_Order set IsPhoneOrder=1, SalesAgentID=" + Convert.ToInt32(Session["AdminID"].ToString()) + " where OrderNumber=" + OrderNumber.ToString());
                    OrderComponent objOrderInventory = new OrderComponent();
                    objOrderInventory.UpdateInventoryByOrderNumber(OrderNumber, -1);
                    int totalItemCount = GVShoppingCartItems.Rows.Count;
                    for (int i = 0; i < totalItemCount; i++)
                    {

                        Label lblProductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductID");
                        Label lblPrice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblPrice");
                        Label lblDiscountprice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblOrginalDiscountPrice");
                        Label lblQty = (Label)GVShoppingCartItems.Rows[i].FindControl("lblQty");

                        Label lblIndiSubTotal = (Label)GVShoppingCartItems.Rows[i].FindControl("lblIndiSubTotal");
                        Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + lblProductID.Text.ToString() + " and ItemType='Swatch'"));
                        if (Isorderswatch == 1)
                        {
                            //String strpp = lblPrice.Text.ToString().Replace("$", "");
                            //String strdiscount = lblDiscountprice.Text.ToString().Replace("$", "");
                            try
                            {


                                Decimal strpp = Convert.ToDecimal(lblPrice.Text.ToString().Replace("$", "").Trim());
                                Decimal strdiscount = Convert.ToDecimal(lblDiscountprice.Text.ToString().Replace("$", "").Trim());
                                Decimal decSubTotal = Convert.ToDecimal(lblIndiSubTotal.Text.ToString().Replace("$", "").Trim());
                                if (strpp > Decimal.Zero)
                                {
                                    if (strpp == decSubTotal)
                                    {
                                        strpp = strpp / Convert.ToDecimal(lblQty.Text.ToString());
                                    }
                                }
                                if (strdiscount > Decimal.Zero)
                                {
                                    if (strdiscount == decSubTotal)
                                    {
                                        strdiscount = strdiscount / Convert.ToDecimal(lblQty.Text.ToString());
                                    }
                                }

                                CommonComponent.ExecuteCommonData("UPDATE tb_OrderedShoppingCartItems SET Price='" + string.Format("{0:0.00}", strpp) + "',DiscountPrice='" + string.Format("{0:0.00}", strdiscount) + "' WHERE RefProductid=" + lblProductID.Text.ToString() + " AND OrderedShoppingCartID in (SELECT ShoppingCardID FROM tb_order WHERE OrderNumber=" + OrderNumber + ")");
                            }
                            catch { }
                        }
                    }

                    SendMail(OrderNumber);
                    Session["CustCouponCode"] = null;
                    Session["CustCouponCodeDiscount"] = null;
                    Session["Storecreaditamount"] = null;
                    if (Request.QueryString["Ono"] != null)
                    {

                        CommonComponent.ExecuteCommonData("update tb_order set IsPhoneOrder =0 ,Deleted =1 where ordernumber =" + Request.QueryString["Ono"].ToString());

                    }

                    if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                    {
                        if (rdoCreditCard.Checked)
                        {

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "jConfirmok", "confirmmsg(" + OrderNumber + ");", true);
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "jConfirmok", "confirmmsg(" + OrderNumber + ");", true);
                    }
                    //Response.Redirect("/Admin/Orders/OrderList.aspx");
                }
            }
        }

        /// <summary>
        /// Order Transaction
        /// </summary>
        /// <param name="PayementGateWay">String PayementGateWay</param>
        /// <param name="OrderNumber">String OrderNumber</param>
        /// <returns>Returns the result as a String according to execution</returns>
        private string OrderPayment(string PayementGateWay, string PayementGateWaystatus, Int32 OrderNumber, decimal orderTotal, tb_Order objorder)
        {
            LinkpointComponent objLinkpoint = new LinkpointComponent();
            tb_Order objOrder = new tb_Order();
            String AVSResult = String.Empty;
            String AuthorizationResult = String.Empty;
            String AuthorizationCode = String.Empty;
            String AuthorizationTransID = String.Empty;
            String TransactionCommand = String.Empty;
            String TransactionResponse = String.Empty;
            string Status = "";
            OrderComponent objDsorder = new OrderComponent();
            DataSet dsOrder = new DataSet();
            dsOrder = objDsorder.GetOrderDetailsByOrderID(OrderNumber);
            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            {//charge logic


                if (orderTotal > decimal.Zero)
                {
                    if (PayementGateWay != null && PayementGateWay.ToString().ToLower().Trim() == "linkpoint")
                    {
                        Status = "OK";
                        //Status = objLinkpoint.ProcessCard(OrderNumber, Session["PaymentGatewayStatus"].ToString().ToLower(), "", "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                    else if (PayementGateWay != null && PayementGateWay.ToString().ToLower().Trim() == "paypal")
                    {
                        PayPalComponent objPaypal = new PayPalComponent();
                        Status = objPaypal.ProcessCardForClientSide(OrderNumber, Convert.ToInt32(HdnCustID.Value.ToString()), orderTotal, AppLogic.AppConfigBool("UseLiveTransactions"), PayementGateWaystatus.ToString(), objorder, "", objorder, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                    else if (PayementGateWay != null && PayementGateWay.ToString().ToLower().Trim() == "paypal")
                    {
                        PayPalComponent objPaypal = new PayPalComponent();
                        Status = objPaypal.ProcessCardForClientSide(OrderNumber, Convert.ToInt32(HdnCustID.Value.ToString()), orderTotal, AppLogic.AppConfigBool("UseLiveTransactions"), PayementGateWaystatus.ToString(), objorder, "", objorder, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                    else if (PayementGateWay != null && PayementGateWay.ToString().ToLower().Trim() == "authorizenet")
                    {
                        // Status = AuthorizeNetComponent.ProcessCardForAdminSide(OrderNumber, Convert.ToInt32(HdnCustID.Value.ToString()), orderTotal, AppLogic.AppConfigBool("UseLiveTransactions"), PayementGateWaystatus.ToString(), objorder, objorder, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                        // Status = "OK";
                        Status = GetPyamentId(OrderNumber);
                    }
                }
                else
                {
                    Status = "OK";
                }

                objOrder = new tb_Order();
                if (Status.ToUpper() == "OK")
                {

                    if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && !Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                    {
                        objOrder.OrderNumber = OrderNumber;
                        objOrder.CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());
                        //objOrder.AVSResult = AVSResult;
                        //objOrder.AuthorizationResult = AuthorizationResult;
                        //objOrder.AuthorizationCode = AuthorizationCode;
                        //objOrder.AuthorizationPNREF = AuthorizationTransID;
                        //objOrder.TransactionCommand = TransactionCommand;
                        objOrder.TransactionCommand = "";
                        if (PayementGateWaystatus != null && PayementGateWaystatus.ToString().ToLower().IndexOf("auth") > -1)
                        {
                            //objOrder.TransactionStatus = "AUTHORIZED";
                            //objOrder.AuthorizedOn = DateTime.Now;
                            if (Convert.ToDecimal(hfTotal.Value) == 0)
                            {
                                objOrder.TransactionStatus = "CAPTURED";
                                objOrder.CapturedOn = DateTime.Now;
                            }
                            else
                            {
                                objOrder.TransactionStatus = "AUTHORIZED";
                                objOrder.AuthorizedOn = DateTime.Now;
                            }
                        }
                        else
                        {
                            objOrder.TransactionStatus = "CAPTURED";
                            objOrder.CapturedOn = DateTime.Now;
                        }
                    }
                    OrderComponent objUpdateOrder = new OrderComponent();
                    Int32 updateOrder = Convert.ToInt32(objUpdateOrder.AddPhoneOrder(objOrder, OrderNumber, Convert.ToInt32(ddlStore.SelectedValue.ToString())));
                    Session["ONo"] = OrderNumber.ToString();
                    CommonComponent.ExecuteCommonData("UPDATE tb_Order SET AuthorizedAmount='" + orderTotal.ToString() + "', TransactionStatus='CAPTURED' WHERE  OrderNumber=" + OrderNumber.ToString() + "");
                    if (rdoCreditCard.Checked)
                    {
                        // GetPyamentId(OrderNumber);
                        Session["Isphone"] = "1";
                    }

                    SendMail(OrderNumber);
                }
                else
                {

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('" + lblchargelogicError.Text + "','Error!');", true);
                    try
                    {
                        //Failed Transaction
                        objDsorder = new OrderComponent();
                        tb_FailedTransaction objFailed = new tb_FailedTransaction();
                        objFailed.OrderNumber = Convert.ToInt32(OrderNumber);
                        objFailed.CustomerID = Convert.ToInt32(Convert.ToInt32(Request.QueryString["CustID"].ToString()));
                        objFailed.PaymentGateway = PayementGateWay.ToString();
                        if (rdoCheque.Checked)
                        {
                            objFailed.Paymentmethod = "OTHER";
                        }
                        else if (rdoCreditCard.Checked)
                        {
                            objFailed.Paymentmethod = "CREDITCARD";
                        }
                        else
                        {
                            objFailed.Paymentmethod = "";
                        }

                        //objFailed.Paymentmethod = Convert.ToString(Session["PaymentMethod"].ToString());
                        //  objFailed.TransactionCommand = Convert.ToString(lblchargelogicError.Text);
                        objFailed.TransactionCommand = "";
                        objFailed.TransactionResult = Convert.ToString(lblchargelogicError.Text);
                        objFailed.OrderDate = DateTime.Now;
                        objFailed.IPAddress = Request.UserHostAddress.ToString();
                        Int32 FaileId = Convert.ToInt32(objDsorder.AddOrderFailedTransaction(objFailed, Convert.ToInt32(AppConfig.StoreID)));
                        CommonComponent.ExecuteCommonData("Update tb_Order set CardVarificationCode='',CardNumber='' where OrderNumber=" + OrderNumber + "");
                    }
                    catch { }



                }



            }

            else
            {
                if (orderTotal > decimal.Zero)
                {
                    if (PayementGateWay != null && PayementGateWay.ToString().ToLower().Trim() == "linkpoint")
                    {
                        Status = "OK";
                        //Status = objLinkpoint.ProcessCard(OrderNumber, Session["PaymentGatewayStatus"].ToString().ToLower(), "", "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                    else if (PayementGateWay != null && PayementGateWay.ToString().ToLower().Trim() == "paypal")
                    {
                        PayPalComponent objPaypal = new PayPalComponent();
                        Status = objPaypal.ProcessCardForClientSide(OrderNumber, Convert.ToInt32(HdnCustID.Value.ToString()), orderTotal, AppLogic.AppConfigBool("UseLiveTransactions"), PayementGateWaystatus.ToString(), objorder, "", objorder, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                    else if (PayementGateWay != null && PayementGateWay.ToString().ToLower().Trim() == "paypal")
                    {
                        PayPalComponent objPaypal = new PayPalComponent();
                        Status = objPaypal.ProcessCardForClientSide(OrderNumber, Convert.ToInt32(HdnCustID.Value.ToString()), orderTotal, AppLogic.AppConfigBool("UseLiveTransactions"), PayementGateWaystatus.ToString(), objorder, "", objorder, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                    else if (PayementGateWay != null && PayementGateWay.ToString().ToLower().Trim() == "authorizenet")
                    {
                        Status = AuthorizeNetComponent.ProcessCardForAdminSide(OrderNumber, Convert.ToInt32(HdnCustID.Value.ToString()), orderTotal, AppLogic.AppConfigBool("UseLiveTransactions"), PayementGateWaystatus.ToString(), objorder, objorder, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                }
                else
                {
                    Status = "OK";
                }

                objOrder = new tb_Order();
                if (Status.ToUpper() == "OK")
                {
                    objOrder.OrderNumber = OrderNumber;
                    objOrder.CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());
                    objOrder.AVSResult = AVSResult;
                    objOrder.AuthorizationResult = AuthorizationResult;
                    objOrder.AuthorizationCode = AuthorizationCode;
                    objOrder.AuthorizationPNREF = AuthorizationTransID;
                    //objOrder.TransactionCommand = TransactionCommand;
                    objOrder.TransactionCommand = "";
                    if (PayementGateWaystatus != null && PayementGateWaystatus.ToString().ToLower().IndexOf("auth") > -1)
                    {
                        //objOrder.TransactionStatus = "AUTHORIZED";
                        //objOrder.AuthorizedOn = DateTime.Now;
                        if (Convert.ToDecimal(hfTotal.Value) == 0)
                        {
                            objOrder.TransactionStatus = "CAPTURED";
                            objOrder.CapturedOn = DateTime.Now;
                        }
                        else
                        {
                            objOrder.TransactionStatus = "AUTHORIZED";
                            objOrder.AuthorizedOn = DateTime.Now;
                        }
                    }
                    else
                    {
                        objOrder.TransactionStatus = "CAPTURED";
                        objOrder.CapturedOn = DateTime.Now;
                    }
                    OrderComponent objUpdateOrder = new OrderComponent();
                    Int32 updateOrder = Convert.ToInt32(objUpdateOrder.AddPhoneOrder(objOrder, OrderNumber, Convert.ToInt32(ddlStore.SelectedValue.ToString())));
                    CommonComponent.ExecuteCommonData("UPDATE tb_Order SET AuthorizedAmount='" + orderTotal.ToString() + "' WHERE  OrderNumber=" + OrderNumber.ToString() + "");
                    SendMail(OrderNumber);
                }
                else
                {

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('" + Status + "','Error!');", true);
                    try
                    {
                        //Failed Transaction
                        objDsorder = new OrderComponent();
                        tb_FailedTransaction objFailed = new tb_FailedTransaction();
                        objFailed.OrderNumber = Convert.ToInt32(OrderNumber);
                        objFailed.CustomerID = Convert.ToInt32(Convert.ToInt32(Request.QueryString["CustID"].ToString()));
                        objFailed.PaymentGateway = PayementGateWay.ToString();
                        if (rdoCheque.Checked)
                        {
                            objFailed.Paymentmethod = "OTHER";
                        }
                        else if (rdoCreditCard.Checked)
                        {
                            objFailed.Paymentmethod = "CREDITCARD";
                        }
                        else
                        {
                            objFailed.Paymentmethod = "";
                        }

                        //objFailed.Paymentmethod = Convert.ToString(Session["PaymentMethod"].ToString());
                        objFailed.TransactionCommand = Convert.ToString(TransactionCommand);
                        objFailed.TransactionCommand = "";
                        objFailed.TransactionResult = Convert.ToString(TransactionResponse);
                        objFailed.OrderDate = DateTime.Now;
                        objFailed.IPAddress = Request.UserHostAddress.ToString();
                        Int32 FaileId = Convert.ToInt32(objDsorder.AddOrderFailedTransaction(objFailed, Convert.ToInt32(AppConfig.StoreID)));
                        CommonComponent.ExecuteCommonData("Update tb_Order set CardVarificationCode='',CardNumber='' where OrderNumber=" + OrderNumber + "");
                    }
                    catch { }



                }
            }

            return Status;
        }

        /// <summary>
        /// Order Receipt Send To Customer & Admin By Mail
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNUmber</param>
        public void SendMail(Int32 OrderNumber)
        {
            string ToID = "";
            ToID = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(EmailID,'') FROM tb_ContactEmail WHERE Subject='Order Confirmation'"));
            if (string.IsNullOrEmpty(ToID))
            {
                ToID = AppLogic.AppConfigs("MailMe_ToAddress");
            }

            string Body = "";
            string url = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + "Admin/Orders/invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
            WebRequest NewWebReq = WebRequest.Create(url);
            WebResponse newWebRes = NewWebReq.GetResponse();
            string format = newWebRes.ContentType;
            Stream ftprespstrm = newWebRes.GetResponseStream();
            StreamReader reader;
            reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
            Body = reader.ReadToEnd().ToString();
            Body = Body.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"display:none;\"");
            AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

            try
            {
                string Emailforshippingmethod = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 ShippingMethod FROM tb_order WHERE OrderNumber=" + OrderNumber + " and ShippingMethod <> 'Ground'"));
                if (!string.IsNullOrEmpty(Emailforshippingmethod))
                {
                    CommonOperations.SendMail(ToID, "New Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                }
            }
            catch { }
            try
            {

                CommonOperations.SendMail(TxtEmail.Text.ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);

            }
            catch { }

        }

        /// <summary>
        /// Add New Phone Order
        /// </summary>
        private void AddIncompleteOrder()
        {




            tb_Order objOrder = new tb_Order();
            objOrder = GetCustomerDetailsForAddOrder();
            objOrder.CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());

            Decimal Storecreaditamount = 0;
            decimal.TryParse(txtstorecreaditAmount.Text, out Storecreaditamount);

            if (rdoCreditCard.Checked)
            {
                objOrder.CardType = ddlCardType.SelectedItem.Text.ToString();
                objOrder.CardVarificationCode = TxtCardVerificationCode.Text.ToString();
                objOrder.CardName = TxtNameOnCard.Text.Trim().ToString();
                objOrder.CardExpirationMonth = ddlMonth.SelectedValue.ToString();
                objOrder.CardExpirationYear = ddlYear.SelectedValue.ToString();
                if (TxtCardNumber.Text.ToString().IndexOf("*") > -1)
                {
                    if (Session["CardNumber"] != null)
                    {
                        objOrder.CardNumber = SecurityComponent.Encrypt(Session["CardNumber"].ToString());
                        try
                        {
                            objOrder.Last4 = Session["CardNumber"].ToString().Substring(Session["CardNumber"].ToString().Length - 4);
                        }
                        catch { }
                    }
                }
                else
                {
                    objOrder.CardNumber = SecurityComponent.Encrypt(TxtCardNumber.Text.ToString());
                    try
                    {
                        if (TxtCardNumber.Text.ToString().Length > 4)
                        {
                            objOrder.Last4 = TxtCardNumber.Text.ToString().Substring(TxtCardNumber.Text.ToString().Length - 4);
                        }
                    }
                    catch { }
                }
            }
            if (ddlShippingMethod.SelectedValue.ToString() != "")
            {
                int Index = ddlShippingMethod.SelectedItem.Text.IndexOf("($");
                if (Index > -1)
                {
                    objOrder.ShippingMethod = ddlShippingMethod.SelectedItem.Text.ToString().Substring(0, Index);
                }
            }


            //if (Session["CustCouponCodeDiscount"] != null)
            //{
            //    objOrder.CouponDiscountAmount = Convert.ToDecimal(Session["CustCouponCodeDiscount"].ToString());
            //}
            //else
            //{
            objOrder.CouponDiscountAmount = decimal.Zero;
            //}

            objOrder.QuantityDiscountAmount = decimal.Zero;

            //if(Session[""])
            //if(Session[""])
            if (Session["CustCouponCode"] != null)
            {
                objOrder.CouponCode = Session["CustCouponCode"].ToString();
            }
            else
            {
                objOrder.CouponCode = "";
            }


            objOrder.OrderNotes = TxtNotes.Text.ToString();



            if (Convert.ToDecimal(hfSubTotal.Value.ToString()) > Decimal.Zero || Convert.ToDecimal(hfSubTotal.Value.ToString()) < Decimal.Zero)
            {
                objOrder.OrderSubtotal = Convert.ToDecimal(hfSubTotal.Value.ToString());
            }
            else
            {
                objOrder.OrderSubtotal = Convert.ToDecimal(lblSubTotal.Text.ToString());

            }
            if (Convert.ToDecimal(hfTotal.Value.ToString()) > Decimal.Zero || Convert.ToDecimal(hfTotal.Value.ToString()) < Decimal.Zero)
            {
                objOrder.OrderTotal = Convert.ToDecimal(hfTotal.Value.ToString());
            }
            else
            {
                objOrder.OrderTotal = Convert.ToDecimal(lblTotal.Text.ToString());
            }
            objOrder.OrderShippingCosts = Convert.ToDecimal(TxtShippingCost.Text.ToString());
            objOrder.OrderTax = Convert.ToDecimal(TxtTax.Text.ToString());

            OrderComponent objPayment = new OrderComponent();
            DataSet dsPayment = new DataSet();
            dsPayment = objPayment.GetPaymentGateway("CREDITCARD", Convert.ToInt32(AppLogic.AppConfigs("StoreId").ToString()));
            string strPaymentgateWay = "";
            string strPaymentgateWaystatus = "";
            if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
            {
                strPaymentgateWay = Convert.ToString(dsPayment.Tables[0].Rows[0]["PaymentService"].ToString().ToUpper());
                strPaymentgateWaystatus = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString().ToUpper());
            }

            if (rdoCreditCard.Checked)
            {
                objOrder.Deleted = true;
                objOrder.PaymentMethod = "CREDITCARD";
                objOrder.PaymentGateway = strPaymentgateWay.ToString().ToUpper();
            }
            else
            {
                objOrder.Deleted = false;
                objOrder.PaymentMethod = "OTHER";
                objOrder.PaymentGateway = "OTHER";
                objOrder.TransactionStatus = "CAPTURED";
                objOrder.CapturedOn = DateTime.Now;
            }



            if (!string.IsNullOrEmpty(txtstorecreaditno.Text) && !string.IsNullOrEmpty(Storecreaditamount.ToString()) && Storecreaditamount > 0)
            {

                objOrder.Storecreaditno = txtstorecreaditno.Text;
                objOrder.StorecreaditAmont = Convert.ToDecimal(txtstorecreaditAmount.Text);
                objOrder.CustomDiscount = Convert.ToDecimal(TxtDiscount.Text.ToString());
            }
            else
            {

                objOrder.CustomDiscount = Convert.ToDecimal(TxtDiscount.Text.ToString());
            }
            objOrder.IsNew = true;
            objOrder.LastIPAddress = Request.UserHostAddress.ToString();
            OrderComponent objAddOrder = new OrderComponent();
            Int32 OrderNumber = 0;
            OrderNumber = Convert.ToInt32(objAddOrder.AddPhoneOrder(objOrder, OrderNumber, Convert.ToInt32(ddlStore.SelectedValue.ToString())));
            if (OrderNumber > 0)
            {

                if (chkisregister.Checked == true && HdnCustID.Value != null && HdnCustID.Value != "" && hdnisregestered.Value != "" && hdnisregestered.Value.ToLower() == "false")
                {
                    CommonComponent.ExecuteCommonData("update tb_customer set IsRegistered=1,Active=1 where CustomerID = " + HdnCustID.Value);
                    SendMailCreateAccount();
                }
                else
                {
                    if (Request.QueryString["CustId"] == null)
                    {
                        if (chkisregister.Checked == false && HdnCustID.Value != null && HdnCustID.Value != "" && hdnisregestered.Value != "" && hdnisregestered.Value.ToLower() == "false")
                        {
                            CommonComponent.ExecuteCommonData("update tb_customer set IsRegistered=1,Active=1 where CustomerID = " + HdnCustID.Value);
                            SendMailCreateAccount();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ddlcustomertype.SelectedItem.Text) && HdnCustID.Value != null && HdnCustID.Value != "")
                { CommonComponent.ExecuteCommonData("update tb_customer set Customertype='" + ddlcustomertype.SelectedItem.Text + "' where CustomerID = " + HdnCustID.Value); }


                if (!string.IsNullOrEmpty(ddlcallref.SelectedItem.Text))
                {
                    string strcallref = ddlcallref.SelectedItem.Text;
                    if (ddlcallref.SelectedItem.Text.ToLower() == "other")
                    { strcallref = txtrefother.Text; }

                    CommonComponent.ExecuteCommonData("update tb_order set Callreference='" + strcallref + "' where OrderNumber = " + OrderNumber);
                }

                if (GVShoppingCartItems.Rows.Count > 0)
                {
                    for (int i = 0; i < GVShoppingCartItems.Rows.Count; i++)
                    {
                        Label lblCustomerCartId = (Label)GVShoppingCartItems.Rows[i].FindControl("lblCustomerCartId");
                        Label lblProductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductID");
                        Label lblVariantNames = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantNames");
                        Label lblVariantValues = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantValues");

                        TextBox txtNotes = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtNotes");
                        if (!string.IsNullOrEmpty(lblCustomerCartId.Text.Trim()) && !string.IsNullOrEmpty(txtNotes.Text.Trim()))
                        {
                            string strnn = "";
                            if (lblVariantValues.Text.Length > 0)
                            {
                                strnn = lblVariantValues.Text.ToString().Substring(0, lblVariantValues.Text.ToString().Length - 1);
                            }
                            CommonComponent.ExecuteCommonData("Update tb_OrderedShoppingCartItems set Notes='" + txtNotes.Text.Trim().ToString().Replace("'", "''") + "' Where OrderedShoppingCartID in (SELECT ShoppingCardID FROM tb_Order WHERE Ordernumber=" + OrderNumber + ") AND RefProductID=" + lblProductID.Text.Trim() + " and VariantNames like '%" + lblVariantNames.Text.ToString() + "%' and VariantValues like '%" + strnn.ToString() + "%'");
                        }
                    }
                }

                if (rdoCreditCard.Checked)
                {
                    //if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
                    //{
                    //  string strResult = OrderPayment(strPaymentgateWay, strPaymentgateWaystatus, OrderNumber, Convert.ToDecimal(lblTotal.Text.ToString()), GetCustomerDetailsForpayment(Convert.ToInt32(HdnCustID.Value.ToString())));
                    // if (strResult.ToUpper() == "OK")
                    //  {
                    //string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortal_Order_QuantityAdjust] " + OrderNumber + ""));

                    //if (strPaymentgateWaystatus.ToLower().IndexOf("auth") > -1)
                    //{
                    //    objAddOrder.InsertOrderlog(1, OrderNumber, "", Convert.ToInt32(Session["AdminID"].ToString()));
                    //}
                    //else if (strPaymentgateWaystatus.ToLower().IndexOf("capture") > -1 || strPaymentgateWaystatus.ToLower().IndexOf("sale") > -1)
                    //{
                    //    objAddOrder.InsertOrderlog(2, OrderNumber, "", Convert.ToInt32(Session["AdminID"].ToString()));
                    //}
                    //objAddOrder.InsertOrderlog(9, OrderNumber, "", Convert.ToInt32(Session["AdminID"].ToString()));

                    if (Request.QueryString["Ono"] != null)
                    {

                        CommonComponent.ExecuteCommonData("update tB_order set IsPhoneOrder=0,Deleted =1,isIncompleteOrder=0 where ordernumber=" + Request.QueryString["Ono"].ToString());
                        CommonComponent.ExecuteCommonData("update tB_order set IsPhoneOrder =1 ,Deleted =1,isIncompleteOrder=1 where ordernumber =" + OrderNumber);

                    }
                    else
                    {

                        CommonComponent.ExecuteCommonData("update tB_order set IsPhoneOrder =1 ,Deleted =1,isIncompleteOrder=1 where ordernumber =" + OrderNumber);
                    }
                    CommonComponent.ExecuteCommonData("update tb_Order set SalesAgentID=" + Convert.ToInt32(Session["AdminID"].ToString()) + " where OrderNumber=" + OrderNumber.ToString());
                    OrderComponent objOrderInventory = new OrderComponent();
                    //objOrderInventory.UpdateInventoryByOrderNumber(OrderNumber, -1);


                    //  }
                    Session["CustCouponCode"] = null;
                    Session["CustCouponCodeDiscount"] = null;
                    Session["Storecreaditamount"] = null;

                    if (Request.QueryString["Ono"] != null)
                    {
                        Response.Redirect("/Admin/Orders/OrderIncompleteList.aspx");
                        // Response.Redirect("/Admin/Orders/OrderList.aspx");
                    }
                    else { Response.Redirect("/Admin/Orders/OrderIncompleteList.aspx"); }
                    //}
                }
                else
                {
                    OrderComponent objOrderComp = new OrderComponent();
                    tb_Order objtb_Order = new tb_Order();
                    objtb_Order = objOrderComp.GetOrderByOrderNumber(OrderNumber);
                    objtb_Order.Referrer = txtCheque.Text.Trim();
                    objOrderComp.UpdateOrder(objtb_Order);

                    if (Request.QueryString["Ono"] != null)
                    {

                        CommonComponent.ExecuteCommonData("update tB_order set IsPhoneOrder =0 ,Deleted =1,isIncompleteOrder=0 where ordernumber =" + Request.QueryString["Ono"].ToString());
                        CommonComponent.ExecuteCommonData("update tB_order set IsPhoneOrder =1 ,Deleted =1,isIncompleteOrder=1 where ordernumber =" + OrderNumber);
                    }
                    else
                    {

                        CommonComponent.ExecuteCommonData("update tB_order set IsPhoneOrder =1 ,Deleted =1,isIncompleteOrder=1 where ordernumber =" + OrderNumber);
                    }
                    CommonComponent.ExecuteCommonData("update tb_Order set SalesAgentID=" + Convert.ToInt32(Session["AdminID"].ToString()) + " where OrderNumber=" + OrderNumber.ToString());
                    OrderComponent objOrderInventory = new OrderComponent();
                    // objOrderInventory.UpdateInventoryByOrderNumber(OrderNumber, -1);
                    //  SendMail(OrderNumber);
                    Session["CustCouponCode"] = null;
                    Session["CustCouponCodeDiscount"] = null;
                    Session["Storecreaditamount"] = null;

                    if (Request.QueryString["Ono"] != null)
                    {
                        Response.Redirect("/Admin/Orders/OrderIncompleteList.aspx");
                    }
                    else { Response.Redirect("/Admin/Orders/OrderIncompleteList.aspx"); }

                }
            }
        }

        protected void btnincompleteorder_Click(object sender, EventArgs e)
        {
            //CountryComponent objCountry = new CountryComponent();
            //string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(hdncountry.Value.ToString()));
            //if (CountryCode.ToString().ToLower() != "us" && CountryCode.ToString().ToLower() != "au" && CountryCode.ToString().ToLower() != "ca" && CountryCode.ToString().ToLower() != "gb" && CountryCode.ToString().Trim().ToUpper() != "CANADA" && CountryCode.ToString().Trim().ToUpper() != "AUSTRALIA" && CountryCode.ToString().Trim().ToUpper() != "UNITED KINGDOM" && CountryCode.ToString().Trim().ToUpper() != "UNITED STATES")
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please order place by Bongo international setup.','Message');", true);
            //    return;
            //}

            if (rdoCreditCard.Checked)
            {
                //if (TxtNameOnCard.Text.ToString().Trim() == "")
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Name of Card.','Required Information','ContentPlaceHolder1_TxtNameOnCard');", true);
                //    TxtNameOnCard.Focus();
                //    return;
                //}
                //else if (ddlCardType.SelectedIndex == 0)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Card Type.','Required Information','ContentPlaceHolder1_ddlCardType');", true);
                //    ddlCardType.Focus();
                //    return;
                //}
                //else if (TxtCardNumber.Text.ToString().Trim() == "")
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Card Number.','Required Information','ContentPlaceHolder1_TxtCardNumber');", true);
                //    TxtCardNumber.Focus();
                //    return;
                //}
                //else if (TxtCardVerificationCode.Text.ToString().Trim() == "")
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Card Verification Code.','Required Information','ContentPlaceHolder1_TxtCardVerificationCode');", true);
                //    TxtCardVerificationCode.Focus();
                //    return;
                //}
                //if (TxtCardNumber.Text.ToString().Trim() != "" && TxtCardNumber.Text.ToString().Trim().IndexOf("*") < -1)
                //{
                //    bool chkflg = false;
                //    long crdNumber = 0;
                //    chkflg = long.TryParse(TxtCardNumber.Text.ToString(), out crdNumber);
                //    if (!chkflg)
                //    {
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter valid Numeric Card Number','Required Information','ContentPlaceHolder1_TxtCardNumber');", true);
                //        TxtCardNumber.Focus();
                //        return;
                //    }
                //}
                //if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && TxtCardNumber.Text.ToString().Trim() != "" && TxtCardNumber.Text.ToString().Trim().Length < 16)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Credit Card Number must be 16 digit long.','Required Information','ContentPlaceHolder1_TxtCardNumber');", true);
                //    TxtCardNumber.Focus();
                //    return;
                //}
                //if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && TxtCardNumber.Text.ToString().Trim() != "" && (TxtCardNumber.Text.ToString().Trim().Length < 15 || TxtCardNumber.Text.ToString().Trim().Length > 15))
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Credit Card Number must be 15 digit long.','Required Information','ContentPlaceHolder1_TxtCardNumber');", true);
                //    TxtCardNumber.Focus();
                //    return;
                //}

                //if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && TxtCardVerificationCode.Text.ToString().Trim() != "" && (TxtCardVerificationCode.Text.ToString().Trim().Length < 3 || TxtCardVerificationCode.Text.ToString().Trim().Length > 3))
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Card Verification Code must be 3 digit long.','Required Information','ContentPlaceHolder1_TxtCardVerificationCode');", true);
                //    TxtCardVerificationCode.Focus();
                //    return;
                //}
                //if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && TxtCardVerificationCode.Text.ToString().Trim() != "" && TxtCardVerificationCode.Text.ToString().Trim().Length < 4)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Card Verification Code must be 4 digit long.','Required Information','ContentPlaceHolder1_TxtCardVerificationCode');", true);
                //    TxtCardVerificationCode.Focus();
                //    return;
                //}
                //if (ddlMonth.SelectedIndex == 0)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Month.','Required Information','ContentPlaceHolder1_ddlMonth');", true);
                //    ddlMonth.Focus();
                //    return;
                //}
                //else if (ddlYear.SelectedIndex == 0)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Year.','Required Information','ContentPlaceHolder1_ddlYear');", true);
                //    ddlYear.Focus();
                //    return;
                //}
                //else if ((Convert.ToInt32(ddlYear.SelectedValue) > DateTime.Now.Year) || (Convert.ToInt32(ddlYear.SelectedValue) == DateTime.Now.Year && Convert.ToInt32(ddlMonth.SelectedValue) >= DateTime.Now.Month))
                //{

                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Valid Expiration Date.','Required Information','ContentPlaceHolder1_ddlYear');", true);
                //    return;
                //}

                if (GVShoppingCartItems.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Your Shopping Cart is Empty.','Sorry!','ContentPlaceHolder1_aRelated');", true);
                    return;
                }
                //if (ddlShippingMethod.SelectedValue.ToString().ToLower().Contains("freight") && Convert.ToDecimal(TxtShippingCost.Text) <= 0)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Shipping Cost for " + ddlShippingMethod.SelectedValue.ToString() + " Method.','Required Information','ContentPlaceHolder1_TxtShippingCost');", true);
                //    return;
                //}
                try
                {
                    if (hdnIsSalesManager.Value != null && hdnIsSalesManager.Value == "1")
                    {
                        if (!string.IsNullOrEmpty(TxtDiscount.Text.ToString()) && Convert.ToDecimal(TxtDiscount.Text.ToString()) > 0)
                        {
                            decimal SubTot = Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("%", ""));
                            decimal SubTotDispercent = (SubTot * 10) / 100;
                            decimal Discount = Convert.ToDecimal(TxtDiscount.Text.ToString());
                            if (Discount > SubTotDispercent)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Discount Should not be apply grater than 10% of SubTotal.','Required Information','ContentPlaceHolder1_TxtDiscount');", true);
                                TxtDiscount.Text = "0";
                                return;
                            }
                        }
                    }
                }
                catch { }
                try
                {
                    if (Request.QueryString["CustId"] == null)
                    {
                        if (!String.IsNullOrEmpty(TxtEmail.Text))
                        {
                            Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(customerid) from tb_Customer where Email='" + TxtEmail.Text.ToString().Trim().Replace("'", "''") + "' and storeid=" + ddlStore.SelectedValue.ToString() + " and isnull(Active,0)=1 and isnull(IsRegistered,0)=1 and isnull(Deleted,0)=0"));
                            if (Count > 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Email Address Already Exists, Please Enter different Email Address.','Required Information','ContentPlaceHolder1_TxtEmail');", true);
                                return;
                            }
                        }
                    }
                }
                catch { }
                AddIncompleteOrder();
            }
            else
            {
                //if (txtCheque.Text.ToString().Trim() == "")
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter other payment detail.','Required Information','ContentPlaceHolder1_txtCheque');", true);
                //    txtCheque.Focus();
                //    return;
                //}
                if (GVShoppingCartItems.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Your Shopping Cart is Empty.','Sorry!','ContentPlaceHolder1_aRelated');", true);
                    return;
                }
                //if (ddlShippingMethod.SelectedValue.ToString().ToLower().Contains("freight") && Convert.ToDecimal(TxtShippingCost.Text) <= 0)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Shipping Cost for " + ddlShippingMethod.SelectedValue.ToString() + " Method.','Required Information','ContentPlaceHolder1_TxtShippingCost');", true);
                //    return;
                //}
                try
                {
                    if (hdnIsSalesManager.Value != null && hdnIsSalesManager.Value == "1")
                    {
                        if (!string.IsNullOrEmpty(TxtDiscount.Text.ToString()) && Convert.ToDecimal(TxtDiscount.Text.ToString()) > 0)
                        {
                            decimal SubTot = Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("%", ""));
                            decimal SubTotDispercent = (SubTot * 10) / 100;
                            decimal Discount = Convert.ToDecimal(TxtDiscount.Text.ToString());
                            if (Discount > SubTotDispercent)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Discount Should not be apply grater than 10% of SubTotal.','Required Information','ContentPlaceHolder1_TxtDiscount');", true);
                                TxtDiscount.Text = "0";
                                return;
                            }
                        }


                    }
                }
                catch { }
                try
                {
                    if (Request.QueryString["CustId"] == null)
                    {
                        if (!String.IsNullOrEmpty(TxtEmail.Text))
                        {
                            Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(customerid) from tb_Customer where Email='" + TxtEmail.Text.ToString().Trim().Replace("'", "''") + "' and storeid=" + ddlStore.SelectedValue.ToString() + " and isnull(Active,0)=1 and isnull(IsRegistered,0)=1 and isnull(Deleted,0)=0"));
                            if (Count > 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Email Address Already Exists, Please Enter different Email Address.','Required Information','ContentPlaceHolder1_TxtEmail');", true);
                                return;
                            }
                        }
                    }
                }
                catch { }
                AddIncompleteOrder();
            }


        }
        protected void btndiscount_Click(object sender, EventArgs e)
        {
            decimal ShippingCost = 0;
            if (ddlShippingMethod.SelectedValue.ToString() != "")
            {
                int Index = ddlShippingMethod.SelectedItem.Text.IndexOf("($");
                int Length = ddlShippingMethod.SelectedItem.Text.LastIndexOf(")") - Index;
                if (Index != -1 && Length != 0)
                {
                    string strShippingCost = ddlShippingMethod.SelectedItem.Text.Substring(Index + 2, Length - 2).Trim();
                    Decimal.TryParse(strShippingCost, out ShippingCost);
                }
            }
            TxtShippingCost.Text = ShippingCost.ToString("f2");

            decimal subtotal = 0;
            decimal.TryParse(lblSubTotal.Text, out subtotal);

            decimal tax = 0;
            try
            {
                tax = Convert.ToDecimal(TxtTax.Text.Trim());
            }
            catch
            {

            }
            decimal ship = 0;
            try
            {
                ship = Convert.ToDecimal(TxtShippingCost.Text.Trim());
            }
            catch
            {

            }

            decimal discount = 0;
            try
            {
                discount = Convert.ToDecimal(TxtDiscount.Text.Trim());
            }
            catch
            {

            }

            try
            {
                if (hfSubTotal.Value.ToString() != "")
                {

                    if (Request.QueryString["CustID"] != null && !string.IsNullOrEmpty(Request.QueryString["CustID"].ToString()))
                    {
                        try
                        {
                            string filedoc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT DocFile FROM dbo.tb_Customer WHERE CustomerID ='" + Request.QueryString["CustID"].ToString() + "'"));
                            string CustDocPathTemp = AppLogic.AppConfigs("Customer.DocumentPath");

                            if (File.Exists(Server.MapPath(CustDocPathTemp + Request.QueryString["CustID"].ToString() + "/" + filedoc)))
                            {
                                TxtTax.Text = "0";
                            }
                            else
                            {
                                TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                            }
                        }
                        catch
                        {
                            TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                        }

                    }
                    else
                    {
                        TxtTax.Text = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Convert.ToDecimal(lblSubTotal.Text.ToString().Replace("$", ""))).ToString("f2");
                    }


                }
                else
                {
                    TxtTax.Text = "0";
                }
            }
            catch { }
            decimal FinalSubTotal = (subtotal + tax + ship) - discount;
            if (FinalSubTotal < 0)
            {
                FinalSubTotal = 0;
            }
            lblTotal.Text = FinalSubTotal.ToString("f2");
            hfSubTotal.Value = lblSubTotal.Text;
            hfTotal.Value = lblTotal.Text;
            BindCartInGrid();
        }
        /// <summary>
        /// Button to Generate Coupon Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            Session["CustCouponCode"] = null;
            Session["CustCouponCodeDiscount"] = null;
            Session["CustCouponvalid"] = null;
            string SPMessage = "";
            decimal CouponDiscount = 0;
            string couponcode = "";
            couponcode = txtCouponCode.Text.ToString().ToLower();
            if (!string.IsNullOrEmpty(txtCouponCode.Text.ToString()) && !string.IsNullOrEmpty(HdnCustID.Value.ToString()) && txtCouponCode.Text.ToString().ToLower().Trim() != "pricematch")
            {
                DataSet dsCoupon = new DataSet();
                dsCoupon = CommonComponent.GetCommonDataSet("Select 0 DiscountPercent , * from tb_Customer Where CustomerID=" + HdnCustID.Value.ToString() + " and CouponCode='" + txtCouponCode.Text.ToString().Replace("'", "''") + "'");
                if (dsCoupon != null && dsCoupon.Tables.Count > 0 && dsCoupon.Tables[0].Rows.Count > 0)
                {
                    decimal DisCoupon = 0;
                    decimal.TryParse(dsCoupon.Tables[0].Rows[0]["DiscountPercent"].ToString(), out DisCoupon);
                    string StrFromdate = Convert.ToString(dsCoupon.Tables[0].Rows[0]["FromDate"].ToString());
                    string StrTodate = Convert.ToString(dsCoupon.Tables[0].Rows[0]["ToDate"].ToString());

                    if (!string.IsNullOrEmpty(StrFromdate.Trim()) && !string.IsNullOrEmpty(StrTodate.Trim()))
                    {
                        DateTime FDate = new DateTime();
                        DateTime TDate = new DateTime();
                        DateTime Currdate = System.DateTime.Now;
                        try { FDate = Convert.ToDateTime(StrFromdate.Trim()); }
                        catch { }

                        try { TDate = Convert.ToDateTime(StrTodate.Trim()); }
                        catch { }

                        if (Convert.ToDateTime(FDate.ToString("MM/dd/yyyy")) <= Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy")) && Convert.ToDateTime(TDate.ToString("MM/dd/yyyy")) >= Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy")))
                        {
                            //if (DisCoupon > 0)
                            //{
                            Session["CustCouponCode"] = txtCouponCode.Text.ToString();
                            Session["CustCouponCodeDiscount"] = DisCoupon.ToString();
                            Session["CustCouponvalid"] = "1";
                            BindCartInGrid();
                            txtB_Zip_TextChanged(null, null);
                            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                            {//charge logic
                                if (rdoCreditCard.Checked)
                                {
                                    rdoCreditCard.Checked = true;
                                }
                            }
                            //}
                        }
                        else
                        {
                            BindCartInGrid();
                            txtB_Zip_TextChanged(null, null);
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgCouponcode", "jAlert('Sorry, Coupon code is expired!','Message');", true);
                            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                            {//charge logic
                                if (rdoCreditCard.Checked == true && ViewState["Hostepaymentid"] != null)
                                {
                                    // Page.ClientScript.RegisterStartupScript(btnGenerate.GetType(), "iframeload555", "document.getElementById('iframecreditcard').src = 'https://connect.chargelogic.com/?HostedPaymentID=" + ViewState["Hostepaymentid"].ToString() + "';", true);
                                }
                            }

                        }
                    }
                    else
                    {
                        Session["CustCouponCode"] = txtCouponCode.Text.ToString();
                        Session["CustCouponCodeDiscount"] = DisCoupon.ToString();
                        Session["CustCouponvalid"] = "1";
                        BindCartInGrid();
                        txtB_Zip_TextChanged(null, null);
                        if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                        {//charge logic
                            if (rdoCreditCard.Checked)
                            {
                                rdoCreditCard.Checked = true;
                            }
                        }
                    }
                }
                else
                {
                    //SPMessage = Convert.ToString(CouponComponent.GetDiscountByCouponCodeFunction(txtCouponCode.Text.Trim(), Convert.ToInt32(HdnCustID.Value.ToString()), Convert.ToInt32(ddlStore.SelectedValue.ToString())));
                    SPMessage = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.GetCouponDiscount_Phoneorder('" + txtCouponCode.Text.Trim() + "'," + HdnCustID.Value.ToString() + "," + ddlStore.SelectedValue.ToString() + ")"));


                    decimal.TryParse(SPMessage.ToString(), out CouponDiscount);
                    if (CouponDiscount > 0)
                    {

                        Session["CustCouponCode"] = txtCouponCode.Text;
                        Session["CustCouponCodeDiscount"] = CouponDiscount;
                        txtCouponCode.Text = "";
                        if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                        {//charge logic
                            if (rdoCreditCard.Checked)
                            {
                                rdoCreditCard.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcoupon", "jAlert('Please Enter Valid Coupon Code!', 'Message', 'ContentPlaceHolder1_txtCouponCode');", true);
                        if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                        {//charge logic
                            if (rdoCreditCard.Checked == true && ViewState["Hostepaymentid"] != null)
                            {
                                // Page.ClientScript.RegisterStartupScript(btnGenerate.GetType(), "iframeload555", "document.getElementById('iframecreditcard').src = 'https://connect.chargelogic.com/?HostedPaymentID=" + ViewState["Hostepaymentid"].ToString() + "';", true);
                            }
                        }
                    }


                    BindCartInGrid();
                    txtB_Zip_TextChanged(null, null);
                }

                if (!string.IsNullOrEmpty(couponcode) && couponcode.ToString().ToLower().Trim() == "pricematch")
                {
                    tddiscount.Attributes.Add("style", "display:none;");
                }
                else
                {
                    tddiscount.Attributes.Add("style", "display:none;");
                }
            }
            else if (!string.IsNullOrEmpty(txtCouponCode.Text.ToString()) && !string.IsNullOrEmpty(HdnCustID.Value.ToString()) && txtCouponCode.Text.ToString().ToLower().Trim() == "pricematch")
            {
                if (!string.IsNullOrEmpty(couponcode) && couponcode.ToString().ToLower().Trim() == "pricematch")
                {
                    tddiscount.Attributes.Add("style", "display:none;");
                }
                else
                {
                    tddiscount.Attributes.Add("style", "display:none;");
                }
                SPMessage = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.GetCouponDiscount_Phoneorder('" + txtCouponCode.Text.Trim() + "'," + HdnCustID.Value.ToString() + "," + ddlStore.SelectedValue.ToString() + ")"));
                decimal.TryParse(SPMessage.ToString(), out CouponDiscount);
                if (CouponDiscount >= 0)
                {

                    Session["CustCouponCode"] = txtCouponCode.Text;
                    Session["CustCouponCodeDiscount"] = CouponDiscount;
                    Session["CustCouponvalid"] = "1";
                    txtCouponCode.Text = "";
                    changeDiscountPrice();
                    BindCartInGrid();
                    txtB_Zip_TextChanged(null, null);
                    if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                    {
                        if (rdoCreditCard.Checked)
                        {
                            rdoCreditCard.Checked = true;
                        }
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcoupon", "jAlert('Please Enter Valid Coupon Code!', 'Message', 'ContentPlaceHolder1_txtCouponCode');", true);
                    if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                    {//charge logic
                        if (rdoCreditCard.Checked == true && ViewState["Hostepaymentid"] != null)
                        {
                            // Page.ClientScript.RegisterStartupScript(btnGenerate.GetType(), "iframeload555", "document.getElementById('iframecreditcard').src = 'https://connect.chargelogic.com/?HostedPaymentID=" + ViewState["Hostepaymentid"].ToString() + "';", true);
                        }
                    }
                }

            }
            else
            {
                //SPMessage = Convert.ToString(CouponComponent.GetDiscountByCouponCodeFunction(txtCouponCode.Text.Trim(), Convert.ToInt32(HdnCustID.Value.ToString()), Convert.ToInt32(ddlStore.SelectedValue.ToString())));
                SPMessage = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.GetCouponDiscount_Phoneorder('" + txtCouponCode.Text.Trim() + "'," + HdnCustID.Value.ToString() + "," + ddlStore.SelectedValue.ToString() + ")"));
                decimal.TryParse(SPMessage.ToString(), out CouponDiscount);
                if (CouponDiscount > 0)
                {

                    Session["CustCouponCode"] = txtCouponCode.Text;
                    Session["CustCouponCodeDiscount"] = CouponDiscount;
                    txtCouponCode.Text = "";
                    if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                    {
                        if (rdoCreditCard.Checked)
                        {
                            rdoCreditCard.Checked = true;
                        }
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcoupon", "jAlert('Please Enter Valid Coupon Code!', 'Message', 'ContentPlaceHolder1_txtCouponCode');", true);
                    if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                    {//charge logic
                        if (rdoCreditCard.Checked == true && ViewState["Hostepaymentid"] != null)
                        {
                            // Page.ClientScript.RegisterStartupScript(btnGenerate.GetType(), "iframeload555", "document.getElementById('iframecreditcard').src = 'https://connect.chargelogic.com/?HostedPaymentID=" + ViewState["Hostepaymentid"].ToString() + "';", true);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(couponcode) && couponcode.ToString().ToLower().Trim() == "pricematch")
                {
                    tddiscount.Attributes.Add("style", "display:none;");
                }
                else
                {
                    tddiscount.Attributes.Add("style", "display:none;");
                }
                BindCartInGrid();
                txtB_Zip_TextChanged(null, null);
            }
        }

        private void changeDiscountPrice()
        {
            for (int i = 0; i < GVShoppingCartItems.Rows.Count; i++)
            {
                Label lblCustomerCartId = (Label)GVShoppingCartItems.Rows[i].FindControl("lblCustomerCartId");
                Label lblProductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductID");
                Label lblVariantNames = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantNames");
                Label lblVariantValues = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantValues");
                Label lblOrginalDiscountPrice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblOrginalDiscountPrice");
                Label lblPrice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblPrice");

                decimal DiscountPrice = 0;
                if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
                {
                    decimal.TryParse(lblPrice.Text.ToString().Trim(), out DiscountPrice);
                }



                if (!string.IsNullOrEmpty(lblCustomerCartId.Text.Trim()))
                {
                    CommonComponent.ExecuteCommonData("Update tb_ShoppingCartItems set DiscountPrice=" + DiscountPrice + " Where CustomCartID=" + lblCustomerCartId.Text.ToString() + " and ProductID=" + lblProductID.Text.Trim() + " and VariantNames='" + lblVariantNames.Text.ToString() + "' and VariantValues='" + lblVariantValues.Text.ToString() + "'");
                }
            }

        }

        protected void btnStorecreaditapply_Click(object sender, EventArgs e)
        {
            Session["Storecreaditamount"] = null;
            string strnotes = "";
            if (!string.IsNullOrEmpty(txtstorecreaditno.Text))
            {
                txtstorecreaditAmount.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(StoreCreditAmount,0) FROM tb_StoreCredit WHERE StoreCredit='" + txtstorecreaditno.Text.ToString().Replace("'", "''") + "' and isnull(Active,0)=1 and isnull(Deleted,0)=0 "));
                if (string.IsNullOrEmpty(txtstorecreaditAmount.Text.ToString()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcoupon", "jAlert('Please Enter Valid Number!', 'Message', 'ContentPlaceHolder1_txtstorecreaditno');", true);
                    BindCartInGrid();
                    if (TxtNotes.Text.ToString().ToLower().Contains("store credit"))
                    {
                        //string[] strNotes = TxtNotes.Text.ToString().Split("\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //strnotes = strNotes[0];
                        //TxtNotes.Text = strnotes + " \" Store Credit Applied , Store Credit Number # " + txtstorecreaditno.Text + " and Store Credit Amount is $" + Amount.ToString() + " \" refer order #" + Request.QueryString["Ono"] + "";

                        strnotes = TxtNotes.Text.ToString();
                        string strnotes1 = TxtNotes.Text.ToString().Substring(TxtNotes.Text.ToString().IndexOf("\""), TxtNotes.Text.ToString().LastIndexOf("\"") - TxtNotes.Text.ToString().IndexOf("\"") + 1);
                        strnotes = strnotes.Replace(strnotes1, " ");
                        TxtNotes.Text = strnotes.Trim();
                    }
                    if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                    {//charge logic
                        if (rdoCreditCard.Checked == true && ViewState["Hostepaymentid"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(btnGenerate.GetType(), "iframeload555", "document.getElementById('iframecreditcard').src = 'https://connect.chargelogic.com/?HostedPaymentID=" + ViewState["Hostepaymentid"].ToString() + "';", true);
                        }
                    }
                    return;
                }
                else
                {
                    txtstorecreaditAmount.Text = String.Format("{0:0.00}", Convert.ToDecimal(txtstorecreaditAmount.Text.ToString()));
                }
                Decimal total, Amount, Finaltotal = 0;
                Decimal.TryParse(hfTotal.Value.ToString(), out total); Decimal.TryParse(txtstorecreaditAmount.Text.ToString(), out Amount);

                Finaltotal = total - Amount;
                if (Finaltotal < 0) { Finaltotal = 0; }
                Session["Storecreaditamount"] = Amount;
                // hfTotal.Value = Finaltotal.ToString();
                //lblTotal.Text = Finaltotal.ToString();
                strnotes = "";
                if (!string.IsNullOrEmpty(Request.QueryString["Ono"]))
                {
                    if (TxtNotes.Text.ToString().ToLower().Contains("store credit"))
                    {
                        //string[] strNotes = TxtNotes.Text.ToString().Split("\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //strnotes = strNotes[0];
                        //TxtNotes.Text = strnotes + " \" Store Credit Applied , Store Credit Number # " + txtstorecreaditno.Text + " and Store Credit Amount is $" + Amount.ToString() + " \" refer order #" + Request.QueryString["Ono"] + "";

                        strnotes = TxtNotes.Text.ToString();
                        string strnotes1 = TxtNotes.Text.ToString().Substring(TxtNotes.Text.ToString().IndexOf("\""), TxtNotes.Text.ToString().LastIndexOf("\"") - TxtNotes.Text.ToString().IndexOf("\"") + 1);
                        strnotes = strnotes.Replace(strnotes1, " \" Store Credit Applied , Store Credit Number # " + txtstorecreaditno.Text + " and Store Credit Amount is $" + Amount.ToString() + " \"");
                        TxtNotes.Text = strnotes.Trim();
                    }
                    else
                    {
                        TxtNotes.Text = TxtNotes.Text + " \" Store Credit Applied , Store Credit Number # " + txtstorecreaditno.Text + " and Store Credit Amount is $" + Amount.ToString() + " \" refer order #" + Request.QueryString["Ono"] + "";
                    }
                }
                else
                {
                    if (TxtNotes.Text.ToString().ToLower().Contains("store credit"))
                    {
                        strnotes = TxtNotes.Text.ToString();
                        string strnotes1 = TxtNotes.Text.ToString().Substring(TxtNotes.Text.ToString().IndexOf("\""), TxtNotes.Text.ToString().LastIndexOf("\"") - TxtNotes.Text.ToString().IndexOf("\"") + 1);
                        strnotes = strnotes.Replace(strnotes1, " \" Store Credit Applied , Store Credit Number # " + txtstorecreaditno.Text + " and Store Credit Amount is $" + Amount.ToString() + " \"");
                        TxtNotes.Text = strnotes.Trim();
                    }
                    else
                    {
                        TxtNotes.Text = TxtNotes.Text + " \" Store Credit Applied , Store Credit Number # " + txtstorecreaditno.Text + " and Store Credit Amount is $" + Amount.ToString() + " \"";
                    }
                }
                if (Finaltotal == 0)
                {

                    rdoCheque.Checked = true;
                    rdoCheque_CheckedChanged(null, null);
                }
                else { }

            }

            BindCartInGrid();
            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            {//charge logic
                if (rdoCreditCard.Checked)
                {
                    rdoCreditCard.Checked = false;
                }
            }


        }

        #region nav

        private string GetPyamentId(int ExOrderNumber)
        {
            int transitionflag = 0;
            ChargelogicPayment.EFT_API_2 obj = new ChargelogicPayment.EFT_API_2();
            try
            {
                lblchargelogicError.Text = "";

                //DataSet dsrep = new DataSet();
                //dsrep = (DataSet)Session["RepCartItems"];
                ChargelogicPayment.Credential objCredential = new ChargelogicPayment.Credential();
                obj.Credentials = new System.Net.NetworkCredential(AppLogic.AppConfigs("Chargelogicusername"), AppLogic.AppConfigs("Chargelogicpassword"), AppLogic.AppConfigs("Chargelogicdomain"));
                //Credential

                objCredential.StoreNo = AppLogic.AppConfigs("Chargelogicusername").ToString();
                objCredential.APIKey = AppLogic.AppConfigs("Chargelogicpassword").ToString();
                objCredential.ApplicationNo = AppLogic.AppConfigs("ChargelogicAppID").ToString();
                objCredential.ApplicationVersion = "4.00.04";

                //Transaction,
                ChargelogicPayment.Transaction objTransaction = new ChargelogicPayment.Transaction();
                objTransaction.Currency = "USD";

                Decimal OrderTotal = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select isnull(OrderTotal,0) as OrderTotal from tb_order where ordernumber=" + ExOrderNumber + ""));
                objTransaction.Amount = String.Format("{0:0.00}", Convert.ToDecimal(OrderTotal));


                objTransaction.Amount = String.Format("{0:0.00}", Convert.ToDecimal(txtBoxcaptureamount.Text.ToString()));
                //}

                objTransaction.ExternalReferenceNumber = ExOrderNumber.ToString();

                objTransaction.ConfirmationID = txtCOnfirmationID.Text.ToString();

                Session["CustID"] = HdnCustID.Value.ToString();


                //Address
                ChargelogicPayment.Address objbillAddress = new ChargelogicPayment.Address();
                objbillAddress.Name = txtB_FName.Text.ToString() + " " + txtB_LName.Text.ToString();
                objbillAddress.StreetAddress = txtB_Add1.Text.ToString();
                objbillAddress.StreetAddress2 = txtB_Add2.Text.ToString();
                objbillAddress.City = txtB_City.Text.ToString();
                if (ddlB_State.SelectedValue.ToString() == "-11")
                {
                    objbillAddress.State = txtB_OtherState.Text.ToString();
                }
                else
                {
                    objbillAddress.State = ddlB_State.SelectedItem.Text.ToString();
                }

                objbillAddress.PostCode = txtB_Zip.Text.ToString();
                string strCode = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(TwoLetterISOCode,'') FROM tb_Country WHERE CountryID=" + ddlB_Country.SelectedValue.ToString() + ""));
                objbillAddress.Country = strCode.ToString();
                objbillAddress.PhoneNumber = txtB_Phone.Text.ToString();
                objbillAddress.Email = TxtEmail.Text.ToString();
                //Address

                ChargelogicPayment.Address objshippAddress = new ChargelogicPayment.Address();
                objshippAddress.Name = txtS_FName.Text.ToString() + " " + txtS_LNAme.Text.ToString();
                objshippAddress.StreetAddress = txtS_Add1.Text.ToString();
                objbillAddress.StreetAddress2 = txtS_Add2.Text.ToString();
                objshippAddress.City = txtS_City.Text.ToString();
                if (ddlS_State.SelectedValue.ToString() == "-11")
                {
                    objshippAddress.State = txtS_OtherState.Text.ToString();
                }
                else
                {
                    objshippAddress.State = ddlS_State.SelectedItem.Text.ToString();
                }

                objshippAddress.PostCode = txtS_Zip.Text.ToString();
                strCode = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(TwoLetterISOCode,'') FROM tb_Country WHERE CountryID=" + ddlS_Country.SelectedValue.ToString() + ""));
                objshippAddress.Country = strCode.ToString();
                objshippAddress.PhoneNumber = txtS_Phone.Text.ToString();
                objshippAddress.Email = TxtEmail.Text.ToString();

                //HostedPayment
                //ChargelogicPayment.HostedPayment objHostedPayment = new ChargelogicPayment.HostedPayment();
                //objHostedPayment.RequireCVV = "Yes";
                //objHostedPayment.ReturnURL = "https://" + Request.Url.Host.ToString() + "/chargelogiccheckout.aspx";
                //objHostedPayment.Language = "ENU";
                //objHostedPayment.ConfirmationID = txtCOnfirmationID.Text.ToString();
                //objHostedPayment.MerchantResourceURL = "https://" + Request.Url.Host.ToString() + "/ChargeLogicMerchantResource.html";
                //objHostedPayment.Embedded = "False";
                //objHostedPayment.FieldLabelFontColor = "393939";
                //objHostedPayment.LogoURL = "https://www.halfpricedrapes.com/images/logo.png";
                //objHostedPayment.BorderColor = "8C191E";
                //objHostedPayment.HeaderFontColor = "8C191E";
                //objHostedPayment.ButtonBackgroundColor = "608B2B";

                ChargelogicPayment.Response objresponse = new ChargelogicPayment.Response();

                //string Hostepaymentid = obj.SetupHostedOrder(objCredential, objTransaction, objbillAddress, objshippAddress, objHostedPayment);
                ChargelogicPayment.Card Card = new ChargelogicPayment.Card();

                Card.CardholderName = TxtNameOnCard.Text.Trim();

                Card.AccountNumber = TxtCardNumber.Text.Trim();

                // Card.Track2Data="";

                Card.ExpirationYear = ddlYear.SelectedValue.ToString().Substring(2, 2);

                Card.CardVerificationValue = TxtCardVerificationCode.Text.ToString();

                Card.ExpirationMonth = ddlMonth.SelectedValue.ToString();

                string Hostepaymentid = "";
                // obj.SetupHostedOrder(objCredential, objTransaction, objbillAddress, objshippAddress, objHostedPayment);

                //obj.SetupHostedOrder(objCredential, objTransaction, objbillAddress, objshippAddress, objHostedPayment);
                // iframeId.Attributes.Add("src", "https://connect.chargelogic.com/?HostedPaymentID=" + Hostepaymentid + "");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "iframeload", "iframerealod('" + Hostepaymentid + "');", true);
                Random rd = new Random();
                ViewState["Hostepaymentid"] = "";
                string ReferenceNumber = "";


                try
                {


                    ReferenceNumber = obj.CreditCardAuthorize(objCredential, Card, objTransaction, objbillAddress, ref objresponse);
                    if (objresponse.TransactionStatus.ToString().ToLower() == "approved")
                    {


                        ViewState["Hostepaymentid"] = Hostepaymentid;

                        string Browser = "";
                        try
                        {

                            System.Web.HttpBrowserCapabilities bb = Request.Browser;
                            Browser = bb.Browser.ToString() + " " + bb.Version.ToString();
                        }
                        catch { }
                        //Int32 UserType = 0;
                        //String Userpass = "";

                        //if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                        //{
                        //    UserType = 1;
                        //}
                        //else if (Session["UserCreated"] != null && Session["UserCreated"].ToString() != "" && Session["UserCreated"].ToString().Trim() == "1")
                        //{
                        //    UserType = 2;
                        //    if (Session["pass"] != null && Session["pass"].ToString() != "")
                        //    {
                        //        Userpass = SecurityComponent.Encrypt(Session["pass"].ToString());
                        //    }

                        //}

                        String sql2 = "update tb_order set " +
                            "PaymentGateway='CREDITCARD',Deleted=0,TransactionStatus='CAPTURED',ChargeHostedPaymentID='{7107BCB0-1514-4503-930C-F03D2A0833DC}',Browser='" + Browser.ToString().Replace("'", "''") + "',ChargeConfirmationID='" + ReferenceNumber + "',CapturedOn=getdate() ," +
                            "AuthorizationResult='', " +
                            "AuthorizationCode='', " +
                            "AuthorizationPNREF='" + ReferenceNumber + "', " +
                            "TransactionCommand='', " +
                            "CaptureTxCommand='', " +
                            "CaptureTXResult='' where OrderNumber=" + ExOrderNumber.ToString();
                        CommonComponent.ExecuteCommonData(sql2);
                        //CommonComponent.ExecuteCommonData("update tb_order set ChargeHostedPaymentID='{7107BCB0-1514-4503-930C-F03D2A0833DC}',Browser='" + Browser.ToString().Replace("'", "''") + "',ChargeConfirmationID='" + ReferenceNumber + "',AuthorizationPNREF='" + ReferenceNumber + "' where ordernumber=" + ExOrderNumber + "");
                        // ProcessOrder(ReferenceNumber.ToString(), ExOrderNumber);
                        return "OK";
                    }
                    else
                    {
                        string responsecode = objresponse.ResponseCode.ToString();
                        string mess = "";

                        // mess = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Message,'') as Message from tb_ChargelogicResponse where ResponseCode='" + responsecode + "'"));

                        if (objresponse.CardVerificationValueAlert.ToString().ToLower().Trim() == "partial match" || objresponse.CardVerificationValueAlert.ToString().ToLower().Trim() == "mismatch" || objresponse.CardVerificationValueAlert.ToString().ToLower().Trim() == "yes" || objresponse.CardVerificationValueAlert.ToString().ToLower().Trim() == "y")
                        {
                            mess = "Error in Credit Card information. Please verify and try again.";
                            transitionflag = 1;

                        }
                        else if (objresponse.AddressVerificationAlert.ToString().ToLower().Trim() == "partial match" || objresponse.AddressVerificationAlert.ToString().ToLower().Trim() == "mismatch" || objresponse.AddressVerificationAlert.ToString().ToLower().Trim() == "yes" || objresponse.AddressVerificationAlert.ToString().ToLower().Trim() == "y")
                        {
                            mess = "Error in Billing address. Please verify and try again.(Note: Billing address must match the address associated with the Credit Card being used for this transaction.)";
                            transitionflag = 2;

                        }

                        else
                        {

                            mess = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Message,'') as Message from tb_ChargelogicResponse where ResponseCode='" + responsecode + "'"));

                        }
                        if (transitionflag == 0)
                        {
                            if (mess.ToString().ToLower() == "declined")
                            {
                                mess = "Error in Credit Card information. Please verify and try again.";
                                transitionflag = 1;
                            }
                        }
                        if (mess.ToString().ToLower().IndexOf("invalid account") > -1)
                        {
                            mess = "Error in Credit Card information. Please verify and try again.";
                        }
                        if (!String.IsNullOrEmpty(mess))
                        {
                            lblchargelogicError.Text = mess.ToString();


                        }
                        else
                        {
                            //mess = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Message,'') as Message from tb_ChargelogicResponse where ResponseCode='" + objresponse.Message.ToString() + "'"));
                            mess = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Message,'') as Message from tb_ChargelogicResponse where ResponseCode='" + objresponse.HostResponseCode.ToString() + "'"));
                            if (transitionflag == 0)
                            {
                                if (mess.ToString().ToLower() == "declined")
                                {
                                    mess = "Error in Credit Card information. Please verify and try again.";
                                    transitionflag = 1;
                                }
                            }
                            if (mess.ToString().ToLower().IndexOf("invalid account") > -1)
                            {
                                mess = "Error in Credit Card information. Please verify and try again.";
                            }
                            if (String.IsNullOrEmpty(mess))
                            {
                                //  mess = "Error in Charge Logic Payment,Please try again.";
                                mess = "Error in Credit Card information. Please verify and try again.";
                                lblchargelogicError.Text = mess;
                            }
                            else
                            {
                                lblchargelogicError.Text = mess;
                            }
                        }


                        return "ERROR";
                        //  ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertwrongmeg11", "window.scrollTo(0,0);alert('" + mess + "');", true);



                    }
                }
                catch (Exception ex)
                {
                    lblchargelogicError.Text = ex.Message.ToString().Trim();
                    if (transitionflag == 0)
                    {
                        if (lblchargelogicError.Text.ToString().ToLower() == "declined")
                        {
                            lblchargelogicError.Text = "Error in Credit Card information. Please verify and try again.";
                        }
                    }
                    if (lblchargelogicError.Text.ToString().ToLower().IndexOf("invalid account") > -1)
                    {
                        lblchargelogicError.Text = "Error in Credit Card information. Please verify and try again.";
                    }


                    // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertwrongmeg11", "window.scrollTo(0,0);alert('" + lblchargelogicError.Text + "');", true);
                    return "ERROR";
                }


            }



            catch (Exception ex)
            {

                Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());
                return "ERROR";
            }


        }

        //private void GetPyamentId(int ExOrderNumber)
        //{
        //    ChargelogicPayment.EFT_API_2 obj = new ChargelogicPayment.EFT_API_2();
        //    try
        //    {

        //        ChargelogicPayment.Credential objCredential = new ChargelogicPayment.Credential();
        //        obj.Credentials = new System.Net.NetworkCredential(AppLogic.AppConfigs("Chargelogicusername"), AppLogic.AppConfigs("Chargelogicpassword"), AppLogic.AppConfigs("Chargelogicdomain"));
        //        //Credential

        //        objCredential.StoreNo = AppLogic.AppConfigs("Chargelogicusername").ToString();
        //        objCredential.APIKey = AppLogic.AppConfigs("Chargelogicpassword").ToString();
        //        objCredential.ApplicationNo = AppLogic.AppConfigs("ChargelogicAppID").ToString();
        //        objCredential.ApplicationVersion = "4.00.04";

        //        //Transaction,
        //        ChargelogicPayment.Transaction objTransaction = new ChargelogicPayment.Transaction();
        //        objTransaction.Currency = "USD";
        //        //if (ViewState["OrderTotal"] != null)
        //        //{
        //        objTransaction.Amount = String.Format("{0:0.00}", Convert.ToDecimal(txtBoxcaptureamount.Text.ToString()));
        //        //}

        //        objTransaction.ExternalReferenceNumber = ExOrderNumber.ToString();

        //        objTransaction.ConfirmationID = txtCOnfirmationID.Text.ToString();

        //        Session["CustID"] = HdnCustID.Value.ToString();


        //        //Address
        //        ChargelogicPayment.Address objbillAddress = new ChargelogicPayment.Address();
        //        objbillAddress.Name = txtB_FName.Text.ToString() + " " + txtB_LName.Text.ToString();
        //        objbillAddress.StreetAddress = txtB_Add1.Text.ToString();
        //        objbillAddress.StreetAddress2 = txtB_Add2.Text.ToString();
        //        objbillAddress.City = txtB_City.Text.ToString();
        //        if (ddlB_State.SelectedValue.ToString() == "-11")
        //        {
        //            objbillAddress.State = txtB_OtherState.Text.ToString();
        //        }
        //        else
        //        {
        //            objbillAddress.State = ddlB_State.SelectedItem.Text.ToString();
        //        }

        //        objbillAddress.PostCode = txtB_Zip.Text.ToString();
        //        string strCode = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(TwoLetterISOCode,'') FROM tb_Country WHERE CountryID=" + ddlB_Country.SelectedValue.ToString() + ""));
        //        objbillAddress.Country = strCode.ToString();
        //        objbillAddress.PhoneNumber = txtB_Phone.Text.ToString();
        //        objbillAddress.Email = TxtEmail.Text.ToString();
        //        //Address

        //        ChargelogicPayment.Address objshippAddress = new ChargelogicPayment.Address();

        //        objshippAddress.Name = txtS_FName.Text.ToString() + " " + txtS_LNAme.Text.ToString();
        //        objshippAddress.StreetAddress = txtS_Add1.Text.ToString();
        //        objbillAddress.StreetAddress2 = txtS_Add2.Text.ToString();
        //        objshippAddress.City = txtS_City.Text.ToString();
        //        if (ddlS_State.SelectedValue.ToString() == "-11")
        //        {
        //            objshippAddress.State = txtS_OtherState.Text.ToString();
        //        }
        //        else
        //        {
        //            objshippAddress.State = ddlS_State.SelectedItem.Text.ToString();
        //        }

        //        objshippAddress.PostCode = txtS_Zip.Text.ToString();
        //        strCode = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(TwoLetterISOCode,'') FROM tb_Country WHERE CountryID=" + ddlS_Country.SelectedValue.ToString() + ""));
        //        objshippAddress.Country = strCode.ToString();
        //        objshippAddress.PhoneNumber = txtS_Phone.Text.ToString();
        //        objshippAddress.Email = TxtEmail.Text.ToString();

        //        //HostedPayment
        //        ChargelogicPayment.HostedPayment objHostedPayment = new ChargelogicPayment.HostedPayment();
        //        objHostedPayment.RequireCVV = "Yes";
        //        objHostedPayment.ReturnURL = "https://" + Request.Url.Host.ToString() + "/chargelogiccheckout.aspx";
        //        objHostedPayment.Language = "ENU";
        //        objHostedPayment.ConfirmationID = txtCOnfirmationID.Text.ToString();
        //        objHostedPayment.MerchantResourceURL = "https://" + Request.Url.Host.ToString() + "/ChargeLogicMerchantResource.html";
        //        objHostedPayment.Embedded = "Yes";
        //        objHostedPayment.FieldLabelFontColor = "393939";
        //        ChargelogicPayment.Response objresponse = new ChargelogicPayment.Response();

        //        string Hostepaymentid = obj.SetupHostedOrder(objCredential, objTransaction, objbillAddress, objshippAddress, objHostedPayment);

        //        //obj.SetupHostedOrder(objCredential, objTransaction, objbillAddress, objshippAddress, objHostedPayment);
        //        // iframeId.Attributes.Add("src", "https://connect.chargelogic.com/?HostedPaymentID=" + Hostepaymentid + "");
        //        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "iframeload", "iframerealod('" + Hostepaymentid + "');", true);
        //        Random rd = new Random();
        //        ViewState["Hostepaymentid"] = Hostepaymentid;
        //        //if (!IsPostBack)
        //        //{
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "iframeload", "iframerealod('" + Hostepaymentid + "'," + rd.Next(1000).ToString() + ");", true);
        //        //}
        //        //else
        //        //{
        //        //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "iframeload", "iframerealod('" + Hostepaymentid + "'," + rd.Next(1000).ToString() + ");", true);
        //        //}


        //    }
        //    catch (Exception ex)
        //    {

        //        //Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());
        //    }


        //}
        #endregion
    }
}
