using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class PurchaseOrder : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "SetHeight123", "javascript:window.parent.document.getElementById('ContentPlaceHolder1_frmPurchaseOrder').removeAttribute('onload'); window.parent.document.getElementById('ContentPlaceHolder1_frmPurchaseOrder').height ='900px';", true);
            if (!IsPostBack)
            {
                Int32 OrderNumber = 0;
                Int32.TryParse(Request.QueryString["ONo"].ToString(), out OrderNumber);
                if (OrderNumber <= 0)
                {
                    litProducts.Text = "<span style='color:red;'>Sorry. The OrderNumber specified is wrong.</span>";
                    return;
                }
                string ONo = SecurityComponent.Encrypt(Request.QueryString["ONo"].ToString());
                AtagBack.HRef = "POOrder.aspx?ono=" + Server.UrlEncode(ONo) + "";
                if (Request.QueryString["PID"] != null)
                    BindCart(OrderNumber);
                btnPreview.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/preview-po.gif";
            }
        }

        /// <summary>
        /// Binds the Cart for the Purchase Order
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        private void BindCart(Int32 OrderNumber)
        {
            string pids = "";
            string customid = "";
            if (Request.QueryString["PID"] != null)
                pids = Request.QueryString["PID"].ToString();
            if (Request.QueryString["customid"] != null)
                customid = Request.QueryString["customid"].ToString();

            DataSet DsCItems = new DataSet();
            VendorComponent objvendor = new VendorComponent();
            DsCItems = objvendor.GetPurchaseOrder(Convert.ToInt32(OrderNumber), pids, customid);
            if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
            {
                grdCart.DataSource = DsCItems;
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
        /// Binds the Vendors into Drop down List
        /// </summary>
        private void BindVendors()
        {
            ddlVendor.Items.Clear();
            int storeid = AppConfig.StoreID;
            if (storeid == 0)
                storeid = 1;
            DataSet dsVen = new DataSet();
            dsVen = CommonComponent.GetCommonDataSet("select ROW_NUMBER() OVER (ORDER BY VendorId ASC) AS id,tb_Vendor.* From tb_Vendor where ISNULL(tb_Vendor.Deleted,0) = 0 and ISNULL(tb_Vendor.IsDropshipper,0) = 1 order by tb_Vendor.VendorId");
            ddlVendor.DataSource = dsVen;
            ddlVendor.DataTextField = "Name";
            ddlVendor.DataValueField = "id";
            ddlVendor.DataBind();
            ListItem li = new ListItem("Select Vendor", "0");
            li.Selected = true;
            ddlVendor.Items.Insert(0, li);
            ddlVendor.SelectedItem.Text = "Select Vendor";
        }

        /// <summary>
        /// Binds the vendor's Mail format.
        /// </summary>
        /// <param name="VendorId">int VendorId</param>
        private void BindVendorsMainFormat(Int32 VendorId)
        {
            Int32 storeid = 0;
            storeid = AppConfig.StoreID;
            if (storeid == 0)
                storeid = 1;
            DataSet dsVenMail = new DataSet();
            dsVenMail = CommonComponent.GetCommonDataSet("Select TemplateID,Label as  Template from tb_EmailTemplate where StoreID=" + storeid.ToString() + " and ISNULL(IsPOTemplate,0)=1 ");
            ddlMailTemplate.DataTextField = "Template";
            ddlMailTemplate.DataValueField = "TemplateID";
            ddlMailTemplate.DataSource = dsVenMail;
            ddlMailTemplate.DataBind();
        }

        /// <summary>
        /// Vendor Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindVendorsMainFormat(Convert.ToInt32(ddlVendor.SelectedValue));
            }
            catch { }
        }

        /// <summary>
        /// Cart Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvCart_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                    Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                    Label lblName = (Label)e.Row.FindControl("lblName");
                    TextBox txtQty = (TextBox)e.Row.FindControl("txtQuantity");
                    TextBox txtPrice = (TextBox)e.Row.FindControl("txtPrice");

                    lblPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(lblPrice.Text), 2));
                    txtPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(lblPrice.Text), 2));
                    HtmlInputHidden hdnname = (HtmlInputHidden)e.Row.FindControl("vname");
                    HtmlInputHidden hdnvalue = (HtmlInputHidden)e.Row.FindControl("vvalue");
                    string variant = "";
                    if (txtQty != null && !string.IsNullOrEmpty(txtQty.Text.ToString()))
                    {
                        Int32 PurchaseQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Quantity,0) As PurchaseOrderQty FROM tb_PurchaseOrderItems WHERE productid =" + lblProductID.Text.ToString() + " and ponumber in (SELECT ponumber from tb_PurchaseOrder where Ordernumber=" + Request.QueryString["ONo"].ToString() + ")"));
                        if (PurchaseQty > 0)
                        {
                            Int32 Qty = Convert.ToInt32(txtQty.Text) - PurchaseQty;
                            txtQty.Text = Qty.ToString();
                        }
                    }

                    if (hdnname.Value != null && hdnname.Value.ToString() != "")
                    {

                        string[] names = hdnname.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] values = hdnvalue.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (names.Length == values.Length)
                        {
                            for (int iLoopValues = 0; iLoopValues < values.Length && names.Length == values.Length; iLoopValues++)
                            {
                                variant += "<br/>" + names[iLoopValues] + ": " + values[iLoopValues];
                            }
                        }
                        else
                        {
                            for (int iLoopValues = 0; iLoopValues < values.Length; iLoopValues++)
                            {
                                variant += "<br/>  - " + values[iLoopValues];
                            }
                        }
                    }
                    lblName.Text += variant.ToString();
                }
                catch { }
            }
        }

        /// <summary>
        ///  Preview Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnPreview_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlMailTemplate.SelectedIndex == -1)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgTest01", "alert('Please select Mail Template for Preview.');", true);
                return;
            }
            else
            {
                Session["PONotes"] = "";
                string Notes = string.Empty;
                if (!string.IsNullOrEmpty(txtDescription.Text.Trim()))
                    Notes = txtDescription.Text.Trim();
                Decimal AdditionalCost = 0, subtotal = 0;
                Decimal Adjustments = 0;
                Decimal Tax = 0;
                Decimal Shipping = 0;
                DataTable dtCart = new DataTable("VendorCart");
                dtCart.Columns.Add(new DataColumn("ProductID", typeof(Int32)));
                dtCart.Columns.Add(new DataColumn("Name", typeof(String)));
                dtCart.Columns.Add(new DataColumn("SKU", typeof(String)));
                dtCart.Columns.Add(new DataColumn("Quantity", typeof(Int32)));
                dtCart.Columns.Add(new DataColumn("Price", typeof(Decimal)));
                dtCart.Columns.Add(new DataColumn("PoQuantity", typeof(Int32)));
                dtCart.Columns.Add(new DataColumn("MaxQuantity", typeof(Int32)));
                dtCart.Columns.Add(new DataColumn("Customcartid", typeof(String)));

                foreach (GridViewRow row in grdCart.Rows)
                {
                    try
                    {
                        Label lblProductID = (Label)row.FindControl("lblProductID");
                        Label lblName = (Label)row.FindControl("lblProductName");
                        Label lblSKU = (Label)row.FindControl("lblSKU");
                        Label lblQty = (Label)row.FindControl("lblQty");
                        TextBox txtQty = (TextBox)row.FindControl("txtQuantity");
                        Label lblPrice = (Label)row.FindControl("lblPrice");
                        TextBox txtPrice = (TextBox)row.FindControl("txtPrice");
                        Int32 pid = 0, qty = 0;
                        Decimal Price = 0;
                        Int32.TryParse(lblProductID.Text.Trim(), out pid);
                        Int32.TryParse(txtQty.Text.Trim(), out qty);
                        Decimal.TryParse(txtPrice.Text, out Price);
                        // Change From here when Demo System
                        if (qty <= 0)
                        {
                            string QtyMsg = "Please enter valid Quanity for : " + lblName.Text + ". ";
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('" + QtyMsg + "');", true);
                            return;
                        }
                        Int32 PurchaseOrderQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT (isnull(sum(dbo.tb_OrderedShoppingCartItems.Quantity),0) - isnull(sum(a.Poqty),0)) as Poqty FROM " +
                                                " (SELECT  ISNULL(SUM(dbo.tb_PurchaseOrderItems.Quantity),0)  as Poqty ,tb_PurchaseOrderItems.ProductID,tb_Order.ShoppingCardID " +
                                                " FROM dbo.tb_PurchaseOrder  " +
                                                " INNER JOIN dbo.tb_Order ON dbo.tb_PurchaseOrder.OrderNumber = dbo.tb_Order.OrderNumber  " +
                                                " INNER JOIN dbo.tb_PurchaseOrderItems ON dbo.tb_PurchaseOrder.PONumber = dbo.tb_PurchaseOrderItems.PONumber " +
                                                " WHERE dbo.tb_PurchaseOrderItems.ProductID=" + pid + " AND dbo.tb_PurchaseOrder.OrderNumber=" + Request.QueryString["ONo"].ToString() + " GROUP BY tb_PurchaseOrderItems.ProductID,tb_Order.ShoppingCardID) as A " +
                                                " RIGHT OUTER JOIN dbo.tb_OrderedShoppingCartItems ON A.ShoppingCardID = dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID  " +
                                                " AND A.ProductID = dbo.tb_OrderedShoppingCartItems.RefProductID  " +
                                                " WHERE dbo.tb_OrderedShoppingCartItems.REfProductID=" + pid + " AND tb_OrderedShoppingCartItems.OrderedShoppingCartID in (SELECT ShoppingCardID FROM tb_Order WHERE OrderNumber=" + Request.QueryString["ONo"].ToString() + ")"));

                        if (qty > PurchaseOrderQty)
                        {
                            string QtyMsg = "Exceed maximum Quantity.\\nPlease enter " + PurchaseOrderQty + " Quanity or less for : " + lblName.Text + ".";
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg02", "alert('" + QtyMsg + "');", true);
                            return;
                        }

                        DataRow dr = dtCart.NewRow();
                        dr["ProductID"] = pid;
                        dr["Name"] = lblName.Text.Trim();
                        dr["SKU"] = lblSKU.Text.Trim();
                        dr["Quantity"] = qty;
                        dr["Price"] = Convert.ToDecimal(lblPrice.Text.Replace("$", ""));
                        subtotal += Convert.ToDecimal(lblPrice.Text.Replace("$", ""));
                        dr["PoQuantity"] = qty;
                        dr["MaxQuantity"] = qty;
                        dr["Customcartid"] = "0";
                        dtCart.Rows.Add(dr);
                    }
                    catch { }
                }
                DataView dvVendor = dtCart.DefaultView;
                if (dvVendor.Count > 0)
                {
                    Session["VendorCart"] = null;
                    Session["VendorCart"] = dtCart;
                    Session["PONotes"] = Notes.ToString();
                    Response.Redirect("POVendormailFormat.aspx?Ono=" + Request.QueryString["ONo"].ToString() + "&VendorID=" + ddlVendor.SelectedValue.ToString() + "&MailTemplate=" + ddlMailTemplate.SelectedValue.ToString() + "&AdditionalCost=" + AdditionalCost + "&Notes=1&Adjustments=" + Adjustments + "&Tax=" + Tax + "&Shipping=" + Shipping + "&Mode=view");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", "alert('Please select your Vendors for your Products.');", true);
                }
            }
        }
    }
}