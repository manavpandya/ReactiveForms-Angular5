using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
namespace Solution.UI.Web
{
    public partial class WishList : System.Web.UI.Page
    {
        #region Local Variables
        CustomerComponent objCustomer = null;
        WishListItemsComponent objWishlList = null;
        tb_WishListItems tb_wishlistitems = null;
        bool flag = false;
        String IsAddeDCart = "";
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            CommonOperations.RedirectWithSSL(true);
            if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
            { }
            else
            {
                Response.Redirect("/Login.aspx", true);
            }

            if (!IsPostBack)
            {
                if (!CheckCustomerIsRestricted())
                {
                    if (Session["CustID"] != null)
                    {
                        FillWishListItems(Convert.ToInt32(Session["CustID"]));
                    }
                    if (Session["WishListProduct"] != null)
                    {
                        Session["WishListProduct"] = null;
                    }
                }
                if (Request.UrlReferrer != null)
                {
                    abacklink.HRef = Request.UrlReferrer.ToString();
                }
            }
        }

        /// <summary>
        /// Fill Wish List Items into repeater
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        private void FillWishListItems(Int32 CustomerID)
        {
            objWishlList = new WishListItemsComponent();
            DataSet dsWLItems = new DataSet();
            dsWLItems = objWishlList.GetAllWishListItemBYCustID(CustomerID);

            if (dsWLItems != null && dsWLItems.Tables.Count > 0 && dsWLItems.Tables[0].Rows.Count > 0)
            {
                flag = true;
                rptWishList.DataSource = dsWLItems.Tables[0];
                rptWishList.DataBind();
                lblEmptyWishList.Visible = false;
                btnUpdateWishlist.Visible = true;
            }
            else
            {
                flag = false;
                rptWishList.DataSource = null;
                rptWishList.DataBind();
                lblEmptyWishList.Visible = true;
                btnUpdateWishlist.Visible = false;
            }
        }

        /// <summary>
        /// Check Customer IsRestricted or not
        /// </summary>
        /// <returns> Returns Boolean value according to results </returns>
        private bool CheckCustomerIsRestricted()
        {
            objWishlList = new WishListItemsComponent();
            bool IsRestricted = objWishlList.CheckBlockedIPAddressByIPAddress(Convert.ToString(Request.UserHostAddress));
            return IsRestricted;
        }

        /// <summary>
        /// Wish List Repeater Item Command event
        /// </summary>
        /// <param name="source">object source</param>
        /// <param name="e">RepeaterCommandEventArgs e</param>
        protected void rptWishList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Code for Remove Items from repeater
            if (e.CommandName.ToString().Trim().ToLower() == "remove")
            {
                objWishlList = new WishListItemsComponent();
                int IsRemoved = objWishlList.RemoveItemsFromWishList(Convert.ToInt32(e.CommandArgument));

                if (IsRemoved == 1)
                {
                    FillWishListItems(Convert.ToInt32(Session["CustID"]));
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Product has been removed from Wish List.');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem while removing product.');", true);
                    return;
                }
            }

                // Code for Remove Items from repeater
            else if (e.CommandName.ToString().Trim().ToLower() == "addcart")
            {
                IsAddeDCart = "";

                System.Web.UI.HtmlControls.HtmlInputHidden hdnProductID = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Item.FindControl("hdnProductID");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnWishListID = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Item.FindControl("hdnWishListID");

                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                TextBox txtQty = (TextBox)e.Item.FindControl("txtQty");

                System.Web.UI.HtmlControls.HtmlInputHidden hdnproductTypeID = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Item.FindControl("hdnproductTypeID");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantNames = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Item.FindControl("hdnVariantNames");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValues = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Item.FindControl("hdnVariantValues");

                AddTocart(hdnProductID.Value.ToString(), lblPrice.Text.ToString(), txtQty.Text.ToString(), hdnproductTypeID.Value.ToString(), hdnVariantNames.Value.ToString(), hdnVariantValues.Value.ToString());

                // objWishlList = new WishListItemsComponent();
                //String IsAddeDCart = objWishlList.WishListToAddtoCart(Convert.ToInt32(e.CommandArgument));

                if (IsAddeDCart.ToString().ToLower() == "success")
                {
                    objWishlList = new WishListItemsComponent();
                    int IsRemoved = objWishlList.RemoveItemsFromWishList(Convert.ToInt32(hdnWishListID.Value));

                    FillWishListItems(Convert.ToInt32(Session["CustID"]));

                    //if (flag == false)
                    //{
                    //    Response.Redirect("/addtocart.aspx", true);
                    //}

                    DataSet dsCart = new DataSet();
                    System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)Page.Master.FindControl("cartlink");
                    dsCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));
                    Decimal Totalprice = Decimal.Zero;
                    if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
                    {
                        int items = Convert.ToInt32(dsCart.Tables[0].Rows[0]["TotalItems"].ToString());
                        Totalprice = Convert.ToDecimal(dsCart.Tables[0].Rows[0]["SubTotal"].ToString());

                        //objAnchor.InnerHtml = items > 1 ? "Items (" + items.ToString("D2") + ")" : "Item (" + items.ToString("D2") + ")";
                        //objAnchor.InnerHtml = items > 1 ? "<strong>" + items.ToString("D2") + "</strong> <span class='span-text1'>Shopping Cart</span> <span class='span-text2'>Items are Selected</span>" : "<strong>" + items.ToString("D2") + "</strong> <span class='span-text1'>Shopping Cart</span> <span class='span-text2'>Items are Selected</span>";
                        // objAnchor.InnerHtml = "<span>" + items.ToString("D2") + "</span> <strong> Item(s) </strong>";
                        if (items <= 1)
                        {
                            objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " item)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(Totalprice)) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='Cart' title='Cart' class='cart-icon'>";
                        }
                        else
                        {
                            objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " items)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(Totalprice)) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='Cart' title='Cart' class='cart-icon'>";
                        }

                        objAnchor.HRef = "/checkoutcommon.aspx";
                        Session["NoOfCartItems"] = dsCart.Tables[0].Rows[0]["TotalItems"].ToString();
                    }
                    else
                    {
                        Session["NoOfCartItems"] = null;
                        //objAnchor.InnerHtml = "Item (00)";
                        //objAnchor.InnerHtml = "<strong>00</strong> <span class='span-text1'>Shopping Cart</span> <span class='span-text2'>Items are Selected</span>";

                        objAnchor.InnerHtml = "<p><span class='navQty'> (00 item)</span><span class='navTotal'> $0.00</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='Cart' title='Cart' class='cart-icon' />";
                        //objAnchor.InnerHtml = "<span>00</span> <strong> Item(s) </strong>";
                        objAnchor.HRef = "javascript:void(0);";
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('" + IsAddeDCart + "');", true);
                    return;
                }
            }
        }

        /// <summary>
        /// Wish List Repeater Item DataBound Event
        /// </summary>
        /// <param name="source">object source</param>
        /// <param name="e">RepeaterCommandEventArgs e</param>
        protected void rptWishList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Image imgProduct = (Image)e.Item.FindControl("imgProduct");
                // System.Web.UI.HtmlControls.HtmlAnchor aProductImage = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lnkProductPage");
                //System.Web.UI.HtmlControls.HtmlAnchor aProductName = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lnkProductName");
                String ImagePath = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ImageName"));
                String MainCategory = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "MainCategory"));
                String SEName = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "SEName"));
                String ProductID = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ProductID"));
                String ProductName = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ProductName"));
                String hdnrealtedProductID = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "RelatedproductID"));
                LinkButton lnkRemove = (LinkButton)e.Item.FindControl("lnkRemove");
                TextBox txtQty = (TextBox)e.Item.FindControl("txtQty");
                ImageButton btnAddtoCart = (ImageButton)e.Item.FindControl("btnAddtoCart");


                if (!string.IsNullOrEmpty(hdnrealtedProductID) && hdnrealtedProductID.ToString().Trim() != "0")
                {
                    lnkRemove.Visible = false;
                    btnAddtoCart.Visible = false;
                    txtQty.Attributes.Add("readonly", "true");

                }
                #region Product Images and Links

                //if (aProductImage != null)
                //{
                //    aProductImage.HRef = "/" + MainCategory + "/" + SEName + "-" + ProductID + ".aspx";
                //    aProductImage.Title = ProductName.ToString();
                //    imgProduct.ToolTip = ProductName.ToString();
                //}
                //if (aProductName != null)
                //{
                //    aProductName.HRef = "/" + MainCategory + "/" + SEName + "-" + ProductID + ".aspx";
                //    aProductName.Title = ProductName.ToString();
                //}

                if (ImagePath != "")
                {
                    imgProduct.ImageUrl = GetMicroImage(ImagePath);
                }
                else
                {
                    imgProduct.ImageUrl = AppLogic.AppConfigs("ImagePathProduct") + "/Micro/image_not_available.jpg";
                }

                #endregion


                string VariantValues = "";
                VariantValues = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "VariantValues"));
                if (!String.IsNullOrEmpty(VariantValues) && VariantValues.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                {


                    string[] variantValues = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "VariantValues")).Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < variantValues.Length; i++)
                    {
                        VariantValues = variantValues[i].ToString();
                        if (!String.IsNullOrEmpty(VariantValues) && VariantValues.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                        {
                            string strvariant = VariantValues.Substring(0, VariantValues.ToString().IndexOf("("));
                            Int32 count = 0;
                            count = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select count(VariantID) from tb_ProductVariantValue Where productid=" + ProductID + " and VariantValue='" + strvariant + "' and cast(Buy1Fromdate as date)<=cast(getdate() as date) and cast(Buy1Todate as date)>=cast(getdate() as date)"));
                            if (count == 1 && !string.IsNullOrEmpty(hdnrealtedProductID) && hdnrealtedProductID.ToString().Trim() == "0")
                            {
                                btnAddtoCart.Visible = true;
                            }
                            else
                            {
                                btnAddtoCart.Visible = false;
                            }
                        }
                    }
                }


                #region Product Variants

                string[] variantName = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "VariantNames")).Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantValue = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "VariantValues")).Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                int variantValueCount = variantValue.Count();
                int variantNameCount = variantName.Count();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < variantValueCount; i++)
                {
                    if (variantNameCount > i)
                    {
                        sb.Append("<br/>" + variantName[i].ToString() + " : " + variantValue[i].ToString());
                    }
                }
                Literal ltrVariant = (Literal)e.Item.FindControl("ltrVariant");
                ltrVariant.Text = sb.ToString();

                #endregion
            }
        }

        /// <summary>
        /// Get Micro Image if exists for display 
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Returns Micro Image Path</returns>
        public String GetMicroImage(String Name)
        {
            String ImagePath = AppLogic.AppConfigs("ImagePathProduct") + "/Micro/" + Name.ToString();
            if (!File.Exists(Server.MapPath(ImagePath)))
            {
                ImagePath = AppLogic.AppConfigs("ImagePathProduct") + "/Micro/image_not_available.jpg";
            }
            return ImagePath;
        }

        /// <summary>
        /// Update WishList Button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateWishlist_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (rptWishList.Items.Count > 0)
                {
                    objWishlList = new WishListItemsComponent();
                    int totalItemCount = rptWishList.Items.Count;
                    for (int j = 0; j < rptWishList.Items.Count; j++)
                    {
                        TextBox txtQty = (TextBox)rptWishList.Items[j].FindControl("txtQty");
                        if (Convert.ToString(txtQty.Text) == "" || Convert.ToInt32(txtQty.Text) == 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Enter valid quantity.');", true);
                            txtQty.Focus();
                            return;
                        }
                    }
                    for (int i = 0; i < rptWishList.Items.Count; i++)
                    {
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnWishListID = (System.Web.UI.HtmlControls.HtmlInputHidden)rptWishList.Items[i].FindControl("hdnWishListID");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnProductID = (System.Web.UI.HtmlControls.HtmlInputHidden)rptWishList.Items[i].FindControl("hdnProductID");
                        int pInventory = 0;
                        TextBox txtQty = (TextBox)rptWishList.Items[i].FindControl("txtQty");

                        //check inventory of product
                        if (hdnProductID != null && Convert.ToInt32(txtQty.Text.ToString().Trim()) > 0)
                        {
                            DataSet ds = ProductComponent.GetProductDetailByID(Convert.ToInt32(hdnProductID.Value), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                {
                                    pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());
                                }
                            }
                            if (pInventory >= Convert.ToInt32(txtQty.Text.Trim()))
                            {
                                if (txtQty != null)
                                {
                                    Int32 IsUpdated = objWishlList.UpdateItemsOfWishList(Convert.ToInt32(hdnWishListID.Value), Convert.ToInt32(txtQty.Text));
                                }
                                lblError.Text = "";
                            }
                            else
                            {
                                lblError.Text = "We have not enough inventory!";
                            }
                        }
                        else
                        {
                            lblError.Text = "Enter Valid Inventory!";
                        }
                    }
                    FillWishListItems(Convert.ToInt32(Session["CustID"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Keep Shopping button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAnotherItem_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/index.aspx");
        }

        public String SetAttribute(String Name)
        {
            return Name.Replace('"', '-').Replace('\'', '-').ToString();
        }



        /// <summary>
        /// Adds Selected Product into the Cart
        /// </summary>
        private void AddTocart(String ProductID, String Price, String Quantity, String ProductTypeID, String TempVariantNames, String TempVariantValues)
        {
            bool outofstock = false;
            Price = Price.ToString().Replace("$", "");
            string StrContact = "";
            int Yardqty = 0;
            double actualYard = 0;
            string strDatenew = "";
            ViewState["AvailableDate"] = null;
            bool IsRestricted = true;
            bool isDropshipProduct = false;
            Int32 Isorderswatch1 = 0;
            String VariantNames = "";
            String VariantValues = "";


            string[] strNmtemp2 = TempVariantNames.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] strValtemp2 = TempVariantValues.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int p = 0; p < strNmtemp2.Length; p++)
            {
                if (strNmtemp2[p].ToString().ToLower().IndexOf("estimated delivery") > -1)
                {
                    //VariantNames += strNmtemp[p].ToString() + ",";
                    //VariantValues += strValtemp[p].ToString() + " " + strVal[k].ToString() + ",";
                }
                else
                {
                    VariantNames += strNmtemp2[p].ToString() + ",";
                    VariantValues += strValtemp2[p].ToString() + ",";
                }
            }
            IsRestricted = CheckCustomerIsRestricted();
            if (!IsRestricted)
            {
                if (Session["CustID"] == null || Session["CustID"].ToString() == "")
                {
                    // AddCustomer();
                }

                string VariantValueId = "";
                string VariantNameId = "";
                string VariantQty = "";
                bool check = false;
                string[] strPIds = { "" };
                string[] strPrices = { "" };
                string[] strQuantitys = { "" };
                Yardqty = 0;
                string VariantValueIdtemp = "";
                actualYard = 0;

                if (!String.IsNullOrEmpty(ProductID) && !String.IsNullOrEmpty(Price) && !String.IsNullOrEmpty(Quantity))
                {
                    strPIds = ProductID.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    strPrices = Price.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    strQuantitys = Quantity.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                }
                else
                {
                    return;
                }

                //if (Request.QueryString["ProdID"] != null && Request.QueryString["Price"] != null && Request.QueryString["Quantity"] != null)
                //{
                //    strPIds = Request.QueryString["ProdID"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //    strPrices = Request.QueryString["Price"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //    strQuantitys = Request.QueryString["Quantity"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                //}
                //else
                //{
                //    return;
                //}



                for (int k = 0; k < strPIds.Length; k++)
                {
                    if (String.IsNullOrEmpty(ProductTypeID))
                    {
                        String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustId"].ToString() + " Order By ShoppingCartID DESC) "));
                        //if (!string.IsNullOrEmpty(strswatchQtyy) && (Convert.ToInt32(strswatchQtyy) + Convert.ToInt32(strQuantitys[k].ToString())) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()))
                        //{
                        strPrices[k] = "0.00";
                        //}
                        //else
                        //{
                        //strPrices[k] = "0.00";
                        //}
                    }
                    else
                    {
                        Isorderswatch1 = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + strPIds[k].ToString() + " and ItemType='Swatch'"));
                        if (Isorderswatch1 == 1)
                        {
                            strPrices[k] = "0.00";
                        }
                    }
                    isDropshipProduct = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(IsdropshipProduct,0) FROM tb_product WHERE productid=" + Convert.ToInt32(strPIds[k].ToString()).ToString() + ""));

                    if (!String.IsNullOrEmpty(ProductTypeID) && (ProductTypeID == "2")) // Made to Measure
                    {
                        DataSet DSFabricDetails = CommonComponent.GetCommonDataSet("Select FabricCode,FabricType  from tb_product where Productid=" + Convert.ToInt32(strPIds[k].ToString()) + " and StoreId=" + AppConfig.StoreID + "");
                        if (DSFabricDetails != null && DSFabricDetails.Tables.Count > 0 && DSFabricDetails.Tables[0].Rows.Count > 0)
                        {
                            string FabricCode = Convert.ToString(DSFabricDetails.Tables[0].Rows[0]["FabricCode"]);
                            string FabricType = Convert.ToString(DSFabricDetails.Tables[0].Rows[0]["FabricType"]);
                            Int32 FabricTypeID = 0;
                            if (!string.IsNullOrEmpty(FabricCode) && !string.IsNullOrEmpty(FabricType))
                            {
                                FabricTypeID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(FabricTypeID,0) as FabricTypeID from tb_ProductFabricType where FabricTypename ='" + FabricType + "'"));
                                if (FabricTypeID > 0)
                                {
                                    DataSet dsFabricWidth = CommonComponent.GetCommonDataSet("Select top 1 * from tb_ProductFabricWidth where FabricCodeID in (Select ISNULL(FabricCodeID,0) from tb_ProductFabricCode Where FabricTypeID=" + FabricTypeID + " and Code='" + FabricCode + "')");
                                    Int32 QtyOnHand = 0, NextOrderQty = 0, TotalQty = 0;
                                    Int32 OrderQty = Convert.ToInt32(strQuantitys[k].ToString());

                                    if (dsFabricWidth != null && dsFabricWidth.Tables.Count > 0 && dsFabricWidth.Tables[0].Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"].ToString()))
                                            QtyOnHand = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"]);

                                        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"].ToString()))
                                            NextOrderQty = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"]);
                                    }
                                    Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Sum(ISNULL(Quantity,0)),0) FROM tb_ShoppingCartItems " +
                                                " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + Convert.ToInt32(Session["CustID"]) + " Order By ShoppingCartID) " +
                                                " AND ProductID=" + Convert.ToInt32(strPIds[k].ToString()) + " AND VariantNames like '" + VariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-") + "%' AND VariantValues like '" + VariantValues.ToString().Replace("'", "''") + "%'"));
                                    OrderQty = OrderQty + ShoppingCartQty;
                                    TotalQty = QtyOnHand + NextOrderQty;
                                    try
                                    {
                                        string Style = "";
                                        double Width = 0;
                                        double Length = 0;
                                        string Options = "";
                                        string[] strNmyard = VariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        string[] strValyeard = VariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        if (strNmyard.Length > 0)
                                        {
                                            if (strValyeard.Length == strNmyard.Length)
                                            {
                                                for (int j = 0; j < strNmyard.Length; j++)
                                                {
                                                    if (strNmyard[j].ToString().ToLower() == "width")
                                                    {
                                                        Width = Convert.ToDouble(strValyeard[j].ToString());
                                                    }
                                                    if (strNmyard[j].ToString().ToLower() == "length")
                                                    {
                                                        Length = Convert.ToDouble(strValyeard[j].ToString());
                                                    }
                                                    if (strNmyard[j].ToString().ToLower() == "options")
                                                    {
                                                        Options = Convert.ToString(strValyeard[j].ToString());
                                                    }
                                                    if (strNmyard[j].ToString().ToLower() == "header")
                                                    {
                                                        Style = Convert.ToString(strValyeard[j].ToString());
                                                    }
                                                }
                                            }
                                        }
                                        string resp = "";
                                        if (Width > Convert.ToDouble(0) && Length > Convert.ToDouble(0) && Style != "" && Options != "")
                                        {

                                            DataSet dsYard = new DataSet();
                                            dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + strPIds[k].ToString() + "," + Width.ToString() + "," + Length.ToString() + "," + OrderQty.ToString() + ",'" + Style + "','" + Options + "'");
                                            if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                            {
                                                //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                                                resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                actualYard = Convert.ToDouble(resp.ToString());
                                            }
                                        }
                                        //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        //{
                                        //    //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                                        //    resp = string.Format("{0:0.00}", Convert.ToDecimal(ds.Tables[0].Rows[0][1].ToString()));
                                        //}
                                        if (resp != "")
                                        {
                                            OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));

                                        }
                                    }
                                    catch
                                    {
                                    }
                                    Yardqty = OrderQty;
                                    if (!String.IsNullOrEmpty(VariantValues) && VariantValues.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                                    {
                                        OrderQty = OrderQty * 2;
                                    }

                                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + strPIds[k].ToString() + " "));

                                    if (!String.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                                    {
                                        if (isDropshipProduct == false)
                                        {
                                            outofstock = true;
                                        }
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                    else
                                    {
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (CheckInventory(Convert.ToInt32(strPIds[k].ToString()), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(strQuantitys[k].ToString()), ProductTypeID, VariantNames, VariantValues))
                        {

                        }
                        else
                        {
                            if (isDropshipProduct == false)
                            {
                                outofstock = true;
                            }
                        }
                    }
                }
                if (!String.IsNullOrEmpty(ProductTypeID) && ProductTypeID == "1" && ViewState["AvailableDate"] == null) // Made to Measure
                {
                    DateTime dtnew = DateTime.Now.Date.AddDays(12);
                    ViewState["AvailableDate"] = Convert.ToString(dtnew);
                    //for (int k = 0; k < strPIds.Length; k++)
                    //{
                    //    Int32 OrderQty = Convert.ToInt32(strQuantitys[k].ToString());
                    //    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + strPIds[k].ToString() + " "));
                    //    if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                    //    {
                    //        ViewState["AvailableDate"] = StrVendor;
                    //    }
                    //    else
                    //    {
                    //        ViewState["AvailableDate"] = StrVendor;
                    //    }
                    //}
                }
                if (outofstock)
                {
                    Response.Clear();
                    string StrRetValue = "";
                    StrRetValue += "<div style=\"height: 120px; position: fixed;display: block;width:100%;\" class=\"description_box\">";
                    StrRetValue += "<div style=\"text-align: center;background: none repeat scroll 0 0 #641114;color: #FFFFFF;font-weight: bold;height: 20px;padding: 5px;\" class=\"title\">";
                    StrRetValue += "<span style=\"font-size:16px;\">Stock Info</span>";
                    StrRetValue += "<div class=\"description_box_border\">";
                    StrRetValue += "<br>";
                    StrRetValue += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">";
                    StrRetValue += "<tbody>";
                    StrRetValue += "<tr>";
                    StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                    StrRetValue += "<div>This product is out of stock.</div>";
                    StrRetValue += "</td>";
                    StrRetValue += "</tr>";
                    StrRetValue += "<tr>";
                    StrRetValue += "<td valign=\"middle\" align=\"center\">";
                    StrRetValue += "<div>Please call <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> for additional assistance.</div>";
                    StrRetValue += "</td>";
                    StrRetValue += "</tr>";
                    StrRetValue += "<tr>";
                    StrRetValue += "<td valign=\"middle\" align=\"center\">&nbsp;</td>";
                    StrRetValue += "</tr>";
                    StrRetValue += "</tbody></table>";
                    StrRetValue += "</div>";
                    StrRetValue += "</div>";

                    Response.Write("<div style='color:cc0000;display:none;'>Not Sufficient Inventory</div>" + StrRetValue.ToString());
                    Response.End();
                }
                if (!String.IsNullOrEmpty(Quantity) && outofstock == false)
                {
                    decimal LengthStdAllow = 0;
                    for (int j = 0; j < strPIds.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(strPIds[j].ToString()) && Session["CustID"] != null)
                        {
                            VariantNameId = "";
                            VariantValueId = "";
                            ShoppingCartComponent objShopping = new ShoppingCartComponent();
                            decimal price = Convert.ToDecimal(strPrices[j].ToString());
                            string avail = "";
                            int RomanShadeId = 0; 
                            decimal fbyardcost = 0;
                            int Qty = Convert.ToInt32(strQuantitys[j].ToString());
                            if (j == 0)
                            {
                                if (!String.IsNullOrEmpty(VariantNames) && !String.IsNullOrEmpty(VariantValues))
                                {
                                    string[] strNm = VariantNames.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    string[] strVal = VariantValues.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    if (strNm.Length > 0)
                                    {
                                        if (strVal.Length == strNm.Length)
                                        {

                                            //Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(RomanShadeId,0) as RomanShadeId from tb_product where ProductId=" + Convert.ToInt32(strPIds[j].ToString()) + " and StoreId=" + AppConfig.StoreID + ""));
                                            for (int pp = 0; pp < strNm.Length; pp++)
                                            {
                                                if (strNm[pp].ToString().ToLower().IndexOf("roman shade design") > -1)
                                                {
                                                    RomanShadeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select TOp 1 ISNULL(RomanShadeId,0) as RomanShadeId from tb_ProductRomanShadeYardage where ShadeName='" + strVal[pp].ToString().Trim() + "' AND isnull(Active,0)=1"));
                                                    //break;
                                                }
                                                if (strNm[pp].ToString().ToLower().IndexOf("color") > -1)
                                                {
                                                    fbyardcost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT isnull(VariantPrice,0) FROM tb_ProductVariantValue WHERE VariantValue='" + strVal[pp].ToString().Trim().Replace("'", "''") + "' and ProductId=" + Convert.ToInt32(strPIds[j].ToString()) + ""));
                                                }


                                            }

                                            decimal VariValue = 0, WidthStdAllow = 0;
                                            bool optiontrue = false;
                                            for (int k = 0; k < strNm.Length; k++)
                                            {
                                                if (VariantNameId.ToString().ToLower().IndexOf(strNm[k].ToString().ToLower()) > -1)
                                                {
                                                    string[] strNmtemp = VariantNameId.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                                    string[] strValtemp = VariantValueId.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                                    VariantNameId = "";
                                                    VariantValueId = "";
                                                    for (int p = 0; p < strNmtemp.Length; p++)
                                                    {
                                                        if (strNmtemp[p].ToString().ToLower().IndexOf(strNm[k].ToString().ToLower()) > -1)
                                                        {
                                                            VariantNameId += strNmtemp[p].ToString() + ",";
                                                            VariantValueId += strValtemp[p].ToString() + " " + strVal[k].ToString() + ",";
                                                        }
                                                        else
                                                        {
                                                            VariantNameId += strNmtemp[p].ToString() + ",";
                                                            VariantValueId += strValtemp[p].ToString() + ",";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    VariantNameId = VariantNameId + strNm[k].ToString() + ",";
                                                    VariantValueId = VariantValueId + strVal[k].ToString() + ",";
                                                }


                                                // Roman Yardage ---------------------------------

                                                if (RomanShadeId > 0 || !String.IsNullOrEmpty(ProductTypeID) && ProductTypeID == "3")
                                                {
                                                    if (strNm[k].ToString().ToLower().Trim().IndexOf("width") > -1)
                                                    {
                                                        if (strVal[k].ToString().IndexOf("/") > -1 && strVal[k].ToString().Trim().IndexOf(" ") > -1)
                                                        {
                                                            WidthStdAllow = Convert.ToDecimal(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")));
                                                            string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/")).Replace(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")), "").Replace(" ", "");

                                                            strwidth = strwidth.Replace("/", "");

                                                            decimal tt = Convert.ToDecimal(strwidth);
                                                            strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "").Replace(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")), "").Replace(" ", "");
                                                            tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                            decimal WidthStdAllow1 = tt;
                                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;


                                                        }
                                                        else if (strVal[k].ToString().IndexOf("/") > -1)
                                                        {
                                                            string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/"));
                                                            strwidth = strwidth.Replace("/", "");

                                                            decimal tt = Convert.ToDecimal(strwidth);
                                                            strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "");
                                                            tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                            decimal WidthStdAllow1 = tt;
                                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;


                                                        }
                                                        else
                                                        {
                                                            decimal WidthStdAllow1 = 0;
                                                            decimal.TryParse(strVal[k].ToString(), out WidthStdAllow1);
                                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;

                                                        }
                                                        //WidthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(Sum(ISNULL(WidthStandardAllowance,0)+" + VariValue + ")/54,0) as WidthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                    }
                                                    if (strNm[k].ToString().ToLower().Trim().IndexOf("length") > -1)
                                                    {
                                                        if (strVal[k].ToString().IndexOf("/") > -1 && strVal[k].ToString().Trim().IndexOf(" ") > -1)
                                                        {
                                                            VariValue = Convert.ToDecimal(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")));
                                                            string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/")).Replace(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")), "").Replace(" ", "");
                                                            decimal tt = Convert.ToDecimal(strwidth);
                                                            strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "").Replace(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")), "").Replace(" ", "");
                                                            tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                            decimal VariValue1 = tt;
                                                            VariValue = VariValue + VariValue1;

                                                        }
                                                        if (VariValue > Convert.ToDecimal(1))
                                                        {
                                                            if (strVal[k].ToString().IndexOf("/") > -1 && strVal[k].ToString().Trim().IndexOf(" ") > -1)
                                                            {
                                                                VariValue = Convert.ToDecimal(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")));
                                                                string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/")).Replace(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")), "").Replace(" ", "");
                                                                decimal tt = Convert.ToDecimal(strwidth);
                                                                strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "").Replace(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")), "").Replace(" ", "");
                                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                                decimal VariValue1 = tt;
                                                                VariValue = VariValue + VariValue1;

                                                            }
                                                            else if (strVal[k].ToString().IndexOf("/") > -1)
                                                            {
                                                                string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/"));
                                                                strwidth = strwidth.Replace("/", "");
                                                                decimal tt = Convert.ToDecimal(strwidth);
                                                                strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "");
                                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                                decimal VariValue1 = tt;
                                                                VariValue = VariValue + VariValue1;
                                                            }
                                                            else
                                                            {
                                                                decimal VariValue1 = 0;
                                                                decimal.TryParse(strVal[k].ToString(), out VariValue1);
                                                                VariValue = VariValue + VariValue1;

                                                            }

                                                            //decimal.TryParse(strVal[k].ToString(), out VariValue);
                                                            decimal tempWidthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(Sum(ISNULL(WidthStandardAllowance,0)+" + WidthStdAllow + ")/54,0) as WidthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                            string strFabricname = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 ShadeName from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + ""));

                                                            if (tempWidthStdAllow > 0)
                                                            {
                                                                if (strFabricname.ToString().ToLower().Trim() == "casual")
                                                                {
                                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                                }
                                                                else if (strFabricname.ToString().ToLower().Trim() == "relaxed")
                                                                {
                                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(2 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                                }
                                                                else if (strFabricname.ToString().ToLower().Trim() == "soft fold")
                                                                {
                                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(2 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                                }
                                                                else if (strFabricname.ToString().ToLower().Trim() == "front slat")
                                                                {
                                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(1.75 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                                }
                                                                else
                                                                {
                                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                                }
                                                                LengthStdAllow = Math.Round(LengthStdAllow, 2);

                                                                for (int p = 0; p < strNm.Length; p++)
                                                                {
                                                                    if (strNm[p].ToString().ToLower().Trim().IndexOf("option") > -1)
                                                                    {
                                                                        optiontrue = true;
                                                                        DataSet dsRoman = new DataSet();
                                                                        dsRoman = CommonComponent.GetCommonDataSet("Select isnull(FabricPerYardCost,0) as FabricPerYardCost,isnull(ManufuacturingCost,0) as ManufuacturingCost from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1");

                                                                        if (dsRoman != null && dsRoman.Tables.Count > 0 && dsRoman.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            decimal yardprice = Convert.ToDecimal(fbyardcost) * Convert.ToDecimal(LengthStdAllow);
                                                                            yardprice = yardprice + Convert.ToDecimal(dsRoman.Tables[0].Rows[0]["ManufuacturingCost"]);
                                                                            decimal rangecost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Cost,0) as Cost from tb_Roman_Shipping where Fromwidth <= " + WidthStdAllow + " And Towidth >=" + WidthStdAllow + " and ISNULL(Active,0)=1"));
                                                                            yardprice = yardprice + rangecost;
                                                                            decimal ProductOptionsPrice = 0;
                                                                            decimal Duties = 0;
                                                                            if (strVal[p].ToString().ToLower() == "lined")
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            else if (strVal[p].ToString().ToLower() == "lined & interlined" || strVal[p].ToString().ToLower() == "lined &amp; interlined")
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  (Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' Or Options='" + strVal[p].ToString().ToLower().Replace("'", "''").Replace("&amp;", "&") + "') and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            else if (strVal[p].ToString().ToLower() == "blackout lining")
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            else
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            Duties = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Duties,0) as Duties from tb_ProductRomanShadeYardage where  RomanShadeId=" + RomanShadeId + ""));
                                                                            //
                                                                            if (Duties > decimal.Zero)
                                                                            {
                                                                                //ProductOptionsPrice = ProductOptionsPrice / Convert.ToDecimal(100);
                                                                                //decimal liningoption = (yardprice * ProductOptionsPrice) + yardprice;

                                                                                ////ProductOptionsPrice = Convert.ToDecimal(15) / Convert.ToDecimal(100);

                                                                                ////liningoption = liningoption * ProductOptionsPrice;
                                                                                ////liningoption = liningoption + yardprice;
                                                                                //price = liningoption;

                                                                                decimal ProductOptionsPrice1 = Duties / Convert.ToDecimal(100);
                                                                                // decimal liningoption = (yardprice * ProductOptionsPrice) + yardprice;

                                                                                //// ProductOptionsPrice = Convert.ToDecimal(15) / Convert.ToDecimal(100);
                                                                                // //liningoption = liningoption * ProductOptionsPrice;
                                                                                // //liningoption = liningoption + yardprice;

                                                                                decimal liningoption = yardprice;
                                                                                ProductOptionsPrice = Convert.ToDecimal(ProductOptionsPrice) / Convert.ToDecimal(100);
                                                                                liningoption = liningoption * ProductOptionsPrice;
                                                                                liningoption = liningoption + yardprice;
                                                                                liningoption = (liningoption * ProductOptionsPrice1);
                                                                                liningoption = liningoption + yardprice;
                                                                                price = liningoption;

                                                                            }
                                                                            else
                                                                            {

                                                                                decimal liningoption = yardprice;

                                                                                ProductOptionsPrice = Convert.ToDecimal(ProductOptionsPrice) / Convert.ToDecimal(100);

                                                                                liningoption = liningoption * ProductOptionsPrice;
                                                                                liningoption = liningoption + yardprice;
                                                                                price = liningoption;
                                                                            }
                                                                        }
                                                                        break;
                                                                    }
                                                                }
                                                                if (optiontrue == false && VariValue == Decimal.Zero && WidthStdAllow == Decimal.Zero)
                                                                {
                                                                    DataSet dsRoman = new DataSet();
                                                                    dsRoman = CommonComponent.GetCommonDataSet("Select isnull(FabricPerYardCost,0) as FabricPerYardCost,isnull(ManufuacturingCost,0) as ManufuacturingCost from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1");

                                                                    if (dsRoman != null && dsRoman.Tables.Count > 0 && dsRoman.Tables[0].Rows.Count > 0)
                                                                    {

                                                                        decimal yardprice = Convert.ToDecimal(fbyardcost) * Convert.ToDecimal(LengthStdAllow);
                                                                        yardprice = yardprice + Convert.ToDecimal(dsRoman.Tables[0].Rows[0]["ManufuacturingCost"]);
                                                                        decimal rangecost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Cost,0) as Cost from tb_Roman_Shipping where Fromwidth <= " + WidthStdAllow + " And Towidth >=" + WidthStdAllow + " and ISNULL(Active,0)=1"));
                                                                        yardprice = yardprice + rangecost;

                                                                        decimal ProductOptionsPrice = 0;// Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0) as AdditionalPricePercentage from tb_ProductOptionsPrice where  ProductId=" + strPIds[j].ToString() + " and Options='" + strVal[p].ToString() + "'"));

                                                                        decimal liningoption = yardprice;
                                                                        ProductOptionsPrice = Convert.ToDecimal(15) / Convert.ToDecimal(100);
                                                                        liningoption = liningoption * ProductOptionsPrice;
                                                                        liningoption = liningoption + yardprice;
                                                                        price = liningoption;

                                                                    }
                                                                    optiontrue = true;
                                                                }
                                                                else
                                                                {
                                                                    if (optiontrue == false)
                                                                    {
                                                                        Decimal shademarkup = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeMarkup' and isnull(deleted,0)=0 and Storeid=1"));

                                                                        Decimal Pricepp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 value FROM tb_ShadeDetail WHERE ShadeWidthID in (SELECT ShadeWidthID FROM tb_Shadewidth WHERE value=floor('" + WidthStdAllow.ToString() + "')) and ShadeLengthID in(SELECT ShadeLengthID FROM tb_ShadeLength WHERE value=floor('" + VariValue.ToString() + "')) "));

                                                                        Decimal yourprice = Pricepp * shademarkup;
                                                                        price = yourprice;
                                                                        optiontrue = true;
                                                                    }
                                                                }
                                                                //price = Select Options



                                                            }
                                                            else
                                                            {
                                                                Decimal shademarkup = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeMarkup' and isnull(deleted,0)=0 and Storeid=1"));

                                                                Decimal Pricepp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 value FROM tb_ShadeDetail WHERE ShadeWidthID in (SELECT ShadeWidthID FROM tb_Shadewidth WHERE value=floor('" + WidthStdAllow.ToString() + "')) and ShadeLengthID in(SELECT ShadeLengthID FROM tb_ShadeLength WHERE value=floor('" + VariValue.ToString() + "')) "));

                                                                Decimal yourprice = Pricepp * shademarkup;
                                                                price = yourprice;
                                                                optiontrue = true;

                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (strVal[k].ToString().IndexOf("/") > -1 && strVal[k].ToString().IndexOf(" ") > -1)
                                                            {
                                                                VariValue = Convert.ToDecimal(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")));
                                                                string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/")).Replace(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")), "").Replace(" ", "");
                                                                decimal tt = Convert.ToDecimal(strwidth);
                                                                strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "").Replace(strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf(" ")), "").Replace(" ", "");
                                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                                decimal VariValue1 = tt;
                                                                VariValue = VariValue + VariValue1;

                                                            }
                                                            else if (strVal[k].ToString().IndexOf("/") > -1)
                                                            {
                                                                string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/"));
                                                                strwidth = strwidth.Replace("/", "");
                                                                decimal tt = Convert.ToDecimal(strwidth);
                                                                strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "");
                                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                                decimal VariValue1 = tt;
                                                                VariValue = VariValue + VariValue1;
                                                            }
                                                            else
                                                            {
                                                                decimal VariValue1 = 0;
                                                                decimal.TryParse(strVal[k].ToString(), out VariValue1);
                                                                VariValue = VariValue + VariValue1;

                                                            }
                                                        }
                                                    }
                                                }
                                                if (ViewState["AvailableDate"] != null && ViewState["AvailableDate"].ToString() != "" && Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"].ToString() == "2")
                                                {
                                                    avail = Convert.ToString(ViewState["AvailableDate"]);
                                                    try
                                                    {
                                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                                if (!String.IsNullOrEmpty(ProductTypeID) && ProductTypeID == "1")
                                                {
                                                    if (avail == "")
                                                    {
                                                        avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 LockQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + strPIds[j].ToString() + "  AND VariantValue='" + strVal[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(LockQuantity,0) >=" + Qty + ""));
                                                        if (avail == "")
                                                        {
                                                            avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 AllowQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + strPIds[j].ToString() + "  AND VariantValue='" + strVal[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(AllowQuantity,0) >=" + Qty + ""));
                                                        }
                                                    }
                                                }


                                            }
                                        }
                                    }
                                }
                                VariantValueIdtemp = VariantValueId;
                                if (LengthStdAllow > 0)
                                {
                                    Yardqty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(LengthStdAllow)));
                                    actualYard = Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDecimal(LengthStdAllow.ToString())));
                                    //// VariantNameId = VariantNameId + "Yardage Required,";
                                    //// VariantValueId = VariantValueId + LengthStdAllow.ToString() + ",";
                                    Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Sum(ISNULL(Quantity,0)),0) FROM tb_ShoppingCartItems " +
                                                " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + Convert.ToInt32(Session["CustID"]) + " Order By ShoppingCartID) " +
                                                " AND ProductID=" + Convert.ToInt32(strPIds[j].ToString()) + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND replace(VariantValues,cast((Quantity * " + LengthStdAllow + ") as nvarchar(100)),'" + LengthStdAllow + "')='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'"));


                                    DataSet dsvariant = new DataSet();
                                    dsvariant = CommonComponent.GetCommonDataSet("SELECT VariantValues FROM tb_ShoppingCartItems  WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Convert.ToInt32(Session["CustID"].ToString()) + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND replace(VariantValues,cast((Quantity * " + LengthStdAllow + ") as nvarchar(100)),'" + LengthStdAllow + "')='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    if (dsvariant != null && dsvariant.Tables.Count > 0 && dsvariant.Tables[0].Rows.Count > 0)
                                    {

                                        VariantValueId = Convert.ToString(dsvariant.Tables[0].Rows[0]["VariantValues"].ToString());
                                    }
                                    Int32 OrderQty = Qty + ShoppingCartQty;
                                    Yardqty = Yardqty * OrderQty;
                                    LengthStdAllow = LengthStdAllow * Convert.ToDecimal(OrderQty);
                                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + strPIds[j].ToString() + ",'" + VariantNames.ToString().Replace("~hpd~", "-") + "','" + VariantValues.ToString().Replace("~hpd~", "-") + "'"));

                                    if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
                                    {
                                        if (isDropshipProduct == false)
                                        {

                                            Response.Clear();
                                            string StrRetValue = "";
                                            StrRetValue += "<div style=\"height: 120px; position: fixed;display: block;width:100%;\" class=\"description_box\">";
                                            StrRetValue += "<div style=\"text-align: center;background: none repeat scroll 0 0 #641114;color: #FFFFFF;font-weight: bold;height: 20px;padding: 5px;\" class=\"title\">";
                                            StrRetValue += "<span style=\"font-size:16px;\">Stock Info</span>";
                                            StrRetValue += "<div class=\"description_box_border\">";
                                            StrRetValue += "<br>";
                                            StrRetValue += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">";
                                            StrRetValue += "<tbody>";
                                            //StrRetValue += "<tr>";
                                            //StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                                            //StrRetValue += "<div>Available in our Warehouse : <span style=\"color:#641114 !important; font-weight:bold;\">" + TotalQuantityValue.ToString() + "</span></div>";
                                            //StrRetValue += "</td>";
                                            //StrRetValue += "</tr>";
                                            //StrRetValue += "<tr>";
                                            //StrRetValue += "<td valign=\"middle\" align=\"center\">";
                                            //StrRetValue += "<div>Call us at <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> if you are looking for more than <span style=\"color:#641114 !important; font-weight:bold;\">" + TotalQuantityValue.ToString() + "</span> Quantity(s)</div>";
                                            //StrRetValue += "</td>";
                                            //StrRetValue += "</tr>";

                                            StrRetValue += "<tr>";
                                            StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                                            StrRetValue += "<div>This product is out of stock.</div>";
                                            StrRetValue += "</td>";
                                            StrRetValue += "</tr>";
                                            StrRetValue += "<tr>";
                                            StrRetValue += "<td valign=\"middle\" align=\"center\">";
                                            StrRetValue += "<div>Please call <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> for additional assistance.</div>";
                                            StrRetValue += "</td>";
                                            StrRetValue += "</tr>";

                                            StrRetValue += "<tr>";
                                            StrRetValue += "<td valign=\"middle\" align=\"center\">&nbsp;</td>";
                                            StrRetValue += "</tr>";
                                            StrRetValue += "</tbody></table>";
                                            StrRetValue += "</div>";
                                            StrRetValue += "</div>";

                                            Response.Write("<div style='color:cc0000;display:none;'>Not Sufficient Inventory</div>" + StrRetValue.ToString());
                                            Response.End();
                                            return;
                                        }
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                    else
                                    {
                                        ViewState["AvailableDate"] = StrVendor;
                                    }



                                    //Yardqty = LengthStdAllow * 
                                    //VariantNameId = VariantNameId + "Yardage Required,";
                                    //// VariantValueIdtemp = VariantValueIdtemp + LengthStdAllow.ToString() + ",";
                                    if (ShoppingCartQty == 0)
                                    {
                                        VariantValueId = VariantValueIdtemp;
                                    }

                                }
                                else
                                {
                                    if (ProductTypeID.ToString() == "3" && RomanShadeId == 0)
                                    {
                                        string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Qty + "," + strPIds[j].ToString() + ",'" + VariantNames.ToString().Replace("~hpd~", "-") + "','" + VariantValues.ToString().Replace("~hpd~", "-") + "'"));
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                }
                                if (ViewState["AvailableDate"] != null)
                                {
                                    try
                                    {
                                        avail = Convert.ToString(ViewState["AvailableDate"]);
                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                    }
                                    catch { }
                                }
                                Int32 IsSwatchproduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + strPIds[j].ToString() + " and ItemType='Swatch'"));
                                if (avail != "" && IsSwatchproduct != 1)
                                {
                                    VariantNameId = VariantNameId + "Estimated Delivery,";
                                    VariantValueId = VariantValueId + avail.ToString() + ",";
                                }
                                if (!string.IsNullOrEmpty(strDatenew))
                                {
                                    VariantNameId = VariantNameId + "Back Order,";
                                    VariantValueId = VariantValueId + "Yes,";
                                }

                                string strResult = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(strPIds[j].ToString()), Qty, price, "", "", VariantNameId, VariantValueId, 0);
                                if (strResult == "success")
                                {
                                    IsAddeDCart = "success";
                                }
                                string strSKU = Convert.ToString(CommonComponent.GetScalarCommonData("select RelatedProduct from tb_Product where productid=" + ProductID + " and StoreId=-1"));
                                DataSet ds = new DataSet();
                                if (!String.IsNullOrEmpty(ProductTypeID))
                                {
                                    Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + strPIds[j].ToString() + " and ItemType='Swatch'"));
                                    if (Isorderswatch == 1)
                                    {
                                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', IsProductType=0 WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    }
                                    else if (ProductTypeID.ToString() == "3")
                                    {
                                        //CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', VariantValues='" + VariantValueIdtemp.ToString().Replace("'", "''").Replace("~hpd~", "-") + "', IsProductType=" + ProductTypeID.ToString() + " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "',  IsProductType=" + ProductTypeID.ToString() + " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    }
                                    else
                                    {
                                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', IsProductType=" + ProductTypeID.ToString() + " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    }
                                }
                                else
                                {

                                    CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', IsProductType=0 WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");

                                }

                                ds = CommonComponent.GetCommonDataSet("select SKU,ProductId from tb_Product where isnull(active,0)=1 AND  isnull(deleted,0)=0 AND Storeid=" + AppLogic.AppConfigs("Storeid").ToString() + " and isnull(sku,'') <>'' and  SKU in (select items from dbo.Split ('" + strSKU.ToString() + "',','))");
                                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                    {
                                        string strResult1 = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(ds.Tables[0].Rows[k]["ProductId"].ToString()), Qty, 0, "", "", "", "", Convert.ToInt32(strPIds[j].ToString()));

                                    }
                                }

                                check = true;
                            }
                        }
                        if (check)
                        {
                            //clsMiniCartComponent objMiniCart = new clsMiniCartComponent(Convert.ToInt32(Session["CustID"]));
                            //Response.Clear();
                            //Response.Write(objMiniCart.GetMiniCart());
                            //Response.End();
                        }

                    }

                }
            }
            else
            {

            }
        }

        /// <summary>
        /// Checks the Inventory.
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="CustomerId">int CustomerId</param>
        /// <param name="Qty">int Qty</param>
        /// <returns>Returns true if sufficient Product Available, false otherwise</returns>
        private bool CheckInventory(Int32 ProductID, Int32 CustomerId, Int32 Qty, String ProductTypeID, String VariantNames, String VariantValues)
        {

            Int32 TotalQuantityValue = 0;

            int Yardqty = 0;
            double actualYard = 0;
            string strDatenew = "";
            strDatenew = "";
            if (!String.IsNullOrEmpty(VariantValues) && VariantValues.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
            {
                Qty = Qty * 2;
            }

            Int32 AssemblyProduct = 0, ReturnQty = 0;
            AssemblyProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect COUNT(*) From tb_product Where ProductId=" + ProductID + " and StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " and ProductTypeID in (Select ProductTypeID From tb_ProductType where Name='Assembly Product')"));
            //if (AssemblyProduct > 0)
            //{
            //    ReturnQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_Check_ProductAssemblyInventory " + ProductID + "," + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + "," + CustomerId + ",1"));
            //    TotalQuantityValue = ReturnQty;
            //    if (ReturnQty <= 0)
            //    {
            //        TotalQuantityValue = 0;
            //        return false;
            //    }
            //    else if (Qty > ReturnQty)
            //    {
            //        return false;
            //    }
            //    else
            //    { return true; }
            //}
            //else
            {
                Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(sum(ISNULL(Quantity,0)),0) FROM tb_ShoppingCartItems " +
                                                     " WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + CustomerId + ") " +
                                                     " AND ProductID=" + ProductID + " AND VariantNames like '" + VariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-") + "%' AND VariantValues like '" + VariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-") + "%'"));
                Qty = Qty + ShoppingCartQty;

                DataSet dscount = new DataSet();
                Int32 alinv = 0;
                if (!String.IsNullOrEmpty(ProductTypeID) && ProductTypeID.ToString() == "1")
                {
                    string[] strNmyard = VariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] strValyeard = VariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //if (strValyeard.Length > 0)
                    //{
                    //    if (strValyeard.Length == strNmyard.Length)
                    //    {
                    //        for (int j = 0; j < strNmyard.Length; j++)
                    //        {
                    //            if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                    //            {
                    //                if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                    //                {
                    //                    string strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                    //                    dscount = CommonComponent.GetCommonDataSet("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + " AND AllowQuantity >= " + Qty + "");
                    //                    alinv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                    //                }
                    //                else
                    //                {
                    //                    string strvalue = strValyeard[j].ToString().Trim();
                    //                    dscount = CommonComponent.GetCommonDataSet("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + " AND AllowQuantity >= " + Qty + "");
                    //                    alinv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                    //                }
                    //            }

                    //        }
                    //    }
                    //}

                }
                else
                {
                    dscount = CommonComponent.GetCommonDataSet("SELECT ISNULL(Inventory,0) as Inventory FROM tb_product WHERE ProductId=" + ProductID + " AND Inventory >= " + Qty + "");
                }
                if (dscount != null && dscount.Tables.Count > 0 && dscount.Tables[0].Rows.Count > 0)
                {
                    #region swatch-hemming
                    if (!String.IsNullOrEmpty(ProductTypeID) && ProductTypeID.ToString() == "0")
                    {

                        Int32 IsHemming = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,0) as ConfigValue from tb_AppConfig where ConfigName='IshemmingActive'"));
                        if (IsHemming == 1)
                        {
                            int SwatchHemmInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(HammingSafetyQty,0) as HammingSafetyQty FROM tb_product WHERE isnull(IsHamming,0)=1 and ProductId=" + ProductID + ""));
                            int CntInv = Convert.ToInt32(dscount.Tables[0].Rows[0][0].ToString()) - SwatchHemmInv;
                            if (CntInv >= Qty)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }

                    #endregion
                }
                else
                {
                    if (!String.IsNullOrEmpty(ProductTypeID) && ProductTypeID.ToString() == "1")
                    {
                        //CommonComponent.ExecuteCommonData("INSERT INTO temp_Qty(IDname,Qty) values ('" + Request.QueryString["VariantValues"].ToString().Replace("'", "''") + "','" + TotalQuantityValue.ToString() + "')");
                        string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + ProductID.ToString() + ""));
                        if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder))
                        {

                            string[] strNmyard = VariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] strValyeard = VariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strValyeard.Length > 0)
                            {
                                if (strValyeard.Length == strNmyard.Length)
                                {
                                    for (int j = 0; j < strNmyard.Length; j++)
                                    {
                                        if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                                        {
                                            if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                                            {
                                                string strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                                                strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                                strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue + "' AND  ProductId=" + ProductID + ""));

                                                int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue + "' AND  ProductId=" + ProductID + ""));
                                                //TotalQuantityValue = Convert.ToInt32(CntInv);

                                                DataSet dsUPC = new DataSet();
                                                dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue + "' AND ProductId=" + ProductID + "");
                                                string upc = "";
                                                string Skuoption = "";
                                                if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                                {
                                                    upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                    Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                    if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                    {
                                                        string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                        if (!string.IsNullOrEmpty(strQty))
                                                        {
                                                            try
                                                            {
                                                                TotalQuantityValue = Convert.ToInt32(strQty.ToString());
                                                                //CommonComponent.ExecuteCommonData("INSERT INTO temp_Qty(IDname,Qty) values ('" + strvalue.Replace("'", "''") + "','" + TotalQuantityValue.ToString() + "')");
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                }


                                                if (TotalQuantityValue <= 0)
                                                {
                                                    TotalQuantityValue = 0;
                                                }
                                                string resp = "";
                                                DataSet dsYard = new DataSet();
                                                string Length = "";
                                                if (strvalue.ToLower().IndexOf("x84l") > -1 || strvalue.ToLower().IndexOf("x 84l") > -1 || strvalue.ToLower().IndexOf("x84 l") > -1 || strvalue.ToLower().IndexOf("x 84 l") > -1)
                                                {
                                                    Length = "84";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x96l") > -1 || strvalue.ToLower().IndexOf("x 96l") > -1 || strvalue.ToLower().IndexOf("x96 l") > -1 || strvalue.ToLower().IndexOf("x 96 l") > -1)
                                                {
                                                    Length = "96";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x108l") > -1 || strvalue.ToLower().IndexOf("x 108l") > -1 || strvalue.ToLower().IndexOf("x108 l") > -1 || strvalue.ToLower().IndexOf("x 108 l") > -1)
                                                {
                                                    Length = "108";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x120") > -1 || strvalue.ToLower().IndexOf("x 120l") > -1 || strvalue.ToLower().IndexOf("x120 l") > -1 || strvalue.ToLower().IndexOf("x 120 l") > -1)
                                                {
                                                    Length = "120";
                                                }
                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductID + ",50," + Length.ToString() + "," + Qty.ToString() + ",'Pole Pocket','Lined'");
                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                {
                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                    actualYard = Convert.ToDouble(resp.ToString());
                                                }
                                                Int32 OrderQty = Qty;
                                                if (resp != "")
                                                {
                                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                }
                                                Yardqty = OrderQty;
                                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProductID.ToString() + " "));
                                                if (!string.IsNullOrEmpty(strDatenew))
                                                {
                                                    ViewState["AvailableDate"] = strDatenew;
                                                    return true;
                                                }
                                                else if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                {

                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return false;
                                                }
                                                else
                                                {
                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return true;
                                                }
                                            }
                                            else
                                            {
                                                string strvalue = strValyeard[j].ToString().Trim();
                                                strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                                int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue + "' AND  ProductId=" + ProductID + ""));
                                                //TotalQuantityValue = Convert.ToInt32(CntInv);
                                                strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue + "' AND  ProductId=" + ProductID + ""));
                                                DataSet dsUPC = new DataSet();
                                                dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue + "' AND  ProductId=" + ProductID + "");
                                                string upc = "";
                                                string Skuoption = "";
                                                if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                                {
                                                    upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                    Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                    if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                    {
                                                        string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                        if (!string.IsNullOrEmpty(strQty))
                                                        {
                                                            try
                                                            {
                                                                TotalQuantityValue = Convert.ToInt32(strQty.ToString());
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                }

                                                if (TotalQuantityValue <= 0)
                                                {
                                                    TotalQuantityValue = 0;
                                                }
                                                string resp = "";
                                                DataSet dsYard = new DataSet();
                                                string Length = "";
                                                if (strvalue.ToLower().IndexOf("x84l") > -1 || strvalue.ToLower().IndexOf("x 84l") > -1 || strvalue.ToLower().IndexOf("x84 l") > -1 || strvalue.ToLower().IndexOf("x 84 l") > -1)
                                                {
                                                    Length = "84";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x96l") > -1 || strvalue.ToLower().IndexOf("x 96l") > -1 || strvalue.ToLower().IndexOf("x96 l") > -1 || strvalue.ToLower().IndexOf("x 96 l") > -1)
                                                {
                                                    Length = "96";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x108l") > -1 || strvalue.ToLower().IndexOf("x 108l") > -1 || strvalue.ToLower().IndexOf("x108 l") > -1 || strvalue.ToLower().IndexOf("x 108 l") > -1)
                                                {
                                                    Length = "108";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x120") > -1 || strvalue.ToLower().IndexOf("x 120l") > -1 || strvalue.ToLower().IndexOf("x120 l") > -1 || strvalue.ToLower().IndexOf("x 120 l") > -1)
                                                {
                                                    Length = "120";
                                                }
                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductID + ",50," + Length.ToString() + "," + Qty.ToString() + ",'Pole Pocket','Lined'");
                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                {
                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                    actualYard = Convert.ToDouble(resp.ToString());
                                                }
                                                Int32 OrderQty = Qty;
                                                if (resp != "")
                                                {
                                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                }
                                                Yardqty = OrderQty;
                                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProductID.ToString() + " "));
                                                if (!string.IsNullOrEmpty(strDatenew))
                                                {
                                                    ViewState["AvailableDate"] = strDatenew;
                                                    return true;
                                                }
                                                else if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                {
                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return false;
                                                }
                                                else
                                                {
                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return true;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {

                            string[] strNmyard = VariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] strValyeard = VariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strValyeard.Length > 0)
                            {
                                if (strValyeard.Length == strNmyard.Length)
                                {
                                    int warehouseId = 0;
                                    for (int j = 0; j < strNmyard.Length; j++)
                                    {
                                        if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                                        {
                                            string strvalue = "";
                                            if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                                            {
                                                strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                                            }
                                            else
                                            {
                                                strvalue = strValyeard[j].ToString().Trim();
                                            }
                                            strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                            strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue + "' AND  ProductId=" + ProductID + ""));
                                            DataSet dsUPC = new DataSet();
                                            dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU,VariantValueID FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue + "' AND  ProductId=" + ProductID + "");
                                            string upc = "";
                                            string Skuoption = "";
                                            string Variantvalueid = "";
                                            if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                            {
                                                upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                Variantvalueid = Convert.ToString(dsUPC.Tables[0].Rows[0]["VariantValueID"].ToString());
                                                warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductVariantInventory inner join tb_WareHouse on tb_WareHouseProductVariantInventory.WareHouseID=tb_WareHouse.WareHouseID where VariantValueID=" + Convert.ToInt32(Variantvalueid) + " and tb_WareHouseProductVariantInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                if (warehouseId == 0)
                                                {
                                                    warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID in (" + ProductID + " ) and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                }
                                                if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                {
                                                    string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                    if (!string.IsNullOrEmpty(strQty))
                                                    {
                                                        try
                                                        {
                                                            TotalQuantityValue = Convert.ToInt32(strQty.ToString());
                                                            // CommonComponent.ExecuteCommonData("INSERT INTO temp_Qty(IDname,Qty) values ('" + strvalue.Replace("'", "''") + "','" + TotalQuantityValue.ToString() + "')");
                                                        }
                                                        catch { }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (strNmyard.Length == 1 && strNmyard[0].ToString().ToLower().IndexOf("estimated") > -1)
                                    {
                                        int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) as Inventory FROM tb_product WHERE ProductId=" + ProductID + ""));
                                        TotalQuantityValue = Convert.ToInt32(CntInv);
                                        warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID =" + ProductID + " and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                        if (warehouseId == 17)
                                        {
                                            TotalQuantityValue = 9999;
                                        }

                                    }
                                    if (warehouseId == 17)
                                    {
                                        TotalQuantityValue = 9999;
                                    }
                                }
                            }
                            else
                            {
                                int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) as Inventory FROM tb_product WHERE ProductId=" + ProductID + ""));
                                TotalQuantityValue = Convert.ToInt32(CntInv);
                                int warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID =" + ProductID + " and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                if (warehouseId == 17)
                                {
                                    TotalQuantityValue = 9999;
                                }
                            }
                            if (TotalQuantityValue <= 0)
                            {
                                TotalQuantityValue = 0;
                            }
                            if (!string.IsNullOrEmpty(strDatenew))
                            {
                                ViewState["AvailableDate"] = strDatenew;
                                return true;
                            }
                            else if (TotalQuantityValue >= Qty)
                            {
                                // CommonComponent.ExecuteCommonData("INSERT INTO temp_Qty(IDname,Qty) values ('Qty:" + Qty.ToString().Replace("'", "''") + "','" + TotalQuantityValue.ToString() + "')");
                                return true;
                            }

                            //TotalQuantityValue = alinv;
                            return false;
                        }
                    }
                    else
                    {
                        int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) as Inventory FROM tb_product WHERE ProductId=" + ProductID + ""));
                        #region swatch-hemming
                        if (!String.IsNullOrEmpty(ProductTypeID) && ProductTypeID.ToString() == "0")
                        {

                            Int32 IsHemming = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,0) as ConfigValue from tb_AppConfig where ConfigName='IshemmingActive'"));
                            if (IsHemming == 1)
                            {
                                int SwatchHemmInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(HammingSafetyQty,0) as HammingSafetyQty FROM tb_product WHERE isnull(IsHamming,0)=1 and  ProductId=" + ProductID + ""));
                                CntInv = CntInv - SwatchHemmInv;
                            }
                        }

                        #endregion

                        TotalQuantityValue = Convert.ToInt32(CntInv);
                        if (TotalQuantityValue <= 0)
                        {
                            TotalQuantityValue = 0;
                        }
                        if (!String.IsNullOrEmpty(ProductTypeID) && ProductTypeID.ToString() == "3")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
            }
        }


    }
}