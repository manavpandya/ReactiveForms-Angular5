using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class AdminList : BasePage
    {
        /// <summary>
        /// Admin Listing form Contains Admin related Code for Operations and Displaying Admin Data      
        /// <author>
        /// Kaushalam Team © Kaushalam Inc. 2012.
        /// </author>
        /// Version 1.0
        /// </summary>


        #region Declarations

        public static bool isDescendFName = false;
        public static bool isDescendLName = false;
        public static bool isDescendEmail = false;

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
                isDescendFName = false;
                isDescendLName = false;
                isDescendEmail = false;
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Admin inserted successfully.', 'Message','');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Admin updated successfully.', 'Message','');});", true);
                    }
                }
                AdminComponent.Filter = "";
            }

            Page.Form.DefaultButton = btnSearch.UniqueID;
        }

        /// <summary>
        /// Page OnInit Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// Admin Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void _AdminGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteAdmin")
            {

                try
                {
                    AdminComponent objAdmin = new AdminComponent();
                    int MasterAdminID = objAdmin.GetMasterAdminID();
                    tb_Admin tb_Admin = null;
                    int AdminID = Convert.ToInt32(e.CommandArgument);
                    if (AdminID == MasterAdminID)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Master Admin can not be deleted.', 'Message','');});", true);
                        return;
                    }
                    tb_Admin = objAdmin.getAdmin(AdminID);
                    tb_Admin.Deleted = true;
                    int isDeleted = Convert.ToInt32(objAdmin.UpdateAdmin(tb_Admin));
                    if (AdminComponent.AdminID == 0)
                        RefreshGrid(true);
                    else
                        this.Response.Redirect("AdminList.aspx", false);
                }
                catch (Exception ex)
                { throw ex; }
            }
            else if (e.CommandName == "edit")
            {
                try
                {
                    int AdminId = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("Admin.aspx?AdminID=" + AdminId);
                }
                catch
                { }
            }
            else if (e.CommandName == "add")
            {
                try
                {
                    Response.Redirect("Admin.aspx");
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Refresh Gridview Function
        /// </summary>
        /// <param name="afterDelete">Boolean afterDelete</param>
        private void RefreshGrid(bool afterDelete = false)
        {
            AdminComponent.AfterDelete = afterDelete;
            _AdminGridView.DataBind();
        }

        /// <summary>
        /// Sorting Gridview Column by ACS or DESC Order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    _AdminGridView.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbFName")
                    {
                        isDescendFName = false;
                    }
                    else if (lb.ID == "lbLName")
                    {
                        isDescendLName = false;
                    }
                    else
                    {
                        isDescendEmail = false;
                    }

                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    _AdminGridView.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbFName")
                    {
                        isDescendFName = true;
                    }
                    else if (lb.ID == "lbLName")
                    {
                        isDescendLName = true;
                    }
                    else
                    {
                        isDescendEmail = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
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
            AdminComponent.AdminID = 0;
            AdminComponent.Filter = txtAdmin.Text.Trim();
            if (txtAdmin.Text.ToString().Length > 0)
            {
                AdminComponent.NewFilter = true;
            }
            else
            {
                AdminComponent.NewFilter = false;
            }

            _AdminGridView.PageIndex = 0;
            _AdminGridView.DataBind();
        }

        /// <summary>
        /// Method for set Image is active or not
        /// </summary>
        /// <param name="_Status">bool _Status</param>
        /// <returns>Returns the Image Path</returns>
        public string SetImage(Boolean _Status)
        {
            String _ReturnUrl = "";
            if (_Status == true)
            {
                _ReturnUrl = "../Images/active.gif";

            }
            else
            {
                _ReturnUrl = "../Images/in-active.gif";

            }
            return _ReturnUrl;
        }

        /// <summary>
        /// Admin Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void _AdminGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendFName == false)
                {
                    ImageButton lbFName = (ImageButton)e.Row.FindControl("lbFName");
                    lbFName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbFName.AlternateText = "Ascending Order";
                    lbFName.ToolTip = "Ascending Order";
                    lbFName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbFName = (ImageButton)e.Row.FindControl("lbFName");
                    lbFName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbFName.AlternateText = "Descending Order";
                    lbFName.ToolTip = "Descending Order";
                    lbFName.CommandArgument = "ASC";
                }
                if (isDescendLName == false)
                {
                    ImageButton lbLName = (ImageButton)e.Row.FindControl("lbLName");
                    lbLName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbLName.AlternateText = "Ascending Order";
                    lbLName.ToolTip = "Ascending Order";
                    lbLName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbLName = (ImageButton)e.Row.FindControl("lbLName");
                    lbLName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbLName.AlternateText = "Descending Order";
                    lbLName.ToolTip = "Descending Order";
                    lbLName.CommandArgument = "ASC";
                }
                if (isDescendEmail == false)
                {
                    ImageButton lbEmail = (ImageButton)e.Row.FindControl("lbEmail");
                    lbEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbEmail.AlternateText = "Ascending Order";
                    lbEmail.ToolTip = "Ascending Order";
                    lbEmail.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbEmail = (ImageButton)e.Row.FindControl("lbEmail");
                    lbEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbEmail.AlternateText = "Descending Order";
                    lbEmail.ToolTip = "Descending Order";
                    lbEmail.CommandArgument = "ASC";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
            }
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtAdmin.Text = "";
            AdminComponent.Filter = "";
            _AdminGridView.PageIndex = 0;
            _AdminGridView.DataBind();
        }
    }
}