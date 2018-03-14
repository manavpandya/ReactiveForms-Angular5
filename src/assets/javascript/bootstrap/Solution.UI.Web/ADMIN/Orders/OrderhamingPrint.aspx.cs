using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class OrderhamingPrint : System.Web.UI.Page
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
                BindInvoiceSignature();
                if (Request.QueryString["ONo"] != null)
                {
                    int OrderNumber = 0;
                    OrderNumber = Convert.ToInt32(Request.QueryString["ONo"].ToString());

                    ltrorderNumber.Text = OrderNumber.ToString();
                    GetOrderDetails(OrderNumber);
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
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["RefOrderID"].ToString()))
                {
                    ltrorderNumber.Text += " (Ref. Order No. : " + objDsorder.Tables[0].Rows[0]["RefOrderID"].ToString() + ")";
                }
                ltrOrderdate.Text = objDsorder.Tables[0].Rows[0]["OrderDate"].ToString();

                try
                {
                    //ImgStoreLogo.Src = AppLogic.AppConfigs("LIVE_SERVER") + "/Images/Store_" + Convert.ToString(objDsorder.Tables[0].Rows[0]["StoreID"]) + ".png";
                    ImgStoreLogo.Src = "/Images/Store_" + Convert.ToString(objDsorder.Tables[0].Rows[0]["StoreID"]) + ".png";
                }
                catch (Exception ex)
                {
                    ImgStoreLogo.Src = AppLogic.AppConfigs("LIVE_SERVER") + "/Images/Logo.png";
                }


                ltrName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["LastName"].ToString());
                ltrName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingLastName"].ToString());

                ltrAddress.Text = "";
                ltrAddress.Text += "<table width=\"100%\" class=\"popup_cantain\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\">";
                ltrAddress.Text += "<tbody>";
                ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "<b>Account</b>";
                //ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td width=\"50%\">";
                ltrAddress.Text += "<b>Billing Address</b>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "<b>Shipping Address</b>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Name:";
                //ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td class=\"font-bold\">";
                ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingLastName"].ToString()); ;
                ltrAddress.Text += "</td>";



                ltrAddress.Text += "<td class=\"font-bold\">";
                ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString()); ;
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Company:";
                //ltrAddress.Text += "</td>";

                //else
                //{
                //    ltrAddress.Text += "-";
                //}


                ltrAddress.Text += "</tr>";

                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Address1:";
                //ltrAddress.Text += "</td>";



                String BillingSuite = "";
                String BillingAddress2 = "";
                String BillingCity = "";
                String BillingZip = "";
                String BillingAddress1 = "";
                String BillingState = "";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingSuite"].ToString()))
                {
                    BillingSuite = " #" + objDsorder.Tables[0].Rows[0]["BillingSuite"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                {
                    BillingAddress2 = " " + objDsorder.Tables[0].Rows[0]["BillingAddress2"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCity"].ToString()))
                {
                    BillingCity = " " + objDsorder.Tables[0].Rows[0]["BillingCity"].ToString() + ",";
                }

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingZip"].ToString()))
                {
                    BillingZip = " " + objDsorder.Tables[0].Rows[0]["BillingZip"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingAddress1"].ToString()))
                {
                    BillingAddress1 = " " + objDsorder.Tables[0].Rows[0]["BillingAddress1"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingState"].ToString()))
                {
                    BillingState = " " + objDsorder.Tables[0].Rows[0]["BillingState"].ToString() + ",";
                }


                String ShippingSuite = "";
                String ShippingAddress2 = "";
                String ShippingCity = "";
                String ShippingZip = "";
                String ShippingAddress1 = "";
                String ShippingState = "";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                {
                    ShippingSuite = " #" + objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                {
                    ShippingAddress2 = " " + objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString()))
                {
                    ShippingCity = " " + objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString() + ",";
                }

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                {
                    ShippingZip = " " + objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                {
                    ShippingAddress1 = " " + objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingState"].ToString()))
                {
                    ShippingState = " " + objDsorder.Tables[0].Rows[0]["ShippingState"].ToString() + ",";
                }

                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td valign='Top' colspan=2>";
                ltrAddress.Text += "<table width=\"100%\" class=\"popup_cantain\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\"><tr><td valign='Top'>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCompany"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingCompany"].ToString() + "<br/>";
                if (BillingAddress1.ToString() != "")
                    ltrAddress.Text += BillingAddress1.ToString() + "<br/>";
                if (BillingAddress2.ToString() != "")
                    ltrAddress.Text += BillingAddress2.ToString() + "<br/>";
                if (BillingSuite.ToString() != "")
                    ltrAddress.Text += BillingSuite.ToString() + "<br/>";
                if (BillingCity.ToString() != "")
                    ltrAddress.Text += BillingCity.ToString() + "<br/>";
                if (BillingState.ToString() != "")
                    ltrAddress.Text += BillingState.ToString() + "<br/>";
                if (BillingZip.ToString() != "")
                    ltrAddress.Text += BillingZip.ToString() + "<br/>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()))
                {
                    if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()) + ", <br/>";
                    }
                    else
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()) + "<br/>";
                    }
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString();
                ltrAddress.Text += "</td>";



                ltrAddress.Text += "<td valign='Top'>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br/>";
                if (ShippingAddress1.ToString() != "")
                    ltrAddress.Text += ShippingAddress1.ToString() + "<br/>";
                if (ShippingAddress2.ToString() != "")
                    ltrAddress.Text += ShippingAddress2.ToString() + "<br/>";
                if (ShippingSuite.ToString() != "")
                    ltrAddress.Text += ShippingSuite.ToString() + "<br/>";
                if (ShippingCity.ToString() != "")
                    ltrAddress.Text += ShippingCity.ToString() + "<br/>";
                if (ShippingState.ToString() != "")
                    ltrAddress.Text += ShippingState.ToString() + "<br/>";
                if (ShippingZip.ToString() != "")
                    ltrAddress.Text += ShippingZip.ToString() + "<br/>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                {
                    if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()) + ", <br/>";
                    }
                    else
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()) + "<br/>";
                    }
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString();
                ltrAddress.Text += "</td></tr></table>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";


                #region Old Order Billing Shipping Detail

                #endregion

                ltrAddress.Text += "</tbody>";
                ltrAddress.Text += "</table>";

                BindCart(Convert.ToInt32(objDsorder.Tables[0].Rows[0]["ShoppingCardID"].ToString()), objDsorder);
            }

        }

        /// <summary>
        /// Bind Order Cart By Order Number
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        private void BindCart(Int32 OrderNumber, DataSet dsOrderdata)
        {

            OrderComponent objOrder = new OrderComponent();

            DataSet dsCart = new DataSet();
            //  dsCart = objOrder.GetProductList(OrderNumber);

            dsCart = CommonComponent.GetCommonDataSet(@"SELECT     dbo.tb_OrderedShoppingCartItems.ProductName, dbo.tb_OrderedShoppingCartItems.SKU,dbo.tb_OrderedShoppingCartItems.SKUupgrade,dbo.tb_OrderedShoppingCartItems.VariantNames, 
                      dbo.tb_OrderedShoppingCartItems.VariantValues, dbo.tb_OrderHaming.Quantity, dbo.tb_OrderHaming.HamingQty, 
                      dbo.tb_OrderHaming.HamingName
                      FROM dbo.tb_OrderedShoppingCartItems INNER JOIN
                      dbo.tb_OrderHaming ON dbo.tb_OrderedShoppingCartItems.OrderedCustomCartID = dbo.tb_OrderHaming.OrderedCustomCartID WHERE dbo.tb_OrderHaming.OrderNumber=" + Request.QueryString["ONo"].ToString() + @""); //objOrder.GetInvoiceProductList(OrderNumber);
            ltrCart.Text = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\" class=\"pop_border_new2\" style=\"border-collapse: collapse;\">";
            //ltrCart.Text += "<tr style=\"line-height: 50px; background-color: rgb(242,242,242);\">";
            ltrCart.Text += "<tr style=\"line-height: 30px; background: none repeat scroll 0 0 #CFCFCF;\">";
            ltrCart.Text += "<td valign=\"middle\" align=\"left\" style=\"width: 55%\">";
            ltrCart.Text += "<b>Product</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "<td valign=\"middle\" align=\"center\" style=\"width: 10%; text-align: center; background: none repeat scroll 0 0 #CFCFCF;\">";
            ltrCart.Text += "<b>SKU</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "<td valign=\"top\" align=\"center\" style=\"width: 10%; text-align: center; background: none repeat scroll 0 0 #CFCFCF;\">";
            ltrCart.Text += "<b>Upgrade SKU</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "<td valign=\"middle\" align=\"center\" style=\"text-align: center; background: none repeat scroll 0 0 #CFCFCF;\">";
            ltrCart.Text += "<b>Quantity</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "<td valign=\"middle\" align=\"center\" style=\"text-align: center; background: none repeat scroll 0 0 #CFCFCF;\">";
            ltrCart.Text += "<b>Hemming Quantity</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "<td valign=\"middle\" align=\"center\" style=\"text-align: center; background: none repeat scroll 0 0 #CFCFCF;\">";
            ltrCart.Text += "<b>Hemming Option</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "</tr>";

            if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsCart.Tables[0].Rows.Count; i++)
                {
                    ltrCart.Text += "<tr style=\"line-height: 30px;\">";
                    ltrCart.Text += "<td valign=\"middle\" align=\"left\">";
                    ltrCart.Text += dsCart.Tables[0].Rows[i]["ProductName"].ToString();
                    string[] strnm = dsCart.Tables[0].Rows[i]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] strval = dsCart.Tables[0].Rows[i]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (strnm.Length > 0)
                    {
                        for (int k = 0; k < strnm.Length; k++)
                        {
                            ltrCart.Text = ltrCart.Text.ToString() + "<br />" + strnm[k].ToString() + ":" + strval[k].ToString();
                        }
                    }
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td valign=\"middle\" align=\"center\">" + dsCart.Tables[0].Rows[i]["SKU"].ToString();
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td valign=\"middle\" align=\"center\">" + dsCart.Tables[0].Rows[i]["SKUupgrade"].ToString();
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td valign=\"middle\" align=\"center\">" + dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td valign=\"middle\" align=\"center\">" + dsCart.Tables[0].Rows[i]["HamingQty"].ToString();
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td valign=\"middle\" align=\"center\">" + dsCart.Tables[0].Rows[i]["HamingName"].ToString();
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                }
            }
            ltrCart.Text += "</table>";

        }

        /// <summary>
        /// Binds the Invoice Signature.
        /// </summary>
        private void BindInvoiceSignature()
        {
            DataSet dsTopic = new DataSet();
            dsTopic = TopicComponent.GetTopicList("InvoiceSignature", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["Description"].ToString()))
                {
                    ltInvoiceSignature.Text = dsTopic.Tables[0].Rows[0]["Description"].ToString();
                }
                else
                {
                    ltInvoiceSignature.Text = "";
                }

                dsTopic.Dispose();
            }
            else
            {
                dsTopic = CommonComponent.GetCommonDataSet("SELECT 'Thank You,<br>'+StoreName as InvoiceSignature FROM dbo.tb_Store WHERE StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["InvoiceSignature"].ToString()))
                    {
                        ltInvoiceSignature.Text = dsTopic.Tables[0].Rows[0]["InvoiceSignature"].ToString();
                    }
                    else
                    {
                        ltInvoiceSignature.Text = "";
                    }
                    dsTopic.Dispose();
                }
            }
        }
    }
}