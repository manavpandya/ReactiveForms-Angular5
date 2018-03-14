using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class FailedTransaction : Solution.UI.Web.BasePage
    {
        StoreComponent stac = new StoreComponent();
        OrderComponent ordercomp = new OrderComponent();

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                DateTime todaydate = DateTime.Now;


                txtMailFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(todaydate.AddMonths(-1)));
                txtMailTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnTopSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btndelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                BindStore();
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
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue);
            }

        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndelete_Click(object sender, EventArgs e)
        {
            int totalRowCount = grdfailedtransaction.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdfailedtransaction.Rows[i].FindControl("hdntransactionid");
                CheckBox chk = (CheckBox)grdfailedtransaction.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    ordercomp.DeleteFailedTransaction(Convert.ToInt32(hdn.Value));
                }
            }
            grdfailedtransaction.DataBind();
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlstore.SelectedValue.ToString() == "0")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue.ToString());
            }
            grdfailedtransaction.PageIndex = 0;
            grdfailedtransaction.DataBind();

        }

        /// <summary>
        /// Failed Transaction Gridview  Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void grdfailedtransaction_DataBound(object sender, EventArgs e)
        {
            if (grdfailedtransaction.Rows.Count > 0)
            {
                trBottom.Visible = true;
                btnTopSave.Visible = true;
            }
            else
            {
                trBottom.Visible = false;
                btnTopSave.Visible = false;
              
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (grdfailedtransaction.Rows.Count > 0)
            {
                foreach (GridViewRow row in grdfailedtransaction.Rows)
                {
                    CheckBox chkAlert = ((CheckBox)row.FindControl("chkalert"));
                    TextBox txtNote = (TextBox)row.FindControl("txtNote");
                    HiddenField hdntransactionid = (HiddenField)row.FindControl("hdntransactionid");
                    CommonComponent.ExecuteCommonData("Update tb_FailedTransaction set FailedTxnNote='" + txtNote.Text.ToString().Trim().Replace("'", "''") + "',IsEmailAlert='" + chkAlert.Checked + "' where TransactionID=" + hdntransactionid.Value + "");
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@testmsg", "alert('Record(s) Updated Successfully !');", true);
                grdfailedtransaction.DataBind();
            }
        }

        /// <summary>
        /// Failed Transaction Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdfailedtransaction_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lborderno = (Label)e.Row.FindControl("lborderno");
                System.Web.UI.HtmlControls.HtmlAnchor lnkFailedOrder = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lnkFailedOrder");
                if (lborderno != null)
                {
                    //string strScript = "var tt = urlencode('" + SecurityComponent.Encrypt(lborderno.Text.ToString()) + "');";
                    //lnkFailedOrder.Attributes.Add("onclick", strScript + "OpenCenterWindow('ViewFailedTransaction.aspx?ONo='+tt,900,600);");

                    lnkFailedOrder.Attributes.Add("onclick", "OpenCenterWindow('ViewFailedTransaction.aspx?ONo=" + lborderno.Text.ToString() + "',900,600);");
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdfailedtransaction.PageIndex = 0;
            grdfailedtransaction.DataBind();

        }
    }
}