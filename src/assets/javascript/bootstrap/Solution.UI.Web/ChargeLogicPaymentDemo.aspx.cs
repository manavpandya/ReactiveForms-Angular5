using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Solution.UI.Web
{
    public partial class ChargeLogicPaymentDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(true);
            if (!IsPostBack)
            {
                // txtAPIKey.Text = "qz6D+vobm4lxQF9E";
                txtAPIKey.Attributes.Add("value", "qz6D+vobm4lxQF9E");
                Guid obj = Guid.NewGuid();
                txtConfirmationID.Text =  obj.ToString();
                //iframeId.Attributes.Add("src", "https://connect.chargelogic.com/?HostedPaymentID=7191DFCA-3421-4A8A-8DF1-92A033D2946E");
                
            }
        }
        private void GetPyamentId()
        {
            ChargelogicPayment.EFT_API_2 obj = new ChargelogicPayment.EFT_API_2();
            try
            {

                ChargelogicPayment.Credential objCredential = new ChargelogicPayment.Credential();
                obj.Credentials = new System.Net.NetworkCredential(txtStoreno.Text.ToString(), txtAPIKey.Text.ToString(), "CHARGELOGIC");
                //Credential

                objCredential.StoreNo = txtStoreno.Text.ToString();
                objCredential.APIKey = txtAPIKey.Text.ToString();
                objCredential.ApplicationNo = txtApplicationNo.Text.ToString();
                objCredential.ApplicationVersion = txtApplicationVersion.Text.ToString();

                //Transaction,
                ChargelogicPayment.Transaction objTransaction = new ChargelogicPayment.Transaction();
                objTransaction.Currency = txtCurrency.Text.ToString();
                objTransaction.Amount = txtAmount.Text.ToString();
                objTransaction.ExternalReferenceNumber = txtExternalReferenceNumber.Text.ToString();
                Guid objtt = Guid.NewGuid();
                string strobj = objtt.ToString();
                objTransaction.ConfirmationID = txtConfirmationID.Text.ToString();




                //Address
                ChargelogicPayment.Address objbillAddress = new ChargelogicPayment.Address();
                objbillAddress.Name = txtNamebill.Text.ToString();
                objbillAddress.StreetAddress = txtStreetAddressbill.Text.ToString();
                objbillAddress.City = txtCitybill.Text.ToString();
                objbillAddress.State = txtStatebill.Text.ToString();
                objbillAddress.PostCode = txtPostCodebill.Text.ToString();
                objbillAddress.Country = txtCountrybill.Text.ToString();
                objbillAddress.PhoneNumber = txtPhoneNumberbill.Text.ToString();
                objbillAddress.Email = txtEmailbill.Text.ToString();
                //Address

                ChargelogicPayment.Address objshippAddress = new ChargelogicPayment.Address();

                objshippAddress.Name = txtNameshipp.Text.ToString();
                objshippAddress.StreetAddress = txtStreetAddressshipp.Text.ToString();
                objshippAddress.City = txtCityshipp.Text.ToString();
                objshippAddress.State = txtStateshipp.Text.ToString();
                objshippAddress.PostCode = txtPostCodeshipp.Text.ToString();
                objshippAddress.Country = txtCountryshipp.Text.ToString();
                objshippAddress.PhoneNumber = txtPhoneNumbershipp.Text.ToString();
                objshippAddress.Email = txtEmailshipp.Text.ToString();

                //HostedPayment
                ChargelogicPayment.HostedPayment objHostedPayment = new ChargelogicPayment.HostedPayment();
                objHostedPayment.RequireCVV = txtRequireCVV.Text.ToString();
                objHostedPayment.ReturnURL = txtReturnURL.Text.ToString();
                objHostedPayment.Language = txtLanguage.Text.ToString();
                objHostedPayment.ConfirmationID = txtConfirmationID.Text.ToString();
                objHostedPayment.MerchantResourceURL = "https://www.halfpricedrapes.com/ChargeLogicMerchantResource.html";
                objHostedPayment.Embedded ="Yes";
                
                ChargelogicPayment.Response objresponse = new ChargelogicPayment.Response();
                obj.SetupHostedOrderCompleted += obj_SetupHostedOrderCompleted;
                string Hostepaymentid = obj.SetupHostedOrder(objCredential, objTransaction, objbillAddress, objshippAddress, objHostedPayment);

                //obj.SetupHostedOrder(objCredential, objTransaction, objbillAddress, objshippAddress, objHostedPayment);
               // iframeId.Attributes.Add("src", "https://connect.chargelogic.com/?HostedPaymentID=" + Hostepaymentid + "");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "iframeload", "iframerealod('" + Hostepaymentid + "');", true);

            }
            catch (Exception ex)
            {
                
                Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());
            }


        }

        void obj_SetupHostedOrderCompleted(object sender, ChargelogicPayment.SetupHostedOrderCompletedEventArgs e)
        {

        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            GetPyamentId();
        }
    }
}