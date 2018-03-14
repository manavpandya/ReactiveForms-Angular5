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
    public partial class VendorQuoteMailFormat : BasePage
    {
        DataSet dsTemplate;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnPrint.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/print.png";
            btnSendmailToVendor.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/send-mail-to-vendor(s).png";

            Int32 storeid = 0;
            Int32.TryParse(AppLogic.AppConfigs("StoreID").ToString(), out storeid);
            if (storeid == 0)
                storeid = 1;

            dsTemplate = new DataSet();
            dsTemplate = CommonComponent.GetCommonDataSet("Select Isnull(EmailType,1) as EmailType,* From tb_EmailTemplate Where Label ='VendorQuote Template' and StoreID=" + storeid + "");
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower().Trim() == "view")
            {
                String EMailSubject = string.Empty;
                String EMailBody = string.Empty;
                Boolean IsHtml = false;

                if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
                {
                    string Vendors = Request.QueryString["Vendors"].ToString();
                    string[] vendorid = Vendors.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    if (dsTemplate.Tables[0].Rows[0]["Subject"] != null && dsTemplate.Tables[0].Rows[0]["Subject"].ToString().Trim() != "")
                        EMailSubject = dsTemplate.Tables[0].Rows[0]["Subject"].ToString().Trim();

                    if (dsTemplate.Tables[0].Rows[0]["EMailBody"] != null && dsTemplate.Tables[0].Rows[0]["EMailBody"].ToString().Trim() != "")
                    {
                        EMailBody = dsTemplate.Tables[0].Rows[0]["EMailBody"].ToString().Trim();
                    }

                    if (dsTemplate.Tables[0].Rows[0]["EmailType"] != null && dsTemplate.Tables[0].Rows[0]["EmailType"].ToString().Trim() != "")
                        IsHtml = Convert.ToBoolean(Convert.ToInt32(dsTemplate.Tables[0].Rows[0]["EmailType"].ToString().Trim()));

                    EMailSubject = Regex.Replace(FillMailTemplateTextForVendor(EMailSubject, vendorid[0]), "FOR ORDER #", "", RegexOptions.IgnoreCase);
                    EMailBody = FillMailTemplateTextForVendor(EMailBody, vendorid[0]);
                    txtDescription.Text = EMailBody;
                    if (IsHtml)
                    {
                        divScript.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Fills the mail template text for vendor.
        /// </summary>
        /// <param name="strText">string strText</param>
        /// <param name="VendorID">string VendorID</param>
        /// <returns>Returns the output value as a string format which contains HTML.</returns>
        private String FillMailTemplateTextForVendor(String strText, String VendorID)
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

            strSource = Regex.Replace(strSource, "###ContactMail_ToAddress###", AppLogic.AppConfigs("ContactMail_ToAddress").ToString(), RegexOptions.IgnoreCase);

            dsVen = CommonComponent.GetCommonDataSet("Select * From tb_Vendor Where VendorId=" + VendorID + "");
            string StrVendorDetails = "";
            if (dsVen != null && dsVen.Tables.Count > 0 && dsVen.Tables[0].Rows.Count > 0)
            {
                StrVendorDetails = "<table cellspacing='0' cellpadding='0' border='0' align='left' style='Padding-bottom:10px;Padding-Left:0px;' class='popup_cantain'><tbody>";
                StrVendorDetails += "<tr><td height='25' align='left' valign='bottom' class='popup_cantain'>Supplier Name: " + dsVen.Tables[0].Rows[0]["Name"].ToString() + "</td></tr>";

                if (dsVen.Tables[0].Rows[0]["Address"] != null && !string.IsNullOrEmpty(dsVen.Tables[0].Rows[0]["Address"].ToString()) && dsVen.Tables[0].Rows[0]["Address"].ToString().Trim() != "")
                    StrVendorDetails += "<tr><td height='25' align='left' valign='bottom' class='popup_cantain'>Supplier Address: " + dsVen.Tables[0].Rows[0]["Address"].ToString() + "</td></tr>";

                if (dsVen.Tables[0].Rows[0]["City"] != null && dsVen.Tables[0].Rows[0]["City"].ToString().Trim() != "")
                    StrVendorDetails += "<tr><td height='25' align='left' valign='bottom' class='popup_cantain'>Supplier City: " + dsVen.Tables[0].Rows[0]["City"].ToString() + "</td></tr>";

                if (dsVen.Tables[0].Rows[0]["ZipCode"] != null && dsVen.Tables[0].Rows[0]["ZipCode"].ToString().Trim() != "")
                    StrVendorDetails += "<tr><td height='25' align='left' valign='bottom' class='popup_cantain'>Supplier Zip Code: " + dsVen.Tables[0].Rows[0]["ZipCode"].ToString() + "</td></tr>";

                StrVendorDetails += "</tbody></table>";
            }

            if (strSource.Contains("###VENDOR_DETAILS###"))
                strSource = Regex.Replace(strSource, "###VENDOR_DETAILS###", StrVendorDetails.ToString(), RegexOptions.IgnoreCase);

            if (strSource.Contains("###ORDER_ITEMS###"))
                strSource = Regex.Replace(strSource, "###ORDER_ITEMS###", BindVendorCart(VendorID.ToString()), RegexOptions.IgnoreCase);

            string StrQuoteLink = "";
            StrQuoteLink = "<b>To submit quote please<a target='_blank' style='color:red;padding-left:2px;' href='" + AppLogic.AppConfigs("LIVE_SERVER") + "/VendorQuoteReply.aspx?vid=" + Server.UrlEncode(SecurityComponent.Encrypt(VendorID)) + "###ReqId###'>click here</a>.</b>";
            try
            {
                strSource = Regex.Replace(strSource, "###QUOTE_LINKS###", StrQuoteLink.ToString(), RegexOptions.IgnoreCase);
            }
            catch { }

            if (Session["VendorQuoteNotes"] != null)
            {
                try
                {
                    strSource = Regex.Replace(strSource, "###NOTES###", "<b>Notes : </b>" + Session["VendorQuoteNotes"].ToString(), RegexOptions.IgnoreCase);
                }
                catch { }
            }
            else
            {
                strSource = Regex.Replace(strSource, "###NOTES###", "", RegexOptions.IgnoreCase);
            }

            return strSource;
        }

        /// <summary>
        /// Binds the vendor cart.
        /// </summary>
        /// <param name="VendorID">string VendorID</param>
        /// <returns>Returns the output value as a string format which contains HTML.</returns>
        private String BindVendorCart(string VendorID)
        {
            if (Session["VendorQuoteCart"] != null)
            {
                ViewState["VendorQuoteCart"] = Session["VendorQuoteCart"];
                DataTable DtVendor = (DataTable)Session["VendorQuoteCart"];
                DataView dvVendor = DtVendor.DefaultView;
                StringBuilder sb = new StringBuilder();
                String strLiveServer = AppLogic.AppConfigs("LIVE_SERVER");

                DataRow[] rows = DtVendor.Select(" VendorIDs like '" + VendorID.ToString() + ",%' or VendorIDs like '%," + VendorID.ToString() + ",%' ");
                sb.Append("<table width='99%' cellspacing='0' cellpadding='0' border='1' style='padding: 10px 0 0;border: 1px solid #ECECEC; border-collapse: collapse; color: #212121; font: 12px;'>");
                sb.Append("<tr style='background-color: rgb(242,242,242);'>");
                sb.Append("<th valign='middle' align='Center' style='padding-left:5px;width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>Package ID</th>");
                sb.Append("<th valign='middle' style='width:50%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>Name</th>");
                sb.Append("<th valign='middle' align='Center' style='width:14%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>SKU</th>");
                sb.Append("<th valign='middle' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='center'>Quantity</th>");
                sb.Append("<th valign='middle' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;padding-right:5px;' align='right'>Price</th>");
                sb.Append("</tr>");

                for (int i = 0; i < rows.Length; i++)
                {
                    sb.Append("<tr>");
                    sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>");
                    sb.Append(i + 1);
                    sb.Append("</td>");
                    sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;'>");
                    sb.Append(rows[i]["Name"].ToString());
                    if (!string.IsNullOrEmpty(rows[i]["ProductOption"].ToString()))
                    {
                        sb.Append("<br/>");
                        sb.Append(Server.HtmlDecode(rows[i]["ProductOption"].ToString()));
                    }
                    sb.Append("</td>");
                    sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>");
                    sb.Append(rows[i]["SKU"].ToString());
                    sb.Append("</td>");
                    sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>");
                    sb.Append(rows[i]["Quantity"].ToString());
                    sb.Append("</td>");
                    sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:right;'>");
                    sb.Append(Convert.ToDecimal(rows[i]["Price"].ToString()).ToString("c"));
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                return sb.ToString();
            }
            else
                return string.Empty;
        }

        /// <summary>
        ///  Send mail to Vendor Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSendmailToVendor_Click(object sender, ImageClickEventArgs e)
        {
            Int32 storeid = 0;
            Int32.TryParse(AppLogic.AppConfigs("StoreID").ToString(), out storeid);
            if (storeid == 0)
                storeid = 1;

            dsTemplate = new DataSet();
            dsTemplate = CommonComponent.GetCommonDataSet("Select Isnull(EmailType,1) as EmailType,* From tb_EmailTemplate Where Label ='VendorQuote Template' and StoreID=" + storeid + "");
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower().Trim() == "view")
            {
                String EMailSubject = string.Empty;
                String EMailBody = string.Empty;
                String EMailFrom = string.Empty;
                String EMailTo = string.Empty;
                Boolean IsHtml = false;
                Int32 VendorQuoteReqId = 0;
                string Notes = "";
                if (Session["VendorQuoteNotes"] != null && Session["VendorQuoteNotes"].ToString().Trim() != "")
                {
                    Notes = Session["VendorQuoteNotes"].ToString().Trim();
                }

                if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
                {
                    string Vendors = Request.QueryString["Vendors"].ToString();
                    string[] vendorid = Vendors.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < vendorid.Length; i++)
                    {
                        if (dsTemplate.Tables[0].Rows[0]["EmailFrom"] != null && dsTemplate.Tables[0].Rows[0]["EmailFrom"].ToString().Trim() != "")
                            EMailFrom = dsTemplate.Tables[0].Rows[0]["EmailFrom"].ToString().Trim();
                        else
                            EMailFrom = AppLogic.AppConfigs("MailFrom");

                        if (dsTemplate.Tables[0].Rows[0]["Subject"] != null && dsTemplate.Tables[0].Rows[0]["Subject"].ToString().Trim() != "")
                            EMailSubject = dsTemplate.Tables[0].Rows[0]["Subject"].ToString().Trim();

                        if (dsTemplate.Tables[0].Rows[0]["EMailBody"] != null && dsTemplate.Tables[0].Rows[0]["EMailBody"].ToString().Trim() != "")
                        {
                            EMailBody = dsTemplate.Tables[0].Rows[0]["EMailBody"].ToString().Trim();
                        }

                        if (dsTemplate.Tables[0].Rows[0]["EmailType"] != null && dsTemplate.Tables[0].Rows[0]["EmailType"].ToString().Trim() != "")
                            IsHtml = Convert.ToBoolean(Convert.ToInt32(dsTemplate.Tables[0].Rows[0]["EmailType"].ToString().Trim()));

                        EMailSubject = Regex.Replace(FillMailTemplateTextForVendor(EMailSubject, vendorid[0]), "FOR ORDER #", "", RegexOptions.IgnoreCase);
                        EMailBody = FillMailTemplateTextForVendor(EMailBody, vendorid[i]);
                        txtDescription.Text = EMailBody;
                        if (IsHtml)
                        {
                            divScript.Visible = true;
                        }
                        EMailTo = Convert.ToString(CommonComponent.GetScalarCommonData("Select Email From tb_vendor Where vendorid=" + vendorid[i].ToString() + ""));
                        AlternateView av = AlternateView.CreateAlternateViewFromString(EMailBody, null, "text/html");
                        int VendorQuoteid = 0;
                        string VendorQuoteids = string.Empty;

                        if (Session["VendorQuoteCart"] != null)
                        {
                            DataTable DtVendor = (DataTable)Session["VendorQuoteCart"];
                            DataRow[] rows = DtVendor.Select(" VendorIDs like '" + vendorid[i].ToString() + ",%' or VendorIDs like '%," + vendorid[i].ToString() + ",%' ");


                            if (VendorQuoteReqId == 0)
                                VendorQuoteReqId = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO [dbo].[tb_VendorQuoteRequest]([RequestedOn],[MailLogid],[Notes])VALUES(GETDATE(),0,'" + Notes.Replace("'", "''") + "'); Select SCOPE_IDENTITY();"));

                            for (int k = 0; k < rows.Length; k++)
                            {
                                VendorComponent objvendor = new VendorComponent();
                                if (VendorQuoteReqId > 0)
                                {
                                    VendorQuoteid = objvendor.SaveVendorQuoteRequest(VendorQuoteReqId, Convert.ToInt32(vendorid[i].ToString()), Convert.ToInt32(rows[k]["ProductID"].ToString()), Convert.ToInt32(rows[k]["Quantity"].ToString()), Convert.ToString(rows[k]["Name"].ToString()), Convert.ToString(rows[k]["ProductOption"].ToString()), Notes.ToString());
                                }
                            }
                        }

                        if (VendorQuoteReqId > 0)
                            EMailBody = Regex.Replace(EMailBody, "###ReqId###", "&VQuoteReqId=" + Server.UrlEncode(SecurityComponent.Encrypt(VendorQuoteReqId.ToString())), RegexOptions.IgnoreCase);
                        else
                            EMailBody = Regex.Replace(EMailBody, "###ReqId###", "", RegexOptions.IgnoreCase);

                        Int32 Mainlog = Convert.ToInt32(CommonOperations.SendMail(EMailFrom, EMailTo.Replace(",", ";"), "", "", Regex.Replace(FillMailTemplateTextForVendor(EMailSubject, vendorid[i].ToString()), "FOR ORDER #", "", RegexOptions.IgnoreCase), EMailBody, Request.UserHostAddress.ToString(), IsHtml, null));
                        CommonComponent.ExecuteCommonData("UPDATE tb_VendorQuoteRequestDetails SET MailLogid=" + Mainlog + " WHERE VendorQuoteRequestID in (" + VendorQuoteReqId + ") and VendorId=" + vendorid[i].ToString() + "");
                    }
                    Response.Redirect("WareHousePO.aspx");
                }
            }
        }
    }
}