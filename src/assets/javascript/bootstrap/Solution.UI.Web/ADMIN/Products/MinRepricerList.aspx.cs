using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class MinRepricerList : BasePage
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

                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record inserted successfully.', 'Message','');});", true);

                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record updated successfully.', 'Message','');});", true);
                    }
                }
                GetMasterList();
            }
        }

        private void GetMasterList()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("Exec GuiGetMinRePricerList 1,'" + txtSearch.Text.ToString().Replace("'", "''") + "'");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdMinrepricerlist.DataSource = ds;
                grdMinrepricerlist.DataBind();
            }
            else
            {
                grdMinrepricerlist.DataSource = null;
                grdMinrepricerlist.DataBind();
            }
        }

        /// <summary>
        /// Email Tempalte Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdMinrepricerlist_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int TemplateID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("MinRepricer.aspx?MinMasterID=" + TemplateID);
            }
        }

        /// <summary>
        /// Email Tempalte Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdMinrepricerlist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                Label lblOverwrite = (Label)e.Row.FindControl("lblOverwrite");
                if (lblOverwrite.Text.ToString().ToLower() == "true" || lblOverwrite.Text.ToString().ToLower() == "1")
                {
                    lblOverwrite.Text = "Yes";
                }
                else
                {
                    lblOverwrite.Text = "No";
                }
                Label lblactive = (Label)e.Row.FindControl("lblactive");
                if (lblactive.Text.ToString().ToLower() == "true" || lblactive.Text.ToString().ToLower() == "1")
                {
                    lblactive.Text = "Active";
                }
                else
                {
                    lblactive.Text = "InActive";
                }

                
                
            }

        }



        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetMasterList();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";

            GetMasterList();
        }


    }
}