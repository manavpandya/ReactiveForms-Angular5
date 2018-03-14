using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.TaxManagement
{
    /// <summary>
    /// Tax Class For Adding/Updating  Tax Class
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public partial class TaxClass : BasePage
    {
        #region Declaration
        tb_TaxClass tblTaxClass = null;
        TaxClassComponent objTaxClass = null;
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

                if (!string.IsNullOrEmpty(Request.QueryString["TaxClassID"]) && Convert.ToString(Request.QueryString["TaxClassID"]) != "0")
                {
                    FillTaxClass(Convert.ToInt32(Request.QueryString["TaxClassID"]));
                    lblTitle.Text = "Edit Tax Class";
                    lblTitle.ToolTip = "Edit Tax Class";
                }
            }
        }

       /// <summary>
        ///  Fill values in TexBox for Particular taxclassid
       /// </summary>
        /// <param name="taxclassid">int taxclassid</param>
        private void FillTaxClass(int taxclassid)
        {
            ddlStore.Enabled = false;
            tblTaxClass = new tb_TaxClass();
            objTaxClass = new TaxClassComponent();
            tblTaxClass = objTaxClass.gettaxclass(taxclassid);
            txtTaxClass.Text = tblTaxClass.TaxName;
            txtTaxCode.Text = tblTaxClass.TaxCode;
            txtDisplayOrder.Text = tblTaxClass.DisplayOrder.ToString();
            StoreId = tblTaxClass.tb_StoreReference.Value.StoreID;
            ddlStore.SelectedValue = StoreId.ToString();
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
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            objTaxClass = new TaxClassComponent();
            tblTaxClass = new tb_TaxClass();

            if (!string.IsNullOrEmpty(Request.QueryString["TaxClassID"]) && Convert.ToString(Request.QueryString["TaxClassID"]) != "0")
            {
                tblTaxClass = objTaxClass.gettaxclass(Convert.ToInt32(Request.QueryString["TaxClassID"]));
                tblTaxClass.TaxName = txtTaxClass.Text.Trim();
                tblTaxClass.TaxCode = txtTaxCode.Text.Trim();
                if (!string.IsNullOrEmpty(txtDisplayOrder.Text.Trim()))
                {
                    tblTaxClass.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
                }
                else
                    tblTaxClass.DisplayOrder = null;
                tblTaxClass.Deleted = false;
                tblTaxClass.UpdatedOn = DateTime.Now;
                tblTaxClass.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                Int32 isupdated = objTaxClass.UpdateTaxClass(tblTaxClass);
                if (isupdated > 0)
                {
                    Response.Redirect("TaxClassList.aspx?status=updated");
                }
            }
            else
            {
                int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                tblTaxClass.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                tblTaxClass.TaxName = txtTaxClass.Text.Trim();
                tblTaxClass.TaxCode = txtTaxCode.Text.Trim();
                if (objTaxClass.CheckDuplicate(tblTaxClass))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('TaxClass with same name or code already exists, please specify another name or code...', 'Message','');});", true);
                    return;
                }

                if (!string.IsNullOrEmpty(txtDisplayOrder.Text.Trim()))
                {
                    tblTaxClass.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
                }
                else
                    tblTaxClass.DisplayOrder = null;
                tblTaxClass.CreatedOn = DateTime.Now;
                tblTaxClass.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                tblTaxClass.Deleted = false;

                Int32 isadded = objTaxClass.Createtaxclass(tblTaxClass);
                if (isadded > 0)
                {
                    Response.Redirect("TaxClassList.aspx?status=inserted");
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
            Response.Redirect("TaxClassList.aspx");
        }
    }
}