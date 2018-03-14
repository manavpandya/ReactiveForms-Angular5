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
    public partial class VendorQuote : BasePage
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
        /// Binds the cart of Vendor Quote.
        /// </summary>
        /// <param name="PId">string PId</param>
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
                // BindVendors();
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
                CheckBoxList chkVendor = (CheckBoxList)e.Row.FindControl("chkVendor");
                if (lblProductID != null && chkVendor != null && !string.IsNullOrEmpty(lblProductID.Text.Trim()))
                {
                    DataSet ds = new DataSet();
                    ds = CommonComponent.GetCommonDataSet("select distinct VendorID,Name,Email from tb_Vendor where ISNULL(deleted,0)=0 and ISNULL(Active,1)=1 and ISNULL(tb_Vendor.IsDropshipper,0) = 1 and ISNULL(Email,'') <> '' ");
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            try
                            {
                                System.Web.UI.WebControls.ListItem ls = new System.Web.UI.WebControls.ListItem();
                                ls.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                                ls.Value = ds.Tables[0].Rows[i]["VendorID"].ToString();
                                ls.Attributes.Add("title", ds.Tables[0].Rows[i]["Email"].ToString());
                                chkVendor.Items.Add(ls);
                            }
                            catch { }
                        }
                    }
                }
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

            foreach (GridViewRow row in grdCart.Rows)
            {
                String VendorIDs = "";
                Boolean VendorValidate = false;
                Label lblProductID = (Label)row.FindControl("lblProductID");
                Label lblSKU = (Label)row.FindControl("lblSKU");
                TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                TextBox txtProductOption = (TextBox)row.FindControl("txtProductOption");
                Label lblProductName = (Label)row.FindControl("lblProductName");
                Label lblPrice = (Label)row.FindControl("lblPrice");
                CheckBoxList chkVendor = (CheckBoxList)row.FindControl("chkVendor");
                Int32 pid = 0, qty = 0;
                Decimal Price = 0;
                Int32.TryParse(lblProductID.Text.Trim(), out pid);
                Int32.TryParse(txtQuantity.Text.Trim(), out qty);
                Decimal.TryParse(lblPrice.Text.Trim(), out Price);

                if (chkVendor != null)
                {
                    for (int i = 0; i < chkVendor.Items.Count; i++)
                    {
                        if (chkVendor.Items[i].Selected)
                        {
                            if (i > 0 && i % 3 == 0)
                            {
                                chkVendor.Items[i].Attributes.Add("style", "border-width:0px;border-style:none;");
                            }

                            VendorIDs += chkVendor.Items[i].Value + ",";
                            VendorValidate = true;

                            if (!Vendors.Contains(chkVendor.Items[i].Value + ",") && !Vendors.Contains("," + chkVendor.Items[i].Value + ","))
                                Vendors += chkVendor.Items[i].Value + ",";
                        }
                    }
                }
                if (VendorValidate)
                {
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
                }
            }
            string notes = txtDescription.Text.Trim();
            DataView dvVendor = dtCart.DefaultView;
            if (string.IsNullOrEmpty(Vendors.ToString()))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", "alert('Please select Vendors.');", true);
                return;
            }
            if (dtCart.Rows.Count > 0)
            {
                Session["VendorQuoteCart"] = dtCart;
                Session["VendorQuoteNotes"] = notes;
                //Response.Redirect("VendorQuoteMailFormat.aspx?Ono=" + Request.QueryString["ONo"].ToString() + "&MailTemplate=" + drpTemplate.SelectedValue.ToString() + "&Vendors=" + Vendors + "&mode=view");
                Response.Redirect("VendorQuoteMailFormat.aspx?Vendors=" + Vendors + "&mode=view");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", "alert('Please select your Vendors for your Products.');", true);
                return;
            }
        }
    }
}