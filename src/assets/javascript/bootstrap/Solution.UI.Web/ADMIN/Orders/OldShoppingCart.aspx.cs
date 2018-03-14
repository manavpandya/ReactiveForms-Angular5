using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using System.Text;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class OldShoppingCart : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ONo"] != null)
            {
                divOrderList.Visible = true;
                divOrder.Visible = false;
                BindOrderGrid(Convert.ToInt32(Request.QueryString["ONo"]));
            }
            else if (Request.QueryString["Backup_Id"] != null)
            {
                divOrderList.Visible = false;
                divOrder.Visible = true;
                Int32 Backup_Id = 0;
                Int32.TryParse(Request.QueryString["Backup_Id"], out Backup_Id);
                if (Backup_Id <= 0)
                {
                    lblMsg.Text = "<span style='color:red;'>Sorry. The OrderNumber specified is wrong.</span>";
                    return;
                }
                BindOrder(Backup_Id);
            }
        }

        /// <summary>
        /// Binds the Order Grid
        /// </summary>
        private void BindOrderGrid(int OrderNumber)
        {
            DataSet ds = CommonComponent.GetCommonDataSet("select * from tb_orderedShoppingCart_backup where orderNumber=" + OrderNumber + "");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvOrders.DataSource = ds;
                gvOrders.DataBind();
            }
            else
            {
                gvOrders.DataSource = null;
                gvOrders.DataBind();
            }
        }

        /// <summary>
        /// Binds the Order
        /// </summary>
        /// <param name="Backup_Id">int Backup_Id</param>
        private void BindOrder(int Backup_Id)
        {
            DataSet dsbackOrder = new DataSet();
            dsbackOrder = CommonComponent.GetCommonDataSet("select CreatedOn,OrderNumber,OrderTax,LevelDiscountAmount,CouponDiscountAmount, " +
                                                           " QuantityDiscountAmount,GiftCertificateDiscountAmount,OrderedShoppingCart_backup_Id,SubTotal,Total,ShippingAmount, " +
                                                           " DiscountAmount,ShippingMethodID,ShippingMethod from tb_OrderedShoppingCart_backup  " +
                                                           " where OrderedShoppingCart_backup_Id=" + Backup_Id + "");
            if (dsbackOrder != null && dsbackOrder.Tables.Count > 0 && dsbackOrder.Tables[0].Rows.Count > 0)
            {
                string State = string.Empty;

                if (Request.QueryString["RONo"] != null)
                    ltOrderNo.Text = "Number : " + Request.QueryString["RONo"];

                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["ShippingMethod"].ToString()))
                    litOrgShippingMethod.Text = dsbackOrder.Tables[0].Rows[0]["ShippingMethod"].ToString();

                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["CreatedOn"].ToString()))
                {
                    try
                    {
                        DateTime CreatedOn = new DateTime();
                        CreatedOn = Convert.ToDateTime(dsbackOrder.Tables[0].Rows[0]["CreatedOn"].ToString());
                        ltDate.Text = "Changed On:- " + CreatedOn.ToString();
                    }
                    catch { }

                }
                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["SubTotal"].ToString()))
                {
                    Decimal OrderTSubTot = Convert.ToDecimal(dsbackOrder.Tables[0].Rows[0]["SubTotal"].ToString());
                    litOrgSubTotal.Text = OrderTSubTot.ToString("f2");
                }

                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["DiscountAmount"].ToString()))
                {
                    Decimal DiscountAmount = Convert.ToDecimal(dsbackOrder.Tables[0].Rows[0]["DiscountAmount"].ToString());
                    litOrgOtherDiscount.Text = DiscountAmount.ToString("f2");
                }

                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["DiscountAmount"].ToString()))
                {
                    Decimal DiscountAmount = Convert.ToDecimal(dsbackOrder.Tables[0].Rows[0]["DiscountAmount"].ToString());
                    litOrgOtherDiscount.Text = DiscountAmount.ToString("f2");
                }

                decimal LevelDiscountAmount = 0, CouponDiscountAmount = 0, QuantityDiscountAmount = 0, GiftCertificateDiscountAmount = 0;
                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()))
                    LevelDiscountAmount = Convert.ToDecimal(dsbackOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString());
                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString()))
                    CouponDiscountAmount = Convert.ToDecimal(dsbackOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString());
                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()))
                    QuantityDiscountAmount = Convert.ToDecimal(dsbackOrder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString());
                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString()))
                    GiftCertificateDiscountAmount = Convert.ToDecimal(dsbackOrder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString());

                Decimal TotalDiscount = LevelDiscountAmount + CouponDiscountAmount + QuantityDiscountAmount + GiftCertificateDiscountAmount;
                litOrgDiscount.Text = TotalDiscount.ToString("f2");

                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["OrderTax"].ToString()))
                {
                    Decimal OrderTax = Convert.ToDecimal(dsbackOrder.Tables[0].Rows[0]["OrderTax"].ToString());
                    litOrgTax.Text = OrderTax.ToString("f2");
                }
                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["ShippingAmount"].ToString()))
                {
                    Decimal OrderShippingCosts = Convert.ToDecimal(dsbackOrder.Tables[0].Rows[0]["ShippingAmount"].ToString());
                    litOrgShippingCost.Text = OrderShippingCosts.ToString("f2");
                }
                if (!string.IsNullOrEmpty(dsbackOrder.Tables[0].Rows[0]["Total"].ToString()))
                {
                    Decimal OrderTotal = Convert.ToDecimal(dsbackOrder.Tables[0].Rows[0]["Total"].ToString());
                    litOrgTotal.Text = OrderTotal.ToString("f2");
                }

                DataSet dsProduct = new DataSet();
                dsProduct = CommonComponent.GetCommonDataSet("select ISNULL(ci.DiscountPrice,0) as DiscountPrice,OrderNumber,OrderedShoppingCart_backup_Id,ci.Avail,ci.ProductID,p.Name,p.SKU,ci.[Quantity],ci.[Price] as SalePrice,isnull(ci.[VariantNames],'') as 'VariantNames', isnull(ci."
                                                             + " [VariantValues],'') as 'VariantValues',ci.createdon from tb_OrderedShoppingCartItems_backup ci join tb_Product p on ci.ProductID=p.ProductID "
                                                             + " where OrderedShoppingCart_backup_Id=" + Backup_Id + " order by createdon");
                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                {
                    litProducts.Text = BindCart(dsProduct);
                }
            }

        }

        /// <summary>
        /// Binds the Cart Items for Display Cart
        /// </summary>
        /// <param name="dsCartItem">Dataset dsCartItem</param>
        /// <returns>Returns the output value as a string formate which contains HTML</returns>
        private string BindCart(DataSet dsCartItem)
        {
            StringBuilder Table = new StringBuilder();
            bool IsCouponDiscount = false;
            if (dsCartItem != null && dsCartItem.Tables.Count > 0 && dsCartItem.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < dsCartItem.Tables[0].Rows.Count; k++)
                {
                    decimal CouponDiscount = 0;
                    if (!string.IsNullOrEmpty(dsCartItem.Tables[0].Rows[k]["DiscountPrice"].ToString()))
                    {
                        decimal.TryParse(dsCartItem.Tables[0].Rows[k]["DiscountPrice"].ToString(), out CouponDiscount);
                        if (CouponDiscount > Decimal.Zero)
                        {
                            IsCouponDiscount = true;
                        }
                    }
                }

                Decimal NetPrice = Decimal.Zero;
                Table.Append(" <table border='0' cellpadding='2' cellspacing='1' class='table-noneforOrder' width='715px'> ");
                Table.Append("<tbody><tr class='list-table-title' style='height:20px;background-color: #F2F2F2;'>");
                Table.Append("<th align='left' valign='middle' style='width:45%' >Product</th>");
                Table.Append("<th align='left' valign='middle' style='width:15%' >SKU</th>");
                Table.Append("<th align='center' valign='middle' style='width:10%'>Price</th>");
                Table.Append("###coupondiscount###");
                Table.Append("<th align='center' valign='middle' style='width:10%'>Quantity</th>");
                Table.Append("<th style='text-align: right;width:15%;'>Sub Total:</th>");

                Table.Append("</tr>");
                for (int i = 0; i < dsCartItem.Tables[0].Rows.Count; i++)
                {
                    NetPrice = Decimal.Zero;
                    NetPrice = Math.Round((Convert.ToDecimal(dsCartItem.Tables[0].Rows[i]["SalePrice"].ToString()) * Convert.ToDecimal(dsCartItem.Tables[0].Rows[i]["Quantity"].ToString())), 2);
                    Table.Append("<tr class='list_table_cell_link' style='background:none;'>");
                    string ProductName = "";
                    ProductName = dsCartItem.Tables[0].Rows[i]["Name"].ToString();
                    string m_VariantNames = dsCartItem.Tables[0].Rows[i]["VariantNames"].ToString();
                    string m_VariantValues = dsCartItem.Tables[0].Rows[i]["VariantValues"].ToString();
                    Table.Append("<td align='left' valign='top'><b>" + ProductName + "</b>");

                    string[] Names = m_VariantNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] Values = m_VariantValues.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for (int iLoopValues = 0; iLoopValues < Values.Length; iLoopValues++)
                    {
                        if (Values.Length == Names.Length)
                            Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
                        else
                            Table.Append("<br/> - " + Values[iLoopValues]);
                    }
                    Table.Append("</td>");
                    Table.Append("<td align='left' valign='top'>" + dsCartItem.Tables[0].Rows[i]["SKU"].ToString() + "</td>");
                    Table.Append("<td valign='top' align='center'>$" + Convert.ToDecimal(dsCartItem.Tables[0].Rows[i]["SalePrice"].ToString()).ToString("f2") + "</td>");

                    decimal CouponDiscount = 0;
                    if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCartItem.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                    {
                        decimal.TryParse(dsCartItem.Tables[0].Rows[i]["DiscountPrice"].ToString(), out CouponDiscount);
                        Table.Append("<td style=\"text-align: right;\">" + Convert.ToDecimal(CouponDiscount).ToString("C") + "</td>");
                    }

                    Table.Append("<td valign='top' align='center'>" + Convert.ToDecimal(dsCartItem.Tables[0].Rows[i]["Quantity"].ToString()) + "</td>");
                    if (CouponDiscount > 0)
                    {
                        NetPrice = Math.Round((Convert.ToDecimal(CouponDiscount) * Convert.ToDecimal(dsCartItem.Tables[0].Rows[i]["Quantity"].ToString())), 2);
                    }
                    Table.Append("<td  valign='top' align='right'> $" + NetPrice.ToString() + "</td>");
                    Table.Append(" </tr>");
                }
                Table.Append("</tbody></table>");
            }

            if (IsCouponDiscount == true && Table.ToString().ToLower().IndexOf("###coupondiscount###") > -1)
            {
                string StrCoupon = "<th align='center' valign='middle' style='width:10%'>Discount Price</th>";
                Table = Table.Replace("###coupondiscount###", StrCoupon.ToString().Trim());
            }
            else
            {
                Table = Table.Replace("###coupondiscount###", "");
            }

            return Table.ToString();
        }

    }
}