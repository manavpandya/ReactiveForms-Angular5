using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class FeatureList : BasePage
    {
        #region Declaration
        public static bool isDescendName = false;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindstore();
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                ImgTagAddFeature.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-feature-template.png";
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Feature Template inserted successfully.', 'Message','');});", true);

                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Feature Template updated successfully.', 'Message','');});", true);
                    }
                }
            }
        }


        protected void grdFeature_RowCommand(object sender, GridViewCommandEventArgs e)
        {
          
            if (e.CommandName == "edit")
            {
                int FeatureId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("AddFeature.aspx?FeatureId=" + FeatureId);
            }

        }

        protected void grdFeature_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendName == false)
                {
                    ImageButton btnFeatureName = (ImageButton)e.Row.FindControl("btnFeatureName");
                    btnFeatureName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnFeatureName.AlternateText = "Ascending Order";
                    btnFeatureName.ToolTip = "Ascending Order";
                    btnFeatureName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnFeatureName = (ImageButton)e.Row.FindControl("btnFeatureName");
                    btnFeatureName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnFeatureName.AlternateText = "Descending Order";
                    btnFeatureName.ToolTip = "Descending Order";
                    btnFeatureName.CommandArgument = "ASC";
                }
            }
        }

        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton btnSorting = (ImageButton)sender;
            if (btnSorting != null)
            {
                if (btnSorting.CommandArgument == "ASC")
                {
                    grdFeature.Sort(btnSorting.CommandName.ToString(), SortDirection.Ascending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (btnSorting.ID == "btnFeatureName")
                    {
                        isDescendName = false;
                    }
                    btnSorting.AlternateText = "Descending Order";
                    btnSorting.ToolTip = "Descending Order";
                    btnSorting.CommandArgument = "DESC";
                }
                else if (btnSorting.CommandArgument == "DESC")
                {
                    grdFeature.Sort(btnSorting.CommandName.ToString(), SortDirection.Descending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (btnSorting.ID == "btnFeatureName")
                    {
                        isDescendName = true;
                    }
                    btnSorting.AlternateText = "Ascending Order";
                    btnSorting.ToolTip = "Ascending Order";
                    btnSorting.CommandArgument = "ASC";
                }
            }
        }

        protected void ImgTagAddFeature_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlStore.SelectedValue) > 0)
            {
                Response.Redirect("AddFeature.aspx?StoreId=" + ddlStore.SelectedValue);
            }
            else
            {
                Response.Redirect("AddFeature.aspx?StoreId=1");
            }

        }
        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdFeature.DataBind();
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
            grdFeature.DataBind();
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

        protected void _gridObjectDataSource_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while deleting record.', 'Message');});", true);
                // e.ExceptionHandled = true;
            }
            else
            {

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Deleted Successfully.', 'Message');});", true);
            }
        }


    }
}