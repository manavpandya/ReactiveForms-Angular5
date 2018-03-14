using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Text;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class AssignInquiry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetContactDetails();
            }
        }
        public void GetContactDetails()
        {
            if (Request.QueryString["MID"] != null)
            {
                DataSet ds = new DataSet();

                ds = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ContactUs WHERE ContactUsID=" + Request.QueryString["MID"] + "");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    txtMessage.Text = Convert.ToString(ds.Tables[0].Rows[0]["Message"].ToString());



                }
            }
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
           if(!String.IsNullOrEmpty(txtEmail.Text))
           {
               SendMail(txtEmail.Text);
               CommonComponent.GetCommonDataSet("Update tb_ContactUs set Email='" + txtEmail.Text + "' where ContactUsID=" + Request.QueryString["MID"] + "");
               Response.Redirect("ContactInquiries.aspx");

           }
        }

        /// <summary>
        /// Sends the Mail.
        /// </summary>
        /// <param name="TOAddress">string TOAddress</param>
        private void SendMail(string TOAddress)
        {
         CustomerComponent objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();

            dsMailTemplate = objCustomer.GetEmailTamplate("ContactUsEmailForAdmin", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {
                DataSet ds = new DataSet();

                ds = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ContactUs WHERE ContactUsID=" + Request.QueryString["MID"] + "");
                StringBuilder sw = new StringBuilder(4000);
                sw.Append("<table width='100%' class='popup_cantain'></tr>");
                sw.Append("<tr><td  style='margin-top:7px;width:30%;'>");
                sw.Append("<b style='color:#000000;'>Name :</b></td><td  style='margin-top:10px;width:70%;'>" + ds.Tables[0].Rows[0]["Name"].ToString().Trim() + "</td></tr>");
                sw.Append("<tr><td ><b style='color:#000000;'>E-Mail address :</b></td><td>  <a href='mailto:" + ds.Tables[0].Rows[0]["Email"].ToString().Trim() + "' style='color:#000000;'>" + ds.Tables[0].Rows[0]["Email"].ToString().Trim() + "</a></td></tr>");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Address"].ToString().Trim() != "")
                        sw.Append("<tr><td><b style='color:#000000;'>Address :</b></td><td > " + ds.Tables[0].Rows[0]["Address"].ToString().Trim() + "</td></tr>");
                    if (ds.Tables[0].Rows[0]["City"].ToString().Trim() != "")
                        sw.Append("<tr><td><b style='color:#000000;'>City :</b></td><td > " + ds.Tables[0].Rows[0]["City"].ToString().Trim() + "</td></tr>");
                    if (ds.Tables[0].Rows[0]["Country"].ToString().Trim() !="")
                        sw.Append("<tr><td><b style='color:#000000;'>Country :</b></td><td > " + ds.Tables[0].Rows[0]["Country"].ToString().Trim() + "</td></tr>");
                    if (ds.Tables[0].Rows[0]["State"].ToString().Trim() != "")
                        sw.Append("<tr><td><b style='color:#000000;'>State :</b> </td><td >" + ds.Tables[0].Rows[0]["State"].ToString().Trim() + "</td></tr>");

                    if (ds.Tables[0].Rows[0]["ZipCode"].ToString().Trim() != "")
                        sw.Append("<tr><td><b style='color:#000000;'>Zip :</b></td><td > " + ds.Tables[0].Rows[0]["ZipCode"].ToString().Trim() + "</td></tr>");
                    if (ds.Tables[0].Rows[0]["PhoneNumber"].ToString().Trim() != "")
                        sw.Append("<tr><td><b style='color:#000000;'>Phone :</b></td><td > " + ds.Tables[0].Rows[0]["PhoneNumber"].ToString().Trim() + "</td></tr>");
                    if (ds.Tables[0].Rows[0]["SubjectStatus"].ToString().Trim() != "")
                        sw.Append("<tr><td><b style='color:#000000;'>Subject :</b></td><td style='padding-top:5px;padding-bottom:5px' > " + ds.Tables[0].Rows[0]["SubjectStatus"].ToString().Trim() + "</td></tr>");
                    if (ds.Tables[0].Rows[0]["Message"].ToString().Trim() != "")
                        sw.Append("<tr><td><b style='color:#000000;'>Specific Information :</b></td><td style='padding-top:5px;padding-bottom:5px' > " + ds.Tables[0].Rows[0]["Message"].ToString().Trim() + "</td></tr>");
                }
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
                CommonOperations.SendMail(TOAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ContactInquiries.aspx");

        }
    }
}