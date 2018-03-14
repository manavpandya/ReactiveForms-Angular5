using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.Data;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class TradeTemplateList : Solution.UI.Web.BasePage
    {

        #region Declaration

        CouponComponent couponcomp = new CouponComponent();
        StoreComponent stac = new StoreComponent();
        public static bool isDescendcoupon = false;
        public static bool isDescendStore = false;
        public static bool isDescendexpiredate = false;
        public static bool isDescenddiscount = false;
        public static bool isDescenddiscountamt = false;
        public static bool isDescendstartdate = false;


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
                isDescendcoupon = false;
                isDescendStore = false;
                isDescendexpiredate = false;
                isDescenddiscount = false;
                isDescenddiscountamt = false;
                isDescendstartdate = false;
                BindStore();
                BindTradeTemplateList();
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnshowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btndelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Trade Template Details inserted successfully.', 'Message');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Trade Template Details updated successfully.', 'Message');});", true);
                    }
                }
                int loginid = 0;
                if (Request.QueryString["loginid"] != "" && Request.QueryString["loginid"] != null)
                {
                    loginid = Convert.ToInt32(Request.QueryString["loginid"]);

                    hdnLoginID.Text = Convert.ToString(loginid);

                }
            }
        }

        protected void gridtradetemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int couponid = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("TradeCustomerTemplate.aspx?Mode=edit&TempID=" + couponid);
            }
        }

        protected void gridtradetemplate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gridtradetemplate.Rows.Count > 0)
                trBottom.Visible = true;
            else
                trBottom.Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lnkedit = (ImageButton)e.Row.FindControl("lnkbtnedit");
                lnkedit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }
        }


        private void BindTradeTemplateList()
        {
            DataSet dsevents = new DataSet();
            dsevents = CommonComponent.GetCommonDataSet("select TradeTemplateID,isnull(TradeTempName,'') as TradeTempName,case when isnull(Active,0)=1 then 'Active' else 'Inactive' end as Active from tb_TradeTempMaster  where isnull(deleted,0)=0  order by createdon desc");
            if (dsevents != null && dsevents.Tables.Count > 0 && dsevents.Tables[0].Rows.Count > 0)
            {
                gridtradetemplate.DataSource = dsevents.Tables[0];
                gridtradetemplate.DataBind();
            }
            else
            {
                gridtradetemplate.DataSource = null;
                gridtradetemplate.DataBind();
            }
        }
        /// <summary>
        /// Binds the Store Drop down
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
            }
            else
                ddlstore.SelectedIndex = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["Storeid"]))
            {

                if (Convert.ToInt32(Request.QueryString["Storeid"].ToString()) > 0)
                {
                    ddlstore.SelectedValue = (Request.QueryString["Storeid"].ToString());
                }
                else
                {
                    ddlstore.SelectedIndex = 0;
                }

            }
        }


        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlstore.SelectedValue.ToString() == "0" || ddlstore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue.ToString());
            }
            AppConfig.StoreID = 1;
            gridtradetemplate.PageIndex = 0;
            gridtradetemplate.DataBind();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataSet dsevents = new DataSet();
            dsevents = CommonComponent.GetCommonDataSet("select * from tb_TradeTempMaster where TradeTempName like '%" + txtSearch.Text + "%' and isnull(deleted,0)=0 ");
            if (dsevents != null && dsevents.Tables.Count > 0 && dsevents.Tables[0].Rows.Count > 0)
            {
                gridtradetemplate.DataSource = dsevents.Tables[0];
                gridtradetemplate.DataBind();
            }
            else
            {
                gridtradetemplate.DataSource = null;
                gridtradetemplate.DataBind();
            }
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnshowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindTradeTemplateList();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndelete_Click(object sender, EventArgs e)
        {
            int totalRowCount = gridtradetemplate.Rows.Count;
            for (int tt = 0; tt < totalRowCount; tt++)
            {
                HiddenField hdn = (HiddenField)gridtradetemplate.Rows[tt].FindControl("hdnTradeTemplateID");
                CheckBox chk = (CheckBox)gridtradetemplate.Rows[tt].FindControl("chkselect");
                if (chk.Checked == true)
                {

                    CommonComponent.ExecuteCommonData("Delete from  tb_MembershipDiscount where MembershipDiscountID in (select MembershipDiscountID from tb_MembershipDiscount inner join tb_TradeTemplateDetail on tb_MembershipDiscount.DiscountObjectID=tb_TradeTemplateDetail.DiscountObjectID where tb_MembershipDiscount.DiscountType=tb_TradeTemplateDetail.DiscountType and custid in (select CustomerID from tb_Customer where isnull(TradeTemplateID,0)=" + hdn.Value.ToString() + "))");
                    CommonComponent.ExecuteCommonData("update tb_Customer set TradeTemplateID=0 where TradeTemplateID=" + hdn.Value.ToString() + "");
                    CommonComponent.ExecuteCommonData("Delete from tb_TradeTemplateDetail where TradeTemplateID=" + hdn.Value.ToString() + "");
                    CommonComponent.ExecuteCommonData("update tb_TradeTempMaster set Deleted=1 where TradeTemplateID=" + hdn.Value.ToString() + "");


                }
            }
            BindTradeTemplateList();
        }


    }
}