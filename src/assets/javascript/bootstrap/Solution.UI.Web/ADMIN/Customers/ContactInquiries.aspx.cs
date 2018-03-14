using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class ContactInquiries : BasePage
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
                //txtMailFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                //txtMailTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                GetContact();
            }
        }

        /// <summary>
        ///  Contact Report Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvContactReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvContactReport.PageIndex = e.NewPageIndex;
            GetContact();
        }

        /// <summary>
        /// Get Contact
        /// </summary>
        private void GetContact()
        {
            string strSql = "SELECT * FROM tb_ContactUs WHERE ContactUsID <> 0 ";

            if (ddlSearch.SelectedIndex > 0 && txtSearch.Text.ToString().ToLower() != "search keyword" && txtSearch.Text.ToString().ToLower() != "searchkeyword")
            {
                strSql += " AND " + ddlSearch.SelectedValue.ToString() + " like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'";
            }

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
                strSql += " AND Convert(char(10),ContactDate,101) >= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }
            if (txtMailTo.Text.ToString() != "")
            {

                strSql += " AND Convert(char(10),ContactDate,101) <= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }

            DataSet dsMail = new DataSet();
            dsMail = CommonComponent.GetCommonDataSet(strSql + " order by ContactDate desc");
            if (dsMail != null && dsMail.Tables.Count > 0 && dsMail.Tables[0].Rows.Count > 0)
            {
                grvContactReport.DataSource = dsMail;
                grvContactReport.DataBind();

            }
            else
            {

                grvContactReport.DataSource = null;
                grvContactReport.DataBind();
            }



        }

        /// <summary>
        /// Search record by Search Criteria Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grvContactReport.PageIndex = 0;
            GetContact();
        }

        /// <summary>
        /// Show All Record by Selected Date Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            grvContactReport.PageIndex = 0;
            ddlSearch.SelectedIndex = 0;
            txtSearch.Text = "Search Keyword";
            txtMailFrom.Text = "";
            txtMailTo.Text = "";
            GetContact();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Response.Write("sfdfgfdg");
        }
    }
}