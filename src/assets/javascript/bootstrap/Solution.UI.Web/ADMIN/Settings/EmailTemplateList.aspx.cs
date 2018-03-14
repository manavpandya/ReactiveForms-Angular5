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
    public partial class EmailTemplateList : BasePage
    {
        #region Declaration
        public static bool isDescendLabel = false;
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
                bindstore();
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Email Template inserted successfully.', 'Message','');});", true);

                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Email Template updated successfully.', 'Message','');});", true);
                    }
                }
            }
        }

        /// <summary>
        /// Email Tempalte Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdEmailTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int TemplateID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("EmailTemplate.aspx?EmailTemplateID=" + TemplateID);
            }
        }

        /// <summary>
        /// Email Tempalte Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdEmailTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendLabel == false)
                {
                    ImageButton btnLabel = (ImageButton)e.Row.FindControl("btnLabel");
                    btnLabel.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnLabel.AlternateText = "Ascending Order";
                    btnLabel.ToolTip = "Ascending Order";
                    btnLabel.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnLabel = (ImageButton)e.Row.FindControl("btnLabel");
                    btnLabel.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnLabel.AlternateText = "Descending Order";
                    btnLabel.ToolTip = "Descending Order";
                    btnLabel.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Sorting Gridview Column by ACS or DESC Order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton btnSorting = (ImageButton)sender;
            if (btnSorting != null)
            {
                if (btnSorting.CommandArgument == "ASC")
                {
                    grdEmailTemplate.Sort(btnSorting.CommandName.ToString(), SortDirection.Ascending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (btnSorting.ID == "btnLabel")
                    {
                        isDescendLabel = false;
                    }
                    btnSorting.AlternateText = "Descending Order";
                    btnSorting.ToolTip = "Descending Order";
                    btnSorting.CommandArgument = "DESC";
                }
                else if (btnSorting.CommandArgument == "DESC")
                {
                    grdEmailTemplate.Sort(btnSorting.CommandName.ToString(), SortDirection.Descending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (btnSorting.ID == "btnLabel")
                    {
                        isDescendLabel = true;
                    }
                    btnSorting.AlternateText = "Ascending Order";
                    btnSorting.ToolTip = "Ascending Order";
                    btnSorting.CommandArgument = "ASC";
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
            grdEmailTemplate.DataBind();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlSearch.SelectedIndex = 0;
            ddlStore.SelectedIndex = 0;
            grdEmailTemplate.DataBind();
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
                ddlStore.SelectedIndex = 0;
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());

                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
                AppConfig.StoreID = 1;

        }
    }
}