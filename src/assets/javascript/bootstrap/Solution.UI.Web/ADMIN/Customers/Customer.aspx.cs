using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.IO;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class Customer : BasePage
    {
        #region Local Variables
        StoreComponent objStorecomponent = null;
        CustomerComponent objCustomer = null;
        public Int32 CustomerID = 0;


        public bool DiscountProductStructure = false;
        public bool DiscountCategoryStructure = false;
        public bool DiscountBrandStructure = false;

        DataSet dsProduct = new DataSet();
        DataSet dsCategory = new DataSet();
        DataSet dsBrand = new DataSet();

        public DataTable dt;

        #endregion


        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["CustID"] != null && Convert.ToString(Request.QueryString["CustID"]).Trim().ToLower() != "")
            {
                CustomerID = Convert.ToInt32(Request.QueryString["CustID"]);
            }
            if (!IsPostBack)
            {
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                btnGenrate.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/generate.png";
                ImgSendMail.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/send-mail.png";
                bindstore();
                FillCountry();
                FillCustomerLevel();
                BindTemplateList();
                ViewState["IsPasswordChanged"] = "0"; // 1 = changed and 0 = not changed
                if (Request.QueryString["Mode"] != null && Convert.ToString(Request.QueryString["Mode"]).Trim().ToLower() == "edit")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "SetUploadDocVisible();", true);
                    divCustDetail.Visible = true;
                    abillinglink.Visible = true;
                    ashippinglink.Visible = true;
                    lblHeader.Text = "Update Customer";
                    FillCustomerData();
                    trBlockIP.Visible = true;
                    ddlStoreName.Enabled = false;
                    trBlockIP.Visible = true;
                    if (Request.QueryString["CustID"] != null && Convert.ToString(Request.QueryString["CustID"]).Trim().ToLower() != "")
                    {
                        btnGenrate.Visible = true;
                    }
                    GetCustomerAddressInfo();
                    ImgSendMail.Visible = true;
                }
                else
                {
                    lblHeader.Text = "Add Customer";
                    ViewState["Password"] = null;
                    trBlockIP.Visible = false;
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "trcoupcode", "if(document.getElementById('trcoupcode') != null){document.getElementById('trcoupcode').style.display='none'; }", true);
                    trcoupcode.Attributes.Add("style", "display:none;");
                    ImgSendMail.Visible = false;
                }

                BinCategoryDiscountDeatailbyCustID();
                BinProductDiscountDeatailbyCustID();
                BindCustomerHistory();
                if (chkShowCustHistory.Checked)
                {
                    trCustHistory.Attributes.Add("style", "display:''");
                }
                else
                {
                    trCustHistory.Attributes.Add("style", "display:none");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "SetUploadDocVisible();", true);
            }
            Page.MaintainScrollPositionOnPostBack = true;
            Page.Form.DefaultButton = imgSave.UniqueID;
            btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
            btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";


        }

        //bind customer history

        private void BindCustomerHistory()
        {
            DataSet dscustomerhistory = new DataSet();
            dscustomerhistory = CommonComponent.GetCommonDataSet("select OrderNumber,OrderDate,OrderTotal,OrderStatus,TransactionStatus  from tb_order where CustomerID=" + CustomerID + " and isnull(deleted,0)=0 order by OrderDate desc ");
            if (dscustomerhistory != null && dscustomerhistory.Tables.Count > 0 && dscustomerhistory.Tables[0].Rows.Count > 0)
            {
                grdcusthistory.DataSource = dscustomerhistory.Tables[0];
                grdcusthistory.DataBind();
            }
            else
            {
                grdcusthistory.DataSource = null;
                grdcusthistory.DataBind();
            }
        }



        /// <summary>
        /// Fill Customer Data of Security, Billing Address and Shipping Address values
        /// </summary>
        private void FillCustomerData()
        {
            objCustomer = new CustomerComponent();
            DataSet dsCustomer = new DataSet();
            dsCustomer = objCustomer.GetAdminCustomerDetailByCustID(CustomerID);

            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Email"].ToString()))
                {
                    txtEmail.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"]);
                    txtEmail.Attributes.Add("readonly", "true");
                }


                txtAltEmail.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["AlternateEmail"].ToString());
                if (Convert.ToString(dsCustomer.Tables[0].Rows[0]["Password"]) == "")
                {
                    ViewState["Password"] = null;
                    lblpassword.Text = "Password is not generated";
                }
                else
                {
                    ViewState["Password"] = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Password"]);
                    lblpassword.Text = "**********";
                }

                txtFirstName.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"]);
                txtLastName.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"]);
                lblIPAddress.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastIPAddress"]);

                if (objCustomer.CheckBlockedIPAddressByIPAddress(lblIPAddress.Text, CustomerID))
                {
                    lblIPAddress.ForeColor = System.Drawing.Color.Red;
                    btnBlockIP.AlternateText = "Unblock IP-Address";
                    btnBlockIP.ToolTip = "Unblock IP-Address";
                    btnBlockIP.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/Unblock-IP.png";
                }
                else
                {
                    lblIPAddress.ForeColor = System.Drawing.Color.Green;
                    btnBlockIP.AlternateText = "Block IP-Address";
                    btnBlockIP.ToolTip = "Block IP-Address";
                    btnBlockIP.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/Block-IP.png";
                }

                if (!String.IsNullOrEmpty(Convert.ToString(dsCustomer.Tables[0].Rows[0]["CustomerLevelID"])))
                {
                    ddlCustomerLevel.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CustomerLevelID"]);
                }
                else
                {
                    if (ddlCustomerLevel.Items.Count > 0)
                        ddlCustomerLevel.SelectedIndex = 0;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(dsCustomer.Tables[0].Rows[0]["StoreID"])))
                {
                    ddlStoreName.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["StoreID"]);
                    AppConfig.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue.ToString());
                }
                else
                {
                    if (ddlStoreName.Items.Count > 0)
                        ddlStoreName.SelectedIndex = 0;
                    AppConfig.StoreID = 1;
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["FromDate"].ToString()))
                {
                    txtFromDate.Text = Convert.ToDateTime(dsCustomer.Tables[0].Rows[0]["FromDate"].ToString()).ToShortDateString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["ToDate"].ToString()))
                {
                    txtToDate.Text = Convert.ToDateTime(dsCustomer.Tables[0].Rows[0]["ToDate"].ToString()).ToShortDateString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["DiscountPercent"].ToString()))
                {
                    txtdiscountpercent.Text = Math.Round(Convert.ToDecimal(dsCustomer.Tables[0].Rows[0]["DiscountPercent"].ToString()), 2).ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CouponCode"].ToString()))
                {
                    txtcouponcode.Text = dsCustomer.Tables[0].Rows[0]["CouponCode"].ToString().ToString();
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Active"].ToString()))
                {
                    chkActive.Checked = Convert.ToBoolean(dsCustomer.Tables[0].Rows[0]["Active"]);
                }
                else
                {
                    chkActive.Checked = false;
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["NoDiscount"].ToString()))
                {
                    chkNoDiscount.Checked = Convert.ToBoolean(dsCustomer.Tables[0].Rows[0]["NoDiscount"]);
                }
                else
                {
                    chkNoDiscount.Checked = false;
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["TradeTemplateID"].ToString()))
                {
                    ddltradetemplate.SelectedValue = dsCustomer.Tables[0].Rows[0]["TradeTemplateID"].ToString();
                    ViewState["TradeTemplateID"] = dsCustomer.Tables[0].Rows[0]["TradeTemplateID"].ToString();
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["BillingEqualShippingbit"].ToString()))
                {
                    chkSameAsBilling.Checked = Convert.ToBoolean(dsCustomer.Tables[0].Rows[0]["BillingEqualShippingbit"]);
                }
                else
                {
                    chkSameAsBilling.Checked = false;
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["isnontaxble"].ToString()))
                {
                    chknontaxable.Checked = Convert.ToBoolean(dsCustomer.Tables[0].Rows[0]["isnontaxble"]);
                }
                if (chknontaxable.Checked)
                {
                    trsellerid.Style.Remove("display");
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["SellerId"].ToString()))
                {
                    txtsellerId.Text = dsCustomer.Tables[0].Rows[0]["SellerId"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["DocFile"].ToString()))
                {
                    lblFileDocname.Text = dsCustomer.Tables[0].Rows[0]["DocFile"].ToString();
                    trDelete.Attributes.Add("Style", "display:'';vertical-align: middle");
                    btnDelete.Attributes.Add("Style", "display:'';vertical-align: middle");
                    btnDownload.Visible = true;
                    btnDownload.HRef = AppLogic.AppConfigs("Customer.DocumentPath") + CustomerID + "/" + dsCustomer.Tables[0].Rows[0]["DocFile"].ToString();
                }

                if (Convert.ToString(dsCustomer.Tables[0].Rows[0]["BillingAddressID"]) != "" && Convert.ToString(dsCustomer.Tables[0].Rows[0]["BillingAddressID"]) != "0")
                {
                    DataSet dsBillingAddress = new DataSet();
                    ViewState["BillingAddressID"] = Convert.ToString(dsCustomer.Tables[0].Rows[0]["BillingAddressID"]);
                    dsBillingAddress = objCustomer.GetAddressDetailByAddressID(Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["BillingAddressID"]));
                    if (dsBillingAddress != null && dsBillingAddress.Tables.Count > 0 && dsBillingAddress.Tables[0].Rows.Count > 0)
                    {
                        txtBillFirstname.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["FirstName"]);
                        txtBillLastname.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["LastName"]);
                        txtBillingCompany.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["Company"]);
                        txtBilladdressLine1.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["Address1"]);
                        txtBillAddressLine2.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["Address2"]);
                        txtBillsuite.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["Suite"]);
                        txtBillCity.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["City"]);

                        try
                        {
                            ddlBillcountry.ClearSelection();
                            ddlBillcountry.SelectedValue = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["Country"]);
                            ddlBillcountry_SelectedIndexChanged(null, null);
                        }
                        catch
                        {
                            ddlBillcountry.SelectedIndex = 0;
                        }
                        ddlBillstate.ClearSelection();
                        if (ddlBillstate.Items.FindByText(Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["State"])) == null)
                        {
                            if (ddlBillstate.Items.FindByText("Other") != null)
                            {
                                ddlBillstate.Items.FindByText("Other").Selected = true;
                                ClientScript.RegisterStartupScript(typeof(Page), "Statebill", "MakeBillingOtherVisible();", true);
                                txtBillingOtherState.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["State"]);
                            }
                        }
                        else
                        {
                            ddlBillstate.Items.FindByText(Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["State"])).Selected = true;
                        }
                        txtBillZipCode.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["ZipCode"]);
                        txtBillphone.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["Phone"]);
                        txtBillingFax.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["Fax"]);
                        txtBillEmail.Text = Convert.ToString(dsBillingAddress.Tables[0].Rows[0]["Email"]);
                    }

                }

                if (Convert.ToString(dsCustomer.Tables[0].Rows[0]["ShippingAddressID"]) != "" && Convert.ToString(dsCustomer.Tables[0].Rows[0]["ShippingAddressID"]) != "0")
                {
                    DataSet dsShippingAddress = new DataSet();
                    ViewState["ShippingAddressID"] = Convert.ToString(dsCustomer.Tables[0].Rows[0]["ShippingAddressID"]);
                    dsShippingAddress = objCustomer.GetAddressDetailByAddressID(Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["ShippingAddressID"]));
                    if (dsShippingAddress != null && dsShippingAddress.Tables.Count > 0 && dsShippingAddress.Tables[0].Rows.Count > 0)
                    {
                        txtShipFirstname.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["FirstName"]);
                        txtShipLastname.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["LastName"]);
                        txtShippingCompany.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["Company"]);
                        txtShipAddressLine1.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["Address1"]);
                        txtshipAddressLine2.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["Address2"]);
                        txtShipSuite.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["Suite"]);
                        txtShipCity.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["City"]);

                        ddlShipCounry.ClearSelection();
                        try
                        {
                            ddlShipCounry.SelectedValue = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["Country"]);
                            ddlShipCounry_SelectedIndexChanged(null, null);
                        }
                        catch
                        {
                            ddlShipCounry.SelectedIndex = 0;
                        }
                        ddlShipState.ClearSelection();
                        if (ddlShipState.Items.FindByText(Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["State"])) == null)
                        {
                            if (ddlShipState.Items.FindByText("Other") != null)
                            {
                                ddlShipState.Items.FindByText("Other").Selected = true;
                                ClientScript.RegisterStartupScript(typeof(Page), "State", "MakeShippingOtherVisible();", true);
                                txtShippingOtherState.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["State"]);
                            }
                        }
                        else
                        {
                            ddlShipState.Items.FindByText(Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["State"])).Selected = true;
                        }
                        txtShipZipCode.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["ZipCode"]);
                        txtShipPhone.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["Phone"]);
                        txtShippingFax.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["Fax"]);
                        txtShipEmailAddress.Text = Convert.ToString(dsShippingAddress.Tables[0].Rows[0]["Email"]);
                    }
                }
            }
        }

        /// <summary>
        /// Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {

            if (lblpassword.Text.ToString().Trim().IndexOf("*") <= -1)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Passwordproblem", "$(document).ready( function() {jAlert('Please generate password first.', 'Message');});", true);
                return;
            }

            objCustomer = new CustomerComponent();
            DataSet dsCustomer = new DataSet();
            dsCustomer = objCustomer.GetAdminCustomerDetailByCustID(CustomerID);
            int CustID = 0;
            int BillingAddressID = 0;
            int ShippingAddressID = 0;
            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                CustID = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["CustomerID"]);
                BillingAddressID = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["BillingAddressID"]);
                ShippingAddressID = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["ShippingAddressID"]);
            }
            if (chkUploadDoc.Checked)
            {
                if (ViewState["DocumentFileName"] == null || ViewState["DocumentFileName"].ToString() == "")
                {
                    string Msg = "Please select file to Upload";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Msg", "$(document).ready( function() {jAlert('" + Msg.ToString() + "', 'Message','ContentPlaceHolder1_fuUplodDoc');});", true);
                    return;
                }
                if (fuUplodDoc.FileName != null && fuUplodDoc.FileName != "" && fuUplodDoc.Visible == true)
                {
                    string Msg = "Please select file to Upload";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Msg", "$(document).ready( function() {jAlert('" + Msg.ToString() + "', 'Message','ContentPlaceHolder1_fuUplodDoc');});", true);
                    return;
                }
            }
            CommonComponent.ExecuteCommonData("Delete from tb_MembershipDiscount where CustID=" + CustID + " and storeid=" + Convert.ToInt32(ddlStoreName.SelectedValue.ToString()) + "");

            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit")
            {
                int Customerupdated = 0;
                divCustDetail.Visible = true;
                Customerupdated = UpdateCustomerData();
                SaveCustomerDocFile(CustomerID);
                if (Customerupdated == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailUpdatew", "$(document).ready( function() {jAlert('Customer already exist with same email address.', 'Message');});", true);
                    return;
                }
                else if (Customerupdated == 1)
                {
                    if (ViewState["IsPasswordChanged"] != null && ViewState["IsPasswordChanged"].ToString() == "1")
                    {
                        try
                        {
                            SendMail();
                        }
                        catch { }
                    }

                    if (UpdateBillingAddress(BillingAddressID, CustID) && UpdateShippingAddress(ShippingAddressID, CustID))
                    {
                        int templateid = 0;
                        templateid = Convert.ToInt32(ddltradetemplate.SelectedValue.ToString());

                        CommonComponent.ExecuteCommonData("update tb_customer set TradeTemplateID=" + templateid + " where CustomerID=" + CustID + "");
                        //

                        //InsertMemberShipDisCount(CustID);

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Updateq", "$(document).ready( function() {jAlert('Customer Inserted Successfully.', 'Message');});", true);
                        if (Request.QueryString["rtn"] != null && Request.QueryString["rtn"].ToString() != "")
                        {
                            Response.Redirect("/Admin/Orders/Orders.aspx?id=" + Request.QueryString["rtn"].ToString() + "", true);
                        }
                        else if (Request.QueryString["Quoted"] != null)
                        {
                            if (Request.QueryString["Quoted"].ToString().Trim() == "0")
                                Response.Redirect("CustomerQuote.aspx?CustID=" + CustID);
                            else Response.Redirect("CustomerQuote.aspx?CustID=" + CustID + "Mode=edit&&ID=" + Request.QueryString["Quoted"].ToString().Trim());
                        }
                        else
                        {
                            Response.Redirect("CustomerList.aspx?status=updated", true);
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailUpdate", "$(document).ready( function() {jAlert('Problem in inserting billing and shipping address, please try again.', 'Message');});", true);
                        return;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailUpdate1", "$(document).ready( function() {jAlert('Problem in inserting customer, please try again.', 'Message');});", true);
                    return;
                }
            }
            else
            {
                int CustomerAdded = 0;
                CustomerAdded = InsertCustomerData();

                SaveCustomerDocFile(CustomerAdded);

                if (CustomerAdded == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert", "$(document).ready( function() {jAlert('Customer already exist with same email address.', 'Message');});", true);
                    return;
                }
                if (CustomerAdded > 0)
                {
                    if (ViewState["IsPasswordChanged"] != null && ViewState["IsPasswordChanged"].ToString() == "1")
                    {
                        try
                        {
                            SendMail();
                        }
                        catch { }
                    }

                    if (InsertBillingAddress(0, CustomerAdded) && InsertShippingAddress(0, CustomerAdded))
                    {
                        int templateid = 0;
                        templateid = Convert.ToInt32(ddltradetemplate.SelectedValue.ToString());

                        CommonComponent.ExecuteCommonData("update tb_customer set TradeTemplateID=" + templateid + " where CustomerID=" + CustID + "");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Insert", "$(document).ready( function() {jAlert('Customer Inserted Successfully.', 'Message');});", true);
                        if (Request.QueryString["rtn"] != null && Request.QueryString["rtn"].ToString() != "")
                        {
                            Response.Redirect("/Admin/Orders/Orders.aspx?id=" + Request.QueryString["rtn"].ToString() + "", true);
                        }
                        else if (Request.QueryString["Quoted"] != null)
                        {
                            if (Request.QueryString["Quoted"].ToString().Trim() == "0")
                                Response.Redirect("CustomerQuote.aspx?CustID=" + CustomerAdded);
                            else Response.Redirect("CustomerQuote.aspx?CustID=" + CustomerAdded + "Mode=edit&&ID=" + Request.QueryString["Quoted"].ToString().Trim());
                        }
                        else
                        {
                            Response.Redirect("CustomerList.aspx?status=inserted", true);
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert1", "$(document).ready( function() {jAlert('Problem in inserting billing and shipping address, please try again.', 'Message');});", true);
                        return;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert2", "$(document).ready( function() {jAlert('Problem in inserting customer, please try again.', 'Message');});", true);
                    return;
                }
            }
        }

        /// <summary>
        /// Save Customer Doc File
        /// </summary>
        /// <param name="CustomerID"></param>
        private void SaveCustomerDocFile(int CustomerID)
        {

            string Sourcetemppath = AppLogic.AppConfigs("Customer.DocumentPathTemp").ToString();
            string targettemppath = AppLogic.AppConfigs("Customer.DocumentPath").ToString() + CustomerID + "/";
            string sourcePath = @Server.MapPath(Sourcetemppath);
            string targetPath = @Server.MapPath(targettemppath);

            string srcfileName = "";
            if (ViewState["DocumentFileName"] != null)
            {
                srcfileName = ViewState["DocumentFileName"].ToString();
            }
            if (srcfileName != "")
            {
                string sourceFile = System.IO.Path.Combine(sourcePath, srcfileName);
                string destFile = System.IO.Path.Combine(targetPath, srcfileName);

                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }

                if (File.Exists(sourceFile))
                {
                    System.IO.File.Copy(sourceFile, destFile, true);
                }
                CommonComponent.ExecuteCommonData("update tb_customer set DocFile ='" + srcfileName + "'  where CustomerID =" + CustomerID);
            }
        }

        /// <summary>
        /// Cancel button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["rtn"] != null && Request.QueryString["rtn"].ToString() != "")
            {
                Response.Redirect("/Admin/Orders/Orders.aspx?id=" + Request.QueryString["rtn"].ToString() + "", true);
            }
            else if (Request.QueryString["Quoted"] != null)
            {
                if (Request.QueryString["Quoted"].Trim() == "0")
                    Response.Redirect("CustomerQuote.aspx");
                else Response.Redirect("CustomerQuote.aspx?Mode=edit&ID=" + Request.QueryString["Quoted"].Trim());
            }
            else
            {
                Response.Redirect("CustomerList.aspx", true);
            }

            Session["TempDsCatDiscount"] = null;
            Session["TempDsProdDiscount"] = null;


        }

        /// <summary>
        /// Insert Customer Data function
        /// </summary>
        /// <returns>Returns Identity value if customer inserted</returns>
        private Int32 InsertCustomerData()
        {
            int AddCustomerID = 0;
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Customer tb_Customer = new tb_Customer();
            tb_Customer.Email = txtEmail.Text;
            tb_Customer.Password = Convert.ToString(ViewState["Password"]);
            tb_Customer.FirstName = txtFirstName.Text;
            tb_Customer.LastName = txtLastName.Text;
            tb_Customer.CustomerLevelID = Convert.ToInt32(ddlCustomerLevel.SelectedValue.ToString());
            if (chkSameAsBilling.Checked)
            {
                tb_Customer.BillingEqualShippingbit = true;
            }
            else
            {
                tb_Customer.BillingEqualShippingbit = false;
            }

            tb_Customer.IsLockedOut = false;
            tb_Customer.FailedPasswordAttemptCount = 0;
            tb_Customer.LastIPAddress = Request.UserHostAddress.ToString();
            tb_Customer.Deleted = false;
            tb_Customer.Active = Convert.ToBoolean(chkActive.Checked);
            if (chkActive.Checked)
            {
                tb_Customer.IsRegistered = true;
            }
            else
            {
                tb_Customer.IsRegistered = false;
            }

            tb_Customer.NoDiscount = Convert.ToBoolean(chkNoDiscount.Checked);
            if (chkNoDiscount.Checked)
            {
                tb_Customer.NoDiscount = true;
            }
            else
            {
                tb_Customer.NoDiscount = false;
            }
            tb_Customer.CreatedOn = DateTime.Now;
            tb_Customer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreId")));

            if (!string.IsNullOrEmpty(txtcouponcode.Text.Trim().ToString()))
                tb_Customer.CouponCode = txtcouponcode.Text;
            else tb_Customer.CouponCode = null;

            if (!String.IsNullOrEmpty(txtFromDate.Text))
            {
                try
                {
                    tb_Customer.FromDate = Convert.ToDateTime(txtFromDate.Text);
                }
                catch { }
            }
            else
                tb_Customer.FromDate = null;

            if (!String.IsNullOrEmpty(txtToDate.Text))
            {
                try
                {
                    tb_Customer.ToDate = Convert.ToDateTime(txtToDate.Text);
                }
                catch { }
            }
            else
                tb_Customer.ToDate = null;

            if (!String.IsNullOrEmpty(txtdiscountpercent.Text))
                tb_Customer.DiscountPercent = Convert.ToDecimal(txtdiscountpercent.Text);

            AddCustomerID = objCustomer.InsertCustomerForAdmin(tb_Customer, Convert.ToInt32(ddlStoreName.SelectedValue));
            InsertMemberShipDisCount(AddCustomerID);
            Int32 Ic = 0;
            if (chknontaxable.Checked)
            {
                Ic = 1;
            }

            CommonComponent.ExecuteCommonData("UPDATE tb_Customer SET isnontaxble=" + Ic + ",SellerId='"+ txtsellerId.Text.ToString().Replace("'","''")+"' WHERE CustomerID=" + AddCustomerID + "");
            CommonComponent.ExecuteCommonData("update tb_Customer set AlternateEmail='" + txtAltEmail.Text.ToString().Replace("'", "''") + "' where CustomerID=" + AddCustomerID);
            return AddCustomerID;
        }

        /// <summary>
        /// Update Customer Data function
        /// </summary>
        /// <returns>Returns 1 = if customer inserted</returns>
        private Int32 UpdateCustomerData()
        {
            int UpdateCustomerID = 0;
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Customer tb_Customer = new tb_Customer();
            tb_Customer.CustomerID = Convert.ToInt32(CustomerID);
            tb_Customer.Email = txtEmail.Text;
            tb_Customer.Password = Convert.ToString(ViewState["Password"]);
            tb_Customer.FirstName = txtFirstName.Text;
            tb_Customer.LastName = txtLastName.Text;
            tb_Customer.CustomerLevelID = Convert.ToInt32(ddlCustomerLevel.SelectedValue.ToString());
            if (chkSameAsBilling.Checked)
            {
                tb_Customer.BillingEqualShippingbit = true;
            }
            else
            {
                tb_Customer.BillingEqualShippingbit = false;
            }
            tb_Customer.CustomerLevelID = Convert.ToInt32(ddlCustomerLevel.SelectedValue.ToString());
            tb_Customer.LastIPAddress = Request.UserHostAddress.ToString();
            tb_Customer.Active = Convert.ToBoolean(chkActive.Checked);
            if (chkActive.Checked)
            {
                //tb_Customer.Deleted = false;
                tb_Customer.IsRegistered = true;
            }
            else
            {
                //tb_Customer.Deleted = true;
                tb_Customer.IsRegistered = false;
            }
            tb_Customer.UpdatedOn = DateTime.Now;
            try
            {
                tb_Customer.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
            }
            catch
            {
                tb_Customer.UpdatedBy = 0;
            }

            tb_Customer.NoDiscount = Convert.ToBoolean(chkNoDiscount.Checked);
            if (chkNoDiscount.Checked)
            {
                tb_Customer.NoDiscount = true;
            }
            else
            {
                tb_Customer.NoDiscount = false;
            }

            if (!string.IsNullOrEmpty(txtcouponcode.Text.Trim().ToString()))
                tb_Customer.CouponCode = txtcouponcode.Text;
            else tb_Customer.CouponCode = null;

            if (!String.IsNullOrEmpty(txtFromDate.Text))
            {
                try
                {
                    tb_Customer.FromDate = Convert.ToDateTime(txtFromDate.Text);
                }
                catch { }
            }
            else
                tb_Customer.FromDate = null;

            if (!String.IsNullOrEmpty(txtToDate.Text))
            {
                try
                {
                    tb_Customer.ToDate = Convert.ToDateTime(txtToDate.Text);
                }
                catch { }
            }
            else
                tb_Customer.ToDate = null;

            if (!String.IsNullOrEmpty(txtdiscountpercent.Text))
                tb_Customer.DiscountPercent = Convert.ToDecimal(txtdiscountpercent.Text);

            tb_Customer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreId")));
            UpdateCustomerID = objCustomer.UpdateCustomerAdmin(tb_Customer, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            Int32 Ic = 0;
            if (chknontaxable.Checked)
            {
                Ic = 1;
            }

            CommonComponent.ExecuteCommonData("UPDATE tb_Customer SET isnontaxble=" + Ic + ",SellerId='" + txtsellerId.Text.ToString().Replace("'", "''") + "' WHERE CustomerID=" + CustomerID + "");
            UpdateMemberShipDisCount(CustomerID);
            CommonComponent.ExecuteCommonData("update tb_Customer set AlternateEmail='" + txtAltEmail.Text.ToString().Replace("'", "''") + "' where CustomerID=" + CustomerID);
            return UpdateCustomerID;
        }

        /// <summary>
        /// function for setting Insert billing Address
        /// </summary>
        /// <param name="AddressID">int AddressID</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns True if Billing data inserted</returns>
        protected bool InsertBillingAddress(Int32 AddressID, Int32 CustomerID)
        {
            bool IsBillingAddressAdded = false;

            objCustomer = new CustomerComponent();
            tb_Address tb_Address = new tb_Address();
            tb_Address = SetBillingValue(AddressID, CustomerID);

            int AddID = objCustomer.InsertCustomerAddress(tb_Address, Convert.ToInt32(ddlStoreName.SelectedValue.ToString()));
            if (AddID > 0)
            {
                IsBillingAddressAdded = objCustomer.UpdateCustomerBillingAddress(CustomerID, AddID);
            }

            return IsBillingAddressAdded;
        }

        /// <summary>
        /// function for setting Insert shipping Address
        /// </summary>
        /// <param name="AddressID">int AddressID</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns True if Shipping data inserted</returns>
        protected bool InsertShippingAddress(Int32 AddressID, Int32 CustomerID)
        {
            bool IsShippningAddressAdded = false;
            objCustomer = new CustomerComponent();
            tb_Address tb_Address = new tb_Address();
            tb_Address = SetShippingValue(AddressID, CustomerID);

            int AddID = objCustomer.InsertCustomerAddress(tb_Address, Convert.ToInt32(ddlStoreName.SelectedValue.ToString()));
            if (AddID > 0)
            {
                IsShippningAddressAdded = objCustomer.UpdateCustomerShippingAddress(CustomerID, AddID);
            }
            return IsShippningAddressAdded;
        }

        /// <summary>
        /// function for setting Update billing Address
        /// </summary>
        /// <param name="AddressID">int AddressID</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns True if Billing data Updated</returns>
        protected bool UpdateBillingAddress(Int32 AddressID, Int32 CustomerID)
        {
            bool IsBillingAddressUpdate = false;
            objCustomer = new CustomerComponent();
            tb_Address tb_Address = new tb_Address();
            tb_Address = SetBillingValue(AddressID, CustomerID);

            int UpdateID = objCustomer.UpdateCustomerAddress(tb_Address, Convert.ToInt32(ddlStoreName.SelectedValue.ToString()));
            if (UpdateID > 0)
            {
                IsBillingAddressUpdate = true;
            }
            return IsBillingAddressUpdate;
        }

        /// <summary>
        /// function for setting Update shipping Address
        /// </summary>
        /// <param name="AddressID">int AddressID</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns a Address table which contains shipping data to be update</returns>
        protected bool UpdateShippingAddress(Int32 AddressID, Int32 CustomerID)
        {
            bool IsShippingAddressUpdate = false;
            objCustomer = new CustomerComponent();
            tb_Address tb_Address = new tb_Address();
            tb_Address = SetShippingValue(AddressID, CustomerID);

            int UpdateID = objCustomer.UpdateCustomerAddress(tb_Address, Convert.ToInt32(ddlStoreName.SelectedValue.ToString()));
            if (UpdateID > 0)
            {
                IsShippingAddressUpdate = true;
            }
            return IsShippingAddressUpdate;
        }

        /// <summary>
        /// Set Billing Address Value
        /// </summary>
        /// <param name="AddressID">Int32 AddressID</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
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
                divCustDetail.Visible = true;
                tb_Address.PaymentMethodIDLastUsed = "";
                tb_Address.CreatedOn = DateTime.Now.Date;
                tb_Address.Deleted = false;
            }
            return tb_Address;
        }

        /// <summary>
        /// Set Shipping Address Value
        /// </summary>
        /// <param name="AddressID">Int32 AddressID</param>
        /// <returns>Returns tb_Address Table Object</returns>
        public tb_Address SetShippingValue(Int32 AddressID, Int32 CustomerID)
        {
            tb_Address tb_Address = new tb_Address();
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Address.CustomerID = CustomerID;

            if (chkSameAsBilling.Checked)
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
                divCustDetail.Visible = true;
                tb_Address.PaymentMethodIDLastUsed = "";
                tb_Address.CreatedOn = DateTime.Now.Date;
                tb_Address.Deleted = false;
            }
            return tb_Address;
        }

        /// <summary>
        /// Bind both Country Dropdown list
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
                    ddlBillstate.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));

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

            if (ddlShipState.SelectedValue.ToString() == "-11")
            {
                ClientScript.RegisterStartupScript(typeof(Page), "ShippingVisible", "MakeShippingOtherVisible();", true);
            }
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
                    ddlShipState.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
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

            if (ddlBillstate.SelectedValue.ToString() == "-11")
            {
                ClientScript.RegisterStartupScript(typeof(Page), "BillingVisible", "MakeBillingOtherVisible();", true);
            }
        }

        /// <summary>
        /// Bind store dropdown
        /// </summary>
        private void bindstore()
        {
            objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail != null && storeDetail.Count > 0)
            {
                ddlStoreName.DataSource = storeDetail;
                ddlStoreName.DataTextField = "StoreName";
                ddlStoreName.DataValueField = "StoreID";
                ddlStoreName.DataBind();
            }
            ddlStoreName.SelectedIndex = 0;
        }

        /// <summary>
        /// Fill Customer Level in Dropdown
        /// </summary>
        private void FillCustomerLevel()
        {
            ddlCustomerLevel.Items.Clear();
            CustomerLevelComponent objCustomerLevel = new CustomerLevelComponent();
            var lstcustomerLevel = objCustomerLevel.GetAllCustomerLevel(Convert.ToInt32(ddlStoreName.SelectedValue.ToString()));
            if (lstcustomerLevel != null && lstcustomerLevel.Count > 0)
            {
                ddlCustomerLevel.DataSource = lstcustomerLevel;
                ddlCustomerLevel.DataTextField = "Name";
                ddlCustomerLevel.DataValueField = "CustomerLevelID";
                ddlCustomerLevel.DataBind();
            }
            ddlCustomerLevel.Items.Insert(0, new ListItem("Select Customer Level", "0"));
            ddlCustomerLevel.SelectedIndex = 0;
        }

        /// <summary>
        ///  Password Link Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkpassword_Click(object sender, EventArgs e)
        {
            ViewState["IsPasswordChanged"] = "1";
            ViewState["Password"] = SecurityComponent.Encrypt(GenerateRandomCode().ToString());
            lblpassword.Text = "**********";

            if (ddlBillstate.SelectedValue.ToString() == "-11")
            {
                ClientScript.RegisterStartupScript(typeof(Page), "BillingVisible", "MakeBillingOtherVisible();", true);
            }

            if (ddlShipState.SelectedValue.ToString() == "-11")
            {
                ClientScript.RegisterStartupScript(typeof(Page), "ShippingVisible", "MakeShippingOtherVisible();", true);
            }
        }

        /// <summary>
        /// Generate random password
        /// </summary>
        /// <returns>Returns the Randomized Generated Password </returns>
        private string GenerateRandomCode()
        {
            Random random = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStoreName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCustomerLevel();
            AppConfig.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue.ToString());
        }

        /// <summary>
        ///  Block IP Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnBlockIP_Click(object sender, ImageClickEventArgs e)
        {
            objCustomer = new CustomerComponent();

            if (objCustomer.CheckBlockedIPAddressByIPAddress(lblIPAddress.Text, CustomerID))
            {
                int IsDeleted = objCustomer.DeleteBlockIPAddress(Convert.ToInt32(ddlStoreName.SelectedValue.ToString()), CustomerID, lblIPAddress.Text.ToString());

                if (IsDeleted == 1)
                {
                    lblIPAddress.ForeColor = System.Drawing.Color.Green;
                    btnBlockIP.AlternateText = "Block IP-Address";
                    btnBlockIP.ToolTip = "Block IP-Address";
                    btnBlockIP.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/Block-IP.png";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('IP-Address unblocked successfully.', 'Message');});", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while unblocking IP-Address.', 'Message');});", true);
                    return;
                }
            }
            else
            {
                int IsAdded = objCustomer.InsertBlockIPAddress(Convert.ToInt32(ddlStoreName.SelectedValue.ToString()), CustomerID, lblIPAddress.Text.ToString());

                if (IsAdded > 0)
                {
                    lblIPAddress.ForeColor = System.Drawing.Color.Red;
                    btnBlockIP.AlternateText = "Unblock IP-Address";
                    btnBlockIP.ToolTip = "Unblock IP-Address";
                    btnBlockIP.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/Unblock-IP.png";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('IP-Address blocked successfully.', 'Message');});", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while blocking IP-Address.', 'Message');});", true);
                    return;
                }
            }
        }

        /// <summary>
        /// Send Mail function
        /// </summary>
        private void SendMail()
        {
            objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();

            dsMailTemplate = objCustomer.GetEmailTamplate("Resetpassword", 1);

            DataSet dsTopic = new DataSet();
            dsTopic = TopicComponent.GetTopicList("InvoiceSignature", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {

                //String strBody = "";
                //String strSubject = "";
                //strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                //strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                //strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                //strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                //strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                //strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                //strBody = Regex.Replace(strBody, "###FIRSTNAME###", Convert.ToString(txtFirstName.Text), RegexOptions.IgnoreCase);

                //strBody = Regex.Replace(strBody, "###LASTNAME###", Convert.ToString(txtLastName.Text), RegexOptions.IgnoreCase);

                //strBody = Regex.Replace(strBody, "###USERNAME###", Convert.ToString(txtEmail.Text.ToString()), RegexOptions.IgnoreCase);

                //strBody = Regex.Replace(strBody, "###PASSWORD###", SecurityComponent.Decrypt(Convert.ToString(ViewState["Password"])), RegexOptions.IgnoreCase);
                //strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);

                //strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                //if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                //{
                //    strBody = Regex.Replace(strBody, "###SIGNATURE###", Convert.ToString(dsTopic.Tables[0].Rows[0]["Description"]), RegexOptions.IgnoreCase);
                //}
                //else
                //{
                //    strBody = Regex.Replace(strBody, "###SIGNATURE###", "Thank You", RegexOptions.IgnoreCase);
                //}


                string strRandom = string.Empty;
                Random rm = new Random();
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefgijklmnopqrstuvwxyz0123456789";
                strRandom = new string(Enumerable.Repeat(chars, 10).Select(s => s[rm.Next(s.Length)]).ToArray());
                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = Regex.Replace(dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString(), "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                strSubject = Regex.Replace(strSubject, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("Live_Contant_Server").ToString(), RegexOptions.IgnoreCase);


                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LINK###", "https://www.halfpricedrapes.com/Home/ResetPasswordhome?Token=" + strRandom + "&Text=Create Password", RegexOptions.IgnoreCase);
                //  strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###FirstName###", txtFirstName.Text.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###Unsubscribelink###", "https://www.halfpricedrapes.com/home/Unsubscribe?email=" + txtEmail.Text.ToString() + "", RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###contactfooterdetail###", "", RegexOptions.IgnoreCase);

                CommonComponent.ExecuteCommonData("update tb_customer SET Token='" + strRandom + "' WHERE Email='" + txtEmail.Text.ToString() + "' and storeId=1 and isnull(Active,0)=1 and isnull(IsRegistered,0)=1 and isnull(Deleted,0)=0");
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMail(txtEmail.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }

        /// <summary>
        ///  Same As Billing check box checked Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void chkSameAsBilling_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSameAsBilling.Checked)
            {
                txtShipFirstname.Text = txtBillFirstname.Text;
                txtShipLastname.Text = txtBillLastname.Text;
                txtShippingCompany.Text = txtBillingCompany.Text;
                txtShipAddressLine1.Text = txtBilladdressLine1.Text;
                txtshipAddressLine2.Text = txtBillAddressLine2.Text;
                txtShipSuite.Text = txtBillsuite.Text;
                txtShipCity.Text = txtBillCity.Text;
                ddlShipCounry.SelectedValue = ddlBillcountry.SelectedValue;
                ddlShipCounry_SelectedIndexChanged(null, null);
                ddlShipState.SelectedValue = ddlBillstate.SelectedValue;
                if (ddlShipState.SelectedValue == "-11")
                {
                    ClientScript.RegisterStartupScript(typeof(Page), "ShippingVisible12", "SetShippingOtherVisible(true);", true);
                    txtShippingOtherState.Text = txtBillingOtherState.Text;
                }
                else
                {
                    ddlShipState.SelectedValue = ddlBillstate.SelectedValue;
                }

                txtShipZipCode.Text = txtBillZipCode.Text;
                txtShipPhone.Text = txtBillphone.Text;
                txtShippingFax.Text = txtBillingFax.Text;
                txtShipEmailAddress.Text = txtBillEmail.Text;

            }
            else
            {
                ClearShippingInformation();
            }
        }

        /// <summary>
        /// Clear Shipping Information
        /// </summary>
        public void ClearShippingInformation()
        {
            txtShipFirstname.Text = "";
            txtShipLastname.Text = "";
            txtShippingCompany.Text = "";
            txtShipAddressLine1.Text = "";
            txtshipAddressLine2.Text = "";
            txtShipSuite.Text = "";
            txtShipCity.Text = "";
            ddlShipCounry.SelectedIndex = 0;
            ddlShipCounry_SelectedIndexChanged(null, null);
            txtShipZipCode.Text = "";
            txtShipPhone.Text = "";
            txtShippingFax.Text = "";
            txtShipEmailAddress.Text = "";
        }

        /// <summary>
        ///  Upload Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            string CustDocPathTemp = AppLogic.AppConfigs("Customer.DocumentPathTemp");
            if (!Directory.Exists(Server.MapPath(CustDocPathTemp)))
                Directory.CreateDirectory(Server.MapPath(CustDocPathTemp));

            btnDownload.Visible = false;
            if (fuUplodDoc.FileName.Length > 0)
            {
                ViewState["DocumentFileName"] = fuUplodDoc.FileName.ToString();
                fuUplodDoc.SaveAs(Server.MapPath(CustDocPathTemp) + fuUplodDoc.FileName);
                lblFileDocname.Text = "[ " + fuUplodDoc.FileName.ToString() + " ]";
                btnDelete.Attributes.Add("style", "display:''; vertical-align: middle");
                trDelete.Attributes.Add("style", "display:''; vertical-align: middle");
                chkUploadDoc.Checked = false;
            }
            else
            {
                string Msg = "Please select file to Upload";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "UplMsg", "$(document).ready( function() {jAlert('" + Msg.ToString() + "', 'Message','ContentPlaceHolder1_fuUplodDoc');});", true);
            }
        }

        /// <summary>
        /// Delete document function
        /// </summary>
        /// <param name="CustomerDoc">string CustomerDoc</param>
        private void DeleteDocument(string CustomerDoc)
        {
            try
            {
                string docPath = AppLogic.AppConfigs("Customer.DocumentPath").ToString() + CustomerID + "/" + CustomerDoc;
                if (File.Exists(Server.MapPath(docPath)))
                    File.Delete(Server.MapPath(docPath));

            }
            catch (Exception ex)
            {

            }
        }

        protected void BindCategoryDetails(Int32 CustomerLevelID, Int32 StoreId)
        {
            CustomerComponent objCustomer = new CustomerComponent();
            if (hdnCatWiseDiscountids != null && hdnCatWiseDiscountids.Value != "")
            {
                dsCategory = CommonComponent.GetCommonDataSet("SELECT NAME FROM tb_category WHERE CategoryID IN ('" + hdnCatWiseDiscountids.Value + "')");
            }
            //dsCategory = objCustomer.GetMembershipDetails(CustomerLevelID, StoreId, "category", 2);
            //if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            //{
            //    grdCategory.DataSource = dsCategory;
            //    grdCategory.DataBind();
            //    DiscountCategoryStructure = true;
            //}
            //else
            //{
            //    DiscountCategoryStructure = false;
            //    grdCategory.DataSource = null;
            //    grdCategory.DataBind();
            //}
        }
        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CommonComponent.ExecuteCommonData("update tb_customer set DocFile =''  where CustomerID =" + CustomerID);
                DeleteDocument(lblFileDocname.Text);
                lblFileDocname.Text = "";
                btnDelete.Attributes.Add("Style", "display:none");
                trDelete.Attributes.Add("Style", "display:none");
                btnDownload.Visible = false;
            }
            catch
            {
            }
        }





        protected void grdProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProduct.PageIndex = e.NewPageIndex;
            BindProductDiscountDeatail();

        }
        protected void grdProduct_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void grdProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit Percent")
            {
                foreach (GridViewRow gr in grdProduct.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    if (lblMembershipDiscountID.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        txtPercent.Text = lblPercent.Text.ToString();
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnEdit.Visible = false;
                        lblPercent.Visible = false;
                        txtPercent.Visible = true;
                    }
                }
            }
            if (e.CommandName == "Stop")
            {
                foreach (GridViewRow gr in grdProduct.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    if (lblMembershipDiscountID.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        btnEdit.Visible = true;
                        lblPercent.Visible = true;
                        txtPercent.Visible = false;
                    }
                }
            }
            if (e.CommandName == "Delete")
            {
                string ProductId = e.CommandArgument.ToString();
                foreach (GridViewRow gr in grdProduct.Rows)
                {
                    Label lblProductId = (Label)gr.FindControl("lblProductId");

                    if (lblProductId.Text == e.CommandArgument.ToString())
                    {
                        if (Session["TempDsProdDiscount"] != null)
                        {
                            DataTable dtProduct;
                            dtProduct = (DataTable)Session["TempDsProdDiscount"];
                            DataRow[] dr;
                            dr = dtProduct.Select("ProductId='" + ProductId + "'");
                            foreach (DataRow AssingDr in dr)
                            {
                                dtProduct.Rows.Remove(AssingDr);
                            }
                            dtProduct.AcceptChanges();
                            Session["TempDsProdDiscount"] = dtProduct;
                            grdProduct.DataSource = dtProduct;
                            grdProduct.DataBind();
                            CommonComponent.ExecuteCommonData("Delete from tb_MembershipDiscount  where DiscountObjectID=" + lblProductId.Text.ToString() + "");
                        }
                    }
                }

            }
            if (e.CommandName == "Add")
            {
                string ProductId = e.CommandArgument.ToString();
                foreach (GridViewRow gr in grdProduct.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    Label lblProductId = (Label)gr.FindControl("lblProductId");
                    if (lblProductId.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        Label lblEmail = (Label)gr.FindControl("lblEmailID");
                        Label lblName = (Label)gr.FindControl("lblName");

                        if (txtPercent.Text != "")
                        {
                            decimal DiscountPerc = 0;
                            try
                            {
                                DiscountPerc = Convert.ToDecimal(txtPercent.Text.Replace("%", ""));
                                if (Session["TempDsProdDiscount"] != null)
                                {
                                    DataTable dtproduct;
                                    dtproduct = (DataTable)Session["TempDsProdDiscount"];
                                    DataRow[] dr;
                                    dr = dtproduct.Select("ProductId='" + ProductId + "'");

                                    dr[0]["ProductDiscount"] = DiscountPerc;

                                    dtproduct.AcceptChanges();
                                    Session["TempDsProdDiscount"] = dtproduct;
                                    grdProduct.DataSource = dtproduct;
                                    grdProduct.DataBind();

                                }
                                CommonComponent.ExecuteCommonData("Update tb_MembershipDiscount set Discount=" + DiscountPerc + " where DiscountObjectID=" + lblProductId.Text.ToString() + "");
                            }
                            catch
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter valid Discount ...', 'Message');});", true);
                                return;
                            }

                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            btnEdit.Visible = true;
                            lblPercent.Visible = true;
                            txtPercent.Visible = false;

                            // BindProductDetails(Convert.ToInt32(Request.QueryString["CustomerLevelID"].ToString()), Convert.ToInt32(ddlStoreName.SelectedValue));
                        }
                    }
                }
            }
        }


        protected void grdCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCategory.PageIndex = e.NewPageIndex;
            BindCategoryDiscountDeatail();

            //BindCategoryDetails(Convert.ToInt32(Request.QueryString["CustomerLevelID"].ToString()), Convert.ToInt32(ddlStoreName.SelectedValue));
            // grdCategory.DataBind();
        }
        protected void grdCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void grdCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit Percent")
            {
                foreach (GridViewRow gr in grdCategory.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    if (lblMembershipDiscountID.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        txtPercent.Text = lblPercent.Text.ToString();
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnEdit.Visible = false;
                        lblPercent.Visible = false;
                        txtPercent.Visible = true;
                    }
                }
            }
            if (e.CommandName == "Stop")
            {
                foreach (GridViewRow gr in grdCategory.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    if (lblMembershipDiscountID.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        btnEdit.Visible = true;
                        lblPercent.Visible = true;
                        txtPercent.Visible = false;
                    }
                }
            }

            if (e.CommandName == "Delete")
            {
                string CategoryId = e.CommandArgument.ToString();
                foreach (GridViewRow gr in grdCategory.Rows)
                {
                    Label lblCategoryID = (Label)gr.FindControl("lblCategoryID");

                    if (lblCategoryID.Text == e.CommandArgument.ToString())
                    {
                        if (Session["TempDsCatDiscount"] != null)
                        {
                            DataTable dtcategory;
                            dtcategory = (DataTable)Session["TempDsCatDiscount"];
                            DataRow[] dr;
                            dr = dtcategory.Select("CategoryId='" + CategoryId + "'");
                            foreach (DataRow AssingDr in dr)
                            {
                                dtcategory.Rows.Remove(AssingDr);
                            }
                            dtcategory.AcceptChanges();
                            Session["TempDsCatDiscount"] = dtcategory;
                            grdCategory.DataSource = dtcategory;
                            grdCategory.DataBind();
                            CommonComponent.ExecuteCommonData("Delete from tb_MembershipDiscount  where DiscountObjectID=" + lblCategoryID.Text.ToString() + "");
                        }
                    }
                }

            }
            if (e.CommandName == "Add")
            {

                string CategoryId = e.CommandArgument.ToString();
                foreach (GridViewRow gr in grdCategory.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    Label lblCategoryID = (Label)gr.FindControl("lblCategoryID");

                    if (lblCategoryID.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        Label lblEmail = (Label)gr.FindControl("lblEmailID");
                        Label lblName = (Label)gr.FindControl("lblName");

                        if (txtPercent.Text != "")
                        {
                            decimal DiscountPerc = 0;
                            try
                            {
                                DiscountPerc = Convert.ToDecimal(txtPercent.Text.Replace("%", ""));
                                if (Session["TempDsCatDiscount"] != null)
                                {
                                    DataTable dtcategory;
                                    dtcategory = (DataTable)Session["TempDsCatDiscount"];
                                    DataRow[] dr;
                                    dr = dtcategory.Select("CategoryId='" + CategoryId + "'");

                                    dr[0]["CategoryDiscount"] = DiscountPerc;

                                    dtcategory.AcceptChanges();
                                    Session["TempDsCatDiscount"] = dtcategory;
                                    grdCategory.DataSource = dtcategory;
                                    grdCategory.DataBind();

                                }

                                CommonComponent.ExecuteCommonData("Update tb_MembershipDiscount set Discount=" + DiscountPerc + " where DiscountObjectID=" + lblCategoryID.Text.ToString() + "");

                            }
                            catch
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter valid Discount ...', 'Message');});", true);
                                return;
                            }

                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            btnEdit.Visible = true;
                            lblPercent.Visible = true;
                            txtPercent.Visible = false;

                            //  BindCategoryDetails(Convert.ToInt32(Request.QueryString["CustomerLevelID"].ToString()), Convert.ToInt32(ddlStoreName.SelectedValue));
                        }
                    }
                }
                //  BinCategoryDiscountDeatailbyCustID();
            }
        }

        private void InsertMemberShipDisCount(int custid)
        {
            bool result = false;
            #region Insert for Category Discount Details
            if (Session["TempDsCatDiscount"] != null)
            {
                DataTable dt1;
                tb_MembershipDiscount tb_MembershipDiscount = new tb_MembershipDiscount();
                dt1 = (DataTable)Session["TempDsCatDiscount"];
                if (dt1 != null)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        tb_MembershipDiscount.CustID = custid;
                        // tb_MembershipDiscount.MembershipDiscountID = Convert.ToInt32(dt1.Rows[i]["MembershipDiscountID"].ToString());
                        tb_MembershipDiscount.Discount = Convert.ToDecimal(dt1.Rows[i]["CategoryDiscount"].ToString());
                        tb_MembershipDiscount.DiscountObjectID = Convert.ToInt32(dt1.Rows[i]["CategoryId"].ToString());
                        tb_MembershipDiscount.DiscountType = "category";
                        tb_MembershipDiscount.CreatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        tb_MembershipDiscount.CreatedOn = DateTime.Now;
                        tb_MembershipDiscount.UpdatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        tb_MembershipDiscount.UpdatedOn = DateTime.Now;
                        tb_MembershipDiscount.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue);

                        result = objCustomer.InsertMembershipDiscount(tb_MembershipDiscount);

                    }
                }

                Session["TempDsCatDiscount"] = null;
            }

            #endregion


            #region Insert for Product Discount Details
            if (Session["TempDsProdDiscount"] != null)
            {
                DataTable dt1;
                tb_MembershipDiscount tb_MembershipDiscount = new tb_MembershipDiscount();
                dt1 = (DataTable)Session["TempDsProdDiscount"];
                if (dt1 != null)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        tb_MembershipDiscount.CustID = custid;
                        // tb_MembershipDiscount.MembershipDiscountID = Convert.ToInt32(dt1.Rows[i]["MembershipDiscountID"].ToString());
                        tb_MembershipDiscount.Discount = Convert.ToDecimal(dt1.Rows[i]["ProductDiscount"].ToString());
                        tb_MembershipDiscount.DiscountObjectID = Convert.ToInt32(dt1.Rows[i]["ProductId"].ToString());
                        tb_MembershipDiscount.DiscountType = "product";
                        tb_MembershipDiscount.CreatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        tb_MembershipDiscount.CreatedOn = DateTime.Now;
                        tb_MembershipDiscount.UpdatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        tb_MembershipDiscount.UpdatedOn = DateTime.Now;
                        tb_MembershipDiscount.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue);
                        result = objCustomer.InsertMembershipDiscount(tb_MembershipDiscount);

                    }
                }

                Session["TempDsProdDiscount"] = null;
            }
            #endregion
        }


        private void UpdateMemberShipDisCount(int custid)
        {

            bool result = false;

            #region Update  for Category Discount Details
            if (Session["TempDsCatDiscount"] != null)
            {
                DataTable dt1;
                tb_MembershipDiscount tb_MembershipDiscount = new tb_MembershipDiscount();
                dt1 = (DataTable)Session["TempDsCatDiscount"];

                if (dt1 != null)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        tb_MembershipDiscount.CustID = custid;
                        tb_MembershipDiscount.Discount = Convert.ToDecimal(dt1.Rows[i]["CategoryDiscount"].ToString());
                        tb_MembershipDiscount.DiscountObjectID = Convert.ToInt32(dt1.Rows[i]["CategoryId"].ToString());
                        tb_MembershipDiscount.DiscountType = "category";
                        tb_MembershipDiscount.CreatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        tb_MembershipDiscount.CreatedOn = DateTime.Now;
                        tb_MembershipDiscount.UpdatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        tb_MembershipDiscount.UpdatedOn = DateTime.Now;
                        tb_MembershipDiscount.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue);
                        result = objCustomer.UpdateMembershipDiscount(tb_MembershipDiscount);
                    }
                }
                Session["TempDsCatDiscount"] = null;
            }
            #endregion

            #region Update  for Product Discount Details
            if (Session["TempDsProdDiscount"] != null)
            {
                DataTable dtProduct;
                tb_MembershipDiscount tb_MembershipDiscount = new tb_MembershipDiscount();
                dtProduct = (DataTable)Session["TempDsProdDiscount"];

                if (dtProduct != null)
                {
                    for (int i = 0; i < dtProduct.Rows.Count; i++)
                    {
                        tb_MembershipDiscount.CustID = custid;
                        tb_MembershipDiscount.Discount = Convert.ToDecimal(dtProduct.Rows[i]["ProductDiscount"].ToString());
                        tb_MembershipDiscount.DiscountObjectID = Convert.ToInt32(dtProduct.Rows[i]["ProductId"].ToString());
                        tb_MembershipDiscount.DiscountType = "product";
                        tb_MembershipDiscount.CreatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        tb_MembershipDiscount.CreatedOn = DateTime.Now;
                        tb_MembershipDiscount.UpdatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        tb_MembershipDiscount.UpdatedOn = DateTime.Now;
                        tb_MembershipDiscount.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue);
                        result = objCustomer.UpdateMembershipDiscount(tb_MembershipDiscount);
                    }
                }
                Session["TempDsProdDiscount"] = null;
            }
            #endregion
        }
        private void BindCategoryDiscountDeatail()
        {
            if (Session["TempDsCatDiscount"] != null)
            {
                dt = (DataTable)Session["TempDsCatDiscount"];
                grdCategory.DataSource = dt;
                grdCategory.DataBind();
                // Session["TempDsCatDiscount"] = null;

            }

        }
        private void BindProductDiscountDeatail()
        {
            if (Session["TempDsProdDiscount"] != null)
            {
                dt = (DataTable)Session["TempDsProdDiscount"];
                grdProduct.DataSource = dt;
                grdProduct.DataBind();
                // Session["TempDsCatDiscount"] = null;

            }

        }

        private void BinCategoryDiscountDeatailbyCustID()
        {
            if (Request.QueryString["CustID"] != null && Convert.ToString(Request.QueryString["CustID"]).Trim().ToLower() != "")
            {

                int CustID = Convert.ToInt32(Request.QueryString["CustID"].ToString());
                string DiscountType = "category";
                int storeid = Convert.ToInt32(ddlStoreName.SelectedValue);
                dsCategory = objCustomer.GetMembershipDetails(CustID, DiscountType, storeid, 2);


                Session["TempDsCatDiscount"] = dsCategory.Tables[0];
                grdCategory.DataSource = dsCategory;
                grdCategory.DataBind();

            }
            else
            {
                Session["TempDsCatDiscount"] = null;
            }
        }
        private void BinProductDiscountDeatailbyCustID()
        {
            if (Request.QueryString["CustID"] != null && Convert.ToString(Request.QueryString["CustID"]).Trim().ToLower() != "")
            {
                int CustID = Convert.ToInt32(Request.QueryString["CustID"].ToString());
                string DiscountType = "product";
                int storeid = Convert.ToInt32(ddlStoreName.SelectedValue);
                dsProduct = objCustomer.GetMembershipDetails(CustID, DiscountType, storeid, 3);


                Session["TempDsProdDiscount"] = dsProduct.Tables[0];
                grdProduct.DataSource = dsProduct;
                grdProduct.DataBind();

            }
            else
            {
                Session["TempDsProdDiscount"] = null;
            }
        }
        protected void btnCustDiscountDetailid_Click(object sender, EventArgs e)
        {

            // BindCategoryDetails(1,1);
            BindCategoryDiscountDeatail();
        }

        protected void btnProdDiscountDetailid_Click(object sender, EventArgs e)
        {
            BindProductDiscountDeatail();

        }

        /// <summary>
        /// Button Click for Generate Coupon Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenrate_Click(object sender, EventArgs e)
        {
            txtcouponcode.Text = CustomerID.ToString() + txtFirstName.Text.ToString();
        }




        protected void btnDeleteaddress_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(hdndeleteaddresid.Value))
            {

                DeleteAddress(Convert.ToInt32(hdndeleteaddresid.Value));

            }
        }


        protected void btnrefreshaddress_Click(object sender, EventArgs e)
        {
            GetCustomerAddressInfo();
        }
        /// <summary>
        /// To Delete Address using AddressID
        /// </summary>
        /// <param name="AddressID">Int32 AddressID</param>
        private void DeleteAddress(Int32 AddressID)
        {
            objCustomer = new CustomerComponent();
            try
            {
                if (objCustomer.DeleteAddressByAddressID(AddressID))
                {
                    GetCustomerAddressInfo();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Address is deleted successfully.');", true);

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Problem in deleting address, please try again.');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Using Customer Identity retrieves Customers information
        /// </summary>
        public void GetCustomerAddressInfo()
        {
            objCustomer = new CustomerComponent();
            DataSet dsCustomerDetail = new DataSet();
            dsCustomerDetail = objCustomer.GetCustomerDetailByCustID(CustomerID);
            if (dsCustomerDetail != null && dsCustomerDetail.Tables.Count > 0 && dsCustomerDetail.Tables[0].Rows.Count > 0)
            {
                String strBillAddress = "";
                String strShippAddress = "";

                if (Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) > 0)
                {
                    DataSet dsAddress = new DataSet();
                    dsAddress = objCustomer.GetAddressDetailByAddressID(Convert.ToInt32(Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"])));

                    if (dsAddress != null && dsAddress.Tables.Count > 0 && dsAddress.Tables[0].Rows.Count > 0)
                    {
                        // strBillAddress += "<tr>";
                        // strBillAddress += "<td><strong>" + Convert.ToString(dsAddress.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(dsAddress.Tables[0].Rows[0]["LastName"]) + "</strong><br>";

                        // if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Company"]) != "")
                        // {
                        //     strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Company"]) + "<br>";
                        // }
                        // strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Address1"]) + "<br>";

                        // if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Address2"]) != "")
                        // {
                        //     strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Address2"]) + "<br>";
                        // }
                        // if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Suite"]) != "")
                        // {
                        //     strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Suite"]) + "<br>";
                        // }

                        // strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["City"]) + "<br>";
                        // strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["State"]) + "<br>";
                        // strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["ZipCode"]) + "<br>";
                        // strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["CountryName"]) + "<br>";
                        // strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Phone"]) + "<br>";
                        // if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Fax"]) != "")
                        // {
                        //     strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Fax"]) + "<br>";
                        // }
                        //// strBillAddress += "<input type='submit' runat='server' style='display: none;' name='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) + "' value='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) + "' id='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) + "' class='btn1' onClick='" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick='document.getElementById(\"bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) + "\").click();'>Edit</a>";

                        // strBillAddress += "<a onclick=\"Editaddress('addtype=billingadd&type=edit&addid=" + dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"].ToString() + "')\" style=\"text-decoration:underline;font-weight:bold;font-size:12px;\" href=\"javascript:void(0);\">Edit</a>";


                        // strBillAddress += "</td></tr>";
                        // ltBilling.Text = strBillAddress.ToString().Trim();
                    }
                }

                if (Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) > 0)
                {
                    DataSet dsAddress = new DataSet();
                    dsAddress = objCustomer.GetAddressDetailByAddressID(Convert.ToInt32(Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"])));

                    if (dsAddress != null && dsAddress.Tables.Count > 0 && dsAddress.Tables[0].Rows.Count > 0)
                    {
                        //strShippAddress += "<tr>";
                        //strShippAddress += "<td><strong>" + Convert.ToString(dsAddress.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(dsAddress.Tables[0].Rows[0]["LastName"]) + "</strong><br>";

                        //if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Company"]) != "")
                        //{
                        //    strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Company"]) + "<br>";
                        //}
                        //strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Address1"]) + "<br>";

                        //if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Address2"]) != "")
                        //{
                        //    strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Address2"]) + "<br>";
                        //}
                        //if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Suite"]) != "")
                        //{
                        //    strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Suite"]) + "<br>";
                        //}

                        //strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["City"]) + "<br>";
                        //strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["State"]) + "<br>";
                        //strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["ZipCode"]) + "<br>";
                        //strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["CountryName"]) + "<br>";
                        //strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Phone"]) + "<br>";
                        //if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Fax"]) != "")
                        //{
                        //    strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Fax"]) + "<br>";
                        //}
                        ////strShippAddress += "<input type='submit' runat='server' style='display: none;' name='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) + "' value='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) + "' id='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) + "' class='btn1' onClick='" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick='document.getElementById(\"bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) + "\").click();'>Edit</a>";
                        //strShippAddress += "<a onclick=\"Editaddress('addtype=shippingadd&type=edit&addid=" + dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"].ToString() + "')\" style=\"text-decoration:underline;font-weight:bold;font-size:12px;\" href=\"javascript:void(0);\">Edit</a>";

                        //strShippAddress += "</td></tr>";
                        //ltShipping.Text = strShippAddress.ToString().Trim();
                    }
                }

                // Get Other Address of Customer

                DataSet dsOtherAddress = new DataSet();

                dsOtherAddress = objCustomer.GetAddressDetailByCustID(CustomerID);

                if (dsOtherAddress != null && dsOtherAddress.Tables.Count > 0 && dsOtherAddress.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsOtherAddress.Tables[0].Rows.Count; i++)
                    {
                        int AddressID = Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString());
                        int AddressType = Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressType"].ToString());

                        if (AddressID > 0 && AddressType == 0)
                        {
                            //Bind Customers Billing Address
                            //strBillAddress = "";
                            if (AddressID != Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]))
                            {
                                strBillAddress += "<tr>";
                                strBillAddress += "<td><strong>" + Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["FirstName"]) + " " + Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["LastName"]) + "</strong><br>";

                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Company"]) != "")
                                {
                                    strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Company"]) + "<br>";
                                }
                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address1"]) + "<br>";

                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address2"]) != "")
                                {
                                    strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address2"]) + "<br>";
                                }
                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Suite"]) != "")
                                {
                                    strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Suite"]) + "<br>";
                                }

                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["City"]) + "<br>";
                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["State"]) + "<br>";
                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["ZipCode"]) + "<br>";
                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["CountryName"]) + "<br>";
                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Phone"]) + "<br>";
                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Fax"]) != "")
                                {
                                    strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Fax"]) + "<br>";
                                }
                                // strBillAddress += "<input type='submit' runat='server' style='display: none;' name='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' value='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' id='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' class='btn1' onClick='" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick='document.getElementById(\"bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "\").click();'>Edit</a>";

                                // addtype=billingadd&type=edit&addid=6603 
                                strBillAddress += "<a onclick=\"Editaddress('addtype=billingadd&type=edit&addid=" + dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString() + "')\" style=\"text-decoration:underline;font-weight:bold;font-size:12px;\" href=\"javascript:void(0);\">Edit</a>";


                                strBillAddress += "&nbsp;&nbsp; &nbsp; <a onclick=\"SetDeleteaddressid(" + dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString() + ",'Billing')\" style=\"text-decoration:underline;font-weight:bold;font-size:12px;\" href=\"javascript:void(0);\">Delete</a>";


                                // strBillAddress += "&nbsp;&nbsp;<input type='submit' runat='server' style='display: none;' name='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' value='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' id='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' class='btn1' onClick='" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick=\"if( confirm('Are you sure you want to delete this Billing Address?') ) { document.getElementById('bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "').click(); } else { return false;}\">Delete</a>";
                                strBillAddress += "</td></tr>";
                                ltBilling.Text = strBillAddress.ToString().Trim();
                            }
                        }
                        if (AddressID > 0 && AddressType == 1)
                        {
                            //Bind Customers Shipping Address
                            //strShippAddress = "";
                            if (AddressID != Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]))
                            {
                                strShippAddress += "<tr>";
                                strShippAddress += "<td><strong>" + Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["FirstName"]) + " " + Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["LastName"]) + "</strong><br>";

                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Company"]) != "")
                                {
                                    strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Company"]) + "<br>";
                                }
                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address1"]) + "<br>";

                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address2"]) != "")
                                {
                                    strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address2"]) + "<br>";
                                }
                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Suite"]) != "")
                                {
                                    strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Suite"]) + "<br>";
                                }

                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["City"]) + "<br>";
                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["State"]) + "<br>";
                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["ZipCode"]) + "<br>";
                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["CountryName"]) + "<br>";
                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Phone"]) + "<br>";
                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Fax"]) != "")
                                {
                                    strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Fax"]) + "<br>";
                                }
                                //strShippAddress += "<input type='submit' runat='server' style='display: none;' name='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' value='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' id='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' class='btn1' onClick='" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick='document.getElementById(\"bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "\").click();'>Edit</a>";
                                //  addtype=shippingadd&type=edit&addid=6610
                                strShippAddress += "<a onclick=\"Editaddress('addtype=shippingadd&type=edit&addid=" + dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString() + "')\" style=\"text-decoration:underline;font-weight:bold;font-size:12px;\" href=\"javascript:void(0);\">Edit</a>";

                                strShippAddress += "&nbsp;&nbsp; <a onclick=\"SetDeleteaddressid(" + dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString() + ",'Shipping')\" style=\"text-decoration:underline;font-weight:bold;font-size:12px;\" href=\"javascript:void(0);\">Delete</a>";
                                // strShippAddress += "&nbsp;&nbsp;<input type='submit' runat='server' style='display: none;' name='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' value='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' id='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' class='btn1' onClick='" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick=\"if( confirm('Are you sure you want to delete this Shipping Address?') ) { document.getElementById('bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "').click(); } else { return false;}\">Delete</a>";
                                strShippAddress += "</td></tr>";
                                ltShipping.Text = strShippAddress.ToString().Trim();
                            }
                        }
                    }
                }
            }
            else { treditdeleteaddress.Visible = false; }
        }
        protected void ImgSendMail_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["CustID"] != null && Convert.ToString(Request.QueryString["CustID"]).Trim().ToLower() != "")
            {
                try
                {
                    SendMail();
                    if (ViewState["Password"] != null && !String.IsNullOrEmpty(ViewState["Password"].ToString()))
                    {
                        CommonComponent.ExecuteCommonData("update tb_customer set password='" + ViewState["Password"].ToString() + "' where customerid=" + Convert.ToInt32(Request.QueryString["CustID"].ToString()) + "");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Update", "$(document).ready( function() {jAlert('Mail Sent Successfully.', 'Message');});", true);
                    }
                }
                catch
                {

                }

            }

        }

        protected void grdcusthistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdcusthistory.PageIndex = e.NewPageIndex;
            BindCustomerHistory();
            if (chkShowCustHistory.Checked)
            {
                trCustHistory.Attributes.Add("style", "display:''");
            }
            else
            {
                trCustHistory.Attributes.Add("style", "display:none");
            }
        }


        private void BindTemplateList()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("select TradeTemplateID,isnull(TradeTempName,'') as TradeTempName from tb_TradeTempMaster  where isnull(active,0)=1 and isnull(deleted,0)=0");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddltradetemplate.DataSource = ds;
                ddltradetemplate.DataTextField = "TradeTempName";
                ddltradetemplate.DataValueField = "TradeTemplateID";
                ddltradetemplate.DataBind();
                ddltradetemplate.Items.Insert(0, new ListItem("Select Trade Template", "0"));
                ddltradetemplate.SelectedIndex = 0;
            }
        }

        protected void ddltradetemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dstempproduct = new DataTable();
            DataTable dstempcategory = new DataTable();
            if (Session["TempDsProdDiscount"] != null)
            {
                dstempproduct = (DataTable)Session["TempDsProdDiscount"];
            }

            if (Session["TempDsCatDiscount"] != null)
            {
                dstempcategory = (DataTable)Session["TempDsCatDiscount"];
            }
            if (ddltradetemplate.SelectedIndex > 0)
            {


                if (ViewState["TradeTemplateID"] != null && ViewState["TradeTemplateID"].ToString() != "" && ViewState["TradeTemplateID"].ToString() != "0")
                {
                    DataSet dsp = new DataSet();
                    dsp = CommonComponent.GetCommonDataSet("select * from tb_TradeTemplateDetail where TradeTemplateID=" + ViewState["TradeTemplateID"].ToString() + " and DiscountType='product'");
                    if (dsp != null && dsp.Tables.Count > 0 && dsp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dstempproduct.Rows.Count; i++)
                        {
                            int productid = Convert.ToInt32(dstempproduct.Rows[i]["productid"].ToString());
                            //double dis = Convert.ToDouble(dstempproduct.Rows[i]["discount"].ToString());
                            DataRow[] dr = dsp.Tables[0].Select("DiscountObjectID=" + productid);
                            if (dr.Length > 0)
                            {
                                dstempproduct.Rows[i].Delete();
                                dstempproduct.AcceptChanges();
                                i--;

                            }
                        }

                    }

                    dsp = new DataSet();
                    dsp = CommonComponent.GetCommonDataSet("select * from tb_TradeTemplateDetail where TradeTemplateID=" + ViewState["TradeTemplateID"].ToString() + " and DiscountType='category'");
                    if (dsp != null && dsp.Tables.Count > 0 && dsp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dstempcategory.Rows.Count; i++)
                        {
                            int categoryid = Convert.ToInt32(dstempcategory.Rows[i]["categoryid"].ToString());
                            //double dis = Convert.ToDouble(dstempproduct.Rows[i]["discount"].ToString());
                            DataRow[] dr = dsp.Tables[0].Select("DiscountObjectID=" + categoryid);
                            if (dr.Length > 0)
                            {
                                dstempcategory.Rows[i].Delete();
                                dstempcategory.AcceptChanges();
                                i--;

                            }
                        }

                    }


                }


                int templateid = Convert.ToInt32(ddltradetemplate.SelectedValue.ToString());
                string DiscountType = "product";
                int storeid = Convert.ToInt32(ddlStoreName.SelectedValue);
                // dsProduct = objCustomer.GetMembershipDetails(CustID, DiscountType, storeid, 3);
                dsProduct = CommonComponent.GetCommonDataSet("Exec usp_TradeTempalteDiscount " + Convert.ToInt32(ddlStoreName.SelectedValue) + ", " + templateid + ",3,0,0,'product'," + Convert.ToInt32(Session["AdminId"].ToString()) + "," + Convert.ToInt32(Session["AdminId"].ToString()) + "");

                if (dstempproduct != null && dstempproduct.Rows.Count > 0)
                {
                    if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                    {


                        for (int i = 0; i < dstempproduct.Rows.Count; i++)
                        {
                            int productid = Convert.ToInt32(dstempproduct.Rows[i]["productid"].ToString());
                            DataRow[] dd = dsProduct.Tables[0].Select("productid=" + productid);
                            if (dd.Length > 0)
                            {

                            }
                            else
                            {

                                dsProduct.Tables[0].Rows.Add(dstempproduct.Rows[i].ItemArray);
                                dsProduct.Tables[0].AcceptChanges();
                            }

                        }

                        Session["TempDsProdDiscount"] = dsProduct.Tables[0];
                        grdProduct.DataSource = dsProduct;
                        grdProduct.DataBind();
                    }
                    else
                    {
                        Session["TempDsProdDiscount"] = dstempproduct;
                        grdProduct.DataSource = dstempproduct;
                        grdProduct.DataBind();
                    }
                }
                else
                {
                    Session["TempDsProdDiscount"] = dsProduct.Tables[0];
                    grdProduct.DataSource = dsProduct;
                    grdProduct.DataBind();
                }






                templateid = Convert.ToInt32(ddltradetemplate.SelectedValue.ToString());
                DiscountType = "category";
                storeid = Convert.ToInt32(ddlStoreName.SelectedValue);
                dsCategory = CommonComponent.GetCommonDataSet("Exec usp_TradeTempalteDiscount " + Convert.ToInt32(ddlStoreName.SelectedValue) + ", " + templateid + ",2,0,0,'category'," + Convert.ToInt32(Session["AdminId"].ToString()) + "," + Convert.ToInt32(Session["AdminId"].ToString()) + "");

                // dsCategory = objCustomer.GetMembershipDetails(CustID, DiscountType, storeid, 2);



                if (dstempcategory != null && dstempcategory.Rows.Count > 0)
                {
                    if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                    {


                        for (int i = 0; i < dstempcategory.Rows.Count; i++)
                        {
                            int categoryid = Convert.ToInt32(dstempcategory.Rows[i]["categoryid"].ToString());
                            DataRow[] dd = dsCategory.Tables[0].Select("categoryid=" + categoryid);
                            if (dd.Length > 0)
                            {

                            }
                            else
                            {

                                dsCategory.Tables[0].Rows.Add(dstempcategory.Rows[i].ItemArray);
                                dsCategory.Tables[0].AcceptChanges();
                            }

                        }

                        Session["TempDsCatDiscount"] = dsCategory.Tables[0];
                        grdCategory.DataSource = dsCategory;
                        grdCategory.DataBind();
                    }
                    else
                    {
                        Session["TempDsCatDiscount"] = dstempcategory;
                        grdCategory.DataSource = dstempcategory;
                        grdCategory.DataBind();
                    }
                }
                else
                {



                    Session["TempDsCatDiscount"] = dsCategory.Tables[0];
                    grdCategory.DataSource = dsCategory;
                    grdCategory.DataBind();
                }
                ViewState["TradeTemplateID"] = ddltradetemplate.SelectedValue.ToString();

            }
            else
            {
                if (ViewState["TradeTemplateID"] != null && ViewState["TradeTemplateID"].ToString() != "" && ViewState["TradeTemplateID"].ToString() != "0")
                {
                    DataSet dsp = new DataSet();
                    dsp = CommonComponent.GetCommonDataSet("select * from tb_TradeTemplateDetail where TradeTemplateID=" + ViewState["TradeTemplateID"].ToString() + " and DiscountType='product'");
                    if (dsp != null && dsp.Tables.Count > 0 && dsp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dstempproduct.Rows.Count; i++)
                        {
                            int productid = Convert.ToInt32(dstempproduct.Rows[i]["productid"].ToString());
                            //double dis = Convert.ToDouble(dstempproduct.Rows[i]["discount"].ToString());
                            DataRow[] dr = dsp.Tables[0].Select("DiscountObjectID=" + productid);
                            if (dr.Length > 0)
                            {
                                dstempproduct.Rows[i].Delete();
                                dstempproduct.AcceptChanges();
                                i--;

                            }
                        }

                        Session["TempDsProdDiscount"] = dstempproduct;
                        grdProduct.DataSource = dstempproduct;
                        grdProduct.DataBind();

                    }

                    dsp = new DataSet();
                    dsp = CommonComponent.GetCommonDataSet("select * from tb_TradeTemplateDetail where TradeTemplateID=" + ViewState["TradeTemplateID"].ToString() + " and DiscountType='category'");
                    if (dsp != null && dsp.Tables.Count > 0 && dsp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dstempcategory.Rows.Count; i++)
                        {
                            int categoryid = Convert.ToInt32(dstempcategory.Rows[i]["categoryid"].ToString());
                            //double dis = Convert.ToDouble(dstempproduct.Rows[i]["discount"].ToString());
                            DataRow[] dr = dsp.Tables[0].Select("DiscountObjectID=" + categoryid);
                            if (dr.Length > 0)
                            {
                                dstempcategory.Rows[i].Delete();
                                dstempcategory.AcceptChanges();
                                i--;

                            }
                        }

                        Session["TempDsCatDiscount"] = dstempcategory;
                        grdCategory.DataSource = dstempcategory;
                        grdCategory.DataBind();

                    }


                }

                ViewState["TradeTemplateID"] = ddltradetemplate.SelectedValue.ToString();
            }
        }
    }
}