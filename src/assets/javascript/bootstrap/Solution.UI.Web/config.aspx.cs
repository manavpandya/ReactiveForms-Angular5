using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using Solution.Data;
using System.Collections;
using System.Data;
using Solution.Bussines.Entities;
using System.Globalization;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net.Mail;


namespace Solution.UI.Web
{
    public partial class config : System.Web.UI.Page
    {
        OrderComponent objOrder = new OrderComponent();
        SQLAccess objdb = new SQLAccess();
        DateTime dt = new DateTime();
        Hashtable htValues = new Hashtable();
        //ShoppingCartComponent objShoppingCart = new ShoppingCartComponent();
        ConfigurationComponent objconfig = new ConfigurationComponent();

        #region Variable for Save Images
        public static string ProductTempPath = string.Empty;
        public static string ProductIconPath = string.Empty;
        public static string ProductMediumPath = string.Empty;
        public static string ProductLargePath = string.Empty;
        public static string ProductMicroPath = string.Empty;
        static Size thumbNailSizeLarge = Size.Empty;
        static Size thumbNailSizeMediam = Size.Empty;
        static Size thumbNailSizeIcon = Size.Empty;
        static Size thumbNailSizeMicro = Size.Empty;
        static int finHeight;
        static int finWidth;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            WriteLog("Start Page Load: " + Request.QueryString["strid"].ToString());
            try
            {
                System.Text.StringBuilder sbText = new System.Text.StringBuilder();
                htValues.Clear();
                foreach (string key in Request.Form.Keys)
                {

                    htValues.Add(key, Request.Form[key]);
                    sbText.AppendLine(key + "," + Request.Form[key]);
                }
                WriteLog(sbText.ToString());
                Deserialize();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deserialize function for Calling Yahoo Order Data function
        /// </summary>
        private void Deserialize()
        {
            try
            {
                if (htValues.Keys.Count > 0)
                {
                    try
                    {
                        Int32 Onumber = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 OrderNumber FROM dbo.tb_Order WHERE RefOrderID='" + Convert.ToString(htValues["ID"]) + "' AND isnull(Deleted,0)=0"));
                        if (Onumber > 0)
                        { return; }
                    }
                    catch
                    {}
                    Int32 Storeid = 0;
                    if (string.IsNullOrEmpty(Request.QueryString["strid"]))
                    {

                        Int32 storeid = AppConfig.StoreID;

                        if (storeid == 0)
                        {
                            storeid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(StoreID,0) FROM dbo.tb_Store WHERE StoreName LIKE '%yahoo%'"));
                        }

                        if (storeid == 0)
                            Storeid = 1;
                        else
                            Storeid = storeid;
                    }
                    else
                        Storeid = Convert.ToInt32(Request.QueryString["strid"]);

                    //AppConfig.StoreID = Storeid;
                    //AppLogic.ApplicationStart();

                    #region Code for  Assing  Images Variable
                    String strImagePath = Convert.ToString(objdb.ExecuteScalarQuery("SELECT ISNULL(ConfigValue,'') AS ConfigValue FROM dbo.tb_AppConfig WHERE ConfigName='ImagePathProduct'AND StoreID=" + Convert.ToInt32(Request.QueryString["strid"]) + ""));
                    ProductTempPath = string.Concat(strImagePath, "Temp/");
                    ProductIconPath = string.Concat(strImagePath, "Icon/");
                    ProductMediumPath = string.Concat(strImagePath, "Medium/");
                    ProductLargePath = string.Concat(strImagePath, "Large/");
                    ProductMicroPath = string.Concat(strImagePath, "Micro/");

                    BindSize(Storeid);
                    #endregion

                    tb_Order tb_order = new tb_Order();
                    try
                    {
                        String RefOrderNo = Convert.ToString(htValues["ID"]);
                        DataSet dsOrderDetails = new DataSet();
                        WriteLog("GetOrderDetails: Ref. Order No." + RefOrderNo.ToString());
                        dsOrderDetails = GetOrderDetails(RefOrderNo, Storeid);
                        //dsOrderDetails = null;
                        SQLAccess objSql = new SQLAccess();

                        if (dsOrderDetails != null)
                        {


                            #region Set Billing Address
                            try
                            {
                                if (htValues.Contains("Bill-Email"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-Email"]).Trim()))
                                        htValues["Bill-Email"] = dsOrderDetails.Tables["GeneralInfo"].Select(" BillToInfo_Id is not null ")[0]["Email"].ToString();
                                }
                                else
                                    htValues.Add("Bill-Email", dsOrderDetails.Tables["GeneralInfo"].Select(" BillToInfo_Id is not null ")[0]["Email"].ToString());

                                if (htValues.Contains("Bill-Firstname"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-Firstname"]).Trim()))
                                        htValues["Bill-Firstname"] = dsOrderDetails.Tables["GeneralInfo"].Select(" BillToInfo_Id is not null ")[0]["FirstName"].ToString();
                                }
                                else
                                    htValues.Add("Bill-Firstname", dsOrderDetails.Tables["GeneralInfo"].Select(" BillToInfo_Id is not null ")[0]["FirstName"].ToString());


                                if (htValues.Contains("Bill-Lastname"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-Lastname"]).Trim()))
                                        htValues["Bill-Lastname"] = dsOrderDetails.Tables["GeneralInfo"].Select(" BillToInfo_Id is not null ")[0]["LastName"].ToString();
                                }
                                else
                                    htValues.Add("Bill-Lastname", dsOrderDetails.Tables["GeneralInfo"].Select(" BillToInfo_Id is not null ")[0]["LastName"].ToString());


                                if (htValues.Contains("Bill-Phone"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-Phone"]).Trim()))
                                        htValues["Bill-Phone"] = dsOrderDetails.Tables["GeneralInfo"].Select(" BillToInfo_Id is not null ")[0]["PhoneNumber"].ToString();
                                }
                                else
                                    htValues.Add("Bill-Phone", dsOrderDetails.Tables["GeneralInfo"].Select(" BillToInfo_Id is not null ")[0]["PhoneNumber"].ToString());

                                if (htValues.Contains("Bill-Company"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-Company"]).Trim()))
                                        htValues["Bill-Company"] = dsOrderDetails.Tables["GeneralInfo"].Select(" BillToInfo_Id is not null ")[0]["Company"].ToString();
                                }
                                else
                                    htValues.Add("Bill-Company", dsOrderDetails.Tables["GeneralInfo"].Select(" BillToInfo_Id is not null ")[0]["Company"].ToString());

                                if (htValues.Contains("Bill-Address1"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-Address1"]).Trim()))
                                        htValues["Bill-Address1"] = dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["Address1"].ToString();
                                }
                                else
                                    htValues.Add("Bill-Address1", dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["Address1"].ToString());

                                if (htValues.Contains("Bill-Address2"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-Address2"]).Trim()))
                                        htValues["Bill-Address2"] = dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["Address2"].ToString();
                                }
                                else
                                    htValues.Add("Bill-Address2", dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["Address2"].ToString());

                                if (htValues.Contains("Bill-City"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-City"]).Trim()))
                                        htValues["Bill-City"] = dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["City"].ToString();
                                }
                                else
                                    htValues.Add("Bill-City", dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["City"].ToString());

                                string strState = dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["State"].ToString();
                                string strState1 = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 ISNULL(NAME,'') AS NAME FROM dbo.tb_State WHERE Abbreviation='" + strState + "'"));
                                if (string.IsNullOrEmpty(strState1))
                                    strState1 = strState;
                                if (htValues.Contains("Bill-State"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-State"]).Trim()))
                                        htValues["Bill-State"] = strState1;
                                }
                                else
                                    htValues.Add("Bill-State", strState1);


                                if (htValues.Contains("Bill-Zip"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-Zip"]).Trim()))
                                        htValues["Bill-Zip"] = dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["Zip"].ToString();
                                }
                                else
                                    htValues.Add("Bill-Zip", dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["Zip"].ToString());

                                if (htValues.Contains("Bill-Country"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Bill-Country"]).Trim()))
                                        htValues["Bill-Country"] = dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["Country"].ToString();
                                }
                                else
                                    htValues.Add("Bill-Country", dsOrderDetails.Tables["AddressInfo"].Select(" BillToInfo_Id is not null ")[0]["Country"].ToString());
                            }
                            catch (Exception ex)
                            {
                                WriteLog("Billing Info Exception :" + ex.Message.ToString());
                            }
                            #endregion
                            #region Set Shipping Address
                            try
                            {
                                if (htValues.Contains("Ship-Email"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-Email"]).Trim()))
                                        htValues["Ship-Email"] = dsOrderDetails.Tables["GeneralInfo"].Select(" ShipToInfo_Id is not null ")[0]["Email"].ToString();
                                }
                                else
                                    htValues.Add("Ship-Email", dsOrderDetails.Tables["GeneralInfo"].Select(" ShipToInfo_Id is not null ")[0]["Email"].ToString());

                                if (htValues.Contains("Ship-Firstname"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-Firstname"]).Trim()))
                                        htValues["Ship-Firstname"] = dsOrderDetails.Tables["GeneralInfo"].Select(" ShipToInfo_Id is not null ")[0]["FirstName"].ToString();
                                }
                                else
                                    htValues.Add("Ship-Firstname", dsOrderDetails.Tables["GeneralInfo"].Select(" ShipToInfo_Id is not null ")[0]["FirstName"].ToString());


                                if (htValues.Contains("Ship-Lastname"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-Lastname"]).Trim()))
                                        htValues["Ship-Lastname"] = dsOrderDetails.Tables["GeneralInfo"].Select(" ShipToInfo_Id is not null ")[0]["LastName"].ToString();
                                }
                                else
                                    htValues.Add("Ship-Lastname", dsOrderDetails.Tables["GeneralInfo"].Select(" ShipToInfo_Id is not null ")[0]["LastName"].ToString());


                                if (htValues.Contains("Ship-Phone"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-Phone"]).Trim()))
                                        htValues["Ship-Phone"] = dsOrderDetails.Tables["GeneralInfo"].Select(" ShipToInfo_Id is not null ")[0]["PhoneNumber"].ToString();
                                }
                                else
                                    htValues.Add("Ship-Phone", dsOrderDetails.Tables["GeneralInfo"].Select(" ShipToInfo_Id is not null ")[0]["PhoneNumber"].ToString());

                                if (htValues.Contains("Ship-Company"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-Company"]).Trim()))
                                        htValues["Ship-Company"] = dsOrderDetails.Tables["GeneralInfo"].Select(" ShipToInfo_Id is not null ")[0]["Company"].ToString();
                                }
                                else
                                    htValues.Add("Ship-Company", dsOrderDetails.Tables["GeneralInfo"].Select(" ShipToInfo_Id is not null ")[0]["Company"].ToString());

                                if (htValues.Contains("Ship-Address1"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-Address1"]).Trim()))
                                        htValues["Ship-Address1"] = dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["Address1"].ToString();
                                }
                                else
                                    htValues.Add("Ship-Address1", dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["Address1"].ToString());

                                if (htValues.Contains("Ship-Address2"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-Address2"]).Trim()))
                                        htValues["Ship-Address2"] = dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["Address2"].ToString();
                                }
                                else
                                    htValues.Add("Ship-Address2", dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["Address2"].ToString());

                                if (htValues.Contains("Ship-City"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-City"]).Trim()))
                                        htValues["Ship-City"] = dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["City"].ToString();
                                }
                                else
                                    htValues.Add("Ship-City", dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["City"].ToString());


                                string strSState = dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["State"].ToString();
                                string strSState1 = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 ISNULL(NAME,'') AS NAME FROM dbo.tb_State WHERE Abbreviation='" + strSState + "'"));
                                if (string.IsNullOrEmpty(strSState1))
                                    strSState1 = strSState;
                                if (htValues.Contains("Ship-State"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-State"]).Trim()))
                                        htValues["Ship-State"] = strSState1;
                                }
                                else
                                    htValues.Add("Ship-State", strSState1);

                                if (htValues.Contains("Ship-Zip"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-Zip"]).Trim()))
                                        htValues["Ship-Zip"] = dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["Zip"].ToString();
                                }
                                else
                                    htValues.Add("Ship-Zip", dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["Zip"].ToString());

                                if (htValues.Contains("Ship-Country"))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Ship-Country"]).Trim()))
                                        htValues["Ship-Country"] = dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["Country"].ToString();
                                }
                                else
                                    htValues.Add("Ship-Country", dsOrderDetails.Tables["AddressInfo"].Select(" ShipToInfo_Id is not null ")[0]["Country"].ToString());
                            }
                            catch (Exception ex)
                            {
                                WriteLog("Shipping Info Exception :" + ex.Message.ToString());
                            }
                            #endregion


                            if (htValues.Contains("Tax-Charge"))
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(htValues["Tax-Charge"]).Trim()))
                                    htValues["Tax-Charge"] = dsOrderDetails.Tables["OrderTotals"].Rows[0]["Tax"].ToString();
                            }
                            else
                                htValues.Add("Tax-Charge", dsOrderDetails.Tables["OrderTotals"].Rows[0]["Tax"].ToString());


                            if (htValues.Contains("Total"))
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(htValues["Total"]).Trim()))
                                    htValues["Total"] = dsOrderDetails.Tables["OrderTotals"].Rows[0]["Total"].ToString();
                            }
                            else
                                htValues.Add("Total", dsOrderDetails.Tables["OrderTotals"].Rows[0]["Total"].ToString());


                            if (htValues.Contains("Shipping-Charge"))
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(htValues["Shipping-Charge"]).Trim()))
                                    htValues["Shipping-Charge"] = dsOrderDetails.Tables["OrderTotals"].Rows[0]["Shipping"].ToString();
                            }
                            else
                                htValues.Add("Shipping-Charge", dsOrderDetails.Tables["OrderTotals"].Rows[0]["Shipping"].ToString());


                            if (htValues.Contains("Shipping"))
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(htValues["Shipping"]).Trim()))
                                    htValues["Shipping"] = dsOrderDetails.Tables["Order"].Rows[0]["ShipMethod"].ToString();
                            }
                            else
                                htValues.Add("Shipping", dsOrderDetails.Tables["Order"].Rows[0]["ShipMethod"].ToString());


                            if (htValues.Contains("Item-Count"))
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Count"]).Trim()))
                                    htValues["Item-Count"] = dsOrderDetails.Tables["Item"].Rows.Count.ToString();
                            }
                            else
                                htValues.Add("Item-Count", dsOrderDetails.Tables["Item"].Rows.Count.ToString());

                            for (int cnt = 0; cnt < dsOrderDetails.Tables["Item"].Rows.Count; cnt++)
                            {
                                if (htValues.Contains("Item-Id-" + (cnt + 1)))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Id-" + (cnt + 1)]).Trim()))
                                        htValues["Item-Id-" + (cnt + 1)] = dsOrderDetails.Tables["Item"].Rows[cnt]["ItemID"].ToString();
                                }
                                else
                                    htValues.Add("Item-Id-" + (cnt + 1), dsOrderDetails.Tables["Item"].Rows[cnt]["ItemID"].ToString());


                                if (htValues.Contains("Item-Quantity-" + (cnt + 1)))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Quantity-" + (cnt + 1)]).Trim()))
                                        htValues["Item-Quantity-" + (cnt + 1)] = dsOrderDetails.Tables["Item"].Rows[cnt]["Quantity"].ToString();
                                }
                                else
                                    htValues.Add("Item-Quantity-" + (cnt + 1), dsOrderDetails.Tables["Item"].Rows[cnt]["Quantity"].ToString());

                                if (htValues.Contains("Item-Code-" + (cnt + 1)))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Code-" + (cnt + 1)]).Trim()))
                                        htValues["Item-Code-" + (cnt + 1)] = dsOrderDetails.Tables["Item"].Rows[cnt]["ItemCode"].ToString();
                                }
                                else
                                    htValues.Add("Item-Code-" + (cnt + 1), dsOrderDetails.Tables["Item"].Rows[cnt]["ItemCode"].ToString());

                                if (htValues.Contains("Item-Thumb-" + (cnt + 1)))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Thumb-" + (cnt + 1)]).Trim()))
                                        htValues["Item-Thumb-" + (cnt + 1)] = dsOrderDetails.Tables["Item"].Rows[cnt]["ThumbnailURL"].ToString();
                                }
                                else
                                    htValues.Add("Item-Thumb-" + (cnt + 1), dsOrderDetails.Tables["Item"].Rows[cnt]["ThumbnailURL"].ToString());

                                if (htValues.Contains("Item-Description-" + (cnt + 1)))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Description-" + (cnt + 1)]).Trim()))
                                        htValues["Item-Description-" + (cnt + 1)] = dsOrderDetails.Tables["Item"].Rows[cnt]["Description"].ToString();
                                }
                                else
                                    htValues.Add("Item-Description-" + (cnt + 1), dsOrderDetails.Tables["Item"].Rows[cnt]["Description"].ToString());

                                if (htValues.Contains("Item-Description-" + (cnt + 1)))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Description-" + (cnt + 1)]).Trim()))
                                        htValues["Item-Description-" + (cnt + 1)] = dsOrderDetails.Tables["Item"].Rows[cnt]["Description"].ToString();
                                }
                                else
                                    htValues.Add("Item-Description-" + (cnt + 1), dsOrderDetails.Tables["Item"].Rows[cnt]["Description"].ToString());


                                if (htValues.Contains("Item-Url-" + (cnt + 1)))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Url-" + (cnt + 1)]).Trim()))
                                        htValues["Item-Url-" + (cnt + 1)] = dsOrderDetails.Tables["Item"].Rows[cnt]["Url"].ToString();
                                }
                                else
                                    htValues.Add("Item-Url-" + (cnt + 1), dsOrderDetails.Tables["Item"].Rows[cnt]["Url"].ToString());

                                if (htValues.Contains("Item-Unit-Price-" + (cnt + 1)))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Unit-Price-" + (cnt + 1)]).Trim()))
                                        htValues["Item-Unit-Price-" + (cnt + 1)] = dsOrderDetails.Tables["Item"].Rows[cnt]["UnitPrice"].ToString();
                                }
                                else
                                    htValues.Add("Item-Unit-Price-" + (cnt + 1), dsOrderDetails.Tables["Item"].Rows[cnt]["UnitPrice"].ToString());


                                if (htValues.Contains("Item-Taxable-" + (cnt + 1)))
                                {
                                    if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Taxable-" + (cnt + 1)]).Trim()))
                                        htValues["Item-Taxable-" + (cnt + 1)] = (dsOrderDetails.Tables["Item"].Rows[cnt]["Taxable"].ToString().ToLower() == "true" ? "yes" : "no");
                                }
                                else
                                    htValues.Add("Item-Taxable-" + (cnt + 1), (dsOrderDetails.Tables["Item"].Rows[cnt]["Taxable"].ToString().ToLower() == "true" ? "yes" : "no"));

                                string selectedoptionlistid = string.Empty;
                                try
                                {
                                    selectedoptionlistid = dsOrderDetails.Tables["SelectedOptionList"].Select("Item_Id=" + dsOrderDetails.Tables["Item"].Rows[cnt]["Item_ID"].ToString() + "")[0]["SelectedOptionList_ID"].ToString();

                                    foreach (DataRow dr in dsOrderDetails.Tables["Option"].Select("SelectedOptionList_ID=" + selectedoptionlistid + ""))
                                    {
                                        if (htValues.Contains("Item-Option-" + (cnt + 1) + "-" + dr["Name"].ToString()))
                                        {
                                            if (String.IsNullOrEmpty(Convert.ToString(htValues["Item-Option-" + (cnt + 1) + "-" + dr["Name"].ToString()]).Trim()))
                                                htValues["Item-Option-" + (cnt + 1) + "-" + dr["Name"].ToString()] = dr["Value"].ToString();
                                        }
                                        else
                                            htValues.Add("Item-Option-" + (cnt + 1) + "-" + dr["Name"].ToString(), dr["Value"].ToString());

                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLog("Product Info Exception :" + ex.Message.ToString());
                                }
                            }
                            if (!string.IsNullOrEmpty(dsOrderDetails.Tables["Order"].Rows[0]["PaymentProcessor"].ToString().Trim()))
                            {
                                if (dsOrderDetails.Tables["Order"].Rows[0]["PaymentProcessor"].ToString().ToLower().Contains("paypal"))
                                {

                                    htValues["Card-Name"] = "PayPal";

                                    // objOrder.AVSResult = Convert.ToString(htValues["PayPal-Address-Status"]);
                                    //objOrder.AuthorizationResult = Convert.ToString(htValues["PayPal-Auth"]);
                                    //objOrder.AuthorizationPNREF = "AUTH=" + Convert.ToString(htValues["PayPal-TxID"]);
                                    //objOrder.AuthorizationCode = "AUTH=" + Convert.ToString(htValues["PayPal-TxID"]);

                                }
                                else
                                {
                                    //htValues["Card-Number"]
                                    //htValues["CardAuth-Amount"].ToString()
                                    // objOrder.AuthorizationResult = Convert.ToString(htValues["CardAuth-Auth-Response"]);
                                    //objOrder.AVSResult = Convert.ToString(htValues["CardAuth-Avs-Response"]);
                                    //objOrder.AuthorizationPNREF = Convert.ToString(htValues["CardAuth-Approval-Number"]);
                                    //objOrder.AuthorizationCode = Convert.ToString(htValues["CardAuth-Approval-Number"]);

                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        WriteLog("Common Exception :" + ex.Message.ToString());
                    }

                    Int32 iBillID = 0;
                    Int32 iShippID = 0;

                    //Insert into OrderShopping Cart
                    objdb = new SQLAccess();
                    Int32 orderSID = 0;
                    //Int32 orderSID = (Int32)objdb.ExecuteScalarQuery("SELECT MAX(OrderedShoppingCartID) FROM dbo.tb_OrderedShoppingCart");
                    //if (!string.IsNullOrEmpty(objdb.Error))
                    //    WriteLog(objdb.Error);


                    string ordEmail = Convert.ToString(htValues["Bill-Email"]);

                    SQLAccess dbAccess = new SQLAccess();
                    Int32 CustID = 0;
                    if (!String.IsNullOrEmpty(ordEmail.Trim()))
                        CustID = Convert.ToInt32(dbAccess.ExecuteScalarQuery("SELECT TOP 1 CustomerID FROM dbo.tb_Customer WHERE Email='" + ordEmail + "' and StoreID=" + Storeid));

                    if (CustID == 0)
                    {
                        CustomerComponent objCustCom = new CustomerComponent();
                        //tb_Customer tb_Customer = new tb_Customer();
                        //tb_Customer.Email = ordEmail;
                        //tb_Customer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Storeid);
                        //tb_Customer.FirstName = Convert.ToString(htValues["Bill-Firstname"]);
                        //tb_Customer.LastName = Convert.ToString(htValues["Bill-Lastname"]);
                        //// tb_Customer.Phone = Convert.ToString(htValues["Bill-Phone"]);
                        //tb_Customer.LastIPAddress = Convert.ToString(htValues["IP"]);
                        //tb_Customer.Deleted = true;
                        //tb_Customer.Active = false;
                        String strsql = "INSERT INTO dbo.tb_Customer( StoreID ,Email ,FirstName ,LastName , LastIPAddress , Active , Deleted ,CreatedOn ) VALUES  (" + Storeid + ",'" + ordEmail + "','" + Convert.ToString(htValues["Bill-Firstname"]).Replace("'", "''") + "','" + Convert.ToString(htValues["Bill-Lastname"]).Replace("'", "''") + "','" + Convert.ToString(htValues["IP"]) + "',0,0,getdate());  SELECT SCOPE_IDENTITY();";
                        CustID = Convert.ToInt32(objdb.ExecuteScalarQuery(strsql));
                        //CustID = objCustCom.InsertCustomer(tb_Customer, Storeid);
                        try
                        {
                            string strBCt = "0";
                            string strBAddState = "";

                            try
                            {
                                strBCt = Convert.ToString(objdb.ExecuteScalarQuery("select top 1 CountryID from tb_Country where  TwoLetterISOCode ='" + htValues["Bill-Country"].ToString().Substring(0, 2) + "'"));
                            }
                            catch
                            {
                                strBCt = "0";
                            }
                            try
                            {

                                strBAddState = Convert.ToString(objdb.ExecuteScalarQuery("SELECT TOP 1 ISNULL(NAME,'') AS NAME FROM dbo.tb_State WHERE Abbreviation='" + Convert.ToString(htValues["Bill-State"]) + "'"));
                            }
                            catch
                            {
                                strBAddState = Convert.ToString(htValues["Bill-State"]);
                            }
                            string strSql = "insert into tb_Address (CustomerID,FirstName,LastName,Company,Address1,Address2,City,[State],ZipCode,Country,Phone,Email,AddressType,StoreID) values (";
                            strSql = strSql + "" + CustID + ",'" + Convert.ToString(htValues["Bill-Firstname"]).Replace("'", "''") + "','" + Convert.ToString(htValues["Bill-Lastname"]).Replace("'", "''") + "',";
                            strSql = strSql + "'" + Convert.ToString(htValues["Bill-Company"]).Replace("'", "''") + "','" + Convert.ToString(htValues["Bill-Address1"]).Replace("'", "''") + "',";
                            strSql = strSql + "'" + Convert.ToString(htValues["Bill-Address2"]).Replace("'", "''") + "','" + Convert.ToString(htValues["Bill-City"]).Replace("'", "''") + "',";
                            strSql = strSql + "'" + Convert.ToString(strBAddState).Replace("'", "''") + "','" + Convert.ToString(htValues["Bill-Zip"]).Replace("'", "''") + "',";
                            strSql = strSql + "" + strBCt + ",'" + Convert.ToString(htValues["Bill-Phone"]).Replace("'", "''") + "',";
                            strSql = strSql + "'" + Convert.ToString(htValues["Bill-Email"]).Replace("'", "''") + "',0,";
                            strSql = strSql + "'" + Storeid + "') SELECT IDENT_CURRENT('tb_Address')";

                            iBillID = Convert.ToInt32(objdb.ExecuteScalarQuery(strSql));
                            try
                            {
                                strBCt = Convert.ToString(objdb.ExecuteScalarQuery("select top 1 CountryID from tb_Country where  TwoLetterISOCode ='" + htValues["Ship-Country"].ToString().Substring(0, 2) + "'"));
                            }
                            catch
                            {
                                strBCt = "0";
                            }
                            try
                            {

                                strBAddState = Convert.ToString(objdb.ExecuteScalarQuery("SELECT TOP 1 ISNULL(NAME,'') AS NAME FROM dbo.tb_State WHERE Abbreviation='" + Convert.ToString(htValues["Ship-State"]) + "'"));
                            }
                            catch
                            {
                                strBAddState = Convert.ToString(htValues["Ship-State"]);
                            }
                            string strSql1 = "insert into tb_Address (CustomerID,FirstName,LastName,Company,Address1,Address2,City,[State],ZipCode,Country,Phone,Email,AddressType,StoreID) values (";
                            strSql1 = strSql1 + "" + CustID + ",'" + Convert.ToString(htValues["Ship-Firstname"]).Replace("'", "''") + "','" + Convert.ToString(htValues["Ship-Lastname"]).Replace("'", "''") + "',";
                            strSql1 = strSql1 + "'" + Convert.ToString(htValues["Ship-Company"]).Replace("'", "''") + "','" + Convert.ToString(htValues["Ship-Address1"]).Replace("'", "''") + "',";
                            strSql1 = strSql1 + "'" + Convert.ToString(htValues["Ship-Address2"]).Replace("'", "''") + "','" + Convert.ToString(htValues["Ship-City"]).Replace("'", "''") + "',";
                            strSql1 = strSql1 + "'" + Convert.ToString(strBAddState).Replace("'", "''") + "','" + Convert.ToString(htValues["Ship-Zip"]).Replace("'", "''") + "',";
                            strSql1 = strSql1 + "" + strBCt + ",'" + Convert.ToString(htValues["Ship-Phone"]).Replace("'", "''") + "',";
                            strSql1 = strSql1 + "'" + Convert.ToString(htValues["Ship-Email"]).Replace("'", "''") + "',1,";
                            strSql1 = strSql1 + "'" + Storeid + "') SELECT IDENT_CURRENT('tb_Address')";

                            iShippID = Convert.ToInt32(objdb.ExecuteScalarQuery(strSql1));

                            objdb.ExecuteNonQuery("UPDATE tb_Customer SET BillingAddressID=" + iBillID + ",ShippingAddressID=" + iShippID + " WHERE CustomerID=" + CustID + "");
                        }
                        catch (Exception ex)
                        {
                            WriteLog("Customer Info Exception :" + ex.Message.ToString());
                        }
                    }
                    else
                    {
                        iBillID = Convert.ToInt32(dbAccess.ExecuteScalarQuery("SELECT BillingAddressID FROM tb_Customer WHERE CustomerID=" + CustID));
                        iShippID = Convert.ToInt32(dbAccess.ExecuteScalarQuery("SELECT ShippingAddressID FROM tb_Customer WHERE CustomerID=" + CustID));
                    }

                    //  orderSID++;
                    //objShoppingCart.OrderedShoppingCartID = orderSID;
                    //objShoppingCart.CustomerID = CustID;
                    //objShoppingCart.CouponCode = Convert.ToString(htValues["Coupon-Id"]);
                    //objShoppingCart.CreatedOn = System.DateTime.Now;


                    try
                    {
                        orderSID = Convert.ToInt32(objdb.ExecuteScalarQuery("insert into tb_OrderedShoppingCart (CustomerID,CreatedOn) values(" + CustID + ",getdate()); SELECT SCOPE_IDENTITY();"));
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message.ToString());
                    }

                    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                    string ProductName = "";
                    string YahooID = "";
                    string ProductURl = "";
                    decimal Price = 0;
                    string VarinatNames = "";
                    string VariantValues = "";
                    string ItemCode = "";
                    int ItemTaxable = 0;
                    int quantity = 0;
                    string ItemThumb = "";
                    string ItemOption = "";

                    int ItemCount = 0;
                    int.TryParse(Convert.ToString(htValues["Item-Count"]), out ItemCount);
                    if (ItemCount == 0)
                        ItemCount = 100;

                    decimal decOrderSubTotal = 0;
                    //Insert into OrderShopping Cart Item
                    for (int cnt = 1; cnt <= ItemCount; cnt++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(htValues["Item-Id-" + cnt])))
                        {
                            YahooID = Convert.ToString(htValues["Item-Id-" + cnt]);
                            ItemOption = Convert.ToString(htValues["Item-Option-" + cnt]);
                            ItemCode = Convert.ToString(htValues["Item-Code-" + cnt]);
                            ItemThumb = Convert.ToString(htValues["Item-Thumb-" + cnt]);
                            ProductName = Convert.ToString(htValues["Item-Description-" + cnt]);
                            ProductURl = Convert.ToString(htValues["Item-Url-" + cnt]);
                            decimal.TryParse(Convert.ToString(htValues["Item-Unit-Price-" + cnt]), out Price);
                            // chosize = Convert.ToString(htValues["Item-Option-" + cnt + "-SIZE"]);
                            // chocolor = Convert.ToString(htValues["Item-Option-" + cnt + "-COLOR"]);
                            VarinatNames = "";
                            VariantValues = "";




                            foreach (object key in htValues.Keys)
                            {
                                if (key.ToString().StartsWith("Item-Option-" + cnt + "-"))
                                {
                                    try
                                    {
                                        VarinatNames += key.ToString().Substring(key.ToString().LastIndexOf("-") + 1).Replace(",", "") + ",";
                                    }
                                    catch
                                    {
                                        VarinatNames += key.ToString().Substring(key.ToString().LastIndexOf("-") + 1) + ",";
                                    }
                                    VariantValues += htValues[key].ToString()+ ",";

                                }
                            }


                            Int32.TryParse(Convert.ToString(htValues["Item-Quantity-" + cnt]), out quantity);
                            ItemTaxable = string.Equals(Convert.ToString(htValues["Item-Taxable-" + cnt]), "yes", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
                            String productID = "0";

                            objdb = new SQLAccess();
                            try
                            {
                                productID = Convert.ToString(objdb.ExecuteScalarQuery("SELECT TOP 1 ProductID FROM dbo.tb_Product WHERE ISNULL(YahooID,'')='" + YahooID + "' AND StoreID=" + Storeid));

                            }
                            catch (Exception ex)
                            {
                                WriteLog(ex.Message.ToString());
                            }

                            if (string.IsNullOrEmpty(productID))
                            {
                                productID = AddProduct(ProductName, ItemCode, YahooID, ProductURl, ItemThumb, Price, Storeid).ToString();
                            }

                            try
                            {

                                bool blsho = objdb.ExecuteNonQuery("insert into tb_OrderedShoppingCartItems (RefProductID,SKU,OrderedShoppingCartID,Quantity,VariantNames,VariantValues,Price,InventoryUpdated,ProductName,YahooID,ProductURL) values(" + productID + ",'" + ItemCode + "'," + orderSID + "," + quantity + ",'" + VarinatNames.Replace("'", "''") + "','" + VariantValues.Replace("'", "''") + "'," + Price + ",0,'" + ProductName.Replace("'", "''") + "','" + YahooID + "','" + ProductURl.ToString() + "')");
                            }
                            catch (Exception ex)
                            {
                                WriteLog(ex.Message.ToString());
                            }

                            decOrderSubTotal += Price * quantity;
                            tb_order.OrderSubtotal = decOrderSubTotal;
                            //objOrder.SubTotal = decOrderSubTotal;

                            //objdb.ExecuteNonQuery("update tb_Ecomm_Product set inventory=inventory-" + quantity + " where productid=" + productID);
                        }
                        else
                            break;
                    }
                    CountryComponent objcntry = new CountryComponent();
                    Int32 billco = 0;

                    //Insert into Order
                    //objOrder.StoreID = Storeid.ToString();
                    //objOrder.storeID = Storeid.ToString();
                    tb_order.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Storeid);


                    //try
                    //{
                    //    //orderSID = (Int32)objdb.ExecuteScalarQuery("SELECT MAX(OrderedShoppingCartID) FROM dbo.tb_OrderedShoppingCart");
                    //}
                    //catch (Exception ex)
                    //{
                    //    WriteLog(ex.Message.ToString());
                    //}

                    //objOrder.ShoppingCartID = orderSID;

                    //objOrder.ShippingLastName = Convert.ToString(htValues["Ship-Lastname"]);
                    //objOrder.ShippingFirstName = Convert.ToString(htValues["Ship-Firstname"]);
                    //objOrder.ShippingCompany = Convert.ToString(htValues["Ship-Company"]);
                    //objOrder.ShippingAddress1 = Convert.ToString(htValues["Ship-Address1"]);
                    //objOrder.ShippingAddress2 = Convert.ToString(htValues["Ship-Address2"]);
                    //objOrder.ShippingCity = Convert.ToString(htValues["Ship-City"]);
                    //objOrder.ShippingState = Convert.ToString(htValues["Ship-State"]);

                    tb_order.ShoppingCardID = orderSID;

                    tb_order.ShippingLastName = Convert.ToString(htValues["Ship-Lastname"]);
                    tb_order.ShippingFirstName = Convert.ToString(htValues["Ship-Firstname"]);
                    tb_order.ShippingCompany = Convert.ToString(htValues["Ship-Company"]);
                    tb_order.ShippingAddress1 = Convert.ToString(htValues["Ship-Address1"]);
                    tb_order.ShippingAddress2 = Convert.ToString(htValues["Ship-Address2"]);
                    tb_order.ShippingCity = Convert.ToString(htValues["Ship-City"]);
                    tb_order.ShippingState = Convert.ToString(htValues["Ship-State"]);

                    string strShipState = "";
                    try
                    {
                        strShipState = Convert.ToString(objdb.ExecuteScalarQuery("SELECT TOP 1 ISNULL(NAME,'') AS NAME FROM dbo.tb_State WHERE Abbreviation='" + tb_order.ShippingState.ToString() + "'"));
                    }
                    catch (Exception ex)
                    {
                        WriteLog("State Exception: " + ex.Message.ToString());
                    }
                    if (!string.IsNullOrEmpty(strShipState))
                        tb_order.ShippingState = strShipState;

                    tb_order.ShippingZip = Convert.ToString(htValues["Ship-Zip"]);

                    string strBCountry = string.Empty;
                    string strSCountryName = string.Empty;
                    if (htValues["Ship-Country"] != null)
                    {
                        try
                        {
                            strBCountry = Convert.ToString(objdb.ExecuteScalarQuery("select top 1 CountryID from dbo.tb_Country where  TwoLetterISOCode ='" + htValues["Ship-Country"].ToString().Substring(0, 2) + "'"));
                        }
                        catch (Exception ex)
                        {
                            WriteLog("Country Exception: " + ex.Message.ToString());
                        }
                        int.TryParse(strBCountry, out billco);
                        try
                        {
                            strSCountryName = Convert.ToString(objdb.ExecuteScalarQuery("select top 1 isnull(Name,'') as Name from dbo.tb_Country where  TwoLetterISOCode ='" + htValues["Ship-Country"].ToString().Substring(0, 2) + "'"));
                        }
                        catch (Exception ex)
                        {
                            WriteLog("Shipping Country Name Exception: " + ex.Message.ToString());
                        }
                    }
                    tb_order.ShippingCountry = strSCountryName.ToString(); //billco.ToString();
                    tb_order.ShippingPhone = Convert.ToString(htValues["Ship-Phone"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(htValues["Bill-Country"])))
                    {
                        tb_order.BillingLastName = Convert.ToString(htValues["Bill-Lastname"]);
                        tb_order.BillingFirstName = Convert.ToString(htValues["Bill-Firstname"]);
                        tb_order.BillingCompany = Convert.ToString(htValues["Bill-Company"]);
                        tb_order.BillingAddress1 = Convert.ToString(htValues["Bill-Address1"]);
                        tb_order.BillingAddress2 = Convert.ToString(htValues["Bill-Address2"]);
                        tb_order.BillingCity = Convert.ToString(htValues["Bill-City"]);
                        tb_order.BillingState = Convert.ToString(htValues["Bill-State"]);
                        tb_order.BillingZip = Convert.ToString(htValues["Bill-Zip"]);
                        string strBillState = "";
                        try
                        {
                            strBillState = Convert.ToString(objdb.ExecuteScalarQuery("SELECT TOP 1 ISNULL(NAME,'') AS NAME FROM dbo.tb_State WHERE Abbreviation='" + tb_order.BillingState.ToString() + "'"));
                        }
                        catch (Exception ex)
                        {
                            WriteLog("State Exception: " + ex.Message.ToString());
                        }
                        if (!string.IsNullOrEmpty(strBillState))
                            tb_order.BillingState = strBillState;

                        try
                        {
                            strBCountry = Convert.ToString(objdb.ExecuteScalarQuery("select top 1 CountryID from tb_Country where  TwoLetterISOCode ='" + htValues["Bill-Country"].ToString().Substring(0, 2) + "'"));

                        }
                        catch (Exception ex)
                        {
                            WriteLog(" Country Exception: " + ex.Message.ToString());
                        }
                        String strBCountryName = "";
                        try
                        {
                            strBCountryName = Convert.ToString(objdb.ExecuteScalarQuery("select top 1 isnull(Name,'') as BName from tb_Country where  TwoLetterISOCode ='" + htValues["Bill-Country"].ToString().Substring(0, 2) + "'"));

                        }
                        catch (Exception ex)
                        {
                            WriteLog("Billing Country Name Exception: " + ex.Message.ToString());
                        }
                        int.TryParse(strBCountry, out billco);

                        //tb_order.BillingCountry = billco.ToString();
                        tb_order.BillingCountry = strBCountryName.ToString();
                        tb_order.BillingPhone = Convert.ToString(htValues["Bill-Phone"]);

                    }
                    else
                    {
                        tb_order.BillingLastName = !string.IsNullOrEmpty(Convert.ToString(htValues["Bill-Lastname"])) ? Convert.ToString(htValues["Bill-Lastname"]) : Convert.ToString(htValues["Ship-Lastname"]);
                        tb_order.BillingFirstName = !string.IsNullOrEmpty(Convert.ToString(htValues["Bill-Firstname"])) ? Convert.ToString(htValues["Bill-Firstname"]) : Convert.ToString(htValues["Ship-Firstname"]);
                        tb_order.BillingCompany = tb_order.ShippingCompany;
                        tb_order.BillingAddress1 = tb_order.ShippingAddress1;
                        tb_order.BillingAddress2 = tb_order.ShippingAddress2;
                        tb_order.BillingCity = tb_order.ShippingCity;
                        tb_order.BillingState = tb_order.ShippingState;
                        tb_order.BillingZip = tb_order.ShippingZip;
                        tb_order.BillingCountry = strSCountryName.ToString();//billco.ToString();
                        tb_order.BillingPhone = tb_order.ShippingPhone;
                    }


                    tb_order.Deleted = false;
                    tb_order.ShippingMethod = Convert.ToString(htValues["Shipping"]);
                    tb_order.CouponCodeDescription = Convert.ToString(htValues["Coupon-Description"]);
                    tb_order.CardVarificationCode = "N.A.";

                    if (!string.IsNullOrEmpty(Convert.ToString(htValues["Card-Expiry"])))
                    {
                        string[] carddate = htValues["Card-Expiry"].ToString().Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        tb_order.CardExpirationMonth = carddate[0];
                        tb_order.CardExpirationYear = carddate[1];
                    }
                    if (htValues["Total"] != null)
                    {
                        decimal oTotal = 0;
                        decimal.TryParse(Convert.ToString(htValues["Total"]), out oTotal);
                        tb_order.OrderTotal = oTotal;
                    }
                    tb_order.OrderSubtotal = decOrderSubTotal;

                    if (htValues["Tax-Charge"] != null)
                    {
                        decimal orTax = 0;
                        decimal.TryParse(htValues["Tax-Charge"].ToString(), out orTax);
                        tb_order.OrderTax = orTax;
                    }
                    if (htValues["Date"] != null)
                    {
                        string[] day = htValues["Date"].ToString().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string Orderdate = day[1] + " " + day[2] + " " + day[4] + " " + day[3];
                        dt = Convert.ToDateTime(Orderdate);
                        tb_order.OrderDate = Convert.ToDateTime(Orderdate);
                    }
                    tb_order.LastIPAddress = Convert.ToString(htValues["IP"]);

                    if (htValues["Shipping-Charge"] != null)
                    {
                        decimal osc = 0;
                        decimal.TryParse(htValues["Shipping-Charge"].ToString(), out osc);
                        tb_order.OrderShippingCosts = osc;
                    }

                    tb_order.StoreName = Convert.ToString(htValues["Store-Name"]);
                    tb_order.RefStoreID = Convert.ToString(htValues["Store-Id"]);

                    tb_order.CouponCodeDescription = Convert.ToString(htValues["Coupon-Description"]);
                    if (htValues["Card-Number"] != null && !String.IsNullOrEmpty(htValues["Card-Number"].ToString().Trim()))
                    {
                        tb_order.CardNumber = "";// SecurityComponent.Encrypt(Convert.ToString(htValues["Card-Number"]));
                    }
                    else
                    {
                        tb_order.CardNumber = "";

                    }
                    //objOrder.CardName = Convert.ToString(htValues["Card-Name"]);


                    tb_order.Notes = Convert.ToString(htValues["Comments"]);
                    tb_order.LastName = Convert.ToString(htValues["Bill-Lastname"]);
                    tb_order.FirstName = Convert.ToString(htValues["Bill-Firstname"]);
                    tb_order.Email = Convert.ToString(htValues["Bill-Email"]);

                    if (htValues["Coupon-Value"] != null)
                    {
                        decimal cda = 0;
                        decimal.TryParse(htValues["Coupon-Value"].ToString(), out cda);
                        tb_order.CouponDiscountAmount = cda;
                    }

                    if (htValues["CardCharge"] != null)
                    {
                        decimal ca = 0;
                        decimal.TryParse(htValues["CardCharge"].ToString(), out ca);
                        tb_order.ChargeAmount = ca;
                    }
                    tb_order.CustomerID = CustID;


                    //tb_order.Phone = Convert.ToString(htValues["Bill-Phone"]);
                    tb_order.RefOrderID = Convert.ToString(htValues["ID"]);
                    tb_order.OrderGUID = System.Guid.NewGuid().ToString();
                    bool isCreditCardMethod = false;
                    if (htValues["Card-Number"] != null && !String.IsNullOrEmpty(htValues["Card-Number"].ToString().Trim()))
                    {
                        try
                        {
                            string cardVerificationcode = "";
                            try
                            {
                                tb_order.Last4 = htValues["Card-Number"].ToString().Substring(htValues["Card-Number"].ToString().Length - 4).ToString();
                                if (htValues["Card-Name"].ToString().ToLower().IndexOf("american") > -1 || htValues["Card-Name"].ToString().ToLower().IndexOf("amex") > -1)
                                {
                                    cardVerificationcode = htValues["Card-Number"].ToString().Substring(htValues["Card-Number"].ToString().Length - 4).ToString();
                                }
                                else
                                {
                                    cardVerificationcode = htValues["Card-Number"].ToString().Substring(htValues["Card-Number"].ToString().Length - 3).ToString();
                                }
                                tb_order.CardVarificationCode = ""; //cardVerificationcode;
                            }
                            catch (Exception ex)
                            {
                                WriteLog("Credit Card Exception: " + ex.Message.ToString());
                            }

                            try
                            {
                                tb_order.CardName = "";
                                if (htValues["Card-Name"].ToString().ToLower() == "mastercard")
                                {
                                    tb_order.CardType = "Master Card";
                                }
                                else if (htValues["Card-Name"].ToString().ToLower().IndexOf("american") > -1)
                                {
                                    tb_order.CardType = "AMEX";
                                }
                                else
                                {
                                    tb_order.CardType = Convert.ToString(htValues["Card-Name"].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog("Credit Card Type Exception: " + ex.Message.ToString());
                            }
                            isCreditCardMethod = true;
                            tb_order.PaymentMethod = "CREDITCARD";
                            tb_order.CardName = Convert.ToString(htValues["Bill-Firstname"]);

                            if (htValues["CardAuth-Amount"] != null)
                            {
                                decimal aa = 0;
                                decimal.TryParse(htValues["CardAuth-Amount"].ToString(), out aa);
                                tb_order.AuthorizedAmount = aa;
                            }
                            tb_order.AuthorizationResult = Convert.ToString(htValues["CardAuth-Auth-Response"]);
                            tb_order.AVSResult = Convert.ToString(htValues["CardAuth-Avs-Response"]);
                            tb_order.AuthorizationPNREF = Convert.ToString(htValues["CardAuth-Approval-Number"]);
                            tb_order.AuthorizationCode = Convert.ToString(htValues["CardAuth-Approval-Number"]);

                            //if (!string.IsNullOrEmpty(tb_order.AuthorizationCode))
                            //{
                            tb_order.TransactionStatus = AppLogic.ro_TXStateAuthorized;
                            tb_order.AuthorizedOn = DateTime.Now;
                            //}
                            //else
                            //if (Storeid == 5)
                            //{
                            //tb_order.TransactionStatus = AppLogic.ro_TXStatePending;
                            //}
                            //else
                            //{
                            //    tb_order.TransactionStatus = AppLogic.ro_TXStateCaptured;
                            //    tb_order.CapturedOn = DateTime.Now;
                            //}

                            // dbAccess.ExecuteNonQuery("update tb_Address set NameOnCard='" + tb_order.CardName + "',Last4='" + tb_order.Last4 + "',CardVarificationCode='" + cardVerificationcode + "', cardtype='" + tb_order.PaymentMethodID + "',CardNumber='" + objOrder.CardNumber + "',CardExpirationMonth=" + objOrder.CardExpirationMonth + ",CardExpirationYear=" + objOrder.CardExpirationYear + " where addressid=" + iBillID);
                            //dbAccess.ExecuteNonQuery("update tb_Address set NameOnCard='" + tb_order.CardName + "',Last4='" + tb_order.Last4 + "',CardVarificationCode='" + cardVerificationcode + "', cardtype='" + tb_order.PaymentMethodID + "',CardNumber='" + objOrder.CardNumber + "',CardExpirationMonth=" + objOrder.CardExpirationMonth + ",CardExpirationYear=" + objOrder.CardExpirationYear + " where addressid=" + iShippID);



                        }
                        catch (Exception ex)
                        {
                            WriteLog("Credit Cart Exception : " + ex.Message.ToString());
                        }
                    }
                    else if (Convert.ToString(htValues["Card-Name"]) == "PayPal")
                    {
                        try
                        {
                            tb_order.PaymentMethod = "PayPalExpress";
                            tb_order.AVSResult = Convert.ToString(htValues["PayPal-Address-Status"]);
                            tb_order.AuthorizationResult = Convert.ToString(htValues["PayPal-Auth"]);
                            tb_order.AuthorizationPNREF = "AUTH=" + Convert.ToString(htValues["PayPal-TxID"]);
                            tb_order.AuthorizationCode = "AUTH=" + Convert.ToString(htValues["PayPal-TxID"]);

                            //if (!string.IsNullOrEmpty(tb_order.AuthorizationCode))
                            //{
                            //    tb_order.TransactionStatus = AppLogic.ro_TXStateAuthorized;
                            //    tb_order.AuthorizedOn = DateTime.Now;
                            //}
                            //else
                            //    tb_order.TransactionStatus = AppLogic.ro_TXStatePending;

                            tb_order.TransactionStatus = AppLogic.ro_TXStateCaptured;
                            tb_order.CapturedOn = DateTime.Now;

                        }
                        catch (Exception ex)
                        {
                            WriteLog("Paypal Exception: " + ex.Message.ToString());
                        }
                    }
                    tb_order.IsNew = true;
                    tb_order.InventoryWasReduced = true;

                    //tb_order.TransactionType = 1;
                    Int32 OrderNumber = 0;
                    OrderNumber = Convert.ToInt32(objOrder.YahooAddOrder(tb_order, OrderNumber, Storeid));

                    //objdb.ExecuteNonQuery("insert into tb_Ecomm_OrderShoppingCart (OrderNumber,OrderedShoppingCartID) values(" + OrderNumber + "," + orderSID + ")");
                    //if (!string.IsNullOrEmpty(objdb.Error))
                    //    WriteLog(objdb.Error);

                    string cvc2res = "";

                    if (htValues["CardAuth-Cvc2-Response"] != null)
                    {
                        cvc2res = htValues["CardAuth-Cvc2-Response"].ToString();
                    }

                    string BillName = "";
                    String ShipName = "";
                    string strCardType = htValues["Card-Name"].ToString();
                    if (strCardType == "PayPal")
                        strCardType = string.Empty;

                    if (htValues["Bill-Name"] != null)
                        BillName = Convert.ToString(htValues["Bill-Name"]).Replace("'", "''");
                    if (htValues["Ship-Name"] != null)
                        ShipName = Convert.ToString(htValues["Ship-Name"]).Replace("'", "''");
                    try
                    {
                        if (Convert.ToString(htValues["Card-Name"]) == "PayPal")
                        {
                            if (htValues["PayPal-Payer-Status"] != null)
                            {
                                cvc2res = htValues["PayPal-Payer-Status"].ToString();
                            }

                            objdb.ExecuteNonQuery("update tb_order set  CardName='" + Convert.ToString(htValues["Bill-Firstname"]) + "',CardType='" + strCardType + "',Cvc2Response='" + cvc2res + "',BillName='" + BillName + "',ShipName='" + ShipName
                           + "',ItemCount=" + ItemCount + ",OrderDate='" + dt + "',AuthorizationPNREF='" + tb_order.AuthorizationPNREF + "',IsYahooOrder=1 where OrderNumber=" + OrderNumber + "");
                        }
                        else
                        {
                            objdb.ExecuteNonQuery("update tb_order set  CardName='" + Convert.ToString(htValues["Bill-Firstname"]) + "',CardType='" + strCardType + "',Cvc2Response='" + cvc2res + "',BillName='" + BillName + "',ShipName='" + ShipName
                          + "',ItemCount=" + ItemCount + ",OrderDate='" + dt + "',AuthorizationPNREF='" + tb_order.AuthorizationPNREF + "',IsYahooOrder=1 where OrderNumber=" + OrderNumber + "");
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Update Order After Add Order: " + ex.Message.ToString());
                    }
                    //string Status = "";
                    //try
                    //{
                    //    //if (Storeid == 5)
                    //    //{
                    //    if (htValues["Card-Number"] != null && !String.IsNullOrEmpty(htValues["Card-Number"].ToString().Trim()))
                    //    {
                    //        if (htValues["CardAuth-Approval-Number"] != null && !String.IsNullOrEmpty(htValues["CardAuth-Approval-Number"].ToString().Trim()))
                    //        {
                    //            /////Order Capture here;

                    //            //Status = AuthorizeNetComponent.CaptureOrder(tb_order);
                    //            ////PayPalComponent objPaypal = new PayPalComponent();
                    //            ////Status = objPaypal.CaptureOrder(tb_order);
                    //            //if (Status.ToUpper() == "OK")
                    //            //{
                    //            //    CommonComponent.ExecuteCommonData("UPDATE tb_order SET CaptureTXCommand='" + tb_order.CaptureTxCommand.ToString().Replace("'", "''") + "',CaptureTXResult='" + tb_order.CaptureTXResult.ToString().Replace("'", "''") + "',AuthorizationPNREF='" + tb_order.AuthorizationPNREF.ToString().Replace("'", "''") + "', CapturedOn=dateadd(hour,-2,getdate()), TransactionStatus='CAPTURED' WHERE orderNumber=" + OrderNumber.ToString() + "");
                    //            //}
                    //        }
                    //        else
                    //        {
                    //            String AVSResult = String.Empty;
                    //            String AuthorizationResult = String.Empty;
                    //            String AuthorizationCode = String.Empty;
                    //            String AuthorizationTransID = String.Empty;
                    //            String TransactionCommand = String.Empty;
                    //            String TransactionResponse = String.Empty;
                    //            OrderComponent objCapture = new OrderComponent();
                    //            DataSet dsPayment = new DataSet();
                    //            dsPayment = objCapture.GetPaymentGateway(tb_order.PaymentMethod.ToString(), Convert.ToInt32(Storeid));
                    //            string PaymentGatewayStatus = "";
                    //            if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
                    //            {
                    //                PaymentGatewayStatus = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString());
                    //            }

                    //            Status = AuthorizeNetComponent.ProcessCardForYahooorder(OrderNumber, Convert.ToInt32(tb_order.CustomerID), Convert.ToDecimal(tb_order.OrderTotal), AppLogic.AppConfigBool("UseLiveTransactions"), PaymentGatewayStatus.ToString(), tb_order, tb_order, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    //            if (Status.ToUpper() == "OK")
                    //            {
                    //                if (PaymentGatewayStatus.ToString().ToLower().Trim().IndexOf("auth_only") > -1)
                    //                {
                    //                    CommonComponent.ExecuteCommonData("UPDATE tb_order SET AuthorizedAmount='" + tb_order.OrderTotal.ToString() + "', AVSResult='" + AVSResult.ToString().Replace("'", "''") + "',AuthorizationResult='" + AuthorizationResult.ToString().Replace("'", "''") + "',AuthorizationCode='" + AuthorizationCode.ToString().Replace("'", "''") + "',AuthorizationPNREF='" + AuthorizationTransID.ToString().Replace("'", "''") + "',TransactionCommand='" + TransactionCommand.ToString().Replace("'", "''") + "',AuthorizedOn=getdate(),TransactionStatus='AUTHORIZED' WHERE orderNumber=" + OrderNumber.ToString() + "");
                    //                }
                    //                else
                    //                {
                    //                    CommonComponent.ExecuteCommonData("UPDATE tb_order SET AuthorizedAmount='" + tb_order.OrderTotal.ToString() + "', AVSResult='" + AVSResult.ToString().Replace("'", "''") + "',AuthorizationResult='" + AuthorizationResult.ToString().Replace("'", "''") + "',AuthorizationCode='" + AuthorizationCode.ToString().Replace("'", "''") + "',AuthorizationPNREF='" + AuthorizationTransID.ToString().Replace("'", "''") + "',TransactionCommand='" + TransactionCommand.ToString().Replace("'", "''") + "',CapturedOn=dateadd(hour,-2,getdate()), TransactionStatus='CAPTURED' WHERE orderNumber=" + OrderNumber.ToString() + "");
                    //                }

                    //            }
                    //            else
                    //            {
                    //                OrderComponent objDsorder = new OrderComponent();
                    //                tb_FailedTransaction objFailed = new tb_FailedTransaction();
                    //                objFailed.OrderNumber = Convert.ToInt32(OrderNumber);
                    //                objFailed.CustomerID = Convert.ToInt32(tb_order.CustomerID);
                    //                objFailed.PaymentGateway = Convert.ToString(tb_order.PaymentGateway);
                    //                objFailed.Paymentmethod = Convert.ToString(tb_order.PaymentMethod.ToString());
                    //                objFailed.TransactionCommand = Convert.ToString(TransactionCommand);
                    //                objFailed.TransactionResult = Convert.ToString(TransactionResponse);
                    //                objFailed.OrderDate = DateTime.Now;
                    //                objFailed.IPAddress = Request.UserHostAddress.ToString();
                    //                Int32 FaileId = Convert.ToInt32(objDsorder.AddOrderFailedTransaction(objFailed, Storeid));
                    //                WriteLog("Order Authorized Error:  orderNumber=" + OrderNumber.ToString() + " - " + Status.ToString());
                    //            }
                    //            try
                    //            {
                    //                SendMail(OrderNumber, tb_order.Email.ToString(), Storeid);
                    //            }
                    //            catch { }
                    //        }
                    //    }
                    //    //}
                    //}
                    //catch
                    //{
                    //    WriteLog("Order Authorized Error: " + Status.ToString());
                    //}
                    //try
                    //{
                    //    if (objOrder.TransactionState == ECommerceSite.Client.AppLogic.ro_TXStatePending && isCreditCardMethod)
                    //        MakeOrder(OrderNumber);
                    //}
                    //catch (Exception ex)
                    //{
                    //    WriteLog("Payment Gateway Exception" + ex.Message.ToString());
                    //}
                }
            }
            catch (Exception ex) { WriteLog(ex.Message.ToString()); }


        }

        //private DataSet GetOrderDetails(String RefOrderNo, Int32 Storeid)
        //{
        //    DataSet ds = new DataSet();
        //    ds.ReadXml(@"E:\YahooOrder.xml");
        //    return ds;
        //}

        /// <summary>
        /// Gets the Order Details from Live Yahoo Store with Download XML
        /// </summary>
        /// <param name="RefOrderNo">String RefOrderNo</param>
        /// <param name="Storeid">int Storeid</param>
        /// <returns>Returns the Yahoo Order Details as a Dataset</returns>
        private DataSet GetOrderDetails(String RefOrderNo, Int32 Storeid)
        {
            DataSet ds = new DataSet();
            if (RefOrderNo.Contains("-"))
            {
                string[] tmp = RefOrderNo.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                RefOrderNo = tmp[tmp.Length - 1];
            }

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            string yahoostoreid = objconfig.GetAppConfigByName("YahooStoreId", Storeid);
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            transactionCommand.Append("<ystorewsRequest>");
            //transactionCommand.Append("<StoreID> yhst-23452185929710 </StoreID>");
            transactionCommand.Append("<StoreID>" + yahoostoreid + "</StoreID>");


            transactionCommand.Append("<SecurityHeader>");
            //transactionCommand.Append("<PartnerStoreContractToken> 1.0_xRhnaOls_wFZK.Ks1lfDDqp545APU0ZOTu5l8blFvFeJS1qearrkUT.QDmPvnxDOpYr5wTNvlujjKPsoQxwArikZL6XUauyafjOIZCKIEB.sNJfiB8qs_ADeoUXz7SQjFLX1YZV_ </PartnerStoreContractToken></SecurityHeader>");
            transactionCommand.Append("<PartnerStoreContractToken>" + objconfig.GetAppConfigByName("YahooToken", Storeid) + "</PartnerStoreContractToken></SecurityHeader>");
            transactionCommand.Append("<Version> 1.0 </Version>");
            transactionCommand.Append("<Verb> get </Verb>");
            transactionCommand.Append("<ResourceList>");
            transactionCommand.Append("<OrderListQuery>");
            transactionCommand.Append("<Filter>");
            transactionCommand.Append("<Include> all </Include>");
            transactionCommand.Append("</Filter>");
            //transactionCommand.Append("<QueryParams>");
            //transactionCommand.Append("<IntervalRange>");
            //transactionCommand.Append("<Start> 19730 </Start>");
            //transactionCommand.Append("<End> 19738 </End>");
            //transactionCommand.Append("</IntervalRange>");
            //transactionCommand.Append("</QueryParams>");
            transactionCommand.Append("<QueryParams>");
            transactionCommand.Append("<OrderID> " + RefOrderNo + " </OrderID>");
            transactionCommand.Append("</QueryParams>");
            transactionCommand.Append("</OrderListQuery>");
            transactionCommand.Append("</ResourceList>");
            transactionCommand.Append("</ystorewsRequest>");
            byte[] data = encoding.GetBytes(transactionCommand.ToString());
            String AuthServer = "https://" + yahoostoreid + ".order.store.yahooapis.com/V1/order";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
            myRequest.Method = "POST";
            myRequest.Timeout = 300000;
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // get the response
            WebResponse myResponse;
            String rawResponseString = String.Empty;
            try
            {
                myResponse = myRequest.GetResponse();
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                {
                    rawResponseString = sr.ReadToEnd();
                    // Close and clean up the StreamReader

                    sr.Close();
                }

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rawResponseString);
                ds.ReadXml(new XmlNodeReader(xDoc));
                try
                {
                    if (!Directory.Exists(Server.MapPath("/YahooConfig/config")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/YahooConfig/config"));
                    }
                    ds.WriteXml(Server.MapPath("/YahooConfig/config/" + DateTime.Now.Ticks.ToString() + ".xml"));
                }
                catch { }
                myResponse.Close();

            }
            catch
            {
                rawResponseString = "0|||Error Calling AuthorizeNet Payment Gateway||||||||";

                return null;
            }

            return ds;
        }

        /// <summary>
        /// Adds the Product into Database
        /// </summary>
        /// <param name="strName">string strName</param>
        /// <param name="strSKU">string strSKU</param>
        /// <param name="strYahooID">string strYahooID</param>
        /// <param name="ProductURL">strng ProductURL</param>
        /// <param name="ImageName">string ImageName</param>
        /// <param name="decPrice">Decimal decPrice</param>
        /// <param name="iStoreID">int iStoreID</param>
        /// <returns>System.Int32.</returns>
        private int AddProduct(string strName, string strSKU, string strYahooID, string ProductURL, string ImageName, decimal decPrice, int iStoreID)
        {
            //tb_Product tb_product = new tb_Product();
            //tb_product.Name = strName;
            //tb_product.Description = strName;
            //tb_product.MainCategory = "";
            //if (strSKU != null)
            //{
            //    tb_product.SKU = strSKU;
            //}
            //else
            //{
            //    tb_product.SKU = strYahooID;
            //}

            //tb_product.YahooID = strYahooID;
            //tb_product.ProductURL = ProductURL.ToString();
            //tb_product.Weight = 1;
            //tb_product.Inventory = 999;
            //tb_product.SalePrice = decPrice;
            //tb_product.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", iStoreID);
            //tb_product.Active = false;
            //tb_product.Deleted = false;
            //tb_product.ImageName = ImageName;
            //int AddedProductID = ProductComponent.InsertProduct(tb_product);

            SQLAccess objsql = new SQLAccess();
            string strsqlproduct = "INSERT INTO  dbo.tb_Product(Name,Description,MainCategory,SKU,YahooID,ProductURL,Weight,Inventory,SalePrice,StoreID,Active,Deleted,ImageName) VALUES('" + strName.Replace("'", "''") + "','" + strName.Replace("'", "''") + "','','" + strSKU.Replace("'", "''") + "','" + strYahooID.Replace("'", "''") + "','" + ProductURL.Replace("'", "''") + "',1,999," + decPrice + "," + iStoreID + ",0,0,'" + ImageName.Replace("'", "''") + "'); SELECT SCOPE_IDENTITY();";
            int AddedProductID = Convert.ToInt32(objsql.ExecuteScalarQuery(strsqlproduct));
            try
            {
                if (AddedProductID > 0)
                {
                    if (ImageName.ToString().Trim().ToLower().IndexOf("http") > -1)
                    {
                        String strImageName = CommonOperations.RemoveSpecialCharacter(strSKU.ToString().ToCharArray()) + "_" + AddedProductID.ToString() + ".jpg";
                        System.Net.WebClient objClient = new System.Net.WebClient();
                        String strSavedImgPath = ProductTempPath + strImageName.ToString();
                        objClient.DownloadFile(ImageName.ToString(), Server.MapPath(strSavedImgPath));
                        if (File.Exists(Server.MapPath(strSavedImgPath)))
                        {
                            ViewState["File"] = strImageName.ToString();
                            SaveImage(strImageName.ToString(), strSavedImgPath);
                            bool isUpdate = objsql.ExecuteNonQuery("UPDATE dbo.tb_Product SET ImageName='" + strImageName + "' WHERE ProductID=" + AddedProductID + "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("Config.aspx => AddProduct() : Save Image " + ex.Message);
            }
            return AddedProductID;
        }

        /// <summary>
        /// Writes the Error log into .txt file
        /// </summary>
        /// <param name="LogString">String LogString</param>
        public void WriteLog(string LogString)
        {
            StreamWriter writer = null;
            try
            {
                if (!Directory.Exists(Server.MapPath("/YahooConfig/config")))
                {
                    Directory.CreateDirectory(Server.MapPath("/YahooConfig/config"));
                }

                FileInfo info = new FileInfo(HttpContext.Current.Server.MapPath("/YahooConfig/config/" + DateTime.Now.ToFileTimeUtc() + ".txt"));
                writer = info.AppendText();
                writer.Write(LogString);
            }
            catch { }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        #region Code for Resize Imageas and Save

        /// <summary>
        /// Bind Sizes
        /// </summary>
        private void BindSize(int StoreID)
        {
            DataSet dsIconWidth = objconfig.GetImageSizeByType(StoreID, "ProductIconWidth");
            DataSet dsIconHeight = objconfig.GetImageSizeByType(StoreID, "ProductIconHeight");
            if ((dsIconWidth != null && dsIconWidth.Tables.Count > 0 && dsIconWidth.Tables[0].Rows.Count > 0) && (dsIconHeight != null && dsIconHeight.Tables.Count > 0 && dsIconHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeIcon = new Size(Convert.ToInt32(dsIconWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsIconHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            DataSet dsLargeWidth = objconfig.GetImageSizeByType(StoreID, "ProductLargeWidth");
            DataSet dsLargeHeight = objconfig.GetImageSizeByType(StoreID, "ProductLargeHeight");
            if ((dsLargeWidth != null && dsLargeWidth.Tables.Count > 0 && dsLargeWidth.Tables[0].Rows.Count > 0) && (dsLargeHeight != null && dsLargeHeight.Tables.Count > 0 && dsLargeHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeLarge = new Size(Convert.ToInt32(dsLargeWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsLargeHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            DataSet dsMediumWidth = objconfig.GetImageSizeByType(StoreID, "ProductMediumWidth");
            DataSet dsMediumHeight = objconfig.GetImageSizeByType(StoreID, "ProductMediumHeight");
            if ((dsMediumWidth != null && dsMediumWidth.Tables.Count > 0 && dsMediumWidth.Tables[0].Rows.Count > 0) && (dsMediumHeight != null && dsMediumHeight.Tables.Count > 0 && dsMediumHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeMediam = new Size(Convert.ToInt32(dsMediumWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsMediumHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            DataSet dsMicroWidth = objconfig.GetImageSizeByType(StoreID, "ProductMicroWidth");
            DataSet dsMicroHeight = objconfig.GetImageSizeByType(StoreID, "ProductMicroHeight");
            if ((dsMicroWidth != null && dsMicroWidth.Tables.Count > 0 && dsMicroWidth.Tables[0].Rows.Count > 0) && (dsMicroHeight != null && dsMicroHeight.Tables.Count > 0 && dsMicroHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeMicro = new Size(Convert.ToInt32(dsMicroWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsMicroHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

        }

        /// <summary>
        /// Save Image
        /// </summary>
        /// <param name="FileName">string FileName</param>
        protected void SaveImage(string FileName, string SavedImgPath)
        {
            //String strImagePath = Convert.ToString(objdb.ExecuteScalarQuery("SELECT ISNULL(ConfigValue,'') AS ConfigValue FROM dbo.tb_AppConfig WHERE ConfigName='ImagePathProduct'AND StoreID=" + Convert.ToInt32(Request.QueryString["strid"]) + ""));
            //ProductTempPath = string.Concat(strImagePath, "Temp/");
            //ProductIconPath = string.Concat(strImagePath, "Icon/");
            //ProductMediumPath = string.Concat(strImagePath, "Medium/");
            //ProductLargePath = string.Concat(strImagePath, "Large/");
            //ProductMicroPath = string.Concat(strImagePath, "Micro/");


            //create icon folder 
            if (!Directory.Exists(Server.MapPath(ProductIconPath)))
                Directory.CreateDirectory(Server.MapPath(ProductIconPath));

            //create Medium folder 
            if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                Directory.CreateDirectory(Server.MapPath(ProductMediumPath));

            //create Large folder 
            if (!Directory.Exists(Server.MapPath(ProductLargePath)))
                Directory.CreateDirectory(Server.MapPath(ProductLargePath));

            //create Micro folder 
            if (!Directory.Exists(Server.MapPath(ProductMicroPath)))
                Directory.CreateDirectory(Server.MapPath(ProductMicroPath));

            CommonOperations.SaveOnContentServer(Server.MapPath(ProductIconPath));
            CommonOperations.SaveOnContentServer(Server.MapPath(ProductMediumPath));
            CommonOperations.SaveOnContentServer(Server.MapPath(ProductLargePath));
            CommonOperations.SaveOnContentServer(Server.MapPath(ProductMicroPath));

            try
            {
                CreateImage("Medium", FileName, SavedImgPath);
                CreateImage("Icon", FileName, SavedImgPath);
                CreateImage("Micro", FileName, SavedImgPath);
                CreateImage("Large", FileName, SavedImgPath);
            }
            catch (Exception ex)
            {
                // lblMsg.Text += "<br />" + ex.Message;
            }
            finally
            {
                DeleteTempFile("icon");
            }
        }

        /// <summary>
        /// Delete Temp files
        /// </summary>
        /// <param name="strsize">string strsize</param>
        protected void DeleteTempFile(string strsize)
        {
            try
            {
                if (strsize == "icon")
                {
                    string path = string.Empty;
                    if (ViewState["File"] != null && ViewState["File"].ToString().Trim().Length > 0)
                    {
                        path = Server.MapPath(ProductTempPath + ViewState["File"].ToString());
                    }

                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                WriteLog("Config.aspx => DeleteTempFile() " + ex.Message);
            }
        }

        /// <summary>
        ///  Create Image
        /// </summary>
        /// <param name="Size">string Size</param>
        /// <param name="FileName">string FileName</param>
        protected void CreateImage(string Size, string FileName, string SavedImgPath)
        {
            try
            {
                string strFile = null;
                strFile = Server.MapPath(SavedImgPath);
                string strFilePath = "";
                Size = Size.ToLower();
                switch (Size)
                {
                    case "large":
                        strFilePath = Server.MapPath(ProductLargePath + FileName);
                        break;

                    case "medium":
                        strFilePath = Server.MapPath(ProductMediumPath + FileName);
                        break;

                    case "icon":
                        strFilePath = Server.MapPath(ProductIconPath + FileName);
                        break;

                    case "micro":
                        strFilePath = Server.MapPath(ProductMicroPath + FileName);
                        break;
                }
                ResizePhoto(strFile, Size, strFilePath);
            }
            catch (Exception ex)
            {
                WriteLog("Config.aspx => CreateImage() " + ex.Message);
            }

        }

        /// <summary>
        /// Resize Photo
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="Size">string Size</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizePhoto(string strFile, string Size, string strFilePath)
        {
            switch (Size)
            {
                case "medium":
                    finHeight = thumbNailSizeMediam.Height;
                    finWidth = thumbNailSizeMediam.Width;
                    break;
                case "icon":
                    finHeight = thumbNailSizeIcon.Height;
                    finWidth = thumbNailSizeIcon.Width;
                    break;
                case "micro":
                    finHeight = thumbNailSizeMicro.Height;
                    finWidth = thumbNailSizeMicro.Width;
                    break;

            }
            if (Size == "large")
            {
                File.Copy(strFile, strFilePath, true);
                CommonOperations.SaveOnContentServer(strFilePath);
            }
            else
                ResizeImage(strFile, finWidth, finHeight, strFilePath);
        }

        /// <summary>
        /// Resize Images
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="FinWidth">int FinWidth</param>
        /// <param name="FinHeight">int FinHeight</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizeImage(string strFile, int FinWidth, int FinHeight, string strFilePath)
        {
            System.Drawing.Image imgVisol = System.Drawing.Image.FromFile(strFile);
            float resizePercent = 0;
            int resizedHeight = imgVisol.Height;
            int resizedWidth = imgVisol.Width;

            if (imgVisol.Height >= FinHeight && imgVisol.Width >= FinWidth)
            {
                float resizePercentHeight = 0;
                float resizePercentWidth = 0;
                resizePercentHeight = (FinHeight * 100) / imgVisol.Height;
                resizePercentWidth = (FinWidth * 100) / imgVisol.Width;
                if (resizePercentHeight < resizePercentWidth)
                {
                    resizedHeight = FinHeight;
                    resizedWidth = (int)Math.Round(resizePercentHeight * imgVisol.Width / 100.0);
                }
                if (resizePercentHeight >= resizePercentWidth)
                {
                    resizedWidth = FinWidth;
                    resizedHeight = (int)Math.Round(resizePercentWidth * imgVisol.Height / 100.0);
                }
            }
            else if (imgVisol.Width >= FinWidth && imgVisol.Height <= FinHeight)
            {
                resizedWidth = FinWidth;
                resizePercent = (FinWidth * 100) / imgVisol.Width;
                resizedHeight = (int)Math.Round((imgVisol.Height * resizePercent) / 100.0);
            }

            else if (imgVisol.Width <= FinWidth && imgVisol.Height >= FinHeight)
            {
                resizePercent = (FinHeight * 100) / imgVisol.Height;
                resizedHeight = FinHeight;
                resizedWidth = (int)Math.Round(resizePercent * imgVisol.Width / 100.0);
            }

            Bitmap resizedPhoto = new Bitmap(resizedWidth, resizedHeight, PixelFormat.Format24bppRgb);
            Graphics grPhoto = Graphics.FromImage(resizedPhoto);

            int destWidth = resizedWidth;
            int destHeight = resizedHeight;
            int sourceWidth = imgVisol.Width;
            int sourceHeight = imgVisol.Height;

            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle DestRect = new Rectangle(0, 0, destWidth, destHeight);
            Rectangle srcRect = new Rectangle(0, 0, sourceWidth, sourceHeight);
            grPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grPhoto.DrawImage(imgVisol, DestRect, srcRect, GraphicsUnit.Pixel);

            GenerateImage(resizedPhoto, strFilePath, FinWidth, FinHeight);

            resizedPhoto.Dispose();
            grPhoto.Dispose();
            imgVisol.Dispose();

        }

        /// <summary>
        /// Generate Image
        /// </summary>
        /// <param name="extBMP">Bitmap extBMP</param>
        /// <param name="DestFileName">string DestFileName</param>
        /// <param name="DefWidth">int DefWidth</param>
        /// <param name="DefHeight">int DefHeight</param>
        private void GenerateImage(Bitmap extBMP, string DestFileName, int DefWidth, int DefHeight)
        {
            System.Drawing.Imaging.Encoder Enc = System.Drawing.Imaging.Encoder.SaveFlag;
            EncoderParameters EncParms = new EncoderParameters(1);
            EncoderParameter EncParm;
            ImageCodecInfo CodecInfo = GetEncoderInfo("image/jpeg");
            EncParm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)600);
            EncParms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)600);

            if (extBMP != null && extBMP.Width < (DefWidth) && extBMP.Height < (DefHeight))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                int startY = (int)(DefHeight / 2) - (extBMP.Height / 2);
                g.DrawImage(extBMP, startX, startY);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null && extBMP.Width < (DefWidth))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                g.DrawImage(extBMP, startX, 0);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null && extBMP.Height < (DefHeight))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startY = (int)(DefHeight / 2) - (extBMP.Height / 2);
                g.DrawImage(extBMP, 0, startY);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null)
            {
                extBMP.Save(DestFileName, CodecInfo, EncParms);
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);
            }
        }

        /// <summary>
        /// Get Encoder Information
        /// </summary>
        /// <param name="resizeMimeType">string resizeMimeType</param>
        /// <returns>Returns the ImageCodecInfo Object</returns>
        private static ImageCodecInfo GetEncoderInfo(string resizeMimeType)
        {
            // Get image codec for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == resizeMimeType)
                    return codecs[i];
            return null;
        }
        #endregion

        public void SendMail(Int32 OrderNumber, string EmailAddress, int Storeid)
        {
            try
            {
                SQLAccess objSql = new SQLAccess();
                string ToID = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='MailMe_ToAddress' AND Storeid=" + Storeid + " AND isnull(Deleted,0)=0"));// AppLogic.AppConfigs("MailMe_ToAddress");
                string Body = "";
                string url = AppLogic.AppConfigs("LIVE_SERVER").ToString().ToLower().Replace("https", "http") + "/" + "Invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
                WebRequest NewWebReq = WebRequest.Create(url);
                WebResponse newWebRes = NewWebReq.GetResponse();
                string format = newWebRes.ContentType;
                Stream ftprespstrm = newWebRes.GetResponseStream();
                StreamReader reader;
                reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
                Body = reader.ReadToEnd().ToString();
                Body = Body.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"display:none;\"");
                Body = Body.Replace("title=\"Facebook\"", "title=\"Facebook\" style=\"display:none;\"");
                Body = Body.Replace("title=\"Twitter\"", "title=\"Twitter\" style=\"display:none;\"");
                Body = Body.Replace("title=\"Pinterest\"", "title=\"Pinterest\" style=\"display:none;\"");
                Body = Body.Replace("title=\"Google Plus\"", "title=\"Google Plus\" style=\"display:none;\"");
                Body = Body.Replace("id=\"trHeaderMenu\"", "id=\"trHeaderMenu\" style=\"display:none;\"");
                Body = Body.Replace("id=\"trStoreBanner\"", "id=\"trStoreBanner\" style=\"display:none;\"");
                Body = Body.Replace("id=\"footerLink\"", "id=\"footerLink\" style=\"display:none;\"");

                AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
                try
                {
                    CommonOperations.SendMail(ToID, "New Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                }
                catch { }
                try
                {
                    if (EmailAddress != "")
                    {
                        CommonOperations.SendMail(EmailAddress, "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                    }
                }
                catch { }
            }
            catch
            {
            }
        }
    }
}