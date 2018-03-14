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
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Solution.ShippingMethods;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ChangeRMAOrder : BasePage
    {
        public int OrderNumber;
        Int32 SCartID;
        Int32 OrderedShoppingCartID = 0, StoreID = 0;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["RMA"] != null)
                {
                    trNotes.Visible = true;
                }
                else
                {
                    trNotes.Visible = false;
                }
                Int32 OrderNumber = 0;
                Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"]), out OrderNumber);
                if (OrderNumber <= 0)
                {
                    litProducts.Text = "<span style='color:red;'>Sorry. The OrderNumber specified is wrong.</span>";
                    lnkAddNew.Visible = false;
                    return;
                }
                if (Request.QueryString["ONo"] != null)
                {
                    Int32 StoreID = 0;
                    OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
                    Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + OrderNumber)), out StoreID);
                    AppConfig.StoreID = StoreID;
                }
                if (Request.QueryString["ONo"] != null & Request.QueryString["RMA"] != null)
                {
                    OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
                    setShoppingcart(OrderNumber, Convert.ToInt32(Request.QueryString["ProductID"]), Convert.ToInt32(Request.QueryString["RMA"]));
                    //lnkOldOrders.Attributes.Add("onclick", "OpenCenterWindow('OldShoppingCart.aspx?ONo=" + OrderNumber + "',800,600);");
                    GetOrderDetailsByOrderNumber(Convert.ToInt32(OrderNumber));
                }
                else
                {
                    GVShoppingCartItems.DataSource = null;
                    GVShoppingCartItems.DataBind();
                }
                if (Request.QueryString["ONo"] != null)
                {
                    lblOrderNo.Text = Convert.ToString(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
                }
            }
        }

        /// <summary>
        /// Sets the Shopping Cart
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="PID">int PID</param>
        private void setShoppingcart(Int32 OrderNumber, Int32 PID, Int32 RMA)
        {
            Int32 OrderedCustomCartID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select OrderedCustomCartID from tb_ReturnItem where ReturnItemID=" + RMA + ""));
            HdnRMA.Value =Convert.ToString(RMA);
            string StrQ1 = "";
            DataSet dsShoppingCart = new DataSet();
            if (OrderedCustomCartID > 0)
            {
                StrQ1 = " and osi.OrderedCustomCartID=" + OrderedCustomCartID + "";
            }

            dsShoppingCart = CommonComponent.GetCommonDataSet(" select osi.RefProductID  as ProductId,osi.VariantNames,osi.Variantvalues,p.name,p.sku, " +
                                                                " osi.price as SalePrice,osi.Quantity from tb_orderedshoppingcartitems osi  " +
                                                                " inner join tb_order o on o.shoppingcardid=osi.orderedshoppingcartid   " +
                                                                " inner join tb_product p on p.productid=osi.RefProductID  " +
                                                                " where o.ordernumber =" + OrderNumber + " and osi.RefProductID =" + PID + " " + StrQ1.ToString() + "");
            Int32 Shoppingcartid = 0;
            HdnOrderNumber.Value = Convert.ToString(OrderNumber);
            HdnProductID.Value = Convert.ToString(PID);
           
            Shoppingcartid = Convert.ToInt32(CommonComponent.GetScalarCommonData(" select ISNULL(ShoppingCartID,0) as ShoppingCartID from tb_shoppingcart where Customerid in (select Customerid from tb_order where Ordernumber=" + OrderNumber + ")"));
            HdnCartID.Value = Convert.ToString(Shoppingcartid);
            if (Shoppingcartid == 0)
            {
                CommonComponent.GetScalarCommonData(" insert into tb_shoppingcart(Customerid,createdon) select Customerid,getdate() from tb_order where Ordernumber=" + OrderNumber + "  ");
                Shoppingcartid = Convert.ToInt32(CommonComponent.GetScalarCommonData(" select ShoppingCartID from tb_shoppingcart where Customerid in (select Customerid from tb_order where Ordernumber=" + OrderNumber + ")"));
                HdnCartID.Value = Convert.ToString(Shoppingcartid);
            }
            if (Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(1) from tb_shoppingcartitems where ShoppingCartID=" + Shoppingcartid)) == 0)
            {
                foreach (DataRow dr in dsShoppingCart.Tables[0].Rows)
                {
                    CommonComponent.ExecuteCommonData(" insert into tb_shoppingcartitems (Shoppingcartid,productid,Quantity,Price,VariantNames,VariantValues) " +
                        " Values(" + Shoppingcartid + "," + dr["ProductID"].ToString() + "," + dr["Quantity"].ToString() + "," + dr["SalePrice"].ToString() + ",'" + dr["VariantNames"].ToString() + "','" + dr["VariantValues"].ToString() + "') ");
                }
            }
        }

        /// <summary>
        /// Gets the Order Details by Order Number
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        private void GetOrderDetailsByOrderNumber(Int32 OrderNumber)
        {
            OrderComponent objOrder = new OrderComponent();
            DataSet objDsorder = new DataSet();
            objDsorder = objOrder.GetOrderDetailsByOrderID(OrderNumber);
            if (objDsorder != null && objDsorder.Tables.Count > 0 && objDsorder.Tables[0].Rows.Count > 0)
            {
                //lnkAddNew.Attributes.Add("onclick", "openCenteredCrossSaleWindow('lblSubTotal','lblSubTotal');");

                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["OrderSubtotal"].ToString()))
                {
                    lblSubTotal.Text = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderSubtotal"]).ToString("f2");
                    hfsuto.Value = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderSubtotal"]).ToString("f2");
                }
                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()))
                    TxtShippingCost.Text = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderShippingCosts"]).ToString("f2");

                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["OrderTax"].ToString()))
                    TxtTax.Text = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderTax"]).ToString("f2");

                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["OrderTotal"].ToString()))
                {
                    lblTotal.Text = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderTotal"]).ToString("f2");
                    hfTotal.Value = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["OrderTotal"]).ToString("f2");
                }
                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["CustomDiscount"].ToString()))
                {
                    decimal CustomDiscountAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["CustomDiscount"].ToString());
                    TxtDiscount.Text = CustomDiscountAmount.ToString("f2");
                }

                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["CustomerID"].ToString()))
                    HdnCustID.Value = objDsorder.Tables[0].Rows[0]["CustomerID"].ToString();

                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingState"].ToString()))
                    HdnS_State.Value = objDsorder.Tables[0].Rows[0]["ShippingState"].ToString();

                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                    HdnS_Country.Value = objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString(); // Country Name
                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                    Session["ShippingZipCode"] = objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString();

                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                    HdnS_Zip.Value = objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString();

                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["StoreID"].ToString()))
                    HdnStoreID.Value = objDsorder.Tables[0].Rows[0]["StoreID"].ToString();
                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShoppingCardID"].ToString()))
                    SCartID = Convert.ToInt32(objDsorder.Tables[0].Rows[0]["ShoppingCardID"].ToString());

                Int32 CountryId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect CountryID from tb_Country where Name in (Select ShippingCountry FROM dbo.tb_Order WHERE OrderNumber =" + OrderNumber + ")"));
                Session["CountryID"] = CountryId.ToString(); // Country Id
                HdnS_CountryID.Value = CountryId.ToString();// Country Id
                BindShippingMethod();
                BindCart(Convert.ToInt32(HdnCustID.Value));
                Int32.TryParse(Convert.ToString(objDsorder.Tables[0].Rows[0]["ShoppingCardID"]), out OrderedShoppingCartID);
                Int32.TryParse(Convert.ToString(objDsorder.Tables[0].Rows[0]["StoreID"]), out StoreID);
                HdnStoreID.Value = StoreID.ToString();
                //lnkAddNew.Attributes.Add("onclick", "OpenCenterWindow('RMA_ProductSkus.aspx?StoreID=" + StoreID + "&ono=" + OrderNumber + "&clientid=" + HdnCustID.Value + "&CustID=" + HdnCustID.Value + "',850,700);");
                lnkAddNew.Attributes.Add("onclick", "openCenteredCrossSaleWindow('lblSubTotal','lblSubTotal');");

            }
            else
            {
                lblMsg.Text = "Record(s) not Found !";
            }
        }

        /// <summary>
        /// Shopping Cart Item Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void GVShoppingCartItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "remove")
            {
                string[] tmp = e.CommandArgument.ToString().Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                CommonComponent.ExecuteCommonData("delete from tb_ShoppingCartitems where ShoppingCartID=" + tmp[0].ToString() + " and PRoductid=" + tmp[1].ToString() + " and CustomCartID=" + tmp[2].ToString());
                BindCart(Convert.ToInt32(HdnCustID.Value));
            }
        }

        /// <summary>
        /// Binds the cart of Order.
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        private void BindCart(Int32 CustomerID)
        {
            StringBuilder Table = new StringBuilder();
            DataSet dsShoppingCart = new DataSet();
            dsShoppingCart = CommonComponent.GetCommonDataSet(" select osi.CustomCartID,osi.Shoppingcartid,osi.Productid,osi.VariantNames,osi.Variantvalues,p.name,p.sku, " +
                                                                " osi.price as SalePrice,osi.Quantity from tb_shoppingcartitems osi " +
                                                                " inner join tb_product p on p.productid=osi.productid " +
                                                                " where osi.Shoppingcartid=(select top 1 Shoppingcartid from tb_Shoppingcart where Customerid = " + CustomerID + " ) ");
            GVShoppingCartItems.DataSource = dsShoppingCart;
            GVShoppingCartItems.DataBind();
            Decimal subtotal = 0;
            foreach (DataRow dr in dsShoppingCart.Tables[0].Rows)
            {
                subtotal = subtotal + (Convert.ToDecimal(dr["SalePrice"]) * Convert.ToInt32(dr["Quantity"].ToString()));
            }
            hfsuto.Value = subtotal.ToString("f2");
            lblSubTotal.Text = subtotal.ToString("f2");
            try
            {
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
                TxtShippingCost.Text = ShippingCost.ToString("f2");
                decimal tax = Convert.ToDecimal(TxtTax.Text.Trim());
                decimal ship = Convert.ToDecimal(TxtShippingCost.Text.Trim());
                decimal discount = Convert.ToDecimal(TxtDiscount.Text.Trim());
                lblTotal.Text = ((subtotal + tax + ship) - discount).ToString("f2");
                hfTotal.Value = lblTotal.Text;
            }
            catch { }
        }

        #region BindShippingMethod

        /// <summary>
        /// Binds the shipping method.
        /// </summary>
        /// 

        private void BindShippingMethod()
        {
            bool FlagIsShipping = false;
            string strUSPSMessage = "";
            string strUPSMessage = "";
            string strFedexSMessage = "";
            lblMsg.Text = "";
            ddlShippingMethod.Items.Clear();
            ShippingComponent objShipping = new ShippingComponent();
            DataSet objShipServices = new DataSet();
            objShipServices = objShipping.GetShippingServices(Convert.ToInt32(HdnStoreID.Value.ToString()));
            ddlShippingMethod.Items.Clear();
            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(HdnS_CountryID.Value.ToString()));
            decimal Weight = decimal.Zero;
            StateComponent objState = new StateComponent();
            string stateName = Convert.ToString(objState.GetStateCodeByName(HdnS_State.Value.ToString()));
            if (ViewState["hdnWeightTotal"] != null)
            {
                Weight = Convert.ToDecimal(ViewState["hdnWeightTotal"].ToString());
            }
            if (ViewState["hdnWeightTotal1"] != null)
            {
                Weight += Convert.ToDecimal(ViewState["hdnWeightTotal1"].ToString());
            }
            if (hfsuto.Value.ToString() != "")
            {
                if (Request.QueryString["CustID"] != null && !string.IsNullOrEmpty(Request.QueryString["CustID"].ToString()))
                {
                    string filedoc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT DocFile FROM dbo.tb_Customer WHERE CustomerID ='" + Request.QueryString["CustID"].ToString() + "'"));
                    string CustDocPathTemp = AppLogic.AppConfigs("Customer.DocumentPath");

                    if (File.Exists(Server.MapPath(CustDocPathTemp + Request.QueryString["CustID"].ToString() + "/" + filedoc)))
                    {
                        TxtTax.Text = "0";
                    }
                    else
                    {
                        TxtTax.Text = SaleTax(HdnS_State.Value.ToString(), HdnS_Zip.Value.ToString(), Convert.ToDecimal(hfsuto.Value.ToString())).ToString("f2");
                    }
                }
                else
                {
                    TxtTax.Text = SaleTax(HdnS_State.Value.ToString(), HdnS_Zip.Value.ToString(), Convert.ToDecimal(hfsuto.Value.ToString())).ToString("f2");
                }
            }
            else
            {
                TxtTax.Text = "0";
            }
            if (Weight == 0)
            {
                Weight = 1;
            }
            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));

            DataTable UPSTable = new DataTable();
            UPSTable.Columns.Add("ShippingMethodName", typeof(String));
            UPSTable.Columns.Add("Price", typeof(decimal));


            DataTable USPSTable = new DataTable();
            USPSTable.Columns.Add("ShippingMethodName", typeof(String));
            USPSTable.Columns.Add("Price", typeof(decimal));


            DataTable FedexTable = new DataTable();
            FedexTable.Columns.Add("ShippingMethodName", typeof(String));
            FedexTable.Columns.Add("Price", typeof(decimal));


            if (objShipServices != null && objShipServices.Tables.Count > 0 && objShipServices.Tables[0].Rows.Count > 0)
            {
                if (objShipServices.Tables[0].Select("ShippingService='UPS'").Length > 0)
                {
                    UPSTable = UPSMethodBind(CountryCode.ToString(), stateName, HdnS_Zip.Value.ToString(), Weight, "UPS", ref strUPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='USPS'").Length > 0)
                {
                    EndiciaService objRate = new EndiciaService();
                    USPSTable = objRate.EndiciaGetRatesAdmin(HdnS_Zip.Value.ToString(), CountryCode.ToString(), Convert.ToDouble(Weight), ref strUSPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='FEDEX'").Length > 0)
                {
                    FedexTable = FedexMethod(Convert.ToDecimal(Weight), stateName, HdnS_Zip.Value.ToString(), CountryCode, ref strFedexSMessage);
                    if (FedexTable != null && FedexTable.Rows.Count == 0)
                    {
                        ViewState["IsFrieght"] = "1";
                    }
                    else
                    {
                        ViewState["IsFrieght"] = "0";
                    }
                }

                if (UPSTable != null && UPSTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(UPSTable);
                }
                if (USPSTable != null && USPSTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(USPSTable);
                }


                if (FedexTable != null && FedexTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(FedexTable);
                }

                if (Request.QueryString["CustID"] != null)
                {
                    bool IsFreeShipping = false;
                    IsFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasFreeShipping,0) FROM tb_CustomerLevel inner JOIN dbo.tb_Customer ON tb_Customer.CustomerLevelID=tb_CustomerLevel.CustomerLevelID WHERE tb_Customer.CustomerID=" + Convert.ToInt32(Request.QueryString["CustID"].ToString()) + ""));
                    if (IsFreeShipping)
                    {
                        if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
                        {
                            //String strFreeShipping = "Standard Shipping($0.00)";
                            //DataRow dataRow = ShippingTable.NewRow();
                            //dataRow["ShippingMethodName"] = strFreeShipping;
                            //dataRow["Price"] = 0;
                            // ShippingTable.Rows.Add(dataRow);
                        }
                    }
                }

                //if (!string.IsNullOrEmpty(CountryCode.ToString().Trim()) && (CountryCode.ToUpper() == "US" || CountryCode.ToUpper() == "CA" || CountryCode.ToUpper() == "AU" || CountryCode.ToUpper() == "GB"))
                //{

                decimal OrderTotal = 0;
                decimal SubTotal = 0;
                double Price = 0;
                if (!string.IsNullOrEmpty(lblTotal.Text.ToString()))
                {
                    OrderTotal = Convert.ToDecimal(lblTotal.Text.ToString());
                }
                if (!string.IsNullOrEmpty(lblSubTotal.Text.ToString()) && Convert.ToDecimal(lblSubTotal.Text.ToString()) > 0)
                {
                    SubTotal = Convert.ToDecimal(lblSubTotal.Text.ToString());
                }

                if (CountryCode.ToString().Trim().ToUpper() == "US" || CountryCode.ToString().Trim().ToUpper() == "UNITED STATES")
                {
                    if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                    {
                        for (int k = 0; k < ShippingTable.Rows.Count; k++)
                        {
                            if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ground") > -1)
                            {
                                string[] strMethodname = ShippingTable.Rows[k]["ShippingMethodName"].ToString().Split("($".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string Shippingname = "";
                                if (strMethodname.Length > 0)
                                {
                                    Shippingname = strMethodname[0].ToString();
                                }
                                if (OrderTotal > 0)
                                {
                                    if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(249))
                                    {
                                        Price = 0;
                                        //lblFreeShippningMsg.Text = "Congratulations!! You qualified for Free Shipping. ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(0) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(9.99))
                                    {
                                        Price = 2.99;
                                        Decimal TotalDiff = Convert.ToDecimal(249) - Convert.ToDecimal(OrderTotal);
                                        // lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(10) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(30.99))
                                    {
                                        Price = 5.99;
                                        Decimal TotalDiff = Convert.ToDecimal(249) - Convert.ToDecimal(OrderTotal);
                                        // lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(31) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(50.99))
                                    {
                                        Price = 9.99;
                                        Decimal TotalDiff = Convert.ToDecimal(249) - Convert.ToDecimal(OrderTotal);
                                        // lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(51) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(99.99))
                                    {
                                        Price = 14.99;
                                        Decimal TotalDiff = Convert.ToDecimal(249) - Convert.ToDecimal(OrderTotal);
                                        //lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(100) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(149.99))
                                    {
                                        Price = 15.99;
                                        Decimal TotalDiff = Convert.ToDecimal(249) - Convert.ToDecimal(OrderTotal);
                                        //lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(150) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(199.99))
                                    {
                                        Price = 17.99;
                                        Decimal TotalDiff = Convert.ToDecimal(249) - Convert.ToDecimal(OrderTotal);
                                        //lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(200) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(248.99))
                                    {
                                        Price = 19.99;

                                        Decimal TotalDiff = Convert.ToDecimal(249) - Convert.ToDecimal(OrderTotal);
                                        //lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }

                                    Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(Price)) + ")";
                                }
                                ShippingTable.Rows[k]["ShippingMethodName"] = Shippingname;
                                ShippingTable.Rows[k]["Price"] = Convert.ToDecimal(Price);
                                ShippingTable.AcceptChanges();
                            }
                            else if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups standard") > -1)
                            {
                                ShippingTable.Rows.RemoveAt(k);
                                ShippingTable.AcceptChanges();
                                k--;

                            }
                        }

                        if (SubTotal > 0)
                        {
                            decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.18);
                            DataRow dr;
                            dr = ShippingTable.NewRow();
                            dr["ShippingMethodName"] = "USA-3 DAY Express Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                            dr["Price"] = Convert.ToDecimal(ShippingPrice);
                            ShippingTable.Rows.Add(dr);

                            ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.22);
                            DataRow drnext;
                            drnext = ShippingTable.NewRow();
                            drnext["ShippingMethodName"] = "USA-Next Day Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                            drnext["Price"] = Convert.ToDecimal(ShippingPrice);
                            ShippingTable.Rows.Add(drnext);
                        }
                    }
                }
                else if (CountryCode.ToUpper() == "CA" || CountryCode.ToString().Trim().ToUpper() == "CANADA")
                {
                    //decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.18);
                    DataRow dr;
                    for (int k = 0; k < ShippingTable.Rows.Count; k++)
                    {
                        if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups standard") <= -1)
                        {
                            ShippingTable.Rows.RemoveAt(k);
                            ShippingTable.AcceptChanges();
                            k--;
                        }
                    }

                    //dr = ShippingTable.NewRow();
                    //dr["ShippingMethodName"] = "UPS Standard to Canada" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(0)) + ")";
                    //dr["Price"] = 0;
                    //ShippingTable.Rows.Add(dr);

                    if (ViewState["AllProductsSwatch"] != null && ViewState["AllProductsSwatch"].ToString().Trim() != "" && ViewState["AllProductsSwatch"].ToString().Trim() == "1")
                    {
                        double SwatchRate = 6.99;
                        if (!string.IsNullOrEmpty(AppLogic.AppConfigs("InternationalSwatchRate").ToString()))
                        {
                            double.TryParse(AppLogic.AppConfigs("InternationalSwatchRate").ToString(), out SwatchRate);
                        }
                        dr = ShippingTable.NewRow();
                        dr["ShippingMethodName"] = "International Swatch Orders" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(SwatchRate)) + ")";
                        dr["Price"] = Convert.ToDecimal(SwatchRate);
                        ShippingTable.Rows.Add(dr);
                    }

                    decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.25);
                    dr = ShippingTable.NewRow();
                    dr["ShippingMethodName"] = "CANADA-International Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                    dr["Price"] = Convert.ToDecimal(ShippingPrice);
                    ShippingTable.Rows.Add(dr);

                    // ONE Pending for Swatch Product
                }
                else if (CountryCode.ToUpper() == "AU" || CountryCode.ToString().Trim().ToUpper() == "AUSTRALIA")
                {
                    if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                    {
                        for (int k = 0; k < ShippingTable.Rows.Count; k++)
                        {

                            ShippingTable.Rows.RemoveAt(k);
                            ShippingTable.AcceptChanges();
                            k--;


                        }
                    }
                    DataRow dr;
                    decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.35);
                    dr = ShippingTable.NewRow();
                    dr["ShippingMethodName"] = "AUSTRALIA-International Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                    dr["Price"] = Convert.ToDecimal(ShippingPrice);
                    ShippingTable.Rows.Add(dr);


                }
                else if (CountryCode.ToUpper() == "GB" || CountryCode.ToString().Trim().ToUpper() == "UNITED KINGDOM")
                {
                    string StrShippingstate = "";
                    DataRow dr;

                    if (!string.IsNullOrEmpty(stateName.Trim().ToString()))
                    {
                        StrShippingstate = stateName;
                    }
                  
                    if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                    {
                        for (int k = 0; k < ShippingTable.Rows.Count; k++)
                        {
                            if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups worldwide") <= -1)
                            {
                                ShippingTable.Rows.RemoveAt(k);
                                ShippingTable.AcceptChanges();
                                k--;

                            }
                        }

                    }


                    decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.35);
                    dr = ShippingTable.NewRow();
                    dr["ShippingMethodName"] = "UK-GB International Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                    dr["Price"] = Convert.ToDecimal(ShippingPrice);
                    ShippingTable.Rows.Add(dr);

                    //if (!string.IsNullOrEmpty(StrShippingstate) && StrShippingstate.ToString().ToLower().IndexOf("virgin islands") > -1)
                    //{
                    //    ShippingPrice = 0;
                    //    //decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.35);
                    //    dr = ShippingTable.NewRow();
                    //    dr["ShippingMethodName"] = "UPS Worldwide Express" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                    //    dr["Price"] = Convert.ToDecimal(ShippingPrice);
                    //    ShippingTable.Rows.Add(dr);
                    //}

                }

                if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                {
                    DataView dvShipping = ShippingTable.DefaultView;
                    dvShipping.Sort = "Price asc";

                    ddlShippingMethod.DataSource = dvShipping.ToTable();
                    ddlShippingMethod.DataTextField = "ShippingMethodName";
                    ddlShippingMethod.DataValueField = "ShippingMethodName";
                    ddlShippingMethod.DataBind();
                    FlagIsShipping = true;
                }
                //}
                //else
                //{
                //    ddlShippingMethod.Items.Clear();
                //    ListItem li = new ListItem("Bongo International", "Bongo International");
                //    ddlShippingMethod.Items.Insert(ddlShippingMethod.Items.Count, li);
                //}

                if (strUSPSMessage != "" && strUPSMessage != "")
                {
                    lblMsg.Text = strUPSMessage + strUSPSMessage;
                    lblMsg.Visible = true;
                }
                else if (strUSPSMessage != "")
                {
                    lblMsg.Text = strUSPSMessage;
                    lblMsg.Visible = true;
                }
                else if (strUPSMessage != "")
                {
                    lblMsg.Text = strUPSMessage;
                    lblMsg.Visible = true;
                }

                if (FlagIsShipping == true)
                    ddlShippingMethod_SelectedIndexChanged(null, null);

            }
        }

        /// <summary>
        /// Fedex Method Bind
        /// </summary>
        /// <param name="Weight">Decimal Weight</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Country">String Country</param>
        /// <param name="StrMessage">Return Error Message </param>
        /// <returns>Returns th Fedex Methods as a DataTable</returns>
        private DataTable FedexMethod(decimal Weight, string State, string ZipCode, string Country, ref string StrMessage)
        {

            //   string GetFedexrate = Convert.ToString(obj.FedexGetRates(Convert.ToDecimal(Weight), "", "", "", State, ZipCode, CountryCode.ToString(), Convert.ToInt32(Session["CustID"]), true));

            if (ZipCode == "" || Country == "")
            {
                return null;
            }
            StringBuilder tmpFixedShipping = new StringBuilder(4096);
            StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            string GetFedexrate = "";
            Fedex obj = new Fedex();
            //      StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            if (Weight > decimal.Zero)
            {
                GetFedexrate = Convert.ToString(obj.FedexGetRates(Convert.ToDecimal(Weight), "", "", "", State, ZipCode, Country.ToString(), Convert.ToInt32(Session["CustID"]), true));
            }
            else
                GetFedexrate = Convert.ToString(obj.FedexGetRates(Convert.ToDecimal(1), "", "", "", State, ZipCode, Country.ToString(), Convert.ToInt32(Session["CustID"]), true));


            //FedEx Methods

            tmpRealTimeShipping.Append((string)GetFedexrate);
            string strResult = GetFedexrate;

            #region Get Fixed Shipping Methods

            try
            {
                ShippingComponent objShipping = new ShippingComponent();
                DataSet dsFixedShippingMethods = new DataSet();
                dsFixedShippingMethods = objShipping.GetFixedShippingMethods(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "FEDEX");
                if (dsFixedShippingMethods != null && dsFixedShippingMethods.Tables.Count > 0 && dsFixedShippingMethods.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsFixedShippingMethods.Tables[0].Rows.Count; i++)
                    {

                        tmpFixedShipping.Append((string)dsFixedShippingMethods.Tables[0].Rows[i]["ShippingMethod"] + ",");
                    }
                }
            }
            catch { }

            #endregion
            string[] strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strResult.ToString() == "")
            {
                //lblMsg.Visible = true;
                StrMessage = strResult + "<br />";
                // lblMsg.Text += strResult + "<br />";
                strResult = tmpFixedShipping.ToString();

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
                    //  rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));
                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                    ShippingTable.Rows.Add(dataRow);
                }
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

                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                    ShippingTable.Rows.Add(dataRow);
                }
            }

            return ShippingTable;

        }

        //private void BindShippingMethod()
        //{
        //    string strUSPSMessage = "";
        //    string strUPSMessage = "";
        //    {
        //        TxtTax.Text = "0";
        //    }
        //    if (Weight == 0)
        //    {
        //        Weight = 1;
        //    lblMsg.Text = "";

        //    ShippingComponent objShipping = new ShippingComponent();
        //    DataSet objShipServices = new DataSet();
        //    objShipServices = objShipping.GetShippingServices(Convert.ToInt32(HdnStoreID.Value.ToString()));
        //    ddlShippingMethod.Items.Clear();
        //    CountryComponent objCountry = new CountryComponent();
        //    string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(HdnS_CountryID.Value.ToString()));
        //    decimal Weight = decimal.Zero;
        //    StateComponent objState = new StateComponent();
        //    string stateName = Convert.ToString(objState.GetStateCodeByName(HdnS_State.Value.ToString()));
        //    if (ViewState["hdnWeightTotal"] != null)
        //    {
        //        Weight = Convert.ToDecimal(ViewState["hdnWeightTotal"].ToString());
        //    }
        //    if (ViewState["hdnWeightTotal1"] != null)
        //    {
        //        Weight += Convert.ToDecimal(ViewState["hdnWeightTotal1"].ToString());
        //    }
        //    if (hfsuto.Value.ToString() != "")
        //    {
        //        TxtTax.Text = SaleTax(HdnS_State.Value.ToString(), HdnS_Zip.Value.ToString(), Convert.ToDecimal(hfsuto.Value.ToString())).ToString("f2");
        //    }
        //    else
        //    }
        //    DataTable ShippingTable = new DataTable();
        //    ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
        //    ShippingTable.Columns.Add("Price", typeof(decimal));

        //    DataTable UPSTable = new DataTable();
        //    UPSTable.Columns.Add("ShippingMethodName", typeof(String));
        //    UPSTable.Columns.Add("Price", typeof(decimal));


        //    DataTable USPSTable = new DataTable();
        //    USPSTable.Columns.Add("ShippingMethodName", typeof(String));
        //    USPSTable.Columns.Add("Price", typeof(decimal));

        //    if (objShipServices != null && objShipServices.Tables.Count > 0 && objShipServices.Tables[0].Rows.Count > 0)
        //    {
        //        if (objShipServices.Tables[0].Select("ShippingService='UPS'").Length > 0)
        //        {
        //            UPSTable = UPSMethodBind(CountryCode.ToString(), stateName, HdnS_Zip.Value.ToString(), Weight, "UPS", ref strUPSMessage);
        //        }
        //        if (objShipServices.Tables[0].Select("ShippingService='USPS'").Length > 0)
        //        {
        //            EndiciaService objRate = new EndiciaService();
        //            USPSTable = objRate.EndiciaGetRatesAdmin(HdnS_Zip.Value.ToString(), CountryCode.ToString(), Convert.ToDouble(Weight), ref strUSPSMessage);
        //        }

        //        if (UPSTable != null && UPSTable.Rows.Count > 0)
        //        {
        //            ShippingTable.Merge(UPSTable);
        //        }
        //        if (USPSTable != null && USPSTable.Rows.Count > 0)
        //        {
        //            ShippingTable.Merge(USPSTable);
        //        }

        //        if (Request.QueryString["CustID"] != null)
        //        {
        //            bool IsFreeShipping = false;
        //            IsFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasFreeShipping,0) FROM tb_CustomerLevel inner JOIN dbo.tb_Customer ON tb_Customer.CustomerLevelID=tb_CustomerLevel.CustomerLevelID WHERE tb_Customer.CustomerID=" + Convert.ToInt32(Request.QueryString["CustID"].ToString()) + ""));
        //            if (IsFreeShipping)
        //            {
        //                if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
        //                {
        //                    String strFreeShipping = "Standard Shipping($0.00)";
        //                    DataRow dataRow = ShippingTable.NewRow();
        //                    dataRow["ShippingMethodName"] = strFreeShipping;
        //                    dataRow["Price"] = 0;
        //                    ShippingTable.Rows.Add(dataRow);
        //                }
        //            }
        //        }
        //        if (ShippingTable != null && ShippingTable.Rows.Count > 0)
        //        {
        //            DataView dvShipping = ShippingTable.DefaultView;
        //            dvShipping.Sort = "Price asc";

        //            ddlShippingMethod.DataSource = dvShipping.ToTable();
        //            ddlShippingMethod.DataTextField = "ShippingMethodName";
        //            ddlShippingMethod.DataValueField = "ShippingMethodName";
        //            ddlShippingMethod.DataBind();
        //        }
        //        else
        //        {
        //            ddlShippingMethod.Items.Clear();
        //            Session["ShippingMethodID"] = null;
        //            Session["ShippingCharge"] = null;
        //        }
        //        if (ddlShippingMethod.Items.Count > 0)
        //        {
        //            decimal ShippingCost = 0;
        //            if (ddlShippingMethod.SelectedValue.ToString() != "")
        //            {
        //                int Index = ddlShippingMethod.SelectedItem.Text.IndexOf("($");
        //                int Length = ddlShippingMethod.SelectedItem.Text.LastIndexOf(")") - Index;
        //                if (Index != -1 && Length != 0)
        //                {
        //                    string strShippingCost = ddlShippingMethod.SelectedItem.Text.Substring(Index + 2, Length - 2).Trim();
        //                    Decimal.TryParse(strShippingCost, out ShippingCost);
        //                }
        //            }
        //            TxtShippingCost.Text = ShippingCost.ToString("f2");
        //            Decimal ship = Convert.ToDecimal(TxtShippingCost.Text.Trim());
        //            Decimal subtotal = Convert.ToDecimal(lblSubTotal.Text);
        //            Decimal tax = Convert.ToDecimal(TxtTax.Text.Trim());
        //            Decimal discount = Convert.ToDecimal(TxtDiscount.Text.Trim());

        //            lblTotal.Text = ((subtotal + tax + ship) - discount).ToString("f2");
        //            hfsuto.Value = lblSubTotal.Text;
        //            hfTotal.Value = lblTotal.Text;
        //        }
        //        if (strUSPSMessage != "" && strUPSMessage != "")
        //        {
        //            lblMsg.Text = strUPSMessage + strUSPSMessage;
        //            lblMsg.Visible = true;
        //        }
        //        else if (strUSPSMessage != "")
        //        {
        //            lblMsg.Text = strUSPSMessage;
        //            lblMsg.Visible = true;
        //        }
        //        else if (strUPSMessage != "")
        //        {
        //            lblMsg.Text = strUPSMessage;
        //            lblMsg.Visible = true;
        //        }
        //    }
        //}

        /// <summary>
        /// UPS Method Bind
        /// </summary>
        /// <param name="Country">String CountryCode</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Weight">Decimal Weight</param>
        /// <returns>Returns the UPS Method List</returns>
        private DataTable UPSMethodBind(string Country, string State, string ZipCode, decimal Weight, string ServiceName, ref string StrMessage)
        {
            if (ZipCode == "" || Country == "")
            {
                return null;
            }

            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));


            UPS obj = new UPS(AppLogic.AppConfigs("Shipping.OriginAddress"), AppLogic.AppConfigs("Shipping.OriginAddress2"), AppLogic.AppConfigs("Shipping.OriginCity"), AppLogic.AppConfigs("Shipping.OriginState"), AppLogic.AppConfigs("Shipping.OriginZip"), AppLogic.AppConfigs("Shipping.OriginCountry"));
            obj.DestinationCountryCode = Country;
            obj.DestinationStateProvince = State;
            obj.DestinationZipPostalCode = ZipCode;
            StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            StringBuilder tmpFixedShipping = new StringBuilder(4096);

            if (Weight > decimal.Zero)
            {
                tmpRealTimeShipping.Append((string)obj.UPSGetRates(Convert.ToDecimal(Weight), Convert.ToDecimal(0), true));
            }
            else
            {
                tmpRealTimeShipping.Append((string)obj.UPSGetRates(Convert.ToDecimal(1), Convert.ToDecimal(0), true));
            }
            string strResult = tmpRealTimeShipping.ToString();

            #region Get Fixed Shipping Methods

            try
            {
                ShippingComponent objShipping = new ShippingComponent();
                DataSet dsFixedShippingMethods = new DataSet();
                dsFixedShippingMethods = objShipping.GetFixedShippingMethods(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), ServiceName);
                if (dsFixedShippingMethods != null && dsFixedShippingMethods.Tables.Count > 0 && dsFixedShippingMethods.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsFixedShippingMethods.Tables[0].Rows.Count; i++)
                    {
                        tmpFixedShipping.Append((string)dsFixedShippingMethods.Tables[0].Rows[i]["ShippingMethod"] + ",");
                    }
                }
            }
            catch { }

            #endregion

            string[] strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strResult.ToString().ToLower().IndexOf("error") > -1)
            {
                //lblMsg.Visible = true;
                StrMessage = strResult + "<br />";
                // lblMsg.Text += strResult + "<br />";
                strResult = tmpFixedShipping.ToString();

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
                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                    ShippingTable.Rows.Add(dataRow);
                }
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

                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                    ShippingTable.Rows.Add(dataRow);
                }
            }
            return ShippingTable;
        }

        /// <summary>
        /// Get Sales Tax By ZipCode or StateName
        /// </summary>
        /// <param name="stateName">string StateName</param>
        /// <param name="zipCode">String ZipCode</param>
        /// <param name="orderAmount">string OrderAmount</param>
        /// <returns>Returns the Sale Tax</returns>
        private decimal SaleTax(string stateName, string zipCode, decimal orderAmount)
        {
            ShoppingCartComponent objTax = new ShoppingCartComponent();
            decimal salesTax = decimal.Zero;
            if (TxtDiscount.Text.Trim() == "")
            {
                orderAmount = orderAmount - Convert.ToDecimal(0);
            }
            else
            {
                orderAmount = orderAmount - Convert.ToDecimal(TxtDiscount.Text.Trim());
            }
            salesTax = Convert.ToDecimal(objTax.GetSalesTax(stateName, zipCode, orderAmount, Convert.ToInt32(HdnStoreID.Value.ToString())));
            return salesTax;
        }

        #endregion

        /// <summary>
        /// Shipping Method Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlShippingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            TxtShippingCost.Text = ShippingCost.ToString("f2");

            decimal subtotal = 0;
            decimal.TryParse(lblSubTotal.Text, out subtotal);
            decimal tax = Convert.ToDecimal(TxtTax.Text.Trim());
            decimal ship = Convert.ToDecimal(TxtShippingCost.Text.Trim());
            decimal discount = Convert.ToDecimal(TxtDiscount.Text.Trim());
            lblTotal.Text = ((subtotal + tax + ship) - discount).ToString("f2");
            hfTotal.Value = lblTotal.Text;
        }

        /// <summary>
        ///  Update Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                BindCart(Convert.ToInt32(HdnCustID.Value));
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
                TxtShippingCost.Text = ShippingCost.ToString("f2");
                decimal subtotal = 0;
                decimal.TryParse(lblSubTotal.Text, out subtotal);
                decimal tax = Convert.ToDecimal(TxtTax.Text.Trim());
                decimal ship = Convert.ToDecimal(TxtShippingCost.Text.Trim());
                decimal discount = Convert.ToDecimal(TxtDiscount.Text.Trim());
                lblTotal.Text = ((subtotal + tax + ship) - discount).ToString("f2");
                hfTotal.Value = lblTotal.Text;
            }
            catch { }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnUpdate_Click(null, null);
                if (GVShoppingCartItems.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('Your Shopping Cart is Empty.');", true);
                    return;
                }
                Int32 ono = 0;
                Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"]), out ono);

                OrderComponent ObjOrder_Old = new OrderComponent();
                DataSet objDsorder = new DataSet();
                OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
                objDsorder = ObjOrder_Old.GetOrderDetailsByOrderID(ono);
                if (objDsorder != null && objDsorder.Tables.Count > 0 && objDsorder.Tables[0].Rows.Count > 0)
                {
                    tb_Order objOrder = new tb_Order();
                    objOrder.OrderGUID = System.Guid.NewGuid().ToString();
                    objOrder.CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());

                    #region Comment
                    objOrder.FirstName = objDsorder.Tables[0].Rows[0]["FirstName"].ToString();
                    objOrder.LastName = objDsorder.Tables[0].Rows[0]["LastName"].ToString();
                    objOrder.ShippingMethod = objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString();
                    objOrder.Deleted = false;

                    objOrder.CardType = objDsorder.Tables[0].Rows[0]["CardType"].ToString();
                    objOrder.CardVarificationCode = objDsorder.Tables[0].Rows[0]["CardVarificationCode"].ToString();
                    objOrder.CardName = objDsorder.Tables[0].Rows[0]["CardName"].ToString();
                    objOrder.CardNumber = objDsorder.Tables[0].Rows[0]["CardNumber"].ToString();
                    objOrder.Last4 = objDsorder.Tables[0].Rows[0]["Last4"].ToString();
                    objOrder.CardExpirationMonth = objDsorder.Tables[0].Rows[0]["CardExpirationMonth"].ToString();
                    objOrder.CardExpirationYear = objDsorder.Tables[0].Rows[0]["CardExpirationYear"].ToString();

                    if (lblSubTotal.Text.Trim() == "")
                        lblSubTotal.Text = "0.00";
                    if (lblTotal.Text.Trim() == "")
                        lblTotal.Text = "0.00";
                    objOrder.CustomDiscount = Math.Round(Convert.ToDecimal(TxtDiscount.Text.Trim()), 2);
                    objOrder.OrderSubtotal = Convert.ToDecimal(lblSubTotal.Text.Trim());
                    objOrder.OrderSubtotal = Convert.ToDecimal(lblSubTotal.Text.Trim());
                    objOrder.OrderTotal = Convert.ToDecimal(hfTotal.Value.ToString().Trim());
                    objOrder.LastIPAddress = Context.Request.UserHostAddress.ToString();
                    objOrder.IsNew = true;
                    if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString()))
                        objOrder.CouponDiscountAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString());
                    else
                        objOrder.CouponDiscountAmount = decimal.Zero;

                    if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()))
                        objOrder.QuantityDiscountAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString());
                    else
                        objOrder.QuantityDiscountAmount = decimal.Zero;

                    if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString()))
                        objOrder.GiftCertificateDiscountAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString());
                    else
                        objOrder.GiftCertificateDiscountAmount = decimal.Zero;

                    if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["LevelDiscountPercent"].ToString()))
                        objOrder.LevelDiscountPercent = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["LevelDiscountPercent"].ToString());
                    else
                        objOrder.GiftCertificateDiscountAmount = decimal.Zero;
                    if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()))
                        objOrder.LevelDiscountAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString());
                    else
                        objOrder.LevelDiscountAmount = decimal.Zero;

                    if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["RefundedAmount"].ToString()))
                        objOrder.RefundedAmount = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["RefundedAmount"].ToString());
                    else
                        objOrder.RefundedAmount = decimal.Zero;

                    objOrder.ChargeAmount = decimal.Zero;
                    objOrder.AuthorizedAmount = decimal.Zero;
                    objOrder.AdjustmentAmount = decimal.Zero;

                    if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["GiftWrapAmt"].ToString()))
                        objOrder.GiftWrapAmt = Convert.ToDecimal(objDsorder.Tables[0].Rows[0]["GiftWrapAmt"].ToString());
                    else
                        objOrder.GiftWrapAmt = decimal.Zero;
                    //objOrder.TransactionType = 1;
                    objOrder.PaymentMethod = objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString();
                    objOrder.PaymentGateway = objDsorder.Tables[0].Rows[0]["PaymentGateway"].ToString();
                    if (TxtShippingCost.Text == "")
                        TxtShippingCost.Text = "0";
                    objOrder.OrderShippingCosts = Convert.ToDecimal(TxtShippingCost.Text.Trim());
                    objOrder.CustomerID = Convert.ToInt32(objDsorder.Tables[0].Rows[0]["CustomerID"].ToString());
                    objOrder.OrderShippingCosts = Decimal.Parse(TxtShippingCost.Text.ToString());
                    objOrder.AuthorizedOn = DateTime.Now;
                    #endregion

                    OrderComponent objAddOrder = new OrderComponent();
                    Int32 NewOrderNo = 0;
                    NewOrderNo = Convert.ToInt32(objAddOrder.AddOrder(objOrder, NewOrderNo, Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));

                    #region Capture OrderTotal
                    try
                    {
                        if (NewOrderNo > 0)
                        {
                            OrderComponent objPayment = new OrderComponent();
                            DataSet dsPayment = new DataSet();
                            string strPaymentgateWay = "";
                            string strPaymentgateWaystatus = "";
                            if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString()))
                            {
                                dsPayment = objPayment.GetPaymentGateway(objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString(), Convert.ToInt32(AppLogic.AppConfigs("StoreId").ToString()));
                                if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
                                {
                                    strPaymentgateWay = Convert.ToString(dsPayment.Tables[0].Rows[0]["PaymentService"].ToString().ToUpper());
                                    strPaymentgateWaystatus = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString().ToUpper());
                                }
                            }
                            string strResult = OrderPayment(strPaymentgateWay, strPaymentgateWaystatus, NewOrderNo, Convert.ToDecimal(objOrder.OrderTotal), GetCustomerDetailsForpayment(Convert.ToInt32(HdnCustID.Value.ToString())));

                            if (strResult.ToUpper() == "OK")
                            {
                                if (strPaymentgateWaystatus.ToLower().IndexOf("auth") > -1)
                                {
                                    objAddOrder.InsertOrderlog(1, OrderNumber, "", Convert.ToInt32(Session["AdminID"].ToString()));
                                }
                                else if (strPaymentgateWaystatus.ToLower().IndexOf("capture") > -1 || strPaymentgateWaystatus.ToLower().IndexOf("sale") > -1)
                                {
                                    objAddOrder.InsertOrderlog(2, OrderNumber, "", Convert.ToInt32(Session["AdminID"].ToString()));
                                }
                                objAddOrder.InsertOrderlog(9, OrderNumber, "", Convert.ToInt32(Session["AdminID"].ToString()));
                            }
                        }
                    }
                    catch { }
                    #endregion

                    String sql2 = String.Empty;

                    if (objDsorder.Tables[0].Rows[0]["CardType"] != null && objDsorder.Tables[0].Rows[0]["CardType"].ToString().Trim() != "")
                    {
                        sql2 = " update tb_order set " +
                                " LastIPAddress='" + objDsorder.Tables[0].Rows[0]["LastIPAddress"].ToString().Trim() + "' " +
                                " ,RefOrderID='" + objDsorder.Tables[0].Rows[0]["RefOrderID"].ToString().Trim() + "' " +
                                " ,CardType='" + objDsorder.Tables[0].Rows[0]["CardType"].ToString() + "' " +
                                " ,CardVarificationCode='" + objDsorder.Tables[0].Rows[0]["CardVarificationCode"].ToString() + "' " +
                                " ,CardNumber='" + objDsorder.Tables[0].Rows[0]["CardNumber"].ToString() + "' " +
                                " ,CardName='" + objDsorder.Tables[0].Rows[0]["CardName"].ToString() + "' " +
                                " ,CardExpirationMonth='" + objDsorder.Tables[0].Rows[0]["CardExpirationMonth"].ToString() + "' " +
                                " ,CardExpirationYear='" + objDsorder.Tables[0].Rows[0]["CardExpirationYear"].ToString() + "' " +
                                " where OrderNumber=" + NewOrderNo.ToString();
                    }
                    else
                    {
                        sql2 = " update tb_order set " +
                               " LastIPAddress='" + objDsorder.Tables[0].Rows[0]["LastIPAddress"].ToString().Trim() + "' " +
                               " ,RefOrderID='" + objDsorder.Tables[0].Rows[0]["RefOrderID"].ToString().Trim() + "' " +
                               " where OrderNumber=" + NewOrderNo.ToString();
                    }

                    CommonComponent.ExecuteCommonData(sql2);
                    CommonComponent.ExecuteCommonData(" insert into tb_RMAMapping (RMAID,OldOrderNumber,OrderNumber,Storeid)  " +
                                                      " values(" + Request.QueryString["RMA"] + "," + ono + "," + NewOrderNo + "," + objDsorder.Tables[0].Rows[0]["StoreId"].ToString() + ") ");

                    if (Request.QueryString["RMA"] != null)
                    {
                        CommonComponent.ExecuteCommonData(" update tb_returnitem set isreturn=1,RetunitemNotes='" + txtNotes.Text.Trim() + "<br/>RMA-" + Request.QueryString["RMA"].ToString() + "' where returnITemid=" + Request.QueryString["RMA"].ToString());
                    }
                    try
                    {
                        CommonComponent.ExecuteCommonData("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber,Description) VALUES (" + Session["AdminID"].ToString() + ",23," + Convert.ToInt32(ono) + "," + Convert.ToInt32(objOrder.OrderNumber) + ",'" + txtNotes.Text.Trim().Replace("'", "''") + "')");
                        objAddOrder.InsertOrderlog(23, Convert.ToInt32(ono), "", Convert.ToInt32(Session["AdminID"].ToString()));
                    }
                    catch { }
                    CommonComponent.ExecuteCommonData("Delete From tb_ShoppingCartItems Where ShoppingCartID  In (Select ShoppingCartID From tb_ShoppingCart Where CustomerID= " + objDsorder.Tables[0].Rows[0]["CustomerID"].ToString() + ")");
                    CommonComponent.ExecuteCommonData("Delete From tb_ShoppingCart Where CustomerID = " + objDsorder.Tables[0].Rows[0]["CustomerID"].ToString() + "");
                    lblMsg.Text = "Order Successfully Placed.";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "alert('Order Processed Successfully..');window.opener.location.href=window.opener.location.href;window.close();", true);
                }
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "alert('Error while Saving Order. Please Retry..');", true);
            }
        }


        /// <summary>
        /// Orders the payment
        /// </summary>
        /// <param name="PayementGateWay">string PayementGateWay</param>
        /// <param name="PayementGateWaystatus">string PayementGateWaystatus</param>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="orderTotal">decimal orderTotal</param>
        /// <param name="objorder">tb_Order objorder</param>
        /// <returns>Returns the output value as a string format which contains HTML</returns>
        private string OrderPayment(string PayementGateWay, string PayementGateWaystatus, Int32 OrderNumber, decimal orderTotal, tb_Order objorder)
        {
            LinkpointComponent objLinkpoint = new LinkpointComponent();
            tb_Order objOrder = new tb_Order();
            String AVSResult = String.Empty;
            String AuthorizationResult = String.Empty;
            String AuthorizationCode = String.Empty;
            String AuthorizationTransID = String.Empty;
            String TransactionCommand = String.Empty;
            String TransactionResponse = String.Empty;
            string Status = "";
            OrderComponent objDsorder = new OrderComponent();
            DataSet dsOrder = new DataSet();
            dsOrder = objDsorder.GetOrderDetailsByOrderID(OrderNumber);
            Status = "OK";
            objOrder = new tb_Order();
            if (Status.ToUpper() == "OK")
            {
                objOrder.OrderNumber = OrderNumber;
                objOrder.CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());
                objOrder.AVSResult = AVSResult;
                objOrder.AuthorizationResult = AuthorizationResult;
                objOrder.AuthorizationCode = AuthorizationCode;
                objOrder.AuthorizationPNREF = AuthorizationTransID;
                objOrder.TransactionCommand = TransactionCommand;
                //if (PayementGateWaystatus != null && PayementGateWaystatus.ToString().ToLower().IndexOf("auth") > -1)
                //{
                //    objOrder.TransactionStatus = "AUTHORIZED";
                //    objOrder.AuthorizedOn = DateTime.Now;
                //}
                //else
                //{
                objOrder.TransactionStatus = "CAPTURED";
                objOrder.CapturedOn = DateTime.Now;
                //}
                OrderComponent objUpdateOrder = new OrderComponent();
                Int32 updateOrder = Convert.ToInt32(objUpdateOrder.AddOrder(objOrder, OrderNumber, Convert.ToInt32(AppConfig.StoreID.ToString())));
                //SendMail(OrderNumber);
            }
            else
            {

                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('" + Status + "','Error!');", true);
            }
            return Status;
        }


        /// <summary>
        /// Gets the customer details for payment
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns tb_Order Table Object</returns>
        private tb_Order GetCustomerDetailsForpayment(Int32 CustomerID)
        {
            DataSet dsCustomer = new DataSet();
            CustomerComponent objCust = new CustomerComponent();
            dsCustomer = objCust.GetCustomerDetails(CustomerID);

            //Billing Address
            tb_Order objorderData = new tb_Order();

            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                objorderData.FirstName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString());
                objorderData.LastName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString());
                objorderData.Email = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());
                objorderData.BillingFirstName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString());
                objorderData.BillingLastName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString());
                objorderData.BillingAddress1 = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address1"].ToString());
                objorderData.BillingAddress2 = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address2"].ToString());
                objorderData.BillingSuite = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Suite"].ToString());
                objorderData.BillingCity = Convert.ToString(dsCustomer.Tables[0].Rows[0]["City"].ToString());
                objorderData.BillingState = Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"].ToString());
                objorderData.BillingZip = Convert.ToString(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString());
                objorderData.BillingCountry = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CountryName"].ToString());
                objorderData.BillingPhone = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Phone"].ToString());
                objorderData.BillingEmail = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());

                objorderData.CardName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["NameOnCard"].ToString());
                objorderData.CardType = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardTypeID"].ToString());
                objorderData.CardVarificationCode = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardVerificationCode"].ToString());
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString()))
                    objorderData.CardNumber = SecurityComponent.Decrypt(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString());
                objorderData.CardExpirationMonth = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardExpirationMonth"].ToString());
                objorderData.CardExpirationYear = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardExpirationYear"].ToString());
                // Credit Card Details
            }

            //Shipping Address
            if (dsCustomer != null && dsCustomer.Tables.Count > 1 && dsCustomer.Tables[1].Rows.Count > 0)
            {
                objorderData.ShippingFirstName = Convert.ToString(dsCustomer.Tables[1].Rows[0]["FirstName"].ToString());
                objorderData.ShippingLastName = Convert.ToString(dsCustomer.Tables[1].Rows[0]["LastName"].ToString());
                objorderData.ShippingAddress1 = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address1"].ToString());
                objorderData.ShippingAddress2 = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address2"].ToString());
                objorderData.ShippingSuite = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Suite"].ToString());
                objorderData.ShippingCity = Convert.ToString(dsCustomer.Tables[1].Rows[0]["City"].ToString());
                objorderData.ShippingState = Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"].ToString());
                objorderData.ShippingZip = Convert.ToString(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString());
                objorderData.ShippingCountry = Convert.ToString(dsCustomer.Tables[1].Rows[0]["CountryName"].ToString());
                objorderData.ShippingPhone = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Phone"].ToString());
                objorderData.ShippingEmail = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Email"].ToString());
            }
            return objorderData;
        }

        /// <summary>
        /// Binds the Subtotal
        /// </summary>
        /// <param name="Price">Decimal Price</param>
        /// <param name="Qty">int Qty</param>
        /// <returns>Returns Sub Total Details as a String</returns>
        public String BindSubtotal(Decimal Price, Int32 Qty)
        {
            Decimal aj = Price * Qty;
            aj = Math.Round(aj, 2);
            return aj.ToString();
        }

        protected void GVShoppingCartItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblVariantNames = (Label)e.Row.FindControl("lblVariantNames");
                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                Literal ltrVariNamevalue = (Literal)e.Row.FindControl("ltrVariNamevalue");

                string[] variantName = lblVariantNames.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantValue = lblVariantValues.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < variantValue.Length; j++)
                {
                    if (variantName.Length > j)
                    {
                        ltrVariNamevalue.Text += variantName[j].ToString() + " : " + variantValue[j].ToString() + "<br />";
                    }
                }
            }
        }
    }
}