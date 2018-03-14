using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class HeaderLink : Solution.UI.Web.BasePage
    {
        int StoreID = 0;
        StoreComponent stac = new StoreComponent();
        tb_HeaderLinks tb_header = new tb_HeaderLinks();
        HeaderlinkComponent headercomp = new HeaderlinkComponent();

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
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                if (!string.IsNullOrEmpty(Request.QueryString["HeaderLinkID"]) && Convert.ToString(Request.QueryString["HeaderLinkID"]) != "0")
                {
                    FillHeaderLink(Convert.ToInt32(Request.QueryString["HeaderLinkID"]));
                    lblTitle.Text = "Edit Header Link";
                    lblTitle.ToolTip = "Edit Header Link";
                }
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                ddlstore.DataSource = Storelist;
                ddlstore.DataTextField = "StoreName";
                ddlstore.DataValueField = "StoreID";
            }
            else
            {
                ddlstore.DataSource = null;
            }
            ddlstore.DataBind();
            ddlstore.Items.Insert(0, new ListItem("All Store", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlstore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue.ToString());
            }
            else
                AppConfig.StoreID = 1;

        }

        /// <summary>
        /// Fills the Header Detail
        /// </summary>
        /// <param name="headerid">int headerid</param>
        private void FillHeaderLink(int headerid)
        {
            tb_header = headercomp.GetHeaderLink(headerid);
            txtheadername.Text = tb_header.PageName;
            txtheaderlink.Text = tb_header.PageURL;
            txtdisplayorder.Text = tb_header.DisplayOrder.ToString();
            StoreID = tb_header.tb_StoreReference.Value.StoreID;
            ddlstore.SelectedValue = StoreID.ToString();
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["HeaderLinkID"]) && Convert.ToString(Request.QueryString["HeaderLinkID"]) != "0")
            {
                tb_header = headercomp.GetHeaderLink(Convert.ToInt32(Request.QueryString["HeaderLinkID"]));
                tb_header.PageName = txtheadername.Text.Trim();
                tb_header.PageURL = txtheaderlink.Text.Trim();
                tb_header.DisplayOrder = Convert.ToInt32(txtdisplayorder.Text.Trim());
                tb_header.Type = "IndexPage";
                int StoreID1 = Convert.ToInt32(ddlstore.SelectedItem.Value.ToString());
                tb_header.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreID1);
                Int32 isupdated = headercomp.UpdateHeaderLink(tb_header);
                if (isupdated > 0)
                {
                    Response.Redirect("HeaderLinkList.aspx?status=updated");
                }
            }
            else
            {//
                int StoreID1 = Convert.ToInt32(ddlstore.SelectedItem.Value.ToString());
                tb_header.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreID1);
                //
                tb_header.PageName = txtheadername.Text.Trim();
                tb_header.PageURL = txtheaderlink.Text.Trim();
                tb_header.DisplayOrder = Convert.ToInt32(txtdisplayorder.Text);
                tb_header.Type = "IndexPage";
                if (headercomp.CheckDuplicateforHeaderLink(tb_header))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Header Link with same name or code already exists, please specify another name..', 'Message');});", true);
                    return;
                }
                //int StoreID1 = Convert.ToInt32(ddlstore.SelectedItem.Value.ToString());
                //tb_header.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreID1);
                Int32 isadded = headercomp.CreateHeaderLink(tb_header);
                if (isadded > 0)
                {
                    Response.Redirect("HeaderLinkList.aspx?status=inserted");
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
            Response.Redirect("HeaderLinkList.aspx");
        }
    }
}