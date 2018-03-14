using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components.Common;
using System.Net.Mail;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text.RegularExpressions;
using System.Text;


namespace Solution.UI.Web
{
    public partial class Contactuspopup : BasePage
    {
        CustomerComponent objCustomer = null;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            if (Request.QueryString["result"] != null && !IsPostBack && Request.UrlReferrer != null
            && Request.UrlReferrer.AbsolutePath.ToLower() == "/contactus.aspx" && Request.QueryString["result"].ToString().ToLower() == "success")
            {
                lblMsg.Text = "E-Mail has been sent successfully.<br />Our Customer Service Representative will respond to you by phone or E-Mail within 24 business hours.";
            }
            if (!IsPostBack)
            {
                ltbrTitle.Text = "Contact Us";
                ltTitle.Text = "Contact Us";
                FillCountry();
                GetCustomerService();
                txtname.Focus();
                Page.MaintainScrollPositionOnPostBack = true;
                BindState(Convert.ToInt32(ddlcountry.SelectedValue.ToString()));
                BindSubject();
            }
        }

        public void BindSubject()
        {
            drpSubject.Items.Clear();
            DataSet dsSubject = CommonComponent.GetCommonDataSet("Select * from tb_ContactEmail where isnull(Active,0)=1 and isnull(Deleted,0)=0");
            if (dsSubject != null && dsSubject.Tables.Count > 0 && dsSubject.Tables[0].Rows.Count > 0)
            {
                drpSubject.DataSource = dsSubject;
                drpSubject.DataTextField = "Subject";
                drpSubject.DataValueField = "EmailID";

                drpSubject.Items.Add(new ListItem("Select Subject", ""));
                drpSubject.DataBind();
            }


        }
        /// <summary>
        /// Bind both Country Drop down list
        /// </summary>
        public void FillCountry()
        {
            ddlcountry.Items.Clear();

            CountryComponent objCountry = new CountryComponent();
            DataSet dscountry = new DataSet();
            dscountry = objCountry.GetAllCountries();

            if (dscountry != null && dscountry.Tables.Count > 0 && dscountry.Tables[0].Rows.Count > 0)
            {
                ddlcountry.DataSource = dscountry.Tables[0];
                ddlcountry.DataTextField = "Name";
                ddlcountry.DataValueField = "CountryID";
                ddlcountry.DataBind();
                ddlcountry.Items.Insert(0, new ListItem("Select Country", "0"));
            }
            else
            {
                ddlcountry.DataSource = null;
                ddlcountry.DataBind();
                ddlcountry.DataSource = null;
                ddlcountry.DataBind();
            }

            if (ddlcountry.Items.FindByText("United States") != null)
            {
                ddlcountry.Items.FindByText("United States").Selected = true;
            }
            ddlcountry_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlcountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtother.Text = "";
            ddlstate.Items.Clear();
            divother.Style.Clear();
            divother.Style.Add("display", "none");
            //divother.Style.Add("Width", "210px; !important");
            //divother.Style.Add("float", "left");
            if (ddlstate.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlcountry.SelectedValue.ToString()));
                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlstate.DataSource = dsState.Tables[0];
                    ddlstate.DataTextField = "Name";
                    ddlstate.DataValueField = "StateID";
                    ddlstate.DataBind();
                    ddlstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    if (ddlcountry.SelectedValue.ToString() != "1")
                    {
                        ddlstate.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    }
                    ClientScript.RegisterStartupScript(typeof(Page), "OtherVisible", "SetOthersVisible(false);", true);
                }
                else
                {
                    ddlstate.DataSource = null;
                    ddlstate.DataBind();
                    ddlstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlstate.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlstate.SelectedIndex = 0;
                }

            }
            else
            {
                ddlstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlstate.Items.Insert(1, new ListItem("Other", "-11"));
                ddlstate.SelectedIndex = 0;
            }
            ClientScript.RegisterStartupScript(typeof(Page), "OtherVisible03", "MakeOthersVisible();window.parent.centerPopupmaster(); window.parent.loadPopupmaster();", true);
        }

        /// <summary>
        /// Binds the State
        /// </summary>
        /// <param name="CountryID">int CountryID</param>
        public void BindState(int CountryID)
        {
            ddlstate.Items.Clear();

            StateComponent objState = new StateComponent();
            DataSet DsState = new DataSet();
            DsState = objState.GetAllState(Convert.ToInt32(ddlcountry.SelectedValue.ToString()));

            if (DsState != null && DsState.Tables[0].Rows.Count > 0)
            {
                ddlstate.DataSource = DsState;
                ddlstate.DataTextField = "Name";
                ddlstate.DataValueField = "StateID";
                ddlstate.DataBind();
            }
            else
            {
                ddlstate.DataSource = null;
                ddlstate.DataBind();
            }
            ListItem lt1 = new ListItem("Select State/Province", "0");
            ddlstate.Items.Insert(0, lt1);
            if (ddlcountry.SelectedValue.ToString() != "1")
            {
                ListItem lt = new ListItem("Other", "-11");
                ddlstate.Items.Insert(1, lt);
            }
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (txtShowcode.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter Shown Code.');", true);
                txtShowcode.Focus();
                txtShowcode.Text = "";
                return;
            }
            else if (txtShowcode.Text != Convert.ToString(Session["CaptchaImageText"]))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter Correct Shown Code.');", true);
                txtShowcode.Text = "";
                txtShowcode.Focus();

                return;
            }

            tb_ContactUs tb_ContactUs = new tb_ContactUs();
            ContactUsComponent objContactus = new ContactUsComponent();
            tb_ContactUs.Name = txtname.Text + " " + txtlastname.Text;
            tb_ContactUs.Email = txtEmail.Text;
            tb_ContactUs.City = txtCity.Text;
            tb_ContactUs.Address = txtAddress.Text.ToString();
            tb_ContactUs.Country = ddlcountry.SelectedItem.ToString();
            if (ddlstate.SelectedValue == "-11")
                tb_ContactUs.State = txtother.Text;
            else
                tb_ContactUs.State = ddlstate.SelectedItem.Text;

            //tb_ContactUs.ZipCode = txtzipcode.Text;
            tb_ContactUs.PhoneNumber = txtPhone.Text;
            tb_ContactUs.Message = txtinformation.Text;
            tb_ContactUs.ContactDate = DateTime.Now;
            tb_ContactUs.SubjectStatus = drpSubject.SelectedItem.Text;
            int isadded = objContactus.InsertContactUsDetail(tb_ContactUs);
            if (isadded == 0)
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in adding contact us details, please try again...');", true);
            else
            {
                string MailAddress = AppLogic.AppConfigs("ContactMail_ToAddress");
                if (!String.IsNullOrEmpty(drpSubject.SelectedValue.ToString()))
                {
                    MailAddress = drpSubject.SelectedValue.ToString().Trim();
                }

                if (MailAddress != string.Empty)
                {
                    SendMail(MailAddress);
                    SendMailToCustomer(txtEmail.Text, isadded);
                }
                //Response.Redirect("~/contactus.aspx?result=success");
                //window.parent.centerPopupmaster(); window.parent.loadPopupmaster();
                ClientScript.RegisterStartupScript(typeof(Page), "OtherVisible03load", "alert('Message Sent Succesfully!');window.location.href=window.location.href;window.parent.centerPopupmaster(); window.parent.loadPopupmaster();", true);
            }

        }

        #region send Mail

        /// <summary>
        /// Sends the Mail.
        /// </summary>
        /// <param name="TOAddress">string TOAddress</param>
        private void SendMail(string TOAddress)
        {
            objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();

            dsMailTemplate = objCustomer.GetEmailTamplate("ContactUsEmailForAdmin", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {

                StringBuilder sw = new StringBuilder(4000);
                sw.Append("<table width='100%' class='popup_cantain'></tr>");
                sw.Append("<tr><td  style='margin-top:7px;width:30%;'>");
                sw.Append("<b style='color:#000000;'>Name :</b></td><td  style='margin-top:10px;width:70%;'>" + txtname.Text.ToString() + "</td></tr>");
                sw.Append("<tr><td ><b style='color:#000000;'>E-Mail address :</b></td><td>  <a href='mailto:" + txtEmail.Text.ToString() + "' style='color:#000000;'>" + txtEmail.Text.ToString() + "</a></td></tr>");

                if (txtAddress.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>Address :</b></td><td > " + txtAddress.Text.Trim() + "</td></tr>");
                if (txtCity.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>City :</b></td><td > " + txtCity.Text.Trim() + "</td></tr>");
                if (ddlcountry.SelectedIndex != -11)
                    sw.Append("<tr><td><b style='color:#000000;'>Country :</b></td><td > " + ddlcountry.SelectedItem.ToString() + "</td></tr>");
                if (ddlstate.SelectedIndex != -11 && ddlstate.SelectedItem.ToString() != "Select State/Province" && ddlstate.SelectedItem.ToString() != "Other")
                    sw.Append("<tr><td><b style='color:#000000;'>State :</b> </td><td >" + ddlstate.SelectedItem.ToString() + "</td></tr>");
                else if (txtother.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>State :</b></td><td > " + txtother.Text.Trim() + "</td></tr>");
                if (txtzipcode.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>Zip :</b></td><td > " + txtzipcode.Text.Trim() + "</td></tr>");
                if (txtPhone.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>Phone :</b></td><td > " + txtPhone.Text.Trim() + "</td></tr>");
                if (txtinformation.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>Specific Information :</b></td><td style='padding-top:5px;padding-bottom:5px' > " + txtinformation.Text.ToString() + "</td></tr>");
                sw.Append("</table>");


                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###ContactUsDetails###", sw.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");

                string ContactUsCCEmailID = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Configvalue FROM tb_AppConfig WHERE Configname like '%ContactUsCCEmailID%' and storeid=1 and isnull(Deleted,0)=0"));
                if (!string.IsNullOrEmpty(ContactUsCCEmailID))
                {
                    TOAddress = TOAddress + ";" + ContactUsCCEmailID;
                }
                CommonOperations.SendMail(TOAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }

        /// <summary>
        /// Sends the Mail to Customer
        /// </summary>
        /// <param name="TOAddress">string TOAddress</param>
        /// <param name="ContactID">int ContactID</param>
        private void SendMailToCustomer(string TOAddress, Int32 ContactID)
        {
            objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();
            dsMailTemplate = objCustomer.GetEmailTamplate("ContactUsEmail", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {
                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();
                strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                strSubject = Regex.Replace(strSubject, "###YOURNAME###", txtname.Text.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);
                CommonComponent.ExecuteCommonData("UPDATE tb_ContactUs SET Subject='" + strSubject.ToString().Replace("'", "''") + "' WHERE ContactUsID=" + ContactID + "");
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMail(TOAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }

        #endregion

        private void GetCustomerService()
        {
            DataTable dtHomeContent = new DataTable();
            DataSet ds = new DataSet();

            ds = TopicComponent.GetTopicAccordingStoreID("contactusservice", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltContactusContent.Text = ds.Tables[0].Rows[0]["Description"].ToString();
            }
            else
            {
                ltContactusContent.Text = "";
            }
        }
    }
}