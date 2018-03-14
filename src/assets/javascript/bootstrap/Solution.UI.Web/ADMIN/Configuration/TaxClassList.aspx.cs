using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Tax_Management
{
    /// <summary>
    /// Tax Class List Page contains list of Tax Class
    /// </summary>
    public partial class TaxClassList : BasePage
    {
        TaxClassComponent objtaxclass = new TaxClassComponent();
        tb_TaxClass tbltaxclass = new tb_TaxClass();
        public static bool isDescendtaxclass = false;
        public static bool isDescendstname = false;
        public static bool isDescendtaxcode = false;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            TaxClassComponent.Filter = "";
            if (!IsPostBack)
            {
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/search.gif";
                ibtnShowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/showall.png";
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                String strStatus = Convert.ToString(Request.QueryString["status"]);
                if (strStatus == "inserted")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Tax Class inserted successfully.', 'Message','');});", true);
                }
                else if (strStatus == "updated")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Tax Class updated successfully.', 'Message','');});", true);
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
        ///  Sorting function For Grid view
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
                    gvTaxClass.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbtaxclass")
                    {
                        isDescendtaxclass = false;
                    }
                    else if (lb.ID == "lbstname")
                    {
                        isDescendstname = false;
                    }
                    else
                    {
                        isDescendtaxcode = false;
                    }
                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    gvTaxClass.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbtaxclass")
                    {
                        isDescendtaxclass = true;
                    }
                    else if (lb.ID == "lbstname")
                    {
                        isDescendstname = true;
                    }
                    else
                    {
                        isDescendtaxcode = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Tax Class Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvTaxClass_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvTaxClass.Rows.Count > 0)
            {
                data.Style["display"] = "block";
            }
            else
            {
                data.Style["display"] = "none";
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendtaxclass == false)
                {
                    ImageButton lbtaxclass = (ImageButton)e.Row.FindControl("lbtaxclass");
                    lbtaxclass.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbtaxclass.AlternateText = "Ascending Order";
                    lbtaxclass.ToolTip = "Ascending Order";
                    lbtaxclass.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbtaxclass = (ImageButton)e.Row.FindControl("lbtaxclass");
                    lbtaxclass.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbtaxclass.AlternateText = "Descending Order";
                    lbtaxclass.ToolTip = "Descending Order";
                    lbtaxclass.CommandArgument = "ASC";
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
                if (isDescendtaxcode == false)
                {
                    ImageButton lbtaxcode = (ImageButton)e.Row.FindControl("lbtaxcode");
                    lbtaxcode.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbtaxcode.AlternateText = "Ascending Order";
                    lbtaxcode.ToolTip = "Ascending Order";
                    lbtaxcode.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbtaxcode = (ImageButton)e.Row.FindControl("lbtaxcode");
                    lbtaxcode.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbtaxcode.AlternateText = "Descending Order";
                    lbtaxcode.ToolTip = "Descending Order";
                    lbtaxcode.CommandArgument = "ASC";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton _editLinkButton = (ImageButton)e.Row.FindControl("_editLinkButton");
                _editLinkButton.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                ImageButton _deleteLinkButton = (ImageButton)e.Row.FindControl("_deleteLinkButton");
                _deleteLinkButton.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delete.gif";
            }
        }

        /// <summary>
        /// Tax Class Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvTaxClass_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteTaxClass")
            {
                int taxclassID = Convert.ToInt32(e.CommandArgument);
                tbltaxclass = null;
                tbltaxclass = objtaxclass.gettaxclass(taxclassID);
                tbltaxclass.Deleted = true;
                objtaxclass.Deletetaxclass(tbltaxclass);
                this.Response.Redirect("TaxClassList.aspx", false);
            }
            else if (e.CommandName == "edit")
            {
                try
                {
                    int TaxClassId = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("TaxClass.aspx?TaxClassId=" + TaxClassId);
                }
                catch
                { }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            gvTaxClass.PageIndex = 0;
            gvTaxClass.DataBind();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int totalRowCount = gvTaxClass.Rows.Count;
            this.Response.Redirect("TaxClassList.aspx", false);
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)gvTaxClass.Rows[i].FindControl("hdnTaxClassid");
                CheckBox chk = (CheckBox)gvTaxClass.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    tbltaxclass = null;
                    tbltaxclass = objtaxclass.gettaxclass(Convert.ToInt16(hdn.Value));
                    tbltaxclass.Deleted = true;
                    objtaxclass.Deletetaxclass(tbltaxclass);
                }
            }
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
            gvTaxClass.PageIndex = 0;
            gvTaxClass.DataBind();
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvTaxClass.PageIndex = 0;
            gvTaxClass.DataBind();

            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
        }

    }
}