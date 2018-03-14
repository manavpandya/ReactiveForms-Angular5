using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class AddFeature : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            if (!IsPostBack)
            {
                btnSaveFeature.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancelFeature.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                bindstore();
               
                if (!string.IsNullOrEmpty(Request.QueryString["FeatureId"]) && Convert.ToString(Request.QueryString["FeatureId"]) != "0")
                {

                    tb_ProductFeature ProductFeature = new tb_ProductFeature();
                    ProductComponent objProduct = new ProductComponent();
                    ProductFeature = objProduct.GetProductFeatureByID(Convert.ToInt32(Request.QueryString["FeatureId"]));
                    txtfeaturename.Text = ProductFeature.Name;
                    txtFeatureBody.Text = ProductFeature.Body;
                    if (!string.IsNullOrEmpty(ProductFeature.Active.ToString()) && Convert.ToBoolean(ProductFeature.Active))
                    {
                        chkIsActive.Checked = true;
                    }
                    else { chkIsActive.Checked = false; }
                    lblHeader.Text = "Edit Product Feature";
                    ddlStore.Enabled = false;
                    int StoreId = ProductFeature.tb_StoreReference.Value.StoreID;
                    ddlStore.SelectedValue = StoreId.ToString();
                }

            }

        }
        protected void btnSaveFeature_Click(object sender, ImageClickEventArgs e)
        {
            int FeatureID = 0;

            tb_ProductFeature ProductFeature = new tb_ProductFeature();
            ProductComponent objProduct = new ProductComponent();

            if (txtFeatureBody.Text.Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Email Body.', 'Message','');});", true);
                return;
            }
            int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
            ProductFeature.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
            if (!string.IsNullOrEmpty(Request.QueryString["FeatureID"]) && Convert.ToString(Request.QueryString["FeatureID"]) != "0")
            {

                ProductFeature = objProduct.GetProductFeatureByID(Convert.ToInt32(Request.QueryString["FeatureId"]));
                ProductFeature.Name = txtfeaturename.Text.Trim();
                if (objProduct.CheckDuplicate(ProductFeature))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Feature Name already exists.', 'Message');});", true);
                    return;
                }


                ProductFeature.Body = txtFeatureBody.Text.Trim();
                ProductFeature.Active = chkIsActive.Checked;
                ProductFeature.Createdon = DateTime.Now;
                FeatureID = objProduct.UpdateProductFeature(ProductFeature);
                if (FeatureID > 0)
                    Response.Redirect("FeatureList.aspx?status=updated");
            }
            else
            {
                ProductFeature.Name = txtfeaturename.Text.Trim();
                if (objProduct.CheckDuplicate(ProductFeature))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Feature Name already exists.', 'Message');});", true);
                    return;
                }


                ProductFeature.Body = txtFeatureBody.Text.Trim();
                ProductFeature.Active = chkIsActive.Checked;
                ProductFeature.Createdon = DateTime.Now;
                FeatureID = objProduct.CreateProductFeature(ProductFeature);
                if (FeatureID > 0)
                    Response.Redirect("FeatureList.aspx?status=inserted");
            }
        }

        protected void btnCancelFeature_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("FeatureList.aspx");

        }
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
                ddlStore.SelectedIndex = 0;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]) && Convert.ToString(Request.QueryString["StoreID"]) != "0")
            {
                ddlStore.SelectedValue = Request.QueryString["StoreID"].ToString();

            }
            else if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
                AppConfig.StoreID = 1;
        }
    }
}