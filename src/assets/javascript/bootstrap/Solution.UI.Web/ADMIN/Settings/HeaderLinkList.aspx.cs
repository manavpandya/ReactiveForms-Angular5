using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class HeaderLinkList : Solution.UI.Web.BasePage
    {
        #region Declaration

        HeaderlinkComponent headercomp = new HeaderlinkComponent();
        StoreComponent stac = new StoreComponent();

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStore();
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnshowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btndelete.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif";
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Header Link inserted successfully.', 'Message');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Header Link updated successfully.', 'Message');});", true);
                    }
                }
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                ddlstore.DataSource = Storelist;
                ddlstore.DataTextField = "StoreName";
                ddlstore.DataValueField = "StoreID";
            }
            else
            {
                ddlstore.DataSource = null;
            }
            ddlstore.DataBind();
            ddlstore.Items.Insert(0, new ListItem("All Store", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlstore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue.ToString());
            }
            else
                AppConfig.StoreID = 1;

        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdheaderlink.PageIndex = 0;
            grdheaderlink.DataBind();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnshowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlstore.SelectedIndex = 0;
            grdheaderlink.PageIndex = 0;
            grdheaderlink.DataBind();
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlstore.SelectedValue.ToString() == "0" || ddlstore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue.ToString());
            }

            grdheaderlink.PageIndex = 0;
            grdheaderlink.DataBind();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndelete_Click(object sender, ImageClickEventArgs e)
        {
            int totalRowCount = grdheaderlink.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdheaderlink.Rows[i].FindControl("hdnheaderlinkid");
                CheckBox chk = (CheckBox)grdheaderlink.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    headercomp.DeleteHeaderLink(Convert.ToInt32(hdn.Value));
                }
            }
            grdheaderlink.DataBind();
        }

        /// <summary>
        ///  Delete1 Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndelete1_Click(object sender, EventArgs e)
        {
            int totalRowCount = grdheaderlink.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdheaderlink.Rows[i].FindControl("hdnheaderlinkid");
                CheckBox chk = (CheckBox)grdheaderlink.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    headercomp.DeleteHeaderLink(Convert.ToInt32(hdn.Value));
                }
            }
            grdheaderlink.DataBind();
        }

        /// <summary>
        /// Header Link Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdheaderlink_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdheaderlink.Rows.Count > 0)
                trBottom.Visible = true;
            else
                trBottom.Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lnkedit = (ImageButton)e.Row.FindControl("lnkedit");
                lnkedit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }
        }

        /// <summary>
        /// Header Link Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdheaderlink_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int headerlinkid = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("HeaderLink.aspx?HeaderLinkID=" + headerlinkid);
            }
        }
    }
}