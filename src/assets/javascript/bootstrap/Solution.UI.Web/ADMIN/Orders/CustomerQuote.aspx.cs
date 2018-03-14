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
using System.Text;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class CustomerQuote : BasePage
    {
        Decimal SwatchQty = Decimal.Zero;
        /// <summary>
        /// Page Load Event
        /// </summary>  
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Isano"] = null;
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnUpdateProduct.ImageUrl = "/App_Themes/" + Page.Theme + "/images/update.png";
                aRelated.ImageUrl = "/App_Themes/" + Page.Theme + "/images/add-products.png";
                BtnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                popupContactClose.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
                btnPreview.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/preview.png";
                imgHeader.Src = "/App_Themes/ " + Page.Theme.ToString() + "/icon/add-product.png";
                btnGenerate.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/apply.png";

                if (Session["AdminID"] != null && Session["AdminID"].ToString() != "")
                {
                    Boolean IsSalesManager = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select ISNULL(isSalesManager,0) as isSalesManager from tb_Admin where AdminID = " + Session["AdminID"].ToString() + " and ISNULL(Deleted,0)=0 and ISNULL(Active,0)=1"));
                }
                GetStoreList();
                FillCountry();
                TxtEmail.Focus();

                if (Request.QueryString["CustID"] != null)
                {
                    if (Request.QueryString["saleorder"] == null)
                    {
                        RemoveCart(Convert.ToInt32(Request.QueryString["CustID"].ToString()));
                        HdnCustID.Value = Request.QueryString["CustID"].ToString();
                        GetCustomerDetails(Convert.ToInt32(Request.QueryString["CustID"].ToString()));
                        GVShoppingCartItems.DataSource = null;
                        GVShoppingCartItems.DataBind();
                        trPreview.Attributes.Add("style", "display:none");
                        tablepreview.Attributes.Add("style", "display:none");
                        trUpdateProduct.Visible = false;
                    }
                    else
                    {
                        HdnCustID.Value = Request.QueryString["CustID"].ToString();
                        Session["CustCouponCode"] = null;
                        Session["CustCouponCodeDiscount"] = null;
                        Session["CustCouponvalid"] = null;
                        Session["Storecreaditamount"] = null;

                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise")
                        {
                            if (Request.QueryString["ID"] != null)
                            {
                                int CustQuoteID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select CustomerQuoteID from tb_CustomerQuote Where QuoteNumber ='" + Request.QueryString["ID"].ToString() + "'"));
                                String CouponCode = Convert.ToString(CommonComponent.GetScalarCommonData("Select CouponCode from tb_CustomerQuote Where QuoteNumber ='" + Request.QueryString["ID"].ToString() + "'"));
                                decimal DiscPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 Isnull(DiscountPrice,0) as DiscountPrice from tb_CustomerQuoteitems Where CustomerId=" + HdnCustID.Value.ToString() + " and CustomerQuoteID=" + CustQuoteID + ""));
                                if (DiscPrice > 0)
                                {
                                    DataSet dsTemp = new DataSet();
                                    dsTemp = CommonComponent.GetCommonDataSet("Select ISNULL(DiscountPercent,0) as DiscountPercent,ISNULL(CouponCode,'') as CouponCode,FromDate,ToDate from tb_Customer Where CustomerID=" + HdnCustID.Value.ToString() + " and CouponCode='" + CouponCode + "'");
                                    if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                                    {
                                        string StrFromdate = Convert.ToString(dsTemp.Tables[0].Rows[0]["FromDate"].ToString());
                                        string StrTodate = Convert.ToString(dsTemp.Tables[0].Rows[0]["ToDate"].ToString());
                                        if (!string.IsNullOrEmpty(StrFromdate.Trim()) && !string.IsNullOrEmpty(StrTodate.Trim()))
                                        {
                                            if (ChkDiscountDateRange(StrFromdate, StrTodate) == true)
                                            {
                                                if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["CouponCode"].ToString()))
                                                {
                                                    txtCouponCode.Text = dsTemp.Tables[0].Rows[0]["CouponCode"].ToString();
                                                    Session["CustCouponCode"] = dsTemp.Tables[0].Rows[0]["CouponCode"].ToString();
                                                }
                                                if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) && Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) > 0)
                                                {
                                                    Session["CustCouponCodeDiscount"] = dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["CouponCode"].ToString()))
                                            {
                                                txtCouponCode.Text = dsTemp.Tables[0].Rows[0]["CouponCode"].ToString();
                                                Session["CustCouponCode"] = dsTemp.Tables[0].Rows[0]["CouponCode"].ToString();
                                            }
                                            if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) && Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) > 0)
                                            {
                                                Session["CustCouponCodeDiscount"] = dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dsTemp = CommonComponent.GetCommonDataSet("Select ISNULL(DiscountPercent,0) as DiscountPercent,ISNULL(CouponCode,'') as CouponCode,ExpirationDate from tb_Coupons Where  CouponCode='" + CouponCode + "'");
                                        // string StrFromdate = Convert.ToString(dsTemp.Tables[0].Rows[0]["FromDate"].ToString());
                                        string StrTodate = Convert.ToString(dsTemp.Tables[0].Rows[0]["ExpirationDate"].ToString());
                                        if (!string.IsNullOrEmpty(StrTodate.Trim()))
                                        {
                                            if (ChkDiscountDateRangeCouponCode(StrTodate) == true)
                                            {
                                                if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["CouponCode"].ToString()))
                                                {
                                                    txtCouponCode.Text = dsTemp.Tables[0].Rows[0]["CouponCode"].ToString();
                                                    Session["CustCouponCode"] = dsTemp.Tables[0].Rows[0]["CouponCode"].ToString();
                                                }
                                                if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) && Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) > 0)
                                                {
                                                    Session["CustCouponCodeDiscount"] = dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["CouponCode"].ToString()))
                                            {
                                                txtCouponCode.Text = dsTemp.Tables[0].Rows[0]["CouponCode"].ToString();
                                                Session["CustCouponCode"] = dsTemp.Tables[0].Rows[0]["CouponCode"].ToString();
                                            }
                                            if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) && Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) > 0)
                                            {
                                                Session["CustCouponCodeDiscount"] = dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteitems set DiscountPrice=0 Where CustomerQuoteID in (Select CustomerQuoteID from tb_CustomerQuote Where CustomerID=" + HdnCustID.Value.ToString() + ")");
                        }
                        GetCustomerDetails(Convert.ToInt32(Request.QueryString["CustID"].ToString()));
                        GetShippIngMethodByBillAddress();

                        BindCartInGrid();
                    }
                }
                else
                {
                    HdnCustID.Value = "0";
                }
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise")
                {
                    int CustQuoteID = 0;
                    ddlStore.Enabled = false;
                    lblHeader.Text = "Revise Customer Quote";
                    imgHeader.Alt = "Revise Customer Quote";
                    imgHeader.Style.Add("title", "Revise Customer Quote");

                    Boolean IsHtmlFormat = false;
                    CustQuoteID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select CustomerQuoteID from tb_CustomerQuote Where QuoteNumber ='" + Request.QueryString["ID"].ToString() + "'"));
                    DataSet dsCustQuote = CommonComponent.GetCommonDataSet("Select * from tb_CustomerQuote Where CustomerQuoteID=" + CustQuoteID + "");
                    if (dsCustQuote != null && dsCustQuote.Tables.Count > 0 && dsCustQuote.Tables[0].Rows.Count > 0)
                    {
                        TxtNotes.Text = Convert.ToString(dsCustQuote.Tables[0].Rows[0]["GeneralNote"]).Trim();
                        if (!string.IsNullOrEmpty(dsCustQuote.Tables[0].Rows[0]["IsHTML"].ToString().Trim()))
                        {
                            IsHtmlFormat = Convert.ToBoolean(dsCustQuote.Tables[0].Rows[0]["IsHTML"]);
                        }
                        if (IsHtmlFormat == true)
                        {
                            rbMailList.Items[0].Selected = true;
                            trPreview.Attributes.Add("style", "display:''");
                            tablepreview.Attributes.Add("style", "display:''");
                            btnPreview_Click(null, null);
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "htmlfor", "ShowHideButtonresetExpand('ImgProd', 'tdSelectedProd', 'divSelectedProd');", true);
                        }
                        else
                        {
                            rbMailList.Items[0].Selected = true;
                            trPreview.Attributes.Add("style", "display:none");
                            tablepreview.Attributes.Add("style", "display:none");
                            trEmail.Attributes.Add("style", "display:none");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "htmlfor", "ShowHideButtonresetExpand('ImgProd', 'tdSelectedProd', 'divSelectedProd');", true);
                        }
                    }
                }

                if (Request.QueryString["searchlinksku"] != null)
                {
                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "searchlinksku", "Showsearcklinksku();", true);
                    hdnsearchlinksku.Value = Request.QueryString["searchlinksku"].ToString();
                    aRelated_Click(null, null);

                }

            }
            else
            {
                //BindCartInGrid();
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise" && hdnIsReviseQuote.Value == "1")
                {
                    if (HdnCustID.Value != "" || HdnCustID.Value != "0")
                    {
                        if (GVShoppingCartItems.Rows.Count > 0)
                        {
                            trEmail.Attributes.Add("style", "display: ''");
                            SendPreviewQuotetoCustomer();
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgreset", "ShowHideButtonreset('ImgProd','tdSelectedProd','divSelectedProd');", true);
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgScroll", "$('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_tablepreview').offset().top }, 'slow');", true);
                        }
                        else
                        {
                            trEmail.Attributes.Add("style", "display: none;");
                            lblMsg.Text = "Please add atleast one product.";
                            return;
                        }
                    }
                }
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
            this.Form.DefaultButton = imgdefaulttemp.UniqueID.ToString();
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
                ddlB_Country.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Country", "0"));
                //ddlBillcountry.Items.Insert(dscountry.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));

                ddlS_Country.DataSource = dscountry.Tables[0];
                ddlS_Country.DataTextField = "Name";
                ddlS_Country.DataValueField = "CountryID";
                ddlS_Country.DataBind();
                ddlS_Country.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Country", "0"));
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
        /// Get Store List and bind into Store Drop down
        /// </summary>
        private void GetStoreList()
        {
            ddlStore.Items.Clear();
            DataSet dsStore = new DataSet();
            //dsStore = StoreComponent.GetStoreList();

            //Only Selected Stores will Display not all ----------------------

            dsStore = CommonComponent.GetCommonDataSet("Select * from tb_store where ISNULL(deleted,0)=0");
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = dsStore;
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataBind();
            }
            else
            {
                ddlStore.DataSource = null;
                ddlStore.DataBind();
            }
            //ddlStore.Items.Insert(0, new ListItem("All", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
            else
            {
                ddlStore.SelectedIndex = 0;
            }
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


            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Email"].ToString()))
                {

                    TxtEmail.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());
                }

                txtB_FName.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString());
                txtB_LName.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString());
                txtB_Company.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Company"].ToString());
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
                    //TxtNameOnCard.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["NameOnCard"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardTypeID"].ToString()))
                {
                    //ddlCardType.ClearSelection();
                    //ddlCardType.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardTypeID"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardVerificationCode"].ToString()))
                {
                    //TxtCardVerificationCode.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardVerificationCode"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString()))
                {
                    //string CardNumber = SecurityComponent.Decrypt(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString());
                    //if (CardNumber.Length > 4)
                    //{
                    //    for (int i = 0; i < CardNumber.Length - 4; i++)
                    //    {
                    //        TxtCardNumber.Text += "*";
                    //    }
                    //    TxtCardNumber.Text += CardNumber.ToString().Substring(CardNumber.Length - 4);
                    //    Session["CardNumber"] = CardNumber.ToString();
                    //}
                    //else
                    //{
                    //    TxtCardNumber.Text = "";
                    //}
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardExpirationMonth"].ToString()))
                {
                    //ddlMonth.ClearSelection();
                    //ddlMonth.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardExpirationMonth"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardExpirationYear"].ToString()))
                {
                    //ddlYear.ClearSelection();
                    //ddlYear.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardExpirationYear"].ToString());
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

        /// <summary>
        /// Binds the Cart in Grid
        /// </summary>
        private void BindCartInGrid()
        {
            Int32 CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());
            Int32 CustID = CustomerID;

            DataSet DsCItems = new DataSet();
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise" && hdnIsReviseQuote.Value == "0")
            {
                Int32 CustQuoteID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select CustomerQuoteID from tb_CustomerQuote Where QuoteNumber ='" + Request.QueryString["ID"].ToString() + "'"));
                string strSql = "select ISNULL(tb_customer.DiscountPercent,0) as DiscountPercent,ISNULL(DiscountPrice,0) as DiscountPrice,cqi.CustomerQuoteItemID,cq.customerid,p.sku,cqi.name,cqi.price,cqi.quantity,cqi.VariantNames,cqi.VariantValues,cqi.options,cqi.productid,IsNull(cqi.notes,'') as Notes,isnull(cqi.RelatedproductID,0) as RelatedproductID,cqi.IsProductType as ProductType,cqi.IsProductType as IsProductType from tb_CustomerQuoteItems cqi  " +
                " inner join tb_CustomerQuote cq  on cq.CustomerQuoteID=cqi.CustomerQuoteID " +
                " inner join tb_Product p  on p.ProductID=cqi.ProductID inner Join tb_customer on tb_customer.CustomerID=cq.CustomerID" +
                " where cq.CustomerQuoteID= " + CustQuoteID;
                DsCItems = CommonComponent.GetCommonDataSet(strSql);
            }
            else
            {
                DsCItems = CommonComponent.GetCommonDataSet("Select ISNULL(DiscountPrice,0) as DiscountPrice,ISNULL(tb_customer.DiscountPercent,0) as DiscountPercent,CustomerQuoteItemID,CustomerQuoteID,Name,SKU,tb_CustomerQuoteitems.CustomerId,Notes,ISNULL(Quantity,0) as Quantity,ISNULL(Price,0)as Price,ProductID,VariantNames,VariantValues ,(ISNULL(Quantity,0) * ISNULL(Price,0)) as IndiSubTotal,isnull(RelatedproductID,0) as RelatedproductID,IsProductType as ProductType,IsProductType as IsProductType  from tb_CustomerQuoteitems inner Join tb_customer on tb_customer.CustomerID=tb_CustomerQuoteitems.CustomerID Where tb_CustomerQuoteitems.CustomerId=" + CustID + " and CustomerQuoteID=0");
            }
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
                    SwatchCnt = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect Count(ProductID) as Isfreefabricswatch from tb_product where ProductID in(" + strProduct + ")   and StoreID = 1 and ItemType='Swatch'"));
                    if (DsCItems.Tables[0].Rows.Count == SwatchCnt)
                    {
                        ViewState["AllProductsSwatch"] = "1";
                    }
                }

                DataView dv = new DataView();
                dv = DsCItems.Tables[0].DefaultView;
                dv.Sort = " CustomerQuoteItemID asc";
                dt = dv.ToTable();
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                GVShoppingCartItems.DataSource = dt;
                GVShoppingCartItems.DataBind();
            }


            if (GVShoppingCartItems.Rows.Count > 0)
            {
                for (int i = 0; i < GVShoppingCartItems.Rows.Count; i++)
                {
                    Label lblCustomerQuoteItemID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblCustomerQuoteItemID");
                    Label lblProductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductID");
                    Label lblVariantNames = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantNames");
                    Label lblVariantValues = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantValues");
                    Label lblOrginalDiscountPrice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblOrginalDiscountPrice");
                    TextBox txtQuantity = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtQuantity");
                    TextBox txtPrice = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtPrice");
                    TextBox txtProductNotes = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtProductNotes");
                    Label lblProductType = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductType");

                    int Quantity = 0;
                    decimal Price = 0;
                    int.TryParse(txtQuantity.Text.ToString(), out Quantity);
                    decimal.TryParse(txtPrice.Text.ToString(), out Price);

                    decimal DiscountPrice = 0;
                    if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
                    {
                        decimal.TryParse(Session["CustCouponCodeDiscount"].ToString().Trim(), out DiscountPrice);
                    }
                    if (!string.IsNullOrEmpty(lblCustomerQuoteItemID.Text.Trim()))
                    {
                        string strnotes = "";
                        if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
                        {
                            if (txtProductNotes.Text.ToString().ToLower().Contains("coupon code"))
                            {
                                //string[] strNotes = txtNotes.Text.ToString().Replace("'", "''").Split("\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                //strnotes = strNotes[0];
                                strnotes = txtProductNotes.Text.ToString();
                                string strnotes1 = txtProductNotes.Text.ToString().Substring(txtProductNotes.Text.ToString().IndexOf("\""), txtProductNotes.Text.ToString().LastIndexOf("\"") - txtProductNotes.Text.ToString().IndexOf("\"") + 1);
                                strnotes = strnotes.Replace(strnotes1, " \"" + "Coupon Code # " + Session["CustCouponCode"].ToString() + "\"");
                                txtProductNotes.Text = strnotes.Trim();


                            }
                            else
                            {
                                txtProductNotes.Text = txtProductNotes.Text.Trim() + " \"" + "Coupon Code # " + Session["CustCouponCode"].ToString() + "\"";
                            }
                        }
                        else
                        {
                            if (txtProductNotes.Text.ToString().ToLower().Contains("coupon code"))
                            {
                                //string[] strNotes = txtNotes.Text.ToString().Replace("'", "''").Split("\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                //strnotes = strNotes[0];
                                strnotes = txtProductNotes.Text.ToString();
                                string strnotes1 = txtProductNotes.Text.ToString().Substring(txtProductNotes.Text.ToString().IndexOf("\""), txtProductNotes.Text.ToString().LastIndexOf("\"") - txtProductNotes.Text.ToString().IndexOf("\"") + 1);
                                strnotes = strnotes.Replace(strnotes1, "");
                                txtProductNotes.Text = strnotes.Trim();

                            }

                        }
                        if (lblProductType.Text.ToString() == "0")
                        {
                            CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteitems set Quantity=" + Quantity + ",Price=0,Notes='" + txtProductNotes.Text.Trim().ToString().Replace("'", "''") + "',DiscountPrice=0 Where CustomerQuoteItemID=" + lblCustomerQuoteItemID.Text.ToString() + " and ProductID=" + lblProductID.Text.Trim() + " and VariantNames='" + lblVariantNames.Text.ToString() + "' and VariantValues='" + lblVariantValues.Text.ToString() + "'");
                        }
                        else
                        {

                            CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteitems set Quantity=" + Quantity + ",Price=" + Price + ",Notes='" + txtProductNotes.Text.Trim().ToString().Replace("'", "''") + "',DiscountPrice=" + DiscountPrice + " Where CustomerQuoteItemID=" + lblCustomerQuoteItemID.Text.ToString() + " and ProductID=" + lblProductID.Text.Trim() + " and VariantNames='" + lblVariantNames.Text.ToString() + "' and VariantValues='" + lblVariantValues.Text.ToString() + "'");
                        }
                    }
                }
            }

            String strswatchQtyy = "";
            if (AppLogic.AppConfigs("SwatchMaxlength") != null && AppLogic.AppConfigs("SwatchMaxlength").ToString() != "")
            {
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise" && hdnIsReviseQuote.Value == "0")
                {
                    Int32 CustQuoteID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select CustomerQuoteID from tb_CustomerQuote Where QuoteNumber ='" + Request.QueryString["ID"].ToString() + "'"));
                    // strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(SUM(Quantity),0) as Quantity   from tb_CustomerQuoteitems inner Join tb_customer on tb_customer.CustomerID=tb_CustomerQuoteitems.CustomerID  Where tb_CustomerQuoteitems.CustomerId=" + CustomerID + " and CustomerQuoteID=" + CustQuoteID + " and  IsProductType=0"));
                }
                else
                {
                    // strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(SUM(Quantity),0) as Quantity   from tb_CustomerQuoteitems inner Join tb_customer on tb_customer.CustomerID=tb_CustomerQuoteitems.CustomerID  Where tb_CustomerQuoteitems.CustomerId=" + CustomerID + " and CustomerQuoteID=0 and  IsProductType=0"));
                }

                //if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
                //{
                SwatchQty = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                //}
            }

            DsCItems = new DataSet();
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise" && hdnIsReviseQuote.Value == "0")
            {
                Int32 CustQuoteID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select CustomerQuoteID from tb_CustomerQuote Where QuoteNumber ='" + Request.QueryString["ID"].ToString() + "'"));
                string strSql = "select ISNULL(tb_customer.DiscountPercent,0) as DiscountPercent,ISNULL(DiscountPrice,0) as DiscountPrice,cqi.CustomerQuoteItemID,cq.customerid,p.sku,cqi.name,cqi.price,cqi.quantity,cqi.VariantNames,cqi.VariantValues,cqi.options,cqi.productid,IsNull(cqi.notes,'') as Notes,isnull(cqi.RelatedproductID,0) as RelatedproductID,cqi.IsProductType as ProductType,cqi.IsProductType as IsProductType from tb_CustomerQuoteItems cqi  " +
                " inner join tb_CustomerQuote cq  on cq.CustomerQuoteID=cqi.CustomerQuoteID " +
                " inner join tb_Product p  on p.ProductID=cqi.ProductID inner Join tb_customer on tb_customer.CustomerID=cq.CustomerID" +
                " where cq.CustomerQuoteID= " + CustQuoteID;
                DsCItems = CommonComponent.GetCommonDataSet(strSql);
            }
            else
            {
                DsCItems = CommonComponent.GetCommonDataSet("Select ISNULL(DiscountPrice,0) as DiscountPrice,ISNULL(tb_customer.DiscountPercent,0) as DiscountPercent,CustomerQuoteItemID,CustomerQuoteID,Name,SKU,tb_CustomerQuoteitems.CustomerId,Notes,ISNULL(Quantity,0) as Quantity,ISNULL(Price,0)as Price,ProductID,VariantNames,VariantValues ,(ISNULL(Quantity,0) * ISNULL(Price,0)) as IndiSubTotal,isnull(RelatedproductID,0) as RelatedproductID,IsProductType as ProductType,IsProductType as IsProductType  from tb_CustomerQuoteitems inner Join tb_customer on tb_customer.CustomerID=tb_CustomerQuoteitems.CustomerID Where tb_CustomerQuoteitems.CustomerId=" + CustID + " and CustomerQuoteID=0");
            }
            dt = new DataTable();
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
                    SwatchCnt = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect Count(ProductID) as Isfreefabricswatch from tb_product where ProductID in(" + strProduct + ")   and StoreID = 1 and ItemType='Swatch'"));
                    if (DsCItems.Tables[0].Rows.Count == SwatchCnt)
                    {
                        ViewState["AllProductsSwatch"] = "1";
                    }
                }

                DataView dv = new DataView();
                dv = DsCItems.Tables[0].DefaultView;
                dv.Sort = " CustomerQuoteItemID asc";
                dt = dv.ToTable();
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                GVShoppingCartItems.DataSource = dt;
                GVShoppingCartItems.DataBind();
                trPreview.Attributes.Add("style", "display:''");
                tablepreview.Attributes.Add("style", "display:''");
                trUpdateProduct.Visible = true;

                if (AppLogic.AppConfigs("SwatchMaxlength") != null && AppLogic.AppConfigs("SwatchMaxlength").ToString() != "")
                {
                    if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise" && hdnIsReviseQuote.Value == "0")
                    {
                        Int32 CustQuoteID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select CustomerQuoteID from tb_CustomerQuote Where QuoteNumber ='" + Request.QueryString["ID"].ToString() + "'"));
                        //strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(SUM(Quantity),0) as Quantity   from tb_CustomerQuoteitems inner Join tb_customer on tb_customer.CustomerID=tb_CustomerQuoteitems.CustomerID  Where tb_CustomerQuoteitems.CustomerId=" + CustomerID + " and CustomerQuoteID=" + CustQuoteID + " and  IsProductType=0"));
                    }
                    else
                    {
                        //strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(SUM(Quantity),0) as Quantity   from tb_CustomerQuoteitems inner Join tb_customer on tb_customer.CustomerID=tb_CustomerQuoteitems.CustomerID  Where tb_CustomerQuoteitems.CustomerId=" + CustomerID + " and CustomerQuoteID=0 and  IsProductType=0"));
                    }

                    //if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
                    //{
                    SwatchQty = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                    //}
                }
                for (int i = 0; i < GVShoppingCartItems.Rows.Count; i++)
                {
                    Label lblCustomerQuoteItemID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblCustomerQuoteItemID");
                    Label lblProductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductID");
                    Label lblVariantNames = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantNames");
                    Label lblVariantValues = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantValues");
                    Label lblOrginalDiscountPrice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblOrginalDiscountPrice");
                    TextBox txtQuantity = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtQuantity");
                    TextBox txtPrice = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtPrice");
                    TextBox txtProductNotes = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtProductNotes");
                    Label lblProductType = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductType");

                    int Quantity = 0;
                    decimal Price = 0;
                    int.TryParse(txtQuantity.Text.ToString(), out Quantity);
                    decimal.TryParse(txtPrice.Text.ToString(), out Price);

                    decimal DiscountPrice = 0;
                    if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
                    {
                        decimal.TryParse(lblOrginalDiscountPrice.Text.ToString().Trim(), out DiscountPrice);
                    }
                    if (!string.IsNullOrEmpty(lblCustomerQuoteItemID.Text.Trim()))
                    {

                        CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteitems set DiscountPrice=" + DiscountPrice + " Where CustomerQuoteItemID=" + lblCustomerQuoteItemID.Text.ToString() + " and ProductID=" + lblProductID.Text.Trim() + " and VariantNames='" + lblVariantNames.Text.ToString() + "' and VariantValues='" + lblVariantValues.Text.ToString() + "'");
                    }

                }


                //try
                //{
                //    decimal subtotal = 0;
                //    if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
                //    {
                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            decimal DiscountPrice = 0, OrgiPrice = 0;
                //            decimal.TryParse(dr["DiscountPercent"].ToString().Trim(), out DiscountPrice);
                //            decimal.TryParse(dr["SalePrice"].ToString().Trim(), out OrgiPrice);
                //            if (DiscountPrice > 0)
                //            {
                //                decimal DicPrice = 0, TempDis = 0;
                //                if (DiscountPrice > 0 && DiscountPrice <= 99)
                //                {
                //                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                //                    DicPrice = OrgiPrice - TempDis;
                //                    if (dr != null)
                //                    {
                //                        subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                //                    }
                //                }
                //                else if (DiscountPrice >= 100)
                //                {
                //                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                //                    DicPrice = TempDis;
                //                    if (dr != null)
                //                    {
                //                        subtotal = subtotal + (Convert.ToDecimal(DicPrice.ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                //                    }
                //                }
                //                else
                //                {
                //                    if (dr != null)
                //                    {
                //                        subtotal = subtotal + (Convert.ToDecimal(dr["SalePrice"].ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (dr != null)
                //                {
                //                    subtotal = subtotal + (Convert.ToDecimal(dr["SalePrice"].ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            if (dr != null)
                //            {
                //                subtotal = subtotal + (Convert.ToDecimal(dr["SalePrice"].ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
                //            }
                //        }
                //    }
                //}
                //catch { }


                try
                {
                    decimal subtotal = 0;
                    if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
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
                                        //StrSaleprice = pp.ToString();
                                        if (Convert.ToDecimal(pp) >= SwatchQty)
                                        {
                                            StrQuantity = dr["Quantity"].ToString();
                                            pp = (pp - SwatchQty) / Convert.ToDecimal(dr["Quantity"].ToString());
                                            StrSaleprice = pp.ToString();
                                            SwatchQty = Decimal.Zero;
                                        }
                                        else
                                        {
                                            if (SwatchQty > Decimal.Zero)
                                            {
                                                StrSaleprice = "0.00";
                                                StrQuantity = dr["Quantity"].ToString();
                                                SwatchQty = SwatchQty - Convert.ToDecimal(pp);
                                            }
                                            else
                                            {
                                                pp = pp / Convert.ToDecimal(dr["Quantity"].ToString());
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
                                        pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + ""));
                                        StrQuantity = dr["Quantity"].ToString();
                                        StrSaleprice = pp.ToString().Trim();
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
                                    String ParentCategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select cast(ParentCategoryID as nvarchar(500))+',' from tb_CategoryMapping WHERE CategoryID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ") FOR XML PATH('')"));
                                    if (ParentCategoryId.Length > 0)
                                    {
                                        ParentCategoryId = ParentCategoryId.Substring(0, ParentCategoryId.Length - 1);
                                    }
                                    else
                                    {
                                        ParentCategoryId = "0";
                                    }

                                    ProductDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT ISNULL(md.Discount,0) AS ProductDiscount "
                                    + " FROM dbo.tb_MembershipDiscount md Left OUTER JOIN dbo.tb_Product Prod ON Prod.ProductID =md.DiscountObjectID " +
                                    " WHERE md.CustID='" + HdnCustID.Value.ToString() + "' AND md.DiscountType='product' AND md.StoreID= " + ddlStore.SelectedValue.ToString() + " AND md.DiscountObjectID=" + ProductId + ""));
                                    if (ProductDiscount <= 0)
                                    {
                                        CategoryDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT top 1 md.Discount AS CategoryDiscount "
                                            + " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                                            + "WHERE md.DiscountType='category' AND  md.CustID= " + HdnCustID.Value.ToString() + "  AND md.storeid=" + ddlStore.SelectedValue.ToString() + " AND (md.DiscountObjectID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ") or md.DiscountObjectID in (" + ParentCategoryId + "))"));
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
                                String strProduct = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidforProduct FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CustCouponCode"].ToString() + "'"));
                                if (!string.IsNullOrEmpty(strCategory))
                                {
                                    DataSet dspp = new DataSet();
                                    dspp = CommonComponent.GetCommonDataSet("SELECT ProductId FROM tb_ProductCategory WHERE ProductId=" + dr["Productid"].ToString() + " and categoryId in (" + strCategory.Replace(" ", "") + ")");
                                    if (dspp != null && dspp.Tables.Count > 0 && dspp.Tables[0].Rows.Count > 0)
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
                                                if (SwatchQty > Decimal.Zero)
                                                {
                                                    Qty = Convert.ToInt32(dr["Quantity"].ToString());
                                                    SwatchQty = SwatchQty - Convert.ToDecimal(pp);
                                                    pp = Decimal.Zero;
                                                }
                                                else
                                                {
                                                    Qty = Convert.ToInt32(dr["Quantity"].ToString());
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
                                            pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT  case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["Productid"].ToString() + ""));
                                            subtotal = subtotal + (Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(dr["Quantity"].ToString()));
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
                    decimal discount = 0;
                    if (Session["QtyDiscount"] != null)
                    {
                        decimal Qty = 0;
                        decimal.TryParse(Session["QtyDiscount"].ToString(), out Qty);
                        discount = Math.Round(Qty, 2);
                    }
                    else
                    {
                        //TxtDiscount.Text = "0.00";
                        discount = 0;

                    }

                    decimal Storecreaditamount = 0;
                    if (Session["Storecreaditamount"] != null)
                    {
                        decimal.TryParse(Session["Storecreaditamount"].ToString(), out Storecreaditamount);

                    }

                    #endregion
                    //   decimal tax = Convert.ToDecimal(TxtTax.Text.Trim());
                    // decimal ship = Convert.ToDecimal(TxtShippingCost.Text.Trim());

                    // decimal discount = Convert.ToDecimal(TxtDiscount.Text.Trim());



                    decimal FinalSubTotal = (subtotal) - discount;
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
                        //txtBoxcaptureamount.Text = string.Format("{0:0.00}", totalzero);
                    }
                    else
                    {
                        // txtBoxcaptureamount.Text = "0.00";
                    }


                }
                catch { }




            }
            else
            {
                GVShoppingCartItems.DataSource = null;
                GVShoppingCartItems.DataBind();
                trPreview.Attributes.Add("style", "display:none");
                tablepreview.Attributes.Add("style", "display:none");
                trUpdateProduct.Visible = false;
                txtBody.Text = "";
                Session["QtyDiscount"] = null;
                Session["QtyDiscount1"] = null;
            }
        }

        /// <summary>
        /// Sends the preview quote to customer.
        /// </summary>
        private void SendPreviewQuotetoCustomer()
        {
            Boolean discountApply = false;
            string EmailID = (string)(CommonComponent.GetScalarCommonData("Select EmailID from tb_Admin where AdminID = " + Session["AdminID"].ToString() + " "));
            String EMailFrom = EmailID;
            String EMailTo = TxtEmail.Text.Trim();
            String EMailSubject = "";
            String Body = "";
            if (string.IsNullOrEmpty(EMailTo))
            {
                lblMsg.Text = "Customer E-Mail Address not found.";
                return;
            }
            StringBuilder sw = new StringBuilder(2000);
            DataSet dsTemplate = new DataSet();
            dsTemplate = CommonComponent.GetCommonDataSet("select * from  dbo.tb_EmailTemplate where storeid=" + ddlStore.SelectedValue + " And Label ='QuoteEmail'");
            if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
            {
                Body = dsTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
            }

            string strFromAddress = AppLogic.AppConfigs("Shipping.OriginContactName") + "<br/>"
                + AppLogic.AppConfigs("Shipping.OriginAddress") + "<br/>";
            if (!String.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginAddress2")))
                strFromAddress += AppLogic.AppConfigs("Shipping.OriginAddress2") + "<br/>";
            strFromAddress += AppLogic.AppConfigs("Shipping.OriginCity") + ", " + AppLogic.AppConfigs("Shipping.OriginState")
                + "<br/>" + AppLogic.AppConfigs("Shipping.OriginZip");

            Body = Body.Replace("###FROM_ADDRESS###", strFromAddress);
            Body = Body.Replace("###STOREPATH###", AppLogic.AppConfigs("STOREPATH"));
            Body = Body.Replace("###YEAR###", AppLogic.AppConfigs("YEAR"));

            Body = Body.Replace("###STORENAME###", AppLogic.AppConfigs("LIVE_SERVER"));
            Body = Body.Replace("###StoreID###", AppLogic.AppConfigs("StoreID"));
            Body = Body.Replace("###TODAY###", DateTime.Now.ToLongDateString());
            Body = Body.Replace("###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER"));

            String QuoteNumber = "";
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise")
            {
                string[] QuoteNumberArray = Request.QueryString["ID"].ToString().Split('-');
                QuoteNumber = QuoteNumberArray[0];

                int TotalCount = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(CustomerQuoteID) from tb_customerQuote Where QuoteNumber like '" + QuoteNumber + "-%'"));
                TotalCount = TotalCount + 1;
                QuoteNumber = Convert.ToString(QuoteNumber + "-" + TotalCount);
            }
            else
            {
                //strSql = "select ISNULL(MAX(CustomerQuoteID),0)+1 as QuoteNumber from tb_CustomerQuote";
                //QuoteNumber = Convert.ToString((Int32)CommonComponent.GetScalarCommonData(strSql));
            }

            if (!string.IsNullOrEmpty(QuoteNumber.ToString().Trim()))
                Body = Body.Replace("###CUSTOMERQUOTEID1###", "QUOTE : " + QuoteNumber.ToString());
            else QuoteNumber = "";

            Body = System.Text.RegularExpressions.Regex.Replace(Body, "###LIVE_SERVER_NAME###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Body = Body.Replace("###CUSTOMERID###", HdnCustID.Value.ToString());
            string StrShippingAddr = "";
            DataSet dsShippingAddress = new DataSet();
            dsShippingAddress = CommonComponent.GetCommonDataSet("Select * from tb_Address where AddressID in (SElect ShippingAddressID from tb_customer Where CustomerId=" + Convert.ToInt32(HdnCustID.Value) + ")");
            if (dsShippingAddress != null && dsShippingAddress.Tables.Count > 0 && dsShippingAddress.Tables[0].Rows.Count > 0)
            {
                StrShippingAddr += "<b>Shipping Address : </b><br/>";
                StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["FirstName"] + " " + dsShippingAddress.Tables[0].Rows[0]["LastName"] + "<br/>";

                if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Company"].ToString().Trim()))
                    StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Company"] + "<br/>";

                StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Address1"] + "<br/>";

                if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Address2"].ToString().Trim()))
                    StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Address2"] + "<br/>";

                if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Suite"].ToString().Trim()))
                    StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Suite"] + "<br/>";

                StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["City"] + ", " + dsShippingAddress.Tables[0].Rows[0]["State"] + " " + dsShippingAddress.Tables[0].Rows[0]["ZipCode"] + "<br/>";
                string ShippingCountry = (string)(CommonComponent.GetScalarCommonData("select name from tb_Country where CountryID=" + dsShippingAddress.Tables[0].Rows[0]["Country"]));
                StrShippingAddr += ShippingCountry + "<br/>";
                StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Phone"] + "<br/>";
            }

            Body = Body.Replace("###SHIPADDRESS###", StrShippingAddr.ToString().Trim().Replace("<b>Shipping Address : </b><br/>", ""));
            Body = Body.Replace("###mailto###", EmailID.Trim());
            Body = Body.Replace("###LIVE_SERVER_PRODUCT###", AppLogic.AppConfigs("LIVE_SERVER_PRODUCT_QUOTE").ToString().Trim());
            bool IsCouponDiscount = false;
            String strcart = "";
            strcart = " <table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\" style=\"padding: 10px 0 0;border: 1px solid #ECECEC; border-collapse: collapse; color: #212121; font: 12px;\"> ";
            strcart += "<tr style=\"background-color: rgb(242,242,242); height: 25px;\">";
            strcart += "  <th valign=\"middle\" align=\"Center\" style=\"width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Item #</th> ";
            strcart += "<th valign=\"middle\" align=\"left\" style=\"width:40%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Name</th>";
            strcart += "<th valign=\"middle\" align=\"right\" style=\"width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Unit Price</th> ";
            strcart += "###coupondiscount###";
            strcart += "<th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Qty</th> ";
            strcart += "<th valign=\"middle\" align=\"right\" style=\"width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Ext. Price</th> ";
            strcart += "<th valign=\"middle\" align=\"left\" style=\"background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Notes</th> ";
            strcart += "</tr> ";

            DataTable dtCart = new DataTable();
            DataSet DsTemp = new DataSet();
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise" && hdnIsReviseQuote.Value == "0")
            {
                Int32 CustQuoteID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select CustomerQuoteID from tb_CustomerQuote Where QuoteNumber ='" + Request.QueryString["ID"].ToString() + "'"));
                string strSql = "select ISNULL(cqi.DiscountPrice,0) as DiscountPrice,cqi.CustomerQuoteItemID,cq.customerid,p.sku,cqi.name,cqi.price,cqi.quantity,cqi.VariantNames,cqi.VariantValues,cqi.options,cqi.productid,IsNull(cqi.notes,'') as Notes from tb_CustomerQuoteItems cqi  " +
                " inner join tb_CustomerQuote cq  on cq.CustomerQuoteID=cqi.CustomerQuoteID " +
                " inner join tb_Product p  on p.ProductID=cqi.ProductID inner Join tb_customer on tb_customer.CustomerID=cq.CustomerID " +
                " where cq.CustomerQuoteID= " + CustQuoteID;
                DsTemp = CommonComponent.GetCommonDataSet(strSql);
            }
            else
            {
                DsTemp = CommonComponent.GetCommonDataSet("Select ISNULL(DiscountPrice,0) as DiscountPrice,CustomerQuoteItemID,CustomerQuoteID,Name,SKU,CustomerId,Notes,ISNULL(Quantity,0) as Quantity,ISNULL(Price,0)as Price,ProductID,VariantNames,VariantValues ,case when ISNULL(DiscountPrice,0)>0 then (ISNULL(Quantity,0) * ISNULL(DiscountPrice,0)) else (ISNULL(Quantity,0) * ISNULL(Price,0)) end as IndiSubTotal from tb_CustomerQuoteitems Where CustomerId=" + Convert.ToInt32(HdnCustID.Value) + " and CustomerQuoteID=0");
            }
            //dtCart = (DataTable)DsTemp.Tables[0];
            //foreach (DataRow dr in dtCart.Rows)
            //{
            //    strcart += "<tr> ";

            //    string[] variantName = dr["VariantNames"].ToString().ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //    string[] variantValue = dr["variantValues"].ToString().ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //    string strMount = "";
            //    string strColor = "";
            //    string strinstrSku = "";
            //    for (int p = 0; p < variantValue.Length; p++)
            //    {

            //        if (variantName[p].ToString().ToLower().IndexOf("mount") > -1)
            //        {
            //            strMount = "mount";
            //        }
            //        if (variantName[p].ToString().ToLower().IndexOf("color") > -1)
            //        {
            //            strColor = variantValue[p].ToString();
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(strMount) && !string.IsNullOrEmpty(strColor))
            //    {
            //        strinstrSku = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE VariantValue='" + strColor.ToString().Trim().Replace("'", "''") + "' and ProductId=" + Convert.ToInt32(dr["ProductID"].ToString()) + ""));
            //    }
            //    if (!string.IsNullOrEmpty(strinstrSku))
            //    {
            //        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"center\" valign=\"middle\"><p>" + strinstrSku.ToString() + "</p></td> ";
            //    }
            //    else
            //    {
            //        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"center\" valign=\"middle\"><p>" + dr["SKU"].ToString() + "</p></td> ";
            //    }


            //    if (variantName.Length > 0)
            //    {
            //        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"left\" valign=\"middle\">" + dr["Name"].ToString() + "";
            //        for (int j = 0; j < variantValue.Length; j++)
            //        {
            //            if (variantName.Length > j)
            //            {
            //                strcart += "<br />" + variantName[j].ToString() + " : " + variantValue[j].ToString();
            //            }
            //        }
            //        strcart += "</td>";
            //    }
            //    else
            //        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"left\" valign=\"middle\">" + dr["Name"].ToString() + "</td> ";

            //    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(dr["Price"]).ToString("f2") + "</td> ";
            //    decimal CouponDiscount = 0;
            //    if (!string.IsNullOrEmpty(dr["DiscountPrice"].ToString()))
            //    {
            //        decimal.TryParse(dr["DiscountPrice"].ToString(), out CouponDiscount);
            //        if (CouponDiscount > 0)
            //        {
            //            IsCouponDiscount = true;
            //            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(CouponDiscount).ToString("f2") + "</td> ";
            //        }

            //    }
            //    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"Center\" valign=\"middle\">" + dr["Quantity"].ToString() + "</td>";
            //    if (CouponDiscount > 0)
            //    {
            //        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(CouponDiscount.ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";
            //    }
            //    else
            //    {
            //        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(dr["Price"].ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";
            //    }
            //    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"Left\" valign=\"middle\">" + dr["Notes"].ToString() + "</td>";
            //    strcart += "</tr>";
            //}

            //strcart += "</table>";
            //Body = Body.Replace("###CART###", strcart.ToString().Trim());

            //if (IsCouponDiscount == true && Body.ToLower().IndexOf("###coupondiscount###") > -1)
            //{
            //    string StrCoupon = "<th valign=\"middle\" align=\"right\" style=\"text-align: center;width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Discount Price</th>";
            //    Body = Body.Replace("###coupondiscount###", StrCoupon.ToString().Trim());
            //}
            //else
            //{
            //    Body = Body.Replace("###coupondiscount###", "");
            //}

            //if (!string.IsNullOrEmpty(TxtNotes.Text.Trim()))
            //    Body = Body.Replace("###GENERAL_TEXT###", TxtNotes.Text.Trim().Trim());
            //else
            //    Body = Body.Replace("###GENERAL_TEXT###", "".Trim());
            dtCart = (DataTable)DsTemp.Tables[0];
            if (dtCart.Rows.Count > 0)
            {
                DataRow[] drr = dtCart.Select("DiscountPrice>0");
                if (drr.Length > 0)
                {
                    discountApply = true;

                }
                else
                {
                    discountApply = false;
                }
            }
            foreach (DataRow dr in dtCart.Rows)
            {
                strcart += "<tr> ";

                string[] variantName = dr["VariantNames"].ToString().ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantValue = dr["variantValues"].ToString().ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"center\" valign=\"middle\"><p>" + dr["SKU"].ToString() + "</p></td> ";

                if (variantName.Length > 0)
                {
                    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"left\" valign=\"middle\">" + dr["Name"].ToString() + "";
                    for (int j = 0; j < variantValue.Length; j++)
                    {
                        if (variantName.Length > j)
                        {
                            strcart += "<br />" + variantName[j].ToString() + " : " + variantValue[j].ToString();
                        }
                    }
                    strcart += "</td>";
                }
                else
                    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"left\" valign=\"middle\">" + dr["Name"].ToString() + "</td> ";
                //if (SwatchQty != 0)
                //{
                Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + " and ItemType='Swatch' "));
                if (Isorderswatch == 1)
                {
                    Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + ""));
                    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(pp).ToString("f2") + "</td> ";

                    //  txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                    // lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                    //   lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));

                }
                else
                {
                    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(dr["Price"]).ToString("f2") + "</td> ";
                }
                //}
                //else
                //{
                //    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(dr["Price"]).ToString("f2") + "</td> ";
                //}

                decimal CouponDiscount = 0;
                if (!string.IsNullOrEmpty(dr["DiscountPrice"].ToString()))
                {
                    decimal.TryParse(dr["DiscountPrice"].ToString(), out CouponDiscount);
                    if (CouponDiscount > 0)
                    {
                        IsCouponDiscount = true;
                        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(CouponDiscount).ToString("f2") + "</td> ";
                    }
                    else
                    {
                        if (discountApply == true)
                        {
                            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(0).ToString("f2") + "</td> ";
                        }
                    }

                }
                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"Center\" valign=\"middle\">" + dr["Quantity"].ToString() + "</td>";


                if (CouponDiscount > 0)
                {
                    //if (SwatchQty > Decimal.Zero)
                    //{

                    //    if (Isorderswatch == 1)
                    //    {
                    //        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + ""));
                    //        //  txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                    //        // lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                    //        //   lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                    //        pp = pp * Convert.ToDecimal(dr["Quantity"].ToString());
                    //        if (Convert.ToDecimal(pp) >= SwatchQty)
                    //        {
                    //            pp = (pp - SwatchQty) / Convert.ToDecimal(dr["Quantity"].ToString());
                    //            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(pp) * Convert.ToDecimal(dr["Quantity"].ToString())).ToString("f2") + " </td>";
                    //            SwatchQty = Decimal.Zero;
                    //        }
                    //        else
                    //        {
                    //            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal("0.00") * Convert.ToDecimal((dr["Quantity"].ToString()))).ToString("f2") + " </td>";
                    //            SwatchQty = SwatchQty - Convert.ToDecimal(pp.ToString());
                    //        }
                    //    }
                    //    else
                    //    {
                    //        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(CouponDiscount.ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";

                    //    }
                    //}
                    //else
                    //{

                    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(CouponDiscount.ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";

                    //}
                }
                else
                {


                    if (SwatchQty > Decimal.Zero)
                    {

                        if (Isorderswatch == 1)
                        {
                            Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + ""));
                            //  txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                            // lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                            //   lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                            pp = pp * Convert.ToDecimal(dr["Quantity"].ToString());
                            if (Convert.ToDecimal(pp) >= SwatchQty)
                            {
                                pp = (pp - SwatchQty) / Convert.ToDecimal(dr["Quantity"].ToString());
                                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(dr["Quantity"].ToString())).ToString("f2") + " </td>";

                                SwatchQty = Decimal.Zero;
                            }
                            else
                            {
                                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal("0.00") * Convert.ToDecimal((dr["Quantity"].ToString()))).ToString("f2") + " </td>";

                                SwatchQty = SwatchQty - Convert.ToDecimal(pp);
                            }
                        }
                        else
                        {
                            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(dr["Price"].ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";

                        }
                    }
                    else
                    {

                        if (Isorderswatch == 1)
                        {
                            Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + ""));
                            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";
                        }
                        else
                        {
                            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(dr["Price"].ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";
                        }


                    }
                }




                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"Left\" valign=\"middle\">" + dr["Notes"].ToString() + "</td>";
                strcart += "</tr>";
            }
            strcart += "</table>";

            Body = Body.Replace("###CART###", strcart.ToString().Trim());

            if (IsCouponDiscount == true && Body.ToLower().IndexOf("###coupondiscount###") > -1)
            {
                string StrCoupon = "<th valign=\"middle\" align=\"right\" style=\"width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Discount Price</th>";
                Body = Body.Replace("###coupondiscount###", StrCoupon.ToString().Trim());
            }
            else
            {
                Body = Body.Replace("###coupondiscount###", "");
            }

            if (!string.IsNullOrEmpty(TxtNotes.Text.Trim()))
                Body = Body.Replace("###GENERAL_TEXT###", TxtNotes.Text.Trim().Trim());
            else
                Body = Body.Replace("###GENERAL_TEXT###", "".Trim());

            txtBody.Text = Body.ToString();

            trEmail.Attributes.Add("style", "display:''");
            EMailSubject = "Customer Quote #" + QuoteNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName");
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
        /// Shopping Cart Items Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void GVShoppingCartItems_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Label lblHeaderDiscount = e.Row.FindControl("lblHeaderDiscount") as Label;
                if (Session["CustCouponCodeDiscount"] != null && Session["CustCouponCode"] != null)
                {
                    decimal Discount = 0;
                    decimal.TryParse(Session["CustCouponCodeDiscount"].ToString(), out Discount);
                    lblHeaderDiscount.Text = "(" + Discount.ToString("f2") + "%)";
                }
                Session["QtyDiscount1"] = null;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblName = e.Row.FindControl("lblName") as Label;
                Label lblItem = e.Row.FindControl("lblItem") as Label;
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                Label lblVariantNames = (Label)e.Row.FindControl("lblVariantNames");
                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                Label lblSKU = (Label)e.Row.FindControl("lblSKU");
                Label lblDiscountPrice = (Label)e.Row.FindControl("lblDiscountPrice");
                Label lblOrginalDiscountPrice = (Label)e.Row.FindControl("lblOrginalDiscountPrice");
                TextBox txtPrice = (TextBox)e.Row.FindControl("txtPrice");
                TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
                Label lblRelatedproductID = (Label)e.Row.FindControl("lblRelatedproductID");
                Label lblISProductType = e.Row.FindControl("lblisProductType") as Label;
                Label lblProductType = e.Row.FindControl("lblProductType") as Label;
                Label lblSwatchTot = e.Row.FindControl("lblSwatchTot") as Label;
                // Label lblIndiSubTotal = (Label)e.Row.FindControl("lblIndiSubTotal");
                // Label lblSubTot = (Label)e.Row.FindControl("lblSubTot");

                System.Web.UI.HtmlControls.HtmlInputHidden lblQuantity = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("lblQuantity");



                if (lblRelatedproductID != null && Convert.ToInt32(lblRelatedproductID.Text.ToString().Trim()) > 0)
                {
                    lblQuantity.Value = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(Quantity,0) FROM tb_ProductAssembly WHERE ProductID=" + lblProductID.Text.ToString() + " AND RefProductID=" + lblRelatedproductID.Text.ToString().Trim() + ""));
                }


                string[] variantName = lblVariantNames.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantValue = lblVariantValues.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
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
                    if (variantName[j].ToString().ToLower().IndexOf("color") > -1)
                    {
                        strColor = variantValue[j].ToString();
                    }
                }



                if (!string.IsNullOrEmpty(strMount) && !string.IsNullOrEmpty(strColor))
                {
                    lblSKU.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE VariantValue='" + strColor.ToString().Trim().Replace("'", "''") + "' and ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + ""));
                }

                decimal optionweight = 0;
                if (lblISProductType.Text.ToString() == "2")
                {
                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and SKU='" + lblSKU.Text.ToString().Trim() + "'"));

                    string strskufind = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(SKU,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and VariantValue like '%custom%'"));

                    if (!string.IsNullOrEmpty(strskufind))
                    {
                        lblSKU.Text = strskufind;
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
                                    sttrval = sttrval.Replace("(Buy 1 Get 1 Free)", "");
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
                                    sttrval = sttrval.Replace("(Buy 1 Get 1 Free)", "");
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

                //if (hdnSubTotalGrid != null)
                //{
                //    ViewState["hdnSubTotal"] = hdnSubTotalGrid.Value.ToString();
                //}
                //if (hdnWeightTotal != null)
                //{
                //    if (optionweight > Decimal.Zero)
                //    {
                //        ViewState["hdnWeightTotal"] = optionweight.ToString();
                //    }
                //    else
                //    {
                //        ViewState["hdnWeightTotal"] = hdnWeightTotal.Value.ToString();
                //    }
                //}
                //if (hdnWeightTotal1 != null)
                //{
                //    decimal dd = decimal.Zero;
                //    if (ViewState["hdnWeightTotal1"] != null)
                //    {
                //        dd = Convert.ToDecimal(ViewState["hdnWeightTotal1"].ToString());
                //    }
                //    dd = dd + Convert.ToDecimal(hdnWeightTotal1.Value.ToString());
                //    ViewState["hdnWeightTotal1"] = dd.ToString();
                //}

                //if (lblProductType != null)
                //{
                //    if (lblProductType.Text.Trim().ToLower() == "parent")
                //    {
                //        lblItem.Text = "-";
                //    }
                //    else
                //    {
                //        lblName.Text = "-";
                //    }
                //}
                Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + lblProductID.Text.ToString() + " and ItemType='Swatch' "));
                if (SwatchQty > Decimal.Zero)
                {

                    if (Isorderswatch == 1)
                    {
                        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + lblProductID.Text.ToString() + ""));
                        txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                        // lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                        //   lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                        pp = pp * Convert.ToDecimal(txtQuantity.Text.ToString());
                        if (Convert.ToDecimal(pp) >= SwatchQty)
                        {
                            //hdnswatchqty.Value = SwatchQty.ToString();
                            // hdnswatchtype.Value = "0";
                            pp = (pp - SwatchQty) / Convert.ToDecimal(txtQuantity.Text.ToString());
                            txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString())));
                            lblSwatchTot.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(txtQuantity.Text.ToString())));
                            //lblIndiSubTotal.Text = "$" + String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty)));
                            // lblSubTot.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty)));
                            SwatchQty = Decimal.Zero;
                        }
                        else
                        {
                            txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal("0.00")));
                            lblSwatchTot.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal("0.00") * Convert.ToDecimal(txtQuantity.Text.ToString())));
                            //  lblIndiSubTotal.Text = "$" + String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(lblQty.Text.ToString())));
                            // lblSubTot.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(lblQty.Text.ToString())));
                            ////  hdnswatchqty.Value = lblQty.Text.ToString();
                            // hdnswatchtype.Value = "0";
                            SwatchQty = SwatchQty - Convert.ToDecimal(pp);
                        }
                    }
                }
                else
                {
                    if (Isorderswatch == 1)
                    {
                        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + lblProductID.Text.ToString() + ""));
                        txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString())));
                        lblSwatchTot.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(txtQuantity.Text.ToString())));
                    }

                }




                decimal Qty = 0;
                decimal.TryParse(txtQuantity.Text.ToString(), out Qty);
                string Quantity = Qty.ToString();
                if (HdnCustID.Value != null && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null) && !string.IsNullOrEmpty(lblDiscountPrice.Text.ToString()))
                {
                    decimal DiscountPrice = 0, OrgiPrice = 0;
                    decimal.TryParse(Session["CustCouponCodeDiscount"].ToString(), out DiscountPrice);
                    decimal.TryParse(txtPrice.Text.ToString().Trim().ToString(), out OrgiPrice);
                    //  txtPrice.Attributes.Add("onkeyup", "CalculateDiscount(" + e.Row.RowIndex + ")");
                    //if (DiscountPrice > 0)
                    //{
                    //    decimal DicPrice = 0, TempDis = 0;
                    //    if (DiscountPrice > 0 && DiscountPrice <= 99)
                    //    {
                    //        TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                    //        DicPrice = OrgiPrice - TempDis;
                    //        lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                    //    }
                    //    else if (DiscountPrice >= 100)
                    //    {
                    //        TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                    //        DicPrice = TempDis;
                    //        lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                    //    }
                    //    else
                    //    { lblOrginalDiscountPrice.Text = "0.00"; }

                    //    GVShoppingCartItems.Columns[5].Visible = true;
                    //}
                    //else
                    //{
                    //    lblOrginalDiscountPrice.Text = "0.00";
                    //    GVShoppingCartItems.Columns[5].Visible = false;
                    //}


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
                            String ParentCategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select cast(ParentCategoryID as nvarchar(500))+',' from tb_CategoryMapping WHERE CategoryID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ") FOR XML PATH('')"));
                            if (ParentCategoryId.Length > 0)
                            {
                                ParentCategoryId = ParentCategoryId.Substring(0, ParentCategoryId.Length - 1);
                            }
                            else
                            {
                                ParentCategoryId = "0";
                            }

                            ProductDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT ISNULL(md.Discount,0) AS ProductDiscount "
                            + " FROM dbo.tb_MembershipDiscount md Left OUTER JOIN dbo.tb_Product Prod ON Prod.ProductID =md.DiscountObjectID " +
                            " WHERE md.CustID='" + HdnCustID.Value.ToString() + "' AND md.DiscountType='product' AND md.StoreID= " + ddlStore.SelectedValue.ToString() + " AND md.DiscountObjectID=" + ProductId + ""));
                            if (ProductDiscount <= 0)
                            {
                                CategoryDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT top 1 md.Discount AS CategoryDiscount "
                                    + " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                                    + "WHERE md.DiscountType='category' AND  md.CustID= " + HdnCustID.Value.ToString() + "  AND md.storeid=" + ddlStore.SelectedValue.ToString() + " AND (md.DiscountObjectID in (select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ") or md.DiscountObjectID in (" + ParentCategoryId + "))"));
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
                                    //  lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else if (DiscountPrice >= 100)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                    DicPrice = TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                    //    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else
                                {
                                    lblOrginalDiscountPrice.Text = string.Format("{0:0.00}", Convert.ToDecimal(OrgiPrice));
                                    // lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }

                                GVShoppingCartItems.Columns[5].Visible = true;


                            }
                            else
                            {
                                lblOrginalDiscountPrice.Text = string.Format("{0:0.00}", Convert.ToDecimal(OrgiPrice));
                                //  lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                            }
                        }
                        #endregion
                    }
                    else
                    {


                        String strCategory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidForCategory FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CustCouponCode"].ToString() + "'"));
                        String strProduct = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidforProduct FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CustCouponCode"].ToString() + "'"));
                        if (!string.IsNullOrEmpty(strCategory))
                        {
                            DataSet dspp = new DataSet();
                            dspp = CommonComponent.GetCommonDataSet("SELECT ProductId FROM tb_ProductCategory WHERE ProductId=" + lblProductID.Text.ToString() + " and categoryId in (" + strCategory.Replace(" ", "") + ")");
                            if (dspp != null && dspp.Tables.Count > 0 && dspp.Tables[0].Rows.Count > 0)
                            {
                                decimal DicPrice = 0, TempDis = 0;
                                if (DiscountPrice > 0 && DiscountPrice <= 99)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                    DicPrice = OrgiPrice - TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    // lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    //lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else if (DiscountPrice >= 100)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                    DicPrice = TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    //  lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    // lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else
                                {
                                    lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    //  lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    // lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
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
                                            //   lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                            //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                        }
                                        else if (DiscountPrice >= 100)
                                        {
                                            TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                            DicPrice = TempDis;
                                            lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                                            GVShoppingCartItems.Columns[5].Visible = true;
                                            //  lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                            //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));

                                        }
                                        else
                                        {
                                            lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                            GVShoppingCartItems.Columns[5].Visible = true;
                                            //    lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                            //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                        }
                                    }
                                    else
                                    {

                                        lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                        GVShoppingCartItems.Columns[5].Visible = true;
                                        //  lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                        //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    }
                                }
                                else
                                {
                                    lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    //  lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    // lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
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
                                    // lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else if (DiscountPrice >= 100)
                                {
                                    TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                                    DicPrice = TempDis;
                                    lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");

                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    //  lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                                else
                                {
                                    lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                    GVShoppingCartItems.Columns[5].Visible = true;
                                    // lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                    // lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                }
                            }
                            else
                            {

                                lblOrginalDiscountPrice.Text = OrgiPrice.ToString("f2");
                                GVShoppingCartItems.Columns[5].Visible = true;
                                //   lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
                                //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(OrgiPrice) * Convert.ToDecimal(Quantity.ToString())));
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
                                // lblIndiSubTotal.Text = "$" + string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                                //  lblSubTot.Text = string.Format("{0:0.00}", (Convert.ToDecimal(DicPrice) * Convert.ToDecimal(Quantity.ToString())));
                            }
                            else
                            {
                                lblOrginalDiscountPrice.Text = "0.00";
                                GVShoppingCartItems.Columns[5].Visible = false;
                            }
                        }
                    }




                }
                else
                {
                    if ((!string.IsNullOrEmpty(lblOrginalDiscountPrice.Text.ToString()) && Convert.ToDecimal(lblOrginalDiscountPrice.Text.ToString()) > 0) && (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise"))
                    {
                        GVShoppingCartItems.Columns[5].Visible = true;
                    }
                    else
                    {
                        GVShoppingCartItems.Columns[5].Visible = false;
                    }
                }
                if (lblRelatedproductID != null && Convert.ToInt32(lblRelatedproductID.Text.ToString()) > 0)
                {
                    // txtQuantity.Enabled = false;
                    // txtPrice.Enabled = false;
                    txtQuantity.Attributes.Add("readonly", "true");
                    txtPrice.Attributes.Add("readonly", "true");
                    //txtPrice.Attributes.Add("readonly", "true");
                }
                else
                {
                    txtQuantity.Attributes.Add("onkeyup", "CalculateQuantity(" + e.Row.RowIndex.ToString() + ");");

                }

            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            //lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));
        }

        /// <summary>
        /// Billing Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlB_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (hfSubTotal.Value == "")
            //    hfSubTotal.Value = "0";
            //if (hfTotal.Value == "")
            //    hfTotal.Value = "0";
            //lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            //lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));

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
                    ddlB_State.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State/Province", "0"));
                    ddlB_State.Items.Insert(dsState.Tables[0].Rows.Count + 1, new System.Web.UI.WebControls.ListItem("Other", "-11"));
                    ddlB_State.SelectedIndex = 0;
                }
                else
                {
                    ddlB_State.DataSource = null;
                    ddlB_State.DataBind();
                    ddlB_State.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State/Province", "0"));
                    ddlB_State.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Other", "-11"));
                    ddlB_State.SelectedIndex = 0;
                }
            }
            else
            {
                ddlB_State.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State/Province", "0"));
                ddlB_State.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Other", "-11"));
                ddlB_State.SelectedIndex = 0;
            }
            if (chkAddress.Checked)
            {
                ddlS_Country.SelectedIndex = ddlB_Country.SelectedIndex;
                ddlS_Country_SelectedIndexChanged(null, null);
                ddlS_State.SelectedIndex = ddlB_State.SelectedIndex;

            }
        }

        /// <summary>
        /// Shipping Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlS_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            //lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));

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
                    ddlS_State.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State/Province", "0"));
                    ddlS_State.Items.Insert(dsState.Tables[0].Rows.Count + 1, new System.Web.UI.WebControls.ListItem("Other", "-11"));
                    ddlS_State.SelectedIndex = 0;
                }

                else
                {
                    ddlS_State.DataSource = null;
                    ddlS_State.DataBind();
                    ddlS_State.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State/Province", "0"));
                    ddlS_State.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Other", "-11"));
                    ddlS_State.SelectedIndex = 0;
                }
            }
            else
            {
                ddlS_State.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select State/Province", "0"));
                ddlS_State.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Other", "-11"));
                ddlS_State.SelectedIndex = 0;


            }
            if (chkAddress.Checked == false)
            {
                hdncountry.Value = ddlS_Country.SelectedValue.ToString();
            }
        }

        /// <summary>
        ///  Shopping Cart Items Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnshoppingcartitems_click(object sender, EventArgs e)
        {
            GetShippIngMethodByBillAddress();
            BindCartInGrid();
        }

        /// <summary>
        /// Billing State Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlB_State_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            //lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));
            if (ddlB_State.SelectedValue == "-11")
            {
                txtB_OtherState.Visible = true;
            }
            else
            {
                txtB_OtherState.Visible = false;
            }
            txtB_Zip.Focus();
            GetShippIngMethodByBillAddress();
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
            //lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            //lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));
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
            //lblSubTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfSubTotal.Value.ToString()), 2));
            //lblTotal.Text = Convert.ToString(Math.Round(Convert.ToDecimal(hfTotal.Value.ToString()), 2));
            GetShippIngMethodByBillAddress();
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
        }

        /// <summary>
        /// Insert New Customer Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void aRelated_Click(object sender, ImageClickEventArgs e)
        {
            if (HdnCustID.Value == "" || HdnCustID.Value == "0")
            {
                CustomerComponent objCustomer = new CustomerComponent();
                tb_Customer objCust = new tb_Customer();
                Int32 CustID = -1;
                //CustID = objCustomer.InsertCustomer(objCust, Convert.ToInt32(ddlStore.SelectedValue.ToString()));
                InsertCustomer(ref CustID);
                HdnCustID.Value = Convert.ToString(CustID);
                Session["Isano"] = "1";
            }
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise" && hdnIsReviseQuote.Value == "0")
            {
                int CustQuoteID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select CustomerQuoteID from tb_CustomerQuote Where QuoteNumber ='" + Request.QueryString["ID"].ToString() + "'"));
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "openCenteredCrossSaleWindowRevise('ContentPlaceHolder1_lblSubTotal','ContentPlaceHolder1_lblSubTotal'," + CustQuoteID + ");", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "openCenteredCrossSaleWindow('ContentPlaceHolder1_lblSubTotal','ContentPlaceHolder1_lblSubTotal');", true);
            }
        }

        /// <summary>
        /// Inserts the Customer
        /// </summary>
        /// <returns>Returns true if Inserted, false otherwise</returns>
        private void InsertCustomer(ref Int32 CustID)
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
                // int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                // objCustomer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);

                Int32 CustomerId = objCust.PhoneOrderAddCustomer(objCustomer, Convert.ToInt32(ddlStore.SelectedValue.ToString()));

                if (CustomerId > 0)
                {
                    HdnCustID.Value = Convert.ToString(CustomerId);
                    CustID = CustomerId;
                }
                else
                {

                }
            }

        }

        /// <summary>
        /// Finally Place order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            btnUpdateProduct_Click(null, null);
            CustomerQuoteComponent objCustomerQuote = new CustomerQuoteComponent();
            Int32 CustomerQuoteID = 0, CustomerID = 0;


            if (!string.IsNullOrEmpty(HdnCustID.Value.ToString()) && HdnCustID.Value.ToString() != "0")
            {
                if (Session["Isano"] != null && Session["Isano"].ToString() == "1")
                {


                    Int32 ShipId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT AddressID FROM tb_Address WHERE AddressID in (SELECT isnull(ShippingAddressID,0) FROM tb_Customer WHERE CustomerID=" + HdnCustID.Value.ToString() + ")"));
                    Int32 BillId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT AddressID FROM tb_Address WHERE AddressID in (SELECT isnull(BillingAddressID,0) FROM tb_Customer WHERE CustomerID=" + HdnCustID.Value.ToString() + ")"));
                    if (ShipId == 0)
                    {
                        // CommonComponent.ExecuteCommonData("INSERT INTO tb_Address(FirstName,LastName,Country,AddressType) values ('','',1,1)");
                        ShipId = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO tb_Address(FirstName,LastName,Country,AddressType) values ('','',1,1); SELECT SCOPE_IDENTITY();"));

                    }
                    if (BillId == 0)
                    {
                        BillId = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO tb_Address(FirstName,LastName,Country,AddressType) values ('','',1,0); SELECT SCOPE_IDENTITY();"));
                    }
                    if (BillId > 0 && ShipId > 0)
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_Customer SET BillingAddressID=" + BillId + ",ShippingAddressID=" + ShipId + " WHERE CustomerID=" + HdnCustID.Value.ToString() + "");
                    }
                    
                    if (ddlB_State.SelectedItem.Text.ToString().ToLower().IndexOf("other") > -1)
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_Address SET FirstName='" + txtB_FName.Text.ToString().Replace("'", "''") + "',LastName='" + txtB_LName.Text.ToString().Replace("'", "''") + "',Company='" + txtB_Company.Text.ToString().Replace("'", "''") + "',Address1='" + txtB_Add1.Text.ToString().Replace("'", "''") + "',Address2='" + txtB_Add2.Text.ToString().Replace("'", "''") + "' ,City='" + txtB_City.Text.ToString().Replace("'", "''") + "',State='" + txtB_OtherState.Text.ToString().Replace("'", "''") + "',Suite='" + txtB_Suite.Text.ToString().Replace("'", "''") + "' ,ZipCode='" + txtB_Zip.Text.ToString().Replace("'", "''") + "',Country=" + ddlB_Country.SelectedValue.ToString().Replace("'", "''") + " ,Phone='" + txtB_Phone.Text.ToString().Replace("'", "''") + "',Email='" + TxtEmail.Text.ToString().Replace("'", "''") + "' WHERE AddressID in (SELECT isnull(BillingAddressID,0) FROM tb_Customer WHERE CustomerID=" + HdnCustID.Value.ToString() + ")");
                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_Address SET FirstName='" + txtB_FName.Text.ToString().Replace("'", "''") + "',LastName='" + txtB_LName.Text.ToString().Replace("'", "''") + "',Company='" + txtB_Company.Text.ToString().Replace("'", "''") + "',Address1='" + txtB_Add1.Text.ToString().Replace("'", "''") + "',Address2='" + txtB_Add2.Text.ToString().Replace("'", "''") + "' ,City='" + txtB_City.Text.ToString().Replace("'", "''") + "',State='" + ddlB_State.SelectedItem.ToString().Replace("'", "''") + "',Suite='" + txtB_Suite.Text.ToString().Replace("'", "''") + "' ,ZipCode='" + txtB_Zip.Text.ToString().Replace("'", "''") + "',Country=" + ddlB_Country.SelectedValue.ToString().Replace("'", "''") + " ,Phone='" + txtB_Phone.Text.ToString().Replace("'", "''") + "',Email='" + TxtEmail.Text.ToString().Replace("'", "''") + "' WHERE AddressID in (SELECT isnull(BillingAddressID,0) FROM tb_Customer WHERE CustomerID=" + HdnCustID.Value.ToString() + ")");
                    }
                    if (ddlS_State.SelectedItem.Text.ToString().ToLower().IndexOf("other") > -1)
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_Address SET FirstName='" + txtS_FName.Text.ToString().Replace("'", "''") + "',LastName='" + txtS_LNAme.Text.ToString().Replace("'", "''") + "',Company='" + txtS_Company.Text.ToString().Replace("'", "''") + "',Address1='" + txtS_Add1.Text.ToString().Replace("'", "''") + "',Address2='" + txtS_Add2.Text.ToString().Replace("'", "''") + "' ,City='" + txtS_City.Text.ToString().Replace("'", "''") + "',State='" + txtS_OtherState.Text.ToString().Replace("'", "''") + "',Suite='" + txtS_Suite.Text.ToString().Replace("'", "''") + "' ,ZipCode='" + txtS_Zip.Text.ToString().Replace("'", "''") + "',Country=" + ddlS_Country.SelectedValue.ToString().Replace("'", "''") + " ,Phone='" + txtS_Phone.Text.ToString().Replace("'", "''") + "',Email='" + TxtEmail.Text.ToString().Replace("'", "''") + "' WHERE AddressID in (SELECT isnull(ShippingAddressID,0) FROM tb_Customer WHERE CustomerID=" + HdnCustID.Value.ToString() + ")");
                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_Address SET FirstName='" + txtS_FName.Text.ToString().Replace("'", "''") + "',LastName='" + txtS_LNAme.Text.ToString().Replace("'", "''") + "',Company='" + txtS_Company.Text.ToString().Replace("'", "''") + "',Address1='" + txtS_Add1.Text.ToString().Replace("'", "''") + "',Address2='" + txtS_Add2.Text.ToString().Replace("'", "''") + "' ,City='" + txtS_City.Text.ToString().Replace("'", "''") + "',State='" + ddlS_State.SelectedItem.Text.ToString().Replace("'", "''") + "',Suite='" + txtS_Suite.Text.ToString().Replace("'", "''") + "' ,ZipCode='" + txtS_Zip.Text.ToString().Replace("'", "''") + "',Country=" + ddlS_Country.SelectedValue.ToString().Replace("'", "''") + " ,Phone='" + txtS_Phone.Text.ToString().Replace("'", "''") + "',Email='" + TxtEmail.Text.ToString().Replace("'", "''") + "' WHERE AddressID in (SELECT isnull(ShippingAddressID,0) FROM tb_Customer WHERE CustomerID=" + HdnCustID.Value.ToString() + ")");
                    }
                    CommonComponent.ExecuteCommonData("UPDATE tb_Customer SET FirstName='" + txtB_FName.Text.ToString().Replace("'", "''") + "',LastName='" + txtB_LName.Text.ToString().Replace("'", "''") + "',Email='" + TxtEmail.Text.ToString().Replace("'", "''") + "'  WHERE  CustomerID=" + HdnCustID.Value.ToString() + "");
                }

                CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());

                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise")
                {
                    string[] QuoteNumberArray = Request.QueryString["ID"].ToString().Split('-');
                    string QuoteNumber = QuoteNumberArray[0];

                    int TotalCount = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(CustomerQuoteID) from tb_customerQuote Where QuoteNumber like '%" + QuoteNumber + "-%'"));
                    TotalCount = TotalCount + 1;
                    String QuoteNumberNew = Convert.ToString(QuoteNumber + "-" + TotalCount);
                    CustomerQuoteID = objCustomerQuote.AddCustomerQuoteRevised(CustomerID, Convert.ToInt32(ddlStore.SelectedValue), QuoteNumberNew);

                    CommonComponent.ExecuteCommonData("Update tb_customerQuote Set IsRevised=1 Where QuoteNumber like '%" + QuoteNumber + "%' and CustomerQuoteID<>" + CustomerQuoteID + "");
                }
                else
                {
                    CustomerQuoteID = objCustomerQuote.AddCustomerQuote(CustomerID, Convert.ToInt32(ddlStore.SelectedValue), Convert.ToInt32(Session["AdminId"].ToString()));
                }
                int sendmailFormat = 0;
                if (rbMailList.SelectedValue.ToString() == "0")
                    sendmailFormat = 1;
                if (Session["CustCouponCode"] != null && !String.IsNullOrEmpty(Session["CustCouponCode"].ToString()))
                {
                    CommonComponent.ExecuteCommonData("Update tb_customerQuote Set GeneralNote='" + TxtNotes.Text.ToString() + "',IsHTML=" + sendmailFormat + ",CouponCode='" + Session["CustCouponCode"].ToString() + "' Where CustomerQuoteID=" + CustomerQuoteID + "");

                }
                else
                {
                    CommonComponent.ExecuteCommonData("Update tb_customerQuote Set GeneralNote='" + TxtNotes.Text.ToString() + "',IsHTML=" + sendmailFormat + " Where CustomerQuoteID=" + CustomerQuoteID + "");
                }
                ViewState["CustomerQuoteID"] = CustomerQuoteID;
                CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteItems set CustomerQuoteID=" + CustomerQuoteID + " Where CustomerId=" + CustomerID + " and CustomerQuoteID=0");
                int TotalItemCount = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(CustomerQuoteID) from tb_customerQuoteItems Where CustomerQuoteID =" + CustomerQuoteID + ""));
                if (TotalItemCount > 0)
                {

                }
                else
                {
                    string[] QuoteNumberArray = Request.QueryString["ID"].ToString().Split('-');
                    string QuoteNumber = QuoteNumberArray[0];

                    CommonComponent.ExecuteCommonData("Insert into  tb_CustomerQuoteItems ([CustomerQuoteID], [ProductID], [SKU], [Name], [Options], [Price], [Quantity], [Notes], [VariantNames], [VariantValues], [CustomerId], [IsProductType],[YardQuantity], [Actualyard],[DiscountPrice], [RelatedproductID] ) select " + CustomerQuoteID + ", ProductID, SKU, Name, Options, Price, Quantity, Notes, VariantNames, VariantValues, CustomerId, IsProductType, YardQuantity, Actualyard, DiscountPrice, RelatedproductID from    tb_CustomerQuoteItems where CustomerQuoteID=" + QuoteNumber + "");
                }

                SendQuotetoCustomer(CustomerQuoteID);
                Session["CustCouponCode"] = null;
                Session["CustCouponCodeDiscount"] = null;
                Session["CustCouponvalid"] = null;
                Response.Redirect("/Admin/Orders/CustomerQuoteList.aspx?status=inserted");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@Valimsg", "jAlert('Please Select Customer','ContentPlaceHolder1_TxtEmail',true);", true);
            }
        }

        /// <summary>
        /// Sends the quote to customer.
        /// </summary>
        private void SendQuotetoCustomer(Int32 CustomerQuoteID)
        {
            Boolean discountApply = false;
            string EmailID = (string)(CommonComponent.GetScalarCommonData("Select EmailID from tb_Admin where AdminID = " + Session["AdminID"].ToString() + " "));
            String EMailFrom = EmailID;
            String EMailTo = TxtEmail.Text.Trim();
            String EMailCC = "";
            String EMailBCC = "";
            String EMailSubject = "";
            String Body = "";
            if (string.IsNullOrEmpty(EMailTo))
            {
                lblMsg.Text = "Customer E-Mail Address not found.";
                return;
            }

            StringBuilder sw = new StringBuilder(2000);
            DataSet dsTemplate = new DataSet();
            dsTemplate = CommonComponent.GetCommonDataSet("select * from  dbo.tb_EmailTemplate where storeid=" + ddlStore.SelectedValue + " And Label ='QuoteEmail'");
            if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
            {
                Body = dsTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
            }

            string strFromAddress = AppLogic.AppConfigs("Shipping.OriginContactName") + "<br/>"
                + AppLogic.AppConfigs("Shipping.OriginAddress") + "<br/>";
            if (!String.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginAddress2")))
                strFromAddress += AppLogic.AppConfigs("Shipping.OriginAddress2") + "<br/>";
            strFromAddress += AppLogic.AppConfigs("Shipping.OriginCity") + ", " + AppLogic.AppConfigs("Shipping.OriginState")
                + "<br/>" + AppLogic.AppConfigs("Shipping.OriginZip");

            Body = Body.Replace("###FROM_ADDRESS###", strFromAddress);
            Body = Body.Replace("###STOREPATH###", AppLogic.AppConfigs("STOREPATH"));

            Body = Body.Replace("###STORENAME###", AppLogic.AppConfigs("LIVE_SERVER"));
            Body = Body.Replace("###YEAR###", AppLogic.AppConfigs("YEAR"));
            Body = Body.Replace("###TODAY###", DateTime.Now.ToLongDateString());
            Body = Body.Replace("###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER"));
            Body = Body.Replace("###StoreID###", AppLogic.AppConfigs("StoreID"));
            Body = Body.Replace("###LIVE_SERVER_PRODUCT###", AppLogic.AppConfigs("LIVE_SERVER_PRODUCT_QUOTE"));
            Body = Body.Replace("###CUSTOMERQUOTEID###", Server.UrlEncode(SecurityComponent.Encrypt(CustomerQuoteID.ToString())));

            string strSql = "Select QuoteNumber from tb_CustomerQuote where CustomerQuoteID=" + CustomerQuoteID.ToString();
            string QuoteNumber = (string)CommonComponent.GetScalarCommonData(strSql);
            Body = Body.Replace("###CUSTOMERQUOTEID1###", "QUOTE : " + QuoteNumber.ToString());
            Body = System.Text.RegularExpressions.Regex.Replace(Body, "###LIVE_SERVER_NAME###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            Body = Body.Replace("###CUSTOMERID###", HdnCustID.Value.ToString());
            string StrShippingAddr = "";
            DataSet dsShippingAddress = new DataSet();
            dsShippingAddress = CommonComponent.GetCommonDataSet("Select * from tb_Address where AddressID in (SElect ShippingAddressID from tb_customer Where CustomerId=" + Convert.ToInt32(HdnCustID.Value) + ")");
            if (dsShippingAddress != null && dsShippingAddress.Tables.Count > 0 && dsShippingAddress.Tables[0].Rows.Count > 0)
            {
                StrShippingAddr += "<b>Shipping Address : </b><br/>";
                StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["FirstName"] + " " + dsShippingAddress.Tables[0].Rows[0]["LastName"] + "<br/>";

                if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Company"].ToString().Trim()))
                    StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Company"] + "<br/>";

                StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Address1"] + "<br/>";

                if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Address2"].ToString().Trim()))
                    StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Address2"] + "<br/>";

                if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Suite"].ToString().Trim()))
                    StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Suite"] + "<br/>";

                StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["City"] + ", " + dsShippingAddress.Tables[0].Rows[0]["State"] + " " + dsShippingAddress.Tables[0].Rows[0]["ZipCode"] + "<br/>";
                string ShippingCountry = (string)(CommonComponent.GetScalarCommonData("select name from tb_Country where CountryID=" + dsShippingAddress.Tables[0].Rows[0]["Country"]));
                StrShippingAddr += ShippingCountry + "<br/>";
                StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Phone"] + "<br/>";
            }

            Body = Body.Replace("###SHIPADDRESS###", StrShippingAddr.ToString().Trim().Replace("<b>Shipping Address : </b><br/>", ""));
            Body = Body.Replace("###mailto###", EmailID.Trim());
            bool IsCouponDiscount = false;
            String strcart = "";
            strcart = " <table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\" style=\"padding: 10px 0 0;border: 1px solid #ECECEC; border-collapse: collapse; color: #212121; font: 12px;\"> ";
            strcart += "<tr style=\"background-color: rgb(242,242,242); height: 25px;\">";
            strcart += "  <th valign=\"middle\" align=\"Center\" style=\"width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Item #</th> ";
            strcart += "<th valign=\"middle\" align=\"left\" style=\"width:50%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Name</th>";
            strcart += "<th valign=\"middle\" align=\"right\" style=\"width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Unit Price</th> ";
            strcart += "###coupondiscount###";
            strcart += "<th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Qty</th> ";
            strcart += "<th valign=\"middle\" align=\"right\" style=\"width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Ext. Price</th> ";
            strcart += "<th valign=\"middle\" align=\"right\" style=\"background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Notes</th> ";
            strcart += "</tr> ";

            String strswatchQtyy = "";
            if (AppLogic.AppConfigs("SwatchMaxlength") != null && AppLogic.AppConfigs("SwatchMaxlength").ToString() != "")
            {
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "revise" && hdnIsReviseQuote.Value == "0")
                {
                    Int32 CustQuoteID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select CustomerQuoteID from tb_CustomerQuote Where QuoteNumber ='" + Request.QueryString["ID"].ToString() + "'"));
                    //strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(SUM(Quantity),0) as Quantity   from tb_CustomerQuoteitems inner Join tb_customer on tb_customer.CustomerID=tb_CustomerQuoteitems.CustomerID  Where tb_CustomerQuoteitems.CustomerId=" + Convert.ToInt32(HdnCustID.Value) + " and CustomerQuoteID=" + CustQuoteID + " and  IsProductType=0"));
                }
                else
                {
                    //strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(SUM(Quantity),0) as Quantity   from tb_CustomerQuoteitems inner Join tb_customer on tb_customer.CustomerID=tb_CustomerQuoteitems.CustomerID  Where tb_CustomerQuoteitems.CustomerId=" + Convert.ToInt32(HdnCustID.Value) + " and CustomerQuoteID=" + CustomerQuoteID.ToString() + " and  IsProductType=0"));
                }
                //if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
                //{
                SwatchQty = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                //}
            }


            DataTable dtCart = new DataTable();
            DataSet DsTemp = new DataSet();
            DsTemp = CommonComponent.GetCommonDataSet("Select ISNULL(DiscountPrice,0) as DiscountPrice,CustomerQuoteItemID,CustomerQuoteID,Name,SKU,CustomerId,Notes,ISNULL(Quantity,0) as Quantity,ISNULL(Price,0)as Price,ProductID,VariantNames,VariantValues ,case when ISNULL(DiscountPrice,0)>0 then (ISNULL(Quantity,0) * ISNULL(DiscountPrice,0)) else (ISNULL(Quantity,0) * ISNULL(Price,0)) end as IndiSubTotal,IsProductType as IsProductType from tb_CustomerQuoteitems Where CustomerId=" + Convert.ToInt32(HdnCustID.Value) + " and CustomerQuoteID=" + CustomerQuoteID.ToString() + "");

            dtCart = (DataTable)DsTemp.Tables[0];
            if (dtCart.Rows.Count > 0)
            {
                DataRow[] drr = dtCart.Select("DiscountPrice>0");
                if (drr.Length > 0)
                {
                    discountApply = true;

                }
                else
                {
                    discountApply = false;
                }
            }
            foreach (DataRow dr in dtCart.Rows)
            {
                strcart += "<tr> ";

                string[] variantName = dr["VariantNames"].ToString().ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantValue = dr["variantValues"].ToString().ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"center\" valign=\"middle\"><p>" + dr["SKU"].ToString() + "</p></td> ";

                if (variantName.Length > 0)
                {
                    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"left\" valign=\"middle\">" + dr["Name"].ToString() + "";
                    for (int j = 0; j < variantValue.Length; j++)
                    {
                        if (variantName.Length > j)
                        {
                            strcart += "<br />" + variantName[j].ToString() + " : " + variantValue[j].ToString();
                        }
                    }
                    strcart += "</td>";
                }
                else
                    strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"left\" valign=\"middle\">" + dr["Name"].ToString() + "</td> ";
                if (SwatchQty > Decimal.Zero)
                {
                    Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + " and ItemType='Swatch' "));
                    if (Isorderswatch == 1)
                    {
                        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + ""));
                        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(pp).ToString("f2") + "</td> ";

                        //  txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                        // lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                        //   lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                        //if (Convert.ToInt32(dr["Quantity"].ToString()) >= SwatchQty)
                        //{
                        //}
                    }
                    else
                    {
                        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(dr["Price"]).ToString("f2") + "</td> ";
                    }
                }
                else
                {
                    Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + " and ItemType='Swatch' "));
                    if (Isorderswatch == 1)
                    {
                        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + ""));
                        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(pp).ToString("f2") + "</td> ";
                    }
                    else
                    {


                        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(dr["Price"]).ToString("f2") + "</td> ";
                    }
                }

                decimal CouponDiscount = 0;
                if (!string.IsNullOrEmpty(dr["DiscountPrice"].ToString()))
                {
                    decimal.TryParse(dr["DiscountPrice"].ToString(), out CouponDiscount);
                    if (CouponDiscount > 0)
                    {
                        IsCouponDiscount = true;
                        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(CouponDiscount).ToString("f2") + "</td> ";
                    }
                    else
                    {
                        if (discountApply == true)
                        {
                            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(0).ToString("f2") + "</td> ";
                        }
                    }

                }
                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"Center\" valign=\"middle\">" + dr["Quantity"].ToString() + "</td>";


                if (CouponDiscount > 0)
                {
                    if (SwatchQty > Decimal.Zero)
                    {
                        Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + " and ItemType='Swatch' "));
                        if (Isorderswatch == 1)
                        {
                            Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + ""));
                            //  txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                            // lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                            //   lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                            pp = pp * Convert.ToDecimal(dr["Quantity"].ToString());
                            if (Convert.ToDecimal(pp) >= SwatchQty)
                            {
                                pp = (pp - SwatchQty) / Convert.ToDecimal(dr["Quantity"].ToString());
                                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(dr["Quantity"].ToString())).ToString("f2") + " </td>";

                                SwatchQty = Decimal.Zero; ;
                            }
                            else
                            {
                                SwatchQty = SwatchQty - Convert.ToDecimal(pp);
                                pp = Decimal.Zero; ;
                                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal((dr["Quantity"].ToString()))).ToString("f2") + " </td>";


                            }
                        }
                        else
                        {
                            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(CouponDiscount.ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";

                        }
                    }
                    else
                    {
                        strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(CouponDiscount.ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";
                    }
                }
                else
                {
                    Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + " and ItemType='Swatch' "));

                    if (SwatchQty > Decimal.Zero)
                    {

                        if (Isorderswatch == 1)
                        {
                            Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + ""));
                            //  txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                            // lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                            //   lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                            pp = pp * Convert.ToDecimal(dr["Quantity"].ToString());
                            if (Convert.ToDecimal(pp) >= SwatchQty)
                            {
                                pp = (pp - SwatchQty) / Convert.ToDecimal(dr["Quantity"].ToString());
                                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(dr["Quantity"].ToString())).ToString("f2") + " </td>";

                                SwatchQty = Decimal.Zero;
                            }
                            else
                            {
                                SwatchQty = SwatchQty - Convert.ToDecimal(pp.ToString());
                                pp = Decimal.Zero;
                                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal((dr["Quantity"].ToString()))).ToString("f2") + " </td>";


                            }
                        }
                        else
                        {
                            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(dr["Price"].ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";

                        }
                    }
                    else
                    {

                        if (Isorderswatch == 1)
                        {
                            Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dr["ProductID"].ToString() + ""));
                            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";
                        }
                        else
                        {
                            strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"right\" valign=\"middle\" class=\"price\">$" + Convert.ToDecimal(Convert.ToDecimal(dr["Price"].ToString()) * Convert.ToInt32(dr["Quantity"].ToString())).ToString("f2") + " </td>";
                        }


                    }
                }




                strcart += "<td style=\"background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 4px;\" align=\"Left\" valign=\"middle\">" + dr["Notes"].ToString() + "</td>";
                strcart += "</tr>";
            }
            strcart += "</table>";

            Body = Body.Replace("###CART###", strcart.ToString().Trim());

            if (IsCouponDiscount == true && Body.ToLower().IndexOf("###coupondiscount###") > -1)
            {
                string StrCoupon = "<th valign=\"middle\" align=\"right\" style=\"width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;\">Discount Price</th>";
                Body = Body.Replace("###coupondiscount###", StrCoupon.ToString().Trim());
            }
            else
            {
                Body = Body.Replace("###coupondiscount###", "");
            }

            if (!string.IsNullOrEmpty(TxtNotes.Text.Trim()))
                Body = Body.Replace("###GENERAL_TEXT###", TxtNotes.Text.Trim().Trim());
            else
                Body = Body.Replace("###GENERAL_TEXT###", "".Trim());

            EMailSubject = "Customer Quote #" + QuoteNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName");

            MakePDFFile(AppLogic.AppConfigs("LIVE_SERVER"), HdnCustID.Value.ToString(), dtCart, QuoteNumber.ToString(), CustomerQuoteID, IsCouponDiscount);

            if (rbMailList.SelectedValue.ToString() == "1")
            {
                AlternateView av = AlternateView.CreateAlternateViewFromString("As per your request we have sent you Quote for your order. Find it attached with this mail. <br/>Feel free to call us about any further clarifications.", null, "text/html");
                //CommonOperations.SendMailAttachment(EMailFrom, EMailTo, EMailCC, EMailBCC, EMailSubject, "As per your request we have sent you Quote for your order. Find it attached with this mail. <br/>Feel free to call us about any further clarifications.", Request.UserHostAddress.ToString(), true, av, Server.MapPath(AppLogic.AppConfigs("CustomerQuotePDFPath") + "Quote_" + QuoteNumber + ".pdf").ToString());
                CommonOperations.SendMailAttachment(EMailFrom, EMailTo, EMailCC, EMailBCC, EMailSubject, "As per your request we have sent you Quote for your order.", Request.UserHostAddress.ToString(), true, av, Server.MapPath(AppLogic.AppConfigs("CustomerQuotePDFPath") + "Quote_" + QuoteNumber + ".pdf").ToString());
            }
            else
            {
                String EmailBody = "";
                if (txtBody.Text.Trim().Length > 0)
                    EmailBody = txtBody.Text.ToString();
                else EmailBody = Body;
                EmailBody = Body;
                EmailBody = EmailBody.Replace("###CUSTOMERQUOTEID###", Server.UrlEncode(SecurityComponent.Encrypt(CustomerQuoteID.ToString())));
                EmailBody = EmailBody.Replace("###CUSTOMERQUOTEID1###", "QUOTE : " + QuoteNumber.ToString());
                AlternateView av = AlternateView.CreateAlternateViewFromString(EmailBody, null, "text/html");
                CommonOperations.SendMail(EMailFrom, EMailTo, EMailCC, EMailBCC, EMailSubject, EmailBody, Request.UserHostAddress.ToString(), true, av);
            }
        }

        /// <summary>
        /// Makes the PDF file.
        /// </summary>
        /// <param name="liveserver">string liveserver</param>
        /// <param name="customerid">string customerid</param>
        /// <param name="dt">DataTable dt</param>
        /// <param name="QuoteNumber">string QuoteNumber</param>
        private void MakePDFFile(string liveserver, string customerid, DataTable dt, string QuoteNumber, Int32 CustomerQuoteID, bool IsCouponDiscount)
        {
            Boolean discountApply = false;
            if (dt.Rows.Count > 0)
            {
                DataRow[] drr = dt.Select("DiscountPrice>0");
                if (drr.Length > 0)
                {
                    discountApply = true;

                }
                else
                {
                    discountApply = false;
                }
            }
            Document document = new Document();
            try
            {
                Rectangle rc = new Rectangle(0, 0, 600, 1024);
                rc.Border = 1;
                rc.BorderWidth = 1;
                rc.BorderWidthLeft = 1;
                rc.BorderWidthRight = 1;
                rc.BorderWidthBottom = 1;
                rc.BorderWidthTop = 1;

                rc.BorderColorBottom = Color.RED;
                rc.BorderColorTop = Color.RED;
                rc.BorderColorLeft = Color.RED;
                rc.BorderColorRight = Color.RED;
                rc.BorderColor = Color.RED;

                document.SetPageSize(rc);

                document.SetMargins(35, 35, 20, 20);
                try
                {
                    if (File.Exists(Server.MapPath(AppLogic.AppConfigs("CustomerQuotePDFPath") + "Quote_" + QuoteNumber + ".pdf").ToString()))
                    {
                        File.Delete(Server.MapPath(AppLogic.AppConfigs("CustomerQuotePDFPath") + "Quote_" + QuoteNumber + ".pdf").ToString());
                    }
                }
                catch { }

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(AppLogic.AppConfigs("CustomerQuotePDFPath") + "Quote_" + QuoteNumber + ".pdf").ToString(), FileMode.Create));

                writer.SetPageSize(rc);
                document.Open();

                float[] headerwidths = { 250, 250 };

                iTextSharp.text.Table aTable2 = new iTextSharp.text.Table(2);// 2 rows, 2 columns
                aTable2.Cellpadding = 1;
                aTable2.Cellspacing = 0;
                aTable2.BorderWidth = 0;
                aTable2.Widths = headerwidths;
                aTable2.WidthPercentage = 100;
                iTextSharp.text.Cell cell = new iTextSharp.text.Cell();
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(liveserver + "/images/logo.png");
                iTextSharp.text.Image img4 = img;
                //cell.BackgroundColor = new Color(243, 243, 243);

                img4.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
                img4.ScalePercent(60f);
                cell.Rowspan = 2;
                cell.Add(img4);
                cell.BorderWidth = 0;
                aTable2.AddCell(cell);

                //iTextSharp.text.Cell cell11 = new iTextSharp.text.Cell("\r\n                            " + AppLogic.AppConfigs("STORENAME").ToString() + "\r\n");
                //cell11.HorizontalAlignment = Element.ALIGN_MIDDLE;
                //cell11.BackgroundColor = new Color(243, 243, 243);
                //cell11.BorderWidth = 0;
                //aTable2.AddCell(cell11);


                //Paragraph paragraphtitle = new Paragraph("\r\n\r\n    Big Business Tools. Small Business Attitude.", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.ITALIC, new Color(0, 0, 0)));
                //iTextSharp.text.Cell cell13 = new iTextSharp.text.Cell();
                //cell13.HorizontalAlignment = Element.ALIGN_MIDDLE;
                //cell13.Add(paragraphtitle);
                //cell13.BackgroundColor = new Color(243, 243, 243);
                //cell13.BorderWidth = 0;
                //aTable2.AddCell(cell13);

                document.Add(aTable2);

                iTextSharp.text.Table aTable3 = new iTextSharp.text.Table(1);
                aTable3.Cellpadding = 0;
                aTable3.Cellspacing = 1;
                aTable3.BorderWidth = 0;
                aTable3.WidthPercentage = 100;


                iTextSharp.text.Cell cell1red = new iTextSharp.text.Cell("");
                cell1red.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1red.BackgroundColor = new Color(255, 0, 0);
                cell1red.BorderWidth = 0;
                aTable3.AddCell(cell1red);

                document.Add(aTable3);

                iTextSharp.text.Table aTable4 = new iTextSharp.text.Table(1);
                aTable4.Cellpadding = 0;
                aTable4.Cellspacing = 0;
                aTable4.BorderWidth = 0;
                aTable4.WidthPercentage = 100;
                aTable3.Alignment = Element.ALIGN_TOP;
                Font fn = FontFactory.GetFont(FontFactory.HELVETICA, 22, Font.BOLD, new Color(0, 0, 0));

                Chunk chunk = new Chunk("\r\nQUOTE: " + QuoteNumber, fn);
                iTextSharp.text.Cell cell1Quaote = new iTextSharp.text.Cell();
                cell1Quaote.HorizontalAlignment = Element.ALIGN_LEFT;

                cell1Quaote.Add(chunk);
                cell1Quaote.BackgroundColor = new Color(255, 255, 255);
                cell1Quaote.BorderWidth = 0;
                aTable4.AddCell(cell1Quaote);
                document.Add(aTable4);

                float[] headerwidths1 = { 250, 250 };
                iTextSharp.text.Table aTable5 = new iTextSharp.text.Table(2);
                aTable5.Cellpadding = 2;
                aTable5.Cellspacing = 1;
                aTable5.BorderWidth = 0;
                aTable5.Widths = headerwidths1;
                aTable5.WidthPercentage = 100;
                DataSet dsShippingAddress1 = new DataSet();
                dsShippingAddress1 = CommonComponent.GetCommonDataSet("SELECT ConfigValue,Configname FROM tb_AppConfig WHERE Storeid=1 and isnull(Deleted,0)=0 and Configname in ('STOREPATH','Shipping.OriginAddress','Shipping.OriginAddress2','Shipping.OriginAddress2','Shipping.OriginCity','Shipping.OriginState','Shipping.OriginZip')");
                string strFromAddress = "";
                if (dsShippingAddress1 != null && dsShippingAddress1.Tables.Count > 0 && dsShippingAddress1.Tables[0].Rows.Count > 0)
                {
                    strFromAddress = dsShippingAddress1.Tables[0].Select("Configname='Shipping.OriginAddress'")[0]["ConfigValue"].ToString() + "\r\n";
                    if (!String.IsNullOrEmpty(dsShippingAddress1.Tables[0].Select("Configname='Shipping.OriginAddress2'")[0]["ConfigValue"].ToString()))
                        strFromAddress += dsShippingAddress1.Tables[0].Select("Configname='Shipping.OriginAddress2'")[0]["ConfigValue"].ToString() + "\r\n";
                    strFromAddress += dsShippingAddress1.Tables[0].Select("Configname='Shipping.OriginCity'")[0]["ConfigValue"].ToString() + ", " + dsShippingAddress1.Tables[0].Select("Configname='Shipping.OriginState'")[0]["ConfigValue"].ToString()
                        + "\r\n" + dsShippingAddress1.Tables[0].Select("Configname='Shipping.OriginZip'")[0]["ConfigValue"].ToString();
                }
                else
                {
                    strFromAddress = AppLogic.AppConfigs("Shipping.OriginAddress") + "\r\n";
                    if (!String.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginAddress2")))
                        strFromAddress += AppLogic.AppConfigs("Shipping.OriginAddress2") + "\r\n";
                    strFromAddress += AppLogic.AppConfigs("Shipping.OriginCity") + ", " + AppLogic.AppConfigs("Shipping.OriginState")
                        + "\r\n" + AppLogic.AppConfigs("Shipping.OriginZip");
                }

                iTextSharp.text.Cell cell1address = new iTextSharp.text.Cell(dsShippingAddress1.Tables[0].Select("Configname='STOREPATH'")[0]["ConfigValue"].ToString() + "\r\n" + strFromAddress);
                cell1address.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1address.Rowspan = 2;
                cell1address.BackgroundColor = new Color(255, 255, 255);
                cell1address.BorderWidth = 0;
                aTable5.AddCell(cell1address);

                string EmailID = (string)(CommonComponent.GetScalarCommonData("Select EmailID from tb_Admin where AdminID = " + Session["AdminID"].ToString() + " "));

                Font chapterFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.ITALIC, new Color(0, 0, 0));
                Paragraph paragraph1 = new Paragraph("          Please contacts us at \r\n           " + EmailID.Trim() + "\r\n           if you have any questions.", chapterFont);

                iTextSharp.text.Cell cell1address111 = new iTextSharp.text.Cell();
                cell1address111.HorizontalAlignment = Element.ALIGN_MIDDLE;
                cell1address111.Add(paragraph1);
                cell1address111.BorderColor = new Color(255, 0, 0);
                aTable5.AddCell(cell1address111);

                //iTextSharp.text.Cell cell1address1d = new iTextSharp.text.Cell();
                //cell1address1d.HorizontalAlignment = Element.ALIGN_MIDDLE;
                //Font fntable = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(0, 0, 0));
                //Paragraph paragraph = new Paragraph("           " + AppLogic.AppConfigs("MailMe_ToAddress").ToString().Trim() + " \r\n                                  or \r\n                   ", fntable);

                //try
                //{
                //    Anchor anchor1 = new Anchor(AppLogic.AppConfigs("MailMe_ToAddress").ToString().Trim(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.UNDERLINE, new Color(255, 0, 0)));
                //    anchor1.Reference = "mailto:" + AppLogic.AppConfigs("MailMe_ToAddress").ToString().Trim();
                //    anchor1.Name = "top";
                //    paragraph.Add(anchor1);
                //}
                //catch
                //{
                //}

                //cell1address1d.Add(paragraph);
                //cell1address1d.BorderColor = new Color(255, 0, 0);
                //aTable5.AddCell(cell1address1d);
                document.Add(aTable5);

                Paragraph phg = new Paragraph("");
                document.Add(phg);

                float[] headerwidths2 = { 248, 70, 182 };
                iTextSharp.text.Table aTable6 = new iTextSharp.text.Table(3);
                aTable6.Cellpadding = 0;
                aTable6.Cellspacing = 0;
                aTable6.BorderWidth = 0;
                aTable6.Widths = headerwidths2;
                aTable6.WidthPercentage = 100;
                iTextSharp.text.Cell celldesc = new iTextSharp.text.Cell("Add this quote to your shopping cart by clicking");
                celldesc.HorizontalAlignment = Element.ALIGN_LEFT;
                celldesc.BorderWidth = 0;
                celldesc.BorderColor = new Color(255, 255, 255);
                aTable6.AddCell(celldesc);

                iTextSharp.text.Cell celldesc1 = new iTextSharp.text.Cell();
                celldesc1.HorizontalAlignment = Element.ALIGN_LEFT;
                //Paragraph PG = new Paragraph();
                //iTextSharp.text.Image imgAdd = iTextSharp.text.Image.GetInstance(AppLogic.AppConfigs("LIVE_SERVER_PRODUCT").ToString() + "/images/add-to-cart.jpg");
                //iTextSharp.text.Image imgAdd1 = imgAdd;
                //Chunk chk = new Chunk(imgAdd1, 10, 5, true);

                // Anchor anchordesc = new Anchor(chk);
                Anchor anchordesc = new Anchor("Add To Cart", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.UNDERLINE, new Color(255, 0, 0)));
                anchordesc.Reference = AppLogic.AppConfigs("LIVE_SERVER_PRODUCT_QUOTE").ToString() + "/customerquotecheckout.aspx?custquoteid=" + Server.UrlEncode(SecurityComponent.Encrypt(CustomerQuoteID.ToString()));
                anchordesc.Name = "top";
                celldesc1.BackgroundColor = new Color(255, 255, 255);
                celldesc1.Add(anchordesc);
                celldesc1.BorderWidth = 0;

                celldesc1.BorderColor = new Color(255, 255, 255);
                aTable6.AddCell(celldesc1);

                iTextSharp.text.Cell celldesc2 = new iTextSharp.text.Cell(" and following the simple");
                celldesc2.BorderWidth = 0;
                celldesc2.BorderColor = new Color(255, 255, 255);
                aTable6.AddCell(celldesc2);

                Font fn1 = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL, new Color(0, 0, 0));

                document.Add(aTable6);
                Chunk chunk1 = new Chunk("directions. This will convert quote into an on-line order, but your order will not be placed until you click Place Order on the confirmation page. You can place this order on-line 24 hours a day, 7 days a week.", fn1);
                document.Add(new Phrase(chunk1));

                string GeneralText = "";
                if (!string.IsNullOrEmpty(TxtNotes.Text.Trim()))
                    GeneralText = "\r\n" + TxtNotes.Text.Trim().ToString();
                else GeneralText = " ";

                Chunk chunk2 = new Chunk(GeneralText.ToString(), fn1);
                document.Add(new Phrase(chunk2));
                Font fn2 = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(0, 0, 0));

                Chunk chunk3 = new Chunk("\r\n\r\nCustomer Number: ", fn2);
                document.Add(new Phrase(chunk3));
                Chunk chunk4 = new Chunk(customerid.ToString(), fn1);
                document.Add(new Phrase(chunk4));
                Chunk chunk5 = new Chunk("                                                           " + DateTime.Now.ToLongDateString(), fn2);
                document.Add(new Phrase(chunk5));

                iTextSharp.text.Table aTable = new iTextSharp.text.Table(1);
                float[] headerwidthswidth = { 500 };
                aTable.Widths = headerwidthswidth;
                aTable.WidthPercentage = 100;
                iTextSharp.text.Cell cellname = new iTextSharp.text.Cell(" To:");
                cellname.VerticalAlignment = Element.ALIGN_MIDDLE;

                cellname.BorderColor = new Color(178, 178, 178);
                cellname.BackgroundColor = new Color(218, 218, 218);

                aTable.AddCell(cellname);
                aTable.DefaultVerticalAlignment = Element.ALIGN_MIDDLE;
                aTable.Padding = 2;
                aTable.Cellspacing = 0;
                aTable.DefaultCellGrayFill = 3;
                aTable.BorderWidth = 1;
                aTable.BorderColor = new Color(178, 178, 178);


                string StrShippingAddr = "";
                DataSet dsShippingAddress = new DataSet();
                dsShippingAddress = CommonComponent.GetCommonDataSet("Select * from tb_Address where AddressID in (SElect ShippingAddressID from tb_customer Where CustomerId=" + Convert.ToInt32(HdnCustID.Value) + ")");
                if (dsShippingAddress != null && dsShippingAddress.Tables.Count > 0 && dsShippingAddress.Tables[0].Rows.Count > 0)
                {
                    StrShippingAddr += "<b>Shipping Address : </b><br/>";
                    StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["FirstName"] + " " + dsShippingAddress.Tables[0].Rows[0]["LastName"] + "<br/>";

                    if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Company"].ToString().Trim()))
                        StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Company"] + "<br/>";

                    StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Address1"] + "<br/>";

                    if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Address2"].ToString().Trim()))
                        StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Address2"] + "<br/>";

                    if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Suite"].ToString().Trim()))
                        StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Suite"] + "<br/>";

                    StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["City"] + ", " + dsShippingAddress.Tables[0].Rows[0]["State"] + " " + dsShippingAddress.Tables[0].Rows[0]["ZipCode"] + "<br/>";
                    string ShippingCountry = (string)(CommonComponent.GetScalarCommonData("select name from tb_Country where CountryID=" + dsShippingAddress.Tables[0].Rows[0]["Country"]));
                    StrShippingAddr += ShippingCountry + "<br/>";
                    StrShippingAddr += dsShippingAddress.Tables[0].Rows[0]["Phone"] + "<br/>";
                }

                iTextSharp.text.Cell cellAdd = new iTextSharp.text.Cell(" " + StrShippingAddr.ToString().Replace("<b>Shipping Address : </b><br/>", "").Replace("<br/>", "\r\n "));
                cellAdd.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellAdd.BorderColor = new Color(178, 178, 178);
                aTable.AddCell(cellAdd);
                document.Add(aTable);

                iTextSharp.text.Table aTableCart;
                if (IsCouponDiscount == true)
                {
                    aTableCart = new iTextSharp.text.Table(7);
                    float[] headerwidthswidthcart = { 70, 150, 60, 60, 40, 60, 100 };
                    aTableCart.Widths = headerwidthswidthcart;
                }
                else
                {
                    aTableCart = new iTextSharp.text.Table(6);
                    float[] headerwidthswidthcart = { 70, 200, 60, 30, 60, 130 };
                    aTableCart.Widths = headerwidthswidthcart;
                }

                aTableCart.WidthPercentage = 100;

                iTextSharp.text.Cell cellsku = new iTextSharp.text.Cell("Item #");
                cellsku.BorderColor = new Color(164, 164, 164);
                cellsku.BackgroundColor = new Color(218, 218, 218);
                cellsku.VerticalAlignment = Element.ALIGN_TOP;
                cellsku.HorizontalAlignment = Element.ALIGN_CENTER;

                aTableCart.AddCell(cellsku);

                iTextSharp.text.Cell cellname1 = new iTextSharp.text.Cell("Name");
                cellname1.VerticalAlignment = Element.ALIGN_MIDDLE;

                cellname1.BorderColor = new Color(164, 164, 164);
                cellname1.BackgroundColor = new Color(218, 218, 218);

                aTableCart.AddCell(cellname1);


                iTextSharp.text.Cell cellunit = new iTextSharp.text.Cell("Unit price");
                cellunit.BorderColor = new Color(164, 164, 164);
                cellunit.BackgroundColor = new Color(218, 218, 218);
                cellunit.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellunit.VerticalAlignment = Element.ALIGN_MIDDLE;
                aTableCart.AddCell(cellunit);

                if (IsCouponDiscount == true)
                {
                    iTextSharp.text.Cell cellDicount = new iTextSharp.text.Cell("Discount Price");
                    cellDicount.BorderColor = new Color(164, 164, 164);
                    cellDicount.BackgroundColor = new Color(218, 218, 218);
                    cellDicount.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellDicount.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellDicount);
                }

                iTextSharp.text.Cell cellQty = new iTextSharp.text.Cell("Qty");
                cellQty.BorderColor = new Color(164, 164, 164);
                cellQty.BackgroundColor = new Color(218, 218, 218);
                cellQty.HorizontalAlignment = Element.ALIGN_CENTER;
                cellQty.VerticalAlignment = Element.ALIGN_MIDDLE;
                aTableCart.AddCell(cellQty);

                iTextSharp.text.Cell cellexprice = new iTextSharp.text.Cell("Ext. Price");
                cellexprice.BorderColor = new Color(164, 164, 164);
                cellexprice.BackgroundColor = new Color(218, 218, 218);
                cellexprice.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellexprice.VerticalAlignment = Element.ALIGN_MIDDLE;
                aTableCart.AddCell(cellexprice);

                iTextSharp.text.Cell cellnotes = new iTextSharp.text.Cell("Notes");
                cellnotes.BorderColor = new Color(164, 164, 164);
                cellnotes.BackgroundColor = new Color(218, 218, 218);
                cellnotes.HorizontalAlignment = Element.ALIGN_LEFT;
                cellnotes.VerticalAlignment = Element.ALIGN_MIDDLE;
                aTableCart.AddCell(cellnotes);

                aTableCart.DefaultVerticalAlignment = Element.ALIGN_MIDDLE;
                aTableCart.Padding = 3;
                aTableCart.DefaultCellGrayFill = 3;
                aTableCart.BorderWidth = 1;
                aTableCart.BorderColor = new Color(178, 178, 178);
                try
                {
                    SwatchQty = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        aTableCart.DefaultCell.GrayFill = 1.0f;
                        string[] variantName = dt.Rows[i]["VariantNames"].ToString().ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] variantValue = dt.Rows[i]["VariantValues"].ToString().ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string strMount = "";
                        string strColor = "";
                        string strinstrSku = "";
                        for (int p = 0; p < variantValue.Length; p++)
                        {

                            if (variantName[p].ToString().ToLower().IndexOf("mount") > -1)
                            {
                                strMount = "mount";
                            }
                            if (variantName[p].ToString().ToLower().IndexOf("color") > -1)
                            {
                                strColor = variantValue[p].ToString();
                            }
                        }
                        if (!string.IsNullOrEmpty(strMount) && !string.IsNullOrEmpty(strColor))
                        {
                            strinstrSku = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE VariantValue='" + strColor.ToString().Trim().Replace("'", "''") + "' and ProductId=" + Convert.ToInt32(dt.Rows[i]["ProductID"].ToString()) + ""));
                        }
                        if (string.IsNullOrEmpty(strinstrSku))
                        {
                            strinstrSku = dt.Rows[i]["SKU"].ToString();
                        }

                        iTextSharp.text.Cell cellsku1 = new iTextSharp.text.Cell(strinstrSku.ToString());
                        cellsku1.BorderColor = new Color(178, 178, 178);
                        cellsku1.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellsku1);



                        string proname = "";
                        if (variantName.Length > 0)
                        {
                            proname += dt.Rows[i]["Name"].ToString();
                            for (int j = 0; j < variantValue.Length; j++)
                            {
                                if (variantName.Length > j)
                                {
                                    proname += " \r\n " + variantName[j].ToString() + " : " + variantValue[j].ToString().Replace("<br/>", "\r\n");
                                }
                            }
                        }
                        else
                        {
                            proname = dt.Rows[i]["Name"].ToString();
                        }
                        iTextSharp.text.Cell cellnamest = new iTextSharp.text.Cell(proname.ToString());
                        cellnamest.BorderColor = new Color(178, 178, 178);
                        cellnamest.VerticalAlignment = Element.ALIGN_MIDDLE;
                        aTableCart.AddCell(cellnamest);

                        Decimal ProductPrc = Decimal.Zero;
                        ProductPrc = Convert.ToDecimal(dt.Rows[i]["Price"].ToString());
                        Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dt.Rows[i]["ProductID"].ToString() + " and ItemType='Swatch' "));
                        if (SwatchQty > Decimal.Zero)
                        {

                            if (Isorderswatch == 1)
                            {
                                Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dt.Rows[i]["ProductID"].ToString() + ""));
                                //  txtPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                                // lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                                //   lblSalePrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp));
                                pp = pp * Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString());
                                if (Convert.ToDecimal(pp) >= SwatchQty)
                                {
                                    pp = (pp - SwatchQty) / Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString());
                                    ProductPrc = pp;
                                    SwatchQty = Decimal.Zero; ;
                                }
                                else
                                {
                                    SwatchQty = SwatchQty - Convert.ToDecimal(pp);
                                    pp = Decimal.Zero;
                                    ProductPrc = pp;

                                }
                            }

                        }
                        else
                        {
                            if (Isorderswatch == 1)
                            {
                                Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dt.Rows[i]["ProductID"].ToString() + ""));
                                ProductPrc = pp;
                            }

                        }


                        iTextSharp.text.Cell cellunit11 = new iTextSharp.text.Cell(new Paragraph("$" + String.Format("{0:0.00}", ProductPrc), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL, new Color(255, 0, 0))));
                        cellunit11.BorderColor = new Color(178, 178, 178);
                        cellunit11.VerticalAlignment = Element.ALIGN_CENTER;
                        cellunit11.HorizontalAlignment = Element.ALIGN_RIGHT;
                        aTableCart.AddCell(cellunit11);

                        decimal CouponDiscount = 0;
                        if (!string.IsNullOrEmpty(dt.Rows[i]["DiscountPrice"].ToString()) && IsCouponDiscount == true)
                        {
                            decimal.TryParse(dt.Rows[i]["DiscountPrice"].ToString(), out CouponDiscount);
                            if (CouponDiscount > 0)
                            {
                                iTextSharp.text.Cell cellunit1 = new iTextSharp.text.Cell(new Paragraph("$" + String.Format("{0:0.00}", Convert.ToDecimal(dt.Rows[i]["DiscountPrice"].ToString())), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL, new Color(255, 0, 0))));
                                cellunit1.BorderColor = new Color(178, 178, 178);
                                cellunit1.VerticalAlignment = Element.ALIGN_CENTER;
                                cellunit1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                aTableCart.AddCell(cellunit1);
                            }
                            else if (discountApply == true)
                            {
                                iTextSharp.text.Cell cellunit1 = new iTextSharp.text.Cell(new Paragraph("$" + String.Format("{0:0.00}", Convert.ToDecimal(0)), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL, new Color(255, 0, 0))));
                                cellunit1.BorderColor = new Color(178, 178, 178);
                                cellunit1.VerticalAlignment = Element.ALIGN_CENTER;
                                cellunit1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                aTableCart.AddCell(cellunit1);
                            }
                        }

                        iTextSharp.text.Cell cellQty1 = new iTextSharp.text.Cell(dt.Rows[i]["Quantity"].ToString());
                        cellQty1.BorderColor = new Color(178, 178, 178);
                        cellQty1.VerticalAlignment = Element.ALIGN_CENTER;
                        cellQty1.HorizontalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellQty1);

                        if (CouponDiscount > 0)
                        {
                            iTextSharp.text.Cell cellexprice1 = new iTextSharp.text.Cell(new Paragraph("$" + Convert.ToDecimal(Convert.ToDecimal(CouponDiscount) * Convert.ToInt32(dt.Rows[i]["Quantity"].ToString())).ToString("f2"), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL, new Color(255, 0, 0))));
                            cellexprice1.BorderColor = new Color(178, 178, 178);
                            cellexprice1.VerticalAlignment = Element.ALIGN_CENTER;
                            cellexprice1.HorizontalAlignment = Element.ALIGN_RIGHT;
                            aTableCart.AddCell(cellexprice1);
                        }
                        else
                        {
                            iTextSharp.text.Cell cellexprice1 = new iTextSharp.text.Cell(new Paragraph("$" + Convert.ToDecimal(Convert.ToDecimal(ProductPrc) * Convert.ToInt32(dt.Rows[i]["Quantity"].ToString())).ToString("f2"), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL, new Color(255, 0, 0))));
                            cellexprice1.BorderColor = new Color(178, 178, 178);
                            cellexprice1.VerticalAlignment = Element.ALIGN_CENTER;
                            cellexprice1.HorizontalAlignment = Element.ALIGN_RIGHT;
                            aTableCart.AddCell(cellexprice1);
                        }

                        iTextSharp.text.Cell cellnotes1 = new iTextSharp.text.Cell(dt.Rows[i]["Notes"].ToString());
                        cellnotes1.BorderColor = new Color(178, 178, 178);
                        cellnotes1.VerticalAlignment = Element.ALIGN_CENTER;
                        cellnotes1.HorizontalAlignment = Element.ALIGN_LEFT;
                        aTableCart.AddCell(cellnotes1);
                    }

                    document.Add(aTableCart);
                }
                catch { }

                float[] headerwidths4 = { 440, 60 };
                iTextSharp.text.Table aTable8 = new iTextSharp.text.Table(2);
                aTable8.Cellpadding = 2;
                aTable8.Cellspacing = 1;
                aTable8.BorderWidth = 0;
                aTable8.Widths = headerwidths4;
                aTable8.WidthPercentage = 100;

                Paragraph Plink = new Paragraph("                         CLICK HERE TO ADD THIS QUOTE TO THE SHOPPING CART --> ", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL, new Color(0, 0, 0)));
                iTextSharp.text.Cell celldesc9 = new iTextSharp.text.Cell();
                celldesc9.Add(Plink);
                celldesc9.BorderWidth = 0;
                celldesc9.BorderColor = new Color(255, 255, 255);

                aTable8.AddCell(celldesc9);

                //iTextSharp.text.Image imgSubmit = iTextSharp.text.Image.GetInstance(AppLogic.AppConfigs("LIVE_SERVER_PRODUCT").ToString() + "/images/submit.jpg");
                //Chunk chkSubmit = new Chunk(imgSubmit, 0, 0);
                //Anchor anchorcontact = new Anchor(chkSubmit);

                Anchor anchorcontact = new Anchor("Submit", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.UNDERLINE, new Color(255, 0, 0)));
                //new Anchor("Submit", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL, new Color(255, 255, 255)));
                anchorcontact.Reference = AppLogic.AppConfigs("LIVE_SERVER_PRODUCT_QUOTE").ToString() + "/customerquotecheckout.aspx?custquoteid=" + Server.UrlEncode(SecurityComponent.Encrypt(CustomerQuoteID.ToString()));
                anchorcontact.Name = "top";
                iTextSharp.text.Cell celldesc10 = new iTextSharp.text.Cell();
                celldesc10.Add(anchorcontact);
                celldesc10.HorizontalAlignment = Element.ALIGN_MIDDLE;
                celldesc10.BorderWidth = 0;
                aTable8.AddCell(celldesc10);
                //document.Add(aTable8);

                float[] headerwidths5 = { 375, 62, 63 };
                iTextSharp.text.Table aTable9 = new iTextSharp.text.Table(3);
                aTable9.Cellpadding = 0;
                aTable9.Cellspacing = 0;
                aTable9.BorderWidth = 0;
                aTable9.Widths = headerwidths5;
                aTable9.WidthPercentage = 100;

                Paragraph Plinkf = new Paragraph("Thank you for the opportunity to quote this equipment. Please feel free to", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL, new Color(0, 0, 0)));

                iTextSharp.text.Cell celldescfooter = new iTextSharp.text.Cell();
                celldescfooter.Add(Plinkf);

                celldescfooter.BorderWidth = 0;
                celldescfooter.BorderColor = new Color(255, 255, 255);
                aTable9.AddCell(celldescfooter);

                Anchor anchorcontactf = new Anchor("contact me", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.UNDERLINE, new Color(255, 0, 0)));

                //anchorcontactf.Reference = AppLogic.AppConfigs("LIVE_SERVER_PRODUCT").ToString() + "/contactus.aspx";
                anchorcontactf.Reference = AppLogic.AppConfigs("LIVE_SERVER_PRODUCT_QUOTE").ToString() + "/customerquotecontact.aspx?custquoteid=" + Server.UrlEncode(SecurityComponent.Encrypt(CustomerQuoteID.ToString()));
                anchorcontactf.Name = "top";

                iTextSharp.text.Cell celldescf = new iTextSharp.text.Cell();
                celldescf.Add(anchorcontactf);

                celldescf.HorizontalAlignment = Element.ALIGN_LEFT;
                celldescf.BorderWidth = 0;
                celldescf.BorderColor = new Color(255, 0, 0);
                aTable9.AddCell(celldescf);

                iTextSharp.text.Cell celldescf1 = new iTextSharp.text.Cell("with any ");
                celldescf1.HorizontalAlignment = Element.ALIGN_LEFT;
                celldescf1.BorderWidth = 0;
                celldescf1.BorderColor = new Color(255, 0, 0);
                aTable9.AddCell(celldescf1);
                document.Add(aTable9);

                Chunk cjj = new Chunk("questions regarding this quote.");
                document.Add(new Phrase(cjj));
                document.Close();
                writer.CloseStream = true;
                writer.Close();
            }
            catch { }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/Admin/Orders/CustomerQuoteList.aspx");
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

        /// <summary>
        /// Update Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            if (GVShoppingCartItems.Rows.Count > 0)
            {
                for (int i = 0; i < GVShoppingCartItems.Rows.Count; i++)
                {
                    decimal DiscountPrice = 0;
                    Label lblCustomerQuoteItemID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblCustomerQuoteItemID");
                    Label lblProductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductID");
                    Label lblVariantNames = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantNames");
                    Label lblVariantValues = (Label)GVShoppingCartItems.Rows[i].FindControl("lblVariantValues");
                    Label lblRelatedproductID = (Label)GVShoppingCartItems.Rows[i].FindControl("lblRelatedproductID");
                    TextBox txtQuantity = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtQuantity");
                    TextBox txtPrice = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtPrice");
                    TextBox txtProductNotes = (TextBox)GVShoppingCartItems.Rows[i].FindControl("txtProductNotes");
                    Label lblOrginalDiscountPrice = (Label)GVShoppingCartItems.Rows[i].FindControl("lblOrginalDiscountPrice");
                    Label lblProductType = (Label)GVShoppingCartItems.Rows[i].FindControl("lblProductType");
                    int Quantity = 0;
                    decimal Price = 0;
                    int.TryParse(txtQuantity.Text.ToString(), out Quantity);
                    decimal.TryParse(txtPrice.Text.ToString(), out Price);

                    if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
                    {
                        decimal.TryParse(lblOrginalDiscountPrice.Text.ToString().Trim(), out DiscountPrice);
                    }

                    //if (lblRelatedproductID != null && Convert.ToInt32(lblProductID.Text.ToString().Trim()) > 0)
                    //{

                    //    CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteitems set Notes='" + txtProductNotes.Text.Trim().ToString().Replace("'", "''") + "',Quantity=" + Quantity + ",Price=" + Price + ",DiscountPrice=" + DiscountPrice + " Where CustomerQuoteItemID=" + lblCustomerQuoteItemID.Text.ToString() + " and ProductID=" + lblProductID.Text.Trim() + " and VariantNames='" + lblVariantNames.Text.ToString() + "' and VariantValues='" + lblVariantValues.Text.ToString() + "'");
                    //}
                    //else
                    //{
                    if (lblProductType.Text.ToString() == "0")
                    {
                        CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteitems set Notes='" + txtProductNotes.Text.Trim().ToString().Replace("'", "''") + "',Quantity=" + Quantity + ",Price=0,DiscountPrice=0 Where CustomerQuoteItemID=" + lblCustomerQuoteItemID.Text.ToString() + " and ProductID=" + lblProductID.Text.Trim() + " and VariantNames='" + lblVariantNames.Text.ToString() + "' and VariantValues='" + lblVariantValues.Text.ToString() + "'");

                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteitems set Notes='" + txtProductNotes.Text.Trim().ToString().Replace("'", "''") + "',Quantity=" + Quantity + ",Price=" + Price + ",DiscountPrice=" + DiscountPrice + " Where CustomerQuoteItemID=" + lblCustomerQuoteItemID.Text.ToString() + " and ProductID=" + lblProductID.Text.Trim() + " and VariantNames='" + lblVariantNames.Text.ToString() + "' and VariantValues='" + lblVariantValues.Text.ToString() + "'");
                    }
                    //  CommonComponent.ExecuteCommonData("update tb_customerquoteitems set Quantity=" + Quantity + " Where isnull(VariantNames,'')='" + lblVariantNames.Text.ToString() + "' and isnull(VariantValues,'')='" + lblVariantValues.Text.ToString() + "' and isnull(RelatedproductID,0) <> 0 AND RelatedproductID in (SELECT ProductID FROM tb_customerquoteitems Where CustomerQuoteItemID=" + lblCustomerQuoteItemID.Text.ToString() + ") AND CustomerQuoteID in (SELECT CustomerQuoteID FROM tb_customerquoteitems Where CustomerQuoteItemID=" + lblCustomerQuoteItemID.Text.ToString() + ")");

                    //}

                    //DataSet dsAssemblyproduct = new DataSet();
                    //dsAssemblyproduct = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductAssembly WHERE RefProductID=" + lblProductID.Text.Trim().ToString() + "");
                    //if (dsAssemblyproduct != null && dsAssemblyproduct.Tables.Count > 0 && dsAssemblyproduct.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int A = 0; A < dsAssemblyproduct.Tables[0].Rows.Count; A++)
                    //    {
                    //        CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteItems set quantity=" + (Quantity * Convert.ToInt32(dsAssemblyproduct.Tables[0].Rows[A]["Quantity"].ToString())) + " Where CustomerId=" + Convert.ToInt32(HdnCustID.Value.ToString()) + " and ProductID = " + dsAssemblyproduct.Tables[0].Rows[A]["ProductID"].ToString() + " AND RelatedproductID=" + lblProductID.Text.Trim().ToString() + "");
                    //    }
                    //}
                }
            }
            BindCartInGrid();
        }

        /// <summary>
        ///  Preview Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            btnUpdateProduct_Click(null, null);
            if (HdnCustID.Value != "" || HdnCustID.Value != "0")
            {
                if (GVShoppingCartItems.Rows.Count > 0)
                {
                    trEmail.Attributes.Add("style", "display: ''");
                    SendPreviewQuotetoCustomer();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgreset", "ShowHideButtonreset('ImgProd','tdSelectedProd','divSelectedProd');", true);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgScroll", "$('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_tablepreview').offset().top }, 'slow');", true);
                    //$('html, body').animate({ scrollTop: $('#footer-part').offset().top }, 'slow');
                }
                else
                {
                    trEmail.Attributes.Add("style", "display: none;");
                    lblMsg.Text = "Please add atleast one product.";
                    return;
                }
            }
        }

        public void PreiviewLoad()
        {
            if (HdnCustID.Value != "" || HdnCustID.Value != "0")
            {
                if (GVShoppingCartItems.Rows.Count > 0)
                {
                    trEmail.Attributes.Add("style", "display: ''");
                    SendPreviewQuotetoCustomer();
                }
                else
                {
                    trEmail.Attributes.Add("style", "display: none;");
                    lblMsg.Text = "Please add atleast one product.";
                    return;
                }
            }
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
            if (Request.QueryString["CustID"] != null)
            {
                HdnCustID.Value = Request.QueryString["CustID"].ToString();
            }
            CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteitems set DiscountPrice=0 Where CustomerQuoteID in (Select CustomerQuoteID from tb_CustomerQuote Where CustomerID=" + HdnCustID.Value.ToString() + ")");

            if (!string.IsNullOrEmpty(txtCouponCode.Text.ToString()) && !string.IsNullOrEmpty(HdnCustID.Value.ToString()))
            {
                DataSet dsCoupon = new DataSet();
                dsCoupon = CommonComponent.GetCommonDataSet("Select  0 DiscountPercent , * from tb_Customer Where CustomerID=" + HdnCustID.Value.ToString() + " and CouponCode='" + txtCouponCode.Text.ToString().Replace("'", "''") + "'");
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
                            if (DisCoupon > 0)
                            {
                                Session["CustCouponCode"] = txtCouponCode.Text.ToString();
                                Session["CustCouponCodeDiscount"] = DisCoupon.ToString();
                                Session["CustCouponvalid"] = "1";
                                BindCartInGrid();
                            }
                        }
                        else
                        {
                            BindCartInGrid();
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgCouponcode", "jAlert('Sorry, Coupon code is expired!','Message');", true);
                        }
                    }
                    else
                    {
                        Session["CustCouponCode"] = txtCouponCode.Text.ToString();
                        Session["CustCouponCodeDiscount"] = DisCoupon.ToString();
                        Session["CustCouponvalid"] = "1";
                        BindCartInGrid();
                    }
                }
                else
                {
                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcoupon", "jAlert('Please Enter Valid Coupon Code!', 'Message', 'ContentPlaceHolder1_txtCouponCode');", true);
                    decimal CouponDiscount = decimal.Zero;
                    //   SPMessage = Convert.ToString(CouponComponent.GetDiscountByCouponCodeFunction(txtCouponCode.Text.Trim(), Convert.ToInt32(HdnCustID.Value.ToString()), Convert.ToInt32(ddlStore.SelectedValue.ToString())));
                    SPMessage = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.GetCouponDiscount_Quote('" + txtCouponCode.Text.Trim() + "'," + HdnCustID.Value.ToString() + "," + Convert.ToInt32(ddlStore.SelectedValue.ToString()) + ")"));
                    decimal.TryParse(SPMessage.ToString(), out CouponDiscount);
                    if (CouponDiscount > 0)
                    {

                        Session["CustCouponCode"] = txtCouponCode.Text;
                        Session["CustCouponCodeDiscount"] = CouponDiscount;
                        txtCouponCode.Text = "";
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcoupon", "jAlert('Please Enter Valid Coupon Code!', 'Message', 'ContentPlaceHolder1_txtCouponCode');", true);
                    }
                    BindCartInGrid();
                }
            }
            else
            {
                decimal CouponDiscount = decimal.Zero;
                //   SPMessage = Convert.ToString(CouponComponent.GetDiscountByCouponCodeFunction(txtCouponCode.Text.Trim(), Convert.ToInt32(HdnCustID.Value.ToString()), Convert.ToInt32(ddlStore.SelectedValue.ToString())));
                SPMessage = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.GetCouponDiscount_Quote('" + txtCouponCode.Text.Trim() + "'," + HdnCustID.Value.ToString() + "," + Convert.ToInt32(ddlStore.SelectedValue.ToString()) + ")"));
                decimal.TryParse(SPMessage.ToString(), out CouponDiscount);
                if (CouponDiscount > 0)
                {

                    Session["CustCouponCode"] = txtCouponCode.Text;
                    Session["CustCouponCodeDiscount"] = CouponDiscount;
                    txtCouponCode.Text = "";
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcoupon", "jAlert('Please Enter Valid Coupon Code!', 'Message', 'ContentPlaceHolder1_txtCouponCode');", true);
                }
                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcoupon", "jAlert('Please Enter Valid Coupon Code!', 'Message', 'ContentPlaceHolder1_txtCouponCode');", true);
                BindCartInGrid();
            }
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcoupon", "jAlert('Please Select Customer!', 'Message', 'ContentPlaceHolder1_TxtEmail');", true);
            //}
        }

        protected bool ChkDiscountDateRange(string StrFromdate, string StrTodate)
        {
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
                    return true;
                }
                else { return false; }
            }
            return false;
        }


        protected bool ChkDiscountDateRangeCouponCode(string StrTodate)
        {
            if (!string.IsNullOrEmpty(StrTodate.Trim()))
            {

                DateTime TDate = new DateTime();
                DateTime Currdate = System.DateTime.Now;


                try { TDate = Convert.ToDateTime(StrTodate.Trim()); }
                catch { }

                if (Convert.ToDateTime(TDate.ToString("MM/dd/yyyy")) >= Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy")))
                {
                    return true;
                }
                else { return false; }
            }
            return false;
        }

    }
}