using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Data;
using System.Net;
using System.IO;
using System.Net.Mail;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class OverStockPackingSlip : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ONo"] != null)
            {
                int OrderNumber = 0;
                Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                Int32 StoreID = 0;
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + OrderNumber)), out StoreID);
                AppConfig.StoreID = StoreID;
                ImgStoreLogo.Src = "/Images/Store_" + StoreID.ToString() + ".png";
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["ONo"] != null)
                {
                    //BindRefNumberDetails();
                    int OrderNumber = 0;
                    bool chkOrder = Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                    if (chkOrder)
                    {
                        ltrorderNumber.Text = OrderNumber.ToString();
                        GetOrderDetails(OrderNumber);
                    }
                }
            }
        }

        /// <summary>
        /// Bind Order Details For Receipt
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        private void GetOrderDetails(Int32 OrderNumber)
        {
            OrderComponent objOrder = new OrderComponent();
            DataSet objDsorder = new DataSet();
            objDsorder = objOrder.GetOrderDetailsByOrderID(OrderNumber);
            if (objDsorder != null && objDsorder.Tables.Count > 0 && objDsorder.Tables[0].Rows.Count > 0)
            {
                string StrDate = "";
                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["OrderDate"].ToString()))
                {
                    StrDate = Convert.ToString(string.Format("{0:mm/dd/YYYY}", objDsorder.Tables[0].Rows[0]["OrderDate"].ToString()));
                }
                ltrOrderdate.Text = StrDate.ToString();
                ltrorderNumber.Text = OrderNumber.ToString();
                ltrshippingMethod.Text = objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString();
                ltrshippedvia.Text = objDsorder.Tables[0].Rows[0]["shippedvia"].ToString();

                string StrShippedVia = "";
                StrShippedVia = Convert.ToString(CommonComponent.GetScalarCommonData("Select distinct cast(ISNULL(ShippedVia,0)+',' as varchar(max)) from tb_OrderShippedItems Where OrderNumber=" + OrderNumber + " for xml path('')"));
                if (!string.IsNullOrEmpty(StrShippedVia) && StrShippedVia.Length > 0)
                {
                    StrShippedVia = StrShippedVia.Substring(0, StrShippedVia.Length - 1);
                }
                ltrshippedvia.Text = StrShippedVia.ToString();

                ltrShiptoName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString());

                string StrShipAddr = "";
                StrShipAddr = Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString()) + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString() + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString() + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString() + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString() + ", ";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingState"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingState"].ToString() + " ";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                    StrShipAddr += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()) + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString();

                ltrAddress.Text += StrShipAddr.ToString();

                BindCart(Convert.ToInt32(OrderNumber.ToString()), objDsorder);

                DataSet dsTopic = new DataSet();
                dsTopic = TopicComponent.GetTopicList("OverStockOPackingSlipTopic", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    if (dsTopic.Tables[0].Rows[0]["Description"].ToString() == "")
                    {
                        ltrOverstockInstruction.Text = "";
                    }
                    else
                    {
                        ltrOverstockInstruction.Text = dsTopic.Tables[0].Rows[0]["Description"].ToString();
                    }
                    dsTopic.Dispose();
                }
            }
        }
        /// <summary>
        /// Bind Order Cart By Order Number
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <param name="dsOrderdata">DataSet dsOrderdata</param>
        private void BindCart(Int32 OrderNumber, DataSet dsOrderdata)
        {
            OrderComponent objOrder = new OrderComponent();
            DataSet dsCart = new DataSet();
            dsCart = objOrder.GetInvoiceProductsWithMarryproduct(OrderNumber);
            DataSet dsPreferred = new DataSet();

            //string strWarehouse = "";
            //string strWarehouse1 = "";
            //string strWarehouseProduct = "";
            //Int32 iWare = 0;


            ltrCart.Text = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\" class=\"datatable\" style=\"border-collapse: collapse;\">";
            ltrCart.Text += "<tr style=\"line-height: 50px; background-color: rgb(242,242,242);\">";

            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 10%;text-align: center;\">";
            ltrCart.Text += "<b>Quantity Ordered</b>";
            ltrCart.Text += "</th>";

            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 15%; text-align: center;\">";
            ltrCart.Text += "<b>Item Number</b>";
            ltrCart.Text += "</th>";

            ltrCart.Text += "<th valign=\"middle\" align=\"left\" style=\"width: 50%\">";
            ltrCart.Text += "<b>Description</b>";
            ltrCart.Text += "</th>";

            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 10%;text-align: center;\">";
            ltrCart.Text += "<b>Quantity Shipped</b>";
            ltrCart.Text += "</th>";

            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 15%; text-align: left;\">";
            ltrCart.Text += "<b>Vendor Sku</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "<th valign=\"middle\" align=\"left\" style=\"width: 15%; text-align: left;\">";
            ltrCart.Text += "<b>Ware House</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "</tr>";
            if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dsCart.Tables[0].Rows.Count; i++)
                {
                    bool titem = false;
                    if (Request.QueryString["Pid"] != null)
                    {
                        string strpids = "~" + Request.QueryString["Pid"].ToString();
                        if (strpids.IndexOf("~" + dsCart.Tables[0].Rows[i]["ProductID"].ToString() + "~") > -1)
                        {
                            titem = true;
                        }
                    }
                    else
                    {
                        titem = true;
                    }
                    if (titem == true)
                    {
                        ltrCart.Text += "<tr>";

                        ltrCart.Text += "<td style=\"text-align: center;\">";
                        ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                        ltrCart.Text += "</td>";

                        ltrCart.Text += "<td style=\"text-align: center;\">";
                        ltrCart.Text += dsCart.Tables[0].Rows[i]["OptionSku"].ToString();
                        ltrCart.Text += "</td>";

                        ltrCart.Text += "<td valign=\"top\" align=\"left\">";
                        ltrCart.Text += dsCart.Tables[0].Rows[i]["ProductName"].ToString() + "<br/>";
                        string sku = "";
                        string[] variantName = dsCart.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] variantValue = dsCart.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < variantValue.Length; j++)
                        {
                            if (variantName.Length > j)
                            {
                                ltrCart.Text += variantName[j].ToString() + " : " + variantValue[j].ToString() + "<br />";
                                SQLAccess objSql = new SQLAccess();
                                DataSet dsoption = new DataSet();
                                dsoption = objSql.GetDs("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + dsCart.Tables[0].Rows[i]["productId"] + " AND VariantValue='" + variantValue[j].ToString().Replace("'", "''") + "'");
                                if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["SKU"].ToString()))
                                    {
                                        sku += "<br/>" + dsoption.Tables[0].Rows[0]["SKU"].ToString();
                                    }
                                }
                            }
                        }
                        ltrCart.Text += "</td>";
                        Int32 shippedqty = 0;
                        ltrCart.Text += "<td style=\"text-align: center;\">";
                        shippedqty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(shippedqty,0) as shippedqty from tb_OrderShippedItems Where OrderNumber=" + OrderNumber + " and RefProductID = " + dsCart.Tables[0].Rows[i]["productId"].ToString() + ""));
                        if (shippedqty > 0)
                        {
                            ltrCart.Text += shippedqty;
                        }
                        else
                        {
                            ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                        }
                        ltrCart.Text += "</td>";

                        ltrCart.Text += "<td style=\"text-align: left;\">";
                        if (!string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["MerchantSKU"].ToString()))
                        {
                            ltrCart.Text += dsCart.Tables[0].Rows[i]["MerchantSKU"].ToString();
                        }
                        else
                        {
                            ltrCart.Text += dsCart.Tables[0].Rows[i]["SKU"].ToString();
                        }
                        ltrCart.Text += "</td>";

                        ltrCart.Text += "<td style=\"text-align: left;\">";
                        if (Request.QueryString["WareHouseId"] != null)
                        {
                            ltrCart.Text += Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(Name,'') FROM tb_WareHouse WHERE WareHouseID=" + Request.QueryString["WareHouseId"].ToString() + ""));
                        }
                        ltrCart.Text += "</td>";

                        ltrCart.Text += "</tr>";
                    }
                }

            }
            ltrCart.Text += "</table>";
        }

    }
}