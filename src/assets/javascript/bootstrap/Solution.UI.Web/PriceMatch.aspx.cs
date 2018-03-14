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

namespace Solution.UI.Web
{
    public partial class PriceMatch : System.Web.UI.Page
    {
        #region Local Variable

        CustomerComponent objCustomer = null;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ShowknowWeb", "changeDiv('Website');", true);

                if (Request.QueryString["ProductID"] != null)
                {
                    DataSet dsImage = new DataSet();
                    dsImage = ProductComponent.GetproductImagename(Convert.ToInt32(Request.QueryString["ProductID"]));
                    if (dsImage != null && dsImage.Tables.Count > 0 && dsImage.Tables[0].Rows.Count > 0)
                    {
                        txtItem1.InnerText = dsImage.Tables[0].Rows[0]["Name"].ToString();
                    }
                }
            }
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            String Email = string.Empty;
            Email = Request["txtEmail"].ToString();

            string MailAddress = AppLogic.AppConfigs("ContactMail_ToAddress");
            if (MailAddress != string.Empty)
            {
                SendMail(MailAddress);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "AlertScript", "Sorry, We are unable to find customer service representative Email Address. Please try after some time.", true);
            }
            popup_content.Style.Add("Display", "none");
            divDone.Style.Add("Display", "block");
        }

        /// <summary>
        /// Sends Mail for Admin 
        /// </summary>
        /// <param name="AddressTo">string AddressTo</param>
        private void SendMail(String AddressTo)
        {
            String Name = String.Empty;
            String Email = String.Empty;
            String Phone = String.Empty;
            String Zipcode = String.Empty;
            String Comment = String.Empty;
            String StoreName = String.Empty;
            String StoreCity = String.Empty;
            String StoreState = String.Empty;
            String StorePhone = String.Empty;
            String StoreWeb = String.Empty;
            decimal ShippingAmt = decimal.Zero;
            decimal TotalAmt = decimal.Zero;
            int hdnTotalProduct = 0;
            hdnTotalProduct = Convert.ToInt16(Request["hdnItemNo"]);
            Name = Request["txtName"].ToString();
            Email = Request["txtEmail"].ToString();
            Phone = Request["txtPhone"].ToString();
            Zipcode = Request["txtZipcode"].ToString();
            Comment = Request["txtComment"].ToString();
            Comment = Comment.Replace("\r\n", "<br />");
            StoreName = Request["txtSstore"].ToString();
            StoreCity = Request["txtScity"].ToString();
            StoreState = Request["txtSstate"].ToString();
            StorePhone = Request["txtSphone"].ToString();
            StoreWeb = Request["txtSweb"].ToString();
            if (Request["txtShipping"] == "")
            {
                ShippingAmt = 0;
            }
            else
            {
                ShippingAmt = Convert.ToDecimal(Request["txtShipping"]);
            }
            TotalAmt = Convert.ToDecimal(Request["txtTotalPrice"]);
            int i;
            String rbtval = string.Empty;
            rbtval = Request["rbtknow"].ToString();

            String strStore = "";
            String strProduct = "";
            objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();

            dsMailTemplate = objCustomer.GetEmailTamplate("PriceMatch", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {

                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                strSubject = Regex.Replace(strSubject, "###FIRSTNAME###", Name, RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###FIRSTNAME###", Name, RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###EMAIL###", Email, RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###PHONE###", Phone, RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###ZIPCODE###", Zipcode, RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###COMMENT###", Comment, RegexOptions.IgnoreCase);

                if (rbtval == "Website")
                {
                    strStore += StoreWeb;
                }
                else
                {
                    strStore += "<tr><td style=\"font-size:12px;line-height: 25px;\">Store Name :</td><td style=\"font-size:12px;line-height: 25px;\">" + StoreName + "</td></tr>";
                    strStore += "<tr><td style=\"font-size:12px;line-height: 25px;\">Store City :</td><td style=\"font-size:12px;line-height: 25px;\">" + StoreCity + "</td></tr>";
                    strStore += "<tr><td style=\"font-size:12px;line-height: 25px;\">Store State :</td><td style=\"font-size:12px;line-height: 25px;\">" + StoreState + "</td></tr>";
                    if (StorePhone != "")
                    {
                        strStore += "<tr><td style=\"font-size:12px;line-height: 25px;\">Store Phone :</td><td style=\"font-size:12px;line-height: 25px;\">" + StorePhone + "</td></tr>";
                    }
                }
                strBody = Regex.Replace(strBody, "###STOREINFO###", strStore, RegexOptions.IgnoreCase);
                strProduct = "</th></tr>";
                for (i = 1; i <= hdnTotalProduct; i++)
                {
                    decimal price = Convert.ToDecimal(Request["txtPrice" + i].ToString());
                    strProduct += "<tr><td>" + txtItem1.InnerText.ToString() + "</td>";
                    strProduct += "<td style=\"text-align: right; padding-right: 5px;\">$ " + string.Format("{0:0.00}", Convert.ToDecimal(price)) + "</td></tr>";
                }
                strProduct += "<tr style=\"display:none;\"><td colspan=\"2\"></td>"; // Don't close </tr> because some reason behind this. more details go to Email Template.
                strBody = Regex.Replace(strBody, "###PRODUCTLIST###</th>", strProduct, RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###SHIPPINGCHARGES###", "$ " + string.Format("{0:0.00}",Convert.ToDecimal(ShippingAmt)), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###TOTALCOST###", "$ " + string.Format("{0:0.00}", Convert.ToDecimal(TotalAmt)), RegexOptions.IgnoreCase);
                strBody = strBody.Replace("$ ", "$");

                DataSet dsTopic = new DataSet();
                dsTopic = TopicComponent.GetTopicList("InvoiceSignature", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    strBody = Regex.Replace(strBody, "###SIGNATURE###", Convert.ToString(dsTopic.Tables[0].Rows[0]["Description"]), RegexOptions.IgnoreCase);
                }
                else
                {
                    strBody = Regex.Replace(strBody, "###SIGNATURE###", "", RegexOptions.IgnoreCase);
                }

                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMailWithReplyTo(AddressTo.Trim(), Email.Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }
    }
}