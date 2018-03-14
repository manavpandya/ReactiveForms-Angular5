using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.Common;

namespace Solution.UI.Web
{
    public partial class ThankYou : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            Session["PageName"] = "Thank You";
            ltbrTitle.Text = "Thank You";
            ltTitle.Text = "Thank You";
            if (Request.QueryString["Page"] != null)
            {
                string str = Convert.ToString(Request.QueryString["Page"]);

                if (str == "TellAFriend")
                {
                    lblfriend.Text = "Thanks for referring " + Convert.ToString(AppLogic.AppConfigs("StoreName")) + " Product to your friend.";
                    lblCreateAccount.Text = "";
                    btnCheckoutNow.Visible = false;
                }


                if (str == "createaccount")
                {
                    lblCreateAccount.Text = "Thanks for creating your account in " + Convert.ToString(AppLogic.AppConfigs("StoreName")) + ".";

                    if (Session["CustID"] != null && Session["CustID"].ToString().Trim() == "Cart")

                        btnkeepshopping.Visible = true;
                    else
                        btnCheckoutNow.Visible = false;
                }
                else if (str == "ReturnMerchandise")
                {
                    ltReturnMerchandise.Text = "<span style='line-height:22px;'>Your Return Merchandise request has been submitted successfully..";
                    ltReturnMerchandise.Text += "<br/>Your RMA reference number is : <b Style='color:#DA526E;font-size:12px;'>RMA-" + Request.QueryString["ID"].ToString() + "</b><br/>";
                    ltReturnMerchandise.Text += "Please <b Style='color:#DA526E;font-size:12px;'><a onclick='javascript:window.open(\"ReturnPackageSlip.aspx?ID=" + Request.QueryString["ID"].ToString() + "\", \"\",\"height=600,width=820,scrollbars=1,left=50,top=50,toolbar=no,menubar=no\");' style='text-decoration:none;'>click here</a></b> to print your RMA slip. Attach this slip with your return shipment.<br/></span>";
                    //ltReturnMerchandise.Text = "Return Merchandise E-Mail has been sent successfully.<br/>Your RMA Numnber is <a href='/PackageSlip.aspx?ID=25&oNo=" + Request.QueryString["ONo"].ToString() + "' style='text-decoration:none;color:#636363'><b>RMA-24</b></a><br/>";
                    btnCheckoutNow.Visible = false;
                }
            }
            spnStoreName.InnerText = Convert.ToString(AppLogic.AppConfigs("StoreName"));
        }

        /// <summary>
        /// Redirect the Page on Index Page for Continue the Shopping
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnkeepshopping_Click(object sender, EventArgs e)
        {
            #region Comment for Future Use
            //String NavigateUrl = ECommerceSite.Client.AppLogic.AppConfig("KeepShoppingURL");
            //if (NavigateUrl.Trim().Length == 0)
            //    NavigateUrl = "Index.aspx";
            #endregion
            Response.Redirect("index.aspx");
        }

        /// <summary>
        /// Redirect the Page on CheckOut Page for CheckOut the Shopping
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCheckoutNow_Click(Object sender, EventArgs e)
        {
            Response.Redirect("checkoutcommon.aspx");
        }
    }
}