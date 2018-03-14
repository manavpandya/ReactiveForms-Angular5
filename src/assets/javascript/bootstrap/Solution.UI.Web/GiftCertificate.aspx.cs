using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.IO;
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web
{
    public partial class GiftCertificate : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            BindGiftCertificateData();
        }

        /// <summary>
        /// Binds the Gift Certificate Data
        /// </summary>
        private void BindGiftCertificateData()
        {
            DataSet repGiftCertificate = new DataSet();
            ProductComponent objproduct = new ProductComponent();
            repGiftCertificate = objproduct.GetGiftCardByStoreID(AppLogic.AppConfigs("StoreId").ToString());
            if (repGiftCertificate != null && repGiftCertificate.Tables.Count > 0 && repGiftCertificate.Tables[0].Rows.Count > 0)
            {
                rptGiftCerti.DataSource = repGiftCertificate;
                rptGiftCerti.DataBind();
                lblMsg.Text = String.Empty;
            }
            else
            {
                lblMsg.Text = "<br/>" + "Currently,We do not have any Gift Certificates.<br/> Please <a style='color: #B01230;pointer:coursor;' href='/ContactUs.aspx'>contact us</a> or check back soon for new Gift Certificates.";
                rptGiftCerti.DataSource = null;
                rptGiftCerti.DataBind();
            }
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
        /// Gets the Icon Image Product.
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Icon Image Path</returns>
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

        /// <summary>
        /// Gift Card Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void rptGiftCerti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Probox = (HtmlGenericControl)e.Item.FindControl("Probox");
                HtmlGenericControl proDisplay = (HtmlGenericControl)e.Item.FindControl("proDisplay");
                Decimal SalePrice = 0, Price = 0;

                Label lblSalePrice = (Label)e.Item.FindControl("lblSalePrice");
                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                Literal ltrYourPrice = (Literal)e.Item.FindControl("ltrYourPrice");

                if ((e.Item.ItemIndex + 1) % 4 == 0 && e.Item.ItemIndex != 0)
                {
                    Probox.Attributes.Add("style", "margin-right:0px;");
                    //proDisplay.Attributes.Add("class", "pro-display-none");
                }

                if (lblSalePrice != null)
                    SalePrice = Convert.ToDecimal(lblSalePrice.Text);
                if (lblPrice != null)
                    Price = Convert.ToDecimal(lblPrice.Text);

                if (SalePrice == Decimal.Zero)
                {
                    //ltrYourPrice.Text = "Sale Price:<strong> " + Price.ToString("C") + "</strong>";// Price.ToString("C");
                    ltrYourPrice.Text = Price.ToString("C");
                }
                else
                {
                    //ltrYourPrice.Text = "Sale Price:<strong> " + SalePrice.ToString("C") + "</strong>";
                    ltrYourPrice.Text = SalePrice.ToString("C");
                }
            }
        }
    }
}