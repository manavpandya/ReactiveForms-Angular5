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
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ChangeOrder : BasePage
    {
        public int OrderNumber;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["ONo"] != null)
            {
                Int32 StoreID = 0;
                OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + OrderNumber)), out StoreID);
                AppConfig.StoreID = StoreID;
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["ONo"] != null)
                {
                    Session["CustCouponCode"] = null;
                    Session["CustCouponCodeDiscount"] = null;

                    OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
                    lnkOldOrders.Attributes.Add("onclick", "OpenCenterWindow('OldShoppingCart.aspx?ONo=" + OrderNumber + "',850,600);");

                    decimal DiscPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 Isnull(DiscountPrice,0) as DiscountPrice from tb_ChangeOrderShoppingCartItems Where OrderNumber=" + OrderNumber + ""));
                    if (DiscPrice > 0)
                    {
                        DataSet dsTemp = new DataSet();
                        dsTemp = CommonComponent.GetCommonDataSet("Select ISNULL(DiscountPercent,0) DiscountPercent,ISNULL(CouponCode,'') as CouponCode,FromDate,ToDate from tb_Customer Where CustomerID in (Select CustomerID from tb_Order where OrderNumber=" + OrderNumber + ")");
                        if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                        {
                            string StrFromdate = Convert.ToString(dsTemp.Tables[0].Rows[0]["FromDate"].ToString());
                            string StrTodate = Convert.ToString(dsTemp.Tables[0].Rows[0]["ToDate"].ToString());
                            if (!string.IsNullOrEmpty(StrFromdate.Trim()) && !string.IsNullOrEmpty(StrTodate.Trim()))
                            {
                                if (ChkDiscountDateRange(StrFromdate, StrTodate) == true)
                                {
                                    if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["CouponCode"].ToString()))
                                    {
                                        Session["CustCouponCode"] = dsTemp.Tables[0].Rows[0]["CouponCode"].ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) && Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) > 0)
                                    {
                                        Session["CustCouponCodeDiscount"] = dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["CouponCode"].ToString()))
                                {
                                    Session["CustCouponCode"] = dsTemp.Tables[0].Rows[0]["CouponCode"].ToString();
                                }
                                if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) && Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString()) > 0)
                                {
                                    Session["CustCouponCodeDiscount"] = dsTemp.Tables[0].Rows[0]["DiscountPercent"].ToString();
                                }
                            }
                        }
                    }

                    GetOrderDetailsByOrderNumber(Convert.ToInt32(OrderNumber));
                    if (Session["AdminID"] != null && Session["AdminID"].ToString() != "")
                    {
                        Boolean IsSalesManager = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select ISNULL(isSalesManager,0) as isSalesManager from tb_Admin where AdminID = " + Session["AdminID"].ToString() + " and ISNULL(Deleted,0)=0 and ISNULL(Active,0)=1"));
                        if (IsSalesManager)
                            hdnIsSalesManager.Value = "0";
                        else
                            hdnIsSalesManager.Value = "1";
                    }
                }
                else
                {
                    grdProducts.DataSource = null;
                    grdProducts.DataBind();
                }
            }
            if (Request.QueryString["ONo"] != null)
            {
                //lblOrderNo.Text = Convert.ToString(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
            }
        }

        protected bool ChkDiscountDateRange(string StrFromdate, string StrTodate)
        {
            if (!string.IsNullOrEmpty(StrFromdate.Trim()) && !string.IsNullOrEmpty(StrTodate.Trim()))
            {
                DateTime FDate = new DateTime();
                DateTime TDate = new DateTime();
                DateTime Currdate = System.DateTime.Now;
                try { FDate = Convert.ToDateTime(StrFromdate.Trim()); }
                catch { }

                try { TDate = Convert.ToDateTime(StrTodate.Trim()); }
                catch { }

                if (Convert.ToDateTime(FDate.ToString("MM/dd/yyyy")) <= Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy")) && Convert.ToDateTime(TDate.ToString("MM/dd/yyyy")) >= Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy")))
                {
                    return true;
                }
                else { return false; }
            }
            return false;
        }

        /// <summary>
        /// Gets the Order Details by Order Number
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        private void GetOrderDetailsByOrderNumber(Int32 OrderNumber)
        {
            DataSet dsOrder = new DataSet();
            dsOrder = OrderComponent.GetOrderDetailsByOrderNumber(Convert.ToInt32(OrderNumber));
            DataSet dtPackProduct = new DataSet();
            Int32 OrderedShoppingCartID = 0, StoreID = 0;
            string PaymentGateway = "";
            string CustomerID = "";
            bool Captured = false;
            int Result = Convert.ToInt32(OrderComponent.ImportOrderCartforChangeOrder(OrderNumber));

            if (Result > 0)
            {
                decimal OrderTotal = 0;
                DataSet dsProduct = new DataSet();
                dsProduct = CommonComponent.GetCommonDataSet("select isnull(ci.RelatedproductID,0) as RelatedproductID,ISNULL(ci.DiscountPrice,0) as DiscountPrice,ci.ProductID,p.Name,p.SKU,ci.[Quantity],ci.[Price] as Price,(ISNULL(ci.[Price],0) * ISNULL(ci.[Quantity],0)) as SalePrice,isnull(ci.[VariantNames],'') as 'VariantNames', " +
                                                            " isnull(ci.[VariantValues],'') as 'VariantValues',ci.createdon from tb_ChangeOrderShoppingCartItems ci  " +
                                                            " join tb_Product p on ci.ProductID=p.ProductID where OrderNumber=" + OrderNumber + " order by createdon");
                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                {
                    grdProducts.DataSource = dsProduct;
                    grdProducts.DataBind();

                    #region Binding from Order Table

                    OrderComponent objOrder = new OrderComponent();
                    DataSet objDsorder = new DataSet();
                    objDsorder = objOrder.GetOrderDetailsByOrderID(OrderNumber);
                    if (objDsorder != null && objDsorder.Tables.Count > 0 && objDsorder.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["OrderSubtotal"].ToString()))
                            litOrgSubTotal.Text = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderSubtotal"]).ToString("f2");

                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["OrderTax"].ToString()))
                            litOrgTax.Text = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderTax"]).ToString("f2");

                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()))
                            litOrgShippingCost.Text = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderShippingCosts"]).ToString("f2");
                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["OrderTotal"].ToString()))
                        {
                            OrderTotal = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderTotal"]);
                            litOrgTotal.Text = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderTotal"]).ToString("f2");
                        }
                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString()))
                            litOrgShippingMethod.Text = objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString();

                        decimal LevelDiscountAmount = 0, CouponDiscountAmount = 0, QuantityDiscountAmount = 0, GiftCertificateDiscountAmount = 0;

                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()))
                            LevelDiscountAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString());
                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString()))
                            CouponDiscountAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString());
                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()))
                            QuantityDiscountAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString());
                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString()))
                            GiftCertificateDiscountAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString());

                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["PaymentGateway"].ToString()))
                            PaymentGateway = objDsorder.Tables[0].Rows[0]["PaymentGateway"].ToString();
                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["CustomerId"].ToString()))
                            CustomerID = objDsorder.Tables[0].Rows[0]["CustomerId"].ToString();

                        Decimal TotalDiscount = LevelDiscountAmount + CouponDiscountAmount + QuantityDiscountAmount + GiftCertificateDiscountAmount;

                        litOrgDiscount.Text = TotalDiscount.ToString("f2");
                        litCurDiscount.Text = TotalDiscount.ToString("f2");

                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["CustomDiscount"].ToString()))
                        {
                            decimal CustomDiscountAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["CustomDiscount"].ToString());
                            if (CustomDiscountAmount > 0)
                                litOrgCustomDiscount.Text = CustomDiscountAmount.ToString("f2");
                        }

                        if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["CapturedOn"].ToString()))
                        {
                            Captured = true;
                        }
                        if (Captured)
                            litTranState.Text = "(Charged)";
                        else
                            litTranState.Text = "(Not charged yet)";

                        ViewState["Transactionstatus"] = objDsorder.Tables[0].Rows[0]["Transactionstatus"].ToString();

                    }

                    #endregion end Binding from Order Table

                    DataSet dsChangeOrder = new DataSet();
                    dsChangeOrder = CommonComponent.GetCommonDataSet("select OrderTax,ReturnedFee,ReturnedStarck,SubTotal,Total,ShippingAmount,DiscountAmount,ShippingMethodID,ShippingMethod from tb_ChangeOrderedShoppingCart where OrderNumber=" + OrderNumber + "");
                    if (dsChangeOrder != null && dsChangeOrder.Tables.Count > 0 && dsChangeOrder.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsChangeOrder.Tables[0].Rows[0]["SubTotal"].ToString()))
                            litCurSubtotal.Text = Convert.ToDecimal(dsChangeOrder.Tables[0].Rows[0]["SubTotal"]).ToString("f2");

                        if (!string.IsNullOrEmpty(dsChangeOrder.Tables[0].Rows[0]["OrderTax"].ToString()))
                            litCurOrgTax.Text = Convert.ToDecimal(dsChangeOrder.Tables[0].Rows[0]["OrderTax"]).ToString("f2");

                        if (!string.IsNullOrEmpty(dsChangeOrder.Tables[0].Rows[0]["DiscountAmount"].ToString()))
                        {
                            decimal DiscountAmount = Convert.ToDecimal(dsChangeOrder.Tables[0].Rows[0]["DiscountAmount"].ToString());
                            if (DiscountAmount > 0)
                            {
                                chkDiscount.Checked = true;
                                txtDiscount.Enabled = true;
                                txtDiscount.Text = DiscountAmount.ToString("f2");
                            }
                        }

                        decimal ChangeOrderShippingCosts = 0;
                        if (!string.IsNullOrEmpty(dsChangeOrder.Tables[0].Rows[0]["ShippingAmount"].ToString()))
                        {
                            ChangeOrderShippingCosts = Convert.ToDecimal(dsChangeOrder.Tables[0].Rows[0]["ShippingAmount"].ToString());
                        }

                        litCurShippingCost.Text = ChangeOrderShippingCosts.ToString("f2");

                        string State = string.Empty;

                        if (string.IsNullOrEmpty(dsChangeOrder.Tables[0].Rows[0]["ShippingMethod"].ToString()))
                        {
                            chkShipping.Checked = true;
                            txtShipping.Text = ChangeOrderShippingCosts.ToString("f2");
                        }

                        decimal ChangeOrderTotal = 0;
                        if (!string.IsNullOrEmpty(dsChangeOrder.Tables[0].Rows[0]["Total"].ToString()))
                        {
                            ChangeOrderTotal = Convert.ToDecimal(dsChangeOrder.Tables[0].Rows[0]["Total"]);
                        }

                        Decimal Diffrence = OrderTotal - ChangeOrderTotal;

                        if (Diffrence > 0)
                        {
                            litCurTotal.Text = ChangeOrderTotal.ToString("f2") + " ($" + Diffrence.ToString("f2") + " Less than Original)";
                            if (Captured)
                            {

                                btnSaveAndProcess.Text = "SAVE AND REFUND $" + Diffrence.ToString("f2");
                                txthOldTotal.Value = "0";
                            }
                            else
                            {
                                //if (ViewState["Transactionstatus"] != null &&(ViewState["Transactionstatus"].ToString().ToLower().Trim() == "authorized" || ViewState["Transactionstatus"].ToString().ToLower().Trim() == "pending"))
                                //{
                                btnSave.Visible = true;
                                btnSaveAndProcess.Visible = false;
                                //}
                                //else
                                //{
                                //    btnSaveAndProcess.Text = "SAVE AND CHARGE $" + ChangeOrderTotal.ToString("f2");
                                //}

                                txthOldTotal.Value = ChangeOrderTotal.ToString("f2");
                            }
                        }
                        else if (Diffrence < 0)
                        {
                            litCurTotal.Text = ChangeOrderTotal.ToString("f2") + " ($" + (-Diffrence).ToString("f2") + " More than Original)";

                            if (!Captured)
                            {
                                //if (ViewState["Transactionstatus"] != null && (ViewState["Transactionstatus"].ToString().ToLower().Trim() == "authorized" || ViewState["Transactionstatus"].ToString().ToLower().Trim() == "pending"))
                                //{
                                btnSave.Visible = true;
                                btnSaveAndProcess.Visible = false;
                                //}
                                //else
                                //{
                                //    btnSaveAndProcess.Text = "SAVE AND CHARGE $" + ChangeOrderTotal.ToString("f2");
                                //}
                                txthOldTotal.Value = OrderTotal.ToString("f2");
                            }
                            else
                            {
                                //if (ViewState["Transactionstatus"] != null && (ViewState["Transactionstatus"].ToString().ToLower().Trim() == "authorized" || ViewState["Transactionstatus"].ToString().ToLower().Trim() == "pending"))
                                //{
                                btnSave.Visible = true;
                                btnSaveAndProcess.Visible = false;
                                //}
                                //else
                                //{
                                //    btnSaveAndProcess.Text = "SAVE AND CHARGE $" + (-Diffrence).ToString("f2");
                                //}
                                txthOldTotal.Value = "0";
                            }
                        }
                        else
                        {
                            //if (ViewState["Transactionstatus"] != null && (ViewState["Transactionstatus"].ToString().ToLower().Trim() == "authorized" || ViewState["Transactionstatus"].ToString().ToLower().Trim() == "pending"))
                            //{
                            btnSave.Visible = false;
                            //}
                            btnSaveAndProcess.Visible = false;
                            litCurTotal.Text = ChangeOrderTotal.ToString("f2");
                            txthOldTotal.Value = "0";
                        }

                        btnSaveAndProcess.CommandArgument = Diffrence.ToString();
                        hfCustomer.Value = CustomerID.ToString();
                        hfGateway.Value = PaymentGateway;
                    }
                }
            }

            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShoppingCardID"]), out OrderedShoppingCartID);
                Int32.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["StoreID"]), out StoreID);

                lnkAddNew.Attributes.Add("onclick", "OpenCenterWindow('ProductSkus.aspx?StoreID=" + StoreID + "&ono=" + OrderNumber + "&clientid=" + CustomerID + "&CustID=" + CustomerID + "',800,600);");
                lnkAddNewBottom.Attributes.Add("onclick", "OpenCenterWindow('ProductSkus.aspx?StoreID=" + StoreID + "&ono=" + OrderNumber + "&clientid=" + CustomerID + "&CustID=" + CustomerID + "',800,600);");
            }
        }

        /// <summary>
        /// Products Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Label lblHeaderDiscount = e.Row.FindControl("lblHeaderDiscount") as Label;
                if (Session["CustCouponCodeDiscount"] != null && Session["CustCouponCode"] != null)
                {
                    decimal Discount = 0;
                    decimal.TryParse(Session["CustCouponCodeDiscount"].ToString(), out Discount);
                    lblHeaderDiscount.Text = "(" + Discount.ToString("f2") + "%)";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btndel = (ImageButton)e.Row.FindControl("btndel");
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                Label lblQty = (Label)e.Row.FindControl("lblQty");
                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                Label lblVariantNames = (Label)e.Row.FindControl("lblVariantNames");
                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                Label lblName = (Label)e.Row.FindControl("lblName");
                Label lblOrginalDiscountPrice = (Label)e.Row.FindControl("lblOrginalDiscountPrice");
                Label lblSubTotal = (Label)e.Row.FindControl("lblSubTotal");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("btndel");
                Label lblRelatedproductID = (Label)e.Row.FindControl("lblRelatedproductID");
                if (lblRelatedproductID != null && lblRelatedproductID.Text.ToString().Trim() != "0")
                {
                    imgDelete.Visible = false;
                }

                if (lblName != null)
                {
                    System.Text.StringBuilder Table = new System.Text.StringBuilder();
                    Table.Append(lblName.Text.Trim());
                    string[] Names = lblVariantNames.Text.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] Values = lblVariantValues.Text.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    int iLoopValues = 0;
                    if (Names.Length == Values.Length)
                    {
                        for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                        {
                            Table.Append("<br/>&nbsp;" + Names[iLoopValues] + " : " + Values[iLoopValues]);
                        }
                    }
                    else if (Values.Length > 0)
                    {
                        for (iLoopValues = 0; iLoopValues < Values.Length; iLoopValues++)
                        {
                            Table.Append("<br/>&nbsp;- " + Values[iLoopValues]);
                        }
                    }
                    else if (Names.Length > 0)
                    {
                        for (iLoopValues = 0; iLoopValues < Names.Length; iLoopValues++)
                        {
                            Table.Append("<br/>&nbsp;- " + Names[iLoopValues]);
                        }
                    }
                    lblName.Text = Table.ToString();
                }
                int Qty = Convert.ToInt32(lblQty.Text.ToString());
                if (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null)
                {
                    decimal DiscountPrice = 0, OrgiPrice = 0;
                    decimal.TryParse(Session["CustCouponCodeDiscount"].ToString(), out DiscountPrice);
                    decimal.TryParse(lblPrice.Text.ToString().Trim().ToString(), out OrgiPrice);
                    if (DiscountPrice > 0)
                    {
                        grdProducts.Columns[5].Visible = true;
                    }
                    else
                    {
                        lblOrginalDiscountPrice.Text = "0.00";
                        grdProducts.Columns[5].Visible = false;
                    }
                }
                else
                {
                    lblOrginalDiscountPrice.Text = "0.00";
                    grdProducts.Columns[5].Visible = false;
                }

                if (lblOrginalDiscountPrice != null && !string.IsNullOrEmpty(lblOrginalDiscountPrice.Text.ToString().Trim()) && Session["CustCouponCodeDiscount"] != null && Session["CustCouponCode"] != null)
                {
                    Decimal DiscountPrice = Convert.ToDecimal(lblOrginalDiscountPrice.Text.ToString());
                    decimal Subtot = Convert.ToDecimal(Qty) * DiscountPrice;
                    lblSubTotal.Text = Convert.ToDecimal(Subtot).ToString("f2");
                }
                else
                {
                    if (!string.IsNullOrEmpty(lblQty.Text.ToString()) && !string.IsNullOrEmpty(lblPrice.Text.ToString()))
                    {
                        Decimal Price = Convert.ToDecimal(lblPrice.Text.ToString());
                        decimal Subtot = Convert.ToDecimal(Qty) * Price;
                        lblSubTotal.Text = Convert.ToDecimal(Subtot).ToString("f2");
                    }
                }
            }
        }

        /// <summary>
        /// Products Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "delMe")
                {
                    int Cnt = 0;
                    int i = Convert.ToInt32(e.CommandArgument.ToString());
                    if (grdProducts.Rows.Count > 0)
                    {
                        foreach (GridViewRow rn in grdProducts.Rows)
                        {
                            Label lblRelatedproductID = (Label)rn.FindControl("lblRelatedproductID");
                            if (lblRelatedproductID != null && lblRelatedproductID.Text.ToString() == "0")
                            {
                                Cnt++;
                            }
                        }
                    }

                    if (grdProducts.Rows.Count > 0 && Cnt != 1)
                    {
                        if (Request.QueryString["ONo"] != null)
                        {
                            OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()));
                            Label lblProductId = (Label)grdProducts.Rows[i].FindControl("lblProductID");
                            Label lblVariantNames = (Label)grdProducts.Rows[i].FindControl("lblVariantNames");
                            Label lblVariantValues = (Label)grdProducts.Rows[i].FindControl("lblVariantValues");
                            Int32 ProductId = Convert.ToInt32(lblProductId.Text.ToString());

                            ChangeOrderComponent objchangeorder = new ChangeOrderComponent();
                            bool Result = false;
                            Result = Convert.ToBoolean(objchangeorder.DeleteChangeOrderCartItem(OrderNumber, ProductId, lblVariantNames.Text.ToString(), lblVariantValues.Text.ToString()));
                            GetOrderDetailsByOrderNumber(Convert.ToInt32(OrderNumber));
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Msg", "jAlert('You can not delete this record, One record is require.','Required Information')", true);
                        return;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        ///  Update Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Decimal subTotal = 0;
            Decimal Total = 0;
            Decimal.TryParse(litCurSubtotal.Text, out subTotal);
            Decimal ShippingCost = 0;
            Decimal Discount = 0;
            String ShippingMethod = String.Empty;
            Total = subTotal;

            if (chkShipping.Checked)
            {
                Decimal.TryParse(txtShipping.Text.Trim(), out ShippingCost);
                litCurShippingCost.Text = ShippingCost.ToString("f2");
                Total += ShippingCost;
                ShippingMethod = "Special Shipping";
            }
            else
            {
                Decimal.TryParse(litOrgShippingCost.Text, out ShippingCost);
                ShippingMethod = litOrgShippingMethod.Text;
                Total += ShippingCost;
            }

            Decimal OrderTax = 0;
            Decimal.TryParse(litOrgTax.Text, out OrderTax);
            Total += OrderTax;

            if (chkDiscount.Checked)
            {
                Decimal.TryParse(txtDiscount.Text.Trim(), out Discount);
                Decimal orgDiscount = 0;
                Decimal.TryParse(litOrgDiscount.Text.Trim(), out orgDiscount);
                if (Discount < 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs01", "jAlert('Enter valid Discount Amount.','Sorry!');", true);
                    chkDiscount.Checked = false;
                    txtDiscount.Enabled = false;
                    txtDiscount.Text = "";
                    return;
                }
                try
                {
                    if (hdnIsSalesManager.Value != null && hdnIsSalesManager.Value.ToString() == "1")
                    {
                        if (Convert.ToDecimal(Discount + orgDiscount) > 0)
                        {
                            decimal SubTot = Convert.ToDecimal(subTotal);
                            decimal SubTotDispercent = (SubTot * 10) / 100;
                            decimal TotDiscount = Convert.ToDecimal(Discount + orgDiscount);

                            if (TotDiscount > SubTotDispercent)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Discount Should not be apply grater than 10% of SubTotal.','Required Information','ContentPlaceHolder1_TxtDiscount');", true);
                                txtDiscount.Text = "0";
                                return;
                            }
                        }
                    }
                }
                catch { }

                if ((Discount + orgDiscount) <= Total)
                {
                    Total -= (Discount + orgDiscount);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs02", "jAlert('Total Discount Amount exceeds order total.','Sorry!');", true);
                    chkDiscount.Checked = false;
                    txtDiscount.Enabled = false;
                    txtDiscount.Text = "";
                    Discount = 0;
                    return;
                }
            }
            else
            {
                Decimal OrgDiscount = 0;
                Decimal.TryParse(litOrgDiscount.Text, out OrgDiscount);
                Total -= OrgDiscount;
            }

            btnSaveAndProcess.Visible = true;
            Decimal Diffrence = 0;
            Decimal.TryParse(litOrgTotal.Text, out Diffrence);
            Diffrence -= Total;
            if (Diffrence > 0)
            {
                litCurTotal.Text = Total.ToString("f2") + " ($" + Diffrence.ToString("f2") + " Less than Original)";
                if (litTranState.Text == "(Charged)")
                    btnSaveAndProcess.Text = "SAVE AND REFUND $" + Diffrence.ToString("f2");
                else
                {
                    btnSaveAndProcess.Text = "SAVE AND CHARGE $" + Total.ToString("f2");
                    txthOldTotal.Value = Total.ToString("f2");
                }
            }
            else if (Diffrence < 0)
            {
                litCurTotal.Text = Total.ToString("f2") + " ($" + (-Diffrence).ToString("f2") + " More than Original)";
                if (litTranState.Text != "(Charged)")
                {
                    btnSaveAndProcess.Text = "SAVE AND CHARGE $" + Total.ToString("f2");
                    txthOldTotal.Value = Total.ToString("f2");

                }
                else
                    btnSaveAndProcess.Text = "SAVE AND CHARGE $" + (-Diffrence).ToString("f2");
            }
            else
            {
                if (litTranState.Text != "(Charged)")
                {
                    btnSaveAndProcess.Text = "SAVE AND CHARGE $" + Total.ToString("f2");
                    btnSaveAndProcess.CommandArgument = Total.ToString("f2");
                    txthOldTotal.Value = Total.ToString("f2");
                }
                else
                {
                    btnSaveAndProcess.Text = "SAVE AND CHARGE $(0.00)";
                }
                btnSaveAndProcess.Visible = true;
                litCurTotal.Text = Total.ToString("f2");
            }
            if (litTranState.Text != "(Charged)")
            {

            }
            else
            {
                btnSaveAndProcess.CommandArgument = Diffrence.ToString();
            }
            //if (ViewState["Transactionstatus"] != null && (ViewState["Transactionstatus"].ToString().ToLower().Trim() == "authorized" || ViewState["Transactionstatus"].ToString().ToLower().Trim() == "pending"))
            //{
            btnSave.Visible = true;
            btnSaveAndProcess.Visible = false;
            //} 
            Int32 OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
            ChangeOrderComponent ObjChangeOrder = new ChangeOrderComponent();
            if (ObjChangeOrder.UpdateChangedCart(Convert.ToInt32(OrderNumber), subTotal, Total, ShippingCost, 0, ShippingMethod, Discount))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs03", "jAlert('Order Reviewed Successfully');", true);
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnUpdate_Click(null, null);
                ChangeOrderComponent ObjChangeOrder = new ChangeOrderComponent();
                Int32 OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
                ObjChangeOrder.ExportOrderCart(OrderNumber);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs04", "jAlert('Order Processed Successfully');window.parent.location.href=window.parent.location.href;", true);
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs05", "jAlert('Error while Saving Order. Please Retry..');", true);
            }
        }

        /// <summary>
        ///  Save and Process Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSaveAndProcess_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
                tb_Order objOrder = new tb_Order();
                objOrder = GetOrderDetailsForPayment(OrderNumber);

                btnUpdate_Click(null, null);

                int CustomerID = 0;
                Int32.TryParse(hfCustomer.Value, out CustomerID);

                ChangeOrderComponent ObjChangeOrder = new ChangeOrderComponent();
                Decimal Diffrence = 0;
                Decimal.TryParse(btnSaveAndProcess.CommandArgument, out Diffrence);
                String Status = AppLogic.ro_OK;

                if (Diffrence > 0)
                {
                    Decimal OrderTotal = 0;
                    if (!string.IsNullOrEmpty(txthOldTotal.Value))
                    {
                        OrderTotal = Convert.ToDecimal(txthOldTotal.Value);
                        objOrder.OrderTotal = OrderTotal;
                    }

                    if (hfGateway.Value.ToLower() == "authorizenet")
                    {
                        if (OrderTotal > 0)
                        {
                            Status = AuthorizeNetComponent.CaptureOrder(objOrder);
                            if (Status == AppLogic.ro_OK)
                            {
                                CommonComponent.ExecuteCommonData("update tb_Order set isnew=0,CapturedOn=dateadd(hour,-2,getdate()), TransactionStatus='" + AppLogic.ro_TXStateCaptured + "' where OrderNumber=" + OrderNumber.ToString());
                                ObjChangeOrder.ExportOrderCart(OrderNumber);
                            }
                        }
                        else
                        {
                            Status = AuthorizeNetComponent.RefundOrder(OrderNumber, Diffrence, "Refund Due to Change in Order.");
                            if (Status == AppLogic.ro_OK)
                            {
                                CommonComponent.ExecuteCommonData("update tb_Order set RefundReason='Refund Due to Change in Order.', TransactionStatus='" + AppLogic.ro_TXStateRefunded + "', RefundedOn=dateadd(hour,-2,getdate()), IsNew=0,RefundedAmount=" + Diffrence + " where OrderNumber=" + OrderNumber.ToString());
                                ObjChangeOrder.ExportOrderCart(OrderNumber);
                            }
                        }
                    }
                    else
                    {
                        Status = "Unknown Payment Gateway for Change Order";
                    }
                }
                else if (Diffrence < 0)
                {
                    if (hfGateway.Value.ToLower() == "authorizenet")
                    {
                        String AVSResult = String.Empty;
                        String AuthorizationResult = String.Empty;
                        String AuthorizationCode = String.Empty;
                        String AuthorizationTransID = String.Empty;
                        String TransactionCommand = String.Empty;
                        String TransactionResponse = String.Empty;

                        Decimal OrderTotal = 0;

                        if (!string.IsNullOrEmpty(txthOldTotal.Value))
                        {
                            OrderTotal = Convert.ToDecimal(txthOldTotal.Value);
                            //objOrder.OrderTotal = OrderTotal;
                        }

                        String status1 = "";
                        if (OrderTotal > 0)
                        {
                            status1 = AuthorizeNetComponent.CaptureOrder(objOrder);
                        }
                        else
                        {
                            status1 = AppLogic.ro_OK;
                        }

                        if (status1 == AppLogic.ro_OK)
                        {
                            string status = AuthorizeNetComponent.ProcessCardForAdminSide(OrderNumber, CustomerID, (-Diffrence), AppLogic.AppConfigBool("UseLiveTransactions"), AppLogic.ro_TXModeAuthCapture, objOrder, objOrder, String.Empty, String.Empty, String.Empty, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand, out TransactionResponse);
                            if (status == AppLogic.ro_OK)
                            {

                                CommonComponent.ExecuteCommonData("update tb_Order set CapturedOn=dateadd(hour,-2,getdate()), TransactionStatus='" + AppLogic.ro_TXStateCaptured + "' where OrderNumber=" + OrderNumber);

                                String sql2 = "update tb_order set " +
                                    "AVSResult='" + AVSResult + "', " +
                                    "AuthorizationResult='" + TransactionResponse + "', " +
                                    "AuthorizationCode='" + AuthorizationCode + "', " +
                                    "AuthorizationPNREF=AuthorizationPNREF+'|CAPTURE=" + AuthorizationTransID + "', " +
                                    "TransactionCommand='" + TransactionCommand +
                                "' where OrderNumber=" + OrderNumber;
                                CommonComponent.ExecuteCommonData(sql2);
                                ObjChangeOrder.ExportOrderCart(OrderNumber);
                            }
                            else
                            {
                                Status = status;
                            }
                        }
                        else
                        {
                            Status = status1;
                        }
                    }
                    else
                    {
                        Status = "Unknown Payment Gateway in Refund Order";
                    }
                }
                else
                    ObjChangeOrder.ExportOrderCart(OrderNumber);


                if (Status == AppLogic.ro_OK)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs05", "CheckAlert();", true);
                }
                else
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs05", "jAlert('Error while Performing Transaction. Please retry..');", true);
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs05", "jAlert('Error while Saving Order. Please retry..');", true);
            }
        }

        /// <summary>
        /// Gets the order details for payment.
        /// </summary>
        /// <param name="OrderNumber">int  OrderNumber</param>
        /// <returns>Returns tb_Order Table Object</returns>
        private tb_Order GetOrderDetailsForPayment(Int32 OrderNumber)
        {
            DataSet dsOrder = new DataSet();
            dsOrder = CommonComponent.GetCommonDataSet("select * from tb_order where OrderNumber=" + OrderNumber);
            tb_Order objOrderData = new tb_Order();
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                objOrderData.FirstName = Convert.ToString(dsOrder.Tables[0].Rows[0]["FirstName"].ToString());
                objOrderData.LastName = Convert.ToString(dsOrder.Tables[0].Rows[0]["LastName"].ToString());
                objOrderData.Email = Convert.ToString(dsOrder.Tables[0].Rows[0]["Email"].ToString());

                //Billing Address
                objOrderData.BillingFirstName = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingFirstName"].ToString());
                objOrderData.BillingLastName = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingLastName"].ToString());
                objOrderData.BillingAddress1 = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString());
                objOrderData.BillingAddress2 = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString());
                objOrderData.BillingSuite = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingSuite"].ToString());
                objOrderData.BillingCity = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCity"].ToString());
                objOrderData.BillingState = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingState"].ToString());
                objOrderData.BillingZip = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingZip"].ToString());
                objOrderData.BillingCountry = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCountry"].ToString());
                objOrderData.BillingPhone = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingPhone"].ToString());
                objOrderData.BillingEmail = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingEmail"].ToString());

                // Credit Card Details
                objOrderData.CardName = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardName"].ToString());
                objOrderData.CardType = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardType"].ToString());
                objOrderData.CardVarificationCode = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardVarificationCode"].ToString());
                objOrderData.CardNumber = SecurityComponent.Decrypt(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString());
                objOrderData.CardExpirationMonth = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationMonth"].ToString());
                objOrderData.CardExpirationYear = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString());

                //Shipping Address
                objOrderData.ShippingFirstName = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString());
                objOrderData.ShippingLastName = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString());
                objOrderData.ShippingAddress1 = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString());
                objOrderData.ShippingAddress2 = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString());
                objOrderData.ShippingSuite = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString());
                objOrderData.ShippingCity = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCity"].ToString());
                objOrderData.ShippingState = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingState"].ToString());
                objOrderData.ShippingZip = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingZip"].ToString());
                objOrderData.ShippingCountry = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString());
                objOrderData.ShippingPhone = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString());
                objOrderData.ShippingEmail = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingEmail"].ToString());

                objOrderData.AuthorizationPNREF = Convert.ToString(dsOrder.Tables[0].Rows[0]["AuthorizationPNREF"].ToString());
                objOrderData.OrderTotal = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString());
            }
            return objOrderData;
        }

    }
}