using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ListManufacturer : System.Web.UI.Page
    {
        #region "Declaration"
        StoreComponent objStorecomponent = new StoreComponent();
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
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/search.gif";

                bindstore();
                bindManufactureDetailwithGrid();
            }
        }
        
        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, "All Store");
            ddlStore.SelectedIndex = 0;
        }

        /// <summary>
        /// Method to bind grid view with manufacture detail
        /// </summary>
        private void bindManufactureDetailwithGrid()
        {
            DataSet dsManufacture;
            if (ddlStore.SelectedItem.Text != "All Store" && txtSearch.Text.Trim() == "" && txtSearch.Text.Trim().Length <= 0)
            {
                dsManufacture = ManufactureComponent.GetManufactureByStoreID(Convert.ToInt16(ddlStore.SelectedValue));
            }
            else if (ddlStore.SelectedItem.Text != "All Store" && txtSearch.Text != null && txtSearch.Text != "" && txtSearch.Text.Trim().Length >= 0)
            {
                dsManufacture = ManufactureComponent.GetManufactureByName(txtSearch.Text.Trim(), Convert.ToInt16(ddlStore.SelectedValue));
            }
            else
            {
                dsManufacture = ManufactureComponent.GetAllManufactureDetail();
            }
            if (dsManufacture != null && dsManufacture.Tables.Count > 0 && dsManufacture.Tables[0].Rows.Count > 0)
            {
                gvManufacturer.DataSource = dsManufacture;
                gvManufacturer.DataBind();
                trbottom.Visible = true;
            }
            else
            {
                gvManufacturer.DataSource = null;
                gvManufacturer.DataBind();
                trbottom.Visible = false;
            }
        }
        
        /// <summary>
        /// Method for set Image is active or not
        /// </summary>
        /// <param name="_Active">bool _Active</param>
        /// <returns>Returns the Image Path</returns>
        public string SetImage(bool _Active)
        {
            string _ReturnUrl;
            if (_Active)
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
        protected void ibtnsearch_Click(object sender, ImageClickEventArgs e)
        {
            bindManufactureDetailwithGrid();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            int totalRowCount = gvManufacturer.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)gvManufacturer.Rows[i].FindControl("hdnManid");
                CheckBox chk = (CheckBox)gvManufacturer.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    ManufactureComponent.Deletecategory(Convert.ToInt16(hdn.Value));
                }
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Manufacturer has been deleted successfully.', 'Message');});", true);
            bindManufactureDetailwithGrid();
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindManufactureDetailwithGrid();
        }

        /// <summary>
        ///  Manufacturer Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvManufacturer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvManufacturer.PageIndex = e.NewPageIndex;
            bindManufactureDetailwithGrid();
        }

        /// <summary>
        /// Manufacturer Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvManufacturer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((HyperLink)e.Row.FindControl("hplEdit")).ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/edit.gif";
            }
        }

    }
}