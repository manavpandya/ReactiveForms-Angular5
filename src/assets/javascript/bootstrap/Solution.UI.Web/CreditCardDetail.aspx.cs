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
namespace Solution.UI.Web
{
    public partial class CreditCardDetail : System.Web.UI.Page
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

            CommonOperations.RedirectWithSSL(true);
            EditCCDetail();
            if (!IsPostBack)
            {
                Session["PageName"] = "Credit Card Detail";

                FillCountry();
                FillCreditCardTypeYear();
                Clear();
                if (Session["CustID"] != null && Convert.ToString(Session["CustID"]) != "")
                {
                    if (Request.QueryString["Type"] == "0")
                    {
                        ViewState["Mode"] = "Insert";
                        pnlAddEdit.Visible = true;
                        pnlViewDetails.Visible = false;
                        ltbrTitle.Text = "Add a Credit Card";
                        ltTitle.Text = "Add a Credit Card";
                        FillBillingAddress();
                    }
                    else if (Request.QueryString["Type"] == "1")
                    {
                        pnlAddEdit.Visible = false;
                        pnlViewDetails.Visible = true;
                        ltbrTitle.Text = "Credit Card Detail";
                        ltTitle.Text = "Credit Card Detail";
                        FillCreditCardDetail();
                        FillBillingAddress();
                    }
                }
                txtcardName.Focus();
            }
            Page.MaintainScrollPositionOnPostBack = true;
        }

        /// <summary>
        /// For Recognize which button is fired either Edit or Delete for Credit Card 
        /// </summary>
        private void EditCCDetail()
        {
            if (Session["CustID"] != null)
            {
                string[] formkeys = Request.Form.AllKeys;
                foreach (String s in formkeys)
                {
                    //To Move Address Details and Redirect to EditCCDetail page
                    if (s.Contains("bt_MoveToEditCCDetail"))
                    {
                        string[] p = s.Split(':');
                        MoveToEditCCDetail(Convert.ToInt32(p[1].ToString()));
                    }
                    //For Delete Address From Address Table
                    if (s.Contains("bt_DeleteCCDetail"))
                    {
                        string[] p = s.Split(':');
                        DeleteCCDetail(Convert.ToInt32(p[1].ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// Move To EditCCDetail to Change Customer's Credit Card Detail
        /// </summary>
        /// <param name="CategoryID">int CategoryID</param>
        private void MoveToEditCCDetail(int CardID)
        {
            Session["__ISREFRESH"] = null;
            ViewState["Mode"] = "Update";
            pnlViewDetails.Visible = false;
            pnlAddEdit.Visible = true;
            ltbrTitle.Text = "Edit a Credit Card";
            SetCreditCardDetail(CardID);
        }

        /// <summary>
        /// Delete Credit Card Details
        /// </summary>
        /// <param name="CardID">int CardID</param>
        private void DeleteCCDetail(Int32 CardID)
        {
            objCreditCard = new CreditCardComponent();
            try
            {
                Int32 CustomerID = Convert.ToInt32(Session["CustID"].ToString().Trim());

                if (!objCreditCard.CheckIsDefaultCreditCard(CustomerID, CardID))
                {
                    if (objCreditCard.DeleteCreditCardDetail(CardID) != -1)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Deleted", "alert('Credit Card Detail is deleted successfully.');", true);
                        FillCreditCardDetail();
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailDeleted", "alert('Problem in deleting Credit Card Detail, please try again.');", true);
                        return;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('You can not delete this payment method because it is used as default credit card.');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set Credit Card Detail
        /// </summary>
        /// <param name="CardID">int CardID</param>
        public void SetCreditCardDetail(Int32 CardID)
        {

            objCustomer = new CustomerComponent();
            DataSet dsCustomerDetail = new DataSet();
            dsCustomerDetail = objCustomer.GetCustomerDetailByCustID(Convert.ToInt32(Session["CustID"]));
            ViewState["CardID"] = CardID.ToString();
            objCreditCard = new CreditCardComponent();
            DataSet dsCCDetails = new DataSet();
            dsCCDetails = objCreditCard.GetCreditCardDetailByCardID(CardID);

            if (dsCCDetails != null && dsCCDetails.Tables.Count > 0 && dsCCDetails.Tables[0].Rows.Count > 0)
            {
                txtcardName.Text = Convert.ToString(dsCCDetails.Tables[0].Rows[0]["NameOnCard"]);
                txtCardNumber.Text = SecurityComponent.Decrypt(Convert.ToString(dsCCDetails.Tables[0].Rows[0]["CardNumber"]));
                ddlyear.Items.Clear();
                ddlyear.Items.Insert(0, new ListItem("Year", "0"));
                FillCreditCardTypeYear();
                ddlCardType.SelectedValue = Convert.ToString(dsCCDetails.Tables[0].Rows[0]["CardTypeID"]);
                if (Convert.ToString(dsCustomerDetail.Tables[0].Rows[0]["CreditCardID"]) == Convert.ToString(CardID))
                {
                    chkDefaultCreditCardID.Checked = true;
                    chkDefaultCreditCardID.Enabled = false;
                    trmsgdefault.Visible = true;
                }
                else
                {
                    chkDefaultCreditCardID.Checked = false;
                    chkDefaultCreditCardID.Enabled = true;
                    trmsgdefault.Visible = false;
                }
                txtCardVarificationCode.Text = Convert.ToString(dsCCDetails.Tables[0].Rows[0]["CardVerificationCode"]);
                ddlmonth.SelectedIndex = Convert.ToInt32(dsCCDetails.Tables[0].Rows[0]["CardExpirationMonth"]);

                if (ddlyear.Items.FindByText(Convert.ToString(dsCCDetails.Tables[0].Rows[0]["CardExpirationYear"])) != null)
                {
                    ddlyear.Items.FindByText(Convert.ToString(dsCCDetails.Tables[0].Rows[0]["CardExpirationYear"])).Selected = true;
                }

                try
                {
                    ddlBillingAddress.SelectedValue = Convert.ToString(dsCCDetails.Tables[0].Rows[0]["DefaultBillingAddressID"]);
                }
                catch
                {
                    ddlBillingAddress.SelectedIndex = 0;
                }

                //if (Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) > 0)
                //{
                //    ddlBillingAddress.SelectedValue = Convert.ToString(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]);
                //}
            }
        }

        /// <summary>
        ///Fill Credit Card Detail
        /// </summary>
        private void FillCreditCardDetail()
        {
            objCustomer = new CustomerComponent();
            DataSet dsCustomerDetail = new DataSet();
            dsCustomerDetail = objCustomer.GetCustomerDetailByCustID(Convert.ToInt32(Session["CustID"]));
            if (dsCustomerDetail != null && dsCustomerDetail.Tables.Count > 0 && dsCustomerDetail.Tables[0].Rows.Count > 0)
            {
                String strCCDetail = "";

                objCreditCard = new CreditCardComponent();
                DataSet dsCCDetail = new DataSet();
                dsCCDetail = objCreditCard.GetCreditCardDetailByCustomerID(Convert.ToInt32(Session["CustID"]));

                if (dsCCDetail != null && dsCCDetail.Tables.Count > 0 && dsCCDetail.Tables[0].Rows.Count > 0)
                {
                    string CardNo = string.Empty;

                    for (int i = 0; i < dsCCDetail.Tables[0].Rows.Count; i++)
                    {
                        DataRow[] drCard = dsCCDetail.Tables[0].Select("CardID=" + Convert.ToInt32(dsCCDetail.Tables[0].Rows[i]["CardID"]));

                        if (drCard.Length > 0)
                        {

                            strCCDetail += "<tr style=\"line-height:7px;\"><td>";
                            strCCDetail += "Name on Card : <strong> " + Convert.ToString(drCard[0]["NameOnCard"]) + "</strong><br>";
                            strCCDetail += "Card Type : " + Convert.ToString(drCard[0]["CardName"]) + "<br>";
                            String CardNumber = SecurityComponent.Decrypt(Convert.ToString(drCard[0]["CardNumber"]));
                            strCCDetail += "Card Number : ************" + CardNumber.ToString().Substring(CardNumber.Length - 4, 4) + "<br>";
                            //strCCDetail += "Card Verification Code : " + Convert.ToString(drCard[0]["CardVerificationCode"]) + "<br>";

                            string CCExpirationMonth = string.Empty;
                            switch (Convert.ToInt32(drCard[0]["CardExpirationMonth"]))
                            {
                                case 1: CCExpirationMonth = "Jan";
                                    break;
                                case 2: CCExpirationMonth = "Feb";
                                    break;
                                case 3: CCExpirationMonth = "Mar";
                                    break;
                                case 4: CCExpirationMonth = "Apr";
                                    break;
                                case 5: CCExpirationMonth = "May";
                                    break;
                                case 6: CCExpirationMonth = "Jun";
                                    break;
                                case 7: CCExpirationMonth = "Jul";
                                    break;
                                case 8: CCExpirationMonth = "Aug";
                                    break;
                                case 9: CCExpirationMonth = "Sep";
                                    break;
                                case 10: CCExpirationMonth = "Oct";
                                    break;
                                case 11: CCExpirationMonth = "Nov";
                                    break;
                                case 12: CCExpirationMonth = "Dec";
                                    break;
                                default: CCExpirationMonth = string.Empty;
                                    break;
                            }


                            strCCDetail += "Expiration Month : " + CCExpirationMonth + "<br>";
                            strCCDetail += "Expiration Year : " + Convert.ToString(drCard[0]["CardExpirationYear"]) + "<br><br>";
                            strCCDetail += "<input type='submit' runat='server' style='display: none;' name='bt_MoveToEditCCDetail:" + Convert.ToString(drCard[0]["CardID"]) + "' value='bt_MoveToEditCCDetail:" + Convert.ToString(drCard[0]["CardID"]) + "' id='bt_MoveToEditCCDetail:" + Convert.ToString(drCard[0]["CardID"]) + "' class='btn1' onClick='" + Convert.ToString(drCard[0]["CardID"]) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick='document.getElementById(\"bt_MoveToEditCCDetail:" + Convert.ToString(drCard[0]["CardID"]) + "\").click();'>Edit</a>";

                            if (Convert.ToString(dsCustomerDetail.Tables[0].Rows[0]["CreditCardID"]) != Convert.ToString(drCard[0]["CardID"]))
                            {
                                strCCDetail += "&nbsp;&nbsp;<input type='submit' runat='server' style='display: none;' name='bt_DeleteCCDetail:" + Convert.ToString(drCard[0]["CardID"]) + "' value='bt_DeleteCCDetail:" + Convert.ToString(drCard[0]["CardID"]) + "' id='bt_DeleteCCDetail:" + Convert.ToString(drCard[0]["CardID"]) + "' class='btn1' onClick='" + Convert.ToString(drCard[0]["CardID"]) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;'   onclick=\"if( confirm('Are you sure you want to delete this Payment Method?') ) { document.getElementById('bt_DeleteCCDetail:" + Convert.ToString(drCard[0]["CardID"]) + "').click(); } else { return false; } \">Delete</a>";
                            }

                            strCCDetail += "</td></tr>";
                        }
                        ltrViewDetails.Text = strCCDetail.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Clear Credit Card Fields
        /// </summary>
        public void Clear()
        {
            this.txtcardName.Text = "";
            this.txtCardNumber.Text = "";
            this.ddlCardType.SelectedIndex = 0;
            this.txtCardVarificationCode.Text = "";
            this.ddlmonth.SelectedIndex = 0;
            this.ddlyear.SelectedIndex = 0;
            this.chkDefaultCreditCardID.Checked = false;
            this.ddlBillingAddress.SelectedIndex = 0;
            if (this.pnlBillingDetails.Visible)
            {
                ClearAddress();
                this.pnlBillingDetails.Visible = false;
            }
        }

        /// <summary>
        /// Clear Address Fields
        /// </summary>
        public void ClearAddress()
        {
            txtBillFirstname.Text = "";
            txtBillLastname.Text = "";
            txtBillingCompany.Text = "";
            txtBilladdressLine1.Text = "";
            txtBillAddressLine2.Text = "";
            txtBillsuite.Text = "";
            txtBillCity.Text = "";
            txtBillZipCode.Text = "";
            txtBillphone.Text = "";
            txtBillEmail.Text = "";
            txtBillingFax.Text = "";
            if (ddlBillcountry.Items.FindByText("United States") != null)
            {
                ddlBillcountry.Items.FindByText("United States").Selected = true;
            }
            else
            {
                ddlBillcountry.SelectedIndex = 0;
            }
            ddlBillcountry_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Validate Card expiration Date
        /// </summary>
        /// <returns>Returns true if Properly Validate</returns>
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
        /// Submit Button Click Event for Credit Card Details Insert
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            int CardID = 0;
            if (!CheckCardExpirationDate())
            {
                objCreditCard = new CreditCardComponent();
                objCustomer = new CustomerComponent();
                tb_CreditCardDetails = new tb_CreditCardDetails();

                tb_CreditCardDetails = SetCreditCardValues();

                if (ViewState["Mode"] != null && ViewState["Mode"].ToString().Trim() == "Insert")
                {
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
                            if (chkDefaultCreditCardID.Checked)
                            {
                                if (!objCreditCard.UpdateCustomerCreditCardID(Convert.ToInt32(Session["CustID"].ToString()), CardID))
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in adding Customer Credit Card ID, please try again.');", true);
                                    return;
                                }
                            }

                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your Credit Card Detail is added successfully.');", true);


                            ltbrTitle.Text = "Manage Payment Methods";

                            pnlBillingDetails.Visible = false;
                            pnlAddEdit.Visible = false;
                            pnlViewDetails.Visible = true;
                            FillCreditCardDetail();
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in adding Credit Card Detail, please try again.');", true);
                            return;
                        }
                    }
                }
                else
                {
                    if (objCreditCard.CheckCreditCardIsExists(Convert.ToInt32(Session["CustID"]), SecurityComponent.Encrypt(txtCardNumber.Text.ToString().Trim()), Convert.ToInt32(ddlCardType.SelectedValue.ToString()), Convert.ToInt32(ViewState["CardID"].ToString())))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your Credit Card Detail already exists.');", true);
                        return;
                    }
                    else
                    {
                        if (objCreditCard.UpdateCreditCardDetil(tb_CreditCardDetails, Convert.ToInt32(ViewState["CardID"].ToString())) != -1)
                        {
                            if (this.chkDefaultCreditCardID.Checked)
                            {
                                if (!objCreditCard.UpdateCustomerCreditCardID(Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(ViewState["CardID"].ToString())))
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in updating Credit Card Detail, please try again.');", true);
                                    return;
                                }
                            }

                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your Credit Card Detail is updated successfully.');", true);

                            pnlAddEdit.Visible = false;
                            pnlViewDetails.Visible = true;
                            FillCreditCardDetail();
                            ltbrTitle.Text = "Manage Payment Methods";
                            pnlBillingDetails.Visible = false;

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in updating Credit Card Detail, please try again.');", true);
                            return;
                        }
                    }
                }
                Clear();
                // Response.Redirect("/creditcarddetail.aspx?Type=1", true);
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
        /// Submit Button Click Event for Billing Address Details Insert
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnFinish_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (this.ddlBillingAddress.SelectedValue == "-12")
                {
                    int BillAddressID = InsertBillAddress(Convert.ToInt32(Session["CustID"]));

                    if (BillAddressID != -1)
                    {
                        ddlBillingAddress.Items.Clear();
                        FillBillingAddress();
                        ddlBillingAddress.SelectedValue = Convert.ToString(BillAddressID);
                        ClearAddress();
                        pnlBillingDetails.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert Billing Address
        /// </summary>
        /// <param name="CustID">Int32 CustID</param>
        /// <returns>Returns Inserted AddressID</returns>
        public Int32 InsertBillAddress(Int32 CustID)
        {
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
            return isadded;
        }

        /// <summary>
        /// Billing Address Selected Index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBillingAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBillingAddress.SelectedValue == "-12")
            {
                pnlBillingDetails.Visible = true;
                ClearAddress();
            }
            else
            {
                pnlBillingDetails.Visible = false;
            }
            if (ddlBillingAddress.SelectedValue != "-12" && ddlBillingAddress.SelectedIndex != 0)
            {
                ViewState["BillingAddress"] = ddlBillingAddress.SelectedValue.ToString();
            }
        }

        /// <summary>
        /// Bind both Country Dropdown list
        /// </summary>
        public void FillCountry()
        {
            ddlBillcountry.Items.Clear();
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
            }
            else
            {
                ddlBillcountry.DataSource = null;
                ddlBillcountry.DataBind();
            }

            if (ddlBillcountry.Items.FindByText("United States") != null)
            {
                ddlBillcountry.Items.FindByText("United States").Selected = true;
            }
            ddlBillcountry_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Billing Country selected change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    ddlBillstate.Items.FindByText("Other").Selected = true;
                    ClientScript.RegisterStartupScript(typeof(Page), "BillingVisible", "SetBillingOtherVisible(true);", true);

                }
            }
            else
            {
                ddlBillstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlBillstate.Items.Insert(1, new ListItem("Other", "-11"));
                ddlBillstate.Items.FindByText("Other").Selected = true;
                ClientScript.RegisterStartupScript(typeof(Page), "BillingVisible", "SetBillingOtherVisible(true);", true);

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
        /// Fill Billing Address
        /// </summary>
        private void FillBillingAddress()
        {
            objCreditCard = new CreditCardComponent();
            DataSet dsBilliAddress = new DataSet();
            dsBilliAddress = objCreditCard.GetAddressForByCustId_AddType(Convert.ToInt32(Session["CustID"]), 0);

            if (dsBilliAddress != null && dsBilliAddress.Tables.Count > 0 && dsBilliAddress.Tables[0].Rows.Count > 0)
            {
                ddlBillingAddress.DataSource = dsBilliAddress.Tables[0];
                ddlBillingAddress.DataTextField = "Address";
                ddlBillingAddress.DataValueField = "AddressID";
                ddlBillingAddress.DataBind();
            }
            ddlBillingAddress.Items.Insert(0, new ListItem("Select Billing Address", "0"));
            ddlBillingAddress.Items.Insert(1, new ListItem("Add New", "-12"));
            pnlBillingDetails.Visible = false;
        }

        /// <summary>
        /// For show panel of Add New Credit Card Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Mode"] = "Insert";
                this.pnlAddEdit.Visible = true;
                this.pnlViewDetails.Visible = false;
                ltbrTitle.Text = "Add a Credit Card";
                ddlBillingAddress.Items.Clear();
                FillBillingAddress();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Billing Cancel event for Hide billing Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBillingCancel_Click(object sender, ImageClickEventArgs e)
        {
            ClearAddress();
            if (pnlBillingDetails.Visible)
            {
                pnlBillingDetails.Visible = false;
            }
            if (ViewState["BillingAddress"] != null)
            {
                try
                {
                    ddlBillingAddress.SelectedValue = Convert.ToString(ViewState["BillingAddress"]);
                }
                catch
                {
                    ddlBillingAddress.SelectedIndex = 0;
                }
            }
            else
            {
                ddlBillingAddress.SelectedIndex = 0;
            }
        }
    }
}