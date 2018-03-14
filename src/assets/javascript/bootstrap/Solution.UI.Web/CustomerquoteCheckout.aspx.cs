using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.IO;
using Solution.Bussines.Entities;
using System.Net;
using System.Net.Mail;
using Solution.ShippingMethods;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web
{
    public partial class CustomerquoteCheckout : System.Web.UI.Page
    {
        bool ChkQtyDiscount = false;
        CustomerComponent objCustomer = null;
        public bool IsGiftCartCount = false;
        public decimal GrandSubTotal = decimal.Zero;
        public static decimal FinalTotal = decimal.Zero;
        public static decimal GiftCardTotalDiscount = decimal.Zero;
        public static decimal FreeShippingAmount = decimal.Zero;
        ShoppingCartComponent objCart = new ShoppingCartComponent();
        public decimal ShippingCharges = decimal.Zero;
        Decimal SwatchQty = Decimal.Zero;
        public string storePath;
        public string strcretio = "";
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            AppLogic.ApplicationStart();
            AppConfig.StoreID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["GeneralStoreID"]);
            storePath = "<b>" + AppLogic.AppConfigs("STOREPATH") + "</b>";
            Page.MaintainScrollPositionOnPostBack = true;
            String strScript = @"$('#content-width').css('margin-top', '15px');
                                    $('#header-part').css('display', 'none');
                                    $('#footer-part').css('display', 'none');
                                    $('.breadcrumbs').css('display', 'none');
                                    $('body').css('background', 'none');$('#wrapper').css('background', 'none');";

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "HideHeaderFooter", strScript.Trim(), true);
            //  rdlPaymentType.SelectedValue = "PAYPALPRO";
            storePath = "<b>" + AppLogic.AppConfigs("STOREPATH") + "</b>";
            Page.MaintainScrollPositionOnPostBack = true;


            if (Request.QueryString["custquoteid"] != null && Request.QueryString["custquoteid"].ToString() != "")
            {


                String custquoteid = "";
                if (Session["QuoteIDCompare"] != null && Session["QuoteIDCompare"].ToString().ToLower() == Request.QueryString["custquoteid"].ToString().ToLower())
                {
                    custquoteid = SecurityComponent.Decrypt(Session["QuoteIDCompare"].ToString());
                }
                else
                {
                    custquoteid = SecurityComponent.Decrypt(Request.QueryString["custquoteid"].ToString());
                }





                String CustId = Convert.ToString(CommonComponent.GetScalarCommonData("select CustomerID From tb_CustomerQuote where CustomerQuoteID='" + custquoteid + "'"));
                Session["CustID"] = CustId.ToString();

                Session["UserName"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Email FROM tb_Customer WHERE CustomeriD=" + CustId + ""));

            }
            if (Session["CustID"] == null || Session["CustID"].ToString() == "")
            {
                LoginTable.Visible = true;
                Response.Redirect("/index.aspx");

            }
            else
            {
                if (Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    // LoginTable.Visible = false;
                    LoginTable.Attributes.Add("style", "display:none;");
                    // tblcartmain.Attributes.Add("style", "display:none;");
                    divrdolist.Visible = false;
                }
                else
                {
                    if (Request.QueryString["stype"] != null)
                    {

                        if (Request.QueryString["stype"].ToString() == "1")
                        {
                            chkCreateNewAccountGuest.Checked = true;
                            LoginTable.Attributes.Add("style", "display:none;");
                            //  tblcartmain.Attributes.Add("style", "display:none;");
                        }
                        else if (Request.QueryString["stype"].ToString() == "2")
                        {
                            chkCreateNewAccount.Checked = true;
                            LoginTable.Attributes.Add("style", "display:none;");
                            // tblcartmain.Attributes.Add("style", "display:none;");
                        }

                        if (Request.QueryString["stype"].ToString() == "1" || Request.QueryString["stype"].ToString() == "2")
                        {
                            if (chkCreateNewAccount.Checked)
                            {
                                trReturningAccount.Attributes.Add("style", "display:none;");
                                trCreAccChangePass01.Attributes.Add("style", "display:'';");
                                trCreAccChangePass02.Attributes.Add("style", "display:'';");
                                trbillrow.Attributes.Add("style", "display:'';");
                                trcardnumdetail.Attributes.Add("style", "display:'';");
                                divplaceorder.Attributes.Add("style", "display:'';");
                                trguest.Attributes.Add("style", "display:none;");
                                if (rptFeaturedProduct != null && rptFeaturedProduct.Items.Count > 0)
                                {
                                    trfeatureproduct.Attributes.Add("style", "display:'';");
                                }
                                if (!IsPostBack)
                                {
                                    txtCreateEmail.Focus();
                                }

                            }
                            else
                            {
                                trCreAccChangePass01.Attributes.Add("style", "display:none;");
                                trCreAccChangePass02.Attributes.Add("style", "display:none;");
                            }

                            if (chkReturningAcHolder.Checked)
                            {

                                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                                {
                                    trReturningAccount.Attributes.Add("style", "display:'';");
                                    trCreAccChangePass01.Attributes.Add("style", "display:none;");
                                    trCreAccChangePass02.Attributes.Add("style", "display:none;");
                                    trbillrow.Attributes.Add("style", "display:'';");
                                    trcardnumdetail.Attributes.Add("style", "display:'';");
                                    divplaceorder.Attributes.Add("style", "display:'';");
                                    trguest.Attributes.Add("style", "display:none;");
                                    if (rptFeaturedProduct != null && rptFeaturedProduct.Items.Count > 0)
                                    {
                                        trfeatureproduct.Attributes.Add("style", "display:'';");
                                    }

                                }
                                else
                                {
                                    trReturningAccount.Attributes.Add("style", "display:'';");
                                }

                            }

                            else { trReturningAccount.Attributes.Add("style", "display:none;"); }


                            if (chkCreateNewAccountGuest.Checked == true)
                            {
                                if (!IsPostBack)
                                {
                                    txtGuestemail.Focus();
                                }
                                trCreAccChangePass01.Attributes.Add("style", "display:none;");
                                trbillrow.Attributes.Add("style", "display:'';");
                                trcardnumdetail.Attributes.Add("style", "display:'';");
                                divplaceorder.Attributes.Add("style", "display:'';");
                                trguest.Attributes.Add("style", "display:'';");
                                if (rptFeaturedProduct != null && rptFeaturedProduct.Items.Count > 0)
                                {
                                    trfeatureproduct.Attributes.Add("style", "display:'';");
                                }
                            }
                        }


                    }
                }
            }

            CommonOperations.RedirectWithSSL(true);
            //  Session["PaymentMethod"] = "creditcard";
            if (!IsPostBack)
            {
                //if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
                //{
                //    btnPlaceOrder.Attributes.Add("onclick", "return ValidationLoginuser();");
                //    btnPlaceOrder.ImageUrl = "/images/place-order.png";
                //}
                //else
                //{
                //    btnPlaceOrder.ImageUrl = "/images/PaypalCheckout.gif";
                //    tblpayment.Visible = false;
                //    btnPlaceOrder.Attributes.Add("onclick", "return ValidationLoginuserOtherPayment();");
                //}

                ltrreturnpolicy.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Configvalue FROM tb_Appconfig WHERE Configname='checkoutploicy' and storeid=1 and deleted=0"));
                if (rdlPaymentType != null && rdlPaymentType.Items.Count > 0)
                {
                    if (Session["PaymentMethod"] == null && Convert.ToString(Session["PaymentMethod"]) == "")
                        Session["PaymentMethod"] = Convert.ToString(rdlPaymentType.SelectedItem.Text).ToLower();
                    else
                        rdlPaymentType.SelectedValue = Convert.ToString(Session["PaymentMethod"]);


                }

                if (Request.QueryString["custquoteid"] != null)
                {
                    CommonComponent.ExecuteCommonData("DELETE FROM tb_ShoppingCartItems WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID =" + Session["CustID"].ToString() + ")");
                    Int32 ShoppingId = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO tb_ShoppingCart(CustomerID,CreatedOn) VALUES (" + Session["CustID"].ToString() + ",getdate()); SELECT SCOPE_IDENTITY();"));
                    String custquoteid = "";
                    if (Session["QuoteIDCompare"] != null && Session["QuoteIDCompare"].ToString().ToLower() == Request.QueryString["custquoteid"].ToString().ToLower())
                    {
                        custquoteid = SecurityComponent.Decrypt(Session["QuoteIDCompare"].ToString());
                    }
                    else
                    {
                        custquoteid = SecurityComponent.Decrypt(Request.QueryString["custquoteid"].ToString());
                    }

                     
                    CommonComponent.ExecuteCommonData("INSERT INTO tb_ShoppingCartItems(ShoppingCartID, ProductID, Price, Quantity,Notes, CategoryID, VariantNames, VariantValues,DiscountPrice,RelatedproductID,IsProductType) SELECT " + ShoppingId + ", ProductID, Price, Quantity, Notes,0, VariantNames, VariantValues,DiscountPrice,RelatedproductID,IsProductType FROM dbo.tb_CustomerQuoteItems WHERE CustomerQuoteID= " + custquoteid + "And CustomerID=" + Session["CustID"].ToString() + "");
                }


                //txtGuestemail.Attributes.Add("onchange", "getemail('" + txtGuestemail.ClientID.ToString() + "'); copyemail('ContentPlaceHolder1_txtGuestemail','ContentPlaceHolder1_txtBillEmail','ContentPlaceHolder1_txtShipEmailAddress');");
                // txtCreateEmail.Attributes.Add("onchange", "getemail('" + txtCreateEmail.ClientID.ToString() + "'); copyemail('ContentPlaceHolder1_txtCreateEmail','ContentPlaceHolder1_txtBillEmail','ContentPlaceHolder1_txtShipEmailAddress');");
                txtGuestemail.Attributes.Add("onkeyup", "copyemail('ContentPlaceHolder1_txtGuestemail','ContentPlaceHolder1_txtBillEmail','ContentPlaceHolder1_txtShipEmailAddress');");
                txtCreateEmail.Attributes.Add("onkeyup", "copyemail('ContentPlaceHolder1_txtCreateEmail','ContentPlaceHolder1_txtBillEmail','ContentPlaceHolder1_txtShipEmailAddress');");

                BindFeaturedProducts();
                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("FreeShippingLimit").ToString().Trim()))
                {
                    FreeShippingAmount = Convert.ToDecimal(AppLogic.AppConfigs("FreeShippingLimit"));
                }
                else
                {

                }
                Session["CustomLevelDiscount"] = null;
                GetCreditCardType();

                if (Session["PaymentMethod"] != null)
                {
                    ltrMethodName.Text = Session["PaymentMethod"].ToString();
                    OrderComponent objPayment = new OrderComponent();
                    DataSet dsPayment = new DataSet();
                    dsPayment = objPayment.GetPaymentGateway(Session["PaymentMethod"].ToString(), Convert.ToInt32(AppLogic.AppConfigs("StoreId").ToString()));
                    if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
                    {
                        Session["PaymentGateway"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["PaymentService"].ToString());
                        Session["PaymentGatewayStatus"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString());
                    }
                }
                else
                {
                    ltrMethodName.Text = "UnKnown";
                }

                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    FillCountry();
                    GetCustomerDetails(Convert.ToInt32(Session["CustID"].ToString()));
                    //*  ChkDetailsReadOnly("true");
                    if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
                    {
                        btnPlaceOrder.Attributes.Add("onclick", "return ValidationLoginuser();");
                        btnPlaceOrder.ImageUrl = "/images/place-order.png";
                        tblpayment.Visible = true;
                    }
                    else
                    {
                        btnPlaceOrder.ImageUrl = "/images/PaypalCheckout.gif";
                        tblpayment.Visible = false;
                        btnPlaceOrder.Attributes.Add("onclick", "return ValidationLoginuserOtherPayment();");
                    }

                    tblBillAddEntry.Visible = true;
                    tblShippAddressEntry.Visible = true;

                    //tblBillAddEntry.Visible = false;
                    //tblShippAddressEntry.Visible = false;
                    //tblBillAddress.Visible = true;
                    //tblShipAddress.Visible = true;
                    txtNameOnCard.Focus();
                    // BindShippingMethod();
                    trbillrow.Attributes.Add("style", "display:'';");
                    trcardnumdetail.Attributes.Add("style", "display:'';");
                    divplaceorder.Attributes.Add("style", "display:'';");
                    trCreatAcclogin.Attributes.Add("style", "display:none;");
                    trfeatureproduct.Attributes.Add("style", "display:'';");

                }
                else
                {
                    FillCountry();
                    tblBillAddEntry.Visible = true;
                    //  chkCreateNewAccountGuest.Checked = true;
                    tblShippAddressEntry.Visible = true;
                    tblBillAddress.Visible = false;
                    //*    tblShipAddress.Visible = false;
                    if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
                    {
                        btnPlaceOrder.Attributes.Add("onclick", "return ValidationNotLogin();");
                        btnPlaceOrder.ImageUrl = "/images/place-order.png";
                        tblpayment.Visible = true;
                    }
                    else
                    {
                        tblpayment.Visible = false;
                        btnPlaceOrder.Attributes.Add("onclick", "return ValidationNotLoginOtherPayment();");
                        btnPlaceOrder.ImageUrl = "/images/PaypalCheckout.gif";
                    }
                    //txtBillFirstname.Focus();

                    #region Fill Automatic Country, ZipCode and Shipping Methods

                    if (Session["SelectedZipCode"] != null && Convert.ToString(Session["SelectedZipCode"]) != "")
                    {
                        try
                        {
                            txtBillZipCode.Text = Convert.ToString(Session["SelectedZipCode"]);
                            txtShipZipCode.Text = Convert.ToString(Session["SelectedZipCode"]);
                            hdnZipCode.Value = Convert.ToString(Session["SelectedZipCode"]);
                        }
                        catch
                        {
                            txtBillZipCode.Text = "";
                            txtShipZipCode.Text = "";
                            hdnZipCode.Value = "";
                        }
                    }

                    if (Session["SelectedCountry"] != null && Convert.ToString(Session["SelectedCountry"]) != "")
                    {
                        hdncountry.Value = Convert.ToString(Session["SelectedCountry"]);
                        try
                        {
                            ddlBillcountry.SelectedValue = Convert.ToString(Session["SelectedCountry"]);
                        }
                        catch
                        {
                            if (ddlBillcountry.Items.FindByText("United States") != null)
                            {
                                ddlBillcountry.Items.FindByText("United States").Selected = true;
                            }
                            else
                            {
                                ddlBillcountry.SelectedIndex = 0;
                            }
                            txtBillZipCode.Text = "";
                        }
                        try
                        {
                            ddlShipCounry.SelectedValue = Convert.ToString(Session["SelectedCountry"]);
                        }
                        catch
                        {
                            if (ddlShipCounry.Items.FindByText("United States") != null)
                            {
                                ddlShipCounry.Items.FindByText("United States").Selected = true;
                            }
                            else
                            {
                                ddlShipCounry.SelectedIndex = 0;
                            }
                            txtShipZipCode.Text = "";
                        }
                    }
                    GetShippingMethodByShipAddress();
                    BindOrderSummary();
                    BindShippingMethod();

                    #endregion


                }

                BindOrderSummary();
                if (Request.QueryString["custquoteid"] != null)
                {
                    String custquoteid = "";
                    if (Session["QuoteIDCompare"] != null && Session["QuoteIDCompare"].ToString().ToLower() == Request.QueryString["custquoteid"].ToString().ToLower())
                    {
                        custquoteid = SecurityComponent.Decrypt(Session["QuoteIDCompare"].ToString());
                    }
                    else
                    {
                        custquoteid = SecurityComponent.Decrypt(Request.QueryString["custquoteid"].ToString());
                    }
                    //String custquoteid = SecurityComponent.Decrypt(Request.QueryString["custquoteid"].ToString());
                    String CouponCode = Convert.ToString(CommonComponent.GetScalarCommonData("select CouponCode from tb_CustomerQuote where CustomerQuoteID=" + custquoteid + ""));
                    Control FooterTemplate = RptCartItems.Controls[RptCartItems.Controls.Count - 1].Controls[0];
                    TextBox txtPromoCode = FooterTemplate.FindControl("txtPromoCode") as TextBox;
                    if (!String.IsNullOrEmpty(CouponCode))
                    {

                        txtPromoCode.Text = CouponCode;
                        btnApply_Click(null, null);

                    }
                    else
                    {
                        txtPromoCode.Text = "";
                        btnApply_Click(null, null);
                    }
                }
                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    BindShippingMethod();

                }
            }
            else
            {
                if (chkCreateNewAccount.Checked)
                {
                    trReturningAccount.Attributes.Add("style", "display:none;");
                    trCreAccChangePass01.Attributes.Add("style", "display:'';");
                    trCreAccChangePass02.Attributes.Add("style", "display:'';");
                    trbillrow.Attributes.Add("style", "display:'';");
                    trcardnumdetail.Attributes.Add("style", "display:'';");
                    divplaceorder.Attributes.Add("style", "display:'';");
                    trguest.Attributes.Add("style", "display:none;");
                    if (rptFeaturedProduct != null && rptFeaturedProduct.Items.Count > 0)
                    {
                        trfeatureproduct.Attributes.Add("style", "display:'';");
                    }
                    if (!IsPostBack)
                    {
                        txtCreateEmail.Focus();
                    }

                }
                else
                {
                    trCreAccChangePass01.Attributes.Add("style", "display:none;");
                    trCreAccChangePass02.Attributes.Add("style", "display:none;");
                }

                if (chkReturningAcHolder.Checked)
                {
                    if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                    {
                        trReturningAccount.Attributes.Add("style", "display:'';");
                        trCreAccChangePass01.Attributes.Add("style", "display:none;");
                        trCreAccChangePass02.Attributes.Add("style", "display:none;");
                        trbillrow.Attributes.Add("style", "display:'';");
                        trcardnumdetail.Attributes.Add("style", "display:'';");
                        divplaceorder.Attributes.Add("style", "display:'';");
                        trguest.Attributes.Add("style", "display:none;");
                        if (rptFeaturedProduct != null && rptFeaturedProduct.Items.Count > 0)
                        {
                            trfeatureproduct.Attributes.Add("style", "display:'';");
                        }

                    }
                    else
                    {
                        trReturningAccount.Attributes.Add("style", "display:'';");
                    }

                }

                else { trReturningAccount.Attributes.Add("style", "display:none;"); }


                if (chkCreateNewAccountGuest.Checked == true)
                {
                    trCreAccChangePass01.Attributes.Add("style", "display:none;");
                    trbillrow.Attributes.Add("style", "display:'';");
                    trcardnumdetail.Attributes.Add("style", "display:'';");
                    divplaceorder.Attributes.Add("style", "display:'';");
                    trguest.Attributes.Add("style", "display:'';");
                    if (rptFeaturedProduct != null && rptFeaturedProduct.Items.Count > 0)
                    {
                        trfeatureproduct.Attributes.Add("style", "display:'';");
                    }
                    if (!IsPostBack)
                    {
                        txtGuestemail.Focus();
                    }
                }


                //if (UseShippingAddress.Checked)
                //{
                //    pnlShippingDetails.Attributes.Add("style", "display:table");
                //}
                //else
                //{
                //    pnlShippingDetails.Attributes.Add("style", "display:table");
                //}
                StateOtherSelection();
            }
        }

        /// <summary>
        ///  State Other Selection
        /// </summary>
        private void StateOtherSelection()
        {
            string strScript = "";
            if (ddlShipState.SelectedValue == "-11")
            {
                strScript += "SetShippingOtherVisible(true);";
            }
            else
            {
                strScript += "SetShippingOtherVisible(false);";
            }
            if (ddlBillstate.SelectedValue == "-11")
            {
                strScript += "SetBillingOtherVisible(true);";
            }
            else
            {
                strScript += "SetBillingOtherVisible(false);";
            }
            if (chkcopy.Checked == false)
            {
                strScript += "SetBillingShippingVisible(true);";
            }
            else
            {
                strScript += "SetBillingShippingVisible(false);";
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShippingVisible", strScript, true);
        }

        /// <summary>
        /// Bind Order Summary By Customer ID
        /// </summary>
        private void BindOrderSummary()
        {
            ViewState["Weight"] = null;
            if (Session["CustID"] != null && Session["CustID"].ToString() != "")
            {
                DataSet dsShoppingCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));
                ViewState["Weight"] = null;
                int GiftCartCount = 0;
                if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
                {
                    ltrCart.Text = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" class=\"table\">";
                    ltrCart.Text += "<tbody><tr>";
                    ltrCart.Text += "<th width=\"24%\">";
                    ltrCart.Text += "Image";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "<th align=\"left\" width=\"48%\">";
                    ltrCart.Text += "Product";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "<th align=\"center\" width=\"13%\">";
                    ltrCart.Text += "Quantity";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "<th align=\"center\" width=\"15%\">";
                    ltrCart.Text += "Price";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "</tr>";

                    for (int i = 0; i < dsShoppingCart.Tables[0].Rows.Count; i++)
                    {
                        ltrCart.Text += "<tr>";
                        ltrCart.Text += "<td align=\"center\" valign=\"top\">";
                        Random rd = new Random();
                        string strmainPath = AppLogic.AppConfigs("ImagePathProduct").ToString();
                        if (!string.IsNullOrEmpty(dsShoppingCart.Tables[0].Rows[i]["ImageName"].ToString()))
                        {
                            string strimgName = strmainPath + "micro/" + dsShoppingCart.Tables[0].Rows[i]["ImageName"].ToString();
                            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (strimgName)))
                            {
                                strimgName = AppLogic.AppConfigs("Live_Contant_Server") + strimgName + "?" + rd.Next(1000).ToString();
                                ltrCart.Text += "<img height=\"57\" width=\"98\" title=\"\" alt=\"\" src=\"" + strimgName.ToString() + "\">";
                            }
                            else
                            {
                                ltrCart.Text += "<img height=\"57\" width=\"98\" title=\"\" alt=\"\" src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + strmainPath + "micro/image_not_available.jpg\" />";
                            }
                        }
                        else
                        {
                            ltrCart.Text += "<img height=\"57\" width=\"98\" title=\"\" alt=\"\" src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + strmainPath + "micro/image_not_available.jpg\" />";
                        }
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td valign=\"top\">";
                        ////ltrCart.Text += "<a title=\"" + Server.HtmlEncode(dsShoppingCart.Tables[0].Rows[i]["Name"].ToString()) + "\" href=\"/" + dsShoppingCart.Tables[0].Rows[i]["Maincategory"].ToString() + "/" + dsShoppingCart.Tables[0].Rows[i]["Sename"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[i]["ProductID"].ToString() + ".aspx\">" + dsShoppingCart.Tables[0].Rows[i]["Name"].ToString() + "</a><br />";
                        //ltrCart.Text += "<a title=\"" + Server.HtmlEncode(dsShoppingCart.Tables[0].Rows[i]["Name"].ToString()) + "\" href=\"" + GetProductUrl(dsShoppingCart.Tables[0].Rows[i]["Maincategory"].ToString(), dsShoppingCart.Tables[0].Rows[i]["Sename"].ToString(), dsShoppingCart.Tables[0].Rows[i]["ProductID"].ToString(), dsShoppingCart.Tables[0].Rows[i]["CustomCartID"].ToString()) + "\">" + dsShoppingCart.Tables[0].Rows[i]["Name"].ToString() + "</a><br />";
                        if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[i]["RelatedproductID"].ToString()) > 0)
                        {
                            ltrCart.Text += "" + dsShoppingCart.Tables[0].Rows[i]["Name"].ToString() + "<br />";
                        }
                        else
                        {
                            ltrCart.Text += "<a title=\"" + Server.HtmlEncode(dsShoppingCart.Tables[0].Rows[i]["Name"].ToString()) + "\" href=\"/" + dsShoppingCart.Tables[0].Rows[i]["ProductURL"].ToString() + "\">" + dsShoppingCart.Tables[0].Rows[i]["Name"].ToString() + "</a><br />";
                        }

                        string[] variantName = dsShoppingCart.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] variantValue = dsShoppingCart.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 0; j < variantValue.Length; j++)
                        {
                            if (variantName.Length > j)
                            {
                                ltrCart.Text += variantName[j].ToString() + " : " + variantValue[j].ToString() + "<br />";
                            }
                        }

                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td align=\"center\" valign=\"top\">";
                        ltrCart.Text += dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString();
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td align=\"right\" valign=\"top\">";
                        // Check Quantity Discount 

                        Decimal QtyDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SElect ISNULL(qt.DiscountPercent,0) as DiscountPercent from tb_QuantityDiscountTable as qt " +
                                            " inner join tb_QauntityDiscount ON qt.QuantityDiscountID = dbo.tb_QauntityDiscount.QuantityDiscountID  " +
                                            " Where qt.LowQuantity<=" + dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString() + " and qt.HighQuantity>=" + dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString() + " and tb_QauntityDiscount.QuantityDiscountID in (Select QuantityDiscountID from  " +
                                            " tb_Product Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + dsShoppingCart.Tables[0].Rows[i]["ProductId"].ToString() + ") "));

                        if (QtyDiscount > Decimal.Zero)
                        {
                            decimal PriceValue = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Price"].ToString());
                            QtyDiscount = Math.Round((Convert.ToDecimal(PriceValue.ToString()) * QtyDiscount) / 100, 2);
                            string dd = string.Format("{0:0.00}", QtyDiscount);

                            if (QtyDiscount > 0)
                            {
                                //decimal QtyDisPrice = Convert.ToDecimal(Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Price"].ToString()) - QtyDiscount);
                                decimal QtyDisPrice = (Convert.ToDecimal(PriceValue) * Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString())) - (Convert.ToDecimal(dd) * Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString()));

                                ltrCart.Text += "<s>$" + String.Format("{0:0.00}", Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Price"].ToString())) + "</s><br />";
                                ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(QtyDisPrice.ToString()));
                            }
                            else
                                ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Price"].ToString()));
                        }
                        else
                        {
                            ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Price"].ToString()));
                        }
                        // End

                        ltrCart.Text += "</td>";
                        ltrCart.Text += "</tr>";
                        DataSet dsWeight = new DataSet();
                        dsWeight = ProductComponent.GetproductImagename(Convert.ToInt32(dsShoppingCart.Tables[0].Rows[i]["ProductId"].ToString()));
                        decimal objdecimal = decimal.Zero;
                        if (dsWeight != null && dsWeight.Tables.Count > 0 && dsWeight.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dsWeight.Tables[0].Rows[0]["IsFreeShipping"]))
                            {
                                objdecimal = 0;
                            }
                            else
                            {
                                //  objdecimal = Convert.ToDecimal(dsWeight.Tables[0].Rows[0]["weight"].ToString());
                                if (Convert.ToDecimal(dsWeight.Tables[0].Rows[0]["weight"].ToString()) > 0)
                                    objdecimal = (Convert.ToDecimal(dsWeight.Tables[0].Rows[0]["weight"].ToString()) * Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString()));
                                else
                                    objdecimal = (Convert.ToDecimal(1) * Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString()));
                            }
                        }
                        int GiftCardProductID = 0;
                        GiftCardProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(GiftCardProductID,0) FROM dbo.tb_GiftCardProduct Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + Convert.ToInt32(dsShoppingCart.Tables[0].Rows[i]["ProductId"].ToString()) + ""));
                        if (GiftCardProductID > 0)
                        {
                            //    GiftCartCount = GiftCartCount + 1;
                        }
                        else
                        {
                            if (ViewState["Weight"] != null)
                            {
                                objdecimal += (Convert.ToDecimal(ViewState["Weight"].ToString()) * Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString()));
                                ViewState["Weight"] = objdecimal.ToString();
                            }
                            else
                            {
                                ViewState["Weight"] = objdecimal.ToString();
                            }
                        }
                    }
                    Decimal SubTotal = Decimal.Zero;
                    for (int i = 0; i < dsShoppingCart.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["price"].ToString()) > Decimal.Zero)
                        {
                            SubTotal = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["SubTotal"].ToString());
                            ViewState["SubTotal"] = SubTotal.ToString();
                            break;
                        }
                    }

                    //Decimal SubTotal = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[0]["SubTotal"].ToString());
                    //ViewState["SubTotal"] = SubTotal.ToString();
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                    ltrCart.Text += "Subtotal :";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td align=\"right\" valign=\"middle\">";
                    if (Session["QtyDiscount"] != null && Convert.ToDecimal(Session["QtyDiscount"].ToString()) > Decimal.Zero)
                    {
                        ltrCart.Text += "<s>$" + String.Format("{0:0.00}", SubTotal) + "</s><br />";
                        SubTotal = SubTotal - Convert.ToDecimal(Session["QtyDiscount"].ToString());
                        ltrCart.Text += "$" + String.Format("{0:0.00}", SubTotal);
                    }
                    else
                    {
                        ltrCart.Text += "$" + String.Format("{0:0.00}", SubTotal);
                    }
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";

                    if (ViewState["CustomerLevelID"] != null && Convert.ToInt32(ViewState["CustomerLevelID"].ToString()) > 0)
                    {
                        ltrCart.Text += "<tr>";
                        ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                        ltrCart.Text += "Customer Level Discount :";
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td align=\"right\" valign=\"middle\">###customlevelDiscount###";
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "</tr>";
                    }

                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                    ltrCart.Text += "Discount :";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td align=\"right\" valign=\"middle\">###CouponCodeDiscount###</td>";
                    ltrCart.Text += "</tr>";

                    #region Code for Show Gift Certificate in Cart

                    if (Session["GiftCertificateDiscount"] != null && Session["GiftCertificateDiscount"].ToString() != "")
                    {
                        ltrCart.Text += "<tr>";
                        ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                        ltrCart.Text += "Gift Card Applied Discount:";
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td align=\"right\" valign=\"middle\">###GiftCertiDiscount###";
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "</tr>";
                    }

                    if (Session["GiftCertificateRemaningBalance"] != null && Session["GiftCertificateRemaningBalance"].ToString() != "")
                    {
                        ltrCart.Text += "<tr>";
                        ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                        ltrCart.Text += "Gift Card Remaning Balance:";
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td align=\"right\" valign=\"middle\">###GiftCertiBalDiscount###";
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "</tr>";
                    }
                    #endregion

                    //if (Session["QtyDiscount"] != null && Convert.ToDecimal(Session["QtyDiscount"].ToString()) > Decimal.Zero)
                    //{
                    //    ltrCart.Text += "<tr>";
                    //    ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                    //    ltrCart.Text += "Quantity Discount :";
                    //    ltrCart.Text += "</td>";
                    //    ltrCart.Text += "<td align=\"right\" valign=\"middle\">" + String.Format("{0:0.00}", Convert.ToDecimal(Session["QtyDiscount"].ToString())) + "";
                    //    ltrCart.Text += "</td>";
                    //    ltrCart.Text += "</tr>";
                    //}

                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                    ltrCart.Text += "Shipping :";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td align=\"right\" valign=\"middle\">";
                    string strShippingName = "";
                    string strShippingCharge = "0.00";

                    if (ViewState["CustomerLevelID"] != null && ViewState["CustomerLevelFreeShipping"] == null)
                    {
                        bool IsFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasFreeShipping,0) AS LevelHasFreeShipping FROM dbo.tb_CustomerLevel WHERE CustomerLevelID=" + Convert.ToInt32(ViewState["CustomerLevelID"]) + ""));

                        if (IsFreeShipping)
                        {
                            ViewState["CustomerLevelFreeShipping"] = "1";
                        }
                        else
                        {
                            if (rdoShippingMethod.SelectedIndex > -1)
                            {
                                strShippingName = rdoShippingMethod.SelectedItem.Text.ToString();

                                if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                                {
                                    strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                                    strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();
                                    ViewState["strShippingCharge"] = strShippingCharge.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (rdoShippingMethod.SelectedIndex > -1)
                        {
                            strShippingName = rdoShippingMethod.SelectedItem.Text.ToString();

                            if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                            {
                                strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                                strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();
                                ViewState["strShippingCharge"] = strShippingCharge.ToString();
                            }
                        }
                    }
                    Decimal Ordertaxsubtotal = 0;
                    if (strShippingCharge.ToString() != "")
                    {
                        Ordertaxsubtotal = SubTotal;
                        ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(strShippingCharge.ToString()));
                        SubTotal = SubTotal + Convert.ToDecimal(strShippingCharge.ToString());

                    }
                    else
                    {
                        Ordertaxsubtotal = SubTotal;
                        ltrCart.Text += "$0.00";
                    }
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                    ltrCart.Text += "Sale Tax :";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td align=\"right\" valign=\"middle\">";

                    #region Custom Level Discount Start

                    Decimal CustomerlevelDiscount = Decimal.Zero;
                    if (ViewState["CustomerLevelID"] != null && Convert.ToInt32(ViewState["CustomerLevelID"].ToString()) > 0 && Session["CustomLevelDiscount"] == null)
                    {
                        DataSet dsDiscount = new DataSet();
                        dsDiscount = CommonComponent.GetCommonDataSet("SELECT isnull(LevelDiscountPercent,0) as LevelDiscountPercent,isnull(LevelDiscountAmount,0) as LevelDiscountAmount FROM tb_CustomerLevel WHERE CustomerLevelID=" + ViewState["CustomerLevelID"].ToString() + "");
                        if (dsDiscount != null && dsDiscount.Tables.Count > 0 && dsDiscount.Tables[0].Rows.Count > 0)
                        {
                            Decimal CustomePersantage = Convert.ToDecimal(dsDiscount.Tables[0].Rows[0]["LevelDiscountPercent"].ToString());
                            if (CustomePersantage == Decimal.Zero)
                            {
                                CustomerlevelDiscount = Convert.ToDecimal(dsDiscount.Tables[0].Rows[0]["LevelDiscountAmount"].ToString());
                            }
                            else
                            {
                                if (ViewState["SubTotal"] != null)
                                {
                                    CustomerlevelDiscount = (Convert.ToDecimal(ViewState["SubTotal"]) * CustomePersantage) / 100;
                                }
                            }
                            Session["CustomLevelDiscount"] = CustomerlevelDiscount.ToString();
                        }
                    }
                    else
                    {
                        if (Session["CustomLevelDiscount"] != null)
                        {
                            CustomerlevelDiscount = Convert.ToDecimal(Session["CustomLevelDiscount"]);
                        }
                    }
                    //if (Session["QtyDiscount"] != null && Convert.ToDecimal(Session["QtyDiscount"].ToString()) > Decimal.Zero)
                    //{
                    //    SubTotal = SubTotal - Convert.ToDecimal(Session["QtyDiscount"].ToString());
                    //}

                    //// SubTotal = SubTotal - CustomerlevelDiscount;

                    // **** Code By Girish ****
                    Decimal SubTotalCustomer = SubTotal;
                    Decimal DiscountCustomer = Math.Round((SubTotalCustomer - CustomerlevelDiscount), 2);
                    if (DiscountCustomer < 0)
                    {
                        SubTotal = 0;
                        DiscountCustomer = SubTotalCustomer;
                    }
                    else
                    {
                        SubTotal = SubTotal - CustomerlevelDiscount;
                        DiscountCustomer = CustomerlevelDiscount;
                    }

                    Decimal TotalCouponDiscount = Decimal.Zero;
                    if (Session["Discount"] != null && Session["Discount"].ToString() != "")
                    {
                        decimal TotalDiscount = Convert.ToDecimal(Session["Discount"]);
                        Decimal SubTotalDiscount = SubTotal;
                        TotalCouponDiscount = Math.Round((SubTotalDiscount - TotalDiscount), 2);
                        if (TotalCouponDiscount < 0)
                        {
                            SubTotal = 0;
                            TotalCouponDiscount = SubTotalDiscount;
                        }
                        else
                        {
                            SubTotal = SubTotal - TotalDiscount;
                            TotalCouponDiscount = TotalDiscount;
                        }
                    }

                    decimal OrderTax = decimal.Zero;
                    if (Session["CustID"] != null && Session["CustID"].ToString() != null && Session["UserName"] != null && Session["UserName"].ToString() != "")
                    {
                        //OrderTax = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), SubTotal);
                        //ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);

                        bool IsFreeTax = false;
                        if (ViewState["CustomerLevelID"] != null)
                        {
                            IsFreeTax = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasnoTax,0) AS LevelHasnoTax FROM dbo.tb_CustomerLevel WHERE CustomerLevelID=" + ViewState["CustomerLevelID"].ToString() + ""));
                        }
                        if (IsFreeTax)
                        {
                            ltrCart.Text += "$" + String.Format("{0:0.00}", 0);
                        }
                        else
                        {
                            OrderTax = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Ordertaxsubtotal);
                            //ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);
                            if (OrderTax < 0)
                                ltrCart.Text += "$" + String.Format("{0:0.00}", 0);
                            else
                                ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);

                        }
                    }
                    else
                    {
                        if (chkcopy.Checked == true && ddlBillstate.SelectedIndex > 0)
                        {
                            if (ddlBillstate.SelectedValue.ToString() == "-11")
                            {
                                OrderTax = SaleTax(txtBillother.Text.ToString(), txtBillZipCode.Text.ToString(), Ordertaxsubtotal);
                                ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);
                            }
                            else
                            {
                                OrderTax = SaleTax(ddlBillstate.SelectedItem.Text.ToString(), txtBillZipCode.Text.ToString(), Ordertaxsubtotal);
                                ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);
                            }
                        }
                        else if (chkcopy.Checked == false && ddlShipState.SelectedIndex > 0)
                        {
                            if (ddlShipState.SelectedValue.ToString() == "-11")
                            {
                                OrderTax = SaleTax(txtShipOther.Text.ToString(), txtShipZipCode.Text.ToString(), Ordertaxsubtotal);
                                ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);
                            }
                            else
                            {
                                OrderTax = SaleTax(ddlShipState.SelectedItem.Text.ToString(), txtShipZipCode.Text.ToString(), Ordertaxsubtotal);
                                ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);
                            }
                        }
                        else
                        {
                            ltrCart.Text += "$0.00";
                        }

                    }
                    SubTotal = SubTotal + OrderTax;
                    ltrCart.Text = ltrCart.Text.ToString().Replace("###customlevelDiscount###", "$" + DiscountCustomer.ToString("F2"));
                    ltrCart.Text = ltrCart.Text.ToString().Replace("###CouponCodeDiscount###", "$" + TotalCouponDiscount.ToString("F2"));
                    hdnCouponDiscount.Value = TotalCouponDiscount.ToString("F2");
                    #endregion

                    #region code for Gift Certificate By Girish

                    if (Session["GiftCertificateDiscount"] != null)
                    {
                        decimal GiftCardAmount = 0;
                        if (Session["GiftCertificateDiscountCode"] != null && Session["GiftCertificateDiscount"] != null)
                        {
                            GiftCardAmount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Balance FROM [tb_GiftCard] where storeid=" + AppLogic.AppConfigs("StoreID") + " And SerialNumber='" + Session["GiftCertificateDiscountCode"].ToString().Trim() + "'"));
                            if (GiftCardAmount == Decimal.Zero)
                            {
                                Session["GiftCertificateDiscountCode"] = null;
                                Session["GiftCertificateDiscount"] = null;

                            }
                            else
                            {
                                Session["GiftCertificateDiscountCode"] = Session["GiftCertificateDiscountCode"].ToString().Trim();
                                Session["GiftCertificateDiscount"] = Math.Round(GiftCardAmount, 2);
                            }
                        }
                        if (Session["GiftCertificateDiscount"] != null)
                        {
                            if (SubTotal <= 0)
                            {
                                Session["GiftCertificateRemaningBalance"] = Convert.ToDecimal(Session["GiftCertificateDiscount"]);
                                Session["GiftCertificateDiscount"] = Convert.ToDecimal(0);
                            }
                            else
                            {
                                GiftCardAmount = Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString());
                                if (GiftCardAmount > 0)
                                {
                                    SubTotal = SubTotal - GiftCardAmount;
                                }
                                if (SubTotal <= 0 && GiftCardAmount != Decimal.Zero)
                                {
                                    Session["GiftCertificateDiscount"] = Math.Round(Convert.ToDecimal(GiftCardAmount) - Math.Abs(Math.Round(SubTotal, 2)), 2);
                                    Session["GiftCertificateRemaningBalance"] = Math.Abs(Math.Round(SubTotal, 2));
                                }
                                else
                                {
                                    Session["GiftCertificateDiscount"] = Math.Round(GiftCardAmount, 2);
                                    Session["GiftCertificateRemaningBalance"] = "0.00";
                                }
                            }

                        }
                        ltrCart.Text = ltrCart.Text.ToString().Replace("###GiftCertiDiscount###", "$" + String.Format("{0:0.00}", Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString())));
                        ltrCart.Text = ltrCart.Text.ToString().Replace("###GiftCertiBalDiscount###", "$" + String.Format("{0:0.00}", Convert.ToDecimal(Session["GiftCertificateRemaningBalance"].ToString())));
                    }
                    #endregion

                    if (SubTotal <= 0)
                    {
                        ViewState["OrderTotal"] = 0;
                    }
                    else
                    {
                        ViewState["OrderTotal"] = SubTotal.ToString();
                    }
                    //ViewState["OrderTotal"] = SubTotal.ToString();
                    FinalTotal = Convert.ToDecimal(SubTotal.ToString());
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<th colspan=\"3\" align=\"right\" valign=\"top\">";
                    ltrCart.Text += "Total :";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "<th align=\"right\" valign=\"middle\">";
                    //ltrCart.Text += "<strong>$" + String.Format("{0:0.00}", SubTotal) + "</strong>";
                    if (SubTotal <= 0)
                    {
                        ltrCart.Text += "$0.00";
                        trPaymentMethods.Visible = false;
                    }
                    else
                    {
                        trPaymentMethods.Visible = true;
                        ltrCart.Text += "$" + String.Format("{0:0.00}", SubTotal) + "";
                    }
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "</tr>";
                    ltrCart.Text += "</tbody></table>";
                }
                else
                {
                    ltrCart.Text = "Your Cart is Empty.";
                    Session["CouponCode"] = null;
                    Session["CouponCodebycustomer"] = null;
                    Session["CouponCodeDiscountPrice"] = null;
                    Session["CustomerDiscount"] = null;
                }
                BindShoppingCartByCustomerID();
                if (FinalTotal <= 0)
                {
                    trPaymentMethods.Visible = false;
                }
                else
                {
                    trPaymentMethods.Visible = true;
                }
            }
        }

        /// <summary>
        /// Get Sales Tax By ZipCode or StateName
        /// </summary>
        /// <param name="stateName">string StateName</param>
        /// <param name="zipCode">String ZipCode</param>
        /// <param name="orderAmount">string OrderAmount</param>
        /// <returns>Returns Sale Tax as a Decimal Value</returns>
        private decimal SaleTax(string stateName, string zipCode, decimal orderAmount)
        {
            ShoppingCartComponent objTax = new ShoppingCartComponent();
            decimal salesTax = decimal.Zero;
            bool IsFreeTax = false;
            try
            {
                zipCode = txtShipZipCode.Text.ToString();
                stateName = ddlShipState.SelectedItem.Text.ToString();
            }
            catch
            {

            }
            if (ViewState["CustomerLevelID"] != null)
            {
                IsFreeTax = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasnoTax,0) AS LevelHasnoTax FROM dbo.tb_CustomerLevel WHERE CustomerLevelID=" + ViewState["CustomerLevelID"].ToString() + ""));
            }
            if (IsFreeTax)
            {

            }
            else
            {
                try
                {
                    if (zipCode.ToString().IndexOf("-") > -1)
                    {
                        string[] strzip = zipCode.ToString().Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (strzip.Length > 1)
                        {
                            if (!string.IsNullOrEmpty(strzip[0].ToString()))
                            {
                                zipCode = strzip[0].ToString();
                            }
                        }
                        zipCode = zipCode.Replace("-", "");
                    }
                }
                catch
                {

                }
               

                salesTax = Convert.ToDecimal(objTax.GetSalesTax(stateName, zipCode, orderAmount, Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
            }
            Session["SalesTax"] = salesTax.ToString();
            return salesTax;
        }

        /// <summary>
        ///  Place Order Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnPlaceOrder_Click(object sender, ImageClickEventArgs e)
        {
            CountryComponent objCountry = new CountryComponent();
            hdnZipCode.Value = txtShipZipCode.Text.ToString();
            if (ddlShipState.SelectedValue == "-11")
            {
                hdnState.Value = txtShipOther.Text.ToString();
            }
            else
            {
                hdnState.Value = ddlShipState.SelectedItem.Text.ToString();
            }
            hdncountry.Value = ddlShipCounry.SelectedValue.ToString();
            BindOrderSummary();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(hdncountry.Value.ToString()));

            if (CountryCode.ToLower() != "us" && CountryCode.ToLower() != "au" && CountryCode.ToLower() != "ca" && CountryCode.ToLower() != "gb" && CountryCode.ToString().Trim().ToUpper() != "CANADA" && CountryCode.ToString().Trim().ToUpper() != "AUSTRALIA" && CountryCode.ToString().Trim().ToUpper() != "UNITED KINGDOM" && CountryCode.ToString().Trim().ToUpper() != "UNITED STATES")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please order place by Bongo international setup.');", true);
                return;
            }


            if (rdoShippingMethod != null && rdoShippingMethod.Items.Count == 0)
            {
                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && (Session["UserName"] == null || Session["UserName"].ToString() == ""))
                {
                    GetShippingMethodByBillAddress();
                }
                BindShippingMethod();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please select Shipping Method.');", true);
                return;
            }
            else
            {
                if (rdoShippingMethod.SelectedIndex < 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please select Shipping Method.');", true);
                    return;
                }
            }
            if (Session["PaymentGateway"] == null || Session["PaymentGateway"].ToString() == "")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Payment Method Not Found.');", true);
                return;
            }
            if (Session["CustID"] != null && Session["CustID"].ToString() != null && Session["UserName"] != null && Session["UserName"].ToString() != "")
            {
                AddOrderWithLogin();
            }
            else
            {
                AddOrderWithoutLogin();
            }
        }

        /// <summary>
        /// Order Placing With Login user
        /// </summary>
        private void AddOrderWithLogin()
        {

            //txtShipEmailAddress.Text = txtGuestemail.Text;
            //txtBillEmail.Text = txtGuestemail.Text;

            //txtShipEmailAddress.Text = txtusername.Text;
            //txtBillEmail.Text = txtusername.Text;


            if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard" && trPaymentMethods.Visible == true)
            {
                if (txtNameOnCard.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter Name of Card.');", true);
                    txtNameOnCard.Focus();
                    return;
                }
                else if (ddlCardType.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Select Card Type.');", true);
                    ddlCardType.Focus();
                    return;
                }
                else if (txtCardNumber.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter Card Number.');", true);
                    txtCardNumber.Focus();
                    return;
                }
                else if (txtCSC.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter Card Verification Code.');", true);
                    txtCSC.Focus();
                    return;
                }
                if (txtCardNumber.Text.ToString().Trim() != "" && txtCardNumber.Text.ToString().Trim().IndexOf("*") < -1)
                {
                    bool chkflg = false;
                    long crdNumber = 0;
                    chkflg = long.TryParse(txtCardNumber.Text.ToString(), out crdNumber);
                    if (!chkflg)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter valid Numeric Card Number.');", true);
                        txtCardNumber.Focus();
                        return;
                    }
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && txtCardNumber.Text.ToString().Trim() != "" && txtCardNumber.Text.ToString().Trim().Length < 16)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Credit Card Number must be 16 digit long.');", true);
                    txtCardNumber.Focus();
                    return;
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && txtCardNumber.Text.ToString().Trim() != "" && (txtCardNumber.Text.ToString().Trim().Length < 15 || txtCardNumber.Text.ToString().Trim().Length > 15))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Credit Card Number must be 15 digit long.');", true);
                    txtCardNumber.Focus();
                    return;
                }

                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && txtCSC.Text.ToString().Trim() != "" && (txtCSC.Text.ToString().Trim().Length < 3 || txtCSC.Text.ToString().Trim().Length > 3))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Card Verification Code must be 4 digit long.');", true);
                    txtCSC.Focus();
                    return;
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && txtCSC.Text.ToString().Trim() != "" && txtCSC.Text.ToString().Trim().Length < 4)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Card Verification Code must be 3 digit long.');", true);
                    txtCSC.Focus();
                    return;
                }
                if (ddlMonth.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Select Month.');", true);
                    ddlMonth.Focus();
                    return;
                }
                else if (ddlYear.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Select Year.');", true);
                    ddlYear.Focus();
                    return;
                }
                else if ((Convert.ToInt32(ddlYear.SelectedValue) > DateTime.Now.Year) || (Convert.ToInt32(ddlYear.SelectedValue) == DateTime.Now.Year && Convert.ToInt32(ddlMonth.SelectedValue) >= DateTime.Now.Month))
                {

                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter Valid Expiration Date.');", true);
                    return;
                }
            }
            string strShippingName = "";
            string strShippingCharge = "0.00";
            if (rdoShippingMethod.SelectedIndex > -1)
            {
                strShippingName = rdoShippingMethod.SelectedItem.Text.ToString();

                if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                {
                    strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                    strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();

                    strShippingName = strShippingName.Substring(0, strShippingName.ToString().ToLower().IndexOf("($"));
                    strShippingName = strShippingName.ToString().Trim();
                }
            }

            BindOrderSummary();
            tb_Order objOrder = new tb_Order();
            objOrder.CustomerID = Convert.ToInt32(Session["CustID"].ToString());
            if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
            {
                objOrder.CardType = ddlCardType.SelectedItem.Text.ToString();
                objOrder.CardVarificationCode = txtCSC.Text.ToString();
                objOrder.CardName = txtNameOnCard.Text.ToString();
                objOrder.CardExpirationMonth = ddlMonth.SelectedValue.ToString();
                objOrder.CardExpirationYear = ddlYear.SelectedValue.ToString();
                if (txtCardNumber.Text.ToString().IndexOf("*") > -1)
                {
                    if (Session["CardNumber"] != null)
                    {
                        objOrder.CardNumber = SecurityComponent.Encrypt(Session["CardNumber"].ToString());
                        try
                        {
                            objOrder.Last4 = Session["CardNumber"].ToString().Substring(Session["CardNumber"].ToString().Length - 4);
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    objOrder.CardNumber = SecurityComponent.Encrypt(txtCardNumber.Text.ToString());
                    try
                    {
                        objOrder.Last4 = txtCardNumber.Text.ToString().Substring(txtCardNumber.Text.ToString().Length - 4);
                    }
                    catch
                    {

                    }
                }
            }
            objOrder.ShippingMethod = strShippingName.ToString();
            objOrder.Deleted = false;
            if (Session["Discount"] != null && Session["Discount"].ToString() != "")
            {
                //objOrder.CouponDiscountAmount = Convert.ToDecimal(Session["Discount"].ToString());
                objOrder.CouponDiscountAmount = Convert.ToDecimal(hdnCouponDiscount.Value);
            }
            else
            {
                objOrder.CouponDiscountAmount = decimal.Zero;
            }
            if (Session["QtyDiscount"] != null && Session["QtyDiscount"].ToString() != "")
            {
                objOrder.QuantityDiscountAmount = Convert.ToDecimal(Session["QtyDiscount"].ToString());
            }
            else
            {
                objOrder.QuantityDiscountAmount = decimal.Zero;
            }
            #region Code for Gift Cetificate

            if (Session["GiftCertificateDiscount"] != null && Session["GiftCertificateDiscount"].ToString() != "")
            {
                objOrder.GiftCertificateDiscountAmount = Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString());
            }
            else
            {
                objOrder.GiftCertificateDiscountAmount = decimal.Zero;
            }
            if (Session["GiftCertificateDiscountCode"] != null && Session["GiftCertificateDiscountCode"].ToString() != "")
            {
                objOrder.GiftCertiSerialNumber = Session["GiftCertificateDiscountCode"].ToString();
            }
            else
            {
                objOrder.GiftCertiSerialNumber = "";
            }
            #endregion

            if (Session["CouponCodebycustomer"] != null && Session["CouponCodebycustomer"].ToString() != "")
            {
                objOrder.CouponCode = Convert.ToString(Session["CouponCodebycustomer"].ToString());
                txtOrderNotes.Text = txtOrderNotes.Text.Trim() + " \"" + "Coupon Code # " + Session["CouponCodebycustomer"].ToString() + "\"";
            }
            else
            {
                objOrder.CouponCode = "";
            }
            objOrder.Notes = txtOrderNotes.Text.ToString();
            objOrder.OrderNotes = txtOrderNotes.Text.ToString();
            if (ViewState["OrderTotal"] != null)
            {
                objOrder.OrderTotal = Convert.ToDecimal(ViewState["OrderTotal"].ToString());
            }
            if (ViewState["SubTotal"] != null)
            {
                objOrder.OrderSubtotal = Convert.ToDecimal(ViewState["SubTotal"].ToString());
            }
            if (ViewState["strShippingCharge"] != null)
            {
                objOrder.OrderShippingCosts = Convert.ToDecimal(ViewState["strShippingCharge"].ToString());
            }
            if (Session["SalesTax"] != null)
            {
                objOrder.OrderTax = Convert.ToDecimal(Session["SalesTax"].ToString());
            }
            if (Session["PaymentMethod"] != null)
            {
                objOrder.PaymentMethod = Session["PaymentMethod"].ToString().ToUpper();
            }
            if (Session["PaymentGateway"] != null)
            {
                objOrder.PaymentGateway = Session["PaymentGateway"].ToString().ToUpper();
            }
            if (Session["CustomLevelDiscount"] != null)
            {
                objOrder.LevelDiscountAmount = Convert.ToDecimal(Session["CustomLevelDiscount"].ToString());
            }
            Int32 BillId = 0;
            Int32 ShippId = 0;
            string strupdateadress = "INSERT INTO tb_Address(LastName ,CustomerID,FirstName,Company,Address1,Address2,City,State,Suite,ZipCode,Country,Phone,Email,AddressType,StoreID) VALUES (";
            string strstate = "";
            if (ddlBillstate.SelectedItem.Text.ToString().ToLower().IndexOf("other") > -1)
            {
                strstate = txtBillother.Text.ToString();
            }
            else
            {
                strstate = ddlBillstate.SelectedItem.Text.ToString();

            }
            strupdateadress += "'" + txtBillLastname.Text.ToString().Replace("'", "''") + "'," + Session["CustID"].ToString() + ",'" + txtBillFirstname.Text.ToString().Replace("'", "''") + "','','" + txtBillAddressLine1.Text.ToString().Replace("'", "''") + "','" + txtBillAddressLine2.Text.ToString().Replace("'", "''") + "','" + txtBillCity.Text.ToString().Replace("'", "''") + "','" + strstate.Replace("'", "''") + "','" + txtBillSuite.Text.ToString().Replace("'", "''") + "','" + txtBillZipCode.Text.ToString().Replace("'", "''") + "'," + ddlBillcountry.SelectedValue.ToString().Replace("'", "''") + ",'" + txtBillphone.Text.ToString().Replace("'", "''") + "','" + txtBillEmail.Text.ToString().Replace("'", "''") + "',0,1); SELECT SCOPE_IDENTITY();";
            BillId = Convert.ToInt32(CommonComponent.GetScalarCommonData(strupdateadress));

            if (chkcopy.Checked == false)
            {
                strupdateadress = "INSERT INTO tb_Address(LastName ,CustomerID,FirstName,Company,Address1,Address2,City,State,Suite,ZipCode,Country,Phone,Email,AddressType,StoreID) VALUES (";
                strstate = "";
                if (ddlShipState.SelectedItem.Text.ToString().ToLower().IndexOf("other") > -1)
                {
                    strstate = txtShipOther.Text.ToString();
                }
                else
                {
                    strstate = ddlShipState.SelectedItem.Text.ToString();

                }
                strupdateadress += "'" + txtShipLastName.Text.ToString().Replace("'", "''") + "'," + Session["CustID"].ToString() + ",'" + txtShipFirstName.Text.ToString().Replace("'", "''") + "','','" + txtshipAddressLine1.Text.ToString().Replace("'", "''") + "','" + txtshipAddressLine2.Text.ToString().Replace("'", "''") + "','" + txtShipCity.Text.ToString().Replace("'", "''") + "','" + strstate.Replace("'", "''") + "','" + txtShipSuite.Text.ToString().Replace("'", "''") + "','" + txtShipZipCode.Text.ToString().Replace("'", "''") + "'," + ddlShipCounry.SelectedValue.ToString() + ",'" + txtShipPhone.Text.ToString().Replace("'", "''") + "','" + txtShipEmailAddress.Text.ToString().Replace("'", "''") + "',1,1); SELECT SCOPE_IDENTITY();";
                ShippId = Convert.ToInt32(CommonComponent.GetScalarCommonData(strupdateadress));
            }
            else
            {
                strupdateadress = "INSERT INTO tb_Address(LastName ,CustomerID,FirstName,Company,Address1,Address2,City,State,Suite,ZipCode,Country,Phone,Email,AddressType,StoreID) VALUES (";
                strstate = "";
                if (ddlBillstate.SelectedItem.Text.ToString().ToLower().IndexOf("other") > -1)
                {
                    strstate = txtBillother.Text.ToString();
                }
                else
                {
                    strstate = ddlBillstate.SelectedItem.Text.ToString();

                }
                strupdateadress += "'" + txtBillLastname.Text.ToString().Replace("'", "''") + "'," + Session["CustID"].ToString() + ",'" + txtBillFirstname.Text.ToString().Replace("'", "''") + "','','" + txtBillAddressLine1.Text.ToString().Replace("'", "''") + "','" + txtBillAddressLine2.Text.ToString().Replace("'", "''") + "','" + txtBillCity.Text.ToString().Replace("'", "''") + "','" + strstate.Replace("'", "''") + "','" + txtBillSuite.Text.ToString().Replace("'", "''") + "','" + txtBillZipCode.Text.ToString().Replace("'", "''") + "'," + ddlBillcountry.SelectedValue.ToString().Replace("'", "''") + ",'" + txtBillphone.Text.ToString().Replace("'", "''") + "','" + txtBillEmail.Text.ToString().Replace("'", "''") + "',1,1); SELECT SCOPE_IDENTITY();";
                ShippId = Convert.ToInt32(CommonComponent.GetScalarCommonData(strupdateadress));

            }
            if (BillId > 0 && ShippId > 0)
            {
                CommonComponent.ExecuteCommonData("UPDATE tb_Customer SET BillingAddressID=" + BillId + ",ShippingAddressID=" + ShippId + " WHERE CustomerID=" + Session["CustID"].ToString() + "");
            }

            objOrder.IsNew = true;
            objOrder.LastIPAddress = Request.UserHostAddress.ToString();
            OrderComponent objAddOrder = new OrderComponent();
            Int32 OrderNumber = 0;
            OrderNumber = Convert.ToInt32(objAddOrder.AddOrder(objOrder, OrderNumber, Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
            if (OrderNumber > 0)
            {
                string strResult = "";
                if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower().Trim() == "paypalexpress")
                {

                    if (Session["PaymentGateway"] != null && Session["PaymentGateway"].ToString().ToLower().Trim() == "paypalexpress")
                    {
                        PaypalCheckout(GetCustomerDetailsForpayment(Convert.ToInt32(Session["CustID"].ToString()), OrderNumber));

                        return;

                    }
                }
                else
                {
                    if (Session["PaymentMethod"] == null)
                    {
                        if (rdlPaymentType != null && rdlPaymentType.Items.Count > 0)
                        {

                            Session["PaymentMethod"] = Convert.ToString(rdlPaymentType.SelectedItem.Value);
                            OrderComponent objPayment = new OrderComponent();
                            DataSet dsPayment = new DataSet();
                            dsPayment = objPayment.GetPaymentGateway(Session["PaymentMethod"].ToString(), Convert.ToInt32(1));
                            if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
                            {
                                Session["PaymentGateway"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["PaymentService"].ToString());
                                Session["PaymentGatewayStatus"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString());
                            }
                        }
                    }


                    strResult = OrderPayment(Session["PaymentGateway"].ToString().ToUpper(), OrderNumber, Convert.ToDecimal(objOrder.OrderTotal), GetCustomerDetailsForpayment(Convert.ToInt32(Session["CustID"].ToString()), OrderNumber));

                }
                if (strResult.ToUpper() == "OK")
                {
                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortal_Order_QuantityAdjust] " + OrderNumber + ""));
                    try
                    {
                        if (Session["PaymentGatewayStatus"] != null && Convert.ToString(Session["PaymentGatewayStatus"]).ToLower().IndexOf("auth") > -1)
                        {
                            objAddOrder.InsertOrderlog(1, OrderNumber, "", 1);
                        }
                    }
                    catch { }

                    if (Session["GiftCertificateDiscountCode"] != null && Session["GiftCertificateDiscountCode"].ToString() != "")
                    {
                        if (Session["GiftCertificateRemaningBalance"] != null && Session["GiftCertificateRemaningBalance"].ToString() != "")
                            UpdateGiftCertificateInitalAndDiscountBalance(Session["GiftCertificateDiscountCode"].ToString(), Convert.ToDecimal(Session["GiftCertificateRemaningBalance"].ToString()));

                        if (Session["GiftCertificateDiscount"] != null && Session["GiftCertificateDiscount"].ToString() != "" && Convert.ToInt32(Session["GiftCertificateDiscount"]) != 0)
                            AddGiftCardUsage(Convert.ToInt32(Session["CustID"].ToString()), OrderNumber, Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString()), Session["GiftCertificateDiscountCode"].ToString());
                    }

                    #region CouponUsage
                    if (Session["CouponCodebycustomer"] != null)
                    {
                        CouponComponent objCoupon = new CouponComponent();
                        tb_CouponUsage tblCouponUsage = new tb_CouponUsage();
                        tblCouponUsage.CustomerID = Convert.ToInt32(Session["CustID"]);
                        tblCouponUsage.StoreID = 1;
                        tblCouponUsage.CouponCode = Convert.ToString(Session["CouponCodebycustomer"]);
                        tblCouponUsage.CreatedOn = DateTime.Now;
                        objCoupon.CreatecouponUsage(tblCouponUsage);
                    }
                    #endregion

                    objAddOrder.UpdateInventoryByOrderNumber(OrderNumber, -1);
                    string custquoteid = "";
                    if (Request.QueryString["custquoteid"] != null && Request.QueryString["custquoteid"].ToString() != "")
                    {
                         
                        if (Session["QuoteIDCompare"] != null && Session["QuoteIDCompare"].ToString().ToLower() == Request.QueryString["custquoteid"].ToString().ToLower())
                        {
                            custquoteid = SecurityComponent.Decrypt(Session["QuoteIDCompare"].ToString());
                        }
                        else
                        {
                            custquoteid = SecurityComponent.Decrypt(Request.QueryString["custquoteid"].ToString());
                        }

                        //custquoteid = SecurityComponent.Decrypt(Request.QueryString["custquoteid"].ToString());

                        CommonComponent.ExecuteCommonData("update tb_customerquote set OrderNumber=" + OrderNumber + " where CustomerQuoteID="
                            + Convert.ToInt32(custquoteid));
                    }
                    Session["PaymentGateway"] = null;
                    Session["PaymentGatewayStatus"] = null;
                    Session["SalesTax"] = null;
                    Session["CouponCode"] = null;
                    Session["Discount"] = null;
                    Session["PaymentMethod"] = null;
                    Session["ONo"] = OrderNumber.ToString();
                    Session["NoOfCartItems"] = null;
                    Session["SelectedCountry"] = null;
                    Session["SelectedZipCode"] = null;
                    Session["SelectedShippingMethod"] = null;
                    Session["CustomLevelDiscount"] = null;

                    Session["GiftCertificateDiscountCode"] = null;
                    Session["GiftCertificateDiscount"] = null;
                    Session["GiftCertificateRemaningBalance"] = null;
                    Session["CouponCodebycustomer"] = null;
                    Session["CouponCodeDiscountPrice"] = null;
                    Session["CustomerDiscount"] = null;
                    Response.Redirect("/orderreceived.aspx");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('" + strResult.ToString() + "');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Billing Email Address Already Exists, Please Enter different Email Address.');", true);
                return;
            }
        }

        /// <summary>
        /// Code For Gift Certificate Update Add Gift Card Usage
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="Amount">Decimal Amount</param>
        /// <param name="SerialNumber">String SerialNumber</param>
        public void AddGiftCardUsage(Int32 CustomerID, Int32 OrderNumber, Decimal Amount, String SerialNumber)
        {
            int GiftCardID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT GiftCardID FROM [tb_GiftCard] where storeid=" + AppLogic.AppConfigs("StoreID") + " And SerialNumber='" + SerialNumber.ToString().Trim() + "'"));
            String Query = "Insert into tb_GiftCardUsage(GiftCardID,StoreID,OrderNumber,CustomerID,Amount) values(" + GiftCardID + "," + AppLogic.AppConfigs("StoreID") + "," + OrderNumber + "," + CustomerID + "," + Amount + "); SELECT SCOPE_IDENTITY()";
            Int32 flag = Convert.ToInt32(CommonComponent.GetScalarCommonData(Query));
            if (flag > 0)
            {
                if (Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Balance FROM [tb_GiftCard] where storeid=" + AppLogic.AppConfigs("StoreID") + " And SerialNumber='" + SerialNumber.ToString().Trim() + "'")) == Decimal.Zero)
                {
                    String Query2 = "Update tb_GiftCard set Balance=0 ,Status=0 Where and storeid=" + AppLogic.AppConfigs("StoreID") + " GiftCardID=" + GiftCardID;
                    CommonComponent.ExecuteCommonData(Query);
                }
            }
        }

        /// <summary>
        /// Update Gift Certificate Initial & Discount Balance
        /// </summary>
        /// <param name="SerialNumber">string SerialNumber</param>
        /// <param name="Balance">Decimal Balance</param>
        /// <returns>Returns True if Updated</returns>
        public bool UpdateGiftCertificateInitalAndDiscountBalance(string SerialNumber, Decimal Balance)
        {
            string _Query = String.Empty;
            _Query = "Update tb_GiftCard Set Balance =" + Balance + " Where SerialNumber='" + SerialNumber + "'";
            try
            {
                CommonComponent.ExecuteCommonData(_Query);
                return true;
            }
            catch { return false; }

        }

        /// <summary>
        /// Order Placing Without Login user
        /// </summary>
        private void AddOrderWithoutLogin()
        {
            Regex reValidEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            if (chkCreateNewAccount.Checked)
            {
                if (txtCreateEmail.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter Email.');", true);
                    txtCreateEmail.Focus();
                    return;
                }
                else if (txtCreateEmail.Text != null && !reValidEmail.IsMatch(txtCreateEmail.Text))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please enter valid email.');", true);
                    txtCreateEmail.Focus();
                    return;
                }
                txtShipEmailAddress.Text = txtCreateEmail.Text;
                txtBillEmail.Text = txtCreateEmail.Text;
                txtusername.Text = txtCreateEmail.Text;
            }
            else if (chkCreateNewAccountGuest.Checked)
            {
                if (txtGuestemail.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter Email.');", true);
                    txtGuestemail.Focus();
                    return;
                }
                else if (txtGuestemail.Text != null && !reValidEmail.IsMatch(txtGuestemail.Text))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please enter valid email.');", true);
                    txtGuestemail.Focus();
                    return;
                }
                txtShipEmailAddress.Text = txtGuestemail.Text;
                txtBillEmail.Text = txtGuestemail.Text;
                txtusername.Text = txtGuestemail.Text;
            }



            if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
            {
                if (txtNameOnCard.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter Name of Card.');", true);
                    txtNameOnCard.Focus();
                    return;
                }
                else if (ddlCardType.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Select Card Type.');", true);
                    ddlCardType.Focus();
                    return;
                }
                else if (txtCardNumber.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter Card Number.');", true);
                    txtCardNumber.Focus();
                    return;
                }
                else if (txtCSC.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter Card Verification Code.');", true);
                    txtCSC.Focus();
                    return;
                }
                if (txtCardNumber.Text.ToString().Trim() != "" && txtCardNumber.Text.ToString().Trim().IndexOf("*") < -1)
                {
                    bool chkflg = false;
                    long crdNumber = 0;
                    chkflg = long.TryParse(txtCardNumber.Text.ToString(), out crdNumber);
                    if (!chkflg)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter valid Numeric Card Number');", true);
                        txtCardNumber.Focus();
                        return;
                    }
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && txtCardNumber.Text.ToString().Trim() != "" && txtCardNumber.Text.ToString().Trim().Length < 16)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Credit Card Number must be 16 digit long.');", true);
                    txtCardNumber.Focus();
                    return;
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && txtCardNumber.Text.ToString().Trim() != "" && (txtCardNumber.Text.ToString().Trim().Length < 15 || txtCardNumber.Text.ToString().Trim().Length > 15))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Credit Card Number must be 15 digit long.');", true);
                    txtCardNumber.Focus();
                    return;
                }

                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && txtCSC.Text.ToString().Trim() != "" && (txtCSC.Text.ToString().Trim().Length < 3 || txtCSC.Text.ToString().Trim().Length > 3))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Card Verification Code must be 4 digit long.');", true);
                    txtCSC.Focus();
                    return;
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && txtCSC.Text.ToString().Trim() != "" && txtCSC.Text.ToString().Trim().Length < 4)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Card Verification Code must be 3 digit long.');", true);
                    txtCSC.Focus();
                    return;
                }
                if (ddlMonth.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Select Month.');", true);
                    ddlMonth.Focus();
                    return;
                }
                else if (ddlYear.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Select Year.');", true);
                    ddlYear.Focus();
                    return;
                }
                else if ((Convert.ToInt32(ddlYear.SelectedValue) > DateTime.Now.Year) || (Convert.ToInt32(ddlYear.SelectedValue) == DateTime.Now.Year && Convert.ToInt32(ddlMonth.SelectedValue) >= DateTime.Now.Month))
                {

                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Please Enter Valid Expiration Date.');", true);
                    return;
                }
            }
            string strShippingName = "";
            string strShippingCharge = "0.00";
            if (rdoShippingMethod.SelectedIndex > -1)
            {
                strShippingName = rdoShippingMethod.SelectedItem.Text.ToString();

                if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                {
                    strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                    strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();

                    strShippingName = strShippingName.Substring(0, strShippingName.ToString().ToLower().IndexOf("($"));
                    strShippingName = strShippingName.ToString().Trim();
                }
            }

            BindOrderSummary();
            tb_Order objOrder = new tb_Order();
            objOrder.CustomerID = Convert.ToInt32(Session["CustID"].ToString());
            objOrder.FirstName = txtBillFirstname.Text.ToString();
            objOrder.LastName = txtBillLastname.Text.ToString();
            objOrder.Email = txtBillEmail.Text.ToString();
            objOrder.BillingFirstName = txtBillFirstname.Text.ToString();
            objOrder.BillingLastName = txtBillLastname.Text.ToString();
            objOrder.BillingEmail = txtBillEmail.Text.ToString();
            objOrder.BillingAddress1 = txtBillAddressLine1.Text.ToString();
            objOrder.BillingAddress2 = txtBillAddressLine2.Text.ToString();
            objOrder.BillingCity = txtBillCity.Text.ToString();
            try
            {


                if (ddlBillstate.SelectedValue.ToString() == "-11")
                {
                    objOrder.BillingState = txtBillother.Text.ToString();
                }
                else
                {
                    objOrder.BillingState = ddlBillstate.SelectedItem.Text.ToString();
                }
            }
            catch
            {

            }
            objOrder.BillingCountry = ddlBillcountry.SelectedItem.Text.ToString();
            objOrder.BillingPhone = txtBillphone.Text.ToString();
            objOrder.BillingZip = txtBillZipCode.Text.ToString();
            objOrder.BillingSuite = txtBillSuite.Text.ToString();
            objOrder.BillingCompany = "";
            Session["BillEmail"] = Convert.ToString(txtBillEmail.Text.ToString());
            //if (UseShippingAddress.Checked == false)
            //{
            //    objOrder.ShippingFirstName = txtBillFirstname.Text.ToString();
            //    objOrder.ShippingLastName = txtBillLastname.Text.ToString();
            //    objOrder.ShippingEmail = txtBillEmail.Text.ToString();
            //    objOrder.ShippingAddress1 = txtBillAddressLine1.Text.ToString();
            //    objOrder.ShippingAddress2 = txtBillAddressLine2.Text.ToString();
            //    objOrder.ShippingCity = txtBillCity.Text.ToString();
            //    objOrder.ShippingState = ddlBillstate.SelectedItem.Text.ToString();
            //    objOrder.ShippingCountry = ddlBillcountry.SelectedItem.Text.ToString();
            //    objOrder.ShippingPhone = txtBillphone.Text.ToString();
            //    objOrder.ShippingZip = txtBillZipCode.Text.ToString();
            //    objOrder.ShippingSuite = txtBillSuite.Text.ToString();
            //    objOrder.ShippingCompany = "";
            //}
            //else
            //{
            objOrder.ShippingFirstName = txtShipFirstName.Text.ToString();
            objOrder.ShippingLastName = txtShipLastName.Text.ToString();
            objOrder.ShippingEmail = txtShipEmailAddress.Text.ToString();
            objOrder.ShippingAddress1 = txtshipAddressLine1.Text.ToString();
            objOrder.ShippingAddress2 = txtshipAddressLine2.Text.ToString();
            objOrder.ShippingCity = txtShipCity.Text.ToString();
            try
            {


                if (ddlShipState.SelectedValue.ToString() == "-11")
                {
                    objOrder.ShippingState = txtShipOther.Text.ToString();
                }
                else
                {
                    objOrder.ShippingState = ddlShipState.SelectedItem.Text.ToString();
                }
            }
            catch
            {

            }
            objOrder.ShippingCountry = ddlShipCounry.SelectedItem.Text.ToString();
            objOrder.ShippingPhone = txtShipPhone.Text.ToString();
            objOrder.ShippingZip = txtShipZipCode.Text.ToString();
            objOrder.ShippingSuite = txtShipSuite.Text.ToString();
            objOrder.ShippingCompany = "";
            //}

            if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
            {
                objOrder.CardType = ddlCardType.SelectedItem.Text.ToString();
                objOrder.CardVarificationCode = txtCSC.Text.ToString();
                objOrder.CardName = txtNameOnCard.Text.ToString();
                objOrder.CardExpirationMonth = ddlMonth.SelectedValue.ToString();
                objOrder.CardExpirationYear = ddlYear.SelectedValue.ToString();
                objOrder.CardNumber = SecurityComponent.Encrypt(txtCardNumber.Text.ToString());
                try
                {
                    objOrder.Last4 = txtCardNumber.Text.ToString().Substring(txtCardNumber.Text.ToString().Length - 4);
                }
                catch
                {

                }

            }
            objOrder.ShippingMethod = strShippingName.ToString();
            objOrder.Deleted = false;
            if (Session["Discount"] != null && Session["Discount"].ToString() != "")
            {
                objOrder.CouponDiscountAmount = Convert.ToDecimal(Session["Discount"].ToString());
            }
            else
            {
                objOrder.CouponDiscountAmount = decimal.Zero;
            }
            if (Session["CouponCodebycustomer"] != null && Session["CouponCodebycustomer"].ToString() != "")
            {
                objOrder.CouponCode = Convert.ToString(Session["CouponCodebycustomer"].ToString());
                txtOrderNotes.Text = txtOrderNotes.Text.Trim() + " \"" + "Coupon Code # " + Session["CouponCodebycustomer"].ToString() + "\"";
            }
            else
            {
                objOrder.CouponCode = "";
            }
            objOrder.Notes = txtOrderNotes.Text.ToString();
            objOrder.OrderNotes = txtOrderNotes.Text.ToString();
            if (ViewState["OrderTotal"] != null)
            {
                objOrder.OrderTotal = Convert.ToDecimal(ViewState["OrderTotal"].ToString());
            }
            if (ViewState["SubTotal"] != null)
            {
                objOrder.OrderSubtotal = Convert.ToDecimal(ViewState["SubTotal"].ToString());
            }
            if (ViewState["strShippingCharge"] != null)
            {
                objOrder.OrderShippingCosts = Convert.ToDecimal(ViewState["strShippingCharge"].ToString());
            }
            if (Session["SalesTax"] != null)
            {
                objOrder.OrderTax = Convert.ToDecimal(Session["SalesTax"].ToString());
            }
            if (Session["PaymentMethod"] != null)
            {
                objOrder.PaymentMethod = Session["PaymentMethod"].ToString().ToUpper();
            }
            if (Session["PaymentGateway"] != null)
            {
                objOrder.PaymentGateway = Session["PaymentGateway"].ToString().ToUpper();
            }
            objOrder.IsNew = true;
            objOrder.LastIPAddress = Request.UserHostAddress.ToString();
            OrderComponent objAddOrder = new OrderComponent();
            Int32 OrderNumber = 0;
            Int32 CustId = 0;
            if (chkCreateNewAccountGuest.Checked)
            {

            }
            else
            {
                CustId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Count(CustomerId),0) as TotCust FROM tb_Customer WHERE isnull(Email,'')='" + txtusername.Text.ToString() + "' AND StoreID = " + Convert.ToInt32(AppLogic.AppConfigs("StoreId")) + " AND isnull(Active,0)=1 AND isnull(IsRegistered,0)=1"));
            }
            if (CustId == 0)
            {
                OrderNumber = Convert.ToInt32(objAddOrder.AddOrder(objOrder, OrderNumber, Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
                if (OrderNumber > 0)
                {
                    //   objOrder.CardNumber = SecurityComponent.Decrypt(objOrder.CardNumber.ToString());
                    //  string strResult = OrderPayment(Session["PaymentGateway"].ToString().ToUpper(), OrderNumber, Convert.ToDecimal(objOrder.OrderTotal), objOrder);


                    string strResult = "";
                    if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower().Trim() == "paypalexpress")
                    {
                        if (Session["PaymentGateway"] != null && Session["PaymentGateway"].ToString().ToLower().Trim() == "paypalexpress")
                        {
                            PaypalCheckout(GetCustomerDetailsForpayment(Convert.ToInt32(Session["CustID"].ToString()), OrderNumber));
                            return;
                        }
                    }
                    else
                    {
                        if (Session["PaymentMethod"] == null)
                        {
                            if (rdlPaymentType != null && rdlPaymentType.Items.Count > 0)
                            {

                                Session["PaymentMethod"] = Convert.ToString(rdlPaymentType.SelectedItem.Value);
                                OrderComponent objPayment = new OrderComponent();
                                DataSet dsPayment = new DataSet();
                                dsPayment = objPayment.GetPaymentGateway(Session["PaymentMethod"].ToString(), Convert.ToInt32(1));
                                if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
                                {
                                    Session["PaymentGateway"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["PaymentService"].ToString());
                                    Session["PaymentGatewayStatus"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString());
                                }
                            }
                        }

                        if (objOrder.CardNumber != null && Convert.ToString(objOrder.CardNumber) != "")
                            objOrder.CardNumber = SecurityComponent.Decrypt(objOrder.CardNumber.ToString());

                        strResult = OrderPayment(Session["PaymentGateway"].ToString().ToUpper(), OrderNumber, Convert.ToDecimal(objOrder.OrderTotal), objOrder);
                    }


                    Boolean UserCreated = false;
                    if (chkCreateNewAccount.Checked)
                    {
                        CreateCustomerAndSendMail(Convert.ToInt32(Session["CustID"].ToString()), OrderNumber);
                        UserCreated = true;
                    }
                    if (strResult.ToUpper() == "OK")
                    {

                        #region CouponUsage
                        if (Session["CouponCodebycustomer"] != null)
                        {
                            CouponComponent objCoupon = new CouponComponent();
                            tb_CouponUsage tblCouponUsage = new tb_CouponUsage();
                            tblCouponUsage.CustomerID = Convert.ToInt32(Session["CustID"]);
                            tblCouponUsage.StoreID = 1;
                            tblCouponUsage.CouponCode = Convert.ToString(Session["CouponCodebycustomer"]);
                            tblCouponUsage.CreatedOn = DateTime.Now;
                            objCoupon.CreatecouponUsage(tblCouponUsage);
                        }
                        #endregion

                        string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortal_Order_QuantityAdjust] " + OrderNumber + ""));

                        try
                        {
                            if (Session["PaymentGatewayStatus"] != null && Convert.ToString(Session["PaymentGatewayStatus"]).ToLower().IndexOf("auth") > -1)
                            {
                                objAddOrder.InsertOrderlog(1, OrderNumber, "", 1);
                            }
                        }
                        catch { }

                        string custquoteid = "";
                        if (Request.QueryString["custquoteid"] != null && Request.QueryString["custquoteid"].ToString() != "")
                        {
                           
                            if (Session["QuoteIDCompare"] != null && Session["QuoteIDCompare"].ToString().ToLower() == Request.QueryString["custquoteid"].ToString().ToLower())
                            {
                                custquoteid = SecurityComponent.Decrypt(Session["QuoteIDCompare"].ToString());
                            }
                            else
                            {
                                custquoteid = SecurityComponent.Decrypt(Request.QueryString["custquoteid"].ToString());
                            }
                            //custquoteid = SecurityComponent.Decrypt(Request.QueryString["custquoteid"].ToString());

                            CommonComponent.ExecuteCommonData("update tb_customerquote set OrderNumber=" + OrderNumber + " where CustomerQuoteID="
                                + Convert.ToInt32(custquoteid));
                        }
                        Session["PaymentGateway"] = null;
                        Session["PaymentGatewayStatus"] = null;
                        Session["SalesTax"] = null;
                        Session["CouponCode"] = null;
                        Session["Discount"] = null;
                        Session["PaymentMethod"] = null;
                        Session["ONo"] = OrderNumber.ToString();
                        Session["NoOfCartItems"] = null;
                        Session["SelectedCountry"] = null;
                        Session["SelectedZipCode"] = null;
                        Session["SelectedShippingMethod"] = null;

                        Session["CustomerDiscount"] = null;

                        Session["CouponCodebycustomer"] = null;
                        Session["CouponCodeDiscountPrice"] = null;
                        if (UserCreated == true)
                        {
                            Response.Redirect("/orderreceived.aspx?UserCreated=true");
                        }
                        else
                        {
                            Response.Redirect("/orderreceived.aspx");
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('" + strResult.ToString() + "');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Billing Email Address Already Exists, Please Enter different Email Address.');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "alert('Contact Email Address Already Exists, Please Enter different Email Address.');", true);
                return;
            }
        }

        /// <summary>
        /// Send mail to Register with Password Customer
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        public void CreateCustomerAndSendMail(Int32 CustomerID, Int32 OrderNumber)
        {
            CustomerComponent objCustomer = new CustomerComponent();
            Int32 IsAdded = Convert.ToInt32(objCustomer.AddCustomerAfterorderPlaced(Convert.ToInt32(OrderNumber.ToString()), txtusername.Text.ToString(), SecurityComponent.Encrypt(txtCreateNewPassword.Text.ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreId")), Convert.ToInt32(CustomerID)));
            if (IsAdded > 0)
            {
                objCustomer = new CustomerComponent();
                DataSet dsCreateAccount = new DataSet();
                dsCreateAccount = objCustomer.GetEmailTamplate("CreateAccount", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                //olution.Bussines.Entities.tb_Customer objCustData = objCustomer.GetCustomerDataByID(Convert.ToInt32(Session["CustID"]));
                string StrName = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(FirstName,'') + ' ' + ISNULL(LastName,'') as Name from tb_Customer where CustomerID =" + Convert.ToInt32(Session["CustID"]) + ""));
                if (dsCreateAccount != null && dsCreateAccount.Tables.Count > 0 && dsCreateAccount.Tables[0].Rows.Count > 0)
                {
                    String strBody = "";
                    String strSubject = "";
                    strBody = dsCreateAccount.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsCreateAccount.Tables[0].Rows[0]["Subject"].ToString();

                    if (strSubject.Contains("###LIVE_SERVER###"))
                    {
                        strSubject = Regex.Replace(strSubject, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                    }
                    strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                    if (strBody.Contains("###LIVE_SERVER###"))
                    {
                        strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###USERNAME###"))
                    {
                        strBody = Regex.Replace(strBody, "###USERNAME###", txtusername.Text.ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###PASSWORD###"))
                    {
                        strBody = Regex.Replace(strBody, "###PASSWORD###", txtCreateNewPassword.Text.ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###YEAR###"))
                    {
                        strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###STOREPATH###"))
                    {
                        strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###FIRSTNAME###"))
                    {
                        //strBody = Regex.Replace(strBody, "###FIRSTNAME###", objCustData.FirstName.ToString() + " " + objCustData.LastName.ToString(), RegexOptions.IgnoreCase);
                        strBody = Regex.Replace(strBody, "###FIRSTNAME###", StrName.ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###STORENAME###"))
                    {
                        strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###StoreID###"))
                    {
                        strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);
                    }

                    try
                    {
                        AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                        CommonOperations.SendMail(txtusername.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                    }
                    catch { }
                }

                Response.Cookies.Add(new System.Web.HttpCookie("ecommcustomer", null));
                txtusername.Text = "";
                txtCreateNewPassword.Text = "";
                txtConfirmPassWord.Text = "";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your Account has been created successfully.');", true);
            }
        }

        /// <summary>
        /// Order Payment Transaction
        /// </summary>
        /// <param name="PayementGateWay">String PayementGateWay</param>
        /// <param name="OrderNumber">String OrderNumber</param>
        /// <returns>Returns the result as a String according to execution</returns>
        private string OrderPayment(string PayementGateWay, Int32 OrderNumber, decimal orderTotal, tb_Order objorder)
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

            if (orderTotal > decimal.Zero)
            {
                if (Session["PaymentMethod"] == null)
                {
                    if (rdlPaymentType != null && rdlPaymentType.Items.Count > 0)
                    {

                        Session["PaymentMethod"] = Convert.ToString(rdlPaymentType.SelectedItem.Value);
                        OrderComponent objPayment = new OrderComponent();
                        DataSet dsPayment = new DataSet();
                        dsPayment = objPayment.GetPaymentGateway(Session["PaymentMethod"].ToString(), Convert.ToInt32(1));
                        if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
                        {
                            Session["PaymentGateway"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["PaymentService"].ToString());
                            Session["PaymentGatewayStatus"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString());
                        }
                    }
                }

                if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower().Trim() == "creditcard")
                {
                    if (Session["PaymentGateway"] != null && Session["PaymentGateway"].ToString().ToLower().Trim() == "linkpoint")
                    {
                        Status = "OK";
                        //Status = objLinkpoint.ProcessCard(OrderNumber, Session["PaymentGatewayStatus"].ToString().ToLower(), "", "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                    else if (Session["PaymentGateway"] != null && Session["PaymentGateway"].ToString().ToLower().Trim() == "paypal")
                    {
                        PayPalComponent objPaypal = new PayPalComponent();
                        Status = objPaypal.ProcessCardForClientSide(OrderNumber, Convert.ToInt32(Session["CustID"].ToString()), orderTotal, AppLogic.AppConfigBool("UseLiveTransactions"), Session["PaymentGatewayStatus"].ToString(), objorder, "", objorder, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                    else if (Session["PaymentGateway"] != null && Session["PaymentGateway"].ToString().ToLower().Trim() == "authorizenet")
                    {
                        if (Convert.ToBoolean(AppLogic.AppConfigBool("UseLiveTransactions")) == false)
                        {
                            Status = "OK";
                        }
                        else
                        {
                            Status = AuthorizeNetComponent.ProcessCardForClientSide(OrderNumber, Convert.ToInt32(Session["CustID"].ToString()), orderTotal, AppLogic.AppConfigBool("UseLiveTransactions"), Session["PaymentGatewayStatus"].ToString(), objorder, objorder, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                        }
                    }
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
                objOrder.CustomerID = Convert.ToInt32(Session["CustID"].ToString());
                objOrder.AVSResult = AVSResult;
                objOrder.AuthorizationResult = AuthorizationResult;
                objOrder.AuthorizationCode = AuthorizationCode;
                objOrder.AuthorizationPNREF = AuthorizationTransID;
                //objOrder.TransactionCommand = TransactionCommand;
                objOrder.TransactionCommand = "";

                if (Session["PaymentGatewayStatus"] != null && Session["PaymentGatewayStatus"].ToString().ToLower().IndexOf("auth") > -1)
                {
                    if (ViewState["OrderTotal"] != null && Convert.ToDecimal(ViewState["OrderTotal"]) == 0)
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
                Int32 updateOrder = Convert.ToInt32(objUpdateOrder.AddOrder(objOrder, OrderNumber, Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
                CommonComponent.ExecuteCommonData("UPDATE tb_Order SET AuthorizedAmount='" + orderTotal.ToString() + "' WHERE  OrderNumber=" + OrderNumber.ToString() + "");



                int totalItemCount = RptCartItems.Items.Count;
                for (int i = 0; i < totalItemCount; i++)
                {

                    HiddenField hdnProductId = (HiddenField)RptCartItems.Items[i].FindControl("hdnProductId");
                    Label lblPrice = (Label)RptCartItems.Items[i].FindControl("lblPrice");
                    Label lblDiscountprice = (Label)RptCartItems.Items[i].FindControl("lblDiscountprice");
                    TextBox txtQty = (TextBox)RptCartItems.Items[i].FindControl("txtQty");
                    Literal ltrSubTotal = (Literal)RptCartItems.Items[i].FindControl("ltrSubTotal");

                    Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + hdnProductId.Value.ToString() + " and ItemType='Swatch'"));
                    if (Isorderswatch == 1)
                    {
                        try
                        {
                            Decimal strpp = Convert.ToDecimal(lblPrice.Text.ToString().Replace("$", "").Trim());
                            Decimal strdiscount = Convert.ToDecimal(lblDiscountprice.Text.ToString().Replace("$", "").Trim());
                            Decimal decSubTotal = Convert.ToDecimal(ltrSubTotal.Text.ToString().Replace("$", "").Trim());
                            if (strpp > Decimal.Zero)
                            {
                                if (decSubTotal == strpp)
                                {
                                    strpp = strpp / Convert.ToDecimal(txtQty.Text.ToString());
                                }
                            }
                            if (strdiscount > Decimal.Zero)
                            {
                                if (decSubTotal == strdiscount)
                                {
                                    strdiscount = strdiscount / Convert.ToDecimal(txtQty.Text.ToString());
                                }
                            }
                            CommonComponent.ExecuteCommonData("UPDATE tb_OrderedShoppingCartItems SET Price='" + string.Format("{0:0.00}", strpp) + "',DiscountPrice='" + string.Format("{0:0.00}", strdiscount) + "' WHERE RefProductid=" + hdnProductId.Value.ToString() + " AND OrderedShoppingCartID in (SELECT ShoppingCardID FROM tb_order WHERE OrderNumber=" + OrderNumber + ")");
                        }
                        catch { }
                    }
                }


                SendMail(OrderNumber);
            }
            else
            {
                //delete failed transaction order from tb_order
                tb_Order objtbOrder = new tb_Order();
                objtbOrder = objDsorder.GetOrderByOrderNumber(Convert.ToInt32(OrderNumber));
                objtbOrder.Deleted = true;
                int RowsAffected = objDsorder.UpdateOrder(objtbOrder);
                //order deleted

                objDsorder = new OrderComponent();
                tb_FailedTransaction objFailed = new tb_FailedTransaction();
                objFailed.OrderNumber = Convert.ToInt32(OrderNumber);
                objFailed.CustomerID = Convert.ToInt32(Session["CustID"].ToString());
                objFailed.PaymentGateway = Convert.ToString(Session["PaymentGateway"].ToString());
                objFailed.Paymentmethod = Convert.ToString(Session["PaymentMethod"].ToString());
               // objFailed.TransactionCommand = Convert.ToString(TransactionCommand);
                objFailed.TransactionCommand = "";
                objFailed.TransactionResult = Convert.ToString(TransactionResponse);
                objFailed.OrderDate = DateTime.Now;
                objFailed.IPAddress = Request.UserHostAddress.ToString();
                Int32 FaileId = Convert.ToInt32(objDsorder.AddOrderFailedTransaction(objFailed, Convert.ToInt32(1)));
                CommonComponent.ExecuteCommonData("Update tb_Order set CardVarificationCode='',CardNumber='' where OrderNumber=" + OrderNumber + "");
                SendMailForFailedTransaction(OrderNumber);
            }

            return Status;
        }

        /// <summary>
        /// Sends the Mail for Failed Transaction.
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        public void SendMailForFailedTransaction(Int32 OrderNumber)
        {
            string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
            string Body = "";
            string url = AppLogic.AppConfigs("LIVE_SERVER") + "/" + "FailedTransactionEmail.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
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
                CommonOperations.SendMail(ToID, "Failed Order Transaction for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
            }
            catch { }
        }

        /// <summary>
        /// Order Receipt Send To Customer & Admin
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNUmber</param>
        public void SendMail(Int32 OrderNumber)
        {
            try
            {
                string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
                string Body = "";
                //      string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + "invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
                string url = AppLogic.AppConfigs("LIVE_SERVER") + "/" + "Invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
                WebRequest NewWebReq = WebRequest.Create(url);
                WebResponse newWebRes = NewWebReq.GetResponse();
                string format = newWebRes.ContentType;
                Stream ftprespstrm = newWebRes.GetResponseStream();
                StreamReader reader;
                reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
                Body = reader.ReadToEnd().ToString();
                Body = Body.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"display:none;\"");

                Body = Body.Replace("title=\"Facebook\"", "title=\"Facebook\" style=\"display:none;\"");
                Body = Body.Replace("title=\"Twitter\"", "title=\"Twitter\" style=\"display:none;\"");
                Body = Body.Replace("title=\"Pinterest\"", "title=\"Pinterest\" style=\"display:none;\"");
                Body = Body.Replace("title=\"Google Plus\"", "title=\"Google Plus\" style=\"display:none;\"");
                Body = Body.Replace("id=\"trHeaderMenu\"", "id=\"trHeaderMenu\" style=\"display:none;\"");
                Body = Body.Replace("id=\"trStoreBanner\"", "id=\"trStoreBanner\" style=\"display:none;\"");


                AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

                try
                {
                    CommonOperations.SendMail(ToID, "New Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                }
                catch { }
                try
                {
                    if (ViewState["BillEmail"] != null)
                    {
                        CommonOperations.SendMail(ViewState["BillEmail"].ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                    }
                    else
                    {
                        CommonOperations.SendMail(txtBillEmail.Text.ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                    }
                }
                catch { }
            }
            catch
            {
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
        /// Billing Country Drop Down Selected Index Changed Event
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
                    ddlBillstate.SelectedIndex = 0;
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
                ddlBillstate.Items.Insert(1, new ListItem("Other", "-11"));
                ddlBillstate.SelectedIndex = 0;
            }
            if (chkcopy.Checked)
            {
                ddlShipCounry.ClearSelection();
                ddlShipCounry.SelectedIndex = ddlBillcountry.SelectedIndex;
                ddlShipCounry_SelectedIndexChanged(null, null);
            }
            //if (UseShippingAddress.Checked)
            //{
            //    pnlShippingDetails.Attributes.Add("style", "display:''");
            //}
            //else
            //{
            //    pnlShippingDetails.Attributes.Add("style", "display:''");
            //}
            /* 11-March-2014
            if (UseShippingAddress.Checked == false)
            {
                GetShippingMethodByBillAddress();
                BindShippingMethod();
                BindOrderSummary();
            }*/
            StateOtherSelection();
        }

        /// <summary>
        /// Shipping Country Drop Down Selected Index Changed Event
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
                    ddlShipState.SelectedIndex = 0;
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
                ddlShipState.Items.Insert(1, new ListItem("Other", "-11"));
                ddlShipState.SelectedIndex = 0;
            }
            //if (UseShippingAddress.Checked)
            //{
            //    pnlShippingDetails.Attributes.Add("style", "display:''");
            //}
            //else
            //{
            //    pnlShippingDetails.Attributes.Add("style", "display:none");
            //}
            //if (UseShippingAddress.Checked)
            //{



            /*  GetShippingMethodByShipAddress();
              BindShippingMethod();
              BindOrderSummary();
              //}
              StateOtherSelection();*/
            try
            {
                if (tempbillid != null && tempbillid.Value.ToString().Trim() != "")
                {


                    ddlShipState.SelectedIndex = Convert.ToInt32(tempbillid.Value.ToString().Trim());
                    tempbillid.Value = "";
                    if (ddlShipState.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("other") > -1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mkBillingothrStatenew", "MakeBillingOtherVisible();MakeShippingOtherVisible();", true);
                    }

                }
            }
            catch { }
            btnReloadShipping.Attributes.Add("Style", "display:block");
            btnReloadShipping_Click(null, null);

        }



        protected void btnReloadShipping_Click(object sender, ImageClickEventArgs e)
        {

            GetShippingMethodByShipAddress();
            BindShippingMethod();
            BindOrderSummary();

        }

        /// <summary>
        /// Bind Shipping Method By ZipCode
        /// </summary>
        /// <param name="Country">String CountryCode</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Weight">Decimal Weight</param>
        private void BindShippingMethod()
        {
            bool CheckFreeShaippingOrder = false;
            string strUSPSMessage = "";
            string strUPSMessage = "";
            string strFedexSMessage = "";
            lblMsg.Text = "";
            StateComponent objState = new StateComponent();
            string stateName = Convert.ToString(objState.GetStateCodeByName(hdnState.Value.ToString()));
            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(hdncountry.Value.ToString()));
            rdoShippingMethod.Items.Clear();
            double Price = 0;
            decimal OrderTotal = 0;
            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));

            DataTable ShippingTable1 = new DataTable();
            ShippingTable1.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable1.Columns.Add("Price", typeof(decimal));
            if (hdncountry.Value.ToString() == "0" || hdncountry.Value.ToString().Trim() == "")
            {
                return;
            }

            if (ViewState["FinalTotal"] != null)
            {
                if (ViewState["ShippingChargesBind"] != null)
                {
                    //if (OrderTotal >= Convert.ToDecimal(ViewState["FinalTotal"].ToString()))
                    //{
                    OrderTotal = Convert.ToDecimal(ViewState["FinalTotal"].ToString());
                    OrderTotal = OrderTotal - Convert.ToDecimal(ViewState["ShippingChargesBind"].ToString());
                    ViewState["ShippingChargesBind"] = null;
                    if (OrderTotal <= 0)
                    {
                        OrderTotal = 0;
                    }
                    //}

                }
                else
                {
                    OrderTotal = Convert.ToDecimal(ViewState["FinalTotal"].ToString());
                }
            }
            if ((CountryCode.ToString().Trim().ToUpper() == "US" || CountryCode.ToString().Trim().ToUpper() == "UNITED STATES"))
            {

                //lblMsg.Visible = true;
                //lblMsg.Text = "Select State/Province and Enter Zip Code";
                string strfreeshipping = Convert.ToString(CommonComponent.GetScalarCommonData("Select isnull(configvalue,'0') from tb_AppConfig WHERE Configname ='FreeShippingLimit' and isnull(Deleted,0)=0 and StoreId=1"));
                if (OrderTotal >= 0)
                {
                    if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(strfreeshipping))
                    {
                        Price = 0;
                        lblFreeShippningMsg.Text = "Congratulations!! You qualified for Free Shipping. ( United States Only )";
                    }
                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(0) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(29.99))
                    {
                        Price = 5.99;
                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                    }
                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(30.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(69.99))
                    {
                        Price = 7.99;
                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                    }
                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(70.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(129.99))
                    {
                        Price = 12.99;
                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                    }
                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(130.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(199.99))
                    {
                        Price = 16.99;
                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                    }
                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(200.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(248.99))
                    {
                        Price = 21.99;
                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                    }
                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(150) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(199.99))
                    //{
                    //    Price = 17.99;
                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                    //}
                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(200) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(248.99))
                    //{
                    //    Price = 19.99;

                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                    //}

                    DataRow dr;
                    dr = ShippingTable1.NewRow();
                    dr["ShippingMethodName"] = "Ground($" + string.Format("{0:0.00}", Convert.ToDecimal(Price)) + ")";
                    dr["Price"] = Convert.ToDecimal(Price);
                    ShippingTable1.Rows.Add(dr);
                    if (ShippingTable1 != null && ShippingTable1.Rows.Count > 0)
                    {

                        DataView dvShipping = ShippingTable1.DefaultView;
                        dvShipping.Sort = "Price asc";
                        rdoShippingMethod.DataSource = dvShipping.ToTable();
                        rdoShippingMethod.DataTextField = "ShippingMethodName";
                        rdoShippingMethod.DataValueField = "ShippingMethodName";
                        rdoShippingMethod.DataBind();
                    }
                    if (Session["SelectedShippingMethod"] != null && Convert.ToString(Session["SelectedShippingMethod"]) != "")
                    {
                        if (rdoShippingMethod.Items.Count > 0)
                        {
                            rdoShippingMethod.ClearSelection();
                            for (int i = 0; i < rdoShippingMethod.Items.Count; i++)
                            {
                                if (Convert.ToString(rdoShippingMethod.Items[i].Text) == Convert.ToString(Session["SelectedShippingMethod"]))
                                {
                                    rdoShippingMethod.Items[i].Selected = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (rdoShippingMethod.Items.Count > 0 && rdoShippingMethod.SelectedIndex <= -1)
                    {
                        rdoShippingMethod.SelectedIndex = 0;
                    }

                    BindOrderSummary();

                }
                if (hdnZipCode.Value.ToString() == "" || stateName == "")
                {
                    return;
                }
            }

            #region Code for Gift Certificate

            DataSet dsShoppingCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));
            int GiftCartCount = 0;
            if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
            {
                int GiftCardProductID = 0;
                for (int i = 0; i < dsShoppingCart.Tables[0].Rows.Count; i++)
                {
                    GiftCardProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(GiftCardProductID,0) FROM dbo.tb_GiftCardProduct Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + Convert.ToInt32(dsShoppingCart.Tables[0].Rows[i]["ProductId"].ToString()) + ""));
                    if (GiftCardProductID > 0)
                    {
                        GiftCartCount = GiftCartCount + 1;
                    }
                }

                if (GiftCartCount == dsShoppingCart.Tables[0].Rows.Count)
                {
                    IsGiftCartCount = true;
                }
                else
                    IsGiftCartCount = false;
            }

            #endregion

            ShippingComponent objShipping = new ShippingComponent();
            DataSet objShipServices = new DataSet();
            objShipServices = objShipping.GetShippingServices(Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            decimal Weight = decimal.Zero;


            if (ViewState["Weight"] != null)
            {
                Weight = Convert.ToDecimal(ViewState["Weight"].ToString());
            }
            if (Weight == 0)
            {
                Weight = 1;

            }



            DataTable UPSTable = new DataTable();
            UPSTable.Columns.Add("ShippingMethodName", typeof(String));
            UPSTable.Columns.Add("Price", typeof(decimal));


            DataTable USPSTable = new DataTable();
            USPSTable.Columns.Add("ShippingMethodName", typeof(String));
            USPSTable.Columns.Add("Price", typeof(decimal));

            DataTable FedexTable = new DataTable();
            FedexTable.Columns.Add("ShippingMethodName", typeof(String));
            FedexTable.Columns.Add("Price", typeof(decimal));


            #region Code for Gift Certificate

            if (IsGiftCartCount == true)
            {
                String strFreeShipping = "Standard Shipping($0.00)";
                DataRow dataRow = ShippingTable.NewRow();
                dataRow["ShippingMethodName"] = strFreeShipping;
                dataRow["Price"] = 0;
                ShippingTable.Rows.Add(dataRow);
                CheckFreeShaippingOrder = true;
                if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                {
                    DataView dvShipping = ShippingTable.DefaultView;
                    dvShipping.Sort = "Price asc";
                    rdoShippingMethod.DataSource = dvShipping.ToTable();
                    rdoShippingMethod.DataTextField = "ShippingMethodName";
                    rdoShippingMethod.DataValueField = "ShippingMethodName";
                    rdoShippingMethod.DataBind();

                    if (Session["SelectedShippingMethod"] != null && Convert.ToString(Session["SelectedShippingMethod"]) != "")
                    {
                        if (rdoShippingMethod.Items.Count > 0)
                        {
                            rdoShippingMethod.ClearSelection();
                            for (int i = 0; i < rdoShippingMethod.Items.Count; i++)
                            {
                                String[] strSplitValue = rdoShippingMethod.Items[i].Text.ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                String[] strSplitSessionValue = Session["SelectedShippingMethod"].ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (strSplitValue[0] != null)
                                {
                                    if (Convert.ToString(strSplitValue[0]) == Convert.ToString(strSplitSessionValue[0]))
                                    {
                                        rdoShippingMethod.Items[i].Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (Session["CustID"] != null && Session["CustID"].ToString() != "")
                {
                    BindOrderSummary();
                }
                return;
            }

            #endregion


            if (objShipServices != null && objShipServices.Tables.Count > 0 && objShipServices.Tables[0].Rows.Count > 0)
            {
                if (objShipServices.Tables[0].Select("ShippingService='UPS'").Length > 0)
                {
                    UPSTable = UPSMethodBind(CountryCode.ToString(), stateName, hdnZipCode.Value.ToString(), Weight, "UPS", ref strUPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='USPS'").Length > 0)
                {
                    EndiciaService objRate = new EndiciaService();
                    USPSTable = objRate.EndiciaGetRates(hdnZipCode.Value.ToString(), CountryCode.ToString(), Convert.ToDouble(Weight), ref strUSPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='FEDEX'").Length > 0)
                {
                    FedexTable = FedexMethod(Convert.ToDecimal(Weight), stateName, hdnZipCode.Value.ToString(), CountryCode, ref strFedexSMessage);
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

                bool IsDiscountAllowIncludeFreeShipping = false;
                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    if (ViewState["CustomerLevelFreeShipping"] != null && Convert.ToString(ViewState["CustomerLevelFreeShipping"]) == "1")
                    {
                        if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
                        {
                            //String strFreeShipping = "Standard Shipping($0.00)";
                            //DataRow dataRow = ShippingTable.NewRow();
                            //dataRow["ShippingMethodName"] = strFreeShipping;
                            //dataRow["Price"] = 0;
                            //ShippingTable.Rows.Add(dataRow);
                            CheckFreeShaippingOrder = true;
                        }
                    }
                }

                if (CheckFreeShaippingOrder == false)
                {
                    if (Session["CouponCode"] != null && Session["CouponCode"].ToString() != "")
                    {
                        IsDiscountAllowIncludeFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT DiscountAllowIncludeFreeShipping FROM dbo.tb_Coupons WHERE CouponCode='" + Session["CouponCode"].ToString() + "'"));
                    }
                }
                if (CheckFreeShaippingOrder == false && IsDiscountAllowIncludeFreeShipping == true)
                {
                    //String strFreeShipping = "Standard Shipping($0.00)";
                    //DataRow dataRow = ShippingTable.NewRow();
                    //dataRow["ShippingMethodName"] = strFreeShipping;
                    //dataRow["Price"] = 0;
                    //ShippingTable.Rows.Add(dataRow);
                    CheckFreeShaippingOrder = true;
                }
                else if (CheckFreeShaippingOrder == false)
                {
                    if (CountryCode.ToString().Trim().ToUpper() == "US" || CountryCode.ToString().Trim().ToUpper() == "UNITED STATES")
                    {
                        if (FinalTotal > FreeShippingAmount)
                        {
                            if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
                            {
                                //String strFreeShipping = "Standard Shipping($0.00)";
                                //DataRow dataRow = ShippingTable.NewRow();
                                //dataRow["ShippingMethodName"] = strFreeShipping;
                                //dataRow["Price"] = 0;
                                //ShippingTable.Rows.Add(dataRow);
                                CheckFreeShaippingOrder = true;

                            }
                        }
                    }
                }


                decimal SubTotal = 0;
                Price = 0;
                //if (ViewState["FinalTotal"] != null)
                //{
                //    OrderTotal = Convert.ToDecimal(ViewState["FinalTotal"].ToString());
                //}
                if (!string.IsNullOrEmpty(hdnSubTotalofProduct.Value) && Convert.ToDecimal(hdnSubTotalofProduct.Value) > 0)
                {
                    SubTotal = Convert.ToDecimal(hdnSubTotalofProduct.Value);
                }

                if (CountryCode.ToString().Trim().ToUpper() == "US" || CountryCode.ToString().Trim().ToUpper() == "UNITED STATES")
                {
                    if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                    {
                        string strfreeshipping = Convert.ToString(CommonComponent.GetScalarCommonData("Select isnull(configvalue,'0') from tb_AppConfig WHERE Configname ='FreeShippingLimit' and isnull(Deleted,0)=0 and StoreId=1"));
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
                                        lblFreeShippningMsg.Text = "Congratulations!! You qualified for Free Shipping. ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(0) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(29.99))
                                    {
                                        Price = 5.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(30.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(69.99))
                                    {
                                        Price = 7.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(70.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(129.99))
                                    {
                                        Price = 12.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(130.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(199.99))
                                    {
                                        Price = 16.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(200.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(248.99))
                                    {
                                        Price = 21.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(150) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(199.99))
                                    //{
                                    //    Price = 17.99;
                                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    //}
                                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(200) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(248.99))
                                    //{
                                    //    Price = 19.99;

                                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    //}

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
                    else
                    {
                        if (strUSPSMessage != "" && strUPSMessage != "")
                        {
                            string strfreeshipping = Convert.ToString(CommonComponent.GetScalarCommonData("Select isnull(configvalue,'0') from tb_AppConfig WHERE Configname ='FreeShippingLimit' and isnull(Deleted,0)=0 and StoreId=1"));
                            if (OrderTotal >= 0)
                            {
                                if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(strfreeshipping))
                                {
                                    Price = 0;
                                    lblFreeShippningMsg.Text = "Congratulations!! You qualified for Free Shipping. ( United States Only )";
                                }
                                else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(0) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(29.99))
                                {
                                    Price = 5.99;
                                    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                }
                                else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(30.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(69.99))
                                {
                                    Price = 7.99;
                                    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                }
                                else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(70.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(129.99))
                                {
                                    Price = 12.99;
                                    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                }
                                else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(130.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(199.99))
                                {
                                    Price = 16.99;
                                    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                }
                                else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(200.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(248.99))
                                {
                                    Price = 21.99;
                                    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                }
                                //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(150) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(199.99))
                                //{
                                //    Price = 17.99;
                                //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                //}
                                //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(200) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(248.99))
                                //{
                                //    Price = 19.99;

                                //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                //}
                                decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.35);
                                DataRow dr;
                                dr = ShippingTable.NewRow();
                                dr["ShippingMethodName"] = "Ground($" + string.Format("{0:0.00}", Convert.ToDecimal(Price)) + ")";
                                dr["Price"] = Convert.ToDecimal(Price);
                                ShippingTable.Rows.Add(dr);

                            }
                        }
                    }
                }
                else if (CountryCode.ToUpper() == "CA" || CountryCode.ToString().Trim().ToUpper() == "CANADA")
                {
                    //decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.18);
                    DataRow dr;
                    //dr = ShippingTable.NewRow();
                    //dr["ShippingMethodName"] = "UPS Standard to Canada" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(0)) + ")";
                    //dr["Price"] = 0;
                    //ShippingTable.Rows.Add(dr);

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

                    if (chkcopy.Checked == false && !string.IsNullOrEmpty(txtShipOther.Text.Trim().ToString()))
                    {
                        StrShippingstate = txtShipOther.Text.Trim();
                    }
                    else if (!string.IsNullOrEmpty(txtBillother.Text.Trim().ToString()))
                    {
                        StrShippingstate = txtBillother.Text.Trim();
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


                if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                {
                    rdoShippingMethod.Items.Clear();
                    btnReloadShipping.Attributes.Add("Style", "display:none");
                    DataView dvShipping = ShippingTable.DefaultView;
                    dvShipping.Sort = "Price asc";
                    rdoShippingMethod.DataSource = dvShipping.ToTable();
                    rdoShippingMethod.DataTextField = "ShippingMethodName";
                    rdoShippingMethod.DataValueField = "ShippingMethodName";
                    rdoShippingMethod.DataBind();
                }
                else
                {
                    btnReloadShipping.Attributes.Add("Style", "display:block");
                }

                if (Session["SelectedShippingMethod"] != null && Convert.ToString(Session["SelectedShippingMethod"]) != "")
                {
                    if (rdoShippingMethod.Items.Count > 0)
                    {
                        rdoShippingMethod.ClearSelection();
                        for (int i = 0; i < rdoShippingMethod.Items.Count; i++)
                        {
                            if (Convert.ToString(rdoShippingMethod.Items[i].Text) == Convert.ToString(Session["SelectedShippingMethod"]))
                            {
                                rdoShippingMethod.Items[i].Selected = true;
                                break;
                            }
                        }
                    }
                }

                if (rdoShippingMethod.Items.Count > 0 && rdoShippingMethod.SelectedIndex <= -1)
                {
                    rdoShippingMethod.SelectedIndex = 0;
                }



                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    BindOrderSummary();
                }

                if (strUSPSMessage != "" && strUPSMessage != "")
                {


                    //lblMsg.Text = strUPSMessage + strUSPSMessage;
                    //lblMsg.Visible = true;
                }
                else if (strUSPSMessage != "")
                {
                    //lblMsg.Text = strUSPSMessage;
                    //lblMsg.Visible = true;
                }
                else if (strUPSMessage != "")
                {
                    //lblMsg.Text = strUPSMessage;
                    //lblMsg.Visible = true;
                }
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
        /// <returns></returns>
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
        private DataTable UPSMethodBind(string Country, string State, string ZipCode, decimal Weight, string ServiceName, ref string StrMessage)
        {
            if ((ZipCode == "" || State == "") && (Country.ToString().Trim().ToUpper() == "US" || Country.ToString().Trim().ToUpper() == "UNITED STATES"))
            {
                return null;
            }

            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));


            UPS obj = new UPS(AppLogic.AppConfigs("Shipping.OriginAddress"), AppLogic.AppConfigs("Shipping.OriginAddress2"), AppLogic.AppConfigs("Shipping.OriginCity"), AppLogic.AppConfigs("Shipping.OriginState"), AppLogic.AppConfigs("Shipping.OriginZip"), AppLogic.AppConfigs("Shipping.OriginCountry"));
            obj.DestinationCountryCode = Country;
            obj.DestinationStateProvince = State;
            obj.DestinationZipPostalCode = ZipCode;
            StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            StringBuilder tmpFixedShipping = new StringBuilder(4096);

            if (Weight > decimal.Zero)
            {
                tmpRealTimeShipping.Append((string)obj.UPSGetRates(Convert.ToDecimal(Weight), Convert.ToDecimal(0), true));
            }
            else
            {
                tmpRealTimeShipping.Append((string)obj.UPSGetRates(Convert.ToDecimal(1), Convert.ToDecimal(0), true));
            }
            string strResult = tmpRealTimeShipping.ToString();

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

                //if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                //{
                //    if (ViewState["CustomerLevelFreeShipping"] != null && Convert.ToString(ViewState["CustomerLevelFreeShipping"]) == "1")
                //    {
                //        bool IsContainFreeShipping = false;
                //        for (int i = 0; i < rdoShippingMethod.Items.Count; i++)
                //        {
                //            if (rdoShippingMethod.Items[i].Text.ToString().Trim().ToLower().IndexOf("free shipping") > -1 || rdoShippingMethod.Items[i].Text.ToString().Trim().ToLower().IndexOf("freeshipping") > -1 || rdoShippingMethod.Items[i].Text.ToString().Trim().ToLower().IndexOf("$0") > -1 || rdoShippingMethod.Items[i].Text.ToString().Trim().ToLower().IndexOf("0.00") > -1)
                //            {
                //                IsContainFreeShipping = true;
                //                rdoShippingMethod.Items[i].Selected = true;
                //                break;
                //            }
                //        }
                //        if (!IsContainFreeShipping)
                //        {
                //            String strFreeShipping = "Standard Shipping($0.00)";
                //            rdoShippingMethod.Items.Insert(0, new ListItem(strFreeShipping, strFreeShipping));
                //            rdoShippingMethod.Items[0].Selected = true;

                //            DataRow dataRow = ShippingTable.NewRow();
                //            dataRow["ShippingMethodName"] = strFreeShipping;
                //            dataRow["Price"] = 0;
                //            ShippingTable.Rows.Add(dataRow);



                //        }
                //    }
                //}

                //if (Session["SelectedShippingMethod"] != null && Convert.ToString(Session["SelectedShippingMethod"]) != "")
                //{
                //    if (rdoShippingMethod.Items.Count > 0)
                //    {
                //        rdoShippingMethod.ClearSelection();
                //        for (int i = 0; i < rdoShippingMethod.Items.Count; i++)
                //        {
                //            String[] strSplitValue = rdoShippingMethod.Items[i].Text.ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //            String[] strSplitSessionValue = Session["SelectedShippingMethod"].ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //            if (strSplitValue[0] != null)
                //            {
                //                if (Convert.ToString(strSplitValue[0]) == Convert.ToString(strSplitSessionValue[0]))
                //                {
                //                    rdoShippingMethod.Items[i].Selected = true;
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //}

            }

            return ShippingTable;
        }

        /// <summary>
        /// Get Credit Card Type By StoreID
        /// </summary>
        private void GetCreditCardType()
        {
            CreditCardComponent objCreditcard = new CreditCardComponent();
            DataSet dsCard = new DataSet();
            ddlCardType.Items.Clear();
            dsCard = objCreditcard.GetAllCarTypeByStoreID(Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
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
        /// Get Shipping Method By ZipCode And State
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtBillZipCode_TextChanged(object sender, EventArgs e)
        {
            if (chkcopy.Checked == true)
            {
                GetShippingMethodByBillAddress();
                BindShippingMethod();
                BindOrderSummary();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowShipButton", "ShowShipButton();", true);
            }
            StateOtherSelection();
        }

        /// <summary>
        /// Get Shipping Method By ZipCode And State
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtShipZipCode_TextChanged(object sender, EventArgs e)
        {
            //if (UseShippingAddress.Checked)
            // {
            GetShippingMethodByShipAddress();
            BindShippingMethod();
            BindOrderSummary();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "laodbutton", "ShowShipButton();", true);
            //}
            StateOtherSelection();
        }

        /// <summary>
        /// Bill State Drop Down Selected Index Changed Event for Get Shipping Method
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlBillstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkcopy.Checked == true)
            {
                GetShippingMethodByBillAddress();
                BindShippingMethod();
                BindOrderSummary();
            }
            StateOtherSelection();
        }

        /// <summary>
        /// Ship State Drop Down Selected Index Changed Event for Get Shipping Method
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlShipState_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (UseShippingAddress.Checked)
            //{
            GetShippingMethodByShipAddress();
            BindShippingMethod();
            BindOrderSummary();
            //}
            StateOtherSelection();
        }

        /// <summary>
        /// Gets the Shipping Method by Bill Address
        /// </summary>
        private void GetShippingMethodByBillAddress()
        {
            if (chkcopy.Checked == true)
            {
                //if (Session["UserName"] == null || Session["UserName"].ToString() == "")
                //{
                hdnZipCode.Value = txtBillZipCode.Text.ToString();
                if (ddlBillstate.SelectedValue == "-11")
                {
                    hdnState.Value = txtBillother.Text.ToString();
                }
                else
                {
                    hdnState.Value = ddlBillstate.SelectedItem.Text.ToString();
                }
                hdncountry.Value = ddlBillcountry.SelectedValue.ToString();

                //}
            }
            else
            {
                //if (Session["UserName"] == null || Session["UserName"].ToString() == "")
                //{
                hdnZipCode.Value = txtShipZipCode.Text.ToString();
                if (ddlShipState.SelectedValue == "-11")
                {
                    hdnState.Value = txtShipOther.Text.ToString();
                }
                else
                {
                    hdnState.Value = ddlShipState.SelectedItem.Text.ToString();
                }
                hdncountry.Value = ddlShipCounry.SelectedValue.ToString();
                //}
            }
        }

        /// <summary>
        /// Gets the Shipping Method by Ship Address.
        /// </summary>
        private void GetShippingMethodByShipAddress()
        {

            hdnZipCode.Value = txtShipZipCode.Text.ToString();
            if (ddlShipState.SelectedValue == "-11")
            {
                hdnState.Value = txtShipOther.Text.ToString();
            }
            else
            {
                hdnState.Value = ddlShipState.SelectedItem.Text.ToString();
            }
            hdncountry.Value = ddlShipCounry.SelectedValue.ToString();


        }

        protected void ChkDetailsReadOnly(string Flag)
        {
            txtusername.Attributes.Add("ReadOnly", Flag);

            txtBillFirstname.Attributes.Add("ReadOnly", Flag);
            txtBillLastname.Attributes.Add("ReadOnly", Flag);
            txtBillAddressLine1.Attributes.Add("ReadOnly", Flag);
            txtBillAddressLine2.Attributes.Add("ReadOnly", Flag);
            txtBillSuite.Attributes.Add("readonly", Flag);
            txtBillCity.Attributes.Add("readonly", Flag);
            txtBillZipCode.Attributes.Add("readonly", Flag);
            txtBillphone.Attributes.Add("readonly", Flag);
            txtBillEmail.Attributes.Add("readonly", Flag);
            txtBillother.Attributes.Add("readonly", Flag);

            txtShipFirstName.Attributes.Add("readonly", Flag);
            txtShipLastName.Attributes.Add("readonly", Flag);
            txtshipAddressLine1.Attributes.Add("readonly", Flag);
            txtshipAddressLine2.Attributes.Add("readonly", Flag);
            txtShipSuite.Attributes.Add("readonly", Flag);
            txtShipCity.Attributes.Add("readonly", Flag);
            txtShipZipCode.Attributes.Add("readonly", Flag);
            txtShipPhone.Attributes.Add("readonly", Flag);
            txtShipEmailAddress.Attributes.Add("readonly", Flag);
            txtShipOther.Attributes.Add("readonly", Flag);

            ddlShipCounry.Attributes.Add("disabled", Flag);
            ddlShipState.Attributes.Add("disabled", Flag);
            ddlBillcountry.Attributes.Add("disabled", Flag);
            ddlBillstate.Attributes.Add("disabled", Flag);
        }

        /// <summary>
        /// Get Customer Details by CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 Customer ID</param>
        private void GetCustomerDetails(Int32 CustomerID)
        {
            DataSet dsCustomer = new DataSet();
            CustomerComponent objCust = new CustomerComponent();
            dsCustomer = objCust.GetCustomerDetails(CustomerID);

            //Billing Address
            ltrBillAdd.Text = "";
            String strEmail = string.Empty;
            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                ltrBillAdd.Text += "" + Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString()) + "<br />";
                txtBillFirstname.Text = dsCustomer.Tables[0].Rows[0]["FirstName"].ToString();
                txtBillLastname.Text = dsCustomer.Tables[0].Rows[0]["LastName"].ToString();


                if (string.IsNullOrEmpty(Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString())))
                {
                    strEmail = Convert.ToString(CommonComponent.GetScalarCommonData("select email from tb_customer where customerid =" + CustomerID));
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Email"].ToString()))
                {
                    txtusername.Text = dsCustomer.Tables[0].Rows[0]["Email"].ToString();
                }
                else { txtusername.Text = strEmail; }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Address1"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address1"].ToString()) + "<br />";
                    txtBillAddressLine1.Text = dsCustomer.Tables[0].Rows[0]["Address1"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Address2"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address2"].ToString()) + "<br />";
                    txtBillAddressLine2.Text = dsCustomer.Tables[0].Rows[0]["Address2"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Suite"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["Suite"].ToString()) + "<br />";
                    txtBillSuite.Text = dsCustomer.Tables[0].Rows[0]["Suite"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["City"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["City"].ToString()) + "<br />";
                    txtBillCity.Text = dsCustomer.Tables[0].Rows[0]["City"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["State"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"].ToString()) + "<br />";
                }

                if (ddlBillcountry.Items.FindByValue(Convert.ToString(dsCustomer.Tables[0].Rows[0]["Country"])) != null)
                {
                    ddlBillcountry.ClearSelection();
                    ddlBillcountry.Items.FindByValue(Convert.ToString(dsCustomer.Tables[0].Rows[0]["Country"])).Selected = true;
                    ddlBillcountry_SelectedIndexChanged(null, null);
                }
                try
                {
                    ddlBillcountry.ClearSelection();
                    ddlBillcountry.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Country"]);
                }
                catch
                {
                    ddlBillcountry.SelectedIndex = 0;
                }

                if (ddlBillstate.Items.FindByText(Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"])) == null)
                {
                    ddlBillstate.ClearSelection();
                    if (ddlBillstate.Items.FindByText("Other") != null)
                    {
                        ddlBillstate.Items.FindByText("Other").Selected = true;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mkBillingothrState", "MakeBillingOtherVisible();", true);
                        txtBillother.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"]);
                    }
                }
                else
                {
                    string Strtest = Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"]);
                    ddlBillstate.ClearSelection();
                    ddlBillstate.Items.FindByText(Strtest).Selected = true;
                    //ddlBillstate.Items.FindByText(Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"])).Selected = true;
                }


                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString()) + "<br />";
                    txtBillZipCode.Text = dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString();
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CountryName"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["CountryName"].ToString()) + "<br />";
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Phone"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["Phone"].ToString()) + "<br />";
                    txtBillphone.Text = dsCustomer.Tables[0].Rows[0]["Phone"].ToString();
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Email"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());
                    ViewState["BillEmail"] = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());
                    txtBillEmail.Text = dsCustomer.Tables[0].Rows[0]["Email"].ToString();
                }
                else
                {
                    txtBillEmail.Text = strEmail;
                    ltrBillAdd.Text += Convert.ToString(strEmail);
                    ViewState["BillEmail"] = Convert.ToString(strEmail);
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CustomerLevelID"].ToString()))
                {
                    ViewState["CustomerLevelID"] = dsCustomer.Tables[0].Rows[0]["CustomerLevelID"].ToString();
                }

                // Credit Card Details

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["NameOnCard"].ToString()))
                {
                    txtNameOnCard.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["NameOnCard"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardTypeID"].ToString()))
                {
                    ddlCardType.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardTypeID"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardVerificationCode"].ToString()))
                {
                    txtCSC.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardVerificationCode"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString()))
                {
                    string CardNumber = SecurityComponent.Decrypt(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString());
                    if (CardNumber.Length > 4)
                    {
                        for (int i = 0; i < CardNumber.Length - 4; i++)
                        {
                            txtCardNumber.Text += "*";
                        }
                        txtCardNumber.Text += CardNumber.ToString().Substring(CardNumber.Length - 4);
                        Session["CardNumber"] = CardNumber.ToString();
                    }
                    else
                    {
                        txtCardNumber.Text = "";
                    }
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardExpirationMonth"].ToString()))
                {
                    ddlMonth.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardExpirationMonth"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardExpirationYear"].ToString()))
                {
                    ddlYear.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardExpirationYear"].ToString());
                }
            }
            else
            {
                ltrBillAdd.Text = "N/A";
            }

            //Shipping Address
            ltrShipAdd.Text = "";
            if (dsCustomer != null && dsCustomer.Tables.Count > 1 && dsCustomer.Tables[1].Rows.Count > 0)
            {
                ltrShipAdd.Text += "" + Convert.ToString(dsCustomer.Tables[1].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(dsCustomer.Tables[1].Rows[0]["LastName"].ToString()) + "<br />";

                txtShipFirstName.Text = dsCustomer.Tables[1].Rows[0]["FirstName"].ToString();
                txtShipLastName.Text = dsCustomer.Tables[1].Rows[0]["LastName"].ToString();
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Address1"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address1"].ToString()) + "<br />";
                    txtshipAddressLine1.Text = dsCustomer.Tables[1].Rows[0]["Address1"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Address2"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address2"].ToString()) + "<br />";
                    txtshipAddressLine2.Text = dsCustomer.Tables[1].Rows[0]["Address2"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Suite"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["Suite"].ToString()) + "<br />";
                    txtShipSuite.Text = dsCustomer.Tables[1].Rows[0]["Suite"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["City"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["City"].ToString()) + "<br />";
                    txtShipCity.Text = dsCustomer.Tables[1].Rows[0]["City"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["State"].ToString()))
                {
                    hdnState.Value = Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"].ToString());
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString()))
                {
                    hdnZipCode.Value = Convert.ToString(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString());
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString()) + "<br />";
                    txtShipZipCode.Text = dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["CountryName"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["CountryName"].ToString()) + "<br />";
                    hdncountry.Value = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Country"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Phone"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["Phone"].ToString()) + "<br />";
                    txtShipPhone.Text = dsCustomer.Tables[1].Rows[0]["Phone"].ToString();
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Email"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["Email"].ToString());
                    txtShipEmailAddress.Text = dsCustomer.Tables[1].Rows[0]["Email"].ToString();
                }
                else
                {
                    txtShipEmailAddress.Text = strEmail;
                    ltrShipAdd.Text += strEmail;
                }

                if (ddlShipCounry.Items.FindByValue(Convert.ToString(dsCustomer.Tables[1].Rows[0]["Country"])) != null)
                {
                    ddlShipCounry.ClearSelection();
                    ddlShipCounry.Items.FindByValue(Convert.ToString(dsCustomer.Tables[1].Rows[0]["Country"])).Selected = true;
                    ddlShipCounry_SelectedIndexChanged(null, null);
                }
                try
                {
                    ddlShipCounry.ClearSelection();
                    ddlShipCounry.SelectedValue = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Country"]);
                    ddlShipCounry_SelectedIndexChanged(null, null);
                }
                catch
                {
                    ddlShipCounry.SelectedIndex = 0;
                }

                if (ddlShipState.Items.FindByText(Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"])) == null)
                {
                    ddlShipState.ClearSelection();
                    if (ddlShipState.Items.FindByText("Other") != null)
                    {
                        ddlShipState.Items.FindByText("Other").Selected = true;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mkShippingothrState", "MakeShippingOtherVisible();", true);
                        txtShipOther.Text = Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"]);
                    }
                }
                else
                {
                    ddlShipState.ClearSelection();
                    ddlShipState.Items.FindByText(Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"])).Selected = true;
                }
                pnlShippingDetails.Attributes.Add("style", "display:''");
                UseShippingAddress.Checked = true;
            }
            else
            {
                ltrShipAdd.Text = "";
            }
        }

        /// <summary>
        /// Gets the Customer Details for Payment
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns tb_Order Table Object</returns>
        private tb_Order GetCustomerDetailsForpayment(Int32 CustomerID, int OrderNumber)
        {
            DataSet dsCustomer = new DataSet();
            CustomerComponent objCust = new CustomerComponent();
            dsCustomer = objCust.GetCustomerDetails(CustomerID);

            //Billing Address
            tb_Order objorderData = new tb_Order();

            objorderData.CustomerID = CustomerID;
            objorderData.OrderNumber = OrderNumber;

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
                objorderData.CardName = Convert.ToString(txtNameOnCard.Text.ToString());
                objorderData.CardType = Convert.ToString(ddlCardType.SelectedItem.Text.ToString());
                objorderData.CardVarificationCode = Convert.ToString(txtCSC.Text.ToString());
                if (txtCardNumber.Text.ToString().IndexOf("*") > -1)
                {
                    objorderData.CardNumber = SecurityComponent.Decrypt(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString());
                }
                else
                {
                    objorderData.CardNumber = txtCardNumber.Text.ToString();
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
        /// Shipping Method Radio Button Selected Index Changed for Add Shipping Amount in Cart Details
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void rdoShippingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOrderSummary();
            StateOtherSelection();
        }

        /// <summary>
        /// Loads any saved view-state information to the <see cref="T:System.Web.UI.Page" /> object.
        /// </summary>
        /// <returns>The saved view state.</returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            if (Session[Session.SessionID] != null)
                return (new LosFormatter().Deserialize((string)Session[Session.SessionID]));
            return null;
        }

        /// <summary>
        /// Saves any view-state and control-state information for the page.
        /// </summary>
        /// <param name="state">The state.</param>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            LosFormatter los = new LosFormatter();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            los.Serialize(sw, state);
            string vs = sw.ToString();
            Session[Session.SessionID] = vs;
        }

        /// <summary>
        /// Gets the Product URL for Display
        /// </summary>
        /// <param name="mainCategory">String mainCategory</param>
        /// <param name="Sename">string Sename</param>
        /// <param name="ProductId">string ProductId</param>
        /// <param name="CustomCartID">string CustomCartID</param>
        /// <returns>Returns a Product URL as a String format</returns>
        public String GetProductUrl(String mainCategory, String Sename, String ProductId, string CustomCartID)
        {
            string Url = "";
            int GiftCardProductID = 0;
            GiftCardProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(GiftCardProductID,0) FROM dbo.tb_GiftCardProduct Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + ProductId.ToString() + ""));
            if (GiftCardProductID > 0)
            {
                Url = "/gi-" + CustomCartID + "-";
                if (Sename != "")
                {
                    Url += Sename.ToString();
                }
            }
            else
            {
                if (mainCategory != "")
                {
                    Url = "/" + mainCategory.ToString();
                }
                if (Sename != "")
                {
                    Url += "/" + Sename.ToString();
                }
            }
            if (ProductId != "")
            {
                Url += "-" + ProductId.ToString() + ".aspx";
            }
            return Url.ToString();
        }


        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {
            MatchUserName();
        }

        public void MatchUserName()
        {
            if (txtusername.Text != "" && txtpassword.Text != "")
            {
                objCustomer = new CustomerComponent();
                int CustID = 0;

                int CurrentCustomerID = 0;
                try
                {
                    CurrentCustomerID = objCustomer.GetCustIDByUserName(txtusername.Text.Trim(), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                }
                catch { }


                if (CurrentCustomerID != 0)
                {
                    DataSet dsCustomer = objCustomer.GetCustomerLockOut(CurrentCustomerID, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                    int CustomerTry = 0;
                    if (dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptCount"] != null && dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptCount"].ToString() != "")
                    {
                        CustomerTry = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptCount"]);
                    }

                    Double TimeOutValue = Convert.ToDouble(AppLogic.AppConfigs("LockoutTime"));

                    DateTime date = new DateTime();
                    if (dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptDate"] != null && dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptDate"].ToString() != "")
                    {
                        date = Convert.ToDateTime(dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptDate"]);
                    }
                    DateTime datenew = date.AddMinutes(TimeOutValue);

                    Int32 isLock = 0;
                    if (dsCustomer.Tables[0].Rows[0]["IsLockedOut"] != null && dsCustomer.Tables[0].Rows[0]["IsLockedOut"].ToString() != "")
                        isLock = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["IsLockedOut"]);

                    Int32 AllowedTry = 5;

                    DataSet dsGetCustomerDetail = new DataSet();
                    dsGetCustomerDetail = objCustomer.GetCustIDByUserNamePassword(txtusername.Text.Trim(), SecurityComponent.Encrypt(txtpassword.Text.ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                    if (dsGetCustomerDetail != null && dsGetCustomerDetail.Tables.Count > 0 && dsGetCustomerDetail.Tables[0].Rows.Count > 0)
                    {
                        CustID = Convert.ToInt32(dsGetCustomerDetail.Tables[0].Rows[0]["CustomerID"].ToString());
                    }
                    if (Convert.ToBoolean(isLock) && DateTime.Now < datenew)
                    {
                        objCustomer.UpdateCustomerLockOut(CurrentCustomerID, isLock, 0, date, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Your Account is Locked for " + TimeOutValue + " Minutes , Please try Later...');", true);
                        txtusername.Text = "";
                        txtpassword.Text = "";
                        txtusername.Focus();
                        return;
                    }
                    else if (CustID == 0)
                    {
                        CustomerTry++;

                        if (CustomerTry >= AllowedTry)
                        {
                            isLock = 1;
                            objCustomer.UpdateCustomerLockOut(CurrentCustomerID, isLock, 0, DateTime.Now, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Your Account is Locked for " + TimeOutValue + " Minutes , Please try Later...');", true);
                            return;
                        }
                        else
                        {
                            objCustomer.UpdateCustomerLockOut(CurrentCustomerID, isLock, CustomerTry, DateTime.Now, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                        }

                        int remaingtrial = AllowedTry - (CustomerTry);
                        string trial = "";
                        if (Convert.ToBoolean(isLock))
                        {
                            trial = "1 trial is";
                        }
                        else
                        {
                            if (remaingtrial > 1)
                                trial = remaingtrial + " trials are";
                            else
                                trial = remaingtrial + " trial is";
                        }
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert(' Invalid User Name or Password. " + trial + " remaining, then your account will be locked.');", true);
                        txtpassword.Text = "";
                        txtpassword.Focus();
                        return;
                    }

                    else
                    {
                        bool flag = CommonOperations.RegisterCart(CustID, false);
                        Session["UserName"] = txtusername.Text.ToString();
                        Session["CustID"] = CustID;
                        Session["IsAnonymous"] = "false";
                        Session["FirstName"] = Convert.ToString(dsGetCustomerDetail.Tables[0].Rows[0]["FirstName"]);
                        Response.Cookies.Add(new System.Web.HttpCookie("ecommcustomer", null));//remove anonymous cookies.

                        objCustomer = new CustomerComponent();
                        isLock = 0;

                        //  LoginTable.Visible = false;
                        divrdolist.Visible = false;
                        objCustomer.UpdateCustomerLockOut(CustID, isLock, 0, DateTime.Now, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                        if (Request.QueryString["ReturnURL"] != null)
                        {
                            Response.Redirect(Request.QueryString["ReturnURL"].ToString());
                        }
                        if (flag)
                        {
                            if (Session["NoOfCartItems"] != null)
                            {
                                if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "2")
                                {
                                    InsertWishListItems();
                                }

                                else if (Request.QueryString["wishlist"] != null)
                                {
                                    ShoppingCartComponent objWishlist = new ShoppingCartComponent();
                                    objWishlist.AddToWishList(Convert.ToInt32(Session["CustID"].ToString()));
                                    Response.Redirect("/Wishlist.aspx", true);
                                }
                                else
                                {
                                    if (Session["PaymentMethod"] != null)
                                    {
                                        Response.Redirect("CustomerquoteCheckout.aspx", true);
                                    }
                                    else
                                    {
                                        //Response.Redirect("CheckOutCommon.aspx", true);
                                    }
                                }
                            }
                            else
                            {
                                if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "2")
                                {
                                    InsertWishListItems();
                                }
                                Response.Redirect("/MyAccount.aspx", true);
                            }
                        }
                        else
                        {
                            if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "2")
                            {
                                InsertWishListItems();
                            }
                            Response.Redirect("/MyAccount.aspx", true);
                        }
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('User does not exist...');", true);
                    txtusername.Text = "";
                    txtpassword.Text = "";
                    txtusername.Focus();
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Please enter User Name and password');", true);
                return;
            }
        }


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
                    for (int i = 0; i < dtProduct.Rows.Count; i++)
                    {
                        IsAdded = objWishList.AddWishListItems(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(dtProduct.Rows[i]["ProductID"]), Convert.ToInt32(dtProduct.Rows[i]["Quantity"]), Convert.ToDecimal(dtProduct.Rows[i]["Price"]), Convert.ToString(dtProduct.Rows[i]["VariantNameId"]), Convert.ToString(dtProduct.Rows[i]["VariantValueId"]));

                    }
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

        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            objCustomer = new CustomerComponent();
            DataSet dsForgotPwd = new DataSet();
            DataSet dsMailTemplate = new DataSet();
            dsForgotPwd = objCustomer.GetPasswordForForgotPassWord(txtEmail.Text.ToString().Trim(), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            dsMailTemplate = objCustomer.GetEmailTamplate("Forgotpassword", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsForgotPwd != null && dsForgotPwd.Tables.Count > 0 && dsForgotPwd.Tables[0].Rows.Count > 0)
            {
                if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                {

                    String strBody = "";
                    String strSubject = "";
                    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();
                    strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###FIRSTNAME###", Convert.ToString(dsForgotPwd.Tables[0].Rows[0]["FirstName"]), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###LASTNAME###", Convert.ToString(dsForgotPwd.Tables[0].Rows[0]["LastName"]), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###USERNAME###", Convert.ToString(dsForgotPwd.Tables[0].Rows[0]["Email"]), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###PASSWORD###", SecurityComponent.Decrypt(Convert.ToString(dsForgotPwd.Tables[0].Rows[0]["Password"])), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                    CommonOperations.SendMail(txtEmail.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Your Password has been sent to your E-Mail Address.');", true);

                    txtEmail.Text = "";
                    int NoOfItems = 0;
                    if (Session["NoOfCartItems"] != null)
                    {
                        Int32.TryParse(Convert.ToString(Session["NoOfCartItems"]), out NoOfItems);
                    }

                    if (NoOfItems == 0)
                    {
                        pnlForgotPassword.Visible = false;
                        pnlLogin.Visible = true;
                        txtusername.Focus();
                    }
                    else
                    {
                        pnlForgotPassword.Visible = false;
                        pnlLogin.Visible = true;
                        txtusername.Focus();
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Invalid E-Mail Address. Please verify it.');", true);
                    txtEmail.Focus();
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msga", "alert('This E-Mail Address is not registered with us.');", true);
                txtEmail.Focus();
                return;
            }
        }

        protected void lkbForgetpwd_Click(object sender, EventArgs e)
        {
            trReturningAccount.Attributes.Add("style", "display=''");
            pnlLogin.Visible = false;
            pnlForgotPassword.Visible = true;
            txtEmail.Focus();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            pnlLogin.Visible = true;
            pnlForgotPassword.Visible = false;
            txtusername.Focus();
        }

        protected void lnkReturnLogin_Click(object sender, EventArgs e)
        {
            trReturningAccount.Attributes.Add("style", "display=''");
            pnlLogin.Visible = true;
            pnlForgotPassword.Visible = false;
            txtusername.Focus();
        }

        protected void txtCreateNewPassword_TextChanged(object sender, EventArgs e)
        {
            txtCreateNewPassword.Attributes.Add("value", txtCreateNewPassword.Text);
        }

        protected void txtConfirmPassWord_TextChanged(object sender, EventArgs e)
        {
            txtConfirmPassWord.Attributes.Add("value", txtConfirmPassWord.Text);
        }

        protected void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtpassword.Attributes.Add("value", txtpassword.Text);
        }

        #region Add To cart Code
        /// <summary>
        /// Method to update shopping cart item
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnUpdateCart_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["Weight"] = null;
            ViewState["AvailableDate"] = null;
            bool isDropshipProduct = false;
            int totalItemCount = RptCartItems.Items.Count;
            for (int i = 0; i < totalItemCount; i++)
            {
                TextBox txtQty = (TextBox)RptCartItems.Items[i].FindControl("txtQty");
                LinkButton lbtndelete = (LinkButton)RptCartItems.Items[i].FindControl("lbtndelete");
                HiddenField hdnProductId = (HiddenField)RptCartItems.Items[i].FindControl("hdnProductId");
                HiddenField hdnRelatedproductID = (HiddenField)RptCartItems.Items[i].FindControl("hdnRelatedproductID");
                HiddenField hdnVariantvalue = (HiddenField)RptCartItems.Items[i].FindControl("hdnVariantvalue");
                HiddenField hdnVariantname = (HiddenField)RptCartItems.Items[i].FindControl("hdnVariantname");

                int pInventory = 0;
                bool outofstock = false;
                bool checkvendordate = false;
                Decimal qtyyrd = Decimal.Zero;
                int Yardqty = 0;
                double actualYard = 0;
                isDropshipProduct = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(IsdropshipProduct,0) FROM tb_product WHERE productid=" + Convert.ToInt32(hdnProductId.Value.ToString()).ToString() + ""));
                Int32 ProductType = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect ISNULL(isProducttype,1) as isProducttype from tb_ShoppingCartItems where CustomCartID = " + lbtndelete.CommandArgument + ""));
                if (hdnRelatedproductID.Value.ToString() == "0")
                {
                    if (!string.IsNullOrEmpty(txtQty.Text.Trim()))
                    {
                        if (hdnProductId != null && Convert.ToInt32(txtQty.Text.ToString().Trim()) > 0)
                        {
                            DataSet ds = ProductComponent.GetProductDetailByID(Convert.ToInt32(hdnProductId.Value), Convert.ToInt32(1));
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                string FabricCode = Convert.ToString(ds.Tables[0].Rows[0]["FabricCode"]);
                                string FabricType = Convert.ToString(ds.Tables[0].Rows[0]["FabricType"]);
                                Int32 FabricTypeID = 0;
                                if (!string.IsNullOrEmpty(FabricCode) && !string.IsNullOrEmpty(FabricType))
                                {
                                    FabricTypeID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(FabricTypeID,0) as FabricTypeID from tb_ProductFabricType where FabricTypename ='" + FabricType + "'"));
                                    if (FabricTypeID > 0 && ProductType == 2)
                                    {
                                        DataSet dsFabricWidth = CommonComponent.GetCommonDataSet("Select top 1 * from tb_ProductFabricWidth where FabricCodeID in (Select ISNULL(FabricCodeID,0) from tb_ProductFabricCode Where FabricTypeID=" + FabricTypeID + " and Code='" + FabricCode + "')");
                                        Int32 QtyOnHand = 0, NextOrderQty = 0;
                                        Int32 OrderQty = Convert.ToInt32(txtQty.Text.Trim());

                                        if (dsFabricWidth != null && dsFabricWidth.Tables.Count > 0 && dsFabricWidth.Tables[0].Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"].ToString()))
                                                QtyOnHand = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"]);

                                            if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"].ToString()))
                                                NextOrderQty = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"]);
                                        }
                                        pInventory = QtyOnHand + NextOrderQty;

                                        try
                                        {
                                            string Style = "";
                                            double Width = 0;
                                            double Length = 0;
                                            string Options = "";
                                            string[] strNmyard = hdnVariantname.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            string[] strValyeard = hdnVariantvalue.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            if (strNmyard.Length > 0)
                                            {
                                                if (strValyeard.Length == strNmyard.Length)
                                                {
                                                    for (int j = 0; j < strNmyard.Length; j++)
                                                    {
                                                        if (strNmyard[j].ToString().ToLower() == "width")
                                                        {
                                                            Width = Convert.ToDouble(strValyeard[j].ToString());
                                                        }
                                                        if (strNmyard[j].ToString().ToLower() == "length")
                                                        {
                                                            Length = Convert.ToDouble(strValyeard[j].ToString());
                                                        }
                                                        if (strNmyard[j].ToString().ToLower() == "options")
                                                        {
                                                            Options = Convert.ToString(strValyeard[j].ToString());
                                                        }
                                                        if (strNmyard[j].ToString().ToLower() == "header")
                                                        {
                                                            Style = Convert.ToString(strValyeard[j].ToString());
                                                        }
                                                    }
                                                }
                                            }
                                            string resp = "";
                                            if (Width > Convert.ToDouble(0) && Length > Convert.ToDouble(0) && Style != "")
                                            {
                                                DataSet dsYard = new DataSet();
                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + hdnProductId.Value.ToString() + "," + Width.ToString() + "," + Length.ToString() + "," + OrderQty.ToString() + ",'" + Style + "','" + Options + "'");
                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                {
                                                    //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                    actualYard = Convert.ToDouble(resp.ToString());
                                                }
                                            }
                                            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            //{
                                            //    //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                                            //    resp = string.Format("{0:0.00}", Convert.ToDecimal(ds.Tables[0].Rows[0][1].ToString()));
                                            //}
                                            if (resp != "")
                                            {
                                                OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                            }
                                        }
                                        catch
                                        {
                                        }
                                        Yardqty = OrderQty;
                                        if (!string.IsNullOrEmpty(hdnVariantvalue.Value.ToString().Trim()) && hdnVariantvalue.Value.ToString().Trim().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                                        {
                                            OrderQty = OrderQty * 2;
                                        }


                                        string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + hdnProductId.Value.ToString() + " "));

                                        if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                                        {
                                            if (isDropshipProduct == false)
                                            {
                                                outofstock = true;
                                                lblInverror.Text = "We have not enough inventory!";
                                            }
                                            ViewState["AvailableDate"] = StrVendor;
                                        }
                                        else
                                        {
                                            checkvendordate = true;
                                            ViewState["AvailableDate"] = StrVendor;
                                        }
                                    }
                                    else if (ProductType == 3)
                                    {

                                        //qtyyrd = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT (" + txtQty.Text.ToString() + " * Actualyard) FROM tb_ShoppingCartItems WHERE CustomCartID=" + lbtndelete.CommandArgument + ""));
                                        //Yardqty = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDecimal(qtyyrd.ToString())))));

                                        //actualYard = Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDecimal(qtyyrd / Convert.ToDecimal(txtQty.Text.ToString()))));
                                        //string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + hdnProductId.Value.ToString() + ",'" + hdnVariantname.Value.ToString().Replace("~hpd~", "-") + "','" + hdnVariantvalue.Value.ToString().Replace("~hpd~", "-") + "'"));

                                        //if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
                                        //{
                                        //    if (isDropshipProduct == false)
                                        //    {
                                        //        outofstock = true;
                                        //        lblInverror.Text = "We have not enough inventory!";

                                        //    }
                                        //    ViewState["AvailableDate"] = StrVendor;
                                        //}
                                        //else
                                        //{
                                        //    checkvendordate = true;
                                        //    ViewState["AvailableDate"] = StrVendor;
                                        //}

                                        qtyyrd = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT (" + txtQty.Text.ToString() + " * Actualyard) FROM tb_ShoppingCartItems WHERE CustomCartID=" + lbtndelete.CommandArgument + ""));
                                        Yardqty = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDecimal(qtyyrd.ToString())))));

                                        actualYard = Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDecimal(qtyyrd / Convert.ToDecimal(txtQty.Text.ToString()))));
                                        string StrVendor = "";
                                        if (Yardqty == 0)
                                        {
                                            StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + txtQty.Text.ToString() + "," + hdnProductId.Value.ToString() + ",'" + hdnVariantname.Value.ToString().Replace("~hpd~", "-") + "','" + hdnVariantvalue.Value.ToString().Replace("~hpd~", "-") + "'"));
                                        }
                                        else
                                        {
                                            StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + hdnProductId.Value.ToString() + ",'" + hdnVariantname.Value.ToString().Replace("~hpd~", "-") + "','" + hdnVariantvalue.Value.ToString().Replace("~hpd~", "-") + "'"));
                                        }


                                        if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
                                        {
                                            if (isDropshipProduct == false)
                                            {
                                                outofstock = true;
                                                lblInverror.Text = "We have not enough inventory!";

                                            }
                                            ViewState["AvailableDate"] = StrVendor;
                                        }
                                        else
                                        {
                                            checkvendordate = true;
                                            ViewState["AvailableDate"] = StrVendor;
                                        }
                                    }
                                    else if (ProductType == 1)
                                    {
                                        string[] strNmyard = hdnVariantname.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        string[] strValyeard = hdnVariantvalue.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                        if (strValyeard.Length > 0)
                                        {
                                            int warehouseId = 0;
                                            if (strValyeard.Length == strNmyard.Length)
                                            {
                                                for (int j = 0; j < strNmyard.Length; j++)
                                                {
                                                    if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                                                    {
                                                        if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                                                        {
                                                            string strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                                                            strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                                            string strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                            int CntInv = 0;//Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                            //pInventory = CntInv;

                                                            DataSet dsUPC = new DataSet();
                                                            dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU,VariantValueID FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue + "' AND  ProductId=" + hdnProductId.Value.ToString() + "");
                                                            string upc = "";
                                                            string Skuoption = "";
                                                            string Variantvalueid = "";
                                                            if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                                            {
                                                                upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                                Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                                Variantvalueid = Convert.ToString(dsUPC.Tables[0].Rows[0]["VariantValueID"].ToString());
                                                                warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductVariantInventory inner join tb_WareHouse on tb_WareHouseProductVariantInventory.WareHouseID=tb_WareHouse.WareHouseID where VariantValueID=" + Convert.ToInt32(Variantvalueid) + " and tb_WareHouseProductVariantInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                                if (warehouseId == 0)
                                                                {
                                                                    warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID in (" + hdnProductId.Value.ToString() + " ) and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                                }
                                                                if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                                {
                                                                    string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                                    if (!string.IsNullOrEmpty(strQty))
                                                                    {
                                                                        try
                                                                        {
                                                                            CntInv = Convert.ToInt32(strQty.ToString());
                                                                        }
                                                                        catch { }
                                                                    }
                                                                }
                                                            }
                                                            pInventory = CntInv;
                                                            string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + hdnProductId.Value.ToString() + ""));
                                                            if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder)) //&& CntInv < Convert.ToInt32(txtQty.Text.ToString())
                                                            {
                                                                string resp = "";

                                                                DataSet dsYard = new DataSet();
                                                                string Length = "";
                                                                if (strvalue.ToLower().IndexOf("x84l") > -1 || strvalue.ToLower().IndexOf("x 84l") > -1 || strvalue.ToLower().IndexOf("x84 l") > -1 || strvalue.ToLower().IndexOf("x 84 l") > -1)
                                                                {
                                                                    Length = "84";
                                                                }
                                                                else if (strvalue.ToLower().IndexOf("x96l") > -1 || strvalue.ToLower().IndexOf("x 96l") > -1 || strvalue.ToLower().IndexOf("x96 l") > -1 || strvalue.ToLower().IndexOf("x 96 l") > -1)
                                                                {
                                                                    Length = "96";
                                                                }
                                                                else if (strvalue.ToLower().IndexOf("x108l") > -1 || strvalue.ToLower().IndexOf("x 108l") > -1 || strvalue.ToLower().IndexOf("x108 l") > -1 || strvalue.ToLower().IndexOf("x 108 l") > -1)
                                                                {
                                                                    Length = "108";
                                                                }
                                                                else if (strvalue.ToLower().IndexOf("x120") > -1 || strvalue.ToLower().IndexOf("x 120l") > -1 || strvalue.ToLower().IndexOf("x120 l") > -1 || strvalue.ToLower().IndexOf("x 120 l") > -1)
                                                                {
                                                                    Length = "120";
                                                                }
                                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + hdnProductId.Value.ToString() + ",50," + Length.ToString() + "," + txtQty.Text.ToString() + ",'Pole Pocket','Lined'");
                                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                                {
                                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                                    actualYard = Convert.ToDouble(resp.ToString());
                                                                }
                                                                Int32 OrderQty = Convert.ToInt32(txtQty.Text.ToString());
                                                                if (resp != "")
                                                                {
                                                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                                }
                                                                Yardqty = OrderQty;
                                                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty.ToString() + "," + hdnProductId.Value.ToString() + " "));
                                                                pInventory = CntInv;
                                                                if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                                {
                                                                    ViewState["AvailableDate"] = StrVendor.ToString();
                                                                    checkvendordate = false;
                                                                }
                                                                else
                                                                {
                                                                    ViewState["AvailableDate"] = StrVendor.ToString();
                                                                    checkvendordate = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (warehouseId == 17)
                                                                {
                                                                    isDropshipProduct = true;
                                                                }
                                                            }
                                                            if (!string.IsNullOrEmpty(strDatenew))
                                                            {
                                                                ViewState["AvailableDate"] = strDatenew.ToString();
                                                                checkvendordate = true;
                                                            }



                                                        }
                                                        else
                                                        {
                                                            string strvalue = strValyeard[j].ToString().Trim();
                                                            strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                                            string strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                            int CntInv = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                            // pInventory = CntInv;
                                                            DataSet dsUPC = new DataSet();
                                                            dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU,VariantValueID FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue + "' AND  ProductId=" + hdnProductId.Value.ToString() + "");
                                                            string upc = "";
                                                            string Skuoption = "";
                                                            string Variantvalueid = "";
                                                            if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                                            {
                                                                upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                                Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                                Variantvalueid = Convert.ToString(dsUPC.Tables[0].Rows[0]["VariantValueID"].ToString());
                                                                warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductVariantInventory inner join tb_WareHouse on tb_WareHouseProductVariantInventory.WareHouseID=tb_WareHouse.WareHouseID where VariantValueID=" + Convert.ToInt32(Variantvalueid) + " and tb_WareHouseProductVariantInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                                if (warehouseId == 0)
                                                                {
                                                                    warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID in (" + hdnProductId.Value.ToString() + " ) and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                                }
                                                                if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                                {
                                                                    string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                                    if (!string.IsNullOrEmpty(strQty))
                                                                    {
                                                                        try
                                                                        {
                                                                            CntInv = Convert.ToInt32(strQty.ToString());
                                                                        }
                                                                        catch { }
                                                                    }
                                                                }
                                                            }
                                                            pInventory = CntInv;
                                                            string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + hdnProductId.Value.ToString() + ""));
                                                            if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder)) // && CntInv < Convert.ToInt32(txtQty.Text.ToString())
                                                            {
                                                                string resp = "";

                                                                DataSet dsYard = new DataSet();
                                                                string Length = "";
                                                                if (strvalue.ToLower().IndexOf("x84l") > -1 || strvalue.ToLower().IndexOf("x 84l") > -1 || strvalue.ToLower().IndexOf("x84 l") > -1 || strvalue.ToLower().IndexOf("x 84 l") > -1)
                                                                {
                                                                    Length = "84";
                                                                }
                                                                else if (strvalue.ToLower().IndexOf("x96l") > -1 || strvalue.ToLower().IndexOf("x 96l") > -1 || strvalue.ToLower().IndexOf("x96 l") > -1 || strvalue.ToLower().IndexOf("x 96 l") > -1)
                                                                {
                                                                    Length = "96";
                                                                }
                                                                else if (strvalue.ToLower().IndexOf("x108l") > -1 || strvalue.ToLower().IndexOf("x 108l") > -1 || strvalue.ToLower().IndexOf("x108 l") > -1 || strvalue.ToLower().IndexOf("x 108 l") > -1)
                                                                {
                                                                    Length = "108";
                                                                }
                                                                else if (strvalue.ToLower().IndexOf("x120") > -1 || strvalue.ToLower().IndexOf("x 120l") > -1 || strvalue.ToLower().IndexOf("x120 l") > -1 || strvalue.ToLower().IndexOf("x 120 l") > -1)
                                                                {
                                                                    Length = "120";
                                                                }
                                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + hdnProductId.Value.ToString() + ",50," + Length.ToString() + "," + txtQty.Text.ToString() + ",'Pole Pocket','Lined'");
                                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                                {
                                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                                    actualYard = Convert.ToDouble(resp.ToString());
                                                                }
                                                                Int32 OrderQty = Convert.ToInt32(txtQty.Text.ToString());
                                                                if (resp != "")
                                                                {
                                                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));

                                                                }

                                                                Yardqty = OrderQty;
                                                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty.ToString() + "," + hdnProductId.Value.ToString() + " "));

                                                                if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                                {
                                                                    ViewState["AvailableDate"] = StrVendor.ToString();
                                                                    checkvendordate = false;
                                                                }
                                                                else
                                                                {
                                                                    ViewState["AvailableDate"] = StrVendor.ToString();
                                                                    checkvendordate = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (warehouseId == 17)
                                                                {
                                                                    isDropshipProduct = true;
                                                                }
                                                            }
                                                            if (!string.IsNullOrEmpty(strDatenew))
                                                            {
                                                                ViewState["AvailableDate"] = strDatenew.ToString();
                                                                checkvendordate = true;
                                                            }

                                                        }
                                                    }

                                                }
                                                if (strNmyard.Length == 1 && strNmyard[0].ToString().ToLower().IndexOf("estimated") > -1)
                                                {
                                                    if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                                    {
                                                        pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());
                                                    }
                                                    if (ds.Tables[0].Rows[0]["ProductID"].ToString().Trim() != "")
                                                    {
                                                        warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID =" + ds.Tables[0].Rows[0]["ProductID"].ToString() + " and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                        if (warehouseId == 17)
                                                        {
                                                            isDropshipProduct = true;
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                            {
                                                pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());
                                            }
                                            if (ds.Tables[0].Rows[0]["ProductID"].ToString().Trim() != "")
                                            {
                                                int warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID =" + ds.Tables[0].Rows[0]["ProductID"].ToString() + " and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                if (warehouseId == 17)
                                                {
                                                    isDropshipProduct = true;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                        {
                                            pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());

                                            #region swatch-hemming
                                            if (ProductType == 0)
                                            {

                                                Int32 IsHemming = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,0) as ConfigValue from tb_AppConfig where ConfigName='IshemmingActive'"));
                                                if (IsHemming == 1)
                                                {
                                                    int SwatchHemmInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(HammingSafetyQty,0) as HammingSafetyQty FROM tb_product WHERE isnull(IsHamming,0)=1 and ProductId=" + ds.Tables[0].Rows[0]["ProductID"].ToString() + ""));
                                                    pInventory = pInventory - SwatchHemmInv;
                                                }
                                            }

                                            #endregion

                                        }
                                    }
                                }
                                else if (ProductType == 3)
                                {

                                    qtyyrd = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT (" + txtQty.Text.ToString() + " * Actualyard) FROM tb_ShoppingCartItems WHERE CustomCartID=" + lbtndelete.CommandArgument + ""));
                                    Yardqty = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDecimal(qtyyrd.ToString())))));
                                    actualYard = Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDecimal(qtyyrd / Convert.ToDecimal(txtQty.Text.ToString()))));
                                    string StrVendor = "";
                                    if (Yardqty == 0)
                                    {
                                        StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + txtQty.Text.ToString() + "," + hdnProductId.Value.ToString() + ",'" + hdnVariantname.Value.ToString().Replace("~hpd~", "-") + "' ,'" + hdnVariantvalue.Value.ToString().Replace("~hpd~", "-") + "'"));
                                    }
                                    else
                                    {
                                        StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + hdnProductId.Value.ToString() + ",'" + hdnVariantname.Value.ToString().Replace("~hpd~", "-") + "' ,'" + hdnVariantvalue.Value.ToString().Replace("~hpd~", "-") + "'"));
                                    }

                                    if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
                                    {
                                        if (isDropshipProduct == false)
                                        {
                                            outofstock = true;
                                            lblInverror.Text = "We have not enough inventory!";

                                        }
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                    else
                                    {
                                        checkvendordate = true;
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                }
                                else if (ProductType == 1)
                                {
                                    string[] strNmyard = hdnVariantname.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    string[] strValyeard = hdnVariantvalue.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                    if (strValyeard.Length > 0)
                                    {
                                        int warehouseId = 0;
                                        if (strValyeard.Length == strNmyard.Length)
                                        {
                                            for (int j = 0; j < strNmyard.Length; j++)
                                            {
                                                if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                                                {
                                                    if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                                                    {
                                                        string strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                                                        strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                                        string strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                        int CntInv = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                        // pInventory = CntInv;
                                                        DataSet dsUPC = new DataSet();
                                                        dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU,VariantValueID FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue + "' AND  ProductId=" + hdnProductId.Value.ToString() + "");
                                                        string upc = "";
                                                        string Skuoption = "";
                                                        string Variantvalueid = "";

                                                        if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                                        {
                                                            upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                            Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                            Variantvalueid = Convert.ToString(dsUPC.Tables[0].Rows[0]["VariantValueID"].ToString());
                                                            warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductVariantInventory inner join tb_WareHouse on tb_WareHouseProductVariantInventory.WareHouseID=tb_WareHouse.WareHouseID where VariantValueID=" + Convert.ToInt32(Variantvalueid) + " and tb_WareHouseProductVariantInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                            if (warehouseId == 0)
                                                            {
                                                                warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID in (" + hdnProductId.Value.ToString() + " ) and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                            }
                                                            if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                            {
                                                                string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                                if (!string.IsNullOrEmpty(strQty))
                                                                {
                                                                    try
                                                                    {
                                                                        CntInv = Convert.ToInt32(strQty.ToString());
                                                                    }
                                                                    catch { }
                                                                }
                                                            }
                                                        }
                                                        pInventory = CntInv;

                                                        string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + hdnProductId.Value.ToString() + ""));
                                                        if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder)) //  && CntInv < Convert.ToInt32(txtQty.Text.ToString())
                                                        {
                                                            string resp = "";

                                                            DataSet dsYard = new DataSet();
                                                            string Length = "";
                                                            if (strvalue.ToLower().IndexOf("x84l") > -1 || strvalue.ToLower().IndexOf("x 84l") > -1 || strvalue.ToLower().IndexOf("x84 l") > -1 || strvalue.ToLower().IndexOf("x 84 l") > -1)
                                                            {
                                                                Length = "84";
                                                            }
                                                            else if (strvalue.ToLower().IndexOf("x96l") > -1 || strvalue.ToLower().IndexOf("x 96l") > -1 || strvalue.ToLower().IndexOf("x96 l") > -1 || strvalue.ToLower().IndexOf("x 96 l") > -1)
                                                            {
                                                                Length = "96";
                                                            }
                                                            else if (strvalue.ToLower().IndexOf("x108l") > -1 || strvalue.ToLower().IndexOf("x 108l") > -1 || strvalue.ToLower().IndexOf("x108 l") > -1 || strvalue.ToLower().IndexOf("x 108 l") > -1)
                                                            {
                                                                Length = "108";
                                                            }
                                                            else if (strvalue.ToLower().IndexOf("x120") > -1 || strvalue.ToLower().IndexOf("x 120l") > -1 || strvalue.ToLower().IndexOf("x120 l") > -1 || strvalue.ToLower().IndexOf("x 120 l") > -1)
                                                            {
                                                                Length = "120";
                                                            }
                                                            dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + hdnProductId.Value.ToString() + ",50," + Length.ToString() + "," + txtQty.Text.ToString() + ",'Pole Pocket','Lined'");
                                                            if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                            {
                                                                resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                                actualYard = Convert.ToDouble(resp.ToString());
                                                            }
                                                            Int32 OrderQty = Convert.ToInt32(txtQty.Text.ToString());
                                                            if (resp != "")
                                                            {
                                                                OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                            }
                                                            Yardqty = OrderQty;
                                                            string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty.ToString() + "," + hdnProductId.Value.ToString() + " "));
                                                            pInventory = CntInv;
                                                            if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                            {
                                                                ViewState["AvailableDate"] = StrVendor.ToString();
                                                                checkvendordate = false;

                                                            }
                                                            else
                                                            {
                                                                ViewState["AvailableDate"] = StrVendor.ToString();
                                                                checkvendordate = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (warehouseId == 17)
                                                            {
                                                                isDropshipProduct = true;
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(strDatenew))
                                                        {
                                                            ViewState["AvailableDate"] = strDatenew.ToString();
                                                            checkvendordate = true;

                                                        }

                                                    }
                                                    else
                                                    {
                                                        string strvalue = strValyeard[j].ToString().Trim();
                                                        strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                                        string strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                        int CntInv = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                        //pInventory = CntInv;
                                                        DataSet dsUPC = new DataSet();
                                                        dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU,VariantValueID FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue + "' AND  ProductId=" + hdnProductId.Value.ToString() + "");
                                                        string upc = "";
                                                        string Skuoption = "";
                                                        string Variantvalueid = "";
                                                        if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                                        {
                                                            upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                            Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                            Variantvalueid = Convert.ToString(dsUPC.Tables[0].Rows[0]["VariantValueID"].ToString());
                                                            warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductVariantInventory inner join tb_WareHouse on tb_WareHouseProductVariantInventory.WareHouseID=tb_WareHouse.WareHouseID where VariantValueID=" + Convert.ToInt32(Variantvalueid) + " and tb_WareHouseProductVariantInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                            if (warehouseId == 0)
                                                            {
                                                                warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID in (" + hdnProductId.Value.ToString() + " ) and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                            }
                                                            if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                            {
                                                                string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                                if (!string.IsNullOrEmpty(strQty))
                                                                {
                                                                    try
                                                                    {
                                                                        CntInv = Convert.ToInt32(strQty.ToString());
                                                                    }
                                                                    catch { }
                                                                }
                                                            }
                                                        }
                                                        pInventory = CntInv;

                                                        string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + hdnProductId.Value.ToString() + ""));
                                                        if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder)) //&& CntInv < Convert.ToInt32(txtQty.Text.ToString())
                                                        {
                                                            string resp = "";

                                                            DataSet dsYard = new DataSet();
                                                            string Length = "";
                                                            if (strvalue.ToLower().IndexOf("x84l") > -1 || strvalue.ToLower().IndexOf("x 84l") > -1 || strvalue.ToLower().IndexOf("x84 l") > -1 || strvalue.ToLower().IndexOf("x 84 l") > -1)
                                                            {
                                                                Length = "84";
                                                            }
                                                            else if (strvalue.ToLower().IndexOf("x96l") > -1 || strvalue.ToLower().IndexOf("x 96l") > -1 || strvalue.ToLower().IndexOf("x96 l") > -1 || strvalue.ToLower().IndexOf("x 96 l") > -1)
                                                            {
                                                                Length = "96";
                                                            }
                                                            else if (strvalue.ToLower().IndexOf("x108l") > -1 || strvalue.ToLower().IndexOf("x 108l") > -1 || strvalue.ToLower().IndexOf("x108 l") > -1 || strvalue.ToLower().IndexOf("x 108 l") > -1)
                                                            {
                                                                Length = "108";
                                                            }
                                                            else if (strvalue.ToLower().IndexOf("x120") > -1 || strvalue.ToLower().IndexOf("x 120l") > -1 || strvalue.ToLower().IndexOf("x120 l") > -1 || strvalue.ToLower().IndexOf("x 120 l") > -1)
                                                            {
                                                                Length = "120";
                                                            }
                                                            dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + hdnProductId.Value.ToString() + ",50," + Length.ToString() + "," + txtQty.Text.ToString() + ",'Pole Pocket','Lined'");
                                                            if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                            {
                                                                resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                                actualYard = Convert.ToDouble(resp.ToString());
                                                            }
                                                            Int32 OrderQty = Convert.ToInt32(txtQty.Text.ToString());
                                                            if (resp != "")
                                                            {
                                                                OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                            }
                                                            Yardqty = OrderQty;
                                                            string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty.ToString() + "," + hdnProductId.Value.ToString() + " "));

                                                            if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                            {
                                                                ViewState["AvailableDate"] = StrVendor.ToString();
                                                                checkvendordate = false;
                                                            }
                                                            else
                                                            {
                                                                ViewState["AvailableDate"] = StrVendor.ToString();
                                                                checkvendordate = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (warehouseId == 17)
                                                            {
                                                                isDropshipProduct = true;
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(strDatenew))
                                                        {
                                                            ViewState["AvailableDate"] = strDatenew.ToString();
                                                            checkvendordate = true;

                                                        }
                                                    }
                                                }

                                            }
                                            if (strNmyard.Length == 1 && strNmyard[0].ToString().ToLower().IndexOf("estimated") > -1)
                                            {
                                                if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                                {
                                                    pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());
                                                }
                                                if (ds.Tables[0].Rows[0]["ProductID"].ToString().Trim() != "")
                                                {
                                                    warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID =" + ds.Tables[0].Rows[0]["ProductID"].ToString() + " and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                    if (warehouseId == 17)
                                                    {
                                                        isDropshipProduct = true;
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                        {
                                            pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());
                                        }
                                        if (ds.Tables[0].Rows[0]["ProductID"].ToString().Trim() != "")
                                        {
                                            int warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID =" + ds.Tables[0].Rows[0]["ProductID"].ToString() + " and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                            if (warehouseId == 17)
                                            {
                                                isDropshipProduct = true;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                    {
                                        pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());

                                        #region swatch-hemming
                                        if (ProductType == 0)
                                        {

                                            Int32 IsHemming = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,0) as ConfigValue from tb_AppConfig where ConfigName='IshemmingActive'"));
                                            if (IsHemming == 1)
                                            {
                                                int SwatchHemmInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(HammingSafetyQty,0) as HammingSafetyQty FROM tb_product WHERE isnull(IsHamming,0)=1 and ProductId=" + ds.Tables[0].Rows[0]["ProductID"].ToString() + ""));
                                                pInventory = pInventory - SwatchHemmInv;
                                            }
                                        }

                                        #endregion

                                    }
                                }
                            }


                            if (ProductType.ToString() == "1" && ViewState["AvailableDate"] == null) // Made to Measure
                            {
                                DateTime dtnew = DateTime.Now.Date.AddDays(12);
                                ViewState["AvailableDate"] = Convert.ToString(dtnew);
                                //Int32 OrderQty = Convert.ToInt32(txtQty.Text.Trim());
                                //string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + hdnProductId.Value.ToString() + " "));
                                //if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                                //{
                                //    ViewState["AvailableDate"] = StrVendor;
                                //}
                                //else
                                //{
                                //    ViewState["AvailableDate"] = StrVendor;
                                //}
                            }
                            if (!string.IsNullOrEmpty(hdnVariantvalue.Value.ToString().Trim()) && hdnVariantvalue.Value.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                            {
                                txtQty.Text = Convert.ToString(Convert.ToInt32(txtQty.Text) * 2);
                            }
                            if (isDropshipProduct == false && hdnRelatedproductID.Value.ToString() == "0")
                            {
                                Int32 AssemblyProduct = 0, ReturnQty = 0;
                                AssemblyProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect COUNT(*) From tb_product Where Productid=" + Convert.ToInt32(hdnProductId.Value.ToString()).ToString() + " and StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " and ProductTypeID in (Select ProductTypeID From tb_ProductType where Name='Assembly Product')"));
                                if (AssemblyProduct > 0)
                                {
                                    ReturnQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_Check_ProductAssemblyInventory " + Convert.ToInt32(hdnProductId.Value.ToString()).ToString() + "," + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + "," + Session["CustID"].ToString() + ",2"));
                                    if (ReturnQty <= 0 && isDropshipProduct == false)
                                    {
                                        outofstock = true;
                                    }
                                    else if (Convert.ToInt32(txtQty.Text.Trim()) > ReturnQty && isDropshipProduct == false)
                                    {
                                        outofstock = true;
                                    }
                                    else
                                    {
                                        // UpdateCart  Query
                                    }
                                }
                            }
                            if ((pInventory >= Convert.ToInt32(txtQty.Text.Trim()) && outofstock == false) || isDropshipProduct == true || checkvendordate == true)
                            {
                                if (txtQty != null && lbtndelete != null && hdnRelatedproductID.Value.ToString() == "0")
                                {
                                    if (!string.IsNullOrEmpty(hdnVariantvalue.Value.ToString().Trim()) && hdnVariantvalue.Value.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                                    {
                                        txtQty.Text = Convert.ToString(Convert.ToInt32(txtQty.Text) / 2);
                                    }
                                    objCart.UpdateCartItemQtyByCustomCartID(Convert.ToInt32(lbtndelete.CommandArgument), Convert.ToInt32(txtQty.Text.Trim()));
                                    try
                                    {
                                        Int32 RelatedproductID = 0, ParentQty = 0;
                                        RelatedproductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(RelatedproductID,0) FROM tb_ShoppingCartItems WHERE RelatedproductID=" + hdnProductId.Value.ToString() + "  AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)"));
                                        if (RelatedproductID > 0)
                                        {
                                            ParentQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Quantity,0) FROM tb_ShoppingCartItems WHERE ProductID=" + RelatedproductID.ToString() + "  AND isnull(VariantNames,'')='" + hdnVariantname.Value.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(VariantValues,'')='" + hdnVariantvalue.Value.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)"));
                                            DataSet dsAseeembly = new DataSet();
                                            dsAseeembly = CommonComponent.GetCommonDataSet("SElect ISNULL(Quantity,0),ProductId from tb_ProductAssembly Where RefProductId=" + RelatedproductID.ToString() + "");
                                            if (dsAseeembly != null && dsAseeembly.Tables.Count > 0 && dsAseeembly.Tables[0].Rows.Count > 0)
                                            {
                                                for (int iAseeembly = 0; iAseeembly < dsAseeembly.Tables[0].Rows.Count; iAseeembly++)
                                                {
                                                    Int32 AssemblyQty = Convert.ToInt32(dsAseeembly.Tables[0].Rows[iAseeembly][0].ToString());
                                                    AssemblyQty = (AssemblyQty * ParentQty);

                                                    CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET Quantity =" + AssemblyQty + " WHERE ProductID=" + dsAseeembly.Tables[0].Rows[iAseeembly]["ProductId"].ToString() + " AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)");
                                                }

                                            }

                                        }
                                    }
                                    catch { }

                                    try
                                    {
                                        string[] strNm = hdnVariantname.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        string[] strVal = hdnVariantvalue.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        string avail = "";
                                        string VariantNameId = "";
                                        string VariantValueId = "";
                                        if (strNm.Length > 0)
                                        {
                                            if (strVal.Length == strNm.Length)
                                            {
                                                for (int k = 0; k < strNm.Length; k++)
                                                {
                                                    if (ProductType != 3 && strNm[k].ToString().ToLower().IndexOf("estimated delivery") <= -1)
                                                    {
                                                        VariantNameId = VariantNameId + strNm[k].ToString() + ",";

                                                        VariantValueId = VariantValueId + strVal[k].ToString() + ",";
                                                        if (avail == "" && ProductType == 1)
                                                        {
                                                            avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 LockQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + hdnProductId.Value.ToString() + "  AND VariantValue='" + strVal[k].ToString() + "' AND isnull(LockQuantity,0) >=" + txtQty.Text + ""));
                                                            if (avail == "")
                                                            {
                                                                avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 AllowQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + hdnProductId.Value.ToString() + "  AND VariantValue='" + strVal[k].ToString() + "' AND isnull(AllowQuantity,0) >=" + txtQty.Text + ""));
                                                            }
                                                        }
                                                    }
                                                    else if (ProductType == 3 && strNm[k].ToString().ToLower().IndexOf("yardage required") <= -1 && strNm[k].ToString().ToLower().IndexOf("estimated delivery") <= -1)
                                                    {
                                                        VariantNameId = VariantNameId + strNm[k].ToString() + ",";

                                                        VariantValueId = VariantValueId + strVal[k].ToString() + ",";
                                                    }
                                                }

                                                if (ViewState["AvailableDate"] != null && ViewState["AvailableDate"].ToString() != "" && ProductType > 0 && ProductType == 2)
                                                {
                                                    avail = Convert.ToString(ViewState["AvailableDate"]);
                                                    try
                                                    {
                                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                                    }
                                                    catch { }
                                                    VariantNameId = VariantNameId + "Estimated Delivery,";
                                                    VariantValueId = VariantValueId + avail.ToString() + ",";
                                                    CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE  CustomCartID=" + lbtndelete.CommandArgument + "");
                                                }
                                                else if (ProductType == 3)
                                                {
                                                    //// VariantNameId = VariantNameId + "Yardage Required,";
                                                    //// VariantValueId = VariantValueId + qtyyrd.ToString() + ",";
                                                    if (ViewState["AvailableDate"] != null && ViewState["AvailableDate"].ToString() != "")
                                                    {
                                                        avail = Convert.ToString(ViewState["AvailableDate"]);
                                                        try
                                                        {
                                                            avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                                        }
                                                        catch { }

                                                    }

                                                    if (avail != "")
                                                    {
                                                        VariantNameId = VariantNameId + "Estimated Delivery,";
                                                        VariantValueId = VariantValueId + avail.ToString() + ",";
                                                    }
                                                    CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ", VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE  CustomCartID=" + lbtndelete.CommandArgument + "");
                                                }
                                                else if (avail == "" && ProductType.ToString() == "2")
                                                {
                                                    CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE  CustomCartID=" + lbtndelete.CommandArgument + "");
                                                }
                                                if (ProductType == 1)
                                                {
                                                    if (avail != "")
                                                    {
                                                        VariantNameId = VariantNameId + "Estimated Delivery,";
                                                        VariantValueId = VariantValueId + avail.ToString() + ",";

                                                        string strVaraintvalues = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VariantValues FROM tb_ShoppingCartItems WHERE CustomCartID=" + lbtndelete.CommandArgument + ""));
                                                        if (!string.IsNullOrEmpty(strVaraintvalues) && strVaraintvalues.ToLower().IndexOf("(buy 1 get 1") > -1)
                                                        {
                                                            CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE  isnull(RelatedproductID,0) > 0 and ShoppingCartID in (SElect ShoppingCartID From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + ") and RelatedproductID in (SElect ProductID From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + ") and ProductID in (SElect ProductID From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + " and isnull(RelatedproductID,0)=0 ) and VariantValues in (SElect VariantValues From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + " ) and VariantNames in (SElect VariantNames From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + " )");
                                                        }

                                                        CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE CustomCartID=" + lbtndelete.CommandArgument + "");
                                                    }
                                                    else
                                                    {

                                                        if (ViewState["AvailableDate"] != null)
                                                        {
                                                            avail = Convert.ToString(ViewState["AvailableDate"]);
                                                            try
                                                            {
                                                                avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                                            }
                                                            catch { }
                                                            VariantNameId = VariantNameId + "Estimated Delivery,";
                                                            VariantValueId = VariantValueId + avail.ToString() + ",";
                                                            string strVaraintvalues = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VariantValues FROM tb_ShoppingCartItems WHERE CustomCartID=" + lbtndelete.CommandArgument + ""));
                                                            if (!string.IsNullOrEmpty(strVaraintvalues) && strVaraintvalues.ToLower().IndexOf("(buy 1 get 1") > -1)
                                                            {
                                                                CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE  isnull(RelatedproductID,0) > 0 and ShoppingCartID in (SElect ShoppingCartID From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + ") and RelatedproductID in (SElect ProductID From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + ") and ProductID in (SElect ProductID From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + " and isnull(RelatedproductID,0)=0 ) and VariantValues in (SElect VariantValues From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + " ) and VariantNames in (SElect VariantNames From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + " )");
                                                            }

                                                            CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE CustomCartID=" + lbtndelete.CommandArgument + "");
                                                        }
                                                    }
                                                }
                                                if (ProductType != 3)
                                                {
                                                    string strVaraintvalues = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VariantValues FROM tb_ShoppingCartItems WHERE CustomCartID=" + lbtndelete.CommandArgument + ""));
                                                    if (!string.IsNullOrEmpty(strVaraintvalues) && strVaraintvalues.ToLower().IndexOf("(buy 1 get 1") > -1)
                                                    {
                                                        CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "' WHERE  isnull(RelatedproductID,0) > 0 and ShoppingCartID in (SElect ShoppingCartID From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + ") and RelatedproductID in (SElect ProductID From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + ") and ProductID in (SElect ProductID From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + " and isnull(RelatedproductID,0)=0 ) and VariantValues in (SElect VariantValues From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + " ) and VariantNames in (SElect VariantNames From tb_ShoppingCartItems Where CustomCartID= " + lbtndelete.CommandArgument + " )");
                                                    }
                                                    CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "' WHERE  CustomCartID=" + lbtndelete.CommandArgument + "");
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                lblInverror.Text = "";
                            }
                            else
                            {
                                lblInverror.Text = "We have not enough inventory!";
                                lblInverror.Visible = true;
                            }
                        }
                        else
                        {
                            lblInverror.Text = "Enter Valid Inventory!";
                            lblInverror.Visible = true;
                        }
                    }
                    else
                    {
                        lblInverror.Text = "Enter Valid Inventory!";
                        lblInverror.Visible = true;
                    }
                }
            }
            CouponCodeCalculation();
            if (chkcopy.Checked == true)
            {
                GetShippingMethodByBillAddress();
            }
            else { GetShippingMethodByShipAddress(); }
            BindShoppingCartByCustomerID();
            BindShippingMethod();

            BindOrderSummary();
            UpdateMinicart();


        }
        private void UpdateMinicart()
        {
            DataSet dsCart = new DataSet();
            if (Session["CustID"] != null && Session["CustID"].ToString() != "")
            {
                dsCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));
                if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
                {
                    int items = Convert.ToInt32(dsCart.Tables[0].Rows[0]["TotalItems"].ToString());
                    String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC) "));
                    Decimal SwatchQty1 = Decimal.Zero;
                    //if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty1 == 0)
                    //{
                    SwatchQty1 = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                    //}
                    Decimal GrandSubTotal = Decimal.Zero;
                    for (int i = 0; i < dsCart.Tables[0].Rows.Count; i++)
                    {
                        Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["ProductId"].ToString() + " and ItemType='Swatch'"));

                        if (Isorderswatch == 1)
                        {
                            Decimal pp = Decimal.Zero;
                            pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + dsCart.Tables[0].Rows[i]["Qty"].ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["ProductId"].ToString() + ""));

                            if (Convert.ToDecimal(pp) >= SwatchQty1)
                            {
                                GrandSubTotal += Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty1))));
                                SwatchQty1 = Decimal.Zero;
                            }
                            else
                            {
                                if (SwatchQty1 > Decimal.Zero)
                                {
                                    GrandSubTotal += Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(0)));
                                    SwatchQty1 = SwatchQty1 - Convert.ToDecimal(pp);
                                }
                                else
                                {
                                    GrandSubTotal += Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(pp.ToString())));
                                }
                            }
                        }
                        else
                        {
                            GrandSubTotal += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["IndiSubTotal"].ToString());
                        }
                    }


                    decimal Price = Convert.ToDecimal(GrandSubTotal);
                    string strallpair = items > 1 ? "<p><span class=\"navQty\">(" + items.ToString("D2") + " items)</span><span class=\"navTotal\"> $" + Math.Round(Price, 2) + "</span></p><img src=\"/images/cart-icon.jpg\" width=\"70\" height=\"22\" alt=\"\" title=\"\" class=\"cart-icon\">" : "<p><span class=\"navQty\">(" + items.ToString("D2") + " item)</span><span class=\"navTotal\"> $" + Math.Round(Price, 2) + "</span></p><img src=\"/images/cart-icon.jpg\" width=\"70\" height=\"22\" alt=\"\" title=\"\" class=\"cart-icon\">";
                    strallpair = "document.getElementById('cartlink').innerHTML='" + strallpair + "';";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scriptminicart", strallpair, true);
                    Session["NoOfCartItems"] = dsCart.Tables[0].Rows[0]["TotalItems"].ToString();
                }
                else
                {
                    Session["NoOfCartItems"] = null;
                    string strallpair = "<p><span class=\"navQty\">(0 item)</span><span class=\"navTotal\"> $0.00</span></p><img src=\"/images/cart-icon.jpg\" width=\"70\" height=\"22\" alt=\"\" title=\"\" class=\"cart-icon\">";
                    strallpair = "document.getElementById('cartlink').innerHTML='" + strallpair + "';";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scriptminicart", strallpair, true);



                }
            }
        }

        /// <summary>
        /// Calculate coupon Discount after Delete an Item From Cart
        /// </summary>
        private void CouponCodeCalculation()
        {
            //if (Session["CouponCode"] != null && Session["Discount"] != null)
            //{
            //    String SPMessage = Convert.ToString(CouponComponent.GetDiscountByCouponCodeFunction(Convert.ToString(Session["CouponCode"]).Trim(), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
            //    decimal CouponDiscount = 0;
            //    if (RptCartItems.Items.Count == 1)
            //    {
            //        HiddenField hdnProductId = (HiddenField)RptCartItems.Items[0].FindControl("hdnProductId");
            //        if (hdnProductId.Value.ToString() == AppLogic.AppConfigs("HotDealProduct").ToString())
            //        {
            //            Session["CouponCode"] = null;
            //            Session["Discount"] = null;
            //            CouponDiscount = 0;
            //        }
            //    }
            //    try
            //    {
            //        CouponDiscount = Convert.ToDecimal(SPMessage.ToString());
            //        Session["Discount"] = CouponDiscount.ToString();
            //    }
            //    catch
            //    {
            //        Session["CouponCode"] = null;
            //        Session["Discount"] = null;
            //        CouponDiscount = 0;
            //    }
            //    decimal GiftCardAmount = 0;
            //    if (Session["GiftCertificateDiscountCode"] != null && Session["GiftCertificateDiscount"] != null)
            //    {
            //        GiftCardAmount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Balance FROM [tb_GiftCard] where storeid=" + AppLogic.AppConfigs("StoreID") + " And SerialNumber='" + Session["GiftCertificateDiscountCode"].ToString().Trim() + "'"));
            //        if (GiftCardAmount == Decimal.Zero)
            //        {
            //            Session["GiftCertificateDiscountCode"] = null;
            //            Session["GiftCertificateDiscount"] = null;
            //        }
            //        else
            //        {
            //            Session["GiftCertificateDiscountCode"] = Session["GiftCertificateDiscountCode"].ToString().Trim();
            //            Session["GiftCertificateDiscount"] = Math.Round(GiftCardAmount, 2);
            //            GiftCardTotalDiscount = Math.Round(GiftCardAmount, 2);
            //        }
            //    }
            //}
            try
            {
                Session["CustomerDiscount"] = null;
                if (Session["CouponCodebycustomer"] != null && Session["CouponCodeDiscountPrice"] != null)
                {
                    decimal CouponDiscount = 0;

                    bool ChkNoDiscount = false;
                    String SPMessage = "";
                    decimal DiscountPercent = decimal.Zero;
                    bool isEle = false;
                    DataSet dsCoupon = new DataSet();
                    dsCoupon = CommonComponent.GetCommonDataSet("SELECT 0 AS DiscountPercent,FromDate,ToDate  FROM dbo.tb_Customer WHERE CustomerID=" + Session["CustID"].ToString() + " And CouponCode ='" + Session["CouponCodebycustomer"].ToString() + "'");
                    string StrFromdate = "";
                    string StrTodate = "";
                    //out DiscountPercent);
                    if (dsCoupon != null && dsCoupon.Tables.Count > 0 && dsCoupon.Tables[0].Rows.Count > 0)
                    {
                        DiscountPercent = Convert.ToDecimal(dsCoupon.Tables[0].Rows[0]["DiscountPercent"]);
                        StrFromdate = Convert.ToString(dsCoupon.Tables[0].Rows[0]["FromDate"].ToString());
                        StrTodate = Convert.ToString(dsCoupon.Tables[0].Rows[0]["ToDate"].ToString());
                    }
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
                            //if (DiscountPercent > 0)
                            //{

                            Session["CouponCodeDiscountPrice"] = DiscountPercent;
                            isEle = true;
                            lblInverror.Text = "Coupon Code Successfully Applied!";
                            Session["CustomerDiscount"] = "1";
                            //}
                        }
                        else
                        {
                            isEle = true;
                            lblInverror.Text = "Sorry, Coupon code is expired!";
                            Session["CouponCodebycustomer"] = null;
                            Session["CouponCodeDiscountPrice"] = null;
                            foreach (RepeaterItem rItem in RptCartItems.Items)
                            {
                                Label lblDiscountPrice = (Label)rItem.FindControl("lblDiscountprice");
                                HiddenField hdnCustomcartId = (HiddenField)rItem.FindControl("hdnCustomcartId");
                                CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET DiscountPrice=0 WHERE CustomCartID=" + hdnCustomcartId.Value.ToString() + "");

                            }

                        }
                    }
                    else
                    {
                        //if (DiscountPercent > 0)
                        //{
                        if (dsCoupon != null && dsCoupon.Tables.Count > 0 && dsCoupon.Tables[0].Rows.Count > 0)
                        {

                            Session["CouponCodeDiscountPrice"] = DiscountPercent;
                            isEle = true;
                            lblInverror.Text = "Coupon Code Successfully Applied!";
                            Session["CustomerDiscount"] = "1";
                        }
                        //}
                    }


                    bool.TryParse(CommonComponent.GetScalarCommonData("SELECT NoDiscount FROM dbo.tb_Customer WHERE CustomerID=" + Session["CustID"].ToString()).ToString(), out ChkNoDiscount);
                    if (ChkNoDiscount == true && isEle == false)
                    {
                        lblInverror.Text = "You are unable to validate Promo code.";
                        Session["CustomerDiscount"] = null;
                        Session["CouponCodebycustomer"] = null;
                        Session["CouponCodeDiscountPrice"] = null;
                        foreach (RepeaterItem rItem in RptCartItems.Items)
                        {
                            Label lblDiscountPrice = (Label)rItem.FindControl("lblDiscountprice");
                            HiddenField hdnCustomcartId = (HiddenField)rItem.FindControl("hdnCustomcartId");
                            CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET DiscountPrice=0 WHERE CustomCartID=" + hdnCustomcartId.Value.ToString() + "");

                        }

                        return;
                    }
                    else
                    {
                        if (ChkNoDiscount == false)
                        {
                            SPMessage = Convert.ToString(CouponComponent.GetDiscountByCouponCodeFunction(Session["CouponCodebycustomer"].ToString(), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
                        }

                    }

                    try
                    {

                        if (isEle == false)
                        {
                            CouponDiscount = Convert.ToDecimal(SPMessage.ToString());
                            if (CouponDiscount > 0)
                            {

                                Session["CouponCodeDiscountPrice"] = CouponDiscount;
                                lblInverror.Text = "Coupon Code Successfully Applied!";
                            }

                        }
                    }
                    catch
                    {
                        if (!string.IsNullOrEmpty(SPMessage))
                        {
                            if (CouponDiscount == 0)
                            {
                                lblInverror.Text = SPMessage.ToString();
                                Session["CustomerDiscount"] = null;
                                Session["CouponCodebycustomer"] = null;
                                Session["CouponCodeDiscountPrice"] = null;
                            }
                        }
                    }
                    if (Session["CouponCodebycustomer"] == null && Session["CouponCodeDiscountPrice"] == null)
                    {
                        foreach (RepeaterItem rItem in RptCartItems.Items)
                        {

                            HiddenField hdnCustomcartId = (HiddenField)rItem.FindControl("hdnCustomcartId");
                            CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET DiscountPrice=0 WHERE CustomCartID=" + hdnCustomcartId.Value.ToString() + "");

                        }
                    }


                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// Method to bind cart items by customer id
        /// </summary>
        private void BindShoppingCartByCustomerID()
        {
            ViewState["Weight"] = null;
            if (Session["CustID"] != null)
            {
                //String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC) "));
                //if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
                //{
                SwatchQty = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                //}
                DataSet dsShoppingCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));

                if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
                {
                    string strProduct = "";
                    for (int i = 0; i < dsShoppingCart.Tables[0].Rows.Count; i++)
                    {
                        strProduct += Convert.ToString(dsShoppingCart.Tables[0].Rows[i]["Productid"].ToString()) + ",";
                    }

                    ViewState["AllProductsSwatch"] = null;
                    int SwatchCnt = 0;
                    if (!string.IsNullOrEmpty(strProduct.Trim()) && strProduct.Length > 0)
                    {
                        strProduct = strProduct.Substring(0, strProduct.Length - 1);
                        SwatchCnt = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect Count(ProductID) as Isfreefabricswatch from tb_product where ProductID in(" + strProduct + ")  and StoreID = 1 and ItemType='Swatch'"));
                        if (dsShoppingCart.Tables[0].Rows.Count == SwatchCnt)
                        {
                            ViewState["AllProductsSwatch"] = "1";
                        }
                    }

                    DataSet ds = new DataSet();
                    GrandSubTotal = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[0]["SubTotal"].ToString());
                    ViewState["SubTotal"] = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[0]["SubTotal"].ToString());
                    Session["NoOfCartItems"] = dsShoppingCart.Tables[0].Rows[0]["TotalItems"].ToString();
                    RptCartItems.DataSource = dsShoppingCart;
                    RptCartItems.DataBind();
                    Int32 items = Convert.ToInt32(dsShoppingCart.Tables[0].Rows[0]["TotalItems"].ToString());
                    System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)Page.Master.FindControl("cartlink");
                    if (items > 1)
                    {
                        objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " items)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(ViewState["SubTotal"].ToString())) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                    }
                    else
                    {
                        objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " item)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(ViewState["SubTotal"].ToString())) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                    }

                    //objAnchor.HRef = "/addtocart.aspx";
                    btnUpdateCart.Visible = true;
                    lblMessage.Text = "";
                    if (ChkQtyDiscount == true)
                    {
                        trPromocode.Visible = false;
                    }
                    else
                    {
                        trPromocode.Visible = true;
                    }
                }
                else
                {
                    Session["NoOfCartItems"] = null;
                    RptCartItems.DataSource = null;
                    RptCartItems.DataBind();
                    btnClearCart.Visible = false;
                    btnUpdateCart.Visible = false;
                    trPromocode.Visible = false;
                    lblMessage.Text = "Your Shopping Cart is Empty!";
                    lblFreeShippningMsg.Visible = false;
                    Int32 items = 0;
                    System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)Page.Master.FindControl("cartlink");
                    if (items > 1)
                    {
                        objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " items)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(0)) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                    }
                    else
                    {
                        objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " item)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(0)) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                    }
                    objAnchor.HRef = "javascript:void(0);";
                    Response.Redirect("/Index.aspx", true);
                }
            }
            else
            {
                Session["NoOfCartItems"] = null;
                lblMessage.Text = "Your Shopping Cart is Empty!";
                btnClearCart.Visible = false;
                btnUpdateCart.Visible = false;
                trPromocode.Visible = false;
                Int32 items = 0;
                System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)Page.Master.FindControl("cartlink");
                if (items > 1)
                {
                    objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " items)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(0)) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                }
                else
                {
                    objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " item)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(0)) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                }
                objAnchor.HRef = "javascript:void(0);";
                Response.Redirect("/Index.aspx", true);
            }
        }

        protected void RptCartItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {




            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox txtQty = (TextBox)e.Item.FindControl("txtQty");
                Literal ltrSubTotal = (Literal)e.Item.FindControl("ltrSubTotal");
                Label lblNettotal = (Label)e.Item.FindControl("lblNettotal");
                HiddenField IndiSubTotal = (HiddenField)e.Item.FindControl("hdnIndTotal");
                HiddenField hdnprice = (HiddenField)e.Item.FindControl("hdnprice");
                HiddenField hdnRelatedproductID = (HiddenField)e.Item.FindControl("hdnRelatedproductID");
                LinkButton lbtndelete = (LinkButton)e.Item.FindControl("lbtndelete");
                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                HiddenField hdnswatchqty = (HiddenField)e.Item.FindControl("hdnswatchqty");
                HiddenField hdnswatchtype = (HiddenField)e.Item.FindControl("hdnswatchtype");
                System.Web.UI.HtmlControls.HtmlAnchor lnkProductName = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lnkProductName");
                Label lblFreeProductName = (Label)e.Item.FindControl("lblFreeProductName");

                txtQty.Attributes.Add("readonly", "true");
                if (hdnRelatedproductID.Value.ToString() != "0")
                {
                    lbtndelete.Visible = false;
                    txtQty.Enabled = false;
                    lblFreeProductName.Visible = true;
                }
                else
                {
                    lnkProductName.Visible = false;
                    lnkProductName.HRef = "/" + DataBinder.Eval(e.Item.DataItem, "ProductURL");
                }
                if (txtQty != null)
                {
                    txtQty.Attributes.Add("onkeypress", "return isNumberKey(event)");
                }
                HiddenField hdnIndTotal = (HiddenField)e.Item.FindControl("hdnIndTotal");
                HiddenField hdnProductId = (HiddenField)e.Item.FindControl("hdnProductId");

                Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + hdnProductId.Value.ToString() + " and ItemType='Swatch'"));
                if (SwatchQty > Decimal.Zero)
                {

                    if (Isorderswatch == 1)
                    {
                        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + txtQty.Text.ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + hdnProductId.Value.ToString() + ""));
                        if (Convert.ToDecimal(pp) >= SwatchQty)
                        {
                            if (Convert.ToDecimal(pp) >= Convert.ToDecimal(SwatchQty))
                            {
                                lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty)) / Convert.ToDecimal(txtQty.Text.ToString())));
                                lblNettotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty)));
                                hdnprice.Value = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty)) / Convert.ToDecimal(txtQty.Text.ToString())));
                                GrandSubTotal += Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty));
                                hdnswatchqty.Value = txtQty.Text.ToString();
                                hdnswatchtype.Value = "0";
                                SwatchQty = Decimal.Zero;
                            }
                            else
                            {
                                lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                                lblNettotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                                GrandSubTotal += Convert.ToDecimal(0);
                                hdnprice.Value = String.Format("{0:0.00}", Convert.ToDecimal(0));
                                hdnswatchqty.Value = txtQty.Text.ToString();
                                hdnswatchtype.Value = "0";
                                SwatchQty = SwatchQty - Convert.ToDecimal(pp.ToString());
                            }

                        }
                        else
                        {

                            //if (pp > Decimal.Zero)
                            //{
                            lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                            lblNettotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                            GrandSubTotal += Convert.ToDecimal(0);
                            hdnprice.Value = String.Format("{0:0.00}", Convert.ToDecimal(0));
                            hdnswatchqty.Value = txtQty.Text.ToString();
                            hdnswatchtype.Value = "0";
                            SwatchQty = SwatchQty - Convert.ToDecimal(pp.ToString());
                            //}
                        }
                    }


                }
                else
                {
                    if (Isorderswatch == 1)
                    {
                        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + txtQty.Text.ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + hdnProductId.Value.ToString() + ""));
                        if (Convert.ToDecimal(pp) >= Convert.ToDecimal(SwatchQty))
                        {
                            lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) / Convert.ToDecimal(txtQty.Text.ToString())));
                            lblNettotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp.ToString()));
                            hdnprice.Value = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) / Convert.ToDecimal(txtQty.Text.ToString())));
                            GrandSubTotal += Convert.ToDecimal(pp.ToString());
                            hdnswatchqty.Value = txtQty.Text.ToString();
                            hdnswatchtype.Value = "0";

                        }
                    }
                }

                #region customer coupon code
                if (Session["CouponCodebycustomer"] != null && Session["CouponCodeDiscountPrice"] != null)
                {
                    //if (txtPromoCode != null && txtPromoCode.Text != "")
                    //{
                    decimal DiscountPercent, Discountprice, OrginalPrice, DisplayPrice = decimal.Zero;
                    DiscountPercent = Convert.ToDecimal(Session["CouponCodeDiscountPrice"].ToString());

                    if (DiscountPercent > 0 || (Session["CustomerDiscount"] != null && Session["CustomerDiscount"].ToString() == "1"))
                    {
                        System.Web.UI.HtmlControls.HtmlTableCell tdproduct = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdproduct");
                        System.Web.UI.HtmlControls.HtmlTableCell tdSkuattr = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdSku");
                        System.Web.UI.HtmlControls.HtmlTableCell tdqty = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdqty");
                        System.Web.UI.HtmlControls.HtmlTableCell tdDiscountprice = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdDiscountprice");
                        tdproduct.Attributes.Add("style", "width:33% !important;");
                        tdSkuattr.Attributes.Add("style", "width:10% !important;");
                        tdqty.Attributes.Add("style", "width:7% !important;");

                        Label lblDiscountprice = (Label)e.Item.FindControl("lblDiscountprice");
                        if (tdDiscountprice != null)
                        {
                            tdDiscountprice.Visible = true;



                            if (Session["CustomerDiscount"] != null && Session["CustomerDiscount"].ToString() == "1")
                            {
                                #region CheckMembership Discount
                                String ProductId = hdnProductId.Value.ToString();
                                decimal ProductDiscount = 0;
                                decimal CategoryDiscount = 0;

                                decimal Price = 0;
                                bool ChkNoDiscount = false;
                                //String CategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ""));
                                String ParentCategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select cast(ParentCategoryID as nvarchar(500))+',' from tb_CategoryMapping WHERE CategoryID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ") FOR XML PATH('')"));
                                if (ParentCategoryId.Length > 0)
                                {
                                    ParentCategoryId = ParentCategoryId.Substring(0, ParentCategoryId.Length - 1);
                                }
                                else
                                {
                                    ParentCategoryId = "0";
                                }
                                decimal.TryParse(hdnprice.Value.ToString(), out Price);
                                bool.TryParse(CommonComponent.GetScalarCommonData("SELECT NoDiscount FROM dbo.tb_Customer WHERE CustomerID=" + Session["CustID"].ToString()).ToString(), out ChkNoDiscount);

                                ProductDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT top 1 ISNULL(md.Discount,0) AS ProductDiscount "
                                + " FROM dbo.tb_MembershipDiscount md Left OUTER JOIN dbo.tb_Product Prod ON Prod.ProductID =md.DiscountObjectID " +
                                " WHERE md.CustID='" + Session["CustID"].ToString() + "' AND md.DiscountType='product' AND md.StoreID= 1 AND md.DiscountObjectID=" + ProductId + ""));
                                if (ProductDiscount <= 0)
                                {
                                    CategoryDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT top 1 md.Discount AS CategoryDiscount "
                                    + " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                                    + "WHERE md.DiscountType='category' AND  md.CustID= " + Session["CustID"].ToString() + "  AND md.storeid=1 AND (md.DiscountObjectID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ") or md.DiscountObjectID in (" + ParentCategoryId + "))"));
                                    DiscountPercent = CategoryDiscount;
                                }
                                else
                                {
                                    DiscountPercent = ProductDiscount;
                                }

                                decimal.TryParse(hdnprice.Value.ToString(), out OrginalPrice);
                                if (DiscountPercent > 0)
                                {
                                    Discountprice = ((OrginalPrice * DiscountPercent)) / 100;
                                    if (DiscountPercent >= 100)
                                    {
                                        DisplayPrice = Discountprice;
                                    }
                                    else
                                    {
                                        DisplayPrice = OrginalPrice - Discountprice;
                                    }
                                }
                                else
                                {
                                    DisplayPrice = OrginalPrice;
                                }

                                #endregion
                            }
                            else
                            {
                                String strCategory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidForCategory FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CouponCodebycustomer"].ToString() + "'"));
                                String strCategoryPercantage = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT CategoryPercentage FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CouponCodebycustomer"].ToString() + "'"));
                                String strProduct = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidforProduct FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CouponCodebycustomer"].ToString() + "'"));
                                if (!string.IsNullOrEmpty(strCategory))
                                {
                                    DataSet dspp = new DataSet();
                                    dspp = CommonComponent.GetCommonDataSet("SELECT ProductId,CategoryId FROM tb_ProductCategory WHERE ProductId=" + hdnProductId.Value.ToString() + " and categoryId in (" + strCategory.Replace(" ", "") + ")");
                                    Decimal disccat = decimal.Zero;
                                    if (dspp != null && dspp.Tables.Count > 0 && dspp.Tables[0].Rows.Count > 0)
                                    {

                                        string[] pers = { "" };
                                        string[] cats = { "" };
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(strCategoryPercantage))
                                            {
                                                cats = strCategory.Split(',');
                                                pers = strCategoryPercantage.Split(',');
                                                for (int co = 0; co < cats.Length; co++)
                                                {
                                                    if (dspp.Tables[0].Rows[0]["categoryId"].ToString() == cats[co].ToString().Trim())
                                                    {

                                                        Decimal.TryParse(pers[co].ToString(), out disccat);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        catch
                                        {

                                        }


                                        if (disccat > Decimal.Zero)
                                        {
                                            //DiscountPercent = disccat;
                                        }
                                        else
                                        {
                                            disccat = DiscountPercent;
                                        }

                                        decimal.TryParse(hdnprice.Value.ToString(), out OrginalPrice);
                                        Discountprice = ((OrginalPrice * disccat)) / 100;
                                        if (disccat >= 100)
                                        {
                                            DisplayPrice = Discountprice;
                                        }
                                        else
                                        {
                                            DisplayPrice = OrginalPrice - Discountprice;
                                        }
                                    }
                                    else
                                    {
                                        decimal.TryParse(hdnprice.Value.ToString(), out OrginalPrice);
                                        if (!string.IsNullOrEmpty(strProduct))
                                        {
                                            strProduct = "," + strProduct.Replace(" ", "") + ",";
                                            if (strProduct.IndexOf("," + hdnProductId.Value.ToString() + ",") > -1)
                                            {

                                                Discountprice = ((OrginalPrice * DiscountPercent)) / 100;
                                                if (DiscountPercent >= 100)
                                                {
                                                    DisplayPrice = Discountprice;
                                                }
                                                else
                                                {
                                                    DisplayPrice = OrginalPrice - Discountprice;
                                                }
                                            }
                                            else
                                            {
                                                DisplayPrice = OrginalPrice;
                                            }
                                        }
                                        else
                                        {
                                            DisplayPrice = OrginalPrice;
                                        }
                                    }
                                    //strCategory = "," + strCategory.Replace(" ", "");

                                    //string dscategory = "";
                                    //dscategory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT cast(categoRyId as nvarchar(max))+',' FROM tb_category WHERE categoryId in (SELECT categoryId FROM tb_ProductCategory WHERE ProductId=" + hdnProductId.Value.ToString() + ") FOR XML PATH('')"));
                                    //dscategory = "," + dscategory;
                                    //if(dscategory.IndexOf(","+ ))

                                }
                                else if (!string.IsNullOrEmpty(strProduct))
                                {
                                    strProduct = "," + strProduct.Replace(" ", "") + ",";
                                    decimal.TryParse(hdnprice.Value.ToString(), out OrginalPrice);
                                    if (strProduct.IndexOf("," + hdnProductId.Value.ToString() + ",") > -1)
                                    {

                                        Discountprice = ((OrginalPrice * DiscountPercent)) / 100;
                                        if (DiscountPercent >= 100)
                                        {
                                            DisplayPrice = Discountprice;
                                        }
                                        else
                                        {
                                            DisplayPrice = OrginalPrice - Discountprice;
                                        }
                                    }
                                    else
                                    {
                                        DisplayPrice = OrginalPrice;
                                    }
                                }
                                else
                                {
                                    decimal.TryParse(hdnprice.Value.ToString(), out OrginalPrice);
                                    Discountprice = ((OrginalPrice * DiscountPercent)) / 100;
                                    if (DiscountPercent >= 100)
                                    {
                                        DisplayPrice = Discountprice;
                                    }
                                    else
                                    {
                                        DisplayPrice = OrginalPrice - Discountprice;
                                    }
                                }
                            }

                            //if (Isorderswatch == 1)
                            //{
                            //    lblDiscountprice.Text = "0.00";
                            //}
                            //else
                            //{
                            lblDiscountprice.Text = Math.Round(DisplayPrice, 2).ToString();
                            //}



                            //string disprice = string.Format("{0:0.00}", Discountprice);
                            decimal Finalsubtotcouponcode = 0;

                            if (hdnswatchtype.Value.ToString() != "")
                            {
                                //if (Isorderswatch == 1)
                                //{
                                //    Finalsubtotcouponcode = Convert.ToDecimal(lblPrice.Text.ToString()) * Convert.ToDecimal(txtQty.Text.ToString());
                                //}
                                //else
                                //{
                                Finalsubtotcouponcode = Convert.ToDecimal(DisplayPrice) * Convert.ToDecimal(txtQty.Text.ToString());
                                //}

                            }
                            else
                            {
                                Finalsubtotcouponcode = Convert.ToDecimal(DisplayPrice) * Convert.ToDecimal(txtQty.Text.ToString());
                            }

                            ltrSubTotal.Text = string.Format("{0:0.00}", Finalsubtotcouponcode);
                            //  lblNettotal.Text = string.Format("{0:0.00}", Finalsubtotcouponcode);
                            hdnprice.Value = DisplayPrice.ToString();
                            if (hdnswatchtype.Value.ToString() != "")
                            {
                                GrandSubTotal = GrandSubTotal - Convert.ToDecimal(lblNettotal.Text.ToString());
                                GrandSubTotal += Finalsubtotcouponcode;
                            }
                            else
                            {
                                GrandSubTotal += Finalsubtotcouponcode;
                            }
                            lblNettotal.Text = string.Format("{0:0.00}", Finalsubtotcouponcode);
                            ViewState["SubTotal"] = GrandSubTotal;
                        }

                    }
                }
                else
                {
                    ViewState["SubTotal"] = GrandSubTotal;
                }
                #endregion





                Decimal QtyDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SElect ISNULL(qt.DiscountPercent,0) as DiscountPercent from tb_QuantityDiscountTable as qt " +
                                            " inner join tb_QauntityDiscount ON qt.QuantityDiscountID = dbo.tb_QauntityDiscount.QuantityDiscountID  " +
                                            " Where qt.LowQuantity<=" + txtQty.Text.ToString() + " and qt.HighQuantity>=" + txtQty.Text.ToString() + " and tb_QauntityDiscount.QuantityDiscountID in (Select QuantityDiscountID from  " +
                                            " tb_Product Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + hdnProductId.Value.ToString() + ") "));

                decimal Subtotal = Convert.ToDecimal(lblNettotal.Text.ToString());
                if (QtyDiscount > Decimal.Zero)
                {
                    ChkQtyDiscount = true;
                    QtyDiscount = (Convert.ToDecimal(hdnprice.Value.ToString()) * QtyDiscount) / 100;
                    if (!string.IsNullOrEmpty(lblNettotal.Text.ToString().Trim()))
                    {
                        if (QtyDiscount > 0 && Subtotal > 0)
                        {
                            string dd = string.Format("{0:0.00}", QtyDiscount);
                            decimal QtyDis01 = 0;
                            if (hdnswatchtype.Value.ToString() != "")
                            {
                                QtyDis01 = (Convert.ToDecimal(hdnprice.Value) * Convert.ToDecimal(hdnswatchqty.Value.ToString())) - (Convert.ToDecimal(dd) * Convert.ToDecimal(hdnswatchqty.Value.ToString()));
                            }
                            else
                            {
                                QtyDis01 = (Convert.ToDecimal(hdnprice.Value) * Convert.ToDecimal(txtQty.Text.ToString())) - (Convert.ToDecimal(dd) * Convert.ToDecimal(txtQty.Text.ToString()));
                            }
                            ltrSubTotal.Text = "<s>$" + Subtotal + "</s><br />$" + string.Format("{0:0.00}", QtyDis01) + "";
                        }
                        else
                            ltrSubTotal.Text = "$" + Subtotal + "";
                    }
                    if (Session["QtyDiscount"] != null)
                    {
                        string dd = string.Format("{0:0.00}", QtyDiscount);
                        decimal Qtydt = 0;
                        if (hdnswatchtype.Value.ToString() != "")
                        {
                            Qtydt = Convert.ToDecimal(Session["QtyDiscount"].ToString()) + (Convert.ToDecimal(dd) * Convert.ToDecimal(hdnswatchqty.Value.ToString()));
                        }
                        else
                        {
                            Qtydt = Convert.ToDecimal(Session["QtyDiscount"].ToString()) + (Convert.ToDecimal(dd) * Convert.ToDecimal(txtQty.Text.ToString()));
                        }
                        Session["QtyDiscount"] = Qtydt;
                    }
                    else
                    {
                        string dd = string.Format("{0:0.00}", QtyDiscount);
                        if (hdnswatchtype.Value.ToString() != "")
                        {
                            Session["QtyDiscount"] = Convert.ToDouble(dd) * Convert.ToDouble(hdnswatchqty.Value.ToString());
                        }
                        else
                        {
                            Session["QtyDiscount"] = Convert.ToDouble(dd) * Convert.ToDouble(txtQty.Text.ToString());
                        }
                    }
                }
                else
                {
                    ltrSubTotal.Text = "$" + Subtotal + "";
                }



                decimal OldSuboti = 0;
                bool isdecimal = decimal.TryParse(hdnSubtotal.Value.ToString(), out OldSuboti);
                if (isdecimal && OldSuboti > 0)
                {
                    OldSuboti = OldSuboti + Subtotal;
                }
                else
                    OldSuboti = Subtotal;
                hdnSubtotal.Value = OldSuboti.ToString();

                DataSet dsWeight = new DataSet();
                dsWeight = ProductComponent.GetproductImagename(Convert.ToInt32(hdnProductId.Value.ToString()));
                decimal objdecimal = decimal.Zero;
                if (dsWeight != null && dsWeight.Tables.Count > 0 && dsWeight.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dsWeight.Tables[0].Rows[0]["IsFreeShipping"]))
                    {
                        objdecimal = 0;
                    }
                    else
                    {
                        if (Convert.ToDecimal(dsWeight.Tables[0].Rows[0]["weight"].ToString()) > 0)
                            objdecimal = (Convert.ToDecimal(dsWeight.Tables[0].Rows[0]["weight"].ToString()) * Convert.ToDecimal(txtQty.Text));
                        else
                        {
                            objdecimal = (Convert.ToDecimal(1) * Convert.ToDecimal(txtQty.Text));
                        }
                    }
                }
                int GiftCardProductID = 0;
                GiftCardProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(GiftCardProductID,0) FROM dbo.tb_GiftCardProduct Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + hdnProductId.Value.ToString() + ""));


                HiddenField hdnCustomcartId = (HiddenField)e.Item.FindControl("hdnCustomcartId");
                HiddenField hdnVariantvalue = (HiddenField)e.Item.FindControl("hdnVariantvalue");
                HiddenField hdnVariantname = (HiddenField)e.Item.FindControl("hdnVariantname");
                System.Web.UI.HtmlControls.HtmlTableCell tdSku = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdSku");



                string strType = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Isnull(IsProductType,1)  FROM tb_ShoppingCartItems WHERE CustomCartID=" + hdnCustomcartId.Value.ToString() + ""));
                // lnkProductName.HRef = GetProductUrl(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "mainCategory")), Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Sename")), Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ProductId")), Convert.ToString(DataBinder.Eval(e.Item.DataItem, "CustomCartID")));
                if (strType.ToString() == "2")
                {
                    string strSku = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 SKU FROM tb_ProductVariantValue WHERE ProductID = " + hdnProductId.Value.ToString() + " AND VariantValue like '%custom%'"));
                    if (strSku != "")
                    {
                        tdSku.InnerHtml = strSku.ToString();
                    }
                }
                string[] variantValue = hdnVariantvalue.Value.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantName = hdnVariantname.Value.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (strType.ToString() == "3")
                {
                    for (int pp = 0; pp < variantName.Length; pp++)
                    {

                        //if (variantName[pp].ToString().ToLower().IndexOf("color") > -1)
                        //{
                        //    tdSku.InnerHtml = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE VariantValue='" + variantValue[pp].ToString().Trim().Replace("'", "''") + "' and ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + ""));
                        //    break;
                        //}

                    }
                }

                decimal optionweight = 0;

                if (strType.ToString() == "2")
                {
                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and SKU='" + tdSku.InnerHtml.ToString().Trim() + "'"));
                }
                else if (strType.ToString() == "3")
                {
                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and SKU='" + tdSku.InnerHtml.ToString().Trim() + "'"));
                }
                else if (strType.ToString() == "1")
                {
                    for (int i = 0; i < variantName.Length; i++)
                    {
                        if (variantName.Length > i)
                        {
                            if (variantName[i].ToString().ToLower().IndexOf("select size") > -1)
                            {
                                if (variantValue[i].ToString().IndexOf("($") > -1)
                                {
                                    string sttrval = variantValue[i].ToString().Substring(0, variantValue[i].ToString().IndexOf("($"));
                                    sttrval = sttrval.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));

                                    string strSKu = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));
                                    if (strSKu != "")
                                    {
                                        tdSku.InnerHtml = strSKu;
                                    }


                                    break;
                                }
                                else
                                {
                                    string sttrval = variantValue[i].ToString();
                                    sttrval = sttrval.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));
                                    string strSKu = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));
                                    if (strSKu != "")
                                    {
                                        tdSku.InnerHtml = strSKu;
                                    }

                                    break;
                                }
                            }

                        }
                    }
                }
                try
                {
                    decimal pcreation = Convert.ToDecimal(ltrSubTotal.Text.ToString().Replace("$", "").Trim()) / Convert.ToDecimal(txtQty.Text.ToString());
                    strcretio += "{id: \"" + tdSku.InnerHtml.ToString().Trim() + "\", price: " + string.Format("{0:0.00}", Convert.ToDecimal(pcreation)) + ", quantity: " + txtQty.Text.ToString() + "},";
                }
                catch { }



                //if (GiftCardProductID > 0) { }
                //else
                //{
                //    if (ViewState["Weight"] != null)
                //    {
                //        objdecimal += (Convert.ToDecimal(ViewState["Weight"].ToString()) * Convert.ToDecimal(txtQty.Text));
                //        ViewState["Weight"] = objdecimal.ToString();
                //    }
                //    else
                //    {
                //        ViewState["Weight"] = objdecimal.ToString();
                //    }
                //}

                if (GiftCardProductID > 0) { }
                else
                {
                    if (ViewState["Weight"] != null)
                    {
                        if (optionweight > Decimal.Zero)
                        {
                            objdecimal += (Convert.ToDecimal(optionweight.ToString()) * Convert.ToDecimal(txtQty.Text));
                        }
                        else
                        {
                            objdecimal += (Convert.ToDecimal(ViewState["Weight"].ToString()) * Convert.ToDecimal(txtQty.Text));
                        }
                        ViewState["Weight"] = objdecimal.ToString();
                    }
                    else
                    {
                        if (optionweight > Decimal.Zero)
                        {
                            ViewState["Weight"] = optionweight.ToString();
                        }
                        else
                        {
                            ViewState["Weight"] = objdecimal.ToString();
                        }
                    }
                }


                int variantValueCount = variantValue.Count();
                int variantNameCount = variantName.Count();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < variantValueCount; i++)
                {
                    if (variantNameCount > i)
                    {
                        if (variantName[i].ToString().ToLower().IndexOf("estimated delivery") > -1)
                        {
                            if (variantValueCount == (i + 1))
                            {
                                sb.Append(variantName[i].ToString() + " : <span style='color:#B92127;'>" + variantValue[i].ToString() + "</span>");
                            }
                            else
                            {

                                sb.Append(variantName[i].ToString() + " : <span style='color:#B92127;'>" + variantValue[i].ToString() + "</span><br/>");
                            }

                        }
                        else
                        {
                            if (variantValueCount == (i + 1))
                            {
                                sb.Append(variantName[i].ToString() + " : " + variantValue[i].ToString() + "");
                            }
                            else
                            {

                                sb.Append(variantName[i].ToString() + " : " + variantValue[i].ToString() + "<br/>");
                            }
                        }
                    }
                }

                Literal ltrlVariane = (Literal)e.Item.FindControl("ltrlVariane");
                ltrlVariane.Text = sb.ToString();
            }
            if (e.Item.ItemType == ListItemType.Header)
            {

                strcretio = "<script type=\"text/javascript\" src=\"//static.criteo.net/js/ld/ld.js\" async=\"true\"></script> <script type=\"text/javascript\"> window.criteo_q = window.criteo_q || []; window.criteo_q.push( {event: \"setAccount\", account: 6853}, {event: \"setSiteType\", type: \"d\"}, {event: \"viewBasket\", product: [";

                if (Session["CouponCodebycustomer"] != null && Session["CouponCodeDiscountPrice"] != null)
                {
                    GrandSubTotal = 0;
                    System.Web.UI.HtmlControls.HtmlTableCell thproduct = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("thproduct");
                    System.Web.UI.HtmlControls.HtmlTableCell thsku = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("thsku");
                    System.Web.UI.HtmlControls.HtmlTableCell thqty = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("thqty");
                    System.Web.UI.HtmlControls.HtmlTableCell thdisprice = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("thdisprice");
                    thproduct.Width = "33%"; thsku.Width = "10%"; thqty.Width = "7%";


                    if (thdisprice != null)
                    {
                        thdisprice.Visible = true;
                    }
                }
                Session["QtyDiscount"] = null;
                Session["QtyDiscount1"] = null;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (strcretio.Length > 0)
                {
                    strcretio = strcretio.Substring(0, strcretio.Length - 1);
                }
                strcretio += "]}); </script>";

                Label lblordertax = (Label)e.Item.FindControl("lblordertax");
                System.Web.UI.HtmlControls.HtmlTableCell tdordertax = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdordertax");
                ImageButton btnApply1 = (ImageButton)e.Item.FindControl("btnApply1");
                TextBox txtPromoCode = (TextBox)e.Item.FindControl("txtPromoCode");
                btnApply1.OnClientClick = "return validation('" + txtPromoCode.ClientID.ToString() + "');";
                if (Session["CouponCodebycustomer"] != null && Session["CouponCodeDiscountPrice"] != null)
                {

                    System.Web.UI.HtmlControls.HtmlTableCell tdShipping = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdShipping");
                    System.Web.UI.HtmlControls.HtmlTableCell tdCustomlevelDiscount = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdCustomlevelDiscount");
                    System.Web.UI.HtmlControls.HtmlTableCell tdQuantityDiscount = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdQuantityDiscount");
                    System.Web.UI.HtmlControls.HtmlTableCell tdGiftCardAppliedDiscount = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdGiftCardAppliedDiscount");
                    System.Web.UI.HtmlControls.HtmlTableCell tdGiftCardRemainingBalance = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdGiftCardRemainingBalance");
                    System.Web.UI.HtmlControls.HtmlTableCell tdDiscount = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdDiscount");
                    System.Web.UI.HtmlControls.HtmlTableCell tdTotal = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdTotal");

                    tdordertax.ColSpan = 4;
                    tdShipping.ColSpan = 4; tdCustomlevelDiscount.ColSpan = 4; tdQuantityDiscount.ColSpan = 4;
                    tdGiftCardAppliedDiscount.ColSpan = 4; tdGiftCardRemainingBalance.ColSpan = 4; tdDiscount.ColSpan = 4;
                    tdTotal.ColSpan = 4;
                }

                string StrShippingSummary = string.Empty;
                Label lblSubtotal = (Label)e.Item.FindControl("lblSubtotal");
                Label lblShippingcost = (Label)e.Item.FindControl("lblShippingcost");
                System.Web.UI.HtmlControls.HtmlTableRow tblCustomDiscountrow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trCustomlevelDiscount");
                System.Web.UI.HtmlControls.HtmlTableRow trQuantitylDiscount = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trQuantitylDiscount");
                Label lblCustomlevel = (Label)e.Item.FindControl("lblCustomlevel");
                Label lblDiscount = (Label)e.Item.FindControl("lblDiscount");
                Label lblQuantityDiscount = (Label)e.Item.FindControl("lblQuantityDiscount");
                bool ChkNoDiscount = false;
                bool.TryParse(CommonComponent.GetScalarCommonData("SELECT NoDiscount FROM dbo.tb_Customer WHERE CustomerID=" + Session["CustID"].ToString()).ToString(), out ChkNoDiscount);
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
                #region Code for Gift Certificate

                System.Web.UI.HtmlControls.HtmlTableRow trGiftCertiDiscount = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trGiftCertiDiscount");
                System.Web.UI.HtmlControls.HtmlTableRow trGiftCardRemBal = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trGiftCardRemBal");
                Label lblGiftCertiDiscount = (Label)e.Item.FindControl("lblGiftCertiDiscount");
                Label lblGiftCardRemBal = (Label)e.Item.FindControl("lblGiftCardRemBal");

                #endregion

                string strShippingName = "";
                string strShippingCharge = "0.00";

                if (ViewState["CustomerLevelFreeShipping"] == null)
                {
                    bool IsFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasFreeShipping,0) AS LevelHasFreeShipping  FROM dbo.tb_Customer INNER JOIN dbo.tb_CustomerLevel ON dbo.tb_Customer.CustomerLevelID = dbo.tb_CustomerLevel.CustomerLevelID AND tb_customer.CustomerID=" + Convert.ToInt32(Session["CustID"]) + ""));

                    if (IsFreeShipping)
                    {
                        ViewState["CustomerLevelFreeShipping"] = "1";
                    }
                    else
                    {
                        if (rdoShippingMethod.SelectedIndex > -1)
                        {
                            strShippingName = rdoShippingMethod.SelectedItem.Text.ToString();
                            if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                            {
                                strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                                strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();
                                Decimal.TryParse(strShippingCharge, out ShippingCharges);
                                ViewState["ShippingCharges"] = ShippingCharges.ToString("F2");
                                ViewState["ShippingChargesBind"] = ShippingCharges.ToString("F2");
                            }
                        }
                    }
                }
                else
                {
                    if (rdoShippingMethod.SelectedIndex > -1)
                    {
                        strShippingName = rdoShippingMethod.SelectedItem.Text.ToString();
                        if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                        {
                            strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                            strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();
                            Decimal.TryParse(strShippingCharge, out ShippingCharges);
                            ViewState["ShippingCharges"] = ShippingCharges.ToString("F2");
                            ViewState["ShippingChargesBind"] = ShippingCharges.ToString("F2");
                        }
                    }
                }

                if (strShippingCharge.ToString() != "")
                {
                    lblShippingcost.Text = String.Format("{0:0.00}", Convert.ToDecimal(strShippingCharge));
                }

                if (lblSubtotal != null)
                {
                    #region Customer level Discount
                    if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                    {
                        Decimal CustomerlevelDiscount = Decimal.Zero;

                        DataSet dsDiscount = new DataSet();
                        dsDiscount = CommonComponent.GetCommonDataSet("SELECT isnull(LevelDiscountPercent,0) as LevelDiscountPercent,isnull(LevelDiscountAmount,0) as LevelDiscountAmount FROM tb_CustomerLevel WHERE CustomerLevelID in (SELECT isnull(CustomerLevelID,0) FROM tb_Customer WHERE CustomerID = " + Session["CustID"].ToString() + ")");
                        if (dsDiscount != null && dsDiscount.Tables.Count > 0 && dsDiscount.Tables[0].Rows.Count > 0)
                        {
                            Decimal CustomePersantage = Convert.ToDecimal(dsDiscount.Tables[0].Rows[0]["LevelDiscountPercent"].ToString());
                            if (CustomePersantage == Decimal.Zero)
                            {
                                CustomerlevelDiscount = Convert.ToDecimal(dsDiscount.Tables[0].Rows[0]["LevelDiscountAmount"].ToString());
                            }
                            else
                            {
                                CustomerlevelDiscount = (Convert.ToDecimal(GrandSubTotal) * CustomePersantage) / 100;
                            }

                        }
                        if (CustomerlevelDiscount > Decimal.Zero)
                        {
                            //tblCustomDiscountrow.Visible = true;
                            //lblCustomlevel.Text = String.Format("{0:0.00}", Convert.ToDecimal(CustomerlevelDiscount.ToString()));
                            //GrandSubTotal = GrandSubTotal - CustomerlevelDiscount;

                            // **** Code By Girish ****
                            Decimal SubTotalCustomer = GrandSubTotal;
                            Decimal DiscountCustomer = Math.Round((SubTotalCustomer - CustomerlevelDiscount), 2);
                            if (DiscountCustomer < 0)
                            {
                                GrandSubTotal = 0;
                                DiscountCustomer = SubTotalCustomer;
                            }
                            else
                            {
                                GrandSubTotal = GrandSubTotal - CustomerlevelDiscount;
                                DiscountCustomer = CustomerlevelDiscount;
                            }
                            tblCustomDiscountrow.Visible = true;
                            lblCustomlevel.Text = DiscountCustomer.ToString("F2");
                        }
                    }
                    #endregion

                    if (Session["QtyDiscount"] != null)
                    {
                        decimal Qty = 0;
                        decimal.TryParse(Session["QtyDiscount"].ToString(), out Qty);
                        lblQuantityDiscount.Text = Math.Round(Qty, 2).ToString();
                        if (Convert.ToDecimal(Session["QtyDiscount"].ToString()) > Decimal.Zero)
                        {
                            trQuantitylDiscount.Visible = true;
                        }
                    }
                    else
                    {
                        lblQuantityDiscount.Text = "0.00";
                    }



                    #region Code for Gift Certificate

                    if (Session["GiftCertificateDiscount"] != null)
                    {
                        lblGiftCertiDiscount.Text = Convert.ToString(Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString()));
                        if (Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString()) > Decimal.Zero)
                        {
                            trGiftCertiDiscount.Visible = true;
                        }
                        Session["GiftCertificateDiscount"] = lblGiftCertiDiscount.Text;
                    }
                    else
                    {
                        lblGiftCertiDiscount.Text = "0.00";
                    }

                    if (Session["GiftCertificateRemaningBalance"] != null)
                    {
                        lblGiftCardRemBal.Text = Convert.ToString(Convert.ToDecimal(Session["GiftCertificateRemaningBalance"].ToString()));
                        if (Convert.ToDecimal(Session["GiftCertificateRemaningBalance"].ToString()) > Decimal.Zero)
                        {
                            trGiftCardRemBal.Visible = true;
                        }
                    }
                    else
                    {
                        lblGiftCardRemBal.Text = "0.00";
                    }

                    #endregion

                    Decimal SubTot = 0;
                    if (Session["Discount"] != null || Session["GiftCertificateDiscount"] != null)
                    {
                        decimal TotalDiscount = Convert.ToDecimal(Session["Discount"]);
                        SubTot = Convert.ToDecimal(GrandSubTotal) + Convert.ToDecimal(strShippingCharge) - Convert.ToDecimal(lblQuantityDiscount.Text.ToString());
                        if (Session["Discount"] != null)
                        {
                            Decimal SubTotalDiscount = SubTot;
                            Decimal TotalCouponDiscount = Math.Round((SubTotalDiscount - TotalDiscount), 2);
                            if (TotalCouponDiscount < 0)
                            {
                                SubTot = 0;
                                TotalCouponDiscount = SubTotalDiscount;
                            }
                            else
                            {
                                SubTot = SubTot - TotalDiscount;
                                TotalCouponDiscount = TotalDiscount;
                            }
                            lblDiscount.Text = TotalCouponDiscount.ToString("F2");
                            lblSubtotal.Text = SubTot.ToString("F2");
                            FinalTotal = SubTot;
                        }
                        if (Session["GiftCertificateDiscount"] != null)
                        {
                            if (SubTot < 0)
                            {
                                lblGiftCertiDiscount.Text = "0.00";
                                lblGiftCardRemBal.Text = Convert.ToDecimal(Session["GiftCertificateDiscount"]).ToString("F2");
                                trGiftCardRemBal.Visible = true;
                            }
                            else
                            {
                                decimal GiftCardAmount = Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString());
                                if (GiftCardAmount != Decimal.Zero)
                                {
                                    SubTot = SubTot - Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString());
                                }
                                if (SubTot < 0 && GiftCardAmount != Decimal.Zero)
                                {
                                    decimal TotGiftCertiDiscount = Convert.ToDecimal(lblGiftCertiDiscount.Text);
                                    lblGiftCertiDiscount.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TotGiftCertiDiscount) - Math.Abs(Math.Round(SubTot, 2)), 2));
                                    lblSubtotal.Text = "0.00";
                                    trGiftCardRemBal.Visible = true;
                                    lblGiftCardRemBal.Text = Math.Abs(Math.Round(SubTot, 2)).ToString("F2");
                                }
                                else
                                {
                                    Session["GiftCertificateDiscount"] = Math.Round(GiftCardAmount, 2);
                                    Session["GiftCertificateRemaningBalance"] = "0.00";
                                    lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(SubTot.ToString()));
                                    trGiftCardRemBal.Visible = false;
                                    lblGiftCardRemBal.Text = "0.00";
                                }
                            }

                        }
                        if (Convert.ToDecimal(lblSubtotal.Text) < 0)
                        {
                            FinalTotal = Convert.ToDecimal(0);
                            lblSubtotal.Text = "0.00";
                        }
                        else
                        {
                            FinalTotal = Convert.ToDecimal(lblSubtotal.Text);
                        }

                    }
                    else
                    {
                        SubTot = Convert.ToDecimal(GrandSubTotal) + Convert.ToDecimal(strShippingCharge) - Convert.ToDecimal(lblQuantityDiscount.Text.ToString());
                        if (SubTot < 0)
                        {
                            lblSubtotal.Text = "0.00";
                        }
                        else
                        {
                            lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(SubTot.ToString()));
                        }
                        FinalTotal = Convert.ToDecimal(lblSubtotal.Text);
                        lblDiscount.Text = "0.00";
                    }
                    ViewState["FinalTotal"] = FinalTotal.ToString("F2");
                    ChkFreeshipping();

                    #region Order tax
                    decimal OrderTax = decimal.Zero;
                    if (hdnState.Value != null && hdnZipCode.Value != null && FinalTotal > 0)
                    {
                        if (ViewState["SubTotal"] != null)
                        {
                            OrderTax = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Math.Round(Convert.ToDecimal(ViewState["SubTotal"].ToString().Replace("$", "")), 2));
                        }
                        else
                        {
                            OrderTax = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), Math.Round(Convert.ToDecimal(lblSubtotal.Text.ToString().Replace("$", "")), 2));
                        }

                        if (OrderTax > 0)
                        {
                            ViewState["FinalTotal"] = Convert.ToDecimal(ViewState["FinalTotal"]) + OrderTax;
                            lblordertax.Text = Math.Round(OrderTax, 2).ToString();
                            lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(ViewState["FinalTotal"].ToString()));
                        }
                    }
                    else
                    {
                        lblordertax.Text = "0.00";
                    }

                    #endregion

                }

                StrShippingSummary = "<table width=\"100%\">";
                StrShippingSummary += "<tbody>";
                StrShippingSummary += "<tr>";
                StrShippingSummary += "<td width=\"66%\" align=\"right\">Sub Total</td>";
                if (!string.IsNullOrEmpty(hdnSubtotal.Value.Trim()) && Convert.ToDecimal(hdnSubtotal.Value) > 0)
                {
                    StrShippingSummary += "<td align=\"right\" style='padding-left:0px;padding-right:16px;'>" + "$" + hdnSubtotal.Value + "</td>";
                }
                else
                {
                    StrShippingSummary += "<td align=\"right\" style='padding-left:0px;padding-right:16px;'>$6.29</td>";
                }
                StrShippingSummary += "</tr>";
                hdnSubtotal.Value = "0";

                StrShippingSummary += "<tr>";
                StrShippingSummary += "<td width=\"66%\" align=\"right\">Shipping </td>";
                if (!string.IsNullOrEmpty(lblShippingcost.Text.Trim()) && Convert.ToDecimal(lblShippingcost.Text) > 0)
                {
                    StrShippingSummary += "<td align=\"right\" style='padding-left:0px;padding-right:16px;'>" + "$" + lblShippingcost.Text + "</td>";
                }
                else
                {
                    StrShippingSummary += "<td align=\"right\" style='padding-left:0px;padding-right:16px;'>$0.00</td>";
                }
                StrShippingSummary += "</tr>";

                if (!string.IsNullOrEmpty(lblCustomlevel.Text.Trim()) && Convert.ToDecimal(lblCustomlevel.Text) > 0)
                {
                    StrShippingSummary += "<tr>";
                    StrShippingSummary += "<td width=\"66%\" align=\"right\">Customer Level Discount </td>";
                    StrShippingSummary += "<td align=\"right\" style='padding-left:0px;padding-right:16px;'>" + "$" + lblCustomlevel.Text + "</td>";
                    StrShippingSummary += "</tr>";
                }

                if (Session["GiftCertificateDiscount"] != null)
                {
                    if (Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString()) > Decimal.Zero)
                    {
                        StrShippingSummary += "<tr>";
                        StrShippingSummary += "<td width=\"66%\" align=\"right\">Gift Card Applied Discount </td>";
                        if (!string.IsNullOrEmpty(lblGiftCertiDiscount.Text.Trim()) && Convert.ToDecimal(lblGiftCertiDiscount.Text) > 0)
                        {
                            StrShippingSummary += "<td align=\"right\" style='padding-left:0px;padding-right:16px;'>" + "$" + lblGiftCertiDiscount.Text + "</td>";
                        }
                        StrShippingSummary += "</tr>";
                    }
                }

                if (Session["GiftCertificateRemaningBalance"] != null)
                {
                    if (Convert.ToDecimal(Session["GiftCertificateRemaningBalance"].ToString()) > Decimal.Zero)
                    {
                        StrShippingSummary += "<tr>";
                        StrShippingSummary += "<td width=\"66%\" align=\"right\">Gift Card Remaining Balance </td>";
                        if (!string.IsNullOrEmpty(lblGiftCardRemBal.Text.Trim()) && Convert.ToDecimal(lblGiftCardRemBal.Text) > 0)
                        {
                            StrShippingSummary += "<td align=\"right\" style='padding-left:0px;padding-right:16px;'>" + "$" + lblGiftCardRemBal.Text + "</td>";
                        }
                        StrShippingSummary += "</tr>";
                    }
                }
                if (Session["QtyDiscount"] != null)
                {
                    decimal QtyDiscount = decimal.Zero;
                    decimal.TryParse(Session["QtyDiscount"].ToString(), out QtyDiscount);
                    if (QtyDiscount > Decimal.Zero)
                    {
                        StrShippingSummary += "<tr>";
                        StrShippingSummary += "<td width=\"66%\" align=\"right\"> Quantity Discount</td>";
                        StrShippingSummary += "<td align=\"right\" style='padding-left:0px;padding-right:16px;'>" + "$" + Math.Round(QtyDiscount, 2) + "</td>";
                        StrShippingSummary += "</tr>";
                    }
                }
                StrShippingSummary += "<tr>";
                StrShippingSummary += "<td width=\"66%\" align=\"right\">Discount </td>";
                if (!string.IsNullOrEmpty(lblDiscount.Text.Trim()) && Convert.ToDecimal(lblDiscount.Text) > 0)
                {
                    StrShippingSummary += "<td align=\"right\" style='padding-left:0px;padding-right:16px;'>" + "$" + lblDiscount.Text + "</td>";
                }
                else
                {
                    StrShippingSummary += "<td align=\"right\" style='padding-left:0px;padding-right:16px;'>$0.00</td>";
                }
                StrShippingSummary += "</tr>";


                StrShippingSummary += "<tr>";
                StrShippingSummary += "<td align=\"right\" style=\"border-top: 1px solid #EBEBEB;padding-left:0px;padding-right:16px;\">";
                StrShippingSummary += "<b>Total</b>";
                StrShippingSummary += "</td>";
                StrShippingSummary += "<td align=\"right\" style=\"border-top: 1px solid #EBEBEB;padding-left:0px;padding-right:16px;\">";
                if (!string.IsNullOrEmpty(lblSubtotal.Text.Trim()) && Convert.ToDecimal(lblSubtotal.Text) > 0)
                {
                    StrShippingSummary += "<b>" + "$" + lblSubtotal.Text + "</b>";
                    ViewState["OrderTotal"] = lblSubtotal.Text.ToString();
                }
                else
                {
                    StrShippingSummary += "<b>$0.00</b>";
                    ViewState["OrderTotal"] = "0.00";
                }
                StrShippingSummary += "</td>";
                StrShippingSummary += "</tr>";
                StrShippingSummary += "</tbody>";
                StrShippingSummary += "</table>";
                if (ViewState["SubTotal"] != null)
                {
                    lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(ViewState["SubTotal"].ToString()));
                }
                else { lblSubtotal.Text = "0.00"; }
                hdnSubTotalofProduct.Value = lblSubtotal.Text;
                StrShippingSummary = "";
                StrShippingSummary = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"border: 1px solid #D22D4F;\" class=\"table-none\">";
                StrShippingSummary += "<tbody><tr>";
                StrShippingSummary += "<th style=\"border: 1px solid #D22D4F; border-bottom:none;\"> <span style=\"font-family: Arial,Helvetica,sans-serif;\" class=\"img-left\">Order Summary</span> </th>";
                StrShippingSummary += "</tr>";

                StrShippingSummary += "<tr>";
                StrShippingSummary += "<td><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"width: 100% !important;\" class=\"table-none-shipp\">";
                StrShippingSummary += " <tbody><tr>";
                StrShippingSummary += "  <td valign=\"middle\" style=\"width: 13%;\">Sub Total</td>";
                StrShippingSummary += " <td valign=\"middle\" align=\"center\" style=\"width: 2%;\">: </td>";
                if (ViewState["SubTotal"] != null)
                {
                    StrShippingSummary += " <td valign=\"middle\" align=\"right\" style=\"width: 44%;\">$" + String.Format("{0:0.00}", Convert.ToDecimal(ViewState["SubTotal"].ToString())) + "</td>";
                }
                else
                {
                    StrShippingSummary += " <td valign=\"middle\" align=\"right\" style=\"width: 44%;\">$0.00</td>";
                }
                StrShippingSummary += "  </tr>";
                StrShippingSummary += " <tr>";
                StrShippingSummary += " <td valign=\"middle\" style=\"width: 13%;\">Shipping</td>";
                StrShippingSummary += " <td valign=\"middle\" align=\"center\" style=\"width: 2%;\">: </td>";
                if (!string.IsNullOrEmpty(lblShippingcost.Text.Trim()) && Convert.ToDecimal(lblShippingcost.Text) > 0)
                {

                    StrShippingSummary += "  <td valign=\"middle\" align=\"right\" style=\"width: 44%;\">$" + String.Format("{0:0.00}", Convert.ToDecimal(lblShippingcost.Text.ToString())) + "</td>";
                }
                else
                {
                    StrShippingSummary += "  <td valign=\"middle\" align=\"right\" style=\"width: 44%;\">$0.00</td>";
                }
                StrShippingSummary += " </tr>";




                StrShippingSummary += "<tr>";
                StrShippingSummary += "<td valign=\"middle\" style=\"width: 13%;\">Order Tax</td>";
                StrShippingSummary += " <td valign=\"middle\" align=\"center\" style=\"width: 2%;\">: </td>";
                if (!string.IsNullOrEmpty(lblDiscount.Text.Trim()) && Convert.ToDecimal(lblordertax.Text) > 0)
                {
                    StrShippingSummary += " <td valign=\"middle\" align=\"right\" style=\"width: 44%;\">$" + lblordertax.Text + "</td>";
                }
                else
                {
                    StrShippingSummary += " <td valign=\"middle\" align=\"right\" style=\"width: 44%;\">$0.00</td>";
                }
                StrShippingSummary += "</tr>";





                StrShippingSummary += " <tr>";
                StrShippingSummary += "<td valign=\"middle\" style=\"width: 13%;\" class=\"border-top\"><strong>Total</strong></td>";
                StrShippingSummary += "   <td valign=\"middle\" align=\"center\" style=\"width: 2%;\" class=\"border-top\">: </td>";
                if (ViewState["FinalTotal"] != null)
                {
                    StrShippingSummary += "  <td valign=\"middle\" align=\"right\" style=\"width: 44%;\" class=\"border-top\">$" + String.Format("{0:0.00}", Convert.ToDecimal(ViewState["FinalTotal"].ToString())) + "</td>";

                }
                else
                {
                    StrShippingSummary += "  <td valign=\"middle\" align=\"right\" style=\"width: 44%;\" class=\"border-top\">$0.00</td>";
                }
                StrShippingSummary += "  </tr>";
                //StrShippingSummary += " <tr>";
                //if (btnPlaceOrder.Visible == true)
                //{
                //    StrShippingSummary += "  <td valign=\"middle\" align=\"right\" colspan=\"3\" class=\"border-top\"><a href=\"javascript:void(0);\" onclick=\"javascript:document.getElementById('" + btnPlaceOrder.ClientID.ToString() + "').click();\"><img   alt=\"SUBMIT\" title=\"SUBMIT\" src=\"/images/submit-order-place.png\" /></a></td>";
                //}
                //else
                //{
                //    StrShippingSummary += "  <td valign=\"middle\" align=\"right\" colspan=\"3\" class=\"border-top\"><a href=\"javascript:void(0);\"><img   alt=\"SUBMIT\" title=\"SUBMIT\" src=\"/images/submit-order-place.png\" /></a></td>";
                //}
                //StrShippingSummary += "  </tr>";

                StrShippingSummary += "</tbody></table></td>";
                StrShippingSummary += " </tr>";
                StrShippingSummary += "</tbody></table>";



                ltrShippingSummary.Text = StrShippingSummary.ToString();
            }
        }

        /// <summary>
        /// Free Shipping Over Specified Amount
        /// </summary>
        private void ChkFreeshipping()
        {
            #region Code for Free Shipping Over Specified Amount
            Int32 ShippingCountry = 0;
            if ((ddlBillcountry.SelectedValue != "Select Country" && ddlBillcountry.SelectedValue != "") || (ddlShipCounry.SelectedValue != "Select Country" && ddlShipCounry.SelectedValue != ""))
            {
                if (chkcopy.Checked)
                    ShippingCountry = Convert.ToInt32(ddlBillcountry.SelectedValue);
                else
                    ShippingCountry = Convert.ToInt32(ddlShipCounry.SelectedValue);
            }
            if ((Convert.ToInt32(ShippingCountry) == 1 && rdoShippingMethod.Items.Count == 0) || (Convert.ToInt32(ShippingCountry) == 1 && rdoShippingMethod.Items.Count > 0))
            {
                if (ViewState["FinalTotal"] != null && ViewState["ShippingCharges"] != null)
                {
                    lblFreeShippningMsg.Visible = true;
                    Decimal FinalTotalNotShipping = Convert.ToDecimal(ViewState["FinalTotal"]) - Convert.ToDecimal(ViewState["ShippingCharges"]);
                    if (FinalTotalNotShipping > FreeShippingAmount)
                    {
                        // lblFreeShippningMsg.Text = "Congratulations!! You qualified for Free Shipping. ( United States Only )";
                    }
                    else
                    {
                        if (FinalTotalNotShipping < 0)
                        {
                            FinalTotalNotShipping = 0;
                        }
                        Decimal TotalDiff = FreeShippingAmount - FinalTotalNotShipping;
                        //lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                    }
                }
                else
                {
                    lblFreeShippningMsg.Visible = false;
                }
            }
            else
            {
                lblFreeShippningMsg.Visible = false;
            }
            #endregion
        }

        protected void RptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                objCart.DeleteCartItemByCustomCartID(Convert.ToInt32(e.CommandArgument));
                CouponCodeCalculation();
                if (chkcopy.Checked == true)
                {
                    GetShippingMethodByBillAddress();
                }
                else { GetShippingMethodByShipAddress(); }
                BindShoppingCartByCustomerID();
                BindShippingMethod();
                BindOrderSummary();


                UpdateMinicart();
            }
        }

        /// <summary>
        /// Clear Shopping Cart
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnClearCart_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
            {
                ShoppingCartComponent objWishlist = new ShoppingCartComponent();
                objWishlist.AddToWishList(Convert.ToInt32(Session["CustID"].ToString()));
                Session["CouponCode"] = null;
                Session["CouponCodebycustomer"] = null;
                Session["CouponCodeDiscountPrice"] = null;
                Session["CustomerDiscount"] = null;
                Response.Redirect("/wishlist.aspx", true);
            }
            else
            {
                Response.Redirect("/login.aspx?wishlist=1", true);
            }
        }

        /// <summary>
        /// Insert Values in the Cart table also add another items
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnAddAnotherItem_Click(object sender, ImageClickEventArgs e)
        {
            if (ViewState["Urlrefferer"] != null)
            {
                DataSet dsShoppingCart = new DataSet();
                if (Session["CustID"] != null)
                    dsShoppingCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));

                string URlString = ViewState["Urlrefferer"].ToString().ToLower();
                if (URlString.Contains("gi-"))
                {
                    Response.Redirect("/GiftCertificate.aspx");
                }
                else if (URlString.Contains("/CustomerquoteCheckout.aspx"))
                {
                    Response.Redirect("/index.aspx");
                }
                else if (URlString.Contains("/createaccount.aspx"))
                {
                    Response.Redirect("/index.aspx");
                }
                else if (URlString.Contains("/addtocart.aspx"))
                {
                    Response.Redirect("/index.aspx");
                }
                else
                {
                    Response.Redirect(ViewState["Urlrefferer"].ToString());
                }
            }
            else
            {
                Response.Redirect("/index.aspx");
            }
        }

        /// <summary>
        /// Method to Set Product Image
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Image</returns>
        public String GetIconImage(String img)
        {
            clsvariables.LoadAllPath();
            String[] AllowedExtensions = AppLogic.AppConfigs("AllowedExtensions").Split(',');
            String imagepath = String.Empty;
            Random rd = new Random();
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Micro/" + img;
            if (img != "")
            {
                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (imagepath)))
                {
                    return AppLogic.AppConfigs("Live_Contant_Server") + imagepath + "?" + rd.Next(1000).ToString();
                }
            }
            else
            {
                return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
            }

            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
        }
        #endregion

        /// <summary>
        ///  Apply Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnApply_Click(object sender, ImageClickEventArgs e)
        {
            Session["CouponCode"] = null;
            Session["CouponCodebycustomer"] = null;
            Session["CouponCodeDiscountPrice"] = null;
            Session["CustomerDiscount"] = null;
            //foreach (RepeaterItem rptfooter in RptCartItems.Items)
            //{
            //    txtPromoCode = (TextBox)rptfooter.FindControl("txtPromoCode");
            //}
            Control FooterTemplate = RptCartItems.Controls[RptCartItems.Controls.Count - 1].Controls[0];
            TextBox txtPromoCode = FooterTemplate.FindControl("txtPromoCode") as TextBox;


            if (Session["CustID"] != null && txtPromoCode != null)
            {


                ViewState["Weight"] = null;
                lblInverror.Text = "";
                if (txtPromoCode.Text.ToString().Trim() != "")
                {
                    if (RptCartItems.Items.Count == 1)
                    {
                        HiddenField hdnProductId = (HiddenField)RptCartItems.Items[0].FindControl("hdnProductId");
                        if (hdnProductId.Value.ToString() == AppLogic.AppConfigs("HotDealProduct").ToString())
                        {
                            lblInverror.Text = "You can't use coupon code for deal of the day product!";
                            Session["CouponCode"] = null;
                            Session["Discount"] = null;
                            return;
                        }

                    }
                    decimal CouponDiscount = 0;
                    decimal GiftCardAmount = 0;
                    bool ChkNoDiscount = false;
                    String SPMessage = "";
                    decimal DiscountPercent = decimal.Zero;
                    bool isEle = false;
                    DataSet dsCoupon = new DataSet();
                    dsCoupon = CommonComponent.GetCommonDataSet("SELECT 0 AS DiscountPercent,FromDate,ToDate  FROM dbo.tb_Customer WHERE CustomerID=" + Session["CustID"].ToString() + " And CouponCode ='" + txtPromoCode.Text.ToString() + "'");
                    string StrFromdate = "";
                    string StrTodate = "";
                    //out DiscountPercent);
                    if (dsCoupon != null && dsCoupon.Tables.Count > 0 && dsCoupon.Tables[0].Rows.Count > 0)
                    {
                        DiscountPercent = Convert.ToDecimal(dsCoupon.Tables[0].Rows[0]["DiscountPercent"]);
                        StrFromdate = Convert.ToString(dsCoupon.Tables[0].Rows[0]["FromDate"].ToString());
                        StrTodate = Convert.ToString(dsCoupon.Tables[0].Rows[0]["ToDate"].ToString());
                    }
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
                            //if (DiscountPercent > 0)
                            //{
                            Session["CouponCodebycustomer"] = txtPromoCode.Text;
                            Session["CouponCodeDiscountPrice"] = DiscountPercent;
                            isEle = true;
                            lblInverror.Text = "Coupon Code Successfully Applied!";
                            Session["CustomerDiscount"] = "1";
                            //}
                        }
                        else
                        {
                            isEle = true;
                            lblInverror.Text = "Sorry, Coupon code is expired!";
                            foreach (RepeaterItem rItem in RptCartItems.Items)
                            {
                                Label lblDiscountPrice = (Label)rItem.FindControl("lblDiscountprice");
                                HiddenField hdnCustomcartId = (HiddenField)rItem.FindControl("hdnCustomcartId");
                                CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET DiscountPrice=0 WHERE CustomCartID=" + hdnCustomcartId.Value.ToString() + "");

                            }
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgCouponcode", "alert('Sorry, Coupon code is expired!');", true);
                        }
                    }
                    else
                    {
                        //if (DiscountPercent > 0)
                        //{
                        if (dsCoupon != null && dsCoupon.Tables.Count > 0 && dsCoupon.Tables[0].Rows.Count > 0)
                        {
                            Session["CouponCodebycustomer"] = txtPromoCode.Text;
                            Session["CouponCodeDiscountPrice"] = DiscountPercent;
                            isEle = true;
                            lblInverror.Text = "Coupon Code Successfully Applied!";
                            Session["CustomerDiscount"] = "1";
                        }
                        //}
                    }


                    bool.TryParse(CommonComponent.GetScalarCommonData("SELECT NoDiscount FROM dbo.tb_Customer WHERE CustomerID=" + Session["CustID"].ToString()).ToString(), out ChkNoDiscount);
                    if (ChkNoDiscount == true && isEle == false)
                    {
                        lblInverror.Text = "You are unable to validate Promo code.";
                        Session["CustomerDiscount"] = null;
                        return;
                    }
                    else
                    {
                        if (ChkNoDiscount == false)
                        {
                            SPMessage = Convert.ToString(CouponComponent.GetDiscountByCouponCodeFunction(txtPromoCode.Text.Trim(), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
                        }

                    }

                    try
                    {

                        if (isEle == false)
                        {
                            CouponDiscount = Convert.ToDecimal(SPMessage.ToString());
                            if (CouponDiscount > 0)
                            {
                                Session["CouponCodebycustomer"] = txtPromoCode.Text;
                                Session["CouponCodeDiscountPrice"] = CouponDiscount;
                                lblInverror.Text = "Coupon Code Successfully Applied!";
                            }
                            txtPromoCode.Text = "";
                        }
                    }
                    catch
                    {
                        if (CouponDiscount == 0)
                        {
                            GiftCardAmount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Balance FROM [tb_GiftCard] where storeid=" + AppLogic.AppConfigs("StoreID") + " And SerialNumber='" + txtPromoCode.Text.Trim() + "'"));
                            if (GiftCardAmount == Decimal.Zero)
                            {
                                Session["GiftCertificateDiscountCode"] = null;
                                Session["GiftCertificateDiscount"] = null;
                                Session["CouponCode"] = null;
                                Session["Discount"] = null;
                                lblInverror.Text = "Sorry, we are unable to validate Promo code given by you.";
                            }
                            else
                            {
                                Session["GiftCertificateDiscountCode"] = txtPromoCode.Text.ToString().Trim();
                                Session["GiftCertificateDiscount"] = Math.Round(GiftCardAmount, 2);
                                GiftCardTotalDiscount = Math.Round(GiftCardAmount, 2);
                            }
                        }
                    }
                    if (Session["CouponCodebycustomer"] == null && Session["CouponCodeDiscountPrice"] == null)
                    {
                        foreach (RepeaterItem rItem in RptCartItems.Items)
                        {

                            HiddenField hdnCustomcartId = (HiddenField)rItem.FindControl("hdnCustomcartId");
                            CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET DiscountPrice=0 WHERE CustomCartID=" + hdnCustomcartId.Value.ToString() + "");

                        }
                    }
                    if (chkcopy.Checked == true)
                    {
                        GetShippingMethodByBillAddress();
                    }
                    else { GetShippingMethodByShipAddress(); }
                    BindShoppingCartByCustomerID();

                    BindShippingMethod();
                    BindOrderSummary();

                    foreach (RepeaterItem rItem in RptCartItems.Items)
                    {
                        Label lblDiscountPrice = (Label)rItem.FindControl("lblDiscountprice");
                        HiddenField hdnCustomcartId = (HiddenField)rItem.FindControl("hdnCustomcartId");
                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET DiscountPrice=" + lblDiscountPrice.Text.ToString() + " WHERE CustomCartID=" + hdnCustomcartId.Value.ToString() + "");

                    }
                }
                else
                {
                    lblInverror.Text = "Please Enter Coupon Code.";
                    txtPromoCode.Focus();
                }
                UpdateMinicart();
            }
            else
            {
                Session["NoOfCartItems"] = null;
                lblMessage.Text = "Your Shopping Cart is Empty!";
                btnClearCart.Visible = false;
                btnUpdateCart.Visible = false;
                trPromocode.Visible = false;
                Int32 items = 0;
                System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)Page.Master.FindControl("cartlink");
                objAnchor.InnerHtml = items > 1 ? "<span>" + items.ToString("D2") + "</span> <strong>Item(s)</strong>" : "<span>" + items.ToString("D2") + "</span> <strong>Item(s)</strong>";
                objAnchor.HRef = "javascript:void(0);";
                Response.Redirect("/Index.aspx", true);
            }
        }
        #region BindFeaturedProducts

        /// <summary>
        /// Method for Bind Product by Different Criteria BestSeller, Featured
        /// </summary>
        private void BindFeaturedProducts()
        {

            string CheckoutFeaturedProduct = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Configvalue FROM tb_Appconfig WHERE configname='CheckoutFeaturedProduct' and  isnull(deleted,0)=0 and Storeid=1")); //AppLogic.AppConfigs("CheckoutFeaturedProduct");
            if (CheckoutFeaturedProduct != "" && CheckoutFeaturedProduct != null)
            {
                string[] CheckoutFeaturedProductarray = CheckoutFeaturedProduct.Split(',');
                CheckoutFeaturedProduct = "";
                for (int i = 0; i < CheckoutFeaturedProductarray.Length; i++)
                {
                    if (CheckoutFeaturedProduct != "")
                    {
                        CheckoutFeaturedProduct += ",'" + CheckoutFeaturedProductarray[i] + "'";
                    }
                    else
                    {
                        CheckoutFeaturedProduct = "'" + CheckoutFeaturedProductarray[i] + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(CheckoutFeaturedProduct))
            {
                DataSet ds = CommonComponent.GetCommonDataSet("select top 5 * from tb_Product where SKU in (" + CheckoutFeaturedProduct + ") ");
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count != 0)
                {
                    rptFeaturedProduct.DataSource = ds.Tables[0];
                    rptFeaturedProduct.DataBind();
                    trfeatureproduct.Visible = true;
                }
                else
                {
                    trfeatureproduct.Visible = false;
                }
            }
            else
            {
                trfeatureproduct.Visible = false;
            }

        }


        /// <summary>
        /// Featured Product Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void rptFeaturedProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Probox = (HtmlGenericControl)e.Item.FindControl("Probox");
                Label lblTagName = (Label)e.Item.FindControl("lblTagImageName");
                Literal lblTagImage = (Literal)e.Item.FindControl("lblTagImage");
                HtmlInputHidden lblFreeEngraving = (HtmlInputHidden)e.Item.FindControl("lblFreeEngraving");
                Literal lblFreeEngravingImage = (Literal)e.Item.FindControl("lblFreeEngravingImage");

                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");


                if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                {
                    string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                    if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                    {
                        if (lblTagName != null && !string.IsNullOrEmpty(lblTagName.Text.ToString().Trim()))
                        {
                            //lblTagImage.Text = "<img title='" + lblTagName.Text.ToString().Trim() + "' src=\"/images/" + lblTagName.Text.ToString().Trim() + ".jpg\" alt=\"" + lblTagName.Text.ToString().Trim() + "\" class='" + lblTagName.Text.ToString().ToLower() + "' style='position: absolute; top: 110px; left: 98px;' width='62' height='17'  />";
                            lblTagImage.Text = "<img title='" + lblTagName.Text.ToString().Trim() + "' src=\"/images/" + lblTagName.Text.ToString().Trim() + ".png\" alt=\"" + lblTagName.Text.ToString().Trim() + "\" class='" + lblTagName.Text.ToString().ToLower() + "' />";
                        }
                    }
                }


                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                Label lblSalePrice = (Label)e.Item.FindControl("lblSalePrice");
                Literal ltrYourPrice = (Literal)e.Item.FindControl("ltrYourPrice");
                //Literal ltrRegularPrice = (Literal)e.Item.FindControl("ltrRegularPrice");
                HtmlInputHidden ltrLink = (HtmlInputHidden)e.Item.FindControl("ltrLink");
                HtmlInputHidden ltrlink1 = (HtmlInputHidden)e.Item.FindControl("ltrLink1");
                Literal ltrInventory = (Literal)e.Item.FindControl("ltrInventory");
                Literal ltrproductid = (Literal)e.Item.FindControl("ltrproductid");
                HtmlAnchor aFeaturedLink = (HtmlAnchor)e.Item.FindControl("aFeaturedLink");
                Decimal hdnprice = 0;
                Decimal SalePrice = 0;
                Decimal Price = 0;

                if (lblPrice != null)
                    Price = Convert.ToDecimal(lblPrice.Text);
                if (lblSalePrice != null)
                    SalePrice = Convert.ToDecimal(lblSalePrice.Text);

                //if (Price > decimal.Zero)
                //{
                //    ltrRegularPrice.Text += "Starting Price: <span>" + Price.ToString("C") + "</span>";// "<p>Regular Price: " + Price.ToString("C") + "</p>";
                //    hdnprice = Price;
                //}
                //else ltrRegularPrice.Text += "<span>&nbsp;</span>";
                if (SalePrice > decimal.Zero && Price > SalePrice)
                {
                    ltrYourPrice.Text += "Starting Price: <span>" + SalePrice.ToString("C") + "</span>";// "<p>Your Price: <strong>" + SalePrice.ToString("C") + "</strong></p>";
                    hdnprice = SalePrice;
                }
                else ltrYourPrice.Text += "Starting Price: <span>" + Price.ToString("C") + "</span>";


                if (lblFreeEngraving.Value == "True")
                {
                    lblFreeEngravingImage.Text = "<img title='Free Engraving' src=\"/images/FreeEngraving.jpg\" alt=\"Free Engraving\" style='position: absolute; top: 110px; left: 4px;'/>";
                }

                aFeaturedLink.Attributes.Remove("onclick");
                aFeaturedLink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
            }
        }

        /// <summary>
        /// Replace the Space which is comes in ProductName to "-"
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>return the ProductName with Replace the '"' and '\' which is comes in ProductName to "-" </returns>
        public String SetAttribute(String Name)
        {
            return Name.Replace('"', '-').Replace('\'', '-').ToString();
        }

        /// <summary>
        /// Add '...', if String length is more than 50 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 50 Length String </returns>
        public String SetName(String Name)
        {
            if (Name.Length > 50)
                Name = Name.Substring(0, 47) + "...";
            return Server.HtmlEncode(Name);
        }


        /// <summary>
        /// Get Icon Image for Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Icon Product Image Full Path</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (imagepath)))
            {
                return AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }

        #endregion
        protected void rdlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdlPaymentType != null && rdlPaymentType.Items.Count > 0)
            {
                Session["PaymentGateway"] = Convert.ToString(rdlPaymentType.SelectedItem.Value);
                Session["PaymentMethod"] = Convert.ToString(rdlPaymentType.SelectedItem.Value);

                OrderComponent objPayment = new OrderComponent();
                DataSet dsPayment = new DataSet();
                dsPayment = objPayment.GetPaymentGateway(Session["PaymentMethod"].ToString(), Convert.ToInt32(1));
                if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0 && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
                {
                    Session["PaymentGateway"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["PaymentService"].ToString());
                    Session["PaymentGatewayStatus"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString());
                }

                Session["PaymentMethod"] = Convert.ToString(rdlPaymentType.SelectedItem.Value);
            }
            if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
            {
                if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
                {
                    btnPlaceOrder.Attributes.Add("onclick", "return ValidationLoginuser();");
                    tblpayment.Visible = true;
                    btnPlaceOrder.ImageUrl = "/images/place-order.png";
                }
                else
                {
                    btnPlaceOrder.ImageUrl = "/images/PaypalCheckout.gif";
                    tblpayment.Visible = false;
                    btnPlaceOrder.Attributes.Add("onclick", "return ValidationLoginuserOtherPayment();");
                }
            }
            else
            {
                if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
                {
                    btnPlaceOrder.Attributes.Add("onclick", "return ValidationNotLogin();");
                    tblpayment.Visible = true;
                    btnPlaceOrder.ImageUrl = "/images/place-order.png";
                }
                else
                {
                    btnPlaceOrder.ImageUrl = "/images/PaypalCheckout.gif";
                    tblpayment.Visible = false;
                    btnPlaceOrder.Attributes.Add("onclick", "return ValidationNotLoginOtherPayment();");
                }

            }


        }
        #region PaypalCheckout
        /// <summary>
        /// Checkout Process Provided by Paypal
        /// </summary>
        private void PaypalCheckout(tb_Order objOrder)
        {
            //Code for Process checkout by Paypal
            PayPalComponent gw = new PayPalComponent();



            CommonComponent.ExecuteCommonData("Update tb_Order set Deleted=1 where OrderNumber='" + objOrder.OrderNumber + "' ");
            try
            {
                int totalItemCount = RptCartItems.Items.Count;
                for (int i = 0; i < totalItemCount; i++)
                {

                    HiddenField hdnProductId = (HiddenField)RptCartItems.Items[i].FindControl("hdnProductId");
                    Label lblPrice = (Label)RptCartItems.Items[i].FindControl("lblPrice");
                    Label lblDiscountprice = (Label)RptCartItems.Items[i].FindControl("lblDiscountprice");
                    TextBox txtQty = (TextBox)RptCartItems.Items[i].FindControl("txtQty");

                    Literal ltrSubTotal = (Literal)RptCartItems.Items[i].FindControl("ltrSubTotal");

                    Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + hdnProductId.Value.ToString() + " and ItemType='Swatch'"));
                    if (Isorderswatch == 1)
                    {
                        try
                        {
                            Decimal strpp = Convert.ToDecimal(lblPrice.Text.ToString().Replace("$", "").Trim());
                            Decimal strdiscount = Convert.ToDecimal(lblDiscountprice.Text.ToString().Replace("$", "").Trim());
                            Decimal decSubTotal = Convert.ToDecimal(ltrSubTotal.Text.ToString().Replace("$", "").Trim());
                            if (strpp > Decimal.Zero)
                            {
                                if (decSubTotal == strpp)
                                {
                                    strpp = strpp / Convert.ToDecimal(txtQty.Text.ToString());
                                }
                            }
                            if (strdiscount > Decimal.Zero)
                            {
                                if (decSubTotal == strdiscount)
                                {
                                    strdiscount = strdiscount / Convert.ToDecimal(txtQty.Text.ToString());
                                }
                            }
                            CommonComponent.ExecuteCommonData("UPDATE tb_OrderedShoppingCartItems SET Price='" + string.Format("{0:0.00}", strpp) + "',DiscountPrice='" + string.Format("{0:0.00}", strdiscount) + "' WHERE RefProductid=" + hdnProductId.Value.ToString() + " AND OrderedShoppingCartID in (SELECT ShoppingCardID FROM tb_order WHERE OrderNumber=" + objOrder.OrderNumber + ")");
                        }
                        catch { }
                    }
                }
            }
            catch { }


            string OrderTotal = "0";

            if (ViewState["OrderTotal"] != null)
            {
                OrderTotal = Convert.ToString(ViewState["OrderTotal"]);
            }

            String SessionKeyName = "CustIDKey" + Convert.ToString(HttpContext.Current.Request.UserHostAddress).Replace('.', '-');


            string Query = String.Format("delete from tb_CustomerStatus where RecUniqueID like '%" + SessionKeyName + "%';Insert into tb_CustomerStatus(RecUniqueID,RecCustID,OrderNumber) values ('" + SessionKeyName + "','" + objOrder.CustomerID + "', '" + objOrder.OrderNumber + "')");
            CommonComponent.ExecuteCommonData(Query);


            gw.ECOrderTotal.Value = OrderTotal;
            gw.ECShippingAddress.Street1 = objOrder.ShippingAddress1;// .Address1;
            gw.ECShippingAddress.Street2 = objOrder.ShippingAddress2;// +(objOrder.ShippingSuite != "" ? " Ste " + objOrder.ShippingSuite : "");
            gw.ECShippingAddress.CityName = objOrder.ShippingCity;


            string StateAbbr = Convert.ToString(CommonComponent.GetScalarCommonData("Select Abbreviation from tb_state where Name='" + objOrder.ShippingState + "'"));

            gw.ECShippingAddress.StateOrProvince = StateAbbr;  //    shippingAddress.GetStateTwoLetterISOCodeByID((shippingAddress.GetIDByStateName(shippingAddress.State)));

            try
            {
                if (gw.ECShippingAddress.StateOrProvince == "")
                {
                    gw.ECShippingAddress.StateOrProvince = objOrder.ShippingState;
                }
            }
            catch { }

            gw.ECShippingAddress.PostalCode = objOrder.ShippingZip;// shippingAddress.ZipCode;
            gw.SetECShippingCountry(objOrder.ShippingCountry);

            string strTranscationMode = "auth";


            if (Session["PaymentMethod"] != null)
            {
                object TranscationMode = CommonComponent.GetScalarCommonData("select InitialPaymentStatus from tb_PaymentServices  where PaymentService like '%" + Convert.ToString(Session["PaymentMethod"]) + "%'");

                if (TranscationMode != null)
                {
                    strTranscationMode = Convert.ToString(TranscationMode);
                }
            }


            if (Request.QueryString["stype"] != null)
            {

                if (Convert.ToString(Request.QueryString["stype"]) != "")
                {
                    String sURL = gw.StartEC(strTranscationMode, "/CustomerquoteCheckout.aspx?resetlinkback=1&stype=" + Convert.ToString(Request.QueryString["stype"]) + "");
                    Response.Redirect(sURL);
                }
                else
                {
                    String sURL = gw.StartEC(strTranscationMode, "/CustomerquoteCheckout.aspx?resetlinkback=1");
                    Response.Redirect(sURL);
                }
            }
            else
            {
                String sURL = gw.StartEC(strTranscationMode, "/CustomerquoteCheckout.aspx?resetlinkback=1");
                Response.Redirect(sURL);
            }
        }
        #endregion
        protected void txtCSC_PreRender(object sender, EventArgs e)
        {
            txtCSC.Attributes["value"] = txtCSC.Text.ToString();
        }

        protected void btnimgCreateNewAccountGuest_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/CustomerquoteCheckout.aspx?stype=1");

        }

        protected void btnimgCreateNewAccount_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/CustomerquoteCheckout.aspx?stype=2");
        }

    }
}