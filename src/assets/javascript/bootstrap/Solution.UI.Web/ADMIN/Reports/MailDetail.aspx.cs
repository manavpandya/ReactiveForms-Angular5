using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using System.Net.Mail;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class MailDetail : System.Web.UI.Page
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
                GetEmailDetails();
            }
        }

        /// <summary>
        /// Get Email Message Details
        /// </summary>
        private void GetEmailDetails()
        {
            if (Request.QueryString["MID"] != null)
            {
                String strBody = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Body FROM tb_MailLog WHERE MailID=" + Request.QueryString["MID"] + ""));
                ltrDetails.Text = strBody.ToString();
            }
        }
        protected void btnresend_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsnew = new DataSet();
            dsnew = CommonComponent.GetCommonDataSet("SELECT * FROM tb_MailLog WHERE MailID=" + Request.QueryString["MID"] + "");

            if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
            {

                string strremove = "";
                try
                {
                    if (dsnew.Tables[0].Rows[0]["body"].ToString().ToLower().IndexOf("problem in sending mail") > -1)
                    {
                        strremove = dsnew.Tables[0].Rows[0]["body"].ToString().Replace(dsnew.Tables[0].Rows[0]["body"].ToString().Substring(dsnew.Tables[0].Rows[0]["body"].ToString().IndexOf("Problem in sending mail"), dsnew.Tables[0].Rows[0]["body"].ToString().IndexOf("Message Body:") - dsnew.Tables[0].Rows[0]["body"].ToString().IndexOf("Problem in sending mail") + 13), "");
                    }
                    else
                    {
                        strremove = dsnew.Tables[0].Rows[0]["body"].ToString();
                    }
                }
                catch
                {
                    strremove = dsnew.Tables[0].Rows[0]["body"].ToString();
                }

                AlternateView av = AlternateView.CreateAlternateViewFromString(strremove, null, "text/html");
                Solution.Bussines.Components.Common.CommonOperations.SendMail(dsnew.Tables[0].Rows[0]["ToEmail"].ToString(), dsnew.Tables[0].Rows[0]["Subject"].ToString().Replace("Failure :", ""), strremove.ToString(), dsnew.Tables[0].Rows[0]["IPAddress"].ToString(), true, av);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgsuccess", "alert('Mail has been sent successfully.');", true);
            }
            //Problem in sending mail
        }
    }
}