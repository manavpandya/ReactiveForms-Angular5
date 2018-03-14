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
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class WishlistSendMail : BasePage
    {
        #region local variables

        System.Web.UI.WebControls.Literal ltrvartable = null;

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
                int CustomerID = Convert.ToInt32(Request.QueryString["CNo"]);
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

                imgLogo.Src = AppLogic.AppConfigs("LIVE_SERVER").TrimEnd("/".ToCharArray()) + "/images/logo.png";
                imgMainDiv.Src = "~/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
                btnSendMail.ImageUrl = "/App_Themes/" + Page.Theme + "/button/send-email.png";
                if (Request.QueryString["CNo"] != null)
                {
                    BindCart(Convert.ToInt32(Request.QueryString["CNo"]));

                }
            }
        }

        /// <summary>
        /// Binds the cart.
        /// </summary>
        /// <param name="CartID">int CartID</param>
        private void BindCart(Int32 CartID)
        {
            string CustomerID = Convert.ToString(Request.QueryString["CNo"]);
            try
            {
                Decimal NetPrice = Decimal.Zero;
                Decimal SubTotal = Decimal.Zero;
                ltCart.Text = "";
                string Customerid = Convert.ToString(Request.QueryString["CNo"]);
                string strSql = "SELECT dbo.tb_WishListItems.WishListID, dbo.tb_Product.StoreID,isnull(dbo.tb_WishListItems.VariantNames,'') as VariantNames,isnull(dbo.tb_WishListItems.VariantValues,'') as VariantValues,  dbo.tb_WishListItems.ProductID, dbo.tb_WishListItems.CustomerID, dbo.tb_Product.Name, dbo.tb_Product.SKU, " +
                                 " dbo.tb_Product.SEName, case when isnull(dbo.tb_Product.SalePrice,0) = 0 then  isnull(dbo.tb_Product.Price,0) else isnull(dbo.tb_Product.SalePrice,0) end as Price, " +
                                 "  dbo.tb_WishListItems.Quantity, dbo.tb_Product.Deleted, dbo.tb_Product.Active, " +
                                " dbo.tb_Product.ImageName, dbo.tb_Product.MainCategory FROM         dbo.tb_WishListItems INNER JOIN  " +
                                 " dbo.tb_Product ON dbo.tb_WishListItems.ProductID = dbo.tb_Product.ProductID where tb_WishListItems.CustomerID=" + Customerid + "";
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

                    FillCKEditor(ds.Tables[0].Rows[0]["WishListID"].ToString(), CustomerID, Convert.ToInt32(ds.Tables[0].Rows[0]["StoreID"].ToString()));
                }
                else
                {
                    Session["cart"] = "Empty";
                    Table.AppendLine("<font class='error'>Your Shopping Cart is Empty.</font>");
                    trsendmail.Visible = false;
                }
                ltCart.Text = Table.ToString();
            }
            catch { }
        }



        /// <summary>
        /// Gets the micro image.
        /// </summary>
        /// <param name="img">string Img</param>
        /// <returns>Returns the Image Path for display</returns>
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



        #region Bind Variant for product


        /// <summary>
        /// Binds the variant for product.
        /// </summary>
        /// <param name="VarName">string VarName</param>
        /// <param name="VarValue">string VarValue</param>
        /// <returns>Returns the Literal Controls.</returns>
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
        ///  Send Mail Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSendMail_Click(object sender, ImageClickEventArgs e)
        {
            string CustomerID = Convert.ToString(Request.QueryString["CNo"]);
            string strSql = "select tb_WishListItems.WishListID,tb_WishListItems.CustomerID,tb_Customer.Email,tb_Customer.StoreID from tb_WishListItems inner join tb_Customer on tb_WishListItems.CustomerID = tb_Customer.CustomerID where tb_WishListItems.CustomerID =" + CustomerID + "";
            DataSet ds = new DataSet();

            ds = CommonComponent.GetCommonDataSet(strSql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                SendMail(ds.Tables[0].Rows[0]["Email"].ToString(), ds.Tables[0].Rows[0]["WishListID"].ToString(), CustomerID, Convert.ToInt32(ds.Tables[0].Rows[0]["StoreID"].ToString()));
            }

        }
        /// <summary>
        /// fill data in ckeditor
        /// </summary>
        public void FillCKEditor(string WishlistID, string CustomerID, int StoreID)
        {
            CustomerComponent objCustomer = null;
            objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();
            if (txtCouponCode.Text.Trim() != "")
            {
                String SPMessage = Convert.ToString(CouponComponent.GetDiscountByCouponCodeFunction(txtCouponCode.Text.Trim(), Convert.ToInt32(CustomerID), Convert.ToInt32(StoreID)));
                try
                {
                    decimal CouponDiscount = Convert.ToDecimal(SPMessage.ToString());

                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", @"alert('" + SPMessage.ToString() + "');", true);
                    txtCouponCode.Text = "";
                    txtCouponCode.Focus();
                    return;
                }

            }
            dsMailTemplate = objCustomer.GetEmailTamplate("WishlistShoppingCartEmail", StoreID);
            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {
                string strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                StringBuilder sw = new StringBuilder(2000);

                string strSql =

                "SELECT (dbo.tb_Customer.FirstName+ ' '+dbo.tb_Customer.LastName) as CustomerName ,dbo.tb_WishListItems.WishListID, dbo.tb_Product.StoreID, isnull(dbo.tb_WishListItems.VariantNames,'') as VariantNames,isnull(dbo.tb_WishListItems.VariantValues,'') as"
+ " VariantValues,  dbo.tb_WishListItems.ProductID, dbo.tb_WishListItems.CustomerID, dbo.tb_Product.Name, dbo.tb_Product.SKU,"
        + "   dbo.tb_Product.SEName, case when isnull(dbo.tb_Product.SalePrice,0) = 0 then  isnull(dbo.tb_Product.Price,0)"
         + "   else isnull(dbo.tb_Product.SalePrice,0) end as Price,"
    + "   dbo.tb_WishListItems.Quantity, dbo.tb_Product.Deleted, dbo.tb_Product.Active, "
 + "   dbo.tb_Product.ImageName, dbo.tb_Product.MainCategory FROM dbo.tb_WishListItems"
    + " inner join dbo.tb_Customer on dbo.tb_WishListItems.CustomerID=dbo.tb_Customer.CustomerID"
    + " INNER JOIN "
 + "dbo.tb_Product ON dbo.tb_WishListItems.ProductID = dbo.tb_Product.ProductID where tb_WishListItems.CustomerID=" + CustomerID + "";

                DataSet DsCItems = new DataSet();
                DsCItems = CommonComponent.GetCommonDataSet(strSql);
                sw.Append("<table border='0' cellpadding='0' cellspacing='0' class='datatable' width='100%'> ");
                sw.Append("<tbody>");
                if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
                {
                    sw.Append("<tr>");
                    sw.Append("<th align='left' valign='middle' style='width:60%' ><b>Product</b></th>");
                    sw.Append("<th align='left' valign='middle' style='width:20%' ><b> SKU</b></th>");
                    sw.Append("<th align='center' valign='middle' style='width:10%'><b>Price</b></th>");
                    sw.Append("<th valign='middle' style='width: 20%;text-align:center;'><b>Quantity</b></th>");
                    sw.Append("</tr>");


                    for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                    {

                        sw.Append("<tr align='center'  valign='middle'>");

                        //if (DsCItems.Tables[0].Rows[i]["VariantNames"].ToString() != "" && DsCItems.Tables[0].Rows[i]["VariantValues"].ToString() != "")
                        //{
                        //    BindVariantForProduct(DsCItems.Tables[0].Rows[i]["VariantNames"].ToString(), DsCItems.Tables[0].Rows[i]["VariantValues"].ToString());
                        //}
                        //else
                        //{
                        //    ltrvartable = new System.Web.UI.WebControls.Literal();
                        //    ltrvartable.Text = string.Empty;
                        //}
                        //sw.Append("<td align='left' valign='top'>" + DsCItems.Tables[0].Rows[i]["Name"].ToString() + "<br />" + ltrvartable.Text + "");
                        //sw.Append("<td align='left' valign='top'>" + DsCItems.Tables[0].Rows[i]["Name"].ToString() + "");
                        //sw.Append("</td>");

                        string[] variantValue = DsCItems.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] variantName = DsCItems.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        sw.Append("<td align='left' valign='top'><span style='font-size:11px;color:#757575;'><b>" + DsCItems.Tables[0].Rows[i]["Name"].ToString() + "</b></span><br />");
                        if (variantValue.Length == variantName.Length && variantName.Length > 0)
                        {
                            for (int v = 0; v < variantName.Length; v++)
                            {
                                sw.Append("<span style='font-size:11px;color:#757575;'>" + variantName[v].ToString() + ": " + variantValue[v].ToString() + "</span><br />");
                            }
                        }

                        sw.Append("</td>");


                        sw.Append("<td  align='left' >" + DsCItems.Tables[0].Rows[i]["SKU"].ToString() + "</td>");
                        sw.Append("<td  align='left' >" + Math.Round(Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["price"].ToString()), 2) + "</td>");
                        sw.Append("<td STYLE='text-align : center;'>" + DsCItems.Tables[0].Rows[i]["Quantity"].ToString() + "</td>");
                        sw.Append("</tr>");

                    }



                    CKEditor1.Text = sw.ToString();

                }
                else
                {
                    sw.Append("<tr>");
                    sw.Append("<td >");
                    sw.AppendLine("<font color='red' CLASS='font-red'>Your Shopping Cart is Empty.</font>");
                    sw.Append("</td>");
                    sw.Append("</tr>");


                }
                if (txtCouponCode.Text != "")
                {
                    sw.Append("<tr align='left'><td  style='border:none;height:30px;' colspan='4'>Coupon Code : " + txtCouponCode.Text + "</td></tr>");
                    sw.Append("<tr align='left'><td  style='border:none;' colspan='4'>Use Coupon Code <font style='color:red;font-weight:bold;'>'" + txtCouponCode.Text + "'</font> and get discount during checkout.<br/></td></tr>");
                }
                sw.Append("</tbody>");
                sw.Append("</table>");

                sw.Append("</td>");
                sw.Append("</tr>");
                strBody = strBody.Replace("###CartDetail###", sw.ToString());
                strBody = Regex.Replace(strBody, "###CUSTOMERNAME###", DsCItems.Tables[0].Rows[0]["CustomerName"].ToString(), RegexOptions.IgnoreCase);
                strBody = strBody.Replace("###STORENAME###", "half price drapes");
                //strBody = strBody.Replace("###COUPONCODE###", txtCouponCode.Text.ToString());
                strBody = strBody.Replace("###LIVE_SERVER###", "http://www.halfpricedrapes.com");
                strBody = strBody.Replace("###YEAR###", DateTime.Now.Year.ToString());
                strBody = strBody.Replace("###STOREPATH###", "<a href='http://www.halfpricedrapes.com/'>http://www.halfpricedrapes.com/</a>");
                CKEditor1.Text = strBody.ToString();
            }


        }
        #region Send Mail to Customer
        /// <summary>
        /// This Function will send an E-Mail to Customer
        /// for Providing Registration Information of Customer
        /// </summary>
        /// <param name="ToAddress">string ToAddress</param>
        /// <param name="WishlistID">string WishlistID</param>
        /// <param name="CustomerID">string CustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        private void SendMail(string ToAddress, string WishlistID, string CustomerID, int StoreID)
        {
            try
            {
                CustomerComponent objCustomer = null;
                objCustomer = new CustomerComponent();
                DataSet dsMailTemplate = new DataSet();

                dsMailTemplate = objCustomer.GetEmailTamplate("WishlistShoppingCartEmail", StoreID);
                if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                {

                    String strBody = "";
                    String strSubject = "";
                    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();
                    strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                    strBody = CKEditor1.Text;
                    if (txtCouponCode.Text.ToString() != "")
                    {
                        strBody = strBody.Replace("###COUPONCODE###", "Use Copuon Code: " + txtCouponCode.Text.ToString());
                    }
                    else
                    {
                        strBody = strBody.Replace("###COUPONCODE###", "");
                    }



                    CommonOperations.SendMail(ToAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, null);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Mail Send Successfully...');window.close();", true);
                }
            }
            catch { }

        }
        #endregion
    }
}