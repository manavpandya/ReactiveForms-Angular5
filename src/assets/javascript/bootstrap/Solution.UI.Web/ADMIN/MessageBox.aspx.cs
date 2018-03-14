using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web.ADMIN
{
    public partial class MessageBox : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "tesd", "<script language='javascript'>RefreshParent()</script>");
            }
            else
            {
                BindMessages();
            }
        }

        /// <summary>
        /// bind Message Frequently
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Timer1_Tick(object sender, System.EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "logoutmsg", "RefreshParent()", true);
            }
            else
            {
                BindMessages();
            }
        }

        /// <summary>
        /// Binds the Messages
        /// </summary>
        private void BindMessages()
        {
            DataSet dsmsgs = new DataSet();
            if (Session["AdminID"].ToString() == "1")
            {
                dsmsgs = CommonComponent.GetCommonDataSet("SELECT Top 10 a.FirstName AS senderfname,B.FirstName AS receiverfname, c.* FROM dbo.tb_Admin AS a, dbo.tb_Admin AS b, dbo.tb_AdminChat c WHERE c.SenderID=a.AdminID AND c.ReceiverID=b.AdminID  ORDER BY c.CreatedOn DESC ");
                if (dsmsgs != null && dsmsgs.Tables.Count > 0 && dsmsgs.Tables[0].Rows.Count > 0)
                {
                    DataView myView = new DataView();
                    myView = dsmsgs.Tables[0].DefaultView;
                    myView.Sort = "CreatedOn asc";
                    grdmessagebox.DataSource = myView;
                    grdmessagebox.DataBind();
                    grdmessagebox.Visible = true;

                }
                else
                {
                    grdmessagebox.DataSource = null;
                    grdmessagebox.DataBind();
                }
            }
            else
            {


                dsmsgs = CommonComponent.GetCommonDataSet("SELECT Top 10 a.FirstName AS senderfname,B.FirstName AS receiverfname, c.* FROM dbo.tb_Admin AS a, dbo.tb_Admin AS b, dbo.tb_AdminChat c WHERE c.SenderID=a.AdminID AND c.ReceiverID=b.AdminID AND (c.SenderID=" + Session["AdminID"] + " OR c.receiverid= " + Session["AdminID"] + ") ORDER BY c.CreatedOn DESC ");
                if (dsmsgs != null && dsmsgs.Tables.Count > 0 && dsmsgs.Tables[0].Rows.Count > 0)
                {
                    DataView myView = new DataView();
                    myView = dsmsgs.Tables[0].DefaultView;
                    myView.Sort = "CreatedOn asc";
                    grdmessagebox.DataSource = myView;
                    grdmessagebox.DataBind();
                    grdmessagebox.Visible = true;

                }
                else
                {
                    grdmessagebox.DataSource = null;
                    grdmessagebox.DataBind();
                }
            }

        }

    }
}