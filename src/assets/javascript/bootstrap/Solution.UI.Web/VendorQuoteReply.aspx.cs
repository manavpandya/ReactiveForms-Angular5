using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components.Common;
using System.Net.Mail;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace Solution.UI.Web
{
    public partial class VendorQuoteReply : System.Web.UI.Page
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
                if (Request.QueryString["vid"] != null && Request.QueryString["vid"].ToString().Length > 0)
                {
                    Int32 VendorId = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["vid"]));
                    if (Request.QueryString["VQuoteReqId"] != null && Request.QueryString["VQuoteReqId"].Length > 0)
                    {
                        Int32 VQuoteReqId = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["VQuoteReqId"]));
                        BindCart(VendorId, VQuoteReqId);
                    }
                }
                else
                {
                    btnsubmit.Visible = false;
                }
            }
        }

        /// <summary>
        /// Gets Vendor Quote Replay Data
        /// </summary>
        /// <param name="VendorId">int VendorId</param>
        /// <param name="VQuoteReqId">int VQuoteReqId</param>
        /// <returns>Returns Vendor Quote Replay Details as a String</returns>
        private String GetQuery(Int32 VendorId, Int32 VQuoteReqId)
        {
            string StrQry = " Select ISNULL(vqr.VendorQuoteRequestID,0) as ChkVendorReqId,tb_Vendor.name as 'VendorName',tb_VendorQuoteRequestDetails.VendorQuoteRequestID as VendorQuoteID,tb_Vendor.VendorID,  p.SEName,case when(isnull(tb_VendorQuoteRequestDetails.ProductName,'')='') then p.Name else tb_VendorQuoteRequestDetails.ProductName end  as 'Name', tb_VendorQuoteRequestDetails.ProductOption,p.ProductID,p.Name,p.SKU,ISNULL(p.Inventory,0) as Qty,ISNULL(tb_VendorQuoteRequestDetails.Quantity,0) as Quantity ,ISNULL(vqr.Quantity,0) as QtyReply,isnull(vqr.AvailableDays,'') as AvailableDays,convert(varchar(20),round(isnull(vqr.price,''),2)) as price,tb_Vendor.address,tb_Vendor.Email,tb_Vendor.phone,ISNULL(vqr.notes,'') as notes,ISNULL(vqr.location,'') as location " +
                            " from tb_VendorQuoteRequestDetails  " +
                            " inner join dbo.tb_Vendor on tb_VendorQuoteRequestDetails.vendorid=tb_Vendor.vendorid    " +
                            " INNER JOIN tb_Product p ON tb_VendorQuoteRequestDetails.ProductID = p.ProductID   " +
                            " left outer join tb_VendorQuoteReply vqr on vqr.VendorQuoteRequestID =tb_VendorQuoteRequestDetails.VendorQuoteRequestID  " +
                            " and vqr.RefProductID=tb_VendorQuoteRequestDetails.ProductId and vqr.VendorID=tb_VendorQuoteRequestDetails.VendorID " +
                            " where tb_Vendor.VendorID = " + VendorId + " and tb_VendorQuoteRequestDetails.VendorQuoteRequestID=" + VQuoteReqId + "";
            return StrQry;
        }

        /// <summary>
        /// Binds the Cart for Vendor Quote Replay Data
        /// </summary>
        /// <param name="VendorId">int VendorId</param>
        /// <param name="VQuoteReqId">int VQuoteReqId</param>
        private void BindCart(Int32 VendorId, Int32 VQuoteReqId)
        {
            DataSet ds = new DataSet();
            DataSet dsRequestDetails = new DataSet();
            dsRequestDetails = CommonComponent.GetCommonDataSet("Select * from tb_VendorQuoteRequest where VendorQuoteRequestID=" + VQuoteReqId + "");
            if (dsRequestDetails != null && dsRequestDetails.Tables.Count > 0 && dsRequestDetails.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsRequestDetails.Tables[0].Rows[0]["Notes"].ToString()))
                {
                    trRequestNotes.Visible = true;
                    lblRequestNotes.Text = dsRequestDetails.Tables[0].Rows[0]["Notes"].ToString();
                }
            }

            string StrQry = Convert.ToString(GetQuery(VendorId, VQuoteReqId));
            ds = CommonComponent.GetCommonDataSet(StrQry);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                gvVendor.DataSource = ds;
                gvVendor.DataBind();
                btnsubmit.Visible = true;
                trVendordetails.Visible = true;
                ltvname.Text = ds.Tables[0].Rows[0]["VendorName"].ToString();
                ltvphone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                ltvaddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["Email"].ToString()))
                {
                    ltvemail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                }
                else
                {
                    ltvemail.Text = "N/A";
                }
                trNotes.Visible = true;
                txtNotes.Text = Convert.ToString(ds.Tables[0].Rows[0]["Notes"].ToString());
                txtLocation.Text = Convert.ToString(ds.Tables[0].Rows[0]["Location"].ToString());
                ltvphone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
            }
            else
            {
                trVendordetails.Visible = false;
                btnsubmit.Visible = false;
                trNotes.Visible = false;
                lblMsg.Text = "No Record found...";
            }
        }

        /// <summary>
        /// Vendor Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvVendor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddl = (DropDownList)e.Row.FindControl("drpAvailDays");
                    TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
                    TextBox txtPrice = (TextBox)e.Row.FindControl("txtPrice");
                    CheckBox chklist = (CheckBox)e.Row.FindControl("chkid");
                    Label lblChkVendorReqId = (Label)e.Row.FindControl("lblChkVendorReqId");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdndays = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("availdays");

                    if (!string.IsNullOrEmpty(lblChkVendorReqId.Text.ToString()) && Convert.ToInt32(lblChkVendorReqId.Text) > 0)
                    {
                        ddl.Enabled = false;
                        chklist.Enabled = false;
                        txtQuantity.Enabled = false;
                        txtPrice.Enabled = false;
                    }
                    if (hdndays.Value.ToString() != "" && Convert.ToInt32(hdndays.Value.ToString()) != 0)
                    {
                        ddl.SelectedValue = hdndays.Value.ToString();
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            string TOAddress = AppLogic.AppConfigs("ContactMail_ToAddress");
            Boolean flag = true;
            foreach (GridViewRow row in gvVendor.Rows)
            {
                TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                Label lblQuantity = (Label)row.FindControl("lblQuantity");
                Label lblName = (Label)row.FindControl("lblName");
                try
                {
                    int qty = 0;
                    int iOrgQuantity = 0;
                    int iNewQuantity = 0;
                    int.TryParse(lblQuantity.Text, out iOrgQuantity);
                    int.TryParse(txtQuantity.Text, out iNewQuantity);
                    if (iNewQuantity > iOrgQuantity)
                    {
                        lblMsg.Text = "Quantity entered is invalid for " + lblName.Text + ".<br/> Please provide " + iOrgQuantity + " or less.";
                        return;
                    }
                    Int32.TryParse(txtQuantity.Text.Trim(), out qty);
                }
                catch
                {
                }
            }
            int Cnt = 0;
            if (flag)
            {
                String vendorname = "";
                foreach (GridViewRow row in gvVendor.Rows)
                {
                    try
                    {
                        Label lblVendorQuote = (Label)row.FindControl("lblVendorQuote");
                        Label lblVendorName = (Label)row.FindControl("lblVendorName");
                        Label lblVendorId = (Label)row.FindControl("lblVendorId");
                        Label lblOrderNo = (Label)row.FindControl("lblOrderNo");
                        Label lblProductID = (Label)row.FindControl("lblProductID");
                        Label lblName = (Label)row.FindControl("lblName");
                        Label lblProOption = (Label)row.FindControl("lblProOption");

                        Label lblSKU = (Label)row.FindControl("lblSKU");
                        Label lblQuantity = (Label)row.FindControl("lblQuantity");
                        TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                        TextBox txtPrice = (TextBox)row.FindControl("txtPrice");
                        CheckBox chklist = (CheckBox)row.FindControl("chkid");
                        DropDownList drpAvailDays = (DropDownList)row.FindControl("drpAvailDays");
                        if (chklist.Checked == true)
                        {
                            Cnt++;
                            if (Convert.ToInt32(txtQuantity.Text) <= 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgqty", "alert('Quantity should be greater then zero.');", true);
                                return;
                            }
                            if (Convert.ToDecimal(txtPrice.Text) <= 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgPrice", "alert('Please enter valid price.');", true);
                                return;
                            }

                            int iOrgQuantity = 0;
                            int iNewQuantity = 0;
                            int.TryParse(lblQuantity.Text, out iOrgQuantity);
                            int.TryParse(txtQuantity.Text, out iNewQuantity);
                            if (iNewQuantity > iOrgQuantity)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgproqty", "alert(@'Quantity entered is invalid for " + lblName.Text + ".<br/> Please provide " + iOrgQuantity + " or less.');", true);
                                return;
                            }

                            if (iNewQuantity != 0)
                            {
                                Int32 VQuoteReqId = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["VQuoteReqId"]));
                                VendorComponent objvendor = new VendorComponent();
                                objvendor.SaveVendorQuoteReply(Convert.ToInt32(VQuoteReqId), Convert.ToInt32(lblVendorId.Text.Trim()), Convert.ToInt32(lblProductID.Text.Trim()), iNewQuantity, Convert.ToString(lblName.Text.Trim()), Convert.ToString(lblProOption.Text.Trim()), txtNotes.Text.ToString(), Convert.ToDecimal(txtPrice.Text.Trim()), Convert.ToInt32(drpAvailDays.SelectedValue.ToString()), txtLocation.Text.ToString().Replace("'", "''"));
                                if (string.IsNullOrEmpty(vendorname.Trim()))
                                    vendorname = lblVendorName.Text;
                            }
                        }
                    }
                    catch { }
                }
                if (Cnt > 0)
                {
                    SendMail(TOAddress);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@recordmsg", "alert('Select atleast one record(s)!');", true);
                    return;
                }
            }
        }


        /// <summary>
        /// Sends the Mail to Vendor
        /// </summary>
        /// <param name="TOAddress">String TOAddress</param>
        public void SendMail(String TOAddress)
        {
            DataSet dsTemplate = new DataSet();

            Int32 VendorId = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["vid"]));
            dsTemplate = CommonComponent.GetCommonDataSet("Select Isnull(EmailType,1) as EmailType,* From tb_EmailTemplate Where Label ='VendorQuoteReplyTemplate'");
            if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
            {
                String EMailSubject = string.Empty;
                String EMailBody = string.Empty;
                String EMailFrom = string.Empty;
                String EMailTo = string.Empty;
                Boolean IsHtml = false;

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

                EMailSubject = Regex.Replace(FillMailTemalateTextForVender(EMailSubject, VendorId.ToString()), "", "", RegexOptions.IgnoreCase);
                EMailBody = FillMailTemalateTextForVender(EMailBody, VendorId.ToString());
                EMailTo = Convert.ToString(CommonComponent.GetScalarCommonData("Select Email From tb_vendor Where vendorid=" + VendorId.ToString() + ""));

                AlternateView av = AlternateView.CreateAlternateViewFromString(EMailBody, null, "text/html");
                CommonOperations.SendMail(EMailFrom, EMailTo.Replace(",", ";"), "", "", Regex.Replace(FillMailTemalateTextForVender(EMailSubject, VendorId.ToString()), "", "", RegexOptions.IgnoreCase), EMailBody, Request.UserHostAddress.ToString(), IsHtml, null);

                maintbl.Visible = false;
                ltrSaveMsg.Text = "<div style=\"padding-bottom: 5px; text-align: center; padding-top: 50px;color:#ff0000;\"><center>Your quote has been submitted successfully.<br />We will mail you Purchase Order information soon if your quote fulfills our requirements.</center></div>";
            }
        }

        /// <summary>
        /// Fills the Mail Template Text for Vendor.
        /// </summary>
        /// <param name="strText">String strText</param>
        /// <param name="VendorID">String VendorID</param>
        /// <returns>Returns  the Mail Formate for Vendor Email</returns>
        private String FillMailTemalateTextForVender(String strText, String VendorID)
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
            if (dsVen != null && dsVen.Tables[0].Rows.Count > 0)
            {
                StrVendorDetails = "<table cellspacing='0' cellpadding='0' border='0' align='left' style='Padding-bottom:10px;Padding-Left:0px;' class='popup_cantain'><tbody>";
                StrVendorDetails += "<tr><td height='25' align='left' valign='bottom' class='popup_cantain'>Supplier Name: " + dsVen.Tables[0].Rows[0]["Name"].ToString() + "</td></tr>";

                strSource = Regex.Replace(strSource, "###Vendor_Name###", dsVen.Tables[0].Rows[0]["Name"].ToString(), RegexOptions.IgnoreCase);

                if (dsVen.Tables[0].Rows[0]["Address"] != null && dsVen.Tables[0].Rows[0]["Address"].ToString().Trim() != "")
                    StrVendorDetails += "<tr><td height='25' align='left' valign='bottom' class='popup_cantain'>Supplier Address: " + dsVen.Tables[0].Rows[0]["Address"].ToString() + "</td></tr>";

                if (dsVen.Tables[0].Rows[0]["City"] != null && dsVen.Tables[0].Rows[0]["City"].ToString().Trim() != "")
                    StrVendorDetails += "<tr><td height='25' align='left' valign='bottom' class='popup_cantain'>Supplier City: " + dsVen.Tables[0].Rows[0]["City"].ToString() + "</td></tr>";

                if (dsVen.Tables[0].Rows[0]["ZipCode"] != null && dsVen.Tables[0].Rows[0]["ZipCode"].ToString().Trim() != "")
                    StrVendorDetails += "<tr><td height='25' align='left' valign='bottom' class='popup_cantain'>Supplier ZipCode: " + dsVen.Tables[0].Rows[0]["ZipCode"].ToString() + "</td></tr>";

                StrVendorDetails += "</tbody></table>";
            }
            else
            {
                strSource = Regex.Replace(strSource, "###Vendor_Name###", "", RegexOptions.IgnoreCase);
            }

            if (strSource.Contains("###VENDOR_DETAILS###"))
                strSource = Regex.Replace(strSource, "###VENDOR_DETAILS###", StrVendorDetails.ToString(), RegexOptions.IgnoreCase);

            if (strSource.Contains("###ORDER_ITEMS###"))
                strSource = Regex.Replace(strSource, "###ORDER_ITEMS###", BindVendorCart(VendorID.ToString()), RegexOptions.IgnoreCase);

            string StrQuoteLink = "";
            StrQuoteLink = "<b>To submit quote please<a target='_blank' style='color:red' href='" + AppLogic.AppConfigs("LIVE_SERVER") + "/VendorQuoteReply.aspx?vid=" + Server.UrlEncode(SecurityComponent.Encrypt(VendorID)) + "'> click here</a>.</b>";
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
        /// Binds the Vendor Cart
        /// </summary>
        /// <param name="VendorID">String VendorID</param>
        /// <returns>Returns the output value as a string format which contains HTML</returns>
        private String BindVendorCart(string VendorID)
        {
            StringBuilder sb = new StringBuilder();
            String strLiveServer = AppLogic.AppConfigs("LIVE_SERVER");

            sb.Append("<table width='99%' cellspacing='0' cellpadding='0' border='1' style='padding: 10px 0 0;border: 1px solid #ECECEC; border-collapse: collapse; color: #212121; font: 12px;'>");
            sb.Append("<tr style='background-color: rgb(242,242,242);'>");
            sb.Append("<th valign='middle'  style='padding-left:5px;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>Product Name</th>");
            sb.Append("<th valign='middle' style='width:15%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='Center'>SKU</th>");
            sb.Append("<th valign='middle' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='center'>Quantity</th>");
            sb.Append("<th valign='middle' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;padding-right:5px;' align='right'>Price</th>");
            sb.Append("<th valign='middle' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='center'>Available<br/>[in Days]</th>");
            sb.Append("</tr>");

            foreach (GridViewRow row in gvVendor.Rows)
            {
                try
                {
                    Label lblVendorName = (Label)row.FindControl("lblVendorName");
                    Label lblName = (Label)row.FindControl("lblName");
                    Label lblSKU = (Label)row.FindControl("lblSKU");
                    TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                    TextBox txtPrice = (TextBox)row.FindControl("txtPrice");
                    DropDownList drpAvailDays = (DropDownList)row.FindControl("drpAvailDays");
                    CheckBox chkid = (CheckBox)row.FindControl("chkid");
                    Label lblProOption = (Label)row.FindControl("lblProOption");

                    if (chkid.Checked == true)
                    {
                        if (txtQuantity.Text.Trim() != "0")
                        {
                            sb.Append("<tr>");
                            if (!string.IsNullOrEmpty(lblProOption.Text.Trim()))
                                sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:Left;'>" + lblName.Text.Trim() + "<br/>Product Options :" + lblProOption.Text.Trim() + "</td>");
                            else
                                sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:Left;'>" + lblName.Text.Trim() + "</td>");

                            sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>" + lblSKU.Text.Trim() + "</td>");
                            sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>" + txtQuantity.Text.Trim() + "</td>");
                            sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:right;'>$" + txtPrice.Text.Trim() + "</td>");
                            sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>" + drpAvailDays.SelectedValue.ToString() + "</td>");
                            sb.Append("</tr>");
                        }
                    }
                }
                catch { }
            }
            sb.Append("</table>");
            return sb.ToString();
        }
    }
}