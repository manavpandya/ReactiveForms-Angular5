using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Web.UI.HtmlControls;
using System.Text;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class POOrder : BasePage
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
                if (Request.QueryString["ONo"] != null)
                {
                    btnGeneratePO.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/generate-purchase-order.png";
                    btnRefreshPO.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/refresh-purchase-order.png";
                    string OrderNumber = Server.UrlDecode(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()));
                    if (Request.QueryString["ONo"] != null && Request.QueryString["ONo"].ToString().Length > 0)
                    {
                        AddCartItem(OrderNumber.ToString());
                        bindOldPo();
                    }
                    FillPoOrder();
                    FillLog();
                }
            }
        }


        /// <summary>
        /// Gets the Quantity discount.
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Qty">int Qty</param>
        /// <returns>Returns the discount value</returns>
        public decimal GetQtyDiscount(int ProductID, int Qty)
        {
            string Query = " SELECT tb_QuantityDiscountTable.DiscountPercent FROM   tb_Product Inner Join " +
                            " tb_QuantityDiscountTable ON tb_Product.QuantityDiscountID = tb_QuantityDiscountTable.QuantityDiscountID  " +
                            " WHERE (tb_QuantityDiscountTable.LowQuantity <=" + Qty + ") AND (tb_QuantityDiscountTable.HighQuantity >=" + Qty + ") AND  tb_Product.ProductID =" + ProductID + "";
            DataSet Ds = CommonComponent.GetCommonDataSet(Query);
            if (Ds.Tables[0].Rows.Count > 0)
                return Convert.ToDecimal(Ds.Tables[0].Rows[0]["DiscountPercent"].ToString());
            else
                return 0;
        }

        /// <summary>
        /// Adds the cart item.
        /// </summary>
        /// <param name="ONo">string ONo</param>
        public void AddCartItem(string ONo)
        {
            decimal QtyDiscount = 0;
            StringBuilder Table = new StringBuilder();
            DataSet DsCItems = new DataSet();
            VendorComponent objvendor = new VendorComponent();
            DsCItems = objvendor.GetPurchaseOrder(Convert.ToInt32(ONo), string.Empty, string.Empty);
            if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
            {
                Table.Append(" <table border='0' cellpadding='0' cellspacing='0' class='table-noneforOrder' width='100%'> ");
                Table.Append("<tbody><tr style='BACKGROUND-COLOR: rgb(242,242,242); ' >");
                Table.Append("<th align='left' valign='middle'>Select</th>");
                Table.Append("<th align='left' valign='middle' style='width:50%' >Product</th>");
                Table.Append("<th align='left' valign='middle' style='width:10%' > SKU</th>");
                Table.Append("<th align='center'>Available <br/> Inventory</th>");
                Table.Append("<th align='center' valign='middle' >Ordered Quantity</th>");
                //Table.Append("<th align='center'>Shipped Quantity</th>");
                Table.Append("<th align='center'>PO Quantity</th>");
                Table.Append("<th align='center'>Generate Purchase Order</th>");
                Table.Append("</tr>");
                decimal TPrice = 0;
                decimal QtyDiscountPercent = 0;
                bool flag = false;

                for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                {
                    decimal Price1 = 0;
                    decimal PTemp = 0;
                    if (DsCItems.Tables[0].Rows[i]["SalePrice"].ToString() != null && DsCItems.Tables[0].Rows[i]["SalePrice"].ToString() != "")
                        PTemp = Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["SalePrice"].ToString());
                    else
                        PTemp = 0;
                    PTemp = Math.Round(PTemp, 2);
                    Price1 = PTemp * Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["Quantity"].ToString());
                    QtyDiscountPercent = GetQtyDiscount(Convert.ToInt32(DsCItems.Tables[0].Rows[i]["ProductID"].ToString()), Convert.ToInt32(DsCItems.Tables[0].Rows[i]["Quantity"].ToString()));
                    QtyDiscount += (Price1 * QtyDiscountPercent) / 100;
                    QtyDiscount = Math.Round(QtyDiscount, 2);
                    TPrice += Price1;
                    Int32 PurchaseOrderQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(SUM(dbo.tb_PurchaseOrderItems.Quantity),0) FROM dbo.tb_PurchaseOrder INNER JOIN dbo.tb_Order ON dbo.tb_PurchaseOrder.OrderNumber = dbo.tb_Order.OrderNumber INNER JOIN dbo.tb_PurchaseOrderItems ON dbo.tb_PurchaseOrder.PONumber = dbo.tb_PurchaseOrderItems.PONumber INNER JOIN dbo.tb_OrderedShoppingCartItems ON dbo.tb_Order.ShoppingCardID = dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID AND dbo.tb_PurchaseOrderItems.ProductID = dbo.tb_OrderedShoppingCartItems.RefProductID WHERE dbo.tb_PurchaseOrderItems.ProductID=" + DsCItems.Tables[0].Rows[i]["ProductID"].ToString() + " AND dbo.tb_PurchaseOrder.OrderNumber=" + ONo + ""));
                    //Int32 pQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(sum(isnull(markquantity,0)+isnull(Markupgradequantity,0)),0) from tb_product p left outer join tb_lockproducts lp on lp.pRoductid=p.productid where lp.productid =" + DsCItems.Tables[0].Rows[i]["ProductID"].ToString() + "  and lp.ordercustomcartId =" + DsCItems.Tables[0].Rows[i]["OrderedCustomCartID"].ToString() + " and iscompleted=1 and ordernumber=" + ONo.ToString() + ""));
                    //if (Convert.ToUInt32(pQty) < Convert.ToUInt32(DsCItems.Tables[0].Rows[i]["Quantity"].ToString()))
                    //{
                    //DsCItems.Tables[0].Rows[i]["PurchaseOrderQty"] = pQty;
                    //DsCItems.Tables[0].Rows[i]["PurchaseOrderQty"] = PurchaseOrderQty;
                    //DsCItems.Tables[0].AcceptChanges();
                    if (Convert.ToUInt32(PurchaseOrderQty) < Convert.ToUInt32(DsCItems.Tables[0].Rows[i]["Quantity"].ToString()))
                    {
                        flag = true;
                        Table.Append("<tr align='center'  valign='middle'>");
                        Table.Append("<tr >");
                        Table.Append("<td align='center' valign='middle'><input type=\"checkbox\" name=\"chk:" + DsCItems.Tables[0].Rows[i]["ProductID"].ToString() + ":" + DsCItems.Tables[0].Rows[i]["OrderedCustomCartID"].ToString() + "\" id=\"chk:" + DsCItems.Tables[0].Rows[i]["ProductID"].ToString() + "\"  /></td>");
                        Table.Append("<td align='left' valign='top'>" + DsCItems.Tables[0].Rows[i]["Name"].ToString());

                        string[] Names = DsCItems.Tables[0].Rows[i]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] Values = DsCItems.Tables[0].Rows[i]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        int iLoopValues = 0;
                        for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                        {
                            Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
                        }
                        Table.Append("</td>");
                        Table.Append("<td  align='left' >" + DsCItems.Tables[0].Rows[i]["SKU"].ToString() + "</td>");
                        if (Convert.ToInt32(DsCItems.Tables[0].Rows[i]["Storeid"].ToString()) != 1)
                        {
                            DataSet dsUPC = new DataSet();
                            dsUPC = CommonComponent.GetCommonDataSet("SELECT Inventory,productId FROM tb_product WHERE UPC='" + DsCItems.Tables[0].Rows[i]["ProductID"].ToString() + "'");
                            if (dsUPC != null && dsUPC.Tables[0].Rows.Count > 0)
                            {
                                Table.Append("<td align='center'><a onclick=\"javascript:window.open('ProductSearch.aspx?pid=" + DsCItems.Tables[0].Rows[i]["ProductID"].ToString() + "','','width=700,height=700,scrollbars=1');\" href='javascript:void(0);' >[Not Found]</a></td>");
                            }
                            else
                            {
                                Table.Append("<td align='center'>" + DsCItems.Tables[0].Rows[i]["Inventory"].ToString() + "</td>");
                            }
                        }
                        else
                        {
                            Table.Append("<td align='center'>" + DsCItems.Tables[0].Rows[i]["Inventory"].ToString() + "</td>");
                        }
                        Table.Append("<td align='center'>" + DsCItems.Tables[0].Rows[i]["Quantity"].ToString() + "</td>");
                        //if (showUP)
                        //{
                        //    // Table.Append("<td  align='left' >" + DsCItems.Tables[0].Rows[i]["UpgradeProducts"].ToString() + "</td>");
                        //    Table.Append("<td  align='left' >" + DsCItems.Tables[0].Rows[i]["UpgradeInventory"].ToString() + "</td>");
                        //}
                        Table.Append("<td align='center'>" + Convert.ToUInt32(PurchaseOrderQty) + "</td>");

                        string ClickHere = "";
                        if (Convert.ToUInt32(PurchaseOrderQty) < Convert.ToUInt32(DsCItems.Tables[0].Rows[i]["Quantity"].ToString()))
                        {
                            ClickHere = "<a href=\"PurchaseOrder.aspx?ONo=" + SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()) + "&customid=" + DsCItems.Tables[0].Rows[i]["OrderedCustomCartID"].ToString() + "&PID=" + DsCItems.Tables[0].Rows[i]["ProductID"].ToString() + "\" >Click Here</a>";
                        }
                        Table.Append("<td  STYLE='text-align : center;  '> " + ClickHere + "</td>");
                        Table.Append(" </tr>");
                        //}
                    }
                }
                Table.Append("</tbody></table>");
                TPrice = (TPrice - QtyDiscount);
                TPrice = Math.Round(TPrice, 2);
                if (!flag)
                {
                    Table = new StringBuilder();
                    //Table.AppendLine("<table><tr><td><font color='red' CLASS='font-red;' style='font-size:12px;'>Purchase Order Not Required.</font></td></tr></table>");
                    Table.AppendLine("<table><tr><td><font color='red' CLASS='font-red;' style='font-size:12px;'>&nbsp;</font></td></tr></table>");
                    btnGeneratePO.Visible = false;
                }
            }
            else
            {
                // Table.AppendLine("<table><tr><td><font color='red' CLASS='font-red' style='font-size:12px;'>Purchase Order Not Required.</font></td></tr></table>");
                Table.AppendLine("<table><tr><td><font color='red' CLASS='font-red;' style='font-size:12px;'>&nbsp;</font></td></tr></table>");
                btnGeneratePO.Visible = false;
            }
            ltCart.Text = Table.ToString();
        }

        /// <summary>
        /// Checks whether PO is Pending or Not
        /// </summary>
        /// <param name="pono">string pono</param>
        /// <returns>Returns Link if exists else blank String.</returns>
        public String CheckPending(String pono)
        {
            String Result = "";
            Int32 ONo = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"]));
            Int32 StoreID = 0;
            Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + ONo)), out StoreID);
            if (Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(1,0) from  tb_PurchaseOrderItems where ponumber=" + pono.Trim() + " and isnull(IsShipped,0)=0")) > 0)
                Result = "<a  href=\"/VendorProducts.aspx?pono=" + pono.Trim() + "&StoreId=" + StoreID.ToString() + "\" target=\"_blank\">Click Here</a>";
            else
                Result = "-";

            return Result;

        }

        /// <summary>
        /// Checks the Resend
        /// </summary>
        /// <param name="pono">string pono</param>
        /// <returns>Returns  Link as a string</returns>
        public String CheckResend(String pono)
        {
            String Result = "";
            if (Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(1,0) from  tb_PurchaseOrderItems where ponumber=" + pono.Trim() + "")) > 0)
                Result = "Resend";
            else
                Result = "";

            return Result;
        }

        /// <summary>
        /// Makes the positive Adjustment Amount
        /// </summary>
        /// <param name="Adjustments">string Adjustments</param>
        /// <returns>Returns Positive Adjustments Amount</returns>
        public String MakePositive(string Adjustments)
        {
            try
            {
                Decimal Adjust = Convert.ToDecimal(Adjustments);
                if (Adjust < 0)
                    return "$" + (-Adjust) + "(-)";
                else if (Adjust == 0)
                    return "$0.00";
                else
                    return "$" + (Adjust) + "(+)";
            }
            catch { return "$0.00"; }
        }

        /// <summary>
        /// Fills the PO Order from Database By Order Number
        /// </summary>
        private void FillPoOrder()
        {
            DataSet dsProducts = new DataSet();
            dsProducts = GetVendorProducts(Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"])));
            if (dsProducts.Tables[0].Rows.Count > 0)
            {
                printPOid.Visible = true;
                gvVendor.DataSource = dsProducts;
                gvVendor.DataBind();
            }
            else
            {
                printPOid.Visible = false;
            }
        }

        /// <summary>
        /// Gets the vendor products.
        /// </summary>
        /// <param name="ono">in ono</param>
        /// <returns>Returns the List of Vendor Products</returns>
        public DataSet GetVendorProducts(int ono)
        {
            return CommonComponent.GetCommonDataSet("SELECT tb_Product.Name AS productName, " +
                                "dbo.tb_PurchaseOrderItems.Quantity, dbo.tb_PurchaseOrderItems.PONumber, dbo.tb_PurchaseOrderItems.ProductID, dbo.tb_PurchaseOrderItems.Price, Case when cast(isnull(dbo.tb_PurchaseOrderItems.isshipped,0) as varchar(20)) ='0' then 'Pending' else 'Shipped' end as isshipped ,isnull(dbo.tb_PurchaseOrderItems.isPaid,0) as isPaid, " +
                                "dbo.tb_Product.SKU, dbo.tb_Vendor.Name AS Vname, dbo.tb_Vendor.Email,  " +
                                "dbo.tb_PurchaseOrder.OrderNumber, dbo.tb_PurchaseOrder.PODate,dbo.tb_Vendor.Phone,dbo.tb_PurchaseOrder.Notes FROM  " +
                                "dbo.tb_Product INNER JOIN dbo.tb_PurchaseOrderItems ON dbo.tb_Product.ProductID = dbo.tb_PurchaseOrderItems.ProductID INNER JOIN dbo.tb_PurchaseOrder ON dbo.tb_PurchaseOrderItems.PONumber = dbo.tb_PurchaseOrder.PONumber INNER JOIN dbo.tb_Vendor ON dbo.tb_PurchaseOrder.VendorID = dbo.tb_Vendor.VendorId " +
                                " WHERE   dbo.tb_PurchaseOrder.OrderNumber=" + ono + "");
        }

        /// <summary>
        /// Gets the log of PO Transaction
        /// </summary>
        /// <param name="ono">int ono</param>
        /// <returns>Returns the List of PO Order Log</returns>
        public DataSet GetLog(int ono)
        {
            return CommonComponent.GetCommonDataSet("SELECT dbo.tb_Timestamplog.Createdon, dbo.tb_Timestamplog.type, dbo.tb_Timestamplog.refnumber, " +
                                "dbo.tb_Timestamplog.orderNumber, dbo.tb_Admin.FirstName+' '+dbo.tb_Admin.LastName as name" +
                                " FROM dbo.tb_Admin INNER JOIN  " +
                                "dbo.tb_Timestamplog ON dbo.tb_Admin.AdminID = dbo.tb_Timestamplog.Createdby WHERE   " +
                                " isnull(dbo.tb_Timestamplog.AppType,0)=0 AND dbo.tb_Timestamplog.type in (10,11) AND dbo.tb_Timestamplog.orderNumber=" + ono + "");

        }

        /// <summary>
        /// Gets the vendor PO log
        /// </summary>
        /// <param name="ono">int ono</param>
        /// <returns>Returns the List of Vendor PO Order Log</returns>
        public DataSet GetVendorPOLog(int ono)
        {
            return CommonComponent.GetCommonDataSet("SELECT dbo.tb_Timestamplog.Createdon, dbo.tb_Timestamplog.type, dbo.tb_Timestamplog.refnumber, " +
                                "dbo.tb_Timestamplog.orderNumber, dbo.tb_Timestamplog.VendorName as name,dbo.tb_Timestamplog.Description " +
                                " FROM   dbo.tb_Timestamplog WHERE isnull(dbo.tb_Timestamplog.AppType,0)=0 AND dbo.tb_Timestamplog.type in (26) AND dbo.tb_Timestamplog.orderNumber=" + ono + "");
        }

        /// <summary>
        /// Fills the PO Order log
        /// </summary>
        private void FillLog()
        {
            DataSet dsProducts = new DataSet();
            dsProducts = GetLog(Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"])));
            string strTable = " <table cellpadding=\"0\" cellspacing=\"0\" width='700px'>";

            if (dsProducts.Tables[0].Rows.Count > 0)
            {
                postatus.Visible = true;
                strTable += "<tr><td style='border-bottom:1px solid #eeeeee;height:30px;background-color:#dde0e5;padding-left:5px;font-weight:bold;' colspan='3'>P.O. Log<td>";
                for (int i = 0; i < dsProducts.Tables[0].Rows.Count; i++)
                {
                    strTable += "<tr>";
                    strTable += "<td style='border-bottom:1px solid #eeeeee;border-left:1px solid #eeeeee;padding-left: 5px;'><b>Po Number</b> : " + dsProducts.Tables[0].Rows[i]["refnumber"].ToString() + "</td>";
                    if (dsProducts.Tables[0].Rows[i]["type"].ToString() == "11")
                    {
                        strTable += "<td style='border-bottom:1px solid #eeeeee'><b>Deleted by</b>: " + dsProducts.Tables[0].Rows[i]["Name"].ToString() + "</td>";
                        strTable += "<td style='border-bottom:1px solid #eeeeee;border-right:1px solid #eeeeee'><b>Deleted on</b>: " + dsProducts.Tables[0].Rows[i]["Createdon"].ToString() + "</td>";
                    }
                    else
                    {
                        strTable += "<td style='border-bottom:1px solid #eeeeee'><b>Created by</b>: " + dsProducts.Tables[0].Rows[i]["Name"].ToString() + "</td>";
                        strTable += "<td style='border-bottom:1px solid #eeeeee;border-right:1px solid #eeeeee'><b>Created on</b>: " + dsProducts.Tables[0].Rows[i]["Createdon"].ToString() + "</td>";
                    }
                    strTable += "</tr>";
                }
                dsProducts = new DataSet();
                dsProducts = GetVendorPOLog(Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"])));
                for (int i = 0; i < dsProducts.Tables[0].Rows.Count; i++)
                {
                    strTable += "<tr>";
                    strTable += "<td style='border-bottom:1px solid #eeeeee;border-left:1px solid #eeeeee'><b>Po Number</b> : " + dsProducts.Tables[0].Rows[i]["refnumber"].ToString() + "</td>";

                    strTable += "<td style='border-bottom:1px solid #eeeeee'><b>Updated by</b>: " + dsProducts.Tables[0].Rows[i]["Name"].ToString() + "</td>";
                    if (!String.IsNullOrEmpty(dsProducts.Tables[0].Rows[i]["Description"].ToString()))
                    {
                        strTable += "<td style='border-bottom:1px solid #eeeeee;border-right:1px solid #eeeeee'><b>Updated on</b>: " + dsProducts.Tables[0].Rows[i]["Createdon"].ToString() + " <b>Description :</b>" + dsProducts.Tables[0].Rows[i]["Description"].ToString() + "</td>";
                    }
                    else
                    {
                        strTable += "<td style='border-bottom:1px solid #eeeeee;border-right:1px solid #eeeeee'><b>Updated on</b>: " + dsProducts.Tables[0].Rows[i]["Createdon"].ToString() + "</td>";
                    }
                    strTable += "</tr>";
                }
            }
            else
            {
                postatus.Visible = false;
            }
            strTable += "</table>";
            postatus.Text = strTable;
        }

        /// <summary>
        /// Binds the old PO Order
        /// </summary>
        private void bindOldPo()
        {
            DataSet DsOldPOrder = new DataSet();
            Int32 ONo = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"]));
            DsOldPOrder = CommonComponent.GetCommonDataSet("select convert(varchar(max),round(isnull(po.Adjustments,0),2)) as Adjustments,convert(varchar(max),round(isnull(po.Tax,0),2)) as Tax,convert(varchar(max),round(isnull(po.Shipping,0),2)) as Shipping,convert(varchar(max),round(isnull(po.additionalcost,0),2)) as additionalcost, convert(varchar(max),round(isnull(po.poamount,0),2)) as poamount,po.PONumber,po.VendorID,po.PODate,po.OrderNumber,v.Name  from tb_PurchaseOrder po  INNER JOIN tb_Vendor v ON po.VendorID = v.VendorID  where po.OrderNumber = " + ONo + " order by po.ponumber desc");
            if (DsOldPOrder != null && DsOldPOrder.Tables[0].Rows.Count > 0)
            {
                gvOldPOrder.DataSource = DsOldPOrder;
                gvOldPOrder.DataBind();
                gvOldPOrder.Visible = true;
                lblPoOrdrs.Visible = true;

                Decimal TotalPOAmount = 0;
                Decimal TotalClearPOAmount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select isnull(sum(isnull(paidamount,0)),0) from tb_vendorpayment  " +
                                                                    " where VendorPaymentID in( " +
                                                                    " select VendorPaymentID from tb_vendorpaymentPurchaseOrder " +
                                                                    " where ponumber in (select ponumber from tb_purchaseorder " +
                                                                    " where ordernumber=" + ONo + "))"));
                Decimal TotalUnClearPOAmount = 0;
                foreach (DataRow dr in DsOldPOrder.Tables[0].Rows)
                {
                    TotalPOAmount += Convert.ToDecimal(dr["POAmount"].ToString());
                }
                TotalUnClearPOAmount = TotalPOAmount - TotalClearPOAmount;
                ltClearPOAmt.Text = "<b>Clear Amount </b>:$" + TotalClearPOAmount.ToString("f2") + "<br />";
                ltUnClearPOAmt.Text = "<b>UnClear Amount </b>:$" + TotalUnClearPOAmount.ToString("f2") + "<br />";
                ltTotalPOAmt.Text = "<b>Total PO Amount </b>:$" + TotalPOAmount.ToString("f2") + "<br />";
                trOldPO.Visible = true;
            }
            else
            {
                gvOldPOrder.Visible = false;
                lblPoOrdrs.Visible = false;
                ltClearPOAmt.Text = "";
                ltUnClearPOAmt.Text = "";
                ltTotalPOAmt.Text = "";
                trOldPO.Visible = false;
            }
        }

        /// <summary>
        /// Vendor Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvVendor_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                try
                {
                    System.Web.UI.WebControls.Label lblstatus = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblStatus");
                    System.Web.UI.WebControls.LinkButton lbldelete = (System.Web.UI.WebControls.LinkButton)e.Row.FindControl("lbldelete");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnpaid = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnpaid");

                    lbldelete.Attributes.Add("onclick", "return ConfirmDelete();");
                    if (lblstatus.Text.ToString().ToLower() == "shipped")
                    {
                        if (hdnpaid.Value.ToString() == "1")
                        {
                            lblstatus.Text = "Paid";
                        }
                        e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                        e.Row.Cells[8].Text = "";

                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Un-mark products shipped for vendor PO
        /// </summary>
        /// <param name="OrderNumber">The order number</param>
        /// <param name="ProductIds">The product ids</param>
        /// <returns><c>true</c> if Unmark product Shipped, <c>false</c> otherwise</returns>
        public bool UnMarkProductsShippedforVendorPO(int OrderNumber, int ProductIds)
        {
            string Query = string.Empty;
            Query += " if exists (select 1 from tb_LockProducts where OrderNumber=" + OrderNumber + " and ProductID=" + ProductIds + " and IsCompleted=1 AND ispo=1";
            Query += ") begin DELETE FROM tb_LockProducts ";
            Query += " where OrderNumber=" + OrderNumber + " and ProductID=" + ProductIds + " and IsCompleted=1 AND ispo=1 end";
            if (!string.IsNullOrEmpty(Query))
            {
                return Convert.ToBoolean(CommonComponent.ExecuteCommonData(Query));
            }
            return false;
        }

        /// <summary>
        /// Vendor Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvVendor_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            System.Web.UI.WebControls.GridViewRow row = gvVendor.Rows[e.RowIndex];
            System.Web.UI.WebControls.Label lblPonumber = (System.Web.UI.WebControls.Label)row.FindControl("lblPonumber");
            System.Web.UI.HtmlControls.HtmlInputHidden hdnNotes = (System.Web.UI.HtmlControls.HtmlInputHidden)row.FindControl("hdnNotes");
            System.Web.UI.WebControls.Label lblProductID = (System.Web.UI.WebControls.Label)row.FindControl("lblProductID");
            CommonComponent.ExecuteCommonData("DELETE FROM tb_PurchaseOrderItems WHERE PONumber=" + lblPonumber.Text.ToString() + "");
            CommonComponent.ExecuteCommonData("DELETE FROM tb_PurchaseOrder WHERE PONumber=" + lblPonumber.Text.ToString() + "");
            try
            {
                CommonComponent.ExecuteCommonData("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber,CreatedOn) VALUES (" + Session["AdminID"].ToString() + ",11," + Convert.ToInt32(lblPonumber.Text) + "," + Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"])) + ",getdate())");
                OrderComponent objAddOrder = new OrderComponent();
                objAddOrder.InsertOrderlog(11, Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"])), "", Convert.ToInt32(Session["AdminID"].ToString()));
            }
            catch { }

            string OrdNum = SecurityComponent.Decrypt(Request.QueryString["ONo"]);
            Response.Redirect("POOrder.aspx?ono=" + Server.UrlEncode(SecurityComponent.Encrypt(OrdNum.ToString())) + "");
        }

        /// <summary>
        /// Old PO Order Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvOldPOrder_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            System.Web.UI.WebControls.Label lblPOName = (System.Web.UI.WebControls.Label)gvOldPOrder.Rows[e.NewEditIndex].FindControl("lblPOName");
            DataSet dsMail = new DataSet();
            dsMail = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Warehousemail WHERE Ponumber=" + lblPOName.Text.ToString().Trim() + "");
            if (dsMail.Tables[0].Rows.Count > 0)
            {
                System.Net.Mail.AlternateView av = System.Net.Mail.AlternateView.CreateAlternateViewFromString(dsMail.Tables[0].Rows[0]["Body"].ToString(), null, "text/html");
                if (!String.IsNullOrEmpty(dsMail.Tables[0].Rows[0]["Filepath"].ToString()))
                {
                    try
                    {
                        if (System.IO.File.Exists(Server.MapPath(dsMail.Tables[0].Rows[0]["Filepath"].ToString())))
                        {
                            CommonOperations.SendMailAttachment(dsMail.Tables[0].Rows[0]["EmailFrom"].ToString(), dsMail.Tables[0].Rows[0]["EmailTo"].ToString().Replace(",", ";"), dsMail.Tables[0].Rows[0]["EmailCC"].ToString(), dsMail.Tables[0].Rows[0]["EMailBCC"].ToString(), dsMail.Tables[0].Rows[0]["Subject"].ToString(), dsMail.Tables[0].Rows[0]["Body"].ToString(), Request.UserHostAddress.ToString(), true, av, Server.MapPath(dsMail.Tables[0].Rows[0]["Filepath"].ToString()).ToString());
                            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Mail has been sent successfully.');", true);
                        }
                        else
                        {
                            CommonOperations.SendMail(dsMail.Tables[0].Rows[0]["EmailFrom"].ToString(), dsMail.Tables[0].Rows[0]["EmailTo"].ToString().Replace(",", ";"), dsMail.Tables[0].Rows[0]["EmailCC"].ToString(), dsMail.Tables[0].Rows[0]["EMailBCC"].ToString(), dsMail.Tables[0].Rows[0]["Subject"].ToString(), dsMail.Tables[0].Rows[0]["Body"].ToString(), Request.UserHostAddress.ToString(), true, av);
                            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Mail has been sent successfully.');", true);
                        }
                    }
                    catch
                    {
                        CommonOperations.SendMail(dsMail.Tables[0].Rows[0]["EmailFrom"].ToString(), dsMail.Tables[0].Rows[0]["EmailTo"].ToString().Replace(",", ";"), dsMail.Tables[0].Rows[0]["EmailCC"].ToString(), dsMail.Tables[0].Rows[0]["EMailBCC"].ToString(), dsMail.Tables[0].Rows[0]["Subject"].ToString(), dsMail.Tables[0].Rows[0]["Body"].ToString(), Request.UserHostAddress.ToString(), true, av);
                        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Mail has been sent successfully.');", true);
                    }
                }
                else
                {
                    CommonOperations.SendMail(dsMail.Tables[0].Rows[0]["EmailFrom"].ToString(), dsMail.Tables[0].Rows[0]["EmailTo"].ToString().Replace(",", ";"), dsMail.Tables[0].Rows[0]["EmailCC"].ToString(), dsMail.Tables[0].Rows[0]["EMailBCC"].ToString(), dsMail.Tables[0].Rows[0]["Subject"].ToString(), dsMail.Tables[0].Rows[0]["Body"].ToString(), Request.UserHostAddress.ToString(), true, av);
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Mail has been sent successfully.');", true);
                }
            }
        }

        /// <summary>
        ///  Generate PO Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGeneratePO_Click(object sender, ImageClickEventArgs e)
        {
            string Pids = "";
            string Customids = "";
            String[] formkeys = Request.Form.AllKeys;
            foreach (String s in formkeys)
            {
                //For Delete Product from Cart and Bind Cart again
                if (s.Contains("chk:"))
                {
                    String[] p = s.Split(':');
                    String str = "";
                    str = Request.Form[s].ToString();
                    if (str == "on")
                    {
                        Pids += p[1] + ",";
                        Customids += p[2] + ",";
                    }
                }
            }
            if (string.IsNullOrEmpty(Pids.Trim()))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgQty", "alert('Select Product to Generate PO.!');", true);
                return;
            }
            // Take Order number after its code
            string OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
            Response.Redirect("PurchaseOrder.aspx?ONo=" + OrderNumber + "&customid=" + Customids + "&PId=" + Pids);
        }


        /// <summary>
        ///  Refresh PO Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnRefreshPO_Click(object sender, ImageClickEventArgs e)
        {
            string OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
            if (Request.QueryString["ONo"] != null && Request.QueryString["ONo"].ToString().Length > 0)
            {
                AddCartItem(OrderNumber.ToString());
                bindOldPo();
            }
            FillPoOrder();
            FillLog();
        }

        /// <summary>
        /// Old Order Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvOldPOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                try
                {
                    System.Web.UI.WebControls.Label lblPoNum = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblPoNum");
                    System.Web.UI.HtmlControls.HtmlAnchor atagOrderNumber = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("atagOrderNumber");
                    atagOrderNumber.HRef = "OldPurchaseOrderCart.aspx?back=1&Ono=" + SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()) + "&PONo=" + lblPoNum.Text.ToString() + "";
                }
                catch { }
            }
        }
    }
}