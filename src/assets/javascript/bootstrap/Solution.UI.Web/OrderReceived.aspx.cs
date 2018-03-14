using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.Text.RegularExpressions;
using System.Data;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;

namespace Solution.UI.Web
{
    public partial class OrderReceived : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        /// 
        public string strCode = "";
        public string strgoogletrack = "";
        public string strcretio = "";
        public string strfacebook = "";
        public string strgoogletrustedstore = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(true);
            if (!IsPostBack)
            {
                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    tblLogin.Visible = false;
                }
                else
                {
                    if (Request.QueryString["UserCreated"] != null && Request.QueryString["UserCreated"].ToString() == "true")
                    {
                        tblLogin.Visible = false;
                    }
                    else
                    {
                        if (Session["BillEmail"] != null)
                        {
                            txtEmailAddress.Text = Session["BillEmail"].ToString();
                            txtPassword.Focus();
                        }
                    }
                }




                if (Session["ONo"] != null && Session["ONo"].ToString().Trim().Length > 0)
                {
                    string url = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
                    aPrint.HRef = url + "/Invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(Session["ONo"].ToString()));
                    Session["NoOfCartItems"] = null;
                    ltrorderNumber.Text = Session["ONo"].ToString();

                    string strGoogleTracking = "";
                    DataSet dsorder = new DataSet();
                    dsorder = CommonComponent.GetCommonDataSet("SELECT *,isnull(CouponCode,'') as CouponCode1  FROM tb_order WHERE OrderNumber=" + ltrorderNumber.Text.ToString() + "");
                    if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
                    {
                        strGoogleTracking = GetGoogleEComTracking(dsorder, Convert.ToInt32(Session["ONo"]));
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Script", strGoogleTracking, false);

                        strCode = "<script type=\"text/javascript\">";
                        strCode += "_springMetq.push([\"setdata\", {revenue: \"" + dsorder.Tables[0].Rows[0]["Ordertotal"].ToString() + "\"}]);";
                        strCode += "_springMetq.push([\"setdata\", {\"orderId\": \"" + dsorder.Tables[0].Rows[0]["Ordernumber"].ToString() + "\" }]);";
                        strCode += "_springMetq.push([\"setdata\", {\"email\": \"" + dsorder.Tables[0].Rows[0]["Email"].ToString() + "\" }]);	";
                        strCode += "_springMetq.push([\"setdata\", {\"promoCode\": \"" + dsorder.Tables[0].Rows[0]["CouponCode1"].ToString() + "\" }]);";
                        strCode += "_springMetq.push([\"convert\", \"sale\" ]);";
                        strCode += "</script>";
                        ltshareasale.Text = "<img src=\"https://shareasale.com/sale.cfm?amount=" + String.Format("{0:0.00}", Convert.ToDecimal(dsorder.Tables[0].Rows[0]["Ordertotal"].ToString())) + "&tracking=" + dsorder.Tables[0].Rows[0]["Ordernumber"].ToString() + "&transtype=sale&merchantID=31170\" width=\"1\" height=\"1\">";


                        strgoogletrack = "<script type=\"text/javascript\">";
                        strgoogletrack += "/* <![CDATA[ */";
                        strgoogletrack += "var google_conversion_id = 1071279567;";
                        strgoogletrack += "var google_conversion_language = \"en\";";
                        strgoogletrack += "var google_conversion_format = \"3\";";
                        strgoogletrack += "var google_conversion_color = \"ffffff\";";
                        strgoogletrack += "var google_conversion_label = \"t8rmCI2e_wEQz9vp_gM\";";
                        strgoogletrack += "var google_conversion_value = " + dsorder.Tables[0].Rows[0]["Ordertotal"].ToString() + ";";
                        strgoogletrack += "var google_remarketing_only = false;";
                        strgoogletrack += "/* ]]> */";
                        strgoogletrack += "</script>";
                        strgoogletrack += "<script type=\"text/javascript\" src=\"//www.googleadservices.com/pagead/conversion.js\">";
                        strgoogletrack += "</script>";
                        strgoogletrack += "<noscript>";
                        strgoogletrack += "<div style=\"display:inline;\">";
                        strgoogletrack += "<img height=\"1\" width=\"1\" style=\"border-style:none;\" alt=\"\" src=\"//www.googleadservices.com/pagead/conversion/1071279567/?value=" + dsorder.Tables[0].Rows[0]["Ordertotal"].ToString() + "&amp;label=t8rmCI2e_wEQz9vp_gM&amp;guid=ON&amp;script=0\"/>";
                        strgoogletrack += "</div>";
                        strgoogletrack += "</noscript>";


                        strfacebook = "<script type=\"text/javascript\">";
                        strfacebook += " var fb_param = {};";
                        strfacebook += " fb_param.pixel_id = '6014170627969';";
                        strfacebook += " fb_param.value = '" + String.Format("{0:0.00}", Convert.ToDecimal(dsorder.Tables[0].Rows[0]["Ordertotal"].ToString())) + "';";
                        strfacebook += " fb_param.currency = 'USD';";
                        strfacebook += " (function () {";
                        strfacebook += " var fpw = document.createElement('script');";
                        strfacebook += " fpw.async = true;";
                        strfacebook += " fpw.src = '//connect.facebook.net/en_US/fp.js';";
                        strfacebook += " var ref = document.getElementsByTagName('script')[0];";
                        strfacebook += " ref.parentNode.insertBefore(fpw, ref);";
                        strfacebook += " })();";
                        strfacebook += " </script>";
                        strfacebook += " <noscript><img height=\"1\" width=\"1\" alt=\"\" style=\"display:none\" src=\"https://www.facebook.com/offsite_event.php?id=6014170627969&amp;value=" + String.Format("{0:0.00}", Convert.ToDecimal(dsorder.Tables[0].Rows[0]["Ordertotal"].ToString())) + "&amp;currency=USD\" /></noscript>";


                        strcretio = "<script type=\"text/javascript\" src=\"//static.criteo.net/js/ld/ld.js\" async=\"true\"> </script> <script type=\"text/javascript\"> window.criteo_q = window.criteo_q || []; window.criteo_q.push( {event: \"setAccount\", account: 6853}, {event: \"setSiteType\", type: \"d\"}, {event: \"trackTransaction\" , id: \"" + dsorder.Tables[0].Rows[0]["Ordernumber"].ToString() + "\", product: [ ";


                        try
                        {


                            Object objCounty = CommonComponent.GetScalarCommonData("select TwoLetterISOCode from tb_Country where Name='" + Convert.ToString(dsorder.Tables[0].Rows[0]["BillingCountry"].ToString()) + "'");

                            string country = "";

                            if (objCounty != null && Convert.ToString(objCounty) != "")
                            {
                                country = Convert.ToString(objCounty);
                            }
                            else
                            {
                                country = dsorder.Tables[0].Rows[0]["BillingCountry"].ToString();
                            }

                            strgoogletrustedstore = "<div id=\"gts-order\" style=\"display:none;\" translate=\"no\">";
                            strgoogletrustedstore += "<span id=\"gts-o-id\">" + dsorder.Tables[0].Rows[0]["Ordernumber"].ToString() + "</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-domain\">www.halfpricedrapes.com</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-email\">" + dsorder.Tables[0].Rows[0]["Email"].ToString() + "</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-country\">" + country.ToString() + "</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-currency\">USD</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-total\">" + string.Format("{0:0.00}", Convert.ToDecimal(dsorder.Tables[0].Rows[0]["Ordertotal"].ToString().Trim())) + "</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-discounts\">0.00</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-shipping-total\">" + string.Format("{0:0.00}", Convert.ToDecimal(dsorder.Tables[0].Rows[0]["OrderShippingCosts"].ToString().Trim())) + "</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-tax-total\">" + string.Format("{0:0.00}", Convert.ToDecimal(dsorder.Tables[0].Rows[0]["OrderTax"].ToString().Trim())) + "</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-est-ship-date\">###SDATE###</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-est-delivery-date\">###EDATE###</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-has-preorder\">###BACKORDER###</span>";
                            strgoogletrustedstore += "<span id=\"gts-o-has-digital\">N</span>";

                        }
                        catch
                        {
                            strgoogletrustedstore = "";
                        }


                        try
                        {
                            DataSet dsCart = new DataSet();
                            OrderComponent objOrderCart = new OrderComponent();
                            dsCart = objOrderCart.GetProductList(Convert.ToInt32(dsorder.Tables[0].Rows[0]["ShoppingCardID"].ToString()));
                            bool isbackOrder = false;
                            bool iscustom = false;
                            string strEstimateddate = "";
                            if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < dsCart.Tables[0].Rows.Count; k++)
                                {
                                    if (Convert.ToDecimal(dsCart.Tables[0].Rows[k]["DiscountPrice"].ToString().Trim()) > Decimal.Zero)
                                    {
                                        strcretio += "{id: \"" + dsCart.Tables[0].Rows[k]["SKU"].ToString().Trim() + "\", price: " + string.Format("{0:0.00}", Convert.ToDecimal(dsCart.Tables[0].Rows[k]["DiscountPrice"].ToString().Trim())) + ", quantity: " + dsCart.Tables[0].Rows[k]["Quantity"].ToString().Trim() + "},";
                                    }
                                    else
                                    {
                                        strcretio += "{id: \"" + dsCart.Tables[0].Rows[k]["SKU"].ToString().Trim() + "\", price: " + string.Format("{0:0.00}", Convert.ToDecimal(dsCart.Tables[0].Rows[k]["Price"].ToString().Trim())) + ", quantity: " + dsCart.Tables[0].Rows[k]["Quantity"].ToString().Trim() + "},";
                                    }
                                    try
                                    {

                                        if (!string.IsNullOrEmpty(strgoogletrustedstore))
                                        {
                                            int ProductID = 0;
                                            Int32.TryParse(dsCart.Tables[0].Rows[k]["RefProductID"].ToString().Trim(), out ProductID);
                                            strgoogletrustedstore += "<span class=\"gts-item\">";
                                            strgoogletrustedstore += "<span class=\"gts-i-name\">" + dsCart.Tables[0].Rows[k]["ProductName"].ToString().Trim() + "</span>";
                                            strgoogletrustedstore += "<span class=\"gts-i-price\">" + string.Format("{0:0.00}", Convert.ToDecimal(dsCart.Tables[0].Rows[k]["Price"].ToString().Trim())) + "</span>";
                                            strgoogletrustedstore += "<span class=\"gts-i-quantity\">" + dsCart.Tables[0].Rows[k]["Quantity"].ToString() + "</span>";
                                            strgoogletrustedstore += "<span class=\"gts-i-prodsearch-id\">" + ProductID.ToString() + "</span>";
                                            strgoogletrustedstore += "<span class=\"gts-i-prodsearch-store-id\">5831867</span>";
                                            strgoogletrustedstore += "<span class=\"gts-i-prodsearch-country\">US</span>";
                                            strgoogletrustedstore += "<span class=\"gts-i-prodsearch-language\">EN</span>";
                                            strgoogletrustedstore += "</span>";
                                            if (dsCart.Tables[0].Rows[k]["VariantNames"].ToString().Trim().ToLower().IndexOf("back order") > -1)
                                            {
                                                isbackOrder = true;
                                            }
                                            if (!string.IsNullOrEmpty(dsCart.Tables[0].Rows[k]["IsProductType"].ToString()) && dsCart.Tables[0].Rows[k]["IsProductType"].ToString() == "2")
                                            {

                                                iscustom = true;
                                            }



                                        }
                                    }
                                    catch
                                    {

                                    }

                                }
                            }
                            if (isbackOrder)
                            {
                                strgoogletrustedstore = strgoogletrustedstore.Replace("###BACKORDER###", "Y");
                                //  strgoogletrustedstore = strgoogletrustedstore.Replace("###SDATE###", "3-5 weeks");
                                // strgoogletrustedstore = strgoogletrustedstore.Replace("###EDATE###", "3-5 weeks");
                                strgoogletrustedstore = strgoogletrustedstore.Replace("###SDATE###", String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(35)));
                                strgoogletrustedstore = strgoogletrustedstore.Replace("###EDATE###", String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(35)));
                            }
                            else
                            {
                                if (iscustom)
                                {
                                    //  strgoogletrustedstore = strgoogletrustedstore.Replace("###SDATE###", "3-5 weeks");
                                    //  strgoogletrustedstore = strgoogletrustedstore.Replace("###EDATE###", "3-5 weeks");
                                    strgoogletrustedstore = strgoogletrustedstore.Replace("###SDATE###", String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(35)));
                                    strgoogletrustedstore = strgoogletrustedstore.Replace("###EDATE###", String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(35)));
                                }
                                else
                                {
                                    // strgoogletrustedstore = strgoogletrustedstore.Replace("###SDATE###", "10-12 days");
                                    // strgoogletrustedstore = strgoogletrustedstore.Replace("###EDATE###", "10-12 days");

                                    strgoogletrustedstore = strgoogletrustedstore.Replace("###SDATE###", String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(12)));
                                    strgoogletrustedstore = strgoogletrustedstore.Replace("###EDATE###", String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(12)));
                                }

                                strgoogletrustedstore = strgoogletrustedstore.Replace("###BACKORDER###", "N");
                            }
                            if (strcretio.Length > 0)
                            {
                                strcretio = strcretio.Substring(0, strcretio.Length - 1);
                            }
                        }
                        catch { }
                        strcretio += "]}); </script>";
                        if (!string.IsNullOrEmpty(strgoogletrustedstore))
                        {
                            strgoogletrustedstore += "</div>";
                        }
                    }
                    //strGoogleTracking = GetGoogleECommerceTracking(Convert.ToInt32(Session["ONo"]));
                    //this.Page.ClientScript.RegisterStartupScript(typeof(string), "Script", strGoogleTracking);

                    #region Code for Send Gift Card Certificate

                    DataSet dsOrderCartDetails = CommonComponent.GetCommonDataSet("Select ISNULL(ShoppingCardID,0) as ShoppingCartID,OrderNumber,CustomerID,FirstName,LastName,Email from tb_Order Where OrderNumber = " + Convert.ToInt32(Session["ONo"]) + "");
                    if (dsOrderCartDetails != null && dsOrderCartDetails.Tables.Count > 0 && dsOrderCartDetails.Tables[0].Rows.Count > 0)
                    {
                        if (ISGiftCardAddIntoCart(Convert.ToInt32(dsOrderCartDetails.Tables[0].Rows[0]["ShoppingCartID"].ToString())))
                        {
                            SendGiftCertificate(Convert.ToInt32(dsOrderCartDetails.Tables[0].Rows[0]["OrderNumber"]), Convert.ToInt32(dsOrderCartDetails.Tables[0].Rows[0]["ShoppingCartID"]), Convert.ToInt32(dsOrderCartDetails.Tables[0].Rows[0]["CustomerID"]), dsOrderCartDetails.Tables[0].Rows[0]["FirstName"].ToString(), dsOrderCartDetails.Tables[0].Rows[0]["LastName"].ToString(), dsOrderCartDetails.Tables[0].Rows[0]["Email"].ToString(),
                                    Request.UserHostAddress, null, (DataSet)Session["MyDataSet"]);

                            Session["MyDataSet"] = null;
                        }
                    }
                    #endregion


                }
                else
                {
                    Response.Redirect("/index.aspx");
                }
            }
        }

        /// <summary>
        /// Fill Password TextBox Back while PostBack Event Occurs
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtPassword_PreRender(object sender, EventArgs e)
        {
            txtPassword.Attributes["value"] = txtPassword.Text.ToString();
        }

        /// <summary>
        /// Fill Confirm Password TextBox Back while PostBack Event Occurs
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtConfirmPassword_PreRender(object sender, EventArgs e)
        {
            txtConfirmPassword.Attributes["value"] = txtConfirmPassword.Text.ToString();
        }

        /// <summary>
        ///  Create Account Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCreateAccount_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (txtEmailAddress.Text.ToString().Trim() == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Email Address.');", true);
                    txtEmailAddress.Focus();
                    return;
                }
                else
                {
                    string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
                    Match match = Regex.Match(txtEmailAddress.Text.ToString().Trim(), pattern, RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter valid Email Address.');", true);
                        txtEmailAddress.Focus();
                        return;
                    }
                    else if (txtPassword.Text.ToString().Trim() == "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Password.');", true);
                        txtPassword.Focus();
                        return;
                    }
                    else if (txtPassword.Text.ToString().Trim().ToString().Length < 6)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Password must be at least 6 characters long.');", true);
                        txtPassword.Focus();
                        return;
                    }
                    else if (txtConfirmPassword.Text.ToString().Trim() == "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Confirm Password.');", true);
                        txtConfirmPassword.Focus();
                        return;
                    }
                    else if (txtPassword.Text.ToString().Trim() != txtConfirmPassword.Text.ToString().Trim())
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Confirm Password must be match with Password.');", true);
                        txtConfirmPassword.Focus();
                        return;
                    }
                }

                CustomerComponent objCustomer = new CustomerComponent();
                Int32 IsAdded = Convert.ToInt32(objCustomer.AddCustomerAfterorderPlaced(Convert.ToInt32(ltrorderNumber.Text.ToString()), txtEmailAddress.Text.ToString(), SecurityComponent.Encrypt(txtPassword.Text.ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreId")), Convert.ToInt32(Session["CustID"])));
                if (IsAdded > 0)
                {
                    SendMail();
                    Session.Clear();
                    Session.Abandon();
                    Response.Cookies.Add(new System.Web.HttpCookie("ecommcustomer", null));
                    txtEmailAddress.Text = "";
                    txtPassword.Text = "";
                    txtConfirmPassword.Text = "";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your Account has been created successfully.');", true);
                    tblLogin.Visible = false;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('User already exists.');", true);
                }
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("orderrecievedCreateAccount: ", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Sending Mail to Customer EmailID
        /// </summary>
        private void SendMail()
        {
            try
            {
                CustomerComponent objCustomer = new CustomerComponent();
                DataSet dsCreateAccount = new DataSet();
                dsCreateAccount = objCustomer.GetEmailTamplate("CreateAccount", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                //Solution.Bussines.Entities.tb_Customer objCustData = objCustomer.GetCustomerDataByID(Convert.ToInt32(Session["CustID"]));
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
                        strBody = Regex.Replace(strBody, "###USERNAME###", txtEmailAddress.Text.ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###PASSWORD###"))
                    {
                        strBody = Regex.Replace(strBody, "###PASSWORD###", txtPassword.Text.ToString(), RegexOptions.IgnoreCase);
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
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                    try
                    {
                        CommonOperations.SendMail(txtEmailAddress.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                    }
                    catch (Exception ex)
                    {
                        CommonComponent.ErrorLog("orderRecievedSentmail: ", ex.Message, ex.StackTrace);
                    }
                }

            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("orderrecieved: ", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Create order Invoice PDF FIle
        /// </summary>
        /// <param name="OrderNumber">Int32 orderNumber</param>
        private void CreatePDFFile(Int32 OrderNumber)
        {
            Document document = new Document();
            PdfWriter writer = null;
            try
            {
                if (Session["CustID"] != null && Session["CustID"].ToString() != "")
                {
                    Int32 SwatchQty = 0;
                    decimal GrandSwatchSubTotal = decimal.Zero;
                    String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_OrderedShoppingCartItems WHERE IsProductType=0 AND OrderedShoppingCartID in (SELECT Top 1 OrderedShoppingCartID FROM tb_OrderedShoppingCart WHERE CustomerID=" + Session["CustID"] + " Order By OrderedShoppingCartID DESC) "));
                    if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
                    {
                        SwatchQty = Convert.ToInt32(strswatchQtyy) - Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                    }

                    DataSet dsOrder = new DataSet();
                    dsOrder = OrderComponent.GetOrderDetailsByOrderNumber(OrderNumber);
                    if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                    {
                        writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(AppLogic.AppConfigs("ImagePathInvoice").ToString() + "/Invoice_" + OrderNumber.ToString() + ".pdf").ToString(), FileMode.Create));

                        document.Open();
                        iTextSharp.text.Table table = new iTextSharp.text.Table(5);
                        iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                        float[] headerwidths = { 200, 80, 50 };
                        aTable.Widths = headerwidths;
                        aTable.WidthPercentage = 100;

                        iTextSharp.text.Table aTable2 = new iTextSharp.text.Table(1);// 2 rows, 2 columns
                        aTable2.Cellpadding = 3;
                        aTable2.Cellspacing = 3;
                        aTable2.BorderWidth = 0;
                        aTable2.WidthPercentage = 100;
                        iTextSharp.text.Cell cell = new iTextSharp.text.Cell();

                        iTextSharp.text.Image img;

                        try
                        {
                            img = iTextSharp.text.Image.GetInstance(AppLogic.AppConfigs("LIVE_SERVER") + "/Images/Store_" + Convert.ToString(dsOrder.Tables[0].Rows[0]["StoreID"]) + ".png");
                        }
                        catch (Exception ex)
                        {
                            img = iTextSharp.text.Image.GetInstance(AppLogic.AppConfigs("Live_Server") + "/images/logo.png");
                        }


                        iTextSharp.text.Image img4 = img;
                        img4.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                        cell.Add(img4);
                        cell.BorderWidth = 0;
                        aTable2.AddCell(cell);
                        document.Add(aTable2);

                        iTextSharp.text.Table aTable1 = new iTextSharp.text.Table(1, 2);
                        aTable1.WidthPercentage = 100;
                        aTable1.BorderWidth = 0;
                        aTable1.DefaultHorizontalAlignment = Element.ALIGN_LEFT;

                        iTextSharp.text.Cell cellContent;
                        Chunk chcontent;
                        chcontent = new Chunk(Convert.ToString(dsOrder.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(dsOrder.Tables[0].Rows[0]["LastName"].ToString() + ","), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                        Paragraph objp;

                        objp = new Paragraph("Dear ");
                        objp.Add(chcontent);
                        cellContent = new iTextSharp.text.Cell();
                        cellContent.BorderColor = new Color(0, 0, 255);
                        cellContent.Add(objp);
                        cellContent.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellContent.BorderWidth = 0;
                        aTable1.AddCell(cellContent);

                        cellContent = new iTextSharp.text.Cell("Your order has been received. Thank you for shopping with " + AppLogic.AppConfigs("StoreName").ToString() + ". \r\nWe appreciate your business and are committed to deliver excellent customer care. \r\n \r\n");
                        cellContent.BorderColor = new Color(0, 0, 255);

                        cellContent.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellContent.BorderWidth = 0;
                        aTable1.AddCell(cellContent);
                        document.Add(aTable1);

                        iTextSharp.text.Table aTableOrderNumber = new iTextSharp.text.Table(1, 5);
                        aTableOrderNumber.WidthPercentage = 100;
                        aTableOrderNumber.BorderWidth = 0;
                        aTableOrderNumber.DefaultHorizontalAlignment = Element.ALIGN_LEFT;

                        iTextSharp.text.Cell cellorder;
                        chcontent = new Chunk(OrderNumber.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                        objp = new Paragraph("Order Number: ");
                        objp.Add(chcontent);
                        cellorder = new iTextSharp.text.Cell(objp);
                        cellorder.BorderColor = new Color(0, 0, 255);
                        cellorder.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellorder.BorderWidth = 0;
                        aTableOrderNumber.AddCell(cellorder);

                        objp = new Paragraph("Order Date: ");
                        chcontent = new Chunk(dsOrder.Tables[0].Rows[0]["OrderDate"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                        objp.Add(chcontent);
                        cellorder = new iTextSharp.text.Cell(objp);
                        cellorder.BorderColor = new Color(0, 0, 255);
                        cellorder.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellorder.BorderWidth = 0;
                        aTableOrderNumber.AddCell(cellorder);

                        if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString()))
                        {
                            objp = new Paragraph("Shipping Method: ");
                            chcontent = new Chunk(dsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                            objp.Add(chcontent);
                            cellorder = new iTextSharp.text.Cell(objp);
                            cellorder.BorderColor = new Color(0, 0, 255);
                            cellorder.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellorder.BorderWidth = 0;
                            aTableOrderNumber.AddCell(cellorder);
                        }

                        if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString()))
                        {
                            objp = new Paragraph("Payment Method: ");
                            chcontent = new Chunk(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                            objp.Add(chcontent);
                            cellorder = new iTextSharp.text.Cell(objp);
                            cellorder.BorderColor = new Color(0, 0, 255);
                            cellorder.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellorder.BorderWidth = 0;
                            aTableOrderNumber.AddCell(cellorder);
                        }

                        string creditcardNumber = "";
                        if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString()) && dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString().ToLower() == "creditcard")
                        {
                            if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString()))
                            {
                                string CardNumber = SecurityComponent.Decrypt(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString());
                                if (CardNumber.Length > 4)
                                {
                                    for (int i = 0; i < CardNumber.Length - 4; i++)
                                    {
                                        creditcardNumber += "*";
                                    }
                                    creditcardNumber += CardNumber.ToString().Substring(CardNumber.Length - 4);
                                }
                                else
                                {
                                    creditcardNumber = "";
                                }
                            }
                        }
                        else
                        {
                            creditcardNumber = "";
                        }
                        if (creditcardNumber != "")
                        {
                            objp = new Paragraph("Card Number: ");
                            chcontent = new Chunk(creditcardNumber.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                            objp.Add(chcontent);
                            cellorder = new iTextSharp.text.Cell(objp);
                            cellorder.BorderColor = new Color(0, 0, 255);
                            cellorder.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellorder.BorderWidth = 0;
                            aTableOrderNumber.AddCell(cellorder);
                        }
                        document.Add(aTableOrderNumber);
                        objp = new Paragraph("\n");
                        document.Add(objp);

                        iTextSharp.text.Table aTableAddress = new iTextSharp.text.Table(3, 11);
                        aTableAddress.WidthPercentage = 100;
                        aTableAddress.BorderWidth = 0;
                        aTableAddress.DefaultHorizontalAlignment = Element.ALIGN_LEFT;

                        iTextSharp.text.Cell cellAddress;
                        chcontent = new Chunk("Account", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                        cellAddress = new iTextSharp.text.Cell(chcontent);
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 100F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);
                        chcontent = new Chunk("Billing Address", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                        cellAddress = new iTextSharp.text.Cell(chcontent);

                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        chcontent = new Chunk("Shipping Address", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                        cellAddress = new iTextSharp.text.Cell(chcontent);
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);
                        objp = new Paragraph(" ");
                        document.Add(objp);

                        cellAddress = new iTextSharp.text.Cell("Name:");
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.Width = 100F;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingLastName"].ToString()));
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString()));
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell("Company:");
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.Width = 100F;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingCompany"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCompany"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell("Address1:");
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.Width = 100F;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell("Address2:");
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.Width = 100F;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell("Suite:");
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.Width = 100F;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingSuite"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingSuite"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell("City:");
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.Width = 100F;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingCity"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCity"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingCity"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCity"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell("State:");
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.Width = 100F;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingState"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingState"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingState"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingState"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell("Zip:");
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.Width = 100F;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingZip"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingZip"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingZip"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell("Country:");
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.Width = 100F;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingCountry"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(dsOrder.Tables[0].Rows[0]["BillingCountry"].ToString());
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);
                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(dsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString());
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }

                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        cellAddress = new iTextSharp.text.Cell("Phone:");
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.Width = 100F;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingPhone"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                        {
                            cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString()));
                        }
                        else
                        {
                            cellAddress = new iTextSharp.text.Cell("-");
                        }
                        cellAddress.BorderColor = new Color(0, 0, 255);
                        cellAddress.Width = 200F;
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.BorderWidth = 0;
                        aTableAddress.AddCell(cellAddress);
                        document.Add(aTableAddress);

                        //Bind Cart
                        Int32 IColumn = 5;
                        string strwidth = "300F, 85F, 72F, 40F, 87F";

                        OrderComponent objOrderCart = new OrderComponent();
                        DataSet dsCart = new DataSet();
                        dsCart = objOrderCart.GetProductList(Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ShoppingCardID"].ToString()));
                        if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < dsCart.Tables[0].Rows.Count; k++)
                            {
                                if (Convert.ToDecimal(dsCart.Tables[0].Rows[k]["DiscountPrice"].ToString()) > Decimal.Zero)
                                {
                                    IColumn = 6;
                                }
                            }
                        }

                        iTextSharp.text.Table aTableCart = new iTextSharp.text.Table(IColumn);
                        aTableCart.WidthPercentage = 100F;
                        if (IColumn == 6)
                        {
                            float[] fl = { 250F, 80F, 72F, 60F, 40F, 82F };
                            aTableCart.Widths = fl;
                        }
                        else
                        {
                            float[] fl = { 300F, 85F, 72F, 40F, 87F };
                            aTableCart.Widths = fl;
                        }
                        iTextSharp.text.Cell cellcart;
                        cellcart = new iTextSharp.text.Cell("Product");
                        cellcart.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cellcart.BorderColor = new Color(164, 164, 164);
                        cellcart.BackgroundColor = new Color(218, 218, 218);
                        aTableCart.AddCell(cellcart);

                        cellcart = new iTextSharp.text.Cell("SKU");
                        cellcart.BorderColor = new Color(164, 164, 164);
                        cellcart.BackgroundColor = new Color(218, 218, 218);
                        cellcart.VerticalAlignment = Element.ALIGN_TOP;
                        aTableCart.AddCell(cellcart);

                        cellcart = new iTextSharp.text.Cell("Price");
                        cellcart.BorderColor = new Color(164, 164, 164);
                        cellcart.BackgroundColor = new Color(218, 218, 218);
                        cellcart.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellcart.VerticalAlignment = Element.ALIGN_MIDDLE;
                        aTableCart.AddCell(cellcart);
                        if (IColumn == 6)
                        {
                            cellcart = new iTextSharp.text.Cell("Discount Price");
                            cellcart.BorderColor = new Color(164, 164, 164);
                            cellcart.BackgroundColor = new Color(218, 218, 218);
                            cellcart.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellcart.VerticalAlignment = Element.ALIGN_MIDDLE;
                            aTableCart.AddCell(cellcart);
                        }


                        cellcart = new iTextSharp.text.Cell("Qty");
                        cellcart.BorderColor = new Color(164, 164, 164);
                        cellcart.BackgroundColor = new Color(218, 218, 218);
                        cellcart.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellcart.VerticalAlignment = Element.ALIGN_MIDDLE;
                        aTableCart.AddCell(cellcart);

                        cellcart = new iTextSharp.text.Cell("Sub Total");
                        cellcart.BorderColor = new Color(164, 164, 164);
                        cellcart.BackgroundColor = new Color(218, 218, 218);
                        cellcart.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellcart.VerticalAlignment = Element.ALIGN_MIDDLE;
                        aTableCart.AddCell(cellcart);

                        aTableCart.DefaultVerticalAlignment = Element.ALIGN_MIDDLE;
                        aTableCart.Padding = 3;
                        aTableCart.DefaultCellGrayFill = 3;
                        aTableCart.BorderWidth = 1;
                        aTableCart.BorderColor = new Color(218, 218, 218);


                        if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
                        {

                            for (int i = 0; i < dsCart.Tables[0].Rows.Count; i++)
                            {
                                iTextSharp.text.Cell cellValue;
                                aTable.DefaultCell.GrayFill = 1.0f;
                                string Pname = "";
                                Pname = dsCart.Tables[0].Rows[i]["ProductName"].ToString() + "\n";
                                objp = new Paragraph(Pname);

                                string[] variantName = dsCart.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string[] variantValue = dsCart.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                Pname = "";
                                for (int j = 0; j < variantValue.Length; j++)
                                {
                                    if (variantName.Length > j)
                                    {
                                        Pname += variantName[j].ToString() + " : " + variantValue[j].ToString() + "\n";
                                    }
                                }
                                chcontent = new Chunk(Pname.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, new Color(35, 31, 32)));
                                objp.Add(chcontent);
                                cellValue = new iTextSharp.text.Cell(objp);
                                cellValue.BorderColor = new Color(164, 164, 164);
                                cellValue.VerticalAlignment = Element.ALIGN_MIDDLE;
                                aTableCart.AddCell(cellValue);

                                cellValue = new iTextSharp.text.Cell(dsCart.Tables[0].Rows[i]["SKU"].ToString());
                                cellValue.BorderColor = new Color(164, 164, 164);
                                cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                aTableCart.AddCell(cellValue);

                                decimal CouponDiscount = 0;
                                if (SwatchQty != 0)
                                {
                                    Int32 Isorderswatch = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["RefProductID"].ToString() + " and ItemType='Swatch'"));
                                    if (Isorderswatch == 1)
                                    {
                                        if (Convert.ToInt32(dsCart.Tables[0].Rows[i]["Quantity"].ToString()) >= SwatchQty)
                                        {
                                            Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["RefProductID"].ToString() + ""));
                                            //price
                                            cellValue = new iTextSharp.text.Cell("$" + String.Format("{0:0.00}", Convert.ToDecimal(pp)));
                                            cellValue.BorderColor = new Color(164, 164, 164);
                                            cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                            cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            aTableCart.AddCell(cellValue);
                                            //discount price
                                            if (IColumn == 6)
                                            {
                                                cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()).ToString("C"));
                                                cellValue.BorderColor = new Color(164, 164, 164);
                                                cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                                cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                aTableCart.AddCell(cellValue);

                                            }
                                            //qty
                                            cellValue = new iTextSharp.text.Cell(dsCart.Tables[0].Rows[i]["Quantity"].ToString());
                                            cellValue.BorderColor = new Color(164, 164, 164);
                                            cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                            cellValue.HorizontalAlignment = Element.ALIGN_CENTER;
                                            aTableCart.AddCell(cellValue);

                                            //Indiotal start
                                            if (IColumn == 6 && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                            {
                                                cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()) * Convert.ToDecimal(SwatchQty)).ToString("C"));
                                                cellValue.BorderColor = new Color(164, 164, 164);
                                                cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                                cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                aTableCart.AddCell(cellValue);
                                                GrandSwatchSubTotal += Convert.ToDecimal(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()) * Convert.ToDecimal(SwatchQty));
                                            }
                                            else
                                            {
                                                cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty)).ToString("C"));
                                                cellValue.BorderColor = new Color(164, 164, 164);
                                                cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                                cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                aTableCart.AddCell(cellValue);
                                                GrandSwatchSubTotal += Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty));
                                            }


                                            SwatchQty = 0;

                                            //  GrandSwatchSubTotal += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["IndiTotal"].ToString());
                                        }
                                        else
                                        {
                                            Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["RefProductID"].ToString() + ""));
                                            cellValue = new iTextSharp.text.Cell("$" + String.Format("{0:0.00}", Convert.ToDecimal(pp)));
                                            cellValue.BorderColor = new Color(164, 164, 164);
                                            cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                            cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            aTableCart.AddCell(cellValue);

                                            if (IColumn == 6)
                                            {
                                                cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()).ToString("C"));
                                                cellValue.BorderColor = new Color(164, 164, 164);
                                                cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                                cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                aTableCart.AddCell(cellValue);
                                            }

                                            cellValue = new iTextSharp.text.Cell(dsCart.Tables[0].Rows[i]["Quantity"].ToString());
                                            cellValue.BorderColor = new Color(164, 164, 164);
                                            cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                            cellValue.HorizontalAlignment = Element.ALIGN_CENTER;
                                            aTableCart.AddCell(cellValue);

                                            //Inditotal
                                            if (IColumn == 6 && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                            {
                                                cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()) * Convert.ToDecimal(SwatchQty)).ToString("C"));
                                                cellValue.BorderColor = new Color(164, 164, 164);
                                                cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                                cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                aTableCart.AddCell(cellValue);
                                                GrandSwatchSubTotal += Convert.ToDecimal(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()) * Convert.ToDecimal(SwatchQty));
                                            }
                                            else
                                            {
                                                cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty)).ToString("C"));
                                                cellValue.BorderColor = new Color(164, 164, 164);
                                                cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                                cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                aTableCart.AddCell(cellValue);
                                                GrandSwatchSubTotal += Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty));
                                            }
                                            SwatchQty = SwatchQty - Convert.ToInt32(dsCart.Tables[0].Rows[i]["Quantity"].ToString());
                                            // GrandSwatchSubTotal += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["IndiTotal"].ToString());
                                        }


                                    }

                                    else
                                    {
                                        cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString()).ToString("C"));
                                        cellValue.BorderColor = new Color(164, 164, 164);
                                        cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                        cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        aTableCart.AddCell(cellValue);
                                        if (IColumn == 6 && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                        {

                                            cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()).ToString("C"));
                                            cellValue.BorderColor = new Color(164, 164, 164);
                                            cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                            cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            aTableCart.AddCell(cellValue);


                                        }
                                        cellValue = new iTextSharp.text.Cell(dsCart.Tables[0].Rows[i]["Quantity"].ToString());
                                        cellValue.BorderColor = new Color(164, 164, 164);
                                        cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                        cellValue.HorizontalAlignment = Element.ALIGN_CENTER;
                                        aTableCart.AddCell(cellValue);

                                        cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["IndiTotal"].ToString()).ToString("C"));
                                        cellValue.BorderColor = new Color(164, 164, 164);
                                        cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                        cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        aTableCart.AddCell(cellValue);
                                        GrandSwatchSubTotal += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["IndiTotal"].ToString());

                                    }
                                }
                                else
                                {
                                    cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString()).ToString("C"));
                                    cellValue.BorderColor = new Color(164, 164, 164);
                                    cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                    cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    aTableCart.AddCell(cellValue);



                                    if (IColumn == 6 && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                    {

                                        cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()).ToString("C"));
                                        cellValue.BorderColor = new Color(164, 164, 164);
                                        cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                        cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        aTableCart.AddCell(cellValue);


                                    }
                                    cellValue = new iTextSharp.text.Cell(dsCart.Tables[0].Rows[i]["Quantity"].ToString());
                                    cellValue.BorderColor = new Color(164, 164, 164);
                                    cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                    cellValue.HorizontalAlignment = Element.ALIGN_CENTER;
                                    aTableCart.AddCell(cellValue);

                                    cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["IndiTotal"].ToString()).ToString("C"));
                                    cellValue.BorderColor = new Color(164, 164, 164);
                                    cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                    cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    aTableCart.AddCell(cellValue);
                                    GrandSwatchSubTotal += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["IndiTotal"].ToString());

                                }
                                //
                                //cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString()).ToString("C"));
                                //cellValue.BorderColor = new Color(164, 164, 164);
                                //cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                //cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //aTableCart.AddCell(cellValue);

                                //if (IColumn == 6)
                                //{
                                //    cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()).ToString("C"));
                                //    cellValue.BorderColor = new Color(164, 164, 164);
                                //    cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                //    cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //    aTableCart.AddCell(cellValue);
                                //}

                                //cellValue = new iTextSharp.text.Cell(dsCart.Tables[0].Rows[i]["Quantity"].ToString());

                                //cellValue.BorderColor = new Color(164, 164, 164);
                                //cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                //cellValue.HorizontalAlignment = Element.ALIGN_CENTER;
                                //aTableCart.AddCell(cellValue);
                                //cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["IndiTotal"].ToString()).ToString("C"));
                                //cellValue.BorderColor = new Color(164, 164, 164);
                                //cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                                //cellValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //aTableCart.AddCell(cellValue);
                                //
                            }
                        }

                        iTextSharp.text.Cell cellCartFooter;
                        aTable.DefaultCell.GrayFill = 1.0f;

                        cellCartFooter = new iTextSharp.text.Cell("Sub Total:");
                        cellCartFooter.BorderColor = new Color(164, 164, 164);
                        cellCartFooter.Colspan = IColumn - 1;
                        cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellCartFooter);
                        //

                        if (GrandSwatchSubTotal > Decimal.Zero)
                        { cellCartFooter = new iTextSharp.text.Cell(GrandSwatchSubTotal.ToString("C")); }
                        else
                        {
                            cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderSubtotal"].ToString()).ToString("C"));

                        }
                        cellCartFooter.BorderColor = new Color(164, 164, 164);
                        cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellCartFooter);

                        cellCartFooter = new iTextSharp.text.Cell("Shipping:");
                        cellCartFooter.BorderColor = new Color(164, 164, 164);
                        cellCartFooter.Colspan = IColumn - 1;
                        cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellCartFooter);

                        cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()).ToString("C"));
                        cellCartFooter.BorderColor = new Color(164, 164, 164);
                        cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellCartFooter);

                        if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()) && (Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()) > 0))
                        {
                            cellCartFooter = new iTextSharp.text.Cell("Customer Level Discount:");
                            cellCartFooter.BorderColor = new Color(164, 164, 164);
                            cellCartFooter.Colspan = IColumn - 1;
                            cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                            aTableCart.AddCell(cellCartFooter);
                            cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()).ToString("C"));
                            cellCartFooter.BorderColor = new Color(164, 164, 164);
                            cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                            aTableCart.AddCell(cellCartFooter);
                        }

                        if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()) && (Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()) > 0))
                        {
                            cellCartFooter = new iTextSharp.text.Cell("Quantity Discount:");
                            cellCartFooter.BorderColor = new Color(164, 164, 164);
                            cellCartFooter.Colspan = IColumn - 1;
                            cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                            aTableCart.AddCell(cellCartFooter);
                            cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()).ToString("C"));
                            cellCartFooter.BorderColor = new Color(164, 164, 164);
                            cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                            aTableCart.AddCell(cellCartFooter);
                        }
                        if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString()) && (Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString()) > 0))
                        {
                            cellCartFooter = new iTextSharp.text.Cell("Gift Certificate Discount:");
                            cellCartFooter.BorderColor = new Color(164, 164, 164);
                            cellCartFooter.Colspan = IColumn - 1;
                            cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                            aTableCart.AddCell(cellCartFooter);
                            cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString()).ToString("C"));
                            cellCartFooter.BorderColor = new Color(164, 164, 164);
                            cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                            aTableCart.AddCell(cellCartFooter);
                        }
                        cellCartFooter = new iTextSharp.text.Cell("Discount:");
                        cellCartFooter.BorderColor = new Color(164, 164, 164);
                        cellCartFooter.Colspan = IColumn - 1;
                        cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellCartFooter);

                        cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString()).ToString("C"));
                        cellCartFooter.BorderColor = new Color(164, 164, 164);
                        cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellCartFooter);

                        cellCartFooter = new iTextSharp.text.Cell("Order Tax:");
                        cellCartFooter.BorderColor = new Color(164, 164, 164);
                        cellCartFooter.Colspan = IColumn - 1;
                        cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellCartFooter);

                        cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTax"].ToString()).ToString("C"));
                        cellCartFooter.BorderColor = new Color(164, 164, 164);
                        cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellCartFooter);

                        cellCartFooter = new iTextSharp.text.Cell("Total:");
                        cellCartFooter.BorderColor = new Color(164, 164, 164);
                        cellCartFooter.Colspan = IColumn - 1;
                        cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellCartFooter);

                        cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString()).ToString("C"));
                        cellCartFooter.BorderColor = new Color(164, 164, 164);
                        cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellCartFooter.VerticalAlignment = Element.ALIGN_CENTER;
                        aTableCart.AddCell(cellCartFooter);
                        document.Add(aTableCart);

                        iTextSharp.text.Table aTableFooter = new iTextSharp.text.Table(1, 3);
                        aTableFooter.WidthPercentage = 100;
                        aTableFooter.BorderWidth = 0;
                        aTableFooter.DefaultHorizontalAlignment = Element.ALIGN_LEFT;

                        iTextSharp.text.Cell cellRegards1;


                        cellRegards1 = new iTextSharp.text.Cell("Congratulations! Your order has been prioritized and is being processed now. You will receive another email containing your tracking information.");
                        cellRegards1.BorderColor = new Color(0, 0, 255);
                        cellRegards1.Width = 100F;
                        cellRegards1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellRegards1.BorderWidth = 0;
                        aTableFooter.AddCell(cellRegards1);

                        //iTextSharp.text.Cell cellRegards2;

                        //cellRegards2 = new iTextSharp.text.Cell("Please note: If you are purchasing a large order it may be delivered separately. \n\n");
                        //cellRegards2.BorderColor = new Color(0, 0, 255);
                        //cellRegards2.Width = 100F;
                        //cellRegards2.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cellRegards2.BorderWidth = 0;
                        //aTableFooter.AddCell(cellRegards2);


                        iTextSharp.text.Cell cellRegards;

                        chcontent = new Chunk(AppLogic.AppConfigs("PdfRegards").ToString().Replace("<br />", "\n").Replace("<br/>", "\n").Replace("<br>", "\n"), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                        cellRegards = new iTextSharp.text.Cell(chcontent);
                        cellRegards.BorderColor = new Color(0, 0, 255);
                        cellRegards.Width = 100F;
                        cellRegards.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellRegards.BorderWidth = 0;
                        aTableFooter.AddCell(cellRegards);







                        Anchor anchorLink = new Anchor(AppLogic.AppConfigs("PdfRegardsLink").ToString());
                        anchorLink.Reference = AppLogic.AppConfigs("PdfRegardsLink").ToString();
                        anchorLink.Name = "top";
                        cellRegards = new iTextSharp.text.Cell(anchorLink);
                        cellRegards.BorderColor = new Color(0, 0, 255);
                        cellRegards.Width = 100F;
                        cellRegards.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellRegards.BorderWidth = 0;
                        aTableFooter.AddCell(cellRegards);

                        document.Add(aTableFooter);
                        document.Close();
                        writer.CloseStream = true;
                        writer.Close();
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Download product PDF File
        /// </summary>
        /// <param name="filepath">string Filepath</param>
        private void DownloadFile(string filepath)
        {
            try
            {
                FileInfo file = new FileInfo(filepath);
                if (file.Exists)
                {
                    Response.ClearContent();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);

                    FileStream sourceFile = new FileStream(file.FullName, FileMode.Open);
                    long FileSize;
                    FileSize = sourceFile.Length;
                    byte[] getContent = new byte[(int)FileSize];
                    sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    sourceFile.Close();
                    Response.BinaryWrite(getContent);
                }
            }
            catch { }
            Response.End();
        }

        /// <summary>
        /// Download Button Click Event Created PDF File Download
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgDownload_Click(object sender, ImageClickEventArgs e)
        {
            if (ltrorderNumber.Text.ToString() != "")
            {
                if (File.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathInvoice").ToString() + "Invoice_" + ltrorderNumber.Text.ToString() + ".pdf").ToString()))
                {

                }
                else
                {
                    CreatePDFFile(Convert.ToInt32(ltrorderNumber.Text.ToString()));
                }

                DownloadFile(Server.MapPath(AppLogic.AppConfigs("ImagePathInvoice").ToString() + "Invoice_" + ltrorderNumber.Text.ToString() + ".pdf").ToString());
            }
        }

        /// <summary>
        /// Function for Get Google ECommerce Tracking
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <returns>returns Tracking Data</returns>
        //public static String GetGoogleECommerceTracking(Int32 OrderNumber)
        //{
        //    System.Text.StringBuilder tmpS = new System.Text.StringBuilder(5000);
        //    try
        //    {
        //        OrderComponent objOrder = new OrderComponent();
        //        DataSet DsScriptOrder = objOrder.GetOrderDetailsByOrderID(OrderNumber);
        //        if (DsScriptOrder != null && DsScriptOrder.Tables.Count > 0 && DsScriptOrder.Tables[0].Rows.Count > 0)
        //        {
        //            tmpS.Append("<script type=\"text/javascript\">");
        //            tmpS.Append("var gaJsHost = ((\"https:\" == document.location.protocol) ? \"https://ssl.\" : \"http://www.\");");
        //            tmpS.Append("document.write(unescape(\"%3Cscript src='\" + gaJsHost + \"google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E\"));");
        //            tmpS.Append("</script>");

        //            tmpS.Append("<script type=\"text/javascript\">\n");
        //            tmpS.Append("var pageTracker = _gat._getTracker(\"" + AppLogic.AppConfigs("GoogleOrderTrackingID") + "\");\n");
        //            tmpS.Append("pageTracker._initData();\n");
        //            tmpS.Append("pageTracker._trackPageview();\n");

        //            StateComponent objState = new StateComponent();
        //            CountryComponent objCountry = new CountryComponent();
        //            tmpS.Append("pageTracker._addTrans(\n");
        //            tmpS.Append(String.Format(" {0},\n{1},\n{2},\n{3},\n{4},\n{5},\n{6},\n{7}",
        //                "\"" + OrderNumber + "\"",
        //                "\"" + AppLogic.AppConfigs("StoreName") + "\"",
        //                "\"" + CommonOperations.CurrencyStringForGatewayWithoutExchangeRate(Convert.ToDecimal(DsScriptOrder.Tables[0].Rows[0]["OrderTotal"])) + "\"",
        //                "\"" + CommonOperations.CurrencyStringForGatewayWithoutExchangeRate(Convert.ToDecimal(DsScriptOrder.Tables[0].Rows[0]["OrderTax"])) + "\"",
        //                "\"" + CommonOperations.CurrencyStringForGatewayWithoutExchangeRate(Convert.ToDecimal(DsScriptOrder.Tables[0].Rows[0]["OrderShippingCosts"])) + "\"",
        //                "\"" + Convert.ToString(DsScriptOrder.Tables[0].Rows[0]["ShippingCity"]) + "\"",
        //                "\"" + objState.GetStateCodeByName(Convert.ToString(DsScriptOrder.Tables[0].Rows[0]["ShippingState"]).Replace("'", "''")) + "\"",
        //                "\"" + objCountry.GetCountryThreeLetterISOCodeByName(Convert.ToString(DsScriptOrder.Tables[0].Rows[0]["ShippingCountry"])) + "\""
        //                ));
        //            tmpS.Append(");");

        //            DataSet dsScriptItems = objOrder.GetProductList(Convert.ToInt32(DsScriptOrder.Tables[0].Rows[0]["ShoppingCardID"]));
        //            if (dsScriptItems != null && dsScriptItems.Tables.Count > 0 && dsScriptItems.Tables[0].Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dsScriptItems.Tables[0].Rows.Count; i++)
        //                {
        //                    int ProductID = 0;
        //                    Int32.TryParse(dsScriptItems.Tables[0].Rows[i]["RefProductID"].ToString().Trim(), out ProductID);
        //                    CategoryComponent objCategory = new CategoryComponent();
        //                    string CategoryName = objCategory.GetCategorySENameByProductID(ProductID);
        //                    tmpS.Append("\n pageTracker._addItem(\n");
        //                    tmpS.Append(String.Format(" {0},\n{1},\n{2},\n{3},\n{4},\n{5}",
        //                        "\"" + OrderNumber + "\"",
        //                        "\"" + dsScriptItems.Tables[0].Rows[i]["SKU"].ToString().Trim() + "\"",
        //                        "\"" + CommonOperations.SetSEName(dsScriptItems.Tables[0].Rows[i]["ProductName"].ToString().Trim()) + "\"",
        //                        "\"" + CategoryName + "\"",
        //                        "\"" + CommonOperations.CurrencyStringForDBWithoutExchangeRate(Convert.ToDecimal(dsScriptItems.Tables[0].Rows[i]["Price"].ToString().Trim())) + "\"",
        //                        "\"" + dsScriptItems.Tables[0].Rows[i]["Quantity"].ToString().Trim() + "\""
        //                        ));
        //                    tmpS.Append(");\n");
        //                }
        //            }
        //            tmpS.Append("pageTracker._trackTrans();\n");
        //            tmpS.Append("</script>");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonComponent.ErrorLog("OrderReceived.aspx.cs", ex.Message, ex.StackTrace);
        //    }
        //    return tmpS.ToString();
        //}

        /// <summary>
        /// IS the gift card Exists into cart.
        /// </summary>
        /// <param name="ShoppingCartID">int ShoppingCartID</param>
        /// <returns>Returns True if Exists else false</returns>
        public Boolean ISGiftCardAddIntoCart(Int32 ShoppingCartID)
        {
            String Query = "SELECT count(ISNULL(tb_OrderedShoppingCartItems.RefProductID,0)) " +
                            " FROM tb_OrderedShoppingCartItems INNER JOIN " +
                            " tb_GiftCardProduct ON tb_OrderedShoppingCartItems.RefProductID = tb_GiftCardProduct.ProductID  " +
                            " WHERE tb_OrderedShoppingCartItems.OrderedShoppingCartID =" + ShoppingCartID + " " +
                            " And tb_GiftCardProduct.StoreID=" + AppLogic.AppConfigs("StoreId").ToString() + "";
            Int32 Count = Convert.ToInt16(CommonComponent.GetScalarCommonData(Query));
            if (Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Sends the Gift Certificate
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="ShoppingCartID">int ShoppingCartID</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="FirstName">String FirstName</param>
        /// <param name="LastName">String LastName</param>
        /// <param name="CustomerEmail">String CustomerEmail</param>
        /// <param name="UserHostAddress">String UserHostAddress</param>
        /// <param name="LogoImagePath">String LogoImagePath</param>
        /// <param name="Mytable">Dataset Mytable</param>
        public static void SendGiftCertificate(int OrderNumber, int ShoppingCartID, int CustomerID, string FirstName, string LastName, string CustomerEmail, string UserHostAddress, string LogoImagePath, DataSet Mytable)
        {
            String Query = "SELECT tb_OrderedShoppingCartItems.RefProductID as ProductID,tb_OrderedShoppingCartItems.Price,tb_OrderedShoppingCartItems.Quantity,ISNULL(dbo.tb_OrderedShoppingCartItems.VariantNames,'') AS VariantNames,ISNULL(dbo.tb_OrderedShoppingCartItems.VariantValues,'') AS VariantValues " +
                            " FROM tb_OrderedShoppingCartItems INNER JOIN  " +
                            " tb_GiftCardProduct ON tb_OrderedShoppingCartItems.RefProductID= tb_GiftCardProduct.ProductID  " +
                            " WHERE tb_OrderedShoppingCartItems.OrderedShoppingCartID =" + ShoppingCartID + "" +
                            " And tb_GiftCardProduct.StoreID=" + AppLogic.AppConfigs("StoreId").ToString();

            ProductComponent ObjProduct = new ProductComponent();
            DataSet DsProduct = new DataSet();
            DsProduct = CommonComponent.GetCommonDataSet(Query);

            if (DsProduct != null && DsProduct.Tables[0].Rows.Count > 0)
            {
                CustomerComponent objCustomer = new CustomerComponent();
                DataSet dsCreateAccount = new DataSet();
                dsCreateAccount = objCustomer.GetEmailTamplate("GiftCertificate", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                for (int i = 0; i < DsProduct.Tables[0].Rows.Count; i++)
                {
                    string MailTo = "";
                    String GiftCardNumber = "";
                    if (Mytable != null)
                    {
                        String[] strEmailID = Convert.ToString(DsProduct.Tables[0].Rows[i]["VariantValues"]).Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        foreach (DataRow dr in Mytable.Tables[0].Select("productId=" + Convert.ToString(DsProduct.Tables[0].Rows[i]["ProductID"]) + " and EmailTo='" + Convert.ToString(strEmailID[1].ToString()) + "'"))
                        {
                            GiftCardNumber = "";
                            if (dr != null)
                            {
                                MailTo = Convert.ToString(dr["EmailTo"].ToString().Trim());
                                for (int k = 0; k < Convert.ToInt32(DsProduct.Tables[0].Rows[i]["Quantity"]); k++)
                                {
                                    GiftCardNumber += ObjProduct.AddGiftCard(CustomerID, Convert.ToInt32(DsProduct.Tables[0].Rows[i]["ProductID"].ToString()), OrderNumber, CustomerEmail, Convert.ToString(dr["EmailTo"].ToString()), dr["EmailName"].ToString(), dr["EmailMessage"].ToString(), Convert.ToInt32(AppLogic.AppConfigs("StoreId"))) + ",";
                                }
                                if (GiftCardNumber.LastIndexOf(",") > -1)
                                {
                                    GiftCardNumber = GiftCardNumber.Substring(0, GiftCardNumber.Length - 1);
                                }

                                if (dsCreateAccount != null && dsCreateAccount.Tables.Count > 0 && dsCreateAccount.Tables[0].Rows.Count > 0)
                                {
                                    String strBody = "";
                                    String strSubject = "";
                                    strBody = dsCreateAccount.Tables[0].Rows[0]["EmailBody"].ToString();
                                    strSubject = dsCreateAccount.Tables[0].Rows[0]["Subject"].ToString();

                                    strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                                    strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                                    strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                                    strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                                    strBody = Regex.Replace(strBody, "###YOURNAME###", FirstName.ToString() + ' ' + LastName.ToString(), RegexOptions.IgnoreCase);
                                    strBody = Regex.Replace(strBody, "###SERIALNO###", GiftCardNumber.ToString(), RegexOptions.IgnoreCase);
                                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                                    strBody = Regex.Replace(strBody, "###ACTUALGIFTAMOUNT###", Convert.ToDecimal(DsProduct.Tables[0].Rows[i]["Price"]).ToString("C2"), RegexOptions.IgnoreCase);

                                    Double GAmount = Convert.ToDouble(Convert.ToDouble(DsProduct.Tables[0].Rows[i]["Quantity"]) * Convert.ToDouble(DsProduct.Tables[0].Rows[i]["Price"].ToString()));
                                    strBody = Regex.Replace(strBody, "###GIFTAMOUNT###", "$" + String.Format("{0:f}", GAmount), RegexOptions.IgnoreCase);
                                    strBody = Regex.Replace(strBody, "###GIFTMESSAGE###", Convert.ToString(dr["EmailMessage"].ToString().Trim()), RegexOptions.IgnoreCase);

                                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                                    CommonOperations.SendMail(MailTo, strSubject.ToString(), strBody.ToString(), UserHostAddress.ToString(), true, av);
                                }
                            }
                            else
                            {
                                MailTo = CustomerEmail.Trim();
                                GiftCardNumber = ObjProduct.AddGiftCard(CustomerID, Convert.ToInt32(DsProduct.Tables[0].Rows[i]["ProductID"].ToString()), OrderNumber, CustomerEmail, CustomerEmail, "", "", Convert.ToInt32(AppLogic.AppConfigs("StoreId")));
                            }
                        }
                    }
                    else
                    {
                        MailTo = CustomerEmail.Trim();
                        GiftCardNumber = ObjProduct.AddGiftCard(CustomerID, Convert.ToInt32(DsProduct.Tables[0].Rows[i]["ProductID"].ToString()), OrderNumber, CustomerEmail, CustomerEmail, "", "", Convert.ToInt32(AppLogic.AppConfigs("StoreId")));
                    }
                }
            }
        }
        public String GetGoogleEComTracking(DataSet objOrder, Int32 OrderNumber)
        {

            DataSet DsScriptOrder = new DataSet();
            DsScriptOrder = objOrder;
            string strStorename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Configvalue FROM tb_AppConfIg WHERE Storeid=1 and ConfigName='StoreName' and Isnull(Deleted,0)=0 "));
            try
            {


                StringBuilder tmpS = new StringBuilder(5000);
                tmpS.Append("<script>");

                tmpS.Append("(function (i, s, o, g, r, a, m) {");
                tmpS.Append("i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {");
                tmpS.Append("(i[r].q = i[r].q || []).push(arguments)");
                tmpS.Append("}, i[r].l = 1 * new Date(); a = s.createElement(o),");
                tmpS.Append("m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)");
                tmpS.Append("})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');");
                tmpS.Append("ga('create', 'UA-2756708-1', 'halfpricedrapes.com');");
                tmpS.Append("ga('send', 'pageview');");
                tmpS.Append("ga('require', 'ecommerce', 'ecommerce.js');\n");

                //string treelettercode = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ThreeLetterISOCode FROM tb_Country WHERE Name='" + DsScriptOrder.Tables[0].Rows[0]["ShippingCountry"].ToString().Replace("'", "''") + "' and Isnull(Deleted,0)=0"));



                tmpS.Append("ga('ecommerce:addTransaction', {\n"); ///UA-2756708-1
                tmpS.Append("id: '" + OrderNumber.ToString() + "',\n");
                tmpS.Append("affiliation: 'Half Price Drapes',\n");
                tmpS.Append("revenue: '" + String.Format("{0:0.00}", Convert.ToDecimal(DsScriptOrder.Tables[0].Rows[0]["OrderTotal"].ToString())) + "',\n");
                tmpS.Append("shipping: '" + String.Format("{0:0.00}", Convert.ToDecimal(DsScriptOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString())) + "',\n");
                tmpS.Append("tax: '" + String.Format("{0:0.00}", Convert.ToDecimal(DsScriptOrder.Tables[0].Rows[0]["OrderTax"].ToString())) + "'});\n");

                DataSet dsScriptItems = new DataSet();
                dsScriptItems = CommonComponent.GetCommonDataSet("SELECT * FROM tb_OrderedShoppingCartItems WHERE OrderedShoppingCartID=" + DsScriptOrder.Tables[0].Rows[0]["ShoppingCardID"].ToString() + "");
                if (dsScriptItems != null && dsScriptItems.Tables.Count > 0 && dsScriptItems.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsScriptItems.Tables[0].Rows.Count; i++)
                    {
                        int ProductID = 0;
                        Int32.TryParse(dsScriptItems.Tables[0].Rows[i]["RefProductID"].ToString().Trim(), out ProductID);
                        string CategoryName = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Name FROM tb_category WHERE CategoryID in (SELECT CategoryId FROM tb_productcategory WHERE ProductId=" + dsScriptItems.Tables[0].Rows[i]["RefProductID"].ToString() + ")")); //ObjCategory.GetCategorySENameByProductID(ProductID);
                        tmpS.Append("ga('ecommerce:addItem', {\n");
                        tmpS.Append("id: '" + OrderNumber.ToString() + "',\n");
                        tmpS.Append("sku: '" + dsScriptItems.Tables[0].Rows[i]["SKU"].ToString().Trim() + "',\n");
                        tmpS.Append("name: '" + dsScriptItems.Tables[0].Rows[i]["ProductName"].ToString() + "',\n");
                        tmpS.Append("category: '" + CategoryName.ToString() + "',\n");
                        tmpS.Append("price: '" + String.Format("{0:0.00}", Convert.ToDecimal(dsScriptItems.Tables[0].Rows[i]["Price"].ToString().Trim())) + "',\n");
                        tmpS.Append("quantity: '" + dsScriptItems.Tables[0].Rows[i]["Quantity"].ToString().Trim() + "'});\n");

                    }
                }

                tmpS.Append("ga('ecommerce:send');\n");
                tmpS.Append("</script>");
                return tmpS.ToString();
            }
            catch (Exception ex)
            {

                return String.Empty;
            }
        }
    }
}