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

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class ExportCustomerVarification : System.Web.UI.Page
    {
        DataSet dsOrder = new DataSet();
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if(!IsPostBack)
            {

            }
            //ppopupviewdetailclose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel-icon.png";
           // GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["Id"].ToString()));
        }

        /// <summary>
        /// Gets the Order Details by Order Number
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        private void GetOrderDetailsByOrderNumber(Int32 OrderNumber)
        {
            dsOrder = OrderComponent.GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["id"].ToString()));
        }
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
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

            string pass =Convert.ToString(CommonComponent.GetScalarCommonData("select configvalue from tb_appconfig where configname='ExportCustomerPassword' and storeid=1"));
            if(!string.IsNullOrEmpty(pass))
            {
                if(txtpassword.Text.ToString().Trim()==pass.ToString().Trim())
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Export", "window.close();window.parent.document.getElementById('ContentPlaceHolder1_btntemp').click();window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose1').click();", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Incorrect Password', 'Message');});", true);
                }
            }
           
        }
    }
}