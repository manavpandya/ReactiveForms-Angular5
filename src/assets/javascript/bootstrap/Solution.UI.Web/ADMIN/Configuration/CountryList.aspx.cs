using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.configuration
{
    /// <summary>
    /// Country Listing form Contains Country related Code for Operations and Displaying Country Data      
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>



    public partial class ListCountry : BasePage
    {
        #region Declarations

        public static bool isDescendName = false;
        public static bool isDescendTLISO = false;
        public static bool isDescendThLISO = false;

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
                isDescendName = false;
                isDescendTLISO = false;
                isDescendThLISO = false;
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Country inserted successfully.', 'Message','');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Country updated successfully.', 'Message','');});", true);
                    }
                }
                CountryComponent.Filter = "";
            }
            Page.Form.DefaultButton = btnSearch.UniqueID;
        }

        /// <summary>
        /// Page OnInit Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// Country Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdCountry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "DeleteCountry")
            //{
            //    try
            //    {
            //        CountryComponent cust = new CountryComponent();
            //        tb_Country tb_Country = null;
            //        int CountryId = Convert.ToInt32(e.CommandArgument);
            //        tb_Country = cust.getCountry(CountryId);
            //        tb_Country.Deleted = true;
            //        cust.UpdateCountry(tb_Country);
            //        if (CountryComponent.CountryID == 0)
            //            RefreshGrid(true);
            //        else
            //            this.Response.Redirect("CountryList.aspx", false);
            //    }
            //    catch (Exception ex)
            //    { throw ex; }
            //}
            //else
            if (e.CommandName == "edit")
            {
                try
                {
                    int CountryId = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("Country.aspx?CountryID=" + CountryId);
                }
                catch
                { }
            }
            else if (e.CommandName == "add")
            {
                try
                {
                    Response.Redirect("Country.aspx");
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Refresh Gridview Function
        /// </summary>
        /// <param name="afterDelete">Boolean afterDelete</param>
        private void RefreshGrid(bool afterDelete = false)
        {
            CountryComponent.AfterDelete = afterDelete;
            grdCountry.DataBind();
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
                    grdCountry.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = false;
                    }
                    else if (lb.ID == "lbTLISO")
                    {
                        isDescendTLISO = false;
                    }
                    else
                    {
                        isDescendThLISO = false;
                    }

                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdCountry.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = true;
                    }
                    else if (lb.ID == "lbTLISO")
                    {
                        isDescendTLISO = true;
                    }
                    else
                    {
                        isDescendThLISO = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CountryComponent.CountryID = 0;
            CountryComponent.Filter = txtCountry.Text.Trim();
            if (txtCountry.Text.ToString().Length > 0)
            {
                CountryComponent.NewFilter = true;
            }
            else
            {
                CountryComponent.NewFilter = false;
            }

            grdCountry.PageIndex = 0;
            grdCountry.DataBind();
        }

        /// <summary>
        /// Country Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdCountry_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    ImageButton lbName = (ImageButton)e.Row.FindControl("lbName");
                    lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbName.AlternateText = "Ascending Order";
                    lbName.ToolTip = "Ascending Order";
                    lbName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbName = (ImageButton)e.Row.FindControl("lbName");
                    lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbName.AlternateText = "Descending Order";
                    lbName.ToolTip = "Descending Order";
                    lbName.CommandArgument = "ASC";
                }
                if (isDescendTLISO == false)
                {
                    ImageButton lbTLISO = (ImageButton)e.Row.FindControl("lbTLISO");
                    lbTLISO.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbTLISO.AlternateText = "Ascending Order";
                    lbTLISO.ToolTip = "Ascending Order";
                    lbTLISO.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbTLISO = (ImageButton)e.Row.FindControl("lbTLISO");
                    lbTLISO.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbTLISO.AlternateText = "Descending Order";
                    lbTLISO.ToolTip = "Descending Order";
                    lbTLISO.CommandArgument = "ASC";
                }
                if (isDescendThLISO == false)
                {
                    ImageButton lbThLISO = (ImageButton)e.Row.FindControl("lbThLISO");
                    lbThLISO.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbThLISO.AlternateText = "Ascending Order";
                    lbThLISO.ToolTip = "Ascending Order";
                    lbThLISO.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbThLISO = (ImageButton)e.Row.FindControl("lbThLISO");
                    lbThLISO.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbThLISO.AlternateText = "Descending Order";
                    lbThLISO.ToolTip = "Descending Order";
                    lbThLISO.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Hidden Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnDelete_Click(object sender, EventArgs e)
        {
            CountryComponent cust = new CountryComponent();
            tb_Country tb_Country = null;
            int CountryId = Convert.ToInt32(hdnDelete.Value);
            tb_Country = cust.getCountry(CountryId);
            tb_Country.Deleted = true;
            cust.UpdateCountry(tb_Country);
            if (CountryComponent.CountryID == 0)
                RefreshGrid(true);
            else
                this.Response.Redirect("CountryList.aspx", false);
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtCountry.Text = "";
            CountryComponent.Filter = "";
            grdCountry.PageIndex = 0;
            grdCountry.DataBind();
        }
    }
}