using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class WarehouseList : Solution.UI.Web.BasePage
    {
        bool isDescend = false;

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
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Warehouse inserted successfully.', 'Message');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Warehouse updated successfully.', 'Message');});", true);
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
            grdWarehouse.PageIndex = 0;
            grdWarehouse.DataBind();
            if (grdWarehouse.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        ///  Seach All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            grdWarehouse.DataBind();
        }

        /// <summary>
        ///  Delete Multiple Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDeleteMultiple_Click(object sender, EventArgs e)
        {
            ProductComponent objVendorComp = new ProductComponent();
            tb_WareHouse warehouse = new tb_WareHouse();
            int totalRowCount = grdWarehouse.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdWarehouse.Rows[i].FindControl("hdnWarehouseID");
                CheckBox chk = (CheckBox)grdWarehouse.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    warehouse = objVendorComp.GetWarehouseByID(Convert.ToInt32(hdn.Value));
                    warehouse.Deleted = true;
                    objVendorComp.UpdateWarehouse(warehouse); //delete Topic
                }
            }
            grdWarehouse.DataBind();
            if (grdWarehouse.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        /// WareHouse Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdWarehouse_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int WarehouseID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("Warehouse.aspx?WarehouseID=" + WarehouseID); //Edit Vendor detail
            }
        }

        /// <summary>
        /// WareHouse Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdWarehouse_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdWarehouse.Rows.Count > 0)
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
                if (isDescend == false)
                {
                    ImageButton btnName = (ImageButton)e.Row.FindControl("btnWarehouseName");
                    btnName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnName.AlternateText = "Ascending Order";
                    btnName.ToolTip = "Ascending Order";
                    btnName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnName = (ImageButton)e.Row.FindControl("btnWarehouseName");
                    btnName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnName.AlternateText = "Descending Order";
                    btnName.ToolTip = "Descending Order";
                    btnName.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Sorting the Grid View for ACS or DESC Order
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
                    grdWarehouse.Sort(btnSorting.CommandName.ToString(), SortDirection.Ascending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (btnSorting.ID == "btnWarehouseName")
                    {
                        isDescend = false;
                    }
                    btnSorting.AlternateText = "Descending Order";
                    btnSorting.ToolTip = "Descending Order";
                    btnSorting.CommandArgument = "DESC";
                }
                else if (btnSorting.CommandArgument == "DESC")
                {
                    grdWarehouse.Sort(btnSorting.CommandName.ToString(), SortDirection.Descending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (btnSorting.ID == "btnWarehouseName")
                    {
                        isDescend = true;
                    }

                    btnSorting.AlternateText = "Ascending Order";
                    btnSorting.ToolTip = "Ascending Order";
                    btnSorting.CommandArgument = "ASC";
                }
            }
        }

    }
}