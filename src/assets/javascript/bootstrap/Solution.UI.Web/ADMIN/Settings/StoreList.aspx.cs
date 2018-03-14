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
    public partial class StoreList : Solution.UI.Web.BasePage
    {
        #region Variables and Property
        public static bool isDescendStoreName = false;
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
                btnGo.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;float:right;");
                btnSearchall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;float:right;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Store inserted successfully.', 'Message','');});", true);

                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Store updated successfully.', 'Message','');});", true);
                    }
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
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    grdStore.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "btnStoreName")
                    {
                        isDescendStoreName = false;
                    }
                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdStore.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "btnStoreName")
                    {
                        isDescendStoreName = true;
                    }

                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Store Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdStore_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int StoreId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("Store.aspx?StoreId=" + StoreId);
            }

            else if (e.CommandName == "DeleteStoreConfig")
            {
                int StoreId = Convert.ToInt32(e.CommandArgument);
                StoreComponent objStoreComp = new StoreComponent();
                tb_Store objStore = null;
                objStore = objStoreComp.getStore(StoreId);
                objStore.Deleted = true;
                objStoreComp.UpdateStoreConfiguration(objStore);
                this.Response.Redirect("StoreList.aspx", false);
            }
        }

        /// <summary>
        /// Store Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdStore_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/Delete.gif";
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {

                if (isDescendStoreName == false)
                {
                    ImageButton btnStoreName = (ImageButton)e.Row.FindControl("btnStoreName");
                    btnStoreName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnStoreName.AlternateText = "Ascending Order";
                    btnStoreName.ToolTip = "Ascending Order";
                    btnStoreName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnStoreName = (ImageButton)e.Row.FindControl("btnStoreName");
                    btnStoreName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnStoreName.AlternateText = "Descending Order";
                    btnStoreName.ToolTip = "Descending Order";
                    btnStoreName.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Search All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
            grdStore.PageIndex = 0;
            grdStore.DataBind();
        }

        /// <summary>
        /// Search Button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            grdStore.PageIndex = 0;
            grdStore.DataBind();
        }
    }
}