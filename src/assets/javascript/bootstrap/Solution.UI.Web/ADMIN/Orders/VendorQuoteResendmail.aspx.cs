using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class VendorQuoteResendmail : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSendReciept.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/send-mail.png";
                FillPage();
            }
        }

        /// <summary>
        /// Fills the page for resend mail.
        /// </summary>
        private void FillPage()
        {
            DataSet Ds = new DataSet();
            Ds = CommonComponent.GetCommonDataSet("SELECT * FROM tb_MailLog WHERE MailID=" + Convert.ToInt32(Request.QueryString["Miallogid"]) + "");
            if (Ds.Tables[0].Rows.Count > 0)
            {
                ltDesc.Text = Ds.Tables[0].Rows[0]["Body"].ToString();
                ltEmail.Text = Ds.Tables[0].Rows[0]["ToEmail"].ToString();
                txtFrom.Text = Ds.Tables[0].Rows[0]["ToEmail"].ToString();
                ltsubject.Text = Ds.Tables[0].Rows[0]["Subject"].ToString();
                ltsfrom.Text = Ds.Tables[0].Rows[0]["FromMail"].ToString();
            }
        }

        /// <summary>
        ///  Send Receipt Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSendReciept_Click(object sender, ImageClickEventArgs e)
        {
            SendMail(ltDesc.Text, txtFrom.Text, ltsubject.Text);
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="body">string body</param>
        /// <param name="ToID">string ToID</param>
        /// <param name="Subject">string Subject</param>
        public void SendMail(String body, String ToID, String Subject)
        {
            CommonOperations.SendMail(ToID, Subject, body, Request.UserHostAddress.ToString(), true, null);
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgmail", "alert('Mail has been sent successfully.');window.close();", true);
        }
    }
}