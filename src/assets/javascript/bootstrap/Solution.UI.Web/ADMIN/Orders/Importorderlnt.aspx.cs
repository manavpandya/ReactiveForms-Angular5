using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using Solution.Data;
using System.Text;
using System.Net;
using System.Xml;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using LumenWorks.Framework.IO.Csv;
using System.Data.OleDb;
using System.Globalization;
using System.Collections.Specialized;
namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class Importorderlnt : System.Web.UI.Page
    {
        int totalrecords = 0;
        Int32 AlreadyExsits = 0;
        Int32 TotalImportOtrders = 0;
        private string StrFileName
        {
            get
            {
                if (ViewState["FileName"] == null)
                {
                    return "";
                }
                else
                {
                    return (ViewState["FileName"].ToString());
                }
            }
            set
            {
                ViewState["FileName"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
    //            var loginData = new NameValueCollection
    //{
    //  { "username", "hlodhi@theeff.com" },
    //  { "password", "EFF2301lnt" }
    //};
    //            CookieContainer container;

    //            var request = (HttpWebRequest)WebRequest.Create("https://client.torreycommerce.com/exclusivefabrics/User/Login");

    //            request.Method = "POST";
    //            request.ContentType = "application/x-www-form-urlencoded";
    //            var buffer = Encoding.ASCII.GetBytes(loginData.ToString());
    //            request.ContentLength = buffer.Length;
    //            var requestStream = request.GetRequestStream();
    //            requestStream.Write(buffer, 0, buffer.Length);
    //            requestStream.Close();

    //            container = request.CookieContainer = new CookieContainer();

    //            var response = request.GetResponse();
    //            response.Close();
    //            //CookieContainer = container;

    //            var myUri = new Uri("https://client.torreycommerce.com/exclusivefabrics/Admin/Edit/Shop_Order/default/default/?id=14109&page=&sort=&direction=&DisableExport=0&DisableNew=0&search=&var=&class=Shop_Order&table=default&method=default&href=&par=");
    //            // Create a 'HttpWebRequest' object for the specified url. 
    //            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
    //            // Set the user agent as if we were a web browser
    //            //myHttpWebRequest.AllowAutoRedirect = true;
    //            myHttpWebRequest.UseDefaultCredentials = true;
    //           // myHttpWebRequest.PreAuthenticate = true;
    //            myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 
    //           // myHttpWebRequest.Credentials = new NetworkCredential("hlodhi@theeff.com", "EFF2301lnt");
    //            var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
    //            var stream = myHttpWebResponse.GetResponseStream();
    //            var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
    //            string html = reader.ReadToEnd();
            }
            btnImport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;padding-bottom: 7px;");
        }

        /// <summary>
        /// Display CSV File data in Grid
        /// </summary>
        /// <param name="FileName">FileName</param>
        /// <returns>DataTable</returns>
        private DataTable LoadCSV(string FileName)
        {




            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportWayfairCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string[] FieldNames = csv.GetFieldHeaders();
                DataTable dtCSV = new DataTable();
                DataColumn columnID = new DataColumn();
                columnID.Caption = "Number";
                columnID.ColumnName = "Number";
                columnID.AllowDBNull = false;
                columnID.AutoIncrement = true;
                columnID.AutoIncrementSeed = 1;
                columnID.AutoIncrementStep = 1;
                // dtCSV.Columns.Add(columnID);
                foreach (string FieldName in FieldNames)
                    dtCSV.Columns.Add(FieldName.Replace(":", ""));
                while (csv.ReadNextRecord())
                {
                    DataRow dr = dtCSV.NewRow();
                    for (int i = 0; i < FieldCount; i++)
                    {
                        string FieldName = FieldNames[i].Replace(":", "");
                        if (!dr.Table.Columns.Contains(FieldName))
                        { continue; }

                        dr[FieldName] = csv[i];
                    }
                    dtCSV.Rows.Add(dr);
                }
                dtCSV.AcceptChanges();
                return dtCSV;
            }
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="CustomerDoc">String CustomerDoc</param>
        private void DeleteDocument(string StrFileName)
        {
            try
            {
                string docPath = Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportWayfairCSV/") + StrFileName;
                if (File.Exists(Server.MapPath(docPath)))
                {
                    File.Delete(Server.MapPath(docPath));
                }

            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Bind Check box based on columns specified in CSV File
        /// </summary>
        /// <param name="FileName">FileName</param>
        private void FillMapping(string FileName)
        {

            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportWayfairCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string FieldStrike = ",";

                if (FieldCount > 0)
                {
                    string[] FieldNames = csv.GetFieldHeaders();

                    foreach (string FieldName in FieldNames)
                    {
                        string tempFieldName = FieldName.ToLower();
                        if (tempFieldName == "order_id" || tempFieldName == "order_date" || tempFieldName == "order_status" || tempFieldName == "order_date_shipped" || tempFieldName == "order_edit_user_ids" || tempFieldName == "order_status" || tempFieldName == "order_ship_address1" || tempFieldName == "order_ship_address2" || tempFieldName == "order_ship_city" || tempFieldName == "order_ship_state" || tempFieldName == "order_ship_zip" || tempFieldName == "order_ship_firstname" || tempFieldName == "order_ship_lastname" || tempFieldName == "order_ship_country" || tempFieldName == "order_ship_phone" || tempFieldName == "order_tcn_commission" || tempFieldName == "order_ship_email" || tempFieldName == "order_bill_firstname" || tempFieldName == "order_bill_lastname" || tempFieldName == "order_bill_address1" || tempFieldName == "order_bill_address2" || tempFieldName == "order_bill_city" || tempFieldName == "order_bill_state" || tempFieldName == "order_bill_zip" || tempFieldName == "order_bill_country" || tempFieldName == "order_bill_phone" || tempFieldName == "order_bill_email" || tempFieldName == "order_tax_charge" || tempFieldName == "order_shipping_charge" || tempFieldName == "order_remote_addr" || tempFieldName == "order_item_count" || tempFieldName == "order_shipping" || tempFieldName == "order_shipping")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",order_id,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",po date,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",must ship by,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",tracking #,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",order status,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",item #,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",item name,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",quantity,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",wholesale price,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",carrier name,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",ship to name,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",ship to address,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",ship to address 2,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",ship to city,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",ship to state,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",ship to zip,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",ship to phone,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",ship speed,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",po date & time,") > -1)
                        {

                        }
                        else
                        {
                            lblMessage.Text = "File Does not contain all columns";
                            lblMessage.Style.Add("color", "#FF0000");
                            lblMessage.Style.Add("font-weight", "normal");
                        }
                    }
                    if (FieldStrike.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    {


                        BindData();

                    }
                    else
                    {
                        lblMessage.Text = "Please Specify P.O. Number:,PO Date,Must Ship By,Tracking #,Order Status,Item #,Item Name,Quantity,Wholesale Price,Carrier Name,Ship To Name,Ship to Address,Ship To Address 2,Ship To City,Ship To State,Ship To Zip,Ship To Phone,Ship Speed,PO Date & Time in file.";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                    }

                }
                else
                {
                    lblMessage.Text = "Please Specify P.O. Number:,PO Date,Must Ship By,Tracking #,Order Status,Item #,Item Name,Quantity,Wholesale Price,Carrier Name,Ship To Name,Ship to Address,Ship To Address 2,Ship To City,Ship To State,Ship To Zip,Ship To Phone,Ship Speed,PO Date & Time in file.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                }
                csv.Dispose();
            }
        }

        /// <summary>
        /// Bind Data with Gridview
        /// </summary>
        private void BindData()
        {
            DataTable dtCSV = LoadCSV(StrFileName);
            if (dtCSV.Rows.Count > 0)
            {

            }
            else
                lblMessage.Text = "No data exists in file.";
            lblMessage.Style.Add("color", "#FF0000");
            lblMessage.Style.Add("font-weight", "normal");
        }
        /// <summary>
        /// Import button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadCSV.HasFile && Path.GetExtension(uploadCSV.FileName).ToLower() == ".csv")
                {
                    lblMessage.Text = "";
                    if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportWayfairCSV/")))
                        Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportWayfairCSV/"));
                    StrFileName = uploadCSV.FileName;
                    DeleteDocument(StrFileName);
                    uploadCSV.SaveAs(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportWayfairCSV/") + StrFileName);
                    //StrFileName = Server.MapPath(AppLogic.AppConfigs("CategoryImportPath") + "CategoryCSV/ImportCSV/") + StrFileName;
                    FillMapping(uploadCSV.FileName);
                }
                else
                {
                    //lblMessage.Text = "Please upload appropriate file.";
                    //lblMessage.Style.Add("color", "#FF0000");
                    //lblMessage.Style.Add("font-weight", "normal");

                }
                if (!string.IsNullOrEmpty(StrFileName))
                {

                    DataTable dtCSV = LoadCSV(StrFileName);
                    DataView dv = dtCSV.DefaultView;
                    dv.Sort = "P.O. Number ASC";

                    DataSet dscsv = new DataSet();
                    dscsv.Tables.Add(dv.ToTable());

                    if (ImportwaifairOrderData(dscsv, 10) && lblMessage.Text == "")
                    {
                        // contVerify.Visible = false;
                        lblMessage.Text = "Orders Imported Successfully";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                        lblMessage.Visible = true;
                        lblTotalRecords.Text = "Total Records In File :" + totalrecords;
                        lblTotalRecords.Visible = true;
                        lblTotalRecords.Style.Add("color", "#FF0000");
                        lblTotalRecords.Style.Add("font-weight", "normal");

                        lblAlreadyExist.Text = "Alredy Exist Records :" + AlreadyExsits;
                        lblAlreadyExist.Visible = true;
                        lblAlreadyExist.Style.Add("color", "#FF0000");
                        lblAlreadyExist.Style.Add("font-weight", "normal");

                        lblImport.Text = "Total Import Records :" + TotalImportOtrders;
                        lblImport.Visible = true;
                        lblImport.Style.Add("color", "#FF0000");
                        lblImport.Style.Add("font-weight", "normal");
                        return;

                    }


                }
                else
                {
                    lblMessage.Text += "Sorry file not found. Please retry uploading.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                    lblMessage.Visible = true;
                }
            }
            catch { }
        }

        private bool CheckLntOrder(String iAmazonOrderNumber, Int32 AmazonStoreid)
        {
            string sql = "select 'True' from tb_Order where isnull(RefOrderID,'')='" + iAmazonOrderNumber + "' and Isnull(deleted,0)=0 and storeid=" + AmazonStoreid;
            return Convert.ToBoolean(CommonComponent.GetScalarCommonData(sql));
        }


        private void FillShipment(int Productid, String carrier, String Trackingnumber, string orderNumber, DateTime shipdate)
        {

            Int32 ON = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT OrderNumber FROM tb_order WHERE isnull(Reforderid,'') ='" + orderNumber + "'"));
            Int32 Shippedid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT count(*) FROM tb_OrderShippedItems WHERE OrderNumber =" + ON + " AND RefProductID=" + Productid.ToString() + ""));
            if (Shippedid == 0)
            {
                CommonComponent.ExecuteCommonData("INSERT INTO tb_OrderShippedItems(TrackingNumber,ShippedVia,Shipped,ShippedOn,OrderNumber,RefProductID) VALUES ('" + Trackingnumber + "','" + carrier + "',1,'" + shipdate + "'," + ON + "," + Productid + ")");
                CommonComponent.ExecuteCommonData("UPDATE tb_Order SET IsNew=0, isfullFillment=1,Orderstatus='Shipped',ShippingTrackingNumber = '" + Trackingnumber + "',ShippedVIA='" + carrier + "', ShippedOn='" + shipdate + "' WHERE OrderNumber=" + ON + "");

            }
        }
        private bool ImportwaifairOrderData(DataSet ds, Int32 StoreID)
        {


            #region Variables
            String iAmazonOrderNumber = "0";
            string strItemID = string.Empty;
            DateTime dtPurchase = DateTime.Now;
            string strCustomerEmail = string.Empty;
            string strCustomerName = string.Empty;
            string strCustomerPhone = string.Empty;
            string strSKU = string.Empty;
            string strProductName = string.Empty;

            string strShippingMethod = string.Empty;
            string strShippingName = string.Empty;
            string strShippingAddress1 = string.Empty;
            string strShippingAddress2 = string.Empty;
            string strShippingAddress3 = string.Empty;
            string strShippingCity = string.Empty;
            string strShippingState = string.Empty;
            string strShippingZip = string.Empty;
            string strShippingCountry = string.Empty;
            string strShippingPhone = string.Empty;

            string strOrderNotes = string.Empty;

            #endregion

            try
            {

                OrderComponent objOrder = new OrderComponent();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {

                            iAmazonOrderNumber = ds.Tables[0].Rows[i][2].ToString(); //P.O. Number:
                            bool bOrderExists = CheckLntOrder(iAmazonOrderNumber, StoreID);
                            strShippingMethod = ds.Tables[0].Rows[i]["Ship Speed"].ToString();
                            int orderSID = 0;
                            Int32 CustID = 0;
                            if (!bOrderExists)
                            {

                                //DataRow[] drAddress = ds.Tables[0].Select("order_id=" + ds.Tables["Order"].Rows[i]["order_id"].ToString() + "");
                                //if (drAddress != null && drAddress.Length > 0)
                                //{
                                strShippingName = Convert.ToString(ds.Tables[0].Rows[i]["Ship To Name"].ToString());
                                strShippingAddress1 = Convert.ToString(ds.Tables[0].Rows[i]["Ship to Address"].ToString());

                                strShippingAddress2 = Convert.ToString(ds.Tables[0].Rows[i]["Ship To Address 2"].ToString());
                                strShippingAddress3 = "";
                                strShippingCity = Convert.ToString(ds.Tables[0].Rows[i]["Ship To City"].ToString());
                                strShippingZip = Convert.ToString(ds.Tables[0].Rows[i]["Ship To Zip"].ToString());
                                strShippingCountry = "";
                                strShippingPhone = Convert.ToString(ds.Tables[0].Rows[i]["Ship To Phone"].ToString());
                                strShippingState = Convert.ToString(CommonComponent.GetScalarCommonData("select Name from tb_State where Abbreviation='" + Convert.ToString(ds.Tables[0].Rows[i]["Ship To State"].ToString()) + "'"));
                                strShippingCountry = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Name,'') from tb_Country where countryid in (select countryid from tb_State where Abbreviation='" + Convert.ToString(ds.Tables[0].Rows[i]["Ship To State"].ToString()) + "')"));

                                //}


                                CustID = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO tb_Customer(StoreID,Email,FirstName,LastName,Active,Deleted,IsRegistered) VALUES (" + StoreID + ",'','" + strShippingName.Replace("'", "''") + "','',0,1,0); SELECT SCOPE_IDENTITY();"));

                                orderSID = Convert.ToInt32(CommonComponent.GetScalarCommonData("insert into tb_OrderedShoppingCart (CustomerID,CreatedOn) values(" + CustID + ",getdate()); SELECT SCOPE_IDENTITY();"));

                                DataRow[] drProduct = ds.Tables[0].Select("[" + ds.Tables[0].Columns[2].ColumnName.ToString().Replace(":", "") + "]='" + iAmazonOrderNumber + "'");
                                Decimal OrderTotal = 0;
                                Decimal OrderSubtotal = 0;
                                Decimal Shippingtotal = 0;
                                if (drProduct != null && drProduct.Length > 0)
                                {
                                    foreach (DataRow dr in drProduct)
                                    {

                                        Int32 ProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Productid from tb_product where (sku='" + dr["Item #"].ToString().Replace("'", "''").Replace("=", "").Replace("\"", "") + "') and storeid=" + StoreID));
                                        if (ProductID <= 0)
                                        {
                                            ProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO tb_product(Name,SKU,Description,MainCategory,Weight,Inventory,Price,SalePrice,StoreID,overstockproductid,deleted,Active,OptionSku) VALUES ('" + dr["Item Name"].ToString().Replace("'", "''") + "','" + dr["Item #"].ToString().Replace("'", "''").Replace("=", "").Replace("\"", "") + "','','',1,999," + dr["Wholesale Price"].ToString() + "," + dr["Wholesale Price"].ToString() + "," + StoreID + ",'',0,1,''); SELECT SCOPE_IDENTITY();"));
                                        }

                                        CommonComponent.ExecuteCommonData("insert into tb_OrderedShoppingCartItems (OrderedShoppingCartID,RefProductID,Quantity,Price,VariantNames,VariantValues,ProductName,SKU,OrderItemID) " +
                                           " values(" + orderSID + ",'" + ProductID + "','" + dr["Quantity"].ToString() + "'," + dr["Wholesale Price"].ToString() + ",'','','" + dr["Item Name"].ToString().Replace("'", "''") + "','" + dr["Item #"].ToString().Replace("'", "''").Replace("=", "").Replace("\"", "") + "','')");
                                        OrderSubtotal = OrderSubtotal + Convert.ToDecimal(Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Wholesale Price"].ToString()));
                                        // Shippingtotal = Shippingtotal + Convert.ToDecimal(dr["ShipCost"].ToString());
                                    }

                                }

                                OrderTotal = OrderSubtotal + Shippingtotal;
                                Decimal OrderTax = 0;
                                Int32 OrderNumber = Convert.ToInt32(CommonComponent.GetScalarCommonData(@"insert into tb_Order (GiftCertificateDiscountAmount,orderStatus,StoreID,CustomerID,ShoppingCardID,LastName,FirstName,Email,BillingLastName,BillingFirstName,BillingCompany,BillingAddress1,BillingAddress2,BillingSuite,BillingCity,BillingState,BillingZip,BillingCountry,BillingPhone,ShippingFirstName,ShippingLastName,ShippingCompany,ShippingAddress1,ShippingAddress2,ShippingState,ShippingCity,ShippingZip,ShippingCountry,ShippingPhone,OrderTotal,OrderSubtotal,OrderShippingCosts,OrderTax,Deleted,RefOrderID,PaymentMethod,OrderDate,Isnew,LevelDiscountAmount,CouponDiscountAmount,TransactionStatus,CardType,CardVarificationCode,CardNumber,CardName,CardExpirationMonth,CardExpirationYear,Last4,paymentgateway,CustomDiscount,ShippingMethod,ShippedOn,ShippedVIA,QuantityDiscountAmount)
 
 values(0,'New'," + StoreID + "," + CustID + "," + orderSID + ",'','" + strShippingName.ToString().Replace("'", "''") + "','','','" + strShippingName.ToString().Replace("'", "''") + "','','" + strShippingAddress1.ToString().Replace("'", "''") + "','" + strShippingAddress2.ToString().Replace("'", "''") + "','','" + strShippingCity.ToString().Replace("'", "''") + "','" + strShippingState + "','" + strShippingZip.ToString().Replace("'", "''") + "','" + strShippingCountry.Replace("'", "''") + "','" + strShippingPhone + "','" + strShippingName.ToString().Replace("'", "''") + "','','','" + strShippingAddress1.ToString().Replace("'", "''") + "','" + strShippingAddress2.ToString().Replace("'", "''") + "','" + strShippingState.Replace("'", "''") + "','" + strShippingCity.ToString().Replace("'", "''") + "','" + strShippingZip.ToString().Replace("'", "''") + "','" + strShippingCountry.Replace("'", "''") + "','" + strShippingPhone + "','" + OrderTotal + "','" + OrderSubtotal + "','" + Shippingtotal + "','" + OrderTax + "',0,'" + iAmazonOrderNumber.ToString().Replace("'", "''").Trim() + "','CREDITCARD','" + String.Format("{0:MM/dd/yyyy hh:mm:ss ttt}", Convert.ToDateTime(ds.Tables[0].Rows[i]["PO Date & Time"].ToString())) + "',1,0,0,'CAPTURED','','','','','','','','AUTHORIZENET',0,'" + strShippingMethod + "','','','0.0'); SELECT SCOPE_IDENTITY();"));

                                TotalImportOtrders++;

                                DataRow[] drtracking = ds.Tables[0].Select("[" + ds.Tables[0].Columns[2].ColumnName.ToString().Replace(":", "") + "]='" + iAmazonOrderNumber + "'");
                                if (drtracking != null && drtracking.Length > 0)
                                {
                                    foreach (DataRow dr in drtracking)
                                    {
                                        if (OrderNumber > 0 && dr["Order Status"].ToString().ToLower() == "shipped" && dr["Tracking #"].ToString().ToLower().IndexOf("+") <= -1)
                                        {
                                            Int32 ProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Productid from tb_product where (sku='" + dr["Item #"].ToString().Replace("'", "''").Replace("=", "").Replace("\"", "") + "') and storeid=" + StoreID));
                                            FillShipment(ProductID, dr["Carrier Name"].ToString(), dr["Tracking #"].ToString(), OrderNumber.ToString(), Convert.ToDateTime(dr["PO Date & Time"].ToString()));

                                        }


                                    }

                                }


                                //dbAccess.ExecuteNonQuery("update tb_order SET OrderDate='" + dtPurchase + "' where OrderNumber='" + OrderNumber + "' and storeid=" + StoreID + "");

                            }
                            else
                            {


                                AlreadyExsits++;
                            }

                        }
                        catch (Exception ex)
                        {
                            // WriteAmazonLog("Over Stock Error :" + ex.Message.ToString());
                        }
                    }
                    int diff = 0;
                    totalrecords = Convert.ToInt32(ds.Tables[0].DefaultView.ToTable(true, "P.O. Number").Rows.Count.ToString()); //Convert.ToInt32(drAll.Length);
                    if (totalrecords != (AlreadyExsits + TotalImportOtrders))
                    {
                        diff = (AlreadyExsits + TotalImportOtrders) - totalrecords;
                        if (diff > 0 && AlreadyExsits >= diff)
                        {
                            AlreadyExsits = AlreadyExsits - diff;
                        }
                    }

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                // WriteAmazonLog("Over Stock Error :" + ex.Message.ToString());
            }
            return true;
        }
    }
}