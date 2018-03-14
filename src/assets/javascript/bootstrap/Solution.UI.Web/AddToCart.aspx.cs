using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.Common;
using System.IO;
using Solution.ShippingMethods;
using System.Text;

namespace Solution.UI.Web
{
    public partial class AddToCart : System.Web.UI.Page
    {
        bool ChkQtyDiscount = false;

        #region Declaration
        ShoppingCartComponent objCart = new ShoppingCartComponent();
        public decimal GrandSubTotal = decimal.Zero;
        public static decimal FinalTotal = decimal.Zero;
        public static decimal GiftCardTotalDiscount = decimal.Zero;
        public static decimal FreeShippingAmount = decimal.Zero;
        public decimal ShippingCharges = decimal.Zero;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</pa
        /// 


        Decimal SwatchQty = Decimal.Zero;

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            lblMsg.Text = "";

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("FreeShippingLimit").ToString().Trim()))
                {
                    FreeShippingAmount = Convert.ToDecimal(AppLogic.AppConfigs("FreeShippingLimit"));
                }
                else
                {
                }
                Session["CouponCode"] = null;
                Session["Discount"] = null;
                Session["QtyDiscount"] = null;
                Session["GiftCertificateDiscountCode"] = null;
                Session["GiftCertificateDiscount"] = null;
                Session["GiftCertificateRemaningBalance"] = null;
                Session["CategoryId"] = null; Session["ParentCategoryId"] = null;

                ViewState["ShippingCharges"] = 0;
                ViewState["FinalTotal"] = 0;
                Page.MaintainScrollPositionOnPostBack = true;
                FillCountry();
                BindShoppingCartByCustomerID();
                if (ViewState["Urlrefferer"] == null && Request.UrlReferrer != null)
                {
                    ViewState["Urlrefferer"] = Request.UrlReferrer.ToString();
                    if (ViewState["Urlrefferer"].ToString().ToLower().IndexOf("quickview.aspx") > -1)
                    {
                        ViewState["Urlrefferer"] = "/index.aspx";
                    }
                }

                BindCrossSellProducts();
                Response.Redirect("/CheckoutCommon.aspx");
            }
        }

        /// <summary>
        /// Method to bind cart items by customer id
        /// </summary>
        private void BindShoppingCartByCustomerID()
        {

            ViewState["Weight"] = null;
            if (Session["CustID"] != null)
            {
                //String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC) "));
                //if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
                //{
                SwatchQty = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                //}
                DataSet dsShoppingCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));

                if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
                {
                    string strSQL1 = "";
                    string strSql = "";
                    DataSet ds = new DataSet();
                    //strSQL1 = "select RelatedProduct from tb_Product where productid=" + dsShoppingCart.Tables[0].Rows[0]["Productid"].ToString() + " and  RelatedProduct in (select items from dbo.Split ('" + dsShoppingCart.Tables[0].Rows[0]["RelatedProduct"].ToString() + "',','))";
                    //ds = CommonComponent.GetCommonDataSet(strSQL1);
                    //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    //{
                    //    strSQL1 = "SELECT p.ProductID, p.SKU,p.Name, p.Sename,p.ImageName, isnull(p.weight,1) as weight FROM tb_Product p WHERE  p.ProductID in (select productid from tb_product where Storeid=" + AppLogic.AppConfigs("StoreID") + " and sku='" + AppLogic.AppConfigs("FreeProductSKU1") + "')";
                    //    dsShoppingCart = CommonComponent.GetCommonDataSet(strSQL1);

                    //    strSql = "insert into tb_ShoppingCartItems(ShoppingCartID,ProductID,Quantity,Price,CategoryID,Weight) values (" + ShoppingCartIDFreeProduct + "," + dsShoppingCart.Tables[0].Rows[0]["ProductID"] + ",1,0.00,0," + dsShoppingCart.Tables[0].Rows[0]["Weight"] + ")";
                    //    CommonComponent.ExecuteCommonData(strSql);
                    //}
                    string strProduct = "";
                    for (int i = 0; i < dsShoppingCart.Tables[0].Rows.Count; i++)
                    {
                        strProduct += Convert.ToString(dsShoppingCart.Tables[0].Rows[i]["Productid"].ToString()) + ",";
                    }

                    ViewState["AllProductsSwatch"] = null;
                    int SwatchCnt = 0;
                    if (!string.IsNullOrEmpty(strProduct.Trim()) && strProduct.Length > 0)
                    {
                        strProduct = strProduct.Substring(0, strProduct.Length - 1);
                        SwatchCnt = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect Count(ProductID) as Isfreefabricswatch from tb_product where ProductID in(" + strProduct + ")   and StoreID = 1 and ItemType='Swatch'"));
                        if (dsShoppingCart.Tables[0].Rows.Count == SwatchCnt)
                        {
                            ViewState["AllProductsSwatch"] = "1";
                        }
                    }

                    GrandSubTotal = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[0]["SubTotal"].ToString());
                    ViewState["SubTotal"] = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[0]["SubTotal"].ToString());
                    Session["NoOfCartItems"] = dsShoppingCart.Tables[0].Rows[0]["TotalItems"].ToString();
                    GetYoumayalsoLikeProduct(Convert.ToInt32(dsShoppingCart.Tables[0].Rows[0]["ProductId"].ToString()));
                    RptCartItems.DataSource = dsShoppingCart;
                    RptCartItems.DataBind();
                    Int32 items = Convert.ToInt32(dsShoppingCart.Tables[0].Rows[0]["TotalItems"].ToString());
                    System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)Page.Master.FindControl("cartlink");
                    //objAnchor.InnerHtml = items > 1 ? "Items (" + items.ToString("D2") + ")" : "Item (" + items.ToString("D2") + ")";
                    //objAnchor.InnerHtml = items > 1 ? "<span>" + items.ToString("D2") + "</span> <strong>Item(s)</strong>" : "<span>" + items.ToString("D2") + "</span> <strong>Item(s)</strong>";

                    if (items > 1)
                    {
                        objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " items)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(ViewState["SubTotal"].ToString())) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                    }
                    else
                    {
                        objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " item)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(ViewState["SubTotal"].ToString())) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                    }

                    objAnchor.HRef = "/addtocart.aspx";

                    //btnClearCart.Visible = true;
                    btnUpdateCart.Visible = true;
                    lblMessage.Text = "";
                    if (ChkQtyDiscount == true)
                    {
                        trPromocode.Visible = false;
                    }
                    else
                    {
                        trPromocode.Visible = true;
                    }
                    CheckHotDealProduct();
                }
                else
                {
                    GetYoumayalsoLikeProduct(0);
                    Session["NoOfCartItems"] = null;
                    RptCartItems.DataSource = null;
                    RptCartItems.DataBind();
                    btnClearCart.Visible = false;
                    btnUpdateCart.Visible = false;
                    trPromocode.Visible = false;
                    trShopping.Visible = false;
                    trCalculateShippingHeader.Visible = false;
                    lblMessage.Text = "Your Shopping Cart is Empty!";
                    lblFreeShippningMsg.Visible = false;
                    Int32 items = 0;
                    System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)Page.Master.FindControl("cartlink");
                    //objAnchor.InnerHtml = items > 1 ? "Items (" + items.ToString("D2") + ")" : "Item (" + items.ToString("D2") + ")";
                    //objAnchor.InnerHtml = items > 1 ? "<span>" + items.ToString("D2") + "</span> <strong>Item(s)</strong>" : "<span>" + items.ToString("D2") + "</span> <strong>Item(s)</strong>";
                    if (items > 1)
                    {
                        objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " items)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(0)) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                    }
                    else
                    {
                        objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " item)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(0)) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                    }
                    objAnchor.HRef = "/addtocart.aspx";

                }

            }
            else
            {
                GetYoumayalsoLikeProduct(0);
                Session["NoOfCartItems"] = null;
                lblMessage.Text = "Your Shopping Cart is Empty!";
                Session["CouponCode"] = null;
                Session["CouponCodebycustomer"] = null;
                Session["CouponCodeDiscountPrice"] = null;
                btnClearCart.Visible = false;
                btnUpdateCart.Visible = false;
                trPromocode.Visible = false;
                trShopping.Visible = false;
                trCalculateShippingHeader.Visible = false;
                Int32 items = 0;
                System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)Page.Master.FindControl("cartlink");
                //objAnchor.InnerHtml = items > 1 ? "Items (" + items.ToString("D2") + ")" : "Item (" + items.ToString("D2") + ")";
                // objAnchor.InnerHtml = items > 1 ? "<span>" + items.ToString("D2") + "</span> <strong>Item(s)</strong>" : "<span>" + items.ToString("D2") + "</span> <strong>Item(s)</strong>";

                if (items > 1)
                {
                    objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " items)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(0)) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                }
                else
                {
                    objAnchor.InnerHtml = "<p><span class='navQty'> (" + items.ToString("D2") + " item)</span><span class='navTotal'> $" + String.Format("{0:0.00}", Convert.ToDecimal(0)) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon' />";
                }

                objAnchor.HRef = "/addtocart.aspx";
            }
        }

        /// <summary>
        /// Method to Set Product Image
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Image</returns>
        public String GetIconImage(String img)
        {
            clsvariables.LoadAllPath();
            String[] AllowedExtensions = AppLogic.AppConfigs("AllowedExtensions").Split(',');
            String imagepath = String.Empty;
            Random rd = new Random();
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Micro/" + img;
            // imagepath = strImgPath;
            if (img != "")
            {
                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (imagepath)))
                {
                    return AppLogic.AppConfigs("Live_Contant_Server") + imagepath + "?" + rd.Next(1000).ToString();
                }
            }
            else
            {
                return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
            }

            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
        }

        /// <summary>
        /// Check Hot Deal Product
        /// </summary>
        private void CheckHotDealProduct()
        {
            #region Code for Check Hot Deal Product for Hide Save To Cart Button
            try
            {
                if (RptCartItems.Items.Count == 1)
                {
                    HiddenField hdnProductId = (HiddenField)RptCartItems.Items[0].FindControl("hdnProductId");
                    if (hdnProductId.Value.ToString() == AppLogic.AppConfigs("HotDealProduct").ToString())
                    {
                        btnClearCart.Visible = false;
                    }
                    else
                    {
                        btnClearCart.Visible = true;
                    }
                }
            }
            catch { btnClearCart.Visible = true; }

            #endregion
        }

        /// <summary>
        /// Cart Item Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void RptCartItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox txtQty = (TextBox)e.Item.FindControl("txtQty");
                Literal ltrSubTotal = (Literal)e.Item.FindControl("ltrSubTotal");
                Label lblNettotal = (Label)e.Item.FindControl("lblNettotal");
                HiddenField IndiSubTotal = (HiddenField)e.Item.FindControl("hdnIndTotal");
                HiddenField hdnprice = (HiddenField)e.Item.FindControl("hdnprice");
                HiddenField hdnRelatedproductID = (HiddenField)e.Item.FindControl("hdnRelatedproductID");
                LinkButton lbtndelete = (LinkButton)e.Item.FindControl("lbtndelete");
                System.Web.UI.HtmlControls.HtmlTableCell tdSku = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdSku");
                HiddenField hdnCustomcartId = (HiddenField)e.Item.FindControl("hdnCustomcartId");
                System.Web.UI.HtmlControls.HtmlAnchor lnkProductName = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lnkProductName");
                Label lblFreeProductName = (Label)e.Item.FindControl("lblFreeProductName");
                HiddenField hdnProductId = (HiddenField)e.Item.FindControl("hdnProductId");
                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                HiddenField hdnswatchqty = (HiddenField)e.Item.FindControl("hdnswatchqty");
                HiddenField hdnswatchtype = (HiddenField)e.Item.FindControl("hdnswatchtype");

                //" + hdnProductId.Value.ToString() + "
                if (hdnRelatedproductID.Value.ToString() != "0")
                {
                    lbtndelete.Visible = false;
                    txtQty.Enabled = false;
                    lblFreeProductName.Visible = true;
                }
                else
                {
                    lnkProductName.Visible = true;
                    lnkProductName.HRef = "/" + DataBinder.Eval(e.Item.DataItem, "ProductURL");
                }

                //Response.Write(SwatchQty.ToString());
                Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + hdnProductId.Value.ToString() + " and ItemType='Swatch'"));
                if (SwatchQty > Decimal.Zero)
                {
                    if (Isorderswatch == 1)
                    {
                        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + txtQty.Text.ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + hdnProductId.Value.ToString() + ""));
                        if (Convert.ToDecimal(pp) >= SwatchQty)
                        {
                            if (Convert.ToDecimal(pp) >= Convert.ToDecimal(SwatchQty))
                            {
                                lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty)) / Convert.ToDecimal(txtQty.Text.ToString())));
                                lblNettotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty)));
                                hdnprice.Value = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty)) / Convert.ToDecimal(txtQty.Text.ToString())));
                                GrandSubTotal += Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty));
                                hdnswatchqty.Value = txtQty.Text.ToString();
                                hdnswatchtype.Value = "0";
                                SwatchQty = Decimal.Zero;
                            }
                            else
                            {
                                lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                                lblNettotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                                GrandSubTotal += Convert.ToDecimal(0);
                                hdnprice.Value = String.Format("{0:0.00}", Convert.ToDecimal(0));
                                hdnswatchqty.Value = txtQty.Text.ToString();
                                hdnswatchtype.Value = "0";
                                SwatchQty = SwatchQty - Convert.ToDecimal(pp.ToString());
                            }

                        }
                        else
                        {

                            //if (pp > Decimal.Zero)
                            //{
                            lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                            lblNettotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                            GrandSubTotal += Convert.ToDecimal(0);
                            hdnprice.Value = String.Format("{0:0.00}", Convert.ToDecimal(0));
                            hdnswatchqty.Value = txtQty.Text.ToString();
                            hdnswatchtype.Value = "0";
                            SwatchQty = SwatchQty - Convert.ToDecimal(pp.ToString());
                            //}
                        }
                    }


                }
                else
                {
                    if (Isorderswatch == 1)
                    {
                        Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + txtQty.Text.ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + hdnProductId.Value.ToString() + ""));
                        if (Convert.ToDecimal(pp) >= Convert.ToDecimal(SwatchQty))
                        {
                            lblPrice.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) / Convert.ToDecimal(txtQty.Text.ToString())));
                            lblNettotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(pp.ToString()));
                            hdnprice.Value = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) / Convert.ToDecimal(txtQty.Text.ToString())));
                            GrandSubTotal += Convert.ToDecimal(pp.ToString());
                            hdnswatchqty.Value = txtQty.Text.ToString();
                            hdnswatchtype.Value = "0";

                        }
                    }
                }


                string strType = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Isnull(IsProductType,1)  FROM tb_ShoppingCartItems WHERE CustomCartID=" + hdnCustomcartId.Value.ToString() + ""));
                // lnkProductName.HRef = GetProductUrl(Convert.ToString(DataBinder.Eval(e.Item.DataItem, "mainCategory")), Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Sename")), Convert.ToString(DataBinder.Eval(e.Item.DataItem, "ProductId")), Convert.ToString(DataBinder.Eval(e.Item.DataItem, "CustomCartID")));
                if (strType.ToString() == "2")
                {
                    string strSku = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 SKU FROM tb_ProductVariantValue WHERE ProductID = " + hdnProductId.Value.ToString() + " AND VariantValue like '%custom%'"));
                    if (strSku != "")
                    {
                        tdSku.InnerHtml = strSku.ToString();
                    }
                }




                if (txtQty != null)
                {
                    txtQty.Attributes.Add("onkeypress", "return isNumberKey(event)");
                }
                HiddenField hdnIndTotal = (HiddenField)e.Item.FindControl("hdnIndTotal");

                #region customer coupon code
                if (Session["CouponCodebycustomer"] != null && Session["CouponCodeDiscountPrice"] != null)
                {
                    //if (txtPromoCode != null && txtPromoCode.Text != "")
                    //{
                    decimal DiscountPercent, Discountprice, OrginalPrice, DisplayPrice = decimal.Zero;
                    Discountprice = 0;
                    DiscountPercent = Convert.ToDecimal(Session["CouponCodeDiscountPrice"].ToString());

                    if (DiscountPercent > 0)
                    {
                        System.Web.UI.HtmlControls.HtmlTableCell tdproduct = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdproduct");
                        System.Web.UI.HtmlControls.HtmlTableCell tdSkuattr = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdSku");
                        System.Web.UI.HtmlControls.HtmlTableCell tdqty = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdqty");
                        System.Web.UI.HtmlControls.HtmlTableCell tdDiscountprice = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdDiscountprice");
                        tdproduct.Attributes.Add("style", "width:33% !important;");
                        tdSkuattr.Attributes.Add("style", "width:10% !important;");
                        tdqty.Attributes.Add("style", "width:7% !important;");

                        Label lblDiscountprice = (Label)e.Item.FindControl("lblDiscountprice");
                        if (tdDiscountprice != null)
                        {
                            tdDiscountprice.Visible = true;

                            String strCategory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidForCategory FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CouponCodebycustomer"].ToString() + "'"));
                            String strProduct = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ValidforProduct FROM tb_Coupons WHERE StoreID=1 and CouponCode='" + Session["CouponCodebycustomer"].ToString() + "'"));
                            if (!string.IsNullOrEmpty(strCategory))
                            {
                                DataSet dspp = new DataSet();
                                dspp = CommonComponent.GetCommonDataSet("SELECT ProductId FROM tb_ProductCategory WHERE ProductId=" + hdnProductId.Value.ToString() + " and categoryId in (" + strCategory.Replace(" ", "") + ")");
                                if (dspp != null && dspp.Tables.Count > 0 && dspp.Tables[0].Rows.Count > 0)
                                {
                                    decimal.TryParse(hdnprice.Value.ToString(), out OrginalPrice);
                                    Discountprice = ((OrginalPrice * DiscountPercent)) / 100;
                                    if (DiscountPercent >= 100)
                                    {
                                        DisplayPrice = Discountprice;
                                    }
                                    else
                                    {
                                        DisplayPrice = OrginalPrice - Discountprice;
                                    }
                                }
                                else
                                {
                                    decimal.TryParse(hdnprice.Value.ToString(), out OrginalPrice);
                                    if (!string.IsNullOrEmpty(strProduct))
                                    {
                                        strProduct = "," + strProduct.Replace(" ", "") + ",";
                                        if (strProduct.IndexOf("," + hdnProductId.Value.ToString() + ",") > -1)
                                        {

                                            Discountprice = ((OrginalPrice * DiscountPercent)) / 100;
                                            if (DiscountPercent >= 100)
                                            {
                                                DisplayPrice = Discountprice;
                                            }
                                            else
                                            {
                                                DisplayPrice = OrginalPrice - Discountprice;
                                            }
                                        }
                                        else
                                        {
                                            DisplayPrice = OrginalPrice;
                                        }
                                    }
                                    else
                                    {
                                        DisplayPrice = OrginalPrice;
                                    }
                                }
                                //strCategory = "," + strCategory.Replace(" ", "");

                                //string dscategory = "";
                                //dscategory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT cast(categoRyId as nvarchar(max))+',' FROM tb_category WHERE categoryId in (SELECT categoryId FROM tb_ProductCategory WHERE ProductId=" + hdnProductId.Value.ToString() + ") FOR XML PATH('')"));
                                //dscategory = "," + dscategory;
                                //if(dscategory.IndexOf(","+ ))

                            }
                            else if (!string.IsNullOrEmpty(strProduct))
                            {
                                strProduct = "," + strProduct.Replace(" ", "") + ",";
                                decimal.TryParse(hdnprice.Value.ToString(), out OrginalPrice);
                                if (strProduct.IndexOf("," + hdnProductId.Value.ToString() + ",") > -1)
                                {

                                    Discountprice = ((OrginalPrice * DiscountPercent)) / 100;
                                    if (DiscountPercent >= 100)
                                    {
                                        DisplayPrice = Discountprice;
                                    }
                                    else
                                    {
                                        DisplayPrice = OrginalPrice - Discountprice;
                                    }
                                }
                                else
                                {
                                    DisplayPrice = OrginalPrice;
                                }
                            }
                            else
                            {

                                decimal.TryParse(hdnprice.Value.ToString(), out OrginalPrice);
                                Discountprice = ((OrginalPrice * DiscountPercent)) / 100;
                                if (DiscountPercent >= 100)
                                {
                                    DisplayPrice = Discountprice;
                                }
                                else
                                {
                                    DisplayPrice = OrginalPrice - Discountprice;
                                }

                            }

                            lblDiscountprice.Text = Math.Round(DisplayPrice, 2).ToString();
                            string disprice = string.Format("{0:0.00}", Discountprice);
                            decimal Finalsubtotcouponcode = 0;
                            if (hdnswatchtype.Value.ToString() != "")
                            {
                                Finalsubtotcouponcode = Convert.ToDecimal(DisplayPrice) * Convert.ToDecimal(hdnswatchqty.Value.ToString());
                            }
                            else
                            {
                                Finalsubtotcouponcode = Convert.ToDecimal(DisplayPrice) * Convert.ToDecimal(txtQty.Text.ToString());
                            }
                            ltrSubTotal.Text = string.Format("{0:0.00}", Finalsubtotcouponcode);

                            hdnprice.Value = DisplayPrice.ToString();
                            if (hdnswatchtype.Value.ToString() != "")
                            {
                                GrandSubTotal = GrandSubTotal - Convert.ToDecimal(lblNettotal.Text.ToString());
                                GrandSubTotal += Finalsubtotcouponcode;
                            }
                            else
                            {
                                GrandSubTotal += Finalsubtotcouponcode;
                            }
                            lblNettotal.Text = string.Format("{0:0.00}", Finalsubtotcouponcode);
                            ViewState["SubTotal"] = GrandSubTotal;
                        }

                    }

                }
                #endregion
                Decimal QtyDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SElect ISNULL(qt.DiscountPercent,0) as DiscountPercent from tb_QuantityDiscountTable as qt " +
                                            " inner join tb_QauntityDiscount ON qt.QuantityDiscountID = dbo.tb_QauntityDiscount.QuantityDiscountID  " +
                                            " Where qt.LowQuantity<=" + txtQty.Text.ToString() + " and qt.HighQuantity>=" + txtQty.Text.ToString() + " and tb_QauntityDiscount.QuantityDiscountID in (Select QuantityDiscountID from  " +
                                            " tb_Product Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + hdnProductId.Value.ToString() + ") "));

                decimal Subtotal = Convert.ToDecimal(lblNettotal.Text.ToString());
                if (QtyDiscount > Decimal.Zero)
                {
                    ChkQtyDiscount = true;
                    QtyDiscount = (Convert.ToDecimal(hdnprice.Value.ToString()) * QtyDiscount) / 100;
                    if (!string.IsNullOrEmpty(lblNettotal.Text.ToString().Trim()))
                    {
                        if (QtyDiscount > 0 && Subtotal > 0)
                        {
                            string dd = string.Format("{0:0.00}", QtyDiscount);
                            decimal QtyDis01 = 0;
                            if (hdnswatchtype.Value.ToString() != "")
                            {

                                QtyDis01 = (Convert.ToDecimal(hdnprice.Value) * Convert.ToDecimal(hdnswatchqty.Value.ToString())) - (Convert.ToDecimal(dd) * Convert.ToDecimal(hdnswatchqty.Value.ToString()));
                            }
                            else
                            {
                                QtyDis01 = (Convert.ToDecimal(hdnprice.Value) * Convert.ToDecimal(txtQty.Text.ToString())) - (Convert.ToDecimal(dd) * Convert.ToDecimal(txtQty.Text.ToString()));
                            }
                            ltrSubTotal.Text = "<s>$" + Subtotal + "</s><br />$" + string.Format("{0:0.00}", QtyDis01) + "";
                        }
                        else
                            ltrSubTotal.Text = "$" + Subtotal + "";
                    }
                    if (Session["QtyDiscount"] != null)
                    {
                        string dd = string.Format("{0:0.00}", QtyDiscount);
                        decimal Qtydt = 0;
                        if (hdnswatchtype.Value.ToString() != "")
                        {
                            Qtydt = Convert.ToDecimal(Session["QtyDiscount"].ToString()) + (Convert.ToDecimal(dd) * Convert.ToDecimal(hdnswatchqty.Value.ToString()));

                        }
                        else
                        {
                            Qtydt = Convert.ToDecimal(Session["QtyDiscount"].ToString()) + (Convert.ToDecimal(dd) * Convert.ToDecimal(txtQty.Text.ToString()));
                        }
                        Session["QtyDiscount"] = Qtydt;
                    }
                    else
                    {
                        string dd = string.Format("{0:0.00}", QtyDiscount);
                        if (hdnswatchtype.Value.ToString() != "")
                        {
                            Session["QtyDiscount"] = Convert.ToDouble(dd) * Convert.ToDouble(hdnswatchqty.Value.ToString());
                        }
                        else
                        {
                            Session["QtyDiscount"] = Convert.ToDouble(dd) * Convert.ToDouble(txtQty.Text.ToString());
                        }
                    }
                }
                else
                {
                    ltrSubTotal.Text = "$" + Subtotal + "";
                }
                #region CheckMembership Discount
                String ProductId = hdnProductId.Value.ToString();
                decimal ProductDiscount = 0;
                decimal CategoryDiscount = 0;
                decimal NewDiscount = 0;
                decimal Price = 0;
                bool ChkNoDiscount = false;
                String CategoryId = Convert.ToString(CommonComponent.GetScalarCommonData("select pc.CategoryID  from tb_Product p inner join tb_ProductCategory pc ON p.ProductID=pc.ProductID  where pc.ProductID=" + ProductId + ""));
                decimal.TryParse(hdnprice.Value.ToString(), out Price);
                bool.TryParse(CommonComponent.GetScalarCommonData("SELECT NoDiscount FROM dbo.tb_Customer WHERE CustomerID=" + Session["CustID"].ToString()).ToString(), out ChkNoDiscount);

                ProductDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT ISNULL(md.Discount,0) AS ProductDiscount "
                + " FROM dbo.tb_MembershipDiscount md Left OUTER JOIN dbo.tb_Product Prod ON Prod.ProductID =md.DiscountObjectID " +
                " WHERE md.CustID='" + Session["CustID"].ToString() + "' AND md.DiscountType='product' AND md.StoreID= " + AppLogic.AppConfigs("StoreID").ToString() + " AND md.DiscountObjectID=" + ProductId + ""));
                if (ProductDiscount <= 0)
                {
                    CategoryDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData(" SELECT md.Discount AS CategoryDiscount "
                    + " FROM dbo.tb_MembershipDiscount md LEFT OUTER JOIN dbo.tb_Category cat ON cat.CategoryID =md.DiscountObjectID   "
                    + "WHERE md.DiscountType='category' AND  md.CustID= " + Session["CustID"].ToString() + "  AND md.storeid=" + AppLogic.AppConfigs("StoreID").ToString() + " AND md.DiscountObjectID=" + CategoryId + " "));
                    NewDiscount = CategoryDiscount;
                }
                else
                {
                    NewDiscount = ProductDiscount;
                }
                if (NewDiscount > 0)
                {
                    if (hdnswatchtype.Value.ToString() != "")
                    {
                        NewDiscount = (((Price * Convert.ToDecimal(hdnswatchqty.Value.ToString())) * NewDiscount) / 100);
                    }
                    else
                    {
                        NewDiscount = (((Price * Convert.ToDecimal(txtQty.Text.ToString())) * NewDiscount) / 100);
                    }
                    if (Session["QtyDiscount1"] != null)
                    {
                        Session["QtyDiscount1"] = NewDiscount + Convert.ToDecimal(Session["QtyDiscount1"].ToString());
                    }
                    else
                    {
                        Session["QtyDiscount1"] = NewDiscount;
                    }
                }

                #endregion

                DataSet dsWeight = new DataSet();
                dsWeight = ProductComponent.GetproductImagename(Convert.ToInt32(hdnProductId.Value.ToString()));
                decimal objdecimal = decimal.Zero;
                if (dsWeight != null && dsWeight.Tables.Count > 0 && dsWeight.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dsWeight.Tables[0].Rows[0]["IsFreeShipping"]))
                    {
                        objdecimal = 0;
                    }
                    else
                    {
                        //objdecimal = Convert.ToDecimal(dsWeight.Tables[0].Rows[0]["weight"].ToString());
                        if (Convert.ToDecimal(dsWeight.Tables[0].Rows[0]["weight"].ToString()) > 0)
                            objdecimal = (Convert.ToDecimal(dsWeight.Tables[0].Rows[0]["weight"].ToString()) * Convert.ToDecimal(txtQty.Text));
                        else
                        {
                            objdecimal = (Convert.ToDecimal(1) * Convert.ToDecimal(txtQty.Text));
                        }
                    }
                }
                int GiftCardProductID = 0;
                GiftCardProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(GiftCardProductID,0) FROM dbo.tb_GiftCardProduct Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + hdnProductId.Value.ToString() + ""));

                //if (GiftCardProductID > 0) { }
                //else
                //{
                //    if (ViewState["Weight"] != null)
                //    {
                //        objdecimal += (Convert.ToDecimal(ViewState["Weight"].ToString()) * Convert.ToDecimal(txtQty.Text));
                //        ViewState["Weight"] = objdecimal.ToString();
                //    }
                //    else
                //    {
                //        ViewState["Weight"] = objdecimal.ToString();
                //    }
                //}

                HiddenField hdnVariantvalue = (HiddenField)e.Item.FindControl("hdnVariantvalue");
                HiddenField hdnVariantname = (HiddenField)e.Item.FindControl("hdnVariantname");
                string[] variantValue = hdnVariantvalue.Value.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantName = hdnVariantname.Value.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (strType.ToString() == "3")
                {
                    for (int pp = 0; pp < variantName.Length; pp++)
                    {

                        //if (variantName[pp].ToString().ToLower().IndexOf("color") > -1)
                        //{
                        //    tdSku.InnerHtml = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE VariantValue='" + variantValue[pp].ToString().Trim().Replace("'", "''") + "' and ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + ""));
                        //    break;
                        //}

                    }
                }
                int variantValueCount = variantValue.Count();
                int variantNameCount = variantName.Count();
                decimal optionweight = 0;
                if (strType.ToString() == "2")
                {
                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and SKU='" + tdSku.InnerHtml.ToString().Trim() + "'"));
                }
                else if (strType.ToString() == "3")
                {
                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and SKU='" + tdSku.InnerHtml.ToString().Trim() + "'"));
                }
                else if (strType.ToString() == "1")
                {
                    for (int i = 0; i < variantValueCount; i++)
                    {
                        if (variantNameCount > i)
                        {
                            if (variantName[i].ToString().ToLower().IndexOf("select size") > -1)
                            {
                                if (variantValue[i].ToString().IndexOf("($") > -1)
                                {
                                    string sttrval = variantValue[i].ToString().Substring(0, variantValue[i].ToString().IndexOf("($"));
                                    sttrval = sttrval.Replace("(Buy 1 Get 1 Free)", "");
                                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));
                                    string strSKu = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));
                                    if (strSKu != "")
                                    {
                                        tdSku.InnerHtml = strSKu;
                                    }
                                    break;
                                }
                                else
                                {
                                    string sttrval = variantValue[i].ToString();
                                    sttrval = sttrval.Replace("(Buy 1 Get 1 Free)", "");
                                    optionweight = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Weight,0) FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));
                                    string strSKu = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(SKU,'') FROM tb_ProductVariantValue WHERE ProductId=" + Convert.ToInt32(hdnProductId.Value.ToString()) + " and VariantValue like '%" + sttrval.ToString().Trim() + "%'"));
                                    if (strSKu != "")
                                    {
                                        tdSku.InnerHtml = strSKu;
                                    }
                                    break;
                                }
                            }

                        }
                    }


                }

                if (GiftCardProductID > 0) { }
                else
                {
                    if (ViewState["Weight"] != null)
                    {
                        if (optionweight > Decimal.Zero)
                        {
                            objdecimal += (Convert.ToDecimal(optionweight.ToString()) * Convert.ToDecimal(txtQty.Text));
                        }
                        else
                        {
                            objdecimal += (Convert.ToDecimal(ViewState["Weight"].ToString()) * Convert.ToDecimal(txtQty.Text));
                        }
                        ViewState["Weight"] = objdecimal.ToString();
                    }
                    else
                    {
                        if (optionweight > Decimal.Zero)
                        {
                            ViewState["Weight"] = optionweight.ToString();
                        }
                        else
                        {
                            ViewState["Weight"] = objdecimal.ToString();
                        }
                    }
                }



                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < variantValueCount; i++)
                {
                    if (variantNameCount > i)
                    {
                        if (variantName[i].ToString().ToLower().IndexOf("estimated delivery") > -1)
                        {
                            if (variantValueCount == (i + 1))
                            {
                                sb.Append(variantName[i].ToString() + " : <span style='color:#B92127;'>" + variantValue[i].ToString() + "</span>");
                            }
                            else
                            {
                                sb.Append(variantName[i].ToString() + " : <span style='color:#B92127;'>" + variantValue[i].ToString() + "</span><br/>");
                            }
                        }
                        else
                        {
                            if (variantValueCount == (i + 1))
                            {
                                sb.Append(variantName[i].ToString() + " : " + variantValue[i].ToString() + "");
                            }
                            else
                            {
                                sb.Append(variantName[i].ToString() + " : " + variantValue[i].ToString() + "<br/>");
                            }
                        }
                    }
                }

                Literal ltrlVariane = (Literal)e.Item.FindControl("ltrlVariane");
                ltrlVariane.Text = sb.ToString();
            }
            if (e.Item.ItemType == ListItemType.Header)
            {


                if (Session["CouponCodebycustomer"] != null && Session["CouponCodeDiscountPrice"] != null)
                {
                    GrandSubTotal = 0;
                    System.Web.UI.HtmlControls.HtmlTableCell thproduct = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("thproduct");
                    System.Web.UI.HtmlControls.HtmlTableCell thsku = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("thsku");
                    System.Web.UI.HtmlControls.HtmlTableCell thqty = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("thqty");
                    System.Web.UI.HtmlControls.HtmlTableCell thdisprice = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("thdisprice");
                    thproduct.Width = "33%"; thsku.Width = "10%"; thqty.Width = "7%";


                    if (thdisprice != null)
                    {
                        thdisprice.Visible = true;
                    }
                }
                Session["QtyDiscount"] = null;
                Session["QtyDiscount1"] = null;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                ViewState["SubTotal"] = GrandSubTotal.ToString();

                Label lblSubtotal = (Label)e.Item.FindControl("lblSubtotal");
                Label lblShippingcost = (Label)e.Item.FindControl("lblShippingcost");
                System.Web.UI.HtmlControls.HtmlTableRow tblCustomDiscountrow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trCustomlevelDiscount");
                System.Web.UI.HtmlControls.HtmlTableRow trQuantitylDiscount = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trQuantitylDiscount");
                Label lblCustomlevel = (Label)e.Item.FindControl("lblCustomlevel");
                Label lblDiscount = (Label)e.Item.FindControl("lblDiscount");
                Label lblQuantityDiscount = (Label)e.Item.FindControl("lblQuantityDiscount");
                ImageButton btnApply1 = (ImageButton)e.Item.FindControl("btnApply1");
                TextBox txtPromoCode = (TextBox)e.Item.FindControl("txtPromoCode");

                btnApply1.OnClientClick = "return validation('" + txtPromoCode.ClientID.ToString() + "');";
                if (Session["CouponCodebycustomer"] != null && Session["CouponCodeDiscountPrice"] != null)
                {

                    System.Web.UI.HtmlControls.HtmlTableCell tdShipping = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdShipping");
                    System.Web.UI.HtmlControls.HtmlTableCell tdCustomlevelDiscount = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdCustomlevelDiscount");
                    System.Web.UI.HtmlControls.HtmlTableCell tdQuantityDiscount = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdQuantityDiscount");
                    System.Web.UI.HtmlControls.HtmlTableCell tdGiftCardAppliedDiscount = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdGiftCardAppliedDiscount");
                    System.Web.UI.HtmlControls.HtmlTableCell tdGiftCardRemainingBalance = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdGiftCardRemainingBalance");
                    System.Web.UI.HtmlControls.HtmlTableCell tdDiscount = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdDiscount");
                    System.Web.UI.HtmlControls.HtmlTableCell tdTotal = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdTotal");


                    tdShipping.ColSpan = 4; tdCustomlevelDiscount.ColSpan = 4; tdQuantityDiscount.ColSpan = 4;
                    tdGiftCardAppliedDiscount.ColSpan = 4; tdGiftCardRemainingBalance.ColSpan = 4; tdDiscount.ColSpan = 4;
                    tdTotal.ColSpan = 4;
                }
                bool ChkNoDiscount = false;
                bool.TryParse(CommonComponent.GetScalarCommonData("SELECT NoDiscount FROM dbo.tb_Customer WHERE CustomerID=" + Session["CustID"].ToString()).ToString(), out ChkNoDiscount);
                if (ChkNoDiscount == true)
                {
                    if (Session["QtyDiscount1"] != null)
                    {
                        Session["QtyDiscount"] = Session["QtyDiscount1"].ToString();
                    }
                }
                else
                {
                    if (Session["QtyDiscount1"] != null)
                    {
                        if (Session["QtyDiscount"] != null)
                        {
                            Session["QtyDiscount"] = Convert.ToDecimal(Session["QtyDiscount1"].ToString()) + Convert.ToDecimal(Session["QtyDiscount"].ToString());
                        }
                        else
                        {
                            Session["QtyDiscount"] = Session["QtyDiscount1"].ToString();
                        }
                    }
                }

                #region Code for Gift Certificate

                System.Web.UI.HtmlControls.HtmlTableRow trGiftCertiDiscount = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trGiftCertiDiscount");
                System.Web.UI.HtmlControls.HtmlTableRow trGiftCardRemBal = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trGiftCardRemBal");
                Label lblGiftCertiDiscount = (Label)e.Item.FindControl("lblGiftCertiDiscount");
                Label lblGiftCardRemBal = (Label)e.Item.FindControl("lblGiftCardRemBal");

                #endregion

                string strShippingName = "";
                string strShippingCharge = "0.00";

                if (ViewState["CustomerLevelFreeShipping"] == null)
                {
                    bool IsFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasFreeShipping,0) AS LevelHasFreeShipping  FROM dbo.tb_Customer INNER JOIN dbo.tb_CustomerLevel ON dbo.tb_Customer.CustomerLevelID = dbo.tb_CustomerLevel.CustomerLevelID AND tb_customer.CustomerID=" + Convert.ToInt32(Session["CustID"]) + ""));

                    if (IsFreeShipping)
                    {
                        ViewState["CustomerLevelFreeShipping"] = "1";
                    }
                    else
                    {
                        if (rdoShipping.SelectedIndex > -1)
                        {
                            strShippingName = rdoShipping.SelectedItem.Text.ToString();
                            if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                            {
                                strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                                strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();
                                Decimal.TryParse(strShippingCharge, out ShippingCharges);
                                ViewState["ShippingCharges"] = ShippingCharges.ToString("F2");
                            }
                        }
                    }
                }
                else
                {
                    if (rdoShipping.SelectedIndex > -1)
                    {
                        strShippingName = rdoShipping.SelectedItem.Text.ToString();
                        if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                        {
                            strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                            strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();
                            Decimal.TryParse(strShippingCharge, out ShippingCharges);
                            ViewState["ShippingCharges"] = ShippingCharges.ToString("F2");
                        }
                    }
                }

                if (strShippingCharge.ToString() != "")
                {
                    lblShippingcost.Text = String.Format("{0:0.00}", Convert.ToDecimal(strShippingCharge));
                }

                if (lblSubtotal != null)
                {
                    #region Customer level Discount
                    if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                    {
                        Decimal CustomerlevelDiscount = Decimal.Zero;

                        DataSet dsDiscount = new DataSet();
                        dsDiscount = CommonComponent.GetCommonDataSet("SELECT isnull(LevelDiscountPercent,0) as LevelDiscountPercent,isnull(LevelDiscountAmount,0) as LevelDiscountAmount FROM tb_CustomerLevel WHERE CustomerLevelID in (SELECT isnull(CustomerLevelID,0) FROM tb_Customer WHERE CustomerID = " + Session["CustID"].ToString() + ")");
                        if (dsDiscount != null && dsDiscount.Tables.Count > 0 && dsDiscount.Tables[0].Rows.Count > 0)
                        {
                            Decimal CustomePersantage = Convert.ToDecimal(dsDiscount.Tables[0].Rows[0]["LevelDiscountPercent"].ToString());
                            if (CustomePersantage == Decimal.Zero)
                            {
                                CustomerlevelDiscount = Convert.ToDecimal(dsDiscount.Tables[0].Rows[0]["LevelDiscountAmount"].ToString());
                            }
                            else
                            {
                                CustomerlevelDiscount = (Convert.ToDecimal(GrandSubTotal) * CustomePersantage) / 100;
                            }

                        }
                        if (CustomerlevelDiscount > Decimal.Zero)
                        {
                            //tblCustomDiscountrow.Visible = true;
                            //lblCustomlevel.Text = String.Format("{0:0.00}", Convert.ToDecimal(CustomerlevelDiscount.ToString()));
                            //GrandSubTotal = GrandSubTotal - CustomerlevelDiscount;

                            // **** Code By Girish ****
                            Decimal SubTotalCustomer = GrandSubTotal;
                            Decimal DiscountCustomer = Math.Round((SubTotalCustomer - CustomerlevelDiscount), 2);
                            if (DiscountCustomer < 0)
                            {
                                GrandSubTotal = 0;
                                DiscountCustomer = SubTotalCustomer;
                            }
                            else
                            {
                                GrandSubTotal = GrandSubTotal - CustomerlevelDiscount;
                                DiscountCustomer = CustomerlevelDiscount;
                            }
                            tblCustomDiscountrow.Visible = true;
                            lblCustomlevel.Text = DiscountCustomer.ToString("F2");
                        }
                    }
                    #endregion

                    if (Session["QtyDiscount"] != null)
                    {
                        decimal Qty = 0;
                        decimal.TryParse(Session["QtyDiscount"].ToString(), out Qty);
                        lblQuantityDiscount.Text = Math.Round(Qty, 2).ToString();
                        if (Convert.ToDecimal(Session["QtyDiscount"].ToString()) > Decimal.Zero)
                        {
                            trQuantitylDiscount.Visible = true;
                        }
                    }
                    else
                    {
                        lblQuantityDiscount.Text = "0.00";
                    }

                    #region Code for Gift Certificate

                    if (Session["GiftCertificateDiscount"] != null)
                    {
                        lblGiftCertiDiscount.Text = Convert.ToString(Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString()));
                        if (Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString()) > Decimal.Zero)
                        {
                            trGiftCertiDiscount.Visible = true;
                        }
                        Session["GiftCertificateDiscount"] = lblGiftCertiDiscount.Text;
                    }
                    else
                    {
                        lblGiftCertiDiscount.Text = "0.00";
                    }

                    if (Session["GiftCertificateRemaningBalance"] != null)
                    {
                        lblGiftCardRemBal.Text = Convert.ToString(Convert.ToDecimal(Session["GiftCertificateRemaningBalance"].ToString()));
                        if (Convert.ToDecimal(Session["GiftCertificateRemaningBalance"].ToString()) > Decimal.Zero)
                        {
                            trGiftCardRemBal.Visible = true;
                        }
                    }
                    else
                    {
                        lblGiftCardRemBal.Text = "0.00";
                    }

                    #endregion

                    Decimal SubTot = 0;
                    //if (Session["Discount"] != null)
                    //{
                    //    SubTot = Convert.ToDecimal(GrandSubTotal) + Convert.ToDecimal(strShippingCharge) - Convert.ToDecimal(Session["Discount"].ToString());
                    //    lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(SubTot));
                    //    lblDiscount.Text = String.Format("{0:0.00}", Convert.ToDecimal(Session["Discount"].ToString()));
                    //}
                    //else
                    //{
                    //    SubTot = Convert.ToDecimal(GrandSubTotal) + Convert.ToDecimal(strShippingCharge) - Convert.ToDecimal(lblQuantityDiscount.Text.ToString());
                    //    lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(SubTot.ToString()));
                    //    lblDiscount.Text = "0.00";
                    //}

                    if (Session["Discount"] != null || Session["GiftCertificateDiscount"] != null)
                    {
                        decimal TotalDiscount = Convert.ToDecimal(Session["Discount"]);
                        SubTot = Convert.ToDecimal(GrandSubTotal) + Convert.ToDecimal(strShippingCharge) - Convert.ToDecimal(lblQuantityDiscount.Text.ToString());
                        if (Session["Discount"] != null)
                        {
                            //SubTot = SubTot - TotalDiscount;
                            //lblDiscount.Text = String.Format("{0:0.00}", Convert.ToDecimal(Session["Discount"].ToString()));
                            //lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(SubTot.ToString()));
                            //FinalTotal = Convert.ToDecimal(lblSubtotal.Text);

                            Decimal SubTotalDiscount = SubTot;
                            Decimal TotalCouponDiscount = Math.Round((SubTotalDiscount - TotalDiscount), 2);
                            if (TotalCouponDiscount < 0)
                            {
                                SubTot = 0;
                                TotalCouponDiscount = SubTotalDiscount;
                            }
                            else
                            {
                                SubTot = SubTot - TotalDiscount;
                                TotalCouponDiscount = TotalDiscount;
                            }
                            lblDiscount.Text = TotalCouponDiscount.ToString("F2");
                            lblSubtotal.Text = SubTot.ToString("F2");
                            FinalTotal = SubTot;
                        }
                        if (Session["GiftCertificateDiscount"] != null)
                        {
                            if (SubTot < 0)
                            {
                                lblGiftCertiDiscount.Text = "0.00";
                                lblGiftCardRemBal.Text = Convert.ToDecimal(Session["GiftCertificateDiscount"]).ToString("F2");
                                trGiftCardRemBal.Visible = true;
                            }
                            else
                            {
                                decimal GiftCardAmount = Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString());
                                //decimal GiftCardAmount = GiftCardTotalDiscount;
                                if (GiftCardAmount != Decimal.Zero)
                                {
                                    SubTot = SubTot - Convert.ToDecimal(Session["GiftCertificateDiscount"].ToString());
                                }
                                if (SubTot < 0 && GiftCardAmount != Decimal.Zero)
                                {
                                    decimal TotGiftCertiDiscount = Convert.ToDecimal(lblGiftCertiDiscount.Text);
                                    //lblGiftCertiDiscount.Text = Convert.ToString(Math.Round(TotalDiscount - Math.Abs(Math.Round(SubTot, 2)), 2));
                                    lblGiftCertiDiscount.Text = Convert.ToString(Math.Round(Convert.ToDecimal(TotGiftCertiDiscount) - Math.Abs(Math.Round(SubTot, 2)), 2));

                                    //Session["GiftCertificateDiscount"] = Math.Round(TotalDiscount - Math.Abs(Math.Round(SubTot, 2)), 2);
                                    //Session["GiftCertificateDiscount"] = Math.Round(Convert.ToDecimal(TotGiftCertiDiscount) - Math.Abs(Math.Round(SubTot, 2)), 2);
                                    //Session["GiftCertificateRemaningBalance"] = Math.Abs(Math.Round(SubTot, 2));

                                    lblSubtotal.Text = "0.00";
                                    trGiftCardRemBal.Visible = true;
                                    lblGiftCardRemBal.Text = Math.Abs(Math.Round(SubTot, 2)).ToString("F2");
                                }
                                else
                                {
                                    Session["GiftCertificateDiscount"] = Math.Round(GiftCardAmount, 2);
                                    Session["GiftCertificateRemaningBalance"] = "0.00";
                                    lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(SubTot.ToString()));
                                    trGiftCardRemBal.Visible = false;
                                    lblGiftCardRemBal.Text = "0.00";
                                }
                            }

                        }
                        if (Convert.ToDecimal(lblSubtotal.Text) < 0)
                        {
                            FinalTotal = Convert.ToDecimal(0);
                            lblSubtotal.Text = "0.00";
                        }
                        else
                        {
                            FinalTotal = Convert.ToDecimal(lblSubtotal.Text);
                        }

                    }
                    else
                    {
                        SubTot = Convert.ToDecimal(GrandSubTotal) + Convert.ToDecimal(strShippingCharge) - Convert.ToDecimal(lblQuantityDiscount.Text.ToString());
                        if (SubTot < 0)
                        {
                            lblSubtotal.Text = "0.00";
                        }
                        else
                        {
                            lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(SubTot.ToString()));
                        }
                        FinalTotal = Convert.ToDecimal(lblSubtotal.Text);
                        lblDiscount.Text = "0.00";
                    }
                    ViewState["FinalTotal"] = FinalTotal.ToString("F2");
                    ChkFreeshipping();
                }
                if (ViewState["SubTotal"] != null)
                {
                    lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(ViewState["SubTotal"].ToString()));
                }
                else { lblSubtotal.Text = "0.00"; }
                hdnSubTotalofProduct.Value = lblSubtotal.Text;
            }
        }

        /// <summary>
        /// Free Shipping Over Specified Amount
        /// </summary>
        private void ChkFreeshipping()
        {
            #region Code for Free Shipping Over Specified Amount
            if ((Convert.ToInt32(ddlcountry.SelectedValue) == 1 && rdoShipping.Items.Count == 0) || (Convert.ToInt32(ddlcountry.SelectedValue) == 1 && rdoShipping.Items.Count > 0))
            {
                if (ViewState["FinalTotal"] != null && ViewState["ShippingCharges"] != null)
                {
                    lblFreeShippningMsg.Visible = true;
                    Decimal FinalTotalNotShipping = Convert.ToDecimal(ViewState["FinalTotal"]) - Convert.ToDecimal(ViewState["ShippingCharges"]);
                    if (FinalTotalNotShipping > FreeShippingAmount)
                    {
                        lblFreeShippningMsg.Text = "Congratulations!! You qualified for Free Shipping. ( United States Only )";
                    }
                    else
                    {
                        if (FinalTotalNotShipping < 0)
                        {
                            FinalTotalNotShipping = 0;
                        }
                        Decimal TotalDiff = FreeShippingAmount - FinalTotalNotShipping;
                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                    }
                }
                else
                {
                    lblFreeShippningMsg.Visible = false;
                }
            }
            else
            {
                lblFreeShippningMsg.Visible = false;
            }

            #endregion

            //ShippingCharges
        }

        /// <summary>
        /// Method to update shopping cart item
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnUpdateCart_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["Weight"] = null;
            ViewState["AvailableDate"] = null;
            bool isDropshipProduct = false;
            int totalItemCount = RptCartItems.Items.Count;
            for (int i = 0; i < totalItemCount; i++)
            {
                TextBox txtQty = (TextBox)RptCartItems.Items[i].FindControl("txtQty");
                LinkButton lbtndelete = (LinkButton)RptCartItems.Items[i].FindControl("lbtndelete");
                HiddenField hdnProductId = (HiddenField)RptCartItems.Items[i].FindControl("hdnProductId");
                HiddenField hdnRelatedproductID = (HiddenField)RptCartItems.Items[i].FindControl("hdnRelatedproductID");
                HiddenField hdnVariantvalue = (HiddenField)RptCartItems.Items[i].FindControl("hdnVariantvalue");
                HiddenField hdnVariantname = (HiddenField)RptCartItems.Items[i].FindControl("hdnVariantname");

                int pInventory = 0;
                bool outofstock = false;
                bool checkvendordate = false;
                Decimal qtyyrd = Decimal.Zero;
                isDropshipProduct = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(IsdropshipProduct,0) FROM tb_product WHERE productid=" + Convert.ToInt32(hdnProductId.Value.ToString()).ToString() + ""));
                int Yardqty = 0;
                double actualYard = 0;
                Int32 ProductType = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(isProducttype,1) as isProducttype from tb_ShoppingCartItems where CustomCartID = " + lbtndelete.CommandArgument + ""));
                if (hdnRelatedproductID.Value.ToString() == "0")
                {
                    if (!string.IsNullOrEmpty(txtQty.Text.Trim()))
                    {
                        if (hdnProductId != null && Convert.ToInt32(txtQty.Text.ToString().Trim()) > 0)
                        {
                            DataSet ds = ProductComponent.GetProductDetailByID(Convert.ToInt32(hdnProductId.Value), Convert.ToInt32(1));
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                string FabricCode = Convert.ToString(ds.Tables[0].Rows[0]["FabricCode"]);
                                string FabricType = Convert.ToString(ds.Tables[0].Rows[0]["FabricType"]);
                                Int32 FabricTypeID = 0;
                                if (!string.IsNullOrEmpty(FabricCode) && !string.IsNullOrEmpty(FabricType))
                                {
                                    FabricTypeID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(FabricTypeID,0) as FabricTypeID from tb_ProductFabricType where FabricTypename ='" + FabricType + "'"));
                                    if (FabricTypeID > 0 && ProductType == 2)
                                    {
                                        DataSet dsFabricWidth = CommonComponent.GetCommonDataSet("Select top 1 * from tb_ProductFabricWidth where FabricCodeID in (Select ISNULL(FabricCodeID,0) from tb_ProductFabricCode Where FabricTypeID=" + FabricTypeID + " and Code='" + FabricCode + "')");
                                        Int32 QtyOnHand = 0, NextOrderQty = 0, TotalQty = 0;
                                        Int32 OrderQty = Convert.ToInt32(txtQty.Text.Trim());

                                        if (dsFabricWidth != null && dsFabricWidth.Tables.Count > 0 && dsFabricWidth.Tables[0].Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"].ToString()))
                                                QtyOnHand = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"]);

                                            if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"].ToString()))
                                                NextOrderQty = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"]);
                                        }
                                        pInventory = QtyOnHand + NextOrderQty;

                                        try
                                        {
                                            string Style = "";
                                            double Width = 0;
                                            double Length = 0;
                                            string Options = "";
                                            string[] strNmyard = hdnVariantname.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            string[] strValyeard = hdnVariantvalue.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
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
                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + hdnProductId.Value.ToString() + "," + Width.ToString() + "," + Length.ToString() + "," + OrderQty.ToString() + ",'" + Style + "','" + Options + "'");
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
                                        if (!string.IsNullOrEmpty(hdnVariantvalue.Value.ToString().Trim()) && hdnVariantvalue.Value.ToString().Trim().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                                        {
                                            OrderQty = OrderQty * 2;
                                        }

                                        //if (pInventory > 0 && OrderQty > pInventory && isDropshipProduct == false)
                                        //{
                                        //    outofstock = true;
                                        //    lblInverror.Text = "We have not enough inventory!";
                                        //}
                                        //else
                                        //{
                                        //    if (OrderQty > QtyOnHand)
                                        //    {
                                        //        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()) && !dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString().ToLower().Contains("1900"))
                                        //        {
                                        //            Int32 TotalDays = 0;
                                        //            if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString()))
                                        //            {
                                        //                TotalDays = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString());
                                        //            }
                                        //            DateTime dtnew = Convert.ToDateTime(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()).AddDays(TotalDays);
                                        //            ViewState["AvailableDate"] = Convert.ToString(dtnew);
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        Int32 TotalDays = 0;
                                        //        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString()))
                                        //        {
                                        //            TotalDays = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString());
                                        //        }
                                        //        DateTime dtnew = DateTime.Now.Date.AddDays(TotalDays);
                                        //        ViewState["AvailableDate"] = Convert.ToString(dtnew);
                                        //    }
                                        //}

                                        string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + hdnProductId.Value.ToString() + " "));

                                        if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                                        {
                                            if (isDropshipProduct == false)
                                            {
                                                outofstock = true;
                                                lblInverror.Text = "We have not enough inventory!";

                                            }
                                            ViewState["AvailableDate"] = StrVendor;
                                        }
                                        else
                                        {
                                            checkvendordate = true;
                                            ViewState["AvailableDate"] = StrVendor;
                                        }
                                    }
                                    else if (ProductType == 3)
                                    {

                                        qtyyrd = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT (" + txtQty.Text.ToString() + " * Actualyard) FROM tb_ShoppingCartItems WHERE CustomCartID=" + lbtndelete.CommandArgument + ""));
                                        Yardqty = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDecimal(qtyyrd.ToString())))));
                                        actualYard = Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDecimal(qtyyrd / Convert.ToDecimal(txtQty.Text.ToString()))));
                                        string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + hdnProductId.Value.ToString() + ",'" + hdnVariantname.Value.ToString().Replace("~hpd~", "-") + "','" + hdnVariantvalue.Value.ToString().Replace("~hpd~", "-") + "'"));

                                        if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
                                        {
                                            if (isDropshipProduct == false)
                                            {
                                                outofstock = true;
                                                lblInverror.Text = "We have not enough inventory!";

                                            }
                                            // ViewState["AvailableDate"] = StrVendor;
                                        }
                                        else
                                        {
                                            checkvendordate = true;
                                            //  ViewState["AvailableDate"] = StrVendor;
                                        }
                                    }
                                    else if (ProductType == 1)
                                    {
                                        string[] strNmyard = hdnVariantname.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        string[] strValyeard = hdnVariantvalue.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

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
                                                            strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "");
                                                            int CntInv = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                            //pInventory = CntInv;
                                                            DataSet dsUPC = new DataSet();
                                                            dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + "");
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
                                                                            CntInv = Convert.ToInt32(strQty.ToString());
                                                                        }
                                                                        catch { }
                                                                    }
                                                                }
                                                            }
                                                            pInventory = CntInv;
                                                            string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + hdnProductId.Value.ToString() + ""));
                                                            if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder)) //&& CntInv < Convert.ToInt32(txtQty.Text.ToString())
                                                            {
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
                                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + hdnProductId.Value.ToString() + ",50," + Length.ToString() + "," + txtQty.Text.ToString() + ",'Pole Pocket','Lined'");
                                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                                {
                                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                                    actualYard = Convert.ToDouble(resp.ToString());
                                                                }
                                                                Int32 OrderQty = Convert.ToInt32(txtQty.Text.ToString());
                                                                if (resp != "")
                                                                {
                                                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                                }
                                                                Yardqty = OrderQty;
                                                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty.ToString() + "," + hdnProductId.Value.ToString() + " "));
                                                                pInventory = CntInv;
                                                                if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                                {
                                                                    ViewState["AvailableDate"] = StrVendor.ToString();
                                                                    checkvendordate = false;
                                                                }
                                                                else
                                                                {
                                                                    ViewState["AvailableDate"] = StrVendor.ToString();
                                                                    checkvendordate = true;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string strvalue = strValyeard[j].ToString().Trim();
                                                            strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "");
                                                            int CntInv = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                            //pInventory = CntInv;
                                                            DataSet dsUPC = new DataSet();
                                                            dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + "");
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
                                                                            CntInv = Convert.ToInt32(strQty.ToString());
                                                                        }
                                                                        catch { }
                                                                    }
                                                                }
                                                            }
                                                            pInventory = CntInv;
                                                            string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + hdnProductId.Value.ToString() + ""));
                                                            if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder)) // && CntInv < Convert.ToInt32(txtQty.Text.ToString())
                                                            {
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
                                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + hdnProductId.Value.ToString() + ",50," + Length.ToString() + "," + txtQty.Text.ToString() + ",'Pole Pocket','Lined'");
                                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                                {
                                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                                    actualYard = Convert.ToDouble(resp.ToString());
                                                                }
                                                                Int32 OrderQty = Convert.ToInt32(txtQty.Text.ToString());
                                                                if (resp != "")
                                                                {
                                                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                                }

                                                                Yardqty = OrderQty;
                                                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty.ToString() + "," + hdnProductId.Value.ToString() + " "));

                                                                if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                                {
                                                                    ViewState["AvailableDate"] = StrVendor.ToString();
                                                                    checkvendordate = false;
                                                                }
                                                                else
                                                                {
                                                                    ViewState["AvailableDate"] = StrVendor.ToString();
                                                                    checkvendordate = true;
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }

                                    }
                                }
                                else if (ProductType == 3)
                                {

                                    qtyyrd = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT (" + txtQty.Text.ToString() + " * Actualyard) FROM tb_ShoppingCartItems WHERE CustomCartID=" + lbtndelete.CommandArgument + ""));
                                    Yardqty = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDecimal(qtyyrd.ToString())))));
                                    actualYard = Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDecimal(qtyyrd / Convert.ToDecimal(txtQty.Text.ToString()))));

                                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + hdnProductId.Value.ToString() + ",'" + hdnVariantname.Value.ToString().Replace("~hpd~", "-") + "','" + hdnVariantvalue.Value.ToString().Replace("~hpd~", "-") + "'"));

                                    if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
                                    {
                                        if (isDropshipProduct == false)
                                        {
                                            outofstock = true;
                                            lblInverror.Text = "We have not enough inventory!";

                                        }
                                        // ViewState["AvailableDate"] = StrVendor;
                                    }
                                    else
                                    {
                                        checkvendordate = true;
                                        //  ViewState["AvailableDate"] = StrVendor;
                                    }
                                }
                                else if (ProductType == 1)
                                {
                                    string[] strNmyard = hdnVariantname.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    string[] strValyeard = hdnVariantvalue.Value.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

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
                                                        strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "");
                                                        int CntInv = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                        //pInventory = CntInv;
                                                        DataSet dsUPC = new DataSet();
                                                        dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + "");
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
                                                                        CntInv = Convert.ToInt32(strQty.ToString());
                                                                    }
                                                                    catch { }
                                                                }
                                                            }
                                                        }
                                                        pInventory = CntInv;

                                                        string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + hdnProductId.Value.ToString() + ""));
                                                        if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder)) // && CntInv < Convert.ToInt32(txtQty.Text.ToString())
                                                        {
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
                                                            dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + hdnProductId.Value.ToString() + ",50," + Length.ToString() + "," + txtQty.Text.ToString() + ",'Pole Pocket','Lined'");
                                                            if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                            {
                                                                resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                                actualYard = Convert.ToDouble(resp.ToString());
                                                            }
                                                            Int32 OrderQty = Convert.ToInt32(txtQty.Text.ToString());
                                                            if (resp != "")
                                                            {
                                                                OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                            }

                                                            Yardqty = OrderQty;

                                                            string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + hdnProductId.Value.ToString() + " "));

                                                            if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                            {
                                                                ViewState["AvailableDate"] = StrVendor.ToString();
                                                                checkvendordate = false;
                                                            }
                                                            else
                                                            {
                                                                ViewState["AvailableDate"] = StrVendor.ToString();
                                                                checkvendordate = true;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string strvalue = strValyeard[j].ToString().Trim();
                                                        strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "");
                                                        int CntInv = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + ""));
                                                        //pInventory = CntInv;
                                                        DataSet dsUPC = new DataSet();
                                                        dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + hdnProductId.Value.ToString() + "");
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
                                                                        CntInv = Convert.ToInt32(strQty.ToString());
                                                                    }
                                                                    catch { }
                                                                }
                                                            }
                                                        }
                                                        pInventory = CntInv;
                                                        string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + hdnProductId.Value.ToString() + ""));
                                                        if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder)) //&& CntInv < Convert.ToInt32(txtQty.Text.ToString())
                                                        {
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
                                                            dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + hdnProductId.Value.ToString() + ",50," + Length.ToString() + "," + txtQty.Text.ToString() + ",'Pole Pocket','Lined'");
                                                            if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                            {
                                                                resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                                actualYard = Convert.ToDouble(resp.ToString());
                                                            }
                                                            Int32 OrderQty = Convert.ToInt32(txtQty.Text.ToString());
                                                            if (resp != "")
                                                            {
                                                                OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                            }

                                                            Yardqty = OrderQty;

                                                            string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty.ToString() + "," + hdnProductId.Value.ToString() + " "));

                                                            if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                            {
                                                                ViewState["AvailableDate"] = StrVendor.ToString();
                                                                checkvendordate = false;
                                                            }
                                                            else
                                                            {
                                                                ViewState["AvailableDate"] = StrVendor.ToString();
                                                                checkvendordate = true;
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                    {
                                        pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());
                                    }
                                }
                            }

                            if (ProductType.ToString() == "1" && ViewState["AvailableDate"] == null) // Made to Measure
                            {
                                DateTime dtnew = DateTime.Now.Date.AddDays(12);
                                ViewState["AvailableDate"] = Convert.ToString(dtnew);
                                //Int32 OrderQty = Convert.ToInt32(txtQty.Text.Trim());
                                //string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + hdnProductId.Value.ToString() + " "));

                                //if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                                //{
                                //    ViewState["AvailableDate"] = StrVendor;
                                //}
                                //else
                                //{
                                //    ViewState["AvailableDate"] = StrVendor;
                                //}
                            }
                            if (!string.IsNullOrEmpty(hdnVariantvalue.Value.ToString().Trim()) && hdnVariantvalue.Value.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                            {
                                txtQty.Text = Convert.ToString(Convert.ToInt32(txtQty.Text) * 2);
                            }
                            if (isDropshipProduct == false && hdnRelatedproductID.Value.ToString() == "0")
                            {
                                Int32 AssemblyProduct = 0, ReturnQty = 0;
                                AssemblyProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect COUNT(*) From tb_product Where Productid=" + Convert.ToInt32(hdnProductId.Value.ToString()).ToString() + " and StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " and ProductTypeID in (Select ProductTypeID From tb_ProductType where Name='Assembly Product')"));
                                if (AssemblyProduct > 0)
                                {
                                    ReturnQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_Check_ProductAssemblyInventory " + Convert.ToInt32(hdnProductId.Value.ToString()).ToString() + "," + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + "," + Session["CustID"].ToString() + ",2"));
                                    if (ReturnQty <= 0 && isDropshipProduct == false)
                                    {
                                        outofstock = true;
                                    }
                                    else if (Convert.ToInt32(txtQty.Text.Trim()) > ReturnQty && isDropshipProduct == false)
                                    {
                                        outofstock = true;
                                    }
                                    else
                                    {
                                        // UpdateCart  Query
                                    }
                                }
                            }
                            if ((pInventory >= Convert.ToInt32(txtQty.Text.Trim()) && outofstock == false) || isDropshipProduct == true || checkvendordate == true)
                            {
                                if (txtQty != null && lbtndelete != null && hdnRelatedproductID.Value.ToString() == "0")
                                {
                                    if (!string.IsNullOrEmpty(hdnVariantvalue.Value.ToString().Trim()) && hdnVariantvalue.Value.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                                    {
                                        txtQty.Text = Convert.ToString(Convert.ToInt32(txtQty.Text) / 2);
                                    }
                                    objCart.UpdateCartItemQtyByCustomCartID(Convert.ToInt32(lbtndelete.CommandArgument), Convert.ToInt32(txtQty.Text.Trim()));
                                    try
                                    {
                                        string[] strNm = hdnVariantname.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        string[] strVal = hdnVariantvalue.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        string avail = "";
                                        string VariantNameId = "";
                                        string VariantValueId = "";
                                        if (strNm.Length > 0)
                                        {
                                            if (strVal.Length == strNm.Length)
                                            {
                                                for (int k = 0; k < strNm.Length; k++)
                                                {
                                                    if (ProductType != 3 && strNm[k].ToString().ToLower().IndexOf("estimated delivery") <= -1)
                                                    {
                                                        VariantNameId = VariantNameId + strNm[k].ToString() + ",";

                                                        VariantValueId = VariantValueId + strVal[k].ToString() + ",";
                                                        if (avail == "" && ProductType.ToString() == "1")
                                                        {
                                                            avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 LockQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + hdnProductId.Value.ToString() + "  AND VariantValue='" + strVal[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(LockQuantity,0) >=" + txtQty.Text + ""));
                                                            if (avail == "")
                                                            {
                                                                avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 AllowQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + hdnProductId.Value.ToString() + "  AND VariantValue='" + strVal[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(AllowQuantity,0) >=" + txtQty.Text + ""));
                                                            }
                                                        }
                                                    }
                                                    else if (ProductType == 3 && strNm[k].ToString().ToLower().IndexOf("yardage required") <= -1)
                                                    {
                                                        VariantNameId = VariantNameId + strNm[k].ToString() + ",";

                                                        VariantValueId = VariantValueId + strVal[k].ToString() + ",";
                                                    }
                                                }

                                                if (ViewState["AvailableDate"] != null && ViewState["AvailableDate"].ToString() != "" && ProductType > 0 && ProductType == 2)
                                                {
                                                    avail = Convert.ToString(ViewState["AvailableDate"]);
                                                    try
                                                    {
                                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                                    }
                                                    catch { }
                                                    //   VariantNameId = VariantNameId + "Expected Delivery,";
                                                    VariantNameId = VariantNameId + "Estimated Delivery,";
                                                    VariantValueId = VariantValueId + avail.ToString() + ",";
                                                    CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE  CustomCartID=" + lbtndelete.CommandArgument + "");
                                                }
                                                else if (ProductType == 3)
                                                {
                                                    VariantNameId = VariantNameId + "Yardage Required,";
                                                    VariantValueId = VariantValueId + qtyyrd.ToString() + ",";
                                                    CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ", VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE  CustomCartID=" + lbtndelete.CommandArgument + "");
                                                }
                                                else if (avail == "" && ProductType.ToString() == "2")
                                                {
                                                    CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE  CustomCartID=" + lbtndelete.CommandArgument + "");
                                                }
                                                if (ProductType.ToString() == "1")
                                                {
                                                    if (avail != "")
                                                    {
                                                        VariantNameId = VariantNameId + "Estimated Delivery,";
                                                        VariantValueId = VariantValueId + avail.ToString() + ",";
                                                        CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE  CustomCartID=" + lbtndelete.CommandArgument + "");
                                                    }
                                                    else
                                                    {
                                                        if (ViewState["AvailableDate"] != null)
                                                        {
                                                            avail = Convert.ToString(ViewState["AvailableDate"]);
                                                            try
                                                            {
                                                                avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                                            }
                                                            catch { }
                                                            VariantNameId = VariantNameId + "Estimated Delivery,";
                                                            VariantValueId = VariantValueId + avail.ToString() + ",";
                                                            CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE  CustomCartID=" + lbtndelete.CommandArgument + "");
                                                        }
                                                    }
                                                }
                                                if (ProductType != 3)
                                                {
                                                    CommonComponent.ExecuteCommonData("UPDATE  tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "' WHERE  CustomCartID=" + lbtndelete.CommandArgument + "");
                                                }
                                            }
                                        }

                                    }
                                    catch
                                    {
                                    }
                                }
                                lblInverror.Text = "";
                            }
                            else
                            {
                                lblInverror.Text = "We have not enough inventory!";
                            }
                        }
                        else
                        {
                            lblInverror.Text = "Enter Valid Inventory!";
                        }
                    }
                    else
                    {
                        lblInverror.Text = "Enter Valid Inventory!";
                    }
                }
            }
            CouponCodeCalculation();
            BindShoppingCartByCustomerID();
            btnSubmit_Click(null, null);

        }

        /// <summary>
        /// Cart Items Item Command Event
        /// </summary>
        /// <param name="source">object source</param>
        /// <param name="e">RepeaterCommandEventArgs e</param>
        protected void RptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                objCart.DeleteCartItemByCustomCartID(Convert.ToInt32(e.CommandArgument));
                BindShoppingCartByCustomerID();
                CouponCodeCalculation();
            }
        }

        /// <summary>
        /// Calculate coupon Discount after Delete an Item From Cart
        /// </summary>
        private void CouponCodeCalculation()
        {
            if (Session["CouponCode"] != null && Session["Discount"] != null)
            {
                String SPMessage = Convert.ToString(CouponComponent.GetDiscountByCouponCodeFunction(Convert.ToString(Session["CouponCode"]).Trim(), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
                decimal CouponDiscount = 0;
                if (RptCartItems.Items.Count == 1)
                {
                    HiddenField hdnProductId = (HiddenField)RptCartItems.Items[0].FindControl("hdnProductId");
                    if (hdnProductId.Value.ToString() == AppLogic.AppConfigs("HotDealProduct").ToString())
                    {
                        Session["CouponCode"] = null;
                        Session["Discount"] = null;
                        CouponDiscount = 0;
                    }
                }
                try
                {
                    CouponDiscount = Convert.ToDecimal(SPMessage.ToString());
                    Session["Discount"] = CouponDiscount.ToString();
                }
                catch
                {
                    Session["CouponCode"] = null;
                    Session["Discount"] = null;
                    CouponDiscount = 0;
                }
                lblMsg.Text = "";
                //if (RptCartItems.Items.Count > 0)
                //{
                //    RepeaterItem riFooter = RptCartItems.Controls[RptCartItems.Controls.Count - 1] as RepeaterItem;
                //    if (riFooter != null && riFooter.ItemType == ListItemType.Footer)
                //    {
                //        Label lblSubtotal = riFooter.FindControl("lblSubtotal") as Label;
                //        Label lblDiscount = riFooter.FindControl("lblDiscount") as Label;
                //        if (lblSubtotal != null)
                //        {
                //            decimal SubTotal = decimal.Zero;
                //            if (ViewState["SubTotal"] != null)
                //            {
                //                SubTotal = Convert.ToDecimal(ViewState["SubTotal"].ToString());
                //            }
                //            decimal Discount = Math.Round((SubTotal - CouponDiscount), 2);
                //            lblSubtotal.Text = Convert.ToDecimal(SubTotal - CouponDiscount).ToString("F2");
                //            lblDiscount.Text = CouponDiscount.ToString("F2");
                //            //Session["CouponCode"] = txtPromoCode.Text.Trim();
                //            if (Session["Discount"] != null)
                //            {
                //                Session["Discount"] = CouponDiscount.ToString();
                //            }
                //        }
                //    }
                //}

                decimal GiftCardAmount = 0;
                if (Session["GiftCertificateDiscountCode"] != null && Session["GiftCertificateDiscount"] != null)
                {
                    GiftCardAmount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Balance FROM [tb_GiftCard] where storeid=" + AppLogic.AppConfigs("StoreID") + " And SerialNumber='" + Session["GiftCertificateDiscountCode"].ToString().Trim() + "'"));
                    if (GiftCardAmount == Decimal.Zero)
                    {
                        Session["GiftCertificateDiscountCode"] = null;
                        Session["GiftCertificateDiscount"] = null;
                    }
                    else
                    {
                        Session["GiftCertificateDiscountCode"] = Session["GiftCertificateDiscountCode"].ToString().Trim();
                        Session["GiftCertificateDiscount"] = Math.Round(GiftCardAmount, 2);
                        GiftCardTotalDiscount = Math.Round(GiftCardAmount, 2);
                    }
                }
                //    BindShoppingCartByCustomerID();
            }
        }

        /// <summary>
        ///  Apply Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnApply_Click(object sender, ImageClickEventArgs e)
        {
            Session["CouponCode"] = null;
            Session["CouponCodebycustomer"] = null;
            Session["CouponCodeDiscountPrice"] = null;
            Control FooterTemplate = RptCartItems.Controls[RptCartItems.Controls.Count - 1].Controls[0];
            TextBox txtPromoCode = FooterTemplate.FindControl("txtPromoCode") as TextBox;

            if (Session["CustID"] != null && txtPromoCode != null)
            {
                ViewState["Weight"] = null;
                lblInverror.Text = "";
                if (txtPromoCode.Text.ToString().Trim() != "")
                {
                    if (RptCartItems.Items.Count == 1)
                    {
                        HiddenField hdnProductId = (HiddenField)RptCartItems.Items[0].FindControl("hdnProductId");
                        if (hdnProductId.Value.ToString() == AppLogic.AppConfigs("HotDealProduct").ToString())
                        {
                            lblInverror.Text = "You can't use coupon code for deal of the day product!";
                            Session["CouponCode"] = null;
                            Session["Discount"] = null;
                            return;
                        }

                    }
                    decimal CouponDiscount = 0;
                    decimal GiftCardAmount = 0;
                    bool ChkNoDiscount = false;
                    String SPMessage = "";
                    decimal DiscountPercent = decimal.Zero;
                    bool isEle = false;
                    DataSet dsCoupon = new DataSet();
                    dsCoupon = CommonComponent.GetCommonDataSet("SELECT ISNULL(DiscountPercent,0) AS DiscountPercent,FromDate,ToDate  FROM dbo.tb_Customer WHERE CustomerID=" + Session["CustID"].ToString() + " And CouponCode ='" + txtPromoCode.Text.ToString() + "'");
                    string StrFromdate = "";
                    string StrTodate = "";
                    //out DiscountPercent);
                    if (dsCoupon != null && dsCoupon.Tables.Count > 0 && dsCoupon.Tables[0].Rows.Count > 0)
                    {
                        DiscountPercent = Convert.ToDecimal(dsCoupon.Tables[0].Rows[0]["DiscountPercent"]);
                        StrFromdate = Convert.ToString(dsCoupon.Tables[0].Rows[0]["FromDate"].ToString());
                        StrTodate = Convert.ToString(dsCoupon.Tables[0].Rows[0]["ToDate"].ToString());
                    }
                    if (!string.IsNullOrEmpty(StrFromdate.Trim()) && !string.IsNullOrEmpty(StrTodate.Trim()))
                    {
                        DateTime FDate = new DateTime();
                        DateTime TDate = new DateTime();
                        DateTime Currdate = System.DateTime.Now;
                        try { FDate = Convert.ToDateTime(StrFromdate.Trim()); }
                        catch { }

                        try { TDate = Convert.ToDateTime(StrTodate.Trim()); }
                        catch { }

                        if (Convert.ToDateTime(FDate.ToString("MM/dd/yyyy")) <= Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy")) && Convert.ToDateTime(TDate.ToString("MM/dd/yyyy")) >= Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy")))
                        {
                            if (DiscountPercent > 0)
                            {
                                Session["CouponCodebycustomer"] = txtPromoCode.Text;
                                Session["CouponCodeDiscountPrice"] = DiscountPercent;
                                isEle = true;
                                lblInverror.Text = "Coupon Code Successfully Applied!";
                            }
                        }
                        else
                        {
                            isEle = true;
                            lblInverror.Text = "Sorry, Coupon code is expired!";
                            foreach (RepeaterItem rItem in RptCartItems.Items)
                            {
                                Label lblDiscountPrice = (Label)rItem.FindControl("lblDiscountprice");
                                HiddenField hdnCustomcartId = (HiddenField)rItem.FindControl("hdnCustomcartId");
                                CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET DiscountPrice=0 WHERE CustomCartID=" + hdnCustomcartId.Value.ToString() + "");

                            }
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgCouponcode", "alert('Sorry, Coupon code is expired!');", true);
                        }
                    }
                    else
                    {
                        if (DiscountPercent > 0)
                        {

                            Session["CouponCodebycustomer"] = txtPromoCode.Text;
                            Session["CouponCodeDiscountPrice"] = DiscountPercent;
                            isEle = true;
                            lblInverror.Text = "Coupon Code Successfully Applied!";
                        }
                    }


                    bool.TryParse(CommonComponent.GetScalarCommonData("SELECT NoDiscount FROM dbo.tb_Customer WHERE CustomerID=" + Session["CustID"].ToString()).ToString(), out ChkNoDiscount);
                    if (ChkNoDiscount == true && isEle == false)
                    {
                        lblInverror.Text = "You are unable to validate Promo code.";
                        return;
                    }
                    else
                    {
                        if (ChkNoDiscount == false)
                        {
                            SPMessage = Convert.ToString(CouponComponent.GetDiscountByCouponCodeFunction(txtPromoCode.Text.Trim(), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
                        }

                    }

                    try
                    {
                        if (isEle == false)
                        {
                            CouponDiscount = Convert.ToDecimal(SPMessage.ToString());
                            if (CouponDiscount > 0)
                            {
                                Session["CouponCodebycustomer"] = txtPromoCode.Text;
                                Session["CouponCodeDiscountPrice"] = CouponDiscount;
                                lblInverror.Text = "Coupon Code Successfully Applied!";
                            }
                            txtPromoCode.Text = "";
                        }
                    }
                    catch
                    {
                        if (CouponDiscount == 0)
                        {
                            GiftCardAmount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Balance FROM [tb_GiftCard] where storeid=" + AppLogic.AppConfigs("StoreID") + " And SerialNumber='" + txtPromoCode.Text.Trim() + "'"));
                            if (GiftCardAmount == Decimal.Zero)
                            {
                                Session["GiftCertificateDiscountCode"] = null;
                                Session["GiftCertificateDiscount"] = null;
                                Session["CouponCode"] = null;
                                Session["Discount"] = null;
                                lblInverror.Text = "Sorry, we are unable to validate Promo code given by you.";
                            }
                            else
                            {
                                Session["GiftCertificateDiscountCode"] = txtPromoCode.Text.ToString().Trim();
                                Session["GiftCertificateDiscount"] = Math.Round(GiftCardAmount, 2);
                                GiftCardTotalDiscount = Math.Round(GiftCardAmount, 2);
                            }
                        }
                    }
                    BindShoppingCartByCustomerID();
                    foreach (RepeaterItem rItem in RptCartItems.Items)
                    {
                        Label lblDiscountPrice = (Label)rItem.FindControl("lblDiscountprice");
                        HiddenField hdnCustomcartId = (HiddenField)rItem.FindControl("hdnCustomcartId");
                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET DiscountPrice=" + lblDiscountPrice.Text.ToString() + " WHERE CustomCartID=" + hdnCustomcartId.Value.ToString() + "");

                    }
                }
                else
                {
                    lblInverror.Text = "Please Enter Coupon Code.";
                    txtPromoCode.Focus();
                }
            }
            else
            {
                GetYoumayalsoLikeProduct(0);
                Session["NoOfCartItems"] = null;
                lblMessage.Text = "Your Shopping Cart is Empty!";
                btnClearCart.Visible = false;
                btnUpdateCart.Visible = false;
                trPromocode.Visible = false;
                trShopping.Visible = false;
                trCalculateShippingHeader.Visible = false;
                Int32 items = 0;
                System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)Page.Master.FindControl("cartlink");
                //objAnchor.InnerHtml = items > 1 ? "Items (" + items.ToString("D2") + ")" : "Item (" + items.ToString("D2") + ")";
                objAnchor.InnerHtml = items > 1 ? "<span>" + items.ToString("D2") + "</span> <strong>Item(s)</strong>" : "<span>" + items.ToString("D2") + "</span> <strong>Item(s)</strong>";
                objAnchor.HRef = "/addtocart.aspx";
            }
        }

        /// <summary>
        /// Bind Country Drop down list
        /// </summary>
        public void FillCountry()
        {
            ddlcountry.Items.Clear();
            CountryComponent objCountry = new CountryComponent();
            DataSet dscountry = new DataSet();
            dscountry = objCountry.GetAllCountries();
            if (dscountry != null && dscountry.Tables.Count > 0 && dscountry.Tables[0].Rows.Count > 0)
            {
                ddlcountry.DataSource = dscountry.Tables[0];
                ddlcountry.DataTextField = "Name";
                ddlcountry.DataValueField = "CountryID";
                ddlcountry.DataBind();
                ddlcountry.Items.Insert(0, new ListItem("Select Country", "0"));
            }
            else
            {
                ddlcountry.DataSource = null;
                ddlcountry.DataBind();
            }
            if (ddlcountry.Items.FindByText("United States") != null)
            {
                ddlcountry.Items.FindByText("United States").Selected = true;
            }
        }

        /// <summary>
        ///  Proceed to Check Out Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnproceedtocheckout_Click(object sender, ImageClickEventArgs e)
        {
            Session["PaymentMethod"] = "creditcard";

            #region Set Sessions for Country, ZipCode and Shipping Methods
            Session["SelectedCountry"] = null;
            Session["SelectedZipCode"] = null;
            Session["SelectedShippingMethod"] = null;

            if (ddlcountry.SelectedIndex != 0)
            {
                Session["SelectedCountry"] = Convert.ToString(ddlcountry.SelectedValue);
            }
            if (txtZipCode.Text != "")
            {
                Session["SelectedZipCode"] = txtZipCode.Text.ToString().Trim();
            }
            for (int i = 0; i < rdoShipping.Items.Count; i++)
            {
                if (rdoShipping.Items[i].Selected)
                {
                    Session["SelectedShippingMethod"] = rdoShipping.Items[i].Value.ToString().Trim();
                }
            }

            #endregion

            if (Session["UserName"] == null || Session["UserName"].ToString() == "")
            {
                //Response.Redirect("/login.aspx");
                Response.Redirect("/CheckoutCommon.aspx");
            }
            else
            {
                CustomerComponent objcust = new CustomerComponent();
                if (objcust.GetBlankDetailsofCustomer(Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreId"))) == 0)
                    Response.Redirect("/createaccount.aspx?type=isfbuser&mode=edit");
                else
                {
                    Response.Redirect("/checkoutcommon.aspx");
                }
            }
        }

        #region Bind ShippingMethod
        /// <summary>
        /// Bind Shipping Methods
        /// </summary>
        /// <param name="Country">string Country</param>
        /// <param name="State">string State</param>
        /// <param name="ZipCode">string ZipCode</param>
        /// <param name="Weight">Decimal Weight</param>
        /// <param name="Service">strnig Service</param>
        private void BindShippingMethod(string Country, string State, string ZipCode, decimal Weight, string Service)
        {
            bool CheckFreeShaippingOrder = false;
            string strUSPSMessage = "";
            string strUPSMessage = "";
            string strFedexSMessage = "";
            lblMsg.Text = "";


            rdoShipping.Items.Clear();
            if (ZipCode == "" || Country == "")
            {
                //lblMsg.Visible = true;
                //lblMsg.Text = "Select Country and Enter Zip Code";
                return;
            }

            ShippingComponent objShipping = new ShippingComponent();
            DataSet objShipServices = new DataSet();
            objShipServices = objShipping.GetShippingServices(Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            string CountryCode = Country;


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


            #region Code for Gift Certificate

            int totalItemCount = RptCartItems.Items.Count;
            int IGiftCount = 0;
            for (int i = 0; i < totalItemCount; i++)
            {
                HiddenField hdnProductId = (HiddenField)RptCartItems.Items[i].FindControl("hdnProductId");
                if (hdnProductId != null)
                {

                    int GiftCardProductID = 0;
                    GiftCardProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(GiftCardProductID,0) FROM dbo.tb_GiftCardProduct Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + hdnProductId.Value.ToString() + ""));
                    if (GiftCardProductID > 0)
                    {
                        IGiftCount += 1;
                    }

                }
            }
            if (IGiftCount == RptCartItems.Items.Count)
            {
                String strFreeShipping = "Standard Shipping($0.00)";
                DataRow dataRow = ShippingTable.NewRow();
                dataRow["ShippingMethodName"] = strFreeShipping;
                dataRow["Price"] = 0;
                ShippingTable.Rows.Add(dataRow);
                CheckFreeShaippingOrder = true;
                if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                {
                    DataView dvShipping = ShippingTable.DefaultView;
                    dvShipping.Sort = "Price asc";
                    rdoShipping.DataSource = dvShipping.ToTable();
                    rdoShipping.DataTextField = "ShippingMethodName";
                    rdoShipping.DataValueField = "ShippingMethodName";
                    rdoShipping.DataBind();
                }
                return;
            }
            #endregion

            if (objShipServices != null && objShipServices.Tables.Count > 0 && objShipServices.Tables[0].Rows.Count > 0)
            {
                if (objShipServices.Tables[0].Select("ShippingService='UPS'").Length > 0)
                {
                    UPSTable = UPSMethodBind(CountryCode.ToString(), State, ZipCode, Weight, "UPS", ref strUPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='USPS'").Length > 0)
                {
                    EndiciaService objRate = new EndiciaService();
                    USPSTable = objRate.EndiciaGetRates(ZipCode, CountryCode.ToString(), Convert.ToDouble(Weight), ref strUSPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='FEDEX'").Length > 0)
                {
                    FedexTable = FedexMethod(Convert.ToDecimal(Weight), State, ZipCode, CountryCode, ref strFedexSMessage);
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

                bool IsFreeShipping = false;
                bool IsDiscountAllowIncludeFreeShipping = false;
                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {

                    IsFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasFreeShipping,0) FROM tb_CustomerLevel inner JOIN dbo.tb_Customer ON tb_Customer.CustomerLevelID=tb_CustomerLevel.CustomerLevelID WHERE tb_Customer.CustomerID=" + Convert.ToInt32(Session["CustID"].ToString()) + ""));

                }

                if (Session["CouponCode"] != null && Session["CouponCode"].ToString() != "")
                {
                    if (IsFreeShipping == false)
                        IsDiscountAllowIncludeFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT DiscountAllowIncludeFreeShipping FROM dbo.tb_Coupons WHERE CouponCode='" + Session["CouponCode"].ToString() + "'"));
                }
                if (IsFreeShipping)
                {

                    if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
                    {
                        //String strFreeShipping = "Standard Shipping($0.00)";
                        //DataRow dataRow = ShippingTable.NewRow();
                        //dataRow["ShippingMethodName"] = strFreeShipping;
                        //dataRow["Price"] = 0;
                        //ShippingTable.Rows.Add(dataRow);
                        CheckFreeShaippingOrder = true;
                    }
                }
                else if (IsDiscountAllowIncludeFreeShipping)
                {
                    if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
                    {
                        //String strFreeShipping = "Standard Shipping($0.00)";
                        //DataRow dataRow = ShippingTable.NewRow();
                        //dataRow["ShippingMethodName"] = strFreeShipping;
                        //dataRow["Price"] = 0;
                        //ShippingTable.Rows.Add(dataRow);
                        CheckFreeShaippingOrder = true;
                    }
                }

                else if (CheckFreeShaippingOrder == false)
                {
                    if (CountryCode.ToString().Trim().ToUpper() == "US" || CountryCode.ToString().Trim().ToUpper() == "UNITED STATES")
                    {
                        if (FinalTotal > FreeShippingAmount)
                        {
                            if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
                            {
                                //String strFreeShipping = "Standard Shipping($0.00)";
                                //DataRow dataRow = ShippingTable.NewRow();
                                //dataRow["ShippingMethodName"] = strFreeShipping;
                                //dataRow["Price"] = 0;
                                //ShippingTable.Rows.Add(dataRow);
                                CheckFreeShaippingOrder = true;
                            }
                        }
                    }
                    else
                    {
                        lblFreeShippningMsg.Visible = false;
                    }
                }

                decimal OrderTotal = 0;
                decimal SubTotal = 0;
                double Price = 0;
                if (ViewState["FinalTotal"] != null)
                {
                    OrderTotal = Convert.ToDecimal(ViewState["FinalTotal"].ToString());
                }
                if (!string.IsNullOrEmpty(hdnSubTotalofProduct.Value) && Convert.ToDecimal(hdnSubTotalofProduct.Value) > 0)
                {
                    SubTotal = Convert.ToDecimal(hdnSubTotalofProduct.Value);
                }
                if (CountryCode.ToString().Trim().ToUpper() == "US" || CountryCode.ToString().Trim().ToUpper() == "UNITED STATES")
                {
                    if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                    {
                        string strfreeshipping = Convert.ToString(CommonComponent.GetScalarCommonData("Select isnull(configvalue,'0') from tb_AppConfig WHERE Configname ='FreeShippingLimit' and isnull(Deleted,0)=0 and StoreId=1"));
                        for (int k = 0; k < ShippingTable.Rows.Count; k++)
                        {
                            if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ground") > -1)
                            {
                                string[] strMethodname = ShippingTable.Rows[k]["ShippingMethodName"].ToString().Split("($".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string Shippingname = "";
                                if (strMethodname.Length > 0)
                                {
                                    Shippingname = strMethodname[0].ToString().Replace("ups ", "").Replace("UPS ", ""); ;
                                }

                                if (OrderTotal >= 0)
                                {
                                    if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(strfreeshipping))
                                    {
                                        Price = 0;
                                        lblFreeShippningMsg.Text = "Congratulations!! You qualified for Free Shipping. ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(0) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(29.99))
                                    {
                                        Price = 5.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(30.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(69.99))
                                    {
                                        Price = 7.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(70.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(129.99))
                                    {
                                        Price = 12.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(130.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(199.99))
                                    {
                                        Price = 16.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(200.00) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(248.99))
                                    {
                                        Price = 21.99;
                                        Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                        lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    }
                                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(0) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(9.99))
                                    //{
                                    //    Price = 2.99;
                                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    //}
                                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(10) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(30.99))
                                    //{
                                    //    Price = 5.99;
                                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    //}
                                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(31) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(50.99))
                                    //{
                                    //    Price = 9.99;
                                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    //}
                                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(51) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(99.99))
                                    //{
                                    //    Price = 14.99;
                                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    //}
                                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(100) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(149.99))
                                    //{
                                    //    Price = 15.99;
                                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    //}
                                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(150) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(199.99))
                                    //{
                                    //    Price = 17.99;
                                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    //}
                                    //else if (Convert.ToDouble(OrderTotal) >= Convert.ToDouble(200) && Convert.ToDouble(OrderTotal) <= Convert.ToDouble(248.99))
                                    //{
                                    //    Price = 19.99;

                                    //    Decimal TotalDiff = Convert.ToDecimal(strfreeshipping) - Convert.ToDecimal(OrderTotal);
                                    //    lblFreeShippningMsg.Text = "Spend " + TotalDiff.ToString("C2") + " more and receive Free Shipping! ( United States Only )";
                                    //}

                                    Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(Price)) + ")";
                                }
                                ShippingTable.Rows[k]["ShippingMethodName"] = Shippingname.Replace("ups ", "").Replace("UPS ", "");
                                ShippingTable.Rows[k]["Price"] = Convert.ToDecimal(Price);
                                ShippingTable.AcceptChanges();
                            }
                            else if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups standard") > -1)
                            {
                                ShippingTable.Rows.RemoveAt(k);
                                ShippingTable.AcceptChanges();
                                k--;

                            }
                            else if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups ") > -1)
                            {
                                ShippingTable.Rows.RemoveAt(k);
                                ShippingTable.AcceptChanges();
                                k--;

                            }

                        }

                        if (SubTotal > 0)
                        {
                            //decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.18);
                            //DataRow dr;
                            //dr = ShippingTable.NewRow();
                            //dr["ShippingMethodName"] = "USA-3 DAY Express Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                            //dr["Price"] = Convert.ToDecimal(ShippingPrice);
                            //ShippingTable.Rows.Add(dr);

                            //ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.22);
                            //DataRow drnext;
                            //drnext = ShippingTable.NewRow();
                            //drnext["ShippingMethodName"] = "USA-Next Day Shipping" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(ShippingPrice)) + ")";
                            //drnext["Price"] = Convert.ToDecimal(ShippingPrice);
                            //ShippingTable.Rows.Add(drnext);
                        }
                    }
                }
                else if (CountryCode.ToUpper() == "CA" || CountryCode.ToString().Trim().ToUpper() == "CANADA")
                {
                    //decimal ShippingPrice = Convert.ToDecimal(SubTotal) * Convert.ToDecimal(0.18);
                    DataRow dr;
                    //dr = ShippingTable.NewRow();
                    //dr["ShippingMethodName"] = "UPS Standard to Canada" + "($" + string.Format("{0:0.00}", Convert.ToDecimal(0)) + ")";
                    //dr["Price"] = 0;
                    //ShippingTable.Rows.Add(dr);

                    for (int k = 0; k < ShippingTable.Rows.Count; k++)
                    {
                        if (ShippingTable.Rows[k]["ShippingMethodName"].ToString().ToLower().IndexOf("ups standard") <= -1)
                        {
                            ShippingTable.Rows.RemoveAt(k);
                            ShippingTable.AcceptChanges();
                            k--;
                        }
                    }

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
                    DataRow dr;
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
                }

                if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                {
                    DataView dvShipping = ShippingTable.DefaultView;
                    dvShipping.Sort = "Price asc";
                    rdoShipping.DataSource = dvShipping.ToTable();
                    rdoShipping.DataTextField = "ShippingMethodName";
                    rdoShipping.DataValueField = "ShippingMethodName";
                    rdoShipping.DataBind();
                }
                if (rdoShipping.Items.Count > 0 && rdoShipping.SelectedIndex <= -1)
                {
                    rdoShipping.SelectedIndex = 0;
                }

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
            }
            ChkFreeshipping();
        }

        /// <summary>
        /// Fedex Method Bind
        /// </summary>
        /// <param name="Weight">Decimal Weight</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Country">String Country</param>
        /// <param name="StrMessage">Return Error Message </param>
        /// <returns></returns>

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

        /// <summary>
        /// UPS Method Bind
        /// </summary>
        /// <param name="Country">String CountryCode</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Weight">Decimal Weight</param>
        /// 

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




        /// <summary>
        /// Get Shipping Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            rdoShipping.Items.Clear();
            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(ddlcountry.SelectedValue.ToString()));
            if (ViewState["Weight"] != null)
            {
                BindShippingMethod(CountryCode, "", txtZipCode.Text.ToString(), Convert.ToDecimal(ViewState["Weight"].ToString()), "UPS");
            }
            else
            {
                BindShippingMethod(CountryCode, "", txtZipCode.Text.ToString(), Convert.ToDecimal(1), "UPS");
            }
        }
        /// <summary>
        /// Shipping Cost Apply in Cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdoShipping_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindShoppingCartByCustomerID();
        }
        #endregion

        /// <summary>
        /// Insert Values in the Cart table also add another items
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnAddAnotherItem_Click(object sender, ImageClickEventArgs e)
        {
            if (ViewState["Urlrefferer"] != null)
            {
                DataSet dsShoppingCart = new DataSet();
                if (Session["CustID"] != null)
                    dsShoppingCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));

                string URlString = ViewState["Urlrefferer"].ToString().ToLower();
                if (URlString.Contains("gi-"))
                {
                    Response.Redirect("/GiftCertificate.aspx");
                }
                else if (URlString.Contains("/checkoutcommon.aspx"))
                {
                    Response.Redirect("/index.aspx");
                }
                else if (URlString.Contains("/createaccount.aspx"))
                {
                    Response.Redirect("/index.aspx");
                }
                else if (URlString.Contains("/addtocart.aspx"))
                {
                    Response.Redirect("/index.aspx");
                }
                else
                {
                    Response.Redirect(ViewState["Urlrefferer"].ToString());
                }
            }
            else
            {
                Response.Redirect("/index.aspx");
            }
        }

        /// <summary>
        /// Clear Shopping Cart
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnClearCart_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
            {
                ShoppingCartComponent objWishlist = new ShoppingCartComponent();
                objWishlist.AddToWishList(Convert.ToInt32(Session["CustID"].ToString()));
                Session["CouponCode"] = null;
                Session["CouponCodebycustomer"] = null;
                Session["CouponCodeDiscountPrice"] = null;
                Response.Redirect("/wishlist.aspx", true);
            }
            else
            {
                Response.Redirect("/login.aspx?wishlist=1", true);
            }
        }

        #region Remove ViewState From page

        /// <summary>
        /// Remove ViewState From page
        /// </summary>
        /// <returns></returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            if (Session[Session.SessionID] != null)
                return (new LosFormatter().Deserialize((string)Session[Session.SessionID]));
            return null;
        }

        /// <summary>
        /// Save Page State To Persistence Medium
        /// </summary>
        /// <param name="state">object state</param>
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
        /// Get You may also Like product
        /// </summary>
        /// <param name="ProductID">int Product ID</param>
        private void GetYoumayalsoLikeProduct(int ProductID)
        {
            DataSet dsYouMay = new DataSet();

            ProductComponent objYoumay = new ProductComponent();
            Int32 Custid = 0;
            if (Session["CustID"] != null)
            {
                Custid = Convert.ToInt32(Session["CustID"].ToString());
            }
            dsYouMay = objYoumay.GetRelatedProductAddTocart(Custid, Convert.ToInt32(AppLogic.AppConfigs("StoreId")));
            Int32 iCount = 0;


            if (dsYouMay != null && dsYouMay.Tables.Count > 0 && dsYouMay.Tables[0].Rows.Count > 0)
            {

                string TName = "";
                decimal price = 0;
                decimal salePrice = 0;
                if (!string.IsNullOrEmpty(dsYouMay.Tables[0].Rows[0]["price"].ToString()))
                {
                    price = Convert.ToDecimal(dsYouMay.Tables[0].Rows[0]["price"].ToString());
                }
                if (!string.IsNullOrEmpty(dsYouMay.Tables[0].Rows[0]["saleprice"].ToString()))
                {
                    salePrice = Convert.ToDecimal(dsYouMay.Tables[0].Rows[0]["saleprice"].ToString());
                }

                ltrImage.Text = "<a title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[0]["Tooltip"].ToString()) + "\" href=\"/" + dsYouMay.Tables[0].Rows[0]["MainCategory"].ToString() + "/" + dsYouMay.Tables[0].Rows[0]["sename"].ToString() + "-" + dsYouMay.Tables[0].Rows[0]["productId"].ToString() + ".aspx\">";
                Random rd = new Random();
                if (!string.IsNullOrEmpty(dsYouMay.Tables[0].Rows[0]["imagename"].ToString()))
                {
                    if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsYouMay.Tables[0].Rows[0]["imagename"].ToString())))
                    {
                        ltrImage.Text += "<img title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[0]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsYouMay.Tables[0].Rows[0]["Tooltip"].ToString()) + "\" src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsYouMay.Tables[0].Rows[0]["imagename"].ToString() + "?" + rd.Next(1000).ToString() + "\">";
                    }
                    else
                    {
                        ltrImage.Text += "<img  title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[0]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsYouMay.Tables[0].Rows[0]["Tooltip"].ToString()) + "\" src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(1000).ToString() + "\">";
                    }
                }
                else
                {
                    ltrImage.Text += "<img  title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[0]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsYouMay.Tables[0].Rows[0]["Tooltip"].ToString()) + "\" src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(1000).ToString() + "\">";
                }

                ltrImage.Text += "</a>";

                ltrname.Text = "<a title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[0]["Tooltip"].ToString()) + "\" href=\"/" + dsYouMay.Tables[0].Rows[0]["MainCategory"].ToString() + "/" + dsYouMay.Tables[0].Rows[0]["sename"].ToString() + "-" + dsYouMay.Tables[0].Rows[0]["productId"].ToString() + ".aspx\">" + dsYouMay.Tables[0].Rows[0]["Sortname"].ToString() + "</a>";

                try
                {
                    TName = dsYouMay.Tables[0].Rows[0]["TagName"].ToString();

                    if (!string.IsNullOrEmpty(TName.ToString().Trim()))
                    {

                        if (!string.IsNullOrEmpty(ltrImage.Text) && !ltrImage.Text.ToString().ToLower().Contains("image_not_available"))
                        {
                            lblTagImage.Text = "<img title='" + TName.ToString().Trim() + "' src=\"/images/" + TName.ToString().Trim() + ".png\" alt=\"" + TName.ToString().Trim() + "\" class='" + TName.ToString().ToLower() + "' style='' />";
                        }
                    }
                }
                catch { }
                if (price > decimal.Zero)
                {
                    ltrregular.Text = "" + price.ToString("C") + "";
                }
                else
                {
                    ltrregular.Text = "&nbsp;";
                }

                if (salePrice > decimal.Zero && price > salePrice)
                {
                    ltryourprice.Text = "" + salePrice.ToString("C") + "";
                }
                else
                {
                    ltryourprice.Text = "" + price.ToString("C") + "";
                }

                ltrAddtocart.Text = "<a title=\"View More\" href=\"/" + dsYouMay.Tables[0].Rows[0]["MainCategory"].ToString() + "/" + dsYouMay.Tables[0].Rows[0]["sename"].ToString() + "-" + dsYouMay.Tables[0].Rows[0]["productId"].ToString() + ".aspx\"><img alt=\"View More\" src=\"/images/view_more.jpg\"></a></span>";


            }


        }

        /// <summary>
        /// Replace the Space which is comes in ProductName to "-"
        /// </summary>
        /// <param name="Name">ProductName</param>
        /// <returns>return the ProductName with Replace the Space which is comes in ProductName to "-" </returns>
        public String SetAttribute(String Name)
        {
            return Name.Replace('"', '-').Replace('\'', '-').ToString();
        }

        /// <summary>
        /// Set Href for Gift Card Products and Normal Products 
        /// </summary>
        /// <param name="mainCategory">String mainCategory</param>
        /// <param name="Sename">String Sename</param>
        /// <param name="ProductId">String ProductId</param>
        /// <param name="CustomCartID">String CustomCartID</param>
        /// <returns>Returns Product URL</returns>
        public String GetProductUrl(String mainCategory, String Sename, String ProductId, string CustomCartID)
        {
            string Url = "";
            int GiftCardProductID = 0;
            GiftCardProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(GiftCardProductID,0) FROM dbo.tb_GiftCardProduct Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + ProductId.ToString() + ""));
            if (GiftCardProductID > 0)
            {
                Url = "/gi-" + CustomCartID + "-";
                if (Sename != "")
                {
                    Url += Sename.ToString();
                }
            }
            else
            {
                if (mainCategory != "")
                {
                    Url = "/" + mainCategory.ToString();
                }
                if (Sename != "")
                {
                    Url += "/" + Sename.ToString();
                }
            }
            if (ProductId != "")
            {
                Url += "-" + ProductId.ToString() + ".aspx";
            }
            return Url.ToString();
        }

        /// <summary>
        /// Set Product Name
        /// </summary>
        /// <param name="Name">Name of Product or Category</param>
        /// <returns>Return Max. 62 Length String </returns>
        public String SetName(String Name)
        {
            if (Name.Length > 40)
                Name = Name.Substring(0, 35) + "...";
            return Server.HtmlEncode(Name);
        }

        /// <summary>
        /// Bind Cross Sell Products
        /// </summary>
        protected void BindCrossSellProducts()
        {
            string CrossSellSKUs = AppLogic.AppConfigs("CrossSellProducts");
            if (CrossSellSKUs != "" && CrossSellSKUs != null)
            {
                string[] CrossSellSKU = CrossSellSKUs.Split(',');
                CrossSellSKUs = "";
                for (int i = 0; i < CrossSellSKU.Length; i++)
                {
                    if (CrossSellSKUs != "")
                    {
                        CrossSellSKUs += ",'" + CrossSellSKU[i] + "'";
                    }
                    else
                    {
                        CrossSellSKUs = "'" + CrossSellSKU[i] + "'";
                    }
                }
            }
            DataSet ds = CommonComponent.GetCommonDataSet("select ProductID, Name, SKU, MainCategory, SEName, ImageName, Tooltip from tb_Product where SKU in (" + CrossSellSKUs + ")");
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count != 0)
            {
                rptCrossSellProducts.DataSource = ds.Tables[0];
                rptCrossSellProducts.DataBind();
            }
        }

        /// <summary>
        /// Get Icon Image Products
        /// </summary>
        /// <param name="img">(String img</param>
        /// <returns>Returns Icon Image Products</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (imagepath)))
            {
                return AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }
    }
}