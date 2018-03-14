using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.WebMail
{
    public partial class SuccessMessage : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["ShowType"] != null && Request.QueryString["ID"].ToString().ToLower() != "compose")
            {
                btnsuccess.Text = "Back To List";
            }
        }

        /// <summary>
        ///  Success Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsuccess_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["ShowType"] != null && Request.QueryString["ID"].ToString().ToLower() != "compose")
            {
                Response.Redirect("/Admin/WebMail/EmailInboxmaster.aspx?ShowType=" + Request.QueryString["ShowType"].ToString() + "&ID=" + Convert.ToString(Request.QueryString["ID"].ToString()) + "");
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "window.parent.location.href=window.parent.location.href;", true);
            }
        }
    }
}