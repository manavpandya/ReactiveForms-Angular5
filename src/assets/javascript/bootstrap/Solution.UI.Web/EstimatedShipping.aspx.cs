using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.Web.UI.HtmlControls;
using StringBuilder = System.Text.StringBuilder;
using System.Text.RegularExpressions;
using System.IO;

namespace Solution.UI.Web
{
    public partial class EstimatedShipping : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            BindProducts();
        }

        /// <summary>
        /// Best Seller Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void rptBestSeller_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl ProboxBestSeller = (HtmlGenericControl)e.Item.FindControl("ProboxBestSeller");
                HtmlGenericControl proDisplay = (HtmlGenericControl)e.Item.FindControl("proDisplay");
                Label lblTagName = (Label)e.Item.FindControl("lblTagImageName");
                Literal lblTagImage = (Literal)e.Item.FindControl("lblTagImage");
                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");

                if ((e.Item.ItemIndex + 1) % 4 == 0 && e.Item.ItemIndex != 0)
                {
                    ProboxBestSeller.Attributes.Add("style", "padding-right: 0px;");
                }
                if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                {
                    string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                    if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                    {
                        if (lblTagName != null && !string.IsNullOrEmpty(lblTagName.Text.ToString().Trim()))
                        {
                            lblTagImage.Text = "<img title='" + lblTagName.Text.ToString().Trim() + "' src=\"/images/" + lblTagName.Text.ToString().Trim() + ".jpg\" alt=\"" + lblTagName.Text.ToString().Trim() + "\" class='" + lblTagName.Text.ToString().ToLower() + "'  />";
                        }
                    }
                }

                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                Label lblSalePrice = (Label)e.Item.FindControl("lblSalePrice");
                Literal ltrYourPrice = (Literal)e.Item.FindControl("ltrYourPrice");
                Literal ltrRegularPrice = (Literal)e.Item.FindControl("ltrRegularPrice");
                HtmlInputHidden ltrLink = (HtmlInputHidden)e.Item.FindControl("ltrLink");
                HtmlInputHidden ltrlink1 = (HtmlInputHidden)e.Item.FindControl("ltrlink1");

                Literal ltrInventory = (Literal)e.Item.FindControl("ltrInventory");
                Literal ltrproductid = (Literal)e.Item.FindControl("ltrproductid");
                HtmlAnchor abestLink = (HtmlAnchor)e.Item.FindControl("abestLink");
                Decimal hdnprice = 0;
                Decimal SalePrice = 0;
                Decimal Price = 0;

                if (lblPrice != null)
                    Price = Convert.ToDecimal(lblPrice.Text);
                if (lblSalePrice != null)
                    SalePrice = Convert.ToDecimal(lblSalePrice.Text);

                if (Price > decimal.Zero)
                {
                    ltrRegularPrice.Text += "<p>" + Price.ToString("C") + "</p>";//"<p>Regular Price: " + Price.ToString("C") + "</p>";
                    hdnprice = Price;
                }
                else
                {
                    ltrRegularPrice.Text += "<p>&nbsp;</p>";
                }
                if (SalePrice > decimal.Zero && Price > SalePrice)
                {
                    ltrYourPrice.Text += "<p>" + SalePrice.ToString("C") + "</p>";//"<p>Your Price: <strong>" + SalePrice.ToString("C") + "</strong></p>";
                    hdnprice = SalePrice;
                }
                else
                {
                    ltrYourPrice.Text += "<p>" + Price.ToString("C") + "</p>";
                }
                //if (Convert.ToInt32(ltrInventory.Text.ToString()) > 0)
                //{

                //    Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count(dbo.tb_ProductVariant.VariantID) FROM  dbo.tb_ProductVariant INNER JOIN dbo.tb_ProductVariantValue ON dbo.tb_ProductVariant.VariantID = dbo.tb_ProductVariantValue.VariantID WHERE dbo.tb_ProductVariant.Productid=" + ltrproductid.Text.ToString() + " "));
                //    if (Count == 0)
                //    {
                //        abestLink.Attributes.Add("onclick", "adtocart('" + hdnprice.ToString() + "'," + ltrproductid.Text.ToString() + ");");
                //    }
                //    else
                //    {
                //        abestLink.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ");");
                //    }
                //}
                //else
                //{
                //    abestLink.Attributes.Remove("onclick");
                //    abestLink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                //}


            }
        }

        /// <summary>
        /// Binds the Products for Best Seller
        /// </summary>
        private void BindProducts()
        {
            ///Bind Best Seller
            DataSet dsBestSeller = ProductComponent.DisplyProductByOption("IsBestSeller", Convert.ToInt32(AppLogic.AppConfigs("StoreID")), 4);
            if (dsBestSeller != null && dsBestSeller.Tables.Count > 0 && dsBestSeller.Tables[0].Rows.Count > 0)
            {
                rptBestSeller.DataSource = dsBestSeller;
                rptBestSeller.DataBind();
            }
        }

        /// <summary>
        /// Gets the Icon Image Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Icon Image Path</returns>
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

        /// <summary>
        /// Sets the Attribute  By Replacing '"' and '\' to '-'.
        /// </summary>
        /// <param name="Name">string Name</param>
        /// <returns>Returns Attribute</returns>
        public String SetAttribute(String Name)
        {
            return Name.Replace('"', '-').Replace('\'', '-').ToString();
        }

        /// <summary>
        /// Add '...', if String length is more than 50 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 50 Length String </returns>
        public String SetName(String Name)
        {
            if (Name.Length > 50)
                Name = Name.Substring(0, 47) + "...";
            return Server.HtmlEncode(Name);
        }
    }
}