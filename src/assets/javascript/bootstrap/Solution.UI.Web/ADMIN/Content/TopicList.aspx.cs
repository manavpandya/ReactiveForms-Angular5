using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Content
{
    public partial class TopicList : Solution.UI.Web.BasePage
    {
        #region Variable declaration
        public static bool isDescendTopicName = false;
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
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnDeleteTopic.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/delet.gif) no-repeat transparent; width: 58px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Topic inserted successfully.', 'Message');});", true);

                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Topic updated successfully.', 'Message');});", true);
                    }
                }
                bindstore();
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }


            grdTopic.DataBind();
            if (grdTopic.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdTopic.PageIndex = 0;
            grdTopic.DataBind();
            if (grdTopic.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlStore.SelectedIndex = 0;
            grdTopic.PageIndex = 0;
            grdTopic.DataBind();
        }

        /// <summary>
        ///  Delete Topic Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDeleteTopic_Click(object sender, EventArgs e)
        {
            TopicComponent objTopicComp = new TopicComponent();
            tb_Topic topic = new tb_Topic();
            int totalRowCount = grdTopic.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdTopic.Rows[i].FindControl("hdnTopicID");
                CheckBox chk = (CheckBox)grdTopic.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    topic = objTopicComp.getTopicByID(Convert.ToInt16(hdn.Value));
                    topic.Deleted = true;
                    objTopicComp.Update(topic); //delete Topic
                }
            }
            grdTopic.DataBind();
            if (grdTopic.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        /// Bind Store dropdown
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
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
                ddlStore.SelectedIndex = 0;
        }

        /// <summary>
        /// Sort records in ASC or DESC order
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
                    grdTopic.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "btnTopicName")
                    {
                        isDescendTopicName = false;
                    }
                    else if (lb.ID == "btnStoreName")
                    {
                        isDescendStoreName = false;
                    }

                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdTopic.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "btnTopicName")
                    {
                        isDescendTopicName = true;
                    }
                    else if (lb.ID == "btnStoreName")
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
        /// Topic Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdTopic_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int TopicID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("Topic.aspx?TopicID=" + TopicID); //Edit Topic
            }
        }

        /// <summary>
        /// Topic Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdTopic_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdTopic.Rows.Count > 0)
            {
                trBottom.Visible = true;
            }
            else
            {
                trBottom.Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendTopicName == false)
                {
                    ImageButton btnTopicName = (ImageButton)e.Row.FindControl("btnTopicName");
                    btnTopicName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnTopicName.AlternateText = "Ascending Order";
                    btnTopicName.ToolTip = "Ascending Order";
                    btnTopicName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnTopicName = (ImageButton)e.Row.FindControl("btnTopicName");
                    btnTopicName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnTopicName.AlternateText = "Descending Order";
                    btnTopicName.ToolTip = "Descending Order";
                    btnTopicName.CommandArgument = "ASC";
                }
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

    }
}