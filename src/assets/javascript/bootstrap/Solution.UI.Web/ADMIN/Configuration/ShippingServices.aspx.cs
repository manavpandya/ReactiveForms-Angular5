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
    public partial class ShippingServices : BasePage
    {
        #region Declaration
        tb_ShippingServices tblShippingService = null;
        ShippingComponent objShipping = null;
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
                bindstore();
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                if (!string.IsNullOrEmpty(Request.QueryString["ShippingServiceID"]) && Convert.ToString(Request.QueryString["ShippingServiceID"]) != "0")
                {
                    FillShippingService(Convert.ToInt32(Request.QueryString["ShippingServiceID"]));
                    lblTitle.Text = "Edit Shipping Service";
                    lblTitle.ToolTip = "Edit Shipping Service";
                }
            }
        }

        /// <summary>
        ///  Bind Store Details in dropdown
        /// </summary>
        private void bindstore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Store", "-1"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
        }

        /// <summary>
        ///  Fill values for Particular ShippingServiceID
        /// </summary>
        /// <param name="ShippingServiceID">int ShippingServiceID</param>
        private void FillShippingService(int ShippingServiceID)
        {
            ddlStore.Enabled = false;
            tblShippingService = new tb_ShippingServices();
            objShipping = new ShippingComponent();
            tblShippingService = objShipping.GetShippingService(ShippingServiceID);
            txtName.Text = tblShippingService.ShippingService;
            if (tblShippingService.Active == true)
            {
                chkActive.Checked = true;
            }
            else
                chkActive.Checked = false;
            StoreId = tblShippingService.tb_StoreReference.Value.StoreID;
            ddlStore.SelectedValue = StoreId.ToString();
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            objShipping = new ShippingComponent();
            tblShippingService = new tb_ShippingServices();

            if (!string.IsNullOrEmpty(Request.QueryString["ShippingServiceID"]) && Convert.ToString(Request.QueryString["ShippingServiceID"]) != "0")
            {
                tblShippingService = objShipping.GetShippingService(Convert.ToInt32(Request.QueryString["ShippingServiceID"]));
                tblShippingService.ShippingService = txtName.Text.Trim();
                if (chkActive.Checked)
                {
                    tblShippingService.Active = true;
                }
                else
                    tblShippingService.Active = false;
                Int32 isupdated = objShipping.UpdateShippingService(tblShippingService);
                if (isupdated > 0)
                {
                    Response.Redirect("ShippingServicesList.aspx?status=updated");
                }
            }
            else
            {
                int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                tblShippingService.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                tblShippingService.ShippingService = txtName.Text.Trim();
                if (objShipping.CheckDuplicate(tblShippingService))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Shipping Service with same name already exists, please specify another name...', 'Message','');});", true);
                    return;
                }
                if (chkActive.Checked)
                {
                    tblShippingService.Active = true;
                }
                else
                    tblShippingService.Active = false;

                //int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                //tblShippingService.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                Int32 isadded = objShipping.CreateShippingService(tblShippingService);
                if (isadded > 0)
                {
                    Response.Redirect("ShippingServicesList.aspx?status=inserted");
                }
            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ShippingServicesList.aspx");
        }
    }
}