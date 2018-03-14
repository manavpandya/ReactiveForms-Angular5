using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class CustomerLevel : Solution.UI.Web.BasePage
    {
        #region Variable declaration
        tb_CustomerLevel custLevel = null;
        CustomerLevelComponent objCustlevelComp = null;
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
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                bindstore();
                if (!string.IsNullOrEmpty(Request.QueryString["CustomerLevelID"]) && Convert.ToString(Request.QueryString["CustomerLevelID"]) != "0")
                {
                    custLevel = new tb_CustomerLevel();
                    objCustlevelComp = new CustomerLevelComponent();
                    //Display selected Customer Level detail for edit mode
                    custLevel = objCustlevelComp.getCustLevel(Convert.ToInt32(Request.QueryString["CustomerLevelID"]));
                    lblHeader.Text = "Edit Customer Level";
                    drpStoreName.Enabled = false;
                    txtName.Text = custLevel.Name;
                    decimal DiscAmt = decimal.Zero;
                    decimal DisPerc = decimal.Zero;
                    if (custLevel.LevelDiscountAmount != Decimal.Zero)
                    {
                        DiscAmt = Math.Round(Convert.ToDecimal(custLevel.LevelDiscountAmount), 2);
                        txtDiscAmount.Text = DiscAmt.ToString();
                    }
                    else
                    {
                        txtDiscAmount.Text = "";
                    }
                    if (custLevel.LevelDiscountPercent != Decimal.Zero)
                    {
                        DisPerc = Math.Round(Convert.ToDecimal(custLevel.LevelDiscountPercent), 2);
                        txtDiscPerc.Text = DisPerc.ToString();
                    }
                    else
                    {
                        txtDiscPerc.Text = "";
                    }
                    int StoreId = custLevel.tb_StoreReference.Value.StoreID;
                    drpStoreName.SelectedValue = StoreId.ToString();
                    chkHasFreeShipping.Checked = custLevel.LevelHasFreeShipping.Value;
                    chkHasnoTax.Checked = custLevel.LevelHasnoTax.Value;
                    txtDisplayOrder.Text = custLevel.DisplayOrder.ToString();
                }
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            custLevel = new tb_CustomerLevel();
            objCustlevelComp = new CustomerLevelComponent();
            if (!string.IsNullOrEmpty(txtDiscAmount.Text.Trim()) && !string.IsNullOrEmpty(txtDiscPerc.Text.Trim()))
            {
                lblMsg.Text = "You can enter either Discount Percent or Discount Amount";

                return;
            }
            else if (string.IsNullOrEmpty(txtDiscAmount.Text.Trim()) && string.IsNullOrEmpty(txtDiscPerc.Text.Trim()))
            {
                lblMsg.Text = "Please enter either Discount Percent or Discount Amount";

                return;
            }
            else if (!string.IsNullOrEmpty(txtDiscPerc.Text.Trim()) && Convert.ToDouble(txtDiscPerc.Text.Trim()) > 100)
            {
                lblMsg.Text = "Discount Percent should be less than 100";

                return;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["CustomerLevelID"]) && Convert.ToString(Request.QueryString["CustomerLevelID"]) != "0")
            {
                custLevel = objCustlevelComp.getCustLevel(Convert.ToInt32(Request.QueryString["CustomerLevelID"]));
                custLevel.Name = txtName.Text;
                if (!string.IsNullOrEmpty(txtDiscPerc.Text))
                {
                    custLevel.LevelDiscountPercent = Convert.ToDecimal(txtDiscPerc.Text);
                }
                else
                    custLevel.LevelDiscountPercent = Decimal.Zero;
                if (!string.IsNullOrEmpty(txtDiscAmount.Text))
                {
                    custLevel.LevelDiscountAmount = Convert.ToDecimal(txtDiscAmount.Text);
                }
                else
                    custLevel.LevelDiscountAmount = Decimal.Zero;
                custLevel.LevelHasFreeShipping = chkHasFreeShipping.Checked;
                custLevel.LevelHasnoTax = chkHasnoTax.Checked;
                if (!string.IsNullOrEmpty(txtDisplayOrder.Text))
                    custLevel.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                else
                    custLevel.DisplayOrder = 0;
                Int32 isupdated = objCustlevelComp.UpdateCustomerLevel(custLevel);
                if (isupdated > 0)
                {
                    Response.Redirect("CustomerLevelList.aspx?status=updated");
                }
            }
            else
            {
                custLevel.Name = txtName.Text.Trim();
                int StoreId1 = Convert.ToInt32(drpStoreName.SelectedItem.Value.ToString());
                custLevel.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                //Check CustomerLevel name already exists or not
                if (objCustlevelComp.CheckDuplicate(custLevel))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Customer Level Name already exists.', 'Message');});", true);
                    return;
                }
                if (!string.IsNullOrEmpty(txtDiscPerc.Text))
                {
                    custLevel.LevelDiscountPercent = Convert.ToDecimal(txtDiscPerc.Text);
                }
                else
                    custLevel.LevelDiscountPercent = Decimal.Zero;
                if (!string.IsNullOrEmpty(txtDiscAmount.Text))
                {
                    custLevel.LevelDiscountAmount = Convert.ToDecimal(txtDiscAmount.Text);
                }
                else
                    custLevel.LevelDiscountAmount = Decimal.Zero;
                custLevel.LevelHasFreeShipping = chkHasFreeShipping.Checked;
                custLevel.LevelHasnoTax = chkHasnoTax.Checked;
                if (!string.IsNullOrEmpty(txtDisplayOrder.Text))
                    custLevel.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                else
                    custLevel.DisplayOrder = 0;
                custLevel.Deleted = false;
                custLevel.CreatedOn = System.DateTime.Now;

                Int32 isadded = objCustlevelComp.CreateCustomerLevel(custLevel);

                if (isadded > 0)
                {
                    Response.Redirect("CustomerLevelList.aspx?status=inserted");
                }

            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("CustomerLevelList.aspx");
        }

        /// <summary>
        /// Bind all Stores in drop down
        /// </summary>
        private void bindstore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                drpStoreName.DataSource = storeDetail;
                drpStoreName.DataTextField = "StoreName";
                drpStoreName.DataValueField = "StoreID";
                drpStoreName.DataBind();
            }

        }

    }
}