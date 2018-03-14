using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.Data;


namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductRomanShipping : BasePage
    {
        #region Declaration
        public static bool isDescendFromwidth, isDescendTowidth, isDescendCost = false;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ImgTagAddromanshipping.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-roman-shipping.png";
                //  bindstore();
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                 
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product roman shipping detail inserted successfully.', 'Message','');});", true);

                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product roman shipping detail updated successfully.', 'Message','');});", true);
                    }
                    else if (strStatus == "deleted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product roman shipping detail deleted successfully.', 'Message','');});", true);
                    }
                }


               bindgrid();
            }
        }

        public void bindgrid()
        {
            DataSet dsromanproductshipping = CommonComponent.GetCommonDataSet("select * from tb_Roman_Shipping  order by CreatedOn  DESC");
            if (dsromanproductshipping.Tables[0].Rows.Count > 0)
            {

                grdshipping.DataSource = dsromanproductshipping;
                grdshipping.DataBind();

                
            }
            else
            {

                grdshipping.DataSource = null;
                grdshipping.DataBind();
            }
          


        }

        protected void grdshipping_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "edit")
            {
                int RomanshippingId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("ProductRomanShippingAdd.aspx?RomanshippingId=" + RomanshippingId);
            }
            else if (e.CommandName == "delete")
            {
                int RomanshippingId = Convert.ToInt32(e.CommandArgument);
                CommonComponent.ExecuteCommonData("DELETE FROM tb_Roman_Shipping WHERE RomanshippingId =" + RomanshippingId);
                Response.Redirect("ProductRomanShipping.aspx?status=deleted");
            }
            

        }

        protected void grdshipping_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";



            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendFromwidth == false)
                {
                    ImageButton btnFromwidth = (ImageButton)e.Row.FindControl("btnFromwidth");
                    btnFromwidth.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnFromwidth.AlternateText = "Ascending Order";
                    btnFromwidth.ToolTip = "Ascending Order";
                    btnFromwidth.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnFromwidth = (ImageButton)e.Row.FindControl("btnFromwidth");
                    btnFromwidth.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnFromwidth.AlternateText = "Descending Order";
                    btnFromwidth.ToolTip = "Descending Order";
                    btnFromwidth.CommandArgument = "ASC";
                }

                if (isDescendTowidth == false)
                {
                    ImageButton btnTowidth = (ImageButton)e.Row.FindControl("btnTowidth");
                    btnTowidth.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnTowidth.AlternateText = "Ascending Order";
                    btnTowidth.ToolTip = "Ascending Order";
                    btnTowidth.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnTowidth = (ImageButton)e.Row.FindControl("btnTowidth");
                    btnTowidth.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnTowidth.AlternateText = "Descending Order";
                    btnTowidth.ToolTip = "Descending Order";
                    btnTowidth.CommandArgument = "ASC";
                }
                if (isDescendCost == false)
                {
                    ImageButton btnCost = (ImageButton)e.Row.FindControl("btnCost");
                    btnCost.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnCost.AlternateText = "Ascending Order";
                    btnCost.ToolTip = "Ascending Order";
                    btnCost.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnCost = (ImageButton)e.Row.FindControl("btnCost");
                    btnCost.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnCost.AlternateText = "Descending Order";
                    btnCost.ToolTip = "Descending Order";
                    btnCost.CommandArgument = "ASC";
                }
            }
        }

        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton btnSorting = (ImageButton)sender;
            if (btnSorting != null)
            {
                if (btnSorting.CommandArgument == "ASC")
                {
                    grdshipping.Sort(btnSorting.CommandName.ToString(), SortDirection.Ascending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (btnSorting.ID == "btnFromwidth")
                    {
                        isDescendFromwidth = false;
                    }
                    else if (btnSorting.ID == "btnTowidth")
                    {
                        isDescendTowidth = false;
                    }
                    else if (btnSorting.ID == "btnCost")
                    {
                        isDescendCost = false;
                    }
                    btnSorting.AlternateText = "Descending Order";
                    btnSorting.ToolTip = "Descending Order";
                    btnSorting.CommandArgument = "DESC";
                }
                else if (btnSorting.CommandArgument == "DESC")
                {
                 //   DataSet dsromanproductshipping = CommonComponent.GetCommonDataSet("select * from tb_Roman_Shipping  order by CreatedOn  DESC");
                //
                  //  DataView view = new DataView(dsromanproductshipping);
                  //  view.Sort = e.SortExpression + " " + "DESC";
                    grdshipping.Sort(btnSorting.CommandName.ToString(), SortDirection.Descending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (btnSorting.ID == "btnFromwidth")
                    {
                        isDescendFromwidth = false;
                    }
                    else if (btnSorting.ID == "btnTowidth")
                    {
                        isDescendTowidth = false;
                    }
                    else if (btnSorting.ID == "btnCost")
                    {
                        isDescendCost = false;
                    }
                    btnSorting.AlternateText = "Ascending Order";
                    btnSorting.ToolTip = "Ascending Order";
                    btnSorting.CommandArgument = "ASC";
                }
            }
        }
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
                ddlStore.SelectedIndex = 0;
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());

                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
                AppConfig.StoreID = 1;

        }

        protected void ImgTagAddromanshipping_Click(object sender, ImageClickEventArgs e)
        {

            Response.Redirect("ProductRomanShippingAdd.aspx");


        }
    }
}