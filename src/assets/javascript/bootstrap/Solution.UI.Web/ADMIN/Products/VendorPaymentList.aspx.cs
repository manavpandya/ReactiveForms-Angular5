using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class VendorPaymentList : BasePage
    {
        static bool isDescendVendorName = false;
        static bool isDescendTransDate = false;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVendorName();
                if ((Request.QueryString["VID"] != null))
                {
                    ddlVendor.SelectedValue = Request.QueryString["VID"].ToString();
                }
            }
            btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdVendorPayment.PageIndex = 0;
            grdVendorPayment.DataBind();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlVendor.SelectedIndex = 0;
            grdVendorPayment.PageIndex = 0;
            grdVendorPayment.DataBind();
        }

        /// <summary>
        /// Sorting Gridview Column for ACS or DESC
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
                    grdVendorPayment.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "btnVendorName")
                    {
                        isDescendVendorName = false;
                    }
                    else if (lb.ID == "btnTransDate")
                    {
                        isDescendTransDate = false;
                    }


                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdVendorPayment.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "btnVendorName")
                    {
                        isDescendVendorName = true;
                    }
                    else if (lb.ID == "btnTransDate")
                    {
                        isDescendTransDate = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
                //
            }
        }

        /// <summary>
        /// Binds the Vendor List
        /// </summary>
        private void BindVendorList()
        {
            IQueryable<tb_Vendor> vendorLst = VendorComponent.GetDataByFilter(0, 20, "Name", string.Empty);
            if (vendorLst != null && vendorLst.Count() > 0)
            {
                ddlVendor.DataSource = vendorLst;
                ddlVendor.DataTextField = "Name";
                ddlVendor.DataValueField = "VendorID";
                ddlVendor.DataBind();
            }
            else
            {
                ddlVendor.DataSource = null;
                ddlVendor.DataBind();
            }
            ddlVendor.Items.Insert(0, new ListItem("All", "-1"));
        }

        /// <summary>
        /// Binds the Name of the Vendor
        /// </summary>
        private void BindVendorName()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("SElect VendorID,Name from tb_Vendor where ISNULL(Active,1)=1 and ISNULL(Deleted,0)=0");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ddlVendor.DataSource = ds;
                ddlVendor.DataValueField = "VendorID";
                ddlVendor.DataTextField = "Name";
                ddlVendor.DataBind();
                ddlVendor.Items.Insert(0, new ListItem("All", "-1"));
            }
            else
            {
                ddlVendor.DataSource = null;
                ddlVendor.DataBind();
            }
        }

        /// <summary>
        /// Vendor Payment Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdVendorPayment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
                if (isDescendTransDate == false)
                {
                    ImageButton btnTransDate = (ImageButton)e.Row.FindControl("btnTransDate");
                    btnTransDate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnTransDate.AlternateText = "Ascending Order";
                    btnTransDate.ToolTip = "Ascending Order";
                    btnTransDate.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnTransDate = (ImageButton)e.Row.FindControl("btnTransDate");
                    btnTransDate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnTransDate.AlternateText = "Descending Order";
                    btnTransDate.ToolTip = "Descending Order";
                    btnTransDate.CommandArgument = "ASC";
                }
            }
        }
    }
}