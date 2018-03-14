using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Components;
using System.IO;
using System.Data;
using System.Net.Mail;
using System.Collections;
using System.Text;

namespace Solution.UI.Web.RMA.HPDYahoo
{
    public partial class ReturnMerchandise : System.Web.UI.Page
    {
        DataSet dsOrderShoppingCart = new DataSet();
        Boolean flag = false;
        String QuesType = null;
        String Ques = null;
        Int32 ReturnID = 0;
        bool flagreturn = false;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Int32 CustomerID = 0, storeid = 0;
                UserControl userleft = (UserControl)Page.Master.FindControl("leftmenu");
                if (userleft != null)
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl hideleftmenu = (System.Web.UI.HtmlControls.HtmlGenericControl)userleft.FindControl("hideleftmenu");
                    if (hideleftmenu != null)
                    {
                        hideleftmenu.Visible = false;
                    }
                }
                if (Session["FirstName"] != null)
                {
                    Int32.TryParse(Session["CustID"].ToString(), out CustomerID);
                }
                else if (Request.QueryString["ONo"] != null)
                {
                    Int32 OrderNumber = 0;
                    Int32.TryParse(Server.UrlDecode(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString())), out OrderNumber);
                    DataSet dstmp = new DataSet();
                    dstmp = CommonComponent.GetCommonDataSet(" select customerid,storeid,isnull(reforderid,'') as reforderid from tb_order where ordernumber=" + OrderNumber);
                    CustomerID = Convert.ToInt32(dstmp.Tables[0].Rows[0]["CustomerID"].ToString());
                    storeid = Convert.ToInt32(dstmp.Tables[0].Rows[0]["Storeid"].ToString());
                    ViewState["StoreId"] = storeid.ToString();
                }
                try
                {
                    hdReturnFee.Value = Convert.ToString(CommonComponent.GetScalarCommonData(" select isnull(configvalue,0) from tb_appconfig where ConfigName='ReturnFee' and storeid=" + storeid)).Trim();
                }
                catch { }
                if (CustomerID == 0)
                    return;

                BindCustomer(CustomerID, storeid.ToString());

                // ******** If Contact Inquiry for Product then it will display product data.
                if (Request.QueryString["ProdID"] != null && !IsPostBack && Request.QueryString["ONo"] != null)
                {
                    OrderComponent objOrder = new OrderComponent();
                    rbtReason1.Checked = true;
                    int ProductId = 0;
                    int OrderNumber = 0;
                    String OrderDate = string.Empty;
                    Int32.TryParse(Request.QueryString["ProdID"].ToString(), out ProductId);
                    Int32.TryParse(Server.UrlDecode(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString())), out OrderNumber);

                    if (ProductId > 0 && OrderNumber > 0)
                    {
                        txtOrderNumber.Text = OrderNumber.ToString();
                        txtOrderNumber.ReadOnly = true;
                        DataSet dsProductReturn = new DataSet();

                        //string StrQuery = "SELECT isnull(tb_Product.ReturnPolicy,'') as ReturnPolicy,tb_Product.Name,tb_Product.DistributorID,tb_Product.ImageName,tb_Product.SKU,tb_Product.MainCategory,tb_Product.SEName, tb_OrderedShoppingCartItems.Quantity," +
                        //                    " tb_OrderedShoppingCartItems.RefProductID,tb_OrderedShoppingCartItems.VariantNames,tb_OrderedShoppingCartItems.VariantValues, " +
                        //                    " isnull(s.TrackingNumber,o.TrackingNumber) TrackingNumber,isnull(s.ShippedVia,o.ShippedVia) ShippedVia, " +
                        //                    " (isnull(s.Shipped,0) | o.shipped) as Shipped,isnull(s.ShippedOn,o.ShippedOn) ShippedOn,  " +
                        //                    " tb_Product.Description,tb_OrderedShoppingCartItems.Price As SalePrice,o.OrderDate FROM tb_Product INNER JOIN tb_OrderedShoppingCartItems " +
                        //                    " left outer join (select RefProductID,OrderNumber,trackingNumber,ShippedVia,Shipped,ShippedOn from  " +
                        //                    " tb_OrderShippedItems where ordernumber=" + OrderNumber + " ) s on s.RefProductID=tb_OrderedShoppingCartItems.RefProductID  " +
                        //                    " inner join (select convert(bit,isnull(len(ShippedOn),0)) as shipped,ShoppingCardID,ShippingTrackingNumber " +
                        //                    " as TrackingNumber,ShippedVia,ShippedOn,orderdate from tb_Order where ordernumber=" + OrderNumber + " ) o ON  " +
                        //                    " o.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID on " +
                        //                    " tb_Product.ProductID = tb_OrderedShoppingCartItems.RefProductID Where  OrderedShoppingCartID = (SELECT Top 1 OrderedShoppingCartID FROM  " +
                        //                    " tb_OrderShoppingCart  Where  tb_OrderShoppingCart.OrderNumber=" + OrderNumber + ") AND tb_OrderedShoppingCartItems.RefProductID NOT IN  " +
                        //                    " (SELECT ProductID FROM tb_ReturnItem WHERE tb_ReturnItem.OrderedNumber=" + OrderNumber + " )  " +
                        //                    " And tb_Product.ProductID=" + ProductId + " order by ShippedOn DESC";

                        string StrQuery = "SELECT     TOP (100) PERCENT ISNULL(dbo.tb_Product.ReturnPolicy, '') AS ReturnPolicy, dbo.tb_Product.Name, dbo.tb_Product.DistributorID, dbo.tb_Product.ImageName,  " +
                            " dbo.tb_Product.SKU, dbo.tb_Product.MainCategory, dbo.tb_Product.SEName, dbo.tb_OrderedShoppingCartItems.Quantity, " +
                            " dbo.tb_OrderedShoppingCartItems.RefProductID, dbo.tb_OrderedShoppingCartItems.VariantNames, dbo.tb_OrderedShoppingCartItems.VariantValues, " +
                            " ISNULL(s.TrackingNumber, o.TrackingNumber) AS TrackingNumber, ISNULL(s.ShippedVia, o.ShippedVIA) AS ShippedVia, ISNULL(s.Shipped, 0) | o.shipped AS Shipped, " +
                            " ISNULL(s.ShippedOn, o.ShippedOn) AS ShippedOn, dbo.tb_Product.Description, dbo.tb_OrderedShoppingCartItems.Price AS SalePrice, o.OrderDate " +
                            " FROM         dbo.tb_Product INNER JOIN " +
                            " dbo.tb_OrderedShoppingCartItems LEFT OUTER JOIN " +
                            " (SELECT     RefProductID, OrderNumber, TrackingNumber, ShippedVia, Shipped, ShippedOn  FROM dbo.tb_OrderShippedItems " +
                            " WHERE      (OrderNumber = " + OrderNumber + ")) AS s ON s.RefProductID = dbo.tb_OrderedShoppingCartItems.RefProductID INNER JOIN  " +
                            " (SELECT     CONVERT(bit, ISNULL(LEN(ShippedOn), 0)) AS shipped, ShoppingCardID, ShippingTrackingNumber AS TrackingNumber, ShippedVIA, ShippedOn,OrderDate " +
                            " FROM          dbo.tb_Order " +
                            " WHERE      (OrderNumber = " + OrderNumber + ")) AS o ON o.ShoppingCardID = dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID ON  " +
                            " dbo.tb_Product.ProductID = dbo.tb_OrderedShoppingCartItems.RefProductID " +
                            " WHERE (dbo.tb_Product.ProductID = " + ProductId + ") and dbo.tb_OrderedShoppingCartItems.RefProductID NOT IN " +
                            " (SELECT   case when isnull(Sum(tb_ReturnItem.Quantity),0) >= Sum(tb_OrderedShoppingCartItems.Quantity) then    ProductID else 0 end as ProductID  FROM dbo.tb_ReturnItem INNER JOIN tb_OrderedShoppingCartItems on RefProductID=ProductID   WHERE OrderedNumber = " + OrderNumber + " GROUP BY ProductID) ORDER BY ShippedOn DESC ";

                        dsProductReturn = CommonComponent.GetCommonDataSet(StrQuery.ToString());

                        if (dsProductReturn.Tables.Count > 0 && dsProductReturn != null && dsProductReturn.Tables[0].Rows.Count > 0)
                        {
                            hdnProductId.Value = ProductId.ToString();
                            lblProductName.Text = dsProductReturn.Tables[0].Rows[0]["Name"].ToString();
                            lblProductSKU.Text = dsProductReturn.Tables[0].Rows[0]["SKU"].ToString();
                            Int32 Qty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select (Select Sum(tb_OrderedShoppingCartItems.Quantity) from tb_OrderedShoppingCartItems WHERE (dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID IN (SELECT ShoppingCardID FROM  dbo.tb_Order  WHERE (OrderNumber = " + SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["ono"])) + ")))  AND (dbo.tb_OrderedShoppingCartItems.RefProductID = " + hdnProductId.Value + "))-(select IsNULL(Sum(tb_ReturnItem.Quantity),0) from dbo.tb_ReturnItem where OrderedNumber=" + SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["ono"])) + " and ProductID=" + hdnProductId.Value + ")"));
                            //TxtProductQty.Text = dsProductReturn.Tables[0].Rows[0]["Quantity"].ToString();
                            TxtProductQty.Text = Qty.ToString();
                            ViewState["Qty"] = TxtProductQty.Text.Trim();
                            if (!string.IsNullOrEmpty(dsProductReturn.Tables[0].Rows[0]["ReturnPolicy"].ToString().Trim()))
                            {
                                lblReturn.Text = dsProductReturn.Tables[0].Rows[0]["ReturnPolicy"].ToString().Trim();
                            }
                            else
                            {
                                lblReturn.Text = Convert.ToString(CommonComponent.GetScalarCommonData(" select isnull(ConfigValue,'') from tb_appconfig where configname='ReturnPolicy' and storeid=" + storeid));
                            }
                            if (lblReturn.Text.Trim() == "")
                                divreturn.Visible = false;
                            txtInvoiceDate.Text = dsProductReturn.Tables[0].Rows[0]["orderdate"].ToString();
                            trReturnProductAll.Visible = false;
                            trRreturnProduct.Visible = true;
                        }
                        else
                        {
                            trReturnProductAll.Visible = true;
                            trRreturnProduct.Visible = false;
                        }
                        // if Order date is null then get records from tb_Order table
                        if (string.IsNullOrEmpty(txtInvoiceDate.Text.ToString()))
                        {
                            DateTime dt = new DateTime();
                        }
                        dsOrderShoppingCart = objOrder.GetInvoiceProductList(OrderNumber);
                        if (dsOrderShoppingCart.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsOrderShoppingCart.Tables[0].Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    txtItemReturned1.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Name"].ToString();
                                    txtItemCode1.Text = dsOrderShoppingCart.Tables[0].Rows[i]["SKU"].ToString();
                                    txtQuantity1.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Quantity"].ToString();
                                    txtproductid1.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Quantity"].ToString();
                                }
                                if (i == 1)
                                {
                                    txtItemReturned2.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Name"].ToString();
                                    txtItemCode2.Text = dsOrderShoppingCart.Tables[0].Rows[i]["SKU"].ToString();
                                    txtQuantity2.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Quantity"].ToString();
                                    txtproductid2.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Quantity"].ToString();
                                }
                                if (i == 2)
                                {
                                    txtItemReturned3.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Name"].ToString();
                                    txtItemCode3.Text = dsOrderShoppingCart.Tables[0].Rows[i]["SKU"].ToString();
                                    txtQuantity3.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Quantity"].ToString();
                                    txtproductid3.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Quantity"].ToString();
                                }
                                if (i == 3)
                                {
                                    txtItemReturned4.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Name"].ToString();
                                    txtItemCode4.Text = dsOrderShoppingCart.Tables[0].Rows[i]["SKU"].ToString();
                                    txtQuantity4.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Quantity"].ToString();
                                    txtproductid4.Text = dsOrderShoppingCart.Tables[0].Rows[i]["Quantity"].ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Bind the Customer Details of CustomerID
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="storeid">string storeid</param>
        private void BindCustomer(int CustomerID, string storeid)
        {
            DataSet dsCustomer = null;
            CustomerComponent ObjCustomer = new CustomerComponent();
            ViewState["CustomerID"] = CustomerID;
            dsCustomer = ObjCustomer.GetCustomerDetailByCustID(CustomerID);
            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                txtCustomerName.Text = dsCustomer.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsCustomer.Tables[0].Rows[0]["LastName"].ToString();
                txtEmail.Text = dsCustomer.Tables[0].Rows[0]["Email"].ToString();
                txtCustomerName.Enabled = false;
                txtEmail.Enabled = false;
                txtOrderNumber.Enabled = false;
                txtInvoiceDate.Enabled = false;
            }
        }

        /// <summary>
        /// This Function Send the Mail on "ToAddress" 
        /// That Contain the Product Information or Feedback Form.    
        /// </summary>
        /// <param name="TOAddress">string TOAddress</param>
        public void SendMail(string TOAddress)
        {

            try
            {
                string returnitem = "", merchcode = "", quantity = "";
                String itemreturn = null;
                String Order = null;
                string Body = "";

                if (chkItem1.Checked == false && chkItem2.Checked == false && chkItem3.Checked == false && chkItem4.Checked == false)
                {
                    itemreturn = "<tr><td style='width: 50%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'>" + lblProductName.Text.Trim() + "</td> " +
                                   " <td style='width: 25%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'>" + lblProductSKU.Text.Trim() + " </td><td style='width: 20%;font-size:12px;font-family:Arial,Helvetica,sans-serif;'> " +
                                   "     " + TxtProductQty.Text.Trim() + "</td></tr>";
                }

                if (chkItem1.Checked)
                {
                    itemreturn = "<tr><td style='width: 50%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'>" + txtItemReturned1.Text + "</td> " +
                                   " <td style='width: 25%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'>" + txtItemCode1.Text + " </td><td style='width: 20%;font-size:12px;font-family:Arial,Helvetica,sans-serif;'> " +
                                   "     " + txtQuantity1.Text + "</td></tr>";
                }

                DateTime OrderDate = Convert.ToDateTime(txtInvoiceDate.Text);
                Order = OrderDate.ToShortDateString();
                if (rbtReason1.Checked)
                    QuesType = lblReason1.Text.ToString();
                else if (rbtReason2.Checked)
                    QuesType = lblReason2.Text.ToString();
                else if (rbtReason3.Checked)
                {
                    if (!String.IsNullOrEmpty(txtWrongItem.Text.Trim()))
                        QuesType = lblReason3.Text.ToString() + " - " + txtWrongItem.Text;
                    else
                        QuesType = lblReason3.Text.ToString();
                }
                else
                    QuesType = lblReason4.Text.ToString();
                Ques = txtAdditionalInformation.Text;


                if (chkItem2.Checked)
                {
                    returnitem = txtItemReturned2.Text;
                    merchcode = txtItemCode2.Text;
                    quantity = txtQuantity2.Text;

                    if ((!string.IsNullOrEmpty(returnitem)) || (!string.IsNullOrEmpty(merchcode)) || (!string.IsNullOrEmpty(quantity)))
                    {
                        itemreturn = itemreturn + "<tr><td style='width: 50%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'>" + txtItemReturned2.Text + "</td> " +
                                   " <td style='width: 25%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'>" + txtItemCode2.Text + " </td><td style='width: 20%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'> " +
                                   "     " + txtQuantity2.Text + "</td></tr>";

                    }
                }
                if (chkItem3.Checked)
                {
                    returnitem = "";
                    merchcode = "";
                    quantity = "";
                    returnitem = txtItemReturned3.Text;
                    merchcode = txtItemCode3.Text;
                    quantity = txtQuantity3.Text;

                    if ((!string.IsNullOrEmpty(returnitem)) || (!string.IsNullOrEmpty(merchcode)) || (!string.IsNullOrEmpty(quantity)))
                    {
                        itemreturn = itemreturn + "<tr><td style='width: 50%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'>" + txtItemReturned3.Text + "</td> " +
                                   " <td style='width: 25%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'>" + txtItemCode3.Text + " </td><td style='width: 20%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'> " +
                                   "     " + txtQuantity3.Text + "</td></tr>";

                    }
                }
                if (chkItem4.Checked)
                {
                    returnitem = "";
                    merchcode = "";
                    quantity = "";
                    returnitem = txtItemReturned4.Text;
                    merchcode = txtItemCode4.Text;
                    quantity = txtQuantity4.Text;

                    if ((!string.IsNullOrEmpty(returnitem)) || (!string.IsNullOrEmpty(merchcode)) || (!string.IsNullOrEmpty(quantity)))
                    {
                        itemreturn = itemreturn + "<tr><td style='width: 50%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'>" + txtItemReturned4.Text + "</td> " +
                                   " <td style='width: 25%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'>" + txtItemCode4.Text + " </td><td style='width: 20%;font-size:12px;font-family:Arial,Helvetica,sans-serif;' class='cell'> " +
                                   "     " + txtQuantity4.Text + "</td></tr>";

                    }
                }

                Int32 RMARequestNumber = 0;
                String Query = "select isnull(max(ReturnItemID),0)+1  from  tb_ReturnItem";
                Object objRMAReqNumber = CommonComponent.GetScalarCommonData(Query);

                if (objRMAReqNumber != null)
                {
                    RMARequestNumber = Convert.ToInt32(objRMAReqNumber.ToString());
                }

                DataSet ds = new DataSet();
                ds = CommonComponent.GetCommonDataSet(" select ConfigValue,configName from tb_appconfig where configName in ('LIVE_SERVER','LIVE_SERVER_NAME','STORENAME') and storeid=( select storeid from tb_order where ordernumber=" + Server.UrlDecode(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString().Trim())) + " )");
                Hashtable ht = new Hashtable();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ht.Add(dr["ConfigName"].ToString().ToUpper(), dr["ConfigValue"].ToString());
                }

                string storename = ht["STORENAME"].ToString();
                string liveserver = ht["LIVE_SERVER"].ToString();

                StringBuilder sw = new StringBuilder(4000);
                #region Mail Formate

                sw.Append("<html>");
                sw.Append("<head>");
                sw.Append("<meta content='text/html; charset=iso-8859-1' http-equiv='Content-Type'/>");
                sw.Append("<title>Welcome to " + storename + "</title>");
                sw.Append("<link type='text/css' rel='stylesheet' href='../Client/style.css' />    ");
                sw.Append("<style type='text/css'>");
                sw.Append("<!-- " +

                          ".popup_docwidth {" +
                         "width:596px;" +
                         "border:solid #d6d6d6 1px;" +
                         "background-color: #FFFFFF;" +
                         "margin:0px auto;" +
                        "} " +
                        ".img_left{ float:left; padding:15px 0px 15px 15px;}" +

                        ".pop_header_row2 {" +
                             "text-align:center;" +
                             "font-family: Verdana, Arial, Helvetica, sans-serif;" +
                             "font-size:12px;" +
                             "line-height:28px;" +
                             "color:#FFF;" +
                             "border-bottom:1px #fff solid;" +
                             "background:#4587CC ;" +
                            "} " +

                        ".pop_header_row2 a {" +
                         "font-family: Verdana, Arial, Helvetica, sans-serif;" +
                         "font-size:12px;" +
                         "color:#FFF;" +
                         "text-decoration:none;" +
                        "}" +
                        ".pop_header_row2 a:hover {" +
                         "font-family: Verdana, Arial, Helvetica, sans-serif;" +
                         "font-size:12px;" +
                         "color:#fff;" +
                         "text-decoration:underline;" +
                        "} " +



                        ".popup_cantain {" +
                         "width:586px;" +
                         "padding-left:10px;padding-right:10px;" +
                         "font-family:Arial, Helvetica, sans-serif;" +
                         "font-size: 12px;" +
                         "font-weight:normal;" +
                         "color: #403f3f;" +
                         "text-decoration: none;" +
                         "line-height: 20px;" +
                        "} " +



                        ".popup_cantain a {" +
                         "font-family: Verdana, Arial, Helvetica, sans-serif;" +
                         "font-size: 12px;" +
                         "color: #4587CC ;" +
                         "text-decoration: none;" +
                        "}" +

                        ".popup_cantain a:hover {" +
                         "font-family: Verdana, Arial, Helvetica, sans-serif;" +
                         "font-size: 12px;" +
                         "color: #000;" +
                         "text-decoration:underline;" +
                        "} " +

                      ".user_bg {" +
                         "width:300px;" +
                         "height:60px;" +
                         "padding-top:7px;" +
                         "padding-left:7px;" +
                         "background:url(images/user_bg.gif) no-repeat left top;" +
                        "} " +


                        ".popup_fotter {width:586px;padding-left:10px;height:30px;font-family:Verdana, Arial, Helvetica, sans-serif;font-size:11px;color:#ffffff;}" +
                        ".style1 {" +
                         "width: 586px;" +
                         "height: 30px;" +
                         "padding-left: 10px;" +
                         "line-height: 30px;" +
                         "font-family: Verdana, Arial, Helvetica, sans-serif;" +
                         "font-size: 11px;" +
                         "color: #fff;" +
                        " font-weight: bold;" +
                        "background:#4587CC " +
                        "}" +

                        ".style1 a{color:#fff;text-decoration:none;}" +
                        ".pop_border_new1 {" +
                        "line-height: 20px;" +
                        "width: 100%;}" +
                        ".pop_border_new1 th {" +
                        "background: none repeat scroll 0 0 #FFFFFF;}" +
                        ".pop_border_new1 td {" +
                         "   border: 1px solid #FFFFFF;" +
                          "  padding: 2px 5px;}" +
                        ".pop_border_new {" +
                            "line-height: 20px;" +
                            "width: 100%;}" +
                        ".pop_border_new th {" +
                            "background: none repeat scroll 0 0 #E4E4E4;}" +
                        ".pop_border_new td {" +
                            "border: 1px solid #DFDFDF;" +
                            "padding: 2px 5px;}" +
                        ".pop_border_new2 {" +
                         "   color: #2A2A2A;" +
                            "font-family: Arial,Helvetica,sans-serif;" +
                            "font-size: 12px;" +
                            "line-height: 25px;" +
                            "text-decoration: none;" +
                            "width: 100%;}" +
                        ".pop_border_new2 th {" +
                            "background: none repeat scroll 0 0 #CFCFCF;" +
                            "color: #2A2A2A;" +
                            "font-family: Arial,Helvetica,sans-serif;" +
                            "font-size: 12px;" +
                            "line-height: 25px;" +
                            "text-decoration: none;}" +
                        ".pop_border_new2 td {" +
                            "border: 1px solid #CFCFCF;" +
                            "color: #2A2A2A;" +
                            "font-family: Arial,Helvetica,sans-serif;" +
                            "font-size: 12px;" +
                            "line-height: 25px;" +
                           " padding: 2px 5px;" +
                            "text-decoration: none;}" +



                        ".username{font-family: Verdana, Arial, Helvetica, sans-serif;" +
                         "font-size: 11px;" +
                         "color: #000; text-align:left } " +


                                      "body { margin:0px; padding:0px;}  " +
                                       "img{ border:0px;} " +
                                        ".toll_free_font {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #8c8c8c;	text-decoration: none;} " +

                                             ".content_font_gray {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #606060;	text-decoration: none;} " +
                                             ".content_font_orange {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #cc0000;	text-decoration: none;} " +
                                             "a.content_font_orange:hover {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #333333;	text-decoration: none;} " +

                                                ".horizone_line { border-bottom:#999999 solid 1px;} " +
                                                 ".footer_font {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #999999;	text-decoration: none;	line-height: 18px;} " +
                                                   "a.footer_font:hover {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #333333;	text-decoration: none;	line-height: 18px;} " +
                                                     "--> " +
                                                     "</style> ");
                sw.Append("</head>");
                sw.Append("<body>");
                sw.Append("<table cellspacing='0' cellpadding='0' align='center' class='popup_docwidth' style='background-color:#ffffff;border:1px solid #CCCCCC;width:600px;font-family:none;'>");
                sw.Append("<tr>");
                sw.Append("<td>");
                sw.Append("<table cellspacing='0' cellpadding='0' border='0' align='center'  >");
                sw.Append("<tr>");
                sw.Append("<td>");
                sw.Append("<tr class='pop_header'>");
                sw.Append("<td style='padding:10px;'>");
                sw.Append("<a title='" + liveserver.ToString() + "' href='http://" + liveserver.ToString().ToLower() + "'>");
                sw.Append("<img style='border-width:0px;' src=\"" + liveserver + "/images/Store_" + Convert.ToInt32(AppLogic.AppConfigs("StoreId").ToString()) + ".png\"/>");
                sw.Append("</a></td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td><table style='padding-left:10px;' cellspacing='0' cellpadding='0' class='popup_cantain' border='0' align='left' width='100%'>");
                sw.Append("<tr>");
                sw.Append("<td height='25' style='width:100%' align='left' valign='bottom'  colspan='2'><div class='popup_cantain'><span style='color: #403F3F;'>Dear Administrator,</span></div></td>"); //TOAddress 
                sw.Append("</tr>");

                sw.Append("     <div class='popup_cantain'>        <tr>");
                sw.Append("      <td height='24px' colspan='2' class='content_font_gray'>");
                sw.Append("        &nbsp;&nbsp;&nbsp;<br />");
                sw.Append("        One of our valuable customer wants some information about our products. Please do");
                sw.Append("         help him/her earliest possible.</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td height='24px' colspan='2' class='content_font_gray'>");
                sw.Append("&nbsp;</td>");
                sw.Append("<tr>");
                sw.Append("<tr>");
                sw.Append("<td height='24px' colspan='2' class='content_font_gray'>");
                sw.Append("The contact information and inquiry provided by customer are as follows:</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td height='24px' width='25%' class='cell' style='margin-top: 10px;'>");
                sw.Append("<b>RMA Request No :</b></td>");
                sw.Append("<td height='24px' width='75%' style='margin-top: 10px;'>");
                sw.Append("" + RMARequestNumber.ToString() + "</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td height='24px' width='20%' class='cell' style='margin-top: 10px;'>");
                sw.Append("<b>Order Number :</b></td>");
                sw.Append("<td height='24px' width='80%' style='margin-top: 10px;'>");
                sw.Append("" + txtOrderNumber.Text.ToString() + "</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td height='24px' class='cell'>");
                sw.Append("    <b>Name :</b></td>");
                sw.Append("<td height='24px'>");
                sw.Append("" + txtCustomerName.Text.ToString() + "</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("   <td height='24px' class='cell'>");
                sw.Append("<b>Email Address :</b><br/><br /></td>");
                sw.Append("<td height='24px'>");
                sw.Append("" + txtEmail.Text.ToString() + "<br/><br /></td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<tr><td colspan='3' height='15px'></td></tr>");
                sw.Append(" <td colspan='3'>");
                //sw.Append("<table width='100%'>");
                sw.Append("<table width='100%' cellspacing='0' cellpadding='0' border='1' style='border-collapse: collapse;' class='pop_border_new2'>");
                sw.Append("<tr>");
                sw.Append("<td style='width: 50%;font-size:13px;font-family:Arial,Helvetica,sans-serif;'>");
                sw.Append("<strong >Items to be Returned </strong>");
                sw.Append("</td>");
                sw.Append("<td style='width: 25%;font-size:13px;font-family:Arial,Helvetica,sans-serif;'>");
                sw.Append("<strong>SKU </strong>");
                sw.Append("</td>");
                sw.Append("<td style='width: 20%;font-size:13px;font-family:Arial,Helvetica,sans-serif;'>");
                sw.Append("<strong>Quantity </strong>");
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("" + itemreturn + "");
                sw.Append("</table>");
                sw.Append("<br /><br />");
                sw.Append("</td>");
                sw.Append("<tr><td colspan='3' height='15px'></td></tr>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td height='24px' class='cell' style='padding-left:10px;'>");
                sw.Append("<b>InvoiceDate :</b></td>");
                sw.Append("<td height='24px'>");
                sw.Append("" + Order + "</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td height='24px' class='cell' style='padding-left:10px;'>");
                sw.Append("<b>Return Reason :</b></td>");
                sw.Append("<td height='24px'>");
                sw.Append("" + QuesType + "</td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td height='24px' class='cell' style='padding-left:10px;'>");
                sw.Append("<b>Other Reasons :</b></td>");
                sw.Append("<td height='24px'>");
                sw.Append("" + Ques + "</td>");
                sw.Append("</tr></div>");
                sw.Append("<tr>");
                sw.Append("<td class='cell' colspan='2' style='padding-bottom: 20px;padding-left:5px;'>");
                sw.Append("<br />");
                sw.Append("Thank you,<br />");
                sw.Append(storename + " Team" + "<br/>");
                sw.Append(" <a class='content_font_orange' style='color:#F2570A' href='http://" + liveserver.ToString().ToLower() + "'>" + AppLogic.AppConfigs("STOREPATH").ToString() + "</a>");
                sw.Append("</td>");
                sw.Append("</tr>");

                sw.Append("</table>");
                String Subject = "RMA Request for Order Number - " + txtOrderNumber.Text.ToString();

                sw.Append("</td>");

                sw.Append("</table></td>");

                sw.Append("</tr>");
                sw.Append("</table>");
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("</table>");
                sw.Append("</body></html>");

                #endregion

                AlternateView av = AlternateView.CreateAlternateViewFromString(sw.ToString(), null, "text/html");
                Body = sw.ToString();
                int CustID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select CustomerID from tb_Order where OrderNumber=" + txtOrderNumber.Text + ""));

                if (CustID != 0)
                {
                    if (CustID.ToString() == ViewState["CustomerID"].ToString())
                    {
                        if (CheckProductAndQuantity())
                        {
                            if (Request.QueryString["ProdID"] != null)
                            {
                                if (InsertRMADeatils() == false)
                                    return;
                            }
                            else
                            {
                                InsertReturnMerchandise();
                            }
                            CommonOperations.SendMail(TOAddress.Trim(), Subject.ToString(), Body.ToString(), Request.UserHostAddress.ToString(), true, av);
                            lblMsg.Visible = true;
                            lblMsg.Text = "E-Mail has been sent successfully.<br />Our Customer Service Representative will respond to you by phone or E-Mail within 24 business hours.";
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (CheckProductAndQuantity())
                        {
                            if (Request.QueryString["ProdID"] != null)
                            {
                                if (InsertRMADeatils() == false)
                                    return;
                            }
                            else
                            {
                                InsertReturnMerchandise();
                            }
                            CommonOperations.SendMail(TOAddress.Trim(), Subject.ToString(), Body.ToString(), Request.UserHostAddress.ToString(), true, av);
                            lblMsg.Visible = true;
                            lblMsg.Text = "E-Mail has been sent successfully.<br />Our Customer Service Representative will respond to you by phone or E-Mail within 24 business hours.";
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please enter valid Order Information.";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('Please enter valid Order Information.');", true);
                }

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg02", "alert('Please enter valid Order Information');", true);
            }
        }

        /// <summary>
        /// check and Insert the Details into Table
        /// </summary>
        protected Boolean CheckProductAndQuantity()
        {
            try
            {
                int Pr1 = 0;
                int Pr2 = 0;
                int Pr3 = 0;
                int Pr4 = 0;
                OrderComponent objOrder = new OrderComponent();
                int qu1 = 0;
                int qu2 = 0;
                int qu3 = 0;
                int qu4 = 0;

                try
                {
                    string storeid = "";
                    storeid = Convert.ToString(CommonComponent.GetScalarCommonData(" select storeid from tb_order where ordernumber=" + Server.UrlDecode(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString().Trim())) + " "));
                }
                catch { }

                if (!String.IsNullOrEmpty(txtItemReturned1.Text.ToString()) && !String.IsNullOrEmpty(txtItemCode1.Text.ToString()) && !String.IsNullOrEmpty(txtQuantity1.Text.ToString()))
                {
                    Int32 PID = 0;
                    if (txtproductid1.Text.ToString() != "" && txtproductid1.Text.ToString() != "0")
                    {
                        PID = Convert.ToInt32(txtproductid1.Text.ToString());
                    }
                    else
                    {

                        PID = GetProductID(txtItemReturned1.Text.ToString().Trim(), txtItemCode1.Text.ToString().Trim());
                    }
                    if (PID == 0)
                    {
                        lblMsg.Visible = false;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg02", "alert('Please enter valid Items to be Returned and Item Code');", true);
                        return false;
                    }
                    else
                    {
                        Pr1 = PID;
                        if (Session["FirstName"] != null)
                            flag = (Convert.ToInt32(CheckForExistsProduct(Convert.ToInt32(PID), Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(txtOrderNumber.Text.ToString()))) == 0) ? true : false;
                        else
                            flag = (Convert.ToInt32(CheckForExistsProduct(Convert.ToInt32(PID), 0, Convert.ToInt32(txtOrderNumber.Text.ToString()))) == 0) ? true : false;

                        if (flag)
                        {
                            if (!string.IsNullOrEmpty(txtQuantity1.Text))
                            {
                                qu1 = CheckQuantityProduct(Convert.ToInt32(txtOrderNumber.Text.ToString()), Convert.ToInt32(Pr1));
                                if (qu1 < Convert.ToInt32(txtQuantity1.Text.ToString()))
                                {
                                    lblMsg.Visible = false;
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg02", "alert('Please enter valid OrderNumber,Item and Quantity of Item1');", true);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            lblMsg.Visible = false;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg02", "alert('Items1 was already returned.'", true);
                            return false;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(txtItemReturned2.Text.ToString()) && !String.IsNullOrEmpty(txtItemCode2.Text.ToString()) && !String.IsNullOrEmpty(txtQuantity2.Text.ToString()))
                {
                    Int32 PID = 0;
                    if (txtproductid2.Text.ToString() != "" && txtproductid2.Text.ToString() != "0")
                    {
                        PID = Convert.ToInt32(txtproductid2.Text.ToString());
                    }
                    else
                    {
                        PID = GetProductID(txtItemReturned2.Text.ToString().Trim(), txtItemCode2.Text.ToString().Trim());
                    }

                    if (PID == 0)
                    {
                        lblMsg.Visible = false;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg03", "alert('Please enter valid Items to be Returned and Item Code');", true);
                        return false;
                    }
                    else
                    {
                        Pr2 = PID;
                        if (Session["FirstName"] != null)
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;
                        else
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), 0, Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;

                        if (flag)
                        {
                            if (!string.IsNullOrEmpty(txtQuantity2.Text))
                            {
                                qu2 = CheckQuantityProduct(Convert.ToInt32(txtOrderNumber.Text.ToString()), Convert.ToInt32(Pr2));
                                if (qu2 < Convert.ToInt32(txtQuantity2.Text.ToString()))
                                {
                                    lblMsg.Visible = false;
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg03", "alert('Please enter valid OrderNumber,Item and  Quantity of Item2');", true);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg03", "alert('Items2 was already returned');", true);
                            lblMsg.Visible = false;
                            return false;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(txtItemReturned3.Text.ToString()) && !String.IsNullOrEmpty(txtItemCode3.Text.ToString()) && !String.IsNullOrEmpty(txtQuantity3.Text.ToString()))
                {
                    Int32 PID = 0;
                    if (txtproductid3.Text.ToString() != "" && txtproductid3.Text.ToString() != "0")
                    {
                        PID = Convert.ToInt32(txtproductid3.Text.ToString());
                    }
                    else
                    {
                        PID = GetProductID(txtItemReturned3.Text.ToString().Trim(), txtItemCode3.Text.ToString().Trim());
                    }
                    if (PID == 0)
                    {
                        lblMsg.Visible = false;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg04", "alert('Please enter valid Items to be Returned and Item Code');", true);
                        return false;
                    }
                    else
                    {
                        Pr3 = PID;
                        if (Session["FirstName"] != null)
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;
                        else
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), 0, Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;

                        if (flag)
                        {
                            if (!string.IsNullOrEmpty(txtQuantity3.Text))
                            {
                                qu3 = CheckQuantityProduct(Convert.ToInt32(txtOrderNumber.Text.ToString()), Convert.ToInt32(Pr3));
                                if (qu3 < Convert.ToInt32(txtQuantity3.Text.ToString()))
                                {
                                    lblMsg.Visible = false;
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg03", "alert('Please enter valid OrderNumber,Item and  Quantity of Item3');", true);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg03", "alert('Items3 was already returned');", true);
                            lblMsg.Visible = false;
                            return false;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(txtItemReturned4.Text.ToString()) && !String.IsNullOrEmpty(txtItemCode4.Text.ToString()) && !String.IsNullOrEmpty(txtQuantity4.Text.ToString()))
                {
                    Int32 PID = 0;
                    if (txtproductid4.Text.ToString() != "" && txtproductid4.Text.ToString() != "0")
                    {
                        PID = Convert.ToInt32(txtproductid4.Text.ToString());
                    }
                    else
                    {
                        PID = GetProductID(txtItemReturned4.Text.ToString().Trim(), txtItemCode4.Text.ToString().Trim());
                    }

                    if (PID == 0)
                    {
                        lblMsg.Visible = false;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg04", "alert('Please enter valid Items to be Returned and Item Code');", true);
                        return false;
                    }
                    else
                    {
                        Pr4 = PID;
                        if (Session["FirstName"] != null)
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;
                        else
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), 0, Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;

                        if (flag)
                        {

                            if (!string.IsNullOrEmpty(txtQuantity4.Text))
                            {
                                qu4 = CheckQuantityProduct(Convert.ToInt32(txtOrderNumber.Text.ToString()), Convert.ToInt32(Pr4));

                                if (qu4 < Convert.ToInt32(txtQuantity4.Text.ToString()))
                                {
                                    lblMsg.Visible = false;
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg04", "alert('Please enter valid OrderNumber,Item and  Quantity of Item4');", true);
                                    return false;
                                }
                            }

                        }
                        else
                        {
                            lblMsg.Visible = false;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg04", "alert('Items4 was already returned');", true);
                            return false;
                        }
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error Occured While Sending Mail";
                return false;
            }
        }

        /// <summary>
        /// Get ProductId
        /// </summary>
        public Int32 GetProductID(string ProductName, String SKU)
        {
            try
            {
                if (ViewState["StoreId"] != null)
                {
                    String Query = "select ProductID from tb_Product where storeid=" + ViewState["StoreId"].ToString() + " and Name='" + ProductName.Replace("'", "''") + "' and SKU='" + SKU + "'";
                    Int32 Newurl = Convert.ToInt32(CommonComponent.GetScalarCommonData(Query));
                    if (Newurl > 0)
                        return Newurl;
                    else
                        return 0;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Insert Return Merchandise Details
        /// </summary>
        /// <returns>Returns Value</returns>
        protected Boolean InsertReturnMerchandise()
        {
            try
            {
                int Pr1 = 0;
                int Pr2 = 0;
                int Pr3 = 0;
                int Pr4 = 0;
                int qu1 = 0;
                int qu2 = 0;
                int qu3 = 0;
                int qu4 = 0;

                if (!String.IsNullOrEmpty(txtItemReturned1.Text.ToString()) && !String.IsNullOrEmpty(txtItemCode1.Text.ToString()) && !String.IsNullOrEmpty(txtQuantity1.Text.ToString()) && chkItem1.Checked)
                {
                    Int32 PID = GetProductID(txtItemReturned1.Text.ToString().Trim(), txtItemCode1.Text.ToString().Trim());

                    if (PID == 0)
                    {
                        lblMsg.Visible = false;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('Please enter valid Items to be Returned and Item Code');", true);
                        return false;
                    }
                    else
                    {
                        Pr1 = PID;
                        if (Session["FirstName"] != null)
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;
                        else
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), 0, Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;

                        if (flag)
                        {
                            if (!string.IsNullOrEmpty(txtQuantity1.Text))
                                qu1 = CheckQuantityProduct(Convert.ToInt32(txtOrderNumber.Text.ToString()), Convert.ToInt32(Pr1));

                            if (qu1 < Convert.ToInt32(txtQuantity1.Text.ToString()))
                            {
                                lblMsg.Visible = false;
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg01", "alert('Please enter valid Quantity of Item1')", true);
                                return false;
                            }

                            String sqlquery = "insert into dbo.tb_ReturnItem " +
                            " (OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,OrderDate,ProductID,Quantity,ReturnReason,AdditionalInformation,Deleted,CreatedOn)" +
                            " values(" + Convert.ToInt32(ViewState["CustomerID"].ToString()) + "," + Convert.ToInt32(txtOrderNumber.Text.ToString()) + ",'" + txtCustomerName.Text.ToString() + "','" + txtEmail.Text.ToString() + "','" + Convert.ToDateTime(txtInvoiceDate.Text.ToString()) + "'," + Convert.ToInt32(PID) + " ," + Convert.ToInt32(txtQuantity1.Text) + ",'" + QuesType + "','" + Ques + "',0,'" + Convert.ToDateTime(System.DateTime.Now.Date.ToString()) + "')";
                            CommonComponent.ExecuteCommonData(sqlquery);
                        }
                        else
                        {
                            lblMsg.Visible = false;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg01", "alert('Items1 was already returned.');", true);
                            return false;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(txtItemReturned2.Text.ToString()) && !String.IsNullOrEmpty(txtItemCode2.Text.ToString()) && !String.IsNullOrEmpty(txtQuantity2.Text.ToString()) && chkItem2.Checked)
                {

                    Int32 PID = GetProductID(txtItemReturned2.Text.ToString().Trim(), txtItemCode2.Text.ToString().Trim());

                    if (PID == 0)
                    {
                        lblMsg.Visible = false;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg02", "alert('Please enter valid Items to be Returned and Item Code');", true);
                        return false;
                    }
                    else
                    {
                        Pr2 = PID;
                        if (Session["FirstName"] != null)
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;
                        else
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), 0, Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;

                        if (flag)
                        {

                            if (!string.IsNullOrEmpty(txtQuantity2.Text))
                                qu2 = CheckQuantityProduct(Convert.ToInt32(txtOrderNumber.Text.ToString()), Convert.ToInt32(Pr2));

                            if (qu2 < Convert.ToInt32(txtQuantity2.Text.ToString()))
                            {
                                lblMsg.Visible = false;
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg02", "alert('Please enter valid Quantity of Item2');", true);
                                return false;
                            }


                            String sqlquery = "insert into dbo.tb_ReturnItem " +
                            " (OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,OrderDate,ProductID,Quantity,ReturnReason,AdditionalInformation,Deleted,CreatedOn)" +
                            " values(" + Convert.ToInt32(ViewState["CustomerID"].ToString()) + "," + Convert.ToInt32(txtOrderNumber.Text.ToString()) + ",'" + txtCustomerName.Text.ToString() + "','" + txtEmail.Text.ToString() + "','" + Convert.ToDateTime(txtInvoiceDate.Text.ToString()) + "'," + Convert.ToInt32(PID) + " ," + Convert.ToInt32(txtQuantity2.Text) + ",'" + QuesType + "','" + Ques + "',0,'" + Convert.ToDateTime(System.DateTime.Now.Date.ToString()) + "')";
                            CommonComponent.ExecuteCommonData(sqlquery);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Items2 was already returned.');", true);
                            lblMsg.Visible = false;
                            return false;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(txtItemReturned3.Text.ToString()) && !String.IsNullOrEmpty(txtItemCode3.Text.ToString()) && !String.IsNullOrEmpty(txtQuantity3.Text.ToString()) && chkItem3.Checked)
                {
                    Int32 PID = GetProductID(txtItemReturned3.Text.ToString().Trim(), txtItemCode3.Text.ToString().Trim());
                    if (PID == 0)
                    {
                        lblMsg.Visible = false;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('Please enter valid Items to be Returned and Item Code');", true);
                        return false;
                    }
                    else
                    {
                        Pr3 = PID;

                        if (Session["FirstName"] != null)
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;
                        else
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), 0, Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;

                        if (flag)
                        {
                            if (!string.IsNullOrEmpty(txtQuantity3.Text))
                                qu3 = CheckQuantityProduct(Convert.ToInt32(txtOrderNumber.Text.ToString()), Convert.ToInt32(Pr3));


                            if (qu3 < Convert.ToInt32(txtQuantity3.Text.ToString()))
                            {
                                lblMsg.Visible = false;
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid Quantity of Item3');", true);
                                return false;
                            }

                            String sqlquery = "insert into dbo.tb_ReturnItem " +
                            " (OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,OrderDate,ProductID,Quantity,ReturnReason,AdditionalInformation,Deleted,CreatedOn)" +
                            " values(" + Convert.ToInt32(ViewState["CustomerID"].ToString()) + "," + Convert.ToInt32(txtOrderNumber.Text.ToString()) + ",'" + txtCustomerName.Text.ToString() + "','" + txtEmail.Text.ToString() + "','" + Convert.ToDateTime(txtInvoiceDate.Text.ToString()) + "'," + Convert.ToInt32(PID) + " ," + Convert.ToInt32(txtQuantity3.Text) + ",'" + QuesType + "','" + Ques + "',0,'" + Convert.ToDateTime(System.DateTime.Now.Date.ToString()) + "')";
                            CommonComponent.ExecuteCommonData(sqlquery); ;
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Items3 was already returned.');", true);
                            lblMsg.Visible = false;
                            return false;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(txtItemReturned4.Text.ToString()) && !String.IsNullOrEmpty(txtItemCode4.Text.ToString()) && !String.IsNullOrEmpty(txtQuantity4.Text.ToString()) && chkItem4.Checked)
                {
                    Int32 PID = GetProductID(txtItemReturned4.Text.ToString().Trim(), txtItemCode4.Text.ToString().Trim());

                    if (PID == 0)
                    {
                        lblMsg.Visible = false;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('Please enter valid Items to be Returned and Item Code');", true);
                        return false;
                    }
                    else
                    {
                        Pr4 = PID;
                        if (Session["FirstName"] != null)
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;
                        else
                            flag = (CheckForExistsProduct(Convert.ToInt32(PID), 0, Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;

                        if (flag)
                        {

                            if (!string.IsNullOrEmpty(txtQuantity4.Text))
                                qu4 = CheckQuantityProduct(Convert.ToInt32(txtOrderNumber.Text.ToString()), Convert.ToInt32(Pr4));


                            if (qu4 < Convert.ToInt32(txtQuantity4.Text.ToString()))
                            {
                                lblMsg.Visible = false;
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid Quantity of Item4');", true);
                                return false;
                            }


                            String sqlquery = "insert into dbo.tb_ReturnItem " +
                            " (OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,OrderDate,ProductID,Quantity,ReturnReason,AdditionalInformation,Deleted,CreatedOn)" +
                            " values(" + Convert.ToInt32(ViewState["CustomerID"].ToString()) + "," + Convert.ToInt32(txtOrderNumber.Text.ToString()) + ",'" + txtCustomerName.Text.ToString() + "','" + txtEmail.Text.ToString() + "','" + Convert.ToDateTime(txtInvoiceDate.Text.ToString()) + "'," + Convert.ToInt32(PID) + " ," + Convert.ToInt32(txtQuantity4.Text) + ",'" + QuesType + "','" + Ques + "',0,'" + Convert.ToDateTime(System.DateTime.Now.Date.ToString()) + "')";
                            CommonComponent.ExecuteCommonData(sqlquery);
                        }
                        else
                        {
                            lblMsg.Visible = false;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Items4 was already returned.');", true);
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error Occured While Sending Mail";
                return false;
            }
        }

        /// <summary>
        /// Insert RMA Details
        /// </summary>
        /// <returns>Returns Value</returns>
        private Boolean InsertRMADeatils()
        {
            //By Anil

            if (!String.IsNullOrEmpty(lblProductName.Text.ToString()) && !String.IsNullOrEmpty(lblProductSKU.Text.ToString()) && !String.IsNullOrEmpty(TxtProductQty.Text.ToString()))
            {
                Int32 PID = 0;
                Int32.TryParse(Request.QueryString["ProdID"].ToString(), out PID);
                if (PID == 0)
                {
                    lblMsg.Visible = false;
                    flagreturn = true;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid Items to be Returned and Item Code');", true);
                    return false;
                }
                else
                {

                    if (Session["FirstName"] != null)
                        flag = (CheckForExistsProduct(Convert.ToInt32(PID), Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;
                    else
                        flag = (CheckForExistsProduct(Convert.ToInt32(PID), 0, Convert.ToInt32(txtOrderNumber.Text.ToString())) == 0) ? true : false;


                    Int32 Qty = 0;
                    if (hdnProductId.Value != "")
                        //Qty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT (Sum(tb_OrderedShoppingCartItems.Quantity) - isnull(Sum(tb_ReturnItem.Quantity),0)) as Qty FROM dbo.tb_ReturnItem RIGHT OUTER JOIN tb_OrderedShoppingCartItems on RefProductID=ProductID   WHERE OrderedShoppingCartID in (SELECT ShoppingCardID FROM tb_Order  WHERE OrderNumber=" + SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["ono"])) + ") AND RefProductID =" + hdnProductId.Value + ""));
                        Qty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select (Select Sum(tb_OrderedShoppingCartItems.Quantity) from tb_OrderedShoppingCartItems WHERE (dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID IN (SELECT ShoppingCardID FROM  dbo.tb_Order  WHERE (OrderNumber = " + SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["ono"])) + ")))  AND (dbo.tb_OrderedShoppingCartItems.RefProductID = " + hdnProductId.Value + "))-(select IsNULL(Sum(tb_ReturnItem.Quantity),0) from dbo.tb_ReturnItem where OrderedNumber=" + SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["ono"])) + " and ProductID=" + hdnProductId.Value + ")"));

                    if (!flag)
                    {
                        if (!string.IsNullOrEmpty(TxtProductQty.Text.ToString()) && Convert.ToInt32(TxtProductQty.Text) > 0)
                        {
                            if (Convert.ToInt32(TxtProductQty.Text) > Qty)
                            {
                                lblMsg.Visible = false;
                                flagreturn = true;
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Items was already returned.');", true);
                                return false;
                            }
                            else { flag = true; }
                        }
                    }

                    if (flag)
                    {
                        Int32 qu1 = 0;
                        //if (!string.IsNullOrEmpty(txtQuantity1.Text))
                        if (!string.IsNullOrEmpty(TxtProductQty.Text))
                            qu1 = CheckQuantityProduct(Convert.ToInt32(txtOrderNumber.Text.ToString()), Convert.ToInt32(PID));

                        if (qu1 < Convert.ToInt32(TxtProductQty.Text.ToString()))
                        {
                            lblMsg.Visible = false;
                            flagreturn = true;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid Quantity.');", true);
                            return false;
                        }
                        Solution.Data.RMADAC objReturnItem = new Data.RMADAC();
                        objReturnItem.OrderedCustomerID = Convert.ToInt32(ViewState["CustomerID"].ToString());
                        objReturnItem.OrderedNumber = Convert.ToInt32(txtOrderNumber.Text.ToString());
                        objReturnItem.CustomerName = txtCustomerName.Text.ToString();
                        objReturnItem.CustomerEMail = txtEmail.Text.ToString();
                        objReturnItem.OrderDate = Convert.ToDateTime(txtInvoiceDate.Text.ToString());
                        objReturnItem.ProductID = Convert.ToInt32(PID);
                        objReturnItem.Quantity = Convert.ToInt32(TxtProductQty.Text);
                        objReturnItem.ReturnReason = QuesType;
                        objReturnItem.AdditionalInformation = Ques;
                        objReturnItem.Deleted = false;
                        objReturnItem.CreatedOn = DateTime.Now;

                        Decimal returnfee = 0;
                        try
                        {
                            if (rbtReason4.Checked)
                                Decimal.TryParse(hdReturnFee.Value, out returnfee);
                        }
                        catch { }
                        objReturnItem.ReturnFee = returnfee.ToString();

                        String ReturnType = "";
                        if (rbtReason4.Checked)
                            ReturnType = "RR";
                        else if (rbtReason3.Checked)
                            ReturnType = "RW";
                        else if (rbtReason1.Checked || rbtReason2.Checked)
                            ReturnType = "RP";

                        objReturnItem.ReturnType = ReturnType;
                        ReturnID = Convert.ToInt32(objReturnItem.AddReturnItem());
                        return true;
                    }
                    else
                    {
                        lblMsg.Visible = false;
                        flagreturn = true;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Items was already returned.');", true);
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check For Exists Product
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="OrderNo">Int32 OrderNo</param>
        /// <returns>Returns Total Count</returns>
        public Int32 CheckForExistsProduct(Int32 ProductID, Int32 CustomerID, Int32 OrderNo)
        {
            DataSet PackProductdetail = new DataSet();
            String Query = "";
            if (CustomerID == 0)
                Query = "select count(*) As Count from tb_ReturnItem where ProductID=" + ProductID + " and OrderedNumber=" + OrderNo + " and Deleted=0";
            else
                Query = "select count(*) As Count from tb_ReturnItem where ProductID=" + ProductID + " and OrderedCustomerID=" + CustomerID + " and OrderedNumber=" + OrderNo + " and Deleted=0";

            PackProductdetail = CommonComponent.GetCommonDataSet(Query);
            return (Convert.ToInt32(PackProductdetail.Tables[0].Rows[0]["Count"].ToString()));
        }

        /// <summary>
        /// Check Quantity Product
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns Int32</returns>
        public Int32 CheckQuantityProduct(int OrderNumber, int ProductID)
        {
            int qu = (Convert.ToInt32(CommonComponent.GetScalarCommonData(" select Count(Quantity) from dbo.tb_OrderedShoppingCartItems where OrderedShoppingCartID = " +
                   " (SELECT ShoppingCardID FROM tb_Order WHERE OrderNumber=" + OrderNumber + ") and RefProductID=" + ProductID + " ")));
            if (qu == 0)
                return 0;
            else
                return (Convert.ToInt32(CommonComponent.GetScalarCommonData(" select Quantity from dbo.tb_OrderedShoppingCartItems where OrderedShoppingCartID = " +
                   " (SELECT ShoppingCardID FROM tb_Order WHERE OrderNumber=" + OrderNumber + ") and RefProductID=" + ProductID + " ")));
        }

        /// <summary>
        /// btnsubmit_Click Save
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (chkreturnpolicy.Checked)
                {
                    if (this.txtCodeshow.Text != this.Session["CaptchaImageText"].ToString())
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CaptchaAlert", "<script type='text/javascript'>alert('Incorrect Verification Code, try again.');</script>");
                        this.txtCodeshow.Text = "";
                        txtCodeshow.Focus();
                        return;
                    }

                    Int32 Qty = 0;
                    if (hdnProductId.Value != "")
                        Qty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select (Select Sum(tb_OrderedShoppingCartItems.Quantity) from tb_OrderedShoppingCartItems WHERE (dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID IN (SELECT ShoppingCardID FROM  dbo.tb_Order  WHERE (OrderNumber = " + SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["ono"])) + ")))  AND (dbo.tb_OrderedShoppingCartItems.RefProductID = " + hdnProductId.Value + "))-(select IsNULL(Sum(tb_ReturnItem.Quantity),0) from dbo.tb_ReturnItem where OrderedNumber=" + SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["ono"])) + " and ProductID=" + hdnProductId.Value + ")"));

                    if (!string.IsNullOrEmpty(TxtProductQty.Text.ToString()) && Convert.ToInt32(TxtProductQty.Text) > 0)
                    {
                        if (Convert.ToInt32(TxtProductQty.Text) > Qty)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CaptchaAlert", "<script type='text/javascript'>alert('Enter Valid Quantity');</script>");
                            TxtProductQty.Focus();
                            return;
                        }

                        string MailAddress = Convert.ToString(CommonComponent.GetScalarCommonData(" select isnull(configvalue,'') from tb_appconfig where configName='ContactMail_ToAddress' and storeid in (select storeid from tb_order where ordernumber=" + Server.UrlDecode(SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["ono"]))) + " ) "));
                        if (MailAddress != string.Empty)
                        {
                            SendMail(MailAddress);
                            if (flagreturn == true || ReturnID == 0)
                                return;
                            else
                                try
                                {
                                    Response.Redirect("ReturnPackageSlip.aspx?Page=ReturnMerchandise&ID=" + ReturnID + "");
                                }
                                catch { }
                        }
                        else
                            lblMsg.Text = "Sorry, We are unable to find customerservice representative Email Address. Please try after some time.";
                    }
                    else { lblMsg.Text = "Enter Valid Quantity "; }

                }
                else
                {
                    lblMsg.Text = "Please read and understand the return policy ";
                    lblMsg.Visible = true;
                }
            }
            catch { }
        }
    }
}