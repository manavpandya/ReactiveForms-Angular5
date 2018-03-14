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
    public partial class OldWareHousePOOrderCart : Solution.UI.Web.BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminName"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "closescript", "window.close();window.location='../Login.aspx';", true);
            }
            if (!IsPostBack)
            {
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                if (Request.QueryString["ONo"] != null && Request.QueryString["PONo"] != null)
                {
                    divOrder.Visible = true;
                    Int32 OrderNumber = 0;
                    Int32 PONumber = 0;
                    Int32.TryParse(Request.QueryString["ONo"], out OrderNumber);
                    Int32.TryParse(Request.QueryString["PONo"], out PONumber);
                    BindOrder(OrderNumber, PONumber);
                    FillOrderNotesLog();

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
        /// Binds the Vendor Purchase Order Details
        /// </summary>
        /// <param name="VendorID">int VendorID</param>
        /// <param name="PONumber">in PONumber</param>
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
                ltDate.Text = "<b>PO Date</b> : " + ds.Tables[0].Rows[0]["PODate"].ToString();
                if (ds.Tables[0].Rows[0]["AdditionalCost"].ToString() != null)
                {
                    litAddCost.Visible = true;
                    litAddCost.Text = ": &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["AdditionalCost"].ToString()).ToString("f2");
                }
                else
                {
                    litAddCost.Text = ": &nbsp;$0.00";
                }

                if (ds.Tables[0].Rows[0]["Tax"].ToString() != null)
                {
                    ltTax.Visible = true;
                    ltTax.Text = ": &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["Tax"].ToString()).ToString("f2");
                }
                else
                {
                    ltTax.Text = ": &nbsp;$0.00";
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
                        ltAdjustmetns.Text = " : &nbsp;(-) $" + (-adjust).ToString("f2");
                    }
                    else
                        ltAdjustmetns.Text = " : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["Adjustments"].ToString()).ToString("f2");
                }
                else
                {
                    ltAdjustmetns.Text = ": &nbsp;$0.00";
                }

                lbPOAmount.Text = " : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["POAmount"].ToString()).ToString("f2");
                if (ds.Tables[0].Rows[0]["Notes"].ToString() != null && ds.Tables[0].Rows[0]["Notes"].ToString() != "")
                {
                    litNotes.Visible = true;
                    litNotes.Text = " : " + ds.Tables[0].Rows[0]["Notes"].ToString();
                }
                else
                {
                    litNotes.Text = " : &nbsp;";
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
        /// <returns>Returns the output value as a string formate which contains HTML</returns>
        public String BindVendorCart(DataSet ds)
        {
            StringBuilder Table = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Table.Append(" <table border='0' cellpadding='2' class='content-table border-td' cellspacing='1' width='700px'> ");
                Table.Append("<tbody><tr style='height:20px;color: #FFFFFF;'>");
                Table.Append("<th align='left' valign='middle' >Order Number</th>");
                Table.Append("<th align='left' valign='middle' style='width:50%' >Product</th>");
                Table.Append("<th align='left' valign='middle' style='width:15%' >SKU</th>");
                Table.Append("<th align='center' valign='middle' style='width:15%' >Tracking #</th>");
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
                    Table.Append("<td align='left' valign='top'>" + ProductName + "");
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
                    Table.Append("<td align='left' valign='top'>" + ds.Tables[0].Rows[i]["Style"].ToString() + "</td>");
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TrackingNumber"].ToString()) && ds.Tables[0].Rows[i]["TrackingNumber"].ToString() != "0")
                        Table.Append("<td align='center' valign='top'>" + ds.Tables[0].Rows[i]["TrackingNumber"].ToString() + "</td>");
                    else Table.Append("<td align='center' valign='top'>-------</td>");
                    Table.Append("<td valign='top' align='center'>" + ds.Tables[0].Rows[i]["Quantity"].ToString() + "</td>");
                    Table.Append(" </tr><tr><td colspan='6'><hr style='height:1px;width:700px;'/></td></tr>");
                    subotatal += (Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString()) * Price);
                }
                ltSubtotal.Text = ": &nbsp;$" + subotatal.ToString("f2");

                Table.Append("</tbody></table>");
            }
            return Table.ToString();
        }

        /// <summary>
        /// Binds the Order
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
                ltDate.Text = "<b>PO Date</b> : " + ds.Tables[0].Rows[0]["PODate"].ToString();
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
                        litShipToAddr.Text = ":&nbsp;" + ShipToAddr.ToString();
                    }
                    else { litShipToAddr.Text = ": N/A"; }
                }
                catch { }

                if (ds.Tables[0].Rows[0]["AdditionalCost"].ToString() != null)
                {
                    litAddCost.Visible = true;
                    litAddCost.Text = ": &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["AdditionalCost"].ToString()).ToString("f2");
                }
                else
                {
                    litAddCost.Text = ": &nbsp;$0.00";
                }
                if (ds.Tables[0].Rows[0]["Tax"].ToString() != null)
                {
                    ltTax.Visible = true;
                    ltTax.Text = ": &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["Tax"].ToString()).ToString("f2");
                }
                else
                {
                    ltTax.Text = ": &nbsp;$0.00";
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
                        ltAdjustmetns.Text = " : &nbsp;(-) $" + (-adjust).ToString("f2");
                    }
                    else
                        ltAdjustmetns.Text = " : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["Adjustments"].ToString()).ToString("f2");
                }
                else
                {
                    ltAdjustmetns.Text = ": &nbsp;$0.00";
                }

                lbPOAmount.Text = " : &nbsp;$" + Convert.ToDecimal(ds.Tables[0].Rows[0]["POAmount"].ToString()).ToString("f2");

                if (ds.Tables[0].Rows[0]["Notes"].ToString() != null && ds.Tables[0].Rows[0]["Notes"].ToString() != "")
                {
                    litNotes.Visible = true;
                    litNotes.Text = " : &nbsp;" + ds.Tables[0].Rows[0]["Notes"].ToString();
                }
                else
                {
                    litNotes.Text = " : &nbsp;";
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
                    ltPoStatus.Text = "<td style='width: 108px; padding-left: 10px;' align='right'><b>PO Status Note </b></td><td>:&nbsp;" + dsStatus.Tables[0].Rows[0]["Description"].ToString() + " </td>";
                }
                string State = string.Empty;
                litProducts.Text = BindCart(ds);
            }
            catch (System.Exception ex)
            {
            }
        }

        /// <summary>
        /// Binds the Cart
        /// </summary>
        /// <param name="ds">DataSet ds</param>
        /// <returns>Returns the output value as a string formate which contains HTML.</returns>
        public String BindCart(DataSet ds)
        {
            StringBuilder Table = new StringBuilder();
            Decimal subotatal = 0;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Table.Append(" <table border='0' cellpadding='2' class='content-table border-td' cellspacing='1' class='table' width='700px'> ");
                Table.Append("<tbody><tr style='height:20px;color: #FFFFFF;'>");
                Table.Append("<th align='left' valign='middle' style='width:40%' >Product</th>");
                Table.Append("<th align='left' valign='middle' style='width:15%' >SKU</th>");
                Table.Append("<th align='center' valign='middle' style='width:15%' >Tracking #</th>");
                Table.Append("<th align='center' valign='middle' style='width:10%'>Quantity</th>");
                Table.Append("<th style='text-align: center;width:10%;'>Status</th>");
                Table.Append("<th style='text-align: center;width:15%;'>Request No.</th>");
                Table.Append("</tr>");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Decimal NetPrice = Decimal.Zero;
                    Decimal Price = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["Price"].ToString()), 2);
                    Int32 Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString());
                    NetPrice = Math.Round((Price * Quantity), 2);
                    Table.Append("<tr class='list_table_cell_link' style='background:none;'>");
                    string ProductName = "";
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ProductOption"].ToString()))
                        ProductName = Convert.ToString(ds.Tables[0].Rows[i]["Name"].ToString()) + "<br/> " + ds.Tables[0].Rows[i]["ProductOption"].ToString().Replace("\r\n", "<br/>");
                    else ProductName = ds.Tables[0].Rows[i]["Name"].ToString();

                    Table.Append("<td align='left' valign='top'>" + ProductName + "");
                    Table.Append("</td>");
                    Table.Append("<td align='left' valign='top'>" + ds.Tables[0].Rows[i]["Style"].ToString() + "</td>");

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TrackingNumber"].ToString()) && ds.Tables[0].Rows[i]["TrackingNumber"].ToString() != "0")
                        Table.Append("<td align='left' valign='top'>" + ds.Tables[0].Rows[i]["TrackingNumber"].ToString() + "</td>");
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

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["QuoteReqDetailId"].ToString()) && ds.Tables[0].Rows[i]["QuoteReqDetailId"].ToString() != "0")
                    {
                        Table.Append("<td  valign='top' align='center'>" + ds.Tables[0].Rows[i]["QuoteReqDetailId"].ToString() + "</td>");
                    }
                    else Table.Append("<td  valign='top' align='center'>---</td>");

                    Table.Append(" </tr></tr>");

                    subotatal += (Convert.ToInt32(ds.Tables[0].Rows[i]["Quantity"].ToString()) * Price);
                }
                ltSubtotal.Text = ": &nbsp;$" + subotatal.ToString("f2");
                Table.Append("</tbody></table>");
            }
            return Table.ToString();
        }

        /// <summary>
        /// Fills the Order Notes Log
        /// </summary>
        private void FillOrderNotesLog()
        {
            DataSet dsProducts = new DataSet();
            dsProducts = CommonComponent.GetCommonDataSet("SELECT dbo.tb_Timestamplog.Description, dbo.tb_Timestamplog.CreatedOn, dbo.tb_Timestamplog.type, dbo.tb_Timestamplog.refnumber, " +
                                    "dbo.tb_Timestamplog.orderNumber, dbo.tb_Admin.FirstName+' '+dbo.tb_Admin.LastName as name" +
                                    " FROM         dbo.tb_Admin INNER JOIN  " +
                                    "dbo.tb_Timestamplog ON dbo.tb_Admin.AdminID = dbo.tb_Timestamplog.Createdby WHERE   " +
                                    " isnull(dbo.tb_Timestamplog.AppType,0)=0 AND dbo.tb_Timestamplog.type in (24) AND dbo.tb_Timestamplog.refnumber=" + Request.QueryString["PONo"] + " Order BY dbo.tb_Timestamplog.CreatedOn");

            string strTable = "<div style='padding-bottom: 5px;'><strong>&nbsp;Additional Notes Log :</strong></div>";
            strTable += " <table cellpadding=\"0\" cellspacing=\"0\" width='100%' class='content-table border-td'>";
            strTable += "<tr style='height:20px;color: #FFFFFF;'><th style='padding-left:2px;border-right:1px solid #eeeeee;border-left:1px solid #eeeeee;height:25px'>PO #</th><th style='padding-left:2px;border-right:1px solid #eeeeee;'>Created by</th><th style='padding-left:2px;border-right:1px solid #eeeeee;'>Created on</th><th style='padding-left:2px;'>Description</th></tr>";
            if (dsProducts.Tables[0].Rows.Count > 0)
            {
                Ordernotesid.Visible = true;
                for (int i = 0; i < dsProducts.Tables[0].Rows.Count; i++)
                {
                    strTable += "<tr style='height:25px;'>";
                    strTable += "<td style='border-right:1px solid #eeeeee'>&nbsp;" + dsProducts.Tables[0].Rows[i]["refnumber"].ToString().Replace(" ", "&nbsp;") + "</td>";
                    strTable += "<td style='border-right:1px solid #eeeeee'>&nbsp;" + dsProducts.Tables[0].Rows[i]["Name"].ToString().Replace(" ", "&nbsp;") + "</td>";
                    strTable += "<td style='border-right:1px solid #eeeeee'>&nbsp;" + dsProducts.Tables[0].Rows[i]["CreatedOn"].ToString().Replace(" ", "&nbsp;") + "</td>";
                    strTable += "<td style='border-right:1px solid #eeeeee' >&nbsp;" + dsProducts.Tables[0].Rows[i]["Description"].ToString() + "</td>";
                    strTable += "</tr>";
                }
                strTable += "</table>";
                Ordernotesid.Text = strTable;
            }
            else
            {
                Ordernotesid.Visible = false;
            }
        }

        /// <summary>
        /// Gets the Old Purchase Order Items for Warehouse
        /// </summary>
        /// <param name="ONO">int ONO</param>
        /// <param name="PONO">int PONO</param>
        /// <returns>Returns the Dataset of Purchase order list</returns>
        public DataSet GetOldPurchaseOrderItemsWarehouse(Int32 ONO, Int32 PONO)
        {
            return CommonComponent.GetCommonDataSet(@" SELECT DISTINCT ISNULL(por.ProductOption,'') as ProductOption,IsNULL(por.VendorQuoteReqDetailsID,0) as QuoteReqDetailId,
                      (CASE WHEN (isnull(por.isshipped, 0) = 0) THEN 'Pending' ELSE 'Shipped' END) AS Shipped, ISNULL(po.Adjustments, 0) AS Adjustments,  ISNULL(por.ispaid, 0) AS ispaid,
                      ISNULL(po.Tax, 0) AS tax, ISNULL(po.Shipping, 0) AS shipping, po.PONumber, p.Name, p.SKU AS Style, por.Quantity, por.ProductID, po.OrderNumber, 
                      po.Notes, ISNULL(po.AdditionalCost, 0) AS AdditionalCost, ISNULL(po.PoAmount, 0) AS poamount, po.PODate, ISNULL(por.Price, 0) AS Price,por.TrackingNumber
                      FROM dbo.tb_PurchaseOrderItems AS por INNER JOIN
                      dbo.tb_PurchaseOrder AS po ON por.PONumber = po.PONumber INNER JOIN
                      dbo.tb_Product AS p ON por.ProductID = p.ProductID
                      WHERE (po.OrderNumber = " + ONO + ") AND (por.PONumber = " + PONO + ")");
        }

        /// <summary>
        /// Gets the Name of the Vendors
        /// </summary>
        /// <param name="ONO">int ONO</param>
        /// <param name="PONO">int PONO</param>
        /// <returns>Returns Name of Vendor as a String</returns>
        public String GetVendorName(Int32 ONO, Int32 PONO)
        {
            return Convert.ToString(CommonComponent.GetScalarCommonData("select Name as VendorName from dbo.tb_Vendor v  " +
                            " inner join dbo.tb_PurchaseOrder po on po.vendorid=v.vendorid  " +
                            "   where po.ponumber=" + PONO + " and po.ordernumber=" + ONO + " and v.Active=1 and v.deleted=0"));
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                CommonComponent.ExecuteCommonData("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber,Description,CreatedOn) VALUES (" + Session["AdminID"].ToString() + ",24," + Convert.ToInt32(Request.QueryString["PONo"].ToString()) + "," + Convert.ToInt32(0) + ",'" + txtNotesMain.Text.ToString().Replace("'", "''") + "',getdate())");
                OrderComponent objAddOrder = new OrderComponent();
                objAddOrder.InsertOrderlog(24, Convert.ToInt32(0), "", Convert.ToInt32(Session["AdminID"].ToString()));

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('Note added successfully..');", true);
                txtNotesMain.Text = "";
                FillOrderNotesLog();
            }
            catch { }
        }
    }
}