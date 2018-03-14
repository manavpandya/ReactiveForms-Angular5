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

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class WareHouseVendormail : BasePage
    {
        DataSet dsTemplate;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnPrint.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/print.png";
                btnSendmailToVendor.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/send-mail-to-vendor(s).png";

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
                txtSubject.Text = Regex.Replace(FillMailTemplateTextForVendor(EMailSubject), "FOR ORDER #", "", RegexOptions.IgnoreCase);
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
                EMailBody = FillMailTemplateTextForVendor(EMailBody);
                ltPrint.Text = EMailBody;
                txtDescription.Text = EMailBody;
                if (IsHtml)
                {
                    divScript.Visible = true;
                }
            }
        }

        /// <summary>
        /// Binds the vendor cart.
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

                sb.Append("<span style='color:red'>Once order is shipped, to change status,</span>&nbsp;&nbsp;&nbsp;<a style='color:#2b68a7;font-weight:bold;' href='" + strLiveServer + "/" + "WarhouseVendorProducts.aspx?pono=<PO_NUMBER>&type=1&storeid=1'><img src='/App_Themes/" + Page.Theme + "/images/click-here.gif' width='71px' height='22px' border='0' /></a><br/><br/>");
                //sb.Append("<span style='color:red'>If there is problem with PO, Please </span>&nbsp;&nbsp;<a style='color:#2b68a7;font-weight:bold;' href='" + strLiveServer + "/" + "VendorPOProblem.aspx?pono=<PO_NUMBER>&type=1&storeid=1'><img src='/App_Themes/" + Page.Theme + "/images/click-here.gif' width='71px' height='22px' border='0' /></a><br><br>");
                sb.Append("<table width='99%' cellspacing='0' cellpadding='0' border='1' style='padding: 10px 0 0;border: 1px solid #ECECEC; border-collapse: collapse; color: #212121; font: 12px;'>");
                sb.Append("<tr style='background-color: rgb(242,242,242);'>");
                sb.Append("<th valign='middle' align='Center' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>Package ID</th>");
                sb.Append("<th valign='middle' style='width:50%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>Name</th>");
                sb.Append("<th valign='middle' align='Center' style='width:17%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>SKU</th>");
                sb.Append("<th valign='middle' style='width:3%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='center'>Quantity</th>");
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
        /// Fills the Mail Template Text for Vendor
        /// </summary>
        /// <param name="strText">string strText</param>
        /// <returns>Returns the output value as a string format which contains HTML</returns>
        private String FillMailTemplateTextForVendor(String strText)
        {
            String strSource = strText;
            string StrVenAddr = string.Empty;
            DataSet dsVen = new DataSet();
            strSource = Regex.Replace(strSource, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###FIRSTNAME###", "", RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###LASTNAME###", "", RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###USERNAME###", "", RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###PASSWORD###", SecurityComponent.Decrypt(Convert.ToString(ViewState["Password"])), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

            strSource = Regex.Replace(strSource, "###TODAY###", DateTime.Now.ToLongDateString(), RegexOptions.IgnoreCase);
            strSource = Regex.Replace(strSource, "###ContactMail_ToAddress###", AppLogic.AppConfigs("ContactMail_ToAddress").ToString(), RegexOptions.IgnoreCase);

            try
            {
                string StrShippingAddr = "";
                StrShippingAddr = "<table width=\"60%\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" class=\"popup_cantain\">";
                StrShippingAddr += "<tbody>";
                StrShippingAddr += "<tr>";
                StrShippingAddr += "<td valign='top'><b>Ship To:</b>";
                StrShippingAddr += "</td>";
                StrShippingAddr += "<td>";
                StrShippingAddr += "<b>" + AppLogic.AppConfigs("Shipping.OriginContactName").ToString() + "</b><br />";
                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.CompanyName").ToString()))
                {
                    StrShippingAddr += AppLogic.AppConfigs("Shipping.CompanyName").ToString() + "<br />";
                }
                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginAddress").ToString()))
                {
                    StrShippingAddr += AppLogic.AppConfigs("Shipping.OriginAddress").ToString() + "<br />";
                }
                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginAddress2").ToString()))
                {
                    StrShippingAddr += AppLogic.AppConfigs("Shipping.OriginAddress2").ToString() + "<br />";
                }
                StrShippingAddr += AppLogic.AppConfigs("Shipping.OriginCity").ToString() + ", " + AppLogic.AppConfigs("Shipping.OriginState").ToString() + " " + AppLogic.AppConfigs("Shipping.OriginZip").ToString() + "<br />";
                StrShippingAddr += AppLogic.AppConfigs("Shipping.OriginCountry").ToString() + "<br />";
                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginPhone").ToString()))
                {
                    StrShippingAddr += "<b>Phone:</b> " + AppLogic.AppConfigs("Shipping.OriginPhone").ToString() + "<br />";
                }
                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("ContactMail_ToAddress").ToString()))
                {
                    StrShippingAddr += "<b>Email:</b> " + AppLogic.AppConfigs("ContactMail_ToAddress").ToString();
                }
                StrShippingAddr += "</td>";
                StrShippingAddr += "</tr>";
                StrShippingAddr += "</tbody>";
                StrShippingAddr += "</table>";
                strSource = Regex.Replace(strSource, "###SHIP_ADDRESS###", StrShippingAddr.ToString(), RegexOptions.IgnoreCase);
            }
            catch { strSource = Regex.Replace(strSource, "###SHIP_ADDRESS###", "", RegexOptions.IgnoreCase); }
            // strSource = Regex.Replace(strSource, "###SHIP_ADDRESS###", "", RegexOptions.IgnoreCase);

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

                Int32 PONumber = Convert.ToInt32(CommonComponent.GetScalarCommonData("insert into tb_PurchaseOrder(VendorID,PODate,AdditionalCost,Notes,OrderNumber,Adjustments,Tax,Shipping) values(" + VenderID + ",'" + Convert.ToDateTime(System.DateTime.Now) + "'," + Math.Round(AdditionalCost, 2) + ",'" + Notes + "',0," + Adjustments + "," + Tax + "," + Shipping + ") select scope_identity()"));
                Decimal PoAmt = AdditionalCost + Adjustments + Tax + Shipping;
                if (PONumber != 0)
                {
                    StringBuilder strQuery = new StringBuilder();
                    foreach (DataRow dr in DtVendor.Rows)
                    {
                        strQuery.Append(" insert into tb_PurchaseOrderItems(ProductID,Quantity,Price,PONumber) values(" + dr["ProductID"] + "," + dr["Quantity"].ToString() + "," + Math.Round(Convert.ToDecimal(dr["Price"].ToString()), 2) + "," + PONumber + ") ");
                        PoAmt += (Math.Round(Convert.ToDecimal(dr["Price"].ToString()), 2) * Convert.ToInt32(dr["Quantity"].ToString()));

                        ProductIDs.Add(dr["ProductID"].ToString());
                        Random r = new Random((int)DateTime.Now.Ticks);
                        CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseproduct WHERE Productid =" + dr["ProductID"].ToString() + "");
                    }
                    strQuery.Append("  update tb_PurchaseOrder set poamount=" + PoAmt + " where ponumber=" + PONumber + " ");
                    if (!string.IsNullOrEmpty(strQuery.ToString().Trim()))
                        CommonComponent.ExecuteCommonData(strQuery.ToString());
                }
                try
                {
                    CommonComponent.ExecuteCommonData("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber) VALUES (" + Session["AdminID"].ToString() + ",16," + Convert.ToInt32(PONumber) + "," + Convert.ToInt32(0) + ")");
                }
                catch { }

                if (EMailSubject.Contains("<PO_NUMBER>"))
                {
                    EMailSubject = Regex.Replace(EMailSubject, "<PO_NUMBER>", PONumber.ToString(), RegexOptions.IgnoreCase);
                }

                string subject = Regex.Replace(FillMailTemplateTextForVendor(EMailSubject), "FOR ORDER #", "", RegexOptions.IgnoreCase);
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
                BindVendorCartPdf(Request.QueryString["VendorID"].ToString(), PONumber);
                if (chkPdfFile.Checked == true)
                {
                    String PdgFilePath = Convert.ToString(AppLogic.AppConfigs("POFilesPath"));
                    CommonOperations.SendMailAttachment(EMailFrom, EMailTo.Replace(",", ";"), EMailCC, EMailBCC, Regex.Replace(FillMailTemplateTextForVendor(EMailSubject), "FOR ORDER #", "", RegexOptions.IgnoreCase), EMailBody, Request.UserHostAddress.ToString(), IsHtml, av, Server.MapPath(PdgFilePath + "PO_" + PONumber.ToString() + ".pdf").ToString());
                }
                else
                {
                    CommonOperations.SendMail(EMailFrom, EMailTo.Replace(",", ";"), EMailCC, EMailBCC, Regex.Replace(FillMailTemplateTextForVendor(EMailSubject), "FOR ORDER #", "", RegexOptions.IgnoreCase), EMailBody, Request.UserHostAddress.ToString(), IsHtml, null);
                }
                try
                {
                    CommonComponent.ExecuteCommonData("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber,CreatedOn) VALUES (" + Session["AdminID"].ToString() + ",27," + Convert.ToInt32(PONumber) + "," + Convert.ToInt32(Request.QueryString["ONo"]) + ",getdate())");
                    OrderComponent objAddOrder = new OrderComponent();
                    objAddOrder.InsertOrderlog(27, Convert.ToInt32(0), "", Convert.ToInt32(Session["AdminID"].ToString()));
                }
                catch { }
                CommonComponent.ExecuteCommonData("INSERT INTO tb_Warehousemail(Ponumber,EmailTo,Subject,Body,Filepath,EmailFrom,EmailCC,EMailBCC,VendorAddress) VALUES (" + PONumber.ToString() + ",'" + EMailTo.Replace(",", ";") + "','" + subject.Replace("'", "''") + "','" + EMailBody.Replace("'", "''") + "','','" + EMailFrom + "','" + EMailCC.Replace(",", ";") + "','" + EMailBCC.Replace(",", ";") + "','')");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@SaveMsg", "alert('Record Saved Successfully!'); window.location.href='WareHousePO.aspx';", true);
            }
        }

        /// <summary>
        /// Binds the vendor cart PDF.
        /// </summary>
        /// <param name="VendorID">string VendorID</param>
        /// <param name="ponumber">int ponumber.</param>
        private void BindVendorCartPdf(string VendorID, Int32 ponumber)
        {
            String strSource = "";
            string OrderNo = "";
            Int32 PoNumber = ponumber;
            string vendorname = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name From tb_Vendor Where ISNULL(Active,1)=1 and ISNULL(deleted,0)=0 and VendorId=" + Request.QueryString["VendorID"].ToString()));

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
                    catch { }

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
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(AppLogic.AppConfigs("LIVE_SERVER") + "/App_Themes/" + Page.Theme.ToString() + "/images/logo.gif");
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
                    //iTextSharp.text.Cell cell1 = new iTextSharp.text.Cell("" + OrderNo.ToString());
                    //cell1.BorderColor = new Color(0, 0, 255);
                    //cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell1.BorderWidth = 0;
                    //aTable1.AddCell(cell1);

                    // iTextSharp.text.Cell cellCustomername = new iTextSharp.text.Cell("Customer Name :  " + objOrder.FirstName + " " + objOrder.LastName.ToString());
                    // iTextSharp.text.Cell cellCustomername = new iTextSharp.text.Cell("");
                    //cellCustomername.BorderColor = new Color(0, 0, 255);
                    //cellCustomername.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cellCustomername.BorderWidth = 0;
                    //aTable1.AddCell(cellCustomername);


                    //iTextSharp.text.Cell cell11 = new iTextSharp.text.Cell("PO Number :      " + PoNumber.ToString());
                    //cell11.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell11.BorderColor = new Color(0, 0, 255);
                    //cell11.BorderWidth = 0;
                    //aTable1.AddCell(cell11);
                    iTextSharp.text.Cell cellShipTo = new iTextSharp.text.Cell("Ship To :  ");
                    cellShipTo.BorderColor = new Color(0, 0, 255);
                    cellShipTo.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellShipTo.BorderWidth = 0;
                    aTable1.AddCell(cellShipTo);

                    //iTextSharp.text.Cell cell111 = new iTextSharp.text.Cell("Vendor Name :   " + vendorname.ToString());
                    iTextSharp.text.Cell cell111 = new iTextSharp.text.Cell("");
                    cell111.BorderColor = new Color(0, 0, 255);
                    cell111.BorderWidth = 0;
                    aTable1.AddCell(cell111);

                    //iTextSharp.text.Cell cellShipname1 = new iTextSharp.text.Cell(objOrder.ShippingAddress.m_FirstName.ToString() + " " + objOrder.ShippingAddress.m_LastName.ToString());
                    //cellShipname1.BorderColor = new Color(0, 0, 255);
                    //cellShipname1.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cellShipname1.BorderWidth = 0;
                    //aTable1.AddCell(cellShipname1);
                    try
                    {
                        string Companyname = "Half Price Drapes";
                        if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginContactName").ToString()))
                            Companyname = AppLogic.AppConfigs("Shipping.OriginContactName").ToString();
                        iTextSharp.text.Cell cellShipname = new iTextSharp.text.Cell(Companyname);
                        cellShipname.BorderColor = new Color(0, 0, 255);
                        cellShipname.BorderWidth = 0;
                        aTable1.AddCell(cellShipname);

                        iTextSharp.text.Cell cellcompany = new iTextSharp.text.Cell("");
                        cellcompany.BorderColor = new Color(0, 0, 255);
                        cellcompany.BorderWidth = 0;

                        aTable1.AddCell(cellcompany);

                        string StrLine01 = "";
                        string StrLine02 = "";
                        string StrLine03 = "";

                        if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginAddress").ToString()))
                            StrLine01 = AppLogic.AppConfigs("Shipping.OriginAddress").ToString();
                        if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginAddress2").ToString()))
                            StrLine01 += "," + AppLogic.AppConfigs("Shipping.OriginAddress2").ToString();

                        if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginCity").ToString()))
                            StrLine02 = ", \r\n" + AppLogic.AppConfigs("Shipping.OriginCity").ToString();

                        if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginState").ToString()))
                            StrLine03 = ",  \r\n" + AppLogic.AppConfigs("Shipping.OriginState").ToString();

                        if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginZip").ToString()))
                            StrLine03 += " " + AppLogic.AppConfigs("Shipping.OriginZip").ToString();

                        //iTextSharp.text.Cell cellcompany1 = new iTextSharp.text.Cell("190 Veterans Dr., \r\nNorthvale,  \r\nNJ 07647");
                        iTextSharp.text.Cell cellcompany1 = new iTextSharp.text.Cell(StrLine01 + StrLine02 + StrLine03);
                        cellcompany1.BorderColor = new Color(0, 0, 255);
                        cellcompany1.BorderWidth = 0;
                        aTable1.AddCell(cellcompany1);

                        iTextSharp.text.Cell cellShipToAdd = new iTextSharp.text.Cell("");
                        cellShipToAdd.BorderColor = new Color(0, 0, 255);
                        cellShipToAdd.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellShipToAdd.BorderWidth = 0;
                        aTable1.AddCell(cellShipToAdd);

                        #region Comments

                        // aTable1.AddCell(cell112);
                        //iTextSharp.text.Cell cellcompany2 = new iTextSharp.text.Cell("");
                        //cellcompany2.BorderColor = new Color(0, 0, 255);
                        //cellcompany2.BorderWidth = 0;
                        //aTable1.AddCell(cellcompany2);

                        //iTextSharp.text.Cell cellShipToAdd2 = new iTextSharp.text.Cell(objOrder.ShippingAddress.m_Address2.ToString());
                        //cellShipToAdd2.BorderColor = new Color(0, 0, 255);
                        //cellShipToAdd2.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cellShipToAdd2.BorderWidth = 0;
                        //aTable1.AddCell(cellShipToAdd2);

                        //iTextSharp.text.Cell cellSuite = new iTextSharp.text.Cell("");
                        //cellSuite.BorderColor = new Color(0, 0, 255);
                        //cellSuite.BorderWidth = 0;
                        //aTable1.AddCell(cellSuite);

                        //iTextSharp.text.Cell cellSuite1 = new iTextSharp.text.Cell("Suite : " + objOrder.ShippingAddress.m_Suite.ToString());
                        //cellSuite1.BorderColor = new Color(0, 0, 255);
                        //cellSuite1.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cellSuite1.BorderWidth = 0;
                        //aTable1.AddCell(cellSuite1);


                        //iTextSharp.text.Cell cellcity = new iTextSharp.text.Cell("");
                        //cellcity.BorderColor = new Color(0, 0, 255);
                        //cellcity.BorderWidth = 0;
                        //aTable1.AddCell(cellcity);

                        //iTextSharp.text.Cell cellcity1 = new iTextSharp.text.Cell(objOrder.ShippingAddress.m_City.ToString());
                        //cellcity1.BorderColor = new Color(0, 0, 255);
                        //cellcity1.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cellcity1.BorderWidth = 0;
                        //aTable1.AddCell(cellcity1);

                        //iTextSharp.text.Cell cellstate = new iTextSharp.text.Cell("");
                        //cellstate.BorderColor = new Color(0, 0, 255);
                        //cellstate.BorderWidth = 0;
                        //aTable1.AddCell(cellstate);

                        //iTextSharp.text.Cell cellstate1 = new iTextSharp.text.Cell(objOrder.ShippingAddress.m_State.ToString() + "," + objOrder.ShippingAddress.m_Zip.ToString());
                        //cellstate1.BorderColor = new Color(0, 0, 255);
                        //cellstate1.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cellstate1.BorderWidth = 0;
                        //aTable1.AddCell(cellstate1);


                        //iTextSharp.text.Cell cellcountry = new iTextSharp.text.Cell("");
                        //cellcountry.BorderColor = new Color(0, 0, 255);
                        //cellcountry.BorderWidth = 0;
                        //aTable1.AddCell(cellcountry);

                        //iTextSharp.text.Cell cellcountry1 = new iTextSharp.text.Cell(objOrder.ShippingAddress.m_CountryName.ToString());
                        //cellcountry1.BorderColor = new Color(0, 0, 255);
                        //cellcountry1.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cellcountry1.BorderWidth = 0;
                        //aTable1.AddCell(cellcountry1);


                        //iTextSharp.text.Cell cellphone = new iTextSharp.text.Cell("");
                        //cellphone.BorderColor = new Color(0, 0, 255);
                        //cellphone.BorderWidth = 0;
                        //aTable1.AddCell(cellphone);

                        //iTextSharp.text.Cell cellphone1 = new iTextSharp.text.Cell(objOrder.ShippingAddress.m_Phone.ToString());
                        //cellphone1.BorderColor = new Color(0, 0, 255);
                        //cellphone1.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cellphone1.BorderWidth = 0;
                        //aTable1.AddCell(cellphone1);

                        #endregion
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

                    //iTextSharp.text.Cell cellPrice = new iTextSharp.text.Cell("Price");
                    //cellPrice.BorderColor = new Color(164, 164, 164);
                    //cellPrice.BackgroundColor = new Color(218, 218, 218);

                    //cellPrice.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cellPrice.HorizontalAlignment = Element.ALIGN_RIGHT;

                    //aTable.AddCell(cellPrice);

                    iTextSharp.text.Cell cellQty = new iTextSharp.text.Cell("Quantity");
                    cellQty.BorderColor = new Color(164, 164, 164);
                    cellQty.BackgroundColor = new Color(218, 218, 218);
                    cellQty.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellQty.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTable.AddCell(cellQty);

                    //iTextSharp.text.Cell cellSub = new iTextSharp.text.Cell("SubTotal");
                    //cellSub.BorderColor = new Color(164, 164, 164);
                    //cellSub.BackgroundColor = new Color(218, 218, 218);
                    //cellSub.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //cellSub.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //aTable.AddCell(cellSub);
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

                        //iTextSharp.text.Cell cellPrice1 = new iTextSharp.text.Cell("$" + dvVendor.Table.Rows[i]["Price"].ToString());
                        //cellPrice1.BorderColor = new Color(164, 164, 164);

                        //cellPrice1.VerticalAlignment = Element.ALIGN_CENTER;
                        //cellPrice1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //aTable.AddCell(cellPrice1);

                        iTextSharp.text.Cell cellQty1 = new iTextSharp.text.Cell(dvVendor.Table.Rows[i]["Quantity"].ToString());
                        cellQty1.BorderColor = new Color(164, 164, 164);
                        cellQty1.VerticalAlignment = Element.ALIGN_CENTER;
                        cellQty1.HorizontalAlignment = Element.ALIGN_CENTER;
                        aTable.AddCell(cellQty1);

                        //iTextSharp.text.Cell cellSub1 = new iTextSharp.text.Cell("$" + (Convert.ToDecimal(dvVendor.Table.Rows[i]["Price"].ToString()) * Convert.ToInt32(dvVendor.Table.Rows[i]["Quantity"].ToString())));
                        //cellSub1.BorderColor = new Color(164, 164, 164);
                        //cellSub1.VerticalAlignment = Element.ALIGN_CENTER;
                        //cellSub1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //aTable.AddCell(cellSub1);


                        //SubTotal += (Convert.ToDecimal(dvVendor.Table.Rows[i]["Price"].ToString()) * Convert.ToDecimal(dvVendor.Table.Rows[i]["Quantity"].ToString()));
                    }
                    //Total = SubTotal + AdditionalCost + Adjustments + Shipping + Tax;
                    //iTextSharp.text.Cell cellSubTotal = new iTextSharp.text.Cell("SubTotal :");
                    //cellSubTotal.BorderColor = new Color(164, 164, 164);
                    //cellSubTotal.Colspan = 4;
                    //cellSubTotal.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cellSubTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(cellSubTotal);
                    //iTextSharp.text.Cell cellSubTotalValue = new iTextSharp.text.Cell("$" + SubTotal.ToString());
                    //cellSubTotalValue.BorderColor = new Color(164, 164, 164);

                    //cellSubTotalValue.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cellSubTotalValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(cellSubTotalValue);

                    //iTextSharp.text.Cell Additional = new iTextSharp.text.Cell("Additional Cost :");
                    //Additional.BorderColor = new Color(164, 164, 164);
                    //Additional.Colspan = 4;
                    //Additional.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //Additional.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(Additional);
                    //iTextSharp.text.Cell Additionalval = new iTextSharp.text.Cell("$" + AdditionalCost.ToString());
                    //Additionalval.BorderColor = new Color(164, 164, 164);

                    //Additionalval.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //Additionalval.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(Additionalval);


                    //iTextSharp.text.Cell Sale = new iTextSharp.text.Cell("Sale Tax :");
                    //Sale.BorderColor = new Color(164, 164, 164);
                    //Sale.Colspan = 4;

                    //Sale.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //Sale.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(Sale);
                    //iTextSharp.text.Cell Saleval = new iTextSharp.text.Cell("$" + Tax.ToString());
                    //Saleval.BorderColor = new Color(164, 164, 164);

                    //Saleval.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //Saleval.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(Saleval);

                    //iTextSharp.text.Cell Shipping1 = new iTextSharp.text.Cell("Shipping :");
                    //Shipping1.BorderColor = new Color(164, 164, 164);
                    //Shipping1.Colspan = 4;
                    //Shipping1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //Shipping1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(Shipping1);
                    //iTextSharp.text.Cell Shippingval = new iTextSharp.text.Cell("$" + Shipping.ToString());
                    //Shippingval.BorderColor = new Color(164, 164, 164);

                    //Shippingval.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //Shippingval.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(Shippingval);

                    //iTextSharp.text.Cell Adjustments1 = new iTextSharp.text.Cell();

                    //if (Adjustments < 0)
                    //{
                    //    Adjustments1.Add(new Phrase("Adjustments(-) :"));

                    //}
                    //else
                    //{
                    //    Adjustments1.Add(new Phrase("Adjustments :"));
                    //}

                    ////iTextSharp.text.Cell Adjustments = new iTextSharp.text.Cell("Adjustments :");
                    //Adjustments1.BorderColor = new Color(164, 164, 164);
                    //Adjustments1.Colspan = 4;
                    //Adjustments1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //Adjustments1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(Adjustments1);
                    //iTextSharp.text.Cell Adjustmentsval = new iTextSharp.text.Cell();
                    //if (Adjustments < 0)
                    //{
                    //    Adjustmentsval.Add(new Phrase("$" + (-Adjustments)));
                    //}
                    //else
                    //{
                    //    Adjustmentsval.Add(new Phrase("$" + Adjustments));

                    //}

                    ////  iTextSharp.text.Cell Adjustmentsval = new iTextSharp.text.Cell("$12.00");
                    //Adjustmentsval.BorderColor = new Color(164, 164, 164);

                    //Adjustmentsval.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //Adjustmentsval.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(Adjustmentsval);


                    //iTextSharp.text.Cell Total1 = new iTextSharp.text.Cell("Total :");
                    //Total1.BorderColor = new Color(164, 164, 164);
                    //Total1.Colspan = 4;
                    //Total1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //Total1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(Total1);
                    //iTextSharp.text.Cell Totalval = new iTextSharp.text.Cell("$" + Total.ToString());
                    //Totalval.BorderColor = new Color(164, 164, 164);

                    //Totalval.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //Totalval.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //aTable.AddCell(Totalval);
                    document.Add(aTable);

                    try
                    {
                        iTextSharp.text.Table aTableNotes = new iTextSharp.text.Table(1);
                        aTableNotes.WidthPercentage = 100;
                        aTableNotes.BorderWidth = 0;
                        aTableNotes.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
                        string strNotes = "Notes: ";
                        if (Session["PONotes"] != null)
                        {
                            if (Session["PONotes"].ToString() == "")
                            {
                                strNotes += "N/A";
                            }
                            else
                            {
                                strNotes += System.Text.RegularExpressions.Regex.Replace(Session["PONotes"].ToString().Trim().Replace("&nbsp;", "").Replace("\r\n\r\n", "\r\n"), @"<[^>]*>", String.Empty);
                            }
                        }
                        else
                        {
                            strNotes += "N/A";
                        }

                        iTextSharp.text.Cell cNotes = new iTextSharp.text.Cell(strNotes);
                        cNotes.BorderColor = new Color(164, 164, 164);
                        cNotes.BorderWidth = 0;
                        cNotes.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cNotes.HorizontalAlignment = Element.ALIGN_LEFT;
                        aTableNotes.AddCell(cNotes);
                        document.Add(aTableNotes);
                    }
                    catch { }

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

                    //iTextSharp.text.Cell visitSite = new iTextSharp.text.Cell(AppLogic.AppConfigs("LIVE_Server_Name").ToString());
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