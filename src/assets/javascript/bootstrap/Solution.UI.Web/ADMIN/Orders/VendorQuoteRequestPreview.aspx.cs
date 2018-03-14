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

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class VendorQuoteRequestPreview : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Int32 storeid = 0;
                    Int32.TryParse(AppLogic.AppConfigs("StoreID").ToString(), out storeid);
                    if (storeid == 0)
                        storeid = 1;
                    ImgStoreLogo.Src = "/Images/Store_" + storeid.ToString() + ".png";
                    imgBanner.Src = "" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/images/welcome.png";
                    ltrHeaderLink.Text = "<a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/index.aspx\" title=\"Home\">Home</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/aboutus\" title=\"About Us\">About Us</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/technicalsupport\" title=\"Technical Support\">Technical Support</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/Contactus.aspx\" title=\"Contact Us\">Contact Us</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/blog\" title=\"Blog\">Blog</a>";
                    ltrBottomHead.Text = "<a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/index.aspx\" title=\"Home\">Home</a> | <a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/aboutus\" title=\"About Us\">About Us</a> | <a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/technicalsupport\" title=\"Tech &amp; Setup\">Tech &amp; Setup</a> | <a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/returnpolicy\" title=\"Return Policy\">Return Policy</a> | <a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/shippinginfo\" title=\"Shipping Policy\">Shipping Policy</a> | <a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/warranty\" title=\"Warranty\">Warranty</a> | <a href=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/Contactus.aspx\" title=\"Contact Us\">Contact Us</a>";
                    lblStoreName.Text = AppLogic.AppConfigs("StoreName");
                    if (Request.QueryString["VendorQuoteID"] != null && Request.QueryString["VendorQuoteID"].ToString() != "")
                    {
                        ltrProductDetails.Text = BindVendorCart(Request.QueryString["VendorQuoteID"].ToString());
                        ltrFooter.Text += AppLogic.AppConfigs("StoreName") + " Team" + "<br/>";
                        ltrFooter.Text += "<a class='lnk'  href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString().ToLower() + "'>" + AppLogic.AppConfigs("LIVE_SERVER_NAME").ToString() + "</a>";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<font color=\"red\">" + ex.Message + "</font>");
            }
        }


        /// <summary>
        /// Binds the vendor cart for Vendor Quote Request.
        /// </summary>
        /// <param name="VendorQuoteID">string VendorQuoteID</param>
        /// <returns>Returns the output value as a string format which contains HTML.</returns>
        private String BindVendorCart(string VendorQuoteID)
        {
            DataSet DsPre = CommonComponent.GetCommonDataSet("Select isnull(tb_VendorQuoteRequestDetails.ProductOption,'') as ProductOption,p.SKU,tb_VendorQuoteRequestDetails.ProductID,tb_VendorQuoteRequestDetails.Quantity,ISNULL(p.Price,0) as Price," +
                                                " case when(isnull(tb_VendorQuoteRequestDetails.ProductName,'')='') then p.Name else tb_VendorQuoteRequestDetails.ProductName end  as 'Name',ISNULL(tb_VendorQuoteRequest.Notes,'') as Notes " +
                                                " from tb_VendorQuoteRequestDetails inner join tb_product p on p.Productid=tb_VendorQuoteRequestDetails.productid " +
                                                " Inner Join tb_VendorQuoteRequest on tb_VendorQuoteRequest.VendorQuoteRequestID=tb_VendorQuoteRequestDetails.VendorQuoteRequestID " +
                                                " where tb_VendorQuoteRequestDetails.VendorQuoteRequestID=" + VendorQuoteID + " and VendorID=" + Request.QueryString["VendorID"] + " and tb_VendorQuoteRequestDetails.ProductId =" + Request.QueryString["ProductId"] + "");

            DataRow[] rows = DsPre.Tables[0].Select("1=1");
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='99%' cellspacing='0' cellpadding='0' border='1' style='padding: 10px 0 0;border: 1px solid #ECECEC; border-collapse: collapse; color: #212121; font: 12px;'>");
            sb.Append("<tr style='background-color: rgb(242,242,242);'>");
            sb.Append("<th valign='middle' align='Center' style='padding-left:5px;height:28px;width:12%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>Package ID</th>");
            sb.Append("<th valign='middle' style='Padding-left:3px;width:50%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>Name</th>");
            sb.Append("<th valign='middle' align='Center' style='width:14%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='left'>SKU</th>");
            sb.Append("<th valign='middle' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;' align='center'>Quantity</th>");
            sb.Append("<th valign='middle' style='width:10%;background-color: #F6F5F5; border: 1px solid #DFDFDF;color: #212121; font-size: 12px; font-weight: bold;padding-right:5px;' align='right'>Price</th>");
            sb.Append("</tr>");

            for (int i = 0; i < rows.Length; i++)
            {
                if (!string.IsNullOrEmpty(rows[0]["Notes"].ToString()))
                    ltrNotes.Text = "<b>Notes</b> : " + rows[0]["Notes"].ToString();

                sb.Append("<tr>");
                sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>");
                sb.Append(i + 1);
                sb.Append("</td>");
                sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;'>");
                sb.Append(rows[i]["Name"].ToString());
                if (!string.IsNullOrEmpty(rows[i]["ProductOption"].ToString()))
                {
                    sb.Append("<br/>");
                    sb.Append(Server.HtmlDecode(rows[i]["ProductOption"].ToString()));
                }
                sb.Append("</td>");
                sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>");
                sb.Append(rows[i]["SKU"].ToString());
                sb.Append("</td>");
                sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:center;'>");
                sb.Append(rows[i]["Quantity"].ToString());
                sb.Append("</td>");
                sb.Append("<td style='background: #FFFFFF;border:1px solid #DADADA; color: #212121; font-size: 12px; line-height: 18px;max-height: 30px; padding: 5px 8px;text-align:right;'>");
                sb.Append(Convert.ToDecimal(rows[i]["Price"].ToString()).ToString("c"));
                sb.Append("</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();

        }
    }
}