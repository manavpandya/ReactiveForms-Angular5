using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class RefundOrder : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            btnReset.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/reser-filter.gif";
            imgClose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel-icon.png";
            if (!IsPostBack)
            {

                if (Request.QueryString["oNo"] != null)
                {
                    txtAmount.Focus();
                    ltorderNumber.Text = Request.QueryString["oNo"].ToString();
                }

            }
            // Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "document.getElementById('ltrTotal').value=parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_hdnAmount').value).toFixed(2);", true);

        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

            string strResult = "";

            DataSet dsAutho = new DataSet();
            dsAutho = CommonComponent.GetCommonDataSet("SELECT isnull(AuthorizationPNREF,'') as AuthorizationPNREF,isnull(PaymentGateway,'') as PaymentGateway,isnull(OrderTotal,0) as OrderTotal,isnull(TransactionStatus,'') as TransactionStatus FROM tb_order where OrderNumber=" + Request.QueryString["oNo"].ToString() + "");
            string PaymentGAteWay = "";
            string AuthorizationPNREF = "";
            string TransactionStatus = "";
            Decimal OrderTotal = decimal.Zero;
            if (dsAutho != null && dsAutho.Tables.Count > 0 && dsAutho.Tables[0].Rows.Count > 0)
            {
                AuthorizationPNREF = dsAutho.Tables[0].Rows[0]["AuthorizationPNREF"].ToString();
                PaymentGAteWay = dsAutho.Tables[0].Rows[0]["PaymentGateway"].ToString();
                OrderTotal = Convert.ToDecimal(dsAutho.Tables[0].Rows[0]["OrderTotal"].ToString());
                TransactionStatus = dsAutho.Tables[0].Rows[0]["TransactionStatus"].ToString();
            }
            if (PaymentGAteWay.ToString().ToLower().Trim() == "paypal")
            {
                PayPalComponent objPay = new PayPalComponent();
                if (TransactionStatus.ToString().ToLower() == "captured" || TransactionStatus.ToString().ToLower() == "partially refunded")
                {
                    strResult = objPay.RefundOrder(Convert.ToInt32(Request.QueryString["oNo"].ToString()), Convert.ToDecimal(txtAmount.Text.ToString()), txtReason.Text.ToString(), AuthorizationPNREF, OrderTotal);
                }
            }
            else if (PaymentGAteWay.ToString().ToLower().Trim() == "paypalexpress")
            {
                PayPalComponent objPay = new PayPalComponent();
                if (TransactionStatus.ToString().ToLower() == "captured" || TransactionStatus.ToString().ToLower() == "partially refunded")
                {
                    strResult = objPay.RefundOrder(Convert.ToInt32(Request.QueryString["oNo"].ToString()), Convert.ToDecimal(txtAmount.Text.ToString()), txtReason.Text.ToString(), AuthorizationPNREF, OrderTotal);
                }
            }

            else if (PaymentGAteWay.ToString().ToLower().Trim() == "authorizenet")
            {
                if (TransactionStatus.ToString().ToLower() == "captured" || TransactionStatus.ToString().ToLower() == "partially refunded")
                {
                    strResult = AuthorizeNetComponent.RefundOrder(Convert.ToInt32(Request.QueryString["oNo"].ToString()), Convert.ToDecimal(txtAmount.Text.ToString()), txtReason.Text.ToString());
                }
            }
            if (strResult == "OK")
            {
                if (Convert.ToDecimal(hdnamtorder.Value.ToString()) == (Convert.ToDecimal(hdnamtrefund.Value.ToString()) + Convert.ToDecimal(txtAmount.Text.ToString())))
                {

                    CommonComponent.ExecuteCommonData("UPDATE tb_order SET TransactionStatus='REFUNDED',RefundedAmount=isnull(RefundedAmount,0)+" + txtAmount.Text + ", RefundedOn=getdate(),RefundReason=isnull(RefundReason,'')+'|" + txtReason.Text.ToString().Replace("'", "''") + "' WHERE orderNumber=" + Request.QueryString["oNo"] + "");
                    //  CommonComponent.ExecuteCommonData("EXEC usp_Product_AdjustInventory " + Request.QueryString["oNo"].ToString() + ",1, 0");
                    OrderComponent objOrder = new OrderComponent();
                    objOrder.UpdateInventoryByOrderNumber(Convert.ToInt32(Request.QueryString["oNo"].ToString()), 1);

                    OrderComponent objOrderlog = new OrderComponent();
                    objOrderlog.InsertOrderlog(4, Convert.ToInt32(Request.QueryString["oNo"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                }
                else
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_order SET TransactionStatus='PARTIALLY REFUNDED',RefundedAmount=isnull(RefundedAmount,0)+" + txtAmount.Text + ", RefundedOn=getdate(),RefundReason=isnull(RefundReason,'')+'|" + txtReason.Text.ToString().Replace("'", "''") + "' WHERE orderNumber=" + Request.QueryString["oNo"] + "");
                    OrderComponent objOrderlog = new OrderComponent();
                    objOrderlog.InsertOrderlog(5, Convert.ToInt32(Request.QueryString["oNo"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                }


            }
            else
            {
                if (strResult != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "','Failed');", true);
                }
            }


            if (Request.QueryString["OrderList"] != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.opener.document.getElementById('ContentPlaceHolder1_btnRefund').click();window.close();", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.opener.location.href=window.opener.location.href;window.close();", true);
            }
        }
    }
}