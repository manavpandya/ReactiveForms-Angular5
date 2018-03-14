using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net.Mail;
using System.Data;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Components;
using System.Text.RegularExpressions;
using System.IO;

namespace Solution.UI.Web
{
    public partial class ProductCompare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CmpProductID"] != null && Session["CmpProductID"].ToString() != "")
                    ShowProductDescription(Session["CmpProductID"].ToString());
            }
        }

        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }

        public String SetDescription(String Description)
        {
            if (Description.Length > 500)
                Description = Description.Substring(0, 495) + "...";
            return Server.HtmlDecode(Description);
        }

        private void ShowProductDescription(string ProIDs)
        {
            string proid = ProIDs.TrimEnd(',');
            string[] arr = proid.Split(',');
            DataSet dsproduct = new DataSet();
            if (ProIDs != "")
            {
                //ltrPageCompro.Text += "<div class='' style='float:right;margin:5px;background-color:transparent;background-image: url(/images/popupclose.png);'>";
               // ltrPageCompro.Text += "<img src='/images/close.png' style='cursor: pointer;' onclick='javascript:window.parent.disablePopup();'></div>";
                ltrPageCompro.Text += "<table width='100%' border='0' cellpadding='0' cellspacing='0' style='border-left: 1px solid #D5D5D5; border-top: 1px solid #D5D5D5;'>";

                string Name = "<tr><td valign='top' style='width: 100px; border-right: 1px solid #D5D5D5; background: #E8E8E8;font-size: 14px; font-weight: bold; color: #B92127; border-bottom: 1px solid #D5D5D5; padding: 5px;'>Name</td>";
                string SKU = "<tr><td valign='top' style='width: 100px; border-right: 1px solid #D5D5D5; background: #E8E8E8;font-size: 14px; font-weight: bold; color: #B92127; border-bottom: 1px solid #D5D5D5; padding: 5px;'>Item Code</td>";
                string Image = "<tr><td valign='top' style='width: 100px; border-right: 1px solid #D5D5D5; background: #E8E8E8;font-size: 14px; font-weight: bold; color: #B92127; border-bottom: 1px solid #D5D5D5; padding: 5px;'>Image</td>";
                string Description = "<tr><td valign='top' style='width: 100px; border-right: 1px solid #D5D5D5; background: #E8E8E8;font-size: 14px; font-weight: bold; color: #B92127; border-bottom: 1px solid #D5D5D5; padding: 5px;'>Description</td>";
                string NewPrice = "<tr><td valign='top' style='width: 100px; border-right: 1px solid #D5D5D5; background: #E8E8E8;font-size: 14px; font-weight: bold; color: #B92127; border-bottom: 1px solid #D5D5D5; padding: 5px;'>Price</td>";
                string NewSalePrice = "<tr><td valign='top' style='width: 100px; border-right: 1px solid #D5D5D5; background: #E8E8E8;font-size: 14px; font-weight: bold; color: #B92127; border-bottom: 1px solid #D5D5D5; padding: 5px;'>Starting Price</td>";
                string NewYouSave = "<tr><td valign='top' style='width: 100px; border-right: 1px solid #D5D5D5; background: #E8E8E8;font-size: 14px; font-weight: bold; color: #B92127; border-bottom: 1px solid #D5D5D5; padding: 5px; display:none;'>You Save</td>";
                string Availlability = "<tr><td valign='top' style='width: 100px; border-right: 1px solid #D5D5D5; background: #E8E8E8;font-size: 14px; font-weight: bold; color: #B92127; border-bottom: 1px solid #D5D5D5; padding: 5px;'>Availability</td>";
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(arr[i].ToString()))
                    {
                        dsproduct = ProductComponent.GetProductDetailByID(Convert.ToInt32(arr[i].ToString()));
                        if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                        {
                            if (i == arr.Length)
                            {
                                Name += "</tr>";
                                SKU += "</tr>";
                                Image += "</tr>";
                                Description += "</tr>";
                                NewPrice += "</tr>";
                                NewSalePrice += "</tr>";
                                NewYouSave += "</tr>";
                                Availlability += "</tr>";
                            }
                            else
                            {
                                Name += "<td style='width: 200px; border-right: 1px solid #D5D5D5; background: #E8E8E8;font-size: 14px; color: #B92127; border-bottom: 1px solid #D5D5D5; padding: 5px;'>" + (dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "</td> ";
                                SKU += "<td style='width: 200px; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #000; padding: 5px;'>" + dsproduct.Tables[0].Rows[0]["SKU"].ToString() + "</td>";
                                Image += "<td style='width: 200px;text-align:center; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #000; padding: 5px;'><img    title='" + (dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "' alt='" + (dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "' style='width:90%;padding: 10px 10px 10px 10px;' src='" + GetIconImageProduct(dsproduct.Tables[0].Rows[0]["ImageName"].ToString()) + "'></td> ";
                                Description += "<td style='width: 200px; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #000; padding: 5px;vertical-align: top;'>" + (dsproduct.Tables[0].Rows[0]["Description"].ToString()) + "</td>";
                                Decimal SalePrice = 0;
                                Decimal Price = 0;
                                string lblPrice = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("f2");
                                string lblSalePrice = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("f2");

                                if (lblPrice != null)
                                    Price = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString());
                                if (lblSalePrice != null)
                                    SalePrice = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString());

                                if (Price > decimal.Zero)
                                {

                                    if (SalePrice > decimal.Zero && Price > SalePrice)
                                    {
                                        NewPrice += "<td style='width: 200px; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #000; padding: 5px;'> <del>" + Price.ToString("C") + "<del></td> ";
                                    }
                                    else
                                    {
                                        NewPrice += "<td style='width: 200px; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #000; padding: 5px;'>" + Price.ToString("C") + "</td> ";
                                    }
                                }
                                else
                                {
                                    NewPrice += "<td style='width: 200px; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #000; padding: 5px;'>" + SalePrice.ToString("C") + "</td> ";
                                }
                                if (SalePrice > decimal.Zero && Price > SalePrice)
                                {
                                    NewSalePrice += "<td style='width: 200px; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #B92127; padding: 5px;'>" + SalePrice.ToString("C") + "</td> ";
                                }
                                else
                                {
                                    NewSalePrice += "<td style='width: 200px; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #B92127; padding: 5px;'> &nbsp;- &nbsp; </td> ";
                                }
                                decimal youSave = Price - SalePrice;
                                decimal Yousavepercentage = (Convert.ToDecimal(100) * youSave) / Price;
                                NewYouSave += "<td style='width: 200px; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #B92127; padding: 5px; display:none;'>" + youSave.ToString("C") + "(" + Math.Round(Yousavepercentage, 2) + "%)" + "</td> ";
                                if (dsproduct.Tables[0].Rows[0]["Inventory"].ToString() != null && dsproduct.Tables[0].Rows[0]["Inventory"].ToString() != "")
                                {
                                    Availlability += "<td style='width: 200px; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #000; padding: 5px;'> In Stock </td> ";
                                }
                                else
                                {
                                    Availlability += "<td style='width: 200px; border-right: 1px solid #D5D5D5; border-bottom: 1px solid #D5D5D5;font-size: 12px; color: #000; padding: 5px;'> Out Of Stock </td> ";
                                }
                            }
                            //ltrPageCompro.Text += "<div class='pro-compare-box'>";
                            //ltrPageCompro.Text += "<h2>" + SetName(dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "</h2>";
                            //ltrPageCompro.Text += "<div class='img-center' style=' width:190px; height:193px;'>";
                            //ltrPageCompro.Text += "<img width='172' height='173' title='" + SetName(dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "' alt='" + SetName(dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "' style='padding: 10px 10px 10px 10px;' src='" + GetIconImageProduct(dsproduct.Tables[0].Rows[0]["ImageName"].ToString()) + "'></div>";
                            //ltrPageCompro.Text += "<div class='pro-compar-pt2'>";
                            //Decimal SalePrice = 0;
                            //Decimal Price = 0;
                            //string lblPrice = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("f2");
                            //string lblSalePrice = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("f2");

                            //if (lblPrice != null)
                            //    Price = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString());
                            //if (lblSalePrice != null)
                            //    SalePrice = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString());

                            //if (Price > decimal.Zero)
                            //{
                            //    //ltrRegularPrice.Text += "<p><del>" + Price.ToString("C") + "</del></p>";
                            //    //hdnprice = Price;
                            //    if (SalePrice > decimal.Zero && Price > SalePrice)
                            //    {
                            //        ltrPageCompro.Text += "<p><h2 style='width:180px;height:25px;'>Price :<del>" + Price.ToString("C") + "<del></h2></p>";
                            //    }
                            //    else
                            //    {
                            //        ltrPageCompro.Text += "<p><h2 style='width:180px;height:25px;'>Price :" + Price.ToString("C") + "</h2></p>";
                            //    }
                            //}
                            //else
                            //{
                            //    ltrPageCompro.Text += "<p><h2 style='width:180px;height:25px;'>Sale Price :" + SalePrice.ToString("C") + "</h2></p>";
                            //}
                            //if (SalePrice > decimal.Zero && Price > SalePrice)
                            //{
                            //    ltrPageCompro.Text += "<p><h2 style='width:180px;height:25px;'>Sale Price :" + SalePrice.ToString("C") + "</h2></p>";
                            //}
                            //else
                            //{
                            //    ltrPageCompro.Text += "&nbsp;";
                            //}


                            //if (dsproduct.Tables[0].Rows[0]["Inventory"].ToString() != null && dsproduct.Tables[0].Rows[0]["Inventory"].ToString() != "")
                            //{
                            //    ltrPageCompro.Text += "<p><h2 style='width:180px;height:25px;'>Availability : In Stock</h2></p>";
                            //}
                            //else
                            //{
                            //    ltrPageCompro.Text += "<p><h2 style='width:180px;height:25px;'>Availability : Out Of Stock</h2></p>";
                            //}
                            //ltrPageCompro.Text += "<p>" + SetDescription(dsproduct.Tables[0].Rows[0]["Description"].ToString()) + "</p>";
                            //ltrPageCompro.Text += "</div>";
                            //ltrPageCompro.Text += "</div>";

                        }
                    }
                }
                ltrPageCompro.Text += Name + SKU + Image + Description + NewPrice + NewSalePrice + NewYouSave + Availlability;
                ltrPageCompro.Text += "</table>";
                ltrPageCompro.Text += "</div>";

            }

        }
    }
}