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
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Orders
{

    public partial class ViewDetail : BasePage
    {
        DataSet dsOrder = new DataSet();
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //popupviewdetailclose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel-icon.png";
            GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["Id"].ToString()));
        }

        /// <summary>
        /// Gets the Order Details by Order Number
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        private void GetOrderDetailsByOrderNumber(Int32 OrderNumber)
        {
            dsOrder = OrderComponent.GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["id"].ToString()));
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (txtpassword.Text.ToString().Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Password.','Required Information','ContentPlaceHolder1_txtpassword');", true);
                txtpassword.Focus();
                return;
            }
            DataSet dsAdmin = new DataSet();
            AdminComponent admin = new AdminComponent();
            tb_Admin tbadmin = admin.getAdmin(Convert.ToInt32(Session["AdminID"].ToString()));
            dsAdmin = AdminComponent.GetAdminForLogin(tbadmin.EmailID.ToString(), SecurityComponent.Encrypt(txtpassword.Text.Trim()));
            if (dsAdmin != null && dsAdmin.Tables.Count > 0 && dsAdmin.Tables[0].Rows.Count > 0)
            {
                lblcardname.Text = dsOrder.Tables[0].Rows[0]["CardName"].ToString();
                lblcardnumber.Text = "***********" + dsOrder.Tables[0].Rows[0]["Last4"].ToString(); //SecurityComponent.Decrypt(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString());
                lblcardtype.Text = dsOrder.Tables[0].Rows[0]["CardType"].ToString();
                lblcustomername.Text = dsOrder.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsOrder.Tables[0].Rows[0]["LastName"].ToString();
                lblcvc.Text = dsOrder.Tables[0].Rows[0]["CardVarificationCode"].ToString();
                lblmonth.Text = dsOrder.Tables[0].Rows[0]["CardExpirationMonth"].ToString();
                lblyear.Text = dsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString();
                creditdetail.Attributes.Add("style", "display:block");
                password.Attributes.Add("style", "display:none");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Incorrect Password', 'Message');});", true);
            }
        }
    }
}