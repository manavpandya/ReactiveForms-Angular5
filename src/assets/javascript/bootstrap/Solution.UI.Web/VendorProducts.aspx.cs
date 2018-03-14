using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Net.Mail;
using System.Text;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;

using ArrayList = System.Collections.ArrayList;
using System.Text.RegularExpressions;

namespace Solution.UI.Web
{
    public partial class VendorProducts : System.Web.UI.Page
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
                if (Request.QueryString["pono"] != null && Request.QueryString["pono"].ToString().Length > 0)
                {
                    BindCart();
                    if (Request.QueryString["StoreId"] != null)
                    {
                        imgStoreLogo.Src = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("LIVE_SERVER") + "/images/Store_" + Request.QueryString["StoreId"].ToString() + ".png";
                    }
                }
                else
                {
                    btnsubmit.Visible = false;
                }
            }
        }

        /// <summary>
        /// Gets the Vendor Products for Warehouse
        /// </summary>
        /// <param name="Pono">int Pono</param>
        /// <returns>Returns the Vendor Product for Warehouse</returns>
        public DataSet GetVendorProductsforWarehouse(int Pono)
        {
            return CommonComponent.GetCommonDataSet(" SELECT dbo.tb_Product.Name, dbo.tb_PurchaseOrderItems.Quantity, dbo.tb_PurchaseOrderItems.PONumber, dbo.tb_PurchaseOrder.OrderNumber, " +
                                                    " dbo.tb_PurchaseOrderItems.ProductID, dbo.tb_PurchaseOrderItems.Price, ISNULL(dbo.tb_PurchaseOrderItems.IsShipped, 0) AS IsShipped,  " +
                                                    " ISNULL(dbo.tb_PurchaseOrderItems.Ispaid, 0) AS Ispaid, dbo.tb_Product.SKU, dbo.tb_Vendor.Name AS Vname, dbo.tb_Vendor.Email,  " +
                                                    " dbo.tb_PurchaseOrder.OrderNumber AS Expr1, dbo.tb_PurchaseOrderItems.TrackingNumber, dbo.tb_PurchaseOrder.PODate, dbo.tb_Vendor.Phone,  " +
                                                    " ISNULL(dbo.tb_PurchaseOrderItems.ShippedVia, '') AS ShippedVia, ISNULL(dbo.tb_OrderShippedItems.ShippedOn, '') AS ShippedOn " +
                                                    " FROM  dbo.tb_Product INNER JOIN   dbo.tb_PurchaseOrderItems ON dbo.tb_Product.ProductID = dbo.tb_PurchaseOrderItems.ProductID INNER JOIN " +
                                                    " dbo.tb_PurchaseOrder ON dbo.tb_PurchaseOrderItems.PONumber = dbo.tb_PurchaseOrder.PONumber INNER JOIN " +
                                                    " dbo.tb_Vendor ON dbo.tb_PurchaseOrder.VendorID = dbo.tb_Vendor.VendorID LEFT OUTER JOIN " +
                                                    " dbo.tb_VendorQuoteReply AS rep ON rep.VendorID = dbo.tb_PurchaseOrder.VendorID AND rep.RefProductID = dbo.tb_PurchaseOrderItems.ProductID AND  " +
                                                    " rep.OrderNumber = dbo.tb_PurchaseOrder.OrderNumber LEFT OUTER JOIN " +
                                                    " dbo.tb_OrderShippedItems ON dbo.tb_OrderShippedItems.RefProductID = dbo.tb_PurchaseOrderItems.ProductID AND  " +
                                                    " dbo.tb_PurchaseOrder.OrderNumber = dbo.tb_OrderShippedItems.OrderNumber WHERE (dbo.tb_PurchaseOrderItems.PONumber = " + Pono + ")");
        }

        /// <summary>
        /// Binds the Cart for Vendor Product
        /// </summary>
        private void BindCart()
        {
            DataSet ds = new DataSet();
            ds = GetVendorProductsforWarehouse(Convert.ToInt32(Request.QueryString["pono"]));

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                gvVendor.DataSource = ds;
                gvVendor.DataBind();
                availdays_SelectedIndexChanged(null, null);
                btnsubmit.Visible = true;
                trVendordetails.Visible = true;
                ltOrder.Text = ds.Tables[0].Rows[0]["OrderNumber"].ToString();
                ltvname.Text = ds.Tables[0].Rows[0]["vname"].ToString();
                ltpodate.Text = ds.Tables[0].Rows[0]["Podate"].ToString();
                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["Email"].ToString()))
                {
                    ltvemail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                }
                else
                {
                    ltvemail.Text = "N/A";
                }
                trNotes.Visible = true;
                ltvphone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
            }
            else
            {
                trVendordetails.Visible = false;
                btnsubmit.Visible = false;
                trNotes.Visible = false;
                lblmsg.Text = "No Record found...";
            }
        }

        /// <summary>
        /// Available Days Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void availdays_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool falg = false;
            foreach (GridViewRow row in gvVendor.Rows)
            {
                try
                {
                    DropDownList drpAvailDays = (DropDownList)row.FindControl("drpAvailDays");
                    Label lblTracking = (Label)row.FindControl("lblTracking");
                    Label lblTrackingNum = (Label)row.FindControl("lblTrackingNum");
                    TextBox txttracking = (TextBox)row.FindControl("txttracking");
                    if (drpAvailDays.Visible == true && (drpAvailDays.SelectedValue == "1" || drpAvailDays.SelectedValue == "2"))
                    {
                        txttracking.Visible = true;
                        txttracking.Focus();
                        lblTracking.Visible = false;
                        lblTrackingNum.Visible = false;
                        falg = true;
                    }
                    else
                    {
                        txttracking.Visible = false;
                        if (lblTrackingNum.Text == "")
                            lblTracking.Visible = true;
                        else
                            lblTrackingNum.Visible = true;
                    }
                }
                catch { }
            }
            if (falg == true)
            {
                trNotes.Attributes.Add("style", "display:''");
            }
            else
            {
                trNotes.Attributes.Add("style", "display:none");
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
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    Label lblShippedVia = (Label)e.Row.FindControl("lblShippedVia");
                    DropDownList drpMethod = (DropDownList)e.Row.FindControl("drpMethod");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdndays = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("availdays");
                    System.Web.UI.HtmlControls.HtmlInputHidden hndpaid = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hndpaid");
                    if (hdndays.Value.ToString() != "" && hdndays.Value.ToString().ToLower() == "false")
                    {
                        lblStatus.Visible = false;
                        ddl.SelectedIndex = 0;
                        lblStatus.Text = "";
                        drpMethod.Visible = true;
                        lblShippedVia.Visible = false;
                    }
                    else
                    {
                        lblShippedVia.Visible = true;
                        drpMethod.Visible = false;
                        try
                        {

                            if (lblShippedVia.Text.ToString().Trim() != "")
                            {
                                drpMethod.SelectedValue = lblShippedVia.Text.ToString().Trim();
                            }
                        }
                        catch
                        {
                        }
                        if (hndpaid.Value.ToString() == "1")
                        {
                            lblStatus.Text = "Paid";
                        }
                        else
                        {
                            lblStatus.Text = "Shipped";
                        }
                        lblStatus.Visible = true;
                        ddl.SelectedIndex = 0;
                        ddl.Visible = false;
                    }

                }
            }
            catch { }
        }

        /// <summary>
        /// Gets the Value of the AppConfig by Name
        /// </summary>
        /// <param name="AppConfigName">String AppConfigName</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the App Config Value </returns>
        public String GetAppConfigByName(string AppConfigName, int StoreID)
        {
            string value = "";
            value = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue From tb_AppConfig Where configname='" + AppConfigName.Replace("'", "''") + "' And Deleted<>1 and StoreID=" + StoreID));
            return value;
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            string TOAddress = GetAppConfigByName("ContactMail_ToAddress", 1);
            bool falg = false;
            foreach (GridViewRow row in gvVendor.Rows)
            {
                try
                {
                    Label lblProductID = (Label)row.FindControl("lblProductID");
                    DropDownList drpAvailDays = (DropDownList)row.FindControl("drpAvailDays");
                    DropDownList drpMethod = (DropDownList)row.FindControl("drpMethod");
                    TextBox txttracking = (TextBox)row.FindControl("txttracking");
                    Label txtQuantity = (Label)row.FindControl("lblQuantity");
                    if ((txttracking.Visible == true) && (txttracking.Text.Trim() == ""))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "QuantityAlert", "alert('Please enter valid Tracking Number !');", true);
                        return;
                    }

                    if (drpAvailDays.SelectedIndex == 1 || drpAvailDays.SelectedIndex == 2)
                    {
                        falg = true;

                        if (drpAvailDays.SelectedIndex == 2)
                        {
                            CommonComponent.ExecuteCommonData("UPDATE tb_PurchaseOrderItems SET  TrackingNumber='" + txttracking.Text.Trim().Replace("'", "''") + "',ShippedVia='" + drpMethod.SelectedValue.ToString() + "',IsShipped=1,Ispaid=1,ShippedOn=getdate() WHERE PONumber=" + Request.QueryString["pono"] + " AND ProductId=" + lblProductID.Text.ToString() + "");
                        }
                        else
                        {
                            CommonComponent.ExecuteCommonData("UPDATE tb_PurchaseOrderItems SET  TrackingNumber='" + txttracking.Text.Trim().Replace("'", "''") + "',ShippedVia='" + drpMethod.SelectedValue.ToString() + "',IsShipped=1,Ispaid=0,ShippedOn=getdate() WHERE PONumber=" + Request.QueryString["pono"] + " AND ProductId=" + lblProductID.Text.ToString() + "");
                        }
                        if (Convert.ToInt32(CommonComponent.GetScalarCommonData("select ISNULL(COUNT(*),0) from tb_OrderShippedItems where OrderNumber=" + ltOrder.Text.Trim() + " and RefProductID=" + lblProductID.Text.ToString() + "")) > 0)
                        {
                            CommonComponent.ExecuteCommonData("UPDATE tb_ordershippeditems SET TrackingNumber=TrackingNumber+'," + txttracking.Text.Trim().Replace("'", "''") + ",',ShippedVia=ShippedVia+'," + drpMethod.SelectedValue.ToString() + ",' WHERE OrderNumber=" + ltOrder.Text.Trim() + " AND RefProductId=" + lblProductID.Text.ToString() + "");
                        }
                        else
                        {
                            string StrQuery = " insert into tb_OrderShippedItems(OrderNumber,RefProductID,TrackingNumber,ShippedVia,Shipped,ShippedOn,ShippedQty,ShippedNote)  " +
                                              " values(" + ltOrder.Text.Trim() + "," + lblProductID.Text.ToString() + ",'" + txttracking.Text.Trim().Replace("'", "''") + "','" + drpMethod.SelectedValue.ToString() + "',1,'" + DateTime.Now + "'," + txtQuantity.Text + ",'Purchase Order(" + txtQuantity.Text.ToString() + ")')";
                            CommonComponent.ExecuteCommonData(StrQuery);
                        }
                        lblMsgUpdated.Text = "Record updated Successfully!";
                        lblMsgUpdated.Visible = true;
                    }
                }
                catch { }
            }
            if (falg == true)
            {
                try
                {
                    CommonComponent.ExecuteCommonData("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber,VendorName,Description,CreatedOn) VALUES (-1,27," + Convert.ToInt32(Request.QueryString["pono"].ToString()) + "," + Convert.ToInt32(ltOrder.Text.Trim()) + ",'" + ltvname.Text.ToString().Replace("'", "''") + "','" + txtNotes.Text.Trim().Replace("'", "''") + "',getdate())");
                    OrderComponent objAddOrder = new OrderComponent();
                    objAddOrder.InsertOrderlog(27, Convert.ToInt32(ltOrder.Text.Trim()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                }
                catch { }
                SendMail();
            }
            else
            {
                return;
            }
            BindCart();
            SendCustomeMail();
        }

        /// <summary>
        /// Sends the Mail to Admin
        /// </summary>
        public void SendMail()
        {
            string TOAddress = GetAppConfigByName("ContactMail_ToAddress", 1);
            DataSet ds1 = new DataSet();
            try
            {
                CustomerComponent objCustomer = new CustomerComponent();
                DataSet dsMailTemplate = new DataSet();
                dsMailTemplate = objCustomer.GetEmailTamplate("VendorAdminEmailTemp", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                {
                    String strBody = "";
                    String strSubject = "";

                    String Subject = "";
                    if (Request.QueryString["pono"] != null)
                    {
                        Subject = "Purchase number " + Request.QueryString["pono"].ToString() + " shipping status.";
                    }
                    else
                    {
                        Subject = "Purchase number shipping status";
                    }

                    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();
                    strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                    strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                    strSubject = Regex.Replace(strSubject, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###YEAR###", AppLogic.AppConfigs("YEAR").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                    if (ltvname.Text.ToString().Trim() != "")
                        strBody = Regex.Replace(strBody, "###VendorName###", ltvname.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                    else
                        strBody = Regex.Replace(strBody, "###VendorName###", "", RegexOptions.IgnoreCase);

                    if (ltvphone.Text.ToString().Trim() != "")
                        strBody = Regex.Replace(strBody, "###Phone###", ltvphone.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                    else
                        strBody = Regex.Replace(strBody, "###Phone###", "--", RegexOptions.IgnoreCase);

                    if (ltvemail.Text.ToString().Trim() != "")
                        strBody = Regex.Replace(strBody, "###Email###", ltvemail.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                    else
                        strBody = Regex.Replace(strBody, "###Email###", "", RegexOptions.IgnoreCase);

                    if (ltOrder.Text.ToString().Trim() != "")
                        strBody = Regex.Replace(strBody, "###ORDER_NUM###", ltOrder.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                    else
                        strBody = Regex.Replace(strBody, "###ORDER_NUM###", "", RegexOptions.IgnoreCase);

                    String ORDERCART = string.Empty;

                    ORDERCART = (" <table width='99%' cellspacing='0' cellpadding='0' border='1' style='padding: 10px 0 0;border: 1px solid #ECECEC; border-collapse: collapse; color: #212121; font: 12px;'> ");
                    ORDERCART += ("<tbody><tr style='background-color: rgb(242,242,242); height: 25px;'>");
                    ORDERCART += ("<th valign='middle' align='Center' style='width:50%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;text-align:left;padding-left: 5px' align='left'>Product Name</th>");
                    ORDERCART += ("<th valign='middle' style='width:20%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold; padding-left: 5px;' align='left'> SKU</th>");
                    ORDERCART += ("<th valign='middle' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold; padding-left: 5px;padding-right: 5px;' align='center'>Quantity</th>");
                    ORDERCART += ("<th valign='middle' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold; padding-left: 5px;text-align:right;padding-right: 5px;' align='right'>Price</th>");
                    ORDERCART += ("<th valign='middle' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold; padding-left: 5px;' align='center'>Status</th>");
                    ORDERCART += ("</tr>");

                    foreach (GridViewRow row in gvVendor.Rows)
                    {
                        try
                        {
                            Label lblName = (Label)row.FindControl("lblName");
                            Label lblSKU = (Label)row.FindControl("lblSKU");
                            Label txtQuantity = (Label)row.FindControl("lblQuantity");
                            Label txtPrice = (Label)row.FindControl("lblPrice");
                            Label lblStatus = (Label)row.FindControl("lblStatus");
                            DropDownList ddlstt = (DropDownList)row.FindControl("drpAvailDays");
                            if (ddlstt.Visible == true)
                            {
                                if (ddlstt.SelectedIndex == 1 || ddlstt.SelectedIndex == 2)
                                {
                                    ORDERCART += ("<TR>");
                                    ORDERCART += ("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:left;font-weight: normal;' align='left' valign='middle' style='width:60%' >" + lblName.Text.Trim() + "</td>");
                                    ORDERCART += ("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:left;font-weight: normal;' align='left' valign='middle' style='width:20%' >" + lblSKU.Text.Trim() + "</td>");
                                    ORDERCART += ("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;font-weight: normal;' valign='middle' style='width: 20%;text-align:center;'>" + txtQuantity.Text.Trim() + "</td>");
                                    ORDERCART += ("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:right;font-weight: normal;' valign='middle' style='width: 20%;text-align:right;'>$" + String.Format("{0:0.00}", Convert.ToDecimal(txtPrice.Text.Trim())) + "</td>");
                                    ORDERCART += ("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;font-weight: normal;' valign='middle' style='width: 20%;text-align:center;'>" + ddlstt.SelectedItem.Text.ToString() + "</td>");
                                    ORDERCART += ("</tr>");
                                }
                            }
                        }
                        catch { }
                    }
                    ORDERCART += ("</tbody></table>");

                    strBody = Regex.Replace(strBody, "###ORDERCART###", ORDERCART.ToString().Trim(), RegexOptions.IgnoreCase);

                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");

                    CommonOperations.SendMail(TOAddress, Subject, strBody, Request.UserHostAddress.ToString(), true, av);
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = "Error Occurred While Sending Mail";
            }
        }

        /// <summary>
        /// Sends a Mail to Customer for Shipping Status
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="Cart">String Cart</param>
        /// <param name="ToID">string ToID</param>
        private void SendShippedMail(int OrderNumber, String Cart, string ToID)
        {
            try
            {
                StringBuilder sw = new StringBuilder();
                sw.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">"
                + "<head id=\"Head1\"><link id=\"stylesheet\" href=\"Client/style.css\" /><title>ShippedItems</title><style type=\"text/css\">.datatable table{border:1px solid #eeeeee;  }"
                + ".datatable tr.alter_row{background-color:#f9f9f9;} .datatable td{padding:2px 2px; text-align:left; border:1px solid #eeeeee; font:11px/14px Verdana, Arial, Helvetica, sans-serif; color:#4c4c4c; line-height:16px;}"
                 + ".datatable th{padding:2px 3px; text-align:left; border:1px solid #eeeeee; font:11px/14px  Verdana, Arial, Helvetica, sans-serif; font-weight:bold; color:#4c4c4c; line-height:16px;}"
                  + ".receiptfont{font:11px/14px Verdana, Arial, Helvetica, sans-serif; color:#4c4c4c;}.receiptlineheight{ style=\"height: 15px\";}</style></head><body>");
                sw.Append("<table class='receiptfont' width='100%' style='font-family:Arial;font-size:10pt' align='center' cellpadding='0' cellspacing='0'>");
                sw.Append("<tr>");
                sw.Append("<td colspan='3' style='height: 14px'>");
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("<tr align='center'>");
                sw.Append("<td align='center' rowspan='1' valign='middle' style='height: 71px'>");
                sw.Append("<img src=\"" + GetAppConfigByName("LIVE_SERVER", 1) + "/images/logo.png\" />");
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td>&nbsp;</td></tr>");
                sw.Append("<tr>");
                sw.Append("<td align='left' rowspan='1' valign='middle'>");
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td style='height: 14px' style='font-family:Arial'>");
                sw.Append("** This is an auto-generated confirmation email. Please do not respond to this email.Your reply will not be received.**");
                sw.Append("<br/></td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td align='left' rowspan='1' valign='middle' height='20'>");
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td style='font-family:Arial'><br /> Thank you for your recent purchase. Your Order was shipped. Please find below tracking information of your ordered items. <br/>");
                sw.Append("(*Note: Some items might not have been shipped. Please wait till further correspondence.) <br/>");
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("<tr><td height='20px'>&nbsp;</td></tr>");
                sw.Append("<tr style='display:none;'>");
                sw.Append("<td style='font-family:Arial'>");
                sw.Append("Order Number : <b>" + OrderNumber);
                sw.Append("</b><br/></td>");
                sw.Append("</tr>");
                sw.Append("<tr><td height='20px'>&nbsp;</td></tr>");
                sw.Append("<tr><td height='20px'><b>Shipped Items:</b><br /></td></tr>");
                sw.Append("<tr><td>");
                sw.Append(Cart);
                sw.Append("</td></tr>");
                sw.Append("<tr><td height='20px'>&nbsp;</td></tr>");
                sw.Append("<tr>");
                sw.Append("<td><br />Thank you again for your business.");
                sw.Append("<br/>");
                sw.Append("<b style='font-size:11pt;'>" + GetAppConfigByName("StoreName", 1) + "<b><br/>");//<a href='http://" + AppLogic.AppConfig("LIVE_SERVER_NAME") + "'>" + AppLogic.AppConfig("LIVE_SERVER_NAME") + "</a>
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("</table>");
                sw.Append("</body></html>");
                string strMailSubject = "";
                strMailSubject = GetAppConfigByName("StoreName", 1) + "'s Product Shipped";//AppLogic.AppConfig("StoreName")
                CommonOperations.SendMail(ToID, strMailSubject, sw.ToString(), Request.UserHostAddress.ToString(), true, null);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Sends the Custome Mail
        /// </summary>
        public void SendCustomeMail()
        {
            StringBuilder Table = new StringBuilder();
            bool GenerateAll = false;
            ArrayList ProductList = new ArrayList();
            ArrayList TrackingList = new ArrayList();
            ArrayList CourierList = new ArrayList(); //= (ArrayList)Session["ShippedOn"];
            ArrayList ShippedQty = new ArrayList();//= (ArrayList)Session["ShippedQty"];
            ArrayList ShippedNote = new ArrayList();//= (ArrayList)Session["ShippedNote"];
            ArrayList CustomCartID = new ArrayList();// = (ArrayList)Session["CustomCartID"];
            ArrayList ShippedOn = new ArrayList();

            Table.Append("  <table border='0' cellpadding='0' cellspacing='0' class='datatable' width='100%'> ");
            Table.Append("<tbody><tr style='line-height: 40px; BACKGROUND-COLOR: rgb(242,242,242); ' >");
            Table.Append("<th align='left' valign='middle' style='width:30%;font-size:10pt;' ><b>Product</b></th>");
            Table.Append("<th align='left' valign='middle' style='width:15%;font-size:10pt;' ><b> Style</b></th>");
            Table.Append("<th align='center' valign='middle' style='width:10%;font-size:10pt;'><b>Quantity</b></th>");
            Table.Append("<th align='left' valign='middle' style='width:15%;font-size:10pt;' ><b> Courier Name</b></th>");
            Table.Append("<th align='center' valign='middle' style='width:10%;font-size:10pt;'><b>ShippedOn</b></th>");
            Table.Append("<th align='center' valign='middle' style='width:19%;font-size:10pt;'><b>Tracking Number</b></th>");
            Table.Append("<th style='width:10%;font-size:10pt;'><b>Status</b></th>");
            Table.Append("</tr>");
            int orderNumber = 0;
            foreach (GridViewRow row in gvVendor.Rows)
            {
                try
                {
                    Label lblProductID = (Label)row.FindControl("lblProductID");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnShippdeOn = (System.Web.UI.HtmlControls.HtmlInputHidden)row.FindControl("hdnShippdeOn");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnOrderNumber = (System.Web.UI.HtmlControls.HtmlInputHidden)row.FindControl("hdnOrderNumber");

                    Label lblStatus = (Label)row.FindControl("lblStatus");
                    Label lblQuantity = (Label)row.FindControl("lblQuantity");
                    DropDownList drpAvailDays = (DropDownList)row.FindControl("drpAvailDays");
                    DropDownList drpMethod = (DropDownList)row.FindControl("drpMethod");
                    Label lblTrackingNum = (Label)row.FindControl("lblTrackingNum");
                    orderNumber = Convert.ToInt32(hdnOrderNumber.Value.ToString());
                    Label lblShippedVia = (Label)row.FindControl("lblShippedVia");
                    Label lblSKU = (Label)row.FindControl("lblSKU");
                    Label lblName = (Label)row.FindControl("lblName");

                    if (lblStatus.Text.ToString().ToLower() == "shipped" || lblStatus.Text.ToString().ToLower() == "paid")
                    {
                        GenerateAll = true;
                        ProductList.Add(lblProductID.Text.ToString());
                        TrackingList.Add(lblTrackingNum.Text.ToString());
                        CourierList.Add(lblShippedVia.Text.ToString());
                        ShippedQty.Add(lblQuantity.Text.ToString());
                        ShippedNote.Add("");
                        DataSet dsCustom = new DataSet();
                        dsCustom = CommonComponent.GetCommonDataSet("select OrderedCustomCartID from tb_OrderedShoppingCartItems where OrderedShoppingCartID in (SELECT ShoppingCardID FROM tb_Order WHERE orderNumber= " + orderNumber + ") AND RefProductid=" + Convert.ToInt32(lblProductID.Text.ToString()) + "");

                        if (dsCustom.Tables[0].Rows.Count > 0)
                        {
                            CustomCartID.Add(dsCustom.Tables[0].Rows[0]["OrderedCustomCartID"].ToString());
                        }
                        ShippedOn.Add(hdnShippdeOn.Value.ToString());
                        Table.Append("<tr align='center'  valign='middle'>");
                        Table.Append("<td align='left' valign='top' style='font-size:10pt;'>" + lblName.Text);
                        Table.Append("</td>");
                        Table.Append("<td  align='left'  style='font-size:10pt;'>" + lblSKU.Text + "</td>");
                        Table.Append("<td align='center'>" + lblQuantity.Text.ToString() + "</td>");
                        Table.Append("<td  align='left'  style='font-size:10pt;'>" + SetCourierLink(lblShippedVia.Text.ToString()) + "</td>");
                        Table.Append("<td align='center'>" + Convert.ToDateTime(hdnShippdeOn.Value).ToShortDateString() + "</td>");
                        Table.Append("<td align='center' style='font-size:10pt;' >" + lblTrackingNum.Text.ToString() + "</td>");
                        Table.Append("<td  style='text-align : right;font-size:10pt;'>Shipped</td>");
                        Table.Append(" </tr>");
                    }
                }
                catch
                {
                }
            }
            Table.Append("</tbody></table></body></html>");
            DataSet custEmail = new DataSet();
            custEmail = CommonComponent.GetCommonDataSet("Select Email,OrderNumber from tb_Order where OrderNumber = " + orderNumber + "");
            if (custEmail.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(custEmail.Tables[0].Rows[0]["Email"].ToString()))
                {
                    SendShippedMail(orderNumber, Table.ToString(), custEmail.Tables[0].Rows[0]["Email"].ToString());
                }
            }
        }

        /// <summary>
        /// Sets the Courier Link
        /// </summary>
        /// <param name="CourierName">String CourierName</param>
        /// <returns> Return  Link of Courier as a string</returns>
        private string SetCourierLink(string CourierName)
        {
            string Link = string.Empty;
            switch (CourierName.ToLowerInvariant())
            {
                case "ups":
                    {
                        if (AppLogic.AppConfigs("UPSTrackingLink") != "")
                            Link = "<a href='" + AppLogic.AppConfigs("UPSTrackLink") + "'>" + CourierName + "</a>";
                        else
                            Link = "<a href='http://www.ups.com/content/us/en/index.jsx'>" + CourierName + "</a>";
                        break;
                    }
                case "usps":
                    {
                        if (AppLogic.AppConfigs("USPSTrackingLink") != "")
                            Link = "<a href='" + AppLogic.AppConfigs("USPSTrackingLink") + "'>" + CourierName + "</a>";
                        else
                            Link = "<a href='http://www.usps.com/shipping/trackandconfirm.htm'>" + CourierName + "</a>";
                        break;
                    }
                case "fedex":
                    {
                        if (AppLogic.AppConfigs("FedExTrackingLink") != "")
                            Link = "<a href='" + AppLogic.AppConfigs("FedExTrackingLink") + "'>" + CourierName + "</a>";
                        else
                            Link = "<a href='http://fedex.com/Tracking'>" + CourierName + "</a>";
                        break;
                    }
                case "dhl":
                    {
                        if (AppLogic.AppConfigs("DHLTrackingLink") != "")
                            Link = "<a href='" + AppLogic.AppConfigs("DHLTrackingLink") + "'>" + CourierName + "</a>";
                        else
                            Link = "<a href='http://track.dhl-usa.com/TrackByNbr.asp'>" + CourierName + "</a>";
                        break;
                    }
                default:
                    {
                        Link = CourierName.ToString();
                        break;
                    }
            }
            return Link;
        }
    }
}