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
using System.Text;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class OldPurchaseOrderCart : Solution.UI.Web.BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "SetHeight123", "javascript:window.parent.document.getElementById('ContentPlaceHolder1_frmPurchaseOrder').removeAttribute('onload'); window.parent.document.getElementById('ContentPlaceHolder1_frmPurchaseOrder').height ='900px';", true);
            if (Session["AdminName"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "closescript", "window.close();window.location='../Login.aspx';", true);
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["ONo"] != null && Request.QueryString["PONo"] != null)
                {
                    divOrder.Visible = true;
                    Int32 OrderNumber = 0;
                    Int32 PONumber = 0;
                    Int32.TryParse(Request.QueryString["ONo"], out OrderNumber);
                    Int32.TryParse(Request.QueryString["PONo"], out PONumber);
                    BindOrder(OrderNumber, PONumber);
                    //FillOrderNotesLog();

                }
                else if (Request.QueryString["VendorID"] != null && Request.QueryString["PONo"] != null)
                {
                    Int32 VendorID = 0;
                    Int32.TryParse(Request.QueryString["VendorID"], out VendorID);
                    Int32 PONumber = 0;
                    Int32.TryParse(Request.QueryString["PoNo"], out PONumber);
                    BindVendorPo(VendorID, PONumber);
                }
                else
                {
                    divOrder.Visible = false;
                    lblMsg.Text = "<span style='color:red;'>Sorry. The Order Not Exists.</span>";
                }
            }
        }

        /// <summary>
        /// Binds the Vendor PO
        /// </summary>
        /// <param name="VendorID">int VendorID</param>
        /// <param name="PONumber">int PONumber</param>
        private void BindVendorPo(int VendorID, int PONumber)
        {
            try
            {
                DataSet ds = new DataSet();
                string StrQuery = " select  distinct po.vendorID,isnull(po.Adjustments,0) as Adjustments,isnull(por.ispaid,0) as ispaid,isnull(po.tax,0) as tax,isnull(po.shipping,0) as shipping,po.ordernumber,po.PONumber,p.Name,p.SKU as Style,por.Quantity,por.ProductID,po.OrderNumber,po.Notes,isnull(po.AdditionalCost,0) as AdditionalCost,isnull(po.poamount,0) as poamount,po.PODate, isnull(por.Price,0) as Price,tb_OrderedShoppingCartItems.VariantNames,tb_OrderedShoppingCartItems.VariantValues  " +
                                        " from tb_PurchaseOrderItems por INNER JOIN tb_PurchaseOrder po ON por.PONumber = po.PONumber INNER JOIN  tb_Product p ON por.ProductID = p.ProductID INNER JOIN tb_OrderedShoppingCartItems ON por.ProductID = tb_OrderedShoppingCartItems.RefProductID " +
                                        " Where po.PONumber = " + Convert.ToInt32(Request.QueryString["PONo"]) + " and po.vendorID =" + VendorID.ToString() + "";

                ds = CommonComponent.GetCommonDataSet(StrQuery.ToString());
                ltOrderNo.Text = "<b>PO Number </b>: PO-" + ds.Tables[0].Rows[0]["PONumber"].ToString();
                ltDate.Text = "<b>PO Date</b>:- " + ds.Tables[0].Rows[0]["PODate"].ToString();
                if (ds.Tables[0].Rows[0]["AdditionalCost"].ToString() != null)
                {
                    litAddCost.Visible = true;
                    litAddCost.Text = "<b>Additional Cost</b> : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["AdditionalCost"].ToString()).ToString("f2");
                }
                else
                {
                    litAddCost.Text = "<b>Additional Cost</b> : &nbsp;$0.00";
                }

                if (ds.Tables[0].Rows[0]["Tax"].ToString() != null)
                {
                    ltTax.Visible = true;
                    ltTax.Text = "<b>Sale Tax</b> : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["Tax"].ToString()).ToString("f2");
                }
                else
                {
                    ltTax.Text = "<b>Sale Tax</b> : &nbsp;$0.00";
                }
                if (ds.Tables[0].Rows[0]["Shipping"].ToString() != null)
                {
                    ltShipping.Visible = true;
                    ltShipping.Text = ": &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["Shipping"].ToString()).ToString("f2");
                }
                else
                {
                    ltShipping.Text = ": &nbsp;$0.00";
                }
                if (ds.Tables[0].Rows[0]["Adjustments"].ToString() != null)
                {
                    ltAdjustmetns.Visible = true;
                    if (Convert.ToDecimal(ds.Tables[0].Rows[0]["Adjustments"].ToString()) < 0)
                    {
                        decimal adjust = Convert.ToDecimal(ds.Tables[0].Rows[0]["Adjustments"].ToString());
                        ltAdjustmetns.Text = "<b>Adjustments Cost(-)</b> : &nbsp;(-) $" + (-adjust).ToString("f2");
                    }
                    else
                        ltAdjustmetns.Text = "<b>Adjustments Cost</b> : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["Adjustments"].ToString()).ToString("f2");
                }
                else
                {
                    ltAdjustmetns.Text = "<b>Adjustments Cost</b> : &nbsp;$0.00";
                }

                lbPOAmount.Text = "<b>PO Amount</b> : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["POAmount"].ToString()).ToString("f2");
                if (ds.Tables[0].Rows[0]["Notes"].ToString() != null && ds.Tables[0].Rows[0]["Notes"].ToString() != "")
                {
                    litNotes.Visible = true;
                    litNotes.Text = "<b>Notes</b> : " + System.Text.RegularExpressions.Regex.Replace(ds.Tables[0].Rows[0]["Notes"].ToString().Trim(), @"<[^>]*>", String.Empty);
                }
                else
                {
                    litNotes.Text = "<b>Notes</b> : &nbsp;";
                }
                string State = string.Empty;
                litProducts.Text = BindVendorCart(ds);
            }
            catch (System.Exception ex)
            {
            }
        }

        /// <summary>
        /// Binds the Vendor Cart
        /// </summary>
        /// <param name="ds">DataSet ds</param>
        /// <returns>Returns the output value as a string formate which contains HTML.</returns>
        public String BindVendorCart(DataSet ds)
        {
            StringBuilder Table = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Table.Append(" <table border='0' cellpadding='2' class='content-table border-td' cellspacing='1' width='700px'> ");
                Table.Append("<tbody><tr style='height:20px;color: #FFFFFF;'>");
                Table.Append("<th align='left' valign='middle' >Order Number</th>");
                Table.Append("<th align='left' valign='middle' style='width:50%' >Product</th>");
                Table.Append("<th align='center' valign='middle' style='width:15%' >SKU</th>");
                Table.Append("<th align='left' valign='middle' style='width:15%' >Tracking #</th>");
                Table.Append("<th align='center' valign='middle' style='width:10%'>Quantity</th>");

                Table.Append("</tr>");
                Decimal subotatal = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    Decimal NetPrice = Decimal.Zero;
                    Decimal Price = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["Price"].ToString()), 2);
                    Int32 Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString());

                    NetPrice = Math.Round((Price * Quantity), 2);

                    Table.Append("<tr class='list_table_cell_link' style='background:none;'>");

                    string ProductName = "";
                    ProductName = ds.Tables[0].Rows[i]["Name"].ToString();
                    Table.Append("<td align='left' valign='top'>" + ds.Tables[0].Rows[i]["OrderNumber"].ToString() + "</td>");
                    Table.Append("<td align='left' valign='top'><b>" + ProductName + "</b>");
                    string[] Names = ds.Tables[0].Rows[i]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] Values = ds.Tables[0].Rows[i]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int iLoopValues = 0; iLoopValues < Values.Length; iLoopValues++)
                    {
                        if (Values.Length == Names.Length)
                            Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
                        else
                            Table.Append("<br/> - " + Values[iLoopValues]);
                    }
                    Table.Append("</td>");
                    Table.Append("<td align='center' valign='top'>" + ds.Tables[0].Rows[i]["Style"].ToString() + "</td>");
                    Table.Append("<td align='left' valign='top'>" + ds.Tables[0].Rows[i]["TrackingNumber"].ToString() + "</td>");
                    Table.Append("<td valign='top' align='center'>" + ds.Tables[0].Rows[i]["Quantity"].ToString() + "</td>");
                    Table.Append(" </tr><tr><td colspan='6'><hr style='height:1px;width:700px;'/></td></tr>");
                    subotatal += (Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString()) * Price);
                }
                ltSubtotal.Text = "<b>PO SubTotal</b> : &nbsp;$" + subotatal.ToString("f2");

                Table.Append("</tbody></table>");
            }
            return Table.ToString();
        }

        /// <summary>
        /// Binds the order.
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="PONumber">int PONumber</param>
        private void BindOrder(int OrderNumber, int PONumber)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = GetOldPurchaseOrderItemsWarehouse(OrderNumber, Convert.ToInt32(Request.QueryString["PONo"]));
                ltOrderNo.Text = "<b>PONumber </b>: PO-" + ds.Tables[0].Rows[0]["PONumber"].ToString();
                ltOrderNumber.Text = "<b>Order Number </b>: " + OrderNumber.ToString();
                ltDate.Text = "<b>PO Date</b>:- " + ds.Tables[0].Rows[0]["PODate"].ToString();
                ltVendorName.Text = "<b>Vendor Name </b>: " + GetVendorName(OrderNumber, Convert.ToInt32(ds.Tables[0].Rows[0]["PONumber"].ToString()));

                string strEmail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT EmailTo FROM tb_Warehousemail WHERE PoNumber=" + ds.Tables[0].Rows[0]["PONumber"].ToString() + ""));
                if (!String.IsNullOrEmpty(strEmail.ToString()))
                {
                    ltVendorName.Text += "<br/><br/><b>Vendor Mail Sent To </b>: " + strEmail.ToString();
                }
                try
                {
                    string ShipToAddr = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VendorAddress as Address FROM tb_Warehousemail WHERE PoNumber=" + ds.Tables[0].Rows[0]["PONumber"].ToString() + ""));
                    if (!string.IsNullOrEmpty(ShipToAddr.ToString()))
                    {
                        litShipToAddr.Visible = true;
                        litShipToAddr.Text = "<b>Ship To Address </b> :&nbsp;" + ShipToAddr.ToString();
                    }
                    else { litShipToAddr.Text = ": N/A"; }
                }
                catch { }

                if (ds.Tables[0].Rows[0]["AdditionalCost"].ToString() != null)
                {
                    litAddCost.Visible = true;
                    litAddCost.Text = "<b>Additional Cost</b> : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["AdditionalCost"].ToString()).ToString("f2");
                }
                else
                {
                    litAddCost.Text = "<b>Additional Cost</b> : &nbsp;$0.00";
                }
                if (ds.Tables[0].Rows[0]["Tax"].ToString() != null)
                {
                    ltTax.Visible = true;
                    ltTax.Text = "<span style='width:200px;'><b>Sale Tax</b></span> : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["Tax"].ToString()).ToString("f2");
                }
                else
                {
                    ltTax.Text = "b>Sale Tax</b> : &nbsp;$0.00";
                }
                if (ds.Tables[0].Rows[0]["Shipping"].ToString() != null)
                {
                    ltShipping.Visible = true;
                    ltShipping.Text = "<b>Shipping</b> : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["Shipping"].ToString()).ToString("f2");
                }
                else
                {
                    ltShipping.Text = ": &nbsp;$0.00";
                }
                if (ds.Tables[0].Rows[0]["Adjustments"].ToString() != null)
                {
                    ltAdjustmetns.Visible = true;
                    if (Convert.ToDecimal(ds.Tables[0].Rows[0]["Adjustments"].ToString()) < 0)
                    {
                        decimal adjust = Convert.ToDecimal(ds.Tables[0].Rows[0]["Adjustments"].ToString());
                        ltAdjustmetns.Text = "<b>Adjustments Cost(-)</b> : &nbsp;(-) $" + (-adjust).ToString("f2");
                    }
                    else
                        ltAdjustmetns.Text = "<b>Adjustments Cost</b> : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["Adjustments"].ToString()).ToString("f2");
                }
                else
                {
                    ltAdjustmetns.Text = "<b>Adjustments Cost</b> : &nbsp;$0.00";
                }

                lbPOAmount.Text = "<b>PO Amount</b> : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["POAmount"].ToString()).ToString("f2");

                if (ds.Tables[0].Rows[0]["Notes"].ToString() != null && ds.Tables[0].Rows[0]["Notes"].ToString() != "")
                {
                    litNotes.Visible = true;
                    litNotes.Text = "<b>Notes</b> : " + System.Text.RegularExpressions.Regex.Replace(ds.Tables[0].Rows[0]["Notes"].ToString().Trim(), @"<[^>]*>", String.Empty);
                }
                else
                {
                    litNotes.Text = "<b>Notes</b> : &nbsp;";
                }
                DataSet dsStatus = new DataSet();
                dsStatus = CommonComponent.GetCommonDataSet("SELECT Description FROM tb_Timestamplog WHERE refnumber=" + Convert.ToInt32(Request.QueryString["PONo"]) + " AND Createdby=-1 And Type<>27 AND Description is not null");
                if (dsStatus.Tables[0].Rows.Count > 0)
                {
                    ltstatus.Visible = true;
                    ltstatus.Text = "<td style='width: 108px; padding-left: 10px;' align='right' valign='top'><b>PO Problem </b></td><td>:&nbsp;" + dsStatus.Tables[0].Rows[0]["Description"].ToString() + " </td>";
                }
                dsStatus = CommonComponent.GetCommonDataSet("SELECT Description FROM tb_Timestamplog WHERE refnumber=" + Convert.ToInt32(Request.QueryString["PONo"]) + " AND Createdby=-1 And type=27 AND Description is not null");
                if (dsStatus.Tables[0].Rows.Count > 0)
                {
                    ltPoStatus.Visible = true;
                    ltPoStatus.Text = "<tr valign='top'><td><b>PO Status Note</b> :&nbsp;" + dsStatus.Tables[0].Rows[0]["Description"].ToString() + " </td></tr>";
                }
                string State = string.Empty;
                litProducts.Text = BindCart(ds);
            }
            catch (System.Exception ex)
            {
            }
        }

        /// <summary>
        /// Gets the Old Purchase Order Items for Warehouse
        /// </summary>
        /// <param name="ONO">int ONO</param>
        /// <param name="PONO">int PONO</param>
        /// <returns>Returns the Dataset of Order Items List</returns>
        public DataSet GetOldPurchaseOrderItemsWarehouse(Int32 ONO, Int32 PONO)
        {
            return CommonComponent.GetCommonDataSet(@" SELECT DISTINCT 
                      (CASE WHEN (isnull(por.isshipped, 0) = 0) THEN 'Pending' ELSE 'Shipped' END) AS Shipped, ISNULL(po.Adjustments, 0) AS Adjustments,  ISNULL(por.ispaid, 0) AS ispaid,
                      ISNULL(po.Tax, 0) AS tax, ISNULL(po.Shipping, 0) AS shipping, po.PONumber, p.Name, p.SKU AS Style, por.Quantity, por.ProductID, po.OrderNumber, 
                      po.Notes, ISNULL(po.AdditionalCost, 0) AS AdditionalCost, ISNULL(po.PoAmount, 0) AS poamount, po.PODate, ISNULL(por.Price, 0) AS Price,por.TrackingNumber
                      FROM dbo.tb_PurchaseOrderItems AS por INNER JOIN
                      dbo.tb_PurchaseOrder AS po ON por.PONumber = po.PONumber INNER JOIN
                      dbo.tb_Product AS p ON por.ProductID = p.ProductID
                      WHERE (po.OrderNumber = " + ONO + ") AND (por.PONumber = " + PONO + ")");
        }

        /// <summary>
        /// Binds the Cart
        /// </summary>
        /// <param name="ds">Dataset ds</param>
        /// <returns>Returns the output value as a string formate which contains HTML</returns>
        public String BindCart(DataSet ds)
        {
            StringBuilder Table = new StringBuilder();
            Decimal subotatal = 0;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Table.Append(" <table border='0' cellpadding='2' class='content-table border-td' cellspacing='1' class='table' width='100%'> ");
                Table.Append("<tbody><tr style='height:20px;color: #FFFFFF;'>");
                Table.Append("<th align='left' valign='middle' style='width:50%' >Product</th>");
                Table.Append("<th align='center' valign='middle' style='width:15%' >SKU</th>");
                Table.Append("<th align='center' valign='middle' style='width:15%' >Tracking #</th>");
                Table.Append("<th align='center' valign='middle' style='width:10%'>Quantity</th>");
                Table.Append("<th style='text-align: center;width:15%;'>Status</th>");

                Table.Append("</tr>");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Decimal NetPrice = Decimal.Zero;
                    Decimal Price = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["Price"].ToString()), 2);
                    Int32 Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString());
                    NetPrice = Math.Round((Price * Quantity), 2);
                    Table.Append("<tr class='list_table_cell_link' style='background:none;'>");
                    string ProductName = "";
                    ProductName = ds.Tables[0].Rows[i]["Name"].ToString();
                    Table.Append("<td align='left' valign='top'><b>" + ProductName + "</b>");
                    Table.Append("</td>");
                    Table.Append("<td align='center' valign='top'>" + ds.Tables[0].Rows[i]["Style"].ToString() + "</td>");
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TrackingNumber"].ToString()) && ds.Tables[0].Rows[i]["TrackingNumber"].ToString() != "0")
                        Table.Append("<td align='center' valign='top'>" + ds.Tables[0].Rows[i]["TrackingNumber"].ToString() + "</td>");
                    else Table.Append("<td align='center' valign='top'>-------</td>");
                    Table.Append("<td valign='top' align='center'>" + ds.Tables[0].Rows[i]["Quantity"].ToString() + "</td>");
                    if (ds.Tables[0].Rows[i]["ispaid"].ToString() == "1")
                    {
                        Table.Append("<td  valign='top' align='center'>Paid</td>");
                    }
                    else
                    {
                        Table.Append("<td  valign='top' align='center'> " + ds.Tables[0].Rows[i]["Shipped"].ToString() + "</td>");
                    }
                    Table.Append(" </tr></tr>");

                    subotatal += (Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString()) * Price);
                }
                ltSubtotal.Text = "<b>PO SubTotal</b> : &nbsp;$" + subotatal.ToString("f2");
                Table.Append("</tbody></table>");
            }
            return Table.ToString();
        }

        /// <summary>
        /// Gets the Name of the Vendors
        /// </summary>
        /// <param name="ONO">int ONO</param>
        /// <param name="PONO">int PONO</param>
        /// <returns>Returns the Names of Vendors as a String</returns>
        public String GetVendorName(Int32 ONO, Int32 PONO)
        {
            return Convert.ToString(CommonComponent.GetScalarCommonData("select Name as VendorName from dbo.tb_Vendor v  " +
                            " inner join dbo.tb_PurchaseOrder po on po.vendorid=v.vendorid  " +
                            "   where po.ponumber=" + PONO + " and po.ordernumber=" + ONO + " and v.Active=1 and v.deleted=0"));
        }
    }
}