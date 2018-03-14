using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using MWSMerchantFulfillmentService;

namespace Solution.UI.Web.ADMIN
{
    public partial class Login : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            AppLogic.ApplicationStart();
            
            if (!IsPostBack)
            {
                //string[] strFile = System.IO.Directory.GetFiles(Server.MapPath("/"), "*_126863_*");
                  ViewState["AmazonMerchantID"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonMerchantID'"));
                    ViewState["AmazonServiceURL"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonServiceURL'"));
                    ViewState["AmazonAccessKey"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonAccessKey'"));
                    ViewState["AmazonSecretKey"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonSecretKey'"));
                    ViewState["AmazonApplicationName"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonApplicationName'"));
                    ViewState["AmazonDefaultMethod"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonDefaultMethod'"));
                 
                //Convert.ToDateTime("1/1/2018".ToString());
                btnLogin.ImageUrl = "/App_Themes/" + Page.Theme + "/images/login.png";
                btnSend.ImageUrl = "/App_Themes/" + Page.Theme + "/button/send-email.png";
                popupContactClose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel-icon.png";
                txtUserName.Focus();
            }
            Page.Form.DefaultButton = btnLogin.UniqueID;
        }

        /// <summary>
        ///  Login Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {


            //string sellerId = ViewState["AmazonMerchantID"].ToString();
            //string mwsAuthToken = "";
            //// The client application version
            //string appVersion = "1.01";
            //MWSMerchantFulfillmentService.Model.CancelShipmentResponse objlabel = new MWSMerchantFulfillmentService.Model.CancelShipmentResponse();
            //// The endpoint for region service and version (see developer guide)
            //// ex: https://mws.amazonservices.com
            //string serviceURL = ViewState["AmazonServiceURL"].ToString();
            //MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
            //config.ServiceURL = serviceURL;
            //// Set other client connection configurations here if needed
            //// Create the client itself
            //MWSMerchantFulfillmentServiceClient client = new MWSMerchantFulfillmentServiceClient(ViewState["AmazonAccessKey"].ToString(), ViewState["AmazonSecretKey"].ToString(), ViewState["AmazonApplicationName"].ToString(), appVersion, config);
            //MWSMerchantFulfillmentServiceSample obj = new MWSMerchantFulfillmentServiceSample(client);
            //try
            //{
            //    objlabel = obj.InvokeCancelShipment(sellerId, "7fc11952-3678-427c-a3b1-3c43d37c8c34");
            //    // Response.Write(objlabel.CancelShipmentResult.Shipment.Status.ToString());
            //}
            //catch (MWSMerchantFulfillmentServiceException ex)
            //{
            //    Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString() + " " + ex.StatusCode.ToString());
            //}
            string descPs = SecurityComponent.Decrypt("Z9fPISmAuYupYTgesmoNuA==");

            DataSet dsAdmin = new DataSet();
            AdminRightsComponent objAdminRight = new AdminRightsComponent();
            dsAdmin = AdminComponent.GetAdminForLogin(txtUserName.Text.Trim(), SecurityComponent.Encrypt(txtPassword.Text.Trim()));
            if (dsAdmin != null && dsAdmin.Tables.Count > 0 && dsAdmin.Tables[0].Rows.Count > 0)
            {
                Session["AdminName"] = dsAdmin.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsAdmin.Tables[0].Rows[0]["LastName"].ToString();
                Session["AdminID"] = dsAdmin.Tables[0].Rows[0]["AdminID"].ToString();
                objAdminRight.GetAllPageRightsByAdminID(Convert.ToInt32(Session["AdminID"]));
                string StrVendoremail = Convert.ToString(dsAdmin.Tables[0].Rows[0]["AdminType"].ToString());
                Session["AdminType"] = StrVendoremail;
                Session["AdminvendorId"] = Convert.ToString(dsAdmin.Tables[0].Rows[0]["VendorId"].ToString());
                //string StrVendoremail = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(email,'') as Vendoremail from tb_Vendor  Where ISNULL(email,'') ='" + txtUserName.Text.Trim().Replace("'", "''") + "' and ISNULL(Active,1)=1 and ISNULL(Deleted,0)=0"));
                if (!string.IsNullOrEmpty(StrVendoremail.ToString()) && StrVendoremail.ToString() == "2")
                {
                    Session["VendorLogin"] = "1";
                    Response.Redirect("/Admin/Products/FabricVendorPortal.aspx");

                }
                else
                {
                    Session["VendorLogin"] = null;
                    if (StrVendoremail.ToString() == "0")
                    {
                        Response.Redirect("/Admin/Dashboard.aspx");
                    }
                    else
                    {
                        Response.Redirect("/Admin/MarketingDashboard.aspx");
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Correct User Name/Password.', 'Message');});", true);
                //lblMsg.Text = "User does not exist...";
            }
        }

        /// <summary>
        ///  Send Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSend_Click(object sender, ImageClickEventArgs e)
        {
            if (txtEmail.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "", true);
                return;
            }
            else
            {
                bool istrue = SendMail();

                if (istrue)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Password has been sent on your email address.', 'Message');});", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgnotexists", "$(document).ready( function() {ShowForGotPassword();jAlert('This Email Address is not Registered Email.', 'Message','txtEmail');});", true);
                    return;
                }
            }
        }

        /// <summary>
        /// Send Mail function
        /// </summary>
        private bool SendMail()
        {
            CustomerComponent objCustomer = new CustomerComponent();
            AdminComponent objAdmin = new AdminComponent();
            tb_Admin tb_admin = objAdmin.GetPasswordByEmail(txtEmail.Text.ToString().Trim());
            bool IsSent = false;

            if (tb_admin != null)
            {
                DataSet dsMailTemplate = new DataSet();
                dsMailTemplate = objCustomer.GetEmailTamplate("Forgotpassword", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                {
                    String strBody = "";
                    String strSubject = "";
                    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                    strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###FIRSTNAME###", Convert.ToString(tb_admin.FirstName.ToString()), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###LASTNAME###", Convert.ToString(tb_admin.LastName.ToString()), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###USERNAME###", Convert.ToString(txtEmail.Text), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###PASSWORD###", SecurityComponent.Decrypt(Convert.ToString(tb_admin.Password)), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("StoreName").ToString(), RegexOptions.IgnoreCase);

                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                    CommonOperations.SendMail(txtEmail.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                    IsSent = true;
                }
            }
            return IsSent;
        }
    }
}