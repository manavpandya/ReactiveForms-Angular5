using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;

namespace Solution.UI.Web.ADMIN
{
    public partial class ViewallMessage : System.Web.UI.Page
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
                txtMailFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtMailTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                //btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");

                GetMessages();
            }
        }

        /// <summary>
        /// Gets All Messages
        /// </summary>
        private void GetMessages()
        {
             string strSql = "SELECT a.FirstName AS senderfname,B.FirstName AS receiverfname, c.* FROM dbo.tb_Admin AS a, dbo.tb_Admin AS b, dbo.tb_AdminChat c WHERE c.SenderID=a.AdminID AND c.ReceiverID=b.AdminID";
             if (txtMailFrom.Text.ToString() != "" && txtMailTo.Text.ToString() != "")
             {
                 if (Convert.ToDateTime(txtMailTo.Text.ToString()) >= Convert.ToDateTime(txtMailFrom.Text.ToString()))
                 {

                 }
                 else
                 {
                     Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                     return;
                 }
             }
             else
             {
                 if (txtMailFrom.Text.ToString() == "" && txtMailTo.Text.ToString() != "")
                 {
                     Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                     return;
                 }
                 else if (txtMailTo.Text.ToString() == "" && txtMailFrom.Text.ToString() != "")
                 {
                     Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                     return;
                 }

             }
             if (txtMailFrom.Text.ToString() != "")
             {
                 strSql += " AND Convert(char(10),c.CreatedOn,101) >= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
             }
             if (txtMailTo.Text.ToString() != "")
             {

                 strSql += " AND Convert(char(10),c.CreatedOn,101) <= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
             }
             strSql = strSql + "ORDER BY c.CreatedOn DESC";



            DataSet dsmsgs = new DataSet();
            dsmsgs =CommonComponent.GetCommonDataSet(strSql);
            //dsmsgs = CommonComponent.GetCommonDataSet("SELECT a.FirstName,c.* FROM tb_admin a INNER JOIN dbo.tb_AdminChat c ON a.AdminID=c.SenderID where (receiverid =1 OR SenderID=1) ORDER BY CreatedOn DESC  ");(c.receiverid =1 OR c.SenderID=1)
            
            //dsmsgs = CommonComponent.GetCommonDataSet("SELECT a.FirstName AS senderfname,B.FirstName AS receiverfname, c.* FROM dbo.tb_Admin AS a, dbo.tb_Admin AS b, dbo.tb_AdminChat c WHERE c.SenderID=a.AdminID AND c.ReceiverID=b.AdminID  ORDER BY c.CreatedOn DESC ");
            if (dsmsgs != null && dsmsgs.Tables.Count > 0 && dsmsgs.Tables[0].Rows.Count > 0)
            {
                // DataView myView = new DataView();
                // myView = dsmsgs.Tables[0].DefaultView;
                // myView.Sort = "CreatedOn asc";
                grdmessagebox.DataSource = dsmsgs;//dsmsgs.Tables[0];
                grdmessagebox.DataBind();
                grdmessagebox.Visible = true;
            }
            else
            {
                grdmessagebox.DataSource = null;
                grdmessagebox.DataBind();
            }
             
        }

        /// <summary>
        ///  Message Box Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdmessagebox_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdmessagebox.PageIndex = e.NewPageIndex;
            GetMessages();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdmessagebox.PageIndex = 0;
            GetMessages();
        }
    }
}