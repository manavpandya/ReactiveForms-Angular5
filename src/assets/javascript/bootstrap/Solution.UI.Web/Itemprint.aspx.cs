using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
using System.IO;
using Solution.Bussines.Components.Common;

namespace Solution.UI.Web
{
    public partial class Itemprint : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["PID"] != null)
                {
                    BindData(Convert.ToInt32(Request.QueryString["PID"].ToString()));
                }
            }

        }

        /// <summary>
        /// Bind Invoice Data to Print
        /// </summary>
        /// <param name="productID">int productID</param>
        private void BindData(Int32 productID)
        {

            DataSet DsProduct = new DataSet();
            DsProduct = ProductComponent.GetProductDetailByID(productID, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            String Colors = String.Empty;
            String Sizes = String.Empty;

            if (DsProduct != null && DsProduct.Tables[0].Rows.Count > 0)
            {
                string ProName = "";

                int HotDealProductID = 0;
                try
                {
                    HotDealProductID = Convert.ToInt32(AppLogic.AppConfigs("HotDealProduct"));
                }
                catch { }

                ProName = Server.HtmlEncode(DsProduct.Tables[0].Rows[0]["Name"].ToString().Trim());
                litProductNamePart.Text = ProName;
                litItemNumber.Text = DsProduct.Tables[0].Rows[0]["SKU"].ToString();
                Decimal Price = Decimal.Zero;
                Decimal salePrice = Decimal.Zero;
                if (DsProduct.Tables[0].Rows[0]["SalePrice"] != null && DsProduct.Tables[0].Rows[0]["SalePrice"].ToString() != "")
                {
                    salePrice = Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["SalePrice"].ToString()), 2);
                }
                if (DsProduct.Tables[0].Rows[0]["Price"] != null && Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["Price"].ToString()).ToString("C") != "")
                {
                    Price = Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["Price"].ToString()), 2);
                }

                ltRegularPrice.Text = Convert.ToDecimal(Price.ToString()).ToString("C");
                litSalePrice.Text = Convert.ToDecimal(salePrice.ToString()).ToString("C");


                //if (Price == 0)
                //{
                //    spanRetail.Attributes.Clear();
                //    spanRetail.Attributes.Add("style", "color:#B50002;");
                //    ltRetailName.Text = "Regular Price : ";
                //    ltRegularPrice.Text = salePrice.ToString("C");
                //}
                //else
                //{
                //    ltRetailName.Text = "Regular Price : ";
                //}
                //if (Price > salePrice)
                //{
                //    if (productID == HotDealProductID)
                //    {
                //        litTodayOnly.Text = Convert.ToDecimal(AppLogic.AppConfigs("HotDealPrice")).ToString("C2");
                //        salePrice = Math.Round(Convert.ToDecimal(AppLogic.AppConfigs("HotDealPrice")), 2);
                //        trTodayOnly.Visible = true;
                //    }
                //    else
                //    {
                //        trTodayOnly.Visible = false;
                //    }


                //    trYourPrice.Visible = true;
                //    trYouSave.Visible = true;
                //    decimal youSave = Price - salePrice;
                //    decimal Yousavepercentage = (Convert.ToDecimal(100) * youSave) / Price;
                //    litYourSave.Text = "<div id=\"spnYousave\" style=\"float:left;\"><span>" + youSave.ToString("C") + "</span> (" + Math.Round(Yousavepercentage, 2) + "%)</div>";
                //}
                //else
                //{
                //    spanRetail.Attributes.Add("style", "text-decoration:none; color:#363636;");
                //    trYourPrice.Visible = false;
                //    trYouSave.Visible = false;
                //    ltRetailName.Text = "Regular Price : ";
                //}

                if (Price == 0)
                {
                    //spanRetail.Attributes.Clear();
                    spanRetail.Attributes.Add("style", "color: #B92127;");
                    ltRetailName.Text = "Your Price : ";
                    ltRegularPrice.Text = salePrice.ToString("C");
                }
                else
                {
                    ltRetailName.Text = "Regular Starting Price : ";
                }
                if (Price > salePrice && salePrice > 0)
                {
                    trYourPrice.Visible = true;
                    trYouSave.Visible = true;
                    decimal youSave = Price - salePrice;
                    decimal Yousavepercentage = (Convert.ToDecimal(100) * youSave) / Price;
                    litYourSave.Text = "<div id=\"spnYousave\" style=\"float:left;\"><span>" + youSave.ToString("C") + "</span> (" + Math.Round(Yousavepercentage, 2) + "%)</div>";
                }
                else
                {
                    //spanRetail.Attributes.Add("style", "text-decoration:none; color:#363636;");
                    spanRetail.Attributes.Add("style", "color: #B92127;");
                    trYourPrice.Visible = false;
                    trYouSave.Visible = false;
                    ltRetailName.Text = "Your Price : ";
                }

                if (DsProduct.Tables[0].Rows[0]["Description"] != null && DsProduct.Tables[0].Rows[0]["Description"].ToString() != "")
                {
                    DivDescription.Visible = true;
                    DivDescriptionTitle.Visible = true;
                    litDescription.Text = DsProduct.Tables[0].Rows[0]["Description"].ToString().Trim();
                }
                else
                {
                    DivDescription.Visible = false;
                    DivDescriptionTitle.Visible = false;
                }
                string ImagePath = GetMediumImage(DsProduct.Tables[0].Rows[0]["ImageName"].ToString());
                DataSet dsProductImage = new DataSet();

                dsProductImage = ProductComponent.GetproductImagename(Convert.ToInt32(Request.QueryString["PID"].ToString()));
                if (dsProductImage != null && dsProductImage.Tables.Count > 0 && dsProductImage.Tables[0].Rows.Count > 0)
                {

                    if (Convert.ToString(dsProductImage.Tables[0].Rows[0]["ImageName"]) != "")
                    {
                        if (File.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "Medium/" + Convert.ToString(dsProductImage.Tables[0].Rows[0]["ImageName"]) + "")))
                        {
                            litProductMainImage.Text = "<img style='width:300px;' src='" + AppLogic.AppConfigs("ImagePathProduct") + "Medium/" + Convert.ToString(dsProductImage.Tables[0].Rows[0]["ImageName"]) + "' />";
                        }
                        else
                        {
                            litProductMainImage.Text = "<img style='width:300px;' src='" + AppLogic.AppConfigs("ImagePathProduct") + "Medium/image_not_available.jpg' />";
                        }
                    }
                    else
                    {
                        litProductMainImage.Text = "<img style='width:300px;' src='" + AppLogic.AppConfigs("ImagePathProduct") + "Medium/image_not_available.jpg" + "' />";
                    }
                }
            }
            else
            {
                Server.Transfer("Rewriter.aspx");
            }
        }

        /// <summary>
        /// Get Medium Image
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Medium Image Path</returns>
        public String GetMediumImage(String img)
        {
            clsvariables.LoadAllPath();
            String[] AllowedExtensions = AppLogic.AppConfigs("AllowedExtensions").Split(',');
            String imagepath = String.Empty;
            Random rd = new Random();
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Medium/" + img;
            if (img != "")
            {
                if (File.Exists(Server.MapPath(imagepath)))
                {
                    return imagepath + "?" + rd.Next(1000).ToString();
                }
            }
            else
            {
                return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Medium/image_not_available.jpg");
            }

            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Medium/image_not_available.jpg");
        }

    }
}