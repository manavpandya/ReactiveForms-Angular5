using System;
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
    public partial class Vendorsku : Solution.UI.Web.BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnCancle.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                FillVendor();
                if (!string.IsNullOrEmpty(Request.QueryString["VendorSKUID"]) && Convert.ToString(Request.QueryString["VendorSKUID"]) != "0")
                {
                    lblHeader.Text = "Edit Vendor SKU";
                    tb_VendorSKU vendor = new tb_VendorSKU();
                    VendorComponent objVendorComponent = new VendorComponent();
                    vendor = objVendorComponent.GetVendorSKUByID(Convert.ToInt32(Request.QueryString["VendorSKUID"]));
                    if (vendor != null)
                    {
                        txtvendorSku.Text = vendor.VendorSKU;

                        txtProductName.Text = vendor.ProductName;
                        ddlvendor.SelectedValue = vendor.VendorID.ToString();
                    }
                }
            }
        }


        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            int VendorID;
            tb_VendorSKU vendor = new tb_VendorSKU();
            VendorComponent objVendorComponent = new VendorComponent();
            if (!string.IsNullOrEmpty(Request.QueryString["VendorSKUID"]) && Convert.ToString(Request.QueryString["VendorSKUID"]) != "0")
            {
                vendor = objVendorComponent.GetVendorSKUByID(Convert.ToInt32(Request.QueryString["VendorSKUID"]));
                vendor.VendorSKU = txtvendorSku.Text.Trim();
                vendor.ProductName = txtProductName.Text.Trim();
                vendor.VendorID = Convert.ToInt32(ddlvendor.SelectedValue.ToString());


                VendorID = objVendorComponent.UpdateVendorSKU(vendor);
                if (VendorID > 0)
                    Response.Redirect("VendorSkuList.aspx?status=updated");
            }
            else
            {

               // vendor = objVendorComponent.GetVendorSKUByID(Convert.ToInt32(Request.QueryString["VendorSKUID"]));
                vendor.VendorSKU = txtvendorSku.Text.Trim();
                vendor.ProductName = txtProductName.Text.Trim();
                vendor.VendorID = Convert.ToInt32(ddlvendor.SelectedValue.ToString());
                vendor.Deleted = false;
                VendorID = objVendorComponent.CreateVendorSKu(vendor);
                if (VendorID > 0)
                    Response.Redirect("VendorSkuList.aspx?status=inserted");
            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("VendorSkuList.aspx");
        }

        /// <summary>
        /// Fills the Country Drop down
        /// </summary>
        private void FillVendor()
        {
            ddlvendor.Items.Clear();
            Solution.Data.VendorDAC objVendorDAC = new Data.VendorDAC();
            DataSet dsVendor = new DataSet();
            dsVendor = objVendorDAC.GetVendorList(1);
            if (dsVendor != null && dsVendor.Tables.Count > 0 && dsVendor.Tables[0].Rows.Count > 0)
            {
                ddlvendor.DataSource = dsVendor.Tables[0];
                ddlvendor.DataTextField = "Name";
                ddlvendor.DataValueField = "VendorID";
                ddlvendor.DataBind();
                ddlvendor.Items.Insert(0, new ListItem("Select DropShipper", "0"));
            }
            else
            {
                ddlvendor.DataSource = null;
                ddlvendor.DataBind();
            }
        }


    }
}