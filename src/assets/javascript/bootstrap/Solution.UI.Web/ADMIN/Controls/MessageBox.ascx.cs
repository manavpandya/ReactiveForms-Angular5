using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using System.Web.Services;

namespace Solution.UI.Web.ADMIN.Controls
{
    public partial class MessageBox : System.Web.UI.UserControl
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            BindUsers();
            btnsend.ImageUrl = "/App_Themes/" + Page.Theme + "/images/send.gif";
            if (Session["AdminID"].ToString() != "1")
            {
                tdviewMsg.Visible = false;
            }

            Iframe1.Attributes.Add("onload", "onloadFrame();");


        }

        #region Bind Users
        /// <summary>
        /// Bind Users
        /// </summary>
        private void BindUsers()
        {
            DataSet dsUsers = new DataSet();
            dsUsers = CommonComponent.GetCommonDataSet(" SELECT AdminID,(FirstName+' '+LastName) AS Name FROM tb_admin WHERE AdminID NOT IN( SELECT adminid FROM tb_Admin WHERE AdminID=" + Session["AdminID"] + " and isnull(Deleted,0)=0) AND isnull(Deleted,0)=0");

            if (dsUsers != null && dsUsers.Tables.Count > 0 && dsUsers.Tables[0].Rows.Count > 0)
            {
                ddlUserlist.DataSource = dsUsers;
                ddlUserlist.DataTextField = "Name";
                ddlUserlist.DataValueField = "AdminID";
                ddlUserlist.DataBind();

            }
            ddlUserlist.Items.Insert(0, new ListItem("Message To", "0"));
            ddlUserlist.SelectedIndex = 0;
        }
        #endregion

        /// <summary>
        ///  Send Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsend_Click(object sender, ImageClickEventArgs e)
        {
            if (txtmsg.Text != null && txtmsg.Text != "" && ddlUserlist.SelectedValue != "0")
            {
                string strSql = @"insert into tb_AdminChat(SenderID,ReceiverID,MessageText) values(" + Convert.ToInt32(Session["AdminID"]) + ",'" + Convert.ToInt32(ddlUserlist.SelectedItem.Value) + "','" + txtmsg.Text + "')";

                CommonComponent.ExecuteCommonData(strSql);
                txtmsg.Text = "";
                BindUsers();
            }

        }

    }
}