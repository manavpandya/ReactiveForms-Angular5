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
    public partial class CreditCardType : Solution.UI.Web.BasePage
    {
        #region Declaration

        StoreComponent stac = new StoreComponent();
        tb_CreditCardTypes tblcredit = new tb_CreditCardTypes();
        CreditCardComponent creditcomp = new CreditCardComponent();
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
                BindStore();
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                if (!string.IsNullOrEmpty(Request.QueryString["CardTypeID"]) && Convert.ToString(Request.QueryString["CardTypeID"]) != "0")
                {
                    FillCreditCardType(Convert.ToInt32(Request.QueryString["CardTypeID"]));
                    lblTitle.Text = "Edit Credit Card Type";
                    lblTitle.ToolTip = "Edit Credit Card Type";
                }
            }
        }

        /// <summary>
        /// Binds the Store Dropdown
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                ddlStore.DataSource = Storelist;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
            }
            else
            {
                ddlStore.DataSource = null;
            }
            ddlStore.DataBind();
            ddlStore.Items.Insert(0, new ListItem("All Store", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
        }

        /// <summary>
        /// Fills the Credit Card Detail
        /// </summary>
        /// <param name="creditcardid">int CreditcardID</param>
        private void FillCreditCardType(int creditcardid)
        {
            tblcredit = creditcomp.GetcreditcardType(creditcardid);
            ddlStore.Enabled = false;
            txtcredit.Text = tblcredit.CardType;
            txtcode.Text = (tblcredit.CardTypeID).ToString(); ;
            if (tblcredit.Active == true)
            {
                chkststus.Checked = true;
            }
            else
                chkststus.Checked = false;
            StoreId = tblcredit.tb_StoreReference.Value.StoreID;
            ddlStore.SelectedValue = StoreId.ToString();
        }

        /// <summary>
        /// Save Button Click Event
        /// </summary>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["CardTypeID"]) && Convert.ToString(Request.QueryString["CardTypeID"]) != "0")
            {
                tblcredit = creditcomp.GetcreditcardType(Convert.ToInt32(Request.QueryString["CardTypeID"]));
                tblcredit.CardType = txtcredit.Text.Trim();
                if (chkststus.Checked)
                {
                    tblcredit.Active = true;
                }
                else
                    tblcredit.Active = false;
                tblcredit.Deleted = false;
                tblcredit.UpdatedOn = DateTime.Now;
                tblcredit.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                Int32 isupdated = creditcomp.Updatecredittypetype(tblcredit);
                if (isupdated > 0)
                {
                    Response.Redirect("CreditcardTypeList.aspx?status=updated");
                }
            }
            else
            {
                tblcredit.CardType = txtcredit.Text.Trim();
                tblcredit.CardTypeID = Convert.ToInt32(txtcode.Text.Trim());
                int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                tblcredit.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                if (creditcomp.CheckDuplicate(tblcredit))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Credit Card Type with same name or code already exists, please specify another name or code...', 'Message');});", true);
                    return;
                }
                if (chkststus.Checked)
                {
                    tblcredit.Active = true;
                }
                else
                    tblcredit.Active = false;
                tblcredit.CreatedOn = DateTime.Now;
                tblcredit.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                tblcredit.Deleted = false;
                Int32 isadded = creditcomp.Createcreditcardtype(tblcredit);
                if (isadded > 0)
                {
                    Response.Redirect("CreditcardTypeList.aspx?status=inserted");
                }
            }
        }

        /// <summary>
        /// Cancel Button Click Event
        /// </summary>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("CreditcardTypeList.aspx");
        }
    }
}