using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.IO;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductFabricList : BasePage
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
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product Fabric inserted successfully.', 'Message');});", true);

                }
                else if (Request.QueryString["Update"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product Fabric updated successfully.', 'Message');});", true);
                }
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/search.gif";
                ibtnShowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/showall.png";
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");

                SearchBy = "";
                SearchValue = "";
                binddata();

            }
        }

        /// <summary>
        /// Bind Gift Card Product Data
        /// </summary>
        private void binddata()
        {
            DataSet dsproduct = new DataSet();

            objproduct = new ProductComponent();

            dsproduct = objproduct.GetProductFabricList(0, Issearch, SearchBy, SearchValue);

            if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
            {
                DvProduct = dsproduct.Tables[0].DefaultView;
                gvProductSize.DataSource = dsproduct;
                gvProductSize.DataBind();
                Issearch = false;
            }
            else
            {
                gvProductSize.DataSource = null;
                gvProductSize.DataBind();
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
                    gvProductSize.DataSource = dv;
                    gvProductSize.DataBind();
                }

            }
        }
         
        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnsearch_Click(object sender, ImageClickEventArgs e)
        {
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
            foreach (GridViewRow r in gvProductSize.Rows)
            {
                CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                Label lb = (Label)r.FindControl("lblFabricID");
                int ID = Convert.ToInt32(lb.Text.ToString());
                if (chk.Checked)
                {
                    count++;
                    indx = ProductComponent.DeleteProductFabric(Convert.ToInt32(ID));
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
        protected void gvProductSize_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                try
                {
                    int FabricID = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("AddProductFabric.aspx?Mode=Edit&ID=" + FabricID);
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
        protected void gvProductSize_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvProductSize.Rows.Count > 0)
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

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton _editLinkButton = (ImageButton)e.Row.FindControl("_editLinkButton");
                _editLinkButton.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }
        }

        /// <summary>
        /// Get Icon Image for Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Icon Product Image Full Path</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Fabric/Micro/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Fabric/Micro/image_not_available.jpg");
        }
    }
}