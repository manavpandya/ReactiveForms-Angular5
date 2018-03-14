using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class CustomerLevelList : Solution.UI.Web.BasePage
    {
        #region Declaration
        public static bool isDescendName = false;
        public static bool isDescendDiscAmt = false;
        public static bool isDescendDiscPerc = false;
        public static bool isDescendStoreName = false;
        public static bool isDescendDispOrder = false;
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
                btnDeleteCustLevel.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                String strStatus = Convert.ToString(Request.QueryString["status"]);
                if (strStatus == "inserted")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Customer Level inserted successfully.', 'Message');});", true);

                }
                else if (strStatus == "updated")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Customer Level updated successfully.', 'Message');});", true);

                }
                bindstore();
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Filter record based on selected field and searchvalue
            grdCustomerLevel.PageIndex = 0;
            grdCustomerLevel.DataBind();
            if (grdCustomerLevel.Rows.Count == 0)
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
            grdCustomerLevel.PageIndex = 0;
            grdCustomerLevel.DataBind();
            trBottom.Visible = true;
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

            grdCustomerLevel.DataBind();
            if (grdCustomerLevel.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        /// Customer Level Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdCustomerLevel_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int CustomerLevelID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("CustomerLevel.aspx?CustomerLevelID=" + CustomerLevelID); //Edit Customer Level
            }
        }

        /// <summary>
        /// Customer Level Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdCustomerLevel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdCustomerLevel.Rows.Count > 0)
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
                if (isDescendName == false)
                {
                    ImageButton btnConfigName = (ImageButton)e.Row.FindControl("btnLevelName");
                    btnConfigName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnConfigName.AlternateText = "Ascending Order";
                    btnConfigName.ToolTip = "Ascending Order";
                    btnConfigName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnConfigName = (ImageButton)e.Row.FindControl("btnLevelName");
                    btnConfigName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnConfigName.AlternateText = "Descending Order";
                    btnConfigName.ToolTip = "Descending Order";
                    btnConfigName.CommandArgument = "ASC";
                }
                if (isDescendDiscAmt == false)
                {
                    ImageButton btnConfigValue = (ImageButton)e.Row.FindControl("btnDisAmount");
                    btnConfigValue.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnConfigValue.AlternateText = "Ascending Order";
                    btnConfigValue.ToolTip = "Ascending Order";
                    btnConfigValue.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnConfigValue = (ImageButton)e.Row.FindControl("btnDisAmount");
                    btnConfigValue.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnConfigValue.AlternateText = "Descending Order";
                    btnConfigValue.ToolTip = "Descending Order";
                    btnConfigValue.CommandArgument = "ASC";
                }

                if (isDescendDiscPerc == false)
                {
                    ImageButton btnConfigValue = (ImageButton)e.Row.FindControl("btnDisperc");
                    btnConfigValue.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnConfigValue.AlternateText = "Ascending Order";
                    btnConfigValue.ToolTip = "Ascending Order";
                    btnConfigValue.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnConfigValue = (ImageButton)e.Row.FindControl("btnDisperc");
                    btnConfigValue.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnConfigValue.AlternateText = "Descending Order";
                    btnConfigValue.ToolTip = "Descending Order";
                    btnConfigValue.CommandArgument = "ASC";
                }

                if (isDescendStoreName == false)
                {
                    ImageButton btnConfigValue = (ImageButton)e.Row.FindControl("btnStoreName");
                    btnConfigValue.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnConfigValue.AlternateText = "Ascending Order";
                    btnConfigValue.ToolTip = "Ascending Order";
                    btnConfigValue.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnConfigValue = (ImageButton)e.Row.FindControl("btnStoreName");
                    btnConfigValue.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnConfigValue.AlternateText = "Descending Order";
                    btnConfigValue.ToolTip = "Descending Order";
                    btnConfigValue.CommandArgument = "ASC";
                }

                if (isDescendDispOrder == false)
                {
                    ImageButton btnConfigValue = (ImageButton)e.Row.FindControl("btnDisplayorder");
                    btnConfigValue.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnConfigValue.AlternateText = "Ascending Order";
                    btnConfigValue.ToolTip = "Ascending Order";
                    btnConfigValue.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnConfigValue = (ImageButton)e.Row.FindControl("btnDisplayorder");
                    btnConfigValue.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnConfigValue.AlternateText = "Descending Order";
                    btnConfigValue.ToolTip = "Descending Order";
                    btnConfigValue.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Sort GridView in Asc or DESC order
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
                    grdCustomerLevel.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "btnLevelName")
                    {
                        isDescendName = false;
                    }
                    else if (lb.ID == "btnDisAmount")
                    {
                        isDescendDiscAmt = false;
                    }
                    else if (lb.ID == "btnDisperc")
                    {
                        isDescendDiscPerc = false;
                    }
                    else if (lb.ID == "btnStoreName")
                    {
                        isDescendStoreName = false;
                    }
                    else if (lb.ID == "btnDisplayorder")
                    {
                        isDescendDispOrder = false;
                    }
                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdCustomerLevel.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "btnLevelName")
                    {
                        isDescendName = true;
                    }
                    else if (lb.ID == "btnDisAmount")
                    {
                        isDescendDiscAmt = true;
                    }
                    else if (lb.ID == "btnDisperc")
                    {
                        isDescendDiscPerc = true;
                    }
                    else if (lb.ID == "btnStoreName")
                    {
                        isDescendStoreName = true;
                    }
                    else if (lb.ID == "btnDisplayorder")
                    {
                        isDescendDispOrder = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
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
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                ddlStore.SelectedIndex = 0;


            }
        }

        /// <summary>
        ///  Delete Customer Level Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDeleteCustLevel_Click(object sender, EventArgs e)
        {
            CustomerLevelComponent objCustLevelComp = new CustomerLevelComponent();
            tb_CustomerLevel custLevel = new tb_CustomerLevel();
            int totalRowCount = grdCustomerLevel.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdCustomerLevel.Rows[i].FindControl("hdnCustLevelID");
                CheckBox chk = (CheckBox)grdCustomerLevel.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    custLevel = objCustLevelComp.getCustLevel(Convert.ToInt16(hdn.Value));
                    custLevel.Deleted = true;
                    objCustLevelComp.DeleteCustomerLevel(custLevel); //delete Customer Level
                }
            }
            grdCustomerLevel.DataBind();
            if (grdCustomerLevel.Rows.Count == 0)
                trBottom.Visible = false;
        }

    }
}