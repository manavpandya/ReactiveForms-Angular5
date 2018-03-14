using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.UI.Web.ADMIN.Configuration
{
    public partial class ShippingMethod : BasePage
    {
        #region Declaration
        tb_ShippingMethods tblShippingMethods = null;
        tb_ShippingServices tblShippingServices = null;
        ShippingComponent objShippingMethods = null;
        private static DataSet DS = new DataSet();
        int StoreId = 0;
        int ShippingServiceId = 0;
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

                if (!string.IsNullOrEmpty(Request.QueryString["ShippingMethodID"]) && Convert.ToString(Request.QueryString["ShippingMethodID"]) != "0")
                {
                    FillShippingMethod(Convert.ToInt32(Request.QueryString["ShippingMethodID"]));
                    lblTitle.Text = "Edit Shipping Method";
                    lblTitle.ToolTip = "Edit Shipping Method";
                }
            }
        }

        /// <summary>
        /// Fill Data  For Shipping Method in Edit Mode 
        /// </summary>
        /// <param name="ShippingMethodID">int ShippingMethodID</param>
        private void FillShippingMethod(int ShippingMethodID)
        {
            ddlStore.Enabled = false;
            ddlShippingService.Enabled = false;
            tblShippingMethods = new tb_ShippingMethods();
            objShippingMethods = new ShippingComponent();
            tblShippingMethods = objShippingMethods.GetShippingMethod(ShippingMethodID);
            ShippingServiceId = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(tblShippingMethods.tb_ShippingServicesReference)).EntityKey.EntityKeyValues[0].Value);
            tblShippingServices = objShippingMethods.GetShippingService(ShippingServiceId);
            StoreId = tblShippingServices.tb_StoreReference.Value.StoreID;
            txtName.Text = tblShippingMethods.Name;
            if (tblShippingMethods.AdditionalPrice != null)
            {
                txtAddPrice.Text = tblShippingMethods.AdditionalPrice.Value.ToString("#.##");
            }
            else
                txtAddPrice.Text = tblShippingMethods.AdditionalPrice.ToString();
            if (tblShippingMethods.FixedPrice != null)
            {
                txtFixedPrice.Text = tblShippingMethods.FixedPrice.Value.ToString("#.##");
            }
            else
                txtFixedPrice.Text = tblShippingMethods.FixedPrice.ToString();
            ddlStore.SelectedValue = StoreId.ToString();
            BindShippingService(StoreId.ToString());
            ddlShippingService.SelectedValue = ShippingServiceId.ToString();

            if (tblShippingMethods.isRTShipping == true)
            {
                chkRTShippping.Checked = true;
            }
            else
                chkRTShippping.Checked = false;

            if (tblShippingMethods.ShowOnClient == true)
            {
                chkShowOnClient.Checked = true;
            }
            else
                chkShowOnClient.Checked = false;
            if (tblShippingMethods.ShowOnAdmin == true)
            {
                chkShowOnAdmin.Checked = true;
            }
            else
                chkShowOnAdmin.Checked = false;
            if (tblShippingMethods.Active == true)
            {
                chkActive.Checked = true;
            }
            else
                chkActive.Checked = false;

            string strDesc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ShippingDescription,'') FROM tb_ShippingMethods WHERE ShippingMethodID=" + ShippingMethodID + ""));
            txtdesc.Text = strDesc.ToString();
        }

        /// <summary>
        /// Bind Store 
        /// </summary>
        private void BindStore()
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
            ddlStore.Items.Insert(0, new ListItem("Select Store", "-1"));
            ddlShippingService.Items.Insert(0, new ListItem("Select Shipping Service", "-1"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                ddlStore_SelectedIndexChanged(null, null);
            }
            else
                ddlStore.SelectedIndex = 0;
        }

        /// <summary>
        ///  imgSave Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            objShippingMethods = new ShippingComponent();
            tblShippingMethods = new tb_ShippingMethods();

            if (!string.IsNullOrEmpty(Request.QueryString["ShippingMethodID"]) && Convert.ToString(Request.QueryString["ShippingMethodID"]) != "0")
            {
                tblShippingMethods = objShippingMethods.GetShippingMethod(Convert.ToInt32(Request.QueryString["ShippingMethodID"]));
                tblShippingMethods.Name = txtName.Text.Trim();
                if (txtAddPrice.Text.Trim() != "")
                {
                    tblShippingMethods.AdditionalPrice = Convert.ToDecimal(txtAddPrice.Text.Trim());
                }
                else
                    tblShippingMethods.AdditionalPrice = null;
                if (txtFixedPrice.Text.Trim() != "")
                {
                    tblShippingMethods.FixedPrice = Convert.ToDecimal(txtFixedPrice.Text.Trim());
                }
                else
                    tblShippingMethods.FixedPrice = null;
                if (chkRTShippping.Checked)
                {
                    tblShippingMethods.isRTShipping = true;
                }
                else
                    tblShippingMethods.isRTShipping = false;
                if (chkShowOnClient.Checked)
                {
                    tblShippingMethods.ShowOnClient = true;
                }
                else
                    tblShippingMethods.ShowOnClient = false;
                if (chkShowOnAdmin.Checked)
                {
                    tblShippingMethods.ShowOnAdmin = true;
                }
                else
                    tblShippingMethods.ShowOnAdmin = false;

                if (chkActive.Checked)
                {
                    tblShippingMethods.Active = true;
                }
                else
                    tblShippingMethods.Active = false;

                int ShippingserviceId1 = Convert.ToInt32(ddlShippingService.SelectedItem.Value.ToString());
                tblShippingMethods.tb_ShippingServicesReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_ShippingServices", "ShippingServiceID", ShippingserviceId1);
                tblShippingMethods.Deleted = false;
                tblShippingMethods.UpdatedOn = DateTime.Now;
                tblShippingMethods.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                Int32 isupdated = objShippingMethods.UpdateShippingMethod(tblShippingMethods);
                CommonComponent.ExecuteCommonData("Update tb_ShippingMethods SET ShippingDescription='" + txtdesc.Text.ToString().Replace("'", "''") + "'  WHERE ShippingMethodID=" + isupdated + "");
                if (isupdated > 0)
                {
                    Response.Redirect("ShippingMethodList.aspx?status=updated");
                }
            }
            else
            {
                tblShippingMethods.Name = txtName.Text.Trim();
                if (objShippingMethods.CheckDuplicateShipppingMethod(txtName.Text.Trim(), ddlShippingService.SelectedItem.Text))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Shipping Method with same Name already exists, please specify another Name...', 'Message','');});", true);
                    return;
                }
                if (txtAddPrice.Text.Trim() != "")
                {
                    tblShippingMethods.AdditionalPrice = Convert.ToDecimal(txtAddPrice.Text.Trim());
                }
                else
                    tblShippingMethods.AdditionalPrice = null;
                if (txtFixedPrice.Text != "")
                    tblShippingMethods.FixedPrice = Convert.ToDecimal(txtFixedPrice.Text);
                else
                    tblShippingMethods.FixedPrice = null;
                if (chkRTShippping.Checked)
                {
                    tblShippingMethods.isRTShipping = true;
                }
                else
                    tblShippingMethods.isRTShipping = false;
                if (chkShowOnClient.Checked)
                {
                    tblShippingMethods.ShowOnClient = true;
                }
                else
                    tblShippingMethods.ShowOnClient = false;
                if (chkShowOnAdmin.Checked)
                {
                    tblShippingMethods.ShowOnAdmin = true;
                }
                else
                    tblShippingMethods.ShowOnAdmin = false;

                if (chkActive.Checked)
                {
                    tblShippingMethods.Active = true;
                }
                else
                    tblShippingMethods.Active = false;
                tblShippingMethods.Deleted = false;
                int ShippingserviceId1 = Convert.ToInt32(ddlShippingService.SelectedItem.Value.ToString());
                tblShippingMethods.tb_ShippingServicesReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_ShippingServices", "ShippingServiceID", ShippingserviceId1);
                tblShippingMethods.CreatedOn = DateTime.Now;
                tblShippingMethods.CreatedBy = Convert.ToInt32(Session["AdminID"]);




                Int32 isadded = objShippingMethods.CreateShippingMethod(tblShippingMethods);
                CommonComponent.ExecuteCommonData("Update tb_ShippingMethods SET ShippingDescription='" + txtdesc.Text.ToString().Replace("'", "''") + "'  WHERE ShippingMethodID=" + isadded + "");
                if (isadded > 0)
                {
                    Response.Redirect("ShippingMethodList.aspx?status=inserted");
                }
            }
        }

        /// <summary>
        ///  imgCancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ShippingMethodList.aspx");
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            string storeID = ddlStore.SelectedValue.ToString();
            BindShippingService(storeID);
        }

        /// <summary>
        /// Bind Shipping Service with Respect to ddlStore
        /// </summary>
        /// <param name="storeID">string storeID</param>
        protected void BindShippingService(string storeID)
        {
            ddlShippingService.Items.Clear();
            objShippingMethods = new ShippingComponent();
            DS = objShippingMethods.SelectShippingService(Convert.ToInt32(storeID));
            if (DS != null && DS.Tables[0] != null && DS.Tables[0].Rows.Count > 0)
            {
                ddlShippingService.DataSource = DS;
                ddlShippingService.DataTextField = "ShippingService";
                ddlShippingService.DataValueField = "ShippingServiceID";
                ddlShippingService.DataBind();
            }
            ddlShippingService.Items.Insert(0, new ListItem("Select Shipping Service", "-1"));
            ddlShippingService.SelectedIndex = 0;
        }
    }
}