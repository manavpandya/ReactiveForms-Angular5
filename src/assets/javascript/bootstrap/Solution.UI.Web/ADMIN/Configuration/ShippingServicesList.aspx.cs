using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Configuration
{
    public partial class ShippingServicesList : BasePage
    {
        #region Declaration
        public static bool isDescendShippingService = false;
        public static bool isDescendstname = false;
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
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/search.gif";
                ibtnShowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/showall.png";
                String strStatus = Convert.ToString(Request.QueryString["status"]);
                if (strStatus == "inserted")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert(' Shipping Service inserted successfully.', 'Message','');});", true);
                }
                else if (strStatus == "updated")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert(' Shipping Service updated successfully.', 'Message','');});", true);
                }
                bindstore();
            }
        }

        /// <summary>
        ///  Bind Store Details in dropdown
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
            }
            ddlStore.Items.Insert(0, new ListItem("All Store", "-1"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue);
            }
        }

        /// <summary>
        /// Sorting function For Grid view
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
                    gvShippingService.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbShippingService")
                    {
                        isDescendShippingService = false;
                    }
                    else if (lb.ID == "lbstname")
                    {
                        isDescendstname = false;
                    }

                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    gvShippingService.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbShippingService")
                    {
                        isDescendShippingService = true;
                    }
                    else if (lb.ID == "lbstname")
                    {
                        isDescendstname = true;
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
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            gvShippingService.PageIndex = 0;
            gvShippingService.DataBind();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
            ddlStore.SelectedIndex = 0;
            gvShippingService.PageIndex = 0;
            gvShippingService.DataBind();
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvShippingService.PageIndex = 0;
            gvShippingService.DataBind();
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }

        }

        /// <summary>
        /// Shipping Service Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvShippingService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendShippingService == false)
                {
                    ImageButton lbShippingService = (ImageButton)e.Row.FindControl("lbShippingService");
                    lbShippingService.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbShippingService.AlternateText = "Ascending Order";
                    lbShippingService.ToolTip = "Ascending Order";
                    lbShippingService.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbShippingService = (ImageButton)e.Row.FindControl("lbShippingService");
                    lbShippingService.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbShippingService.AlternateText = "Descending Order";
                    lbShippingService.ToolTip = "Descending Order";
                    lbShippingService.CommandArgument = "ASC";
                }
                if (isDescendstname == false)
                {
                    ImageButton lbstname = (ImageButton)e.Row.FindControl("lbstname");
                    lbstname.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbstname.AlternateText = "Ascending Order";
                    lbstname.ToolTip = "Ascending Order";
                    lbstname.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbstname = (ImageButton)e.Row.FindControl("lbstname");
                    lbstname.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbstname.AlternateText = "Descending Order";
                    lbstname.ToolTip = "Descending Order";
                    lbstname.CommandArgument = "ASC";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lnkedit = (ImageButton)e.Row.FindControl("_editLinkButton");
                lnkedit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }
        }

        /// <summary>
        /// Shipping Service Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvShippingService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                try
                {
                    int ShippingServiceID = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("ShippingServices.aspx?ShippingServiceID=" + ShippingServiceID);
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Method for set Image is active or not
        /// </summary>
        /// <param name="_Active">bool _Active</param>
        /// <returns>Returns the Image Path</returns>
        public string SetImage(bool _Active)
        {
            string _ReturnUrl;
            if (_Active)
            {
                _ReturnUrl = "../Images/active.gif";

            }
            else
            {
                _ReturnUrl = "../Images/in-active.gif";

            }
            return _ReturnUrl;
        }
    }
}