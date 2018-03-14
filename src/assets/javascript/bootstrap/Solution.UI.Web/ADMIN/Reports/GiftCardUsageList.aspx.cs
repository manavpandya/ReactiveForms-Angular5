using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class GiftCardUsageList : Solution.UI.Web.BasePage
    {
        public static bool isDescendName = false;
        public static bool isDescendStore = false;
        public static bool isDescendUsedBy = false;

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
                //BindData(Convert.ToInt32(ddlstore.SelectedValue));
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void BindStore()
        {
            StoreComponent stac = new StoreComponent();
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

                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue);
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindData(Convert.ToInt32(ddlstore.SelectedValue));
            grdGiftCardUsage.PageIndex = 0;
            grdGiftCardUsage.DataBind();
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
                    grdGiftCardUsage.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lblName")
                    {
                        isDescendName = false;
                    }
                    else if (lb.ID == "lblstorename")
                    {
                        isDescendStore = false;
                    }
                    else if (lb.ID == "lblUsedBy")
                    {
                        isDescendUsedBy = false;
                    }
                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdGiftCardUsage.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lblName")
                    {
                        isDescendName = true;
                    }
                    else if (lb.ID == "lblstorename")
                    {
                        isDescendStore = true;
                    }
                    else if (lb.ID == "lblUsedBy")
                    {
                        isDescendUsedBy = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Gift Card Usage Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdGiftCardUsage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendName == false)
                {
                    ImageButton lbName = (ImageButton)e.Row.FindControl("lblName");
                    lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbName.AlternateText = "Ascending Order";
                    lbName.ToolTip = "Ascending Order";
                    lbName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbName = (ImageButton)e.Row.FindControl("lblName");
                    lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbName.AlternateText = "Descending Order";
                    lbName.ToolTip = "Descending Order";
                    lbName.CommandArgument = "ASC";
                }
                if (isDescendStore == false)
                {
                    ImageButton lbstore = (ImageButton)e.Row.FindControl("lblstorename");
                    lbstore.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbstore.AlternateText = "Ascending Order";
                    lbstore.ToolTip = "Ascending Order";
                    lbstore.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbstore = (ImageButton)e.Row.FindControl("lblstorename");
                    lbstore.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbstore.AlternateText = "Descending Order";
                    lbstore.ToolTip = "Descending Order";
                    lbstore.CommandArgument = "ASC";
                }
                if (isDescendUsedBy == false)
                {
                    ImageButton lbstore = (ImageButton)e.Row.FindControl("lblUsedBy");
                    lbstore.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbstore.AlternateText = "Ascending Order";
                    lbstore.ToolTip = "Ascending Order";
                    lbstore.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbstore = (ImageButton)e.Row.FindControl("lblUsedBy");
                    lbstore.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbstore.AlternateText = "Descending Order";
                    lbstore.ToolTip = "Descending Order";
                    lbstore.CommandArgument = "ASC";
                }
            }
        }
    }
}