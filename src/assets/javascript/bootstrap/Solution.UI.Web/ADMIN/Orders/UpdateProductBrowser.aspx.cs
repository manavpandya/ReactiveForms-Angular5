using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class UpdateProductBrowser : BasePage
    {
        Int32 OrderedShoppingCartID = 0;
        bool flg = true;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtProduct.Focus();
                hfStoreID.Value = Request.QueryString["StoreID"];
                imgLogo.Src = AppLogic.AppConfigs("LIVE_SERVER").TrimEnd("/".ToCharArray()) + "/images/logo.png";
            }
            imgMainDiv.Src = "~/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
            btnBrowse.ImageUrl = "/App_Themes/" + Page.Theme + "/images/search.gif";
            btnAddtoCart1.ImageUrl = "/App_Themes/" + Page.Theme + "/images/Add-to-cart.gif";
            btnAddtoCart2.ImageUrl = "/App_Themes/" + Page.Theme + "/images/Add-to-cart.gif";
        }

        /// <summary>
        ///  Browse Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnBrowse_Click(object sender, ImageClickEventArgs e)
        {
            SearchProduct();
        }

        /// <summary>
        /// Searches the Product
        /// </summary>
        private void SearchProduct()
        {
            try
            {
                Int32 StoreID = 0;
                if (Request.QueryString["StoreID"] != null)
                    StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);

                if (StoreID == 0)
                    StoreID = 1;

                DataSet dsProducts = new DataSet();
                String strSql = "";
                strSql = "select distinct p.ProductID,SKU,Name, "
                    + " (Case When (SalePrice Is Not Null And SalePrice!=0) Then SalePrice  Else Price End) as SalePrice,Inventory,Weight "
                    + " from tb_Product p where p.ProductID not in(Select ProductID from tb_GiftCardProduct) and StoreID=" + StoreID
                    + " and p.productid in "
                    + " (select ProductID from tb_Product where ProductTypeID in (1,2,3) and isnull(inventory,0) > 0 and deleted=0 and active=1 and (SKU like '%" + txtProduct.Text.Trim().Replace("'", "''") + "%' or Name like '%" + txtProduct.Text.Trim().Replace("'", "''") + "%')) "
                    + " OR (p.productid in (select ProductID from tb_Product where ProductTypeID in (4) and (SKU like '%" + txtProduct.Text.Trim().Replace("'", "''") + "%' or Name like '%" + txtProduct.Text.Trim().Replace("'", "''") + "%'))) order by name,SKU";

                dsProducts = CommonComponent.GetCommonDataSet(strSql);
                if (dsProducts != null && dsProducts.Tables.Count > 0 && dsProducts.Tables[0].Rows.Count > 0)
                {
                    btnAddtoCart1.Visible = true;
                    btnAddtoCart2.Visible = true;
                }
                else
                {
                    btnAddtoCart1.Visible = false;
                    btnAddtoCart2.Visible = false;
                }

                grdProducts.DataSource = dsProducts.Tables[0];
                grdProducts.DataBind();
                txtProduct.Focus();
            }
            catch { }
        }

        /// <summary>
        ///  Add to cart Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddtoCart_Click(object sender, ImageClickEventArgs e)
        {
            lblMsg.Text = "";
            if (string.IsNullOrEmpty(hfAddToCart.Value))
                return;

            String[] arrProducts = hfAddToCart.Value.Split("::".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Int32 OrderNumber = 0;
            Int32.TryParse(hfOrderNumber.Value, out OrderNumber);
            Int32 ProductID, Quantity;
            Decimal SalePrice = 0, Weight = 0;

            foreach (String strProduct in arrProducts)
            {
                String[] arrPortions = strProduct.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (arrPortions.Length < 2)
                    continue;
                Int32.TryParse(arrPortions[0], out ProductID);
                flg = CheckDuplicateProduct(ProductID);
                if (flg == false)
                    goto a;
            }

            foreach (String strProduct in arrProducts)
            {
                String[] arrPortions = strProduct.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (arrPortions.Length < 2)
                    continue;
                Int32.TryParse(arrPortions[0], out ProductID);
                Int32.TryParse(arrPortions[1], out Quantity);
                Decimal.TryParse(arrPortions[2], out SalePrice);
                Decimal.TryParse(arrPortions[3], out Weight);
                arrPortions[0] = null; arrPortions[1] = null;
                arrPortions[2] = null; arrPortions[3] = null;

                AddToUpdateProductcart(ProductID, Quantity, SalePrice, Weight);
            }

            Page.RegisterStartupScript("parentLoad", "<script type='text/javascript' language='javascript'>if(window.opener){ window.opener.location=window.opener.location;window.opener.focus();window.close();}</script>");
        a: { }
        }

        /// <summary>
        /// Binds the Quantity by ProductID
        /// </summary>
        /// <param name="ProductID">string ProductID</param>
        /// <returns>Returns Quantity as a Link String</returns>
        protected string BindQuantity(String ProductID)
        {
            if (String.IsNullOrEmpty(ProductID))
                return string.Empty;
            return "<input type='text' onkeypress=\"return onKeyPressBlockNumbersWithoutDot(event)\" class='textfield_small' id='qty" + ProductID + "' style='width:30px;text-align:center;border:1px solid #BCC0C1;' value='0'/>";
        }

        /// <summary>
        /// Display the Price as a Link
        /// </summary>
        /// <param name="SalePrice">string SalePrice</param>
        /// <param name="ProductID">string ProductID</param>
        /// <returns>Returns Price as a Link String</returns>
        protected string BindPrice(String SalePrice, String ProductID)
        {
            if (String.IsNullOrEmpty(SalePrice))
                return string.Empty;
            return "<input type='text' onkeypress=\"return onKeyPressBlockNumbers(event)\" class='textfield_small' id='txtprice" + ProductID + "' style='width:60px;text-align:center;border:1px solid #BCC0C1;' value='" + Convert.ToDecimal(SalePrice).ToString("f2") + "'/>";
        }

        /// <summary>
        /// Display the Weight as a Link
        /// </summary>
        /// <param name="Weight">string weight</param>
        /// <param name="ProductID">string ProductID</param>
        /// <returns>Returns Weight as a Link String</returns>
        protected string BindWeight(String Weight, String ProductID)
        {
            if (String.IsNullOrEmpty(Weight))
                return string.Empty;
            return "<input type='text' class='textfield_small' id='txtweight" + ProductID + "' style='width:40px;text-align:center;border:1px solid #BCC0C1;' value='" + Convert.ToDecimal(Weight).ToString("f2") + "'/>";
        }

        /// <summary>
        /// Display the Variant Name and Its Value
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="ProductID">String ProductID</param>
        /// <param name="variantValues">String variantValues</param>
        /// <returns>Returns Variant Details as a String</returns>
        protected string BindVariant(int ID, String ProductID, string variantValues)
        {
            if (String.IsNullOrEmpty(variantValues) || String.IsNullOrEmpty(ProductID))
                return string.Empty;

            String[] arrValues = variantValues.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (arrValues.Length < 2)
                return String.Empty;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<select id='sel" + ID + "-" + ProductID + "' class='select_box'>");
            sb.Append("<option>" + arrValues[0] + "</option>");
            arrValues[0] = null;
            foreach (String value in arrValues)
            {
                if (!string.IsNullOrEmpty(value))
                    sb.Append("<option>" + value + "</option>");
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// Display the Colors
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="ProductID">String ProductID</param>
        /// <param name="Colors">string Color</param>
        /// <returns>Returns the Colors as a String Value</returns>
        protected string BindColors(int ID, String ProductID, string Colors)
        {
            if (String.IsNullOrEmpty(Colors) || String.IsNullOrEmpty(ProductID))
                return string.Empty;

            String[] arrValues = Colors.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (arrValues.Length == 0)
                return String.Empty;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<select id='sel" + ID + "-" + ProductID + "' class='select_box'>");
            sb.Append("<option>Color</option>");
            //arrValues[0] = null;
            foreach (String value in arrValues)
            {
                if (!string.IsNullOrEmpty(value))
                    sb.Append("<option>" + value + "</option>");
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// Binds the Size
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="ProductID">string ProductID</param>
        /// <param name="Size">string Size</param>
        /// <returns>Returns the size as a string</returns>
        protected string BindSize(int ID, String ProductID, string Size)
        {
            if (String.IsNullOrEmpty(Size) || String.IsNullOrEmpty(ProductID))
                return string.Empty;

            String[] arrValues = Size.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (arrValues.Length == 0)
                return String.Empty;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<select id='sel" + ID + "-" + ProductID + "' class='select_box'>");
            sb.Append("<option>Size</option>");
            //arrValues[0] = null;
            foreach (String value in arrValues)
            {
                if (!string.IsNullOrEmpty(value))
                    sb.Append("<option>" + value + "</option>");
            }
            sb.Append("</select>");
            return sb.ToString();
        }

        /// <summary>
        /// Checks the Duplicate Product
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>true if exists, false otherwise</returns>
        protected bool CheckDuplicateProduct(int ProductID)
        {
            string SKU = "";
            OrderedShoppingCartID = Convert.ToInt32(Request.QueryString["OSCID"]);

            string strSql = "select osi.refproductid,p.sku from tb_OrderedShoppingCartItems osi, tb_Product p where p.productid=osi.refproductid and osi.OrderedShoppingCartID=" + OrderedShoppingCartID + " and osi.refProductID=" + ProductID;
            DataSet ds = CommonComponent.GetCommonDataSet(strSql);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                SKU = ds.Tables[0].Rows[0]["SKU"].ToString();
                lblMsg.Text += "<br/>This product " + SKU + " is already available in Order List, Please revise it from there.";
                flg = false;
            }
            return flg;
        }

        /// <summary>
        /// Adds Product into  Update Product Cart
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Qty">int Qty</param>
        /// <param name="SalePrice">decimal SalePrice</param>
        /// <param name="Weight">decimal Weight</param>
        protected void AddToUpdateProductcart(Int32 ProductID, Int32 Qty, Decimal SalePrice, Decimal Weight)
        {
            OrderedShoppingCartID = Convert.ToInt32(Request.QueryString["OSCID"]);

            #region Variant

            string variantproductids = "";
            string strVariantNames = "";
            try
            {
                DataSet dsvariant = new DataSet();
                dsvariant = CommonComponent.GetCommonDataSet("SELECT pv.VariantID,pv.ProductID,pv.VariantName,pvv.VariantValue,pvv.VariantProductID FROM dbo.tb_ProductVariant pv  " +
                                        " INNER JOIN dbo.tb_ProductVariantValue pvv ON pvv.VariantID=pv.VariantID " +
                                        " WHERE pv.ProductID=" + ProductID);


                foreach (DataRow dr in dsvariant.Tables[0].Rows)
                {
                    if (strVariantNames != (dr["VariantName"].ToString()))
                    {
                        strVariantNames = dr["VariantName"].ToString();
                        variantproductids = variantproductids + dr["VariantProductID"].ToString() + ",";
                    }
                }
            }
            catch (Exception)
            {

            }

            #endregion

            string strSql = @"Insert into tb_OrderedShoppingCartItems 
                        (OrderedShoppingCartID,RefProductID,quantity,price,ProductName,SKU,Weight) 
                        select " + OrderedShoppingCartID + "," + ProductID + "," + Qty + "," + SalePrice + ",name,SKU," + Weight + "  from tb_Product where ProductID=" + ProductID;
            decimal OrderTotal = 0;
            OrderTotal = Convert.ToDecimal(Qty * SalePrice);
            string Query = "update tb_Order set OrderSubTotal= OrderSubTotal + " + OrderTotal + ",OrderTotal= OrderTotal + " + OrderTotal + "  where OrderNumber=" + Request.QueryString["Ono"].ToString() + "";
            CommonComponent.ExecuteCommonData(Query);
            strSql += " Select ident_current('tb_OrderedShoppingCartItems')";
            Int32 id1 = Convert.ToInt32(CommonComponent.GetScalarCommonData(strSql));

            #region Package ShoppingCartItems

            try
            {
                strSql = "";
                if (!string.IsNullOrEmpty(variantproductids))
                {
                    string[] tmp = variantproductids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        DataSet dstmp = new DataSet();
                        dstmp = CommonComponent.GetCommonDataSet("select name,SKU,isnull(SalePrice,0) as SalePrice from tb_Product where productID=" + tmp[i] + "");
                        string prName = Convert.ToString(dstmp.Tables[0].Rows[0]["Name"]);
                        string prsku = Convert.ToString(dstmp.Tables[0].Rows[0]["SKU"]);
                        decimal price1 = 0;
                        decimal.TryParse(Convert.ToString(dstmp.Tables[0].Rows[0]["SalePrice"]), out price1);
                        strSql += "insert into tb_OrderedPackageShoppingCartItems (OrderedCustomCartID,ProductID,Name,Quantity,Price,SKU) values(" + id1 + "," + tmp[i] + ",'" + prName + "'," + Qty + "," + Convert.ToDecimal(price1) + ",'" + prsku + "')";
                    }

                    if (!string.IsNullOrEmpty(strSql))
                        CommonComponent.ExecuteCommonData(strSql);
                }
            }
            catch { }

            #endregion
        }

    }
}