using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class BulkPickingSlip : BasePage
    {
        string strResponse1 = string.Empty;
        string strResponseFinal = string.Empty;
        string strResponse = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOrders();
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgauto", "window.parent.document.getElementById('ContentPlaceHolder1_frmPickingSlip').height = document['body'].offsetHeight;", true);
                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgauto1", "window.parent.document.getElementById('ContentPlaceHolder1_frmPickingSlip').height = window.parent.document.getElementById('ContentPlaceHolder1_frmPickingSlip').contentWindow.document.body.scrollHeight +'px';", true);//document['body'].offsetHeight;
                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgauto1", "window.parent.document.getElementById('ContentPlaceHolder1_frmPickingSlip').height = window.parent.document.getElementById('ContentPlaceHolder1_frmPickingSlip').contentDocument.scrollHeight;", true);
            }

        }



        protected void BindOrders()
        {

            try
            {
                //System.IO.StreamReader objStream = new System.IO.StreamReader(Server.MapPath("/Admin/Orders/PrintSlip.htm"));
                //strResponse = objStream.ReadToEnd();
                //strResponse1 = strResponse;
                //objStream.Close();
                strResponse1 = ltBultprint.InnerHtml.ToString();
                strResponse = strResponse1;
            }
            catch (Exception ex)
            {

            }
            OrderComponent objOrder = new OrderComponent();
            DataSet dsOrder = new DataSet();
            dsOrder = objOrder.GetOrderDetailsByOrderID(Convert.ToInt32(Request.QueryString["ONo"].ToString()));
            strResponse1 = strResponse;
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {

                string ONo = dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString();
                int StoreID = 0;
                Int32.TryParse(dsOrder.Tables[0].Rows[0]["StoreID"].ToString(), out StoreID);

                if (!System.IO.File.Exists(Server.MapPath("/Resources/Barcodes/ONO-" + ONo + ".png")))
                {
                    DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                    bCodeControl.BarCode = "ONO-" + ONo;
                    bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                    bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                    bCodeControl.BarCodeHeight = 80;
                    bCodeControl.ShowHeader = false;
                    bCodeControl.ShowFooter = true;
                    bCodeControl.Size = new System.Drawing.Size(250, 150);
                    bCodeControl.SaveImage(Server.MapPath("/Resources/Barcodes/ONo-" + ONo + ".png"));
                }
                string imgesrc = "<img  src=\"/Resources/Barcodes/ONo-" + ONo + ".png\" height=\"150\" style='vertical-align: top; padding-top: 10px; width: 250px;'>";
                strResponse1 = strResponse1.Replace("###src###", imgesrc.ToString());
                strResponse1 = strResponse1.Replace("###logo###", "<img src='" + AppLogic.AppConfigs("Live_Server").ToString() + "/images/Store_" + StoreID.ToString() + ".png' />");
                string strReforderid = "";
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["RefOrderID"].ToString()))
                {
                    strReforderid = " (Ref. No. " + dsOrder.Tables[0].Rows[0]["RefOrderID"].ToString() + ")";
                }

                strResponse1 = strResponse1.Replace("###lblDate###", dsOrder.Tables[0].Rows[0]["OrderDate"].ToString());
                strResponse1 = strResponse1.Replace("###lblOrderId###", dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString() + strReforderid);
                PaymentComponent objpay = new PaymentComponent();
                strResponse1 = strResponse1.Replace("###lblPaymentMethod###", dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString());

                if (dsOrder.Tables[0].Rows[0]["PaymentGateway"].ToString().ToLower().Contains("google checkout"))
                {
                    strResponse1 = strResponse1.Replace("###lblCardType###", "Google Checkout");
                    strResponse1 = strResponse1.Replace("###lblName###", "N/A");
                    strResponse1 = strResponse1.Replace("###lblCardNo###", "N/A");
                }
                else if (dsOrder.Tables[0].Rows[0]["PaymentGateway"].ToString().ToLower().Contains("paypal"))
                {
                    strResponse1 = strResponse1.Replace("###lblCardType###", "Paypal Pro");
                    strResponse1 = strResponse1.Replace("###lblName###", "N/A");
                    strResponse1 = strResponse1.Replace("###lblCardNo###", "N/A");
                }

                else if (dsOrder.Tables[0].Rows[0]["PaymentGateway"].ToString().ToLower().Contains("paypalexpress"))
                {
                    strResponse1 = strResponse1.Replace("###lblCardType###", "Paypal Express");
                    strResponse1 = strResponse1.Replace("###lblName###", "N/A");
                    strResponse1 = strResponse1.Replace("###lblCardNo###", "N/A");
                }

                else
                {
                    string name = "";
                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["CardName"].ToString().Trim()))
                        name = dsOrder.Tables[0].Rows[0]["CardName"].ToString();
                    else
                        name = dsOrder.Tables[0].Rows[0]["BillingFirstName"].ToString() + " " + dsOrder.Tables[0].Rows[0]["BillingLastName"].ToString();
                    strResponse1 = strResponse1.Replace("###lblCardType###", dsOrder.Tables[0].Rows[0]["CardType"].ToString());
                    strResponse1 = strResponse1.Replace("###lblName###", name.ToString());
                    string Last4 = "";
                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString()))
                    {
                        string CardNumber = SecurityComponent.Decrypt(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString());
                        if (CardNumber.Length > 8)
                        {
                            Last4 = CardNumber.Substring(CardNumber.Length - 4, 4);
                        }
                    }

                    strResponse1 = strResponse1.Replace("###lblCardNo###", "************" + Last4.ToString());
                }
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["Notes"].ToString()))
                {
                    strResponse1 = strResponse1.Replace("###Notes####", dsOrder.Tables[0].Rows[0]["Notes"].ToString());
                }
                else
                {
                    strResponse1 = strResponse1.Replace("###Notes####", "N/A");
                }

                string st_b = "";
                st_b = "<table  align='left' cellpadding='0' cellspacing='0' width='45%'>";

                st_b += "<tr>";

                st_b += "<td style='padding-top:0px;font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;color:#000000'>";
                st_b += "<b>Bill To:</b> ";
                st_b += "</td>";
                st_b += "</tr>";
                st_b += "<tr>";

                st_b += "<td style='padding-top:0px;' class='billaddtext'>" + dsOrder.Tables[0].Rows[0]["BillingFirstName"].ToString() + " " + dsOrder.Tables[0].Rows[0]["BillingLastName"].ToString() + "</td></tr>";



                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingCompany"].ToString()))
                {
                    st_b += "<tr>";

                    st_b += "<td class='billaddtext'>";

                    st_b += dsOrder.Tables[0].Rows[0]["BillingCompany"].ToString();
                    st_b += "</td>";
                    st_b += "</tr>";

                }

                st_b += "<tr>";

                st_b += "<td class='billaddtext'>";
                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString()))
                    st_b += dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString();
                st_b += "</td>";
                st_b += "</tr>";

                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                {
                    st_b += "<tr>";

                    st_b += "<td class='billaddtext'>";

                    st_b += dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString();
                    st_b += "</td>";
                    st_b += "</tr>";
                }
                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingSuite"].ToString()))
                {
                    st_b += "<tr>";

                    st_b += "<td class='billaddtext'>";

                    st_b += dsOrder.Tables[0].Rows[0]["BillingSuite"].ToString();
                    st_b += "</td>";
                    st_b += "</tr>";
                }
                st_b += "<tr>";

                st_b += "<td class='billaddtext'>";
                st_b += dsOrder.Tables[0].Rows[0]["BillingCity"].ToString() + ", ";
                st_b += dsOrder.Tables[0].Rows[0]["BillingState"].ToString() + " ";
                st_b += dsOrder.Tables[0].Rows[0]["BillingZip"].ToString();
                st_b += "</td>";
                st_b += "</tr>";
                st_b += "<tr>";

                st_b += "<td class='billaddtext'>";
                st_b += dsOrder.Tables[0].Rows[0]["BillingCountry"].ToString();
                st_b += "</td>";
                st_b += "</tr>";

                st_b += "<tr>";

                st_b += "<td class='billaddtext'>";
                st_b += dsOrder.Tables[0].Rows[0]["BillingPhone"].ToString();
                st_b += "</td>";

                st_b += "</tr>";

                st_b += "<tr>";

                st_b += "<td class='billaddtext'>";
                st_b += dsOrder.Tables[0].Rows[0]["Email"].ToString();
                st_b += "</td>";
                st_b += "</tr>";
                st_b += "</table>";

                st_b += "<table align='left' cellpadding='0'cellspacing='0' width='50%' style='padding-left:10px'>";

                st_b += "<tr>";
                st_b += "<td style='padding-top:0px;font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;color:#000000'>";
                st_b += "<b>Ship To:</b> ";
                st_b += "</td>";
                st_b += "</tr>";

                st_b += "<tr>";


                st_b += "<td class='billaddtext' style='padding-top:0px;'>" + dsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + dsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString() + "</td></tr>";


                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                {
                    st_b += "<tr>";
                    //st_b += "<td>";
                    //st_b += "Suite: ";
                    //st_b += "</td>";
                    st_b += "<td class='billaddtext'>";

                    st_b += dsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString();
                    st_b += "</td>";
                    st_b += "</tr>";

                }

                st_b += "<tr>";
                //st_b += "<td>";
                //st_b += "Suite: ";
                //st_b += "</td>";
                st_b += "<td class='billaddtext'>";
                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                    st_b += dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString();
                st_b += "</td>";
                st_b += "</tr>";
                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                {
                    st_b += "<tr>";
                    //st_b += "<td>";
                    //st_b += "Suite: ";
                    //st_b += "</td>";
                    st_b += "<td class='billaddtext'>";

                    st_b += dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString();
                    st_b += "</td>";
                    st_b += "</tr>";
                }
                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                {
                    st_b += "<tr>";
                    //st_b += "<td>";
                    //st_b += "Suite: ";
                    //st_b += "</td>";
                    st_b += "<td class='billaddtext'>";

                    st_b += dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString();
                    st_b += "</td>";
                    st_b += "</tr>";
                }

                st_b += "<tr>";
                //st_b += "<td>";
                //st_b += "City: ";
                //st_b += "</td>";
                st_b += "<td class='billaddtext'>";
                st_b += dsOrder.Tables[0].Rows[0]["ShippingCity"].ToString() + ", ";
                st_b += dsOrder.Tables[0].Rows[0]["ShippingState"].ToString() + " ";
                st_b += dsOrder.Tables[0].Rows[0]["ShippingZip"].ToString();

                st_b += "</td>";

                st_b += "</tr>";
                st_b += "<tr>";
                //st_b += "<td>";
                //st_b += "State: ";
                //st_b += "</td>";

                st_b += "<td class='billaddtext'>";
                st_b += dsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString();
                st_b += "</td>";
                st_b += "</tr>";

                st_b += "<tr>";
                //st_b += "<td>";
                //st_b += "Phone: ";
                //st_b += "</td>";

                st_b += "<td class='billaddtext'>";
                st_b += dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString();
                st_b += "</td>";
                st_b += "</tr>";

                st_b += "<tr>";
                //st_b += "<td>";
                //st_b += "E-Mail: ";
                //st_b += "</td>";
                st_b += "<td class='billaddtext'>";
                st_b += dsOrder.Tables[0].Rows[0]["ShippingEmail"].ToString();
                st_b += "</td>";
                st_b += "</tr>";


                st_b += "</table>";
                // strResponse1 = strResponse1.Replace("###ltBilling###", st_b.ToString());
                strResponse1 = strResponse1.Replace("###ltBilling###", st_b.ToString());

                DataSet DsCItems = new DataSet();
                DataSet dsInsert = new DataSet();
                DsCItems = objOrder.GetShippedItemsforOrderPOPicking(Convert.ToInt32(dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()), StoreID);

                System.Text.StringBuilder Table = new System.Text.StringBuilder();
                Table.Append(" <table border='0' cellpadding='5' align='center' cellspacing='0' width='600px' style='border:1px solid #cccccc;'> ");
                Table.Append("<tbody><tr  bgcolor=\"#CCCCCC\">");
                Table.Append("<th align='center' valign='middle' style='width:10%;text-align:left;border:solid 1px #cccccc;background-color:#cccccc' ><span class=\"style1\"> SKU</span></th>");
                Table.Append("<th align='left' valign='middle' style='width:45%;border:solid 1px #cccccc;background-color:#cccccc'><span class=\"style1\">Product Name</span></th>");
                Table.Append("<th align='center' valign='middle' style='text-align:center;border:solid 1px #cccccc;background-color:#cccccc' ><span class=\"style1\"> Quantity</span></th>");
                Table.Append("<th align='center' valign='middle' style='text-align:center;border:solid 1px #cccccc;background-color:#cccccc' ><span class=\"style1\"> Status</span></th>");
                //Table.Append("<th align='center' valign='middle' style='text-align:center;border:solid 1px #cccccc;background-color:#cccccc' ><span class=\"style1\"> Location</span></th>");
                //Table.Append("<th align='center' valign='middle' style='text-align:center;border:solid 1px #cccccc;background-color:#cccccc;###display###' ><span class=\"style1\">UPC</span></th>");
                Table.Append("</tr>");
                bool flagCart = false;

                for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                {
                    if (DsCItems.Tables[0].Rows[i]["type"].ToString().ToLower() == "upgrade")
                    {
                        for (int j = 0; j < DsCItems.Tables[0].Rows.Count; j++)
                        {
                            if (DsCItems.Tables[0].Rows[j]["ordercustomcartId"].ToString() == DsCItems.Tables[0].Rows[i]["ordercustomcartId"].ToString() && DsCItems.Tables[0].Rows[j]["type"].ToString().ToLower() == "main")
                            {
                                DsCItems.Tables[0].Rows[j]["Quantity"] = Convert.ToInt32(DsCItems.Tables[0].Rows[j]["Quantity"]) - Convert.ToInt32(DsCItems.Tables[0].Rows[i]["Quantity"].ToString());
                                DsCItems.Tables[0].Rows[j]["wtype"] = "Drop Ship";
                                break;
                            }

                        }
                    }
                    if (DsCItems.Tables[0].Rows[i]["type"].ToString().ToLower() == "lock")
                    {
                        for (int j = 0; j < DsCItems.Tables[0].Rows.Count; j++)
                        {
                            if (DsCItems.Tables[0].Rows[j]["ordercustomcartId"].ToString() == DsCItems.Tables[0].Rows[i]["ordercustomcartId"].ToString() && DsCItems.Tables[0].Rows[j]["type"].ToString().ToLower() == "main")
                            {
                                DsCItems.Tables[0].Rows[j]["Quantity"] = Convert.ToInt32(DsCItems.Tables[0].Rows[j]["Quantity"]) - Convert.ToInt32(DsCItems.Tables[0].Rows[i]["Quantity"].ToString());
                                DsCItems.Tables[0].Rows[j]["wtype"] = "Drop Ship";
                                break;
                            }

                        }
                    }


                }
                DsCItems.Tables[0].DefaultView.Sort = "wtype desc";
                DataTable dtPickingSorted = DsCItems.Tables[0].DefaultView.ToTable();

                bool checkcart = true;
                bool makesleep = true;
                for (int i = 0; i < dtPickingSorted.Rows.Count; i++)
                {
                    flagCart = true;
                    checkcart = true;
                    makesleep = true;
                    string sku = "";
                    if (Convert.ToInt32(dtPickingSorted.Rows[i]["Quantity"].ToString()) > 0)
                    {

                        Table.Append("<tr height=\"24px\">");
                        Table.Append("<td class='style3'  align='left' style='font-weight:normal;border:solid 1px #cccccc;font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;font-size:12px'  valign='top'>###SKU###</td>");
                        Table.Append("<td class='style3' align='left' style='font-weight:normal;border:solid 1px #cccccc;font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;font-size:12px' valign='top'>" + dtPickingSorted.Rows[i]["ProductName"].ToString() + "");
                        try
                        {
                            string[] names = dtPickingSorted.Rows[i]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] values = dtPickingSorted.Rows[i]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            if (names.Length == values.Length)
                            {
                                for (int iLoopValues = 0; iLoopValues < values.Length && names.Length == values.Length; iLoopValues++)
                                {
                                    Table.Append("<br/>" + names[iLoopValues] + ": " + values[iLoopValues]);

                                    SQLAccess objSql = new SQLAccess();
                                    DataSet dsoption = new DataSet();
                                    dsoption = objSql.GetDs("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + dtPickingSorted.Rows[i]["productId"].ToString() + " AND VariantValue='" + values[iLoopValues].ToString().Replace("'", "''") + "'");
                                    if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["SKU"].ToString()))
                                        {
                                            sku += "<br/>" + dsoption.Tables[0].Rows[0]["SKU"].ToString();
                                        }
                                        if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["UPC"].ToString()))
                                        {

                                            String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                                            CreateFolder(FPath.ToString());
                                            if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                            {
                                                sku += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                            }
                                            else
                                            {
                                                if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                                {
                                                    try
                                                    {
                                                        DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                                                        bCodeControl.BarCode = dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                                        bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                                                        bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                                                        bCodeControl.BarCodeHeight = 70;
                                                        bCodeControl.ShowHeader = false;
                                                        bCodeControl.ShowFooter = true;
                                                        bCodeControl.FooterText = "UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                                        bCodeControl.Size = new System.Drawing.Size(250, 100);
                                                        bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png"));
                                                    }
                                                    catch
                                                    {

                                                    }
                                                    if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                                    {
                                                        sku += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                                    }
                                                }
                                            }

                                        }
                                    }

                                }
                            }
                            else
                            {
                                for (int iLoopValues = 0; iLoopValues < values.Length; iLoopValues++)
                                {
                                    Table.Append("<br/>  - " + values[iLoopValues]);

                                    SQLAccess objSql = new SQLAccess();
                                    DataSet dsoption = new DataSet();
                                    dsoption = objSql.GetDs("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + dtPickingSorted.Rows[i]["productId"].ToString() + " AND VariantValue='" + values[iLoopValues].ToString().Replace("'", "''") + "'");
                                    if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["SKU"].ToString()))
                                        {
                                            sku += "<br/>" + dsoption.Tables[0].Rows[0]["SKU"].ToString();
                                        }
                                        if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["UPC"].ToString()))
                                        {

                                            String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                                            CreateFolder(FPath.ToString());
                                            if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                            {
                                                sku += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                            }
                                            else
                                            {
                                                if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                                {
                                                    try
                                                    {
                                                        DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                                                        bCodeControl.BarCode = dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                                        bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                                                        bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                                                        bCodeControl.BarCodeHeight = 70;
                                                        bCodeControl.ShowHeader = false;
                                                        bCodeControl.ShowFooter = true;
                                                        bCodeControl.FooterText = "UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                                        bCodeControl.Size = new System.Drawing.Size(250, 100);
                                                        bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png"));
                                                    }
                                                    catch
                                                    {

                                                    }
                                                    if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                                    {
                                                        sku += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            if (sku != "")
                            {
                                Table = Table.Replace("###SKU###", sku);
                            }
                            else
                            {
                                Table = Table.Replace("###SKU###", dtPickingSorted.Rows[i]["SKU"].ToString());
                            }

                            if (!string.IsNullOrEmpty(dtPickingSorted.Rows[i]["ProductID"].ToString()))
                            {
                                string StrQuery = " SElect ProductAssemblyID,RefProductID, tb_ProductAssembly.ProductID,tb_product.name,tb_product.Sku,ISNULL(Quantity,0) as Quantity from tb_ProductAssembly " +
                                                  " inner join tb_product on tb_ProductAssembly.ProductID=tb_product.ProductID " +
                                                  " where RefProductID= " + dtPickingSorted.Rows[i]["ProductID"].ToString() + " and ISNULL(tb_product.Active,1)=1 and ISNULL(Deleted,0)=0";
                                DataSet dsAssamble = new DataSet();
                                dsAssamble = CommonComponent.GetCommonDataSet(StrQuery.ToString());
                                if (dsAssamble != null && dsAssamble.Tables.Count > 0 && dsAssamble.Tables[0].Rows.Count > 0)
                                {
                                    Table.Append("<div style=\"padding-left: 5px;\">");
                                    for (int k = 0; k < dsAssamble.Tables[0].Rows.Count; k++)
                                    {
                                        Table.Append(dsAssamble.Tables[0].Rows[k]["Name"].ToString() + " - Qty (" + dsAssamble.Tables[0].Rows[k]["Quantity"].ToString() + ")<br />");
                                    }
                                    Table.Append("<div/>");
                                }
                            }
                        }
                        catch
                        {
                        }
                        Table.Append("</td>");
                        Table.Append("<td class='style3' align='Center' style='font-weight:normal;border:solid 1px #cccccc;font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;font-size:12px' valign='top'>" + dtPickingSorted.Rows[i]["Quantity"].ToString() + "</td>");
                        Table.Append("<td class='style3' align='Center' style='font-weight:normal;border:solid 1px #cccccc;font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;font-size:12px' valign='top'>" + dtPickingSorted.Rows[i]["wtype"].ToString() + "</td>");
                        //if (dtPickingSorted.Rows[i]["Location"].ToString() == "")
                        //{
                        //    Table.Append("<td class='style3' align='Center' style='font-weight:normal;border:solid 1px #cccccc;font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;font-size:12px' valign='top'>&nbsp;&nbsp;</td> ");
                        //}
                        //else
                        //{
                        //    Table.Append("<td class='style3' align='Center' style='font-weight:normal;border:solid 1px #cccccc;font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;font-size:12px' valign='top'>" + dtPickingSorted.Rows[i]["Location"].ToString() + "</td> ");
                        //}

                        //    if (Convert.ToString(DsCItems.Tables[0].Rows[i]["upc"]) != "")
                        //    {

                        //        String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                        //        if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + Convert.ToString(DsCItems.Tables[0].Rows[i]["upc"]).Trim() + ".png")))
                        //        {
                        //            string strliveServer = Convert.ToString(AppLogic.AppConfigs("LIVE_SERVER"));
                        //            if (sku != "")
                        //            {
                        //                Table.Append("<td class='style3' align='Center' style='border:solid 1px #cccccc;###display###' valign='top'>&nbsp;&nbsp</td></tr>");
                        //            }
                        //            else
                        //            {
                        //                Table.Append("<td class='style3' align='Center' style='border:solid 1px #cccccc;###display###' valign='top'><img src=\"" + strliveServer + FPath + "/UPC-" + DsCItems.Tables[0].Rows[i]["upc"].ToString().Trim() + ".png\" border=\"0\" /></td></tr>");
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (sku != "")
                        //            {
                        //                Table.Append("<td class='style3' align='Center' style='border:solid 1px #cccccc;###display###' valign='top'>&nbsp;&nbsp</td></tr>");
                        //            }
                        //            else
                        //            {
                        //                Table.Append("<td class='style3' align='Center' style='border:solid 1px #cccccc;###display###' valign='top'>" + GenerateBarcode(Convert.ToString(DsCItems.Tables[0].Rows[i]["upc"]).Trim()) + "</td></tr>");
                        //            }

                        //        }
                        //    }
                        //    else
                        //    {
                        //        Table.Append("<td class='style3' align='Center' style='font-weight:normal;border:solid 1px #cccccc;###display###font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;font-size:12px' valign='top'>&nbsp;&nbsp;</td></tr>");
                        //    }
                        Table.Append("</tr>");
                    }


                    //if (sku != "")
                    //{
                    //    Table = Table.Replace("###display###", "display:none;");
                    //}
                    //else
                    //{
                    //    Table = Table.Replace("###display###", "");
                    //}

                }
                Table.Append("</tbody></table>");
                if (flagCart == true)
                {
                    strResponse1 = strResponse1.Replace("###ltCart###", Table.ToString());
                    strResponse1 = strResponse1.Replace("###lblMsg###", "");
                    idprint.InnerHtml = "<input class='myprint' type='image' src='/App_Themes/" + Page.Theme.ToString() + "/button/print.png' onclick='window.print();'/>";
                    //idprint1.InnerHtml = "<input class='myprint' type='image' src='/App_Themes/" + Page.Theme.ToString() + "/button/print.png' onclick='window.print();'/>";
                }
                else
                {
                    idprint.InnerHtml = "";
                    //idprint1.InnerHtml = "";
                    strResponse1 = strResponse1.Replace("###lblMsg###", "<font style=\"color:red\">Your cart is empty.</font>");
                    strResponse1 = strResponse1.Replace("###ltCart###", "");
                }
                strResponse1 = strResponse1.Replace("###Storename###", AppLogic.AppConfigs("Storename").ToString());

                strResponseFinal += strResponse1;
                ltBultprint.InnerHtml = "<div style='float:left;'>" + strResponseFinal.Substring(0, strResponseFinal.Length - 29) + "</div>";

                //Response.Clear();
                //Response.Write("<div style='float:left;padding-left:95px;'>" + strResponseFinal.Substring(0, strResponseFinal.Length - 29) + "</div><div style='float:left;'><input class='myprint' type='image' src='/Admin/images/print.gif' onclick='window.print();'/></div>");
                //Response.End();
            }
        }
        private void CreateFolder(String FPath)
        {
            if (!Directory.Exists(Server.MapPath(FPath.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(FPath.ToString()));
            }
        }

        private string GenerateBarcode(String UPCCode)
        {

            String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
            CreateFolder(FPath.ToString());
            string strliveServer = Convert.ToString(AppLogic.AppConfigs("LIVE_SERVER"));
            if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png")))
            {
                DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                bCodeControl.BarCode = UPCCode.Trim();
                bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                bCodeControl.BarCodeHeight = 70;
                bCodeControl.ShowHeader = false;
                bCodeControl.ShowFooter = true;
                bCodeControl.FooterText = "UPC-" + UPCCode.Trim();
                bCodeControl.Size = new System.Drawing.Size(250, 100);
                bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png"));
            }
            return "<img src=\"" + strliveServer + FPath + "/UPC-" + UPCCode.Trim() + ".png\" border=\"0\" />";
        }
    }
}