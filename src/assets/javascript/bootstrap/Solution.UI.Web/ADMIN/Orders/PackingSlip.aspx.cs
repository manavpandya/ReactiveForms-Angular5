using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Data;
using System.Net;
using System.IO;
using System.Net.Mail;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class PackingSlip : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Request.QueryString["ONo"] != null)
            {
                Int32 StoreID = 0;
                int OrderNumber = 0;
                Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + OrderNumber)), out StoreID);
                AppConfig.StoreID = StoreID;
                ImgStoreLogo.Src = "/Images/Store_" + StoreID.ToString() + ".png";
            }

            if (!IsPostBack)
            {
                BindInvoiceSignature();
                if (Request.QueryString["ONo"] != null)
                {
                    BindRefNumberDetails();
                    int OrderNumber = 0;
                    bool chkOrder = Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                    if (chkOrder)
                    {
                        ltrorderNumber.Text = OrderNumber.ToString();
                        GetOrderDetails(OrderNumber);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the Folder for Specified path
        /// </summary>
        /// <param name="FPath">The F path.</param>
        private void CreateFolder(String FPath)
        {
            if (!Directory.Exists(Server.MapPath(FPath.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(FPath.ToString()));
            }
        }

        /// <summary>
        /// Bind Order Details For Receipt
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        private void GetOrderDetails(Int32 OrderNumber)
        {
            OrderComponent objOrder = new OrderComponent();
            DataSet objDsorder = new DataSet();
            objDsorder = objOrder.GetOrderDetailsByOrderID(OrderNumber);
            if (objDsorder != null && objDsorder.Tables.Count > 0 && objDsorder.Tables[0].Rows.Count > 0)
            {
                ltrOrderdate.Text = objDsorder.Tables[0].Rows[0]["OrderDate"].ToString();
                ltrShippingMethod.Text = objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString();

                ltrName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString()) + "<br/>";


                #region Generate Barcode From OrderNo. By Girish
                String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                CreateFolder(FPath.ToString());
                if (!System.IO.File.Exists(Server.MapPath(FPath + "/ONo-" + OrderNumber.ToString() + ".png")))
                {
                    DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                    bCodeControl.BarCode = OrderNumber.ToString();
                    bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                    bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                    bCodeControl.BarCodeHeight = 80;
                    bCodeControl.ShowHeader = false;
                    bCodeControl.ShowFooter = true;
                    bCodeControl.FooterText = "ONo-" + OrderNumber.ToString();
                    bCodeControl.Size = new System.Drawing.Size(250, 150);
                    bCodeControl.SaveImage(Server.MapPath(FPath + "/ONo-" + OrderNumber + ".png"));
                }
                imgOrderBarcode.Src = FPath + "/ONo-" + OrderNumber + ".png";
                #endregion

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                    ltrShippingAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br/>";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                    ltrShippingAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString() + "<br/>";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                    ltrShippingAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString() + "<br/>";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                    ltrShippingAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString() + "<br/>";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString()))
                    ltrShippingAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString() + ", ";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingState"].ToString()))
                    ltrShippingAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingState"].ToString() + " ";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                    ltrShippingAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br/>";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                    ltrShippingAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()) + "<br/>";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    ltrShippingAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString();

                BindCart(Convert.ToInt32(objDsorder.Tables[0].Rows[0]["ShoppingCardID"].ToString()), objDsorder);
            }
        }

        /// <summary>
        /// Bind Order Cart By Order Number
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        private void BindCart(Int32 OrderNumber, DataSet dsOrderdata)
        {
            OrderComponent objOrder = new OrderComponent();
            DataSet dsCart = new DataSet();
            
            //dsCart = objOrder.GetShippedItemsforOrderwithoutPOpackage(Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString())));
            dsCart = objOrder.GetInvoiceProductList(OrderNumber);
            ltrCart.Text = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"datatable\" style=\"border-collapse: collapse;\">";
            ltrCart.Text += "<tr style=\"line-height: 50px; background-color: rgb(242,242,242);\">";
            ltrCart.Text += "<th valign=\"middle\" align=\"left\" style=\"width: 55%\">";
            ltrCart.Text += "<b>Product Details</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 10%; text-align: center;\">";
            ltrCart.Text += "<b>SKU</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"text-align: center;\">";
            ltrCart.Text += "<b>Quantity</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "<th valign=\"middle\" align=\"left\" style=\"text-align: left;\">";
            ltrCart.Text += "<b>Ware House</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "</tr>";
            if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsCart.Tables[0].Rows.Count; i++)
                {
                    bool titem = false;
                    if (Request.QueryString["Pid"] != null)
                    {
                        string strpids = "~" + Request.QueryString["Pid"].ToString();
                        if (strpids.IndexOf("~" + dsCart.Tables[0].Rows[i]["RefProductID"].ToString() + "~") > -1)
                        {
                            titem = true;
                        }
                    }
                    else
                    {
                        titem = true;
                    }
                    //                    if (Request.QueryString["WareHouseId"] != null)
                    //                    {
                    //                        dsPreferred = CommonComponent.GetCommonDataSet(@"SELECT DISTINCT isnull(tb_WareHouseProductInventory.WareHouseID,1) as WareHouseID
                    //FROM         dbo.tb_OrderedShoppingCartItems INNER JOIN
                    //                      dbo.tb_Order ON dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID = dbo.tb_Order.ShoppingCardID LEFT OUTER JOIN
                    //                      dbo.tb_WareHouseProductInventory ON dbo.tb_OrderedShoppingCartItems.RefProductID = dbo.tb_WareHouseProductInventory.ProductID
                    //                      WHERE tb_Order.orderNumber=" + OrderNumber.ToString() + @" AND dbo.tb_OrderedShoppingCartItems.RefProductID=" + dsCart.Tables[0].Rows[i]["RefProductID"].ToString() + @"  AND isnull(tb_WareHouseProductInventory.Inventory,0) > 0 order By tb_WareHouseProductInventory.WareHouseID");
                    //                        if (dsPreferred != null && dsPreferred.Tables.Count > 0 && dsPreferred.Tables[0].Rows.Count > 0)
                    //                        {
                    //                            for (int j = 0; j < dsPreferred.Tables[0].Rows.Count; j++)
                    //                            {
                    //                                if (dsPreferred.Tables[0].Rows[j]["WareHouseID"].ToString() == Request.QueryString["WareHouseId"].ToString())
                    //                                {
                    //                                    titem = true;
                    //                                }
                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            titem = true;
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        titem = true;
                    //                    }
                    if (titem == true)
                    {
                        ltrCart.Text += "<tr>";
                        ltrCart.Text += "<td valign=\"top\" align=\"left\">";
                        ltrCart.Text += dsCart.Tables[0].Rows[i]["ProductName"].ToString() + "<br/>";

                        string[] variantName = dsCart.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] variantValue = dsCart.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string sku = "";
                        for (int j = 0; j < variantValue.Length; j++)
                        {
                            if (variantName.Length > j)
                            {
                                ltrCart.Text += variantName[j].ToString().Replace("Estimated Delivery", "Estimated Ship Date") + " : " + variantValue[j].ToString() + "<br />";
                                SQLAccess objSql = new SQLAccess();
                                DataSet dsoption = new DataSet();
                                dsoption = objSql.GetDs("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + dsCart.Tables[0].Rows[i]["RefproductId"] + " AND VariantValue='" + variantValue[j].ToString().Replace("'", "''") + "'");
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
                                            //sku += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
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
                                                    bCodeControl.Dispose();
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
                        }

                        if (!string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["RefProductID"].ToString()))
                        {
                            string StrQuery = " SElect ProductAssemblyID,RefProductID, tb_ProductAssembly.ProductID,tb_product.name,tb_product.Sku,ISNULL(Quantity,0) as Quantity from tb_ProductAssembly " +
                                              " inner join tb_product on tb_ProductAssembly.ProductID=tb_product.ProductID " +
                                              " where RefProductID= " + dsCart.Tables[0].Rows[i]["RefProductID"].ToString() + " and ISNULL(tb_product.Active,1)=1 and ISNULL(Deleted,0)=0";
                            DataSet dsAssamble = new DataSet();
                            dsAssamble = CommonComponent.GetCommonDataSet(StrQuery.ToString());
                            if (dsAssamble != null && dsAssamble.Tables.Count > 0 && dsAssamble.Tables[0].Rows.Count > 0)
                            {
                                ltrCart.Text += "<div style=\"padding-left: 20px;\">";
                                for (int k = 0; k < dsAssamble.Tables[0].Rows.Count; k++)
                                {
                                    ltrCart.Text += dsAssamble.Tables[0].Rows[k]["Name"].ToString() + " - Qty (" + dsAssamble.Tables[0].Rows[k]["Quantity"].ToString() + ")<br />";
                                }
                                ltrCart.Text += "<div/>";
                            }
                        }

                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td style=\"text-align: center;\">";
                        //if (sku != "")
                        //{
                        //    ltrCart.Text += sku.ToString();
                        //}
                        //else
                        //{
                            ltrCart.Text += dsCart.Tables[0].Rows[i]["SKU"].ToString();
                        //}
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td style=\"text-align: center;\">";
                        ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                        ltrCart.Text += "</td>";

                       
                        ltrCart.Text += "<td style=\"text-align: left;\">";
                        if (Request.QueryString["WareHouseId"] != null)
                        {
                            ltrCart.Text += Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(Name,'') FROM tb_WareHouse WHERE WareHouseID=" + Request.QueryString["WareHouseId"].ToString() + ""));
                        }
                        ltrCart.Text += "</td>";

                        ltrCart.Text += "</tr>";
                    }
                }
            }
            else
            {
                ltrCart.Text += "<tr>";
                ltrCart.Text += "<td valign=\"top\" align=\"center\" colspan=\"4\">No Record(s) Found.</td></tr>";
            }
            ltrCart.Text += "</table>";
        }

        /// <summary>
        /// Binds the Invoice Signature
        /// </summary>
        private void BindInvoiceSignature()
        {
            DataSet dsTopic = new DataSet();
            dsTopic = TopicComponent.GetTopicList("InvoiceSignature", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["Description"].ToString()))
                {
                    ltInvoiceSignature.Text = dsTopic.Tables[0].Rows[0]["Description"].ToString();
                }
                else
                {
                    ltInvoiceSignature.Text = "";
                }
                dsTopic.Dispose();
            }
        }

        /// <summary>
        ///  Invoice Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnInvoice_Click(object sender, EventArgs e)
        {
            int OrderNumber = 0;
            bool chkOrder = Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
            if (chkOrder)
            {
                SendMail(OrderNumber);
            }
        }


        /// <summary>
        /// Order Receipt Send To Customer  by Mail
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNUmber</param>
        public void SendMail(Int32 OrderNumber)
        {
            string Body = "";
            string url = Request.Url.ToString();
            WebRequest NewWebReq = WebRequest.Create(url);
            WebResponse newWebRes = NewWebReq.GetResponse();
            string format = newWebRes.ContentType;
            Stream ftprespstrm = newWebRes.GetResponseStream();
            StreamReader reader;
            reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
            Body = reader.ReadToEnd().ToString();
            Body = Body.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"display:none;\"");

            AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

            try
            {
                CommonOperations.SendMail(hdmemail.Value.ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.parent.Tabdisplay(" + hdntabid.Value + ");jAlert('Mail has been sent successfully.','Message'); window.parent.document.getElementById('prepage').style.display = 'none'; ", true);
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.parent.Tabdisplay(" + hdntabid.Value + ");jAlert('Mail Sending Problem.','Error');window.parent.document.getElementById('prepage').style.display = 'none';", true);
            }
        }



        #region Remove ViewState From page


        /// <summary>
        /// Loads any saved view-state information to the <see cref="T:System.Web.UI.Page" /> object.
        /// </summary>
        /// <returns>The saved view state.</returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            if (Session[Session.SessionID] != null)
                return (new LosFormatter().Deserialize((string)Session[Session.SessionID]));
            return null;
        }

        /// <summary>
        /// Saves any view-state and control-state information for the page.
        /// </summary>
        /// <param name="state">The state.</param>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            LosFormatter los = new LosFormatter();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            los.Serialize(sw, state);
            string vs = sw.ToString();
            Session[Session.SessionID] = vs;
        }

        #endregion

        /// <summary>
        /// Bind Ref Number Details
        /// </summary>
        public void BindRefNumberDetails()
        {
            OrderComponent objordercomp = new OrderComponent();
            DataSet dsorder = objordercomp.GetRefNumberByOrderNumber(Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString())));
            if (dsorder != null && dsorder.Tables.Count > 0)
            {
                if (dsorder.Tables[0].Rows.Count > 0 && dsorder.Tables[1].Rows.Count > 0)
                {
                    trRefOrderNo.Visible = true;
                    ltrstore.Text = dsorder.Tables[0].Rows[0]["StoreName"].ToString() + " Order #";
                    if (!string.IsNullOrEmpty(dsorder.Tables[1].Rows[0]["RefOrderID"].ToString()) && dsorder.Tables[1].Rows[0]["RefOrderID"].ToString() != "0")
                    {
                        ltrRef.Text = dsorder.Tables[1].Rows[0]["RefOrderID"].ToString();
                    }
                    else
                    {
                        trRefOrderNo.Visible = false;
                    }
                }
                else
                {
                    trRefOrderNo.Visible = false;
                }
            }
        }

    }
}