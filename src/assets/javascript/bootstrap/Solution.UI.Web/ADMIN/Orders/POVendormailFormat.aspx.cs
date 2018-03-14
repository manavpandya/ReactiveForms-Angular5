using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class POVendormailFormat : BasePage
    {
        DataSet dsTemplate;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "SetHeight123", "javascript:window.parent.document.getElementById('ContentPlaceHolder1_frmPurchaseOrder').removeAttribute('onload'); window.parent.document.getElementById('ContentPlaceHolder1_frmPurchaseOrder').height ='900px';", true);
            if (!IsPostBack)
            {
                btnPrint.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/print.png";
                btnSendmailToVendor.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/send-mail-to-vendor.png";

                Int32 storeid = 0;
                Int32.TryParse(AppLogic.AppConfigs("StoreID").ToString(), out storeid);
                Int32 TemplateID = 0;
                if (Request.QueryString["MailTemplate"] != null && Request.QueryString["MailTemplate"].ToString().Trim() != "")
                {
                    TemplateID = Convert.ToInt32(Request.QueryString["MailTemplate"].ToString().Trim());
                }
                dsTemplate = new DataSet();
                dsTemplate = CommonComponent.GetCommonDataSet("Select Isnull(EmailType,1) as EmailType,* From tb_EmailTemplate Where TemplateID=" + TemplateID + "");

                string dsVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Select Email From tb_vendor Where vendorid=" + Request.QueryString["VendorID"].ToString() + ""));
                String EMailTo = string.Empty;
                String EMailCC = string.Empty;
                String EMailBCC = string.Empty;
                String EMailSubject = string.Empty;
                String EMailBody = string.Empty;
                Boolean IsHtml = false;

                if (dsVendor != null && dsVendor.ToString().Trim() != "")
                    EMailTo = dsVendor.ToString().Trim();

                String EMailFrom = string.Empty;

                if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
                {
                    if (dsTemplate.Tables[0].Rows[0]["EmailFrom"] != null && dsTemplate.Tables[0].Rows[0]["EmailFrom"].ToString().Trim() != "")
                        EMailFrom = dsTemplate.Tables[0].Rows[0]["EmailFrom"].ToString().Trim();
                    else
                        EMailFrom = AppLogic.AppConfigs("MailFrom");

                    if (dsTemplate.Tables[0].Rows[0]["EMailCC"] != null && dsTemplate.Tables[0].Rows[0]["EMailCC"].ToString().Trim() != "")
                        EMailCC = dsTemplate.Tables[0].Rows[0]["EMailCC"].ToString().Trim();

                    if (dsTemplate.Tables[0].Rows[0]["EMailBCC"] != null && dsTemplate.Tables[0].Rows[0]["EMailBCC"].ToString().Trim() != "")
                        EMailBCC = dsTemplate.Tables[0].Rows[0]["EMailBCC"].ToString().Trim();

                    if (dsTemplate.Tables[0].Rows[0]["Subject"] != null && dsTemplate.Tables[0].Rows[0]["Subject"].ToString().Trim() != "")
                        EMailSubject = dsTemplate.Tables[0].Rows[0]["Subject"].ToString().Trim();

                    if (dsTemplate.Tables[0].Rows[0]["EMailBody"] != null && dsTemplate.Tables[0].Rows[0]["EMailBody"].ToString().Trim() != "")
                    {
                        EMailBody = dsTemplate.Tables[0].Rows[0]["EMailBody"].ToString().Trim();
                    }

                    if (dsTemplate.Tables[0].Rows[0]["EmailType"] != null && dsTemplate.Tables[0].Rows[0]["EmailType"].ToString().Trim() != "")
                        IsHtml = Convert.ToBoolean(Convert.ToInt32(dsTemplate.Tables[0].Rows[0]["EmailType"].ToString().Trim()));

                }
                if (storeid == 0)
                    storeid = AppConfig.StoreID;
                txtFrom.Text = EMailFrom;
                txtTo.Text = EMailTo;
                txtSubject.Text = Regex.Replace(FillMailTemalateTextForVender(EMailSubject), "FOR ORDER #", "", RegexOptions.IgnoreCase);
                //if (!string.IsNullOrEmpty(EMailCC.ToString()))
                //{
                //    EMailCC += ";hollied@HPD.com";
                //}
                //else
                //{
                //    EMailCC = "hollied@HPD.com";
                //}
                txtCC.Text = EMailCC;
                txtBCC.Text = EMailBCC;
                EMailBody = FillMailTemalateTextForVender(EMailBody);
                //   ltPrint.Text = EMailBody;
                txtDescription.Text = EMailBody;
                if (IsHtml)
                {
                    divScript.Visible = true;
                }
            }
        }

        /// <summary>
        /// Binds the Vendor Cart
        /// </summary>
        /// <param name="VendorID">string VendorID</param>
        /// <returns>Returns the output value as a string format which contains HTML.</returns>
        private String BindVendorCart(string VendorID)
        {
            if (Session["VendorCart"] != null)
            {
                Decimal AdditionalCost = 0;
                Decimal.TryParse(Request.QueryString["AdditionalCost"].ToString(), out AdditionalCost);
                Decimal Adjustments = 0;
                Decimal.TryParse(Request.QueryString["Adjustments"].ToString(), out Adjustments);
                Decimal Tax = 0;
                Decimal.TryParse(Request.QueryString["Tax"].ToString(), out Tax);
                Decimal Shipping = 0;
                Decimal.TryParse(Request.QueryString["Shipping"].ToString(), out Shipping);

                ViewState["VendorCart"] = Session["VendorCart"];
                DataTable DtVendor = (DataTable)Session["VendorCart"];
                DataView dvVendor = DtVendor.DefaultView;
                Decimal SubTotal = 0, Total = 0;
                StringBuilder sb = new StringBuilder();
                String strLiveServer = AppLogic.AppConfigs("LIVE_SERVER");

                //sb.Append("<span style='color:red'>Once order is shipped, to change status,</span>&nbsp;&nbsp;&nbsp;<a style='color:#2b68a7;font-weight:bold;' href='" + strLiveServer + "/" + "VendorProducts.aspx?pono=<PO_NUMBER>&type=1&storeid=1'><img src='/App_Themes/" + Page.Theme + "/images/click-here.gif' border='0' /></a><br/><br/>");
                //sb.Append("<span style='color:red'>If there is problem with PO, Please </span>&nbsp;&nbsp;<a style='color:#2b68a7;font-weight:bold;' href='" + strLiveServer + "/" + "Vendorpoproblem.aspx?pono=<PO_NUMBER>&type=1&storeid=1'><img src='/App_Themes/" + Page.Theme + "/images/click-here.gif' width='71px' height='22px' border='0' /></a><br><br>");
                sb.Append("<table width='99%' cellspacing='0' cellpadding='0' border='1' style='padding: 10px 0 0;border: 1px solid #ECECEC; border-collapse: collapse; color: #212121; font: 12px;'>");
                sb.Append("<tr style='background-color: rgb(242,242,242); height: 25px;'>");
                sb.Append("<th valign='middle' align='Center' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>Package ID</th>");
                sb.Append("<th valign='middle' style='width:50%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold; padding-left: 5px;' align='left'>Name</th>");
                sb.Append("<th valign='middle' align='Center' style='width:17%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>SKU</th>");
                sb.Append("<th valign='middle' style='width:3%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;padding-left: 5px; padding-right: 5px;' align='center'>Quantity</th>");
                sb.Append("</tr>");
                for (int i = 0; i < dvVendor.Count; i++)
                {
                    sb.Append("<tr>");
                    sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>");
                    sb.Append(i + 1);
                    sb.Append("</td>");
                    sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;'>");
                    sb.Append(dvVendor.Table.Rows[i]["Name"].ToString());
                    sb.Append("</td>");
                    sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>");
                    sb.Append(dvVendor.Table.Rows[i]["SKU"].ToString());
                    sb.Append("</td>");
                    sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>");
                    sb.Append(dvVendor.Table.Rows[i]["Quantity"].ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    SubTotal += (Convert.ToDecimal(dvVendor.Table.Rows[i]["Price"].ToString()) * Convert.ToDecimal(dvVendor.Table.Rows[i]["Quantity"].ToString()));
                }
                Total = SubTotal + AdditionalCost + Adjustments + Shipping + Tax;
                sb.Append("</table>");
                return sb.ToString();
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Fills the mail template text for vendor.
        /// </summary>
        /// <param name="strText">string strText</param>
        /// <returns>Returns the Replaced value String</returns>
        private String FillMailTemalateTextForVender(String strText)
        {
            String strSource = strText;
            string StrVenAddr = string.Empty;
            DataSet dsVen = new DataSet();
            strSource = Regex.Replace(strSource, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###ContactMail_ToAddress###", AppLogic.AppConfigs("ContactMail_ToAddress").ToString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###TODAY###", DateTime.Now.ToLongDateString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###FIRSTNAME###", "", RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###LASTNAME###", "", RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###USERNAME###", "", RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###PASSWORD###", SecurityComponent.Decrypt(Convert.ToString(ViewState["Password"])), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

            dsVen = CommonComponent.GetCommonDataSet("Select * From tb_Vendor Where VendorId=" + Request.QueryString["VendorID"].ToString() + "");
            if (dsVen != null && dsVen.Tables[0].Rows.Count > 0)
            {
                strSource = Regex.Replace(strSource, "###Vendor_Name###", dsVen.Tables[0].Rows[0]["Name"].ToString(), RegexOptions.IgnoreCase);
                if (dsVen.Tables[0].Rows[0]["Phone"] != null && dsVen.Tables[0].Rows[0]["Phone"].ToString().Trim() != "")
                    strSource = Regex.Replace(strSource, "<VENDOR_PHONE>", dsVen.Tables[0].Rows[0]["Phone"].ToString(), RegexOptions.IgnoreCase);
                else
                    strSource = Regex.Replace(strSource, "<VENDOR_PHONE>", "", RegexOptions.IgnoreCase);
                if (dsVen.Tables[0].Rows[0]["Address"] != null && dsVen.Tables[0].Rows[0]["Address"].ToString().Trim() != "")
                    strSource = Regex.Replace(strSource, "<VENDOR_SHIP_ADDRESS1>", dsVen.Tables[0].Rows[0]["Address"].ToString(), RegexOptions.IgnoreCase);
                else
                    strSource = Regex.Replace(strSource, "<VENDOR_SHIP_ADDRESS1>", "", RegexOptions.IgnoreCase);
            }
            if (strSource.Contains("###ORDER_ITEMS###"))
                strSource = Regex.Replace(strSource, "###ORDER_ITEMS###", BindVendorCart(Request.QueryString["VendorID"].ToString()), RegexOptions.IgnoreCase);

            try
            {
                OrderComponent objOrder = new OrderComponent();
                DataSet dsOrder = new DataSet();
                dsOrder = objOrder.GetOrderDetailsByOrderID(Convert.ToInt32(Request.QueryString["ONo"].ToString()));
                string StrShippingAddr = "";
                if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                {
                    StrShippingAddr = "<table width=\"60%\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" class=\"popup_cantain\">";
                    StrShippingAddr += "<tbody>";
                    StrShippingAddr += "<tr>";
                    StrShippingAddr += "<td valign='top'><b>Ship To:</b>";
                    StrShippingAddr += "</td>";
                    StrShippingAddr += "<td>";

                    StrShippingAddr += "<b>" + dsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + dsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString() + "</b><br />";
                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                    {
                        StrShippingAddr += dsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                    {
                        StrShippingAddr += dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                    {
                        StrShippingAddr += dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                    {
                        StrShippingAddr += dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString() + "<br />";
                    }
                    StrShippingAddr += dsOrder.Tables[0].Rows[0]["ShippingCity"].ToString() + ", " + dsOrder.Tables[0].Rows[0]["ShippingState"].ToString() + " " + dsOrder.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br />";
                    StrShippingAddr += dsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString() + "<br />";
                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    {
                        StrShippingAddr += "<b>Phone:</b> " + dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingEmail"].ToString()))
                    {
                        StrShippingAddr += "<b>Email:</b> " + dsOrder.Tables[0].Rows[0]["ShippingEmail"].ToString();
                    }

                    StrShippingAddr += "</td>";
                    StrShippingAddr += "</tr>";
                    StrShippingAddr += "</tbody>";
                    StrShippingAddr += "</table>";
                    strSource = Regex.Replace(strSource, "###SHIP_ADDRESS###", StrShippingAddr.ToString(), RegexOptions.IgnoreCase);
                }
                else
                {
                    strSource = Regex.Replace(strSource, "###SHIP_ADDRESS###", "", RegexOptions.IgnoreCase);
                }
            }
            catch { strSource = Regex.Replace(strSource, "###SHIP_ADDRESS###", "", RegexOptions.IgnoreCase); }

            Decimal Adjustments = 0;
            Decimal.TryParse(Request.QueryString["Adjustments"].ToString(), out Adjustments);
            strSource = Regex.Replace(strSource, "<ADJUSTMENTS>", String.Format("{0:0.00}", Adjustments), RegexOptions.IgnoreCase);

            Decimal AdditionalCost = 0;
            Decimal.TryParse(Request.QueryString["AdditionalCost"].ToString(), out AdditionalCost);
            strSource = Regex.Replace(strSource, "<ADDITIONAL_COST>", String.Format("{0:0.00}", Convert.ToDecimal(AdditionalCost)), RegexOptions.IgnoreCase);
            Decimal Tax = 0;
            Decimal.TryParse(Request.QueryString["Tax"].ToString(), out Tax);
            strSource = Regex.Replace(strSource, "<SALE_TAX>", String.Format("{0:0.00}", Tax), RegexOptions.IgnoreCase);

            Decimal Shipping = 0;
            Decimal.TryParse(Request.QueryString["Shipping"].ToString(), out Shipping);
            strSource = Regex.Replace(strSource, "<SHIPPING_COST>", String.Format("{0:0.00}", Shipping), RegexOptions.IgnoreCase);
            if (Request.QueryString["Notes"] != null)
            {
                try
                {
                    strSource = Regex.Replace(strSource, "###NOTES###", Session["PONotes"].ToString() == "" ? "N/A" : Session["PONotes"].ToString(), RegexOptions.IgnoreCase);
                }
                catch { }
            }
            else
            {
                strSource = Regex.Replace(strSource, "###NOTES###", "N/A", RegexOptions.IgnoreCase);
            }
            if (strSource.Contains("<ORDER_SUBTOTAL>") || strSource.Contains("<ORDER_TOTAL>"))
            {
                if (Session["VendorCart"] != null)
                {
                    DataTable DtVendor = (DataTable)Session["VendorCart"];
                    Decimal SubTotal = 0, Total = 0;
                    foreach (DataRow dr in DtVendor.Rows)
                    { SubTotal += (Convert.ToDecimal(dr["Price"].ToString()) * Convert.ToInt32(dr["Quantity"].ToString())); }
                    Total = SubTotal + AdditionalCost + Shipping + Adjustments + Tax;
                    strSource = Regex.Replace(strSource, "<ORDER_SUBTOTAL>", String.Format("{0:0.00}", SubTotal), RegexOptions.IgnoreCase);
                    strSource = Regex.Replace(strSource, "<ORDER_TOTAL>", String.Format("{0:0.00}", Total), RegexOptions.IgnoreCase);
                }
            }
            return strSource;

        }

        /// <summary>
        ///  Send Mail to Vendor Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSendmailToVendor_Click(object sender, ImageClickEventArgs e)
        {
            Int32 storeid = 0;
            Int32.TryParse(AppLogic.AppConfigs("StoreID").ToString(), out storeid);
            Int32 TemplateID = 0;
            if (Request.QueryString["MailTemplate"] != null && Request.QueryString["MailTemplate"].ToString().Trim() != "")
            {
                TemplateID = Convert.ToInt32(Request.QueryString["MailTemplate"].ToString().Trim());
            }
            dsTemplate = new DataSet();
            dsTemplate = CommonComponent.GetCommonDataSet("Select Isnull(EmailType,1) as EmailType,* From tb_EmailTemplate Where TemplateID=" + TemplateID + "");
            string dsVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Select Email From tb_vendor Where vendorid=" + Request.QueryString["VendorID"].ToString() + ""));
            String EMailTo = string.Empty;
            String EMailCC = string.Empty;
            String EMailBCC = string.Empty;
            String EMailSubject = string.Empty;
            String EMailBody = string.Empty;
            Boolean IsHtml = false;

            if (!string.IsNullOrEmpty(txtTo.Text.ToString()))
            {
                EMailTo = txtTo.Text.ToString();
            }
            else
            {
                if (dsVendor != null && dsVendor.ToString().Trim() != "")
                    EMailTo = dsVendor.ToString().Trim();
            }
            String EMailFrom = string.Empty;

            if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
            {
                if (dsTemplate.Tables[0].Rows[0]["EmailFrom"] != null && dsTemplate.Tables[0].Rows[0]["EmailFrom"].ToString().Trim() != "")
                    EMailFrom = dsTemplate.Tables[0].Rows[0]["EmailFrom"].ToString().Trim();
                else
                    EMailFrom = AppLogic.AppConfigs("MailFrom");

                if (!string.IsNullOrEmpty(txtCC.Text.ToString()))
                {
                    EMailCC = txtCC.Text.ToString();
                }
                else
                {
                    if (dsTemplate.Tables[0].Rows[0]["EMailCC"] != null && dsTemplate.Tables[0].Rows[0]["EMailCC"].ToString().Trim() != "")
                        EMailCC = dsTemplate.Tables[0].Rows[0]["EMailCC"].ToString().Trim();
                }

                if (!string.IsNullOrEmpty(txtBCC.Text.ToString()))
                {
                    EMailBCC = txtBCC.Text.ToString();
                }
                else
                {
                    if (dsTemplate.Tables[0].Rows[0]["EMailBCC"] != null && dsTemplate.Tables[0].Rows[0]["EMailBCC"].ToString().Trim() != "")
                        EMailBCC = dsTemplate.Tables[0].Rows[0]["EMailBCC"].ToString().Trim();
                }

                if (dsTemplate.Tables[0].Rows[0]["Subject"] != null && dsTemplate.Tables[0].Rows[0]["Subject"].ToString().Trim() != "")
                    EMailSubject = dsTemplate.Tables[0].Rows[0]["Subject"].ToString().Trim();

                if (dsTemplate.Tables[0].Rows[0]["EmailType"] != null && dsTemplate.Tables[0].Rows[0]["EmailType"].ToString().Trim() != "")
                    IsHtml = Convert.ToBoolean(Convert.ToInt32(dsTemplate.Tables[0].Rows[0]["EmailType"].ToString().Trim()));
            }
            if (!string.IsNullOrEmpty(txtDescription.Text.Trim()))
                EMailBody = txtDescription.Text.Trim();
            string VenderID = Request.QueryString["VendorID"].ToString();

            if (ViewState["VendorCart"] != null)
            {
                DataTable DtVendor = (DataTable)ViewState["VendorCart"];

                Decimal AdditionalCost = 0;
                Decimal.TryParse(Request.QueryString["AdditionalCost"].ToString(), out AdditionalCost);

                Decimal Adjustments = 0;
                Decimal.TryParse(Request.QueryString["Adjustments"].ToString(), out Adjustments);

                Decimal Tax = 0;
                Decimal.TryParse(Request.QueryString["Tax"].ToString(), out Tax);

                Decimal Shipping = 0;
                Decimal.TryParse(Request.QueryString["Shipping"].ToString(), out Shipping);
                String Notes = string.Empty;
                Notes = Session["PONotes"].ToString(); //Server.UrlDecode(Request.QueryString["Notes"].ToString());

                ArrayList ProductIDs = new ArrayList();
                ArrayList TrackingIDs = new ArrayList();
                ArrayList Couriers = new ArrayList();
                ArrayList ShippedOn = new ArrayList();
                ArrayList ShippedQty = new ArrayList();
                ArrayList ShippedNote = new ArrayList();
                ArrayList customcartid = new ArrayList();

                Int32 PONumber = Convert.ToInt32(CommonComponent.GetScalarCommonData("insert into tb_PurchaseOrder(VendorID,PODate,AdditionalCost,Notes,OrderNumber,Adjustments,Tax,Shipping) values(" + VenderID + ",'" + Convert.ToDateTime(System.DateTime.Now) + "'," + Math.Round(AdditionalCost, 2) + ",'" + Notes + "'," + Request.QueryString["ONo"] + "," + Adjustments + "," + Tax + "," + Shipping + ") select scope_identity()"));
                Decimal PoAmt = AdditionalCost + Adjustments + Tax + Shipping;
                if (PONumber != 0)
                {
                    StringBuilder strQuery = new StringBuilder();
                    foreach (DataRow dr in DtVendor.Rows)
                    {
                        strQuery.Append(" insert into tb_PurchaseOrderItems(ProductID,Quantity,Price,PONumber,Ordercustomcartid) values(" + dr["ProductID"] + "," + dr["Quantity"].ToString() + "," + Math.Round(Convert.ToDecimal(dr["Price"].ToString()), 2) + "," + PONumber + "," + dr["Customcartid"] + ") ");
                        PoAmt += (Math.Round(Convert.ToDecimal(dr["Price"].ToString()), 2) * Convert.ToInt32(dr["Quantity"].ToString()));

                        ProductIDs.Add(dr["ProductID"].ToString());
                        Random r = new Random((int)DateTime.Now.Ticks);
                        string strRandom = "";
                        TrackingIDs.Add(strRandom);
                        Couriers.Add("");
                        ShippedOn.Add(DateTime.Now);
                        ShippedQty.Add(Convert.ToInt32(dr["Quantity"].ToString()));
                        ShippedNote.Add(Convert.ToString("Purchase Order(" + dr["Quantity"].ToString() + ")"));
                        customcartid.Add(Convert.ToString(dr["Customcartid"].ToString()));
                        //CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseproduct WHERE Productid =" + dr["ProductID"].ToString() + "");
                    }
                    strQuery.Append("  update tb_PurchaseOrder set poamount=" + PoAmt + " where ponumber=" + PONumber + " ");
                    if (!string.IsNullOrEmpty(strQuery.ToString().Trim()))
                        CommonComponent.ExecuteCommonData(strQuery.ToString());
                }
                VendorComponent objVen = new VendorComponent();

                //if (ProductIDs.Count > 0) // 
                //    objVen.MarkProductsShippedforVendorPO(Convert.ToInt32(Request.QueryString["ono"]), ProductIDs, TrackingIDs, Couriers, ShippedOn, ShippedQty, ShippedNote, customcartid);

                //AlternateView av = AlternateView.CreateAlternateViewFromString(EMailBody.ToString(), null, "text/html");

                if (EMailSubject.Contains("<PO_NUMBER>"))
                {
                    EMailSubject = Regex.Replace(EMailSubject, "<PO_NUMBER>", PONumber.ToString(), RegexOptions.IgnoreCase);
                }

                string subject = Regex.Replace(FillMailTemalateTextForVender(EMailSubject), "FOR ORDER #", "", RegexOptions.IgnoreCase);
                if (subject.Contains("<PO_NUMBER>"))
                {
                    subject = Regex.Replace(subject, "<PO_NUMBER>", PONumber.ToString(), RegexOptions.IgnoreCase);
                }
                if (EMailBody.Contains("<PO_NUMBER>"))
                {
                    EMailBody = Regex.Replace(EMailBody, "<PO_NUMBER>", PONumber.ToString(), RegexOptions.IgnoreCase);
                }
                if (EMailBody.Contains("&lt;PO_NUMBER&gt;"))
                {
                    EMailBody = Regex.Replace(EMailBody, "&lt;PO_NUMBER&gt;", PONumber.ToString(), RegexOptions.IgnoreCase);
                }
                AlternateView av = AlternateView.CreateAlternateViewFromString(EMailBody, null, "text/html");
                BindVendorCartPdf(Request.QueryString["VendorID"].ToString(), PONumber.ToString());
                if (chkPdfFile.Checked == true)
                {
                    String PdgFilePath = Convert.ToString(AppLogic.AppConfigs("POFilesPath"));
                    CommonOperations.SendMailAttachment(EMailFrom, EMailTo.Replace(",", ";"), EMailCC, EMailBCC, Regex.Replace(FillMailTemalateTextForVender(EMailSubject), "FOR ORDER #", "", RegexOptions.IgnoreCase), EMailBody, Request.UserHostAddress.ToString(), IsHtml, av, Server.MapPath(PdgFilePath + "PO_" + PONumber.ToString() + ".pdf").ToString());
                }
                else
                {
                    CommonOperations.SendMail(EMailFrom, EMailTo.Replace(",", ";"), EMailCC, EMailBCC, Regex.Replace(FillMailTemalateTextForVender(EMailSubject), "FOR ORDER #", "", RegexOptions.IgnoreCase), EMailBody, Request.UserHostAddress.ToString(), IsHtml, null);
                }
                try
                {
                    CommonComponent.ExecuteCommonData("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber,CreatedOn) VALUES (" + Session["AdminID"].ToString() + ",10," + Convert.ToInt32(PONumber) + "," + Convert.ToInt32(Request.QueryString["ONo"]) + ",getdate())");
                    CommonComponent.ExecuteCommonData("UPDATE tb_Order SET OrderStatus='PO Generated' WHERE OrderNumber=" + Request.QueryString["ONo"].ToString() + "");
                    OrderComponent objAddOrder = new OrderComponent();
                    objAddOrder.InsertOrderlog(10, Convert.ToInt32(Request.QueryString["ONo"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                }
                catch { }

                CommonComponent.ExecuteCommonData("INSERT INTO tb_Warehousemail(Ponumber,EmailTo,Subject,Body,Filepath,EmailFrom,EmailCC,EMailBCC,VendorAddress) VALUES (" + PONumber.ToString() + ",'" + EMailTo.Replace(",", ";") + "','" + subject.Replace("'", "''") + "','" + EMailBody.Replace("'", "''") + "','','" + EMailFrom + "','" + EMailCC.Replace(",", ";") + "','" + EMailBCC.Replace(",", ";") + "','')");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "SaveMsg1", "javascript:window.parent.Tabdisplay(11);window.parent.chkHeight();window.parent.iframereload('ContentPlaceHolder1_frmPurchaseOrder');window.parent.document.getElementById('prepage').style.display = 'none';", true);
                // window.location.href='/Orders.aspx?Id=" + Request.QueryString["ono"] + "&PurchaseOrder=true';
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Msgsave", "alert('sda');", true);
            }
        }

        /// <summary>
        /// Binds the vendor cart PDF for Generate by Order Number
        /// </summary>
        /// <param name="VendorID">string VendorID</param>
        /// <param name="PONumber">string PONumber</param>
        private void BindVendorCartPdf(string VendorID, string PONumber)
        {
            string OrderNo = Request.QueryString["ONo"].ToString().Trim();
            OrderComponent objOrder = new OrderComponent();
            SQLAccess dbAccess = new SQLAccess();
            DataSet objDsorder = new DataSet();
            objDsorder = objOrder.GetOrderDetailsByOrderID(Convert.ToInt32(OrderNo));
            Int32 PoNumber = 0;
            PoNumber = Convert.ToInt32(PONumber);
            string vendorname = Convert.ToString(dbAccess.ExecuteScalarQuery("Select ISNULL(Name,'') AS Name From tb_Vendor Where ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 and VendorId=" + VendorID));

            if (objDsorder != null && objDsorder.Tables.Count > 0 && objDsorder.Tables[0].Rows.Count > 0)
            {
                if (ViewState["VendorCart"] != null)
                {

                    Decimal AdditionalCost = 0;
                    Decimal.TryParse(Request.QueryString["AdditionalCost"].ToString(), out AdditionalCost);

                    Decimal Adjustments = 0;
                    Decimal.TryParse(Request.QueryString["Adjustments"].ToString(), out Adjustments);

                    Decimal Tax = 0;
                    Decimal.TryParse(Request.QueryString["Tax"].ToString(), out Tax);


                    Decimal Shipping = 0;
                    Decimal.TryParse(Request.QueryString["Shipping"].ToString(), out Shipping);

                    DataTable DtVendor = (DataTable)ViewState["VendorCart"];
                    DataView dvVendor = DtVendor.DefaultView;
                    Decimal SubTotal = 0, Total = 0;
                    Document document = new Document();

                    try
                    {
                        String PdgFilePath = Convert.ToString(AppLogic.AppConfigs("POFilesPath"));
                        if (!System.IO.Directory.Exists(Server.MapPath(PdgFilePath)))
                        {
                            System.IO.Directory.CreateDirectory(Server.MapPath(PdgFilePath));
                        }
                        try
                        {
                            if (File.Exists(Server.MapPath(PdgFilePath + "PO_" + PoNumber.ToString() + ".pdf").ToString()))
                            {
                                File.Delete(Server.MapPath(PdgFilePath + "PO_" + PoNumber.ToString() + ".pdf").ToString());
                            }
                        }
                        catch
                        {
                        }

                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(PdgFilePath + "PO_" + PoNumber.ToString() + ".pdf").ToString(), FileMode.Create));
                        document.Open();

                        iTextSharp.text.Table table = new iTextSharp.text.Table(5);
                        iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                        float[] headerwidths = { 200, 80, 50 };
                        aTable.Widths = headerwidths;
                        aTable.WidthPercentage = 100;

                        iTextSharp.text.Table aTable2 = new iTextSharp.text.Table(1);// 2 rows, 2 columns
                        aTable2.Cellpadding = 3;
                        aTable2.Cellspacing = 3;
                        aTable2.BorderWidth = 0;
                        aTable2.WidthPercentage = 100;
                        iTextSharp.text.Cell cell = new iTextSharp.text.Cell();
                        //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(AppLogic.AppConfigs("LIVE_SERVER") + "/images/logo.png");
                        string LogoPath = "";
                        if (String.IsNullOrEmpty(AppLogic.AppConfigs("StoreId")))
                        {
                            LogoPath = "logo";
                        }
                        else
                        {
                            LogoPath = "Store_" + AppLogic.AppConfigs("StoreId");
                        }
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(AppLogic.AppConfigs("LIVE_SERVER") + "/images/" + LogoPath + ".png");
                        iTextSharp.text.Image img4 = img;
                        img4.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                        cell.Add(img4);
                        cell.BorderWidth = 0;
                        aTable2.AddCell(cell);
                        document.Add(aTable2);
                        //logo created


                        iTextSharp.text.Table aTable1 = new iTextSharp.text.Table(2);
                        aTable1.WidthPercentage = 100;
                        aTable1.BorderWidth = 0;
                        aTable1.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
                        iTextSharp.text.Cell cell1 = new iTextSharp.text.Cell("Order Number :  " + OrderNo.ToString());
                        cell1.BorderColor = new Color(0, 0, 255);
                        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell1.BorderWidth = 0;
                        aTable1.AddCell(cell1);

                        iTextSharp.text.Cell cellShipTo = new iTextSharp.text.Cell("Ship To :  ");
                        cellShipTo.BorderColor = new Color(0, 0, 255);
                        cellShipTo.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellShipTo.BorderWidth = 0;
                        aTable1.AddCell(cellShipTo);

                        iTextSharp.text.Cell cell111 = new iTextSharp.text.Cell("Shipping Method: " + objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString());
                        cell111.BorderColor = new Color(0, 0, 255);
                        cell111.BorderWidth = 0;
                        aTable1.AddCell(cell111);

                        iTextSharp.text.Cell cellShipname1 = new iTextSharp.text.Cell(Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString()));
                        cellShipname1.BorderColor = new Color(0, 0, 255);
                        cellShipname1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellShipname1.BorderWidth = 0;
                        aTable1.AddCell(cellShipname1);

                        try
                        {
                            iTextSharp.text.Cell cellShipname = new iTextSharp.text.Cell("");
                            cellShipname.BorderColor = new Color(0, 0, 255);
                            cellShipname.BorderWidth = 0;
                            aTable1.AddCell(cellShipname);

                            iTextSharp.text.Cell cellcompany = new iTextSharp.text.Cell(Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString()));
                            cellcompany.BorderColor = new Color(0, 0, 255);
                            cellcompany.BorderWidth = 0;
                            aTable1.AddCell(cellcompany);

                            iTextSharp.text.Cell cellcompany1 = new iTextSharp.text.Cell("");
                            cellcompany1.BorderColor = new Color(0, 0, 255);
                            cellcompany1.BorderWidth = 0;
                            aTable1.AddCell(cellcompany1);

                            iTextSharp.text.Cell cellShipToAdd = new iTextSharp.text.Cell(Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingAddress1"]));
                            cellShipToAdd.BorderColor = new Color(0, 0, 255);
                            cellShipToAdd.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellShipToAdd.BorderWidth = 0;
                            aTable1.AddCell(cellShipToAdd);
                            // aTable1.AddCell(cell112);


                            iTextSharp.text.Cell cellcompany2 = new iTextSharp.text.Cell("");
                            cellcompany2.BorderColor = new Color(0, 0, 255);
                            cellcompany2.BorderWidth = 0;
                            aTable1.AddCell(cellcompany2);

                            iTextSharp.text.Cell cellShipToAdd2 = new iTextSharp.text.Cell(Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingAddress2"]));
                            cellShipToAdd2.BorderColor = new Color(0, 0, 255);
                            cellShipToAdd2.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellShipToAdd2.BorderWidth = 0;
                            aTable1.AddCell(cellShipToAdd2);

                            iTextSharp.text.Cell cellSuite = new iTextSharp.text.Cell("");
                            cellSuite.BorderColor = new Color(0, 0, 255);
                            cellSuite.BorderWidth = 0;
                            aTable1.AddCell(cellSuite);

                            iTextSharp.text.Cell cellSuite1 = new iTextSharp.text.Cell("Suite : " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingSuite"]));
                            cellSuite1.BorderColor = new Color(0, 0, 255);
                            cellSuite1.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellSuite1.BorderWidth = 0;
                            aTable1.AddCell(cellSuite1);


                            iTextSharp.text.Cell cellcity = new iTextSharp.text.Cell("");
                            cellcity.BorderColor = new Color(0, 0, 255);
                            cellcity.BorderWidth = 0;
                            aTable1.AddCell(cellcity);

                            iTextSharp.text.Cell cellcity1 = new iTextSharp.text.Cell(Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCity"]));
                            cellcity1.BorderColor = new Color(0, 0, 255);
                            cellcity1.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellcity1.BorderWidth = 0;
                            aTable1.AddCell(cellcity1);

                            iTextSharp.text.Cell cellstate = new iTextSharp.text.Cell("");
                            cellstate.BorderColor = new Color(0, 0, 255);
                            cellstate.BorderWidth = 0;
                            aTable1.AddCell(cellstate);

                            iTextSharp.text.Cell cellstate1 = new iTextSharp.text.Cell(Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingState"]) + "," + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingZip"]));
                            cellstate1.BorderColor = new Color(0, 0, 255);
                            cellstate1.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellstate1.BorderWidth = 0;
                            aTable1.AddCell(cellstate1);


                            iTextSharp.text.Cell cellcountry = new iTextSharp.text.Cell("");
                            cellcountry.BorderColor = new Color(0, 0, 255);
                            cellcountry.BorderWidth = 0;
                            aTable1.AddCell(cellcountry);

                            iTextSharp.text.Cell cellcountry1 = new iTextSharp.text.Cell(Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()));
                            cellcountry1.BorderColor = new Color(0, 0, 255);
                            cellcountry1.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellcountry1.BorderWidth = 0;
                            aTable1.AddCell(cellcountry1);


                            iTextSharp.text.Cell cellphone = new iTextSharp.text.Cell("");
                            cellphone.BorderColor = new Color(0, 0, 255);
                            cellphone.BorderWidth = 0;
                            aTable1.AddCell(cellphone);

                            iTextSharp.text.Cell cellphone1 = new iTextSharp.text.Cell(Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()));
                            cellphone1.BorderColor = new Color(0, 0, 255);
                            cellphone1.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellphone1.BorderWidth = 0;
                            aTable1.AddCell(cellphone1);
                        }
                        catch
                        {
                        }

                        document.Add(new Paragraph(" "));
                        document.Add(new Paragraph(" "));
                        document.Add(aTable1);

                        iTextSharp.text.Cell cellname = new iTextSharp.text.Cell("Name");
                        cellname.VerticalAlignment = Element.ALIGN_MIDDLE;

                        cellname.BorderColor = new Color(164, 164, 164);
                        cellname.BackgroundColor = new Color(218, 218, 218);

                        aTable.AddCell(cellname);

                        iTextSharp.text.Cell cellsku = new iTextSharp.text.Cell("SKU");
                        cellsku.BorderColor = new Color(164, 164, 164);
                        cellsku.BackgroundColor = new Color(218, 218, 218);
                        cellsku.VerticalAlignment = Element.ALIGN_TOP;

                        aTable.AddCell(cellsku);

                        iTextSharp.text.Cell cellQty = new iTextSharp.text.Cell("Quantity");
                        cellQty.BorderColor = new Color(164, 164, 164);
                        cellQty.BackgroundColor = new Color(218, 218, 218);
                        cellQty.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellQty.VerticalAlignment = Element.ALIGN_MIDDLE;
                        aTable.AddCell(cellQty);

                        aTable.DefaultVerticalAlignment = Element.ALIGN_MIDDLE;
                        aTable.Padding = 3;
                        aTable.DefaultCellGrayFill = 3;
                        aTable.BorderWidth = 1;
                        aTable.BorderColor = new Color(218, 218, 218);
                        for (int i = 0; i < dvVendor.Count; i++)
                        {

                            aTable.DefaultCell.GrayFill = 1.0f;

                            iTextSharp.text.Cell cellname1 = new iTextSharp.text.Cell(dvVendor.Table.Rows[i]["Name"].ToString().Replace("<br/>", "\r\n"));
                            cellname1.BorderColor = new Color(164, 164, 164);
                            cellname1.VerticalAlignment = Element.ALIGN_MIDDLE;

                            aTable.AddCell(cellname1);

                            iTextSharp.text.Cell cellsku1 = new iTextSharp.text.Cell(dvVendor.Table.Rows[i]["SKU"].ToString());
                            cellsku1.BorderColor = new Color(164, 164, 164);

                            cellsku1.VerticalAlignment = Element.ALIGN_CENTER;

                            aTable.AddCell(cellsku1);

                            iTextSharp.text.Cell cellQty1 = new iTextSharp.text.Cell(dvVendor.Table.Rows[i]["Quantity"].ToString());
                            cellQty1.BorderColor = new Color(164, 164, 164);
                            cellQty1.VerticalAlignment = Element.ALIGN_CENTER;
                            cellQty1.HorizontalAlignment = Element.ALIGN_CENTER;
                            aTable.AddCell(cellQty1);
                        }

                        document.Add(aTable);

                        document.Add(new Paragraph(" "));
                        document.Add(new Paragraph(" "));
                        iTextSharp.text.Table aTableVisit = new iTextSharp.text.Table(1);
                        aTableVisit.WidthPercentage = 100;
                        aTableVisit.BorderWidth = 0;
                        aTableVisit.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
                        iTextSharp.text.Cell visit = new iTextSharp.text.Cell("Thank You,");
                        visit.BorderColor = new Color(164, 164, 164);
                        visit.BorderWidth = 0;
                        visit.VerticalAlignment = Element.ALIGN_MIDDLE;
                        visit.HorizontalAlignment = Element.ALIGN_LEFT;
                        aTableVisit.AddCell(visit);

                        iTextSharp.text.Cell visitSite = new iTextSharp.text.Cell(AppLogic.AppConfigs("LIVE_Server").ToString());
                        visitSite.BorderColor = new Color(164, 164, 164);
                        visitSite.BorderWidth = 0;
                        visitSite.VerticalAlignment = Element.ALIGN_MIDDLE;
                        visitSite.HorizontalAlignment = Element.ALIGN_LEFT;
                        aTableVisit.AddCell(visitSite);

                        document.Add(aTableVisit);
                        document.Close();
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}