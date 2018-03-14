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
    public partial class StoreCredit : BasePage
    {

        #region Declaration

        StoreCreditComponent paycomp = new StoreCreditComponent();

        tb_StoreCredit tblpay = new tb_StoreCredit();

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

                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                if (!string.IsNullOrEmpty(Request.QueryString["StoreCreditID"]) && Convert.ToString(Request.QueryString["StoreCreditID"]) != "0")
                {
                    FillStoreCreditType(Convert.ToInt32(Request.QueryString["StoreCreditID"]));
                    lblTitle.Text = "Edit StoreCredit Option";
                    lblTitle.ToolTip = "Edit StoreCredit Option";
                }
            }
        }


        /// <summary>
        /// Fills the StoreCredit type Detail
        /// </summary>
        /// <param name="StoreCreditid">int StoreCreditID</param>
        private void FillStoreCreditType(int StoreCreditid)
        {
            tblpay = paycomp.getStoreCreditType(StoreCreditid);

            txtStoreCredit.Text = Convert.ToString(tblpay.StoreCredit);
            txtStoreCreditAmount.Text = Convert.ToString(Math.Round(Convert.ToDecimal(tblpay.StoreCreditAmount), 2));

            if (tblpay.Active == true)
            {
                chkststus.Checked = true;
            }
            else
                chkststus.Checked = false;


        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["StoreCreditID"]) && Convert.ToString(Request.QueryString["StoreCreditID"]) != "0")
            {
                tblpay = paycomp.getStoreCreditType(Convert.ToInt32(Request.QueryString["StoreCreditID"]));
                tblpay.StoreCredit = txtStoreCredit.Text.Trim();
                tblpay.StoreCreditAmount = Convert.ToDecimal(txtStoreCreditAmount.Text.ToString());

                if (chkststus.Checked)
                {
                    tblpay.Active = true;
                }
                else
                    tblpay.Active = false;
                tblpay.Deleted = false;

                Int32 isupdated = paycomp.UpdateStoreCredittype(tblpay);
                if (isupdated > 0)
                {
                    Response.Redirect("StoreCreditList.aspx?status=updated");
                }

            }
            else
            {
                tblpay.StoreCredit = txtStoreCredit.Text.Trim();
                tblpay.StoreCreditAmount = Convert.ToDecimal(txtStoreCreditAmount.Text.ToString());


                //  tblpay.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                if (paycomp.CheckDuplicate(tblpay))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('StoreCredit  with same name or code already exists, please specify another name or code...', 'Message');});", true);
                    return;
                }
                if (chkststus.Checked)
                {
                    tblpay.Active = true;
                }
                else
                    tblpay.Active = false;

                tblpay.Deleted = false;
                Int32 isadded = paycomp.CreateStoreCredittype(tblpay);
                if (isadded > 0)
                {
                    Response.Redirect("StoreCreditList.aspx?status=inserted");
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
            Response.Redirect("StoreCreditList.aspx");
        }
    }
}