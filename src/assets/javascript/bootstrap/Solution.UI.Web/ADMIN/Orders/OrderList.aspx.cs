#region Name Space
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using Solution.Bussines.Components;
using System.Text;
using System.Security;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using Solution.UI.Web.MicrosoftNavservice;
using System.Configuration;
using System.Net;
using System.IO;


#endregion

namespace Solution.UI.Web.ADMIN.Orders
{
    /// <summary>
    /// Order List For Order related Information     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public partial class OrderList : BasePage
    {
        #region Declaration
        int pageSize = 25;
        int selectedStore = 0;
        const int pageDispCount = 1;
        int checknavclear = 0;
        Decimal hdnSubtotal1 = Decimal.Zero;
        Decimal hdnTotal1 = Decimal.Zero;
        Decimal HdnShippingCost1 = Decimal.Zero;
        Decimal hdnordertax1 = Decimal.Zero;
        Decimal hdnDiscount1 = Decimal.Zero;
        Decimal hdnRefund1 = Decimal.Zero;
        Decimal hdnAdjAmt1 = Decimal.Zero;


        Decimal hdnSubtotal1F = Decimal.Zero;
        Decimal hdnTotal1F = Decimal.Zero;
        Decimal HdnShippingCost1F = Decimal.Zero;
        Decimal hdnordertax1F = Decimal.Zero;
        Decimal hdnDiscount1F = Decimal.Zero;
        Decimal hdnRefund1F = Decimal.Zero;
        Decimal hdnAdjAmt1F = Decimal.Zero;

        Decimal hdncanceledOrder = Decimal.Zero;
        Decimal hdnvoidOrder = Decimal.Zero;


        public bool isAscend = false;



        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {


                ViewState["SearchFilter"] = "";//" AND dbo.tb_Order.orderStatus='All'";
                GetStoreList(ddlStoregeneral);
                if (Request.QueryString["Storeid"] == null && !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
                {
                    ddlStoregeneral.SelectedValue = "0";// Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                    AppConfig.StoreID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                }
                else
                {
                    ddlStoregeneral.SelectedValue = Request.QueryString["StoreID"].ToString();// Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                    AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                }
                BindWareHouse();
                GetOrderListByStoreId(1, pageSize);

                GetPaymentMethod();
                GetState();
                GetSalesAgents();
                btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnadvanceSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/advance-search.png) no-repeat transparent; width: 117px; height: 23px; border:none;cursor:pointer;");
                btnUploadOrder.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/upload-order.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnReset.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/reser-filter.gif";
                btnGo.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/search.gif";
                popupContactClose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel-icon.png";
                btnMultiplePrint.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/printall.gif) no-repeat transparent; width: 75px; height: 23px; border:none;cursor:pointer;");
                btnnavuplod.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/upload-orders-to-nav.gif) no-repeat transparent; width: 150px; height: 23px; border:none;cursor:pointer;");

            }

            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && !Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
            {
                if (ddlStoregeneral.SelectedValue == "1")
                {
                    imgbtnMultipleCapture.Visible = true;
                    imgbtnMultipleCapture.Attributes.Add("src", "/App_Themes/" + Page.Theme.ToString() + "/images/multi-capture.gif");
                }
                else
                {
                    imgbtnMultipleCapture.Visible = false;
                }
            }
            else
            {
                imgbtnMultipleCapture.Visible = false;
            }
            try
            {
                if (!IsPostBack)
                {
                    foreach (GridViewRow gr in grvOrderlist.Rows)
                    {
                        Button btnSearch = (Button)gr.FindControl("btnSearch");
                        Page.Form.DefaultButton = btnSearch.UniqueID.ToString();
                        break;
                    }
                }

            }
            catch
            {

            }

            //if (ddlPageRecord.Items.Count > 0)
            //{
            pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue);
            //}
            //else
            //{
            //    pageSize = 50;
            //}
            grvOrderlist.PageSize = pageSize;
            SetAdminPageRight();

            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")) && ddlStoregeneral.SelectedValue.ToString() != "0")
            {
                btnnavuplod.Visible = true;
                btnUploadOrder.Visible = false;


            }
            else
            {
                btnnavuplod.Visible = false;
                btnUploadOrder.Visible = false;
                if (ddlStoregeneral.SelectedValue.ToString() == "0")
                {
                    btnUploadOrder.Visible = false;
                }

            }

        }

        /// <summary>
        /// Get Order Data From Database
        /// </summary>
        /// <param name="PageNumber">PageNumber</param>
        /// <param name="PageSize">Grid PageSize</param>
        private void GetOrderListByStoreId(Int32 PageNumber, Int32 PageSize)
        {
            DataSet dsOrder = new DataSet();
            string OrderNumbers = "";
            string strtext = "";
            if (ddlwarehouse.SelectedIndex > 0)
            {
                if (ddlStoregeneral.SelectedIndex > 0)
                {
                    OrderNumbers = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT cast(OrderNumber as nvarchar(max))+',' FROM tb_Order WHERE StoreId=" + ddlStoregeneral.SelectedValue.ToString() + " AND ShoppingCardID in (SELECT OrderedShoppingCartID FROM tb_OrderedShoppingCartItems INNER JOIN tb_product  ON tb_OrderedShoppingCartItems.RefProductId=tb_product.ProductID LEFT OUTER JOIN tb_WareHouseProductInventory on tb_WareHouseProductInventory.ProductId=tb_OrderedShoppingCartItems.RefProductId WHERE tb_WareHouseProductInventory.WareHouseID=" + ddlwarehouse.SelectedValue.ToString() + " ) FOR XML PATH('')"));
                }
                else
                {
                    OrderNumbers = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT cast(OrderNumber as nvarchar(max))+',' FROM tb_Order WHERE ShoppingCardID in (SELECT OrderedShoppingCartID FROM tb_OrderedShoppingCartItems INNER JOIN tb_product  ON tb_OrderedShoppingCartItems.RefProductId=tb_product.ProductID LEFT OUTER JOIN tb_WareHouseProductInventory on tb_WareHouseProductInventory.ProductId=tb_product.ProductID WHERE tb_WareHouseProductInventory.WareHouseID=" + ddlwarehouse.SelectedValue.ToString() + " ) FOR XML PATH('')"));
                }
                //OrderNumbers = objOrderproduct.GetOrderByProductDetails(txtOrderNo.Text.ToString().Replace("'", "''"));
                if (!string.IsNullOrEmpty(OrderNumbers) && OrderNumbers.Length > 1)
                {
                    if (OrderNumbers.Substring(OrderNumbers.Length - 1, 1) == ",")
                    {
                        OrderNumbers = OrderNumbers.Substring(0, OrderNumbers.Length - 1);
                    }
                    strtext += " AND tb_order.orderNumber in (" + OrderNumbers + ")";

                }
            }
            if (ViewState["SearchFilter"] != null)
            {
                dsOrder = OrderComponent.GetorderListByStoreId(PageNumber, PageSize, ViewState["SearchFilter"].ToString() + " " + strtext, Convert.ToInt32(ddlStoregeneral.SelectedValue.ToString()));
            }
            else
            {
                dsOrder = OrderComponent.GetorderListByStoreId(PageNumber, PageSize, strtext, Convert.ToInt32(ddlStoregeneral.SelectedValue.ToString()));
            }
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                Session["GridDataTable"] = dsOrder;
                grvOrderlist.DataSource = dsOrder;
                grvOrderlist.DataBind();

                if (dsOrder.Tables[0].Rows.Count > 1)
                {
                    managePaging(Convert.ToInt32(dsOrder.Tables[0].Rows[1]["TotalRows"].ToString()));
                }
                else
                {
                    if (IsPostBack)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('No Record(s) Found.','Sorry!!!');", true);
                        btnUploadOrder.Visible = false;
                        if (grvOrderlist.Rows.Count == 1)
                        {
                            System.Web.UI.HtmlControls.HtmlTable chkUploadvalue = (System.Web.UI.HtmlControls.HtmlTable)grvOrderlist.Rows[0].FindControl("chkUploadvalue");
                            chkUploadvalue.Visible = false;
                        }
                    }
                    managePaging(0);
                }

            }
            else
            {
                grvOrderlist.DataSource = null;
                grvOrderlist.DataBind();
            }
        }

        private void BindWareHouse()
        {
            string sqlWareHouse = "SELECT WareHouseID,Name FROM dbo.tb_WareHouse WHERE Active=1 AND ISNULL(Deleted,0)=0";
            DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);

            if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
            {
                ddlwarehouse.DataSource = dsWareHouse;
                ddlwarehouse.DataTextField = "Name";
                ddlwarehouse.DataValueField = "WareHouseID";
            }
            else
            {
                ddlwarehouse.DataSource = null;
            }
            ddlwarehouse.DataBind();
            ddlwarehouse.Items.Insert(0, new ListItem("All", "0"));
            ddlwarehouse.SelectedIndex = 0;
        }

        /// <summary>
        /// Order List Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grvOrderlist_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.Pager)
            {

                e.Row.Visible = false;
            }

        }

        /// <summary>
        /// Make Custom Paging
        /// </summary>
        /// <param name="_dt">int _dt</param>
        protected void managePaging(Int32 _dt)
        {
            //ddlPageRecord.Items.Clear();
            if (_dt > 0)
            {
                ltrprevious.Controls.Clear();
                ltrnext.Controls.Clear();
                // Variable declaration
                int numberOfPages;
                int numberOfRecords = _dt;
                int currentPage = (grvOrderlist.PageIndex);
                StringBuilder strSummary = new StringBuilder();

                // If number of records is more then the page size (specified in global variable)
                // Just to check either gridview have enough records to implement paging
                if (numberOfRecords > pageSize)
                {
                    // Calculating the total number of pages
                    numberOfPages = (int)Math.Ceiling((double)numberOfRecords / (double)pageSize);
                    //ddlPageRecord.Items.Insert(0, new ListItem(pageSize.ToString(), pageSize.ToString()));
                    //for (int i = 1; i <= 5; i++)
                    //{
                    //    if (numberOfRecords < (pageSize * (i + 1)))
                    //    {
                    //        ddlPageRecord.Items.Insert(i, new ListItem((pageSize * (i + 1)).ToString(), (pageSize * (i + 1)).ToString()));
                    //    }
                    //}

                }
                else
                {
                    numberOfPages = 1;
                    //ddlPageRecord.Items.Add(new ListItem(pageSize.ToString(), pageSize.ToString()));
                }

                ddlPageRecord.SelectedValue = pageSize.ToString();

                ltrPages.Text = numberOfPages.ToString();
                hdnpages.Value = numberOfPages.ToString();
                ltrRecord.Text = numberOfRecords.ToString();
                ltrnext.Text = "";
                ltrprevious.Text = "";
                if (Convert.ToInt32(hdnpages.Value.ToString()) > Convert.ToInt32(txtPagenumber.Text.ToString()))
                {
                    ltrnext.Text = "<a title=\"Next\" href=\"javascript:void(0)\" onclick=\"javascript:chkHeight(); document.getElementById('" + objpageclicknew.ClientID.ToString() + "').click();\"><img title=\"\" alt=\"\" src=\"/App_Themes/" + this.Theme.ToString() + "/icon/next.png\"></a>";
                }
                if (Convert.ToInt32(txtPagenumber.Text.ToString()) > 1 && Convert.ToInt32(txtPagenumber.Text.ToString()) <= Convert.ToInt32(hdnpages.Value.ToString()))
                {
                    ltrprevious.Text = "<a title=\"Previous\" href=\"javascript:void(0)\" onclick=\"javascript:chkHeight(); document.getElementById('" + objpageclickpre.ClientID.ToString() + "').click();\"><img title=\"\" alt=\"\" src=\"/App_Themes/" + this.Theme.ToString() + "/icon/previous.png\"></a>";
                }

            }
            else
            {
                //ddlPageRecord.Items.Add(new ListItem(pageSize.ToString(), pageSize.ToString()));
                ltrPages.Text = "1";
                hdnpages.Value = "1";
                ltrRecord.Text = "0";
                ltrnext.Text = "";
                ltrprevious.Text = "";
                txtPagenumber.Text = "1";
            }

        }

        /// <summary>
        /// Paging Button click
        /// </summary>
        ///  <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void objLbPre_Click(object sender, EventArgs e)
        {
            if (txtPagenumber.Text.ToString().Trim() != "" && Convert.ToInt32(txtPagenumber.Text.ToString()) > 0)
            {
                ltrprevious.Controls.Clear();
                ltrnext.Controls.Clear();
                int ii = Convert.ToInt32(txtPagenumber.Text.ToString());
                ii = ii - 1;
                if (Convert.ToInt32(hdnpages.Value.ToString()) >= ii)
                {
                    pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue);
                    txtPagenumber.Text = ii.ToString();
                    if (ii > 0)
                    {
                        grvOrderlist.PageIndex = (int.Parse(txtPagenumber.Text.ToString()) - 1);
                    }
                    else
                    {
                        txtPagenumber.Text = "1";
                        grvOrderlist.PageIndex = 0;
                    }
                    GetOrderListByStoreId(Convert.ToInt32(txtPagenumber.Text.ToString()), pageSize);
                }
                else
                {
                    pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue);
                    txtPagenumber.Text = hdnpages.Value.ToString();
                    if (Convert.ToInt32(hdnpages.Value.ToString()) > 0)
                    {
                        grvOrderlist.PageIndex = (int.Parse(txtPagenumber.Text.ToString()) - 1);
                    }
                    else
                    {
                        txtPagenumber.Text = "1";
                        grvOrderlist.PageIndex = 0;
                    }
                    GetOrderListByStoreId(Convert.ToInt32(txtPagenumber.Text.ToString()), pageSize);
                }
            }
            else
            {
                ltrprevious.Controls.Clear();
                ltrnext.Controls.Clear();
                int ii = 0;
                ii = 1;
                if (Convert.ToInt32(hdnpages.Value.ToString()) >= ii)
                {
                    pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue);
                    txtPagenumber.Text = ii.ToString();
                    grvOrderlist.PageIndex = (int.Parse(txtPagenumber.Text.ToString()) - 1);
                    GetOrderListByStoreId(Convert.ToInt32(txtPagenumber.Text.ToString()), pageSize);
                }
                else
                {
                    pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue);
                    txtPagenumber.Text = hdnpages.Value.ToString();
                    grvOrderlist.PageIndex = (int.Parse(txtPagenumber.Text.ToString()) - 1);
                    GetOrderListByStoreId(Convert.ToInt32(txtPagenumber.Text.ToString()), pageSize);
                }
            }
        }

        /// <summary>
        /// Paging Button click
        /// </summary>
        ///  <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void objLb_Click(object sender, EventArgs e)
        {

            ltrprevious.Controls.Clear();
            ltrnext.Controls.Clear();
            int ii = 0;
            if (txtPagenumber.Text.ToString().Trim() == "")
            {
                ii = 1;
            }
            else
            {
                ii = Convert.ToInt32(txtPagenumber.Text.ToString());
                ii = ii + 1;
            }
            if (Convert.ToInt32(hdnpages.Value.ToString()) >= ii)
            {
                pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue);
                txtPagenumber.Text = ii.ToString();
                grvOrderlist.PageIndex = (int.Parse(txtPagenumber.Text.ToString()) - 1);
                GetOrderListByStoreId(Convert.ToInt32(txtPagenumber.Text.ToString()), pageSize);
            }
            else
            {
                pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue);
                txtPagenumber.Text = hdnpages.Value.ToString();
                grvOrderlist.PageIndex = (int.Parse(txtPagenumber.Text.ToString()) - 1);
                GetOrderListByStoreId(Convert.ToInt32(txtPagenumber.Text.ToString()), pageSize);
            }
        }

        /// <summary>
        /// Set Status
        /// </summary>
        /// <param name="statusName">String statusName</param>
        /// <param name="Transtatus">String Transtatus</param>
        /// <param name="ltrStatus">Literal ltrStatus</param>
        /// <param name="ltrAction">Literal ltrAction</param>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="OrderTotal">string OrderTotal</param>
        /// <param name="RefundAmount">string RefundAmount</param>
        /// <param name="CaptureTXResult">string CaptureTXResult</param>
        private void SetStatus(string statusName, string Transtatus, Literal ltrStatus, Literal ltrAction, Int32 OrderNumber, String OrderTotal, string RefundAmount, string CaptureTXResult, string PaymentMethod)
        {

            ltrAction.Text = "<a title=\"\" href=\"Orders.aspx?ID=" + OrderNumber + "&PS=1\"><img title=\"\" alt=\"Print Packing Slip\" src=\"/App_Themes/" + Page.Theme + "/images/print-packing-skip.png\"></a><br />";

            //if (statusName.ToLower().IndexOf("processing") > -1)
            //{
            //    ltrStatus.Text = "<strong style=\"color:#436DA0;\">" + statusName.ToString() + "</strong><br />";
            //}
            //else if (statusName.ToLower().IndexOf("pending") > -1)
            //{
            //    ltrStatus.Text = "<strong style=\"color:#ff0000;\">" + statusName.ToString() + "</strong><br />";
            //}
            //else if (statusName.ToLower().IndexOf("complete") > -1)
            //{
            //    ltrStatus.Text = "<strong style=\"color:#00A818;\">" + statusName.ToString() + "</strong><br />";
            //}
            //else if (statusName.ToLower().IndexOf("cancelled") > -1)
            //{

            //}
            ltrStatus.Text = "";
            if (Transtatus.ToLower() == "authorized")
            {
                ltrStatus.Text += "<strong style=\"color:#FF7F00;font-size:10px;\"><br />" + Transtatus.ToString() + "</strong>";
                ltrStatus.Text += "<br /><span style=\"color:#000000;\">" + statusName.ToString() + "</span>";
                //ltrAction.Text += "<a title=\"\" href=\"javascript:void(0);\"><img title=\"\" alt=\"Confirm Shipment\" src=\"/App_Themes/" + Page.Theme + "/images/confirm-shipment.png\"></a><br />";


                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")) && PaymentMethod.ToString().ToLower().IndexOf("paypal") <= -1)
                {
                }
                else
                {
                    ltrAction.Text += "<a title=\"Capture\" href=\"javascript:void(0);\" onclick=\"CaptureClick(" + OrderNumber.ToString() + ");\"><img title=\"\" alt=\"Capture\" src=\"/App_Themes/" + Page.Theme + "/images/capture.png\"></a><br />";
                }

            }
            else if (Transtatus.ToLower() == "pending")
            {
                ltrStatus.Text += "<strong style=\"color:#D3321C;font-size:10px;\"><br />" + Transtatus.ToString() + "</strong>";
                ltrStatus.Text += "<br /><span style=\"color:#000000;\">" + statusName.ToString() + "</span>";
                //ltrAction.Text += "<a title=\"\" href=\"javascript:void(0);\"><img title=\"\" alt=\"Confirm Shipment\" src=\"/App_Themes/" + Page.Theme + "/images/confirm-shipment.png\"></a><br />";
                //ltrAction.Text += "<a title=\"\" href=\"javascript:void(0);\"><img title=\"\" alt=\"Capture\" src=\"/App_Themes/" + Page.Theme + "/images/capture.png\"></a><br />";
            }
            else if (Transtatus.ToLower() == "captured")
            {
                ltrStatus.Text += "<strong style=\"color:#348934;font-size:10px;\"><br />" + Transtatus.ToString() + "</strong>";
                ltrStatus.Text += "<br /><span style=\"color:#000000;\">" + statusName.ToString() + "</span>";
                if (CaptureTXResult.ToString().Trim() != "" && Convert.ToDecimal(OrderTotal) > decimal.Zero)
                {
                    if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")) && PaymentMethod.ToString().ToLower().IndexOf("paypal") <= -1)
                    {
                    }
                    else
                    {
                        ltrAction.Text += "<a title=\"Refund\" href=\"javascript:void(0);\" onclick=\"RefundClick(" + OrderNumber.ToString() + ",'" + OrderTotal.ToString() + "','" + RefundAmount.ToString() + "');\"><img title=\"Refund\" alt=\"Refund\" src=\"/App_Themes/" + Page.Theme + "/images/RefundList.png\"></a><br />";
                    }
                }
            }
            else if (Transtatus.ToLower() == "partially refunded")
            {
                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")) && PaymentMethod.ToString().ToLower().IndexOf("paypal") <= -1)
                {
                }
                else
                {
                    ltrAction.Text += "<a title=\"Refund\" href=\"javascript:void(0);\" onclick=\"RefundClick(" + OrderNumber.ToString() + ",'" + OrderTotal.ToString() + "','" + RefundAmount.ToString() + "');\"><img title=\"Refund\" alt=\"Refund\" src=\"/App_Themes/" + Page.Theme + "/images/RefundList.png\"></a><br />";
                }
                ltrStatus.Text += "<strong style=\"color:#00AAFF;font-size:10px;\"><br />" + Transtatus.ToString() + "</strong>";
                ltrStatus.Text += "<br /><span style=\"color:#000000;\">" + statusName.ToString() + "</span>";
            }
            else if (Transtatus.ToLower() == "canceled")
            {
                ltrStatus.Text += "<strong style=\"color:#FF0000;font-size:10px;\"><br />" + Transtatus.ToString() + "</strong>";
                ltrStatus.Text += "<br /><span style=\"color:#000000;\">" + statusName.ToString() + "</span>";
            }
            else if (Transtatus.ToLower() == "refunded")
            {
                ltrStatus.Text += "<strong style=\"color:#00AAFF;font-size:10px;\"><br />" + Transtatus.ToString() + "</strong>";
                ltrStatus.Text += "<br /><span style=\"color:#000000;\">" + statusName.ToString() + "</span>";
            }
            else
            {
                ltrStatus.Text += "<strong style=\"color:#000000;font-size:10px;\"><br />" + Transtatus.ToString() + "</strong>";
                ltrStatus.Text += "<br /><span style=\"color:#000000;\">" + statusName.ToString() + "</span>";
            }

        }

        /// <summary>
        /// Get Order Wise Product
        /// </summary>
        /// <param name="cartId">int cartId</param>
        /// <param name="ltrProduct">Literal ltrProduct</param>
        private void GetProduct(Int32 cartId, Literal ltrProduct, Literal ltrStore)
        {
            DataSet dsProduct = new DataSet();
            OrderComponent objOrder = new OrderComponent();
            dsProduct = objOrder.GetProductList(cartId);
            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsProduct.Tables[0].Rows.Count; i++)
                {
                    string[] variantName = dsProduct.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] variantValue = dsProduct.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string strVariantname = "";
                    for (int j = 0; j < variantValue.Length; j++)
                    {
                        if (variantName.Length > j)
                        {
                            strVariantname += variantName[j].ToString().Replace("Estimated Delivery", "Estimated Ship Date") + " : " + variantValue[j].ToString() + "<br />";
                        }
                    }

                    if (ltrStore != null && ltrStore.Text.ToString().ToLower().IndexOf("overstock") > -1)
                    {
                        DataSet dsTemp = new DataSet();
                        dsTemp = CommonComponent.GetCommonDataSet("select isnull(Inventory,0) as Inventory,isnull(sku,'') as sku,isnull(upc,'') as UPC,storeid from tb_product where productid=" + dsProduct.Tables[0].Rows[i]["RefProductID"].ToString() + "");
                        if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                        {
                            String Inv = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + dsTemp.Tables[0].Rows[0]["UPC"].ToString() + "','" + dsTemp.Tables[0].Rows[0]["sku"].ToString() + "'," + dsTemp.Tables[0].Rows[0]["storeid"].ToString() + ");"));
                            if (strVariantname != "")
                            {
                                ltrProduct.Text += "<strong>" + dsProduct.Tables[0].Rows[i]["ProductName"] + "</strong><br />" + strVariantname + " QTY: " + dsProduct.Tables[0].Rows[i]["Quantity"] + "<span style=\"color:#2A7FFF;font-weight:bold;\"> [Available Qty : " + dsTemp.Tables[0].Rows[0]["Inventory"].ToString() + "][Sales Channel Qty : " + Inv.ToString() + "]</span><br />SKU: " + dsProduct.Tables[0].Rows[i]["SKu"] + "<br />";
                            }
                            else
                            {
                                ltrProduct.Text += "<strong>" + dsProduct.Tables[0].Rows[i]["ProductName"] + "</strong><br />QTY: " + dsProduct.Tables[0].Rows[i]["Quantity"] + " <span style=\"color:#2A7FFF;font-weight:bold;\">[Available Qty : " + dsTemp.Tables[0].Rows[0]["Inventory"].ToString() + "][Sales Channel Qty : " + Inv.ToString() + "]</span> <br />SKU: " + dsProduct.Tables[0].Rows[i]["SKu"] + "<br />";
                            }
                        }
                        else
                        {
                            if (strVariantname != "")
                            {
                                ltrProduct.Text += "<strong>" + dsProduct.Tables[0].Rows[i]["ProductName"] + "</strong><br />" + strVariantname + " QTY: " + dsProduct.Tables[0].Rows[i]["Quantity"] + "<br />SKU: " + dsProduct.Tables[0].Rows[i]["SKu"] + "<br />";
                            }
                            else
                            {
                                ltrProduct.Text += "<strong>" + dsProduct.Tables[0].Rows[i]["ProductName"] + "</strong><br />QTY: " + dsProduct.Tables[0].Rows[i]["Quantity"] + "<br />SKU: " + dsProduct.Tables[0].Rows[i]["SKu"] + "<br />";
                            }
                        }




                    }
                    else
                    {
                        if (strVariantname != "")
                        {
                            ltrProduct.Text += "<strong>" + dsProduct.Tables[0].Rows[i]["ProductName"] + "</strong><br />" + strVariantname + " QTY: " + dsProduct.Tables[0].Rows[i]["Quantity"] + "<br />SKU: " + dsProduct.Tables[0].Rows[i]["SKu"] + "<br />";
                        }
                        else
                        {
                            ltrProduct.Text += "<strong>" + dsProduct.Tables[0].Rows[i]["ProductName"] + "</strong><br />QTY: " + dsProduct.Tables[0].Rows[i]["Quantity"] + "<br />SKU: " + dsProduct.Tables[0].Rows[i]["SKu"] + "<br />";
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Get Store List
        /// </summary>
        /// <param name="ddlStore">DropDownlist ddlStore</param>
        private void GetStoreList(DropDownList ddlStore)
        {
            ddlStore.Items.Clear();
            DataSet dsStore = new DataSet();
            dsStore = StoreComponent.GetStoreListByMenu();
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = dsStore;
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataTextField = "StoreName";

                ddlStore.DataBind();
            }
            else
            {
                ddlStore.DataSource = null;
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All", "0"));


            ddlStore.SelectedValue = Convert.ToString(selectedStore);
            selectedStore = 0;


        }

        /// <summary>
        /// Order List Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grvOrderlist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddlStoregeneral.SelectedValue == "1" || ddlStoregeneral.SelectedValue == "3" || ddlStoregeneral.SelectedValue == "4" || ddlStoregeneral.SelectedValue == "11" || ddlStoregeneral.SelectedValue == "13")
                {
                    e.Row.Cells[0].Visible = true;
                }
                else
                {
                    e.Row.Cells[0].Visible = false;
                }

                Literal ltr1 = (Literal)e.Row.FindControl("ltr1");
                Literal ltr2 = (Literal)e.Row.FindControl("ltr2");
                Literal ltr3 = (Literal)e.Row.FindControl("ltr3");
                Literal ltr4 = (Literal)e.Row.FindControl("ltr4");
                Literal ltr5 = (Literal)e.Row.FindControl("ltr5");
                Literal ltr6 = (Literal)e.Row.FindControl("ltr6");
                Literal ltr7 = (Literal)e.Row.FindControl("ltr7");

                Button btnResetfilter = (Button)e.Row.FindControl("btnResetfilter");
                Button btnSearch = (Button)e.Row.FindControl("btnSearch");
                Button btnSearchDate = (Button)e.Row.FindControl("btnSearchDate");
                Button btnSearchOrdNumber = (Button)e.Row.FindControl("btnSearchOrdNumber");
                Button btnSearchOrdtotal = (Button)e.Row.FindControl("btnSearchOrdtotal");
                Button btnSearchStore = (Button)e.Row.FindControl("btnSearchStore");
                Button btnSearchCust = (Button)e.Row.FindControl("btnSearchCust");
                Button btnSearchOrdStatus = (Button)e.Row.FindControl("btnSearchOrdStatus");

                Int32 OrderNumber = Convert.ToInt32(ltr2.Text.Trim());
                System.Web.UI.HtmlControls.HtmlInputHidden hdnSubtotal = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnSubtotal");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnTotal");
                System.Web.UI.HtmlControls.HtmlInputHidden HdnShippingCost = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("HdnShippingCost");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnordertax = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnordertax");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnDiscount = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnDiscount");

                System.Web.UI.HtmlControls.HtmlInputHidden hdnRefund = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnRefund");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnAdjAmt = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnAdjAmt");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnOrderTotalNew = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnOrderTotalNew");
                ////Footer
                System.Web.UI.HtmlControls.HtmlInputHidden hdnSubtotalF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnSubtotalF");
                System.Web.UI.HtmlControls.HtmlInputHidden HdnShippingCostF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("HdnShippingCostF");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnordertaxF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnordertaxF");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnDiscountF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnDiscountF");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnlvelDiscountF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnlvelDiscountF");
                System.Web.UI.HtmlControls.HtmlInputHidden hdncoponDiscountF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdncoponDiscountF");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnGiftCardDiscount = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnGiftCardDiscount");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnRefundF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnRefundF");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnAdjAmtF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnAdjAmtF");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnQtyDiscountAmountF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnQtyDiscountAmountF");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnCaptureTXResult = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnCaptureTXResult");
                System.Web.UI.HtmlControls.HtmlInputHidden hdncustomername = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdncustomername");
                System.Web.UI.HtmlControls.HtmlTable chkprintall = (System.Web.UI.HtmlControls.HtmlTable)e.Row.FindControl("chkprintall");
                CheckBox chkprintsalesorder = (CheckBox)e.Row.FindControl("chkprintsalesorder");
                if (ltr3 != null && ltr3.Text.ToString().ToLower().IndexOf("overstock") > -1)
                {
                    ltr4.Text = ltr4.Text.ToString().Replace(hdncustomername.Value.ToString(), "OverStock");
                }
                if (e.Row.RowIndex != 0)
                {
                    hdnSubtotal1 = Convert.ToDecimal(hdnSubtotal.Value);
                    hdnTotal1 = Convert.ToDecimal(hdnTotal.Value);
                    HdnShippingCost1 = Convert.ToDecimal(HdnShippingCost.Value);
                    hdnordertax1 = Convert.ToDecimal(hdnordertax.Value);
                    hdnDiscount1 = Convert.ToDecimal(hdnDiscount.Value);
                    hdnRefund1 = Convert.ToDecimal(hdnRefund.Value);
                    hdnAdjAmt1 = Convert.ToDecimal(hdnAdjAmt.Value);
                }

                System.Web.UI.HtmlControls.HtmlTable tblDate = (System.Web.UI.HtmlControls.HtmlTable)e.Row.FindControl("tblDate");
                System.Web.UI.HtmlControls.HtmlTable tblordertotal = (System.Web.UI.HtmlControls.HtmlTable)e.Row.FindControl("tblordertotal");
                System.Web.UI.HtmlControls.HtmlTable tblordernodetails = (System.Web.UI.HtmlControls.HtmlTable)e.Row.FindControl("tblordernodetails");
                System.Web.UI.HtmlControls.HtmlTable chkUploadvalue = (System.Web.UI.HtmlControls.HtmlTable)e.Row.FindControl("chkUploadvalue");

                TextBox txtOrderNo = (TextBox)e.Row.FindControl("txtOrderNo");
                TextBox txtCustomername = (TextBox)e.Row.FindControl("txtCustomername");

                TextBox txtOrderFrom = (TextBox)e.Row.FindControl("txtOrderFrom");
                TextBox txtOrderTo = (TextBox)e.Row.FindControl("txtOrderTo");

                TextBox txtOrderTotalFrom = (TextBox)e.Row.FindControl("txtOrderTotalFrom");
                TextBox txtOrderTotalTo = (TextBox)e.Row.FindControl("txtOrderTotalTo");

                DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
                DropDownList ddlStore = (DropDownList)e.Row.FindControl("ddlStore");

                System.Web.UI.HtmlControls.HtmlInputHidden hdnshoppingcartid = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnshoppingcartid");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnorderStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnorderStatus");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnIsPhoneOrder = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnIsPhoneOrder");

                Label lblIsBackEnd = (Label)e.Row.FindControl("lblIsBackEnd");
                Label lblOrderNumber = (Label)e.Row.FindControl("lblOrderNumber");
                Label lblBackEndGUID = (Label)e.Row.FindControl("lblBackEndGUID");
                Label lblUploadMsg = (Label)e.Row.FindControl("lblUploadMsg");
                CheckBox chkUploadOrderStock = (CheckBox)e.Row.FindControl("chkUploadOrderStock");
                CheckBox chkgrndshipp = (CheckBox)e.Row.FindControl("chkgrndshipp");
                DropDownList ddlstockstat = (DropDownList)e.Row.FindControl("ddlstockstat");
                Label lblpaymentmethod = (Label)e.Row.FindControl("lblpaymentmethod");

                //nav
                Label lblnavstatus = (Label)e.Row.FindControl("lblnavstatus");
                Label lblnavcompleted = (Label)e.Row.FindControl("lblnavcompleted");
                Label lblisnaverror = (Label)e.Row.FindControl("lblisnaverror");
                Image imgnaverror = (Image)e.Row.FindControl("imgnaverror");
                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                {
                    if (Convert.ToInt32(lblisnaverror.Text.ToString()) == 1)
                    {
                        imgnaverror.Visible = true;
                    }
                    else
                    {
                        imgnaverror.Visible = false;
                    }
                }
                else
                {
                    imgnaverror.Visible = false;
                }

                //ImageButton imgvaceluplod = (ImageButton)e.Row.FindControl("imgvaceluplod");

                System.Web.UI.HtmlControls.HtmlInputHidden hdnIsMailSent = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnIsMailSent");
                Label lblIsMailSent = (Label)e.Row.FindControl("lblIsMailSent");


                if (hdnIsMailSent != null && hdnIsMailSent.Value.ToString() == "1" || hdnIsMailSent.Value.ToString().ToLower() == "true")
                {
                    if (lblIsMailSent != null)
                    {
                        lblIsMailSent.Text = "<br />Shipment Mail Sent";
                    }
                }
                else
                {
                    if (lblIsMailSent != null)
                    {
                        lblIsMailSent.Text = "";
                    }
                }

                if (ViewState["ddlStockStore"] != null)
                {
                    ddlstockstat.SelectedValue = ViewState["ddlStockStore"].ToString();
                }

                if (e.Row.RowIndex == 0)
                {
                    lblUploadMsg.Text = "";
                    if (ViewState["DateFrom"] != null)
                    {
                        txtOrderFrom.Text = ViewState["DateFrom"].ToString();
                    }

                    if (ViewState["DateTo"] != null)
                    {
                        txtOrderTo.Text = ViewState["DateTo"].ToString();
                    }
                    if (ViewState["OrderNumber"] != null)
                    {
                        txtOrderNo.Text = ViewState["OrderNumber"].ToString();
                    }
                    if (ViewState["Customername"] != null)
                    {
                        txtCustomername.Text = ViewState["Customername"].ToString();
                    }
                    if (ViewState["ddlStatus"] != null)
                    {
                        ddlStatus.SelectedValue = ViewState["ddlStatus"].ToString();
                    }
                    if (ViewState["orderTotalFrom"] != null)
                    {
                        txtOrderTotalFrom.Text = ViewState["orderTotalFrom"].ToString();
                    }
                    if (ViewState["orderTotalTo"] != null)
                    {
                        txtOrderTotalTo.Text = ViewState["orderTotalTo"].ToString();
                    }
                    GetStoreList(ddlStore);
                    try
                    {
                        ddlStore.SelectedValue = ddlStoregeneral.SelectedValue;
                    }
                    catch { }
                    if (ViewState["ddlStore"] != null)
                    {
                        ddlStore.SelectedValue = ViewState["ddlStore"].ToString();
                    }
                    if (ViewState["chkgrndshipp"] != null)
                    {
                        chkgrndshipp.Checked = Convert.ToBoolean(ViewState["chkgrndshipp"]);
                    }
                    ltr1.Visible = false;
                    tblordernodetails.Visible = false;
                    chkUploadvalue.Visible = true;
                    ltr3.Visible = false;
                    ltr4.Visible = false;
                    ltr5.Visible = false;
                    ltr6.Visible = false;
                    ltr7.Visible = false;
                    lblOrderNumber.Text = "";
                    chkUploadOrderStock.Visible = false;
                    //btnSearch.Visible = true;
                    btnSearchOrdNumber.Visible = true;
                    btnSearchOrdtotal.Visible = true;
                    btnSearchStore.Visible = true;
                    btnSearchCust.Visible = true;
                    btnSearchOrdStatus.Visible = true;
                    btnSearchDate.Visible = true;
                    btnResetfilter.Visible = true;
                    tblDate.Visible = true;
                    tblordertotal.Visible = true;

                    btnResetfilter.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/reser-filter.gif) no-repeat transparent; width: 87px; height: 23px; border:none;cursor:pointer;");
                    btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search_icon.png) no-repeat transparent; width: 26px; height: 26px; border:none;cursor:pointer;");
                    btnSearchDate.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search_icon.png) no-repeat transparent; width: 26px; height: 26px; border:none;cursor:pointer;");
                    btnSearchOrdNumber.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search_icon.png) no-repeat transparent; width: 26px; height: 26px; border:none;cursor:pointer;");
                    btnSearchOrdtotal.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search_icon.png) no-repeat transparent; width: 26px; height: 26px; border:none;cursor:pointer;");
                    btnSearchStore.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search_icon.png) no-repeat transparent; width: 26px; height: 26px; border:none;cursor:pointer;");
                    btnSearchCust.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search_icon.png) no-repeat transparent; width: 26px; height: 26px; border:none;cursor:pointer;");
                    btnSearchOrdStatus.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search_icon.png) no-repeat transparent; width: 26px; height: 26px; border:none;cursor:pointer;");

                    ddlStatus.Visible = true;
                    ddlStore.Visible = true;

                    txtOrderNo.Visible = true;
                    txtCustomername.Visible = true;
                    chkgrndshipp.Visible = true;
                    chkprintsalesorder.Visible = false;
                    e.Row.Attributes.Add("class", "altrow");
                }
                else
                {
                    chkprintall.Visible = false;
                    if (ddlStoregeneral.SelectedValue == "4" || ddlStoregeneral.SelectedValue == "1" || ddlStoregeneral.SelectedValue == "3" || ddlStoregeneral.SelectedValue == "11" || ddlStoregeneral.SelectedValue == "13")
                    {

                        if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                        {
                            if (lblnavstatus.Text.ToString() == "1" && lblnavcompleted.Text.ToString() == "0")
                            {
                                lblUploadMsg.Text = "Pending";

                                chkUploadOrderStock.Visible = true;
                                checknavclear = 1;
                                //    btnUploadOrder.Visible = true;
                            }
                            else if (lblnavcompleted.Text.ToString() == "1")
                            {
                                lblUploadMsg.Text = "Ready to Import";
                                lblUploadMsg.Attributes.Add("style", "color: #FF7F00;font-weight: bold;");
                            }
                            else if (lblIsBackEnd.Text.ToString() == "1" && lblnavcompleted.Text.ToString() == "2")
                            {
                                lblUploadMsg.Attributes.Add("style", "color: #348934;font-weight: bold;");
                                lblUploadMsg.Text = "Imported";
                            }
                        }
                        else
                        {
                            if (lblIsBackEnd.Text.ToString() == "1" && string.IsNullOrEmpty(lblBackEndGUID.Text.ToString().Trim()))
                            {
                                lblUploadMsg.Text = "Pending";
                                chkUploadOrderStock.Visible = true;
                                checknavclear = 1;
                                // btnUploadOrder.Visible = true;
                            }
                            else if (lblIsBackEnd.Text.ToString() == "0" && string.IsNullOrEmpty(lblBackEndGUID.Text.ToString().Trim()))
                            {
                                lblUploadMsg.Text = "Ready to Import";
                                //  imgvaceluplod.Visible = true;
                            }
                            else if (lblIsBackEnd.Text.ToString() == "1" && !string.IsNullOrEmpty(lblBackEndGUID.Text.ToString().Trim()))
                            {
                                lblUploadMsg.Attributes.Add("style", "color: #348934;font-weight: bold;");
                                lblUploadMsg.Text = "Imported";
                            }

                        }


                    }

                    if (Convert.ToString(ltr6.Text.ToString()).ToLower() == "canceled")
                    {
                        hdncanceledOrder += Convert.ToDecimal(hdnOrderTotalNew.Value.ToString()) + Convert.ToDecimal(hdnAdjAmtF.Value);
                        hdnSubtotal1F += Convert.ToDecimal(hdnSubtotalF.Value);
                        HdnShippingCost1F += Convert.ToDecimal(HdnShippingCostF.Value);
                        hdnordertax1F += Convert.ToDecimal(hdnordertaxF.Value);
                        hdnDiscount1F += Convert.ToDecimal(hdnDiscountF.Value) + Convert.ToDecimal(hdnlvelDiscountF.Value) + Convert.ToDecimal(hdncoponDiscountF.Value) + Convert.ToDecimal(hdnQtyDiscountAmountF.Value.ToString());
                        hdnRefund1F += Convert.ToDecimal(hdnRefundF.Value);
                        hdnAdjAmt1F += Convert.ToDecimal(hdnAdjAmtF.Value);

                    }

                    if (Convert.ToString(ltr6.Text.ToString()).ToLower().Trim() == "canceled" || hdnorderStatus.Value.ToString().ToLower().Trim() == "canceled")
                    {
                        chkUploadOrderStock.Visible = false;
                        lblUploadMsg.Text = "";
                    }
                    //if (Convert.ToString(ltr6.Text.ToString()).ToLower() == "voided")
                    //{
                    //   // hdnvoidOrder += Convert.ToDecimal(hdnOrderTotalNew.Value.ToString());

                    //    hdnSubtotal1F += Convert.ToDecimal(hdnSubtotalF.Value);
                    //    HdnShippingCost1F += Convert.ToDecimal(HdnShippingCostF.Value);
                    //    hdnordertax1F += Convert.ToDecimal(hdnordertaxF.Value);
                    //    hdnDiscount1F += Convert.ToDecimal(hdnDiscountF.Value) + Convert.ToDecimal(hdnlvelDiscountF.Value) + Convert.ToDecimal(hdncoponDiscountF.Value);
                    //    hdnRefund1F += Convert.ToDecimal(hdnRefundF.Value);
                    //    hdnAdjAmt1F += Convert.ToDecimal(hdnAdjAmtF.Value);
                    //}

                    HtmlInputHidden hdnAddress1 = (HtmlInputHidden)e.Row.FindControl("hdnAddress1");
                    HtmlInputHidden hdnAddress2 = (HtmlInputHidden)e.Row.FindControl("hdnAddress2");
                    HtmlInputHidden hdnSuite = (HtmlInputHidden)e.Row.FindControl("hdnSuite");
                    HtmlInputHidden hdnCity = (HtmlInputHidden)e.Row.FindControl("hdnCity");
                    HtmlInputHidden hdnState = (HtmlInputHidden)e.Row.FindControl("hdnState");
                    HtmlInputHidden hdnPhone = (HtmlInputHidden)e.Row.FindControl("hdnPhone");
                    HtmlInputHidden hdnCountry = (HtmlInputHidden)e.Row.FindControl("hdnCountry");
                    HtmlInputHidden hdnZip = (HtmlInputHidden)e.Row.FindControl("hdnZip");
                    HtmlInputHidden hdnCompany = (HtmlInputHidden)e.Row.FindControl("hdnCompany");
                    HtmlInputHidden hdnTrackinguplaoded = (HtmlInputHidden)e.Row.FindControl("hdnTrackinguplaoded");
                    HtmlInputHidden hdnreforder = (HtmlInputHidden)e.Row.FindControl("hdnreforder");
                    HtmlInputHidden hdnShippingMethod = (HtmlInputHidden)e.Row.FindControl("hdnShippingMethod");
                    HtmlImage imgorderalert = (HtmlImage)e.Row.FindControl("imgorderalert");
                    ltr4.Text += "<br />";
                    if (!string.IsNullOrEmpty(hdnCompany.Value.ToString()))
                    {
                        ltr4.Text += hdnCompany.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnAddress1.Value.ToString()))
                    {
                        ltr4.Text += hdnAddress1.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnAddress2.Value.ToString()))
                    {
                        ltr4.Text += hdnAddress2.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnSuite.Value.ToString()))
                    {
                        ltr4.Text += hdnSuite.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnCity.Value.ToString()))
                    {
                        ltr4.Text += hdnCity.Value.ToString() + ", " + hdnState.Value.ToString() + " " + hdnZip.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnCountry.Value.ToString()))
                    {
                        ltr4.Text += hdnCountry.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnPhone.Value.ToString()))
                    {
                        ltr4.Text += hdnPhone.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnShippingMethod.Value.ToString()))
                    {
                        ltr4.Text += "<span style='color:#436DA0;font-weight:bold;font-size:11px;'>" + hdnShippingMethod.Value.ToString() + "</span><br />";

                        if (hdnShippingMethod.Value.ToString().ToLower().Trim() != "ground" && hdnShippingMethod.Value.ToString().ToLower().Trim() != "grnd" && hdnShippingMethod.Value.ToString().ToLower().Trim() != "standard")
                        {
                            imgorderalert.Visible = true;
                        }
                    }
                    Decimal hdnRefundTotal = Convert.ToDecimal(hdnOrderTotalNew.Value.ToString()) + Convert.ToDecimal(hdnAdjAmtF.Value);
                    SetStatus(hdnorderStatus.Value.ToString(), ltr6.Text.ToString(), ltr6, ltr7, OrderNumber, hdnRefundTotal.ToString(), hdnRefundF.Value.ToString(), hdnCaptureTXResult.Value.ToString(), lblpaymentmethod.Text.ToString());
                    ltr2.Text = "<a class=\"order-no\" href=\"/Admin/Orders/Orders.aspx?id=" + ltr2.Text.ToString() + "\">" + ltr2.Text.ToString() + "</a>";
                    if (hdnTrackinguplaoded != null && (hdnTrackinguplaoded.Value.ToString() == "1" || hdnTrackinguplaoded.Value.ToString().ToLower() == "true"))
                    {
                        ltr6.Text += "<br /><span style=\"color:#000000;\">Uploaded To <br />Partner Portal</span>";
                    }

                    string SalesAgentName = "";
                    if (hdnIsPhoneOrder.Value.ToString() == "1")
                    {
                        SalesAgentName = Convert.ToString(CommonComponent.GetScalarCommonData("SElect ISNULL(FirstName,'') +' '+ ISNULL(LastName,'') as SalesAgentName  from tb_Admin where ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 " +
                                                                " and  adminid in (Select ISNULL(SalesAgentID,0) as SId from tb_Order where ISNULL(IsPhoneOrder,0)=1 and OrderNumber=" + OrderNumber + ")"));
                    }
                    if (hdnreforder.Value.ToString() != "")
                    {
                        ltr2.Text += "&nbsp;&nbsp;<span style=\"color:#436DA0;\">(Ref. Order #: " + hdnreforder.Value.ToString() + ")</span><br />";

                        if (!string.IsNullOrEmpty(SalesAgentName.ToString()))
                        {
                            ltr2.Text += "&nbsp;<span style=\"color:#436DA0;\">(Phone Order by " + SalesAgentName.ToString() + ")</span><br />";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(SalesAgentName.ToString()))
                        {
                            ltr2.Text += "&nbsp;<span style=\"color:#436DA0;\">(Phone Order by " + SalesAgentName.ToString() + ")</span><br />";
                        }
                        else
                        {
                            ltr2.Text += "<br />";
                        }
                    }
                    GetProduct(Convert.ToInt32(hdnshoppingcartid.Value), ltr2, ltr3);
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                btnUploadOrder.Visible = false;
                if (ddlStoregeneral.SelectedValue == "4" || ddlStoregeneral.SelectedValue == "1" || ddlStoregeneral.SelectedValue == "3" || ddlStoregeneral.SelectedValue == "11" || ddlStoregeneral.SelectedValue == "13")
                {
                    grvOrderlist.Columns[0].Visible = true;
                }
                else
                {
                    grvOrderlist.Columns[0].Visible = false;
                }
                hdncanceledOrder = 0;
                hdnvoidOrder = 0;
                hdnSubtotal1F = 0;
                HdnShippingCost1F = 0;
                hdnordertax1F = 0;
                hdnDiscount1F = 0;
                hdnRefund1F = 0;
                hdnAdjAmt1F = 0;
                if (isAscend == false)
                {
                    ImageButton blImage = (ImageButton)e.Row.FindControl("blImage");
                    if (ViewState["sortExpression"] == null || ViewState["sortExpression"].ToString().ToUpper() == "DESC")
                    {
                        blImage.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                        blImage.AlternateText = "Descending Order";
                        blImage.ToolTip = "Descending Order";
                        isAscend = true;
                        blImage.CommandArgument = "DESC";
                    }
                    else if (ViewState["sortExpression"] != null || ViewState["sortExpression"].ToString().ToUpper() == "ASC")
                    {
                        blImage.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                        blImage.AlternateText = "Ascending Order";
                        blImage.ToolTip = "Ascending Order";
                        isAscend = false;
                        blImage.CommandArgument = "ASC";

                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Literal ltrSummary = (Literal)e.Row.FindControl("ltrSummary");
                Literal ltrtotalName = (Literal)e.Row.FindControl("ltrtotalName");

                System.Web.UI.HtmlControls.HtmlTable chkUploadvalue = (System.Web.UI.HtmlControls.HtmlTable)grvOrderlist.Rows[0].FindControl("chkUploadvalue");
                DropDownList ddlstockstat = (DropDownList)grvOrderlist.Rows[0].FindControl("ddlstockstat");
                if (ddlStoregeneral.SelectedValue != "4" && ddlStoregeneral.SelectedValue != "1" && ddlStoregeneral.SelectedValue != "3" && ddlStoregeneral.SelectedValue != "11" && ddlStoregeneral.SelectedValue != "13")
                {
                    btnUploadOrder.Visible = false;
                    ddlstockstat.Visible = false;
                }
                else
                {
                    ddlstockstat.Visible = true;
                }
                if (checknavclear == 1)
                {
                    chkUploadvalue.Visible = true;
                }
                else
                {
                    chkUploadvalue.Visible = false;
                }

                //if (btnUploadOrder.Visible == true)
                //{
                //    chkUploadvalue.Visible = true;
                //}
                //else
                //{
                //    chkUploadvalue.Visible = false;
                //}


                //if (btnnavuplod.Visible == true)
                //{
                //    chkUploadvalue.Visible = true;
                //}
                //else
                //{
                //    chkUploadvalue.Visible = false;
                //}

                if (ltrSummary != null)
                {
                    if (grvOrderlist.Rows.Count > 1)
                    {
                        e.Row.Cells[0].Attributes.Add("Style", "border-right:none;border-top:1px solid #DFDFDF;");
                        e.Row.Cells[1].Attributes.Add("Style", "border-right:none;border-top:1px solid #DFDFDF;");
                        e.Row.Cells[2].Attributes.Add("Style", "border-right:none;border-top:1px solid #DFDFDF;");
                        e.Row.Cells[3].Attributes.Add("Style", "border-right:none;border-top:1px solid #DFDFDF;");
                        e.Row.Cells[4].Attributes.Add("Style", "border-right:none;border-top:1px solid #DFDFDF;");
                        e.Row.Cells[5].Attributes.Add("Style", "border-right:none;border-top:1px solid #DFDFDF;");
                        e.Row.Cells[6].Attributes.Add("Style", "border-right:none;border-top:1px solid #DFDFDF;");
                        ltrSummary.Text = Convert.ToDecimal(hdnSubtotal1 - hdnSubtotal1F).ToString("C") + "<br />";
                        ltrSummary.Text += Convert.ToDecimal(hdnordertax1 - hdnordertax1F).ToString("C") + "<br />";
                        ltrSummary.Text += Convert.ToDecimal(HdnShippingCost1 - HdnShippingCost1F).ToString("C") + "<br />";
                        ltrSummary.Text += Convert.ToDecimal(hdnDiscount1 - hdnDiscount1F).ToString("C") + "<br />";
                        ltrSummary.Text += Convert.ToDecimal(hdnRefund1 - hdnRefund1F).ToString("C") + "<br />";
                        ltrSummary.Text += Convert.ToDecimal(hdnAdjAmt1 - hdnAdjAmt1F).ToString("C").Replace("(", "-").Replace(")", "") + "<br />";
                        ltrSummary.Text += Convert.ToDecimal(hdncanceledOrder).ToString("C").Replace("(", "-").Replace(")", "") + "<br />";
                        //ltrSummary.Text += Convert.ToDecimal(hdnvoidOrder).ToString("C").Replace("(", "-").Replace(")", "") + "<br />";
                        hdnTotal1 = (hdnTotal1 + hdnAdjAmt1) - hdnRefund1 - hdncanceledOrder;
                        ltrSummary.Text += Convert.ToDecimal(hdnTotal1).ToString("C").Replace("(", "-").Replace(")", "");

                        ltrtotalName.Text = "Sub Total : <br />";
                        ltrtotalName.Text += "Order Tax : <br />";
                        ltrtotalName.Text += "Shipping : <br />";
                        ltrtotalName.Text += "Discount : <br />";
                        ltrtotalName.Text += "Refunded : <br />";
                        ltrtotalName.Text += "Adjustment Amount : <br />";
                        ltrtotalName.Text += "Canceled : <br />";
                        //ltrtotalName.Text += "Voided : <br />";
                        ltrtotalName.Text += "Total : ";
                    }
                }
            }
        }

        /// <summary>
        ///  Reset Filter Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        public void btnResetfilter_Click(object sender, EventArgs e)
        {
            ViewState["SearchFilter"] = " AND dbo.tb_Order.orderStatus='New'";
            ViewState["DateFrom"] = null;
            ViewState["DateTo"] = null;
            ViewState["OrderNumber"] = null;
            ViewState["Customername"] = null;
            ViewState["ddlStatus"] = null;
            ViewState["orderTotalFrom"] = null;
            ViewState["orderTotalTo"] = null;
            ViewState["ddlStore"] = null;
            ViewState["ddlStockStore"] = null;
            ViewState["chkgrndshipp"] = false;
            isAscend = false;
            GetOrderListByStoreId(1, pageSize);
        }

        /// <summary>
        /// Order List Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grvOrderlist_RowEditing(object sender, GridViewEditEventArgs e)
        {
            bool flag = false;
            ViewState["DateFrom"] = null;
            ViewState["DateTo"] = null;
            ViewState["OrderNumber"] = null;
            ViewState["Customername"] = null;
            ViewState["ddlStatus"] = null;
            ViewState["orderTotalFrom"] = null;
            ViewState["orderTotalTo"] = null;
            ViewState["ddlStore"] = null;
            ViewState["ddlStockStore"] = null;
            string strSearch = "";
            TextBox txtOrderFrom = (TextBox)grvOrderlist.Rows[e.NewEditIndex].FindControl("txtOrderFrom");
            TextBox txtOrderTo = (TextBox)grvOrderlist.Rows[e.NewEditIndex].FindControl("txtOrderTo");
            TextBox txtOrderNo = (TextBox)grvOrderlist.Rows[e.NewEditIndex].FindControl("txtOrderNo");
            DropDownList ddlStore = (DropDownList)grvOrderlist.Rows[e.NewEditIndex].FindControl("ddlStore");
            TextBox txtCustomername = (TextBox)grvOrderlist.Rows[e.NewEditIndex].FindControl("txtCustomername");
            TextBox txtOrderTotalFrom = (TextBox)grvOrderlist.Rows[e.NewEditIndex].FindControl("txtOrderTotalFrom");
            TextBox txtOrderTotalTo = (TextBox)grvOrderlist.Rows[e.NewEditIndex].FindControl("txtOrderTotalTo");
            DropDownList ddlStatus = (DropDownList)grvOrderlist.Rows[e.NewEditIndex].FindControl("ddlStatus");
            DropDownList ddlstockstat = (DropDownList)grvOrderlist.Rows[e.NewEditIndex].FindControl("ddlstockstat");
            CheckBox chkgrndshipp = (CheckBox)grvOrderlist.Rows[e.NewEditIndex].FindControl("chkgrndshipp");
            if (txtOrderFrom.Text.ToString() != "" && txtOrderTo.Text.ToString() != "")
            {
                if (Convert.ToDateTime(txtOrderTo.Text.ToString()) >= Convert.ToDateTime(txtOrderFrom.Text.ToString()))
                {

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select valid date.', 'Message');});", true);
                    return;
                }
            }
            else
            {
                if (txtOrderFrom.Text.ToString() == "" && txtOrderTo.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select valid date.', 'Message');});", true);
                    return;
                }
                else if (txtOrderTo.Text.ToString() == "" && txtOrderFrom.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select valid date.', 'Message');});", true);
                    return;
                }

            }


            if (txtOrderFrom.Text.ToString() != "")
            {
                flag = true;
                strSearch += " AND cast(orderdate as date) >= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtOrderFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
                ViewState["DateFrom"] = txtOrderFrom.Text.ToString();
            }
            if (txtOrderTo.Text.ToString() != "")
            {
                flag = true;
                strSearch += " AND cast(orderdate as date) <= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtOrderTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
                ViewState["DateTo"] = txtOrderTo.Text.ToString();
            }
            if ((ddlStore.SelectedValue.ToString() == "4" || ddlStore.SelectedValue.ToString() == "3" || ddlStore.SelectedValue.ToString() == "1" || ddlStore.SelectedValue.ToString() == "11" || ddlStore.SelectedValue.ToString() == "13") && ddlstockstat.SelectedValue.ToString() == "1")
            {
                flag = true;
                //strSearch += " AND isnull(isbackEnd,1)= 1 AND isnull(backEndGuid,'')='' ";
                strSearch += " AND isnull(isNAVInserted,0)= 1 AND isnull(isnavcompleted,0)=0 and isnull(OrderStatus,'')<>'Canceled' ";
                ViewState["ddlStockStore"] = "1";
                //ViewState["DateTo"] = txtOrderTo.Text.ToString();
            }
            else if ((ddlStore.SelectedValue.ToString() == "4" || ddlStore.SelectedValue.ToString() == "3" || ddlStore.SelectedValue.ToString() == "1" || ddlStore.SelectedValue.ToString() == "11" || ddlStore.SelectedValue.ToString() == "13") && ddlstockstat.SelectedValue.ToString() == "2")
            {
                flag = true;
                // strSearch += " AND isnull(isbackEnd,1)= 0 AND isnull(backEndGuid,'')='' ";
                strSearch += " AND isnull(isNAVInserted,0)= 0 AND isnull(isnavcompleted,0)=1 and isnull(OrderStatus,'')<>'Canceled' ";
                ViewState["ddlStockStore"] = "2";
                //ViewState["DateTo"] = txtOrderTo.Text.ToString();
            }
            else if ((ddlStore.SelectedValue.ToString() == "4" || ddlStore.SelectedValue.ToString() == "3" || ddlStore.SelectedValue.ToString() == "1" || ddlStore.SelectedValue.ToString() == "11" || ddlStore.SelectedValue.ToString() == "13") && ddlstockstat.SelectedValue.ToString() == "3")
            {
                flag = true;
                //  strSearch += " AND isnull(isbackEnd,1)= 1 AND isnull(backEndGuid,'') <> '' ";
                strSearch += " AND isnull(isNAVInserted,0)= 1 AND isnull(isnavcompleted,0)=2 and isnull(OrderStatus,'')<>'Canceled' ";
                ViewState["ddlStockStore"] = "3";
                //ViewState["DateTo"] = txtOrderTo.Text.ToString();
            }
            else
            {
                ViewState["ddlStockStore"] = "0";
            }
            if (chkgrndshipp.Checked == true)
            {
                flag = true;
                strSearch += " AND isnull(ShippingMethod,'') NOT IN  ('ground','grnd','standard') ";
                ViewState["chkgrndshipp"] = true;

            }
            else
            {
                ViewState["chkgrndshipp"] = false;
            }
            if (txtOrderNo.Text.ToString() != "")
            {
                bool OrderNum = false;
                int Num;
                OrderNum = int.TryParse(txtOrderNo.Text.ToString(), out Num);
                if (OrderNum)
                {
                    strSearch += " AND ( (tb_order.orderNumber=" + txtOrderNo.Text.ToString().Replace("'", "''") + ")  or (tb_order.RefOrderID like '%" + txtOrderNo.Text.ToString().Replace("'", "''") + "%') )";
                    ViewState["OrderNumber"] = txtOrderNo.Text.ToString();
                }
                else
                {
                    OrderComponent objOrderproduct = new OrderComponent();
                    string OrderNumbers = "";
                    //if (ddlwarehouse.SelectedIndex > 0)
                    //{

                    //    OrderNumbers = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT cast(OrderNumber as nvarchar(max))+',' FROM tb_Order WHERE ShoppingCardID in (SELECT OrderedShoppingCartID FROM tb_OrderedShoppingCartItems INNER JOIN tb_product  ON tb_OrderedShoppingCartItems.RefProductId=tb_product.ProductID LEFT OUTER JOIN tb_WareHouseProductInventory on tb_WareHouseProductInventory.ProductId=tb_product.ProductID WHERE tb_WareHouseProductInventory.WareHouseID=" + ddlwarehouse.SelectedValue.ToString() + " AND (ProductName like '%" + txtOrderNo.Text.ToString().Replace("'", "''") + "%' Or tb_OrderedShoppingCartItems.SKU like '%" + txtOrderNo.Text.ToString().Replace("'", "''") + "%' or VariantNames like '%" + txtOrderNo.Text.ToString().Replace("'", "''") + "%' or VariantValues like '%" + txtOrderNo.Text.ToString().Replace("'", "''") + "%')) FOR XML PATH('')"));
                    //    //OrderNumbers = objOrderproduct.GetOrderByProductDetails(txtOrderNo.Text.ToString().Replace("'", "''"));

                    //}
                    //else
                    //{
                    //  OrderNumbers = objOrderproduct.GetOrderByProductDetails(txtOrderNo.Text.ToString().Replace("'", "''"));
                    OrderNumbers = Convert.ToString(CommonComponent.GetScalarCommonData("Exec usp_OrderedShoppingCartItems_GetProductByProductDetailsStorewise '" + txtOrderNo.Text.ToString().Replace("'", "''") + "'," + ddlStore.SelectedValue.ToString() + ""));

                    //}
                    ViewState["OrderNumber"] = txtOrderNo.Text.ToString();
                    if (!string.IsNullOrEmpty(OrderNumbers) && OrderNumbers.Length > 1)
                    {
                        if (OrderNumbers.Substring(OrderNumbers.Length-1, 1) == ",")
                        {
                            OrderNumbers = OrderNumbers.Substring(0, OrderNumbers.Length - 1);
                        }

                        strSearch += " AND ( tb_order.orderNumber in (" + OrderNumbers + ") or (tb_order.RefOrderID like '%" + txtOrderNo.Text.ToString().Replace("'", "''") + "%') )";

                    }
                    else
                    {
                        strSearch += " AND ( (tb_order.orderNumber=0) or (tb_order.RefOrderID like '%" + txtOrderNo.Text.ToString().Replace("'", "''") + "%') )";
                    }





                }
                flag = true;

            }
            else
            {
                //string OrderNumbers = "";
                //if (ddlwarehouse.SelectedIndex > 0)
                //{

                //    OrderNumbers = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT cast(OrderNumber as nvarchar(max))+',' FROM tb_Order WHERE ShoppingCardID in (SELECT OrderedShoppingCartID FROM tb_OrderedShoppingCartItems INNER JOIN tb_product  ON tb_OrderedShoppingCartItems.RefProductId=tb_product.ProductID LEFT OUTER JOIN tb_WareHouseProductInventory on tb_WareHouseProductInventory.ProductId=tb_product.ProductID WHERE tb_WareHouseProductInventory.WareHouseID=" + ddlwarehouse.SelectedValue.ToString() + ") FOR XML PATH('')"));
                //    //OrderNumbers = objOrderproduct.GetOrderByProductDetails(txtOrderNo.Text.ToString().Replace("'", "''"));
                //    if (!string.IsNullOrEmpty(OrderNumbers) && OrderNumbers.Length > 1)
                //    {
                //        OrderNumbers = OrderNumbers.Substring(0, OrderNumbers.Length - 1);
                //        strSearch += " AND (tb_order.orderNumber in (" + OrderNumbers + "))";

                //    }
                //}
            }
            if (ddlStore.SelectedIndex >= 0)
            {
                flag = true;
                if (ddlStore.SelectedIndex > 0)
                {
                    strSearch += " AND dbo.tb_Order.StoreId=" + ddlStore.SelectedValue.ToString() + "";


                }
                ViewState["ddlStore"] = ddlStore.SelectedValue.ToString();
            }
            if (ddlStatus.SelectedIndex >= 0)
            {
                flag = true;
                if (ddlStatus.SelectedIndex > 0)
                {
                    if (ddlStatus.SelectedValue.ToString().ToLower().IndexOf("trackinguploaded") > -1)
                    {
                        strSearch += " AND isnull(dbo.tb_Order.IsBackendProcessed,0)=1";

                    }
                    else
                    {
                        strSearch += " AND dbo.tb_Order.orderStatus='" + ddlStatus.SelectedValue.ToString() + "'";
                    }

                    ViewState["ddlStatus"] = ddlStatus.SelectedValue.ToString();
                }
                else
                {
                    //   strSearch += " AND dbo.tb_Order.orderStatus='New'";
                    ViewState["ddlStatus"] = ddlStatus.SelectedValue.ToString();
                }
            }
            if (txtCustomername.Text.ToString() != "")
            {



                flag = true;

                if (txtCustomername.Text.Trim().Split((" ").ToCharArray()).Length > 1)
                {
                    try
                    {
                        string[] customername = txtCustomername.Text.Trim().Split((" ").ToCharArray());
                        strSearch += " AND (rtrim(ltrim(FirstName))+' '+rtrim(ltrim(LastName)) like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' OR rtrim(ltrim(BillingFirstName))+' '+rtrim(ltrim(BillingLastName)) like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' OR rtrim(ltrim(ShippingFirstName))+' '+rtrim(ltrim(ShippingLastName)) like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' OR rtrim(ltrim(LastName))+' '+rtrim(ltrim(FirstName)) like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' OR rtrim(ltrim(BillingLastName))+' '+rtrim(ltrim(BillingFirstName)) like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' OR rtrim(ltrim(ShippingLastName))+' '+rtrim(ltrim(ShippingFirstName)) like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' ";
                        //strSearch += " or FirstName like '%" + customername[0].Trim().ToString().Replace("'", "''") + "%' or LastName like '%" + customername[0].Trim().ToString().Replace("'", "''") + "%' Or FirstName like '%" + customername[1].Trim().ToString().Replace("'", "''") + "%' or LastName like '%" + customername[1].Trim().ToString().Replace("'", "''") + "%'";

                        //strSearch += " Or BillingFirstName like '%" + customername[0].Trim().ToString().Replace("'", "''") + "%' or BillingLastName like '%" + customername[0].ToString().Replace("'", "''") + "%'";
                        //strSearch += " Or BillingFirstName like '%" + customername[1].Trim().ToString().Replace("'", "''") + "%' or BillingLastName like '%" + customername[1].ToString().Replace("'", "''") + "%'";

                        //strSearch += " Or ShippingFirstName like '%" + customername[0].Trim().ToString().Replace("'", "''") + "%' or ShippingLastName like '%" + customername[0].ToString().Replace("'", "''") + "%'";
                        //strSearch += " Or ShippingFirstName like '%" + customername[1].Trim().ToString().Replace("'", "''") + "%' or ShippingLastName like '%" + customername[1].ToString().Replace("'", "''") + "%'";
                    }
                    catch
                    {
                        strSearch += " AND (FirstName like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' or LastName like '%" + txtCustomername.Text.ToString().Replace("'", "''") + "%'";
                        strSearch += " Or BillingFirstName like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' or BillingLastName like '%" + txtCustomername.Text.ToString().Replace("'", "''") + "%'";
                        strSearch += " Or ShippingFirstName like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' or ShippingLastName like '%" + txtCustomername.Text.ToString().Replace("'", "''") + "%'";
                    }
                }
                else
                {
                    strSearch += " AND (FirstName like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' or LastName like '%" + txtCustomername.Text.ToString().Replace("'", "''") + "%'";
                    strSearch += " Or BillingFirstName like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' or BillingLastName like '%" + txtCustomername.Text.ToString().Replace("'", "''") + "%'";
                    strSearch += " Or ShippingFirstName like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' or ShippingLastName like '%" + txtCustomername.Text.ToString().Replace("'", "''") + "%'";
                }

                strSearch += " Or BillingZip like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%' ";
                strSearch += " Or ShippingZip like '%" + txtCustomername.Text.Trim().ToString().Replace("'", "''") + "%') ";
                ViewState["Customername"] = txtCustomername.Text.ToString();

            }
            if (txtOrderTotalFrom.Text.ToString() != "")
            {
                flag = true;
                strSearch += " AND round((isnull(orderTotal,0) + isnull(AdjustmentAmount,0)) -isnull(RefundedAmount,0),2)  >=" + string.Format("{0:0.00}", Convert.ToDecimal(txtOrderTotalFrom.Text.ToString())) + "";
                ViewState["orderTotalFrom"] = txtOrderTotalFrom.Text.ToString();
            }
            if (txtOrderTotalTo.Text.ToString() != "")
            {
                flag = true;
                strSearch += " AND round((isnull(orderTotal,0) + isnull(AdjustmentAmount,0)) -isnull(RefundedAmount,0),2) <= " + string.Format("{0:0.00}", Convert.ToDecimal(txtOrderTotalTo.Text.ToString())) + "";
                ViewState["orderTotalTo"] = txtOrderTotalTo.Text.ToString();
            }
            if (flag == false)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Fill Up Search Criteria.', 'Message');", true);
                return;
            }

            ViewState["SearchFilter"] = strSearch.ToString();
            ddlStoregeneral.SelectedValue = ddlStore.SelectedValue;
            GetOrderListByStoreId(1, pageSize);

        }

        /// <summary>
        /// Remove Comma, Space and '\' From String
        /// </summary>
        /// <param name="sFieldValueToEscape">String sFieldValueToEscape</param>
        /// <param>return String</param>
        private string _EscapeCsvField(string sFieldValueToEscape)
        {
            sFieldValueToEscape = sFieldValueToEscape.Replace("\\r\\n", System.Environment.NewLine);
            if (sFieldValueToEscape.Contains(","))
            {
                if (sFieldValueToEscape.Contains("\""))
                {
                    return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
                }
                else
                {
                    return "\"" + sFieldValueToEscape + "\"";
                }
            }
            else
            {
                return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
            }
        }

        /// <summary>
        /// Export Data into .CSV file
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (ddlexport.SelectedItem.Value.ToString().ToLower() == "csv")
            {
                if (grvOrderlist.Rows.Count <= 1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('No Record(s) Found.','Message');", true);
                    btnUploadOrder.Visible = false;
                    if (grvOrderlist.Rows.Count == 1)
                    {
                        System.Web.UI.HtmlControls.HtmlTable chkUploadvalue = (System.Web.UI.HtmlControls.HtmlTable)grvOrderlist.Rows[0].FindControl("chkUploadvalue");
                        chkUploadvalue.Visible = false;
                    }
                    return;
                }
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition",
                 "attachment;filename=Orderlist_Export.csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                StringBuilder sb = new StringBuilder();
                //string[] strColumn = { "Order Date", "Order Number", "Customer Name", "Zip Code", "Customer Email", "Order Total", "Payment Status", "Order Status" };
                //foreach (String strCol in strColumn)
                //{
                //    //add separator
                //    //sb.Append(System.Text.RegularExpressions.Regex.Replace(grvOrderlist.Columns[k].HeaderText.ToString().Trim(), @"<[^>]*>", String.Empty) + ',');
                //    sb.Append(strCol.ToString() + ",");

                //}


                //append new line
                // sb.Append("\r\n");
                DataSet dsorder = new DataSet();
                //foreach (GridViewRow gr in grvOrderlist.Rows)
                //{
                //    DropDownList ddlStatus = (DropDownList)gr.FindControl("ddlStatus");
                //    if (ddlStatus.SelectedIndex > 0)
                //    {
                //        dsorder = CommonComponent.GetCommonDataSet("SELECT orderDate,Ordernumber,firstname+' '+ lastname as name,ShippingZip,Email,orderTotal,TransactionStatus,orderstatus  FROM tb_order WHERE orderstatus='" + ddlStatus.SelectedValue.ToString() + "' AND isnull(deleted,0)=0");
                //    }
                //    else
                //    {
                //        dsorder = CommonComponent.GetCommonDataSet("SELECT orderDate,Ordernumber,firstname+' '+ lastname as name,ShippingZip,Email,orderTotal,TransactionStatus,orderstatus  FROM tb_order WHERE isnull(deleted,0)=0");
                //    }
                //    break;
                //}

                bool falg = false;
                if (Session["GridDataTable"] != null)
                {
                    dsorder = (DataSet)Session["GridDataTable"];
                    foreach (GridViewRow gr in grvOrderlist.Rows)
                    {
                        DropDownList ddlStatus = (DropDownList)gr.FindControl("ddlStatus");
                        if (ddlStatus.SelectedIndex > 0)
                        {
                            if (ddlStatus.SelectedItem.Text.ToString().ToLower().IndexOf("shipped") > -1)
                            {
                                falg = true;
                                sb.AppendLine("Order Date,Order Number,Customer Name,Zip Code,Customer Email,Order Total,Payment Status,Order Status,Tracking NUmber,Shipped Via");
                            }
                            else
                            {
                                sb.AppendLine("Order Date,Order Number,Customer Name,Zip Code,Customer Email,Order Total,Payment Status,Order Status");
                            }
                        }
                        else
                        {
                            sb.AppendLine("Order Date,Order Number,Customer Name,Zip Code,Customer Email,Order Total,Payment Status,Order Status");
                        }
                        break;
                    }
                }
                else
                {
                    foreach (GridViewRow gr in grvOrderlist.Rows)
                    {
                        DropDownList ddlStatus = (DropDownList)gr.FindControl("ddlStatus");
                        if (ddlStatus.SelectedIndex > 0)
                        {
                            if (ddlStatus.SelectedItem.Text.ToString().ToLower().IndexOf("shipped") > -1)
                            {
                                falg = true;
                                dsorder = CommonComponent.GetCommonDataSet("SELECT orderDate,Ordernumber,firstname+' '+ lastname as name,ShippingZip,Email,orderTotal,TransactionStatus,orderstatus,ShippingTrackingNumber,ShippedVIA  FROM tb_order WHERE orderstatus='" + ddlStatus.SelectedValue.ToString() + "' AND isnull(deleted,0)=0");
                                sb.AppendLine("Order Date,Order Number,Customer Name,Zip Code,Customer Email,Order Total,Payment Status,Order Status,Tracking NUmber,Shipped Via");
                            }
                            else
                            {
                                sb.AppendLine("Order Date,Order Number,Customer Name,Zip Code,Customer Email,Order Total,Payment Status,Order Status");
                            }
                        }
                        else
                        {
                            sb.AppendLine("Order Date,Order Number,Customer Name,Zip Code,Customer Email,Order Total,Payment Status,Order Status");
                            dsorder = CommonComponent.GetCommonDataSet("SELECT orderDate,Ordernumber,firstname+' '+ lastname as name,ShippingZip,Email,orderTotal,TransactionStatus,orderstatus  FROM tb_order WHERE isnull(deleted,0)=0");
                        }
                        break;
                    }
                }
                for (int i = 1; i < dsorder.Tables[0].Rows.Count; i++)
                {
                    if (falg == true)
                    {
                        object[] args = new object[10];
                        args[0] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["orderDate"].ToString().Trim());
                        args[1] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["Ordernumber"].ToString().Trim());
                        args[2] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["CustName"].ToString().Replace("-", " "));
                        args[3] = dsorder.Tables[0].Rows[i]["ShippingZip"].ToString().Trim();
                        args[4] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["Email"].ToString().Trim());
                        args[5] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["orderTotal"].ToString().Trim());
                        args[6] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["TransactionStatus"].ToString().Trim());
                        args[7] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["orderstatus"].ToString().Trim());
                        args[8] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["ShippingTrackingNumber"].ToString().Trim());
                        args[9] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["ShippedVIA"].ToString().Trim());

                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", args));
                    }
                    else
                    {
                        object[] args = new object[8];
                        args[0] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["orderDate"].ToString().Trim());
                        args[1] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["Ordernumber"].ToString().Trim());
                        args[2] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["CustName"].ToString().Replace("-", " "));
                        args[3] = dsorder.Tables[0].Rows[i]["ShippingZip"].ToString().Trim();
                        args[4] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["Email"].ToString().Trim());
                        args[5] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["orderTotal"].ToString().Trim());
                        args[6] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["TransactionStatus"].ToString().Trim());
                        args[7] = _EscapeCsvField(dsorder.Tables[0].Rows[i]["orderstatus"].ToString().Trim());

                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", args));
                    }

                    //sb.Append(_EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i]["orderDate"].ToString().Trim().Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"), @"<[^>]*>", String.Empty)) + ',');
                    //sb.Append(_EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i]["Ordernumber"].ToString().Trim().Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"), @"<[^>]*>", String.Empty)) + ',');
                    //sb.Append(_EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i]["name"].ToString().Trim().Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"), @"<[^>]*>", String.Empty)) + ',');
                    //sb.Append(_EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i]["ShippingZip"].ToString().Trim().Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"), @"<[^>]*>", String.Empty)) + ',');
                    //sb.Append(_EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i]["Email"].ToString().Trim().Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"), @"<[^>]*>", String.Empty)) + ',');
                    //sb.Append(_EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i]["orderTotal"].ToString().Trim().Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"), @"<[^>]*>", String.Empty)) + ',');
                    //sb.Append(_EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i]["TransactionStatus"].ToString().Trim().Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"), @"<[^>]*>", String.Empty)) + ',');
                    //sb.Append(_EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i]["orderstatus"].ToString().Trim().Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"), @"<[^>]*>", String.Empty)) + ',');
                    //sb.Append("\r\n");
                }
                //foreach (GridViewRow gr in grvOrderlist.Rows)
                //{
                //    if (gr.RowIndex != 0)
                //    {

                //        for (int k = 0; k < grvOrderlist.Columns.Count - 1; k++)
                //        {

                //            //add separator
                //            Literal ltr = (Literal)gr.FindControl("ltr" + (k + 1).ToString());
                //            sb.Append(_EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(ltr.Text.ToString().Trim().Replace("<br />", "\r\n").Replace("<br/>", "\r\n").Replace("<br>", "\r\n"), @"<[^>]*>", String.Empty)) + ',');
                //        }
                //        //append new line
                //        sb.Append("\r\n");
                //    }

                //}

                Response.Output.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }



        }

        /// <summary>
        /// Sorting Data Grid
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton blImage = (ImageButton)sender;
            if (blImage != null)
            {
                if (blImage.CommandArgument == "ASC")
                {
                    DataView dv = new DataView();
                    DataSet dt = new DataSet();
                    ViewState["sortExpression"] = "DESC";
                    if (Session["GridDataTable"] != null)
                    {
                        dt = (DataSet)Session["GridDataTable"];
                        dt.Tables[0].Rows[0]["orderdate"] = DateTime.Now.ToString();
                        dt.Tables[0].AcceptChanges();
                        dv = dt.Tables[0].DefaultView;
                        dv.Sort = blImage.CommandName.ToString() + " DESC";
                        dv.ToTable();

                        //dv.Table.Rows.InsertAt(row, 0);
                        //dv.ToTable();
                        grvOrderlist.DataSource = dv;
                        grvOrderlist.DataBind();
                    }

                }
                else if (blImage.CommandArgument == "DESC")
                {
                    DataView dv = new DataView();
                    DataSet dt = new DataSet();
                    ViewState["sortExpression"] = "ASC";
                    if (Session["GridDataTable"] != null)
                    {
                        dt = (DataSet)Session["GridDataTable"];
                        dt.Tables[0].Rows[0]["orderdate"] = DateTime.Now.Date.AddYears(-50);
                        dt.Tables[0].AcceptChanges();

                        dv = dt.Tables[0].DefaultView;
                        dv.Sort = blImage.CommandName.ToString() + " ASC";
                        dv.ToTable();

                        grvOrderlist.DataSource = dv;
                        grvOrderlist.DataBind();
                    }
                    isAscend = false;

                }
            }
        }

        /// <summary>
        /// Order List Gridview Sorting Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewSortEventArgs e</param>
        protected void grvOrderlist_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["SearchFilter"] = null;

            string strSearch = "";
            TextBox txtOrderFrom = (TextBox)grvOrderlist.Rows[0].FindControl("txtOrderFrom");
            TextBox txtOrderTo = (TextBox)grvOrderlist.Rows[0].FindControl("txtOrderTo");

            CheckBox chkgrndshipp = (CheckBox)grvOrderlist.Rows[0].FindControl("chkgrndshipp");

            DropDownList ddlstockstat = (DropDownList)grvOrderlist.Rows[0].FindControl("ddlstockstat");
            if (txtOrderFrom.Text.ToString() != "")
            {

                strSearch += " AND cast(orderdate as date) >= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtOrderFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
                ViewState["DateFrom"] = txtOrderFrom.Text.ToString();
            }
            if (txtOrderTo.Text.ToString() != "")
            {

                strSearch += " AND cast(orderdate as date) <= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtOrderTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
                ViewState["DateTo"] = txtOrderTo.Text.ToString();
            }
            if ((ddlStoregeneral.SelectedValue.ToString() == "4" || ddlStoregeneral.SelectedValue.ToString() == "3" || ddlStoregeneral.SelectedValue.ToString() == "1" || ddlStoregeneral.SelectedValue.ToString() == "11") && ddlstockstat.SelectedValue.ToString() == "1")
            {
                strSearch += " AND isnull(isNAVInserted,0)= 1 AND isnull(isnavcompleted,0)=0 ";
                // strSearch += " AND isnull(isbackEnd,1)= 1 AND isnull(backEndGuid,'')='' ";
                ViewState["ddlStockStore"] = "1";
                //ViewState["DateTo"] = txtOrderTo.Text.ToString();
            }
            else if ((ddlStoregeneral.SelectedValue.ToString() == "4" || ddlStoregeneral.SelectedValue.ToString() == "3" || ddlStoregeneral.SelectedValue.ToString() == "1" || ddlStoregeneral.SelectedValue.ToString() == "11") && ddlstockstat.SelectedValue.ToString() == "2")
            {
                strSearch += " AND isnull(isNAVInserted,0)= 0 AND isnull(isnavcompleted,0)=1 ";
                // strSearch += " AND isnull(isbackEnd,1)= 0 AND isnull(backEndGuid,'')='' ";
                ViewState["ddlStockStore"] = "2";
                //ViewState["DateTo"] = txtOrderTo.Text.ToString();
            }
            else if ((ddlStoregeneral.SelectedValue.ToString() == "4" || ddlStoregeneral.SelectedValue.ToString() == "3" || ddlStoregeneral.SelectedValue.ToString() == "1" || ddlStoregeneral.SelectedValue.ToString() == "11") && ddlstockstat.SelectedValue.ToString() == "3")
            {
                strSearch += " AND isnull(isNAVInserted,0)= 1 AND isnull(isnavcompleted,0)=2 ";
                // strSearch += " AND isnull(isbackEnd,1)= 1 AND isnull(backEndGuid,'') <> '' ";
                ViewState["ddlStockStore"] = "3";
                //ViewState["DateTo"] = txtOrderTo.Text.ToString();
            }

            else
            {
                ViewState["ddlStockStore"] = "0";
            }
            if (chkgrndshipp.Checked == true)
            {
                strSearch += " AND isnull(ShippingMethod,'') NOT IN  ('ground','grnd','standard') ";
                ViewState["chkgrndshipp"] = true;

            }
            else
            {
                ViewState["chkgrndshipp"] = false;
            }

            if (txtEmail.Text.ToString().Trim() != "")
            {
                strSearch += " AND (Email like '%" + txtEmail.Text.ToString().Replace("'", "''") + "%' ";
                strSearch += " Or BillingEmail like '%" + txtEmail.Text.ToString().Replace("'", "''") + "%' ";
                strSearch += " Or ShippingEmail like '%" + txtEmail.Text.ToString().Replace("'", "''") + "%') ";
            }
            if (txtzipcode.Text.ToString().Trim() != "")
            {
                strSearch += " AND (BillingZip like '%" + txtzipcode.Text.ToString().Replace("'", "''") + "%' ";
                strSearch += " Or ShippingZip like '%" + txtzipcode.Text.ToString().Replace("'", "''") + "%') ";

            }
            if (txtRefOrderNo.Text.ToString().Trim() != "")
            {
                strSearch += " AND RefOrderID like '%" + txtRefOrderNo.Text.ToString().Trim().Replace("'", "''") + "%' ";
            }
            if (txtpo.Text.ToString().Trim() != "")
            {
                int poNumber = 0;
                bool fl = Int32.TryParse(txtpo.Text.ToString().Trim(), out poNumber);
                if (fl)
                {
                    strSearch += " AND OrderNumber in (SELECT OrderNumber FROM tb_PurchaseOrder WHERE PONumber=" + poNumber + ")";
                }
            }

            if (txtcompanyname.Text.ToString().Trim() != "")
            {

                strSearch += " AND (BillingCompany like '%" + txtcompanyname.Text.ToString().Replace("'", "''") + "%' ";
                strSearch += " Or ShippingCompany like '%" + txtcompanyname.Text.ToString().Replace("'", "''") + "%') ";
            }
            if (txtcompanyname.Text.ToString().Trim() != "")
            {

                strSearch += " AND (BillingCompany like '%" + txtcompanyname.Text.ToString().Replace("'", "''") + "%' ";
                strSearch += " Or ShippingCompany like '%" + txtcompanyname.Text.ToString().Replace("'", "''") + "%') ";
            }
            if (ddlpayment.SelectedIndex > 0)
            {

                strSearch += " AND PaymentMethod like '%" + ddlpayment.SelectedValue.ToString().Replace("'", "''") + "%' ";

            }
            if (ddlTransactionStatus.SelectedIndex > 0)
            {
                strSearch += " AND TransactionStatus = '" + ddlTransactionStatus.SelectedValue.ToString().Replace("'", "''") + "' ";

            }
            if (txtcoupncode.Text.ToString().Trim() != "")
            {
                strSearch += " AND CouponCode like '%" + txtcoupncode.Text.ToString().Replace("'", "''") + "%' ";
            }
            if (ddlsalesagents.SelectedIndex > 0)
            {
                strSearch += "AND SalesAgentID='" + ddlsalesagents.SelectedValue.ToString().Replace("'", "''") + "' ";
            }
            if (dlState.SelectedIndex > 0)
            {

                strSearch += " AND (BillingState like '%" + dlState.SelectedValue.ToString().Replace("'", "''") + "%' ";
                strSearch += " Or ShippingState like '%" + dlState.SelectedValue.ToString().Replace("'", "''") + "%') ";
            }
            ViewState["SearchFilter"] = strSearch;
            GetOrderListByStoreId(1, pageSize);
        }

        /// <summary>
        /// Gets the Payment Method
        /// </summary>
        private void GetPaymentMethod()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT PaymentType FROM tb_Payment WHERE isnull(Deleted,0)=0 ORDER BY PaymentType");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlpayment.DataSource = ds;
                ddlpayment.DataTextField = "PaymentType";
                ddlpayment.DataValueField = "PaymentType";
                ddlpayment.DataBind();
            }
            else
            {
                ddlpayment.DataSource = null;
                ddlpayment.DataBind();
            }
            ddlpayment.Items.Insert(0, new ListItem("All", "0"));

        }

        /// <summary>
        /// Gets the State
        /// </summary>
        private void GetState()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT Name,Abbreviation FROM tb_State WHERE isnull(Deleted,0)=0 ORDER BY Name");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dlState.DataSource = ds;
                dlState.DataTextField = "Name";
                dlState.DataValueField = "Name";
                dlState.DataBind();
            }
            else
            {
                dlState.DataSource = null;
                dlState.DataBind();
            }
            dlState.Items.Insert(0, new ListItem("All", "0"));
        }

        /// <summary>
        /// Gets the sales agents.
        /// </summary>
        private void GetSalesAgents()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet(" SELECT AdminID,(FirstName+' '+LastName) AS Name FROM tb_admin WHERE AdminID IN( SELECT adminid FROM tb_Admin where Deleted=0)");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsalesagents.DataSource = ds;
                ddlsalesagents.DataTextField = "Name";
                ddlsalesagents.DataValueField = "AdminID";
                ddlsalesagents.DataBind();
            }
            else
            {
                ddlsalesagents.DataSource = null;
                ddlsalesagents.DataBind();
            }
            ddlsalesagents.Items.Insert(0, new ListItem("All", "0"));
        }

        /// <summary>
        /// Store General Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStoregeneral_SelectedIndexChanged(object sender, EventArgs e)
        {

            //ViewState["SearchFilter"] = " AND dbo.tb_Order.orderStatus='New'";
            ViewState["SearchFilter"] = "";
            ViewState["DateFrom"] = null;
            ViewState["DateTo"] = null;
            ViewState["OrderNumber"] = null;
            ViewState["Customername"] = null;
            ViewState["ddlStatus"] = null;
            ViewState["orderTotalFrom"] = null;
            ViewState["orderTotalTo"] = null;
            ViewState["ddlStore"] = null;
            ViewState["ddlStockStore"] = null;
            ViewState["chkgrndshipp"] = false;

            if (ddlStoregeneral.SelectedValue.ToString() == "0" || ddlStoregeneral.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStoregeneral.SelectedValue.ToString());
            }
            AppLogic.ApplicationStart();
            if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")) && ddlStoregeneral.SelectedValue.ToString() != "0")
            {

                btnnavuplod.Visible = true;
                btnUploadOrder.Visible = false;
                imgbtnMultipleCapture.Visible = false;


            }
            else
            {
                btnnavuplod.Visible = false;
                btnUploadOrder.Visible = false;
                if (ddlStoregeneral.SelectedValue.ToString() == "0")
                {
                    btnUploadOrder.Visible = false;
                }

                if (ddlStoregeneral.SelectedValue == "1")
                {
                    imgbtnMultipleCapture.Visible = true;



                }
                else
                {
                    imgbtnMultipleCapture.Visible = false;

                }
            }


            GetOrderListByStoreId(1, pageSize);
        }

        protected void ddlwarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {

            //ViewState["SearchFilter"] = " AND dbo.tb_Order.orderStatus='New'";

            GetOrderListByStoreId(1, pageSize);
        }

        /// <summary>
        /// Page Record Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlPageRecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPagenumber.Text = "1";
            pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue.ToString());
            GetOrderListByStoreId(1, pageSize);
        }

        /// <summary>
        ///  Capture Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCapture_Click(object sender, EventArgs e)
        {
            string strResult = "OK";
            tb_Order objOrder = new tb_Order();
            DataSet dsAutho = new DataSet();
            objOrder.CaptureTxCommand = "";
            objOrder.CaptureTXResult = "";
            objOrder.AuthorizationPNREF = "";
            string Transactionstatus = "";

            dsAutho = CommonComponent.GetCommonDataSet("SELECT isnull(AuthorizationPNREF,'') as AuthorizationPNREF,isnull(PaymentGateway,'') as PaymentGateway,isnull(OrderTotal,0) as OrderTotal,isnull(TransactionStatus,'') as TransactionStatus FROM tb_order where OrderNumber=" + hdnordernumber.Value.ToString() + "");
            string PaymentGAteWay = "";
            if (dsAutho != null && dsAutho.Tables.Count > 0 && dsAutho.Tables[0].Rows.Count > 0)
            {
                objOrder.AuthorizationPNREF = dsAutho.Tables[0].Rows[0]["AuthorizationPNREF"].ToString();
                PaymentGAteWay = dsAutho.Tables[0].Rows[0]["PaymentGateway"].ToString();
                objOrder.OrderTotal = Convert.ToDecimal(dsAutho.Tables[0].Rows[0]["OrderTotal"].ToString());
                Transactionstatus = Convert.ToString(dsAutho.Tables[0].Rows[0]["TransactionStatus"].ToString());
                objOrder.OrderNumber = Convert.ToInt32(hdnordernumber.Value.ToString());
            }
            if (PaymentGAteWay.ToString().ToLower().Trim() == "paypal")
            {
                PayPalComponent objPay = new PayPalComponent();
                strResult = objPay.CaptureOrder(objOrder);
            }
            if (PaymentGAteWay.ToString().ToLower().Trim() == "paypalexpress")
            {
                PayPalComponent objPay = new PayPalComponent();
                strResult = objPay.CaptureOrder(objOrder);
            }

            else if (PaymentGAteWay.ToString().ToLower().Trim() == "authorizenet")
            {

                if (Transactionstatus.ToLower().Trim() == "authorized")
                {

                    tb_Order objorderCapture = new tb_Order();
                    OrderComponent objCapture = new OrderComponent();
                    objorderCapture = objCapture.GetOrderByOrderNumber(Convert.ToInt32(hdnordernumber.Value.ToString()));
                    strResult = AuthorizeNetComponent.CaptureOrderFirst(objOrder);
                    Decimal AuthorizedAmount1 = Convert.ToDecimal(objorderCapture.AuthorizedAmount.ToString());
                    Decimal AuthorizedAmount = Convert.ToDecimal(objorderCapture.OrderTotal) - AuthorizedAmount1;
                    if (AuthorizedAmount > Convert.ToDecimal(0) && AuthorizedAmount1 > Convert.ToDecimal(0) && strResult == "OK")
                    {

                        String AVSResult = String.Empty;
                        String AuthorizationResult = String.Empty;
                        String AuthorizationCode = String.Empty;
                        String AuthorizationTransID = String.Empty;
                        String TransactionCommand = String.Empty;
                        String TransactionResponse = String.Empty;
                        string strResult1 = AuthorizeNetComponent.ProcessCardForYahooorderAdmin(Convert.ToInt32(hdnordernumber.Value.ToString()), Convert.ToInt32(objorderCapture.CustomerID), Convert.ToDecimal(AuthorizedAmount.ToString()), AppLogic.AppConfigBool("UseLiveTransactions"), "auth_capture", objorderCapture, objorderCapture, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                        if (strResult1 == "OK")
                        {
                            try
                            {
                                objOrder.AuthorizationCode += "," + AuthorizationCode.ToString();
                                objOrder.AuthorizationPNREF += "|CAPTURE=" + AuthorizationTransID.ToString();
                                objOrder.CaptureTXResult += "," + AuthorizationResult.ToString();
                                objOrder.CaptureTxCommand += "," + TransactionCommand.ToString();
                                objOrder.AVSResult += "," + AVSResult;
                            }
                            catch
                            {

                            }
                            //CommonComponent.ExecuteCommonData("UPDATE tb_order SET AuthorizedAmount=AuthorizedAmount + " + AuthorizedAmount.ToString() + " WHERE orderNumber=" + Request.QueryString["id"].ToString() + "");

                        }
                    }

                }
            }
            if (strResult == "OK")
            {
                OrderComponent objOrderlog = new OrderComponent();
                objOrderlog.InsertOrderlog(2, Convert.ToInt32(hdnordernumber.Value.ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                CommonComponent.ExecuteCommonData("UPDATE tb_order SET CaptureTXCommand='" + objOrder.CaptureTxCommand.ToString().Replace("'", "''") + "',CaptureTXResult='" + objOrder.CaptureTXResult.ToString().Replace("'", "''") + "',AuthorizationPNREF='" + objOrder.AuthorizationPNREF.ToString().Replace("'", "''") + "', CapturedOn=dateadd(hour,-2,getdate()), TransactionStatus='CAPTURED' WHERE orderNumber=" + hdnordernumber.Value.ToString() + "");
                CommonComponent.ExecuteCommonData("EXEC usp_Product_AdjustInventory " + hdnordernumber.Value.ToString() + ",-1, 0");
                pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue.ToString());
                GetOrderListByStoreId(Convert.ToInt32(txtPagenumber.Text.ToString()), pageSize);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Your Transaction State has been CAPTURED.','Success');", true);

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "','Failed');", true);
            }

        }

        /// <summary>
        ///  Refund Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnRefund_Click(object sender, EventArgs e)
        {
            pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue.ToString());
            GetOrderListByStoreId(Convert.ToInt32(txtPagenumber.Text.ToString()), pageSize);
        }

        public void SetAdminPageRight()
        {
            if (Session["dtAdminRightsList"] != null)
            {
                DataTable dtright = new DataTable();
                dtright = (DataTable)Session["dtAdminRightsList"];
                if (dtright != null && dtright.Rows.Count > 0)
                {
                    for (int i = 0; i < dtright.Rows.Count; i++)
                    {
                        switch (Convert.ToString(dtright.Rows[i]["PageName"]))
                        {

                            case "Phone Orders":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    btnCustomerPhoneOrder.Visible = true;
                                }
                                else
                                {
                                    btnCustomerPhoneOrder.Visible = false; ;
                                }
                                break;
                        }
                    }
                }
            }
        }

        public static bool SetOverstockValue(string val)
        {

            return true;
        }

        protected void btnUploadOrder_Click(object sender, EventArgs e)
        {
            // for loop


            if (grvOrderlist.Rows.Count > 0)
            {


                for (int i = 0; i < grvOrderlist.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                    }
                    else
                    {
                        Label lblOrderNumber = (Label)grvOrderlist.Rows[i].FindControl("lblOrderNumber");
                        CheckBox chkUploadOrderStock = (CheckBox)grvOrderlist.Rows[i].FindControl("chkUploadOrderStock");

                        if (chkUploadOrderStock.Checked == true)
                        {
                            CommonComponent.ExecuteCommonData("Update tb_order set IsBackEnd=0,IsuploadCancel=0 where ordernumber=" + lblOrderNumber.Text.ToString() + "");
                            btnUploadOrder.Visible = false;
                            OrderComponent objAddOrder = new OrderComponent();
                            objAddOrder.InsertOrderlog(28, Convert.ToInt32(lblOrderNumber.Text.ToString()), Request.UserHostAddress.ToString(), Convert.ToInt32(Session["AdminID"].ToString()));

                        }
                    }
                }
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@uploorder", "jAlert('Order uploaded successfully, Please check in Acctivate after 5 minutes.','Message');", true);
            GetOrderListByStoreId(1, pageSize);
        }


        /// <summary>
        /// Button Click of multiple invoice print
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnMultiplePrint_Click(object sender, EventArgs e)
        {
            OrderComponent ordcomp = new OrderComponent();
            Session["PrintOrderIdOlrderlist"] = "";
            //  int i = 0;
            string strprint = "";
            for (int i = 0; i < grvOrderlist.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)grvOrderlist.Rows[i].FindControl("chkprintsalesorder");

                //    Literal ltrPrinted = (Literal)r.FindControl("ltrPrinted");
                Label lb = (Label)grvOrderlist.Rows[i].FindControl("lblMultiCapOrderNnumber");
                string[] fls = { "" };
                if (chk.Checked)
                {
                    if (i == 0)
                        Session["PrintOrderIdOlrderlist"] += lb.Text.ToString();
                    else
                        Session["PrintOrderIdOlrderlist"] += "," + lb.Text.ToString();


                    DataSet ds = new DataSet();
                    ds = ordcomp.GetDetailforOrder(lb.Text.Trim());

                    string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
                    string Body = "";



                    string url = "http://www.halfpricedrapes.us/Admin/Orders/" + "invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(lb.Text.ToString()));
                    WebRequest NewWebReq = WebRequest.Create(url);
                    WebResponse newWebRes = NewWebReq.GetResponse();
                    string format = newWebRes.ContentType;
                    Stream ftprespstrm = newWebRes.GetResponseStream();
                    StreamReader reader;
                    reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
                    string strbiody = reader.ReadToEnd().ToString();
                    strbiody = strbiody.Replace("'", "&#39;");
                    string sttt = @"<style type='text/css'>
        .table_invoice
        {
            width: 100%;
        }
        .table_invoice td
        {
            padding: 3px;
        }
        .table_invoice td strong
        {
            font-weight: bold;
            color: #d5321c;
        }
        .datatable table
        {
            border: 1px solid #eeeeee;
        }
        .datatable tr.alter_row
        {
            background-color: #f9f9f9;
        }
        .datatable td
        {
            text-align: left;
            border: 1px solid #eeeeee;
            font-size: 11px;font-family: Arial, Helvetica, sans-serif;
            color: #4c4c4c;
            line-height: 14px;
        }
        .datatable th
        {
            padding: 2px 3px;
            text-align: left;
            border: 1px solid #eeeeee;
            font-size: 11px;font-family: Arial, Helvetica, sans-serif;
            font-weight: bold;
            color: #4c4c4c;
            line-height: 16px;
        }
        .receiptfont
        {
            font-size: 11px;font-family: Arial, Helvetica, sans-serif;
            color: #4c4c4c;
        }
        .receiptlineheight
        {
            height: 15px;
        }
        .popup_cantain
        {
            font-size: 11px;font-family: Arial,Helvetica,sans-serif;
            color: #4C4C4C;
            text-decoration: none;
        }
        .popup_cantain a
        {
           font-size: 11px;font-family: Arial,Helvetica,sans-serif;
            color: #FE0000;
            text-decoration: none;
        }
        .popup_cantain a:hover
        {
            font-size: 11px;font-family: Arial,Helvetica,sans-serif;
            color: #000;
            text-decoration: underline;
        }
        .Printinvoice
        {
        }
    </style>
    <style type='text/css' media='print'>
        .Printinvoice
        {
            display: none;
        }
    </style>";
                    Body += strbiody.ToString();


                    Body = Body.Replace("title=\"Print Invoice\"", "title=\"Print Invoice\" style=\"display:none;\"");
                    Body = Body.Replace("title=\"Send Invoice\"", "title=\"Send Invoice\" style=\"display:none;\"");
                    Body = Body.Substring(Body.IndexOf("<table"), Body.LastIndexOf("</table") - Body.IndexOf("<table"));
                    string strpackageslip = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT OrderNumber FROM tb_Order WHERE OrderNumber=" + lb.Text.ToString() + " and (isnull(Transactionstatus,'')='canceled' or isnull(Orderstatus,'')='canceled')"));
                    if (!string.IsNullOrEmpty(strpackageslip))
                    {

                        strprint += "<div style=\"page-break-after:always\">" + sttt.Replace("'", "\"") + Body + "</table><img src=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/images/watermark_canceled.png\" style=\"left:30%;top:35%;position:fixed;z-index:1000;\"></div>";
                    }
                    else
                    {
                        strprint += "<div style=\"page-break-after:always\">" + sttt.Replace("'", "\"") + Body + "</table></div>";
                    }




                }
            }
            if (strprint.ToString().LastIndexOf("style=\"page-break-after:always\"") > -1)
            {
                strprint = strprint.Remove(strprint.ToString().LastIndexOf("style=\"page-break-after:always\""), 31);
                strprint = strprint.Replace("<div >", "<div>");
                strprint = strprint.Replace(System.Environment.NewLine, "");
                String strPrinting = @"var BrowserName = navigator.appName.toString();
                if (BrowserName.toString().toLowerCase().indexOf('internet explorer') > -1) {
                    w = window.open('', 'msg', 'directories=no, location=no, menubar=no, status=yes,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=10,Width=10,left=0,top=0,visible=false,alwaysLowered=yes');
                    w.document.write('" + strprint.ToString() + @"');
                    w.document.close();
                    w.print();
                    w.close();
                }
                else {
                    var pri = document.getElementById('ifmcontentstoprint').contentWindow;
                    pri.document.open();
                    pri.document.write('" + strprint.ToString() + @"');
                    pri.document.close();
                    pri.focus();
                    pri.print();
                }";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "iframeprint", "" + strPrinting.ToString() + "", true);



            }

            if (Session["PrintOrderIdOlrderlist"] != null && Session["PrintOrderIdOlrderlist"].ToString() != "")
            {

                GetOrderListByStoreId(1, pageSize);
            }
            Session["PrintOrderIdOlrderlist"] = "";

        }

        /// <summary>
        /// Button click of multiple captured
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnMultipleCapture_Click(object sender, EventArgs e)
        {

            Int32 faliedcount = 0;
            Int32 successcount = 0;
            if (grvOrderlist.Rows.Count > 0)
            {
                string strResult = "";

                for (int i = 0; i < grvOrderlist.Rows.Count; i++)
                {
                    strResult = "";
                    CheckBox ChkSelect = (CheckBox)grvOrderlist.Rows[i].FindControl("chkUploadOrderStock");
                    if (ChkSelect.Checked)
                    {
                        Label lblMultiCapOrderNnumber = (Label)grvOrderlist.Rows[i].FindControl("lblMultiCapOrderNnumber");
                        if (lblMultiCapOrderNnumber != null)
                        {
                            Int32 OrderNum = Convert.ToInt32(lblMultiCapOrderNnumber.Text.ToString());

                            tb_Order objOrder = new tb_Order();
                            DataSet dsAutho = new DataSet();
                            objOrder.CaptureTxCommand = "";
                            objOrder.CaptureTXResult = "";
                            objOrder.AuthorizationPNREF = "";
                            string Transactionstatus = "";
                            dsAutho = CommonComponent.GetCommonDataSet("SELECT isnull(AuthorizationPNREF,'') as AuthorizationPNREF,isnull(PaymentGateway,'') as PaymentGateway,isnull(OrderTotal,0) as OrderTotal,isnull(TransactionStatus,'') as TransactionStatus FROM tb_order where OrderNumber=" + OrderNum.ToString() + "");
                            string PaymentGAteWay = "";
                            if (dsAutho != null && dsAutho.Tables.Count > 0 && dsAutho.Tables[0].Rows.Count > 0)
                            {
                                objOrder.AuthorizationPNREF = dsAutho.Tables[0].Rows[0]["AuthorizationPNREF"].ToString();
                                PaymentGAteWay = dsAutho.Tables[0].Rows[0]["PaymentGateway"].ToString();
                                objOrder.OrderTotal = Convert.ToDecimal(dsAutho.Tables[0].Rows[0]["OrderTotal"].ToString());
                                Transactionstatus = Convert.ToString(dsAutho.Tables[0].Rows[0]["TransactionStatus"].ToString());
                                objOrder.OrderNumber = Convert.ToInt32(OrderNum.ToString());
                            }
                            if (PaymentGAteWay.ToString().ToLower().Trim() == "paypal")
                            {
                                PayPalComponent objPay = new PayPalComponent();
                                strResult = objPay.CaptureOrder(objOrder);
                            }
                            if (PaymentGAteWay.ToString().ToLower().Trim() == "paypalexpress")
                            {
                                PayPalComponent objPay = new PayPalComponent();
                                strResult = objPay.CaptureOrder(objOrder);
                            }

                            else if (PaymentGAteWay.ToString().ToLower().Trim() == "authorizenet")
                            {

                                if (Transactionstatus.ToLower().Trim() == "authorized")
                                {

                                    tb_Order objorderCapture = new tb_Order();
                                    OrderComponent objCapture = new OrderComponent();
                                    objorderCapture = objCapture.GetOrderByOrderNumber(Convert.ToInt32(OrderNum.ToString()));
                                    strResult = AuthorizeNetComponent.CaptureOrderFirst(objOrder);
                                    Decimal AuthorizedAmount1 = Convert.ToDecimal(objorderCapture.AuthorizedAmount.ToString());
                                    Decimal AuthorizedAmount = Convert.ToDecimal(objorderCapture.OrderTotal) - AuthorizedAmount1;
                                    if (AuthorizedAmount > Convert.ToDecimal(0) && AuthorizedAmount1 > Convert.ToDecimal(0) && strResult == "OK")
                                    {

                                        String AVSResult = String.Empty;
                                        String AuthorizationResult = String.Empty;
                                        String AuthorizationCode = String.Empty;
                                        String AuthorizationTransID = String.Empty;
                                        String TransactionCommand = String.Empty;
                                        String TransactionResponse = String.Empty;
                                        string strResult1 = AuthorizeNetComponent.ProcessCardForYahooorderAdmin(Convert.ToInt32(OrderNum.ToString()), Convert.ToInt32(objorderCapture.CustomerID), Convert.ToDecimal(AuthorizedAmount.ToString()), AppLogic.AppConfigBool("UseLiveTransactions"), "auth_capture", objorderCapture, objorderCapture, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                                        if (strResult1 == "OK")
                                        {
                                            try
                                            {
                                                objOrder.AuthorizationCode += "," + AuthorizationCode.ToString();
                                                objOrder.AuthorizationPNREF += "|CAPTURE=" + AuthorizationTransID.ToString();
                                                objOrder.CaptureTXResult += "," + AuthorizationResult.ToString();
                                                objOrder.CaptureTxCommand += "," + TransactionCommand.ToString();
                                                objOrder.AVSResult += "," + AVSResult;
                                            }
                                            catch
                                            {

                                            }
                                            //CommonComponent.ExecuteCommonData("UPDATE tb_order SET AuthorizedAmount=AuthorizedAmount + " + AuthorizedAmount.ToString() + " WHERE orderNumber=" + Request.QueryString["id"].ToString() + "");

                                        }
                                    }

                                }
                            }
                            if (strResult == "OK")
                            {
                                successcount++;
                                OrderComponent objOrderlog = new OrderComponent();
                                objOrderlog.InsertOrderlog(2, Convert.ToInt32(OrderNum.ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                                CommonComponent.ExecuteCommonData("UPDATE tb_order SET CaptureTXCommand='" + objOrder.CaptureTxCommand.ToString().Replace("'", "''") + "',CaptureTXResult='" + objOrder.CaptureTXResult.ToString().Replace("'", "''") + "',AuthorizationPNREF='" + objOrder.AuthorizationPNREF.ToString().Replace("'", "''") + "', CapturedOn=dateadd(hour,-2,getdate()), TransactionStatus='CAPTURED' WHERE orderNumber=" + OrderNum.ToString() + "");
                                //CommonComponent.ExecuteCommonData("EXEC usp_Product_AdjustInventory " + OrderNum.ToString() + ",-1, 0");



                            }
                            else
                            {
                                faliedcount++;
                                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "','Failed');", true);
                            }
                        }
                    }

                }
                pageSize = Convert.ToInt32(ddlPageRecord.SelectedValue.ToString());
                GetOrderListByStoreId(Convert.ToInt32(txtPagenumber.Text.ToString()), pageSize);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Total Order(s) " + successcount.ToString() + " Captured Successfully.','Message');", true);


            }


        }



        #region "Microsoft nav "

        protected void btnnavuplod_Click(object sender, EventArgs e)
        {
            // for loop
            bool chk = false;
            if (grvOrderlist.Rows.Count > 0)
            {
                TextBox txtOrderFrom = (TextBox)grvOrderlist.Rows[0].FindControl("txtOrderFrom");
                TextBox txtOrderTo = (TextBox)grvOrderlist.Rows[0].FindControl("txtOrderTo");
                if (txtOrderFrom.Text.ToString().Trim() != "" && txtOrderTo.Text.ToString().Trim() != "")
                {
                    TimeSpan span = Convert.ToDateTime(txtOrderTo.Text.ToString()) - Convert.ToDateTime(txtOrderFrom.Text.ToString());
                    Int32 Totaldays = Convert.ToInt32(span.TotalDays.ToString());
                    if (Totaldays > 60)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@uploorderdate", "jAlert('Order search date invalid.','Message');", true);
                        return;
                    }
                }
                for (int i = 0; i < grvOrderlist.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                    }
                    else
                    {
                        Label lblOrderNumber = (Label)grvOrderlist.Rows[i].FindControl("lblOrderNumber");
                        CheckBox chkUploadOrderStock = (CheckBox)grvOrderlist.Rows[i].FindControl("chkUploadOrderStock");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdncustId = (System.Web.UI.HtmlControls.HtmlInputHidden)grvOrderlist.Rows[i].FindControl("hdncustId");
                        if (chkUploadOrderStock.Checked == true)
                        {
                            chk = true;
                            CommonComponent.ExecuteCommonData("UPDATE tb_order SET isNAVInserted=0,isnavcompleted=1,IsNavError=0,NAVError='' WHERE OrderNumber=" + lblOrderNumber.Text.ToString() + "");
                        }
                    }
                }
            }
            if (chk == true)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@uploorder", "jAlert('Order ready for import to NAV, Please check in NAV after 5 minutes.','Message');", true);
            }

        }






        #endregion
    }
}