using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class LowInventory : BasePage
    {
        DataSet dsCategory = new DataSet();
        int storeID = 1;

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
                FillCategoryDropDown(Convert.ToInt32(ddlStore.SelectedValue.ToString()));
                BindLowInventoryReport();

            }
            btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; padding-right:0px; height: 23px; border:none;cursor:pointer;");

        }

        /// <summary>
        ///  List Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdLowInventoryReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLowInventoryReport.PageIndex = e.NewPageIndex;
            FillLowInventoryReport();
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
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            if (!String.IsNullOrEmpty(Request.QueryString["storeid"]))
            {
                ddlStore.SelectedValue = Convert.ToString(Request.QueryString["storeid"]);

            }
        }

        /// <summary>
        /// Fills the Low Inventory Report
        /// </summary>
        private void FillLowInventoryReport()
        {
            DataSet dsLowInventory = new DataSet();
            dsLowInventory = DashboardComponent.GetLowInventoryForReport(Convert.ToInt32(ddlStore.SelectedValue.ToString()));

            if (dsLowInventory != null && dsLowInventory.Tables.Count > 0 && dsLowInventory.Tables[0].Rows.Count > 0)
            {
                grdLowInventoryReport.DataSource = dsLowInventory.Tables[0];
                grdLowInventoryReport.DataBind();
            }
            else
            {
                grdLowInventoryReport.DataSource = null;
                grdLowInventoryReport.DataBind();
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedIndex > 0)
            {
                ddlCategory.Enabled = true;
            }
            else
            {
                ddlCategory.Enabled = false;
            }
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }

            FillCategoryDropDown(Convert.ToInt32(ddlStore.SelectedValue.ToString()));
            BindLowInventoryReport();
        }

        /// <summary>
        /// Category Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindLowInventoryReport();
        }

        /// <summary>
        /// Fills the Category Dropdown
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        private void FillCategoryDropDown(int StoreID)
        {
            ddlCategory.Items.Clear();
            if (ddlStore.SelectedIndex == 0)
            {
                return;
            }
            else
            {
                dsCategory = CategoryComponent.GetCategoryByStoreID(StoreID, 3); //Option 3: To display category order by Display order
            }

            int count = 1;
            ListItem LT2 = new ListItem();
            DataRow[] drCatagories = null;
            if (ddlStore.SelectedIndex > 0)
            {
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=0 and StoreID=" + Convert.ToInt32(ddlStore.SelectedValue.ToString()), "DisplayOrder");
            }
            else
            {
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=0");
            }

            if (dsCategory != null && drCatagories.Length > 0)
            {
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = "...|" + count + "|" + selDR["Name"].ToString();
                    LT2.Value = selDR["CategoryID"].ToString();
                    ddlCategory.Items.Add(LT2);
                    SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), count);
                }
            }
            ddlCategory.Items.Insert(0, new ListItem("Root Category", "0"));
        }

        /// <summary>
        /// Set Child category
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="Number">int Number</param>
        private void SetChildCategory(int ID, int Number)
        {
            int count = Number;
            string st = "...";
            for (int i = 0; i < count; i++)
            {
                st += st;
            }
            DataRow[] drCatagories = null;
            if (storeID == 0)
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=" + ID.ToString());
            else
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=" + ID.ToString() + " and StoreID=" + storeID);
            ListItem LT2;
            int innercount = 0;
            if (drCatagories.Length > 0)
            {
                innercount++;
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = st + "|" + (count + 1) + "|" + selDR["Name"].ToString();
                    LT2.Value = selDR["CategoryID"].ToString();
                    ddlCategory.Items.Add(LT2);
                    SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), innercount + Number);
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            BindLowInventoryReport();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Enabled = false;
            ddlStore.SelectedIndex = 0;
            ddlSearch.SelectedIndex = 0;
            txtSearch.Text = "";
            BindLowInventoryReport();
        }

        /// <summary>
        /// Binds the Low Inventory Report
        /// </summary>
        private void BindLowInventoryReport()
        {
            int categoryid = 0;
            DataSet dsLowInventory = new DataSet();
            DashboardComponent objDashboardComp = new DashboardComponent();
            int.TryParse(ddlCategory.SelectedValue.ToString(), out categoryid);
            dsLowInventory = objDashboardComp.SearchLowInventoryProduct(categoryid, Convert.ToInt32(ddlStore.SelectedValue.ToString()), ddlSearch.SelectedValue.ToString(), txtSearch.Text.Trim().ToString());

            if (dsLowInventory != null && dsLowInventory.Tables.Count > 0 && dsLowInventory.Tables[0].Rows.Count > 0)
            {
                grdLowInventoryReport.DataSource = dsLowInventory.Tables[0];
                grdLowInventoryReport.DataBind();
            }
            else
            {
                grdLowInventoryReport.DataSource = null;
                grdLowInventoryReport.DataBind();
            }
        }
    }
}