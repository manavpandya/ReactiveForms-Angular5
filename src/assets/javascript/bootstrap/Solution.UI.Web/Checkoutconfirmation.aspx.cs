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
    public partial class Checkoutconfirmation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ONo"] != null)
                {
                    GetPyamentId(Convert.ToInt32(Session["ONo"].ToString()));
                    checkouttablecart.InnerHtml = Session["cartitemall"].ToString();
                }
                else
                {
                    Response.Redirect("/index.aspx");
                }

            }
        }
        #region ChargeLogic
        private void GetPyamentId(int ExOrderNumber)
        {
            ChargelogicPayment.EFT_API_2 obj = new ChargelogicPayment.EFT_API_2();
            try
            {

                Guid objGuid = Guid.NewGuid();
                string stGuid = objGuid.ToString();

                DataSet dsOrderdetail = new DataSet();
                dsOrderdetail = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Order WHERE Ordernumber=" + ExOrderNumber + "");
                if (dsOrderdetail != null && dsOrderdetail.Tables.Count > 0 && dsOrderdetail.Tables[0].Rows.Count > 0)
                {

                    txtbilladdress.Text = txtbilladdress.Text + dsOrderdetail.Tables[0].Rows[0]["BillingFirstName"].ToString() + " " + dsOrderdetail.Tables[0].Rows[0]["BillingLastName"].ToString() + "<br/>";
                    txtbilladdress.Text = txtbilladdress.Text + dsOrderdetail.Tables[0].Rows[0]["BillingAddress1"].ToString() + "<br/>";
                    if (!string.IsNullOrEmpty(dsOrderdetail.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                    {
                        txtbilladdress.Text = txtbilladdress.Text + dsOrderdetail.Tables[0].Rows[0]["BillingAddress2"].ToString() + "<br/>";
                    }
                    txtbilladdress.Text = txtbilladdress.Text + dsOrderdetail.Tables[0].Rows[0]["BillingCity"].ToString() + ", " + dsOrderdetail.Tables[0].Rows[0]["BillingState"].ToString() + " " + dsOrderdetail.Tables[0].Rows[0]["BillingZip"].ToString() + "<br/>";
                    txtbilladdress.Text = txtbilladdress.Text + dsOrderdetail.Tables[0].Rows[0]["BillingCountry"].ToString() + "<br/>";
                    txtbilladdress.Text = txtbilladdress.Text + dsOrderdetail.Tables[0].Rows[0]["BillingPhone"].ToString();
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
                    //if (ViewState["OrderTotal"] != null)
                    //{
                    objTransaction.Amount = String.Format("{0:0.00}", Convert.ToDecimal(dsOrderdetail.Tables[0].Rows[0]["OrderTotal"].ToString()));
                    //}

                    objTransaction.ExternalReferenceNumber = ExOrderNumber.ToString();

                    objTransaction.ConfirmationID = stGuid.ToString();




                    //Address
                    ChargelogicPayment.Address objbillAddress = new ChargelogicPayment.Address();
                    objbillAddress.Name = dsOrderdetail.Tables[0].Rows[0]["BillingFirstName"].ToString() + " " + dsOrderdetail.Tables[0].Rows[0]["BillingLastName"].ToString();

                    objbillAddress.StreetAddress = dsOrderdetail.Tables[0].Rows[0]["BillingAddress1"].ToString();

                    objbillAddress.StreetAddress2 = dsOrderdetail.Tables[0].Rows[0]["BillingAddress2"].ToString();
                    objbillAddress.City = dsOrderdetail.Tables[0].Rows[0]["BillingCity"].ToString();

                    objbillAddress.State = dsOrderdetail.Tables[0].Rows[0]["BillingState"].ToString();


                    objbillAddress.PostCode = dsOrderdetail.Tables[0].Rows[0]["BillingZip"].ToString();

                    string strCode = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(TwoLetterISOCode,'') FROM tb_Country WHERE name='" + dsOrderdetail.Tables[0].Rows[0]["BillingCountry"].ToString().Replace("'", "''") + "'"));
                    objbillAddress.Country = strCode.ToString();

                    objbillAddress.PhoneNumber = dsOrderdetail.Tables[0].Rows[0]["BillingPhone"].ToString();
                    objbillAddress.Email = dsOrderdetail.Tables[0].Rows[0]["Email"].ToString();
                    //Address

                    ChargelogicPayment.Address objshippAddress = new ChargelogicPayment.Address();

                    txtshipaddress.Text = txtshipaddress.Text + dsOrderdetail.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + dsOrderdetail.Tables[0].Rows[0]["ShippingLastName"].ToString() + " <br/>";
                    txtshipaddress.Text = txtshipaddress.Text + dsOrderdetail.Tables[0].Rows[0]["ShippingAddress1"].ToString() + " <br />";

                    if (!string.IsNullOrEmpty(dsOrderdetail.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                    {
                        txtshipaddress.Text = txtshipaddress.Text + dsOrderdetail.Tables[0].Rows[0]["ShippingAddress2"].ToString() + "<br />";
                    }
                    txtshipaddress.Text = txtshipaddress.Text + dsOrderdetail.Tables[0].Rows[0]["ShippingCity"].ToString() + ", " + dsOrderdetail.Tables[0].Rows[0]["ShippingState"].ToString() + " " + dsOrderdetail.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br />";

                    txtshipaddress.Text = txtshipaddress.Text + dsOrderdetail.Tables[0].Rows[0]["ShippingCountry"].ToString() + "<br />";
                    txtshipaddress.Text = txtshipaddress.Text + dsOrderdetail.Tables[0].Rows[0]["ShippingPhone"].ToString() + "";

                    objshippAddress.Name = dsOrderdetail.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + dsOrderdetail.Tables[0].Rows[0]["ShippingLastName"].ToString();
                    objshippAddress.StreetAddress = dsOrderdetail.Tables[0].Rows[0]["ShippingAddress1"].ToString();
                    objbillAddress.StreetAddress2 = dsOrderdetail.Tables[0].Rows[0]["ShippingAddress2"].ToString();
                    objshippAddress.City = dsOrderdetail.Tables[0].Rows[0]["ShippingCity"].ToString();

                    objshippAddress.State = dsOrderdetail.Tables[0].Rows[0]["ShippingState"].ToString();


                    objshippAddress.PostCode = dsOrderdetail.Tables[0].Rows[0]["ShippingZip"].ToString();
                    strCode = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(TwoLetterISOCode,'') FROM tb_Country WHERE name='" + dsOrderdetail.Tables[0].Rows[0]["ShippingCountry"].ToString().Replace("'", "''") + "'"));
                    objshippAddress.Country = strCode.ToString();
                    objshippAddress.PhoneNumber = dsOrderdetail.Tables[0].Rows[0]["ShippingPhone"].ToString();
                    objshippAddress.Email = dsOrderdetail.Tables[0].Rows[0]["ShippingEmail"].ToString();

                    //HostedPayment
                    ChargelogicPayment.HostedPayment objHostedPayment = new ChargelogicPayment.HostedPayment();
                    objHostedPayment.RequireCVV = "Yes";
                     objHostedPayment.ReturnURL = "https://www.halfpricedrapes.com/chargelogiccheckout.aspx";
                   // objHostedPayment.ReturnURL = "https://" + Request.Url.Host.ToString() + "/chargelogiccheckout.aspx";
                    objHostedPayment.Language = "ENU";
                    objHostedPayment.ConfirmationID = stGuid.ToString();
                    objHostedPayment.MerchantResourceURL = "https://www.halfpricedrapes.com/ChargeLogicMerchantResource.html";
                   // objHostedPayment.MerchantResourceURL = "https://" + Request.Url.Host.ToString() + "/ChargeLogicMerchantResource.html";
                    objHostedPayment.Embedded = "Yes";
                    objHostedPayment.FieldLabelFontColor = "393939";
                    ChargelogicPayment.Response objresponse = new ChargelogicPayment.Response();

                    string Hostepaymentid = obj.SetupHostedOrder(objCredential, objTransaction, objbillAddress, objshippAddress, objHostedPayment);

                    //obj.SetupHostedOrder(objCredential, objTransaction, objbillAddress, objshippAddress, objHostedPayment);
                    // iframeId.Attributes.Add("src", "https://connect.chargelogic.com/?HostedPaymentID=" + Hostepaymentid + "");
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "iframeload", "iframerealod('" + Hostepaymentid + "');", true);
                    Random rd = new Random();
                    ViewState["Hostepaymentid"] = Hostepaymentid;
                    // hdnhostedpaymentid.Value = Hostepaymentid.ToString();
                    //if (!IsPostBack)
                    //{
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "iframeload" + rd.Next(1000).ToString(), "iframerealod('" + Hostepaymentid + "'," + rd.Next(1000).ToString() + ");", true);
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "iframeload" + rd.Next(1000).ToString(), "iframerealod('" + Hostepaymentid + "'," + rd.Next(1000).ToString() + ");", true);
                    //}

                }

            }
            catch (Exception ex)
            {

                Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());
            }


        }
        protected void imgtempbutton_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["ONo"] != null)
            {
                txtshipaddress.Text = "";
                txtbilladdress.Text = "";
                GetPyamentId(Convert.ToInt32(Session["ONo"].ToString()));
                checkouttablecart.InnerHtml = Session["cartitemall"].ToString();
            }
        }
        #endregion
    }
}