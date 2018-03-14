using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Configuration
{
    public partial class PaymentType : Solution.UI.Web.BasePage
    {

        #region Declaration

        StoreComponent stac = new StoreComponent();
        tb_Payment tblpay = new tb_Payment();
        PaymentComponent paycomp = new PaymentComponent();
        int StoreId = 0;
        
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
                BindStore();
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                if (!string.IsNullOrEmpty(Request.QueryString["PaymentID"]) && Convert.ToString(Request.QueryString["PaymentID"]) != "0")
                {
                    FillPaymentType(Convert.ToInt32(Request.QueryString["PaymentID"]));
                    lblTitle.Text = "Edit Payment Option";
                    lblTitle.ToolTip = "Edit Payment Option";
                }
            }
        }

        /// <summary>
        /// Binds the Store Dropdown
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                ddlStore.DataSource = Storelist;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
            }
            else
            {
                ddlStore.DataSource = null;
            }
            ddlStore.DataBind();
            ddlStore.Items.Insert(0, new ListItem("All Store", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
        }

        /// <summary>
        /// Fills the payment type Detail
        /// </summary>
        /// <param name="paymentid">int PaymentID</param>
        private void FillPaymentType(int paymentid)
        {
            tblpay = paycomp.getPaymentType(paymentid);
            ddlStore.Enabled = false;
            txtpayment.Text = tblpay.PaymentType;
            txtdescription.Text = tblpay.Description;
            if (tblpay.Active == true)
            {
                chkststus.Checked = true;
            }
            else
                chkststus.Checked = false;
            StoreId = tblpay.tb_StoreReference.Value.StoreID;
            ddlStore.SelectedValue = StoreId.ToString();
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["PaymentID"]) && Convert.ToString(Request.QueryString["PaymentID"]) != "0")
            {
                tblpay = paycomp.getPaymentType(Convert.ToInt32(Request.QueryString["PaymentID"]));
                tblpay.PaymentType = txtpayment.Text.Trim();
                tblpay.Description = txtdescription.Text.Trim();
                if (chkststus.Checked)
                {
                    tblpay.Active = true;
                }
                else
                    tblpay.Active = false;
                tblpay.Deleted = false;
                tblpay.UpdatedOn = DateTime.Now;
                tblpay.UdatedBy = Convert.ToInt32(Session["AdminID"]);
                Int32 isupdated = paycomp.Updatepaymenttype(tblpay);
                if (isupdated > 0)
                {
                    Response.Redirect("PaymentTypeList.aspx?status=updated");
                }

            }
            else
            {
                tblpay.PaymentType = txtpayment.Text.Trim();
                tblpay.Description = txtdescription.Text.Trim();
                int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                tblpay.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                if (paycomp.CheckDuplicate(tblpay))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Payment Option with same name or code already exists, please specify another name or code...', 'Message');});", true);
                    return;
                }
                if (chkststus.Checked)
                {
                    tblpay.Active = true;
                }
                else
                    tblpay.Active = false;
                tblpay.CreatedOn = DateTime.Now;
                tblpay.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                tblpay.Deleted = false;
                Int32 isadded = paycomp.Createpaymenttype(tblpay);
                if (isadded > 0)
                {
                    Response.Redirect("PaymentTypeList.aspx?status=inserted");
                }
            }
        }

        /// <summary>
        ///  ImgCancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void imgCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("PaymentTypeList.aspx");
        }
    }
}