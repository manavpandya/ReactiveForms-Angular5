using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Text;
using System.IO;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class Wishlistitems : BasePage
    {
        #region local variables

        System.Web.UI.WebControls.Literal ltrvartable = null;
        Int32 CustomerID = 0;

        #endregion



        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
                {
                    AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                }
                else
                {
                    if (!string.IsNullOrEmpty(AppLogic.AppConfigs("StoreID")))
                        AppConfig.StoreID = Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString());
                    else
                        AppConfig.StoreID = 1;
                }
                CustomerID = Convert.ToInt32(Request.QueryString["CNo"]);

                imgLogo.Src = AppLogic.AppConfigs("LIVE_SERVER").TrimEnd("/".ToCharArray()) + "/images/logo.png";
                imgMainDiv.Src = "~/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
                btnPrint.ImageUrl = "/App_Themes/" + Page.Theme + "/images/print.png";
                if (Request.QueryString["CNo"] != null)
                {
                    BindCart(Convert.ToInt32(Request.QueryString["CNo"]));
                }
            }
        }

        /// <summary>
        /// Binds the cart.
        /// </summary>
        /// <param name="CartID">The CartID</param>
        private void BindCart(Int32 CartID)
        {

            try
            {
                Decimal NetPrice = Decimal.Zero;
                Decimal SubTotal = Decimal.Zero;
                //Decimal OrderTotal = Decimal.Zero;
                // Int32 WishlistID = Convert.ToInt32(Request.QueryString["WNo"]);
                CustomerID = Convert.ToInt32(Request.QueryString["CNo"]);
                // Int32 StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
                //Decimal QtyDiscount = Decimal.Zero;
                ltCart.Text = "";

                string strSql = "SELECT isnull(dbo.tb_WishListItems.VariantNames,'') as VariantNames,isnull(dbo.tb_WishListItems.VariantValues,'') as VariantValues,  dbo.tb_WishListItems.ProductID, dbo.tb_WishListItems.CustomerID, dbo.tb_Product.Name, dbo.tb_Product.SKU, " +
                                 " dbo.tb_Product.SEName, case when isnull(dbo.tb_Product.SalePrice,0) = 0 then  isnull(dbo.tb_Product.Price,0) else isnull(dbo.tb_Product.SalePrice,0) end as Price, " +
                                 "  dbo.tb_WishListItems.Quantity, dbo.tb_Product.Deleted, dbo.tb_Product.Active, " +
                                " dbo.tb_Product.ImageName, dbo.tb_Product.MainCategory FROM         dbo.tb_WishListItems INNER JOIN  " +
                                 " dbo.tb_Product ON dbo.tb_WishListItems.ProductID = dbo.tb_Product.ProductID where tb_WishListItems.CustomerID=" + CustomerID + "";
                DataSet ds = new DataSet();
                ds = CommonComponent.GetCommonDataSet(strSql);
                System.Text.StringBuilder Table = new StringBuilder(); int NumberOfItems = 0;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Table.Append("<table width=\"100%\" border=\"0\"  cellpadding=\"0\" cellspacing=\"0\" style='margin-bottom:10px;' class=\"table-noneforOrder\">");
                    Table.Append("<tr>");
                    Table.Append("<th width='15%' align='left' valign='top'>Thumbnail</th>");
                    Table.Append("<th width='32%' align='left' valign='top'>Product</th>");
                    Table.Append("<th width='13%' align='left' valign='top'>SKU</th>");
                    Table.Append("<th width='10%' align='right' valign='top'>Price</th>");
                    Table.Append("<th width='8%' align='center' valign='top'>Quantity</th>");
                    Table.Append("<th width='8%' align='center' valign='top'>Sub Total</th>");
                    Table.Append("</tr>");
                    NetPrice = 0;
                    String Name = String.Empty;
                    for (Int32 CartItemNo = 0; CartItemNo < ds.Tables[0].Rows.Count; CartItemNo++)
                    {
                        SubTotal = Convert.ToDecimal(ds.Tables[0].Rows[CartItemNo]["Price"].ToString()) * Convert.ToDecimal(ds.Tables[0].Rows[CartItemNo]["Quantity"].ToString());
                        NetPrice += SubTotal;
                        NumberOfItems += Convert.ToInt32(ds.Tables[0].Rows[CartItemNo]["Quantity"].ToString());
                        Table.Append("<tr>");
                        Table.Append("<td align='center' valign='middle'><img border='0px' Title=\"" + ds.Tables[0].Rows[CartItemNo]["Name"].ToString() + "\" src=\"" + GetMicroImage(Convert.ToString(ds.Tables[0].Rows[CartItemNo]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(ds.Tables[0].Rows[CartItemNo]["Name"].ToString()) + "\">");
                        Table.Append("</td>");
                        //if (ds.Tables[0].Rows[CartItemNo]["VariantNames"].ToString() != "" && ds.Tables[0].Rows[CartItemNo]["VariantValues"].ToString() != "")
                        //{
                        //    BindVariantForProduct(ds.Tables[0].Rows[CartItemNo]["VariantNames"].ToString(), ds.Tables[0].Rows[CartItemNo]["VariantValues"].ToString());
                        //}
                        //else
                        //{
                        //    ltrvartable = new System.Web.UI.WebControls.Literal();
                        //    ltrvartable.Text = string.Empty;
                        //}
                        // Table.Append("<td align='left' valign='top'><span style='font-size:11px;color:#757575;'><b>" + ds.Tables[0].Rows[CartItemNo]["Name"].ToString() + "</b></span><br />" + ltrvartable.Text + "");

                        string[] variantValue = ds.Tables[0].Rows[CartItemNo]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] variantName = ds.Tables[0].Rows[CartItemNo]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        Table.Append("<td align='left' valign='top'><span style='font-size:11px;color:#757575;'><b>" + ds.Tables[0].Rows[CartItemNo]["Name"].ToString() + "</b></span><br />");
                        if (variantValue.Length == variantName.Length && variantName.Length > 0)
                        {
                            for (int v = 0; v < variantName.Length; v++)
                            {
                                Table.Append("<span style='font-size:11px;color:#757575;'>" + variantName[v].ToString() + ": " + variantValue[v].ToString() + "</span><br />");
                            }
                        }

                        Table.Append("</td>");
                        Table.Append("<td align='left' valign='top'>" + ds.Tables[0].Rows[CartItemNo]["SKU"].ToString());
                        Table.Append("</td>");
                        Table.Append("<td align='right' valign='top'>$" + Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[CartItemNo]["Price"].ToString()), 2));

                        Table.Append("</td>");
                        Table.Append("<td align='center' valign='top' >" + ds.Tables[0].Rows[CartItemNo]["Quantity"].ToString() + "</td>");
                        Table.Append("<td align='right' valign='top' >$" + Math.Round(Convert.ToDecimal(SubTotal.ToString()), 2));
                        Table.Append("</td>");
                        Table.Append("</tr>");
                    }
                    Table.Append("<tr>");
                    Table.Append("<td colspan='5' align='right' valign='top'><strong>Sub Total:</strong>&nbsp;&nbsp;</td>");

                    //  Table.Append("<td align='center' valign='top'>" + NumberOfItems.ToString() + "</td>");
                    Table.Append("<td align='right' valign='top'>$" + Math.Round(Convert.ToDecimal(NetPrice.ToString()), 2));
                    Table.Append("</td>");
                    Table.Append("</tr>");
                    Table.Append("</table>");
                }
                else
                {
                    Session["cart"] = "Empty";
                    Table.AppendLine("<font class='error'>Your Shopping Cart is Empty.</font>");
                }
                ltCart.Text = Table.ToString();
            }
            catch { }
        }

        #region GetMicroImages

        /// <summary>
        /// Get Micro image for display
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Micro image path</returns>
        public String GetMicroImage(String img)
        {
            Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
            String[] AllowedExtensions = AppLogic.AppConfigs("AllowedExtensions").Split(',');
            String imagepath = String.Empty;
            Random rd = new Random();
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Micro/" + img;
            if (img != "")
            {
                if (File.Exists(Server.MapPath(imagepath)))
                {
                    return imagepath + "?" + rd.Next(1000).ToString();
                }
            }
            else
            {
                return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
            }

            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
        }

        #endregion

        #region Bind Variant for product

        /// <summary>
        /// Bind variant for product
        /// </summary>
        /// <param name="VarName">string VarName</param>
        /// <param name="VarValue">string VarValue</param>
        /// <returns>Returns the Literal control for display</returns>
        public System.Web.UI.WebControls.Literal BindVariantForProduct(String VarName, String VarValue)
        {
            string[] varname = VarName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] varvalue = VarValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sbvartable = new StringBuilder();
            ltrvartable = new System.Web.UI.WebControls.Literal();
            if (varname.Length > 0)
            {

                for (int i = 0; i < varname.Length; i++)
                {
                    sbvartable.AppendLine("" + varname[i].ToString() + " : " + varvalue[i].ToString() + "<br />");
                }
            }
            if (sbvartable.ToString() != "")
            {
                ltrvartable.Text = sbvartable.ToString();
            }
            else
            {
                ltrvartable.Text = "";
            }
            return ltrvartable;
        }

        #endregion

        /// <summary>
        /// Print Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnPrint_Click(object sender, ImageClickEventArgs e)
        {

        }

    }
}