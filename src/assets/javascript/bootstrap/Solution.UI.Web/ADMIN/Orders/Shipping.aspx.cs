using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Xml;
using System.Net.Security;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class Shipping : BasePage
    {
        #region Declaration
        OrderComponent ObjOrder = null;
        DataSet DsOrder = new DataSet();
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string OrderNumber = string.Empty;
                if (Request.QueryString["ONo"] != null)
                {
                    OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                }
                Session["ProductIDs"] = null;
                Session["TrackingNumbers"] = null;
                Session["ShippedVia"] = null;
                Session["ShippedOn"] = null;
                Session["CustomCartID"] = null;
                Session["ShippedQty"] = null;
                Session["WarehouseId"] = null;
                Session["OldShippedQty"] = null;
                Session["OldWarehouseId"] = null;
                txtGeneralShippedOn.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                Int32 StoreID = 0;
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + OrderNumber)), out StoreID);
                if (StoreID == 4)
                {
                    btnItemShippingUpdate.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Update-tracking-to-overstock.png) no-repeat transparent; width: 198px; height: 23px; border:none;cursor:pointer;");
                }
                else
                {
                    btnItemShippingUpdate.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Update-and-send-mail.png) no-repeat transparent; width: 138px; height: 23px; border:none;cursor:pointer;");
                }

                BindWareHouse();
                BindData(Convert.ToInt32(OrderNumber), 0);
            }
            FillmanualShipppingLog();
        }

        /// <summary>
        /// Bind Data
        /// </summary>
        /// <param name="ONo">Order No</param>
        private void BindData(Int32 ONo, int FrmWarehouse)
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
                    BindShippingGrid(ONo, FrmWarehouse);
                }
            }
            catch { }
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
        /// Bind Shipping Grid
        /// </summary>
        /// <param name="OrderNumber">OrderNumber</param>
        private void BindShippingGrid(int OrderNumber, Int32 FrmWarehouse)
        {
            DataSet DsCItems = new DataSet();
            DsCItems = CommonComponent.GetCommonDataSet("SELECT Distinct Isnull(tb_OrderedShoppingCartItems.Inventoryupdated,0) as Inventoryupdated, (isnull(tb_OrderedShoppingCartItems.WareHouseID,0)) as WareHouseID,case when isnull(s.ShippedQty,0) > 0 then isnull(s.ShippedQty,0) else isnull(tb_OrderedShoppingCartItems.ShippedQty,0) end as ShippedQty,tb_Product.Name,isnull(tb_OrderedShoppingCartItems.SKU,tb_Product.SKU) as SKU, tb_OrderedShoppingCartItems.Quantity,"
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
                                                        + "    as TrackingNumber,ShippedVia,ShippedOn from tb_Order ) o ON  o.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID "
                                                        + "    on tb_Product.ProductID = tb_OrderedShoppingCartItems.RefProductID Where OrderedShoppingCartID = (select ShoppingCardID FROM  tb_Order where OrderNumber=" + OrderNumber + ")");
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
            if (FrmWarehouse == 0)
            {
                Session["ProductIDs"] = null;
                Session["TrackingNumbers"] = null;
                Session["ShippedVia"] = null;
                Session["ShippedOn"] = null;
                Session["CustomCartID"] = null;
                Session["ShippedQty"] = null;
                Session["WarehouseId"] = null;
                Session["OldShippedQty"] = null;
                Session["OldWarehouseId"] = null;
            }
            btnItemShippingUpdate.Visible = true;
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
                btnEdit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/edit-price.gif";
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
                Label lblavailQuantity = (Label)e.Row.FindControl("lblavailQuantity");
                Label lblInventoryupdated = (Label)e.Row.FindControl("lblInventoryupdated");

                Label lblVariantNames = (Label)e.Row.FindControl("lblVariantNames");
                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                Label lblProductName = (Label)e.Row.FindControl("lblProductName");
                // RequiredFieldValidator reqDate = (RequiredFieldValidator)e.Row.FindControl("reqDate");
                //// reqDate.ID = reqDate +""+e.Row.RowIndex;
                // reqDate.ControlToValidate = txtShippedOn.ID + "_" + e.Row.RowIndex;
                // CompareValidator compDate = (CompareValidator)e.Row.FindControl("compDate");
                // compDate.ControlToValidate = txtShippedOn.ID + "_" + e.Row.RowIndex;



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

                if (lblInventoryupdated != null && Convert.ToBoolean(lblInventoryupdated.Text))
                {
                    btnEdit.Visible = false;
                }
                else
                {
                    btnEdit.Visible = true;
                }

                int qty = 0;
                try { qty = Convert.ToInt32(lblQuantity.Text); }
                catch { qty = 0; }
                string sqlinv = "SELECT ISNULL(Inventory,0) AS Inventory FROM dbo.tb_WareHouseProductInventory WHERE ProductID=" + lblProductID.Text + " AND WareHouseID='" + ddlWareHouse.SelectedItem.Value + "'";
                Int32 winventory = Convert.ToInt32(CommonComponent.GetScalarCommonData(sqlinv));
                if (winventory <= 0)
                {
                    lblavailQuantity.Text = "0";
                    lblavailQuantity.ForeColor = System.Drawing.Color.Red;
                    lblavailQuantity.Font.Bold = true;
                }
                else
                {
                    lblavailQuantity.Text = winventory.ToString();
                    lblavailQuantity.ForeColor = System.Drawing.Color.Green;
                    lblavailQuantity.Font.Bold = true;
                }

                if (lblCustomCartID != null)
                {
                    Int32 OrderedCustomCartID = Convert.ToInt32(lblCustomCartID.Text.ToString());
                    if (OrderedCustomCartID > 0)
                    {
                        if (Session["CustomCartID"] != null)
                        {
                            ArrayList Productlist = null;
                            ArrayList Trackinglist = null;
                            ArrayList Courierlist = null;
                            ArrayList ShippedDateList = null;
                            ArrayList ShippedQty = null;
                            ArrayList ShippedNote = null;
                            ArrayList CustomCartID = null;
                            ArrayList WarehouseId = null;

                            Productlist = Session["ProductIDs"] as ArrayList;
                            Trackinglist = Session["TrackingNumbers"] as ArrayList;
                            Courierlist = Session["ShippedVia"] as ArrayList;
                            if (Session["ShippedOn"] != null)
                            {
                                ShippedDateList = Session["ShippedOn"] as ArrayList;
                            }
                            ShippedQty = Session["ShippedQty"] as ArrayList;
                            ShippedNote = Session["ShippedNote"] as ArrayList;
                            CustomCartID = Session["CustomCartID"] as ArrayList;
                            WarehouseId = Session["WarehouseId"] as ArrayList;

                            for (int j = 0; j < CustomCartID.Count; j++)
                            {
                                if (CustomCartID[j].ToString() == lblCustomCartID.Text.ToString())
                                {
                                    lblTracking.Text = Trackinglist[j].ToString();
                                    txtTracking.Text = Trackinglist[j].ToString();

                                    lblShippedQty.Text = ShippedQty[j].ToString();
                                    txtShippedQty.Text = ShippedQty[j].ToString();

                                    lblShippedOn.Text = SetShortDate(ShippedDateList[j].ToString());
                                    txtShippedOn.Text = SetShortDate(ShippedDateList[j].ToString());

                                    lblShippedVia.Text = Courierlist[j].ToString();
                                    ddlCourier.SelectedItem.Text = Courierlist[j].ToString();

                                    lblShippedNote.Text = ShippedNote[j].ToString();
                                    txtShippedNote.Text = ShippedNote[j].ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// GridView Row Copmmand Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdShipping_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region edit
            if (e.CommandName == "CustomEdit")
            {
                foreach (GridViewRow dr in grdShipping.Rows)
                {
                    Label lblID = (Label)dr.FindControl("lblProductID");
                    Label lblCustomCartID1 = (Label)dr.FindControl("lblCustomCartID");

                    if (lblCustomCartID1.Text == e.CommandArgument.ToString())
                    {
                        Label lblTracking = (Label)dr.FindControl("lblTrackingNumber");
                        TextBox txtTracking = (TextBox)dr.FindControl("txtTrackingNumber");
                        DropDownList ddlCourier = (DropDownList)dr.FindControl("ddlShippedVIA");
                        Label lblShippedVia = (Label)dr.FindControl("lblShippedVia");
                        ImageButton btnEdit = (ImageButton)dr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)dr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)dr.FindControl("btnCancel");
                        CheckBox chkShipped = (CheckBox)dr.FindControl("chkShipped");
                        Label lblShippedOn = (Label)dr.FindControl("lblShippedOn");

                        //TextBox txtShippedOn = (TextBox)dr.FindControl("txtShippedOn");
                        // eWorld.UI.CalendarPopup txtShippedOn = (eWorld.UI.CalendarPopup)dr.FindControl("txtShippedOn2");
                        // System.Web.UI.HtmlControls.HtmlContainerControl divcalendor = (System.Web.UI.HtmlControls.HtmlContainerControl)dr.FindControl("divcalendor");

                        TextBox txtShippedOn = (TextBox)dr.FindControl("txtShippedOn2");
                        txtShippedOn.Visible = true;
                        txtShippedOn.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));

                        Label lblShippedNote = (Label)dr.FindControl("lblShippedNote");
                        TextBox txtShippedNote = (TextBox)dr.FindControl("txtShippedNote");

                        Label lblShippedQty = (Label)dr.FindControl("lblShippedQty");
                        TextBox txtShippedQty = (TextBox)dr.FindControl("txtShippedQty");

                        lblShippedQty.Visible = false;
                        txtShippedQty.Visible = true;


                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnEdit.Visible = false;

                        lblShippedVia.Visible = false;
                        ddlCourier.Visible = true;

                        //if (lblShippedVia.Text == "")
                        //{
                        //    lblShippedVia.Text = "FedEx";
                        //}

                        lblTracking.Visible = false;
                        txtTracking.Visible = true;

                        lblShippedOn.Visible = false;
                        //txtShippedOn.Visible = true;
                        //divcalendor.Style.Add("display", "block");

                        lblShippedNote.Visible = false;
                        txtShippedNote.Visible = true;

                        lblShippedOn.Visible = false;


                        chkShipped.Enabled = true;
                        if (lblShippedOn.Text.ToString() != "")
                        {
                            txtShippedOn.Text = lblShippedOn.Text;
                        }
                        //else
                        //{

                        //    txtShippedOn.Reset();

                        //}

                        txtTracking.Text = lblTracking.Text;
                        txtShippedQty.Text = lblShippedQty.Text;
                        txtShippedNote.Text = lblShippedNote.Text.ToString();
                        try
                        {
                            if (lblShippedVia.Text.ToString().IndexOf(",") > -1)
                            {
                                string[] splitName = lblShippedVia.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                if (splitName[0].ToString().ToLower() == "other")
                                {
                                    ddlCourier.SelectedValue = "Other";
                                }
                                else
                                {
                                    ddlCourier.SelectedValue = splitName[0].ToString();
                                }
                            }
                            else
                            {
                                ddlCourier.SelectedIndex = ddlCourier.Items.IndexOf(new ListItem(lblShippedVia.Text));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            #endregion

            #region cancel
            else if (e.CommandName == "CustomCancel")
            {
                foreach (GridViewRow dr in grdShipping.Rows)
                {
                    Label lblID = (Label)dr.FindControl("lblProductID");
                    Label lblCustomCartID1 = (Label)dr.FindControl("lblCustomCartID");
                    if (lblCustomCartID1.Text == e.CommandArgument.ToString())
                    {
                        Label lblTracking = (Label)dr.FindControl("lblTrackingNumber");
                        TextBox txtTracking = (TextBox)dr.FindControl("txtTrackingNumber");
                        DropDownList ddlCourier = (DropDownList)dr.FindControl("ddlShippedVIA");
                        Label lblShippedVia = (Label)dr.FindControl("lblShippedVia");
                        ImageButton btnEdit = (ImageButton)dr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)dr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)dr.FindControl("btnCancel");
                        CheckBox chkShipped = (CheckBox)dr.FindControl("chkShipped");
                        Label lblShippedOn = (Label)dr.FindControl("lblShippedOn");
                        //TextBox txtShippedOn = (TextBox)dr.FindControl("txtShippedOn");
                        //eWorld.UI.CalendarPopup txtShippedOn = (eWorld.UI.CalendarPopup)dr.FindControl("txtShippedOn2");
                        System.Web.UI.HtmlControls.HtmlContainerControl divcalendor = (System.Web.UI.HtmlControls.HtmlContainerControl)dr.FindControl("divcalendor");
                        Label lblShippedQty = (Label)dr.FindControl("lblShippedQty");
                        TextBox txtShippedQty = (TextBox)dr.FindControl("txtShippedQty");

                        Label lblShippedNote = (Label)dr.FindControl("lblShippedNote");
                        TextBox txtShippedNote = (TextBox)dr.FindControl("txtShippedNote");


                        TextBox txtShippedOn = (TextBox)dr.FindControl("txtShippedOn2");
                        txtShippedOn.Visible = false;

                        txtShippedQty.Text = lblShippedQty.Text;

                        lblShippedQty.Visible = true;
                        txtShippedQty.Visible = false;

                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        btnEdit.Visible = true;

                        lblShippedVia.Visible = true;
                        ddlCourier.Visible = false;

                        lblTracking.Visible = true;
                        txtTracking.Visible = false;

                        lblShippedOn.Visible = true;
                        //   txtShippedOn.Visible = false;
                        //divcalendor.Style.Add("display", "none");
                        //divcalendor.Visible = false;
                        lblShippedNote.Visible = true;
                        txtShippedNote.Visible = false;

                        txtTracking.Text = "0";

                        //txtShippedOn.Reset();
                        txtShippedNote.Text = "";

                        ddlCourier.SelectedIndex = -1;
                        ddlCourier.SelectedIndex = 0;
                        chkShipped.Enabled = false;
                    }
                }
            }
            #endregion

            #region save
            else if (e.CommandName == "CustomSave")
            {
                //if (ddlWareHouse.SelectedIndex == 0)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@warehousemsg", "alert('Please Select Warehouse');", true);
                //    return;
                //}
                //else
                //{
                foreach (GridViewRow dr in grdShipping.Rows)
                {
                    Label lblID = (Label)dr.FindControl("lblProductID");
                    Label lblCustomCartID1 = (Label)dr.FindControl("lblCustomCartID");
                    if (lblCustomCartID1.Text == e.CommandArgument.ToString())
                    {
                        Label lblTracking = (Label)dr.FindControl("lblTrackingNumber");
                        TextBox txtTracking = (TextBox)dr.FindControl("txtTrackingNumber");
                        DropDownList ddlCourier = (DropDownList)dr.FindControl("ddlShippedVIA");

                        Label lblCustomCartID = (Label)dr.FindControl("lblCustomCartID");
                        Label lblShippedVia = (Label)dr.FindControl("lblShippedVia");
                        ImageButton btnEdit = (ImageButton)dr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)dr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)dr.FindControl("btnCancel");
                        CheckBox chkShipped = (CheckBox)dr.FindControl("chkShipped");
                        Label lblShippedOn = (Label)dr.FindControl("lblShippedOn");
                        //  TextBox txtShippedOn = (TextBox)dr.FindControl("txtShippedOn");
                        // eWorld.UI.CalendarPopup txtShippedOn = (eWorld.UI.CalendarPopup)dr.FindControl("txtShippedOn2");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnshipped = (System.Web.UI.HtmlControls.HtmlInputHidden)dr.FindControl("hdnshipped");
                        System.Web.UI.HtmlControls.HtmlContainerControl divcalendor = (System.Web.UI.HtmlControls.HtmlContainerControl)dr.FindControl("divcalendor");
                        Label lblQty = (Label)dr.FindControl("lblQty");
                        Label lblShippedQty = (Label)dr.FindControl("lblShippedQty");
                        TextBox txtShippedQty = (TextBox)dr.FindControl("txtShippedQty");
                        TextBox txtShippedOn = (TextBox)dr.FindControl("txtShippedOn2");
                        Label lblShippedNote = (Label)dr.FindControl("lblShippedNote");
                        TextBox txtShippedNote = (TextBox)dr.FindControl("txtShippedNote");
                        Label lblavailQuantity = (Label)dr.FindControl("lblavailQuantity");

                        Label lblOldQty = (Label)dr.FindControl("lblOldQty");
                        Label lblOldWarehouseId = (Label)dr.FindControl("lblOldWarehouseId");

                        Int32 maxqty = 0, shipqty = 0, qty = 0, AvailQuantity = 0;
                        Int32.TryParse(lblQty.Text, out maxqty);
                        Int32.TryParse(lblShippedQty.Text, out shipqty);
                        Int32.TryParse(txtShippedQty.Text, out qty);
                        Int32.TryParse(lblavailQuantity.Text, out AvailQuantity);

                        if (qty <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msgvalid01", "alert('Please enter valid Quantity.');", true);
                            txtShippedQty.Focus();
                            return;
                        }
                        else if (qty <= maxqty)
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "msgvalid", "alert('Please enter " + shipqty + " Quantity or less.');", true);
                            //txtShippedQty.Text = (maxqty - shipqty).ToString();
                            //txtShippedQty.Focus();
                            //return;
                        }
                        else if ((qty) > maxqty)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msgvalid02", "alert('Please enter " + maxqty + " Quantity or less.');", true);
                            txtShippedQty.Focus();
                            return;
                        }
                        //else if ((shipqty + qty) > maxqty)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "msgvalid02", "alert('Please enter " + (maxqty - shipqty) + " Quantity or less.');", true);
                        //    txtShippedQty.Text = (maxqty - shipqty).ToString();
                        //    txtShippedQty.Focus();
                        //    return;
                        //}
                        //if (qty > AvailQuantity)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "msgvalid03", "alert('Total Available Quantity is :" + (AvailQuantity) + "');", true);
                        //    txtShippedQty.Focus();
                        //    return;
                        //}
                        if (Convert.ToString(txtShippedOn.Text).IndexOf("1900") > -1)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msgvalid0", "alert('Please enter valid date.');", true);
                            txtShippedOn.Focus();
                            return;
                        }

                        ArrayList Productlist = null;
                        ArrayList Trackinglist = null;
                        ArrayList Courierlist = null;
                        ArrayList ShippedDateList = null;
                        ArrayList ShippedQty = null;
                        ArrayList ShippedNote = null;
                        ArrayList CustomCartID = null;
                        ArrayList WarehouseId = null;

                        ArrayList OldShippedQty = null;
                        ArrayList OldWarehouseId = null;

                        if (Session["ProductIDs"] == null)
                        {
                            Productlist = new ArrayList();
                            Trackinglist = new ArrayList();
                            Courierlist = new ArrayList();
                            ShippedDateList = new ArrayList();
                            ShippedQty = new ArrayList();
                            ShippedNote = new ArrayList();
                            CustomCartID = new ArrayList();
                            WarehouseId = new ArrayList();
                            OldShippedQty = new ArrayList();
                            OldWarehouseId = new ArrayList();
                        }
                        else
                        {
                            Productlist = Session["ProductIDs"] as ArrayList;
                            Trackinglist = Session["TrackingNumbers"] as ArrayList;
                            Courierlist = Session["ShippedVia"] as ArrayList;
                            if (Session["ShippedOn"] != null)
                            {
                                ShippedDateList = Session["ShippedOn"] as ArrayList;
                            }
                            ShippedQty = Session["ShippedQty"] as ArrayList;
                            ShippedNote = Session["ShippedNote"] as ArrayList;
                            CustomCartID = Session["CustomCartID"] as ArrayList;
                            OldShippedQty = Session["OldShippedQty"] as ArrayList;
                            WarehouseId = Session["WarehouseId"] as ArrayList;
                            OldWarehouseId = Session["OldWarehouseId"] as ArrayList;

                            if (Productlist != null && Productlist.Contains(lblID.Text) && CustomCartID.Contains(lblCustomCartID.Text.Trim()))
                            {
                                int cnt = CustomCartID.IndexOf(lblCustomCartID.Text.Trim());
                                Productlist.RemoveAt(cnt);
                                Trackinglist.RemoveAt(cnt);
                                Courierlist.RemoveAt(cnt);
                                ShippedDateList.RemoveAt(cnt);

                                ShippedQty.RemoveAt(cnt);
                                CustomCartID.RemoveAt(cnt);
                                WarehouseId.RemoveAt(cnt);

                                OldShippedQty.RemoveAt(cnt);
                                OldWarehouseId.RemoveAt(cnt);
                                try
                                {
                                    ShippedNote.RemoveAt(cnt);
                                }
                                catch
                                {
                                }
                            }
                        }

                        CustomCartID.Add(lblCustomCartID.Text.Trim());
                        Session["CustomCartID"] = CustomCartID;
                        //if(hdnshipped.Value.ToString().ToLower() == "true")
                        //{
                        //    ShippedQty.Add(Convert.ToInt32(0));
                        //}
                        //else
                        //{
                        ShippedQty.Add(Convert.ToInt32(txtShippedQty.Text));
                        // }

                        Session["ShippedQty"] = ShippedQty;

                        OldShippedQty.Add(lblOldQty.Text);
                        Session["OldShippedQty"] = OldShippedQty;
                        OldWarehouseId.Add(lblOldWarehouseId.Text);
                        Session["OldWarehouseId"] = OldWarehouseId;

                        Productlist.Add(lblID.Text);
                        Session["ProductIDs"] = Productlist;
                        Trackinglist.Add(txtTracking.Text);
                        Session["TrackingNumbers"] = Trackinglist;
                        Courierlist.Add(ddlCourier.SelectedValue);
                        Session["ShippedVia"] = Courierlist;
                        ShippedDateList.Add(Convert.ToDateTime(txtShippedOn.Text));
                        Session["ShippedOn"] = ShippedDateList;
                        ShippedNote.Add(Convert.ToString(txtShippedNote.Text));
                        Session["ShippedNote"] = ShippedNote;

                        // assign value of warehouseid from here
                        WarehouseId.Add(Convert.ToString(ddlWareHouse.SelectedValue.ToString()));
                        Session["WarehouseId"] = WarehouseId;

                        lblShippedQty.Text = txtShippedQty.Text;
                        lblTracking.Text = txtTracking.Text;
                        lblShippedOn.Text = SetShortDate(txtShippedOn.Text.ToString());
                        lblShippedVia.Text = ddlCourier.SelectedItem.Text;
                        // lblShippedOn.Text = SetShortDate(txtShippedOn.SelectedDate.ToString());
                        lblShippedNote.Text = txtShippedNote.Text.ToString();

                        lblShippedQty.Visible = true;
                        txtShippedQty.Visible = false;


                        txtShippedOn.Visible = false;

                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        btnEdit.Visible = true;

                        lblShippedVia.Visible = true;
                        ddlCourier.Visible = false;

                        lblTracking.Visible = true;
                        txtTracking.Visible = false;

                        lblShippedOn.Visible = true;
                        //   txtShippedOn.Visible = false;
                        //divcalendor.Visible = false;
                        // divcalendor.Style.Add("display", "none");
                        lblShippedNote.Visible = true;
                        txtShippedNote.Visible = false;

                        txtShippedQty.Text = "";
                        txtTracking.Text = "";
                        txtShippedNote.Text = "";

                        ddlCourier.SelectedIndex = -1;
                        ddlCourier.SelectedIndex = 0;
                        chkShipped.Checked = true;
                        chkShipped.Enabled = false;
                        btnItemShippingUpdate.Visible = true;
                        //uppan1.Update();
                    }
                }
                // }
            }
            #endregion
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

        /// <summary>
        /// Shipping Update Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ShippingUpdate_Click(object sender, EventArgs e)
        {
            int LableNo = 1;
            int pckcount = 1;

            string strError = "";

            string OrderNumber = string.Empty;
            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
            }

            try
            {
                #region Fill Session if NULL - Ashish
                if (Session["ProductIDs"] == null && Session["TrackingNumbers"] == null
                   && Session["ShippedVia"] == null && Session["ShippedOn"] == null && Session["ShippedQty"] == null && Session["CustomCartID"] == null && Session["WarehouseId"] == null && Session["OldShippedQty"] == null && Session["OldWarehouseId"] == null)
                {

                    ArrayList Productlist = new ArrayList();
                    ArrayList Trackinglist = new ArrayList();
                    ArrayList Courierlist = new ArrayList();
                    ArrayList ShippedDateList = new ArrayList();
                    ArrayList ShippedQty = new ArrayList();
                    ArrayList ShippedNote = new ArrayList();
                    ArrayList CustomCartID = new ArrayList();
                    ArrayList WarehouseId = new ArrayList();
                    ArrayList OldShippedQty = new ArrayList();
                    ArrayList OldWarehouseId = new ArrayList();

                    //clsOrderedShoppingCartItems ObjOCart = new clsOrderedShoppingCartItems();
                    DataSet DsCItems = new DataSet();
                    DsCItems = CommonComponent.GetCommonDataSet("SELECT Isnull(tb_OrderedShoppingCartItems.Inventoryupdated,0) as Inventoryupdated,(isnull(tb_OrderedShoppingCartItems.WareHouseID,0)) as WareHouseID,case when isnull(tb_OrderedShoppingCartItems.ShippedQty,0) =0 then isnull(s.ShippedQty,0) else isnull(tb_OrderedShoppingCartItems.ShippedQty,0) end as ShippedQty,tb_Product.Name,tb_Product.SKU, tb_OrderedShoppingCartItems.Quantity,"
                                                        + "    tb_OrderedShoppingCartItems.OrderedCustomCartID, tb_OrderedShoppingCartItems.RefProductID,"
                                                        + "    tb_OrderedShoppingCartItems.VariantNames,tb_OrderedShoppingCartItems.VariantValues,"
                                                        + "    (isnull(s.TrackingNumber,tb_OrderedShoppingCartItems.TrackingNumber)) TrackingNumber,isnull(s.ShippedVia,tb_OrderedShoppingCartItems.ShippedVia) ShippedVia,"
                                                        + "   (isnull(s.Shipped,0)) as Shipped,isnull(s.Shipped,0) as ShippedProduct,"
                                                        + "    isnull(s.ShippedOn,tb_OrderedShoppingCartItems.ShippedOn) ShippedOn,  tb_Product.Description,tb_OrderedShoppingCartItems.Price As "
                                                        + "    SalePrice, isnull(s.ShippedNote,'') as ShippedNote"
                                                        + "    FROM tb_Product INNER JOIN tb_OrderedShoppingCartItems"
                                                        + "    left outer join (select RefProductID,OrderNumber,trackingNumber,ShippedVia,Shipped,ShippedOn,ShippedQty,"
                                                        + "    ShippedNote from  tb_OrderShippedItems where OrderNumber=" + OrderNumber + ") s on s.RefProductID=tb_OrderedShoppingCartItems.RefProductID  "
                                                        + "    inner join (select convert(bit,isnull(len(ShippedOn),0)) as shipped,ShoppingCardID,ShippingTrackingNumber"
                                                        + "    as TrackingNumber,ShippedVia,ShippedOn from tb_Order ) o ON  o.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID "
                                                        + "    on tb_Product.ProductID = tb_OrderedShoppingCartItems.RefProductID Where OrderedShoppingCartID = (select ShoppingCardID FROM  tb_Order where OrderNumber=" + OrderNumber + ")");
                    if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(DsCItems.Tables[0].Rows[i]["Shipped"]) == true)
                            {
                                Productlist.Add(Convert.ToString(DsCItems.Tables[0].Rows[i]["RefProductID"]));
                                Session["ProductIDs"] = Productlist;

                                Trackinglist.Add(DsCItems.Tables[0].Rows[i]["TrackingNumber"]);
                                Session["TrackingNumbers"] = Trackinglist;

                                Courierlist.Add(DsCItems.Tables[0].Rows[i]["ShippedVia"]);
                                Session["ShippedVia"] = Courierlist;

                                ShippedDateList.Add(DsCItems.Tables[0].Rows[i]["ShippedOn"]);
                                Session["ShippedOn"] = ShippedDateList;

                                ShippedQty.Add(DsCItems.Tables[0].Rows[i]["ShippedQty"]);
                                Session["ShippedQty"] = ShippedQty;

                                ShippedNote.Add(DsCItems.Tables[0].Rows[i]["ShippedNote"]);
                                Session["ShippedNote"] = ShippedNote;

                                CustomCartID.Add(DsCItems.Tables[0].Rows[i]["OrderedCustomCartID"]);
                                Session["CustomCartID"] = CustomCartID;

                                WarehouseId.Add(DsCItems.Tables[0].Rows[i]["WarehouseId"]);
                                Session["WarehouseId"] = WarehouseId;

                                OldShippedQty.Add(DsCItems.Tables[0].Rows[i]["ShippedQty"]);
                                Session["OldShippedQty"] = OldShippedQty;

                                OldWarehouseId.Add(DsCItems.Tables[0].Rows[i]["WarehouseId"]);
                                Session["OldWarehouseId"] = OldWarehouseId;
                            }
                        }
                    }
                }
                #endregion Fill Session if NULL - Ashish

                if (Session["ProductIDs"] != null && Session["TrackingNumbers"] != null
                    && Session["ShippedVia"] != null && Session["ShippedOn"] != null && Session["ShippedQty"] != null && Session["CustomCartID"] != null && Session["WarehouseId"] != null && Session["OldShippedQty"] != null && Session["OldWarehouseId"] != null)
                {
                    //ObjOrder = new clsOrder(OrderNumber);
                    //clsOrderedShoppingCartItems ObjOCart = new clsOrderedShoppingCartItems();
                    DataSet DsCItems = new DataSet();
                    DsCItems = CommonComponent.GetCommonDataSet("SELECT Isnull(tb_OrderedShoppingCartItems.Inventoryupdated,0) as Inventoryupdated,(isnull(tb_OrderedShoppingCartItems.WareHouseID,0)) as WareHouseID,case when isnull(tb_OrderedShoppingCartItems.ShippedQty,0) =0 then isnull(s.ShippedQty,0) else isnull(tb_OrderedShoppingCartItems.ShippedQty,0) end as ShippedQty,tb_Product.Name,tb_Product.SKU, tb_OrderedShoppingCartItems.Quantity,"
                                                                            + "    tb_OrderedShoppingCartItems.OrderedCustomCartID, tb_OrderedShoppingCartItems.RefProductID,"
                                                                            + "    tb_OrderedShoppingCartItems.VariantNames,tb_OrderedShoppingCartItems.VariantValues,"
                                                                            + "    (isnull(s.TrackingNumber,tb_OrderedShoppingCartItems.TrackingNumber)) TrackingNumber,isnull(s.ShippedVia,tb_OrderedShoppingCartItems.ShippedVia) ShippedVia,"
                                                                            + "   (isnull(s.Shipped,0)) as Shipped,isnull(s.Shipped,0) as ShippedProduct,"
                                                                            + "    isnull(s.ShippedOn,tb_OrderedShoppingCartItems.ShippedOn) ShippedOn,  tb_Product.Description,tb_OrderedShoppingCartItems.Price As "
                                                                            + "    SalePrice, isnull(s.ShippedNote,'') as ShippedNote"
                                                                            + "    FROM tb_Product INNER JOIN tb_OrderedShoppingCartItems"
                                                                            + "    left outer join (select RefProductID,OrderNumber,trackingNumber,ShippedVia,Shipped,ShippedOn,ShippedQty,"
                                                                            + "    ShippedNote from  tb_OrderShippedItems where OrderNumber=" + OrderNumber + ") s on s.RefProductID=tb_OrderedShoppingCartItems.RefProductID  "
                                                                            + "    inner join (select convert(bit,isnull(len(ShippedOn),0)) as shipped,ShoppingCardID,ShippingTrackingNumber"
                                                                            + "    as TrackingNumber,ShippedVia,ShippedOn from tb_Order ) o ON  o.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID "
                                                                            + "    on tb_Product.ProductID = tb_OrderedShoppingCartItems.RefProductID Where OrderedShoppingCartID = (select ShoppingCardID FROM  tb_Order where OrderNumber=" + OrderNumber + ")");
                    if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
                    {
                        //OrderCustomer = new clsCustomer(ObjOrder.CustomerID);
                        ArrayList ProductList = (ArrayList)Session["ProductIDs"];
                        ArrayList TrackingList = (ArrayList)Session["TrackingNumbers"];
                        ArrayList CourierList = (ArrayList)Session["ShippedVia"];
                        ArrayList ShippedDateList = (ArrayList)Session["ShippedOn"];
                        ArrayList ShippedQty = (ArrayList)Session["ShippedQty"];
                        ArrayList ShippedNote = (ArrayList)Session["ShippedNote"];
                        ArrayList CustomCartID = (ArrayList)Session["CustomCartID"];
                        ArrayList WarehouseId = (ArrayList)Session["WarehouseId"];

                        ArrayList OldShippedQty = (ArrayList)Session["OldShippedQty"];
                        ArrayList OldWarehouseId = (ArrayList)Session["OldWarehouseId"];

                        String ServerURL = "http:///www." + AppLogic.AppConfigs("LiveServer") + "/";
                        string FedExlink = "";
                        string cart = GenerateAndUpdateCartHavingShippingStatus(DsCItems.Tables[0].Rows, ProductList, TrackingList, CourierList, ShippedDateList, ShippedQty, ShippedNote, CustomCartID, WarehouseId, OldShippedQty, OldWarehouseId, ref FedExlink);

                        string TrackNo = string.Empty;
                        for (int j = 0; j < TrackingList.Count; j++)
                        {
                            TrackNo += TrackingList[j] + ", ";
                        }

                        TrackNo = TrackNo.Substring(0, TrackNo.Length - 2);

                        string sql = "select * from tb_Order where OrderNumber=" + OrderNumber;
                        DataSet dsOrderDetails = CommonComponent.GetCommonDataSet(sql);
                        Int32 storeId = Convert.ToInt32(AppConfig.StoreID);
                        string ShipToAddress = string.Empty;
                        if (dsOrderDetails != null && dsOrderDetails.Tables.Count > 0 && dsOrderDetails.Tables[0].Rows.Count > 0)
                        {
                            storeId = Convert.ToInt32(dsOrderDetails.Tables[0].Rows[0]["StoreID"]);
                            string partnerNumber = Convert.ToString(dsOrderDetails.Tables[0].Rows[0]["PartnerNumber"].ToString());
                            ShipToAddress += "<table class='popup_cantain' cellspacing='0' cellpadding='0' border='0' align='left' style='padding-left:0px;'>";
                            ShipToAddress += "<tr><td style='font-family:Arial,Helvetica,sans-serif;font-size:12px;'>";

                            if (!string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingFirstName"].ToString()) || !string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingLastName"].ToString()))
                            {
                                ShipToAddress += dsOrderDetails.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + dsOrderDetails.Tables[0].Rows[0]["ShippingLastName"].ToString() + "<br />";
                            }
                            else
                            {
                                ShipToAddress += dsOrderDetails.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsOrderDetails.Tables[0].Rows[0]["LastName"].ToString() + "<br />";
                            }
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingCompany"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br />");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingAddress1"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingAddress1"].ToString() + "<br />");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingAddress2"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingAddress2"].ToString() + "<br /> ");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingSuite"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingSuite"].ToString() + "<br /> ");

                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingCity"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingCity"].ToString() + ", ");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingState"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingState"].ToString() + " ");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingZip"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br />");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingCountry"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingCountry"].ToString() + "<br /> ");
                            ShipToAddress += ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["ShippingPhone"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["ShippingPhone"].ToString() + "");

                            ShipToAddress += "</td></tr>";
                            ShipToAddress += "</table>";
                            if (storeId == 14 || storeId == 4)
                            {
                                if (string.IsNullOrEmpty(partnerNumber))
                                {
                                    GetConfirmShipmentOverStock(storeId, Convert.ToInt32(OrderNumber), dsOrderDetails.Tables[0].Rows[0]["RefOrderID"].ToString());
                                }
                                else
                                {
                                    GetConfirmShipmentOverStock(storeId, Convert.ToInt32(OrderNumber), partnerNumber.ToString());
                                }
                            }

                        }

                        StringBuilder messageBody = new StringBuilder(10000);

                        #region mail format

                        CustomerComponent objCustomer = new CustomerComponent();
                        DataSet dsMailInfo = new DataSet();
                        dsMailInfo = objCustomer.GetEmailTamplate("ManualShippingMailConfirm", storeId);

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

                            strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("StoreName").ToString(), RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###FIRSTNAME###", dsOrderDetails.Tables[0].Rows[0]["FirstName"].ToString() + ' ' + dsOrderDetails.Tables[0].Rows[0]["LastName"].ToString(), RegexOptions.IgnoreCase);

                              strBody = Regex.Replace(strBody, "###Product_Lists###", cart, RegexOptions.IgnoreCase);

                              strBody = Regex.Replace(strBody, "###FedExlink###", FedExlink, RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###Ship To###", ShipToAddress, RegexOptions.IgnoreCase);

                            //strBody = Regex.Replace(strBody, "###shipping method###", type, RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###Order Number###", OrderNumber, RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###Tracking Number###", TrackNo + "&nbsp;&nbsp;", RegexOptions.IgnoreCase);

                            //strBody = Regex.Replace(strBody, "###Number of Packages###", LableNo.ToString() + " of " + pckcount, RegexOptions.IgnoreCase);

                            strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                            //strBody = Regex.Replace(strBody, "###Number of Packages###", "1 of 1", RegexOptions.IgnoreCase);

                            AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");

                            string ToName = ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["BillingFirstName"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["BillingLastName"].ToString())) ? "" : dsOrderDetails.Tables[0].Rows[0]["BillingLastName"].ToString());
                            string ToMail = ((string.IsNullOrEmpty(dsOrderDetails.Tables[0].Rows[0]["BillingEmail"].ToString())) ? dsOrderDetails.Tables[0].Rows[0]["Email"].ToString() : dsOrderDetails.Tables[0].Rows[0]["BillingEmail"].ToString());
                            try
                            {
                                if (ToMail.Trim() == "")
                                {
                                    ToMail = dsOrderDetails.Tables[0].Rows[0]["Email"].ToString();
                                }
                                string ToID = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(EmailID,'') FROM tb_ContactEmail WHERE Subject='Order Shipped'"));
                                if (!string.IsNullOrEmpty(ToID))
                                {
                                    CommonOperations.SendMail(ToID + ";" + ToMail, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                                }
                                else
                                {
                                    CommonOperations.SendMail(ToMail, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                                }

                            }
                            catch (Exception ex)
                            {
                                strError = Convert.ToString(ex.Message.ToString());
                            }
                            try
                            {

                                // Response.Write(strError);

                                CommonComponent.ExecuteCommonData("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber,CreatedOn) VALUES (" + Session["AdminID"].ToString() + ",12," + Convert.ToInt32(OrderNumber) + "," + Convert.ToInt32(OrderNumber) + ",'" + DateTime.Now.ToString() + "')");
                                OrderComponent objAddOrder = new OrderComponent();
                                objAddOrder.InsertOrderlog(12, Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"])), "", Convert.ToInt32(Session["AdminID"].ToString()));

                                FillmanualShipppingLog();
                            }
                            catch (Exception ex)
                            {
                            }
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
                            // messageBody.Append("<td height='25' align='left' valign='bottom' class='popup_cantain' style='color:#585858;font-family:Arial,Helvetica,sans-serif;font-size:12px;line-height:20px;padding-left:10px;text-decoration:none;'><span><br/>This message was sent to notify you that the electronic shipment has been transmitted to<b> " + type + "</b>. The physical package(s) may or may not have actually been tendered to <b> " + type + "</b> for shipment.<br /><br /></span></td>");
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
                            messageBody.Append("<td class='popup_cantain' style='color:#585858; width: 100px;'>Tracking Number  :</td></tr>");
                            // messageBody.Append("<td class='popup_cantain' style='color:#585858;width: 311px;'>" + TrackNo.ToString() + "&nbsp;&nbsp;<a  href=\"" + url + "\">Click Here to Track Your Package</a></td></tr>");
                            //messageBody.Append("<tr>");
                            //messageBody.Append("<td class='popup_cantain' style='color:#585858; width: 100px;'>Number of Packages  :</td></tr>");
                            //messageBody.Append("<td class='popup_cantain' style='color:#585858;width: 311px;'>" + LableNo.ToString() + " of " + pckcount + "</td></tr>");
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

                        lblmailmsg.Text = "Mail Sent Successfully..";
                        CommonComponent.ExecuteCommonData("Update tb_order set isMailSent =1 where Ordernumber =" + OrderNumber);
                        if (lblmailmsg.Text != "")
                        {
                            trMessage.Visible = true;
                        }
                        BindShippingGrid(Convert.ToInt32(OrderNumber), 0);
                    }
                    else
                    {
                        //lblMsg.Text = "Error while details of order..";
                        return;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@ValidMsg", "alert('Please Update Tracking Number and Shipped On Date..');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                //lblMsg.Text = ex.Message;
                // CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->ShippingUpdate_Click() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
            finally
            {
                //  BindShippingGrid(Convert.ToInt32(OrderNumber), 0);
            }
        }

        /// <summary>
        /// Fill Manual Shipping Log
        /// </summary>
        private void FillmanualShipppingLog()
        {
            string OrderNumber = string.Empty;
            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
            }

            DataSet dsProducts = new DataSet();
            string sqlquery = "SELECT dbo.tb_Timestamplog.CreatedOn, dbo.tb_Timestamplog.type, dbo.tb_Timestamplog.refnumber, "
                           + "dbo.tb_Timestamplog.orderNumber, dbo.tb_Admin.FirstName+' '+dbo.tb_Admin.LastName as name"
                           + " FROM  dbo.tb_Admin INNER JOIN  "
                           + "dbo.tb_Timestamplog ON dbo.tb_Admin.AdminID = dbo.tb_Timestamplog.Createdby WHERE   "
                           + " dbo.tb_Timestamplog.type in (12) AND dbo.tb_Timestamplog.orderNumber=" + OrderNumber + "";

            dsProducts = CommonComponent.GetCommonDataSet(sqlquery);
            string strTable = " <table cellpadding=\"5\" cellspacing=\"0\" width='600px'>";

            if (dsProducts.Tables[0].Rows.Count > 0)
            {
                ltmanualShipping.Visible = true;
                strTable += "<tr><td style='border-bottom:1px solid #eeeeee;height:30px;background-color:#f3f3f3' colspan='3'><strong>&nbsp;Manual Shipping Log</strong><td>";
                for (int i = 0; i < dsProducts.Tables[0].Rows.Count; i++)
                {
                    strTable += "<tr>";
                    strTable += "<td style='border-bottom:1px solid #eeeeee;border-left:1px solid #eeeeee'><b>Order Number</b> : " + dsProducts.Tables[0].Rows[i]["refnumber"].ToString() + "</td>";

                    if (dsProducts.Tables[0].Rows[i]["type"].ToString() == "3")
                    {
                        strTable += "<td style='border-bottom:1px solid #eeeeee'><b>Deleted by</b>: " + dsProducts.Tables[0].Rows[i]["Name"].ToString() + "</td>";
                        strTable += "<td style='bottom-border:1px solid #eeeeee;border-right:1px solid #eeeeee'><b>Deleted on</b>: " + dsProducts.Tables[0].Rows[i]["CreatedOn"].ToString() + "</td>";

                    }
                    else
                    {
                        strTable += "<td style='border-bottom:1px solid #eeeeee'><b>Created by</b>: " + dsProducts.Tables[0].Rows[i]["Name"].ToString() + "</td>";
                        strTable += "<td style='border-bottom:1px solid #eeeeee;border-right:1px solid #eeeeee'><b>Created on</b>: " + dsProducts.Tables[0].Rows[i]["CreatedOn"].ToString() + "</td>";
                    }
                    strTable += "</tr>";
                }
            }
            else
            {
                ltmanualShipping.Visible = false;
            }
            strTable += "</table>";
            ltmanualShipping.Text = strTable;
        }

        /// <summary>
        /// Generate And Update Cart Having Shipping Status
        /// </summary>
        /// <param name="CartItem">CartItem</param>
        /// <param name="ProductIDs">ProductIDs</param>
        /// <param name="TrackingIDs">TrackingIDs</param>
        /// <param name="Couriers">Couriers</param>
        /// <param name="ShippedOn">ShippedOn</param>
        /// <param name="ShippedQty">ShippedQty</param>
        /// <param name="ShippedNote">ShippedNote</param>
        /// <param name="CustomCartID">CustomCartID</param>
        /// <returns>Result</returns>
        public string GenerateAndUpdateCartHavingShippingStatus(DataRowCollection CartItem, ArrayList ProductIDs, ArrayList TrackingIDs, ArrayList Couriers, ArrayList ShippedOn, ArrayList ShippedQty, ArrayList ShippedNote, ArrayList CustomCartID, ArrayList WarehouseId, ArrayList OldShippedQty, ArrayList OldWarehouseId, ref string trackinglink)
        {
            StringBuilder Table = new StringBuilder();
            string OrderNumber = string.Empty;
            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
            }

            bool GenerateAll = false;
            if (ProductIDs.Count == 0)
                GenerateAll = true;
            if (!GenerateAll)
            {
                MarkProductsShippedForRe(Convert.ToInt32(OrderNumber), ProductIDs, TrackingIDs, Couriers, ShippedOn, ShippedQty, ShippedNote, CustomCartID, WarehouseId);
            }
            else
            {
                //ObjOrder.MarkOrderAsShipped(OrderNumber, TrackingIDs[0].ToString(), Couriers[0].ToString(), ShippedOn[0].ToString());
            }
            try
            {
                if (CartItem != null && CartItem.Count > 0)
                {
                    int TotalProducts = CartItem.Count;
                    int ShippedProducts = 0;

                    Table.Append(" <table  cellpadding='0' cellspacing='0' width='100%' style='margin-top: 5px;padding-right: 0px'> ");
                    Table.Append(" <tr><td>");
                    Table.Append(" <table  cellpadding='0' cellspacing='0' class='table-noneforOrder' width='100%'> ");
                    Table.Append("<tbody><tr style='BACKGROUND-COLOR: rgb(242,242,242); ' >");
                    Table.Append("<th align='left' valign='middle' style='width:30%' >Product</th>");
                    Table.Append("<th align='left' valign='middle' style='width:10%' > SKU</th>");
                    //Table.Append("<th valign='middle' style='width: 10%;text-align:center;'>Quantity</th>");
                    Table.Append("<th valign='middle' style='width: 15%;text-align:center;'>Courier Name</th>");
                    Table.Append("<th valign='middle' style='width: 10%;text-align:center;'>Shipped On</th>");
                    Table.Append("<th valign='middle' style='width: 15%;text-align:center;'>Tracking Number</th>");

                    Table.Append("<th valign='middle' style='width: 10%;text-align:center;'>Status</th>");
                    Table.Append("</tr>");

                    for (int i = 0; i < CartItem.Count; i++)
                    {
                        //if (Convert.ToBoolean(CartItem[i]["ShippedProduct"]))
                        //   ShippedProducts++;
                        if ((ProductIDs.Contains(CartItem[i]["RefProductID"].ToString()) || GenerateAll) && !Convert.ToBoolean(CartItem[i]["ShippedProduct"]))
                        {
                            int Index = ProductIDs.IndexOf(CartItem[i]["RefProductID"].ToString());
                            if (GenerateAll)
                                Index = 0;
                            Table.Append("<tr align='center'  valign='middle'>");
                            Table.Append("<tr >");
                            Table.Append("<td align='left' valign='top'>" + CartItem[i]["Name"].ToString());

                            string[] Names = CartItem[i]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] Values = CartItem[i]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            int iLoopValues = 0;
                            for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                            {
                                Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
                            }
                            //if (iLoopValues == 0)****
                            //{
                            //    if (CartItem[i]["Color"].ToString().Trim().Length > 0)
                            //        Table.Append("<br/>Color: " + CartItem[i]["Color"].ToString());
                            //    if (CartItem[i]["Size"].ToString().Trim().Length > 0)
                            //        Table.Append("<br/>Size: " + CartItem[i]["Size"].ToString());
                            //}
                            Table.Append("</td>");
                            Table.Append("<td  align='left' >" + CartItem[i]["SKU"].ToString() + "</td>");
                            //Table.Append("<td STYLE='text-align : center;'>" + CartItem[i]["Quantity"].ToString() + "</td>");
                            //Table.Append("<td STYLE='text-align : center;'>" + ShippedQty[Index].ToString() + "</td>");
                            if (string.IsNullOrEmpty(trackinglink))
                            {
                                trackinglink = SetCourierLinkWithtracking(Couriers[Index].ToString(), TrackingIDs[Index].ToString());
                            }
                            Table.Append("<td  align='center'  style='font-size:10pt;'>" + SetCourierLink(Couriers[Index].ToString(), TrackingIDs[Index].ToString()) + "</td>");
                            Table.Append("<td align='center'>" + Convert.ToDateTime(ShippedOn[Index]).ToShortDateString() + "</td>");

                            Table.Append("<td align='center' >" + TrackingIDs[Index].ToString() + "</td>");



                            Table.Append("<td  style='text-align : right;'>Shipped</td>");

                            Table.Append(" </tr>");
                            ShippedProducts++;
                        }
                        else
                        {
                            if ((ProductIDs.Contains(CartItem[i]["RefProductID"].ToString()) || GenerateAll))// && !Convert.ToBoolean(CartItem[i]["ShippedProduct"]))
                            {
                                int Index = ProductIDs.IndexOf(CartItem[i]["RefProductID"].ToString());
                                if (GenerateAll)
                                    Index = 0;
                                Table.Append("<tr align='center'  valign='middle'>");
                                Table.Append("<tr >");
                                Table.Append("<td align='left' valign='top'>" + CartItem[i]["Name"].ToString());

                                string[] Names = CartItem[i]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string[] Values = CartItem[i]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                int iLoopValues = 0;
                                for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                                {
                                    Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
                                }
                                //if (iLoopValues == 0)****
                                //{
                                //    if (CartItem[i]["Color"].ToString().Trim().Length > 0)
                                //        Table.Append("<br/>Color: " + CartItem[i]["Color"].ToString());
                                //    if (CartItem[i]["Size"].ToString().Trim().Length > 0)
                                //        Table.Append("<br/>Size: " + CartItem[i]["Size"].ToString());
                                //}
                                Table.Append("</td>");
                                Table.Append("<td  align='left' >" + CartItem[i]["SKU"].ToString() + "</td>");
                                //Table.Append("<td STYLE='text-align : center;'>" + CartItem[i]["Quantity"].ToString() + "</td>");
                                //Table.Append("<td STYLE='text-align : center;'>" + ShippedQty[Index].ToString() + "</td>");
                                if (string.IsNullOrEmpty(trackinglink))
                                {
                                    trackinglink = SetCourierLinkWithtracking(Couriers[Index].ToString(), TrackingIDs[Index].ToString());
                                }
                                Table.Append("<td  align='center'  style='font-size:10pt;'>" + SetCourierLink(Couriers[Index].ToString(), TrackingIDs[Index].ToString()) + "</td>");
                                Table.Append("<td align='center'>" + Convert.ToDateTime(ShippedOn[Index]).ToShortDateString() + "</td>");
                                Table.Append("<td align='center'  >" + TrackingIDs[Index].ToString() + "</td>");
                                Table.Append("<td  style='text-align : right;'>Shipped</td>");

                                Table.Append(" </tr>");
                                ShippedProducts++;
                            }
                        }

                        if (ProductIDs.Contains(CartItem[i]["RefProductID"].ToString()))
                        {
                            bool TotInvUpdate = Convert.ToBoolean(CommonComponent.GetScalarCommonData("select ISNULL(InventoryUpdated,0) as InventoryUpdated from tb_orderedShoppingcartitems where OrderedCustomCartID in (" + CartItem[i]["OrderedCustomCartID"].ToString() + ")"));
                            if (!TotInvUpdate)
                            {
                                int IndexId = ProductIDs.IndexOf(CartItem[i]["RefProductID"].ToString());
                                int Qty = Convert.ToInt32(ShippedQty[IndexId].ToString());

                                Int32 OldShippedQuanity = Convert.ToInt32(OldShippedQty[IndexId].ToString());
                                Int32 OldWarehouId = Convert.ToInt32(OldWarehouseId[IndexId].ToString());

                                if (OldShippedQuanity > 0)
                                {
                                    // Qty = Qty - OldShippedQuanity;
                                }
                                //Qty = Qty + OldShippedQuanity;

                                int Inventory = 0;
                                Inventory = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 isnull(Inventory,0) from tb_WareHouseProductInventory  where ProductID='" + CartItem[i]["RefProductID"].ToString() + "' and WareHouseID=" + WarehouseId[IndexId].ToString() + " "));
                                if (Inventory != null && Inventory > 0)
                                {
                                    //if (OldShippedQuanity > 0 && OldWarehouId > 0)
                                    //{
                                    //    CommonComponent.ExecuteCommonData("Update tb_WareHouseProductInventory set Inventory=Inventory+" + OldShippedQuanity + " where ProductID='" + CartItem[i]["RefProductID"].ToString() + "'  and WareHouseID=" + OldWarehouId.ToString() + " ");
                                    //    CommonComponent.ExecuteCommonData("UPDATE dbo.tb_Product SET Inventory=Inventory+" + OldShippedQuanity + " WHERE ProductID='" + CartItem[i]["RefProductID"].ToString() + "'");
                                    //}

                                    //Inventory = Inventory - Qty;
                                    //CommonComponent.ExecuteCommonData("Update tb_WareHouseProductInventory set Inventory='" + Inventory + "' where ProductID='" + CartItem[i]["RefProductID"].ToString() + "'  and WareHouseID=" + WarehouseId[IndexId].ToString() + " ");

                                    //Object ObjPinventory = CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM dbo.tb_Product WHERE ProductID='" + CartItem[i]["RefProductID"].ToString() + "'");
                                    //int Pinventory = 0;
                                    //int.TryParse(Convert.ToString(ObjPinventory), out Pinventory);
                                    //Pinventory = Pinventory - Qty;
                                    //CommonComponent.ExecuteCommonData("UPDATE dbo.tb_Product SET Inventory='" + Pinventory + "' WHERE ProductID='" + CartItem[i]["RefProductID"].ToString() + "'");

                                    //// user this last if qty and shipped qty is same
                                    //Int32 QtyTotal = Convert.ToInt32(CommonComponent.GetScalarCommonData("select (ISNULL(Quantity,0)-ISNULL(ShippedQty,0)) as TotQty from tb_orderedShoppingcartitems where OrderedCustomCartID in (" + CartItem[i]["OrderedCustomCartID"].ToString() + ") and RefProductID=" + CartItem[i]["RefProductID"].ToString() + ""));
                                    //if (QtyTotal == 0)
                                    //    CommonComponent.ExecuteCommonData("Update tb_orderedShoppingcartitems set InventoryUpdated= 1 where RefProductID='" + CartItem[i]["RefProductID"].ToString() + "' and OrderedCustomCartID='" + CartItem[i]["OrderedCustomCartID"].ToString() + "'");
                                    //else
                                    //    CommonComponent.ExecuteCommonData("Update tb_orderedShoppingcartitems set InventoryUpdated= 0 where RefProductID='" + CartItem[i]["RefProductID"].ToString() + "' and OrderedCustomCartID='" + CartItem[i]["OrderedCustomCartID"].ToString() + "'");
                                }
                            }
                        }
                    }
                    if (TotalProducts == ShippedProducts)
                    {
                        CommonComponent.ExecuteCommonData("Update tb_Order set OrderStatus='Shipped', ShippingTrackingNumber='" + TrackingIDs[0].ToString() + "', ShippedVIA='" + Couriers[0].ToString() + "', ShippedOn='" + (DateTime)ShippedOn[0] + "' where OrderNumber=" + OrderNumber.ToString() + "");
                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("Update tb_Order set OrderStatus='Partially Shipped' where OrderNumber=" + OrderNumber.ToString() + "");
                    }
                    Table.Append("</tbody></table>");
                    Table.Append("</td> </tr></table>");
                }
            }
            catch { Table = null; }
            return Table.ToString();

            #region old code
            //if (CartItem != null && CartItem.Count > 0)
            //{
            //    int TotalProducts = CartItem.Count;
            //    int ShippedProducts = 0;


            //    Table.Append("  <table border='0' cellpadding='0' cellspacing='0' class='datatable' width='100%'> ");
            //    Table.Append("<tbody><tr style='line-height: 40px; BACKGROUND-COLOR: rgb(242,242,242); ' >");
            //    Table.Append("<th align='left' valign='middle' style='width:30%;font-size:10pt;' ><b>Product</b></th>");
            //    Table.Append("<th align='left' valign='middle' style='width:15%;font-size:10pt;' ><b> Style</b></th>");
            //    Table.Append("<th align='center' valign='middle' style='width:10%;font-size:10pt;'><b>Quantity</b></th>");
            //    Table.Append("<th align='left' valign='middle' style='width:15%;font-size:10pt;' ><b> CourierName</b></th>");
            //    Table.Append("<th align='center' valign='middle' style='width:10%;font-size:10pt;'><b>ShippedOn</b></th>");
            //    Table.Append("<th align='center' valign='middle' style='width:15%;font-size:10pt;'><b>TrackingNumber</b></th>");
            //    Table.Append("<th style='width:10%;font-size:10pt;'><b>Status:</b></th>");
            //    Table.Append("</tr>");
            //    for (int i = 0; i < CartItem.Count; i++)
            //    {
            //        if (Convert.ToBoolean(CartItem[i]["ShippedProduct"]))
            //            ShippedProducts++;
            //        if (ProductIDs.Contains(CartItem[i]["ProductID"].ToString()) || GenerateAll)
            //        {
            //            int Index = ProductIDs.IndexOf(CartItem[i]["ProductID"].ToString());
            //            if (GenerateAll)
            //                Index = 0;
            //            Table.Append("<tr align='center'  valign='middle'>");
            //            Table.Append("<td align='left' valign='top' style='font-size:10pt;'>" + CartItem[i]["Name"]);
            //            string[] Names = CartItem[i]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //            string[] Values = CartItem[i]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //            int iLoopValues = 0;
            //            for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
            //            {
            //                Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
            //            }
            //            if (iLoopValues == 0)
            //            {
            //                if (CartItem[i]["ChosenColor"].ToString().Trim().Length > 0)
            //                    Table.Append("<br/>Color: " + CartItem[i]["ChosenColor"].ToString());
            //                if (CartItem[i]["ChosenSize"].ToString().Trim().Length > 0)
            //                    Table.Append("<br/>Size: " + CartItem[i]["ChosenSize"].ToString());
            //            }
            //            Table.Append("</td>");
            //            Table.Append("<td  align='left'  style='font-size:10pt;'>" + CartItem[i]["SKU"] + "</td>");
            //            //Table.Append("<td align='center'>" + CartItem[i]["Quantity"] + "</td>");
            //            Table.Append("<td align='center'>" + ShippedQty[Index].ToString() + "</td>");

            //            //* Table.Append("<td  align='left'  style='font-size:10pt;'>" + SetCourierLink(Couriers[Index].ToString(), TrackingIDs[Index].ToString()) + "</td>");
            //            Table.Append("<td align='center'>" + Convert.ToDateTime(ShippedOn[Index]).ToShortDateString() + "</td>");
            //            Table.Append("<td align='center' style='font-size:10pt;' >" + TrackingIDs[Index].ToString() + "</td>");
            //            Table.Append("<td  style='text-align : right;font-size:10pt;'>Shipped</td>");
            //            Table.Append(" </tr>");
            //            ShippedProducts++;
            //        }
            //    } 
            #endregion
        }

        /// <summary>
        /// Mark Products Shipped For
        /// </summary>
        /// <param name="OrderNumber">OrderNumber</param>
        /// <param name="ProductIds">ProductIds</param>
        /// <param name="TrackingNumber">TrackingNumber</param>
        /// <param name="CourierName">CourierName</param>
        /// <param name="ShippedDateList">ShippedDateList</param>
        /// <param name="ShippedQty">ShippedQty</param>
        /// <param name="ShippedNote">ShippedNote</param>
        /// <param name="CustomCartID">CustomCartID</param>
        /// <returns>True if Shipped Successfully else False</returns>
        private bool MarkProductsShippedForRe(int OrderNumber, ArrayList ProductIds, ArrayList TrackingNumber, ArrayList CourierName, ArrayList ShippedDateList, ArrayList ShippedQty, ArrayList ShippedNote, ArrayList CustomCartID, ArrayList WarehouseId)
        {
            string Query = string.Empty;
            //SQLAccess dbAccess = new SQLAccess();
            for (int iLoopIds = 0; iLoopIds < ProductIds.Count; iLoopIds++)
            {
                Query += " if exists (select 1 from tb_OrderShippedItems where OrderNumber=" + OrderNumber + " and RefProductID=" + ProductIds[iLoopIds]
                    + " and OrderedCustomCartID=" + CustomCartID[iLoopIds] + ") begin if exists (select 1 from tb_OrderShippedItems where OrderNumber=" + OrderNumber + " and RefProductID=" + ProductIds[iLoopIds] + " and Shipped=1 and OrderedCustomCartID=" + CustomCartID[iLoopIds] + ") begin update tb_OrderShippedItems set ShippedQty=" + ShippedQty[iLoopIds] + ",ShippedOn='" + ShippedDateList[iLoopIds] + "', TrackingNumber='" + TrackingNumber[iLoopIds].ToString().Replace("'", "''") + "',shipped=1,ShippedNote='" + ShippedNote[iLoopIds].ToString().Replace("'", "''") + "',ShippedVia='" + CourierName[iLoopIds]
                    + "' where OrderNumber=" + OrderNumber + " and RefProductID=" + ProductIds[iLoopIds]
                    + " and OrderedCustomCartID=" + CustomCartID[iLoopIds] + " end else begin update tb_OrderShippedItems set ShippedQty=isnull(ShippedQty,0)+" + ShippedQty[iLoopIds] + ",shipped=1,ShippedOn='" + ShippedDateList[iLoopIds] + "', TrackingNumber='" + TrackingNumber[iLoopIds].ToString().Replace("'", "''") + "',ShippedVia='" + CourierName[iLoopIds] + "',ShippedNote='" + ShippedNote[iLoopIds].ToString().Replace("'", "''") + "' where OrderNumber=" + OrderNumber + " and RefProductID=" + ProductIds[iLoopIds] + " and OrderedCustomCartID=" + CustomCartID[iLoopIds] + " end end else begin insert into tb_OrderShippedItems(OrderNumber,RefProductID,TrackingNumber,ShippedVia,Shipped,ShippedOn,ShippedQty,ShippedNote,OrderedCustomCartID) values(" + OrderNumber + "," + ProductIds[iLoopIds]
                    + ",'" + TrackingNumber[iLoopIds].ToString().Replace("'", "''") + "','" + CourierName[iLoopIds] + "',1,'" + ShippedDateList[iLoopIds] + "'," + ShippedQty[iLoopIds] + ",'" + ShippedNote[iLoopIds].ToString().Replace("'", "''") + "'," + CustomCartID[iLoopIds] + ") end"

                    + " if exists(select 1 from tb_OrderedShoppingCartItems where OrderedCustomCartID='" + CustomCartID[iLoopIds] + "'"
                    + ") begin update tb_OrderedShoppingCartItems set TrackingNumber='" + TrackingNumber[iLoopIds].ToString().Replace("'", "''") + "',ShippedVia='" + CourierName[iLoopIds] + "',ShippedQty='" + ShippedQty[iLoopIds] + "',ShippedOn='" + ShippedDateList[iLoopIds] + "',WareHouseID='" + WarehouseId[iLoopIds] + "', IsManualShipped=1 where OrderedCustomCartID=" + CustomCartID[iLoopIds] + " "
                    + "  end";
                //update tb_order set ShippingTrackingNumber='" + TrackingNumber[iLoopIds].ToString().Replace("'", "''") + "' where ordernumber=" + OrderNumber + " 
                //    +" if exists (select 1 from tb_ecomm_LockProducts where OrderNumber=" + OrderNumber + " and ProductID=" + ProductIds[iLoopIds] + " and ordercustomcartId=" + CustomCartID[iLoopIds] + " and IsCompleted=1"
                //   + ") begin update tb_ecomm_LockProducts set IsCompleted=1,Quantity=" + ShippedQty[iLoopIds] + ",MarkQuantity=" + ShippedQty[iLoopIds]
                //   + " where OrderNumber=" + OrderNumber + " and ProductID=" + ProductIds[iLoopIds] + " and ordercustomcartId=" + CustomCartID[iLoopIds] + " and IsCompleted=0"
                //   + " end else begin  insert into tb_ecomm_LockProducts(OrderNumber,ProductID,Quantity,IsCompleted,MarkQuantity,ordercustomcartId) values (" + OrderNumber + "," + ProductIds[iLoopIds] + "," + ShippedQty[iLoopIds] + ",1," + ShippedQty[iLoopIds] + "," + CustomCartID[iLoopIds] + ")end";
                //Query += "Update tb_OrderedShoppingCartItems set TrackingNumber='" + TrackingNumber[iLoopIds] + "',ShippedVia='" + CourierName[iLoopIds] + "',ShippedQty='" + ShippedQty[iLoopIds] + "',ShippedOn='" + ShippedDateList[iLoopIds] + "' where OrderedCustomCartID='" + CustomCartID[iLoopIds] + "' ";
            }
            if (!string.IsNullOrEmpty(Query))
            {
                //return Convert.ToBoolean(dbAccess.ExecuteNonQuery(Query));
                CommonComponent.ExecuteCommonData(Query);
            }
            return false;
        }

        /// <summary>
        /// Set Courier Link
        /// </summary>
        /// <param name="CourierName">CourierName</param>
        /// <param name="strackingnumber">strackingnumber</param>
        /// <returns>Courier Link</returns>
        private string SetCourierLink(string CourierName, string strackingnumber)
        {
            string Link = string.Empty;
            switch (CourierName.ToLowerInvariant())
            {
                //case "ups":
                //    {
                //        //if (AppLogic.AppConfig("UPSTrackingLink") != "")
                //        //    Link = "<a href='" + AppLogic.AppConfig("UPSTrackLink") + "'>" + CourierName + "</a>";
                //        //else
                //        Link = "<a href='http://wwwapps.ups.com/WebTracking/processInputRequest?sort_by=status&tracknums_displayed=1&TypeOfInquiryNumber=T&loc=en_US&InquiryNumber1=" + strackingnumber + "&track.x=0&track.y=0'>" + CourierName + "</a>";
                //        break;
                //    }
                //case "usps":
                //    {
                //        //if (AppLogic.AppConfig("USPSTrackingLink") != "")
                //        //    Link = "<a href='" + AppLogic.AppConfig("USPSTrackingLink") + "'>" + CourierName + "</a>";
                //        //else
                //        Link = "<a href='http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?origTrackNum=" + strackingnumber + "'>" + CourierName + "</a>";
                //        break;
                //    }

                //case "usps":
                //    {
                //        //if (AppLogic.AppConfig("USPSTrackingLink") != "")
                //        //    Link = "<a href='" + AppLogic.AppConfig("USPSTrackingLink") + "'>" + CourierName + "</a>";
                //        //else
                //        Link = "<a href='http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?origTrackNum=" + strackingnumber + "'>" + CourierName + "</a>";
                //        break;
                //    }


                case "ups":
                    {
                        //if (AppLogic.AppConfig("UPSTrackingLink") != "")
                        //    Link = "<a href='" + AppLogic.AppConfig("UPSTrackLink") + "'>" + CourierName + "</a>";
                        //else
                        Link = "<a href='http://wwwapps.ups.com/WebTracking/processInputRequest?sort_by=status&tracknums_displayed=1&TypeOfInquiryNumber=T&loc=en_US&InquiryNumber1=" + strackingnumber + "&track.x=0&track.y=0'>" + CourierName + "</a>";
                        break;
                    }
                case "usps":
                    {
                        //if (AppLogic.AppConfig("USPSTrackingLink") != "")
                        //    Link = "<a href='" + AppLogic.AppConfig("USPSTrackingLink") + "'>" + CourierName + "</a>";
                        //else
                        Link = "<a href='http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?origTrackNum=" + strackingnumber + "'>" + CourierName + "</a>";
                        break;
                    }
                case "fedex":
                    {
                        //if (AppLogic.AppConfig("FedExTrackingLink") != "")
                        //    Link = "<a href='" + AppLogic.AppConfig("FedExTrackingLink") + "'>" + CourierName + "</a>";
                        //else
                        Link = "<a href='http://www.fedex.com/Tracking?action=track&language=english&cntry_code=us&initial=x&tracknumbers=" + strackingnumber + "'>" + CourierName + "</a>";
                        break;
                    }
                case "freight":
                    {
                        //Link = "<a href='javascript:void(0);'>" + CourierName + "</a>";
                        Link = "<span style=\"color:#F2570A;\">" + CourierName + "</span>";
                        break;
                    }
                case "other":
                    {
                        //Link = "<a href='javascript:void(0);'>" + CourierName + "</a>";
                        Link = "<span style=\"color:#F2570A;\">" + CourierName + "</span>";
                        break;
                    }
            }
            return Link;
        }
        private string SetCourierLinkWithtracking(string CourierName, string strackingnumber)
        {
            string Link = string.Empty;
            switch (CourierName.ToLowerInvariant())
            {
                //case "ups":
                //    {
                //        //if (AppLogic.AppConfig("UPSTrackingLink") != "")
                //        //    Link = "<a href='" + AppLogic.AppConfig("UPSTrackLink") + "'>" + CourierName + "</a>";
                //        //else
                //        Link = "<a href='http://wwwapps.ups.com/WebTracking/processInputRequest?sort_by=status&tracknums_displayed=1&TypeOfInquiryNumber=T&loc=en_US&InquiryNumber1=" + strackingnumber + "&track.x=0&track.y=0'>" + CourierName + "</a>";
                //        break;
                //    }
                //case "usps":
                //    {
                //        //if (AppLogic.AppConfig("USPSTrackingLink") != "")
                //        //    Link = "<a href='" + AppLogic.AppConfig("USPSTrackingLink") + "'>" + CourierName + "</a>";
                //        //else
                //        Link = "<a href='http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?origTrackNum=" + strackingnumber + "'>" + CourierName + "</a>";
                //        break;
                //    }

                //case "usps":
                //    {
                //        //if (AppLogic.AppConfig("USPSTrackingLink") != "")
                //        //    Link = "<a href='" + AppLogic.AppConfig("USPSTrackingLink") + "'>" + CourierName + "</a>";
                //        //else
                //        Link = "<a href='http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?origTrackNum=" + strackingnumber + "'>" + CourierName + "</a>";
                //        break;
                //    }


                case "ups":
                    {
                        //if (AppLogic.AppConfig("UPSTrackingLink") != "")
                        //    Link = "<a href='" + AppLogic.AppConfig("UPSTrackLink") + "'>" + CourierName + "</a>";
                        //else
                        Link = "<a href='http://wwwapps.ups.com/WebTracking/processInputRequest?sort_by=status&tracknums_displayed=1&TypeOfInquiryNumber=T&loc=en_US&InquiryNumber1=" + strackingnumber + "&track.x=0&track.y=0' style='color:#b92127 !important;'>" + CourierName + ": " + strackingnumber + "</a>";
                        break;
                    }
                case "usps":
                    {
                        //if (AppLogic.AppConfig("USPSTrackingLink") != "")
                        //    Link = "<a href='" + AppLogic.AppConfig("USPSTrackingLink") + "'>" + CourierName + "</a>";
                        //else
                        Link = "<a href='http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?origTrackNum=" + strackingnumber + "' style='color:#b92127 !important;'>" + CourierName + ": " + strackingnumber + "</a>";
                        break;
                    }
                case "fedex":
                    {
                        //if (AppLogic.AppConfig("FedExTrackingLink") != "")
                        //    Link = "<a href='" + AppLogic.AppConfig("FedExTrackingLink") + "'>" + CourierName + "</a>";
                        //else
                        Link = "<a href='http://www.fedex.com/Tracking?action=track&language=english&cntry_code=us&initial=x&tracknumbers=" + strackingnumber + "' style='color:#b92127 !important;'>" + CourierName + ": " + strackingnumber + "</a>";
                        break;
                    }
                case "freight":
                    {
                        //Link = "<a href='javascript:void(0);'>" + CourierName + "</a>";
                        Link = "<span style=\"color:#F2570A;\">" + CourierName + ": " + strackingnumber + "</span>";
                        break;
                    }
                case "other":
                    {
                        //Link = "<a href='javascript:void(0);'>" + CourierName + "</a>";
                        Link = "<span style=\"color:#F2570A;\">" + CourierName + ": " + strackingnumber + "</span>";
                        break;
                    }
            }
            return Link;
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
            //ddlWareHouse.Items.Insert(0, new ListItem("Select Warehouse", "0"));
        }

        protected void ddlWareHouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            string OrderNumber = "0";
            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                BindData(Convert.ToInt32(OrderNumber), 1);
            }
        }

        private void GetConfirmShipmentOverStock(Int32 StoreId, Int32 OrderNumber, string Reforderid)
        {

            Reforderid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(RefSalesChannel,RefOrderId) FROM tb_Order WHERE  OrderNumber=" + OrderNumber + ""));
            ServicePointManager.ServerCertificateValidationCallback = new
            RemoteCertificateValidationCallback
(
delegate { return true; }
);

            Solution.Data.SQLAccess objdb = new Solution.Data.SQLAccess();
            string OverstockUserName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='OverstockUserName' AND Storeid=" + StoreId + ""));
            string OverstockPassword = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='OverstockPassword' AND Storeid=" + StoreId.ToString() + ""));
           // String AuthServer = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=" + StoreId.ToString() + ""));
            string shippingmethodname = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(ShippingMethod,'')  from tb_Order where ordernumber=" + OrderNumber + ""));

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            transactionCommand.Append("<supplierShipmentMessage xmlns=\"api.supplieroasis.com\">");
            DataSet DsCItems = new DataSet();
            DsCItems = CommonComponent.GetCommonDataSet("SELECT Isnull(tb_OrderedShoppingCartItems.Inventoryupdated,0) as Inventoryupdated,isnull(tb_OrderedShoppingCartItems.linenumber,0) as linenumber, (isnull(tb_OrderedShoppingCartItems.WareHouseID,0)) as WareHouseID,case when isnull(tb_OrderedShoppingCartItems.ShippedQty,0) =0 then isnull(s.ShippedQty,0) else isnull(tb_OrderedShoppingCartItems.ShippedQty,0) end as ShippedQty,tb_Product.Name,tb_Product.SKU, tb_OrderedShoppingCartItems.Quantity,"
                                                        + "    tb_OrderedShoppingCartItems.OrderedCustomCartID, tb_OrderedShoppingCartItems.RefProductID,"
                                                        + "    tb_OrderedShoppingCartItems.VariantNames,tb_OrderedShoppingCartItems.VariantValues,"
                                                        + "    (isnull(s.TrackingNumber,tb_OrderedShoppingCartItems.TrackingNumber)) TrackingNumber,isnull(s.ShippedVia,tb_OrderedShoppingCartItems.ShippedVia) ShippedVia,"
                                                        + "   (isnull(s.Shipped,0)) as Shipped,isnull(s.Shipped,0) as ShippedProduct,"
                                                        + "    isnull(s.ShippedOn,tb_OrderedShoppingCartItems.ShippedOn) ShippedOn,  tb_Product.Description,tb_OrderedShoppingCartItems.Price As "
                                                        + "    SalePrice, isnull(s.ShippedNote,'') as ShippedNote,isnull(tb_OrderedShoppingCartItems.OrderItemID,'') as OrderItemID"
                                                        + "    FROM tb_Product INNER JOIN tb_OrderedShoppingCartItems"
                                                        + "    left outer join (select RefProductID,OrderNumber,trackingNumber,ShippedVia,Shipped,ShippedOn,ShippedQty,"
                                                        + "    ShippedNote from  tb_OrderShippedItems where OrderNumber=" + OrderNumber + ") s on s.RefProductID=tb_OrderedShoppingCartItems.RefProductID  "
                                                        + "    inner join (select convert(bit,isnull(len(ShippedOn),0)) as shipped,ShoppingCardID,ShippingTrackingNumber"
                                                        + "    as TrackingNumber,ShippedVia,ShippedOn from tb_Order ) o ON  o.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID "
                                                        + "    on tb_Product.ProductID = tb_OrderedShoppingCartItems.RefProductID Where OrderedShoppingCartID = (select ShoppingCardID FROM  tb_Order where OrderNumber=" + OrderNumber + ")");
            if (DsCItems != null && DsCItems.Tables.Count > 0 && DsCItems.Tables[0].Rows.Count > 0)
            {
                bool chkisShipp = false;
                for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(DsCItems.Tables[0].Rows[i]["TrackingNumber"].ToString()) && !string.IsNullOrEmpty(DsCItems.Tables[0].Rows[i]["ShippedOn"].ToString()) && !string.IsNullOrEmpty(DsCItems.Tables[0].Rows[i]["ShippedVia"].ToString()))
                    {
                        chkisShipp = true;
                        //transactionCommand.Append("<InvoiceLine>");
                        //transactionCommand.Append("<Id>" + DsCItems.Tables[0].Rows[i]["OrderItemID"].ToString() + "</Id>");
                        //transactionCommand.Append("<Quantity>" + DsCItems.Tables[0].Rows[i]["ShippedQty"].ToString() + "</Quantity>");
                        //transactionCommand.Append("<Shipment>");
                        //if (DsCItems.Tables[0].Rows[i]["ShippedVia"].ToString().ToLower().Trim() == "fedex")
                        //{
                        //    transactionCommand.Append("<CarrierCode>FEDX</CarrierCode>");
                        //}
                        //else
                        //{
                        //    transactionCommand.Append("<CarrierCode>" + DsCItems.Tables[0].Rows[i]["ShippedVia"].ToString() + "</CarrierCode>");
                        //}
                        //transactionCommand.Append("<ShipDate>" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(DsCItems.Tables[0].Rows[i]["ShippedOn"].ToString())) + "</ShipDate>");
                        //transactionCommand.Append("<TrackingNumber>" + DsCItems.Tables[0].Rows[i]["TrackingNumber"].ToString() + "</TrackingNumber>");
                        //transactionCommand.Append("<PartnerInvoiceNumber>" + Reforderid + "</PartnerInvoiceNumber>");
                        //transactionCommand.Append("</Shipment>");
                        //transactionCommand.Append("</InvoiceLine>");

                        transactionCommand.Append("<supplierShipment>");
                        transactionCommand.Append("<salesChannelName>OSTK</salesChannelName>");
                        transactionCommand.Append("<salesChannelOrderNumber>" + Reforderid.ToString() + "</salesChannelOrderNumber>");
                        transactionCommand.Append("<salesChannelLineNumber>" + DsCItems.Tables[0].Rows[i]["linenumber"].ToString() + "</salesChannelLineNumber>");
                        transactionCommand.Append("<warehouse><code>Exclusive</code></warehouse>");
                        transactionCommand.Append("<supplierShipConfirmation><quantity>" + DsCItems.Tables[0].Rows[i]["ShippedQty"].ToString() + "</quantity><carrier><code>" + DsCItems.Tables[0].Rows[i]["ShippedVia"].ToString() + "</code></carrier>");

                        
                        transactionCommand.Append("<trackingNumber>" + DsCItems.Tables[0].Rows[i]["TrackingNumber"].ToString() + "</trackingNumber>");
                        transactionCommand.Append("<shipDate>" + string.Format("{0:yyyy-MM-ddThh:mm:ss}", Convert.ToDateTime(DsCItems.Tables[0].Rows[i]["ShippedOn"].ToString())) + "</shipDate>");
                        transactionCommand.Append("<serviceLevel><code>" + shippingmethodname.ToString() + "</code></serviceLevel></supplierShipConfirmation>");
                        transactionCommand.Append("</supplierShipment>");
                       
                    }
                }

                transactionCommand.Append("</supplierShipmentMessage>");
                if (chkisShipp == true)
                {
                    //System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
                    //byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());

                    ////String AuthServer = "https://sapiqa.overstock.com/api";
                    //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                    //myRequest.Method = "POST";
                    //myRequest.Timeout = 300000;
                    //myRequest.Headers.Add("SapiMethodName", "ConfirmShipment");
                    //// myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
                    //myRequest.ContentType = "application/xml";
                    //myRequest.ContentLength = data.Length;
                    //Stream newStream = myRequest.GetRequestStream();
                    //// Send the data.
                    //newStream.Write(data, 0, data.Length);
                    //newStream.Close();
                    //// get the response
                    //WebResponse myResponse;
                    //String rawResponseString = String.Empty;

                    System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
                    byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());

                    //String AuthServer = "https://sapiqa.overstock.com/api";
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://api.supplieroasis.com/shipments?jobName=");
                    myRequest.Method = "POST";

                    myRequest.Headers.Add("Authorization", string.Format("Basic {0}", GetAuthorization(OverstockUserName, OverstockPassword)));
                    myRequest.ContentType = "application/xml; charset=UTF-8";

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
                            if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                            {
                                Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                            }
                            ds.WriteXml(Server.MapPath("/OverstockOrder/" + Reforderid.ToString() + "-" + DateTime.Now.Ticks.ToString() + ".xml"));
                        }
                        catch { }
                        myResponse.Close();
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = new StreamWriter(Server.MapPath("/OverstockOrder/" + Reforderid.ToString() + "-" + DateTime.Now.ToString() + "_1.txt"));
                        sw.WriteLine(DateTime.Now.Date.ToString());
                        sw.WriteLine(ex.Message.ToString() + " " + ex.StackTrace.ToString());
                        sw.Close();
                        sw.Dispose();
                    }
                }

            }


        }
        private static string GetAuthorization(string User, string Password)
        {
            UTF8Encoding utf8encoder = new UTF8Encoding(false, true);

            return Convert.ToBase64String(utf8encoder.GetBytes(string.Format("{0}:{1}", User, Password)));
        }
        protected void btnGeneralUpdate_Click(object sender, ImageClickEventArgs e)
        {
            if (grdShipping.Rows.Count > 0)
            {
                string OrderNumber = string.Empty;
                if (Request.QueryString["ONo"] != null)
                {
                    OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                }

                foreach (GridViewRow dr in grdShipping.Rows)
                {
                    Label lblID = (Label)dr.FindControl("lblProductID");
                    Label lblCustomCartID1 = (Label)dr.FindControl("lblCustomCartID");

                    Label lblTracking = (Label)dr.FindControl("lblTrackingNumber");
                    // TextBox txtTracking = (TextBox)dr.FindControl("txtTrackingNumber");
                    // DropDownList ddlCourier = (DropDownList)dr.FindControl("ddlShippedVIA");

                    Label lblCustomCartID = (Label)dr.FindControl("lblCustomCartID");
                    Label lblShippedVia = (Label)dr.FindControl("lblShippedVia");
                    //  ImageButton btnEdit = (ImageButton)dr.FindControl("btnEdit");
                    //   ImageButton btnSave = (ImageButton)dr.FindControl("btnSave");
                    //  ImageButton btnCancel = (ImageButton)dr.FindControl("btnCancel");
                    CheckBox chkShipped = (CheckBox)dr.FindControl("chkShipped");
                    Label lblShippedOn = (Label)dr.FindControl("lblShippedOn");
                    //  TextBox txtShippedOn = (TextBox)dr.FindControl("txtShippedOn");
                    //  eWorld.UI.CalendarPopup txtShippedOn = (eWorld.UI.CalendarPopup)dr.FindControl("txtShippedOn2");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnshipped = (System.Web.UI.HtmlControls.HtmlInputHidden)dr.FindControl("hdnshipped");
                    System.Web.UI.HtmlControls.HtmlContainerControl divcalendor = (System.Web.UI.HtmlControls.HtmlContainerControl)dr.FindControl("divcalendor");
                    Label lblQty = (Label)dr.FindControl("lblQty");
                    Label lblShippedQty = (Label)dr.FindControl("lblShippedQty");
                      TextBox txtShippedQty = (TextBox)dr.FindControl("txtShippedQty");
                    // TextBox txtShippedOn = (TextBox)dr.FindControl("txtShippedOn2");
                    Label lblShippedNote = (Label)dr.FindControl("lblShippedNote");
                    //  TextBox txtShippedNote = (TextBox)dr.FindControl("txtShippedNote");
                    //  Label lblavailQuantity = (Label)dr.FindControl("lblavailQuantity");

                    Label lblOldQty = (Label)dr.FindControl("lblOldQty");
                    Label lblOldWarehouseId = (Label)dr.FindControl("lblOldWarehouseId");

                    Int32 maxqty = 0, shipqty = 0, qty = 0, AvailQuantity = 0;
                    Int32.TryParse(lblQty.Text, out maxqty);
                    //  Int32.TryParse(lblShippedQty.Text, out shipqty);
                    //  Int32.TryParse(txtShippedQty.Text, out qty);
                    //  Int32.TryParse(lblavailQuantity.Text, out AvailQuantity);


                    if (Convert.ToString(txtGeneralShippedOn.Text).IndexOf("1900") > -1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msgvalid0", "alert('Please enter valid date.');", true);
                        txtGeneralShippedOn.Focus();
                        return;
                    }

                    ArrayList ProductList = null;
                    ArrayList TrackingList = null;
                    ArrayList CourierList = null;
                    ArrayList ShippedDateList = null;
                    ArrayList ShippedQty = null;
                    ArrayList ShippedNote = null;
                    ArrayList CustomCartID = null;
                    ArrayList WarehouseId = null;

                    ArrayList OldShippedQty = null;
                    ArrayList OldWarehouseId = null;

                    if (Session["ProductIDs"] == null)
                    {
                        ProductList = new ArrayList();
                        TrackingList = new ArrayList();
                        CourierList = new ArrayList();
                        ShippedDateList = new ArrayList();
                        ShippedQty = new ArrayList();
                        ShippedNote = new ArrayList();
                        CustomCartID = new ArrayList();
                        WarehouseId = new ArrayList();
                        OldShippedQty = new ArrayList();
                        OldWarehouseId = new ArrayList();
                    }
                    else
                    {
                        ProductList = Session["ProductIDs"] as ArrayList;
                        TrackingList = Session["TrackingNumbers"] as ArrayList;
                        CourierList = Session["ShippedVia"] as ArrayList;
                        if (Session["ShippedOn"] != null)
                        {
                            ShippedDateList = Session["ShippedOn"] as ArrayList;
                        }
                        ShippedQty = Session["ShippedQty"] as ArrayList;
                        ShippedNote = Session["ShippedNote"] as ArrayList;
                        CustomCartID = Session["CustomCartID"] as ArrayList;
                        OldShippedQty = Session["OldShippedQty"] as ArrayList;
                        WarehouseId = Session["WarehouseId"] as ArrayList;
                        OldWarehouseId = Session["OldWarehouseId"] as ArrayList;

                        if (ProductList != null && ProductList.Contains(lblID.Text) && CustomCartID.Contains(lblCustomCartID.Text.Trim()))
                        {
                            int cnt = CustomCartID.IndexOf(lblCustomCartID.Text.Trim());
                            ProductList.RemoveAt(cnt);
                            TrackingList.RemoveAt(cnt);
                            CourierList.RemoveAt(cnt);
                            ShippedDateList.RemoveAt(cnt);

                            ShippedQty.RemoveAt(cnt);
                            CustomCartID.RemoveAt(cnt);
                            WarehouseId.RemoveAt(cnt);

                            OldShippedQty.RemoveAt(cnt);
                            OldWarehouseId.RemoveAt(cnt);
                            try
                            {
                                ShippedNote.RemoveAt(cnt);
                            }
                            catch
                            {
                            }
                        }
                    }


                    CustomCartID.Add(lblCustomCartID.Text.Trim());
                    Session["CustomCartID"] = CustomCartID;
                    //if(hdnshipped.Value.ToString().ToLower() == "true")
                    //{
                    //    ShippedQty.Add(Convert.ToInt32(0));
                    //}
                    //else
                    //{
                    ShippedQty.Add(Convert.ToInt32(lblQty.Text));
                    // }

                    Session["ShippedQty"] = ShippedQty;
                    lblShippedQty.Text = maxqty.ToString();
                    txtShippedQty.Text = maxqty.ToString();

                    OldShippedQty.Add(lblOldQty.Text);
                    Session["OldShippedQty"] = OldShippedQty;
                    OldWarehouseId.Add(lblOldWarehouseId.Text);
                    Session["OldWarehouseId"] = OldWarehouseId;

                    ProductList.Add(lblID.Text);
                    Session["ProductIDs"] = ProductList;
                    if (chkShipped.Checked)
                    {
                        TrackingList.Add(lblTracking.Text);
                    }
                    else
                    {
                        lblTracking.Text = txtGeneralTrackingNumber.Text;
                        TrackingList.Add(txtGeneralTrackingNumber.Text);
                    }
                    Session["TrackingNumbers"] = TrackingList;
                    if (chkShipped.Checked)
                    {
                        CourierList.Add(lblShippedVia.Text);
                    }
                    else
                    {
                        lblShippedVia.Text = ddlGeneralShippedVIA.SelectedValue;
                        CourierList.Add(ddlGeneralShippedVIA.SelectedValue);
                    }
                    Session["ShippedVia"] = CourierList;


                    if (chkShipped.Checked)
                    {
                        ShippedDateList.Add(Convert.ToDateTime(lblShippedOn.Text));
                    }
                    else
                    {
                        lblShippedOn.Text = txtGeneralShippedOn.Text;
                        ShippedDateList.Add(Convert.ToDateTime(txtGeneralShippedOn.Text));
                    }

                    Session["ShippedOn"] = ShippedDateList;
                    ShippedNote.Add(Convert.ToString(lblShippedNote.Text));
                    Session["ShippedNote"] = ShippedNote;

                    // assign value of warehouseid from here
                    WarehouseId.Add(Convert.ToString(ddlWareHouse.SelectedValue.ToString()));
                    Session["WarehouseId"] = WarehouseId;

                }

            }
        }

    }
}