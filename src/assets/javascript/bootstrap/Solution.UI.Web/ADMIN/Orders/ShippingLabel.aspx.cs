using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using System.Collections;
using System.Text;
using Solution.ShippingMethods;
using Solution.ShippingMethods.Endicia;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ShippingLabel : BasePage
    {
        Decimal OrWeight = Decimal.Zero;
        OrderComponent ObjOrder = null;
        public DataTable DtShippingLableDetail = new DataTable("ShippingLableDetail");
        DataSet DsOrder = new DataSet();

        DataTable dt = new DataTable();
        ArrayList PIDs;
        decimal dTotalWeight = 0M;

        int Packid = 0;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblmsg.Text = "";
            lblshiperror.Text = "";
            ltUSPSShippingLabel.Text = "";
            warehouseinventoryMsg.Text = "";
            if (Request.QueryString["ONo"] != null)
            {
                Int32 StoreID = 0;

                int OrderNumber = 0;
                Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + OrderNumber)), out StoreID);
                AppConfig.StoreID = StoreID;
            }

            if (!Page.IsPostBack)
            {
                string OrderNumber = string.Empty;
                if (Request.QueryString["ONo"] != null)
                {
                    OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                }
                txtWeight.Text = "0";
                BindWareHouse();

                ViewState["lastTable"] = null;
                GetExistingFilesUSPS(OrderNumber);
                GetExistingFilesUPS(OrderNumber);
                GetExistingFilesFEDEX(OrderNumber);
                if (ddlWareHouse.Items.Count > 0)
                {
                    ddlWareHouse.SelectedIndex = 1;
                    ddlWareHouse_SelectedIndexChanged(null, null);

                }
                else
                    BindData(Convert.ToInt32(OrderNumber));
            }

            GetNewshipping();
            GetPackId();
        }

        /// <summary>
        /// Gets the Pack ID
        /// </summary
        private void GetPackId()
        {
            string OrderNumber = string.Empty;
            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
            }
            Int32 dspkid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ISNULL(max(p.packid),0) from tb_OrderedShoppingCartItems p inner join tb_Order o on p.OrderedShoppingCartID=o.ShoppingCardID where o.OrderNumber='" + OrderNumber + "'"));

            if (dspkid > 0)
            {
                Packid = dspkid + 1;
            }
            else
            {
                if (grdUSPS.Rows.Count > 0)
                {
                    Packid = grdUSPS.Rows.Count;
                }
                if (grdUPS.Rows.Count > 0)
                {
                    Packid = Packid + grdUPS.Rows.Count;
                }

                //*
                if (grdFEDEX.Rows.Count > 0)
                {
                    Packid = Packid + grdFEDEX.Rows.Count;
                }

                Packid = grdShipping.Rows.Count + Packid + 1;
            }
            // Packid = grdShipping.Rows.Count + Packid + 1;
        }

        /// <summary>
        /// Binds the Warehouse into Drop down
        /// </summary>
        private void BindWareHouse()
        {

            string sqlWareHouse = "SELECT WareHouseID,Name FROM dbo.tb_WareHouse WHERE Active=1 AND ISNULL(Deleted,0)=0";
            DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
            ddlWareHouse.DataSource = dsWareHouse;
            ddlWareHouse.DataBind();


            if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
            {
                ddlWareHouse.DataSource = dsWareHouse;
                ddlWareHouse.DataTextField = "Name";
                ddlWareHouse.DataValueField = "WareHouseID";
            }
            else
            {
                ddlWareHouse.DataSource = null;
            }
            ddlWareHouse.DataBind();
            ddlWareHouse.Items.Insert(0, new ListItem("Select Warehouse", "0"));


            //ddlWareHouse
        }

        /// <summary>
        /// Gets the warehouse address.
        /// </summary>
        /// <param name="WarehouseID">int WarehouseID</param>
        private void GetWarehouseAdddress(Int32 WarehouseID)
        {
            string sqlWareHouse = "SELECT * FROM dbo.tb_WareHouse WHERE Active=1 AND ISNULL(Deleted,0)=0 and WareHouseID=" + WarehouseID;
            DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
            if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
            {
                string ShipFromAddress = "";

                ShipFromAddress += "<br /><b>Shipping From Address</b><br />";
                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginContactName").ToString().Trim()))
                    ShipFromAddress += AppLogic.AppConfigs("Shipping.OriginContactName") + "<br />";

                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginPhone").ToString().Trim()))
                    ShipFromAddress += AppLogic.AppConfigs("Shipping.OriginPhone") + "<br />";

                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.CompanyName").ToString().Trim()))
                    ShipFromAddress += AppLogic.AppConfigs("Shipping.CompanyName") + "<br />";



                if (dsWareHouse.Tables[0].Rows[0]["Address1"] != null && dsWareHouse.Tables[0].Rows[0]["Address1"].ToString() != "")
                    ShipFromAddress += dsWareHouse.Tables[0].Rows[0]["Address1"].ToString();// +"<br />";

                if (dsWareHouse.Tables[0].Rows[0]["Address2"] != null && dsWareHouse.Tables[0].Rows[0]["Address2"].ToString() != "")
                    ShipFromAddress += "," + dsWareHouse.Tables[0].Rows[0]["Address2"].ToString();// +", ";

                if (dsWareHouse.Tables[0].Rows[0]["City"] != null && dsWareHouse.Tables[0].Rows[0]["City"].ToString() != "")
                    ShipFromAddress += "<br/>" + dsWareHouse.Tables[0].Rows[0]["City"].ToString();// +"<br />";

                if (dsWareHouse.Tables[0].Rows[0]["Suite"] != null && dsWareHouse.Tables[0].Rows[0]["Suite"].ToString() != "")
                    ShipFromAddress += "," + dsWareHouse.Tables[0].Rows[0]["Suite"].ToString();

                if (dsWareHouse.Tables[0].Rows[0]["State"] != null && dsWareHouse.Tables[0].Rows[0]["State"].ToString() != "")
                    ShipFromAddress += "<br/>" + dsWareHouse.Tables[0].Rows[0]["State"].ToString();// +"<br />";

                if (dsWareHouse.Tables[0].Rows[0]["Zipcode"] != null && dsWareHouse.Tables[0].Rows[0]["Zipcode"].ToString() != "")
                    ShipFromAddress += "&nbsp;&nbsp;" + dsWareHouse.Tables[0].Rows[0]["Zipcode"].ToString();

                if (dsWareHouse.Tables[0].Rows[0]["Country"] != null && dsWareHouse.Tables[0].Rows[0]["Country"].ToString() != "")
                {
                    CountryComponent obj = new CountryComponent();
                    String CountryName = obj.GetCountryNameByCodeForShippingLabel(Convert.ToInt32(dsWareHouse.Tables[0].Rows[0]["Country"].ToString()));
                    ShipFromAddress += "<br />" + CountryName;
                }

                ltShippingFrom.Text = ShipFromAddress;
            }
            else
                ltShippingFrom.Text = "";
        }

        /// <summary>
        /// Binds the data for Shipping Label
        /// </summary>
        /// <param name="ONo">int ONo</param>
        private void BindData(Int32 ONo)
        {
            try
            {
                ObjOrder = new OrderComponent();
                DsOrder = new DataSet();
                DsOrder = ObjOrder.GetOrderDetailsByOrderID(ONo);

                string ShipToAddress = string.Empty;
                string ShipFromAddress = string.Empty;
                if (DsOrder.Tables[0].Rows.Count > 0)
                {
                    GetPersonalInfo(DsOrder);
                    AddCartItem(Convert.ToInt32(DsOrder.Tables[0].Rows[0]["CustomerID"].ToString()), ONo.ToString());
                }
            }
            catch { }
        }

        /// <summary>
        /// Add Cart Items 
        /// </summary>
        /// <param name="CustID">int CustID</param>
        /// <param name="ONo">string ONo</param>
        public void AddCartItem(int CustID, string ONo)
        {
            ShoppingCartComponent ObjOCart = new ShoppingCartComponent();
            DataSet DsCItems = new DataSet();
            DsCItems = ObjOCart.GetOrderedShoppingCartItemsByCartId((Convert.ToInt32(ONo)));

            warehouseinventoryMsg.Text = "";

            if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
            {
                grdShipping.DataSource = DsCItems;
                grdShipping.DataBind();
            }
            else
            {
                DataRow dr = DtShippingLableDetail.NewRow();
                DtShippingLableDetail.Columns.Add("PackageId");
                DtShippingLableDetail.Columns.Add("SKU");
                DtShippingLableDetail.Columns.Add("Name");
                DtShippingLableDetail.Columns.Add("Quantity");
                DtShippingLableDetail.Columns.Add("Height");
                DtShippingLableDetail.Columns.Add("Width");
                DtShippingLableDetail.Columns.Add("Length");
                DtShippingLableDetail.Columns.Add("Weight");
                DtShippingLableDetail.Columns.Add("SalePrice");

                dr["PackageId"] = "";
                dr["SKU"] = "";
                dr["Name"] = "";
                dr["Quantity"] = "";
                dr["Height"] = "";
                dr["Width"] = "";
                dr["Length"] = "";
                dr["Weight"] = "";
                dr["SalePrice"] = "";

                DtShippingLableDetail.Rows.Add(dr);

                grdShipping.DataSource = DtShippingLableDetail;
                grdShipping.DataBind();

            }

            if (grdShipping != null)
                lblPackageIDFP.Text = Packid.ToString(); //**(grdShipping.Rows.Count + 1).ToString();
        }

        /// <summary>
        /// Gets the personal info of Customer.
        /// </summary>
        /// <param name="Ds">Dataset ds.</param>
        public void GetPersonalInfo(DataSet Ds)
        {
            //    clsAddress ObjAddress = new clsAddress();
            string st_b = null;

            AppConfig.StoreID = Convert.ToInt32(Ds.Tables[0].Rows[0]["Storeid"].ToString());

            ViewState["StoreID"] = Convert.ToInt32(Ds.Tables[0].Rows[0]["Storeid"].ToString());
            ViewState["CustomerID"] = Convert.ToInt32(Ds.Tables[0].Rows[0]["CustomerID"].ToString());

            st_b += "<b>Shipping To Address</b><br />";

            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingFirstName"].ToString().Trim()))
                st_b += Convert.ToString(Ds.Tables[0].Rows[0]["ShippingFirstName"]) + " ";

            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ShippingLastName"].ToString().Trim()))
                st_b += Convert.ToString(Ds.Tables[0].Rows[0]["ShippingLastName"]) + "<br />";


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
        /// Shipping Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdShipping_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddProduct")
            {
                if (Page.IsValid)
                {
                    GridViewRow gvr = grdShipping.FooterRow;
                    string strSKU = (gvr.FindControl("txtAddSKU") as System.Web.UI.WebControls.TextBox).Text;
                    string strName = (gvr.FindControl("txtAddProdName") as System.Web.UI.WebControls.TextBox).Text;
                    string strQuantity = (gvr.FindControl("txtAddQuantity") as System.Web.UI.WebControls.TextBox).Text;
                    string strHeight = (gvr.FindControl("txtAddHeight") as System.Web.UI.WebControls.TextBox).Text;
                    string strWidth = (gvr.FindControl("txtAddWidth") as System.Web.UI.WebControls.TextBox).Text;
                    string strLength = (gvr.FindControl("txtAddLength") as System.Web.UI.WebControls.TextBox).Text;
                    string strWeight = (gvr.FindControl("txtAddWeight") as System.Web.UI.WebControls.TextBox).Text;
                    string strProductId = "0";
                    //string strDimension = strHeight + "X" + strWeight + "X" + strLength;
                    //AddToGrid(strSKU, strName, strQuantity, strDimension, strWeight);
                    AddToGrid(strSKU, strName, strQuantity, strWeight, strHeight, strWidth, strLength, strProductId.ToString());
                }
            }
        }

        /// <summary>
        /// Shipping Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdShipping_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.EmptyDataRow)
                {
                    //System.Web.UI.WebControls.Label lbldimention = (System.Web.UI.WebControls.Label)e.Row.FindControl("lbldimention");
                    System.Web.UI.WebControls.TextBox txtHeight = (System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtHeightgrid");
                    System.Web.UI.WebControls.TextBox txtWidth = (System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtWidthgrid");
                    System.Web.UI.WebControls.TextBox txtLength = (System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtLengthgrid");
                    System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtProWeight");
                    System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblQuantity");





                    //System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblavailQuantity");
                    // System.Web.UI.WebControls.Label lblProductID = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblProductID");

                    //string sqlinv = "SELECT Inventory FROM dbo.tb_WareHouseProductInventory WHERE ProductID=" + lblProductID + " AND WareHouseID="+ddlWareHouse.SelectedItem.Value+"";
                    //DataSet dsavailDty = CommonComponent.GetCommonDataSet(sqlinv);
                    // DataSet dsavailDty =Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Inventory FROM dbo.tb_WareHouseProductInventory WHERE ProductID="+lblProductID+" AND WareHouseID=1"));

                    //lblavailQuantity.Text = "3";
                    int intQuantity = 0;
                    Int32.TryParse(lblQuantity.Text, out intQuantity);
                    if (txtHeight != null && txtWidth != null && txtLength != null)
                    {
                        txtHeight.Text = (txtHeight.Text.Trim().Length > 0 && !string.IsNullOrEmpty(txtHeight.Text.Trim())) ? txtHeight.Text.Trim() : "0";
                        txtWidth.Text = (txtWidth.Text.Trim().Length > 0 && !string.IsNullOrEmpty(txtWidth.Text.Trim())) ? txtWidth.Text.Trim() : "0";
                        txtLength.Text = (txtLength.Text.Trim().Length > 0 && !string.IsNullOrEmpty(txtLength.Text.Trim())) ? txtLength.Text.Trim() : "0";
                    }
                    //if (txtProWeight != null)
                    //    txtWeight.Text = Convert.ToString(Convert.ToDecimal((string.IsNullOrEmpty(txtWeight.Text)) ? "0" : txtWeight.Text) + ((Decimal)intQuantity * Convert.ToDecimal((string.IsNullOrEmpty(txtProWeight.Text)) ? "0" : txtProWeight.Text)));

                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtProWeight");
                    //*System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblavailQuantity");
                    System.Web.UI.WebControls.Label lblProductID = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblProductID");
                    System.Web.UI.WebControls.CheckBox chkAllowShip = (System.Web.UI.WebControls.CheckBox)e.Row.FindControl("chkAllowShip");
                    System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblShipping");
                    System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblQuantity");


                    int qty = 0;
                    try { qty = Convert.ToInt32(lblQuantity.Text); }
                    catch { qty = 0; }

                    string sqlinv = "SELECT ISNULL(Inventory,0) AS Inventory FROM dbo.tb_WareHouseProductInventory WHERE ProductID=" + lblProductID.Text + " AND WareHouseID='" + ddlWareHouse.SelectedItem.Value + "'";
                    Int32 winventory = Convert.ToInt32(CommonComponent.GetScalarCommonData(sqlinv));


                    if (winventory <= 0)
                    {
                        //lblavailQuantity.Text = "<span Style='color:red;font-weight:bold;'> " + winventory.ToString() + " </span>";

                       //* lblavailQuantity.Text = "0";
                       //* lblavailQuantity.ForeColor = System.Drawing.Color.Red;
                       //* lblavailQuantity.Font.Bold = true;
                        //**br
                        //chkAllowShip.Visible = false;
                        //  txtProWeight.ReadOnly = true;
                        txtProWeight.ForeColor = System.Drawing.Color.Red;
                        // txtProWeight.Text = "0";// true;
                        ((CheckBox)e.Row.FindControl("chkAllowShip")).Attributes.Add("onclick", "javascript:chkboxChecked('0','" + ((CheckBox)e.Row.FindControl("chkAllowShip")).ClientID + "')");
                    }
                    else if (qty > winventory)
                    {
                        //* lblavailQuantity.Text = winventory.ToString();
                        //* lblavailQuantity.ForeColor = System.Drawing.Color.Red;
                        //* lblavailQuantity.Font.Bold = true;
                        //**br
                        //chkAllowShip.Visible = false;
                        //  txtProWeight.ReadOnly = true;
                        txtProWeight.ForeColor = System.Drawing.Color.Red;
                        ((CheckBox)e.Row.FindControl("chkAllowShip")).Attributes.Add("onclick", "javascript:chkboxChecked('0','" + ((CheckBox)e.Row.FindControl("chkAllowShip")).ClientID + "')");

                    }
                    else if (lblShipping.Text.ToLower() == "yes")
                    {
                        //*  lblavailQuantity.Text = winventory.ToString();
                        //* lblavailQuantity.ForeColor = System.Drawing.Color.Green;
                        //* lblavailQuantity.Font.Bold = true;
                        txtProWeight.ForeColor = System.Drawing.Color.Green;
                        //((CheckBox)e.Row.FindControl("chkAllowShip")).Attributes.Add("onclick", "javascript:chkboxChecked('1','" + ((CheckBox)e.Row.FindControl("chkAllowShip")).ClientID + "')");
                        ((CheckBox)e.Row.FindControl("chkAllowShip")).Attributes.Add("onclick", "javascript:chklblgen('yes','" + ((CheckBox)e.Row.FindControl("chkAllowShip")).ClientID + "')");
                    }
                    else
                    {

                        //* lblavailQuantity.Text = winventory.ToString();
                        //* lblavailQuantity.ForeColor = System.Drawing.Color.Green;
                        //*lblavailQuantity.Font.Bold = true;

                        dTotalWeight += Convert.ToDecimal(txtProWeight.Text);
                    }




                    //   lblavailQuantity.Text = "<span Style='color:green;font-weight:bold;'> " + winventory.ToString() + " </span>";
                }
            }
            catch { }
            txtWeight.Text = dTotalWeight.ToString("f2");
            hfWeight.Value = txtWeight.Text;
        }

        /// <summary>
        /// Add Data into Grid
        /// </summary>
        /// <param name="strAddSKU">string strAddSKU</param>
        /// <param name="strAddName">string strAddName</param>
        /// <param name="strAddQuantity">string strAddQuantity</param>
        /// <param name="strAddWeight">string strAddWeight</param>
        /// <param name="strAddHeight">string strAddHeight</param>
        /// <param name="strAddWidth">string strAddWidth</param>
        /// <param name="strAddLength">string strAddLength</param>
        /// <param name="productId">string productId</param>
        private void AddToGrid(string strAddSKU, string strAddName, string strAddQuantity, string strAddWeight, string strAddHeight, string strAddWidth, string strAddLength, string productId)
        {
            DataSet dsPackages = new DataSet();
            DataTable dtPackages = new DataTable("Packages");
            dsPackages.Tables.Add(dtPackages);
            DataColumn colID = new DataColumn("PackageId", typeof(Int32));
            colID.AutoIncrement = true;
            colID.AutoIncrementSeed = 1;
            colID.AutoIncrementStep = 1;
            dtPackages.Columns.Add(colID);
            //dtPackages.Columns.Add("ShippingCartID");
            dtPackages.Columns.Add("SalePrice");
            dtPackages.Columns.Add("SKU");
            dtPackages.Columns.Add("Name");
            dtPackages.Columns.Add("Quantity");
            dtPackages.Columns.Add("Weight");
            dtPackages.Columns.Add("Height");
            dtPackages.Columns.Add("Width");
            dtPackages.Columns.Add("Length");
            // dtPackages.Columns.Add("ProductId");
            DataRow drPackage = null;
            foreach (GridViewRow gvr in grdShipping.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    string strPackageId = (gvr.FindControl("lblPackageId") as System.Web.UI.WebControls.Label).Text;
                    string strSKU = (gvr.FindControl("lblSKU") as System.Web.UI.WebControls.Label).Text;
                    string strName = (gvr.FindControl("lblProductName") as System.Web.UI.WebControls.Label).Text;
                    string strQuantity = (gvr.FindControl("lblQuantity") as System.Web.UI.WebControls.Label).Text;
                    string strWeight = (gvr.FindControl("txtProWeight") as System.Web.UI.WebControls.TextBox).Text;
                    //string strShoppingCartID = (gvr.FindControl("lblShippingCartID") as System.Web.UI.WebControls.Label).Text;
                    string strPrice = (gvr.FindControl("txtProductPrice") as System.Web.UI.WebControls.TextBox).Text;
                    string strHeight = (gvr.FindControl("txtHeightgrid") as System.Web.UI.WebControls.TextBox).Text;
                    string strWidth = (gvr.FindControl("txtWidthgrid") as System.Web.UI.WebControls.TextBox).Text;
                    string strLength = (gvr.FindControl("txtLengthgrid") as System.Web.UI.WebControls.TextBox).Text;
                    //  string strProductId = (gvr.FindControl("lblProductID") as System.Web.UI.WebControls.Label).Text;
                    drPackage = dtPackages.NewRow();

                    // drPackage["ShippingCartID"] = strShoppingCartID;
                    drPackage["SalePrice"] = strPrice;
                    drPackage["SKU"] = strSKU;
                    drPackage["Name"] = strName;
                    drPackage["Quantity"] = strQuantity;
                    drPackage["Weight"] = strWeight;
                    drPackage["Height"] = strHeight;
                    drPackage["Width"] = strWidth;
                    drPackage["Length"] = strLength;
                    //  drPackage["ProductId"] = strProductId;
                    dtPackages.Rows.Add(drPackage);
                }
            }
            drPackage = dtPackages.NewRow();
            //drPackage["ShippingCartID"] = 0;
            drPackage["SalePrice"] = 0;
            drPackage["SKU"] = strAddSKU;
            drPackage["Name"] = strAddName;
            drPackage["Quantity"] = strAddQuantity;
            drPackage["Weight"] = strAddWeight;
            drPackage["Height"] = strAddHeight;
            drPackage["Width"] = strAddWidth;
            drPackage["Length"] = strAddLength;
            // drPackage["productId"] = productId;

            dtPackages.Rows.Add(drPackage);

            grdShipping.DataSource = dsPackages;
            grdShipping.DataBind();
        }

        /// <summary>
        /// Extra Text Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtExtra_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Binds the shipping method.
        /// </summary>
        private void BindShippingMethod()
        { // int UPScnt = 0;
            int USPScnt = 0;// *//
            string OrgShippingZip = "";
            string OrgCountry = "";
            string sqlWareHouse = "SELECT ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse  INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + ddlWareHouse.SelectedItem.Value;
            DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
            if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
            {

                OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
                OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
            }


            rdRadioForShipping.Items.Clear();
            string strUSPSMessage = "";
            string strUPSMessage = "";
            lblmsg.Text = "";
            decimal decTotalWeight = 0;

            DataTable ShippingTable = new DataTable();//*//
            ShippingTable.Columns.Add("sn", typeof(int));
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            ShippingTable.Columns.Add("Shippingmethod", typeof(String));

            DataTable USPSTable = new DataTable();
            USPSTable.Columns.Add("sn", typeof(int));
            USPSTable.Columns.Add("ShippingMethodName", typeof(String));
            USPSTable.Columns.Add("Price", typeof(decimal));
            USPSTable.Columns.Add("Shippingmethod", typeof(String));

            DataTable UPSTable = new DataTable();
            UPSTable.Columns.Add("sn", typeof(int));
            UPSTable.Columns.Add("ShippingMethodName", typeof(String));
            UPSTable.Columns.Add("Price", typeof(decimal));
            UPSTable.Columns.Add("Shippingmethod", typeof(String));

            DataTable FEDEXTable = new DataTable();
            FEDEXTable.Columns.Add("sn", typeof(int));
            FEDEXTable.Columns.Add("ShippingMethodName", typeof(String));
            FEDEXTable.Columns.Add("Price", typeof(decimal));
            FEDEXTable.Columns.Add("Shippingmethod", typeof(String));

            ShippingComponent objShipping = new ShippingComponent();
            DataSet objShipServices = new DataSet();
            objShipServices = objShipping.GetShippingServices(Convert.ToInt32(ViewState["StoreID"].ToString()));

            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(ViewState["CountryName"].ToString()));
            decimal Weight = decimal.Zero;
            Weight = Convert.ToDecimal(txtWeight.Text);

            if (Weight == 0)
            {
                Weight = 1;
            }


            if (objShipServices != null && objShipServices.Tables.Count > 0 && objShipServices.Tables[0].Rows.Count > 0)
            {

                //if (objShipServices.Tables[0].Select("ShippingService='USPS'").Length > 0)
                //{
                //    EndiciaService objRate = new EndiciaService();

                //    // New 
                //    foreach (GridViewRow gvr in grdShipping.Rows)
                //    {
                //        System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");
                //        System.Web.UI.WebControls.TextBox txtProductPrice = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProductPrice");
                //        //* System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                //        System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                //        System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");

                //        int Qty = 0;
                //        //* int availQuantity = 0;
                //        try { Qty = Convert.ToInt32(lblQuantity.Text); }
                //        catch { Qty = 0; }
                //       //* try { availQuantity = Convert.ToInt32(lblavailQuantity.Text); }
                //        //*catch { availQuantity = 0; }

                //        //if (Qty > availQuantity) { availQuantity = 0; }


                //        if (!chkAllowShipment.Checked)
                //        {
                //            continue;
                //        }

                //        Decimal ProWeight = Convert.ToDecimal("1");

                //        System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                //        // System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");

                //        //if (availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                //        if (lblShipping.Text.ToLower() == "no")
                //            decTotalWeight += Convert.ToDecimal(string.IsNullOrEmpty(txtProWeight.Text) ? "0" : txtProWeight.Text);

                //        //if (txtProWeight != null && !string.IsNullOrEmpty(txtProWeight.Text) && availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                //        if (txtProWeight != null && !string.IsNullOrEmpty(txtProWeight.Text) && lblShipping.Text.ToLower() == "no")
                //            ProWeight = Convert.ToDecimal(txtProWeight.Text);


                //        USPSTable = objRate.EndiciaGetRatesAdminWarehouseShippingLabel(CountryCode, OrgShippingZip, ViewState["Zip"].ToString(), CountryCode.ToString(), Convert.ToDouble(ProWeight), ref strUSPSMessage);
                //        if (USPSTable != null && USPSTable.Rows.Count > 0)
                //        {

                //            if (ShippingTable.Rows.Count > 0)
                //            {
                //                for (int j = 0; j < ShippingTable.Rows.Count; j++)
                //                {
                //                    if (USPSTable.Select("Shippingmethod ='" + ShippingTable.Rows[j]["Shippingmethod"].ToString() + "'").Length > 0)
                //                    {
                //                        for (int k = 0; k < USPSTable.Rows.Count; k++)
                //                        {
                //                            if (USPSTable.Rows[k]["Shippingmethod"].ToString().ToLower() == ShippingTable.Rows[j]["Shippingmethod"].ToString().ToLower())
                //                            {
                //                                ShippingTable.Rows[j]["Price"] = Convert.ToDouble(Convert.ToDouble(ShippingTable.Rows[j]["Price"]) + Convert.ToDouble(USPSTable.Rows[k]["Price"].ToString()));

                //                                ShippingTable.Rows[j]["ShippingMethodName"] = "USPS - " + ShippingTable.Rows[j]["Shippingmethod"].ToString() + " ($" + Math.Round(Convert.ToDecimal(ShippingTable.Rows[j]["Price"].ToString()), 2).ToString() + ")";

                //                            }
                //                        }

                //                    }
                //                    else
                //                    {
                //                        ShippingTable.Rows.RemoveAt(j);
                //                        ShippingTable.AcceptChanges();
                //                    }

                //                }

                //                USPScnt = 0;
                //                for (int k = 0; k < ShippingTable.Rows.Count; k++)
                //                {
                //                    USPScnt += 3;
                //                    ShippingTable.Rows[k]["sn"] = USPScnt;
                //                }
                //            }
                //            else
                //            {
                //                USPSTable.DefaultView.Sort = "Price asc";
                //                for (int cntUSPS = 0; cntUSPS < USPSTable.DefaultView.ToTable().Rows.Count; cntUSPS++)
                //                {
                //                    //USPScnt += 2;
                //                    USPScnt += 3;
                //                    DataRow dataRow = ShippingTable.NewRow();
                //                    dataRow["sn"] = USPScnt;
                //                    dataRow["Shippingmethod"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["Shippingmethod"].ToString();
                //                    dataRow["ShippingMethodName"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["ShippingMethodName"].ToString();
                //                    dataRow["Price"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["Price"].ToString();
                //                    ShippingTable.Rows.Add(dataRow);

                //                }
                //            }

                //        }
                //    }

                //    // For unchecked product
                //    decTotalWeight = Convert.ToDecimal(hfWeight.Value);
                //    // ByAnil// txtweight.Text = hfWeight.Value;
                //    try
                //    {
                //        decimal w = Convert.ToDecimal(txtWeight.Text.Trim());
                //    }
                //    catch (Exception)
                //    {
                //        txtWeight.Text = decTotalWeight.ToString();

                //    }
                //    if (Convert.ToDecimal(txtWeight.Text) > Convert.ToDecimal(0.00))
                //    {
                //        Weight = Convert.ToDecimal(txtWeight.Text);
                //        USPSTable = null;

                //        USPSTable = objRate.EndiciaGetRatesAdminWarehouseShippingLabel(CountryCode, OrgShippingZip, ViewState["Zip"].ToString(), CountryCode.ToString(), Convert.ToDouble(Weight), ref strUSPSMessage);
                //        if (USPSTable != null && USPSTable.Rows.Count > 0)
                //        {

                //            if (ShippingTable.Rows.Count > 0)
                //            {
                //                for (int j = 0; j < ShippingTable.Rows.Count; j++)
                //                {
                //                    if (USPSTable.Select("Shippingmethod ='" + ShippingTable.Rows[j]["Shippingmethod"].ToString() + "'").Length > 0)
                //                    {
                //                        for (int k = 0; k < USPSTable.Rows.Count; k++)
                //                        {
                //                            if (USPSTable.Rows[k]["Shippingmethod"].ToString().ToLower() == ShippingTable.Rows[j]["Shippingmethod"].ToString().ToLower())
                //                            {
                //                                ShippingTable.Rows[j]["Price"] = Convert.ToDouble(Convert.ToDouble(ShippingTable.Rows[j]["Price"]) + Convert.ToDouble(USPSTable.Rows[k]["Price"].ToString()));

                //                                ShippingTable.Rows[j]["ShippingMethodName"] = "USPS - " + ShippingTable.Rows[j]["Shippingmethod"].ToString() + " ($" + Math.Round(Convert.ToDecimal(ShippingTable.Rows[j]["Price"].ToString()), 2).ToString() + ")";

                //                            }
                //                        }

                //                    }
                //                    else
                //                    {
                //                        ShippingTable.Rows.RemoveAt(j);
                //                        ShippingTable.AcceptChanges();
                //                    }


                //                }
                //                USPScnt = 0;
                //                for (int k = 0; k < ShippingTable.Rows.Count; k++)
                //                {
                //                    USPScnt += 3;
                //                    ShippingTable.Rows[k]["sn"] = USPScnt;
                //                }
                //            }
                //            else
                //            {
                //                USPSTable.DefaultView.Sort = "Price asc";
                //                for (int cntUSPS = 0; cntUSPS < USPSTable.DefaultView.ToTable().Rows.Count; cntUSPS++)
                //                {
                //                    USPScnt += 3;
                //                    DataRow dataRow = ShippingTable.NewRow();
                //                    dataRow["sn"] = USPScnt;
                //                    dataRow["Shippingmethod"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["Shippingmethod"].ToString();
                //                    dataRow["ShippingMethodName"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["ShippingMethodName"].ToString();
                //                    dataRow["Price"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["Price"].ToString();
                //                    ShippingTable.Rows.Add(dataRow);

                //                }
                //            }

                //        }
                //    }

                //    // New end


                //}


                //* if (USPSTable != null && USPSTable.Rows.Count > 0)
                //* {
                //*    ShippingTable.Merge(USPSTable);
                //* }


                //if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                //{
                //    DataView dvShipping = ShippingTable.DefaultView;
                //    dvShipping.Sort = "Price asc";

                //    rdRadioForShipping.DataSource = dvShipping.ToTable();
                //    rdRadioForShipping.DataTextField = "ShippingMethodName";
                //    rdRadioForShipping.DataValueField = "ShippingMethodName";
                //    rdRadioForShipping.RepeatDirection = RepeatDirection.Horizontal;
                //    rdRadioForShipping.DataBind();
                //    rdRadioForShipping.SelectedIndex = 0;
                //}
            }
            //if (objShipServices.Tables[0].Select("ShippingService='UPS'").Length > 0)
            //{
            //    // ViewState["Zip"].ToString(), CountryCode.ToString()
            //    UPSTable = UPSMethodBind(CountryCode.ToString(), ViewState["State"].ToString(), ViewState["Zip"].ToString(), Weight, "UPS", Convert.ToInt32(ddlWareHouse.SelectedItem.Value), false, ref strUPSMessage);


            //    if (UPSTable != null && UPSTable.Rows.Count > 0)
            //    {
            //    }
            //    else { panUPS.Visible = false; grdUPS.Visible = false; }

            //}

            if (objShipServices.Tables[0].Select("ShippingService='FEDEX'").Length > 0)
            {
                // ViewState["Zip"].ToString(), CountryCode.ToString()
                FEDEXTable = FedExMethodBind(CountryCode.ToString(), ViewState["State"].ToString(), ViewState["Zip"].ToString(), Weight, "UPS", Convert.ToInt32(ddlWareHouse.SelectedItem.Value), false, ref strUPSMessage);

                if (FEDEXTable != null && FEDEXTable.Rows.Count > 0)
                {
                }
                else { panUPS.Visible = false; grdUPS.Visible = false; }
            }


            if (UPSTable != null && UPSTable.Rows.Count > 0)
            {
                ShippingTable.Merge(UPSTable);
            }

            if (FEDEXTable != null && FEDEXTable.Rows.Count > 0)
            {
                ShippingTable.Merge(FEDEXTable);
            }

            //if (UPSTable == null && UPSTable.Rows.Count == 0 || FEDEXTable == null && FEDEXTable.Rows.Count == 0 || USPSTable == null && USPSTable.Rows.Count == 0)
            //{
            //    panSelectMethod.Visible = false;
            //}

            int max = 0;
            int min = 0;
            DataTable dtNew = new DataTable();

            if (ShippingTable.Rows.Count > 0)
            {
                dtNew = ShippingTable;
                dtNew.DefaultView.Sort = "sn asc";
                max = Convert.ToInt32(dtNew.DefaultView.ToTable().Rows[dtNew.DefaultView.ToTable().Rows.Count - 1]["sn"].ToString());
                min = Convert.ToInt32(dtNew.DefaultView.ToTable().Rows[0]["sn"].ToString());
            }

            if (max != min)
            {
                for (int i = min; i < max; i++)
                {
                    bool falg = false;
                    for (int j = 0; j < dtNew.Rows.Count; j++)
                    {
                        int inum = 0;
                        if (dtNew.Rows[j]["sn"].ToString() == i.ToString())
                        {
                            falg = true;
                        }
                    }
                    if (falg == false)
                    {
                        DataRow dataRow = ShippingTable.NewRow();
                        dataRow["sn"] = Convert.ToInt32(i.ToString());
                        dataRow["ShippingMethodName"] = "0";
                        dataRow["Shippingmethod"] = "Unknown Method";
                        ShippingTable.Rows.Add(dataRow);
                    }
                }
            }
            ViewState["lastTable"] = ShippingTable;
            ShippingTable.DefaultView.Sort = "sn asc";
            if (ShippingTable != null && ShippingTable.Rows.Count > 0)
            {
                ListItem itemMethod = null;
                foreach (DataRow drMethod in ShippingTable.DefaultView.ToTable().Rows)
                {
                    itemMethod = new ListItem(drMethod["ShippingMethodName"].ToString(), drMethod["ShippingMethodName"].ToString());
                    if (drMethod["ShippingMethodName"].ToString() == "0")
                        itemMethod.Attributes.Add("style", "display:none;");
                    rdRadioForShipping.Items.Add(itemMethod);
                }
                panSelectMethod.Visible = true;
                rdRadioForShipping.RepeatDirection = RepeatDirection.Horizontal;
                rdRadioForShipping.SelectedIndex = 0;
            }

            if (strUSPSMessage != "" && strUPSMessage != "")
            {
                lblmsg.Text = strUPSMessage + strUSPSMessage;
                lblmsg.Visible = true;
            }
            else if (strUSPSMessage != "")
            {
                lblmsg.Text = strUSPSMessage;
                lblmsg.Visible = true;
            }

            if (rdRadioForShipping.Items.Count == 0)
            { panweight.Visible = false; MsgShippingMethods.Visible = false; }
            
        }

        /// <summary>
        /// Gets the new shipping.
        /// </summary>
        private void GetNewshipping()
        {
            //
            if (rdRadioForShipping.Items.Count > 0 && ViewState["lastTable"] != null)
            {
                int SelInd = rdRadioForShipping.SelectedIndex;
                rdRadioForShipping.Items.Clear();
                DataTable myTable = (DataTable)ViewState["lastTable"];
                myTable.DefaultView.Sort = "sn asc";
                ListItem itemMethod = null;
                foreach (DataRow drMethod in myTable.DefaultView.ToTable().Rows)
                {
                    itemMethod = new ListItem(drMethod["ShippingMethodName"].ToString(), drMethod["ShippingMethodName"].ToString());
                    if (drMethod["ShippingMethodName"].ToString() == "0")
                        itemMethod.Attributes.Add("style", "display:none;");
                    rdRadioForShipping.Items.Add(itemMethod);
                }
                //panSelectMethod.Visible = true;
                rdRadioForShipping.RepeatDirection = RepeatDirection.Horizontal;
                rdRadioForShipping.SelectedIndex = SelInd;
            }

            if (grdUPS.Rows.Count <= 0)
            {
                panUPS.Visible = false;

            }

            if (grdUSPS.Rows.Count <= 0)
            {
                panUSPS.Visible = false;
            }

            if (grdFEDEX.Rows.Count <= 0)
            {
                grdFEDEX.Visible = false;
            }

        }

        /// <summary>
        /// Binds the UPS Methods.
        /// </summary>
        /// <param name="WareHouseID">int WareHouseID</param>
        /// <returns>Returns the UPS Methods Data</returns>
        private DataTable BindUPS(int WareHouseID)
        {
            string strUPSMessage = "";
            string OrgShippingZip = "";
            string OrgCountry = "";
            string sqlWareHouse = "SELECT ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse  INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + WareHouseID.ToString();
            DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
            if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
            {
                OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
                OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
            }

            DataTable UPSTable = new DataTable();
            UPSTable.Columns.Add("sn", typeof(int));
            UPSTable.Columns.Add("ShippingMethodName", typeof(String));
            UPSTable.Columns.Add("Price", typeof(decimal));
            UPSTable.Columns.Add("Shippingmethod", typeof(String));

            ShippingComponent objShipping = new ShippingComponent();
            DataSet objShipServices = new DataSet();
            objShipServices = objShipping.GetShippingServices(Convert.ToInt32(ViewState["StoreID"].ToString()));

            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(ViewState["CountryName"].ToString()));
            decimal Weight = decimal.Zero;
            Weight = Convert.ToDecimal(txtWeight.Text);

            if (Weight == 0)
            {
                Weight = 1;
            }

            if (objShipServices.Tables[0].Select("ShippingService='UPS'").Length > 0)
            {
                // ViewState["Zip"].ToString(), CountryCode.ToString()
                UPSTable = UPSMethodBind(CountryCode.ToString(), ViewState["State"].ToString(), ViewState["Zip"].ToString(), Weight, "UPS", WareHouseID, true, ref strUPSMessage);
            }

            lblshiperror.Visible = false;
            lblshiperror.Text = "";
            return UPSTable;
        }

        /// <summary>
        /// Binds the USPS Methods.
        /// </summary>
        /// <param name="WareHouseID">int WareHouseID</param>
        /// <returns>Returns the USPS Methods Data</returns>
        private DataTable BindUSPS(int WareHouseID)
        {
            int USPScnt = 0;
            string OrgShippingZip = "";
            string OrgCountry = "";
            string sqlWareHouse = "SELECT ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse  INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + WareHouseID.ToString();
            DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
            if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
            {
                OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
                OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
            }

            rdRadioForShipping.Items.Clear();
            string strUSPSMessage = "";
            string strUPSMessage = "";
            lblmsg.Text = "";
            decimal decTotalWeight = 0;

            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("sn", typeof(int));
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            ShippingTable.Columns.Add("Shippingmethod", typeof(String));

            DataTable USPSTable = new DataTable();
            USPSTable.Columns.Add("sn", typeof(int));
            USPSTable.Columns.Add("ShippingMethodName", typeof(String));
            USPSTable.Columns.Add("Price", typeof(decimal));
            USPSTable.Columns.Add("Shippingmethod", typeof(String));

            ShippingComponent objShipping = new ShippingComponent();
            DataSet objShipServices = new DataSet();
            objShipServices = objShipping.GetShippingServices(Convert.ToInt32(ViewState["StoreID"].ToString()));

            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(ViewState["CountryName"].ToString()));
            decimal Weight = decimal.Zero;
            Weight = Convert.ToDecimal(txtWeight.Text);

            if (Weight == 0)
            {
                Weight = 1;
            }

            if (objShipServices != null && objShipServices.Tables.Count > 0 && objShipServices.Tables[0].Rows.Count > 0)
            {

                if (objShipServices.Tables[0].Select("ShippingService='USPS'").Length > 0)
                {
                    EndiciaService objRate = new EndiciaService();
                    foreach (GridViewRow gvr in grdShipping.Rows)
                    {
                        System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");
                        System.Web.UI.WebControls.TextBox txtProductPrice = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProductPrice");
                       //* System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                        System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                        System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");

                        int Qty = 0;
                        try { Qty = Convert.ToInt32(lblQuantity.Text); }
                        catch { Qty = 0; }
                       //* int availQuantity = 0;
                        //try
                        //{
                        //    availQuantity = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) FROM tb_WareHouseProductInventory WHERE ProductID= 1 AND WareHouseID=1"));
                        //    if (availQuantity <= 0)
                        //    {
                        //        availQuantity = 0;
                        //    }
                        //}
                        //catch
                        //{
                        //    availQuantity = 0;
                        //}

                       // if (Qty > availQuantity) { availQuantity = 0; }

                        if (!chkAllowShipment.Checked)
                        {
                            continue;
                        }

                        Decimal ProWeight = Convert.ToDecimal("1");

                        System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                        // System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");

                        //  if (txtProductPrice.Text != "0")
                        //if (availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                        if (lblShipping.Text.ToLower() == "no")
                            decTotalWeight += Convert.ToDecimal(string.IsNullOrEmpty(txtProWeight.Text) ? "0" : txtProWeight.Text);

                        //if (availQuantity != 0 && lblShipping.Text.ToLower() == "no" && txtProWeight != null && !string.IsNullOrEmpty(txtProWeight.Text))
                        if (lblShipping.Text.ToLower() == "no" && txtProWeight != null && !string.IsNullOrEmpty(txtProWeight.Text))
                            ProWeight = Convert.ToDecimal(txtProWeight.Text);


                        USPSTable = objRate.EndiciaGetRatesAdminWarehouseShippingLabel(CountryCode, OrgShippingZip, ViewState["Zip"].ToString(), CountryCode.ToString(), Convert.ToDouble(ProWeight), ref strUSPSMessage);
                        if (USPSTable != null && USPSTable.Rows.Count > 0)
                        {

                            if (ShippingTable.Rows.Count > 0)
                            {
                                for (int j = 0; j < ShippingTable.Rows.Count; j++)
                                {
                                    if (USPSTable.Select("Shippingmethod ='" + ShippingTable.Rows[j]["Shippingmethod"].ToString() + "'").Length > 0)
                                    {
                                        for (int k = 0; k < USPSTable.Rows.Count; k++)
                                        {
                                            if (USPSTable.Rows[k]["Shippingmethod"].ToString().ToLower() == ShippingTable.Rows[j]["Shippingmethod"].ToString().ToLower())
                                            {
                                                ShippingTable.Rows[j]["Price"] = Convert.ToDouble(Convert.ToDouble(ShippingTable.Rows[j]["Price"]) + Convert.ToDouble(USPSTable.Rows[k]["Price"].ToString()));

                                                ShippingTable.Rows[j]["ShippingMethodName"] = "USPS - " + ShippingTable.Rows[j]["Shippingmethod"].ToString() + " ($" + Math.Round(Convert.ToDecimal(ShippingTable.Rows[j]["Price"].ToString()), 2).ToString() + ")";

                                            }
                                        }

                                    }
                                    else
                                    {
                                        ShippingTable.Rows.RemoveAt(j);
                                        ShippingTable.AcceptChanges();
                                    }


                                }
                            }
                            else
                            {
                                USPSTable.DefaultView.Sort = "Price asc";
                                for (int cntUSPS = 0; cntUSPS < USPSTable.DefaultView.ToTable().Rows.Count; cntUSPS++)
                                {
                                    USPScnt += 2;
                                    DataRow dataRow = ShippingTable.NewRow();
                                    dataRow["sn"] = USPScnt;
                                    dataRow["Shippingmethod"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["Shippingmethod"].ToString();
                                    dataRow["ShippingMethodName"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["ShippingMethodName"].ToString();
                                    dataRow["Price"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["Price"].ToString();
                                    ShippingTable.Rows.Add(dataRow);

                                }
                            }

                        }
                    }

                    // For unchecked product
                    decTotalWeight = Convert.ToDecimal(hfWeight.Value);
                    try
                    {
                        decimal w = Convert.ToDecimal(txtWeight.Text.Trim());
                    }
                    catch (Exception)
                    {
                        txtWeight.Text = decTotalWeight.ToString();

                    }
                    if (Convert.ToDecimal(txtWeight.Text) > Convert.ToDecimal(0.00))
                    {
                        Weight = Convert.ToDecimal(txtWeight.Text);
                        USPSTable = null;

                        USPSTable = objRate.EndiciaGetRatesAdminWarehouseShippingLabel(CountryCode, OrgShippingZip, ViewState["Zip"].ToString(), CountryCode.ToString(), Convert.ToDouble(Weight), ref strUSPSMessage);
                        if (USPSTable != null && USPSTable.Rows.Count > 0)
                        {

                            if (ShippingTable.Rows.Count > 0)
                            {
                                for (int j = 0; j < ShippingTable.Rows.Count; j++)
                                {
                                    if (USPSTable.Select("Shippingmethod ='" + ShippingTable.Rows[j]["Shippingmethod"].ToString() + "'").Length > 0)
                                    {
                                        for (int k = 0; k < USPSTable.Rows.Count; k++)
                                        {
                                            if (USPSTable.Rows[k]["Shippingmethod"].ToString().ToLower() == ShippingTable.Rows[j]["Shippingmethod"].ToString().ToLower())
                                            {
                                                ShippingTable.Rows[j]["Price"] = Convert.ToDouble(Convert.ToDouble(ShippingTable.Rows[j]["Price"]) + Convert.ToDouble(USPSTable.Rows[k]["Price"].ToString()));

                                                ShippingTable.Rows[j]["ShippingMethodName"] = "USPS - " + ShippingTable.Rows[j]["Shippingmethod"].ToString() + " ($" + Math.Round(Convert.ToDecimal(ShippingTable.Rows[j]["Price"].ToString()), 2).ToString() + ")";

                                            }
                                        }

                                    }
                                    else
                                    {
                                        ShippingTable.Rows.RemoveAt(j);
                                        ShippingTable.AcceptChanges();
                                    }


                                }
                            }
                            else
                            {
                                USPSTable.DefaultView.Sort = "Price asc";
                                for (int cntUSPS = 0; cntUSPS < USPSTable.DefaultView.ToTable().Rows.Count; cntUSPS++)
                                {
                                    USPScnt += 3;
                                    DataRow dataRow = ShippingTable.NewRow();
                                    dataRow["sn"] = USPScnt;
                                    dataRow["Shippingmethod"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["Shippingmethod"].ToString();
                                    dataRow["ShippingMethodName"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["ShippingMethodName"].ToString();
                                    dataRow["Price"] = USPSTable.DefaultView.ToTable().Rows[cntUSPS]["Price"].ToString();
                                    ShippingTable.Rows.Add(dataRow);

                                }
                            }

                        }
                    }

                    // New end
                }
            }
            lblshiperror.Visible = false;
            lblshiperror.Text = "";
            return ShippingTable;
        }

        /// <summary>
        /// Binds the FEDEX Methods
        /// </summary>
        /// <param name="WareHouseID">int WareHouseID</param>
        /// <returns>Returns the FEDEX Methods Data</returns>
        private DataTable BindFEDEX(int WareHouseID)
        {
            string strUPSMessage = "";
            string OrgShippingZip = "";
            string OrgCountry = "";
            string sqlWareHouse = "SELECT ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse  INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + WareHouseID.ToString();
            DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
            if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
            {
                OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
                OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
            }

            DataTable FEDEXTable = new DataTable();
            FEDEXTable.Columns.Add("sn", typeof(int));
            FEDEXTable.Columns.Add("ShippingMethodName", typeof(String));
            FEDEXTable.Columns.Add("Price", typeof(decimal));
            FEDEXTable.Columns.Add("Shippingmethod", typeof(String));

            ShippingComponent objShipping = new ShippingComponent();
            DataSet objShipServices = new DataSet();
            objShipServices = objShipping.GetShippingServices(Convert.ToInt32(ViewState["StoreID"].ToString()));

            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(ViewState["CountryName"].ToString()));
            decimal Weight = decimal.Zero;
            Weight = Convert.ToDecimal(txtWeight.Text);

            if (Weight == 0)
            {
                Weight = 1;
            }

            if (objShipServices.Tables[0].Select("ShippingService='FEDEX'").Length > 0)
            {
                // ViewState["Zip"].ToString(), CountryCode.ToString()
                FEDEXTable = FedExMethodBind(CountryCode.ToString(), ViewState["State"].ToString(), ViewState["Zip"].ToString(), Weight, "UPS", WareHouseID, true, ref strUPSMessage);
            }

            lblshiperror.Visible = false;
            lblshiperror.Text = "";
            return FEDEXTable;
        }

        /// <summary>
        /// Bind UPS the Method
        /// </summary>
        /// <param name="Country">string Country</param>
        /// <param name="State">string State</param>
        /// <param name="ZipCode">string ZipCode</param>
        /// <param name="Weight">decimal Weight</param>
        /// <param name="ServiceName">string ServiceName</param>
        /// <param name="WareHouseID">int WareHouseID</param>
        /// <param name="ISMulti">bool ISMulti</param>
        /// <param name="StrMessage">string StrMessage</param>
        /// <returns>Returns the UPS the method Dataset.</returns>
        private DataTable UPSMethodBind(string Country, string State, string ZipCode, decimal Weight, string ServiceName, int WareHouseID, bool ISMulti, ref string StrMessage)
        {
            //if (ZipCode == "" || Country == "")
            //{
            //    return null;
            //}
            int UPScnt = 1;
            int PackageID = 1;
            Decimal remainingItemsInsuranceValue = 0.0M;
            Decimal TotalPrice = 0;
            decimal decTotalWeight = 0;
            string OrgShippingZip = "";
            string OrgCountry = "";
            string OrgAddress1 = "";
            string OrgAddress2 = "";
            string OrgCity = "";
            string OrgState = "";
            string sqlWareHouse = "SELECT Address1,Address2,City,State,ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse  INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + WareHouseID.ToString();
            DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
            if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
            {

                OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
                OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
                OrgAddress1 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address1"]);
                OrgAddress2 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address2"]);
                OrgCity = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["City"]);
                OrgState = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["State"]);
            }
            CountryComponent objCountry = new CountryComponent();
            OrgCountry = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(OrgCountry));

            StateComponent objState = new StateComponent();
            OrgState = Convert.ToString(objState.GetStateCodeByName(OrgState));

            DataTable UPSTable = new DataTable();
            UPSTable.Columns.Add("sn", typeof(int));
            UPSTable.Columns.Add("ShippingMethodName", typeof(String));
            UPSTable.Columns.Add("Price", typeof(decimal));
            UPSTable.Columns.Add("Shippingmethod", typeof(String));

            UPS obj = new UPS(OrgAddress1, OrgAddress2, OrgCity, OrgState, OrgShippingZip, OrgCountry);
            obj.DestinationCountryCode = Country;
            obj.DestinationStateProvince = State;
            obj.DestinationZipPostalCode = ZipCode;

            UPS.Packages UPSshipment = new UPS.Packages();

            UPSshipment.PickupType = "UPSDailyPickup";
            UPSshipment.DestinationZipPostalCode = ZipCode;
            UPSshipment.DestinationCountryCode = Country;
            UPSshipment.DestinationStateProvince = "";
            UPSshipment.DestinationResidenceType = ResidenceTypes.Residential;
            obj.DestinationResidenceType = UPSshipment.DestinationResidenceType;

            StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            StringBuilder tmpFixedShipping = new StringBuilder(4096);

            foreach (GridViewRow gvr in grdShipping.Rows)
            {
                System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");

                if (!chkAllowShipment.Checked)
                    continue;
                Decimal ProWeight = Convert.ToDecimal("1");

                System.Web.UI.WebControls.TextBox txtHeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtHeightgrid");
                System.Web.UI.WebControls.TextBox txtWidth = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtWidthtgrid");
                System.Web.UI.WebControls.TextBox txtLength = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtLengthtgrid");
                System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");
              //*  System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");

                int Qty = 0;
               // int availQuantity = 0;
               // try { Qty = Convert.ToInt32(lblQuantity.Text); }
               // catch { Qty = 0; }



                if (ISMulti == true)
                {
                    //try
                    //{
                    //    availQuantity = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) FROM tb_WareHouseProductInventory WHERE ProductID= 1 AND WareHouseID=1"));
                    //    if (availQuantity <= 0)
                    //    {
                    //        availQuantity = 0;
                    //    }
                    //}
                    //catch
                    //{
                    //    availQuantity = 0;
                    //}
                }
                else
                {
                   // try { availQuantity = Convert.ToInt32(lblavailQuantity.Text); }
                   // catch { availQuantity = 0; }
                }
                //if (Qty > availQuantity)
                    //availQuantity = 0;


                //if (availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                if (lblShipping.Text.ToLower() == "no")
                    decTotalWeight += Convert.ToDecimal(string.IsNullOrEmpty(txtProWeight.Text) ? "0" : txtProWeight.Text);

                UPS.Package p = new UPS.Package();
                p.PackageId = PackageID;
                PackageID += 1;


                if (txtHeight != null && !string.IsNullOrEmpty(txtHeight.Text))
                    p.Height = Convert.ToDecimal(txtHeight.Text);
                if (txtWidth != null && !string.IsNullOrEmpty(txtWidth.Text))
                    p.Width = Convert.ToDecimal(txtWidth.Text);
                if (txtLength != null && !string.IsNullOrEmpty(txtLength.Text))
                    p.Length = Convert.ToDecimal(txtLength.Text);
                if (txtProWeight != null && !string.IsNullOrEmpty(txtProWeight.Text))
                    ProWeight = Convert.ToDecimal(txtProWeight.Text);
                p.Weight = ProWeight;
                p.Insured = chkInsured.Checked; //false;
                p.InsuredValue = remainingItemsInsuranceValue;
                UPSshipment.AddPackage(p);
                p = null;
            }
            decTotalWeight = Convert.ToDecimal(hfWeight.Value);
            try
            {
                decimal w = Convert.ToDecimal(txtWeight.Text.Trim());
            }
            catch (Exception)
            {
                txtWeight.Text = decTotalWeight.ToString();

            }

            if (Convert.ToDecimal(txtWeight.Text) > Convert.ToDecimal(0.00))
            {
                UPS.Package p = new UPS.Package();
                p.PackageId = PackageID;
                PackageID += 3;

                if (!string.IsNullOrEmpty(txtHeight.Text))
                    p.Height = Convert.ToDecimal(txtHeight.Text);
                if (!string.IsNullOrEmpty(txtWidth.Text))
                    p.Width = Convert.ToDecimal(txtWidth.Text);
                if (!string.IsNullOrEmpty(txtLength.Text))
                    p.Length = Convert.ToDecimal(txtLength.Text);
                //p.Weight = decTotalWeight;
                p.Weight = Convert.ToDecimal(txtWeight.Text);
                p.Insured = false;
                UPSshipment.AddPackage(p);
            }

            if (Convert.ToDecimal(txtWeight.Text) > Convert.ToDecimal(0.00) || Weight > Convert.ToDecimal(0.00))
            {
                tmpRealTimeShipping.Append((string)obj.UPSGetRatesPackage(UPSshipment));
            }

            string strResult = tmpRealTimeShipping.ToString();


            string[] strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strResult.ToString().ToLower().IndexOf("error") > -1)
            {
                lblshiperror.Visible = true;

                lblshiperror.Text += strResult + "<br />";

                //new added on 4 Mar 13

                //MsgShippingMethods.Visible = false;
                //panweight.Visible=false;
            }
            else
            {
                strResult = tmpFixedShipping.ToString() + strResult;
                strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strMethod.Length; i++)
                {
                    string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string Shippingname = "";
                    if (strMethodname.Length > 0)
                    {
                        Shippingname = strMethodname[0].ToString();
                    }
                    if (strMethodname.Length > 1)
                    {
                        Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                    }
                    // rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));
                    UPScnt += 3;
                    //if (ShippingTable.Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in ShippingTable.Rows)
                    //    {
                    //        if (Convert.ToInt32(dr["sn"]) == UPScnt)
                    //        {
                    //            UPScnt += 1;
                    //        }
                    //    }
                    //}



                    DataRow dataRow = UPSTable.NewRow();

                    dataRow["sn"] = UPScnt;
                    dataRow["Shippingmethod"] = Shippingname;
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                    UPSTable.Rows.Add(dataRow);

                }

            }

            return UPSTable;

        }

        /// <summary>
        /// Bind FedEx Methods
        /// </summary>
        /// <param name="Country">string Country</param>
        /// <param name="State">string State</param>
        /// <param name="ZipCode">string ZipCode</param>
        /// <param name="Weight">decimal Weight</param>
        /// <param name="ServiceName">string ServiceName</param>
        /// <param name="WareHouseID">int WareHouseID</param>
        /// <param name="ISMulti">bool ISMulti</param>
        /// <param name="StrMessage">string StrMessage</param>
        /// <returns>Returns the FedEx the method Dataset.</returns>
        private DataTable FedExMethodBind(string Country, string State, string ZipCode, decimal Weight, string ServiceName, int WareHouseID, bool ISMulti, ref string StrMessage)
        {
            int FEDEXcnt = 2;
            decimal decTotalWeight = 0;
            string OrgShippingZip = "";
            string OrgCountry = "";
            string OrgAddress1 = "";
            string OrgAddress2 = "";
            string OrgCity = "";
            string OrgState = "";
            string sqlWareHouse = "SELECT Address1,Address2,City,State,ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse  INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + WareHouseID.ToString();
            DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
            if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
            {
                OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
                OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
                OrgAddress1 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address1"]);
                OrgAddress2 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address2"]);
                OrgCity = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["City"]);
                OrgState = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["State"]);
            }
            CountryComponent objCountry = new CountryComponent();
            OrgCountry = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(OrgCountry));

            StateComponent objState = new StateComponent();
            OrgState = Convert.ToString(objState.GetStateCodeByName(OrgState));

            DataTable FEDEXTable = new DataTable();
            FEDEXTable.Columns.Add("sn", typeof(int));
            FEDEXTable.Columns.Add("ShippingMethodName", typeof(String));
            FEDEXTable.Columns.Add("Price", typeof(decimal));
            FEDEXTable.Columns.Add("Shippingmethod", typeof(String));

            DataTable tempFEDEXTable = new DataTable();
            tempFEDEXTable.Columns.Add("sn", typeof(int));
            tempFEDEXTable.Columns.Add("ShippingMethodName", typeof(String));
            tempFEDEXTable.Columns.Add("Price", typeof(decimal));
            tempFEDEXTable.Columns.Add("Shippingmethod", typeof(String));

            //StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            StringBuilder tmpFixedShipping = new StringBuilder(4096);

            foreach (GridViewRow gvr in grdShipping.Rows)
            {
                System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");

                if (!chkAllowShipment.Checked)
                    continue;
                Decimal ProWeight = Convert.ToDecimal("1");

                System.Web.UI.WebControls.TextBox txtHeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtHeightgrid");
                System.Web.UI.WebControls.TextBox txtWidth = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtWidthtgrid");
                System.Web.UI.WebControls.TextBox txtLength = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtLengthtgrid");
                System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");
               //* System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");

                int Qty = 0;
              //&  int availQuantity = 0;
                try { Qty = Convert.ToInt32(lblQuantity.Text); }
                catch { Qty = 0; }

                if (ISMulti == true)
                {
                    //try
                    //{
                    //    availQuantity = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) FROM tb_WareHouseProductInventory WHERE ProductID= 1 AND WareHouseID=1"));
                    //    if (availQuantity <= 0)
                    //    {
                    //        availQuantity = 0;
                    //    }
                    //}
                    //catch
                    //{
                    //    availQuantity = 0;
                    //}
                }
                else
                {
                    //try { availQuantity = Convert.ToInt32(lblavailQuantity.Text); }
                    //catch { availQuantity = 0; }
                }
                //if (Qty > availQuantity)
                //    availQuantity = 0;

                //if (availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                if (lblShipping.Text.ToLower() == "no")
                    decTotalWeight += Convert.ToDecimal(string.IsNullOrEmpty(txtProWeight.Text) ? "0" : txtProWeight.Text);
            }

            decTotalWeight = Convert.ToDecimal(hfWeight.Value);
            try
            {
                decimal w = Convert.ToDecimal(txtWeight.Text.Trim());
            }
            catch (Exception)
            {
                txtWeight.Text = decTotalWeight.ToString();

            }
            Fedex objFedex = new Fedex();
            string GetFedexrate = "";
            //  if (Weight > decimal.Zero)
            if (Convert.ToDecimal(txtWeight.Text) > Convert.ToDecimal(0.00) || Weight > Convert.ToDecimal(0.00))
            {
                //GetFedexrate = Convert.ToString(FedexGetRatesAdmin(WareHouseID, Convert.ToDecimal(txtWeight.Text), "", "", "", State, ZipCode, Country.ToString(), Convert.ToInt32(Session["CustID"]), true));
                Fedex obj = new Fedex();
               // GetFedexrate = Convert.ToString(FedexGetRatesAdmin(WareHouseID, Convert.ToDecimal(Weight), "", "", "", State, ZipCode, Country.ToString(), Convert.ToInt32(Session["CustID"]), true));
                StateComponent objCountry1 = new StateComponent();
                string Abbreviation = objCountry1.GetStateCodeByName(State.ToString());
                GetFedexrate = Convert.ToString(obj.FedexGetRates(Convert.ToDecimal(Weight), "", "", "", Abbreviation, ZipCode, Country.ToString(), Convert.ToInt32(Session["CustID"]), true));
            }
            //else
            // GetFedexrate = Convert.ToString(objFedex.FedexGetRatesAdmin(WareHouseID, Convert.ToDecimal(1), "", "", "", State, ZipCode, Country.ToString(), Convert.ToInt32(Session["CustID"]), true));

            string strResult = GetFedexrate;

            string[] strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strResult.ToString().ToLower().IndexOf("error") > -1)
            {
                lblshiperror.Visible = true;
                lblshiperror.Text += strResult + "<br />";
            }
            else
            {
                strResult = tmpFixedShipping.ToString() + strResult;
                strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (String s in strMethod)
                {
                    String s2 = s.Trim();
                    if (s2.Length != 0 && s2.IndexOf("|") != -1)
                    {
                        DataRow dataRow = tempFEDEXTable.NewRow();
                        dataRow["sn"] = 1;
                        dataRow["Shippingmethod"] = s2.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                        dataRow["ShippingMethodName"] = s2.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0] + " ($" + Math.Round(Convert.ToDecimal(s2.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]), 2).ToString() + ")";
                        dataRow["Price"] = Math.Round(Convert.ToDecimal(s2.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]), 2);
                        tempFEDEXTable.Rows.Add(dataRow);
                    }
                }

                if (tempFEDEXTable != null && tempFEDEXTable.Rows.Count > 0)
                {
                    tempFEDEXTable.DefaultView.Sort = "Price asc";
                    for (int cntFedEx = 0; cntFedEx < tempFEDEXTable.DefaultView.ToTable().Rows.Count; cntFedEx++)
                    {

                        FEDEXcnt += 3;
                        DataRow dataRow = FEDEXTable.NewRow();
                        dataRow["sn"] = FEDEXcnt;
                        dataRow["Shippingmethod"] = tempFEDEXTable.DefaultView.ToTable().Rows[cntFedEx]["Shippingmethod"].ToString();
                        dataRow["ShippingMethodName"] = tempFEDEXTable.DefaultView.ToTable().Rows[cntFedEx]["ShippingMethodName"].ToString();
                        dataRow["Price"] = tempFEDEXTable.DefaultView.ToTable().Rows[cntFedEx]["Price"].ToString();
                        FEDEXTable.Rows.Add(dataRow);
                    }
                }


                #region old code
                //for (int i = 0; i < strMethod.Length; i++)
                //{
                //    string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //    string Shippingname = "";
                //    if (strMethodname.Length > 0)
                //    {
                //        Shippingname = strMethodname[0].ToString();
                //    }
                //    if (strMethodname.Length > 1)
                //    {
                //        Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                //    }

                //    FEDEXcnt += 3;
                //    DataRow dataRow = FEDEXTable.NewRow();
                //    dataRow["sn"] = FEDEXcnt;
                //    dataRow["Shippingmethod"] = Shippingname;
                //    dataRow["ShippingMethodName"] = Shippingname;
                //    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                //    FEDEXTable.Rows.Add(dataRow);
                //} 
                #endregion

            }

            return FEDEXTable;

        }

        /// <summary>
        ///  Ship Method Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnshipmethod_Click(object sender, EventArgs e)
        {
            panUSPS.Visible = false;
            string OrderNumber = "0";

            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
            }
            ShoppingCartComponent ObjOCart = new ShoppingCartComponent();
            DataSet DsCItems = new DataSet();
            DsCItems = ObjOCart.GetOrderedShoppingCartItemsByCartId((Convert.ToInt32(OrderNumber)));

            warehouseinventoryMsg.Text = "";
            bool isavailQty = false;
            int chkQty = 0;
            int chkQtylabel = 0;
            if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
            {
                panSelectMethod.Visible = true;

                foreach (GridViewRow gvr in grdShipping.Rows)
                {
                   //* System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                    System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                    System.Web.UI.WebControls.Label lblSKU = (System.Web.UI.WebControls.Label)gvr.FindControl("lblSKU");

                    System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");

                    int Qty = 0;
                   // int availQuantity = 0;
                    try { Qty = Convert.ToInt32(lblQuantity.Text); }
                    catch { Qty = 0; }
                    //try { availQuantity = Convert.ToInt32(lblavailQuantity.Text); }
                    //catch { availQuantity = 0; }

                    //if (Qty > availQuantity) { availQuantity = 0; }

                    //if (lblShipping.Text.ToLower() == "yes")//
                    //{
                    //    chkQtylabel += 1;
                    //}


                    //if (availQuantity == 0 && lblShipping.Text.ToLower() == "no")
                    //if (lblShipping.Text.ToLower() == "no")
                    //{
                    //    chkQty += 1;
                    //    warehouseinventoryMsg.Text += lblSKU.Text + ",";
                    //}
                }

                if (chkQtylabel == DsCItems.Tables[0].Rows.Count)
                {

                    warehouseinventoryMsg.Text = "Shipping label already generated";
                    warehouseinventoryMsg.Visible = true;

                    panSelectMethod.Visible = false;
                    lblshiperror.Visible = false; ;
                }
                else  if (chkQty == DsCItems.Tables[0].Rows.Count)
                {
                    if (warehouseinventoryMsg.Text.Length > 0)
                        warehouseinventoryMsg.Text = warehouseinventoryMsg.Text.Substring(0, (warehouseinventoryMsg.Text.Length - 1));
                    warehouseinventoryMsg.Text = "No Inventory for SKU " + warehouseinventoryMsg.Text;
                    warehouseinventoryMsg.Visible = true;

                    panSelectMethod.Visible = false;
                    lblshiperror.Visible = false; ;
                }
                else
                {
                    warehouseinventoryMsg.Visible = false;
                    BindShippingMethod();

                }
                //   for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                //{
                //    string warehouseinventory = "SELECT Inventory from dbo.tb_WareHouseProductInventory where ProductID=" + DsCItems.Tables[0].Rows[i]["ProductID"] + " and WareHouseID='" + ddlWareHouse.SelectedItem.Value + "'";
                //    Int32 winventory = Convert.ToInt32(CommonComponent.GetScalarCommonData(warehouseinventory));
                //    if (winventory == 0)
                //    {
                //        warehouseinventoryMsg.Visible = true;

                //        warehouseinventoryMsg.Text += DsCItems.Tables[0].Rows[i]["SKU"].ToString() + ",";

                //        isavailQty = true;
                //    }

                //}
                //if (isavailQty == false)
                //{
                // BindShippingMethod();
                //}
                //else
                //{
                //    warehouseinventoryMsg.Text = warehouseinventoryMsg.Text.Substring(0, (warehouseinventoryMsg.Text.Length - 1));
                //    // warehouseinventoryMsg.Text = "No Inventory for SKU " + warehouseinventoryMsg.Text;
                //    warehouseinventoryMsg.Text = ddlWareHouse.SelectedItem.Text + "- Not enough inventory available for SKU" + warehouseinventoryMsg.Text;
                //}
            }
            //GetExistingFilesUPS(OrderNumber);
          //  GetExistingFilesUSPS(OrderNumber);
            GetExistingFilesFEDEX(OrderNumber);


        }

        /// <summary>
        ///  Get label Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGetLabel_Click(object sender, EventArgs e)
        {
            try
            {
                lblmsg.Text = "";
                int chkQty = 0;
                string OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                if (Request.QueryString["ONo"] != null)
                {
                    if (rdRadioForShipping.SelectedValue != "0")
                    {

                        panSelectMethod.Visible = false;
                        foreach (GridViewRow gvr in grdShipping.Rows)
                        {
                            // System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                            System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                            System.Web.UI.WebControls.Label lblSKU = (System.Web.UI.WebControls.Label)gvr.FindControl("lblSKU");

                            //if (lblShipping.Text.ToLower() == "yes")
                            //{
                            //    chkQty += 1;
                            //}
                        }

                        if (chkQty == grdShipping.Rows.Count)
                        {
                            lblmsg.Text = "Shipping label is already generated, if you want to recreated, please remove old shipping label";
                            return;
                        }

                        if (rdRadioForShipping.SelectedItem.Text.Contains("USPS"))
                        {
                            panUSPS.Visible = true;

                            string sql = "select * from tb_Order where OrderNumber=" + OrderNumber;
                            DataSet dsOrder = CommonComponent.GetCommonDataSet(sql);

                            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                            {
                                if (ViewState["Country"] != null && AppLogic.AppConfigs("Shipping.OriginCountry").ToString() != ViewState["Country"].ToString())
                                {
                                    getUSPSEndicianInternationalShippingLabel(dsOrder);
                                }
                                else
                                {
                                    getUSPSEndicianShippingLabel(dsOrder);
                                }
                            }
                        }
                        else if (rdRadioForShipping.SelectedItem.Text.Contains("UPS"))
                        {
                            getUPSShippingLabel(OrderNumber);
                        }
                        else if (rdRadioForShipping.SelectedItem.Text.Contains("FEDEX"))
                        {
                            string sql = "select * from tb_Order where OrderNumber=" + OrderNumber;
                            DataSet dsOrder = CommonComponent.GetCommonDataSet(sql);
                            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                            {
                                getFedExLabel(dsOrder);
                            }
                        }
                    }
                    else
                    {
                        lblmsg.Text = "Please Select Known Shipping Method";
                        return;
                    }
                }

                if (grdUPS.Rows.Count > 0)
                    BindData(Convert.ToInt32(OrderNumber));
                if (grdUSPS.Rows.Count > 0)
                    BindData(Convert.ToInt32(OrderNumber));
                if (grdFEDEX.Rows.Count > 0)
                    BindData(Convert.ToInt32(OrderNumber));
            }
            catch { }

        }

        /// <summary>
        /// Gets the UPS shipping label.
        /// </summary>
        /// <param name="OrderNo">string OrderNo</param>
        private void getUPSShippingLabel(string OrderNo)
        {
            try
            {
                string Productid = "";
                string AllProductid = "";
                #region UPS loginInfo

                string strShoppingCartID = string.Empty;
                Object ObjCartID = CommonComponent.GetScalarCommonData("select ShoppingCardID from tb_Order where OrderNumber=" + OrderNo.ToString());
                if (ObjCartID != null)
                {
                    strShoppingCartID = ObjCartID.ToString();
                }
                string UPSUserName = AppLogic.AppConfigs("UPS.UserName");
                string UPSpassword = AppLogic.AppConfigs("UPS.Password");
                string UPSLicense = AppLogic.AppConfigs("UPS.License");
                string UPSAccountNo = AppLogic.AppConfigs("UPS.AccountNumber");
                string ImgSavePath = AppLogic.AppConfigs("UPS.LabelSavePath");
                string MsgToClient = AppLogic.AppConfigs("UPS.ShipLabelMsg");

                bool IsTestMode = Convert.ToBoolean(Convert.ToInt16((AppLogic.AppConfigs("UPS.IsTestMode").ToString() == "") ? "1" : AppLogic.AppConfigs("UPS.IsTestMode").ToString()));
                string[] UPSlogininfo = { UPSLicense, UPSUserName, UPSpassword, UPSAccountNo };
                //if (!string.IsNullOrEmpty(ImgSavePath))
                //    ImgSavePath = Server.MapPath(ImgSavePath);
                #endregion

                #region GetProducts
                #region Create Table
                DataTable myProductTable = new DataTable();
                myProductTable.Columns.Add("PackageId", typeof(Int32));
                myProductTable.Columns.Add("ProductName", typeof(String));
                myProductTable.Columns.Add("Weight", typeof(String));
                myProductTable.Columns.Add("Price", typeof(String));
                myProductTable.Columns.Add("ShippingCartID", typeof(String));
                myProductTable.Columns.Add("IsInsured", typeof(Boolean));
                myProductTable.Columns.Add("ProductID", typeof(String));
                #endregion

                decimal decFullPackagePrice = 0;
                foreach (GridViewRow gvr in grdShipping.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");

                    System.Web.UI.WebControls.Label lblProductID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductID");
                   //* System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                    System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                    System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");




                    int Qty = 0;
                  //*  int availQuantity = 0;
                    try { Qty = Convert.ToInt32(lblQuantity.Text); }
                    catch { Qty = 0; }
                    //try { availQuantity = Convert.ToInt32(lblavailQuantity.Text); }
                    //catch { availQuantity = 0; }

                    //if (Qty > availQuantity) { availQuantity = 0; }




                    //if (lblavailQuantity.Text != "0" && lblShipping.Text.ToLower() == "no")

                    //if (availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                    if (lblShipping.Text.ToLower() == "no")
                    {
                        if (!chkAllowShipment.Checked)
                            AllProductid = AllProductid + lblProductID.Text + ",";


                        if (!chkAllowShipment.Checked)
                        { continue; }

                        Decimal ProWeight = Convert.ToDecimal("1");
                        System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                        System.Web.UI.WebControls.Label lblProductName = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductName");
                        System.Web.UI.WebControls.TextBox txtProductPrice = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProductPrice");
                        System.Web.UI.WebControls.Label lblShippingCartID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShippingCartID");
                        System.Web.UI.WebControls.Label lblPackageID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblPackageID");
                        System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");


                        lblShippingCartID.Text = strShoppingCartID;
                        Productid = lblProductID.Text;

                        if (txtProWeight != null && !string.IsNullOrEmpty(txtProWeight.Text))
                            ProWeight = Convert.ToDecimal(txtProWeight.Text);
                        if (ProWeight < 1)
                            ProWeight = 1;
                        if (!string.IsNullOrEmpty(lblProductName.Text) && !string.IsNullOrEmpty(txtProductPrice.Text))
                        {
                            DataRow dataRow = myProductTable.NewRow();
                            dataRow["ProductName"] = Regex.Replace(lblProductName.Text.Trim(), @"<(.|\n)*?>", string.Empty);
                            dataRow["Weight"] = ProWeight.ToString().Trim();
                            dataRow["Price"] = txtProductPrice.Text.Trim();
                            dataRow["ShippingCartID"] = strShoppingCartID;
                            dataRow["PackageId"] = Packid.ToString();// lblPackageID.Text.Trim();
                            dataRow["IsInsured"] = chkInsured.Checked ? "TRUE" : "FALSE";
                            dataRow["ProductID"] = Productid;
                            myProductTable.Rows.Add(dataRow);

                        }
                        else
                        { decFullPackagePrice += Convert.ToDecimal(txtProductPrice.Text); }
                        if (chkAllowShipment.Checked)
                        {
                            Packid += 1;

                        }
                    }
                }
                decimal decTotalWeight = Convert.ToDecimal(hfWeight.Value);
                txtWeight.Text = hfWeight.Value;

                //    dbAccess = new SQLAccess();


                if (decTotalWeight > 0)
                {
                    if (decTotalWeight < 1)
                        decTotalWeight = 1;
                    DataRow dataRow = myProductTable.NewRow();
                    dataRow["ProductName"] = "Full Package";
                    dataRow["Weight"] = decTotalWeight.ToString();
                    dataRow["Price"] = decFullPackagePrice.ToString();
                    //if (string.IsNullOrEmpty(strShoppingCartID))
                    //    dataRow["ShippingCartID"] = "-1";
                    //else
                    dataRow["ShippingCartID"] = strShoppingCartID;
                    dataRow["PackageId"] = Packid.ToString(); //grdShipping.Rows.Count + 1;
                    dataRow["IsInsured"] = "FALSE";

                    if (AllProductid != "")
                    {
                        AllProductid = AllProductid.Substring(0, AllProductid.Length - 1);
                    }
                    dataRow["ProductID"] = AllProductid;
                    myProductTable.Rows.Add(dataRow);
                }

                #endregion

                if (!Directory.Exists(Server.MapPath(ImgSavePath)))
                    Directory.CreateDirectory(Server.MapPath(ImgSavePath));


                ImgSavePath = Server.MapPath(ImgSavePath);

                string Result = AccesslabelUPS.Main(UPSlogininfo, OrderNo, GetUPSCode(rdRadioForShipping.SelectedValue), IsTestMode, ImgSavePath, myProductTable, chkMail.Checked, Convert.ToInt32(ddlWareHouse.SelectedItem.Value));

                string DispleyStr = string.Empty;
                if (!string.IsNullOrEmpty(Result))
                {
                    if (Result.Contains("||"))
                    {
                        string[] LabelMsgType = Result.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (LabelMsgType.Length > 1)
                        {

                            #region Create Table

                            DataTable myTable = new DataTable();
                            myTable.Columns.Add("SerialNo", typeof(Int32));
                            myTable.Columns.Add("PackageID", typeof(Int32));
                            myTable.Columns.Add("ImgUrl", typeof(String));
                            myTable.Columns.Add("TrackingNo", typeof(String));
                            myTable.Columns.Add("CreateDate", typeof(DateTime));
                            myTable.Columns.Add("ShippingCartID", typeof(String));

                            #endregion

                            DispleyStr += "<br/>";
                            string TrackingNo = string.Empty;
                            string[] TrackingNoArr = LabelMsgType[0].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (TrackingNoArr.Length > 2)
                                TrackingNo = TrackingNoArr[2];
                            string[] ImagesArray = LabelMsgType[1].Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (ImagesArray.Length > 0)
                            {
                                for (int cnt = 0; cnt < ImagesArray.Length; cnt++)
                                {
                                    string ShoppingCartID = string.Empty;
                                    string PackageID = string.Empty;
                                    try
                                    {
                                        ShoppingCartID = ImagesArray[cnt].Substring(ImagesArray[cnt].LastIndexOf("_") + 1, ImagesArray[cnt].LastIndexOf("@") - ImagesArray[cnt].LastIndexOf("_") - 1);
                                        PackageID = ImagesArray[cnt].Substring(0, ImagesArray[cnt].IndexOf("_")).Replace("UPS-Package", "");
                                    }
                                    catch { }

                                    string[] strImageParts = ImagesArray[cnt].Split('_');
                                    DataRow dataRow = myTable.NewRow();
                                    dataRow["SerialNo"] = cnt + 1;
                                    dataRow["PackageID"] = PackageID;
                                    dataRow["ImgUrl"] = ImagesArray[cnt];
                                    dataRow["TrackingNo"] = strImageParts[1];
                                    dataRow["CreateDate"] = DateTime.Now;
                                    dataRow["ShippingCartID"] = ShoppingCartID;
                                    myTable.Rows.Add(dataRow);
                                }

                                grdUPS.DataSource = myTable;
                                grdUPS.Visible = true;
                                panUPS.Visible = true;
                                grdUPS.Columns[0].Visible = true;
                                grdUPS.Columns[1].Visible = true;
                                grdUPS.Columns[2].Visible = true;
                                grdUPS.Columns[3].Visible = true;
                                grdUPS.Columns[4].Visible = true;
                                grdUPS.DataBind();
                                lblshiperror.Text = "";
                            }
                        }

                    }
                    else
                    {
                        DispleyStr = Result;
                        grdUPS.Visible = false;
                        panUPS.Visible = false;
                    }
                }
                lblshiperror.Text = DispleyStr;
                lblshiperror.Visible = true;
            }
            catch { }
        }

        private void getFedExLabel(DataSet ds)
        {
            try
            {
                string ErrStr = "";
                string imagepath = "";
                try { imagepath = Server.MapPath(AppLogic.AppConfigs("FedEx.LabelSavePath")); }
                catch { }
                DataTable dt = getGridtable();
                if (dt.Rows.Count > 1)
                {
                    InterNationalShippinglabelFedExMPS oLabel = new InterNationalShippinglabelFedExMPS();
                    ErrStr = oLabel.ShipLableFedExInterna(Convert.ToInt32(ddlWareHouse.SelectedItem.Value), ds, txtWeight.Text, dt, imagepath, rdRadioForShipping.SelectedValue);
                }
                else
                {
                    InterNationalShippinglabelFedEx oLabel = new InterNationalShippinglabelFedEx();
                    ErrStr = oLabel.ShipLableFedExInterna(Convert.ToInt32(ddlWareHouse.SelectedItem.Value), ds, txtWeight.Text, dt, imagepath, rdRadioForShipping.SelectedValue);
                }
                if (!ErrStr.ToLower().Contains(".png"))
                    lblmsg.Text = ErrStr;
                else
                {
                    //BindFedExData(ErrStr);

                    #region Create Table

                    DataTable myTable = new DataTable();
                    myTable.Columns.Add("SerialNo", typeof(Int32));
                    myTable.Columns.Add("ImgUrl", typeof(String));
                    myTable.Columns.Add("TrackingNo", typeof(String));
                    myTable.Columns.Add("CreateDate", typeof(DateTime));
                    myTable.Columns.Add("ShippingCartID", typeof(String));
                    myTable.Columns.Add("PackageID", typeof(String));

                    #endregion

                    string[] ImagesArray = ErrStr.Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string srrorder = "";

                    if (ImagesArray.Length > 0)
                    {
                        for (int cnt = 0; cnt < ImagesArray.Length; cnt++)
                        {
                            string param = ImagesArray[cnt].Split('@')[0];

                            string param2 = ImagesArray[cnt].Split('@')[1];

                            string[] param3 = param2.Split('-');
                            string[] packageid = param3[1].Split('.');
                            string[] strImageParts = param.Split('_');
                            DataRow dataRow = myTable.NewRow();
                            dataRow["SerialNo"] = cnt + 1;
                            dataRow["ImgUrl"] = ImagesArray[cnt];
                            dataRow["TrackingNo"] = strImageParts[1];
                            dataRow["CreateDate"] = DateTime.Now;
                            dataRow["ShippingCartID"] = strImageParts[3];
                            dataRow["PackageID"] = packageid[0];//cnt + 1;
                            try
                            {
                                srrorder = strImageParts[2];
                            }
                            catch { }
                            myTable.Rows.Add(dataRow);
                        }
                    }
                    if (myTable.Rows.Count > 0)
                    {
                        grdFEDEX.DataSource = myTable;
                        grdFEDEX.DataBind();
                        grdFEDEX.Visible = true;
                        panFEDEX.Visible = true;
                    }
                }
            }
            catch (Exception ex) { lblmsg.Text = ex.Message; }
        }

        private DataTable getGridtable()
        {
            #region new code
            DataTable myTableTemp = new DataTable();
            string strcartId = "";
            decimal price = 0;
            int qty = 0;
            int num = 0;
            string Productid = "";//
            string AllProductid = "";//
            string OrderNumber = string.Empty;
            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
            }
            string strShoppingCartID = string.Empty;
            Object ObjCartID = CommonComponent.GetScalarCommonData("select ShoppingCardID from tb_Order where OrderNumber=" + OrderNumber.ToString());
            if (ObjCartID != null)
            {
                strShoppingCartID = ObjCartID.ToString();
            }

            try
            {
                #region Create Table

                myTableTemp.Columns.Add("Height", typeof(String));
                myTableTemp.Columns.Add("Width", typeof(String));
                myTableTemp.Columns.Add("Length", typeof(String));
                myTableTemp.Columns.Add("ProWeight", typeof(String));
                myTableTemp.Columns.Add("chkInsured", typeof(bool));
                myTableTemp.Columns.Add("ProductPrice", typeof(String));
                myTableTemp.Columns.Add("ProductName", typeof(String));
                myTableTemp.Columns.Add("Quantity", typeof(String));
                myTableTemp.Columns.Add("SCartID", typeof(String));
                myTableTemp.Columns.Add("packageid", typeof(String));
                myTableTemp.Columns.Add("ProductID", typeof(String));
                myTableTemp.Columns.Add("ShippingCartID", typeof(String));
                #endregion

                foreach (GridViewRow gvr in grdShipping.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");
                    //*
                    System.Web.UI.WebControls.Label lblProductID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductID");

                    System.Web.UI.WebControls.Label lblSCartID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblSCartID");
                    System.Web.UI.WebControls.TextBox txtProductPrice = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProductPrice");
                    System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");
                   //* System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                    System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                    strcartId = lblSCartID.Text.ToString();


                    int Qty = 0;
                  //  int availQuantity = 0;
                    try { Qty = Convert.ToInt32(lblQuantity.Text); }
                    catch { Qty = 0; }
                    //try { availQuantity = Convert.ToInt32(lblavailQuantity.Text); }
                    //catch { availQuantity = 0; }

                    //if (Qty > availQuantity) { availQuantity = 0; }


                    if (!chkAllowShipment.Checked)
                    {
                        price += Convert.ToDecimal(txtProductPrice.Text.ToString());
                        qty += Convert.ToInt32(lblQuantity.Text.ToString());
                    }

                    //if (availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                    if (lblShipping.Text.ToLower() == "no")
                    {
                        if (!chkAllowShipment.Checked)//
                            AllProductid = AllProductid + lblProductID.Text + ",";//

                        if (!chkAllowShipment.Checked)
                            continue;
                        //System.Web.UI.WebControls.TextBox txtHeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtHeight");
                        //System.Web.UI.WebControls.TextBox txtWidth = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtWidth");
                        //System.Web.UI.WebControls.TextBox txtLength = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtLength");
                        //System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                        //System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");

                        System.Web.UI.WebControls.TextBox txtHeightg = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtHeightgrid");
                        System.Web.UI.WebControls.TextBox txtWidthg = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtWidthgrid");
                        System.Web.UI.WebControls.TextBox txtLengthg = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtLengthgrid");
                        System.Web.UI.WebControls.TextBox txtProWeightg = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                        System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");



                        System.Web.UI.WebControls.Label lblProductName = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductName");

                        System.Web.UI.WebControls.CheckBox chkAllowShip = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");

                        Productid = lblProductID.Text;

                        if (chkAllowShip.Checked)
                        {
                            num++;
                            DataRow dataRow = myTableTemp.NewRow();
                            dataRow["Height"] = txtHeightg.Text;
                            dataRow["Width"] = txtWidthg.Text;
                            dataRow["Length"] = txtLengthg.Text;
                            dataRow["ProWeight"] = txtProWeightg.Text;
                            dataRow["chkInsured"] = chkInsured.Checked;
                            dataRow["ProductPrice"] = txtProductPrice.Text;
                            dataRow["ProductName"] = lblProductName.Text;
                            dataRow["Quantity"] = lblQuantity.Text;
                            dataRow["SCartID"] = lblSCartID.Text;
                            dataRow["packageid"] = Packid.ToString();// num.ToString();
                            dataRow["ProductID"] = Productid;
                            dataRow["ShippingCartID"] = strShoppingCartID;
                            myTableTemp.Rows.Add(dataRow);

                        }

                        if (chkAllowShipment.Checked)
                        {
                            Packid += 1;

                        }
                    }
                }
                if (Convert.ToDecimal(txtWeight.Text) > Convert.ToDecimal(0.00))
                {
                    num++;
                    DataRow dataRow = myTableTemp.NewRow();
                    dataRow["Height"] = txtHeight.Text;
                    dataRow["Width"] = txtWidth.Text;
                    dataRow["Length"] = txtLength.Text;
                    dataRow["ProWeight"] = txtWeight.Text;
                    dataRow["chkInsured"] = false;
                    dataRow["ProductPrice"] = price.ToString();
                    dataRow["ProductName"] = "";
                    dataRow["Quantity"] = qty.ToString();
                    dataRow["SCartID"] = strcartId;
                    dataRow["packageid"] = Packid.ToString(); // num.ToString();
                    dataRow["ShippingCartID"] = strShoppingCartID;
                    //  dataRow["ProductID"] = Productid;
                    if (AllProductid != "")
                    {
                        AllProductid = AllProductid.Substring(0, AllProductid.Length - 1);
                    }
                    dataRow["ProductID"] = AllProductid;

                    myTableTemp.Rows.Add(dataRow);
                }
            }
            catch { }
            return myTableTemp;
            #endregion

            //#region Updated code by saiyam
            //string Productid = "";
            //string AllProductid = "";
            //string strShoppingCartID = string.Empty;
            //string OrderNo = string.Empty;


            //if (Request.QueryString["ONo"] != null && Request.QueryString["ONo"].ToString().Trim().Length > 0)
            //    OrderNo = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
            //Object ObjCartID = CommonComponent.GetScalarCommonData("select ShoppingCardID from tb_Order where OrderNumber=" + OrderNo.ToString());
            //if (ObjCartID != null)
            //{
            //    strShoppingCartID = ObjCartID.ToString();
            //}


            //#region GetProducts
            //#region Create Table
            //DataTable myProductTable = new DataTable();
            //myProductTable.Columns.Add("PackageId", typeof(Int32));
            //myProductTable.Columns.Add("ProductName", typeof(String));
            //myProductTable.Columns.Add("Weight", typeof(String));
            //myProductTable.Columns.Add("Price", typeof(String));
            //myProductTable.Columns.Add("ShippingCartID", typeof(String));
            //myProductTable.Columns.Add("IsInsured", typeof(Boolean));
            //myProductTable.Columns.Add("ProductID", typeof(String));
            //#endregion

            //decimal decFullPackagePrice = 0;


            //#region checkgrid
            //foreach (GridViewRow gvr in grdShipping.Rows)
            //{
            //    System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");

            //    System.Web.UI.WebControls.Label lblProductID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductID");
            //    System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
            //    System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
            //    System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");




            //    int Qty = 0;
            //    int availQuantity = 0;
            //    try { Qty = Convert.ToInt32(lblQuantity.Text); }
            //    catch { Qty = 0; }
            //    try { availQuantity = Convert.ToInt32(lblavailQuantity.Text); }
            //    catch { availQuantity = 0; }

            //    if (Qty > availQuantity) { availQuantity = 0; }




            //    //if (lblavailQuantity.Text != "0" && lblShipping.Text.ToLower() == "no")
            //    if (availQuantity != 0 && lblShipping.Text.ToLower() == "no")
            //    {
            //        if (!chkAllowShipment.Checked)
            //            AllProductid = AllProductid + lblProductID.Text + ",";


            //        if (!chkAllowShipment.Checked)
            //        { continue; }

            //        Decimal ProWeight = Convert.ToDecimal("1");
            //        System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
            //        System.Web.UI.WebControls.Label lblProductName = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductName");
            //        System.Web.UI.WebControls.TextBox txtProductPrice = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProductPrice");
            //        System.Web.UI.WebControls.Label lblShippingCartID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShippingCartID");
            //        System.Web.UI.WebControls.Label lblPackageID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblPackageID");
            //        System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");


            //        lblShippingCartID.Text = strShoppingCartID;
            //        Productid = lblProductID.Text;

            //        if (txtProWeight != null && !string.IsNullOrEmpty(txtProWeight.Text))
            //            ProWeight = Convert.ToDecimal(txtProWeight.Text);
            //        if (ProWeight < 1)
            //            ProWeight = 1;
            //        if (!string.IsNullOrEmpty(lblProductName.Text) && !string.IsNullOrEmpty(txtProductPrice.Text))
            //        {
            //            DataRow dataRow = myProductTable.NewRow();
            //            dataRow["ProductName"] = Regex.Replace(lblProductName.Text.Trim(), @"<(.|\n)*?>", string.Empty);
            //            dataRow["Weight"] = ProWeight.ToString().Trim();
            //            dataRow["Price"] = txtProductPrice.Text.Trim();
            //            dataRow["ShippingCartID"] = strShoppingCartID;
            //            dataRow["PackageId"] = Packid.ToString();// lblPackageID.Text.Trim();
            //            dataRow["IsInsured"] = chkInsured.Checked ? "TRUE" : "FALSE";
            //            dataRow["ProductID"] = Productid;
            //            myProductTable.Rows.Add(dataRow);

            //        }
            //        else
            //        { decFullPackagePrice += Convert.ToDecimal(txtProductPrice.Text); }
            //        if (chkAllowShipment.Checked)
            //        {
            //            Packid += 1;

            //        }
            //    }
            //}
            //#endregion
            //decimal decTotalWeight = Convert.ToDecimal(hfWeight.Value);
            //txtWeight.Text = hfWeight.Value;

            ////    dbAccess = new SQLAccess();


            //if (decTotalWeight > 0)
            //{
            //    if (decTotalWeight < 1)
            //        decTotalWeight = 1;
            //    DataRow dataRow = myProductTable.NewRow();
            //    dataRow["ProductName"] = "Full Package";
            //    dataRow["Weight"] = decTotalWeight.ToString();
            //    dataRow["Price"] = decFullPackagePrice.ToString();
            //    //if (string.IsNullOrEmpty(strShoppingCartID))
            //    //    dataRow["ShippingCartID"] = "-1";
            //    //else
            //    dataRow["ShippingCartID"] = strShoppingCartID;
            //    dataRow["PackageId"] = Packid.ToString(); //grdShipping.Rows.Count + 1;
            //    dataRow["IsInsured"] = "FALSE";

            //    if (AllProductid != "")
            //    {
            //        AllProductid = AllProductid.Substring(0, AllProductid.Length - 1);
            //    }
            //    dataRow["ProductID"] = AllProductid;
            //    myProductTable.Rows.Add(dataRow);
            //}
            //return myProductTable;
            //#endregion 
            //#endregion
        }

        #region "Method"

        /// <summary>
        ///  btnHdn Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnHdn_Click(object sender, ImageClickEventArgs e)
        {
            //if(ddlWareHouse.Items.Count<1)
            //{

            // }
            // else
            // {
            warehouseinventoryMsg.Visible = false;
            if (grdShipping.Rows.Count <= 0)
            {
                warehouseinventoryMsg.Visible = true;
                warehouseinventoryMsg.Text = "Product(s) are not available.";
                return;
            }

            DataTable dtUPS = new DataTable();
            DataTable dtUSPS = new DataTable();
            DataTable dtFEDEX = new DataTable();
            string sql = "SELECT WareHouseID,Name FROM dbo.tb_WareHouse WHERE Active=1 AND ISNULL(Deleted,0)=0";
            DataSet dsWare = CommonComponent.GetCommonDataSet(sql);
            StringBuilder st = new StringBuilder();
            st.Append("<table width='100%'>");
            int n = 0;
            int row = 0;
            int count = dsWare.Tables[0].Rows.Count;
            for (int i = 0; i < (count / 2); i++)
            {
                st.Append("<tr>");
                if (count % 2 == 1)
                {
                    row = 2;
                }
                for (int j = 0; j < 2; j++)
                {
                    st.Append(@"<td  style='width: 50%; vertical-align: top;'>
                                                                     <table width='100%'>
                                                                     <tr style='background-color: #696969; height: 25px;'>
                                                                     <td colspan='3' valign='middle' align='left' style='color: #fff; font-family: Arial,Helvetica,sans-serif;
                                                                     font-weight: bold;'>&nbsp;" + dsWare.Tables[0].Rows[n]["Name"] + "</td></tr>");
                    st.Append("<tr>");
                    st.Append("<td align='top' valign='top'>");
                    //

                    //saiyam

                    //
                    string OrderNumber = "0";

                    if (Request.QueryString["ONo"] != null)
                    {
                        OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                    }
                    ShoppingCartComponent ObjOCart = new ShoppingCartComponent();
                    DataSet DsCItems = new DataSet();
                    DsCItems = ObjOCart.GetOrderedShoppingCartItemsByCartId((Convert.ToInt32(OrderNumber)));

                    invMsg.Text = "";
                    bool isavailQty = false;
                    string sku = "";
                    if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
                    {
                        for (int inv = 0; inv < DsCItems.Tables[0].Rows.Count; inv++)
                        {
                            string warehouseinventory = "SELECT Inventory from dbo.tb_WareHouseProductInventory where ProductID=" + DsCItems.Tables[0].Rows[inv]["ProductID"] + " and WareHouseID='" + dsWare.Tables[0].Rows[n]["WareHouseID"] + "'";
                            Int32 winventory = Convert.ToInt32(CommonComponent.GetScalarCommonData(warehouseinventory));
                            if (winventory == 0)
                            {
                                //  invMsg.Visible = true;

                                // invMsg.Text += DsCItems.Tables[0].Rows[inv]["SKU"].ToString() + ",";

                                sku = sku + DsCItems.Tables[0].Rows[inv]["SKU"].ToString() + ",";

                                isavailQty = true;


                            }

                        }
                        if (isavailQty == false)
                        {
                            dtUSPS = BindUSPS(Convert.ToInt32(dsWare.Tables[0].Rows[n]["WareHouseID"].ToString()));
                            if (dtUSPS != null && dtUSPS.Rows.Count > 0)
                            {
                                for (int f = 0; f < dtUSPS.Rows.Count; f++)
                                {
                                    st.Append(dtUSPS.Rows[f]["ShippingMethodName"]);
                                    st.Append("<br/>");
                                }
                            }


                            st.Append("</td>");
                            st.Append("<td align='top' valign='top'>");

                            dtUPS = BindUPS(Convert.ToInt32(dsWare.Tables[0].Rows[n]["WareHouseID"].ToString()));
                            if (dtUPS != null && dtUPS.Rows.Count > 0)
                            {
                                for (int f = 0; f < dtUPS.Rows.Count; f++)
                                {
                                    st.Append(dtUPS.Rows[f]["ShippingMethodName"]);
                                    st.Append("<br/>");
                                }
                            }

                            st.Append("</td>");

                            st.Append("<td align='top' valign='top'>");

                            dtFEDEX = BindFEDEX(Convert.ToInt32(dsWare.Tables[0].Rows[n]["WareHouseID"].ToString()));
                            if (dtFEDEX != null && dtFEDEX.Rows.Count > 0)
                            {
                                for (int f = 0; f < dtFEDEX.Rows.Count; f++)
                                {
                                    st.Append(dtFEDEX.Rows[f]["ShippingMethodName"]);
                                    st.Append("<br/>");
                                }
                            }

                            st.Append("</td>");

                            st.Append("</tr></table></td>");
                            //n++;
                        }
                        else
                        {
                            //invMsg.Text = invMsg.Text.Substring(0, (invMsg.Text.Length - 1));
                            //invMsg.Text = "<span style='color:red;'>No Inventory for SKU " + invMsg.Text + "</span>";
                            if (sku.Length > 1)
                                sku = sku.Substring(0, (sku.Length - 1));
                            //st.Append(invMsg.Text);
                            st.Append("<span style='color:red;'>No Inventory for SKU " + sku + "</span>");
                            st.Append("</td>");

                            st.Append("</tr></table></td>");

                        }

                    }
                    n++;


                }
                st.Append("</tr>");
            }
            if (row == 2)
            {

                st.Append(@"<td  style='width: 50%; vertical-align: top;'>
                                                                     <table width='100%'>
                                                                     <tr style='background-color: #696969; height: 25px;'>
                                                                     <td colspan='2' valign='middle' align='left' style='color: #fff; font-family: Arial,Helvetica,sans-serif;
                                                                     font-weight: bold;'>&nbsp;" + dsWare.Tables[0].Rows[n]["Name"] + "</td></tr>");

                st.Append("<tr>");
                st.Append("<td align='top' valign='top'>");

                //saiyam
                string OrderNumber = "0";

                if (Request.QueryString["ONo"] != null)
                {
                    OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                }
                ShoppingCartComponent ObjOCart = new ShoppingCartComponent();
                DataSet DsCItems = new DataSet();
                DsCItems = ObjOCart.GetOrderedShoppingCartItemsByCartId((Convert.ToInt32(OrderNumber)));

                invMsg.Text = "";
                string sku = "";
                bool isavailQty = false;
                if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
                {
                    for (int inv = 0; inv < DsCItems.Tables[0].Rows.Count; inv++)
                    {
                        string warehouseinventory = "SELECT Inventory from dbo.tb_WareHouseProductInventory where ProductID=" + DsCItems.Tables[0].Rows[inv]["ProductID"] + " and WareHouseID='" + dsWare.Tables[0].Rows[n]["WareHouseID"] + "'";
                        Int32 winventory = Convert.ToInt32(CommonComponent.GetScalarCommonData(warehouseinventory));
                        if (winventory == 0)
                        {
                            //invMsg.Visible = true;

                            //invMsg.Text += DsCItems.Tables[0].Rows[inv]["SKU"].ToString() + ",";

                            sku = sku + DsCItems.Tables[0].Rows[inv]["SKU"].ToString() + ",";
                            isavailQty = true;
                        }

                    }
                    if (isavailQty == false)
                    {
                        dtUSPS = BindUSPS(Convert.ToInt32(dsWare.Tables[0].Rows[n]["WareHouseID"].ToString()));
                        if (dtUSPS != null && dtUSPS.Rows.Count > 0)
                        {
                            for (int f = 0; f < dtUSPS.Rows.Count; f++)
                            {
                                st.Append(dtUSPS.Rows[f]["ShippingMethodName"]);
                                st.Append("<br/>");
                            }
                        }

                        st.Append("</td>");
                        st.Append("<td align='top' valign='top'>");

                        dtUPS = BindUPS(Convert.ToInt32(dsWare.Tables[0].Rows[n]["WareHouseID"].ToString()));
                        if (dtUPS != null && dtUPS.Rows.Count > 0)
                        {
                            for (int f = 0; f < dtUPS.Rows.Count; f++)
                            {
                                st.Append(dtUPS.Rows[f]["ShippingMethodName"]);
                                st.Append("<br/>");
                            }
                        }
                    }
                    else
                    {
                        if (sku.Length > 1)
                            sku = sku.Substring(0, (sku.Length - 1));
                        st.Append("<span style='color:red;'>No Inventory for SKU " + sku + "</span>");
                        //  invMsg.Text = invMsg.Text.Substring(0, (invMsg.Text.Length - 1));
                        //  invMsg.Text = "<span style='color:red;'>No Inventory for SKU " + invMsg.Text + "</span>";

                        // st.Append(invMsg.Text);
                        //st.Append("</td>");

                        // st.Append("</tr></table></td>");

                    }

                }




                st.Append("</td>");

                st.Append("</tr>");

                st.Append("</table></td>");
            }
            st.Append("</table>");
            warehouseinventoryMsg.Visible = false;
            Literal1.Text = st.ToString();
            this.ModalPopupExtender1.Show();
            //}
        }

        /// <summary>
        /// Gets the UPS Code
        /// </summary>
        /// <param name="method">string method.</param>
        /// <returns>Returns the UPS Code as a String.</returns>
        private string GetUPSCode(string method)
        {
            string RetShipMethod = "";
            if (method.Contains("Today Dedicate Courrier"))
                RetShipMethod = "83";
            else if (method.Contains("Wordwide Express"))
                RetShipMethod = "07";
            else if (method.Contains("Worldwide Expedited"))
                RetShipMethod = "08";
            else if (method.Contains("Saver Canada"))
                RetShipMethod = "13";
            else if (method.Contains("Express Early AM"))
                RetShipMethod = "54";
            else if (method.Contains("Today Standard"))
                RetShipMethod = "82";
            else if (method.Contains("Today Intercity"))
                RetShipMethod = "84";
            else if (method.Contains("Today Express"))
                RetShipMethod = "85";
            else if (method.Contains("Express Saver"))
                RetShipMethod = "86";
            else if (method.Contains("Next Day Air Saver"))
                RetShipMethod = "13";
            else if (method.Contains("Next Day Air Early"))
                RetShipMethod = "14";
            else if (method.Contains("Next Day Air"))
                RetShipMethod = "01";
            else if (method.Contains("Ground"))
                RetShipMethod = "03";
            else if (method.Contains("3 Day Selected") || method.Contains("3-Day Select"))
                RetShipMethod = "12";
            else if (method.Contains("Second Day Air AM") || method.Contains("2nd Day Air"))
                RetShipMethod = "59";
            else if (method.Contains("Standard"))
                RetShipMethod = "11";
            else if (method.Contains("Saver") || method.Contains("Next Day Air Saver"))
                RetShipMethod = "65";
            else
                RetShipMethod = "00";

            return RetShipMethod;
        }

        /// <summary>
        /// Gets the USPS Endician shipping label.
        /// </summary>
        /// <param name="ds">Dataset ds.</param>
        private void getUSPSEndicianShippingLabel(DataSet ds)
        {
            try
            {
                string Productid = "";
                int iLoop = 1;

                string orderid = "";
                if (Request.QueryString["ONo"] != null)
                    orderid = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());


                #region Buy Postage - Ashish

                EwsLabelService obj = new EwsLabelService();
                AccountStatusRequest objAccountStatus = new AccountStatusRequest();
                objAccountStatus.RequesterID = "Livedata";
                objAccountStatus.RequestID = "Livedata";

                CertifiedIntermediary objCertified = new CertifiedIntermediary();
                objCertified.AccountID = AppLogic.AppConfigs("USPS.UserName");
                objCertified.PassPhrase = AppLogic.AppConfigs("USPS.Password");
                objAccountStatus.CertifiedIntermediary = objCertified;
                AccountStatusResponse objAccountResponse = new AccountStatusResponse();
                objAccountResponse = obj.GetAccountStatus(objAccountStatus);

                if (objAccountResponse.CertifiedIntermediary.PostageBalance < 20)
                {
                    RecreditRequest objRecreditRequest = new RecreditRequest();
                    objRecreditRequest.CertifiedIntermediary = objCertified;
                    objRecreditRequest.RecreditAmount = "20";
                    objRecreditRequest.RequesterID = "Livedata";
                    objRecreditRequest.RequestID = "Livedata";

                    RecreditRequestResponse objRecreditRequestResponse = new RecreditRequestResponse();

                    objRecreditRequestResponse = obj.BuyPostage(objRecreditRequest);
                }

                #endregion


                bool isExpress = false;
                //   ServiceType Stype = new ServiceType();
                //  clsStampslabel objstamp = new clsStampslabel();
                string strShippingMethod = rdRadioForShipping.SelectedItem.Text.ToLower();

                String MailCalss = "";


                if (strShippingMethod.StartsWith("usps - express mail international"))
                {
                    MailCalss = "ExpressMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - first class mail international"))
                {
                    MailCalss = "FirstClassMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail international"))
                {
                    MailCalss = "PriorityMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - first-class mail"))
                { MailCalss = "First"; }
                else if (strShippingMethod.StartsWith("usps - priority mail"))
                {
                    MailCalss = "Priority";
                }
                else if (strShippingMethod.StartsWith("usps - express mail"))
                {
                    MailCalss = "Express";
                }
                else if (strShippingMethod.StartsWith("usps - library mail"))
                {
                    MailCalss = "LibraryMail";

                }
                else if (strShippingMethod.StartsWith("usps - media mail"))
                {
                    MailCalss = "MediaMail";
                }
                else if (strShippingMethod.StartsWith("usps - parcel post"))
                {
                    MailCalss = "ParcelPost";
                }
                else if (strShippingMethod.StartsWith("usps - critical mail"))
                {
                    MailCalss = "CriticalMail";
                }
                else if (strShippingMethod.StartsWith("usps - standard mail"))
                {
                    MailCalss = "StandardMail";
                }
                else if (strShippingMethod.StartsWith("usps - parcel select"))
                {
                    MailCalss = "ParcelSelect";
                }

                else if (strShippingMethod.StartsWith("usps - Priority mail flat rate envelope"))
                {
                    MailCalss = "Priority";
                }

                else if (strShippingMethod.StartsWith("usps - express mail flat rate envelope"))
                {
                    MailCalss = "Express";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail small flat rate box"))
                {
                    MailCalss = "Priority";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail medium flat rate box"))
                {
                    MailCalss = "Priority";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail large flat rate box"))
                {
                    MailCalss = "Priority";
                }
                else if (strShippingMethod.StartsWith("usps - express mail international flat rate envelope"))
                {
                    MailCalss = "ExpressMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail international flat rate envelope"))
                {
                    MailCalss = "PriorityMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail international small flat rate box"))
                {
                    MailCalss = "PriorityMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - express mail international small flat rate box"))
                {
                    MailCalss = "ExpressMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail international medium flat rate box"))
                {
                    MailCalss = "PriorityMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - express mail international medium flat rate box"))
                {
                    MailCalss = "ExpressMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail international large flat rate box"))
                {
                    MailCalss = "PriorityMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - express mail international large flat rate box"))
                {
                    MailCalss = "ExpressMailInternational";
                }
                else
                { }

                string ErrString = string.Empty;
                string OrgComapnyName = AppLogic.AppConfigs("STORENAME");
                string OrgContact = AppLogic.AppConfigs("Shipping.OriginContactName");
                string OrgPhoneZip = AppLogic.AppConfigs("Shipping.OriginPhone");

                //string OrgAddress1 = AppLogic.AppConfigs("Shipping.OriginAddress");
                //string OrgAddress2 = AppLogic.AppConfigs("Shipping.OriginAddress2");
                //string OrgCity = AppLogic.AppConfigs("Shipping.OriginCity");
                //string OrgState = AppLogic.AppConfigs("Shipping.OriginState");
                //string OrgShippingZip = AppLogic.AppConfigs("Shipping.OriginZip");
                //string OrgCountry = AppLogic.AppConfigs("Shipping.OriginCountry");


                string OrgAddress1 = "";
                string OrgAddress2 = "";
                string OrgCity = "";
                string OrgState = "";
                string OrgShippingZip = "";
                string OrgCountry = "";


                string sqlWareHouse = "SELECT Address1,Address2,Suite,City,ZipCode,tb_Country.Name AS countryName,State FROM dbo.tb_WareHouse  INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + ddlWareHouse.SelectedItem.Value;
                DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
                if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
                {

                    OrgAddress1 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address1"]);
                    if (dsWareHouse.Tables[0].Rows[0]["Address2"] != null && dsWareHouse.Tables[0].Rows[0]["Address2"] != DBNull.Value)
                        OrgAddress2 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address2"]);
                    OrgCity = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["City"]);
                    OrgState = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["State"]);
                    OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
                    OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
                    StateComponent objState = new StateComponent();
                    OrgState = Convert.ToString(objState.GetStateCodeByName(OrgState));
                }


                string ShippingCountry = ViewState["Country"].ToString();
                string ShippingCountryName = ViewState["CountryName"].ToString();
                string Contact = ds.Tables[0].Rows[0]["ShippingFirstName"] + " " + ds.Tables[0].Rows[0]["ShippingLastName"];
                string Address1 = "";
                string Suitedata = "";

                string OrderedShoppingCartID = "";
                if (ds.Tables[0].Rows[0]["ShoppingCardID"].ToString() != "")
                {
                    OrderedShoppingCartID = ds.Tables[0].Rows[0]["ShoppingCardID"].ToString();
                }


                if (ds.Tables[0].Rows[0]["ShippingSuite"].ToString() != "")
                {
                    Suitedata = " " + ds.Tables[0].Rows[0]["ShippingSuite"].ToString();
                }
                string Address2 = string.Empty;
                if (string.IsNullOrEmpty(Address1))
                    Address1 = ds.Tables[0].Rows[0]["ShippingAddress1"].ToString() + Suitedata;

                else
                    Address2 = ds.Tables[0].Rows[0]["ShippingAddress1"].ToString() + Suitedata;
                if (string.IsNullOrEmpty(Address2))
                {

                    Address2 = ds.Tables[0].Rows[0]["ShippingAddress2"].ToString();
                }
                string City = ds.Tables[0].Rows[0]["ShippingCity"].ToString();
                string Suite = ds.Tables[0].Rows[0]["ShippingSuite"].ToString();
                string PhoneNumber = ds.Tables[0].Rows[0]["ShippingPhone"].ToString();


                StateComponent objCountry = new StateComponent();
                string Abbreviation = objCountry.GetStateCodeByName(ds.Tables[0].Rows[0]["ShippingState"].ToString());
                string State = Abbreviation;
                string Zip = ds.Tables[0].Rows[0]["ShippingZip"].ToString();
                if (Zip.Contains("-"))
                {
                    Zip = Zip.Substring(0, Zip.IndexOf("-"));
                }
                if (string.IsNullOrEmpty(OrgContact))
                    ErrString = "Source Contact Name is not available.";

                if (string.IsNullOrEmpty(OrgAddress1))
                    ErrString = "Source Address is not available.";

                if (string.IsNullOrEmpty(OrgCity))
                    ErrString = "Source City name is not available.";

                if (string.IsNullOrEmpty(OrgState) || OrgState.Length != 2)
                    ErrString = "Source State name is not available.";

                if (string.IsNullOrEmpty(OrgShippingZip) || OrgShippingZip.Length < 4 || OrgShippingZip.Length > 6)
                    ErrString = "Source Zip Code is not available or Invalid";

                if (string.IsNullOrEmpty(Contact))
                    ErrString = "Contact Name is not available.";

                if (string.IsNullOrEmpty(City))
                    ErrString = "City name of the recipient is not available.";

                if (string.IsNullOrEmpty(State))
                    ErrString = "State name of the recipient is not available.";

                if (!string.IsNullOrEmpty(ErrString))
                {
                    ErrString = "Error: " + ErrString;
                    ltUSPSShippingLabel.Text = ErrString;
                    return; //close
                }

                #region Create Table
                DataTable myTable = new DataTable();
                myTable.Columns.Add("SerialNo", typeof(Int32));
                myTable.Columns.Add("PackageID", typeof(Int32));
                myTable.Columns.Add("ImgUrl", typeof(String));
                myTable.Columns.Add("TrackingNo", typeof(String));
                myTable.Columns.Add("CreateDate", typeof(DateTime));
                myTable.Columns.Add("ShippingCartID", typeof(String));
                #endregion

                string strError = string.Empty;
                string ShoppingCartID = string.Empty;
                Decimal decTotal = 0;

                System.Web.UI.WebControls.Label lblPackageID = new System.Web.UI.WebControls.Label();
                System.Web.UI.WebControls.Label lblShippingCartID = new System.Web.UI.WebControls.Label();
                System.Web.UI.WebControls.Label lblProductID = new System.Web.UI.WebControls.Label();
                System.Web.UI.WebControls.Label lblShippedQty = new System.Web.UI.WebControls.Label();
                int i = 0;
                int iItemCOunt = 0;
                String strCartid = "";
                String packageidd = "";

                foreach (GridViewRow gvr in grdShipping.Rows)
                {


                    String srImagename = "";

                    System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");
                    System.Web.UI.WebControls.TextBox txtProductPrice = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProductPrice");


                    Int32 price = 0;
                    Int32.TryParse(txtProductPrice.Text.Trim().Replace("$", ""), out price);



                    //lblShippingCartID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShippingCartID");
                    lblShippingCartID.Text = OrderedShoppingCartID;
                    System.Web.UI.WebControls.Label lblPackageId = (System.Web.UI.WebControls.Label)gvr.FindControl("lblPackageId");
                    lblShippedQty = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");

                    packageidd = Packid.ToString(); // lblPackageId.Text.ToString();
                    if (chkAllowShipment.Checked)
                    {
                        Packid += 1;

                    }
                    strCartid = lblShippingCartID.Text.ToString();
                    if (!chkAllowShipment.Checked)
                    {
                        Decimal tmptotal = 0;
                        Decimal.TryParse(txtProductPrice.Text, out tmptotal);
                        decTotal = tmptotal + decTotal;
                        iItemCOunt = iItemCOunt + 1;
                        //strCartid = "";
                        if (ShippingCountry != OrgCountry)
                        {
                            if (iItemCOunt > 6)
                            {
                                grdUSPS.Visible = false;
                                trUSPSAll.Visible = false;
                                strError += "<br/>" + "You can select max five item(s) per package.";
                                lblmsg.Text = strError;
                                return;
                            }
                        }
                        continue;
                    }
                    else
                    {
                        i = i + 1;
                    }

                    EwsLabelService objEwsLabelService = new EwsLabelService();
                    LabelRequest lblRequest = new LabelRequest();

                    Decimal ProWeight = Convert.ToDecimal("1");
                    System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");


                    System.Web.UI.WebControls.TextBox txtHeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtHeightgrid");
                    System.Web.UI.WebControls.TextBox txtWidth = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtWidthgrid");
                    System.Web.UI.WebControls.TextBox txtLength = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtLengthgrid");
                    lblPackageID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblPackageId");
                    //  lblShippingCartID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShippingCartID");

                    System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");
                    System.Web.UI.WebControls.Label lblSKU = (System.Web.UI.WebControls.Label)gvr.FindControl("lblSKU");
                    System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");
                    System.Web.UI.WebControls.Label lblVstore = (System.Web.UI.WebControls.Label)gvr.FindControl("lblVstore");


                    System.Web.UI.WebControls.Label lblProductName = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductName");
                    lblProductID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductID");





                    Productid = lblProductID.Text;

                    Int32 quantity = 0;
                    Int32.TryParse(lblQuantity.Text.Trim(), out quantity);
                    if (quantity == 0)
                        quantity = 1;



                    decimal decHeight = 0;
                    decimal decLength = 0;
                    decimal decWidth = 0;

                    if (txtHeight != null && !string.IsNullOrEmpty(txtHeight.Text))
                        decHeight = Convert.ToDecimal(txtHeight.Text);
                    if (txtWidth != null && !string.IsNullOrEmpty(txtWidth.Text))
                        decWidth = Convert.ToDecimal(txtWidth.Text);
                    if (txtLength != null && !string.IsNullOrEmpty(txtLength.Text))
                        decLength = Convert.ToDecimal(txtLength.Text);

                    {

                        Dimensions objdimension = new Dimensions();
                        objdimension.Height = Convert.ToDouble(decHeight);
                        objdimension.Width = Convert.ToDouble(decWidth);
                        objdimension.Length = Convert.ToDouble(decLength);
                        lblRequest.MailpieceDimensions = objdimension;





                        string TestAccount = AppLogic.AppConfigs("USPS.IsTestMode");
                        if (TestAccount == "0")
                        {
                            TestAccount = "No";

                        }
                        else
                            TestAccount = "Yes";
                        lblRequest.AccountID = AppLogic.AppConfigs("USPS.UserName");
                        lblRequest.PassPhrase = AppLogic.AppConfigs("USPS.Password");
                        lblRequest.Test = TestAccount;
                        lblRequest.MailClass = MailCalss;
                        lblRequest.LabelType = "Default";
                        lblRequest.LabelSize = "4X6";
                        lblRequest.ImageFormat = "GIF";
                        lblRequest.RequesterID = "12";
                        lblRequest.MailpieceShape = "Parcel";
                        lblRequest.Stealth = "True";
                        //   lblRequest.Description = lblProductName.Text.ToString();

                        if (lblProductName.Text.ToString().Length > 50)
                            lblRequest.Description = lblProductName.Text.ToString().Substring(0, 47) + "..";
                        else
                            lblRequest.Description = lblProductName.Text.ToString();


                        lblRequest.Value = Convert.ToSingle(txtProductPrice.Text);



                        if (chkInsured.Checked)
                            lblRequest.InsuredValue = txtProductPrice.Text == "" ? "0" : txtProductPrice.Text;



                        lblRequest.FromName = OrgContact;
                        lblRequest.ReturnAddress1 = OrgAddress1;
                        if (!string.IsNullOrEmpty(OrgAddress2))
                        {
                            lblRequest.ReturnAddress2 = OrgAddress2;
                        }
                        lblRequest.FromCity = OrgCity;
                        lblRequest.FromState = OrgState;
                        lblRequest.FromPostalCode = OrgShippingZip;
                        lblRequest.FromPhone = OrgPhoneZip.Replace("-", "").Replace("(", "").Replace(")", "");
                        lblRequest.FromCompany = OrgComapnyName;

                        lblRequest.ToName = Contact;
                        lblRequest.ToAddress1 = Address1.Replace("#", "");
                        if (!string.IsNullOrEmpty(Address2))
                        {

                            lblRequest.ToAddress2 = Address2.Replace("#", "");
                        }
                        lblRequest.ToCity = City;
                        lblRequest.ValidateAddress = "FALSE";
                        lblRequest.ToState = State;
                        lblRequest.ToPostalCode = Zip;
                        lblRequest.ToDeliveryPoint = "00";
                        lblRequest.ToPhone = PhoneNumber.Replace("-", "").Replace("(", "").Replace(")", "");
                        if (ShippingCountry != OrgCountry)
                        {
                            lblRequest.ToCountryCode = ShippingCountry;
                            lblRequest.ToCountry = ShippingCountryName;
                        }


                        ResponseOptions objPrice = new ResponseOptions();
                        objPrice.PostagePrice = "TRUE";
                        lblRequest.ResponseOptions = objPrice;



                        double Weight = 1;
                        if (txtProWeight.Text != "")
                            Weight = Convert.ToDouble(Convert.ToDecimal(txtProWeight.Text));



                        lblRequest.WeightOz = Math.Floor((Convert.ToDouble(Weight * 16)));
                        lblRequest.PartnerCustomerID = "0";
                        lblRequest.PartnerTransactionID = "0";

                    }



                    if (chkMail.Checked)
                    {

                        SpecialServices objServices = new SpecialServices();
                        objServices.InsuredMail = "OFF";
                        objServices.DeliveryConfirmation = "ON";
                        //     objServices.InsuredMail = "brijeshs@kaushalam.com";
                        //sobjServices.RegisteredMail = "brijeshs@kaushalam.com";
                        //  objServices.COD = "ON";
                        // objServices.
                        lblRequest.Services = objServices;

                    }

                    LabelRequestResponse objLabelRequestResponse = new LabelRequestResponse();

                    objLabelRequestResponse = objEwsLabelService.GetPostageLabel(lblRequest);


                    if (!string.IsNullOrEmpty(objLabelRequestResponse.ErrorMessage))
                    {
                        grdUSPS.Visible = false;
                        trUSPSAll.Visible = false;
                        strError += "<br/>" + objLabelRequestResponse.ErrorMessage;
                    }
                    else
                    {
                        string ImgSavePath = AppLogic.AppConfigs("USPS.LabelSavePath");


                        if (!Directory.Exists(Server.MapPath(ImgSavePath)))
                            Directory.CreateDirectory(Server.MapPath(ImgSavePath));


                        #region Create Image Name
                        string OrderNo = "0";
                        if (Request.QueryString["ONo"] != null && Request.QueryString["ONo"].ToString().Trim().Length > 0)
                            OrderNo = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                        //OrderNo = Request.QueryString["ONo"].ToString().Trim();     //"100140"; 

                        string filename = "USPS-Package" + packageidd.ToString() + "_" + objLabelRequestResponse.TrackingNumber.Replace(" ", "").Trim().ToString() + "_" + OrderNo + "_" + strCartid.ToString() + "@" + DateTime.Now.Year.ToString() + "-" +
                                  DateTime.Now.Month.ToString() + "-" +
                                  DateTime.Now.Day.ToString() + "-T-" +
                                  DateTime.Now.Hour.ToString() + "-" +
                                  DateTime.Now.Minute.ToString() + "-" +
                                  DateTime.Now.Second.ToString() + "-" +
                                  ".gif"; // Path of the Label PDF
                        string ImgName = Server.MapPath(ImgSavePath + filename);
                        #endregion

                        string lblString = objLabelRequestResponse.Base64LabelImage;
                        byte[] arrlbl = Convert.FromBase64String(lblString);

                        try
                        {
                            FileStream fs = new FileStream(ImgName, FileMode.Create, FileAccess.ReadWrite);
                            BinaryWriter bw = new BinaryWriter(fs);
                            bw.Write(arrlbl);
                            bw.Close();

                            DataRow dataRow = myTable.NewRow();

                            dataRow["SerialNo"] = iLoop;
                            dataRow["PackageID"] = packageidd.ToString();
                            dataRow["ImgUrl"] = filename.Replace("#", "");
                            dataRow["TrackingNo"] = objLabelRequestResponse.TrackingNumber;
                            dataRow["CreateDate"] = DateTime.Now;
                            dataRow["ShippingCartID"] = lblShippingCartID.Text;
                            myTable.Rows.Add(dataRow);
                            UpdatCartItem(objLabelRequestResponse.TrackingNumber, Productid, "USPS", Convert.ToInt32(OrderedShoppingCartID), Convert.ToInt32(lblShippedQty.Text), Convert.ToInt32(ddlWareHouse.SelectedItem.Value), Convert.ToInt32(packageidd));
                            iLoop++;
                        }
                        catch { }
                    }
                }
                //create Package 

                decimal decTotalWeight = Convert.ToDecimal(hfWeight.Value);
                txtWeight.Text = hfWeight.Value;
                if (decTotalWeight > 0)
                {
                    strCartid = "";

                    EwsLabelService objEwsLabelService = new EwsLabelService();
                    LabelRequest lblRequest = new LabelRequest();
                    int iCount = 1;
                    packageidd = Packid.ToString();//** lblPackageIDFP.Text.ToString();
                    double TotalWight = 0;
                    decimal TotalPrice = 0;
                    Productid = "";
                    foreach (GridViewRow gvr in grdShipping.Rows)
                    {

                        System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");

                        if (chkAllowShipment.Checked == false)
                        {

                            System.Web.UI.WebControls.Label lblProductName = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductName");
                            System.Web.UI.WebControls.TextBox txtProductPrice = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProductPrice");
                            System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                            System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");
                            System.Web.UI.WebControls.Label lblVstore = (System.Web.UI.WebControls.Label)gvr.FindControl("lblVstore");
                            lblProductID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductID");
                           //* System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                            System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                            int Qty = 0;
                           // int availQuantity = 0;
                            try { Qty = Convert.ToInt32(lblQuantity.Text); }
                            catch { Qty = 0; }
                           // try { availQuantity = Convert.ToInt32(lblavailQuantity.Text); }
                           // catch { availQuantity = 0; }

                            //if (Qty > availQuantity) { availQuantity = 0; }



                            //if (availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                            if (lblShipping.Text.ToLower() == "no")
                                Productid = Productid + lblProductID.Text + ",";

                            if (strCartid == "")
                            {
                                strCartid = Convert.ToString(CommonComponent.GetScalarCommonData("select ShoppingCardID from tb_Order where OrderNumber =" + orderid.ToString()));
                            }
                            double WeightTotal = 0;
                            //if (txtProWeight.Text != "" && availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                            if (txtProWeight.Text != "" && lblShipping.Text.ToLower() == "no")
                                WeightTotal = Convert.ToDouble(Convert.ToDecimal(txtProWeight.Text));

                            TotalWight += WeightTotal;

                            //if (txtProductPrice.Text != "" && availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                            if (txtProductPrice.Text != "" && lblShipping.Text.ToLower() == "no")
                            {
                                TotalPrice += Convert.ToDecimal(txtProductPrice.Text);

                            }
                            //if (iCount == 1 && availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                            if (iCount == 1 && lblShipping.Text.ToLower() == "no")
                            {
                                lblRequest.CustomsDescription1 = lblProductName.Text.ToString();
                                lblRequest.CustomsValue1 = Convert.ToSingle(txtProductPrice.Text);
                                double Weight = 1;
                                if (txtProWeight.Text != "")
                                    Weight = Convert.ToDouble(Convert.ToDecimal(txtProWeight.Text));
                                lblRequest.CustomsWeight1 = (Convert.ToUInt32(Weight * 16));
                                lblRequest.CustomsQuantity1 = Convert.ToUInt32(lblQuantity.Text);
                            }
                            //if (iCount == 2 && availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                            if (iCount == 2 && lblShipping.Text.ToLower() == "no")
                            {
                                lblRequest.CustomsDescription2 = lblProductName.Text.ToString();
                                lblRequest.CustomsValue2 = Convert.ToSingle(txtProductPrice.Text);
                                double Weight = 1;
                                if (txtProWeight.Text != "")
                                    Weight = Convert.ToDouble(Convert.ToDecimal(txtProWeight.Text));

                                lblRequest.CustomsWeight2 = (Convert.ToUInt32(Weight * 16));
                                lblRequest.CustomsQuantity2 = Convert.ToUInt32(lblQuantity.Text);
                            }

                            //if (iCount == 3 && availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                            if (iCount == 3 && lblShipping.Text.ToLower() == "no")
                            {
                                lblRequest.CustomsDescription3 = lblProductName.Text.ToString();
                                lblRequest.CustomsValue3 = Convert.ToSingle(txtProductPrice.Text);
                                double Weight = 1;
                                if (txtProWeight.Text != "")
                                    Weight = Convert.ToDouble(Convert.ToDecimal(txtProWeight.Text));
                                lblRequest.CustomsWeight3 = (Convert.ToUInt32(Weight * 16));
                                lblRequest.CustomsQuantity3 = Convert.ToUInt32(lblQuantity.Text);
                            }
                            //if (iCount == 4 && availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                            if (iCount == 4 && lblShipping.Text.ToLower() == "no")
                            {
                                lblRequest.CustomsDescription4 = lblProductName.Text.ToString();
                                lblRequest.CustomsValue4 = Convert.ToSingle(txtProductPrice.Text);
                                double Weight = 1;
                                if (txtProWeight.Text != "")
                                    Weight = Convert.ToDouble(Convert.ToDecimal(txtProWeight.Text));
                                lblRequest.CustomsWeight4 = (Convert.ToUInt32(Weight * 16));
                                lblRequest.CustomsQuantity4 = Convert.ToUInt32(lblQuantity.Text);
                            }

                            //if (iCount == 5 && availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                            if (iCount == 5 && lblShipping.Text.ToLower() == "no")
                            {
                                lblRequest.CustomsDescription5 = lblProductName.Text.ToString();
                                lblRequest.CustomsValue5 = Convert.ToSingle(txtProductPrice.Text);
                                double Weight = 1;
                                if (txtProWeight.Text != "")
                                    Weight = Convert.ToDouble(Convert.ToDecimal(txtProWeight.Text));
                                lblRequest.CustomsWeight5 = (Convert.ToUInt32(Weight * 16));
                                lblRequest.CustomsQuantity5 = Convert.ToUInt32(lblQuantity.Text);
                            }
                            iCount++;
                        }
                    }
                    string TestAccount = AppLogic.AppConfigs("USPS.IsTestMode");
                    if (TestAccount == "0")
                    {
                        TestAccount = "No";
                    }
                    else
                        TestAccount = "Yes";
                    lblRequest.AccountID = AppLogic.AppConfigs("USPS.UserName");
                    lblRequest.PassPhrase = AppLogic.AppConfigs("USPS.Password");
                    lblRequest.Test = TestAccount;
                    lblRequest.MailClass = MailCalss;
                    lblRequest.LabelType = "Default";
                    lblRequest.LabelSize = "4X6";
                    lblRequest.ImageFormat = "GIF";
                    lblRequest.RequesterID = "12";
                    lblRequest.MailpieceShape = "Parcel";
                    lblRequest.Stealth = "True";

                    Dimensions objdimension = new Dimensions();
                    objdimension.Height = Convert.ToDouble(txtHeight.Text);
                    objdimension.Width = Convert.ToDouble(txtWeight.Text);
                    objdimension.Length = Convert.ToDouble(txtLength.Text);
                    lblRequest.MailpieceDimensions = objdimension;
                    lblRequest.InsuredValue = TotalPrice.ToString();
                    lblRequest.WeightOz = Math.Floor((Convert.ToDouble(TotalWight) * 16));
                    lblRequest.FromName = OrgContact;
                    lblRequest.ReturnAddress1 = OrgAddress1;
                    if (!string.IsNullOrEmpty(OrgAddress2))
                    {
                        lblRequest.ReturnAddress2 = OrgAddress2;
                    }
                    lblRequest.FromCity = OrgCity;
                    lblRequest.FromState = OrgState;
                    lblRequest.FromPostalCode = OrgShippingZip;
                    lblRequest.FromPhone = OrgPhoneZip.Replace("-", "").Replace("(", "").Replace(")", "");
                    lblRequest.FromCompany = OrgComapnyName;
                    lblRequest.ToName = Contact;
                    lblRequest.ToAddress1 = Address1.Replace("#", "");
                    if (!string.IsNullOrEmpty(Address2))
                    {
                        lblRequest.ToAddress2 = Address2.Replace("#", "");
                    }
                    lblRequest.ToCity = City;
                    lblRequest.ValidateAddress = "FALSE";
                    lblRequest.ToState = State;
                    lblRequest.ToPostalCode = Zip;
                    lblRequest.ToDeliveryPoint = "00";
                    lblRequest.ToPhone = PhoneNumber.Replace("-", "").Replace("(", "").Replace(")", "");
                    if (ShippingCountry != OrgCountry)
                    {
                        lblRequest.ToCountryCode = ShippingCountry;
                        lblRequest.ToCountry = ShippingCountryName;
                    }
                    ResponseOptions objPrice = new ResponseOptions();
                    objPrice.PostagePrice = "TRUE";
                    lblRequest.ResponseOptions = objPrice;
                    lblRequest.PartnerCustomerID = "0";
                    lblRequest.PartnerTransactionID = "0";
                    if (chkMail.Checked)
                    {
                        SpecialServices objServices = new SpecialServices();
                        objServices.InsuredMail = "OFF";
                        objServices.DeliveryConfirmation = "ON";
                        //     objServices.InsuredMail = "brijeshs@kaushalam.com";
                        //sobjServices.RegisteredMail = "brijeshs@kaushalam.com";
                        //  objServices.COD = "ON";
                        // objServices.
                        lblRequest.Services = objServices;
                    }
                    LabelRequestResponse objLabelRequestResponse = new LabelRequestResponse();
                    objLabelRequestResponse = objEwsLabelService.GetPostageLabel(lblRequest);
                    if (!string.IsNullOrEmpty(objLabelRequestResponse.ErrorMessage))
                    {
                        grdUSPS.Visible = false;
                        trUSPSAll.Visible = false;
                        strError += "<br/>" + objLabelRequestResponse.ErrorMessage;
                    }
                    else
                    {
                        string ImgSavePath = AppLogic.AppConfigs("USPS.LabelSavePath");

                        if (!Directory.Exists(Server.MapPath(ImgSavePath)))
                            Directory.CreateDirectory(Server.MapPath(ImgSavePath));
                        #region Create Image Name
                        string OrderNo = "0";
                        if (Request.QueryString["ONo"] != null && Request.QueryString["ONo"].ToString().Trim().Length > 0)
                            OrderNo = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                        //   OrderNo = Request.QueryString["ONo"].ToString().Trim();     //"100140"; 
                        string filename = "USPS-Package" + packageidd.ToString() + "_" + objLabelRequestResponse.TrackingNumber.Replace(" ", "").Trim().ToString() + "_" + OrderNo + "_" + strCartid.ToString() + "@" + DateTime.Now.Year.ToString() + "-" +
                                  DateTime.Now.Month.ToString() + "-" +
                                  DateTime.Now.Day.ToString() + "-T-" +
                                  DateTime.Now.Hour.ToString() + "-" +
                                  DateTime.Now.Minute.ToString() + "-" +
                                  DateTime.Now.Second.ToString() + "-" +
                                  ".gif"; // Path of the Label PDF
                        string ImgName = Server.MapPath(ImgSavePath + filename);
                        #endregion

                        string lblString = objLabelRequestResponse.Base64LabelImage;
                        byte[] arrlbl = Convert.FromBase64String(lblString);

                        try
                        {
                            FileStream fs = new FileStream(ImgName, FileMode.Create, FileAccess.ReadWrite);
                            BinaryWriter bw = new BinaryWriter(fs);
                            bw.Write(arrlbl);
                            bw.Close(); //Thanks Karlo for pointing out!
                            DataRow dataRow = myTable.NewRow();
                            dataRow["SerialNo"] = iLoop;
                            dataRow["PackageID"] = packageidd;
                            dataRow["ImgUrl"] = filename.Replace("#", "");
                            dataRow["TrackingNo"] = objLabelRequestResponse.TrackingNumber;
                            dataRow["CreateDate"] = DateTime.Now;
                            dataRow["ShippingCartID"] = strCartid.ToString();
                            myTable.Rows.Add(dataRow);

                            if (Productid != "")
                            {
                                Productid = Productid.Substring(0, Productid.Length - 1);
                            }

                            UpdatCartItem(objLabelRequestResponse.TrackingNumber, Productid, "USPS", Convert.ToInt32(OrderedShoppingCartID), Convert.ToInt32(lblShippedQty.Text), Convert.ToInt32(ddlWareHouse.SelectedItem.Value), Convert.ToInt32(packageidd));

                            iLoop++;
                        }
                        catch { }
                    }
                }
                if (myTable.Rows.Count > 0)
                {
                    grdUSPS.DataSource = myTable;
                    grdUSPS.DataBind();
                    grdUSPS.Visible = true;
                    trUSPSAll.Visible = true;

                }
                if (!string.IsNullOrEmpty(strError))
                    ltUSPSShippingLabel.Text = strError;
            }
            catch (Exception ex)
            {
                grdUSPS.Visible = false;
                trUSPSAll.Visible = false;
                lblmsg.Text = ex.Message;
            }
        }

        /// <summary>
        /// Returns the Form Number
        /// </summary>
        /// <param name="Mailclass">string Mailclass</param>
        /// <param name="MailPiece">string MailPiece</param>
        /// <param name="ProductCount">int ProductCount</param>
        /// <returns>Returns the form number as a string </returns>
        private string ReturnFormNumber(string Mailclass, string MailPiece, int ProductCount)
        {
            string Rteunform = "";


            if (Mailclass == "FirstClassMailInternational" && ProductCount < 6)
            {
                Rteunform = "Form2976";
            }
            else if (Mailclass == "FirstClassMailInternational" && ProductCount > 5)
            {
                Rteunform = "Form2976A";
            }
            else if (Mailclass == "ExpressMailInternational" && MailPiece == "FlatRateEnvelope")
            {
                Rteunform = "Form2976A";
            }
            else if (Mailclass == "ExpressMailInternational" && MailPiece == "FlatRateLegalEnvelope")
            {
                Rteunform = "Form2976A";
            }
            else if (Mailclass == "ExpressMailInternational" && MailPiece == "Parcel")
            {
                Rteunform = "Form2976A";
            }

            else if (Mailclass == "PriorityMailInternational" && MailPiece == "SmallFlatRateBox")
            {
                Rteunform = "Form2976";
            }


            else if (Mailclass == "PriorityMailInternational" && MailPiece == "FlatRatePaddedEnvelope")
            {
                Rteunform = "Form2976";
            }

            else if (Mailclass == "PriorityMailInternational" && MailPiece == "FlatRateLegalEnvelope")
            {
                Rteunform = "Form2976";
            }

            else if (Mailclass == "PriorityMailInternational" && MailPiece == "FlatRateEnvelope")
            {
                Rteunform = "Form2976";
            }


            else if (Mailclass == "PriorityMailInternational" && MailPiece == "Parcel")
            {
                Rteunform = "Form2976A";
            }
            else if (Mailclass == "PriorityMailInternational" && MailPiece == "MediumFlatRateBox")
            {
                Rteunform = "Form2976A";
            }
            else if (Mailclass == "PriorityMailInternational" && MailPiece == "LargeFlatRateBox")
            {
                Rteunform = "Form2976A";
            }



            else if (Mailclass == "ExpressMailInternational" && ProductCount < 6)
            {
                Rteunform = "Form2976";
            }
            else if (Mailclass == "ExpressMailInternational" && ProductCount > 5)
            {
                Rteunform = "Form2976A";
            }

            else if (Mailclass == "PriorityMailInternational" && ProductCount < 6)
            {
                Rteunform = "Form2976";
            }
            else if (Mailclass == "PriorityMailInternational" && ProductCount > 5)
            {
                Rteunform = "Form2976A";
            }
            else
            {
                if (ProductCount < 6)
                {
                    Rteunform = "Form2976";
                }
                else
                {
                } Rteunform = "Form2976A";

            }
            return Rteunform;
        }

        /// <summary>
        /// Check Valid for the Custom Data
        /// </summary>
        /// <param name="Mailclass">string Mailclass</param>
        /// <param name="MailPiece">string MailPiece</param>
        /// <param name="ProductCount">int ProductCount</param>
        /// <returns>Returns string value</returns>
        private string CheckvalidforCustomData(string Mailclass, string MailPiece, int ProductCount)
        {
            string ReturnClass = "";
            if (Mailclass == "FirstClassMailInternational" && ProductCount > 5)
            {
                ReturnClass = Mailclass;
            }

            else if (Mailclass == "PriorityMailInternational" && MailPiece == "FlatRateEnvelope" && ProductCount > 5)
            {
                ReturnClass = Mailclass + "-" + MailPiece;
            }
            else if (Mailclass == "PriorityMailInternational" && MailPiece == "FlatRateLegalEnvelope" && ProductCount > 5)
            {
                ReturnClass = Mailclass + "-" + MailPiece;
            }
            else if (Mailclass == "PriorityMailInternational" && MailPiece == "FlatRatePaddedEnvelope" && ProductCount > 5)
            {
                ReturnClass = Mailclass + "-" + MailPiece;
            }
            else if (Mailclass == "PriorityMailInternational" && MailPiece == "SmallFlatRateBox" && ProductCount > 5)
            {
                ReturnClass = Mailclass + "-" + MailPiece;
            }
            return ReturnClass;


        }

        /// <summary>
        /// Gets the USPS Endician International Shipping Label
        /// </summary>
        /// <param name="ds">Dataset ds</param>
        private void getUSPSEndicianInternationalShippingLabel(DataSet ds)
        {
            try
            {
                int iLoop = 1;

                string Productid = "";
                string orderid = "";
                if (Request.QueryString["ONo"] != null)
                    orderid = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());


                #region Buy Postage - Ashish

                EwsLabelService obj = new EwsLabelService();
                AccountStatusRequest objAccountStatus = new AccountStatusRequest();
                objAccountStatus.RequesterID = "Livedata";
                objAccountStatus.RequestID = "Livedata";

                CertifiedIntermediary objCertified = new CertifiedIntermediary();
                objCertified.AccountID = AppLogic.AppConfigs("USPS.UserName");
                objCertified.PassPhrase = AppLogic.AppConfigs("USPS.Password");
                objAccountStatus.CertifiedIntermediary = objCertified;
                AccountStatusResponse objAccountResponse = new AccountStatusResponse();
                objAccountResponse = obj.GetAccountStatus(objAccountStatus);

                if (objAccountResponse.CertifiedIntermediary.PostageBalance < 20)
                {
                    RecreditRequest objRecreditRequest = new RecreditRequest();
                    objRecreditRequest.CertifiedIntermediary = objCertified;
                    objRecreditRequest.RecreditAmount = "200";
                    objRecreditRequest.RequesterID = "Livedata";
                    objRecreditRequest.RequestID = "Livedata";

                    RecreditRequestResponse objRecreditRequestResponse = new RecreditRequestResponse();

                    objRecreditRequestResponse = obj.BuyPostage(objRecreditRequest);
                }

                #endregion

                bool isExpress = false;
                string strShippingMethod = rdRadioForShipping.SelectedItem.Text.ToLower();

                String MailCalss = "";

                if (strShippingMethod.StartsWith("usps - express mail international"))
                {
                    MailCalss = "ExpressMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - first class mail international"))
                {
                    MailCalss = "FirstClassMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail international"))
                {
                    MailCalss = "PriorityMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - first-class mail"))
                { MailCalss = "First"; }
                else if (strShippingMethod.StartsWith("usps - priority mail"))
                {
                    MailCalss = "Priority";
                }
                else if (strShippingMethod.StartsWith("usps - express mail"))
                {
                    MailCalss = "Express";
                }
                else if (strShippingMethod.StartsWith("usps - library mail"))
                {
                    MailCalss = "LibraryMail";

                }
                else if (strShippingMethod.StartsWith("usps - media mail"))
                {
                    MailCalss = "MediaMail";
                }
                else if (strShippingMethod.StartsWith("usps - parcel post"))
                {
                    MailCalss = "ParcelPost";
                }
                else if (strShippingMethod.StartsWith("usps - critical mail"))
                {
                    MailCalss = "CriticalMail";
                }
                else if (strShippingMethod.StartsWith("usps - standard mail"))
                {
                    MailCalss = "StandardMail";
                }
                else if (strShippingMethod.StartsWith("usps - parcel select"))
                {
                    MailCalss = "ParcelSelect";
                }

                else if (strShippingMethod.StartsWith("usps - Priority mail flat rate envelope"))
                {
                    MailCalss = "Priority";
                }

                else if (strShippingMethod.StartsWith("usps - express mail flat rate envelope"))
                {
                    MailCalss = "Express";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail small flat rate box"))
                {
                    MailCalss = "Priority";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail medium flat rate box"))
                {
                    MailCalss = "Priority";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail large flat rate box"))
                {
                    MailCalss = "Priority";
                }
                else if (strShippingMethod.StartsWith("usps - express mail international flat rate envelope"))
                {
                    MailCalss = "ExpressMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail international flat rate envelope"))
                {
                    MailCalss = "PriorityMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail international small flat rate box"))
                {
                    MailCalss = "PriorityMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - express mail international small flat rate box"))
                {
                    MailCalss = "ExpressMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail international medium flat rate box"))
                {
                    MailCalss = "PriorityMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - express mail international medium flat rate box"))
                {
                    MailCalss = "ExpressMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - priority mail international large flat rate box"))
                {
                    MailCalss = "PriorityMailInternational";
                }
                else if (strShippingMethod.StartsWith("usps - express mail international large flat rate box"))
                {
                    MailCalss = "ExpressMailInternational";
                }
                else
                {
                }


                string ErrString = string.Empty;
                string OrgComapnyName = AppLogic.AppConfigs("STORENAME");
                string OrgContact = AppLogic.AppConfigs("Shipping.OriginContactName");
                string OrgPhoneZip = AppLogic.AppConfigs("Shipping.OriginPhone");

                // string OrgAddress1 = AppLogic.AppConfigs("Shipping.OriginAddress");
                // string OrgAddress2 = AppLogic.AppConfigs("Shipping.OriginAddress2");
                // string OrgCity = AppLogic.AppConfigs("Shipping.OriginCity");
                // string OrgState = AppLogic.AppConfigs("Shipping.OriginState");
                // string OrgShippingZip = AppLogic.AppConfigs("Shipping.OriginZip");
                // string OrgCountry = AppLogic.AppConfigs("Shipping.OriginCountry");

                string OrgAddress1 = "";
                string OrgAddress2 = "";
                string OrgCity = "";
                string OrgState = "";
                string OrgShippingZip = "";
                string OrgCountry = "";


                string sqlWareHouse = "SELECT Address1,Address2,Suite,City,ZipCode,tb_Country.Name AS countryName,State FROM dbo.tb_WareHouse  INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + ddlWareHouse.SelectedItem.Value;
                DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
                if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
                {

                    OrgAddress1 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address1"]);
                    if (dsWareHouse.Tables[0].Rows[0]["Address2"] != null && dsWareHouse.Tables[0].Rows[0]["Address2"] != DBNull.Value)
                        OrgAddress2 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address2"]);
                    OrgCity = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["City"]);
                    OrgState = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["State"]);
                    OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
                    OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
                    StateComponent objState = new StateComponent();
                    OrgState = Convert.ToString(objState.GetStateCodeByName(OrgState));
                }



                string ShippingCountry = ViewState["Country"].ToString();
                string ShippingCountryName = ViewState["CountryName"].ToString();
                string Contact = ds.Tables[0].Rows[0]["ShippingFirstName"] + " " + ds.Tables[0].Rows[0]["ShippingLastName"];
                string Address1 = "";
                string Suitedata = "";



                string OrderedShoppingCartID = "";
                if (ds.Tables[0].Rows[0]["ShoppingCardID"].ToString() != "")
                {
                    OrderedShoppingCartID = ds.Tables[0].Rows[0]["ShoppingCardID"].ToString();
                }

                string ShippingCompany = "";
                if (ds.Tables[0].Rows[0]["ShippingCompany"].ToString() != "")
                {
                    ShippingCompany = ds.Tables[0].Rows[0]["ShippingCompany"].ToString();
                }


                if (ds.Tables[0].Rows[0]["ShippingSuite"].ToString() != "")
                {
                    //Suitedata = ",SUITE- " + ds.Tables[0].Rows[0]["ShippingSuite"].ToString();
                    Suitedata = " " + ds.Tables[0].Rows[0]["ShippingSuite"].ToString();
                }

                string Address2 = string.Empty;
                if (string.IsNullOrEmpty(Address1))
                    Address1 = ds.Tables[0].Rows[0]["ShippingAddress1"].ToString() + Suitedata;

                else
                    Address2 = ds.Tables[0].Rows[0]["ShippingAddress1"].ToString() + Suitedata;
                if (string.IsNullOrEmpty(Address2))
                {

                    Address2 = ds.Tables[0].Rows[0]["ShippingAddress2"].ToString();

                }

                string City = ds.Tables[0].Rows[0]["ShippingCity"].ToString();
                string Suite = ds.Tables[0].Rows[0]["ShippingSuite"].ToString();
                string PhoneNumber = ds.Tables[0].Rows[0]["ShippingPhone"].ToString();

                StateComponent objCountry = new StateComponent();
                string Abbreviation = objCountry.GetStateCodeByName(ds.Tables[0].Rows[0]["ShippingState"].ToString());
                string State = "";
                if (Abbreviation != "")
                {
                    State = Abbreviation;
                }
                else
                {
                    State = ds.Tables[0].Rows[0]["ShippingState"].ToString();
                }



                string Zip = ds.Tables[0].Rows[0]["ShippingZip"].ToString();


                if (string.IsNullOrEmpty(OrgContact))
                    ErrString = "Source Contact Name is not available.";

                if (string.IsNullOrEmpty(OrgAddress1))
                    ErrString = "Source Address is not available.";

                if (string.IsNullOrEmpty(OrgCity))
                    ErrString = "Source City name is not available.";

                if (string.IsNullOrEmpty(OrgState) || OrgState.Length != 2)
                    ErrString = "Source State name is not available.";

                if (string.IsNullOrEmpty(OrgShippingZip) || OrgShippingZip.Length < 4 || OrgShippingZip.Length > 6)
                    ErrString = "Source Zip Code is not available or Invalid";

                if (string.IsNullOrEmpty(Contact))
                    ErrString = "Contact Name is not available.";

                if (string.IsNullOrEmpty(City))
                    ErrString = "City name of the recipient is not available.";

                if (ShippingCountry == OrgCountry)
                {
                    if (string.IsNullOrEmpty(State))
                        ErrString = "State name of the recipient is not available.";
                }

                if (!string.IsNullOrEmpty(ErrString))
                {
                    ErrString = "Error: " + ErrString;
                    ltUSPSShippingLabel.Text = ErrString;
                    return; //close
                }

                #region Create Table
                DataTable myTable = new DataTable();
                myTable.Columns.Add("SerialNo", typeof(Int32));
                myTable.Columns.Add("PackageID", typeof(Int32));
                myTable.Columns.Add("ImgUrl", typeof(String));
                myTable.Columns.Add("TrackingNo", typeof(String));
                myTable.Columns.Add("CreateDate", typeof(DateTime));
                myTable.Columns.Add("ShippingCartID", typeof(String));
                #endregion


                string strError = string.Empty;
                string ShoppingCartID = string.Empty;
                Decimal decTotal = 0;

                System.Web.UI.WebControls.Label lblPackageID = new System.Web.UI.WebControls.Label();
                System.Web.UI.WebControls.Label lblShippingCartID = new System.Web.UI.WebControls.Label();
                System.Web.UI.WebControls.Label lblProductID = new System.Web.UI.WebControls.Label();
                System.Web.UI.WebControls.Label lblShippedQty = new System.Web.UI.WebControls.Label();

                int i = 0;
                int iItemCOunt = 0;
                String strCartid = "";
                String packageidd = "";

                foreach (GridViewRow gvr in grdShipping.Rows)
                {
                    String srImagename = "";
                    System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");
                    System.Web.UI.WebControls.TextBox txtProductPrice = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProductPrice");
                    Int32 price = 0;
                    Int32.TryParse(txtProductPrice.Text.Trim().Replace("$", ""), out price);
                    //lblShippingCartID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShippingCartID");
                    lblShippingCartID.Text = OrderedShoppingCartID;

                    System.Web.UI.WebControls.Label lblPackageId = (System.Web.UI.WebControls.Label)gvr.FindControl("lblPackageId");
                    lblShippedQty = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");

                    packageidd = Packid.ToString();//** lblPackageId.Text.ToString();
                    if (chkAllowShipment.Checked)
                    {
                        Packid += 1;

                    }

                    strCartid = lblShippingCartID.Text.ToString();
                    if (!chkAllowShipment.Checked)
                    {
                        Decimal tmptotal = 0;
                        Decimal.TryParse(txtProductPrice.Text, out tmptotal);
                        decTotal = tmptotal + decTotal;
                        iItemCOunt = iItemCOunt + 1;

                        continue;

                    }
                    else
                    {
                        i = i + 1;
                    }

                    EwsLabelService objEwsLabelService = new EwsLabelService();
                    LabelRequest lblRequest = new LabelRequest();
                    Decimal ProWeight = Convert.ToDecimal("1");
                    System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                    System.Web.UI.WebControls.TextBox txtHeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtHeightgrid");
                    System.Web.UI.WebControls.TextBox txtWidth = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtWidthgrid");
                    System.Web.UI.WebControls.TextBox txtLength = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtLengthgrid");
                    lblPackageID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblPackageId");
                    //lblShippingCartID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShippingCartID");
                    System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");
                    System.Web.UI.WebControls.Label lblSKU = (System.Web.UI.WebControls.Label)gvr.FindControl("lblSKU");
                    System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");
                    System.Web.UI.WebControls.Label lblVstore = (System.Web.UI.WebControls.Label)gvr.FindControl("lblVstore");
                    System.Web.UI.WebControls.Label lblProductName = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductName");
                    lblProductID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductID");

                    Productid = lblProductID.Text;
                    Int32 quantity = 0;
                    Int32.TryParse(lblQuantity.Text.Trim(), out quantity);
                    if (quantity == 0)
                        quantity = 1;

                    //    MAX.USPS.Package p = new MAX.USPS.Package();

                    decimal decHeight = 0;
                    decimal decLength = 0;
                    decimal decWidth = 0;

                    if (txtHeight != null && !string.IsNullOrEmpty(txtHeight.Text))
                        decHeight = Convert.ToDecimal(txtHeight.Text);
                    if (txtWidth != null && !string.IsNullOrEmpty(txtWidth.Text))
                        decWidth = Convert.ToDecimal(txtWidth.Text);
                    if (txtLength != null && !string.IsNullOrEmpty(txtLength.Text))
                        decLength = Convert.ToDecimal(txtLength.Text);
                    {
                        Dimensions objdimension = new Dimensions();
                        objdimension.Height = Convert.ToDouble(decHeight);
                        objdimension.Width = Convert.ToDouble(decWidth);
                        objdimension.Length = Convert.ToDouble(decLength);
                        lblRequest.MailpieceDimensions = objdimension;
                        string TestAccount = AppLogic.AppConfigs("USPS.IsTestMode");
                        if (TestAccount == "0")
                        {
                            TestAccount = "No";

                        }
                        else
                            TestAccount = "Yes";
                        lblRequest.AccountID = AppLogic.AppConfigs("USPS.UserName");
                        lblRequest.PassPhrase = AppLogic.AppConfigs("USPS.Password");
                        lblRequest.Test = TestAccount;
                        lblRequest.MailClass = MailCalss;
                        lblRequest.LabelType = "Default";
                        lblRequest.LabelSize = "4X6";
                        lblRequest.ImageFormat = "GIF";
                        lblRequest.RequesterID = "12";
                        lblRequest.MailpieceShape = "Parcel";
                        lblRequest.Stealth = "True";

                        if (lblVstore.Text != null && lblVstore.Text != "")
                        {
                            if (lblProductName.Text.ToString().Replace(lblVstore.Text.ToString(), "").Length > 50)
                                lblRequest.Description = lblProductName.Text.ToString().Replace(lblVstore.Text.ToString(), "").Substring(0, 47) + "..";
                            else
                                lblRequest.Description = lblProductName.Text.ToString().Replace(lblVstore.Text.ToString(), "");


                        }
                        else
                        {
                            if (lblProductName.Text.ToString().Length > 50)
                                lblRequest.Description = lblProductName.Text.ToString().Substring(0, 47) + "..";
                            else
                            {
                                lblRequest.Description = lblProductName.Text.ToString();
                            }

                        }

                        lblRequest.Value = Convert.ToSingle(txtProductPrice.Text);



                        if (chkInsured.Checked)
                            lblRequest.InsuredValue = txtProductPrice.Text == "" ? "0" : txtProductPrice.Text;



                        lblRequest.FromName = OrgContact;
                        lblRequest.ReturnAddress1 = OrgAddress1;
                        if (!string.IsNullOrEmpty(OrgAddress2))
                        {
                            lblRequest.ReturnAddress2 = OrgAddress2;
                        }
                        lblRequest.FromCity = OrgCity;
                        lblRequest.FromState = OrgState;
                        lblRequest.FromPostalCode = OrgShippingZip;
                        lblRequest.FromPhone = OrgPhoneZip.Replace("-", "").Replace("(", "").Replace(")", "");
                        lblRequest.FromCompany = OrgComapnyName;

                        lblRequest.ToName = Contact;
                        lblRequest.ToAddress1 = Address1.Replace("#", "");
                        if (!string.IsNullOrEmpty(Address2))
                        {

                            lblRequest.ToAddress2 = Address2.Replace("#", "");
                        }
                        lblRequest.ToCity = City;
                        if (ShippingCompany != "")
                            lblRequest.ToCompany = ShippingCompany;
                        // ShippingCompany
                        lblRequest.ValidateAddress = "FALSE";

                        lblRequest.ToPostalCode = Zip;
                        lblRequest.ToDeliveryPoint = "00";
                        lblRequest.ToPhone = PhoneNumber.Replace("-", "").Replace("(", "").Replace(")", "");
                        if (ShippingCountry != OrgCountry)
                        {
                            lblRequest.ToCountryCode = ShippingCountry;
                            lblRequest.ToCountry = ShippingCountryName;

                            lblRequest.ToState = State;
                        }
                        else
                        {
                            lblRequest.ToState = State;
                        }


                        ResponseOptions objPrice = new ResponseOptions();
                        objPrice.PostagePrice = "TRUE";
                        lblRequest.ResponseOptions = objPrice;
                        double Weight = 1;
                        if (txtProWeight.Text != "")
                            Weight = Convert.ToDouble(Convert.ToDecimal(txtProWeight.Text));
                        lblRequest.WeightOz = Math.Floor((Convert.ToDouble(Weight * 16)));// (Convert.ToInt32(Weight) * 16);
                        lblRequest.PartnerCustomerID = "0";
                        lblRequest.PartnerTransactionID = "0";

                    }


                    if (chkMail.Checked)
                    {

                        SpecialServices objServices = new SpecialServices();
                        objServices.InsuredMail = "OFF";
                        objServices.DeliveryConfirmation = "ON";
                        lblRequest.Services = objServices;

                    }

                    LabelRequestResponse objLabelRequestResponse = new LabelRequestResponse();

                    objLabelRequestResponse = objEwsLabelService.GetPostageLabel(lblRequest);


                    if (!string.IsNullOrEmpty(objLabelRequestResponse.ErrorMessage))
                    {
                        grdUSPS.Visible = false;
                        trUSPSAll.Visible = false;
                        strError += "<br/>" + objLabelRequestResponse.ErrorMessage;
                    }
                    else
                    {
                        string ImgSavePath = AppLogic.AppConfigs("USPS.LabelSavePath");
                        #region Create Image Name
                        string OrderNo = "0";
                        if (Request.QueryString["ONo"] != null && Request.QueryString["ONo"].ToString().Trim().Length > 0)
                            OrderNo = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());    //"100140"; 

                        string filename = "USPS-Package" + packageidd.ToString() + "_" + objLabelRequestResponse.TrackingNumber.Replace(" ", "").Trim().ToString() + "_" + OrderNo + "_" + strCartid.ToString() + "@" + DateTime.Now.Year.ToString() + "-" +
                                  DateTime.Now.Month.ToString() + "-" +
                                  DateTime.Now.Day.ToString() + "-T-" +
                                  DateTime.Now.Hour.ToString() + "-" +
                                  DateTime.Now.Minute.ToString() + "-" +
                                  DateTime.Now.Second.ToString() + "-" +
                                  ".gif"; // Path of the Label PDF
                        string ImgName = Server.MapPath(ImgSavePath + filename);
                        #endregion

                        string lblString = objLabelRequestResponse.Base64LabelImage;
                        byte[] arrlbl = Convert.FromBase64String(lblString);

                        try
                        {
                            FileStream fs = new FileStream(ImgName, FileMode.Create, FileAccess.ReadWrite);
                            BinaryWriter bw = new BinaryWriter(fs);
                            bw.Write(arrlbl);
                            bw.Close();

                            DataRow dataRow = myTable.NewRow();

                            dataRow["SerialNo"] = iLoop;
                            dataRow["PackageID"] = packageidd.ToString();
                            dataRow["ImgUrl"] = filename.Replace("#", "");
                            dataRow["TrackingNo"] = objLabelRequestResponse.TrackingNumber;
                            dataRow["CreateDate"] = DateTime.Now;
                            dataRow["ShippingCartID"] = lblShippingCartID.Text;
                            UpdatCartItem(objLabelRequestResponse.TrackingNumber, Productid, "USPS", Convert.ToInt32(OrderedShoppingCartID), Convert.ToInt32(lblShippedQty.Text), Convert.ToInt32(ddlWareHouse.SelectedItem.Value), Convert.ToInt32(packageidd));
                            myTable.Rows.Add(dataRow);
                            iLoop++;
                        }
                        catch { }
                    }
                }
                //create Package 

                if (ShippingCountry != OrgCountry)
                {
                    if (iItemCOunt > 5)
                    {
                        string MSGdata = "";
                        MSGdata = CheckvalidforCustomData(MailCalss, "Parcel", iItemCOunt);
                        if (MSGdata != null && MSGdata != "")
                        {
                            grdUSPS.Visible = false;
                            trUSPSAll.Visible = false;
                            strError += "<br/>" + "You can select max five item(s) per package. " + MSGdata;
                            lblmsg.Text = strError;
                            return;
                        }
                    }
                    if (iItemCOunt > 30)
                    {
                        grdUSPS.Visible = false;
                        trUSPSAll.Visible = false;
                        strError += "<br/>" + "You can select max Thirty item(s) per package.";
                        lblmsg.Text = strError;
                        return;
                    }
                }


                decimal decTotalWeight = Convert.ToDecimal(hfWeight.Value);
                txtWeight.Text = hfWeight.Value;
                if (decTotalWeight > 0)
                {

                    strCartid = "";

                    EwsLabelService objEwsLabelService = new EwsLabelService();
                    LabelRequest lblRequest = new LabelRequest();
                    int iCount = 0;
                    packageidd = Packid.ToString(); //** lblPackageIDFP.Text.ToString();
                    double TotalWight = 0;
                    decimal TotalPrice = 0;


                    CustomsInfo objcust = new CustomsInfo();
                    objcust.ContentsType = "Other";
                    objcust.ContentsExplanation = "Merchandise";
                    objcust.RestrictionType = "NONE";
                    Productid = "";
                    #region "Optional"
                    objcust.NonDeliveryOption = "RETURN";
                    objcust.EelPfc = "NOEEI 30.37(a)";
                    #endregion
                    CustomsItem[] objItem = new CustomsItem[iItemCOunt];

                    foreach (GridViewRow gvr in grdShipping.Rows)
                    {

                        System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");

                        if (chkAllowShipment.Checked == false)
                        {

                            System.Web.UI.WebControls.Label lblProductName = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductName");
                            System.Web.UI.WebControls.TextBox txtProductPrice = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProductPrice");
                            System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                            System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");
                            System.Web.UI.WebControls.Label lblVstore = (System.Web.UI.WebControls.Label)gvr.FindControl("lblVstore");
                           //* System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                            System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                            lblProductID = (System.Web.UI.WebControls.Label)gvr.FindControl("lblProductID");


                            int Qty = 0;
                            //int availQuantity = 0;
                            try { Qty = Convert.ToInt32(lblQuantity.Text); }
                            catch { Qty = 0; }
                            //try { availQuantity = Convert.ToInt32(lblavailQuantity.Text); }
                            //catch { availQuantity = 0; }

                            //if (Qty > availQuantity) { availQuantity = 0; }

                            //if (availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                            if (lblShipping.Text.ToLower() == "no")
                            {
                                Productid = Productid + lblProductID.Text + ",";
                                if (strCartid == "")
                                {
                                    strCartid = Convert.ToString(CommonComponent.GetScalarCommonData("select ShoppingCardID from tb_Order where OrderNumber =" + orderid.ToString()));
                                }
                                double WeightTotal = 1;
                                if (txtProWeight.Text != "")
                                    WeightTotal = Convert.ToDouble(Convert.ToDecimal(txtProWeight.Text));

                                TotalWight += WeightTotal;

                                //if (txtProductPrice.Text != "" && availQuantity != 0 && lblShipping.Text.ToLower() == "no")
                                if (txtProductPrice.Text != "" && lblShipping.Text.ToLower() == "no")
                                {
                                    TotalPrice += Convert.ToDecimal(txtProductPrice.Text);
                                }


                                objItem[iCount] = new CustomsItem();

                                if (lblVstore.Text != null && lblVstore.Text != "")
                                {
                                    if (lblProductName.Text.ToString().Replace(Convert.ToString(lblVstore.Text), "").Length > 50)
                                    {
                                        objItem[iCount].Description = lblProductName.Text.ToString().Replace(Convert.ToString(lblVstore.Text), "").Substring(0, 47) + "..";
                                    }
                                    else
                                        objItem[iCount].Description = lblProductName.Text.ToString().Replace(Convert.ToString(lblVstore.Text), "");

                                }
                                else
                                {
                                    if (lblProductName.Text.ToString().Length > 50)
                                        objItem[iCount].Description = lblProductName.Text.ToString().Substring(0, 47) + "..";
                                    else
                                        objItem[iCount].Description = lblProductName.Text.ToString();
                                }



                                objItem[iCount].Quantity = Convert.ToInt32(lblQuantity.Text); ;
                                objItem[iCount].Value = Convert.ToDecimal(txtProductPrice.Text);
                                objItem[iCount].Weight = Convert.ToDecimal(Math.Floor(Convert.ToDecimal(txtProWeight.Text) * 16));
                                objItem[iCount].CountryOfOrigin = "US";


                                iCount++;

                            }
                        }

                    }
                    objcust.CustomsItems = objItem;
                    lblRequest.CustomsInfo = objcust;
                    string TestAccount = AppLogic.AppConfigs("USPS.IsTestMode");
                    if (TestAccount == "0")
                    {
                        TestAccount = "No";
                    }
                    else
                        TestAccount = "Yes";

                    lblRequest.AccountID = AppLogic.AppConfigs("USPS.UserName");
                    lblRequest.PassPhrase = AppLogic.AppConfigs("USPS.Password");
                    lblRequest.Test = TestAccount;
                    lblRequest.MailClass = MailCalss;
                    lblRequest.LabelType = "International";
                    lblRequest.LabelSubtype = "Integrated";
                    lblRequest.IntegratedFormType = ReturnFormNumber(MailCalss, "Parcel", iItemCOunt);
                    lblRequest.LabelSize = "4X6";
                    lblRequest.ImageFormat = "GIF";
                    lblRequest.RequesterID = "12";
                    lblRequest.ImageResolution = "300";
                    lblRequest.MailpieceShape = "Parcel";
                    lblRequest.Stealth = "True";

                    Dimensions objdimension = new Dimensions();
                    objdimension.Height = Convert.ToDouble(txtHeight.Text);
                    objdimension.Width = Convert.ToDouble(txtWeight.Text);
                    objdimension.Length = Convert.ToDouble(txtLength.Text);
                    lblRequest.MailpieceDimensions = objdimension;
                    //lblRequest.Description = lblProductName.Text;
                    //lblRequest.Value = Convert.ToSingle(txtProductPrice.Text);
                    lblRequest.InsuredValue = TotalPrice.ToString();
                    lblRequest.WeightOz = Math.Floor((Convert.ToDouble(TotalWight) * 16));
                    lblRequest.FromName = OrgContact;
                    lblRequest.ReturnAddress1 = OrgAddress1;
                    if (!string.IsNullOrEmpty(OrgAddress2))
                    {
                        lblRequest.ReturnAddress2 = OrgAddress2;
                    }
                    lblRequest.FromCity = OrgCity;
                    lblRequest.FromState = OrgState;
                    lblRequest.FromPostalCode = OrgShippingZip;
                    lblRequest.FromPhone = OrgPhoneZip.Replace("-", "").Replace("(", "").Replace(")", "");
                    lblRequest.FromCompany = OrgComapnyName;

                    lblRequest.ToName = Contact;
                    lblRequest.ToAddress1 = Address1.Replace("#", "");
                    if (!string.IsNullOrEmpty(Address2))
                    {
                        lblRequest.ToAddress2 = Address2.Replace("#", "");
                    }
                    lblRequest.ToCity = City;
                    if (ShippingCompany != "")
                        lblRequest.ToCompany = ShippingCompany;
                    lblRequest.ValidateAddress = "FALSE";

                    lblRequest.ToPostalCode = Zip;
                    lblRequest.ToDeliveryPoint = "00";
                    lblRequest.ToPhone = PhoneNumber.Replace("-", "").Replace("(", "").Replace(")", "");
                    if (ShippingCountry != OrgCountry)
                    {
                        lblRequest.ToCountryCode = ShippingCountry;
                        lblRequest.ToCountry = ShippingCountryName;

                        lblRequest.ToState = State;
                    }
                    else
                    {
                        lblRequest.ToState = State;

                    }

                    ResponseOptions objPrice = new ResponseOptions();
                    objPrice.PostagePrice = "TRUE";
                    lblRequest.ResponseOptions = objPrice;

                    lblRequest.PartnerCustomerID = "0";
                    lblRequest.PartnerTransactionID = "0";

                    if (chkMail.Checked)
                    {
                        SpecialServices objServices = new SpecialServices();
                        objServices.InsuredMail = "OFF";
                        objServices.DeliveryConfirmation = "ON";
                        lblRequest.Services = objServices;
                    }

                    LabelRequestResponse objLabelRequestResponse = new LabelRequestResponse();

                    objLabelRequestResponse = objEwsLabelService.GetPostageLabel(lblRequest);

                    if (!string.IsNullOrEmpty(objLabelRequestResponse.ErrorMessage))
                    {
                        grdUSPS.Visible = false;
                        trUSPSAll.Visible = false;
                        strError += "<br/>" + objLabelRequestResponse.ErrorMessage;
                    }
                    else
                    {

                        if (objLabelRequestResponse.Label.Image.Length == 1)
                        {

                            string ImgSavePath = AppLogic.AppConfigs("USPS.LabelSavePath");
                            if (!Directory.Exists(Server.MapPath(ImgSavePath)))
                                Directory.CreateDirectory(Server.MapPath(ImgSavePath));
                            #region Create Image Name
                            string OrderNo = "0";
                            if (Request.QueryString["ONo"] != null && Request.QueryString["ONo"].ToString().Trim().Length > 0)
                                OrderNo = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());     //"100140"; 
                            string filename = "USPS-Package" + packageidd.ToString() + "_" + objLabelRequestResponse.TrackingNumber.Replace(" ", "").Trim().ToString() + "_" + OrderNo + "_" + strCartid.ToString() + "@" + DateTime.Now.Year.ToString() + "-" +
                                      DateTime.Now.Month.ToString() + "-" +
                                      DateTime.Now.Day.ToString() + "-T-" +
                                      DateTime.Now.Hour.ToString() + "-" +
                                      DateTime.Now.Minute.ToString() + "-" +
                                      DateTime.Now.Second.ToString() + "-" +
                                      ".gif"; // Path of the Label PDF
                            string ImgName = Server.MapPath(ImgSavePath + filename);
                            #endregion

                            //string lblString = objLabelRequestResponse.Base64LabelImage;
                            string lblString = objLabelRequestResponse.Label.Image[0].Value;
                            byte[] arrlbl = Convert.FromBase64String(lblString);

                            try
                            {
                                FileStream fs = new FileStream(ImgName, FileMode.Create, FileAccess.ReadWrite);
                                BinaryWriter bw = new BinaryWriter(fs);
                                bw.Write(arrlbl);
                                bw.Close(); //Thanks Karlo for pointing out!
                                DataRow dataRow = myTable.NewRow();


                                dataRow["SerialNo"] = iLoop;
                                dataRow["PackageID"] = packageidd;
                                dataRow["ImgUrl"] = filename.Replace("#", "");
                                dataRow["TrackingNo"] = objLabelRequestResponse.TrackingNumber;
                                dataRow["CreateDate"] = DateTime.Now;
                                dataRow["ShippingCartID"] = strCartid.ToString();
                                myTable.Rows.Add(dataRow);


                                if (Productid != "")
                                {
                                    Productid = Productid.Substring(0, Productid.Length - 1);
                                }


                                UpdatCartItem(objLabelRequestResponse.TrackingNumber, Productid, "USPS", Convert.ToInt32(OrderedShoppingCartID), Convert.ToInt32(lblShippedQty.Text), Convert.ToInt32(ddlWareHouse.SelectedItem.Value), Convert.ToInt32(packageidd));

                                iLoop++;
                            }
                            catch { }

                        }
                        else
                        {
                            int ImgCount = 1;
                            for (int iLabel = 0; iLabel <= objLabelRequestResponse.Label.Image.Length - 1; iLabel++)
                            {
                                string ImgSavePath = AppLogic.AppConfigs("USPS.LabelSavePath");
                                if (!Directory.Exists(Server.MapPath(ImgSavePath)))
                                    Directory.CreateDirectory(Server.MapPath(ImgSavePath));
                                #region Create Image Name
                                string OrderNo = "0";
                                if (Request.QueryString["ONo"] != null && Request.QueryString["ONo"].ToString().Trim().Length > 0)
                                    OrderNo = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                                string filename = "USPS-Package" + packageidd.ToString() + " _" + ImgCount + "_" + objLabelRequestResponse.TrackingNumber.Replace(" ", "").Trim().ToString() + "_" + OrderNo + "_" + strCartid.ToString() + "@" + DateTime.Now.Year.ToString() + "-" +
                                          DateTime.Now.Month.ToString() + "-" +
                                          DateTime.Now.Day.ToString() + "-T-" +
                                          DateTime.Now.Hour.ToString() + "-" +
                                          DateTime.Now.Minute.ToString() + "-" +
                                          DateTime.Now.Second.ToString() + "-" +
                                          ".gif";
                                string ImgName = Server.MapPath(ImgSavePath + filename);
                                #endregion


                                string lblString = objLabelRequestResponse.Label.Image[iLabel].Value;
                                byte[] arrlbl = Convert.FromBase64String(lblString);

                                try
                                {
                                    FileStream fs = new FileStream(ImgName, FileMode.Create, FileAccess.ReadWrite);
                                    BinaryWriter bw = new BinaryWriter(fs);
                                    bw.Write(arrlbl);
                                    bw.Close();
                                    DataRow dataRow = myTable.NewRow();


                                    dataRow["SerialNo"] = iLoop;
                                    dataRow["PackageID"] = packageidd;
                                    dataRow["ImgUrl"] = filename.Replace("#", "");
                                    dataRow["TrackingNo"] = objLabelRequestResponse.TrackingNumber;
                                    dataRow["CreateDate"] = DateTime.Now;
                                    dataRow["ShippingCartID"] = strCartid.ToString();
                                    myTable.Rows.Add(dataRow);




                                    iLoop++;
                                }
                                catch { }
                                ImgCount++;
                            }
                            if (Productid != "")
                            {
                                Productid = Productid.Substring(0, Productid.Length - 1);
                            }
                            UpdatCartItem(objLabelRequestResponse.TrackingNumber, Productid, "USPS", Convert.ToInt32(OrderedShoppingCartID), Convert.ToInt32(lblShippedQty.Text), Convert.ToInt32(ddlWareHouse.SelectedItem.Value), Convert.ToInt32(packageidd));

                        }
                    }

                }
                if (myTable.Rows.Count > 0)
                {
                    grdUSPS.DataSource = myTable;
                    grdUSPS.DataBind();
                    grdUSPS.Visible = true;
                    trUSPSAll.Visible = true;
                    //ltUSPSShippingLabel.Text = "";
                }
                if (!string.IsNullOrEmpty(strError))
                    ltUSPSShippingLabel.Text = strError;

            }
            catch (Exception ex)
            {
                grdUSPS.Visible = false;
                trUSPSAll.Visible = false;
                lblmsg.Text = ex.Message;
                lblmsg.Text = ex.Message;
            }
        }

        #endregion

        /// <summary>
        ///  Delete USPS Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnlnkDelUSPS_Click(object sender, EventArgs e)
        {
            try
            {
                panSelectMethod.Visible = false;
                DeleteLabel(string.Empty, "USPS");
            }
            catch { }
        }

        /// <summary>
        ///  Delete UPS Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnlnkDelUPS_Click(object sender, EventArgs e)
        {
            try
            {
                panSelectMethod.Visible = false;
                DeleteLabel(string.Empty, "UPS");
            }
            catch { }
        }

        /// <summary>
        ///  Delete FEDEX Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnlnkDelFEDEX_Click(object sender, EventArgs e)
        {
            try
            {
                panSelectMethod.Visible = false;
                DeleteLabel(string.Empty, "FEDEX");
            }
            catch { }
        }

        /// <summary>
        ///  Send Mail All USPS Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSendMailAllUSPS_Click(object sender, EventArgs e)
        {
            try
            {
                //*   SendConfirmationMailAll("USPS", getMailData("USPS"));
            }
            catch { }
        }

        /// <summary>
        ///  Send Mail All UPS Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSendMailAllUPS_Click(object sender, EventArgs e)
        {
            try
            {
                //*   SendConfirmationMailAll("USPS", getMailData("USPS"));
            }
            catch { }
        }

        /// <summary>
        /// USPS Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdUSPS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                if (e.CommandArgument.ToString() != "")
                {
                    try
                    {
                        downloadfile(Server.MapPath(AppLogic.AppConfigs("USPS.LabelSavePath") + e.CommandArgument.ToString().Trim()));
                    }
                    catch { }
                }
            }
            else if (e.CommandName == "SendMail")
            {
                if (e.CommandArgument.ToString() != "")
                {
                    try
                    {
                        SendConfirmationMail(e.CommandArgument.ToString(), "USPS");
                        string OrderNumber = string.Empty;
                        if (Request.QueryString["ONo"] != null)
                        {
                            OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                        }
                        BindData(Convert.ToInt32(OrderNumber));

                    }
                    catch { }
                }
            }
            else if (e.CommandName == "Dellabel")
            {
                if (e.CommandArgument.ToString() != "")
                {
                    try
                    {
                        DeleteLabel(e.CommandArgument.ToString(), "USPS");
                        if (grdUSPS.Rows.Count <= 0)
                        {
                            panUSPS.Visible = false;
                        }
                    }
                    catch { }
                }
            }

        }

        /// <summary>
        /// UPS Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdUPS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                if (e.CommandArgument.ToString() != "")
                {
                    try
                    {
                        downloadfile(Server.MapPath(AppLogic.AppConfigs("UPS.LabelSavePath") + e.CommandArgument.ToString().Trim()));
                    }
                    catch { }
                }
            }
            else if (e.CommandName == "SendMail")
            {
                if (e.CommandArgument.ToString() != "")
                {
                    try
                    {
                        SendConfirmationMail(e.CommandArgument.ToString(), "UPS");
                        string OrderNumber = string.Empty;
                        if (Request.QueryString["ONo"] != null)
                        {
                            OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                        }
                        BindData(Convert.ToInt32(OrderNumber));
                    }
                    catch { }
                }
            }
            else if (e.CommandName == "Dellabel")
            {
                if (e.CommandArgument.ToString() != "")
                {
                    try
                    {
                        DeleteLabel(e.CommandArgument.ToString(), "UPS");

                        if (grdUPS.Rows.Count <= 0)
                        {
                            grdUPS.Visible = false;
                        }
                    }
                    catch { }
                }
            }

        }

        /// <summary>
        /// UPS Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdFEDEX_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                if (e.CommandArgument.ToString() != "")
                {
                    try
                    {
                        downloadfile(Server.MapPath(AppLogic.AppConfigs("FEDEX.LabelSavePath") + e.CommandArgument.ToString().Trim()));
                    }
                    catch { }
                }
            }
            else if (e.CommandName == "SendMail")
            {
                if (e.CommandArgument.ToString() != "")
                {
                    try
                    {
                        SendConfirmationMail(e.CommandArgument.ToString(), "FEDEX");
                        string OrderNumber = string.Empty;
                        if (Request.QueryString["ONo"] != null)
                        {
                            OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                        }
                        BindData(Convert.ToInt32(OrderNumber));
                    }
                    catch { }
                }
            }
            else if (e.CommandName == "Dellabel")
            {
                if (e.CommandArgument.ToString() != "")
                {
                    try
                    {
                        DeleteLabel(e.CommandArgument.ToString(), "FEDEX");

                        if (grdUPS.Rows.Count <= 0)
                        {
                            grdUPS.Visible = false;
                        }
                    }
                    catch { }
                }
            }

        }

        /// <summary>
        /// Sends the Confirmation Mail
        /// </summary>
        /// <param name="TrkNo">string TrkNo</param>
        /// <param name="type">string type</param>
        private void SendConfirmationMail(string TrkNo, string type)
        {
            try
            {
                int LableNo = 1;
                int cntLable = 1;
                int pckcount = 0;
                string url = "";
                string PackageID = string.Empty;
                string ProductID = string.Empty;

                if (type == "USPS")
                {
                    foreach (GridViewRow gvr in grdUSPS.Rows)
                    {
                        System.Web.UI.WebControls.LinkButton btnSendMail = (System.Web.UI.WebControls.LinkButton)gvr.FindControl("btnSendMail");
                        if (btnSendMail != null && !string.IsNullOrEmpty(btnSendMail.CommandArgument))
                        {
                            if (btnSendMail.CommandArgument == TrkNo)
                            {
                                LableNo = cntLable;

                                PackageID = (gvr.FindControl("lblPackageID") as System.Web.UI.WebControls.Label).Text;
                            }
                            cntLable++;
                        }
                    }
                    pckcount = grdUSPS.Rows.Count;
                    url = "http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?origTrackNum={0}";
                }

                if (type == "UPS")
                {
                    foreach (GridViewRow gvr in grdUPS.Rows)
                    {
                        System.Web.UI.WebControls.LinkButton btnSendMail = (System.Web.UI.WebControls.LinkButton)gvr.FindControl("btnSendMail");
                        if (btnSendMail != null && !string.IsNullOrEmpty(btnSendMail.CommandArgument))
                        {
                            if (btnSendMail.CommandArgument == TrkNo)
                            {
                                LableNo = cntLable;

                                PackageID = (gvr.FindControl("lblPackageID") as System.Web.UI.WebControls.Label).Text;
                            }
                            cntLable++;
                        }
                    }
                    pckcount = grdUPS.Rows.Count;
                    url = "http://wwwapps.ups.com/WebTracking/processInputRequest?sort_by=status&tracknums_displayed=1&TypeOfInquiryNumber=T&loc=en_US&InquiryNumber1={0}&track.x=0&track.y=0";
                }

                if (type == "FEDEX")
                {
                    foreach (GridViewRow gvr in grdFEDEX.Rows)
                    {
                        System.Web.UI.WebControls.LinkButton btnSendMail = (System.Web.UI.WebControls.LinkButton)gvr.FindControl("btnSendMail");
                        if (btnSendMail != null && !string.IsNullOrEmpty(btnSendMail.CommandArgument))
                        {
                            if (btnSendMail.CommandArgument == TrkNo)
                            {
                                LableNo = cntLable;

                                PackageID = (gvr.FindControl("lblPackageID") as System.Web.UI.WebControls.Label).Text;
                            }
                            cntLable++;
                        }
                    }
                    pckcount = grdFEDEX.Rows.Count;
                    url = "http://www.fedex.com/Tracking?action=track&language=english&cntry_code=us&initial=x&tracknumbers={0}";
                }

                string TrackNo = (TrkNo.Split('-').Length > 0) ? TrkNo.Split('-')[0] : "";

                url = string.Format(url, TrackNo);
                string ShoppingCartID = (TrkNo.Split('-').Length > 1) ? TrkNo.Split('-')[1] : "";
                ProductID = GetProductIDsFromPackageIDs(PackageID, type);
                if (Request.QueryString["ONo"] != null && Request.QueryString["ONo"].ToString().Trim().Length > 0)
                {
                    string OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());//"100140"; 


                    if (!string.IsNullOrEmpty(OrderNumber))
                    {


                        //DataSet dsPproductInt = CommonComponent.GetCommonDataSet("select RefProductID,WareHouseID,Quantity from tb_orderedShoppingcartitems  where TrackingNumber='" + TrackNo + "' and OrderedShoppingCartID='" + ShoppingCartID + "' AND PackId='" + PackageID + "'  and  ISNULL(InventoryUpdated,0)=0 ");
                        //// DataSet dsPproductInt = CommonComponent.GetCommonDataSet("select RefProductID,WareHouseID,Quantity from tb_orderedShoppingcartitems  where TrackingNumber='" + TrackNo + "' and OrderedCustomCartID='" + ShoppingCartID + "' AND PackId='" + PackageID + "'  and  ISNULL(InventoryUpdated,0)=0 ");

                        ////***2
                        //if (dsPproductInt != null && dsPproductInt.Tables.Count > 0 && dsPproductInt.Tables[0].Rows.Count > 0)
                        //{

                        //    for (int i = 0; i <= dsPproductInt.Tables[0].Rows.Count - 1; i++)
                        //    {
                        //        int Qty = Convert.ToInt32(dsPproductInt.Tables[0].Rows[i]["Quantity"].ToString());
                        //        //  dsPproductInt.Tables[0].Rows[i]["RefProductID"].ToString();
                        //        //dsPproductInt.Tables[0].Rows[i]["WareHouseID"].ToString();

                        //        int Inventory = 0;
                        //        Inventory = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 isnull(Inventory,0) from tb_WareHouseProductInventory  where ProductID='" + dsPproductInt.Tables[0].Rows[i]["RefProductID"].ToString() + "' and WareHouseID='" + dsPproductInt.Tables[0].Rows[i]["WareHouseID"].ToString() + "' "));
                        //        if (Inventory != null)
                        //        {
                        //            Inventory = Inventory - Qty;
                        //            CommonComponent.ExecuteCommonData("Update tb_WareHouseProductInventory set Inventory='" + Inventory + "' where ProductID='" + dsPproductInt.Tables[0].Rows[i]["RefProductID"].ToString() + "'  and WareHouseID='" + dsPproductInt.Tables[0].Rows[i]["WareHouseID"].ToString() + "' ");
                        //            CommonComponent.ExecuteCommonData("Update tb_orderedShoppingcartitems set InventoryUpdated= 1 where RefProductID='" + dsPproductInt.Tables[0].Rows[i]["RefProductID"].ToString() + "' and WareHouseID='" + dsPproductInt.Tables[0].Rows[i]["WareHouseID"].ToString() + "'");

                        //            //

                        //            Object ObjPinventory = CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM dbo.tb_Product WHERE ProductID='" + dsPproductInt.Tables[0].Rows[i]["RefProductID"].ToString() + "'");
                        //            int Pinventory = 0;
                        //            int.TryParse(Convert.ToString(ObjPinventory), out Pinventory);

                        //            //if (Pinventory > Qty)
                        //            //{
                        //            Pinventory = Pinventory - Qty;
                        //            //}

                        //            CommonComponent.ExecuteCommonData("UPDATE dbo.tb_Product SET Inventory='" + Pinventory + "' WHERE ProductID='" + dsPproductInt.Tables[0].Rows[i]["RefProductID"].ToString() + "'");
                        //            //
                        //        }
                        //        //
                        //    }
                        //}

                        string sql = "select * from tb_Order where OrderNumber=" + OrderNumber;
                        DataSet dsOrderDetails = CommonComponent.GetCommonDataSet(sql);

                        //*        clsOrder ord = new clsOrder(Convert.ToInt32(OrderNumber));
                        try
                        {
                            ArrayList ProductIds = new ArrayList();
                            ArrayList TrackingNumber = new ArrayList();
                            ArrayList CourierName = new ArrayList();
                            ArrayList ShippedDateList = new ArrayList();

                            ProductIds.Add(ProductID);
                            TrackingNumber.Add(TrackNo);
                            CourierName.Add(type);
                            ShippedDateList.Add(DateTime.Now);

                            MarkProductsShipped(Convert.ToInt32(OrderNumber), TrackNo, type, Convert.ToInt32(PackageID));
                            //if (ProductID != "-1")
                            //    MarkProductsShipped(Convert.ToInt32(OrderNumber), TrackNo, type);
                            //MarkOrderAsShippedforShippingLabel(Convert.ToInt32(OrderNumber), type, TrackNo, DateTime.Now);

                        }
                        catch { }

                        String ServerURL = "http:///www." + AppLogic.AppConfigs("LiveServer") + "/";
                        String cart = getProductDetailForMail(ShoppingCartID, OrderNumber, ProductID, TrackNo, PackageID).ToString();
                        string ShipToAddress = string.Empty;
                        if (dsOrderDetails != null && dsOrderDetails.Tables.Count > 0 && dsOrderDetails.Tables[0].Rows.Count > 0)
                        {
                            ShipToAddress += "<table>";
                            ShipToAddress += "<tr><td>";
                            ShipToAddress += "<tr><td>" + dsOrderDetails.Tables[0].Rows[0]["ShippingCompany"].ToString() + "</td></tr>";
                            ShipToAddress += dsOrderDetails.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsOrderDetails.Tables[0].Rows[0]["LastName"].ToString() + "<br />";
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingAddress1"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingAddress1"].ToString() + "<br />");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingAddress2"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingAddress2"].ToString() + ", ");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingPhone"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingPhone"].ToString() + "<br />");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingCity"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingCity"].ToString() + ", ");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingState"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingState"].ToString() + "<br />");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingCountry"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingCountry"].ToString() + ", ");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingZip"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br />");
                            ShipToAddress += "</td></tr>";
                            ShipToAddress += "</table>";

                        }

                        StringBuilder messageBody = new StringBuilder(10000);

                        #region mail format

                        CustomerComponent objCustomer = new CustomerComponent();
                        DataSet dsMailInfo = new DataSet();
                        dsMailInfo = objCustomer.GetEmailTamplate("ShippingMailConfirm", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                        if (dsMailInfo != null && dsMailInfo.Tables.Count > 0 && dsMailInfo.Tables[0].Rows.Count > 0)
                        {
                            String strBody = "";
                            String strSubject = "";
                            strBody = dsMailInfo.Tables[0].Rows[0]["EmailBody"].ToString();
                            strSubject = dsMailInfo.Tables[0].Rows[0]["Subject"].ToString();

                            strSubject = Regex.Replace(strSubject, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                            strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("StoreName").ToString(), RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("StoreName").ToString(), RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###FIRSTNAME###", dsOrderDetails.Tables[0].Rows[0]["FirstName"].ToString() + ' ' + dsOrderDetails.Tables[0].Rows[0]["LastName"].ToString(), RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###Product_Lists###", cart, RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###Ship To###", ShipToAddress, RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###shipping method###", type, RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###Order Number###", OrderNumber, RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###Tracking Number###", TrackNo + "&nbsp;&nbsp;<a  href=\"" + url + "\">Click Here to Track Your Package</a>", RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###Number of Packages###", LableNo.ToString() + " of " + pckcount, RegexOptions.IgnoreCase);

                            AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");

                            string ToName = ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["BillingFirstName"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["BillingLastName"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["BillingLastName"].ToString());
                            string ToMail = ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["BillingEmail"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["BillingEmail"].ToString());
                            CommonOperations.SendMail(ToMail, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);

                        }

                        #region Else
                        else
                        {
                            messageBody.Append("<html>");
                            messageBody.Append("<head>");
                            messageBody.Append("<meta content='text/html; charset=iso-8859-1' http-equiv='Content-Type'/>");
                            messageBody.Append("<title>Welcome to " + AppLogic.AppConfigs("StoreName").ToString() + " - Order Shipped</title>");
                            messageBody.Append("<link type='text/css' rel='stylesheet' href='../Client/style.css' />    ");
                            messageBody.Append("<style type='text/css'>");
                            messageBody.Append("<!-- " +

                                           ".popup_box {" +
                                           " background-color: #F6F1EB;" +
                                            "margin-left:auto;" +
                                           " margin-right:auto;" +
                                            "padding:14px;" +
                                           " position:relative;" +
                                            "width:596px;" +
                                            "}" +

                                       " .popup_docwidth {" +
                                        "background-color:#ffffff;" +
                                        "border:1px solid #CCCCCC;" +
                                        "width:596px;" +
                                        "}" +

                                       ".pop_header {" +
                                       " background:url(" + AppLogic.AppConfigs("Live_Server") + "/Client/images/Header_bg_Kit.gif) center top repeat-x; background-color:#000000;" +
                                        "float:left;" +
                                        "height:54px;" +
                                        "padding:0;" +
                                        "width:100%;" +
                                        "}" +

                                        ".pop_header_row1 {" +
                                        "clear:both;" +
                                        "padding-left:155px;" +
                                        "padding-top:12px;*padding-top:0px;" +
                                        "width:500px;" +
                                        "}" +

                                      " .pop_header_row2 {" +
                                        "background:#DBD4CC none repeat scroll 0 0;" +
                                        "border:1px solid #DBD4CC;" +
                                        "clear:both;" +
                                        "color:#95182B;" +
                                        "font-family:Arial,Helvetica,sans-serif;" +
                                        "font-size:12px;" +
                                        "line-height:23px;" +
                                        "text-align:center;" +
                                        "width:596px;" +
                                        "}" +

                                  ".pop_header_row2 a {" +
                                   " color:#95182B;" +
                                    "font-family:Arial,Helvetica,sans-serif;" +
                                    "font-size:12px;" +
                                     "font-weight:bold;" +
                                    "text-decoration:none;" +
                                    "}" +

                                " .pop_header_row2 a:hover {" +
                                //"color:#FDBE41;" +
                                    "color:#2E2D2D;" +
                                    "font-family:Arial,Helvetica,sans-serif;" +
                                   " font-size:12px;" +
                                    "text-decoration:none;" +
                                    "}" +

                                    ".popup_header2 {" +
                                        "width:596px;" +
                                        "}" +


                                    " .popup_cantain {" +
                                        "color:#585858;" +
                                        "font-family:Arial,Helvetica,sans-serif;" +
                                        "font-size:12px;" +
                                        "line-height:20px;" +
                                        "padding-left:10px;" +
                                        "text-decoration:none;" +
                                        "}" +


                                   " .popup_cantain a {" +
                                    "color:#95182B;" +
                                    "font-family:Arial,Helvetica,sans-serif;" +
                                    "font-size:12px;" +
                                    "font-weight:bold;" +
                                    "outline-color:-moz-use-text-color;" +
                                    "outline-style:none;" +
                                    "outline-width:medium;" +
                                    "text-decoration:none;" +
                                    "}" +

                                  ".popup_cantain a:hover {" +
                                    "color:#938B85;" +
                                    "font-family:Arial,Helvetica,sans-serif;" +
                                    "font-size:12px;" +
                                    "text-decoration:underline;" +
                                    "}" +


                                    ".popup_fotter {" +
                                    "color:#585858;" +
                                    "font-family:Arial,Helvetica,sans-serif;" +
                                    "font-size:11px;" +
                                    "height:40px;" +
                                    "line-height:40px;" +
                                    "padding-left:10px;" +
                                    "width:586px;" +
                                    "}" +

                                           " .style1 {" +
                                            " width: 586px;" +
                                             "padding-left: 10px;" +
                                             "height: 30px;" +
                                            "line-height: 25px;" +
                                            "font-family: Verdana, Arial, Helvetica, sans-serif;" +
                                            " font-size: 11px;" +
                                            " color: #000000;" +
                                             "text-align:center;" +
                                            " font-weight: bold;" +
                                            "} " +
                                            ".datatable { font-family: Verdana, Arial, Helvetica, sans-serif;" +
                                     "font-size: 11px;" +
                                     "color: #333333;}" +



                                      "body { margin-right: 6px;font-size:12px; font-family:Verdana,Arial,Helvetica,sans-serif;	margin-bottom: 6px;	margin-left: 6px;	background-color: #F6F1EB;} " +
                                       "img{ border:0px;} " +
                                        ".toll_free_font {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #8c8c8c;	text-decoration: none;} " +

                                             ".content_font_gray {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #606060;	text-decoration: none;} " +
                                             ".content_font_orange {	font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #cc0000;	text-decoration: none;} " +
                                             "a.content_font_orange:hover {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #333333;	text-decoration: none;} " +
                                               " .user_bg {width:300px;height:60px;padding-top:7px;padding-left:7px;background-color:#ECE9DE;color:#938B85;}" +
                                                ".horizone_line { border-bottom:#999999 solid 1px;} " +
                                                 ".footer_font {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #999999;	text-decoration: none;	line-height: 18px;} " +
                                                   "a.footer_font:hover {font-family: Verdana,Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #333333;	text-decoration: none;	line-height: 18px;} " +
                                                     "--> " +
                                                     "</style> ");
                            messageBody.Append("</head>");
                            messageBody.Append("<body style='margin-right: 6px;font-size:12px; font-family:Verdana,Arial,Helvetica,sans-serif;	margin-bottom: 6px;	margin-left: 6px;	background-color: #F6F1EB;'>");
                            messageBody.Append("<table cellspacing='0' cellpadding='0' align='center' class='popup_box' style='background-color: #F6F1EB;margin-left:auto; margin-right:auto;padding:14px;position:relative;width:596px;'>");
                            messageBody.Append("<tr>");
                            messageBody.Append("<td>");
                            messageBody.Append("<table cellspacing='0' cellpadding='0' border='0' align='center' class='popup_docwidth' style='background-color:#ffffff;border:1px solid #CCCCCC;width:596px;'>");
                            messageBody.Append("<tr>");

                            messageBody.Append("<td>");
                            messageBody.Append("<table cellspacing='0' cellpadding='0' border='0' align='left' width='100%'>");
                            messageBody.Append("<tr class=''>");
                            messageBody.Append("<td class='' style=''>");
                            messageBody.Append("<a title='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "' href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "'>");

                            messageBody.Append("<img style='border-width:0px;' src=\"" + AppLogic.AppConfigs("LIVE_SERVER") + "/Client/images/mail_logo.jpg\"/>");

                            messageBody.Append("</a></td>");
                            messageBody.Append("</tr>");
                            messageBody.Append("<tr>");
                            messageBody.Append("<td><table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                            messageBody.Append("<tr >");

                            messageBody.Append("<td  align='center'   class='pop_header_row2' style='background:#DBD4CC none repeat scroll 0 0;border:1px solid #DBD4CC;clear:both;color:#95182B;font-family:Arial,Helvetica,sans-serif;font-size:12px;line-height:23px;text-align:center;width:596px;'>");
                            messageBody.Append("<a title='About Us' style='text-decoration:none;' href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/about-us.aspx'>About Us</a>   |     <a title='Contact Us' style='text-decoration:none;' href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/ContactUs.aspx'>Contact Us</a>   |   <a style='text-decoration:none;' href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/Login.aspx' title='Login'>Login</a>");
                            messageBody.Append("</td></tr></table></td>");
                            messageBody.Append("</tr>");


                            messageBody.Append("<tr>");
                            messageBody.Append("<td class='popup_header2' style=''><img width='594px' style='border-width:0px;'  src='" + AppLogic.AppConfigs("LIVE_SERVER") + "/Client/images/mail_banner.jpg'/></td>");
                            messageBody.Append("</tr>");



                            messageBody.Append("</table></td>");
                            messageBody.Append("</tr>");


                            messageBody.Append("<tr>");
                            messageBody.Append("<td><table style='color:#585858;font-family:Arial,Helvetica,sans-serif;font-size:12px;line-height:20px;padding-left:10px;text-decoration:none;' class='popup_cantain' cellspacing='0' cellpadding='0' border='0' align='left' >");
                            messageBody.Append("<tr>");
                            messageBody.Append("<td height='25' align='left' valign='bottom' class='popup_cantain' style='color:#585858;font-family:Arial,Helvetica,sans-serif;font-size:12px;line-height:20px;padding-left:10px;text-decoration:none;'><span><br/>This message was sent to notify you that the electronic shipment has been transmitted to<b> " + type + "</b>. The physical package(s) may or may not have actually been tendered to <b> " + type + "</b> for shipment.<br /><br /></span></td>");



                            messageBody.Append("</tr>");

                            messageBody.Append("<tr>");

                            messageBody.Append("<td height='75' class='popup_cantain' valign='top'><table cellspacing='0' cellpadding='0' border='0' style='width:100%;padding-right:20px'>");
                            messageBody.Append("<tr>");

                            messageBody.Append("<td  width='550' height='67' class='user_bg' style='background-color:#ECE9DE;'><table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                            messageBody.Append("<tr valign='top'>");

                            messageBody.Append("<td class='popup_cantain' height='22'   style='color:#585858; width: 100px;'>Order Number :</td>");

                            //*if (ord.OldOrderNumber != 0)
                            //*    messageBody.Append("<td class='popup_cantain' style='color:#585858;width: 311px;' >" + ord.OldOrderNumber.ToString() + "</td>");
                            //*else
                            messageBody.Append("<td class='popup_cantain' style='color:#585858;width: 311px;' >" + OrderNumber.ToString() + "</td>");

                            messageBody.Append("</tr> <tr>");
                            messageBody.Append("<td class='popup_cantain' style='color:#585858; width: 100px;'>Tracking Number  :</td>");
                            messageBody.Append("<td class='popup_cantain' style='color:#585858;width: 311px;'>" + TrackNo.ToString() + "&nbsp;&nbsp;<a  href=\"" + url + "\">Click Here to Track Your Package</a></td></tr>");
                            messageBody.Append("<tr>");
                            messageBody.Append("<td class='popup_cantain' style='color:#585858; width: 100px;'>Number of Packages  :</td>");
                            messageBody.Append("<td class='popup_cantain' style='color:#585858;width: 311px;'>" + LableNo.ToString() + " of " + pckcount + "</td></tr>");
                            messageBody.Append("</table></td>");

                            messageBody.Append("</tr></table></td>");
                            messageBody.Append("</tr>");
                            messageBody.Append("<tr>");
                            messageBody.Append("<td height='25' align='left' valign='bottom' class='popup_cantain' style='color:#585858;font-family:Arial,Helvetica,sans-serif;font-size:12px;line-height:20px;padding-left:10px;text-decoration:none;'><br><strong>Product Detail:</strong><br>" + cart + "</td>");



                            messageBody.Append("</tr>");
                            messageBody.Append("<tr>");
                            messageBody.Append("<td height='25' align='left' valign='bottom' class='popup_cantain' style='color:#585858;font-family:Arial,Helvetica,sans-serif;font-size:12px;line-height:20px;padding-left:10px;text-decoration:none;'><br/><strong>Ship To</strong><br/>" + ShipToAddress + "</td>");



                            messageBody.Append("</tr>");

                            messageBody.Append("<tr><td> </td></tr><tr>");
                            messageBody.Append("<td align='left' class='popup_cantain' style='padding-bottom: 10px;'>");


                            messageBody.Append("<br/>Thank you,<br/>");
                            messageBody.Append(AppLogic.AppConfigs("StoreName").ToString() + "<br/>");

                            messageBody.Append("<a  href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "'>" + AppLogic.AppConfigs("LIVE_SERVER_NAME").ToString() + "</a>");
                            messageBody.Append("</td></tr></table></td>");
                            messageBody.Append("</tr>");



                            messageBody.Append("<tr>");
                            messageBody.Append("<td class='popup_fotter' style='padding-left:20px;'>");
                            messageBody.Append("" + AppLogic.AppConfigs("FOOTERRIGHT" + ""));
                            messageBody.Append("</td></tr></table></td>");
                            messageBody.Append("</tr>");

                            messageBody.Append("</table>");

                            messageBody.Append("</body></html>");

                            AlternateView av = AlternateView.CreateAlternateViewFromString(messageBody.ToString(), null, "text/html");

                            string ToName = ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["BillingFirstName"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["BillingLastName"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["BillingLastName"].ToString());
                            string ToMail = ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["BillingEmail"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["BillingEmail"].ToString());

                        }
                        #endregion

                        #endregion

                        lblshiperror.Text = "Mail Sent Successfully..";

                    }
                }

            }
            catch (Exception ex) { lblshiperror.Text = ex.Message; }
        }

        /// <summary>
        /// Gets the Product Detail for mail
        /// </summary>
        /// <param name="ShoppingCartID">string ShoppingCartID</param>
        /// <param name="OrderNumber">string OrderNumber</param>
        /// <param name="ProductIds">string ProductIds.</param>
        /// <param name="trackno">string trackno</param>
        /// <param name="packageid">string packageid</param>
        /// <returns>Returns Result as a StringBuilder</returns>
        private StringBuilder getProductDetailForMail(string ShoppingCartID, string OrderNumber, string ProductIds, string trackno, string packageid)
        {
            StringBuilder Table = new StringBuilder();
            try
            {

                DataSet DsCItems = null;
                if (ShoppingCartID != "0")
                {
                    if (ProductIds.ToString() == "-1")
                    {
                        string sql = "select p.ProductID,p.Name,p.SKU,p.SeName,p.Color,p.Size,c.Price,c.Quantity,c.VariantNames,c.VariantValues from dbo.tb_OrderedShoppingCartItems c inner join tb_Product p on c.RefProductID=p.productid where  c.OrderedShoppingCartId=" + ShoppingCartID + " and TrackingNumber='" + trackno.ToString() + "' and PackId='" + packageid.ToString() + "' ";

                        DsCItems = CommonComponent.GetCommonDataSet(sql);
                    }
                    else
                    {
                        string sql = "select p.ProductID,p.Name,p.SKU,p.Color,p.Size,p.SeName,c.Price,c.Quantity,c.VariantNames,c.VariantValues from dbo.tb_OrderedShoppingCartItems c inner join tb_Product p on c.RefProductID=p.productid where  c.OrderedShoppingCartId in (SELECT OrderedShoppingCartId FROM tb_OrderedShoppingCartItems WHERE OrderedCustomCartID=" + ShoppingCartID + ") AND c.RefProductID in(" + ProductIds + ")";


                        DsCItems = CommonComponent.GetCommonDataSet(sql);
                    }
                }
                #region Remainig  (Check First and impleament)
                /*
                 else
                {
                  clsOrderedShoppingCartItems oGetOrderedShoppingCartItemsByOrderId = new clsOrderedShoppingCartItems();
                    if (ProductIds.ToString() == "-1")
                    {
                        DsCItems = oGetOrderedShoppingCartItemsByOrderId.GetOrderedShoppingCartItemsByOrderId(Convert.ToInt32(OrderNumber));
                    }
                    else
                    {
                        DsCItems = oGetOrderedShoppingCartItemsByOrderId.GetOrderedShoppingCartItemsByOrderIdAndproductId(Convert.ToInt32(OrderNumber), ProductIds.Trim(','));
                    }
                }
                 */
                #endregion

                if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
                {
                    Table.Append(" <table  cellpadding='0' cellspacing='0' width='100%' style='margin-top: 5px;padding-right: 20px'> ");

                    Table.Append(" <tr><td>");

                    Table.Append(" <table  cellpadding='3' cellspacing='0' class='datatable' width='100%' style='border:1px solid #cccccc;'> ");
                    Table.Append("<tbody><tr style='BACKGROUND-COLOR: rgb(242,242,242); ' >");
                    Table.Append("<th align='left' valign='middle' style='width:60%' ><b>Product</b></th>");
                    Table.Append("<th align='left' valign='middle' style='width:20%' ><b> SKU</b></th>");
                    Table.Append("<th valign='middle' style='width: 20%;text-align:center;'><b>Quantity</b></th>");
                    //Table.Append("<th align='center' valign='middle' style='width: 20%;'><b>Shipped</b></th>");
                    //Table.Append("<th style='text-align: right;'><b>Sub Total:</b></th>");
                    decimal QtyDiscount = 0;
                    Table.Append("</tr>");
                    decimal TPrice = 0;
                    decimal QtyDiscountPercent = 0;

                    for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                    {
                        decimal Price1 = 0;
                        Decimal PTemp = Decimal.Zero;
                        string price = (ShoppingCartID != "0") ? DsCItems.Tables[0].Rows[i]["Price"].ToString() : DsCItems.Tables[0].Rows[i]["SalePrice"].ToString();
                        Decimal.TryParse(price, out PTemp);
                        PTemp = Math.Round(PTemp, 2);
                        Price1 = PTemp * Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["Quantity"].ToString());

                        QtyDiscountPercent = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SElect ISNULL(qt.DiscountPercent,0) as DiscountPercent from tb_QuantityDiscountTable as qt " +
                                           " inner join tb_QauntityDiscount ON qt.QuantityDiscountID = dbo.tb_QauntityDiscount.QuantityDiscountID  " +
                                           " Where qt.LowQuantity<=" + Convert.ToInt32(DsCItems.Tables[0].Rows[i]["Quantity"].ToString()) + " and qt.HighQuantity>=" + Convert.ToInt32(DsCItems.Tables[0].Rows[i]["Quantity"].ToString()) + " and tb_QauntityDiscount.QuantityDiscountID in (Select QuantityDiscountID from  " +
                                           " tb_Product Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + Convert.ToInt32(DsCItems.Tables[0].Rows[i]["ProductID"].ToString()) + ") "));

                        QtyDiscount += (Price1 * QtyDiscountPercent) / 100;
                        QtyDiscount = Math.Round(QtyDiscount, 2);
                        TPrice += Price1;
                        Table.Append("<tr align='center'  valign='middle'>");
                        Table.Append("<tr >");
                        Table.Append("<td align='left' valign='top'>" + DsCItems.Tables[0].Rows[i]["Name"].ToString());

                        string[] Names = DsCItems.Tables[0].Rows[i]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] Values = DsCItems.Tables[0].Rows[i]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        int iLoopValues = 0;
                        for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                        {
                            Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
                        }
                        if (iLoopValues == 0)
                        {
                            if (DsCItems.Tables[0].Rows[i]["Color"].ToString().Trim().Length > 0)
                                Table.Append("<br/>Color: " + DsCItems.Tables[0].Rows[i]["Color"].ToString());
                            if (DsCItems.Tables[0].Rows[i]["Size"].ToString().Trim().Length > 0)
                                Table.Append("<br/>Size: " + DsCItems.Tables[0].Rows[i]["Size"].ToString());
                        }
                        Table.Append("</td>");
                        Table.Append("<td  align='left' >" + DsCItems.Tables[0].Rows[i]["SKU"].ToString() + "</td>");
                        Table.Append("<td STYLE='text-align : center;'>" + DsCItems.Tables[0].Rows[i]["Quantity"].ToString() + "</td>");
                        Table.Append(" </tr>");
                    }
                    Table.Append("</tbody></table>");
                    Table.Append("</td> </tr></table>");
                }
            }
            catch { Table = null; }
            return Table;
        }

        /// <summary>
        /// Gets the Product Ids from Package Ids
        /// </summary>
        /// <param name="PackageIDs">string PackageIDs</param>
        /// <param name="type">string type.</param>
        /// <returns>Returns ProductIDs as a String</returns>
        private string GetProductIDsFromPackageIDs(string PackageIDs, string type)
        {
            string ProductIDs = string.Empty;
            try
            {
                PackageIDs = "," + PackageIDs + ",";
                foreach (GridViewRow gvr in grdShipping.Rows)
                {
                    if (PackageIDs.Contains("," + (gvr.FindControl("lblPackageId") as System.Web.UI.WebControls.Label).Text + ","))
                    {
                        ProductIDs += (gvr.FindControl("lblProductID") as System.Web.UI.WebControls.Label).Text + ",";
                    }
                }
            }
            catch (Exception ex) { lblshiperror.Text = ex.Message; }

            if (string.IsNullOrEmpty(ProductIDs.TrimEnd(',')))
            {
                ProductIDs = "-1,";
                return ProductIDs.TrimEnd(',');
            }
            else
            {
                return ProductIDs.TrimEnd(',');
            }
        }

        /// <summary>
        /// Download Files from  Specified File Path
        /// </summary>
        /// <param name="filepath">string filepath</param>
        private void downloadfile(string filepath)
        {
            try
            {

                FileInfo file = new FileInfo(filepath);
                if (file.Exists)
                {
                    Response.ClearContent();
                    Response.ContentType = ReturnExtension(file.Extension.ToLower());
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);

                    FileStream sourceFile = new FileStream(file.FullName, FileMode.Open);
                    long FileSize;
                    FileSize = sourceFile.Length;
                    byte[] getContent = new byte[(int)FileSize];
                    sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    sourceFile.Close();
                    Response.BinaryWrite(getContent);

                }
            }
            catch { }
            Response.End();
        }

        /// <summary>
        /// Deletes the label
        /// </summary>
        /// <param name="img">string img</param>
        /// <param name="type">string type</param>
        private void DeleteLabel(string img, string type)
        {
            string err = string.Empty;
            string suc = string.Empty;
            string imgName = string.Empty;
            string imgNamepath = string.Empty;
            try
            {
                if (type == "USPS")
                    imgNamepath = AppLogic.AppConfigs("USPS.LabelSavePath");
                else if (type == "UPS")
                    imgNamepath = AppLogic.AppConfigs("UPS.LabelSavePath");
                else if (type == "FEDEX")
                    imgNamepath = AppLogic.AppConfigs("FEDEX.LabelSavePath");


                if (!string.IsNullOrEmpty(img))
                {
                    imgName = imgNamepath + img;
                    if (File.Exists(Server.MapPath(imgName)))
                    {
                        File.Delete(Server.MapPath(imgName));
                        suc += img;
                        DeleteShippingLabel(imgName, "", "", type, "");
                    }
                    else
                        err += img;
                }
                else
                {
                    DataTable dt = getMailData(type);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        imgName = imgNamepath + dt.Rows[i]["ImgUrl"].ToString();
                        if (File.Exists(Server.MapPath(imgName)))
                        {
                            File.Delete(Server.MapPath(imgName));
                            suc += dt.Rows[i]["ImgUrl"].ToString();
                            DeleteShippingLabel("", dt.Rows[i]["TrackingNo"].ToString(), dt.Rows[i]["SCartID"].ToString(), type, dt.Rows[i]["PackageID"].ToString());
                        }
                        else
                            err += dt.Rows[i]["ImgUrl"].ToString();
                    }
                }

                string OrderNumber = string.Empty;
                if (Request.QueryString["ONo"] != null && Request.QueryString["ONo"].ToString().Trim().Length > 0)
                {

                    OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());

                    if (type == "USPS")
                    {
                        GetExistingFilesUSPS(OrderNumber);
                        GetPackId();

                    }
                    else if (type == "UPS")
                    {
                        GetExistingFilesUPS(OrderNumber);
                        GetPackId();
                    }
                    else if (type == "FEDEX")
                    {
                        GetExistingFilesFEDEX(OrderNumber);
                        GetPackId();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                lblmsg.Text = "";
                if (!string.IsNullOrEmpty(suc))
                {
                    lblmsg.Text += suc + " File Deleted Successfully.<br>";

                    string OrderNumber = string.Empty;
                    if (Request.QueryString["ONo"] != null)
                    {
                        OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                    }
                    BindData(Convert.ToInt32(OrderNumber));
                }
                if (!string.IsNullOrEmpty(err))
                    lblmsg.Text += err + " File Can not Delete, please contact Administrator.";
            }
        }

        /// <summary>
        /// Deletes the Shipping Label
        /// </summary>
        /// <param name="FileName">string FileName.</param>
        /// <param name="TrackingNumber">string TrackingNumber</param>
        /// <param name="ShoppingCartID">string ShoppingCartID</param>
        /// <param name="ShippedVia">string ShippedVia</param>
        /// <param name="Packids">String Packids</param>
        private void DeleteShippingLabel(string FileName, string TrackingNumber, string ShoppingCartID, string ShippedVia, string Packids)
        {
            string Packid = "0";
            if (FileName != "")
            {

                // string TrackingNo = string.Empty;
                string[] TrackingNoArr = FileName.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (TrackingNoArr.Length > 1)
                    TrackingNumber = TrackingNoArr[1];

                Packid = TrackingNoArr[0].Split('-')[1].Replace("Package", "");

                ShoppingCartID = string.Empty;
                try
                {
                    ShoppingCartID = FileName.Substring(FileName.LastIndexOf("_") + 1, FileName.LastIndexOf("@") - FileName.LastIndexOf("_") - 1);
                }
                catch
                {
                    if (TrackingNoArr.Length > 2)
                        ShoppingCartID = TrackingNoArr[2];
                }
            }
            else
                Packid = Packids;

            //DataSet dsCartInvent = CommonComponent.GetCommonDataSet("SELECT Quantity,WareHouseID,RefProductID FROM tb_OrderedShoppingCartItems WHERE TrackingNumber='" + TrackingNumber + "' AND OrderedShoppingCartID='" + ShoppingCartID + "' AND PackId='" + Packid + "' and  ISNULL(InventoryUpdated,0)=1");
            ////DataSet dsCartInvent = CommonComponent.GetCommonDataSet("SELECT Quantity,WareHouseID,RefProductID FROM tb_OrderedShoppingCartItems WHERE TrackingNumber='" + TrackingNumber + "' AND OrderedCustomCartID='" + ShoppingCartID + "' AND PackId='" + Packid + "' and  ISNULL(InventoryUpdated,0)=1");

            //if (dsCartInvent != null && dsCartInvent.Tables.Count > 0 && dsCartInvent.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i <= dsCartInvent.Tables[0].Rows.Count - 1; i++)
            //    {

            //        int Qty = 0;
            //        int WareHouseID = 0;
            //        int RefProductID = 0;

            //        if (dsCartInvent.Tables[0].Rows[i]["Quantity"] != null && dsCartInvent.Tables[0].Rows[i]["Quantity"] != DBNull.Value)
            //        {
            //            Qty = Convert.ToInt32(dsCartInvent.Tables[0].Rows[i]["Quantity"]);
            //        }

            //        if (dsCartInvent.Tables[0].Rows[0]["WareHouseID"] != null && dsCartInvent.Tables[0].Rows[i]["WareHouseID"] != DBNull.Value)
            //        {
            //            WareHouseID = Convert.ToInt32(dsCartInvent.Tables[0].Rows[0]["WareHouseID"]);
            //        }

            //        if (dsCartInvent.Tables[0].Rows[i]["RefProductID"] != null && dsCartInvent.Tables[0].Rows[i]["RefProductID"] != DBNull.Value)
            //        {
            //            RefProductID = Convert.ToInt32(dsCartInvent.Tables[0].Rows[i]["RefProductID"]);
            //        }

            //        if (Qty != 0 && WareHouseID != 0 && RefProductID != 0)
            //        {


            //            CommonComponent.ExecuteCommonData("UPDATE dbo.tb_WareHouseProductInventory SET Inventory=Inventory +" + Qty + "  WHERE WareHouseID= " + WareHouseID + " AND ProductID=" + RefProductID + "");
            //            CommonComponent.ExecuteCommonData("UPDATE dbo.tb_Product SET Inventory=Inventory+" + Qty + " WHERE ProductID=" + RefProductID + "");
            //        }

            //    }
            //}
            CommonComponent.ExecuteCommonData("UPDATE dbo.tb_OrderedShoppingCartItems SET TrackingNumber=NULL,PackId=NULL,ShippedVia=NULL ,ShippedOn=NULL,InventoryUpdated=0,LastTrackingNumber='" + TrackingNumber + "',LastShippedVia='" + ShippedVia + "' WHERE TrackingNumber='" + TrackingNumber + "' AND OrderedShoppingCartID='" + ShoppingCartID + "'  AND PackId='" + Packid + "'");
            // CommonComponent.ExecuteCommonData("UPDATE dbo.tb_OrderedShoppingCartItems SET TrackingNumber=NULL,PackId=NULL,ShippedVia=NULL ,ShippedOn=NULL,InventoryUpdated=0,LastTrackingNumber='" + TrackingNumber + "',LastShippedVia='" + ShippedVia + "' WHERE TrackingNumber='" + TrackingNumber + "' AND OrderedCustomCartID='" + ShoppingCartID + "'  AND PackId='" + Packid + "'");


            //UPDATE dbo.tb_WareHouseProductInventory SET Inventory='' WHERE WareHouseID= 1 AND ProductID=1


        }

        /// <summary>
        /// Gets the Existing Files USPS
        /// </summary>
        /// <param name="OrderNumber">string OrderNumber</param>
        private void GetExistingFilesUSPS(string OrderNumber)
        {
            try
            {
                string ImgSavePath = AppLogic.AppConfigs("USPS.LabelSavePath");
                ImgSavePath = Server.MapPath(ImgSavePath);
                DirectoryInfo dInfo = new DirectoryInfo(ImgSavePath);
                FileInfo[] FilesList = dInfo.GetFiles();
                #region Create Table
                DataTable myTable = new DataTable();
                myTable.Columns.Add("SerialNo", typeof(Int32));
                myTable.Columns.Add("PackageID", typeof(Int32));
                myTable.Columns.Add("ImgUrl", typeof(String));
                myTable.Columns.Add("TrackingNo", typeof(String));
                myTable.Columns.Add("CreateDate", typeof(DateTime));
                myTable.Columns.Add("ShippingCartID", typeof(String));
                #endregion
                int i = 0;
                foreach (FileInfo fi in FilesList)
                {
                    if (fi.FullName.Contains("_" + OrderNumber + "_"))
                    {
                        string TrackingNo = string.Empty;
                        string[] TrackingNoArr = fi.FullName.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (TrackingNoArr.Length > 1)
                            TrackingNo = TrackingNoArr[1];

                        string ShoppingCartID = string.Empty;
                        try
                        {
                            ShoppingCartID = fi.FullName.Substring(fi.FullName.LastIndexOf("_") + 1, fi.FullName.LastIndexOf("@") - fi.FullName.LastIndexOf("_") - 1);
                        }
                        catch
                        {
                            if (TrackingNoArr.Length > 2)
                                ShoppingCartID = TrackingNoArr[2];
                        }

                        DataRow dataRow = myTable.NewRow();
                        dataRow["SerialNo"] = i++ + 1;
                        dataRow["ImgUrl"] = fi.Name;
                        dataRow["PackageID"] = fi.Name.Split('_')[0].Replace("USPS-Package", "");
                        dataRow["TrackingNo"] = TrackingNo;
                        dataRow["CreateDate"] = fi.LastWriteTime;
                        dataRow["ShippingCartID"] = ShoppingCartID;
                        //    UpdatCartItem(TrackingNo, Convert.ToInt32(dataRow["PackageID"].ToString()), "USPS", Convert.ToInt32(OrderedShoppingCartID));
                        myTable.Rows.Add(dataRow);
                    }

                }
                if (myTable != null && myTable.Rows.Count > 0)
                {
                    DataView dv = myTable.DefaultView;
                    dv.Sort = "CreateDate desc";
                    ltUSPSShippingLabel.Text = "Existing Labels for this Order.";
                    grdUSPS.DataSource = dv;
                    grdUSPS.DataBind();
                    grdUSPS.Visible = true;
                    trUSPSAll.Visible = true;
                    panUSPS.Visible = true;
                    grdUSPS.Columns[0].Visible = true;
                    grdUSPS.Columns[1].Visible = true;
                    grdUSPS.Columns[2].Visible = true;
                    grdUSPS.Columns[3].Visible = true;
                    grdUSPS.Columns[4].Visible = true;
                }
                else
                {
                    grdUSPS.DataSource = null;
                    grdUSPS.DataBind();
                    panUSPS.Visible = false;

                }
            }
            catch { }
        }

        /// <summary>
        /// Gets the existing files USPS
        /// </summary>
        /// <param name="OrderNumber">string OrderNumber</param>
        private void GetExistingFilesUPS(string OrderNumber)
        {
            try
            {
                string ImgSavePath = AppLogic.AppConfigs("UPS.LabelSavePath");
                ImgSavePath = Server.MapPath(ImgSavePath);
                DirectoryInfo dInfo = new DirectoryInfo(ImgSavePath);
                FileInfo[] FilesList = dInfo.GetFiles();
                #region Create Table
                DataTable myTable = new DataTable();
                myTable.Columns.Add("SerialNo", typeof(Int32));
                myTable.Columns.Add("PackageID", typeof(Int32));
                myTable.Columns.Add("ImgUrl", typeof(String));
                myTable.Columns.Add("TrackingNo", typeof(String));
                myTable.Columns.Add("CreateDate", typeof(DateTime));
                myTable.Columns.Add("ShippingCartID", typeof(String));
                #endregion
                int i = 0;
                foreach (FileInfo fi in FilesList)
                {
                    if (fi.FullName.Contains("_" + OrderNumber + "_"))
                    {
                        string TrackingNo = string.Empty;
                        string[] TrackingNoArr = fi.FullName.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (TrackingNoArr.Length > 1)
                            TrackingNo = TrackingNoArr[1];

                        string ShoppingCartID = string.Empty;
                        try
                        {
                            ShoppingCartID = fi.FullName.Substring(fi.FullName.LastIndexOf("_") + 1, fi.FullName.LastIndexOf("@") - fi.FullName.LastIndexOf("_") - 1);
                        }
                        catch
                        {
                            if (TrackingNoArr.Length > 2)
                                ShoppingCartID = TrackingNoArr[3];
                        }

                        DataRow dataRow = myTable.NewRow();
                        dataRow["SerialNo"] = i++ + 1;
                        dataRow["ImgUrl"] = fi.Name;
                        dataRow["PackageID"] = fi.Name.Split('_')[0].Replace("UPS-Package", "");
                        dataRow["TrackingNo"] = TrackingNo;
                        dataRow["CreateDate"] = fi.LastWriteTime;
                        dataRow["ShippingCartID"] = ShoppingCartID;
                        //    UpdatCartItem(TrackingNo, Convert.ToInt32(dataRow["PackageID"].ToString()), "USPS", Convert.ToInt32(OrderedShoppingCartID));
                        myTable.Rows.Add(dataRow);
                    }

                }
                if (myTable != null && myTable.Rows.Count > 0)
                {
                    DataView dv = myTable.DefaultView;
                    dv.Sort = "CreateDate desc";
                    ltUSPSShippingLabel.Text = "Existing Labels for this Order.";
                    grdUPS.DataSource = dv;
                    grdUPS.DataBind();
                    grdUPS.Visible = true;
                    panUPS.Visible = true;
                    //   panUSPS.Visible = true;
                    grdUPS.Columns[0].Visible = true;
                    grdUPS.Columns[1].Visible = true;
                    grdUPS.Columns[2].Visible = true;
                    grdUPS.Columns[3].Visible = true;
                    grdUPS.Columns[4].Visible = true;

                }
                else
                {
                    panUPS.Visible = false;
                    grdUPS.DataSource = null;
                    grdUPS.DataBind();
                    grdUPS.Visible = false;

                }
            }
            catch { }
        }

        /// <summary>
        /// Gets the existing files FEDEX
        /// </summary>
        /// <param name="OrderNumber">string OrderNumber</param>
        private void GetExistingFilesFEDEX(string OrderNumber)
        {
            try
            {
                string ImgSavePath = AppLogic.AppConfigs("FEDEX.LabelSavePath");
                ImgSavePath = Server.MapPath(ImgSavePath);
                DirectoryInfo dInfo = new DirectoryInfo(ImgSavePath);
                FileInfo[] FilesList = dInfo.GetFiles();

                #region Create Table

                DataTable myTable = new DataTable();
                myTable.Columns.Add("SerialNo", typeof(Int32));
                myTable.Columns.Add("PackageID", typeof(Int32));
                myTable.Columns.Add("ImgUrl", typeof(String));
                myTable.Columns.Add("TrackingNo", typeof(String));
                myTable.Columns.Add("CreateDate", typeof(DateTime));
                myTable.Columns.Add("ShippingCartID", typeof(String));

                #endregion

                int i = 0;
                foreach (FileInfo fi in FilesList)
                {
                    if (fi.FullName.Contains("_" + OrderNumber + "_"))
                    {
                        string TrackingNo = string.Empty;
                        string[] TrackingNoArr = fi.FullName.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (TrackingNoArr.Length > 1)
                            TrackingNo = TrackingNoArr[1];

                        string ShoppingCartID = string.Empty;
                        try
                        {
                            ShoppingCartID = fi.FullName.Substring(fi.FullName.LastIndexOf("_") + 1, fi.FullName.LastIndexOf("@") - fi.FullName.LastIndexOf("_") - 1);
                        }
                        catch
                        {
                            if (TrackingNoArr.Length > 2)
                                ShoppingCartID = TrackingNoArr[3];
                        }

                        DataRow dataRow = myTable.NewRow();
                        dataRow["SerialNo"] = i++ + 1;
                        dataRow["ImgUrl"] = fi.Name;
                        dataRow["PackageID"] = fi.Name.Split('_')[0].Replace("FedEx-Package", "");
                        dataRow["TrackingNo"] = TrackingNo;
                        dataRow["CreateDate"] = fi.LastWriteTime;
                        dataRow["ShippingCartID"] = ShoppingCartID;
                        //UpdatCartItem(TrackingNo, Convert.ToInt32(dataRow["PackageID"].ToString()), "FEDEX", Convert.ToInt32(OrderedShoppingCartID));
                        myTable.Rows.Add(dataRow);
                    }

                }
                if (myTable != null && myTable.Rows.Count > 0)
                {
                    DataView dv = myTable.DefaultView;
                    dv.Sort = "CreateDate desc";
                    ltUSPSShippingLabel.Text = "Existing Labels for this Order.";
                    grdFEDEX.DataSource = dv;
                    grdFEDEX.DataBind();
                    grdFEDEX.Visible = true;
                    panFEDEX.Visible = true;
                    grdFEDEX.Columns[0].Visible = true;
                    grdFEDEX.Columns[1].Visible = true;
                    grdFEDEX.Columns[2].Visible = true;
                    grdFEDEX.Columns[3].Visible = true;
                    grdFEDEX.Columns[4].Visible = true;
                }
                else
                {
                    panFEDEX.Visible = false;
                    grdFEDEX.DataSource = null;
                    grdFEDEX.DataBind();
                    grdFEDEX.Visible = false;

                }
            }
            catch { }
        }

        /// <summary>
        /// Gets the Mail Data
        /// </summary>
        /// <param name="type">string type</param>
        /// <returns>Returns the Mail Data as a DataTable</returns>
        private DataTable getMailData(string type)
        {
            DataTable myTableTemp = new DataTable();
            try
            {
                #region Create Table
                myTableTemp.Columns.Add("TrackingNo", typeof(String));
                myTableTemp.Columns.Add("PackageID", typeof(String));
                myTableTemp.Columns.Add("SCartID", typeof(String));
                myTableTemp.Columns.Add("ImgUrl", typeof(String));
                #endregion

                if (type == "USPS")
                {
                    foreach (GridViewRow gvr in grdUSPS.Rows)
                    {
                        System.Web.UI.WebControls.LinkButton btnSendMail = (System.Web.UI.WebControls.LinkButton)gvr.FindControl("btnSendMail");
                        System.Web.UI.WebControls.Label lblimgurl = (System.Web.UI.WebControls.Label)gvr.FindControl("lbluspsimgurl");
                        if (btnSendMail != null)
                        {
                            string TrkNo = btnSendMail.CommandArgument.ToString();
                            DataRow dataRow = myTableTemp.NewRow();
                            dataRow["TrackingNo"] = (TrkNo.Split('-').Length > 0) ? TrkNo.Split('-')[0] : ""; ;
                            dataRow["PackageID"] = (gvr.FindControl("lblPackageID") as System.Web.UI.WebControls.Label).Text;
                            dataRow["SCartID"] = (TrkNo.Split('-').Length > 1) ? TrkNo.Split('-')[1] : ""; ;
                            dataRow["ImgUrl"] = lblimgurl.Text;
                            myTableTemp.Rows.Add(dataRow);
                        }
                    }
                }

                if (type == "UPS")
                {
                    foreach (GridViewRow gvr in grdUPS.Rows)
                    {
                        System.Web.UI.WebControls.LinkButton btnSendMail = (System.Web.UI.WebControls.LinkButton)gvr.FindControl("btnSendMail");
                        System.Web.UI.WebControls.Label lblimgurl = (System.Web.UI.WebControls.Label)gvr.FindControl("lbluspsimgurl");
                        if (btnSendMail != null)
                        {
                            string TrkNo = btnSendMail.CommandArgument.ToString();
                            DataRow dataRow = myTableTemp.NewRow();
                            dataRow["TrackingNo"] = (TrkNo.Split('-').Length > 0) ? TrkNo.Split('-')[0] : ""; ;
                            dataRow["PackageID"] = (gvr.FindControl("lblPackageID") as System.Web.UI.WebControls.Label).Text;
                            dataRow["SCartID"] = (TrkNo.Split('-').Length > 1) ? TrkNo.Split('-')[1] : ""; ;
                            dataRow["ImgUrl"] = lblimgurl.Text;
                            myTableTemp.Rows.Add(dataRow);
                        }
                    }
                }

                if (type == "FEDEX")
                {
                    foreach (GridViewRow gvr in grdFEDEX.Rows)
                    {
                        System.Web.UI.WebControls.LinkButton btnSendMail = (System.Web.UI.WebControls.LinkButton)gvr.FindControl("btnSendMail");
                        System.Web.UI.WebControls.Label lblimgurl = (System.Web.UI.WebControls.Label)gvr.FindControl("lbluspsimgurl");
                        if (btnSendMail != null)
                        {
                            string TrkNo = btnSendMail.CommandArgument.ToString();
                            DataRow dataRow = myTableTemp.NewRow();
                            dataRow["TrackingNo"] = (TrkNo.Split('-').Length > 0) ? TrkNo.Split('-')[0] : ""; ;
                            dataRow["PackageID"] = (gvr.FindControl("lblPackageID") as System.Web.UI.WebControls.Label).Text;
                            dataRow["SCartID"] = (TrkNo.Split('-').Length > 1) ? TrkNo.Split('-')[1] : ""; ;
                            dataRow["ImgUrl"] = lblimgurl.Text;
                            myTableTemp.Rows.Add(dataRow);
                        }
                    }
                }

            }
            catch { myTableTemp = null; }
            return myTableTemp;
        }

        /// <summary>
        /// Returns the Extension
        /// </summary>
        /// <param name="fileExtension">string fileExtension</param>
        /// <returns>Returns the File Extension</returns>
        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }

        /// <summary>
        /// Mark Products Shipped
        /// </summary>
        public bool MarkProductsShipped(int OrderNumber, string TrackingNumber, string ShippedVIA, int packidd)
        {
            string OrderStatus = string.Empty;
            string PackageID = string.Empty;
            string ProductID = string.Empty;
            string TrkNo = string.Empty;
            string Query = string.Empty;

            string ShoppingcartID = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ShoppingCardID FROM dbo.tb_Order WHERE OrderNumber=" + OrderNumber));


            if (ShippedVIA.ToLower() == "usps")
            {
                if (grdUSPS.Rows.Count == 1)
                {
                    OrderStatus = "Shipped";
                    Query = "UPDATE tb_orderedShoppingcartitems SET  ShippedOn=GETDATE() WHERE OrderedShoppingCartID=" + ShoppingcartID + " AND TrackingNumber='" + TrackingNumber + "' AND PackId=" + packidd;
                    ShippingComponent.MarkProductsShipped(Query);
                    ShippingComponent.MarkOrderAsShippedforShippingLabel(OrderNumber, ShippedVIA, TrackingNumber, DateTime.Now, OrderStatus);
                    CommonComponent.ExecuteCommonData("UPDATE tb_OrderShippedItems set ShippedOn=GETDATE(), Shipped=1 where OrderNumber='" + OrderNumber + "' and TrackingNumber='" + TrackingNumber + "'");

                    Query = " SELECT COUNT(OrderedCustomCartID) FROM tb_orderedShoppingcartitems WHERE  ShippedOn Is null and OrderedShoppingCartID='" + ShoppingcartID + "'";

                    Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData(Query));
                    if (Count > 0)
                    {
                        OrderStatus = "Partially Shipped";
                        ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                    }
                    else if (Count == 0)
                    {
                        OrderStatus = "Shipped";
                        ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                    }

                }
                else if (grdUSPS.Rows.Count > 1)
                {
                    OrderStatus = "Partially Shipped";
                    foreach (GridViewRow gvr in grdUSPS.Rows)
                    {
                        System.Web.UI.WebControls.Label lblTrackingNo = (System.Web.UI.WebControls.Label)gvr.FindControl("lblTrackingNo");

                        Query = "UPDATE tb_orderedShoppingcartitems SET  ShippedOn=GETDATE() WHERE OrderedShoppingCartID=" + ShoppingcartID + "  AND TrackingNumber='" + TrackingNumber + "' AND PackId=" + packidd;
                        ShippingComponent.MarkProductsShipped(Query);
                        CommonComponent.ExecuteCommonData("UPDATE tb_OrderShippedItems set ShippedOn=GETDATE(), Shipped=1 where OrderNumber='" + OrderNumber + "' and TrackingNumber='" + TrackingNumber + "'");

                        Query = " SELECT COUNT(OrderedCustomCartID) FROM tb_orderedShoppingcartitems WHERE  ShippedOn Is null and OrderedShoppingCartID='" + ShoppingcartID + "'";

                        Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData(Query));

                        if (Count > 0)
                        {
                            OrderStatus = "Partially Shipped";
                            ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                        }
                        else if (Count == 0)
                        {
                            OrderStatus = "Shipped";
                            ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                        }



                        //USPSTable = objRate.EndiciaGetRatesAdminShippingLabel(ViewState["Zip"].ToString(), CountryCode.ToString(), Convert.ToDouble(ProWeight), ref strUSPSMessage);
                    }
                }

            }
            else if (ShippedVIA.ToLower() == "ups")
            {

                if (grdUPS.Rows.Count == 1)
                {
                    OrderStatus = "Shipped";
                    Query = "UPDATE tb_orderedShoppingcartitems SET  ShippedOn=GETDATE() WHERE OrderedShoppingCartID=" + ShoppingcartID + "  AND TrackingNumber='" + TrackingNumber + "' AND PackId=" + packidd;
                    ShippingComponent.MarkProductsShipped(Query);
                    ShippingComponent.MarkOrderAsShippedforShippingLabel(OrderNumber, ShippedVIA, TrackingNumber, DateTime.Now, OrderStatus);
                    CommonComponent.ExecuteCommonData("UPDATE tb_OrderShippedItems set ShippedOn=GETDATE(), Shipped=1 where OrderNumber='" + OrderNumber + "' and TrackingNumber='" + TrackingNumber + "'");
                    Query = " SELECT COUNT(OrderedCustomCartID) FROM tb_orderedShoppingcartitems WHERE  ShippedOn Is null and OrderedShoppingCartID='" + ShoppingcartID + "'";

                    Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData(Query));
                    if (Count > 0)
                    {
                        OrderStatus = "Partially Shipped";
                        ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                    }
                    else if (Count == 0)
                    {
                        OrderStatus = "Shipped";
                        ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                    }

                }
                else if (grdUPS.Rows.Count > 1)
                {
                    OrderStatus = "Partially Shipped";
                    foreach (GridViewRow gvr in grdUPS.Rows)
                    {
                        System.Web.UI.WebControls.Label lblTrackingNo = (System.Web.UI.WebControls.Label)gvr.FindControl("lblTrackingNo");

                        Query = "UPDATE tb_orderedShoppingcartitems SET  ShippedOn=GETDATE() WHERE OrderedShoppingCartID=" + ShoppingcartID + "  AND TrackingNumber='" + TrackingNumber + "' AND PackId=" + packidd;
                        ShippingComponent.MarkProductsShipped(Query);
                        CommonComponent.ExecuteCommonData("UPDATE tb_OrderShippedItems set ShippedOn=GETDATE(), Shipped=1 where OrderNumber='" + OrderNumber + "' and TrackingNumber='" + TrackingNumber + "'");
                        Query = " SELECT COUNT(OrderedCustomCartID) FROM tb_orderedShoppingcartitems WHERE  ShippedOn Is null and OrderedShoppingCartID='" + ShoppingcartID + "'";

                        Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData(Query));

                        if (Count > 0)
                        {
                            OrderStatus = "Partially Shipped";
                            ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                        }
                        else if (Count == 0)
                        {
                            OrderStatus = "Shipped";
                            ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                        }
                        //USPSTable = objRate.EndiciaGetRatesAdminShippingLabel(ViewState["Zip"].ToString(), CountryCode.ToString(), Convert.ToDouble(ProWeight), ref strUSPSMessage);
                    }
                }

            }
            else if (ShippedVIA.ToLower() == "fedex")
            {

                if (grdFEDEX.Rows.Count == 1)
                {
                    OrderStatus = "Shipped";
                    Query = "UPDATE tb_orderedShoppingcartitems SET  ShippedOn=GETDATE() WHERE OrderedShoppingCartID=" + ShoppingcartID + "  AND TrackingNumber='" + TrackingNumber + "' AND PackId=" + packidd;
                    ShippingComponent.MarkProductsShipped(Query);
                    ShippingComponent.MarkOrderAsShippedforShippingLabel(OrderNumber, ShippedVIA, TrackingNumber, DateTime.Now, OrderStatus);
                    CommonComponent.ExecuteCommonData("UPDATE tb_OrderShippedItems set ShippedOn=GETDATE(), Shipped=1 where OrderNumber='" + OrderNumber + "' and TrackingNumber='" + TrackingNumber + "'");
                    Query = " SELECT COUNT(OrderedCustomCartID) FROM tb_orderedShoppingcartitems WHERE  ShippedOn Is null and OrderedShoppingCartID='" + ShoppingcartID + "'";

                    Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData(Query));
                    if (Count > 0)
                    {
                        OrderStatus = "Partially Shipped";
                        ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                    }
                    else if (Count == 0)
                    {
                        OrderStatus = "Shipped";
                        ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                    }

                }
                else if (grdFEDEX.Rows.Count > 1)
                {
                    OrderStatus = "Partially Shipped";
                    foreach (GridViewRow gvr in grdFEDEX.Rows)
                    {
                        System.Web.UI.WebControls.Label lblTrackingNo = (System.Web.UI.WebControls.Label)gvr.FindControl("lblTrackingNo");

                        Query = "UPDATE tb_orderedShoppingcartitems SET  ShippedOn=GETDATE() WHERE OrderedShoppingCartID=" + ShoppingcartID + "  AND TrackingNumber='" + TrackingNumber + "' AND PackId=" + packidd;
                        ShippingComponent.MarkProductsShipped(Query);
                        CommonComponent.ExecuteCommonData("UPDATE tb_OrderShippedItems set ShippedOn=GETDATE(), Shipped=1 where OrderNumber='" + OrderNumber + "' and TrackingNumber='" + TrackingNumber + "'");
                        Query = " SELECT COUNT(OrderedCustomCartID) FROM tb_orderedShoppingcartitems WHERE  ShippedOn Is null and OrderedShoppingCartID='" + ShoppingcartID + "'";

                        Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData(Query));

                        if (Count > 0)
                        {
                            OrderStatus = "Partially Shipped";
                            ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                        }
                        else if (Count == 0)
                        {
                            OrderStatus = "Shipped";
                            ShippingComponent.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
                        }
                        //USPSTable = objRate.EndiciaGetRatesAdminShippingLabel(ViewState["Zip"].ToString(), CountryCode.ToString(), Convert.ToDouble(ProWeight), ref strUSPSMessage);
                    }
                }

            }


            return false;

            #region old Logic
            //start old Logic 

            //string Query = string.Empty;
            //for (int iLoopIds = 0; iLoopIds < ProductIds.Count; iLoopIds++)
            //{
            //    Query += " if exists (select 1 from tb_OrderShippedItems where OrderNumber=" + OrderNumber + " and RefProductID=" + ProductIds[iLoopIds]
            //        + ") begin update tb_OrderShippedItems set TrackingNumber='" + TrackingNumber[iLoopIds] + "',ShippedVia='" + CourierName[iLoopIds]
            //        + "' where OrderNumber=" + OrderNumber + " and RefProductID=" + ProductIds[iLoopIds]
            //        + " end else begin insert into tb_OrderShippedItems values(" + OrderNumber + "," + ProductIds[iLoopIds]
            //        + ",null,'" + TrackingNumber[iLoopIds] + "','" + CourierName[iLoopIds] + "',1,'" + ShippedDateList[iLoopIds] + "',null,null) end";
            //}
            //if (!string.IsNullOrEmpty(Query))
            //{
            //    return Convert.ToBoolean(ShippingComponent.MarkProductsShipped(Query));
            //}
            //return false;

            //end oldlogic
            #endregion
        }

        //public bool MarkOrderAsShippedforShippingLabel(int OrderNumber, String ShippedVIA, String ShippingTrackingNumber, DateTime ShippedOn)
        //{
        //    return Convert.ToBoolean(ShippingComponent.MarkOrderAsShippedforShippingLabel(OrderNumber, ShippedVIA, ShippingTrackingNumber, ShippedOn));
        //}

        /// <summary>
        /// Updates the Cart item
        /// </summary>
        /// <param name="TrackingNumber">string TrackingNumber.</param>
        /// <param name="ProductId">string ProductId</param>
        /// <param name="CourierName">string CourierName</param>
        /// <param name="OrderedShoppingCartID">int OrderedShoppingCartID</param>
        /// <param name="ShippedQty">int ShippedQty</param>
        /// <param name="WareHouseID">int WareHouseID</param>
        /// <param name="Packid">int Packid</param>
        /// <returns>Returns true if updated</returns>
        private bool UpdatCartItem(string TrackingNumber, string ProductId, string CourierName, int OrderedShoppingCartID, int ShippedQty, int WareHouseID, int Packid)
        {
            return Convert.ToBoolean(ShippingComponent.UpdateorderedShoppingcartitems(TrackingNumber, ProductId, CourierName, OrderedShoppingCartID, ShippedQty, WareHouseID, Packid));
        }

        /// <summary>
        /// Ware House Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlWareHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            string OrderNumber = "0";
            GetWarehouseAdddress(Convert.ToInt32(ddlWareHouse.SelectedValue));
            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                BindData(Convert.ToInt32(OrderNumber));
            }
        }


        public Object FedexGetRatesAdmin(int WareHouseID, decimal Weight, string Address, string Address2, string City, string State, string Zip, string Country, int CustomerID, bool isclient)
        {
            decimal ShipmentWeight = 0;
            Fedex.Packages shipment = new Fedex.Packages();

            ShipmentWeight = Convert.ToDecimal(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("FedEx.MaxWeight"));
            shipment.DestinationZipPostalCode = Zip;
            shipment.DestinationCountryCode = Country;

            StateComponent objstate = new StateComponent();
            State = objstate.GetStateCodeByName(State);
            if (shipment.DestinationCountryCode == "US")
            {
                shipment.DestinationStateProvince = State;
            }

            shipment.DestinationResidenceType = ResidenceTypes.Residential;
            ShippingMethods.ResidenceTypes DestinationResidenceType = new ResidenceTypes();
            DestinationResidenceType = shipment.DestinationResidenceType;

            int PackageID = 1;

            Decimal FixedShipWeightRange = Decimal.Zero;
            Decimal FixedShipWeight = Decimal.Zero;

            if (FixedShipWeightRange == decimal.Zero)
                Decimal.TryParse(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("FedEx.FixedShipWeightRange"), out FixedShipWeightRange);

            if (FixedShipWeight == decimal.Zero)
                Decimal.TryParse(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("FedEx.FixedShippingWeight"), out FixedShipWeight);

            DataSet DsCartItems = CommonComponent.GetCommonDataSet("SELECT tb_Product.Name,tb_Product.SEName, " +
            " (Case tb_ShoppingCartItems.weight  when 0 then tb_Product.weight else tb_ShoppingCartItems.weight end) as Weight," +
            " isnull(tb_Product.isFreeShipping,0) as 'IsFreeShipping',tb_Product.SurCharge,tb_Product.SEName,tb_Product.SKU," +
            " tb_Product.Name + ISNull(Convert(nvarchar(max),SUBSTRING(tb_Product.Description,0,180)),'') as Description," +
            " tb_ShoppingCartItems.Price As SalePrice, tb_ShoppingCartItems.Quantity,tb_ShoppingCartItems.ProductID, " +
            " tb_ShoppingCartItems.ShoppingCartID, tb_Product.Name AS ProductName,tb_ShoppingCartItems.VariantNames," +
            " tb_ShoppingCartItems.VariantValues FROM tb_Product INNER JOIN tb_ShoppingCartItems ON " +
            " tb_Product.ProductID = tb_ShoppingCartItems.ProductID Where tb_Product.StoreID=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreID") + "" +
            " And ShoppingCartID In (Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + CustomerID + ")" +
            " And tb_Product.Weight < " + FixedShipWeightRange + " or tb_Product.Weight >" + FixedShipWeight + "");

            String Query = "SELECT sum(tb_ShoppingCartItems.Quantity) as TotalPackCount FROM tb_Product  " +
            " INNER JOIN tb_ShoppingCartItems ON tb_Product.ProductID = tb_ShoppingCartItems.ProductID   " +
            " Where tb_Product.StoreID=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreID") + " And isnull(isFreeShipping,0) <> 1  " +
            " And ShoppingCartID In (Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + CustomerID + ")";

            Object objTotalPackCount = CommonComponent.GetScalarCommonData(Query);
            Int32 TotalPackCount = 0;
            if (objTotalPackCount != null)
                Int32.TryParse(objTotalPackCount.ToString(), out TotalPackCount);

            //Weight
            Decimal remainingItemsWeight = 0.0M;
            Decimal remainingItemsInsuranceValue = 0.0M;
            Decimal TotalWeight = 0;
            if (Weight != null)
                Decimal.TryParse(Weight.ToString(), out TotalWeight);
            int LoopCount = 1;



            FedExRateAdmin oFedEx = new FedExRateAdmin();

            //For Client
            if (WareHouseID > 0)
            {
                string OrgShippingZip = "";
                string OrgCountry = "";
                string OrgAddress1 = "";
                string OrgAddress2 = "";
                string OrgCity = "";
                string OrgState = "";

                string sqlWareHouse = "SELECT Address1,Address2,City,State,ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse "
                    + " INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + WareHouseID;
                DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
                if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
                {
                    OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
                    OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
                    OrgAddress1 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address1"]);
                    OrgAddress2 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address2"]);
                    OrgCity = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["City"]);
                    OrgState = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["State"]);
                }
                CountryComponent objCountry = new CountryComponent();
                OrgCountry = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(OrgCountry));
                StateComponent objState = new StateComponent();
                OrgState = Convert.ToString(objState.GetStateCodeByName(OrgState));

                oFedEx.oCity = OrgCity;
                oFedEx.oCountryCode = OrgCountry;
                oFedEx.oPostalCode = OrgShippingZip;
                oFedEx.oStateOrProvinceCode = OrgState;
                oFedEx.oStreetLines = OrgAddress1 + ", " + OrgAddress2;
            }
            else
            {
                oFedEx.oCity = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginCity");
                oFedEx.oCountryCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginCountry");
                oFedEx.oPostalCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginZip");
                oFedEx.oStateOrProvinceCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginState");
                oFedEx.oStreetLines = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginAddress") + ", " + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginAddress2");
            }

            oFedEx.dCountryCode = Country;
            oFedEx.dPostalCode = Zip;
            if (Country.ToString().ToLower() != "us")
                oFedEx.dStateOrProvinceCode = "";
            else
                oFedEx.dStateOrProvinceCode = State;


            ShippingMethods.FedEx.RateRequest request = oFedEx.Main();


            #region Make Packages

            int ctotalcntr = 0;

            foreach (GridViewRow grvdw in grdShipping.Rows)
            {
                int totalingrdpkg = 0;
                Decimal grdremweight = 0.0M;
                System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)grvdw.FindControl("chkAllowShip");

                System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)grvdw.FindControl("txtProWeight");
                if (!chkAllowShipment.Checked)
                    continue;
                if (Convert.ToDecimal(txtProWeight.Text) > ShipmentWeight)
                {
                    totalingrdpkg = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(Convert.ToDecimal(txtProWeight.Text) / ShipmentWeight)));
                    grdremweight = Convert.ToDecimal(txtProWeight.Text) % ShipmentWeight;
                    if (grdremweight == 0)
                    { ctotalcntr = ctotalcntr + totalingrdpkg; }
                    else
                    {
                        ctotalcntr = ctotalcntr + totalingrdpkg + 1;
                    }
                    // grdremainingItemsWeight = ProWeight % ShipmentWeight;
                }
                else
                {
                    ctotalcntr++;
                }

            }
            if (Convert.ToDecimal(txtWeight.Text) > Convert.ToDecimal(0.00))
            {
                int totalinpkg = 0;
                Decimal remweight = 0.0M;
                if (Convert.ToDecimal(txtWeight.Text) > ShipmentWeight)
                {
                    totalinpkg = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(Convert.ToDecimal(txtWeight.Text) / ShipmentWeight)));
                    remweight = Convert.ToDecimal(txtWeight.Text) % ShipmentWeight;
                    if (remweight == 0)
                    {
                        ctotalcntr = ctotalcntr + totalinpkg;
                    }
                    else
                    {
                        ctotalcntr = ctotalcntr + totalinpkg + 1;
                    }
                }
                else
                {
                    ctotalcntr++;
                }
            }

            request.RequestedShipment.PackageCount = ctotalcntr.ToString();
            request.RequestedShipment.RequestedPackageLineItems = new ShippingMethods.FedEx.RequestedPackageLineItem[ctotalcntr]; PackageID = 1;

            decimal decTotalWeight = 0;
            int cnt1 = 0;
            foreach (GridViewRow gvr in grdShipping.Rows)
            {
                System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");

                if (!chkAllowShipment.Checked)
                    continue;
                Decimal ProWeight = Convert.ToDecimal("1");

                System.Web.UI.WebControls.TextBox txtHeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtHeightgrid");
                System.Web.UI.WebControls.TextBox txtWidth = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtWidthgrid");
                System.Web.UI.WebControls.TextBox txtLength = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtLengthgrid");
                System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
                System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");
               //* System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
                System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
                System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");


                if (lblShipping.Text.ToLower() == "no")
                    decTotalWeight += Convert.ToDecimal(string.IsNullOrEmpty(txtProWeight.Text) ? "0" : txtProWeight.Text);

                if (txtProWeight != null && !string.IsNullOrEmpty(txtProWeight.Text))
                    ProWeight = Convert.ToDecimal(txtProWeight.Text);

                int grdloopcount = 1;
                Int32 grdTotalPackCount = 0;
                Decimal grdremainingItemsWeight = 0.0M;
                int grdpakcnt = 0;

                if (ProWeight > ShipmentWeight)
                {
                    grdloopcount = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(ProWeight / ShipmentWeight)));
                    grdremainingItemsWeight = ProWeight % ShipmentWeight;
                }
                else
                {
                    grdremainingItemsWeight = 0.0M;
                }

                if (grdremainingItemsWeight > 0)
                    grdTotalPackCount = grdloopcount + 1;
                else grdTotalPackCount = grdloopcount;

                if (grdTotalPackCount != 0)
                    grdpakcnt = grdTotalPackCount;
                else
                    grdpakcnt = 1;

                int grdPackageID = 1;


                for (int iNew = 0; iNew < grdloopcount; iNew++)
                {
                    Fedex.Package p = new Fedex.Package();
                    p.PackageId = PackageID;
                    PackageID += 1;


                    if (txtHeight != null && !string.IsNullOrEmpty(txtHeight.Text))
                        p.Height = Convert.ToDecimal(txtHeight.Text);
                    if (txtWidth != null && !string.IsNullOrEmpty(txtWidth.Text))
                        p.Width = Convert.ToDecimal(txtWidth.Text);
                    if (txtLength != null && !string.IsNullOrEmpty(txtLength.Text))
                        p.Length = Convert.ToDecimal(txtLength.Text);
                    if (txtProWeight != null && !string.IsNullOrEmpty(txtProWeight.Text))
                        ProWeight = Convert.ToDecimal(txtProWeight.Text);
                    p.Weight = ProWeight;
                    // p.Insured = chkInsured.Checked; //false;
                    p.Insured = AppLogic.AppConfigBool("FedEx.Insured");
                    p.InsuredValue = remainingItemsInsuranceValue;
                    p.PackageId = grdPackageID;
                    grdPackageID = grdPackageID + 1;
                    if (ProWeight > ShipmentWeight)
                    {
                        p.Weight = ShipmentWeight;
                    }
                    else
                    {
                        p.Weight = ProWeight;
                    }

                    // Set insurance. Get from products db shipping values?
                    p.Insured = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigBool("RTShipping.Insured"); //false;
                    p.InsuredValue = remainingItemsInsuranceValue;



                    #region FedEx Package

                    request.RequestedShipment.RequestedPackageLineItems[cnt1] = new ShippingMethods.FedEx.RequestedPackageLineItem();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].SequenceNumber = Convert.ToString(cnt1 + 1); // package sequence number
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight = new ShippingMethods.FedEx.Weight(); // package weight
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight.Units = ShippingMethods.FedEx.WeightUnits.LB;
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight.Value = p.Weight;
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions = new ShippingMethods.FedEx.Dimensions(); // package dimensions
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Length = p.Length.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Width = p.Width.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Height = p.Height.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Units = ShippingMethods.FedEx.LinearUnits.IN;
                    if (p.Insured)
                    {
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue = new ShippingMethods.FedEx.Money(); // insured value
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue.Amount = p.InsuredValue;
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue.Currency = "USD";
                    }
                    cnt1++;

                    shipment.AddPackage(p);
                    p = null;
                }

                if (grdremainingItemsWeight != 0.0M)
                {
                    Fedex.Package p = new Fedex.Package();
                    p.PackageId = grdPackageID;
                    grdPackageID = grdPackageID + 1;
                    p.Weight = grdremainingItemsWeight;

                    // Set insurance. Get from products db shipping values?
                    p.Insured = AppLogic.AppConfigBool("FedEx.Insured"); //false;
                    p.InsuredValue = remainingItemsInsuranceValue;

                    #region FedEx Package

                    request.RequestedShipment.RequestedPackageLineItems[cnt1] = new ShippingMethods.FedEx.RequestedPackageLineItem();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].SequenceNumber = Convert.ToString(cnt1 + 1); // package sequence number
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight = new ShippingMethods.FedEx.Weight(); // package weight
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight.Units = ShippingMethods.FedEx.WeightUnits.LB;
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight.Value = p.Weight;
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions = new ShippingMethods.FedEx.Dimensions(); // package dimensions
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Length = p.Length.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Width = p.Width.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Height = p.Height.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Units = ShippingMethods.FedEx.LinearUnits.IN;
                    if (p.Insured)
                    {
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue = new ShippingMethods.FedEx.Money(); // insured value
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue.Amount = p.InsuredValue;
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue.Currency = "USD";
                    }
                    cnt1++;
                    shipment.AddPackage(p);
                    p = null;
                    #endregion

                }
                    #endregion



            }

            decTotalWeight = Convert.ToDecimal(hfWeight.Value);
            try
            {
                decimal w = Convert.ToDecimal(txtWeight.Text.Trim());
            }
            catch (Exception)
            {
                txtWeight.Text = decTotalWeight.ToString();

            }

            if (Convert.ToDecimal(txtWeight.Text) > Convert.ToDecimal(0.00))
            {
                int pakcnt = 0;
                int LoopCountNew = 1;
                if (TotalWeight > ShipmentWeight)
                {
                    LoopCountNew = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(TotalWeight / ShipmentWeight)));
                    remainingItemsWeight = TotalWeight % ShipmentWeight;
                }
                else
                {
                    remainingItemsWeight = 0.0M;
                }

                if (remainingItemsWeight > 0)
                    TotalPackCount = LoopCount + 1;
                else TotalPackCount = LoopCount;



                if (TotalPackCount != 0)
                    pakcnt = TotalPackCount;
                else
                    pakcnt = DsCartItems.Tables[0].Rows.Count;

                for (int iNew = 0; iNew < LoopCountNew; iNew++)
                {
                    Fedex.Package p = new Fedex.Package();
                    p.PackageId = PackageID;
                    PackageID = PackageID + 1;
                    if (TotalWeight > ShipmentWeight)
                    {
                        p.Weight = ShipmentWeight;
                    }
                    else
                    {
                        p.Weight = TotalWeight;
                    }
                    if (!string.IsNullOrEmpty(txtHeight.Text))
                        p.Height = Convert.ToDecimal(txtHeight.Text);
                    if (!string.IsNullOrEmpty(txtWidth.Text))
                        p.Width = Convert.ToDecimal(txtWidth.Text);
                    if (!string.IsNullOrEmpty(txtLength.Text))
                        p.Length = Convert.ToDecimal(txtLength.Text);
                    //  p.Weight = Convert.ToDecimal(txtWeight.Text);
                    p.Insured = true;

                    // Set insurance. Get from products db shipping values?
                    // p.Insured = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigBool("RTShipping.Insured"); //false;
                    p.InsuredValue = remainingItemsInsuranceValue;

                    #region FedEx Package

                    request.RequestedShipment.RequestedPackageLineItems[cnt1] = new ShippingMethods.FedEx.RequestedPackageLineItem();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].SequenceNumber = Convert.ToString(cnt1 + 1); // package sequence number
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight = new ShippingMethods.FedEx.Weight(); // package weight
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight.Units = ShippingMethods.FedEx.WeightUnits.LB;
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight.Value = p.Weight;
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions = new ShippingMethods.FedEx.Dimensions(); // package dimensions
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Length = p.Length.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Width = p.Width.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Height = p.Height.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Units = ShippingMethods.FedEx.LinearUnits.IN;
                    if (p.Insured)
                    {
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue = new ShippingMethods.FedEx.Money();// insured value
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue.Amount = p.InsuredValue;
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue.Currency = "USD";
                    }
                    cnt1++;

                    #endregion
                    shipment.AddPackage(p);
                    p = null;
                }

                if (remainingItemsWeight != 0.0M)
                {
                    Fedex.Package p = new Fedex.Package();
                    p.PackageId = PackageID;
                    PackageID = PackageID + 1;
                    p.Weight = remainingItemsWeight;

                    // Set insurance. Get from products db shipping values?
                    p.Insured = AppLogic.AppConfigBool("FedEx.Insured"); //false;
                    p.InsuredValue = remainingItemsInsuranceValue;

                    #region FedEx Package

                    request.RequestedShipment.RequestedPackageLineItems[cnt1] = new ShippingMethods.FedEx.RequestedPackageLineItem();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].SequenceNumber = Convert.ToString(cnt1 + 1); // package sequence number
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight = new ShippingMethods.FedEx.Weight(); // package weight
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight.Units = ShippingMethods.FedEx.WeightUnits.LB;
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Weight.Value = p.Weight;
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions = new ShippingMethods.FedEx.Dimensions(); // package dimensions
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Length = p.Length.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Width = p.Width.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Height = p.Height.ToString();
                    request.RequestedShipment.RequestedPackageLineItems[cnt1].Dimensions.Units = ShippingMethods.FedEx.LinearUnits.IN;
                    if (p.Insured)
                    {
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue = new ShippingMethods.FedEx.Money(); // insured value
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue.Amount = p.InsuredValue;
                        request.RequestedShipment.RequestedPackageLineItems[cnt1].InsuredValue.Currency = "USD";
                    }
                    cnt1++;

                    #endregion
                    shipment.AddPackage(p);
                    // p = null;
                }




            }


            #endregion

            StringBuilder tmpS = new StringBuilder(4096);
            String RTShipRequest = String.Empty;
            String RTShipResponse = String.Empty;
            object returnObject = null;

            ShippingMethods.Fedex objGetRates = new Fedex();
            tmpS.Append((string)objGetRates.GetRates(shipment, out RTShipRequest, out RTShipResponse, request, isclient));
            returnObject = (object)tmpS;

            return returnObject;
        }


        #region old_admin function
        //public Object FedexGetRatesAdmin_old(int WareHouseID, decimal Weight, string Address, string Address2, string City, string State, string Zip, string Country, int CustomerID, bool isclient)
        //{
        //    decimal ShipmentWeight = 0;
        //    Fedex.Packages shipment = new Fedex.Packages();

        //    ShipmentWeight = Convert.ToDecimal(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("FedEx.MaxWeight"));
        //    shipment.DestinationZipPostalCode = Zip;
        //    shipment.DestinationCountryCode = Country;

        //    StateComponent objstate = new StateComponent();
        //    State = objstate.GetStateCodeByName(State);
        //    if (shipment.DestinationCountryCode == "US")
        //    {
        //        shipment.DestinationStateProvince = State;
        //    }

        //    shipment.DestinationResidenceType = ResidenceTypes.Residential;
        //    ShippingMethods.ResidenceTypes DestinationResidenceType = new ResidenceTypes();
        //    DestinationResidenceType = shipment.DestinationResidenceType;

        //    int PackageID = 1;

        //    Decimal FixedShipWeightRange = Decimal.Zero;
        //    Decimal FixedShipWeight = Decimal.Zero;

        //    if (FixedShipWeightRange == decimal.Zero)
        //        Decimal.TryParse(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("FedEx.FixedShipWeightRange"), out FixedShipWeightRange);

        //    if (FixedShipWeight == decimal.Zero)
        //        Decimal.TryParse(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("FedEx.FixedShippingWeight"), out FixedShipWeight);

        //    DataSet DsCartItems = CommonComponent.GetCommonDataSet("SELECT tb_Product.Name,tb_Product.SEName, " +
        //    " (Case tb_ShoppingCartItems.weight  when 0 then tb_Product.weight else tb_ShoppingCartItems.weight end) as Weight," +
        //    " isnull(tb_Product.isFreeShipping,0) as 'IsFreeShipping',tb_Product.SurCharge,tb_Product.SEName,tb_Product.SKU," +
        //    " tb_Product.Name + ISNull(Convert(nvarchar(max),SUBSTRING(tb_Product.Description,0,180)),'') as Description," +
        //    " tb_ShoppingCartItems.Price As SalePrice, tb_ShoppingCartItems.Quantity,tb_ShoppingCartItems.ProductID, " +
        //    " tb_ShoppingCartItems.ShoppingCartID, tb_Product.Name AS ProductName,tb_ShoppingCartItems.VariantNames," +
        //    " tb_ShoppingCartItems.VariantValues FROM tb_Product INNER JOIN tb_ShoppingCartItems ON " +
        //    " tb_Product.ProductID = tb_ShoppingCartItems.ProductID Where tb_Product.StoreID=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreID") + "" +
        //    " And ShoppingCartID In (Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + CustomerID + ")" +
        //    " And tb_Product.Weight < " + FixedShipWeightRange + " or tb_Product.Weight >" + FixedShipWeight + "");

        //    String Query = "SELECT sum(tb_ShoppingCartItems.Quantity) as TotalPackCount FROM tb_Product  " +
        //    " INNER JOIN tb_ShoppingCartItems ON tb_Product.ProductID = tb_ShoppingCartItems.ProductID   " +
        //    " Where tb_Product.StoreID=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreID") + " And isnull(isFreeShipping,0) <> 1  " +
        //    " And ShoppingCartID In (Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + CustomerID + ")";

        //    Object objTotalPackCount = CommonComponent.GetScalarCommonData(Query);
        //    Int32 TotalPackCount = 0;
        //    if (objTotalPackCount != null)
        //        Int32.TryParse(objTotalPackCount.ToString(), out TotalPackCount);

        //    //Weight
        //    Decimal remainingItemsWeight = 0.0M;
        //    Decimal remainingItemsInsuranceValue = 0.0M;
        //    Decimal TotalWeight = 0;
        //    if (Weight != null)
        //        Decimal.TryParse(Weight.ToString(), out TotalWeight);
        //    int LoopCount = 1;
        //    if (TotalWeight > ShipmentWeight)
        //    {
        //        LoopCount = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(TotalWeight / ShipmentWeight)));
        //        remainingItemsWeight = TotalWeight % ShipmentWeight;
        //    }
        //    else
        //    {
        //        remainingItemsWeight = 0.0M;
        //    }

        //    FedExRateAdmin oFedEx = new FedExRateAdmin();

        //    //For Client
        //    if (WareHouseID > 0)
        //    {
        //        string OrgShippingZip = "";
        //        string OrgCountry = "";
        //        string OrgAddress1 = "";
        //        string OrgAddress2 = "";
        //        string OrgCity = "";
        //        string OrgState = "";

        //        string sqlWareHouse = "SELECT Address1,Address2,City,State,ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse "
        //            + " INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + WareHouseID;
        //        DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
        //        if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
        //        {
        //            OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
        //            OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
        //            OrgAddress1 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address1"]);
        //            OrgAddress2 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address2"]);
        //            OrgCity = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["City"]);
        //            OrgState = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["State"]);
        //        }
        //        CountryComponent objCountry = new CountryComponent();
        //        OrgCountry = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(OrgCountry));
        //        StateComponent objState = new StateComponent();
        //        OrgState = Convert.ToString(objState.GetStateCodeByName(OrgState));

        //        oFedEx.oCity = OrgCity;
        //        oFedEx.oCountryCode = OrgCountry;
        //        oFedEx.oPostalCode = OrgShippingZip;
        //        oFedEx.oStateOrProvinceCode = OrgState;
        //        oFedEx.oStreetLines = OrgAddress1 + ", " + OrgAddress2;
        //    }
        //    else
        //    {
        //        oFedEx.oCity = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginCity");
        //        oFedEx.oCountryCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginCountry");
        //        oFedEx.oPostalCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginZip");
        //        oFedEx.oStateOrProvinceCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginState");
        //        oFedEx.oStreetLines = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginAddress") + ", " + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginAddress2");
        //    }

        //    oFedEx.dCountryCode = Country;
        //    oFedEx.dPostalCode = Zip;
        //    if (Country.ToString().ToLower() != "us")
        //        oFedEx.dStateOrProvinceCode = "";
        //    else
        //        oFedEx.dStateOrProvinceCode = State;


        //    ShippingMethods.FedEx.RateRequest request = oFedEx.Main();
        //    int pakcnt = 0;

        //    #region NewCode

        //    int LoopCountNew = 1;
        //    if (TotalWeight > ShipmentWeight)
        //    {
        //        LoopCountNew = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(TotalWeight / ShipmentWeight)));
        //        remainingItemsWeight = TotalWeight % ShipmentWeight;
        //    }
        //    else
        //    {
        //        remainingItemsWeight = 0.0M;
        //    }

        //    if (remainingItemsWeight > 0)
        //        TotalPackCount = LoopCount + 1;
        //    else TotalPackCount = LoopCount;

        //    #endregion

        //    if (TotalPackCount != 0)
        //        pakcnt = TotalPackCount;
        //    else
        //        pakcnt = DsCartItems.Tables[0].Rows.Count;

        //    request.RequestedShipment.PackageCount = pakcnt.ToString();
        //    request.RequestedShipment.RequestedPackageLineItems = new ShippingMethods.FedEx.RequestedPackageLineItem[pakcnt];

        //    #region Make Packages

        //    PackageID = 1;
        //    int cnt = 0;

        //    #region NewCode

        //    for (int iNew = 0; iNew < LoopCountNew; iNew++)
        //    {
        //        Fedex.Package p = new Fedex.Package();
        //        p.PackageId = PackageID;
        //        PackageID = PackageID + 1;
        //        if (TotalWeight > ShipmentWeight)
        //        {
        //            p.Weight = ShipmentWeight;
        //        }
        //        else
        //        {
        //            p.Weight = TotalWeight;
        //        }

        //        // Set insurance. Get from products db shipping values?
        //        p.Insured = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigBool("RTShipping.Insured"); //false;
        //        p.InsuredValue = remainingItemsInsuranceValue;

        //        #region FedEx Package

        //        request.RequestedShipment.RequestedPackageLineItems[cnt] = new ShippingMethods.FedEx.RequestedPackageLineItem();
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].SequenceNumber = Convert.ToString(cnt + 1); // package sequence number
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Weight = new ShippingMethods.FedEx.Weight(); // package weight
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Units = ShippingMethods.FedEx.WeightUnits.LB;
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Value = p.Weight;
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions = new ShippingMethods.FedEx.Dimensions(); // package dimensions
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Length = p.Length.ToString();
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Width = p.Width.ToString();
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Height = p.Height.ToString();
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Units = ShippingMethods.FedEx.LinearUnits.IN;
        //        if (p.Insured)
        //        {
        //            request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue = new ShippingMethods.FedEx.Money();// insured value
        //            request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Amount = p.InsuredValue;
        //            request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Currency = "USD";
        //        }
        //        cnt++;

        //        #endregion

        //        p = null;
        //    }

        //    if (remainingItemsWeight != 0.0M)
        //    {
        //        Fedex.Package p = new Fedex.Package();
        //        p.PackageId = PackageID;
        //        PackageID = PackageID + 1;
        //        p.Weight = remainingItemsWeight;

        //        // Set insurance. Get from products db shipping values?
        //        p.Insured = AppLogic.AppConfigBool("FedEx.Insured"); //false;
        //        p.InsuredValue = remainingItemsInsuranceValue;

        //        #region FedEx Package

        //        request.RequestedShipment.RequestedPackageLineItems[cnt] = new ShippingMethods.FedEx.RequestedPackageLineItem();
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].SequenceNumber = Convert.ToString(cnt + 1); // package sequence number
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Weight = new ShippingMethods.FedEx.Weight(); // package weight
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Units = ShippingMethods.FedEx.WeightUnits.LB;
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Value = p.Weight;
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions = new ShippingMethods.FedEx.Dimensions(); // package dimensions
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Length = p.Length.ToString();
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Width = p.Width.ToString();
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Height = p.Height.ToString();
        //        request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Units = ShippingMethods.FedEx.LinearUnits.IN;
        //        if (p.Insured)
        //        {
        //            request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue = new ShippingMethods.FedEx.Money(); // insured value
        //            request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Amount = p.InsuredValue;
        //            request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Currency = "USD";
        //        }
        //        cnt++;

        //        #endregion
        //        p = null;
        //    }

        //    #endregion

        //    decimal decTotalWeight = 0;

        //    foreach (GridViewRow gvr in grdShipping.Rows)
        //    {
        //        System.Web.UI.WebControls.CheckBox chkAllowShipment = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkAllowShip");

        //        if (!chkAllowShipment.Checked)
        //            continue;
        //        Decimal ProWeight = Convert.ToDecimal("1");

        //        System.Web.UI.WebControls.TextBox txtHeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtHeightgrid");
        //        System.Web.UI.WebControls.TextBox txtWidth = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtWidthtgrid");
        //        System.Web.UI.WebControls.TextBox txtLength = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtLengthtgrid");
        //        System.Web.UI.WebControls.TextBox txtProWeight = (System.Web.UI.WebControls.TextBox)gvr.FindControl("txtProWeight");
        //        System.Web.UI.WebControls.CheckBox chkInsured = (System.Web.UI.WebControls.CheckBox)gvr.FindControl("chkInsured");
        //        System.Web.UI.WebControls.Label lblavailQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblavailQuantity");
        //        System.Web.UI.WebControls.Label lblShipping = (System.Web.UI.WebControls.Label)gvr.FindControl("lblShipping");
        //        System.Web.UI.WebControls.Label lblQuantity = (System.Web.UI.WebControls.Label)gvr.FindControl("lblQuantity");


        //        if (lblShipping.Text.ToLower() == "no")
        //            decTotalWeight += Convert.ToDecimal(string.IsNullOrEmpty(txtProWeight.Text) ? "0" : txtProWeight.Text);

        //        Fedex.Package p = new Fedex.Package();
        //        p.PackageId = PackageID;
        //        PackageID += 1;


        //        if (txtHeight != null && !string.IsNullOrEmpty(txtHeight.Text))
        //            p.Height = Convert.ToDecimal(txtHeight.Text);
        //        if (txtWidth != null && !string.IsNullOrEmpty(txtWidth.Text))
        //            p.Width = Convert.ToDecimal(txtWidth.Text);
        //        if (txtLength != null && !string.IsNullOrEmpty(txtLength.Text))
        //            p.Length = Convert.ToDecimal(txtLength.Text);
        //        if (txtProWeight != null && !string.IsNullOrEmpty(txtProWeight.Text))
        //            ProWeight = Convert.ToDecimal(txtProWeight.Text);
        //        p.Weight = ProWeight;
        //        p.Insured = chkInsured.Checked; //false;
        //        p.InsuredValue = remainingItemsInsuranceValue;
        //        shipment.AddPackage(p);
        //        p = null;
        //    }

        //    decTotalWeight = Convert.ToDecimal(hfWeight.Value);
        //    try
        //    {
        //        decimal w = Convert.ToDecimal(txtWeight.Text.Trim());
        //    }
        //    catch (Exception)
        //    {
        //        txtWeight.Text = decTotalWeight.ToString();

        //    }

        //    if (Convert.ToDecimal(txtWeight.Text) > Convert.ToDecimal(0.00))
        //    {
        //        Fedex.Package p = new Fedex.Package();
        //        p.PackageId = PackageID;
        //        PackageID += 1;

        //        if (!string.IsNullOrEmpty(txtHeight.Text))
        //            p.Height = Convert.ToDecimal(txtHeight.Text);
        //        if (!string.IsNullOrEmpty(txtWidth.Text))
        //            p.Width = Convert.ToDecimal(txtWidth.Text);
        //        if (!string.IsNullOrEmpty(txtLength.Text))
        //            p.Length = Convert.ToDecimal(txtLength.Text);
        //        p.Weight = Convert.ToDecimal(txtWeight.Text);
        //        p.Insured = false;
        //        shipment.AddPackage(p);
        //    }


        //    #endregion

        //    StringBuilder tmpS = new StringBuilder(4096);
        //    String RTShipRequest = String.Empty;
        //    String RTShipResponse = String.Empty;
        //    object returnObject = null;

        //    ShippingMethods.Fedex objGetRates = new Fedex();
        //    tmpS.Append((string)objGetRates.GetRates(shipment, out RTShipRequest, out RTShipResponse, request, isclient));
        //    returnObject = (object)tmpS;

        //    return returnObject;
        //}

         
        #endregion  
    }
}