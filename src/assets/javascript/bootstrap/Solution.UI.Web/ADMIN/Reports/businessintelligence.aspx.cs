using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN
{
    public partial class businessintelligence : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStore();
            }
        }

        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdminComponent.BIStoreID = Convert.ToInt32(ddlStore.SelectedValue);
            Response.Redirect(Request.RawUrl);
        }

        /// <summary>
        /// Bind All Stores in Drop down
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
                ddlStore.SelectedValue = Convert.ToString(AdminComponent.BIStoreID);
                ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
                AppConfig.StoreID = 1;
            }
            else
            {
                ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
                AppConfig.StoreID = 1;
                ddlStore.SelectedIndex = 0;
            }
        }
    }
}