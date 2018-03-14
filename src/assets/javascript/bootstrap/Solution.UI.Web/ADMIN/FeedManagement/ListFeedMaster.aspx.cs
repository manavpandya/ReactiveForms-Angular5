using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Web.UI.HtmlControls;
using Solution.Bussines.Components.AdminCommon;
using System.Data;

namespace Solution.UI.Web.ADMIN.FeedManagement
{
    public partial class ListFeedMaster : BasePage
    {
        #region Declaration

        FeedComponent objFeed = null;
        tb_FeedMaster tbFeedMaster = null;
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
                if (Request.QueryString["status"] != null)
                {
                    if (Convert.ToString(Request.QueryString["status"]).ToLower() == "insert")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkfeedInsert", "$(document).ready( function() {jAlert('Record inserted successfully.', 'Message');});", true);
                    }
                    else if (Convert.ToString(Request.QueryString["status"]).ToLower() == "update")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkfeedInsert", "$(document).ready( function() {jAlert('Record updated successfully.', 'Message');});", true);
                    }
                }

                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                bindstore();
                bindFeed("");
            }
        }

        /// <summary>
        /// Bind all stores into drop down list
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
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            try
            {
                if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) > 0)
                {
                    ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]);
                }
                else
                {
                    ddlStore.SelectedIndex = 0;
                }
            }
            catch
            {
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Binds the Feed Data
        /// </summary>
        /// <param name="SearchVal">String SearchVal</param>
        private void bindFeed(String SearchVal)
        {
            Int32 StoreId = 0;
            DataSet dsFeedMaster = new DataSet();
            FeedComponent objFeed = new FeedComponent();

            if (ddlStore.SelectedIndex == 0)
                StoreId = Convert.ToInt32(0);
            else StoreId = Convert.ToInt32(ddlStore.SelectedValue.ToString());

            dsFeedMaster = objFeed.GetFeedData(StoreId, SearchVal.ToString());
            if (dsFeedMaster != null && dsFeedMaster.Tables.Count > 0 && dsFeedMaster.Tables[0].Rows.Count > 0)
            {
                grdFeedMaster.DataSource = dsFeedMaster;
                grdFeedMaster.DataBind();
                lblTotcount.Text = "Total Record : " + dsFeedMaster.Tables[0].Rows.Count.ToString();
            }
            else
            {
                grdFeedMaster.DataSource = null;
                grdFeedMaster.DataBind();
                lblTotcount.Text = "";
            }
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="_Status">Boolean _Status</param>
        /// <returns>Returns the Image Path either Active or In-active</returns>
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
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Filter record based on selected field and search value
            if (!string.IsNullOrEmpty(txtSearch.Text.ToString()))
            {
                string SearchValue = txtSearch.Text.ToString().Trim().Replace("'", "''");
                bindFeed(SearchValue.ToString());
            }
            grdFeedMaster.PageIndex = 0;
            grdFeedMaster.DataBind();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            bindFeed("");
        }

        /// <summary>
        ///  Feed Mater Gridview Row Editing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewEditEventArgs e</param>
        protected void grdFeedMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Int32 i = Convert.ToInt16(e.NewEditIndex);
            Int32 FeedId = Convert.ToInt32(((Label)grdFeedMaster.Rows[i].FindControl("lblFeedId")).Text);

            if (FeedId > 0)
            {
                Response.Redirect("FeedMaster.aspx?Mode=edit&Id=" + FeedId + "");
            }
        }

        /// <summary>
        /// Feed Master Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdFeedMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("btnEdit")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
            }
        }

        /// <summary>
        ///  Feed Mater Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdFeedMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdFeedMaster.PageIndex = e.NewPageIndex;
            bindFeed(txtSearch.Text.ToString().Trim().Replace("'", "''"));
        }


        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            txtSearch.Text = "";
            grdFeedMaster.PageIndex = 0;
            bindFeed("");
        }
    }
}