using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class Store : Solution.UI.Web.BasePage
    {
        # region Variables and Property
        tb_Store tbStore = null;
        StoreComponent objStoreComp = new StoreComponent();
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
                if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]) && Convert.ToString(Request.QueryString["StoreID"]) != "0")
                {
                    tbStore = new tb_Store();
                    //Display selected store data for edit mode
                    tbStore = objStoreComp.getStore(Convert.ToInt32(Request.QueryString["StoreID"]));
                    txtStoreName.Text = tbStore.StoreName;
                    lblHeader.Text = "Edit Store Configuration";
                }
            }
        }

        /// <summary>
        /// Insert -Update store detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            tbStore = new tb_Store();
            if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]) && Convert.ToString(Request.QueryString["StoreID"]) != "0")
            {
                //Update store config detail
                tbStore = objStoreComp.getStore(Convert.ToInt32(Request.QueryString["StoreID"]));
                tbStore.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                tbStore.UpdatedOn = DateTime.Now;
                tbStore.StoreName = txtStoreName.Text.Trim();
                //Check modified store name is exists or duplicate
                if (objStoreComp.CheckDuplicate(tbStore))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Store Name already exists.', 'Message','');});", true);
                    return;
                }
                Int32 isupdated = objStoreComp.UpdateStoreConfiguration(tbStore);
                if (isupdated > 0)
                {
                    Response.Redirect("StoreList.aspx?status=updated");
                }
            }
            else
            {
                //insert new store config
                tbStore.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                tbStore.CreatedOn = DateTime.Now;
                tbStore.StoreName = txtStoreName.Text.Trim();
                tbStore.Deleted = false;
                if (objStoreComp.CheckDuplicate(tbStore))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Store Name already exists.', 'Message','');});", true);
                    return;
                }
                Int32 isadded = objStoreComp.CreateStoreConfiguration(tbStore);
                if (isadded > 0)
                {
                    Response.Redirect("StoreList.aspx?status=inserted");
                }

            }
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("StoreList.aspx");
        }
    }
}