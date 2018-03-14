﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class VendorSkuList : Solution.UI.Web.BasePage
    {
        #region Declaration
        bool isDescendVendorName = false;
        bool isDecProductName = false;
        bool isDesceVendorSKU = false;
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
                btnGo.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 62px; height: 23px; border:none;cursor:pointer;");
                btnSearchall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnDeleteMultiple.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('DropShipper SKU inserted successfully.', 'Message');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('DropShipper SKU updated successfully.', 'Message');});", true);
                    }
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, EventArgs e)
        {
            grdVendor.DataBind();
            if (grdVendor.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        ///  Search All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            grdVendor.DataBind();
        }

        /// <summary>
        /// Sort GridView in Asc or DESC order
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
                    grdVendor.Sort(btnSorting.CommandName.ToString(), SortDirection.Ascending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (btnSorting.ID == "btnVendorName")
                    {
                        isDescendVendorName = false;
                    }
                    else if (btnSorting.ID == "btnVendorSKU")
                    {
                        isDesceVendorSKU = false;
                    }
                    else if (btnSorting.ID == "btnProductName")
                    {
                        isDecProductName = false;
                    }
                    btnSorting.AlternateText = "Descending Order";
                    btnSorting.ToolTip = "Descending Order";
                    btnSorting.CommandArgument = "DESC";
                }
                else if (btnSorting.CommandArgument == "DESC")
                {
                    grdVendor.Sort(btnSorting.CommandName.ToString(), SortDirection.Descending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (btnSorting.ID == "btnVendorName")
                    {
                        isDescendVendorName = true;
                    }
                    else if (btnSorting.ID == "btnVendorSKU")
                    {
                        isDesceVendorSKU = true;
                    }
                    else if (btnSorting.ID == "btnProductName")
                    {
                        isDecProductName = true;
                    }

                    btnSorting.AlternateText = "Ascending Order";
                    btnSorting.ToolTip = "Ascending Order";
                    btnSorting.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Vendor Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdVendor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int VendorID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("Vendorsku.aspx?VendorSKUID=" + VendorID); //Edit Vendor detail
            }
        }

        /// <summary>
        /// Vendor Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdVendor_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (grdVendor.Rows.Count > 0)
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
                if (isDescendVendorName == false)
                {
                    ImageButton btnVendorName = (ImageButton)e.Row.FindControl("btnVendorName");
                    btnVendorName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnVendorName.AlternateText = "Ascending Order";
                    btnVendorName.ToolTip = "Ascending Order";
                    btnVendorName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnVendorName = (ImageButton)e.Row.FindControl("btnVendorName");
                    btnVendorName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnVendorName.AlternateText = "Descending Order";
                    btnVendorName.ToolTip = "Descending Order";
                    btnVendorName.CommandArgument = "ASC";
                }
                if (isDecProductName == false)
                {
                    ImageButton btnEmail = (ImageButton)e.Row.FindControl("btnProductName");
                    btnEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnEmail.AlternateText = "Ascending Order";
                    btnEmail.ToolTip = "Ascending Order";
                    btnEmail.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnEmail = (ImageButton)e.Row.FindControl("btnProductName");
                    btnEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnEmail.AlternateText = "Descending Order";
                    btnEmail.ToolTip = "Descending Order";
                    btnEmail.CommandArgument = "ASC";
                }

                if (isDesceVendorSKU == false)
                {
                    ImageButton btnEmail = (ImageButton)e.Row.FindControl("btnVendorSKU");
                    btnEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnEmail.AlternateText = "Ascending Order";
                    btnEmail.ToolTip = "Ascending Order";
                    btnEmail.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnEmail = (ImageButton)e.Row.FindControl("btnVendorSKU");
                    btnEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnEmail.AlternateText = "Descending Order";
                    btnEmail.ToolTip = "Descending Order";
                    btnEmail.CommandArgument = "ASC";
                }
            }

        }

        /// <summary>
        /// Delete Multiple button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDeleteMultiple_Click(object sender, EventArgs e)
        {
            VendorComponent objVendorComp = new VendorComponent();
            tb_VendorSKU vendor = new tb_VendorSKU();
            int totalRowCount = grdVendor.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdVendor.Rows[i].FindControl("hdnVendorID");
                CheckBox chk = (CheckBox)grdVendor.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    vendor = objVendorComp.GetVendorSKUByID(Convert.ToInt32(hdn.Value));
                    vendor.Deleted = true;
                    objVendorComp.UpdateVendorSKU(vendor); //delete Topic
                }
            }
            grdVendor.DataBind();
            if (grdVendor.Rows.Count == 0)
                trBottom.Visible = false;
        }
    }
}