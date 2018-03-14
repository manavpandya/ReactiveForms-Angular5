using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class GiftCardProductList : BasePage
    {
        ProductComponent objproduct = null;
        public static bool isDescendproductname = false;
        public static bool isDescendstname = false;
        public static bool isDescendproductcode = false;
        public static bool isDescendourprice = false;
        public static bool Issearch = false;
        public static string SearchBy = null;
        public static string SearchValue = null;
        int StoreID = 0;
        public static DataView DvProduct = null;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Insert"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Gift Card Product inserted successfully.', 'Message');});", true);

                }
                else if (Request.QueryString["Update"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Gift Card Product updated successfully.', 'Message');});", true);
                }
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/search.gif";
                ibtnShowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/showall.png";
                //btnAddNew.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/add-quantity-discount.png) no-repeat transparent; width: 154px; height: 23px; border:none;cursor:pointer;");
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                // btndeleteQuantitytable.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                bindstore();
                SearchBy = "";
                SearchValue = "";
                binddata();

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
            ddlStore.Items.Insert(0, new ListItem("All Store", "-1"));

            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
                StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                StoreID = 1;
                ddlStore.SelectedIndex = 0;

            }
        }

        /// <summary>
        /// Bind Gift Card Product Data
        /// </summary>
        private void binddata()
        {
            DataSet dsproduct = new DataSet();

            objproduct = new ProductComponent();

            dsproduct = objproduct.GetGiftCardProductList(StoreID, Issearch, SearchBy, SearchValue);

            if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
            {
                DvProduct = dsproduct.Tables[0].DefaultView;
                gvGiftcardProduct.DataSource = dsproduct;
                gvGiftcardProduct.DataBind();
                Issearch = false;
            }
            else
            {
                gvGiftcardProduct.DataSource = null;
                gvGiftcardProduct.DataBind();
            }
        }

        /// <summary>
        /// Sorting function for Grid view
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                DataView dv = new DataView();
                if (DvProduct != null)
                {
                    dv = DvProduct;
                    dv.Sort = lb.CommandName + " " + lb.CommandArgument;
                    if (lb.CommandArgument == "ASC")
                    {
                        lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                        if (lb.ID == "lbProductName")
                        {
                            isDescendproductname = false;
                        }
                        else if (lb.ID == "lbProductCode")
                        {
                            isDescendproductcode = false;
                        }
                        else
                        {
                            isDescendstname = false;
                        }


                        lb.AlternateText = "Descending Order";
                        lb.ToolTip = "Descending Order";
                        lb.CommandArgument = "DESC";
                    }
                    else if (lb.CommandArgument == "DESC")
                    {
                        //          gvListQuantityDiscount.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                        lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                        if (lb.ID == "lbProductName")
                        {
                            isDescendproductname = true;
                        }
                        else if (lb.ID == "lbProductCode")
                        {
                            isDescendproductcode = true;
                        }
                        else
                        {
                            isDescendstname = true;
                        }

                        lb.AlternateText = "Ascending Order";
                        lb.ToolTip = "Ascending Order";
                        lb.CommandArgument = "ASC";
                    }
                    gvGiftcardProduct.DataSource = dv;
                    gvGiftcardProduct.DataBind();
                }

            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                Issearch = true;
                SearchBy = ddlSearch.SelectedItem.Value;
                SearchValue = txtSearch.Text.Trim();
            }
            else
            {
                SearchBy = "";
                SearchValue = "";
            }
            binddata();


        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnsearch_Click(object sender, ImageClickEventArgs e)
        {
            StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                Issearch = true;
                SearchBy = ddlSearch.SelectedItem.Value;
                SearchValue = txtSearch.Text.Trim();
            }
            else
            {
                SearchBy = "";
                SearchValue = "";
            }
            binddata();

        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnShowall_Click(object sender, ImageClickEventArgs e)
        {
            ddlStore.SelectedIndex = 0;
            StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            txtSearch.Text = "";
            SearchBy = "";
            SearchValue = "";
            binddata();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {


            int count = 0;
            int indx = 0;
            foreach (GridViewRow r in gvGiftcardProduct.Rows)
            {
                CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                Label lb = (Label)r.FindControl("lblProductID");
                int ID = Convert.ToInt32(lb.Text.ToString());
                if (chk.Checked)
                {
                    count++;
                    indx = ProductComponent.DeleteGiftCardProduct(ID, Convert.ToInt32(ddlStore.SelectedValue.ToString()));
                }
            }


            if (count == 0)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please select at least one record...');", true);
            }
            else if (indx == 1)
            {
                binddata();
            }
            else if (indx == -1)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('This Quantity Name is Assigned to Product,So First Delete From Product Table...');", true);
            }
            StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            txtSearch.Text = "";
            SearchBy = "";
            SearchValue = "";
            binddata();

        }

        /// <summary>
        /// Gift Card Product Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvGiftcardProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                try
                {
                    int ProductID = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("GiftCardProduct.aspx?Mode=Edit&StoreID=" + ddlStore.SelectedValue + "&ID=" + ProductID);
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Gift Card Product Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvGiftcardProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvGiftcardProduct.Rows.Count > 0)
            {
                Productdata.Visible = true;
            }
            else
            {
                Productdata.Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {

                if (isDescendproductname == false)
                {
                    ImageButton lbProductName = (ImageButton)e.Row.FindControl("lbProductName");
                    lbProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbProductName.AlternateText = "Ascending Order";
                    lbProductName.ToolTip = "Ascending Order";
                    lbProductName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbProductName = (ImageButton)e.Row.FindControl("lbProductName");
                    lbProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbProductName.AlternateText = "Descending Order";
                    lbProductName.ToolTip = "Descending Order";
                    lbProductName.CommandArgument = "ASC";
                }
                if (isDescendproductcode == false)
                {
                    ImageButton lbProductCode = (ImageButton)e.Row.FindControl("lbProductCode");
                    lbProductCode.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbProductCode.AlternateText = "Ascending Order";
                    lbProductCode.ToolTip = "Ascending Order";
                    lbProductCode.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbProductCode = (ImageButton)e.Row.FindControl("lbProductCode");
                    lbProductCode.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbProductCode.AlternateText = "Descending Order";
                    lbProductCode.ToolTip = "Descending Order";
                    lbProductCode.CommandArgument = "ASC";
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
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton _editLinkButton = (ImageButton)e.Row.FindControl("_editLinkButton");
                _editLinkButton.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }
        }

    }
}