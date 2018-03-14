using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net;
using System.IO;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class BulkOrderPrint : BasePage
    {
        #region Declaration


        int pageSize = 20;
        int selectedStore = 0;
        DataSet dsOrder = new DataSet();
        OrderComponent ordcomp = new OrderComponent();

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
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            grvBulkPrint.PageSize = pageSize;
            if (!IsPostBack)
            {
                GetStoreList(ddlStore);
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
                {
                    ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                    AppConfig.StoreID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                }
                GetOrderListByStoreId(1, pageSize);
                txtFromdate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtTodate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnPrintBulk.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/printall.gif) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
            }
        }

        /// <summary>
        /// Gets the Store List
        /// </summary>
        /// <param name="ddlStore">DropDownList ddlStore</param>
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
        /// Gets the Order List by StoreID
        /// </summary>
        /// <param name="PageNumber">int PageNumber</param>
        /// <param name="PageSize">int PageSize</param>
        private void GetOrderListByStoreId(Int32 PageNumber, Int32 PageSize)
        {
            if (ViewState["SearchFilter"] != null)
            {
                dsOrder = OrderComponent.GetorderListByStoreIdforBulkPrint(PageNumber, PageSize, ViewState["SearchFilter"].ToString().Replace("'","''"), Convert.ToInt32(ddlStore.SelectedValue.ToString()));
            }
            else
            {
                dsOrder = OrderComponent.GetorderListByStoreIdforBulkPrint(PageNumber, PageSize, "", Convert.ToInt32(ddlStore.SelectedValue.ToString()));
            }
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                grvBulkPrint.DataSource = dsOrder;
                grvBulkPrint.DataBind();
                trTop.Visible = true;
                trBottom.Visible = true;
                btnPrintBulk.Visible = true;
            }
            else
            {
                grvBulkPrint.DataSource = null;
                grvBulkPrint.DataBind();
                trTop.Visible = false;
                trBottom.Visible = false;
                btnPrintBulk.Visible = false;
            }
        }

        /// <summary>
        ///  Print Bulk Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnPrintBulk_Click(object sender, EventArgs e)
        {
            Session["PrintOrderId"] = "";
            int i = 0;
            string strprint = "";
            foreach (GridViewRow r in grvBulkPrint.Rows)
            {
                CheckBox chk = (CheckBox)r.FindControl("chkPrint");
                Literal ltrPrinted = (Literal)r.FindControl("ltrPrinted");
                Label lb = (Label)r.FindControl("OrderId");
                string[] fls = { "" };
                if (chk.Checked)
                {
                    if (i == 0)
                        Session["PrintOrderId"] += lb.Text.ToString();
                    else
                        Session["PrintOrderId"] += "," + lb.Text.ToString();
                    i++;

                    DataSet ds = new DataSet();
                    ds = ordcomp.GetDetailforOrder(lb.Text.Trim());

                    string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
                    string Body = "";
                    string url = "http://www.halfpricedrapes.us/Admin/Orders/invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(lb.Text.ToString()));
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
                    //Body = Body.Replace("class=\"bkground123\">", "class=\"bkground123\">" + sttt.Replace("'","\""));

                    Body = Body.Replace("title=\"Print Invoice\"", "title=\"Print Invoice\" style=\"display:none;\"");
                    Body = Body.Replace("title=\"Send Invoice\"", "title=\"Send Invoice\" style=\"display:none;\"");
                    Body = Body.Substring(Body.IndexOf("<table"), Body.LastIndexOf("</table") - Body.IndexOf("<table"));
                    strprint += "<div style=\"page-break-after:always\">" + sttt.Replace("'", "\"") + Body + "</table></div>";
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string Cartnumber = ds.Tables[0].Rows[0]["ShoppingCardID"].ToString();
                        DataSet dsnew = ordcomp.GetShippingDetailforCartNumber(Cartnumber);
                        if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsnew.Tables[0].Rows.Count; j++)
                            {
                                string[] shippvia = dsnew.Tables[0].Rows[j]["ShippedVia"].ToString().Replace(",,", ",").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string[] TrackingNumber = dsnew.Tables[0].Rows[j]["TrackingNumber"].ToString().Replace(",,", ",").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (shippvia.Length == TrackingNumber.Length)
                                {
                                    for (int k = 0; k < shippvia.Length; k++)
                                    {
                                        if (System.IO.Directory.Exists(Server.MapPath("/ShippingLabels/" + shippvia[k] + "/")))
                                        {
                                            fls = System.IO.Directory.GetFiles(Server.MapPath("/ShippingLabels/" + shippvia[k] + "/"));
                                            foreach (string str in fls)
                                            {
                                                if (str.IndexOf("_" + TrackingNumber[k].ToString() + "_") > -1 && str.IndexOf("_" + lb.Text.ToString() + "_") > -1)
                                                {
                                                    string path = "";
                                                    System.IO.FileInfo fl = new System.IO.FileInfo(str);
                                                    path = AppLogic.AppConfigs("LIVE_SERVER") + "/ShippingLabels/" + shippvia[k] + "/" + fl.Name.ToString();
                                                    string imgesrc = "<div style=\"page-break-after:always\"><img  src=\"" + path.ToString().Replace(" ", "%20") + "\" /></div>";
                                                    strprint += imgesrc.ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "iframeprint", "var pri = document.getElementById(\"ifmcontentstoprint\").contentWindow;pri.document.open();pri.document.write('" + strprint + "');pri.document.close();pri.focus();pri.print();", true);


            }
            bool Printed = false;
            if (Session["PrintOrderId"] != null && Session["PrintOrderId"].ToString() != "")
            {
                Printed = ordcomp.UpdatePrintStatusforOrder(Session["PrintOrderId"].ToString());
                GetOrderListByStoreId(1, pageSize);
            }
            Session["PrintOrderId"] = "";
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnPrintBulkpackaeSlip_Click(object sender, EventArgs e)
        {

            int i = 0;
            string strprint = "";
            foreach (GridViewRow r in grvBulkPrint.Rows)
            {
                CheckBox chk = (CheckBox)r.FindControl("chkPrint");
                Literal ltrPrinted = (Literal)r.FindControl("ltrPrinted");
                Label lb = (Label)r.FindControl("OrderId");
                string[] fls = { "" };
                if (chk.Checked)
                {
                    i++;
                    Int32 POStoreID = 0;
                    Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + Convert.ToInt32(lb.Text.ToString()))), out POStoreID);
                    string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
                    string Body = "";
                    string url = "";
                    if (POStoreID == 4)
                    {
                        url = AppLogic.AppConfigs("LIVE_SERVER") + "/Admin/Orders/" + "OverStockPackingSlip.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(lb.Text.ToString()));
                    }
                    else if (POStoreID == 3)
                    {
                        url = AppLogic.AppConfigs("LIVE_SERVER") + "/Admin/Orders/" + "AmazonPackingSlip.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(lb.Text.ToString()));
                    }
                    else
                    {
                        url = AppLogic.AppConfigs("LIVE_SERVER") + "/Admin/Orders/" + "PackingSlip.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(lb.Text.ToString()));
                    }
                    WebRequest NewWebReq = WebRequest.Create(url);
                    WebResponse newWebRes = NewWebReq.GetResponse();
                    string format = newWebRes.ContentType;
                    Stream ftprespstrm = newWebRes.GetResponseStream();
                    StreamReader reader;
                    reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
                    string strbiody = reader.ReadToEnd().ToString();
                    strbiody = strbiody.Replace("'", "&#39;");
                    Body = @"<style type='text/css'>
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
            padding: 2px 2px;
            text-align: left;
            border: 1px solid #eeeeee;
            font-size: 11px; font-family: Arial, Helvetica, sans-serif;
            color: #4c4c4c;
            line-height: 16px;
        }
        .datatable th
        {
            padding: 2px 3px;
            text-align: left;
            border: 1px solid #eeeeee;
          font-size: 11px; font-family: Arial, Helvetica, sans-serif;
            font-weight: bold;
            color: #4c4c4c;
            line-height: 16px;
        }
        .receiptfont
        {
            font-size: 11px; font-family: Arial, Helvetica, sans-serif;
            color: #4c4c4c;
        }
        .receiptlineheight
        {
            height: 15px;
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
                    Body = Body.Replace("title=\"Print Packing Slip\"", "title=\"Print Packing Slip\" style=\"display:none;\"");

                    Body = Body.Substring(Body.IndexOf("<table"), Body.LastIndexOf("</table") - Body.IndexOf("<table"));

                    strprint += "<div style=\"page-break-after:always\">" + Body + "</table></div>";

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
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "iframeprint", "var pri = document.getElementById(\"ifmcontentstoprint\").contentWindow;pri.document.open();pri.document.write('" + strprint + "');pri.document.close();pri.focus();pri.print();", true);
            }


            //Printed = ordcomp.UpdatePrintStatusforOrder(Session["PrintOrderId"].ToString());
            GetOrderListByStoreId(1, pageSize);

        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["SearchFilter"] = null;
            string strSearch = "";
            if (txtFromdate.Text.ToString() != "" && txtTodate.Text.ToString() != "")
            {
                if (Convert.ToDateTime(txtTodate.Text.ToString()) >= Convert.ToDateTime(txtFromdate.Text.ToString()))
                {
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }
            else
            {
                if (txtFromdate.Text.ToString() == "" && txtTodate.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
                else if (txtTodate.Text.ToString() == "" && txtFromdate.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }

            if (txtFromdate.Text.ToString() != "")
            {
                strSearch += " AND cast(orderdate as date) >= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtFromdate.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
            }
            if (txtTodate.Text.ToString() != "")
            {
                strSearch += " AND cast(orderdate as date) <= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtTodate.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
            }
            grvBulkPrint.PageIndex = 0;
            ViewState["SearchFilter"] = strSearch.ToString();
            GetOrderListByStoreId(1, pageSize);
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            GetOrderListByStoreId(1, pageSize);
        }

        /// <summary>
        ///  Bulk Print Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvBulkPrint_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvBulkPrint.PageIndex = e.NewPageIndex;
            GetOrderListByStoreId(grvBulkPrint.PageIndex + 1, pageSize);
        }

        /// <summary>
        /// Card Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grvBulkPrint_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grvBulkPrint.Rows.Count > 0)
            {
                trBottom.Visible = true;
                trTop.Visible = true;
            }
            else
            {
                trBottom.Visible = false;
                trTop.Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ltr2 = (Literal)e.Row.FindControl("ltr2");
                Literal ltr3 = (Literal)e.Row.FindControl("ltr3");
                Literal ltr4 = (Literal)e.Row.FindControl("ltr4");
                Literal ltr5 = (Literal)e.Row.FindControl("ltr5");
                Literal ltr6 = (Literal)e.Row.FindControl("ltr6");
                Literal ltr7 = (Literal)e.Row.FindControl("ltr7");
                Literal ltrShip = (Literal)e.Row.FindControl("ltrShip");
                Literal ltrPrinted = (Literal)e.Row.FindControl("ltrPrinted");
                CheckBox chkPrint = (CheckBox)e.Row.FindControl("chkPrint");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnshoppingcartid = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnshoppingcartid");

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
                System.Web.UI.HtmlControls.HtmlInputHidden hdnRefundF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnRefundF");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnAdjAmtF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnAdjAmtF");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnQtyDiscountAmountF = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnQtyDiscountAmountF");

                System.Web.UI.HtmlControls.HtmlInputHidden hdnorderStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnorderStatus");


                HtmlInputHidden hdnAddress1 = (HtmlInputHidden)e.Row.FindControl("hdnAddress1");
                HtmlInputHidden hdnAddress2 = (HtmlInputHidden)e.Row.FindControl("hdnAddress2");
                HtmlInputHidden hdnSuite = (HtmlInputHidden)e.Row.FindControl("hdnSuite");
                HtmlInputHidden hdnCity = (HtmlInputHidden)e.Row.FindControl("hdnCity");
                HtmlInputHidden hdnState = (HtmlInputHidden)e.Row.FindControl("hdnState");
                HtmlInputHidden hdnPhone = (HtmlInputHidden)e.Row.FindControl("hdnPhone");
                HtmlInputHidden hdnCountry = (HtmlInputHidden)e.Row.FindControl("hdnCountry");
                HtmlInputHidden hdnZip = (HtmlInputHidden)e.Row.FindControl("hdnZip");
                HtmlInputHidden hdnCompany = (HtmlInputHidden)e.Row.FindControl("hdnCompany");
                HtmlInputHidden hdnShippingMethod = (HtmlInputHidden)e.Row.FindControl("hdnShippingMethod");
                ltr4.Text += "<br />";

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
                }
                Int32 OrderNumber = Convert.ToInt32(ltr2.Text.Trim());
                ltr2.Text = "<a class=\"order-no\" onclick=\"chkHeight();\" href=\"/Admin/Orders/Orders.aspx?id=" + ltr2.Text.ToString() + "\">" + ltr2.Text.ToString() + "</a><br />";
                Decimal hdnRefundTotal = Convert.ToDecimal(hdnOrderTotalNew.Value.ToString()) + Convert.ToDecimal(hdnAdjAmtF.Value);
                SetStatus(hdnorderStatus.Value.ToString(), ltr6.Text.ToString(), ltr6, ltr7, OrderNumber, hdnRefundTotal.ToString(), hdnRefundF.Value.ToString());
                GetProduct(Convert.ToInt32(hdnshoppingcartid.Value), ltr2);
            }
        }

        /// <summary>
        /// Sets the Status
        /// </summary>
        /// <param name="statusName">string statusName</param>
        /// <param name="Transtatus">string Transtatus.</param>
        /// <param name="ltrStatus">Literal ltrStatus</param>
        /// <param name="ltrAction">Literal ltrAction</param>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="OrderTotal">string OrderTotal</param>
        /// <param name="RefundAmount">string RefundAmount</param>
        private void SetStatus(string statusName, string Transtatus, Literal ltrStatus, Literal ltrAction, Int32 OrderNumber, String OrderTotal, string RefundAmount)
        {

            ltrAction.Text = "<a title=\"\" href=\"Orders.aspx?ID=" + OrderNumber + "&PS=1\"><img title=\"\" alt=\"Print Packing Slip\" src=\"/App_Themes/" + Page.Theme + "/images/print-packing-skip.png\"></a><br />";


            ltrStatus.Text = "";
            if (Transtatus.ToLower() == "authorized")
            {
                ltrStatus.Text += "<strong style=\"color:#FF7F00;font-size:10px;\"><br />" + Transtatus.ToString() + "</strong>";
                ltrStatus.Text += "<br /><span style=\"color:#000000;\">" + statusName.ToString() + "</span>";
                //ltrAction.Text += "<a title=\"\" href=\"javascript:void(0);\"><img title=\"\" alt=\"Confirm Shipment\" src=\"/App_Themes/" + Page.Theme + "/images/confirm-shipment.png\"></a><br />";
                ltrAction.Text += "<a title=\"Capture\" href=\"javascript:void(0);\" onclick=\"CaptureClick(" + OrderNumber.ToString() + ");\"><img title=\"\" alt=\"Capture\" src=\"/App_Themes/" + Page.Theme + "/images/capture.png\"></a><br />";
            }
            else if (Transtatus.ToLower() == "pending")
            {
                ltrStatus.Text += "<strong style=\"color:#D3321C;font-size:10px;\"><br />" + Transtatus.ToString() + "</strong>";
                ltrStatus.Text += "<br /><span style=\"color:#000000;\">" + statusName.ToString() + "</span>";
            }
            else if (Transtatus.ToLower() == "captured")
            {
                ltrStatus.Text += "<strong style=\"color:#348934;font-size:10px;\"><br />" + Transtatus.ToString() + "</strong>";
                ltrStatus.Text += "<br /><span style=\"color:#000000;\">" + statusName.ToString() + "</span>";
                ltrAction.Text += "<a title=\"Refund\" href=\"javascript:void(0);\" onclick=\"RefundClick(" + OrderNumber.ToString() + ",'" + OrderTotal.ToString() + "','" + RefundAmount.ToString() + "');\"><img title=\"Refund\" alt=\"Refund\" src=\"/App_Themes/" + Page.Theme + "/images/RefundList.png\"></a><br />";
            }
            else if (Transtatus.ToLower() == "partially refunded")
            {
                ltrAction.Text += "<a title=\"Refund\" href=\"javascript:void(0);\" onclick=\"RefundClick(" + OrderNumber.ToString() + ",'" + OrderTotal.ToString() + "','" + RefundAmount.ToString() + "');\"><img title=\"Refund\" alt=\"Refund\" src=\"/App_Themes/" + Page.Theme + "/images/RefundList.png\"></a><br />";
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
        /// Gets the Products List By CartID
        /// </summary>
        /// <param name="cartId">int CartID</param>
        /// <param name="ltrProduct">Literal ltrProduct</param>
        private void GetProduct(Int32 cartId, Literal ltrProduct)
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

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            grvBulkPrint.PageIndex = 0;
            ViewState["SearchFilter"] = null;
            GetOrderListByStoreId(1, pageSize);
        }
    }
}