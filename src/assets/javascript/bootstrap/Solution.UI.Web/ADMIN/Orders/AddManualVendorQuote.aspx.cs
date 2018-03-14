using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class AddManualVendorQuote : BasePage
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
                if (Request.QueryString["PID"] != null)
                    BindCart(Request.QueryString["PID"].ToString());
                btnPreview.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/preview-vendor-quote.gif";
            }
        }

        /// <summary>
        /// Binds the Vendors
        /// </summary>
        private void BindVendors()
        {
            ddlVendor.Items.Clear();
            int storeid = AppConfig.StoreID;
            if (storeid == 0)
                storeid = 1;
            DataSet dsVen = new DataSet();
            dsVen = CommonComponent.GetCommonDataSet("select VendorId AS id,tb_Vendor.* From tb_Vendor where tb_Vendor.Deleted = 0 order by tb_Vendor.VendorId");
            ddlVendor.DataSource = dsVen;
            ddlVendor.DataTextField = "Name";
            ddlVendor.DataValueField = "id";
            ddlVendor.DataBind();
            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select Vendor", "0");
            li.Selected = true;
            ddlVendor.Items.Insert(0, li);
            ddlVendor.SelectedItem.Text = "Select Vendor";
        }

        /// <summary>
        /// Binds the cart for display.
        /// </summary>
        /// <param name="PId">int PId</param>
        private void BindCart(string PId)
        {
            if (PId.IndexOf(",") > -1)
            {
                PId = PId.Substring(0, PId.Length - 1);
            }
            DataSet dsBindCart = new DataSet();
            dsBindCart = CommonComponent.GetCommonDataSet("Select tb_WareHouseProduct.*,ISNULL(tb_Product.Inventory,0) as AvailableQuantity from tb_WareHouseProduct " +
                                        " inner Join tb_Product on tb_Product.ProductID=tb_WareHouseProduct.ProductID WHERE tb_WareHouseProduct.productid in (" + PId + ")");
            if (dsBindCart != null && dsBindCart.Tables.Count > 0 && dsBindCart.Tables[0].Rows.Count > 0)
            {
                grdCart.DataSource = dsBindCart;
                grdCart.DataBind();
                BindVendors();
            }
            else
            {
                grdCart.DataSource = null;
                grdCart.DataBind();
            }
        }

        /// <summary>
        /// Cart Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdCart_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
            }
        }

        /// <summary>
        ///  Preview Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnPreview_Click(object sender, ImageClickEventArgs e)
        {
            string Vendors = "";
            DataTable dtCart = new DataTable("VendorCart");
            dtCart.Columns.Add(new DataColumn("ProductID", typeof(Int32)));
            dtCart.Columns.Add(new DataColumn("Name", typeof(String)));
            dtCart.Columns.Add(new DataColumn("ProductOption", typeof(String)));
            dtCart.Columns.Add(new DataColumn("SKU", typeof(String)));
            dtCart.Columns.Add(new DataColumn("Quantity", typeof(Int32)));
            dtCart.Columns.Add(new DataColumn("Price", typeof(String)));
            dtCart.Columns.Add(new DataColumn("VendorIds", typeof(String)));

            Int32 VendorQuoteReqId = 0;

            if (VendorQuoteReqId == 0)
                VendorQuoteReqId = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO [dbo].[tb_VendorQuoteRequest]([RequestedOn],[MailLogid],[Notes])VALUES(GETDATE(),0,''); Select SCOPE_IDENTITY();"));

            foreach (GridViewRow row in grdCart.Rows)
            {
                String VendorIDs = "";

                Label lblProductID = (Label)row.FindControl("lblProductID");
                Label lblSKU = (Label)row.FindControl("lblSKU");
                TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                TextBox txtProductOption = (TextBox)row.FindControl("txtProductOption");
                Label lblProductName = (Label)row.FindControl("lblProductName");
                Label lblPrice = (Label)row.FindControl("lblPrice");
                DropDownList drpAvailDays = (DropDownList)row.FindControl("drpAvailDays");

                Int32 pid = 0, qty = 0;
                Decimal Price = 0;
                Int32.TryParse(lblProductID.Text.Trim(), out pid);
                Int32.TryParse(txtQuantity.Text.Trim(), out qty);
                Decimal.TryParse(lblPrice.Text.Trim(), out Price);

                Vendors = Convert.ToString(ddlVendor.SelectedValue);

                int VendorQuoteid = 0;
                string VendorQuoteids = string.Empty;

                if (qty <= 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", "alert('Please enter valid Quanity for " + lblProductName.Text.ToString() + ".');", true);
                    return;
                }
                DataRow dr = dtCart.NewRow();
                dr["ProductID"] = pid;
                dr["Name"] = lblProductName.Text.Trim();
                dr["ProductOption"] = txtProductOption.Text.Trim();
                dr["SKU"] = lblSKU.Text.Trim();
                dr["Quantity"] = qty;
                dr["Price"] = Price;
                dr["VendorIds"] = VendorIDs;
                dtCart.Rows.Add(dr);

                VendorComponent objvendor = new VendorComponent();
                if (VendorQuoteReqId > 0)
                {
                    VendorQuoteid = objvendor.SaveVendorQuoteRequest(VendorQuoteReqId, Convert.ToInt32(ddlVendor.SelectedValue), Convert.ToInt32(pid), Convert.ToInt32(qty), Convert.ToString(lblProductName.Text.Trim()), Convert.ToString(txtProductOption.Text.Trim().ToString()), "");
                }

                objvendor.SaveVendorQuoteReply(Convert.ToInt32(VendorQuoteReqId), Convert.ToInt32(ddlVendor.SelectedValue), Convert.ToInt32(lblProductID.Text.Trim()), qty, Convert.ToString(lblProductName.Text.Trim()), Convert.ToString(txtProductOption.Text.Trim()), txtNotes.Text.ToString(), Convert.ToDecimal(Price), Convert.ToInt32(drpAvailDays.SelectedValue.ToString()), txtLocation.Text.ToString().Replace("'", "''"));
            }
            DataView dvVendor = dtCart.DefaultView;
            if (ddlVendor.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", "alert('Please select Vendors.');", true);
                return;
            }
            if (dtCart.Rows.Count > 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", "alert('New Quotes Added Successfully.');", true);
                Response.Redirect("WareHousePO.aspx");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", "alert('Please select your Vendors for your Products.');", true);
                return;
            }
        }

    }
}