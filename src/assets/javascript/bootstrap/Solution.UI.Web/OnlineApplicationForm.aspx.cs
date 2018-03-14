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
using System.IO;

namespace Solution.UI.Web
{
    public partial class OnlineApplicationForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            if (Request.QueryString["result"] != null && !IsPostBack && Request.UrlReferrer != null
            && Request.UrlReferrer.AbsolutePath.ToLower() == "/onlineapplicationform.aspx" && Request.QueryString["result"].ToString().ToLower() == "success")
            {
                lblMsg.Text = "E-Mail has been sent successfully.<br />";
            }
            if (!IsPostBack)
            {
                txtCompany.Focus();
                ltbrTitle.Text = "Trade Membership Application";
                ltTitle.Text = "Trade Membership Application";
                Page.MaintainScrollPositionOnPostBack = true;
            }
        }
        private void Insert(Int32 MailID)
        {
            try
            {


                string strSql;
                strSql = "INSERT INTO tb_TradeApplication(CompanyName,Street,City,State,ZipCode,Website,FirstName1,FirstName2,FirstName3,LastName1,LastName2,LastName3,Title1,Title2,Title3,Email1,Email2," +
        "Email3,Phone1,Phone2,Phone3,Fax1,Fax2,Fax3,NoOfEmployee,ProjectType,DocumentFile,MailID) values(" +
       "'" + txtCompany.Text.ToString().Replace("'", "''") + "'" + "," +
      "'" + txtStreet.Text.ToString().Replace("'", "''") + "'" + "," +
                "'" + txtCity.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtState.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtzipcode.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtWebSite.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM1FirstName.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM2FirstName.Text.ToString().Replace("'", "''") + "'" + "," +
                  "'" + txtM3FirstName.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM1LastName.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM2LastName.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM3LastName.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM1Title.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM2Email.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM3Title.Text.ToString().Replace("'", "''") + "'" + "," +
              "'" + txtM1Email.Text.ToString().Replace("'", "''") + "'" + "," + "'" + TextBox4.Text.ToString().Replace("'", "''") + "'" + "," +
                 "'" + txtM3Email.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM1Phone.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM2Email.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM3Phone.Text.ToString().Replace("'", "''") + "'" + "," + "'" +
                 txtM1Fax.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM2Fax.Text.ToString().Replace("'", "''") + "'" + "," + "'" + txtM3Fax.Text.ToString().Replace("'", "''") + "'" + "," +

               "'" + Convert.ToInt32(txtNoOfEmployee.Text.ToString().Replace("'", "''")) + "'" + "," +
                 "'" + ddlProjectType.SelectedItem.Text + "'" + "," +
                   "'" + lblFileName.Text.ToString().Replace("'", "''") + "'" + "," + "'" + MailID + "'" +


                ")";
               
                CommonComponent.ExecuteCommonData(strSql);


            }
            catch
            {

            }

        }
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            string MailAddress = AppLogic.AppConfigs("ContactMail_ToAddress");
            if (MailAddress != string.Empty)
            {
                if (fuUplodDoc.HasFile)
                {
                    btnUpload_Click(null, null);
                }
                SendMail(MailAddress);

                lblMsg.Text = "E-Mail has been sent successfully.<br />";
            }
            txtCompany.Text = "";
            txtStreet.Text = "";
            txtCity.Text = "";
            txtzipcode.Text = "";
            txtWebSite.Text = "";
            txtM1FirstName.Text = "";
            txtM1LastName.Text = "";
            txtM1Title.Text = "";
            txtM1Email.Text = "";
            txtM1Phone.Text = "";
            ddlProjectType.SelectedValue = "0";
            txtNoOfEmployee.Text = "";
            lblFileName.Text = "";
            txtCompany.Focus();

        }

        private void SendMail(string TOAddress)
        {
            CustomerComponent objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();

            dsMailTemplate = objCustomer.GetEmailTamplate("OnlineFormMailForAdmin", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {

                StringBuilder sw = new StringBuilder(4000);
                sw.Append("<table width='100%' class='popup_cantain'></tr>");
                sw.Append("<tr><td  style='margin-top:7px;width:30%;'>");
                sw.Append("<b style='color:#000000;'>Name :</b></td><td  style='margin-top:10px;width:70%;'>" + txtM1FirstName.Text.ToString() + " " + txtM1LastName.Text.ToString() + "</td></tr>");
                sw.Append("<tr><td ><b style='color:#000000;'>E-Mail address :</b></td><td>  <a href='mailto:" + txtM1Email.Text.ToString() + "' style='color:#000000;'>" + txtM1Email.Text.ToString() + "</a></td></tr>");

                if (txtCompany.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>Company Name :</b></td><td > " + txtCompany.Text.Trim() + "</td></tr>");
                if (txtStreet.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>Street Name :</b></td><td > " + txtStreet.Text.Trim() + "</td></tr>");
                if (txtCity.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>City :</b></td><td > " + txtCity.Text.Trim() + "</td></tr>");
                if (txtzipcode.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>Zip :</b></td><td > " + txtzipcode.Text.Trim() + "</td></tr>");
                if (txtWebSite.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>Website :</b></td><td > " + txtWebSite.Text.Trim() + "</td></tr>");
                if (ddlProjectType.SelectedIndex != -11)
                    sw.Append("<tr><td><b style='color:#000000;'>Project Type :</b></td><td > " + ddlProjectType.SelectedItem.ToString() + "</td></tr>");

                if (txtNoOfEmployee.Text.Trim() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>No of Emplayees in Company :</b></td><td > " + txtNoOfEmployee.Text.Trim() + "</td></tr>");

                sw.Append("</table>");


                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###OnlineFormDetails###", sw.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");

                string ToID = "";
                ToID = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(EmailID,'') FROM tb_ContactEmail WHERE Subject='Trade Application'"));
                if (!string.IsNullOrEmpty(ToID))
                {
                    TOAddress = ToID;
                }
                Int32 mailId = 0;
                if (ViewState["DocumentFileName"] != null)
                {
                    string docPath = AppLogic.AppConfigs("OnlineApplication.DocumentPath").ToString() + "/" + ViewState["DocumentFileName"].ToString();
                    if (File.Exists(Server.MapPath(docPath)))
                    {
                        String FromID = AppLogic.AppConfigs("MailFrom");

                        mailId = Convert.ToInt32(CommonOperations.SendMailAttachment(FromID, TOAddress, "", "", strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av, Server.MapPath(docPath.ToString())));

                    }
                    else
                    {
                        mailId = Convert.ToInt32(CommonOperations.SendMail(TOAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av));
                    }
                    ViewState["DocumentFileName"] = null;
                }
                else
                {
                    mailId = Convert.ToInt32(CommonOperations.SendMail(TOAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av));
                }
                Insert(mailId);
                txtCompany.Text = "";
                txtStreet.Text = "";
                txtCity.Text = "";
                txtzipcode.Text = "";
                txtWebSite.Text = "";
                txtM1FirstName.Text = "";
                txtM1LastName.Text = "";
                txtM1Title.Text = "";
                txtM1Email.Text = "";
                txtM1Phone.Text = "";
                ddlProjectType.SelectedValue = "0";
                txtNoOfEmployee.Text = "";
                lblFileName.Text = "";
                txtCompany.Focus();
                txtState.Text = "";
                txtM1Fax.Text = "";
                txtM3FirstName.Text = "";
                txtM2FirstName.Text = "";
                txtM2LastName.Text = "";
                txtM3LastName.Text = "";
                txtM2Title.Text = "";
                txtM3Title.Text = "";
                txtM2Email.Text = "";
                txtM3Email.Text = "";
                TextBox4.Text = "";
                txtM3Phone.Text = "";
                txtM2Fax.Text = "";
                txtM3Fax.Text = "";
                imgdeletebtn.Visible = false;
                
            }
        }


        protected void imgdeletebtn_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["DocumentFileName"] = null;
                DeleteDocument(lblFileName.Text);
                lblFileName.Text = "";
                imgdeletebtn.Visible = false;
            }
            catch { }
        }
        #region  document
        /// <summary>
        ///  Upload Document Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string CustDocPathTemp = AppLogic.AppConfigs("OnlineApplication.DocumentPath");
                if (!Directory.Exists(Server.MapPath(CustDocPathTemp)))
                    Directory.CreateDirectory(Server.MapPath(CustDocPathTemp));
                if (fuUplodDoc.FileName.Length > 0)
                {

                    ViewState["DocumentFileName"] = fuUplodDoc.FileName.ToString();
                    DeleteDocument(fuUplodDoc.FileName);
                    fuUplodDoc.SaveAs(Server.MapPath(CustDocPathTemp) + "/" + fuUplodDoc.FileName);
                    lblFileName.Text = ViewState["DocumentFileName"].ToString();
                    imgdeletebtn.Visible = true;
                }
                else
                {
                    ViewState["DocumentFileName"] = null;
                    lblFileName.Text = "";
                    string Msg = "Please select file to Upload";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "UplMsg", "$(document).ready( function() {jAlert('" + Msg.ToString() + "', 'Message','fuUplodDoc');});", true);
                }
            }
            catch { }


        }
        /// <summary>
        /// Deletes the document od Customer.
        /// </summary>
        /// <param name="CustomerDoc">String CustomerDoc</param>
        private void DeleteDocument(string CustomerDoc)
        {
            try
            {
                string docPath = AppLogic.AppConfigs("OnlineApplication.DocumentPath").ToString() + "/" + CustomerDoc;
                if (File.Exists(Server.MapPath(docPath)))
                {
                    File.Delete(Server.MapPath(docPath));
                }

            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// Gets the order documents.
        /// </summary>
        #endregion

    }
}