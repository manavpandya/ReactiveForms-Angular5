using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text.RegularExpressions;
using System.Collections;
using MWSMerchantFulfillmentService;
using System.IO;
using Ionic.Zlib;
using System.Drawing;
using PdfSharp.Drawing;
using System.Drawing.Drawing2D;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class MultipleShippingPopupForLabel : BasePage
    {
        OrderComponent ObjOrder = null;
        DataSet DsOrder = new DataSet();
        int checkCount, GridrowCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["pdfnm"] != null)
                {
                    
                   // string strrrr = ResizeImagewithpdf("/ShippingLabels/FEDEX/temp", "/ShippingLabels/FEDEX/", "FedEx-Package1_789050748377_1158967_3209385@201712208322-1.png", "/ShippingLabels/FEDEX/");
                }
                //ViewState["AmazonMerchantID"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonMerchantID'"));
                //ViewState["AmazonServiceURL"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonServiceURL'"));
                //ViewState["AmazonAccessKey"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonAccessKey'"));
                //ViewState["AmazonSecretKey"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonSecretKey'"));
                //ViewState["AmazonApplicationName"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonApplicationName'"));
                //ViewState["AmazonDefaultMethod"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonDefaultMethod'"));

                //MWSMerchantFulfillmentService.Model.GetShipmentResponse objShippent = new MWSMerchantFulfillmentService.Model.GetShipmentResponse();

                 

                //string sellerId = ViewState["AmazonMerchantID"].ToString();
                //string mwsAuthToken = "";
                //// The client application version
                //string appVersion = "1.01";

                //// The endpoint for region service and version (see developer guide)
                //// ex: https://mws.amazonservices.com
                //string serviceURL = ViewState["AmazonServiceURL"].ToString();
                //MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
                //config.ServiceURL = serviceURL;
                //// Set other client connection configurations here if needed
                //// Create the client itself
                //MWSMerchantFulfillmentServiceClient client = new MWSMerchantFulfillmentServiceClient(ViewState["AmazonAccessKey"].ToString(), ViewState["AmazonSecretKey"].ToString(), ViewState["AmazonApplicationName"].ToString(), appVersion, config);
                //MWSMerchantFulfillmentServiceSample obj = new MWSMerchantFulfillmentServiceSample(client);

                //objShippent = obj.InvokeGetShipment(sellerId, "f1c46a64-ab19-4e1b-9a2b-eb5b2915212d");

                if (Request.QueryString["Ono"] != null && !string.IsNullOrEmpty(Request.QueryString["Ono"].ToString()))
                {

                    ViewState["AmazonMerchantID"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonMerchantID'"));
                    ViewState["AmazonServiceURL"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonServiceURL'"));
                    ViewState["AmazonAccessKey"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonAccessKey'"));
                    ViewState["AmazonSecretKey"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonSecretKey'"));
                    ViewState["AmazonApplicationName"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonApplicationName'"));
                    ViewState["AmazonDefaultMethod"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonDefaultMethod'"));
                    lblordernumer.Text = Request.QueryString["Ono"].ToString();
                    try
                    {
                        ObjOrder = new OrderComponent();
                        DsOrder = new DataSet();
                        DsOrder = ObjOrder.GetOrderDetailsByOrderID(Convert.ToInt32(Request.QueryString["Ono"]));

                        string ShipToAddress = string.Empty;
                        string ShipFromAddress = string.Empty;
                        if (DsOrder.Tables[0].Rows.Count > 0)
                        {
                            GetPersonalInfo(DsOrder);
                            lblordernumer.Text = lblordernumer.Text.ToString() + " (Amazon #: " + DsOrder.Tables[0].Rows[0]["ReforderId"].ToString() + ")";
                            BindShippingGrid(Convert.ToInt32(Request.QueryString["Ono"].ToString()));

                        }
                    }
                    catch { }
                }
            }

        }


        /// <summary>
        /// Get Personal Info
        /// </summary>
        /// <param name="Ds">DataSet ds</param>
        public void GetPersonalInfo(DataSet Ds)
        {
            //    clsAddress ObjAddress = new clsAddress();
            string st_b = null;

            AppConfig.StoreID = Convert.ToInt32(Ds.Tables[0].Rows[0]["Storeid"].ToString());

            ViewState["StoreID"] = Convert.ToInt32(Ds.Tables[0].Rows[0]["Storeid"].ToString());
            ViewState["CustomerID"] = Convert.ToInt32(Ds.Tables[0].Rows[0]["CustomerID"].ToString());

            //st_b += "<b>Shipping To Address</b><br />";

            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingFirstName"].ToString().Trim()))
                st_b += "<b>" + Convert.ToString(Ds.Tables[0].Rows[0]["ShippingFirstName"]) + " </b>";

            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingLastName"].ToString().Trim()))
                st_b += "<b>" + Convert.ToString(Ds.Tables[0].Rows[0]["ShippingLastName"]) + "</b><br />";


            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingCompany"].ToString().Trim()))
                st_b += Convert.ToString(Ds.Tables[0].Rows[0]["ShippingCompany"]) + "<br />";


            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingAddress1"].ToString().Trim()))
                st_b += Convert.ToString(Ds.Tables[0].Rows[0]["ShippingAddress1"]) + "<br />";

            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingAddress2"].ToString().Trim()))
                st_b += Convert.ToString(Ds.Tables[0].Rows[0]["ShippingAddress2"]) + "<br />";

            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingSuite"].ToString().Trim()))
                st_b += "Suite " + Convert.ToString(Ds.Tables[0].Rows[0]["ShippingSuite"]) + "<br/> ";

            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingCity"].ToString().Trim()))
                st_b += Convert.ToString(Ds.Tables[0].Rows[0]["ShippingCity"]) + ", ";

            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingState"].ToString().Trim()))
            {
                st_b += Convert.ToString(Ds.Tables[0].Rows[0]["ShippingState"]) + " ";

                ViewState["State"] = Ds.Tables[0].Rows[0]["ShippingState"].ToString();
            }
            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingZip"].ToString().Trim()))
            {
                st_b += Convert.ToString(Ds.Tables[0].Rows[0]["ShippingZip"]) + "<br />";

                if (Ds.Tables[0].Rows[0]["ShippingZip"].ToString().Contains("-"))
                {
                    ViewState["Zip"] = Ds.Tables[0].Rows[0]["ShippingZip"].ToString().Substring(0, Ds.Tables[0].Rows[0]["ShippingZip"].ToString().IndexOf("-"));
                }
                else
                {
                    ViewState["Zip"] = Ds.Tables[0].Rows[0]["ShippingZip"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingCountry"].ToString().Trim()))
            {
                //      st_b += ObjAddress.GetCountryByID(Convert.ToInt32(Convert.ToString(Ds.Tables[0].Rows[0]["ShippingCountry"]))) + "<br/> ";
                st_b += Convert.ToString(Ds.Tables[0].Rows[0]["ShippingCountry"]) + "<br/> ";

                CountryComponent objCountry = new CountryComponent();


                ViewState["Country"] = objCountry.GetCountryCodeByNameForShippingLabel(Convert.ToString(Ds.Tables[0].Rows[0]["ShippingCountry"]));

                ViewState["CountryName"] = Convert.ToString(Ds.Tables[0].Rows[0]["ShippingCountry"]);
                //        clsCountry objcountry = new clsCountry();
                //ViewState["Country"] = Convert.ToString(Ds.Tables[0].Rows[0]["ShippingCountry"]);
                //ViewState["CountryName"] = ObjAddress.GetCountryByID(Convert.ToInt32(Convert.ToString(Ds.Tables[0].Rows[0]["ShippingCountry"])));
            }
            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingPhone"].ToString().Trim()))
                st_b += Convert.ToString(Ds.Tables[0].Rows[0]["ShippingPhone"]) + "<br />";

            ltShippingTo.Text = st_b.ToString().Trim();

        }

        /// <summary>
        /// Set Image
        /// </summary>
        /// <param name="_Status">_Status</param>
        /// <returns>Image Path</returns>
        public string SetImage(Boolean _Status)
        {
            string _ReturnUrl = "";
            if (_Status == false)
            {
                _ReturnUrl = "/App_Themes/" + Page.Theme + "/Images/isInactive.png";

            }
            else
            {

                _ReturnUrl = "/App_Themes/" + Page.Theme + "/Images/isActive.png";

            }
            return _ReturnUrl;
        }


        protected void txtProWeight_TextChanged(object sender, EventArgs e)
        {

            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            TextBox txtProWeight = (TextBox)gvRow.FindControl("txtProWeight");
            TextBox txtHeightgrid = (TextBox)gvRow.FindControl("txtHeightgrid");
            TextBox txtWidthgrid = (TextBox)gvRow.FindControl("txtWidthgrid");
            TextBox txtShippedQty = (TextBox)gvRow.FindControl("txtShippedQty");
            TextBox txtLengthgrid = (TextBox)gvRow.FindControl("txtLengthgrid");
            Label lblOrderNumber = (Label)gvRow.FindControl("lblOrderNumber");
            Label lblCustomCartID = (Label)gvRow.FindControl("lblCustomCartID");
            DropDownList ddlShippingMethod = (DropDownList)gvRow.FindControl("ddlShippingMethod");
            DropDownList ddlWareHouse = (DropDownList)gvRow.FindControl("ddlWareHouse");
            DropDownList ddlDimensions = (DropDownList)gvRow.FindControl("ddlDimensions");

            // DropDownList ddlMailpieceShape = (DropDownList)gvRow.FindControl("ddlMailpieceShape");

            try
            {
                DropDownList ddlboxes = (DropDownList)gvRow.FindControl("ddlboxes");
                string[] strboxes = ddlboxes.SelectedValue.ToString().ToUpper().Split("X".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (strboxes.Length > 0)
                {
                    txtLengthgrid.Text = strboxes[0].ToString().Trim().Replace(" ", "");
                }
                if (strboxes.Length > 1)
                {
                    txtWidthgrid.Text = strboxes[1].ToString().Trim().Replace(" ", "");
                }
                if (strboxes.Length > 2)
                {
                    txtHeightgrid.Text = strboxes[2].ToString().Trim().Replace(" ", "");
                }

                decimal decHeight = 0, decLength = 0, decWidth = 0, decWeight = 0;
                Int32 OrderNumber = 0;
                OrderNumber = Convert.ToInt32(lblOrderNumber.Text);

                if (!string.IsNullOrEmpty(txtProWeight.Text.ToString()))
                    decWeight = Convert.ToDecimal(txtProWeight.Text.ToString());

                if (!string.IsNullOrEmpty(txtHeightgrid.Text.ToString()))
                    decHeight = Convert.ToDecimal(txtHeightgrid.Text);

                if (!string.IsNullOrEmpty(txtLengthgrid.Text.ToString()))
                    decLength = Convert.ToDecimal(txtLengthgrid.Text);

                if (!string.IsNullOrEmpty(txtWidthgrid.Text.ToString()))
                    decWidth = Convert.ToDecimal(txtWidthgrid.Text);

                BindShippingMethodcart(ddlShippingMethod, Convert.ToInt32(lblCustomCartID.Text), decWeight, decHeight, decWidth, decLength, ddlWareHouse, "", Convert.ToInt32(txtShippedQty.Text));
            }
            catch { }
        }

        private void BindShippingMethodRemaining(DropDownList ddlShippingMethod, Int32 OrderNumber, decimal decWeight, decimal decHeight, decimal decWidth, decimal decLength, DropDownList ddlWareHouse, string mailreceipe, Int32 Qty)
        {
            ddlShippingMethod.Items.Clear();
            string OrgShippingZip = "";
            string OrgCountry = "";
            string strUSPSMessage = "";
            string strUPSMessage = "";
            decimal decTotalWeight = 0;
            DataTable ShippingTable = new DataTable();//*//
            ShippingTable.Columns.Add("sn", typeof(int));
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            ShippingTable.Columns.Add("Shippingmethod", typeof(String));
            ShippingTable.Columns.Add("ShippingServiceId", typeof(String));
            ShippingTable.Columns.Add("ShippingOfferId", typeof(String));
            ShippingTable.Columns.Add("Shippingcarrier", typeof(String));

            ShippingComponent objShipping = new ShippingComponent();
            CountryComponent objCountry = new CountryComponent();

            decimal Weight = decimal.Zero;
            Weight = Convert.ToDecimal(decWeight);

            if (Weight == 0)
            {
                Weight = 1;
            }

            DataTable dtOrder = new DataTable();


            dtOrder.Columns.Add("Qty", typeof(int));
            dtOrder.Columns.Add("AmazonItemId", typeof(String));
            dtOrder.Columns.Add("RefOrderId", typeof(String));
            foreach (GridViewRow gvr in grdShipping.Rows)
            {

                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkgeneratelbl = (CheckBox)gvr.FindControl("chkgeneratelbl");
                    TextBox txtShippedQty = (TextBox)gvr.FindControl("txtShippedQty");
                    Label lblCustomCartID = (Label)gvr.FindControl("lblCustomCartID");
                    Label lblweight = (Label)gvr.FindControl("lblweight");
                    if (chkgeneratelbl.Checked == false && !string.IsNullOrEmpty(txtShippedQty.Text.ToString()) && txtShippedQty.Text.ToString() != "0")
                    {
                        DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE tb_OrderedShoppingCartItems.OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + "");
                        if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                            {
                                DataRow dataRow = dtOrder.NewRow();
                                dataRow["qty"] = txtShippedQty.Text.ToString();
                                dataRow["AmazonItemId"] = dtORderdetails.Tables[0].Rows[i]["OrderItemID"].ToString();
                                dataRow["RefOrderId"] = dtORderdetails.Tables[0].Rows[i]["RefOrderID"].ToString();
                                dtOrder.Rows.Add(dataRow);
                            }

                        }
                    }

                }

            }


            decTotalWeight = decWeight;
            string accessKey = ViewState["AmazonAccessKey"].ToString();

            // Developer AWS secret key
            string secretKey = ViewState["AmazonSecretKey"].ToString();

            // The client application name
            string appName = ViewState["AmazonApplicationName"].ToString();
            string sellerId = ViewState["AmazonMerchantID"].ToString();
            string mwsAuthToken = "";
            // The client application version
            string appVersion = "1.01";

            // The endpoint for region service and version (see developer guide)
            // ex: https://mws.amazonservices.com
            string serviceURL = ViewState["AmazonServiceURL"].ToString();
            MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
            config.ServiceURL = serviceURL;
            // Set other client connection configurations here if needed
            // Create the client itself
            MWSMerchantFulfillmentServiceClient client = new MWSMerchantFulfillmentServiceClient(accessKey, secretKey, appName, appVersion, config);
            MWSMerchantFulfillmentServiceSample obj = new MWSMerchantFulfillmentServiceSample(client);
            MWSMerchantFulfillmentService.Model.GetEligibleShippingServicesResponse res = new MWSMerchantFulfillmentService.Model.GetEligibleShippingServicesResponse();
            //res = obj.GetAllEligibleShippingServices(sellerId, "");
            res = obj.InvokeGetEligibleShippingServices(sellerId, "", Convert.ToDecimal(decLength.ToString()), Convert.ToDecimal(decWidth.ToString()), Convert.ToDecimal(decHeight.ToString()), decTotalWeight, dtOrder);
            for (int i = 0; i < res.GetEligibleShippingServicesResult.ShippingServiceList.Count(); i++)
            {
                string ShippingServiceId = res.GetEligibleShippingServicesResult.ShippingServiceList[i].ShippingServiceId;
                string ShippingServiceName = res.GetEligibleShippingServicesResult.ShippingServiceList[i].ShippingServiceName;
                string ShippingServiceOfferId = res.GetEligibleShippingServicesResult.ShippingServiceList[i].ShippingServiceOfferId;
                string Amount1 = res.GetEligibleShippingServicesResult.ShippingServiceList[i].Rate.Amount.ToString();

                DataRow dataRow = ShippingTable.NewRow();
                dataRow["sn"] = (i + 1).ToString();
                dataRow["Shippingmethod"] = ShippingServiceName;
                dataRow["ShippingMethodName"] = " " + ShippingServiceName + "($" + string.Format("{0:0.00}", Convert.ToDecimal(Amount1)) + ")";
                dataRow["ShippingServiceId"] = ShippingServiceId + "~" + ShippingServiceOfferId;
                dataRow["ShippingOfferId"] = ShippingServiceOfferId;
                dataRow["Price"] = Amount1;
                dataRow["Shippingcarrier"] = res.GetEligibleShippingServicesResult.ShippingServiceList[i].CarrierName.ToString();
                ShippingTable.Rows.Add(dataRow);
            }
            if (ShippingTable.Rows.Count > 0)
            {
                ShippingTable.DefaultView.Sort = "Shippingmethod asc,Price asc";
            }

            if (ShippingTable != null && ShippingTable.Rows.Count > 0)
            {
                System.Web.UI.WebControls.ListItem itemMethod = null;
                string strall = "";
                ddlShippingMethod.DataSource = ShippingTable;
                ddlShippingMethod.DataValueField = "ShippingServiceId";
                ddlShippingMethod.DataTextField = "ShippingMethodName";
                ddlShippingMethod.DataBind();
                Int32 items = 0;
                bool isselected = false;
                if (ViewState["AmazonDefaultMethod"] != null && !string.IsNullOrEmpty(ViewState["AmazonDefaultMethod"].ToString()))
                {
                    foreach (DataRow drMethod in ShippingTable.DefaultView.ToTable().Rows)
                    {
                        //if (ViewState["AmazonDefaultMethod"].ToString().ToLower().Trim() == drMethod["Shippingmethod"].ToString().ToLower().Trim())
                        //{
                        //    try
                        //    {
                        //        ddlShippingMethod.SelectedIndex = items;
                        //    }
                        //    catch
                        //    { }
                        //}
                        if (drMethod["Shippingmethod"].ToString().ToLower().Trim().IndexOf("fedex home delivery") > -1 && isselected == false)
                        {
                            isselected = true;
                            ddlShippingMethod.SelectedIndex = items;
                        }
                        else if (drMethod["Shippingmethod"].ToString().ToLower().IndexOf("ups ground") > -1 && isselected == false)
                        {
                            isselected = true;
                            ddlShippingMethod.SelectedIndex = items;
                        }
                        items++;
                        //itemMethod = new System.Web.UI.WebControls.ListItem(drMethod["ShippingMethodName"].ToString(), drMethod["ShippingServiceId"].ToString());

                    }
                }



            }
            else
            {
                ddlShippingMethod.DataSource = null;
                ddlShippingMethod.DataBind();
            }

        }
        private void BindShippingMethodcart(DropDownList ddlShippingMethod, Int32 OrderNumber, decimal decWeight, decimal decHeight, decimal decWidth, decimal decLength, DropDownList ddlWareHouse, string mailreceipe, Int32 Qty)
        {
            ddlShippingMethod.Items.Clear();
            string OrgShippingZip = "";
            string OrgCountry = "";
            string strUSPSMessage = "";
            string strUPSMessage = "";
            decimal decTotalWeight = 0;
            DataTable ShippingTable = new DataTable();//*//
            ShippingTable.Columns.Add("sn", typeof(int));
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            ShippingTable.Columns.Add("Shippingmethod", typeof(String));
            ShippingTable.Columns.Add("ShippingServiceId", typeof(String));
            ShippingTable.Columns.Add("ShippingOfferId", typeof(String));
            ShippingTable.Columns.Add("Shippingcarrier", typeof(String));

            ShippingComponent objShipping = new ShippingComponent();
            CountryComponent objCountry = new CountryComponent();

            decimal Weight = decimal.Zero;
            Weight = Convert.ToDecimal(decWeight);

            if (Weight == 0)
            {
                Weight = 1;
            }

            DataTable dtOrder = new DataTable();


            dtOrder.Columns.Add("Qty", typeof(int));
            dtOrder.Columns.Add("AmazonItemId", typeof(String));
            dtOrder.Columns.Add("RefOrderId", typeof(String));
            DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE tb_OrderedShoppingCartItems.OrderedCustomCartID=" + OrderNumber + "");
            if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                {
                    DataRow dataRow = dtOrder.NewRow();
                    dataRow["qty"] = Qty.ToString();
                    dataRow["AmazonItemId"] = dtORderdetails.Tables[0].Rows[i]["OrderItemID"].ToString();
                    dataRow["RefOrderId"] = dtORderdetails.Tables[0].Rows[i]["RefOrderID"].ToString();
                    dtOrder.Rows.Add(dataRow);
                }

            }

            decTotalWeight = decWeight;
            string accessKey = ViewState["AmazonAccessKey"].ToString();

            // Developer AWS secret key
            string secretKey = ViewState["AmazonSecretKey"].ToString();

            // The client application name
            string appName = ViewState["AmazonApplicationName"].ToString();
            string sellerId = ViewState["AmazonMerchantID"].ToString();
            string mwsAuthToken = "";
            // The client application version
            string appVersion = "1.01";

            // The endpoint for region service and version (see developer guide)
            // ex: https://mws.amazonservices.com
            string serviceURL = ViewState["AmazonServiceURL"].ToString();
            MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
            config.ServiceURL = serviceURL;
            // Set other client connection configurations here if needed
            // Create the client itself
            MWSMerchantFulfillmentServiceClient client = new MWSMerchantFulfillmentServiceClient(accessKey, secretKey, appName, appVersion, config);
            MWSMerchantFulfillmentServiceSample obj = new MWSMerchantFulfillmentServiceSample(client);
            MWSMerchantFulfillmentService.Model.GetEligibleShippingServicesResponse res = new MWSMerchantFulfillmentService.Model.GetEligibleShippingServicesResponse();
            //res = obj.GetAllEligibleShippingServices(sellerId, "");
            res = obj.InvokeGetEligibleShippingServices(sellerId, "", Convert.ToDecimal(decLength.ToString()), Convert.ToDecimal(decWidth.ToString()), Convert.ToDecimal(decHeight.ToString()), decTotalWeight, dtOrder);
            for (int i = 0; i < res.GetEligibleShippingServicesResult.ShippingServiceList.Count(); i++)
            {
                string ShippingServiceId = res.GetEligibleShippingServicesResult.ShippingServiceList[i].ShippingServiceId;
                string ShippingServiceName = res.GetEligibleShippingServicesResult.ShippingServiceList[i].ShippingServiceName;
                string ShippingServiceOfferId = res.GetEligibleShippingServicesResult.ShippingServiceList[i].ShippingServiceOfferId;
                string Amount1 = res.GetEligibleShippingServicesResult.ShippingServiceList[i].Rate.Amount.ToString();

                DataRow dataRow = ShippingTable.NewRow();
                dataRow["sn"] = (i + 1).ToString();
                dataRow["Shippingmethod"] = ShippingServiceName;
                dataRow["ShippingMethodName"] = " " + ShippingServiceName + "($" + string.Format("{0:0.00}", Convert.ToDecimal(Amount1)) + ")";
                dataRow["ShippingServiceId"] = ShippingServiceId + "~" + ShippingServiceOfferId;
                dataRow["ShippingOfferId"] = ShippingServiceOfferId;
                dataRow["Price"] = Amount1;
                dataRow["Shippingcarrier"] = res.GetEligibleShippingServicesResult.ShippingServiceList[i].CarrierName.ToString();
                ShippingTable.Rows.Add(dataRow);
            }
            if (ShippingTable.Rows.Count > 0)
            {
                ShippingTable.DefaultView.Sort = "Shippingmethod asc,Price asc";
            }

            if (ShippingTable != null && ShippingTable.Rows.Count > 0)
            {
                System.Web.UI.WebControls.ListItem itemMethod = null;
                string strall = "";
                ddlShippingMethod.DataSource = ShippingTable;
                ddlShippingMethod.DataValueField = "ShippingServiceId";
                ddlShippingMethod.DataTextField = "ShippingMethodName";
                ddlShippingMethod.DataBind();
                Int32 items = 0;
                bool isselected = false;

                if (ViewState["AmazonDefaultMethod"] != null && !string.IsNullOrEmpty(ViewState["AmazonDefaultMethod"].ToString()))
                {
                    foreach (DataRow drMethod in ShippingTable.DefaultView.ToTable().Rows)
                    {
                        if (drMethod["Shippingmethod"].ToString().ToLower().Trim().IndexOf("fedex home delivery") > -1 && isselected == false)
                        {
                            isselected = true;
                            ddlShippingMethod.SelectedIndex = items;
                        }
                        else if (drMethod["Shippingmethod"].ToString().ToLower().IndexOf("ups ground") > -1 && isselected == false)
                        {
                            isselected = true;
                            ddlShippingMethod.SelectedIndex = items;
                        }
                        //if (ViewState["AmazonDefaultMethod"].ToString().ToLower().Trim() == drMethod["Shippingmethod"].ToString().ToLower().Trim())
                        //{
                        //    try
                        //    {
                        //        ddlShippingMethod.SelectedIndex = items;
                        //    }
                        //    catch
                        //    { }
                        //}
                        items++;
                        //itemMethod = new System.Web.UI.WebControls.ListItem(drMethod["ShippingMethodName"].ToString(), drMethod["ShippingServiceId"].ToString());

                    }
                }



            }
            else
            {
                ddlShippingMethod.DataSource = null;
                ddlShippingMethod.DataBind();
            }

        }

        /// <summary>
        /// Bind Shipping Grid
        /// </summary>
        /// <param name="OrderNumber">OrderNumber</param>
        private void BindShippingGrid(int OrderNumber)
        {
            DataSet DsCItems = new DataSet();
            DsCItems = CommonComponent.GetCommonDataSet("SELECT Distinct  1 as Height,11 as Width,13 as Length,o.ShoppingCardID,o.RefOrderID,ISNULL(o.ShippingLabelMethod,'') as ShippingLabelMethod,ISNULL(o.ShippingLabelFileName,'') as ShippingLabelFileName,ISNULL(o.ShippingLabelCost,0) as ShippingLabelCost,o.OrderNumber as OrderNumber, isnull(o.DimensionValue,0) as DimensionValue,Isnull(tb_OrderedShoppingCartItems.Inventoryupdated,0) as Inventoryupdated,ISNULL(o.ShippingLabelWeight,0)as ShippingLabelWeight,ISNULL(o.ShippingLabelPackageHeight,0) as ShippingLabelPackageHeight,ISNULL(o.ShippingLabelPackageWidth,0) as ShippingLabelPackageWidth,ISNULL(o.ShippingLabelPackageLength,0) as ShippingLabelPackageLength, (isnull(tb_OrderedShoppingCartItems.WareHouseID,0)) as WareHouseID,case when isnull(s.ShippedQty,0) > 0 then isnull(s.ShippedQty,0) else isnull(tb_OrderedShoppingCartItems.ShippedQty,0) end as ShippedQty,tb_Product.Name,isnull(tb_OrderedShoppingCartItems.SKU,tb_Product.SKU) as SKU, tb_OrderedShoppingCartItems.Quantity,"
                                                        + "    tb_OrderedShoppingCartItems.OrderedCustomCartID, tb_OrderedShoppingCartItems.RefProductID,"
                                                        + "    tb_OrderedShoppingCartItems.VariantNames,tb_OrderedShoppingCartItems.VariantValues,"
                                                        + "    (isnull(s.TrackingNumber,tb_OrderedShoppingCartItems.TrackingNumber)) TrackingNumber,isnull(s.ShippedVia,tb_OrderedShoppingCartItems.ShippedVia) ShippedVia,"
                                                        + "   (isnull(s.Shipped,0)) as Shipped,isnull(s.Shipped,0) as ShippedProduct,"
                                                        + "    isnull(s.ShippedOn,tb_OrderedShoppingCartItems.ShippedOn) ShippedOn,  tb_Product.Description,tb_OrderedShoppingCartItems.Price As "
                                                        + "    SalePrice, isnull(s.ShippedNote,'') as ShippedNote,isnull(tb_OrderedShoppingCartItems.OrderItemID,'') as OrderItemID"
                                                        + "    FROM tb_Product INNER JOIN tb_OrderedShoppingCartItems"
                                                        + "    left outer join (select RefProductID,OrderNumber,case when isnull(tb_OrderShippedItems.AllTrackingNumber,'')<>'' then isnull(tb_OrderShippedItems.AllTrackingNumber,'') else   isnull(tb_OrderShippedItems.trackingNumber,'') end as trackingNumber,ShippedVia,Shipped,ShippedOn,case when (SELECT sum(cast(isnull(Items,'0') as int))  FROM dbo.Split(AllShippedQty,',')) > 0 then (SELECT sum(cast(isnull(Items,'0') as int))  FROM dbo.Split(AllShippedQty,',')) else ShippedQty end as ShippedQty,"
                                                        + "    ShippedNote from  tb_OrderShippedItems where OrderNumber=" + OrderNumber + ") s on s.RefProductID=tb_OrderedShoppingCartItems.RefProductID   "
                                                        + "    inner join (select convert(bit,isnull(len(ShippedOn),0)) as shipped,ShoppingCardID,ShippingTrackingNumber"
                                                        + "    as TrackingNumber, RefOrderID,ISNULL(ShippingLabelMethod,'') as ShippingLabelMethod,ISNULL(ShippingLabelFileName,'') as ShippingLabelFileName,ISNULL(ShippingLabelCost,0) as ShippingLabelCost, OrderNumber,DimensionValue,ISNULL(ShippingLabelWeight,0)as ShippingLabelWeight,ISNULL(ShippingLabelPackageHeight,0) as ShippingLabelPackageHeight,ISNULL(ShippingLabelPackageWidth,0) as ShippingLabelPackageWidth,ISNULL(ShippingLabelPackageLength,0) as ShippingLabelPackageLength,ShippedVia,ShippedOn,isnull(isNAVInserted,0) as isNAVInserted,isnull(isnavcompleted,0) as isnavcompleted from tb_Order ) o ON  o.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID "
                                                        + "    on tb_Product.ProductID = tb_OrderedShoppingCartItems.RefProductID Where isnull(o.isNAVInserted,0)=1 and isnull(o.isnavcompleted,0)=2 and OrderedShoppingCartID = (select ShoppingCardID FROM  tb_Order where OrderNumber=" + OrderNumber + ") ");
            if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
            {
                grdShipping.DataSource = DsCItems;
                grdShipping.DataBind();
                // trShippedItem.Visible = true;


            }
            else
            {
                grdShipping.DataSource = null;
                grdShipping.DataBind();
                //trShippedItem.Visible = false;
            }
            if (grdShipping.Rows.Count > 0)
            {
                Decimal weight = 0;
                bool tt = false;
                foreach (GridViewRow gvr in grdShipping.Rows)
                {

                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkgeneratelbl = (CheckBox)gvr.FindControl("chkgeneratelbl");
                        TextBox txtShippedQty = (TextBox)gvr.FindControl("txtShippedQty");
                        Label lblCustomCartID = (Label)gvr.FindControl("lblCustomCartID");
                        Label lblweight = (Label)gvr.FindControl("lblweight");
                        if (chkgeneratelbl.Checked == false && !string.IsNullOrEmpty(txtShippedQty.Text.ToString()) && txtShippedQty.Text.ToString() != "0")
                        {
                            weight += Convert.ToDecimal(lblweight.Text.ToString()) * Convert.ToDecimal(txtShippedQty.Text.ToString());
                            tt = true;
                        }

                    }

                }
                if (Convert.ToDecimal(weight) > Decimal.Zero)
                {
                    txtProWeightall.Text = weight.ToString();
                    allorderqty.Attributes.Add("style", "display:'';");
                    BindShippingMethodRemaining(ddlShippingMethodall, 0, weight, Convert.ToDecimal(txtHeightgridall.Text), Convert.ToDecimal(txtWidthgridall.Text), Convert.ToDecimal(txtLengthgridall.Text), null, "", Convert.ToInt32(0));

                }
                else if (tt == true)
                {
                    txtProWeightall.Text = weight.ToString();
                    allorderqty.Attributes.Add("style", "display:'';");


                }
                else
                {
                    allorderqty.Attributes.Add("style", "display:none;");
                }
            }
            else
            {
                allorderqty.Attributes.Add("style", "display:none;");
            }

            //btnItemShippingUpdate.Visible = true;
        }


        /// <summary>
        /// GridView Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdShipping_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Set image dynamically
                ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                ImageButton btnSave = (ImageButton)e.Row.FindControl("btnSave");
                ImageButton btnCancel = (ImageButton)e.Row.FindControl("btnCancel");
                //btnEdit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/edit-price.gif";
                btnEdit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/generate-label.gif";
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/save.png";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/CloseIcon.png";
                TextBox txtShippedOn = (TextBox)e.Row.FindControl("txtShippedOn2");
                txtShippedOn.Visible = false;
                Label lblCustomCartID = (Label)e.Row.FindControl("lblCustomCartID");
                TextBox txtTracking = (TextBox)e.Row.FindControl("txtTrackingNumber");
                Label lblTracking = (Label)e.Row.FindControl("lblTrackingNumber");

                DropDownList ddlCourier = (DropDownList)e.Row.FindControl("ddlShippedVIA");
                Label lblShippedVia = (Label)e.Row.FindControl("lblShippedVia");
                Label lblShippedQty = (Label)e.Row.FindControl("lblShippedQty");
                TextBox txtShippedQty = (TextBox)e.Row.FindControl("txtShippedQty");
                Label lblShippedOn = (Label)e.Row.FindControl("lblShippedOn");
                Label lblShippedNote = (Label)e.Row.FindControl("lblShippedNote");
                TextBox txtShippedNote = (TextBox)e.Row.FindControl("txtShippedNote");
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                Label lblQuantity = (Label)e.Row.FindControl("lblQuantity");
                Label lblQty = (Label)e.Row.FindControl("lblQty");
                Label lblOldQty = (Label)e.Row.FindControl("lblOldQty");

                Label lblavailQuantity = (Label)e.Row.FindControl("lblavailQuantity");
                Label lblInventoryupdated = (Label)e.Row.FindControl("lblInventoryupdated");
                ImageButton btnRebindShippingMethods = (ImageButton)e.Row.FindControl("btnRebindShippingMethods");
                btnRebindShippingMethods.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/refresh-icon.png";
                Label lblVariantNames = (Label)e.Row.FindControl("lblVariantNames");
                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                Label lblProductName = (Label)e.Row.FindControl("lblProductName");
                // RequiredFieldValidator reqDate = (RequiredFieldValidator)e.Row.FindControl("reqDate");
                //// reqDate.ID = reqDate +""+e.Row.RowIndex;
                // reqDate.ControlToValidate = txtShippedOn.ID + "_" + e.Row.RowIndex;
                // CompareValidator compDate = (CompareValidator)e.Row.FindControl("compDate");
                // compDate.ControlToValidate = txtShippedOn.ID + "_" + e.Row.RowIndex;

                Literal ltrCustomerShipping = (Literal)e.Row.FindControl("ltrCustomerShipping");
                Literal ltrTackingNo = (Literal)e.Row.FindControl("ltrTackingNo");
                TextBox txtHeightgrid = (TextBox)e.Row.FindControl("txtHeightgrid");
                TextBox txtWidthgrid = (TextBox)e.Row.FindControl("txtWidthgrid");
                TextBox txtLengthgrid = (TextBox)e.Row.FindControl("txtLengthgrid");
                TextBox txtProWeight = (TextBox)e.Row.FindControl("txtProWeight");
                Label lblOrderNumber = (Label)e.Row.FindControl("lblOrderNumber");
                Label lblGenerateLabelMsg = (Label)e.Row.FindControl("lblGenerateLabelMsg");
                Label lblDimensionName = (Label)e.Row.FindControl("lblDimensionName");
                
                DropDownList ddlShippingMethod = (DropDownList)e.Row.FindControl("ddlShippingMethod");
                DropDownList ddlWareHouse = (DropDownList)e.Row.FindControl("ddlWareHouse");
                Label lblorderNumber = (Label)e.Row.FindControl("lblorderNumber");
                Int32 OrderNumber = Convert.ToInt32(lblorderNumber.Text);
                Literal lblremaining = (Literal)e.Row.FindControl("lblremaining");
                CheckBox chkgeneratelbl = (CheckBox)e.Row.FindControl("chkgeneratelbl");
                DropDownList ddlboxes = (DropDownList)e.Row.FindControl("ddlboxes");
                chkgeneratelbl.Attributes.Add("onchange", "getWeightall();");
                lblOldQty.Text = Convert.ToString(CommonComponent.GetScalarCommonData("select Sum(isnull(ShipQty,0)) from tb_AmazonlabelDetails WHERE OrderedCustomcartId=" + lblCustomCartID.Text.ToString() + ""));
                DataSet dsAmazon = new DataSet();
                dsAmazon = CommonComponent.GetCommonDataSet("select * from tb_AmazonlabelDetails WHERE OrderedCustomcartId=" + lblCustomCartID.Text.ToString() + "");
                if (string.IsNullOrEmpty(lblOldQty.Text))
                {
                    lblOldQty.Text = "0";
                }
                if (Convert.ToInt32(lblOldQty.Text.ToString()) >= Convert.ToInt32(lblQty.Text.ToString()))
                {
                    txtShippedQty.Text = "0";
                    txtShippedQty.Visible = false;
                    btnEdit.Visible = false;
                    lblremaining.Text = "";
                    if (dsAmazon != null && dsAmazon.Tables.Count > 0 && dsAmazon.Tables[0].Rows.Count > 0)
                    {
                        ltrCustomerShipping.Text = dsAmazon.Tables[0].Rows[0]["ShippingLabelMethod"].ToString();
                        txtProWeight.Text = dsAmazon.Tables[0].Rows[0]["ShippingLabelWeight"].ToString();
                        lblDimensionName.Text = dsAmazon.Tables[0].Rows[0]["ShippingLabelPackageLength"].ToString() + " X " + dsAmazon.Tables[0].Rows[0]["ShippingLabelPackageWidth"].ToString() + " X " + dsAmazon.Tables[0].Rows[0]["ShippingLabelPackageHeight"].ToString();

                        lblDimensionName.Visible = true;

                    }
                    ddlboxes.Visible = false;
                }
                else
                {
                    txtShippedQty.Text = Convert.ToString(Convert.ToInt32(lblQty.Text.ToString()) - Convert.ToInt32(lblOldQty.Text.ToString()));
                    txtShippedQty.Visible = true;
                    btnEdit.Visible = true;
                    lblremaining.Text = "Remaining:";
                }



                if (lblProductName != null)
                {
                    System.Text.StringBuilder Table = new System.Text.StringBuilder();
                    Table.Append(lblProductName.Text.Trim());
                    string[] Names = lblVariantNames.Text.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] Values = lblVariantValues.Text.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    int iLoopValues = 0;
                    if (Names.Length == Values.Length)
                    {
                        for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                        {
                            Table.Append("<br/>&nbsp;" + Names[iLoopValues].ToString().Replace("Estimated Delivery", "Estimated Ship Date") + " : " + Values[iLoopValues]);
                        }
                    }
                    else if (Values.Length > 0)
                    {
                        for (iLoopValues = 0; iLoopValues < Values.Length; iLoopValues++)
                        {
                            Table.Append("<br/>&nbsp;- " + Values[iLoopValues]);
                        }
                    }
                    else if (Names.Length > 0)
                    {
                        for (iLoopValues = 0; iLoopValues < Names.Length; iLoopValues++)
                        {
                            Table.Append("<br/>&nbsp;- " + Names[iLoopValues].ToString().Replace("Estimated Delivery", "Estimated Ship Date"));
                        }
                    }
                    lblProductName.Text = Table.ToString();
                }

                btnRebindShippingMethods.Attributes.Add("onclick", "return Clacu(" + e.Row.RowIndex + ",'" + txtProWeight.ClientID.ToString() + "','" + btnRebindShippingMethods.ClientID.ToString() + "');");
                //btnSearchlabel.Attributes.Add("onclick", "return Clacu(" + e.Row.RowIndex + ",'" + txtProWeight.UniqueID.ToString() + "');");

                Label lblShippingMethod = (Label)e.Row.FindControl("lblShippingMethod");
                Label lblweight = (Label)e.Row.FindControl("lblweight");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnShippingLabelMethod = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnShippingLabelMethod");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnShippingLabelWeight = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnShippingLabelWeight");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnShippingLabelPackageHeight = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnShippingLabelPackageHeight");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnShippingLabelPackageWidth = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnShippingLabelPackageWidth");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnShippingLabelPackageLength = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnShippingLabelPackageLength");
                Label lblShippingLabelFileName = (Label)e.Row.FindControl("lblShippingLabelFileName");
                Label lblAmazonTrackingNo = (Label)e.Row.FindControl("lblAmazonTrackingNo");

                string StrQuery = "SElect distinct ISNULL(TrackingNumber,'') as TrackingNumber,ISNULL(ShippedVia,'') as ShippedVia from tb_OrderShippedItems where OrderNumber in (" + lblOrderNumber.Text.ToString() + ") AND isnull(TrackingNumber,'') <> ''";

                // divaddproductdetail.Visible = true;

                // // btnRemoveBatchOrder.Visible = true;
                // btnRemoveBatchOrder.Visible = false;
                txtProWeight.Attributes.Add("onchange", "return Clacu(" + e.Row.RowIndex + ",'" + txtProWeight.ClientID.ToString() + "','" + txtProWeight.ClientID.ToString() + "');");
                ddlShippingMethod.Visible = true;
                //btnSearchlabel.Visible = true;
                //bntDownUSPS.Visible = false;
                //btnRebindShippingMethods.Visible = true;
                lblShippingMethod.Text = "";


                string StrQueryProduct = "SELECT sum(A.Weight) as Weight,  Height, Length, Width,A.Weight as indweight  FROM (select  (SELECT isnull(tb_Product.Weight,0) FROM tb_Product INNER JOIN tb_ProductVariantValue on tb_Product.ProductID=tb_ProductVariantValue.ProductID WHERE tb_Product.StoreId=1 and isnull(tb_Product.Deleted,0)=0 and tb_ProductVariantValue.SKU=OSCI.SKU UNION SELECT isnull(tb_Product.Weight,0) FROM tb_Product WHERE tb_Product.StoreId=1 and isnull(tb_Product.Deleted,0)=0 and tb_Product.SKU=OSCI.SKU) as Weight,1 AS Height,13 as Length,11 as Width from tb_OrderedShoppingCartItems AS OSCI inner join tb_Order on tb_Order.ShoppingCardID=OSCI.OrderedShoppingCartID where OSCI.OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + ") AS A GROUP BY  Height, Length, Width,A.Weight";
                DataSet dsProduct = CommonComponent.GetCommonDataSet(StrQueryProduct);
                decimal decHeight = 0, decLength = 0, decWidth = 0, decWeight = 0;




                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["Height"].ToString()))
                        decHeight = Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["Height"].ToString());
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["Width"].ToString()))
                        decWidth = Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["Width"].ToString());
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["Length"].ToString()))
                        decLength = Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["Length"].ToString());

                    //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["Weight"].ToString()))
                    //    decWeight = Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["Weight"].ToString()) * Convert.ToDecimal(txtShippedQty.Text.ToString());
                    decWeight = 0;
                }


                string strAmazonlabelDetailsQuery = "select AmazonLabelGenerationID as ID, ISNULL(TrackingNumber,'') as AmazonTrackingNumber, isnull(ShippingLabelFileName,'') as ShippingLabelFileName from tb_AmazonlabelDetails  where OrderedCustomcartId = " + lblCustomCartID.Text.ToString() + "";
                DataSet dsAmazonProduct = CommonComponent.GetCommonDataSet(strAmazonlabelDetailsQuery);
                if (dsAmazonProduct != null && dsAmazonProduct.Tables.Count > 0 && dsAmazonProduct.Tables[0].Rows.Count > 0)
                {
                    string strtracking = "";
                    string LabelFPath = "";
                    for (int z = 0; z < dsAmazonProduct.Tables[0].Rows.Count; z++)
                    {

                        if (!string.IsNullOrEmpty(dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString()) && !string.IsNullOrEmpty(dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString()))
                        {
                            if (dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString().ToUpper().IndexOf("USPS") > -1)
                            {
                                LabelFPath = "/ShippingLabels/USPS/";// Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                            }
                            if (dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString().ToUpper().IndexOf("UPS") > -1)
                            {
                                LabelFPath = "/ShippingLabels/UPS/"; //Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                            }
                            if (dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString().ToUpper().IndexOf("FEDEX") > -1)
                            {
                                LabelFPath = "/ShippingLabels/FEDEX/"; //Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                            }
                           
                            strtracking += "<a href=\"" + LabelFPath.ToString() + "" + dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString() + "\"   style='color:#000 !important;' target=\"_blank\">" + (dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString()) + "</a>&nbsp;<a onclick=\"return DeleteTrackingNo('" + dsAmazonProduct.Tables[0].Rows[z]["ID"].ToString() + "','" + dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString() + "');\" href=\"javascript:void(0);\">X</a>,&nbsp;";
                        }

                        //if (!string.IsNullOrEmpty(dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString()))
                        //    strtracking += "<a href=\"" + dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString() + "\"   style='color:#000 !important;' target=\"_blank\">" + (dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString()) + "</a>&nbsp;<a onclick=\"return DeleteTrackingNo('" + dsAmazonProduct.Tables[0].Rows[z]["ID"].ToString() + "','" + dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString() + "');\" href=\"javascript:void(0);\">X</a>,&nbsp;";

                        //if (!string.IsNullOrEmpty(dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString()))
                        //    lblShippingLabelFileName.Text = (dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString());
                    }

                    lblAmazonTrackingNo.Text = strtracking.ToString();


                }


                string[] strboxes = ddlboxes.SelectedValue.ToString().ToUpper().Split("X".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (strboxes.Length > 0)
                {
                    decLength = Convert.ToDecimal(strboxes[0].ToString().Trim().Replace(" ", ""));
                }
                if (strboxes.Length > 1)
                {
                    decWidth = Convert.ToDecimal(strboxes[1].ToString().Trim().Replace(" ", ""));
                }
                if (strboxes.Length > 2)
                {
                    decHeight = Convert.ToDecimal(strboxes[2].ToString().Trim().Replace(" ", ""));
                }

                txtHeightgrid.Text = Convert.ToString(decHeight);
                txtWidthgrid.Text = Convert.ToString(decWidth);
                txtLengthgrid.Text = Convert.ToString(decLength);
                if (txtShippedQty.Visible == true)
                {
                    txtProWeight.Text = Convert.ToString(Math.Round(decWeight, 2));
                }

                lblweight.Text = "0.00";// Convert.ToString(Math.Round(Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["indweight"].ToString()), 2));
                if (Convert.ToInt32(lblQty.Text.ToString()) > Convert.ToInt32(lblOldQty.Text.ToString()))
                {
                    if (decWeight > Decimal.Zero)
                    {
                        BindShippingMethodcart(ddlShippingMethod, Convert.ToInt32(lblCustomCartID.Text.ToString()), decWeight, decHeight, decWidth, decLength, ddlWareHouse, "", Convert.ToInt32(txtShippedQty.Text.ToString()));
                    }

                    btnEdit.Visible = true;
                    ddlShippingMethod.Visible = true;
                    btnRebindShippingMethods.Visible = true;
                }
                else
                {
                    ddlShippingMethod.Visible = false;
                    btnEdit.Visible = false;
                    btnRebindShippingMethods.Visible = false;
                }






            }
        }

        public static byte[] Base64DecodeString(string inputStr)
        {
            byte[] decodedByteArray =
              Convert.FromBase64CharArray(inputStr.ToCharArray(),
                                            0, inputStr.Length);
            return (decodedByteArray);
        }

        /// <summary>
        /// GridView Row Copmmand Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdShipping_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandSource.GetType() != typeof(GridView))
                {
                    GridViewRow gvRow = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).NamingContainer);
                    if (e.CommandName == "RefreshShipping")
                    {
                        // TextBox txt = (TextBox)sender;

                        TextBox txtProWeight = (TextBox)gvRow.FindControl("txtProWeight");
                        TextBox txtHeightgrid = (TextBox)gvRow.FindControl("txtHeightgrid");
                        TextBox txtWidthgrid = (TextBox)gvRow.FindControl("txtWidthgrid");
                        TextBox txtShippedQty = (TextBox)gvRow.FindControl("txtShippedQty");
                        TextBox txtLengthgrid = (TextBox)gvRow.FindControl("txtLengthgrid");
                        Label lblOrderNumber = (Label)gvRow.FindControl("lblOrderNumber");
                        Label lblCustomCartID = (Label)gvRow.FindControl("lblCustomCartID");
                        DropDownList ddlShippingMethod = (DropDownList)gvRow.FindControl("ddlShippingMethod");
                        DropDownList ddlWareHouse = (DropDownList)gvRow.FindControl("ddlWareHouse");
                        DropDownList ddlDimensions = (DropDownList)gvRow.FindControl("ddlDimensions");
                        // DropDownList ddlMailpieceShape = (DropDownList)gvRow.FindControl("ddlMailpieceShape");

                        DropDownList ddlboxes = (DropDownList)gvRow.FindControl("ddlboxes");
                        string[] strboxes = ddlboxes.SelectedValue.ToString().ToUpper().Split("X".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (strboxes.Length > 0)
                        {
                            txtLengthgrid.Text = strboxes[0].ToString().Trim().Replace(" ", "");
                        }
                        if (strboxes.Length > 1)
                        {
                            txtWidthgrid.Text = strboxes[1].ToString().Trim().Replace(" ", "");
                        }
                        if (strboxes.Length > 2)
                        {
                            txtHeightgrid.Text = strboxes[2].ToString().Trim().Replace(" ", "");
                        }

                        try
                        {
                            decimal decHeight = 0, decLength = 0, decWidth = 0, decWeight = 0;
                            Int32 OrderNumber = 0;
                            OrderNumber = Convert.ToInt32(lblOrderNumber.Text);

                            if (!string.IsNullOrEmpty(txtProWeight.Text.ToString()))
                                decWeight = Convert.ToDecimal(txtProWeight.Text.ToString());

                            if (!string.IsNullOrEmpty(txtHeightgrid.Text.ToString()))
                                decHeight = Convert.ToDecimal(txtHeightgrid.Text);

                            if (!string.IsNullOrEmpty(txtLengthgrid.Text.ToString()))
                                decLength = Convert.ToDecimal(txtLengthgrid.Text);

                            if (!string.IsNullOrEmpty(txtWidthgrid.Text.ToString()))
                                decWidth = Convert.ToDecimal(txtWidthgrid.Text);

                            BindShippingMethodcart(ddlShippingMethod, Convert.ToInt32(lblCustomCartID.Text), decWeight, decHeight, decWidth, decLength, ddlWareHouse, "", Convert.ToInt32(txtShippedQty.Text));
                        }
                        catch { }

                        if (grdShipping.Rows.Count > 0)
                        {
                            Decimal weight = 0;
                            bool tt = false;
                            foreach (GridViewRow gvr in grdShipping.Rows)
                            {

                                if (gvr.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox chkgeneratelbl = (CheckBox)gvr.FindControl("chkgeneratelbl");
                                    TextBox txtShippedQty1 = (TextBox)gvr.FindControl("txtShippedQty");
                                    Label lblCustomCartID1 = (Label)gvr.FindControl("lblCustomCartID");
                                    Label lblweight = (Label)gvr.FindControl("lblweight");
                                    if (chkgeneratelbl.Checked == false && !string.IsNullOrEmpty(txtShippedQty1.Text.ToString()) && txtShippedQty1.Text.ToString() != "0")
                                    {
                                        weight += Convert.ToDecimal(lblweight.Text.ToString()) * Convert.ToDecimal(txtShippedQty1.Text.ToString());
                                        tt = true;
                                    }

                                }

                            }
                            if (Convert.ToDecimal(weight) > Decimal.Zero)
                            {
                                txtProWeightall.Text = weight.ToString();
                                allorderqty.Attributes.Add("style", "display:'';");
                                BindShippingMethodRemaining(ddlShippingMethodall, 0, weight, Convert.ToDecimal(txtHeightgridall.Text), Convert.ToDecimal(txtWidthgridall.Text), Convert.ToDecimal(txtLengthgridall.Text), null, "", Convert.ToInt32(0));

                            }
                            else if (tt == true)
                            {
                                txtProWeightall.Text = weight.ToString();
                                allorderqty.Attributes.Add("style", "display:'';");


                            }
                            else
                            {
                                allorderqty.Attributes.Add("style", "display:none;");
                            }
                        }
                        else
                        {
                            allorderqty.Attributes.Add("style", "display:none;");
                        }

                    }
                    else if (e.CommandName == "CustomEdit")
                    {
                        //TextBox txt = (TextBox)sender;

                        TextBox txtProWeight = (TextBox)gvRow.FindControl("txtProWeight");
                        TextBox txtHeightgrid = (TextBox)gvRow.FindControl("txtHeightgrid");
                        TextBox txtWidthgrid = (TextBox)gvRow.FindControl("txtWidthgrid");
                        TextBox txtShippedQty = (TextBox)gvRow.FindControl("txtShippedQty");
                        TextBox txtLengthgrid = (TextBox)gvRow.FindControl("txtLengthgrid");
                        Label lblOrderNumber = (Label)gvRow.FindControl("lblOrderNumber");
                        Label lblCustomCartID = (Label)gvRow.FindControl("lblCustomCartID");
                        Label lblQty = (Label)gvRow.FindControl("lblQty");
                        DropDownList ddlShippingMethod = (DropDownList)gvRow.FindControl("ddlShippingMethod");
                        DropDownList ddlWareHouse = (DropDownList)gvRow.FindControl("ddlWareHouse");
                        DropDownList ddlDimensions = (DropDownList)gvRow.FindControl("ddlDimensions");
                        CheckBox chkgeneratelbl = (CheckBox)gvRow.FindControl("chkgeneratelbl");


                        DropDownList ddlboxes = (DropDownList)gvRow.FindControl("ddlboxes");
                        string[] strboxes = ddlboxes.SelectedValue.ToString().ToUpper().Split("X".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (strboxes.Length > 0)
                        {
                            txtLengthgrid.Text = strboxes[0].ToString().Trim().Replace(" ", "");
                        }
                        if (strboxes.Length > 1)
                        {
                            txtWidthgrid.Text = strboxes[1].ToString().Trim().Replace(" ", "");
                        }
                        if (strboxes.Length > 2)
                        {
                            txtHeightgrid.Text = strboxes[2].ToString().Trim().Replace(" ", "");
                        }

                        decimal decHeight = 0, decLength = 0, decWidth = 0, decWeight = 0;
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnRefOrderNo = (System.Web.UI.HtmlControls.HtmlInputHidden)gvRow.FindControl("hdnRefOrderNo");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnshoppingcartid = (System.Web.UI.HtmlControls.HtmlInputHidden)gvRow.FindControl("hdnshoppingcartid");
                        if (!string.IsNullOrEmpty(txtProWeight.Text.ToString()))
                            decWeight = Convert.ToDecimal(txtProWeight.Text.ToString());

                        if (!string.IsNullOrEmpty(txtHeightgrid.Text.ToString()))
                            decHeight = Convert.ToDecimal(txtHeightgrid.Text);

                        if (!string.IsNullOrEmpty(txtLengthgrid.Text.ToString()))
                            decLength = Convert.ToDecimal(txtLengthgrid.Text);

                        if (!string.IsNullOrEmpty(txtWidthgrid.Text.ToString()))
                            decWidth = Convert.ToDecimal(txtWidthgrid.Text);
                        int chkQty = 0;
                        string ShippingLabelFileName = "";
                        string OrderNumber = Convert.ToString(lblOrderNumber.Text);
                        string ShipmentId = "";

                        Int32 maxqty = 0, qty = 0;
                        Int32.TryParse(lblQty.Text, out maxqty);
                        Int32.TryParse(txtShippedQty.Text, out qty);
                        //if (qty <= 0)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "msgvalid01", "alert('Please enter valid Quantity.');", true);
                        //    txtShippedQty.Focus();
                        //    return;
                        //}
                        //else if ((qty) > maxqty)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "msgvalid02", "alert('Please enter " + maxqty + " Quantity or less.');", true);
                        //    txtShippedQty.Focus();
                        //    return;
                        //}

                        if (!string.IsNullOrEmpty(OrderNumber.ToString()))
                        {
                            if (!string.IsNullOrEmpty(ddlShippingMethod.SelectedItem.Text) && chkgeneratelbl != null && chkgeneratelbl.Checked == true)
                            {
                                CountryComponent objCountry = new CountryComponent();
                                if (ddlShippingMethod.SelectedItem != null)
                                {
                                    //MWSMerchantFulfillmentService.Model.GetShipmentResponse objShippent = new MWSMerchantFulfillmentService.Model.GetShipmentResponse();
                                    MWSMerchantFulfillmentService.Model.Shipment objShippent = new MWSMerchantFulfillmentService.Model.Shipment();
                                    MWSMerchantFulfillmentService.Model.CreateShipmentResponse objlabel = new MWSMerchantFulfillmentService.Model.CreateShipmentResponse();
                                    if (!string.IsNullOrEmpty(ddlShippingMethod.SelectedItem.Text))
                                    {
                                        string sellerId = ViewState["AmazonMerchantID"].ToString();
                                        string mwsAuthToken = "";
                                        // The client application version
                                        string appVersion = "1.01";

                                        // The endpoint for region service and version (see developer guide)
                                        // ex: https://mws.amazonservices.com
                                        string serviceURL = ViewState["AmazonServiceURL"].ToString();
                                        MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
                                        config.ServiceURL = serviceURL;
                                        // Set other client connection configurations here if needed
                                        // Create the client itself
                                        MWSMerchantFulfillmentServiceClient client = new MWSMerchantFulfillmentServiceClient(ViewState["AmazonAccessKey"].ToString(), ViewState["AmazonSecretKey"].ToString(), ViewState["AmazonApplicationName"].ToString(), appVersion, config);
                                        MWSMerchantFulfillmentServiceSample obj = new MWSMerchantFulfillmentServiceSample(client);

                                        DataTable dtOrder = new DataTable();
                                        dtOrder.Columns.Add("Qty", typeof(int));
                                        dtOrder.Columns.Add("AmazonItemId", typeof(String));
                                        dtOrder.Columns.Add("RefOrderId", typeof(String));
                                        DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE OrderedCustomCartID=" + lblCustomCartID.Text + "");
                                        if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                                            {
                                                DataRow dataRow = dtOrder.NewRow();
                                                dataRow["qty"] = txtShippedQty.Text.ToString();
                                                dataRow["AmazonItemId"] = dtORderdetails.Tables[0].Rows[i]["OrderItemID"].ToString();
                                                dataRow["RefOrderId"] = dtORderdetails.Tables[0].Rows[i]["RefOrderID"].ToString();
                                                dtOrder.Rows.Add(dataRow);
                                            }
                                        }

                                        string[] strlids = ddlShippingMethod.SelectedValue.ToString().Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        objlabel = obj.InvokeCreateShipment(hdnRefOrderNo.Value.ToString(), sellerId, "", strlids[0].ToString(), strlids[1].ToString(), decLength, decWidth, decHeight, decWeight, dtOrder);

                                        objShippent = objlabel.CreateShipmentResult.Shipment;
                                        if (objShippent == null)
                                        {
                                            return;
                                        }
                                        // objShippent = obj.InvokeGetShipment(sellerId, "537d8635-c5fc-4aa8-aa35-a9af0a80ab78");
                                        string strddd = objlabel.CreateShipmentResult.Shipment.Label.FileContents.Contents.ToString();
                                        //objShippent = obj.InvokeGetShipment(sellerId, "537d8635-c5fc-4aa8-aa35-a9af0a80ab78");
                                        //string strddd = objShippent.GetShipmentResult.Shipment.Label.FileContents.Contents.ToString();

                                        byte[] imageBytes = Base64DecodeString(strddd);
                                        string fileName = objlabel.CreateShipmentResult.Shipment.TrackingId.ToString();
                                        ShipmentId = objlabel.CreateShipmentResult.Shipment.ShipmentId.ToString();
                                        CommonComponent.ExecuteCommonData("Update tb_Order set ShipmentId='" + ShipmentId + "'  where  OrderNumber=" + OrderNumber + "");
                                        // string fileName = objShippent.GetShipmentResult.Shipment.TrackingId.ToString();
                                        string imagepath = "";
                                        imagepath = "/ShippingLabels/FEDEX/";// AppLogic.AppConfigs("FedEx.LabelSavePath");
                                        string fileFormat = AppLogic.AppConfigs("Shipping.LabelFormat").ToString();
                                        if (ddlShippingMethod.SelectedValue.ToString().ToUpper().IndexOf("USPS") > -1)
                                        {
                                            imagepath = "/ShippingLabels/USPS/";// Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                                            fileName = "USPS-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.Value.ToString())) ? "0" : hdnshoppingcartid.Value.ToString()) + "@" +
                             DateTime.Now.Year.ToString() +
                             DateTime.Now.Month.ToString() +
                             DateTime.Now.Day.ToString() +
                             DateTime.Now.Hour.ToString() +
                             DateTime.Now.Minute.ToString() +
                             DateTime.Now.Second.ToString() + "-1." +
                             fileFormat.ToLower();
                                        }
                                        else if (ddlShippingMethod.SelectedValue.ToString().ToUpper().IndexOf("UPS") > -1)
                                        {
                                            imagepath = "/ShippingLabels/UPS/";//Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                                            fileName = "UPS-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.Value.ToString())) ? "0" : hdnshoppingcartid.Value.ToString()) + "@" +
                                DateTime.Now.Year.ToString() +
                                DateTime.Now.Month.ToString() +
                                DateTime.Now.Day.ToString() +
                                DateTime.Now.Hour.ToString() +
                                DateTime.Now.Minute.ToString() +
                                DateTime.Now.Second.ToString() + "-1." +
                                fileFormat.ToLower();
                                        }
                                        else if (ddlShippingMethod.SelectedValue.ToString().ToUpper().IndexOf("FEDEX") > -1)
                                        {
                                            imagepath = "/ShippingLabels/FEDEX/";//Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                                            fileName = "FedEx-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.Value.ToString())) ? "0" : hdnshoppingcartid.Value.ToString()) + "@" +
                                        DateTime.Now.Year.ToString() +
                                        DateTime.Now.Month.ToString() +
                                        DateTime.Now.Day.ToString() +
                                        DateTime.Now.Hour.ToString() +
                                        DateTime.Now.Minute.ToString() +
                                        DateTime.Now.Second.ToString() + "-1." + fileFormat.ToLower();
                                        }

                                        ShippingLabelFileName = fileName;
                                        FileStream fstrm = new FileStream(Server.MapPath(imagepath + fileName.Replace("." + fileFormat.ToLower(), ".gzip")), FileMode.CreateNew, FileAccess.Write);
                                        BinaryWriter writer = new BinaryWriter(fstrm);
                                        writer.Write(imageBytes);
                                        writer.Close();
                                        fstrm.Close();

                                        using (FileStream fInStream = new FileStream(Server.MapPath(imagepath + fileName.Replace("." + fileFormat.ToLower(), ".gzip")),
                            FileMode.Open, FileAccess.Read))
                                        {
                                            using (GZipStream zipStream = new GZipStream(fInStream, CompressionMode.Decompress))
                                            {
                                                using (FileStream fOutStream = new FileStream(Server.MapPath(imagepath + "temp/" + fileName),
                                                FileMode.Create, FileAccess.Write))
                                                {
                                                    byte[] tempBytes = new byte[4096];
                                                    int i;
                                                    while ((i = zipStream.Read(tempBytes, 0, tempBytes.Length)) != 0)
                                                    {
                                                        fOutStream.Write(tempBytes, 0, i);
                                                    }
                                                }
                                            }
                                        }
                                        if (fileName.ToString().ToLower().IndexOf(".png") > -1)
                                        {
                                            ResizeImagewithpdf(imagepath + "temp", imagepath, fileName, imagepath);
                                        }
                                        else
                                        {
                                            System.IO.File.Copy(imagepath + "temp/" + fileName, imagepath + fileName);
                                        }

                                    }
                                    else
                                    {
                                        Session["ProductDetail"] = null;
                                        return;
                                    }
                                    decimal ShippingCost = 0;
                                    if (ddlShippingMethod.SelectedValue.ToString() != "")
                                    {
                                        int Index = ddlShippingMethod.SelectedItem.Text.IndexOf("($");
                                        int Length = ddlShippingMethod.SelectedItem.Text.LastIndexOf(")") - Index;
                                        if (Index != -1 && Length != 0)
                                        {
                                            string strShippingCost = ddlShippingMethod.SelectedItem.Text.Substring(Index + 2, Length - 2).Trim();
                                            Decimal.TryParse(strShippingCost, out ShippingCost);
                                        }
                                    }

                                    if (string.IsNullOrEmpty(ShippingLabelFileName) || ShippingLabelFileName.ToString().Trim() == "")
                                    {
                                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg", "jAlert('Error While Generating Shipping Label.','Message');", true);
                                        Session["ProductDetail"] = null;
                                        return;
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(ShippingLabelFileName.Trim())) && Convert.ToString(ShippingLabelFileName.Trim()).ToLower().IndexOf("error :") > -1)
                                    {
                                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg", "jAlert('" + ShippingLabelFileName.ToString() + "','Message');", true);
                                        Session["ProductDetail"] = null;
                                        return;
                                    }
                                    if (!string.IsNullOrEmpty(ShippingLabelFileName.ToString()))
                                    {
                                        string StrShipping = Convert.ToString(ddlShippingMethod.SelectedItem.Text);
                                        if (!string.IsNullOrEmpty(StrShipping.ToString().Trim()) && StrShipping.ToLower().IndexOf("($") > -1)
                                        {
                                            StrShipping = StrShipping.Substring(0, StrShipping.IndexOf("($"));
                                        }
                                        string strdimension = Convert.ToString(Convert.ToString(decHeight) + " x " + Convert.ToString(decWidth) + " x " + Convert.ToString(decLength));

                                        CommonComponent.ExecuteCommonData("Update tb_Order set IsBackendProcessed=1,ShipmentId='" + ShipmentId + "', ShippingDimension ='" + strdimension + "', ShippingLabelFileName='" + ShippingLabelFileName.ToString().ToLower().Replace(".png", ".pdf") + "',ShippingLabelMethod='" + ddlShippingMethod.SelectedItem.Text + "',ShippingLabelCost=" + ShippingCost + ",ShippingLabelWeight=" + decWeight + ",ShippingLabelPackageHeight=" + decHeight + ",ShippingLabelPackageWidth=" + decWidth + ",ShippingLabelPackageLength=" + decLength + "  where  OrderNumber=" + OrderNumber + "");
                                        DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID,SKU FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + "");
                                        if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                                            {
                                                CommonComponent.ExecuteCommonData("EXEC usp_MicrosoftNav_PackageHeader " + OrderNumber + ",'" + objlabel.CreateShipmentResult.Shipment.TrackingId.ToString() + "','" + objlabel.CreateShipmentResult.Shipment.ShippingService.CarrierName.ToString() + "','" + objlabel.CreateShipmentResult.Shipment.ShippingService.ShipDate.ToString() + "','0','" + dtORderdetails.Tables[0].Rows[i]["SKU"].ToString() + "'," + txtShippedQty.Text.ToString() + "");

                                            }
                                        }
                                        CommonComponent.ExecuteCommonData("INSERT INTO tb_AmazonlabelDetails(OrderNumber, OrderedCustomcartId, ShipmentId, ShippingDimension, ShippingLabelFileName, ShippingLabelMethod, ShippingLabelCost, ShippingLabelWeight, ShippingLabelPackageHeight, ShippingLabelPackageWidth, ShippingLabelPackageLength, ShipQty, TrackingNumber, CreatedOn) SELECT OrderNumber, OrderedCustomcartId, '" + ShipmentId + "', '" + strdimension + "', '" + ShippingLabelFileName.ToString().ToLower().Replace(".png", ".pdf") + "', '" + ddlShippingMethod.SelectedItem.Text + "', " + ShippingCost + ", " + decWeight + ", " + decHeight + ", " + decWidth + ", " + decLength + ", " + txtShippedQty.Text + ", '" + objlabel.CreateShipmentResult.Shipment.TrackingId.ToString() + "', getdate()  FROM tb_OrderedShoppingCartItems INNER JOIN tb_Order on tb_Order.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID WHERE OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + "");
                                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@UpdateMsg", "jAlert('Shipping Label Generated Successfully.','Success');", true);

                                        BindShippingGrid(Convert.ToInt32(Request.QueryString["Ono"].ToString()));

                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch
            {
            }


        }

        /// <summary>
        /// Resize Image with PDF
        /// </summary>
        /// <param name="Orgpath"></param>
        /// <param name="destpath"></param>
        /// <param name="fileName"></param>
        /// <param name="PDFPath"></param>
        /// <returns></returns>
        public String ResizeImagewithpdf(string Orgpath, string destpath, string fileName, string PDFPath)
        {
            string strReturn = "";
            try
            {
                if (File.Exists(Server.MapPath(Orgpath) + "/" + fileName))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(Orgpath) + "/" + fileName))
                    {

                        Bitmap newBMP = new Bitmap(1200, 1800);
                        newBMP.SetResolution(300, 300);
                        Graphics g = Graphics.FromImage(newBMP);
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 1200, 1800);
                        g.DrawImage(img, rect);
                        g.Dispose();
                        img.Dispose();
                        newBMP.Save(Server.MapPath(destpath) + fileName);
                        try
                        {
                            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
                            PdfSharp.Pdf.PdfPage PFNEW = new PdfSharp.Pdf.PdfPage();
                            PFNEW.Size = PdfSharp.PageSize.RA5;
                            XUnit xy = new XUnit(435F);
                            XUnit xy1 = new XUnit(290F);
                            PFNEW.Height = xy;
                            PFNEW.Width = xy1;
                            doc.Pages.Add(PFNEW);
                            XGraphics xgr = XGraphics.FromPdfPage(PFNEW);
                            FileInfo fl = new FileInfo(Server.MapPath(PDFPath) + fileName);
                            string strname = fl.FullName.Replace(fl.Name.ToString(), "");
                            XImage img1 = XImage.FromFile(Server.MapPath(destpath) + fileName);
                            xgr.DrawImage(img1, 0, 0);
                            doc.Save(fl.FullName.Replace(fl.Extension.ToString(), ".pdf"));

                            strReturn = fl.FullName.Replace(fl.Extension.ToString(), ".pdf");
                            img1.Dispose();
                            doc.Close();


                        }
                        catch { }

                    }
                }
            }
            catch { }
            return strReturn;
        }

        /// <summary>
        /// Set Short Date
        /// </summary>
        /// <param name="Date">Date</param>
        /// <returns>Short Date String</returns>
        public string SetShortDate(string Date)
        {
            try
            {
                DateTime date = Convert.ToDateTime(Date);
                return date.ToShortDateString();
            }
            catch (Exception ex)
            {
                ///CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->SetShortDate() \r\n Date: " + System.DateTime.Now + "\r\n");
                return string.Empty;
            }

        }

        protected void btnAmazonRemove_Click(object sender, EventArgs e)
        {

            DataSet allshipping = CommonComponent.GetCommonDataSet("SELECT * FROM tb_OrderShippedItems WHERE isnull(AllTrackingNumber,'')<>'' and isnull(AllTrackingNumber,'') like '%" + hdnAmazonTrackinNo.Value.ToString() + "%' and OrderNumber in (SELECT OrderNumber FROM tb_AmazonlabelDetails WHERE AmazonLabelGenerationID=" + hdnAmazonCartID.Value + ")");

            if (allshipping != null && allshipping.Tables.Count > 0 && allshipping.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < allshipping.Tables[0].Rows.Count; k++)
                {
                    string qty = "";
                    string tracking = "";

                    string strallqty = allshipping.Tables[0].Rows[k]["AllShippedQty"].ToString();
                    string stralltracking = allshipping.Tables[0].Rows[k]["AllTrackingNumber"].ToString();
                    string[] s1 = stralltracking.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] sqty = strallqty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (s1.Length > 0)
                    {
                        try
                        {
                            for (int i = 0; i < s1.Length; i++)
                            {
                                if (s1[i].ToString() != hdnAmazonTrackinNo.Value.ToString())
                                {
                                    qty = qty + sqty[i].ToString() + ",";
                                    tracking = tracking + s1[i].ToString() + ",";
                                }
                            }
                        }
                        catch
                        {

                        }

                    }
                    if (string.IsNullOrEmpty(tracking))
                    {
                        CommonComponent.ExecuteCommonData("DELETE FROM tb_OrderShippedItems WHERE OrderedCustomCartID=" + allshipping.Tables[0].Rows[k]["OrderedCustomCartID"].ToString() + " and OrderNumber in  (SELECT OrderNumber FROM tb_AmazonlabelDetails WHERE AmazonLabelGenerationID=" + hdnAmazonCartID.Value + ")");
                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("update  tb_OrderShippedItems SET AllShippedQty='" + qty + "',AllTrackingNumber='" + tracking + "' WHERE OrderedCustomCartID=" + allshipping.Tables[0].Rows[k]["OrderedCustomCartID"].ToString() + " and OrderNumber in  (SELECT OrderNumber FROM tb_AmazonlabelDetails WHERE AmazonLabelGenerationID=" + hdnAmazonCartID.Value + ")");
                    }
                }

            }

            string shipmentId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ShipmentId,'') FROM tb_AmazonlabelDetails WHERE AmazonLabelGenerationID=" + hdnAmazonCartID.Value + ""));
            MWSMerchantFulfillmentService.Model.CancelShipmentResponse objlabel = new MWSMerchantFulfillmentService.Model.CancelShipmentResponse();
            if (!string.IsNullOrEmpty(shipmentId))
            {
                string sellerId = ViewState["AmazonMerchantID"].ToString();
                string mwsAuthToken = "";
                // The client application version
                string appVersion = "1.01";

                // The endpoint for region service and version (see developer guide)
                // ex: https://mws.amazonservices.com
                string serviceURL = ViewState["AmazonServiceURL"].ToString();
                MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
                config.ServiceURL = serviceURL;
                // Set other client connection configurations here if needed
                // Create the client itself
                MWSMerchantFulfillmentServiceClient client = new MWSMerchantFulfillmentServiceClient(ViewState["AmazonAccessKey"].ToString(), ViewState["AmazonSecretKey"].ToString(), ViewState["AmazonApplicationName"].ToString(), appVersion, config);
                MWSMerchantFulfillmentServiceSample obj = new MWSMerchantFulfillmentServiceSample(client);
                try
                {
                    objlabel = obj.InvokeCancelShipment(sellerId, shipmentId);
                    // Response.Write(objlabel.CancelShipmentResult.Shipment.Status.ToString());
                }
                catch (MWSMerchantFulfillmentServiceException ex)
                {
                    Response.Write(ex.StackTrace.ToString() + " " + ex.StatusCode.ToString());
                }
               
               
            }
            string shipmentfileName = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ShippingLabelFileName,'')  FROM tb_AmazonlabelDetails WHERE AmazonLabelGenerationID=" + hdnAmazonCartID.Value + ""));
            string LabelFPath = "";
            if (!string.IsNullOrEmpty(shipmentfileName))
            {
                if (shipmentfileName.ToString().ToUpper().IndexOf("USPS") > -1)
                {
                    LabelFPath = "/ShippingLabels/USPS/";// Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                }
                if (shipmentfileName.ToString().ToUpper().IndexOf("UPS") > -1)
                {
                    LabelFPath = "/ShippingLabels/UPS/"; //Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                }
                if (shipmentfileName.ToString().ToUpper().IndexOf("FEDEX") > -1)
                {
                    LabelFPath = "/ShippingLabels/FEDEX/"; //Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                }

                if (File.Exists(Server.MapPath(LabelFPath + shipmentfileName)))
                {
                    try
                    {
                        File.Delete(Server.MapPath(LabelFPath + shipmentfileName));
                    }
                    catch
                    {

                    }

                }
            }

            CommonComponent.ExecuteCommonData("DELETE FROM tb_AmazonlabelDetails WHERE AmazonLabelGenerationID=" + hdnAmazonCartID.Value + "");
            BindShippingGrid(Convert.ToInt32(Request.QueryString["Ono"].ToString()));
            hdnAmazonTrackinNo.Value = "";
            hdnAmazonCartID.Value = "";
        }

        protected void imggetmethod_Click(object sender, ImageClickEventArgs e)
        {
            string[] strboxes = ddlboxesall.SelectedValue.ToString().ToUpper().Split("X".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strboxes.Length > 0)
            {
                txtLengthgridall.Text = strboxes[0].ToString().Trim().Replace(" ", "");
            }
            if (strboxes.Length > 1)
            {
                txtWidthgridall.Text = strboxes[1].ToString().Trim().Replace(" ", "");
            }
            if (strboxes.Length > 2)
            {
                txtHeightgridall.Text = strboxes[2].ToString().Trim().Replace(" ", "");
            }
            BindShippingMethodRemaining(ddlShippingMethodall, 0, Convert.ToDecimal(txtProWeightall.Text), Convert.ToDecimal(txtHeightgridall.Text), Convert.ToDecimal(txtWidthgridall.Text), Convert.ToDecimal(txtLengthgridall.Text), null, "", Convert.ToInt32(0));
            allorderqty.Attributes.Add("style", "display:'';");
        }

        protected void btngeneratealllabelall_Click(object sender, ImageClickEventArgs e)
        {
            string stt = "1";
            try
            {


                decimal decHeight = 0, decLength = 0, decWidth = 0, decWeight = 0;


                string[] strboxes = ddlboxesall.SelectedValue.ToString().ToUpper().Split("X".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (strboxes.Length > 0)
                {
                    txtLengthgridall.Text = strboxes[0].ToString().Trim().Replace(" ", "");
                }
                if (strboxes.Length > 1)
                {
                    txtWidthgridall.Text = strboxes[1].ToString().Trim().Replace(" ", "");
                }
                if (strboxes.Length > 2)
                {
                    txtHeightgridall.Text = strboxes[2].ToString().Trim().Replace(" ", "");
                }

                if (!string.IsNullOrEmpty(txtProWeightall.Text.ToString()))
                    decWeight = Convert.ToDecimal(txtProWeightall.Text.ToString());

                if (!string.IsNullOrEmpty(txtHeightgridall.Text.ToString()))
                    decHeight = Convert.ToDecimal(txtHeightgridall.Text);

                if (!string.IsNullOrEmpty(txtLengthgridall.Text.ToString()))
                    decLength = Convert.ToDecimal(txtLengthgridall.Text);

                if (!string.IsNullOrEmpty(txtWidthgridall.Text.ToString()))
                    decWidth = Convert.ToDecimal(txtWidthgridall.Text);
                stt = "2";
                int chkQty = 0;
                string ShippingLabelFileName = "";
                string OrderNumber = Convert.ToString(Request.QueryString["Ono"]);
                string ShipmentId = "";
                if (!string.IsNullOrEmpty(OrderNumber.ToString()))
                {
                    if (!string.IsNullOrEmpty(ddlShippingMethodall.SelectedItem.Text))
                    {
                        CountryComponent objCountry = new CountryComponent();
                        if (ddlShippingMethodall.SelectedItem != null)
                        {
                            //MWSMerchantFulfillmentService.Model.GetShipmentResponse objShippent = new MWSMerchantFulfillmentService.Model.GetShipmentResponse();
                            MWSMerchantFulfillmentService.Model.Shipment objShippent = new MWSMerchantFulfillmentService.Model.Shipment();
                            MWSMerchantFulfillmentService.Model.CreateShipmentResponse objlabel = new MWSMerchantFulfillmentService.Model.CreateShipmentResponse();
                            if (!string.IsNullOrEmpty(ddlShippingMethodall.SelectedItem.Text))
                            {
                                stt = "3";
                                string sellerId = ViewState["AmazonMerchantID"].ToString();
                                string mwsAuthToken = "";
                                // The client application version
                                string appVersion = "1.01";

                                // The endpoint for region service and version (see developer guide)
                                // ex: https://mws.amazonservices.com
                                string serviceURL = ViewState["AmazonServiceURL"].ToString();
                                MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
                                config.ServiceURL = serviceURL;
                                // Set other client connection configurations here if needed
                                // Create the client itself
                                MWSMerchantFulfillmentServiceClient client = new MWSMerchantFulfillmentServiceClient(ViewState["AmazonAccessKey"].ToString(), ViewState["AmazonSecretKey"].ToString(), ViewState["AmazonApplicationName"].ToString(), appVersion, config);
                                MWSMerchantFulfillmentServiceSample obj = new MWSMerchantFulfillmentServiceSample(client);

                                stt = "4";

                                DataTable dtOrder = new DataTable();
                                dtOrder.Columns.Add("Qty", typeof(int));
                                dtOrder.Columns.Add("AmazonItemId", typeof(String));
                                dtOrder.Columns.Add("RefOrderId", typeof(String));
                                string hdnRefOrderNo = "";
                                string hdnshoppingcartid = "";
                                stt = "5";
                                foreach (GridViewRow gvr in grdShipping.Rows)
                                {

                                    if (gvr.RowType == DataControlRowType.DataRow)
                                    {
                                        CheckBox chkgeneratelbl = (CheckBox)gvr.FindControl("chkgeneratelbl");
                                        TextBox txtShippedQty = (TextBox)gvr.FindControl("txtShippedQty");
                                        Label lblCustomCartID = (Label)gvr.FindControl("lblCustomCartID");
                                        Label lblweight = (Label)gvr.FindControl("lblweight");
                                        if (chkgeneratelbl.Checked == false && !string.IsNullOrEmpty(txtShippedQty.Text.ToString()) && txtShippedQty.Text.ToString() != "0")
                                        {
                                            stt = "6";
                                            DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID,tb_OrderedShoppingCartItems.OrderedShoppingCartID FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + "");
                                            if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
                                            {
                                                for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                                                {
                                                    DataRow dataRow = dtOrder.NewRow();
                                                    dataRow["qty"] = txtShippedQty.Text.ToString();
                                                    dataRow["AmazonItemId"] = dtORderdetails.Tables[0].Rows[i]["OrderItemID"].ToString();
                                                    dataRow["RefOrderId"] = dtORderdetails.Tables[0].Rows[i]["RefOrderID"].ToString();
                                                    dtOrder.Rows.Add(dataRow);
                                                    hdnRefOrderNo = dtORderdetails.Tables[0].Rows[i]["RefOrderID"].ToString();
                                                    hdnshoppingcartid = dtORderdetails.Tables[0].Rows[i]["OrderedShoppingCartID"].ToString();
                                                    stt = "7";
                                                }
                                            }
                                        }
                                    }
                                }


                                stt = "8";
                                string[] strlids = ddlShippingMethodall.SelectedValue.ToString().Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                objlabel = obj.InvokeCreateShipment(hdnRefOrderNo.ToString(), sellerId, "", strlids[0].ToString(), strlids[1].ToString(), decLength, decWidth, decHeight, decWeight, dtOrder);

                                objShippent = objlabel.CreateShipmentResult.Shipment;
                                if (objShippent == null)
                                {
                                    return;
                                }
                                stt = "9";
                                // objShippent = obj.InvokeGetShipment(sellerId, "537d8635-c5fc-4aa8-aa35-a9af0a80ab78");
                                string strddd = objlabel.CreateShipmentResult.Shipment.Label.FileContents.Contents.ToString();
                                //objShippent = obj.InvokeGetShipment(sellerId, "537d8635-c5fc-4aa8-aa35-a9af0a80ab78");
                                //string strddd = objShippent.GetShipmentResult.Shipment.Label.FileContents.Contents.ToString();

                                byte[] imageBytes = Base64DecodeString(strddd);
                                string fileName = objlabel.CreateShipmentResult.Shipment.TrackingId.ToString();
                                ShipmentId = objlabel.CreateShipmentResult.Shipment.ShipmentId.ToString();
                                CommonComponent.ExecuteCommonData("Update tb_Order set ShipmentId='" + ShipmentId + "'  where  OrderNumber=" + OrderNumber + "");
                                // string fileName = objShippent.GetShipmentResult.Shipment.TrackingId.ToString();
                                string imagepath = "";
                                imagepath = "/ShippingLabels/FEDEX/";// AppLogic.AppConfigs("FedEx.LabelSavePath");
                                string fileFormat = AppLogic.AppConfigs("Shipping.LabelFormat").ToString();
                                if (ddlShippingMethodall.SelectedValue.ToString().ToUpper().IndexOf("USPS") > -1)
                                {
                                    imagepath = "/ShippingLabels/USPS/";// Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                                    fileName = "USPS-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.ToString())) ? "0" : hdnshoppingcartid.ToString()) + "@" +
                     DateTime.Now.Year.ToString() +
                     DateTime.Now.Month.ToString() +
                     DateTime.Now.Day.ToString() +
                     DateTime.Now.Hour.ToString() +
                     DateTime.Now.Minute.ToString() +
                     DateTime.Now.Second.ToString() + "-1." +
                     fileFormat.ToLower();
                                }
                                else if (ddlShippingMethodall.SelectedValue.ToString().ToUpper().IndexOf("UPS") > -1)
                                {
                                    imagepath = "/ShippingLabels/UPS/";//Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                                    fileName = "UPS-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.ToString())) ? "0" : hdnshoppingcartid.ToString()) + "@" +
                        DateTime.Now.Year.ToString() +
                        DateTime.Now.Month.ToString() +
                        DateTime.Now.Day.ToString() +
                        DateTime.Now.Hour.ToString() +
                        DateTime.Now.Minute.ToString() +
                        DateTime.Now.Second.ToString() + "-1." +
                        fileFormat.ToLower();
                                }
                                else if (ddlShippingMethodall.SelectedValue.ToString().ToUpper().IndexOf("FEDEX") > -1)
                                {
                                    imagepath = "/ShippingLabels/FEDEX/";//Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                                    fileName = "FedEx-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.ToString())) ? "0" : hdnshoppingcartid.ToString()) + "@" +
                                DateTime.Now.Year.ToString() +
                                DateTime.Now.Month.ToString() +
                                DateTime.Now.Day.ToString() +
                                DateTime.Now.Hour.ToString() +
                                DateTime.Now.Minute.ToString() +
                                DateTime.Now.Second.ToString() + "-1." + fileFormat.ToLower();
                                }

                                ShippingLabelFileName = fileName;
                                FileStream fstrm = new FileStream(Server.MapPath(imagepath + fileName.Replace("." + fileFormat.ToLower(), ".gzip")), FileMode.CreateNew, FileAccess.Write);
                                BinaryWriter writer = new BinaryWriter(fstrm);
                                writer.Write(imageBytes);
                                writer.Close();
                                fstrm.Close();

                                using (FileStream fInStream = new FileStream(Server.MapPath(imagepath + fileName.Replace("." + fileFormat.ToLower(), ".gzip")),
                    FileMode.Open, FileAccess.Read))
                                {
                                    using (GZipStream zipStream = new GZipStream(fInStream, CompressionMode.Decompress))
                                    {
                                        using (FileStream fOutStream = new FileStream(Server.MapPath(imagepath + "temp/" + fileName),
                                        FileMode.Create, FileAccess.Write))
                                        {
                                            byte[] tempBytes = new byte[4096];
                                            int i;
                                            while ((i = zipStream.Read(tempBytes, 0, tempBytes.Length)) != 0)
                                            {
                                                fOutStream.Write(tempBytes, 0, i);
                                            }
                                        }
                                    }
                                }
                                if (fileName.ToString().ToLower().IndexOf(".png") > -1)
                                {
                                    ResizeImagewithpdf(imagepath + "temp", imagepath, fileName, imagepath);
                                }
                                else
                                {
                                    System.IO.File.Copy(imagepath + "temp/" + fileName, imagepath + fileName);
                                }

                            }
                            else
                            {
                                Session["ProductDetail"] = null;
                                return;
                            }
                            decimal ShippingCost = 0;
                            if (ddlShippingMethodall.SelectedValue.ToString() != "")
                            {
                                int Index = ddlShippingMethodall.SelectedItem.Text.IndexOf("($");
                                int Length = ddlShippingMethodall.SelectedItem.Text.LastIndexOf(")") - Index;
                                if (Index != -1 && Length != 0)
                                {
                                    string strShippingCost = ddlShippingMethodall.SelectedItem.Text.Substring(Index + 2, Length - 2).Trim();
                                    Decimal.TryParse(strShippingCost, out ShippingCost);
                                }
                            }

                            if (string.IsNullOrEmpty(ShippingLabelFileName) || ShippingLabelFileName.ToString().Trim() == "")
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg", "jAlert('Error While Generating Shipping Label.','Message');", true);
                                Session["ProductDetail"] = null;
                                return;
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(ShippingLabelFileName.Trim())) && Convert.ToString(ShippingLabelFileName.Trim()).ToLower().IndexOf("error :") > -1)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg", "jAlert('" + ShippingLabelFileName.ToString() + "','Message');", true);
                                Session["ProductDetail"] = null;
                                return;
                            }
                            if (!string.IsNullOrEmpty(ShippingLabelFileName.ToString()))
                            {
                                string StrShipping = Convert.ToString(ddlShippingMethodall.SelectedItem.Text);
                                if (!string.IsNullOrEmpty(StrShipping.ToString().Trim()) && StrShipping.ToLower().IndexOf("($") > -1)
                                {
                                    StrShipping = StrShipping.Substring(0, StrShipping.IndexOf("($"));
                                }
                                string strdimension = Convert.ToString(Convert.ToString(decHeight) + " x " + Convert.ToString(decWidth) + " x " + Convert.ToString(decLength));

                                CommonComponent.ExecuteCommonData("Update tb_Order set IsBackendProcessed=1,ShipmentId='" + ShipmentId + "', ShippingDimension ='" + strdimension + "', ShippingLabelFileName='" + ShippingLabelFileName.ToString().ToLower().Replace(".png", ".pdf") + "',ShippingLabelMethod='" + ddlShippingMethodall.SelectedItem.Text + "',ShippingLabelCost=" + ShippingCost + ",ShippingLabelWeight=" + decWeight + ",ShippingLabelPackageHeight=" + decHeight + ",ShippingLabelPackageWidth=" + decWidth + ",ShippingLabelPackageLength=" + decLength + "  where  OrderNumber=" + OrderNumber + "");


                                foreach (GridViewRow gvr in grdShipping.Rows)
                                {
                                    if (gvr.RowType == DataControlRowType.DataRow)
                                    {
                                        CheckBox chkgeneratelbl = (CheckBox)gvr.FindControl("chkgeneratelbl");
                                        TextBox txtShippedQty = (TextBox)gvr.FindControl("txtShippedQty");
                                        Label lblCustomCartID = (Label)gvr.FindControl("lblCustomCartID");
                                        Label lblweight = (Label)gvr.FindControl("lblweight");
                                        if (chkgeneratelbl.Checked == false && !string.IsNullOrEmpty(txtShippedQty.Text.ToString()) && txtShippedQty.Text.ToString() != "0")
                                        {
                                            DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID,SKU FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + "");
                                            if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
                                            {
                                                for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                                                {
                                                    CommonComponent.ExecuteCommonData("EXEC usp_MicrosoftNav_PackageHeader " + OrderNumber + ",'" + objlabel.CreateShipmentResult.Shipment.TrackingId.ToString() + "','" + objlabel.CreateShipmentResult.Shipment.ShippingService.CarrierName.ToString() + "','" + objlabel.CreateShipmentResult.Shipment.ShippingService.ShipDate.ToString() + "','0','" + dtORderdetails.Tables[0].Rows[i]["SKU"].ToString() + "'," + txtShippedQty.Text.ToString() + "");

                                                }
                                            }
                                            CommonComponent.ExecuteCommonData("INSERT INTO tb_AmazonlabelDetails(OrderNumber, OrderedCustomcartId, ShipmentId, ShippingDimension, ShippingLabelFileName, ShippingLabelMethod, ShippingLabelCost, ShippingLabelWeight, ShippingLabelPackageHeight, ShippingLabelPackageWidth, ShippingLabelPackageLength, ShipQty, TrackingNumber, CreatedOn) SELECT OrderNumber, OrderedCustomcartId, '" + ShipmentId + "', '" + strdimension + "', '" + ShippingLabelFileName.ToString().ToLower().Replace(".png", ".pdf") + "', '" + ddlShippingMethodall.SelectedItem.Text + "', " + ShippingCost + ", " + decWeight + ", " + decHeight + ", " + decWidth + ", " + decLength + ", " + txtShippedQty.Text + ", '" + objlabel.CreateShipmentResult.Shipment.TrackingId.ToString() + "', getdate()  FROM tb_OrderedShoppingCartItems INNER JOIN tb_Order on tb_Order.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID WHERE OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + "");

                                        }
                                    }
                                }
                                BindShippingGrid(Convert.ToInt32(Request.QueryString["Ono"].ToString()));

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(stt.ToString());
            }

        }
    }
}