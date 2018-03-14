using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.Common;

namespace Solution.UI.Web
{
    public partial class MyAccount : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(true);
            ltbrTitle.Text = "My Account";
            ltTitle.Text = "My Account";

            if (!IsPostBack)
            {
                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                }
                else
                {
                    Response.Redirect("/Login.aspx");
                }
                if (!string.IsNullOrEmpty(Request.QueryString["ChangePasswordStatus"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["ChangePasswordStatus"]);
                    if (strStatus == "true")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Password updated successfully.', 'Message','');;", true);
                    }
                }
            }
        }
    }
}