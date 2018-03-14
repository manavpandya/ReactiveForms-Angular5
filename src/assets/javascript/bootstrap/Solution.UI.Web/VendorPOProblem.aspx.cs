using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.Text;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Solution.Data;

namespace Solution.UI.Web
{
    public partial class VendorPOProblem : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetValidUntilExpires(false);

            if (!IsPostBack)
            {
                if (Request.QueryString["type"] != null)
                {
                    ordertr.Visible = false;
                }
                if (Request.QueryString["pono"] != null && Request.QueryString["pono"].ToString().Length > 0)
                {
                    BindCart();
                }
                else
                {
                    btnSubmit.Visible = false;
                }
            }
        }

        /// <summary>
        /// Binds the Cart for Vendor OP Problem
        /// </summary>
        private void BindCart()
        {
            VendorComponent objVendorReply = new VendorComponent();
            DataSet ds = new DataSet();
            if (Request.QueryString["type"] != null)
            {
                ds = GetVendorProductsforWarehouse(Convert.ToInt32(Request.QueryString["pono"]));
            }
            else
            {
                ds = GetVendorProductsForPO(Convert.ToInt32(Request.QueryString["pono"]));
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                gvVendor.DataSource = ds;
                gvVendor.DataBind();
                btnSubmit.Visible = true;
                trVendordetails.Visible = true;
                divGridlist.Visible = true;
                ltPONumber.Text = Convert.ToString(Request.QueryString["pono"]);
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

                ltvphone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
            }
            else
            {
                trVendordetails.Visible = false;
                divGridlist.Visible = false;
                btnSubmit.Visible = false;
                lblmsg.Text = "No Record found...";
            }
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
            SQLAccess dbAccess = new SQLAccess();
            value = Convert.ToString(dbAccess.ExecuteScalarQuery("Select ConfigValue From dbo.tb_AppConfig Where ConfigName='" + AppConfigName.Replace("'", "''") + "' And Deleted<>1 and StoreID=" + StoreID));
            return value;
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            SQLAccess objSql = new SQLAccess();
            if (Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT PONumber FROM tb_PurchaseOrder WHERE PONumber=" + Request.QueryString["pono"].ToString() + " AND isnull(IsProblem,0)=1")) > 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Already request has been sent.');", true);
                return;
            }
            if (rdocheck.SelectedIndex == -1)
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Please choose option.');", true);
                return;
            }
            if (rdocheck.SelectedIndex == 2)
            {
                if (txtOther.Text.ToString().Trim() == "")
                {
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Please write text.');", true);
                    txtOther.Focus();
                    return;
                }
            }

            objSql.ExecuteNonQuery("UPDATE tb_PurchaseOrder SET IsProblem=1,ProblemType='" + rdocheck.SelectedItem.Text.ToString() + "',ProblemDesc='" + txtOther.Text.ToString().Replace("'", "''") + "' WHERE PONumber=" + Request.QueryString["pono"].ToString() + "");
            try
            {

                if (Request.QueryString["type"] != null)
                {
                    //Ware House log.
                    objSql.ExecuteNonQuery("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber,[Description],VendorName,CreaedOn) VALUES (-1,25," + Convert.ToInt32(Request.QueryString["pono"].ToString()) + "," + Convert.ToInt32(0) + ",'" + rdocheck.SelectedItem.Text.ToString() + "<br/>" + txtOther.Text.ToString().Trim().Replace("'", "''") + "','" + ltvname.Text.ToString().Replace("'", "''") + "'," + DateTime.Now.ToString() + ")");
                    OrderComponent objAddOrder = new OrderComponent();
                    objAddOrder.InsertOrderlog(25, Convert.ToInt32(0), "", Convert.ToInt32(Session["AdminID"].ToString()));
                }
                else
                {
                    objSql.ExecuteNonQuery("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber,[Description],VendorName,CreaedOn) VALUES (-1,26," + Convert.ToInt32(Request.QueryString["pono"].ToString()) + "," + Convert.ToInt32(ltOrder.Text.ToString()) + ",'" + rdocheck.SelectedItem.Text.ToString() + "<br/>" + txtOther.Text.ToString().Trim().Replace("'", "''") + "','" + ltvname.Text.ToString().Replace("'", "''") + "'," + DateTime.Now.ToString() + ")");
                    OrderComponent objAddOrder = new OrderComponent();
                    objAddOrder.InsertOrderlog(26, Convert.ToInt32(ltOrder.Text.ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                }
            }
            catch
            {
            }
            SendMail();


            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Your problem request has been sent successfully.');", true);
        }

        /// <summary>
        /// Sends the Mail to Admin
        /// </summary>
        public void SendMail()
        {

            VendorComponent objVendorReply = new VendorComponent();
            String vendorname = "";
            int storeid = 0;
            if (Request.QueryString["storeid"] != null)
            {

                storeid = Convert.ToInt32(Request.QueryString["storeid"].ToString());
            }
            else
            {
                storeid = 1;
            }
            string TOAddress = GetAppConfigByName("ContactMail_ToAddress", storeid);//AppLogic.AppConfig("ContactMail_ToAddress");

            TOAddress = TOAddress + ";Demo@DemoSystem.com";
            DataSet ds1 = new DataSet();

            try
            {
                string Body = "";

                StringBuilder sw = new StringBuilder(4000);
                sw.Append("<html>");
                sw.Append("<head>");
                sw.Append("<meta content='text/html; charset=iso-8859-1' http-equiv='Content-Type'/>");
                sw.Append("<title>Welcome to " + GetAppConfigByName("StoreName", storeid) + "</title>");

                sw.Append("<link type='text/css' rel='stylesheet' href='" + GetAppConfigByName("LIVE_SERVER", storeid) + "/App_Themes/Gray/css/style.css' />    ");

                sw.Append("</head>");
                sw.Append("<body style='background:none;'>");
                sw.Append("<table cellspacing='0' cellpadding='0' align='center' class='popup_docwidth' style='background-color:#ffffff;border:1px solid #CCCCCC;width:600px;font-family:none;'>");
                sw.Append("<tr>");
                sw.Append("<td>");
                sw.Append("<table cellspacing='0' cellpadding='0' border='0' align='center'>");
                sw.Append("<tr>");
                sw.Append("<td>");
                sw.Append("<table cellspacing='0' cellpadding='0' border='0' align='left' width='100%'>");
                sw.Append("<tr class='pop_header'>");
                sw.Append("<td style='padding:10px;'>");
                sw.Append("<img  style='border-width:0px;' src=\"" + GetAppConfigByName("LIVE_SERVER", storeid) + "/images/welcome.png\"/>");
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td><table style='padding-left:10px;' cellspacing='0' cellpadding='4' border='0' align='left' width='100%'>");
                sw.Append("<tr>");
                sw.Append("<td height='25' style='width:100%' align='left' valign='bottom'  colspan='2'><div><span>Dear Administrator,</span></div></td>"); //TOAddress 
                sw.Append("</tr>");
                sw.Append("<tr><td height='10px'></td></tr>");
                if (ltOrder.Text.ToString().Trim() != "")
                    sw.Append("<tr><td><b style='color:#4D4D4D;'>Order Number :</b>&nbsp;&nbsp;&nbsp;&nbsp; " + ltOrder.Text.ToString() + "</td></tr>");
                if (ltvname.Text.ToString().Trim() != "")
                    sw.Append("<tr><td><b style='color:#4D4D4D;'>Vendor Name :</b>&nbsp;&nbsp;&nbsp;&nbsp; " + ltvname.Text.ToString().Trim() + "</td></tr>");
                if (ltvphone.Text.ToString().Trim() != "")
                    sw.Append("<tr><td><b style='color:#4D4D4D;'>Phone :</b>&nbsp;&nbsp;&nbsp;&nbsp; " + ltvphone.Text.ToString() + "</td></tr>");
                if (ltvemail.Text.ToString().Trim() != "")
                    sw.Append("<tr><td><b style='color:#4D4D4D;'>Email :</b>&nbsp;&nbsp;&nbsp;&nbsp; " + ltvemail.Text.ToString() + "</td></tr>");

                sw.Append("</table>");

                String Subject = "";

                Subject = "Purchase order number " + Request.QueryString["pono"].ToString() + " Problem.";

                if (Request.QueryString["ID"] != null)
                    Subject = "Availability notification Inquiry";
                sw.Append("</td></tr>");

                sw.Append(" <tr>");

                sw.Append("<td align='left'  style='padding:4px 0 4px 0;color:red'>");
                sw.Append("<b>&nbsp;Reason:</b> " + rdocheck.SelectedItem.Text.ToString() + "<br/>" + txtOther.Text.ToString() + "");

                sw.Append("</td></tr>");

                sw.Append("<tr><td>");
                sw.Append(" <table border='0' cellpadding='0' cellspacing='0' class='table-noneforOrder' width='100%'> ");
                sw.Append("<tbody><tr>");
                sw.Append("<th align='left' valign='middle' style='width40%' ><b>Product Name</b></th>");
                sw.Append("<th align='left' valign='middle' style='width:20%' ><b> SKU</b></th>");
                sw.Append("<th valign='middle' style='width: 10%;text-align:center;'><b>Quantity</b></th>");
                sw.Append("</tr>");

                foreach (GridViewRow row in gvVendor.Rows)
                {
                    try
                    {
                        Label lblName = (Label)row.FindControl("lblName");
                        Label lblSKU = (Label)row.FindControl("lblSKU");
                        Label txtQuantity = (Label)row.FindControl("lblQuantity");
                        Label txtPrice = (Label)row.FindControl("lblPrice");
                        sw.Append("<tr>");
                        sw.Append("<td align='left' valign='middle' style='width:60%' >" + lblName.Text.Trim() + "</td>");
                        sw.Append("<td align='left' valign='middle' style='width:20%' >" + lblSKU.Text.Trim() + "</td>");
                        sw.Append("<td valign='middle' style='width: 20%;text-align:center;'>" + txtQuantity.Text.Trim() + "</td>");
                        sw.Append("</tr>");
                    }
                    catch { }
                }
                sw.Append("</tbody></table>");
                sw.Append("</td></tr>");

                sw.Append(" <tr>");

                sw.Append("<td align='left'  style='padding-bottom: 10px;'>");
                sw.Append("<br/>Thank you,<br/>");
                sw.Append(GetAppConfigByName("StoreName", storeid) + " Team" + "<br/>");
                sw.Append("</td></tr>");
                sw.Append("</table></td></tr>");
                sw.Append("<tr>");
                sw.Append("<td style='padding-bottom:5px;vertical-align:middle;font-size:11px;color:#ffffff;font-family:Verdana,Arial,Helvetica,sans-serif;font-weight:bold;line-height:30px;'>");
                sw.Append(" " + GetAppConfigByName("footerrightmail", storeid) + " ");
                sw.Append("</td></tr></table></td>");
                sw.Append("</tr>");
                sw.Append("</table>");
                sw.Append("</body></html>");
                AlternateView av = AlternateView.CreateAlternateViewFromString(sw.ToString(), null, "text/html");
                Body = sw.ToString();
                Body = Body.ToString().Replace("<br><br><br><br>", "<br>");
                CommonOperations.SendMail(TOAddress, Subject, Body, Request.UserHostAddress.ToString(), true, av);
            }
            catch (Exception ex)
            {
                lblmsg.Text = "Error Occurred While Sending Mail";
            }
        }

        /// <summary>
        /// Gets the Vendor Products for PO
        /// </summary>
        /// <param name="Pono">int Pono</param>
        /// <returns>Returns the Vendor Product PO Details</returns>
        public DataSet GetVendorProductsForPO(int Pono)
        {
            return CommonComponent.GetCommonDataSet(@"SELECT  dbo.tb_Product.Name, dbo.tb_PurchaseOrderItems.Quantity, dbo.tb_PurchaseOrderItems.PONumber,dbo.tb_PurchaseOrder.OrderNumber,
                                    dbo.tb_PurchaseOrderItems.ProductID, dbo.tb_PurchaseOrderItems.Price, ISNULL(dbo.tb_PurchaseOrderItems.IsShipped, 0) 
                                    AS IsShipped,ISNULL(dbo.tb_PurchaseOrderItems.Ispaid, 0) as Ispaid, dbo.tb_Product.SKU, dbo.tb_Vendor.Name AS Vname, dbo.tb_Vendor.Email, 
                                    dbo.tb_PurchaseOrder.OrderNumber,tb_OrderShippedItems.TrackingNumber, dbo.tb_PurchaseOrder.PODate,dbo.tb_Vendor.Phone,isnull(tb_OrderShippedItems.ShippedVia,'') as ShippedVia,isnull(tb_OrderShippedItems.ShippedOn,'') as ShippedOn 
                                    FROM dbo.tb_Product INNER JOIN
                                    dbo.tb_PurchaseOrderItems ON dbo.tb_Product.ProductID = dbo.tb_PurchaseOrderItems.ProductID INNER JOIN
                                    dbo.tb_PurchaseOrder ON dbo.tb_PurchaseOrderItems.PONumber = dbo.tb_PurchaseOrder.PONumber INNER JOIN
                                    dbo.tb_Vendor ON dbo.tb_PurchaseOrder.VendorID = dbo.tb_Vendor.VendorId LEFT OUTER JOIN
                                    dbo.tb_VendorQuoteReply AS rep ON rep.VendorID = dbo.tb_PurchaseOrder.VendorID AND  
                                    rep.RefProductID = dbo.tb_PurchaseOrderItems.ProductID AND rep.OrderNumber = dbo.tb_PurchaseOrder.OrderNumber 
                                    inner join dbo.tb_OrderShippedItems on tb_OrderShippedItems.RefProductID = dbo.tb_PurchaseOrderItems.ProductID  
                                    WHERE dbo.tb_PurchaseOrderItems.PONumber= " + Pono + " and tb_OrderShippedItems.Ordernumber = dbo.tb_PurchaseOrder.OrderNumber");

        }

        /// <summary>
        /// Gets the Vendor Products for Warehouse.
        /// </summary>
        /// <param name="Pono">int Pono</param>
        /// <returns>Returns the Vendor Product for Warehouse</returns>
        public DataSet GetVendorProductsforWarehouse(int Pono)
        {
            return CommonComponent.GetCommonDataSet(@"SELECT  dbo.tb_Product.Name, dbo.tb_PurchaseOrderItems.Quantity, dbo.tb_PurchaseOrderItems.PONumber,dbo.tb_PurchaseOrder.OrderNumber,
                                dbo.tb_PurchaseOrderItems.ProductID, dbo.tb_PurchaseOrderItems.Price, ISNULL(dbo.tb_PurchaseOrderItems.IsShipped, 0) 
                                AS IsShipped,ISNULL(dbo.tb_PurchaseOrderItems.Ispaid, 0) as Ispaid,  dbo.tb_Product.SKU, dbo.tb_Vendor.Name AS Vname, dbo.tb_Vendor.Email, 
                                dbo.tb_PurchaseOrder.OrderNumber,dbo.tb_PurchaseOrderItems.TrackingNumber, dbo.tb_PurchaseOrder.PODate,dbo.tb_Vendor.Phone,isnull(tb_PurchaseOrderItems.ShippedVia,'') as ShippedVia,isnull(tb_PurchaseOrderItems.ShippedOn,'') as ShippedOn 
                                FROM dbo.tb_Product INNER JOIN
                                dbo.tb_PurchaseOrderItems ON dbo.tb_Product.ProductID = dbo.tb_PurchaseOrderItems.ProductID INNER JOIN
                                dbo.tb_PurchaseOrder ON dbo.tb_PurchaseOrderItems.PONumber = dbo.tb_PurchaseOrder.PONumber INNER JOIN
                                dbo.tb_Vendor ON dbo.tb_PurchaseOrder.VendorID = dbo.tb_Vendor.VendorId LEFT OUTER JOIN
                                dbo.tb_VendorQuoteReply AS rep ON rep.VendorID = dbo.tb_PurchaseOrder.VendorID AND  
                                rep.RefProductID = dbo.tb_PurchaseOrderItems.ProductID AND rep.OrderNumber = dbo.tb_PurchaseOrder.OrderNumber 
                                WHERE dbo.tb_PurchaseOrderItems.PONumber=" + Pono + " AND tb_PurchaseOrder.OrderNumber=0");

        }
    }
}