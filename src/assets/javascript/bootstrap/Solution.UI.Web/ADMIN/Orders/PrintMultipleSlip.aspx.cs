using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.IO;
using Solution.Data;
using System.Net;
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class PrintMultipleSlip : BasePage
    {
        String billing;
        String shipping;
        String link;
        DataSet dsOrder = null;
        String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string httpurl = Request.Url.ToString();
            if (httpurl.ToLower().StartsWith("https://"))
            {
                httpurl = httpurl.Replace("https", "http").ToString();
                Response.Redirect(httpurl);
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["ONo"] != null)
                {
                }
                else if (Session["PrintOrderId"] != null && Session["PrintOrderId"].ToString() != "")
                {
                    StringArrayConverter ConStr = new StringArrayConverter();
                    link = AppLogic.AppConfigs("STORENAME") + "<br /><a style='font-size:12px;color:#F2570A;' href='" + AppLogic.AppConfigs("LIVE_SERVER") + "'>" + AppLogic.AppConfigs("STOREPATH") + "</a>";
                    Array AryOrderId = (Array)ConStr.ConvertFrom(Session["PrintOrderId"]);
                    for (int i = 0; i < AryOrderId.Length; i++)
                    {
                        if (AryOrderId.GetValue(i).ToString() != "")
                        {
                            SlipOrder(Convert.ToInt32(AryOrderId.GetValue(i)));
                            if (i != AryOrderId.Length - 1)
                                litSlip.Text += "<br/><br/><br/>";
                        }
                    }
                }
                imgPrint.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print.png";
                imgPrintTop.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print.png";
            }
        }

        /// <summary>
        /// Gets the personal info for Customer.
        /// </summary>
        /// <param name="Ds">Dataset ds</param>
        public void GetPersonalInfo(DataSet Ds)
        {

            string st_b = null;
            string st_s = null;

            st_b += "<strong>" + Ds.Tables[0].Rows[0]["BillingFirstName"].ToString() + " " + Ds.Tables[0].Rows[0]["BillingLastName"].ToString() + "</strong><br/>";
            st_s += "<strong>" + Ds.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + Ds.Tables[0].Rows[0]["ShippingLastName"].ToString() + "</strong><br/>";

            if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[0]["BillingCompany"].ToString()))
                st_b += Ds.Tables[0].Rows[0]["BillingCompany"].ToString() + "<br/>";
            if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                st_s += Ds.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br/>";
            st_b += Ds.Tables[0].Rows[0]["BillingAddress1"].ToString() + "<br/>";
            st_s += Ds.Tables[0].Rows[0]["ShippingAddress1"].ToString() + "<br/>";
            if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                st_b += Ds.Tables[0].Rows[0]["BillingAddress2"].ToString() + "<br/>";
            if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                st_s += Ds.Tables[0].Rows[0]["ShippingAddress2"].ToString() + "<br/>";
            if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[0]["BillingSuite"].ToString()))
                st_b += Ds.Tables[0].Rows[0]["BillingSuite"].ToString() + "<br/>";
            if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                st_s += Ds.Tables[0].Rows[0]["ShippingSuite"].ToString() + "<br/>";
            st_b += Ds.Tables[0].Rows[0]["BillingCity"].ToString() + ", ";
            st_s += Ds.Tables[0].Rows[0]["ShippingCity"].ToString() + ", ";
            st_b += Ds.Tables[0].Rows[0]["BillingState"].ToString() + " ";
            st_s += Ds.Tables[0].Rows[0]["ShippingState"].ToString() + " ";
            st_b += Ds.Tables[0].Rows[0]["BillingZip"].ToString() + "<br/>";
            st_s += Ds.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br/>";
            st_b += (Ds.Tables[0].Rows[0]["BillingCountry"].ToString()) + "<br/>";
            st_s += (Ds.Tables[0].Rows[0]["ShippingCountry"].ToString()) + "<br/>";
            st_b += Ds.Tables[0].Rows[0]["BillingPhone"].ToString() + "<br/>";
            st_s += Ds.Tables[0].Rows[0]["ShippingPhone"].ToString() + "<br/>";
            billing = st_b.Trim();
            shipping = st_s.Trim();
        }

        /// <summary>
        /// Gets the micro image.
        /// </summary>
        /// <param name="img">string img</param>
        /// <returns>Returns the Image Path for display.</returns>
        public String GetMicroImage(String img)
        {
            String imagepath = String.Empty;
            String Temp = AppLogic.AppConfigs("ImagePathProduct") + "micro/" + img;
            imagepath = Temp;
            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }

            return AppLogic.AppConfigs("ImagePathProduct") + "micro/" + "image_not_available.jpg";
        }

        /// <summary>
        /// Add the cart item for display.
        /// </summary>
        /// <param name="ShoppingCardID">int ShoppingCardID</param>
        /// <param name="ONo">int Ono</param>
        /// <returns>Returns the output value as a string format which contains HTML.</returns>
        public string AddCartItem(int ShoppingCardID, int ONo)
        {
            StringBuilder Table = new StringBuilder();
            OrderComponent objOrder = new OrderComponent();
            DataSet DsCItems = new DataSet();
            DataSet dsCart = new DataSet();
            // DsCItems = objOrder.GetShippedItemsforOrderwithoutPOpackage(ONo);
            DsCItems = objOrder.GetInvoiceProductList(ShoppingCardID);
            if (DsCItems != null && DsCItems.Tables.Count > 0 && DsCItems.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                {
                    string Imagename = Convert.ToString(CommonComponent.GetScalarCommonData("SElect ISNULL(Imagename,'') as Imagename from tb_Product where ProductId=" + DsCItems.Tables[0].Rows[i]["RefProductID"].ToString() + " and ISNULL(Deleted,0)=0 and ISNULL(Active,1)=1"));
                    Table.Append("<tr>");
                    Table.Append("<td valign='top' align='left'>" + DsCItems.Tables[0].Rows[i]["ProductName"].ToString());
                    string[] Names = DsCItems.Tables[0].Rows[i]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] Values = DsCItems.Tables[0].Rows[i]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    int iLoopValues = 0;
                    string cart = "";
                    string sku = "";
                    for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                    {
                        cart += "<br/>" + Names[iLoopValues] + " " + Values[iLoopValues];

                        SQLAccess objSql = new SQLAccess();
                        DataSet dsoption = new DataSet();
                        dsoption = objSql.GetDs("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + DsCItems.Tables[0].Rows[i]["RefProductID"].ToString() + " AND VariantValue='" + Values[iLoopValues].ToString().Replace("'", "''") + "'");
                        if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["SKU"].ToString()))
                            {
                               // sku += "<br/>" + dsoption.Tables[0].Rows[0]["SKU"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["UPC"].ToString()))
                            {

                                String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                                CreateFolder(FPath.ToString());
                                if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                {
                                   // sku += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
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
                                           // sku += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                        }
                                    }
                                }

                            }
                        }
                    }
                    Table.Append(cart);
                    Table.Append("</td>");
                    //if (sku != "")
                    //{
                    //    Table.Append("<td style='text-align: center;'>" + sku.ToString() + "</td>");
                    //}
                    //else
                    //{
                        Table.Append("<td style='text-align: center;'>" + DsCItems.Tables[0].Rows[i]["SKU"].ToString() + "</td>");
                    //}
                    Table.Append("<td style='text-align: center;'>" + DsCItems.Tables[0].Rows[i]["Quantity"].ToString() + "</td>");
                    Table.Append("</tr>");
                }
            }
            else
            {
                Table.AppendLine("<font color='red' CLASS='font-red'>Your Shopping Cart is Empty.</font>");
            }

            return Table.ToString();
        }

        /// <summary>
        /// Display Order Slip
        /// </summary>
        /// <param name="OrderId">int OrderId</param>
        protected void SlipOrder(int OrderId)
        {
            OrderComponent objOrder = new OrderComponent();
            dsOrder = objOrder.GetOrderDetailsByOrderID(Convert.ToInt32(OrderId));
            if (dsOrder.Tables[0].Rows.Count > 0)
            {
                Int32 StoreID = 0;
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + Convert.ToInt32(OrderId))), out StoreID);
                AppConfig.StoreID = StoreID;
                string ImgPath = AppLogic.AppConfigs("LIVE_SERVER") + "/Images/Store_" + StoreID.ToString() + ".png";
                string Cart = AddCartItem(Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ShoppingCardID"].ToString()), OrderId);
                GetPersonalInfo(dsOrder);

                CreateFolder(FPath.ToString());
                if (!System.IO.File.Exists(Server.MapPath(FPath + "/ONo-" + dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString() + ".png")))
                {
                    DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                    bCodeControl.BarCode = dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString();
                    bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                    bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                    bCodeControl.BarCodeHeight = 80;
                    bCodeControl.ShowHeader = false;
                    bCodeControl.ShowFooter = true;
                    bCodeControl.FooterText = "ONo-" + dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString();
                    bCodeControl.Size = new System.Drawing.Size(250, 150);
                    bCodeControl.SaveImage(Server.MapPath(FPath + "/ONo-" + dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString() + ".png"));
                }
                string strReforderId = "";
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["RefOrderID"].ToString()))
                {
                    strReforderId = " (Ref. Order No. : " + dsOrder.Tables[0].Rows[0]["RefOrderID"].ToString() + ")";
                }
               // if (StoreID == 4)
                {
                    string url = "http://www.halfpricedrapes.us/Admin/Orders/PackingSlipMultiwarehouse.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()));
                    WebRequest NewWebReq = WebRequest.Create(url);
                    WebResponse newWebRes = NewWebReq.GetResponse();
                    string format = newWebRes.ContentType;
                    Stream ftprespstrm = newWebRes.GetResponseStream();
                    StreamReader reader;
                    reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
                    string strbiody = reader.ReadToEnd().ToString();
                   // litSlip.Text += "<div style='page-break-after: always;'>";
                    string[] stroverstock = Regex.Split(strbiody, "\"body12\"", RegexOptions.IgnoreCase);
                    if (stroverstock.Length > 0)
                    {
                        string strdata = stroverstock[1].ToString();
                        strdata = strdata.Substring(strdata.IndexOf("<table") - 6);
                        strdata = strdata.Substring(0, strdata.LastIndexOf("</table>") + 8);
                        strdata = strdata.Replace("class=\"Printinvoice\"", "style=\"display:none;\"");
                        litSlip.Text += strdata;
                    }


                    //litSlip.Text += "</div>";
                }
//                else
//                {
//                    litSlip.Text += @"<div style='page-break-after: always;'><table width='60%' cellspacing='0' cellpadding='0' align='center' class='form'>
//
//        <tr>
//                <td width='100%' align='center' class='receiptfont'>
//                <div align='center' class='bkground123'>
//                <table width='100%' cellspacing='0' cellpadding='0' align='center' class='signup-row'>
//                <tr>
//                    <td colspan='2' style='height: 14px'>
//                                            </td>
//                                    </tr>
//                <tr align='center' style='height: 85px'>
//                                        <td valign='middle' align='center' colspan='2' rowspan='1'>
//                                            <img border='0' class='img_left' alt='logo' id='ImgStoreLogo' src='" + ImgPath.ToString() + @"'>
//                                        </td>
//                                    </tr>
//                                    <tr>
//                                        <td valign='middle' align='center' colspan='2' rowspan='1'>
//                                        </td>
//                                    </tr>
//                    <tr>
//                            <td valign='middle' align='left' colspan='2' rowspan='1' style='height: 22px' class='receiptlineheight'>
//                                            Ship To:<br>
//                            <b>" + shipping +

//                                @"</b>
//                                            </td>
//                            </tr>
//                            <tr>
//                                                 <td valign='middle' align='center' colspan='2' rowspan='1' style='height: 20px'>
//                                                                    </td>
//                                    </tr>
//                            <tr>
//                                                        <td valign='middle' align='left' colspan='2' rowspan='1' style='height: 20px'>
//                                -----------------------------------------------------------------------------------------------------------------------------------------
//                                                                    </td>
//                                    </tr>
//            <tr>
//                <td align='left'>
//                                                  <table width='100%' cellspacing='0' cellpadding='0' border='0'>
//                                                                <tr>
//                                                                                    <td valign='middle' align='left' style='height: 20px'>
//                                                                                        Order Number:&nbsp; <b><span id='lblOrderId'>"
//                                                                                                + dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString() + strReforderId + @"</span></b>
//                                                                                    </td>
//                                                                                </tr>
//                                                                        <tr>
//                                                                                    <td valign='middle' align='left' rowspan='1' style='height: 20px'>
//                                                                                        Order Date:&nbsp; <b><span id='lblDate'>"
//                                                                                                + dsOrder.Tables[0].Rows[0]["OrderDate"].ToString() + @"</span></b>
//                                                                                    </td>
//                                                                                </tr>
//                                                                        <tr>
//                                                                                    <td valign='middle' align='left' rowspan='1' style='height: 20px'>
//                                                                                        <span id='lblDelMethod'>Shipping Method:</span> <b><span id='lblDeliveryMethod'>"
//                                                                                                + dsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString() + @"</span></b>
//                                                                                    </td>
//                                                                                </tr>
//                                                    </table> 
//                               </td>
//                        <td><img id='imgOrderBarcode' src='" + FPath + "/ONo-" + dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString() + ".png" + @"'></td>
//            </tr>
//                                <tr>
//                                        <td valign='middle' align='left' colspan='2' rowspan='1' style='height: 20px'>
//                                            Thank you for buying from "
//                                               + AppLogic.AppConfigs("StoreName").ToString() + @".
//                                        </td>
//                                    </tr>
//                            <tr>
//                                        <td valign='middle' align='left' colspan='2' rowspan='1'>                                            
//                                        </td>
//                                    </tr>
//                        </table>
//                        </div>
//                        </td>
//                    </tr>
//
//                        <tr>
//                                            <td align='center' style='height: 20px'></td>
//                        </tr>
//
//<tr><td width='100%' align='left' style='height: 19px'>
//                    <table width='100%' cellspacing='0' cellpadding='0'>
//                                        <tr>
//                                                        <td style='height: 19px'>
//                                                                                <table width='100%' cellspacing='0' cellpadding='0' border='0' class='datatable'>
//                                                                                <tr style='line-height: 50px; background-color: rgb(242,242,242);'><th valign='middle' align='left' style='width: 55%'><b>Product Details</b></th><th valign='middle' align='center' style='width: 10%; text-align: center;'><b>SKU</b></th><th valign='middle' align='center' style='text-align: center;'><b>Quantity</b></th></tr>"
//                                                                                    + Cart +
//                                                                                    @"</table>
//                                                        </td>
//                                        </tr>
//                                                    <tr>
//                                                        <td width='100%' align='center'>
//                                                        </td>
//                                                    </tr>
//                                                    <tr>
//                                                        <td width='100%' align='center'>
//                                                        </td>
//                                                    </tr>
//                                                    <tr>
//                                                        <td width='100%' align='center'>
//                                                        </td>
//                                                    </tr>
//                                                    <tr>
//                                                        <td width='100%' align='center'>
//                                                        </td>
//                                                    </tr>
//                    </table>
//</td>
//</tr></table></div>";
//                }

            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (Session["PrintOrderId"] != null && Session["PrintOrderId"].ToString() != "")
            {
                CommonComponent.ExecuteCommonData("update tb_Order set IsPrinted = 1 where ordernumber in (" + Session["PrintOrderId"].ToString() + ")");
                Session["PrintOrderId"] = "";
            }
        }

        /// <summary>
        /// Creates the folder at Specified path.
        /// </summary>
        /// <param name="FPath">string FPath</param>
        private void CreateFolder(String FPath)
        {
            if (!Directory.Exists(Server.MapPath(FPath.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(FPath.ToString()));
            }
        }
    }
}