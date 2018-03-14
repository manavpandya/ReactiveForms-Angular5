using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Configuration
{
    public partial class CreditcardTypeList : Solution.UI.Web.BasePage
    {
        #region Declaration

        tb_CreditCardTypes credit = null;
        CreditCardComponent creditcomp = new CreditCardComponent();
        StoreComponent stac = new StoreComponent();
        public static bool isDescendName = false;
        public static bool isDescendStore = false;

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
                isDescendName = false;
                isDescendStore = false;
                BindStore();
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnshowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btndelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Credit Card Type inserted successfully.', 'Message');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Credit Card Type updated successfully.', 'Message');});", true);
                    }
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
        /// Credit Card Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdcreditcard_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdcreditcard.Rows.Count > 0)
                trBottom.Visible = true;
            else
                trBottom.Visible = false;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendName == false)
                {
                    ImageButton lbName = (ImageButton)e.Row.FindControl("lbName");
                    lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbName.AlternateText = "Ascending Order";
                    lbName.ToolTip = "Ascending Order";
                    lbName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbName = (ImageButton)e.Row.FindControl("lbName");
                    lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbName.AlternateText = "Descending Order";
                    lbName.ToolTip = "Descending Order";
                    lbName.CommandArgument = "ASC";
                }
                if (isDescendStore == false)
                {
                    ImageButton lbstore = (ImageButton)e.Row.FindControl("lbstore");
                    lbstore.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbstore.AlternateText = "Ascending Order";
                    lbstore.ToolTip = "Ascending Order";
                    lbstore.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbstore = (ImageButton)e.Row.FindControl("lbstore");
                    lbstore.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbstore.AlternateText = "Descending Order";
                    lbstore.ToolTip = "Descending Order";
                    lbstore.CommandArgument = "ASC";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lnkedit = (ImageButton)e.Row.FindControl("_editLinkButton");
                lnkedit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                ImageButton lnkdelete = (ImageButton)e.Row.FindControl("_deleteLinkButton");
                lnkdelete.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delete.gif";
            }
        }

        /// <summary>
        /// Method for set Image is active or not
        /// </summary>
        /// <param name="_Active">bool _Active</param>
        /// <returns>Returns the Image Path</returns>
        public string SetImage(bool _Active)
        {
            string _ReturnUrl;
            if (_Active)
            {
                _ReturnUrl = "../Images/active.gif";

            }
            else
            {
                _ReturnUrl = "../Images/in-active.gif";

            }
            return _ReturnUrl;
        }

        /// <summary>
        /// Credit Card Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdcreditcard_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCredit")
            {
                int creditcardid = Convert.ToInt32(e.CommandArgument);
                credit = creditcomp.GetcreditcardType(creditcardid);
                credit.Deleted = true;
                creditcomp.Deletecredittypetype(credit);
                grdcreditcard.DataBind();
            }
            else if (e.CommandName == "edit")
            {
                int creditcardid = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("CreditCardType.aspx?CardTypeID=" + creditcardid);
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdcreditcard.PageIndex = 0;
            grdcreditcard.DataBind();

            if (ddlstore.SelectedValue.ToString() == "0")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue.ToString());
            }

        }

        /// <summary>
        /// Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdcreditcard.PageIndex = 0;
            grdcreditcard.DataBind();
        }

        /// <summary>
        ///  ShowAll Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnshowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlstore.SelectedIndex = 0;
            grdcreditcard.PageIndex = 0;
            grdcreditcard.DataBind();
        }

        /// <summary>
        ///  Grid view Sorting Function
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    grdcreditcard.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = false;
                    }
                    else
                    {
                        isDescendStore = false;
                    }
                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdcreditcard.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = true;
                    }
                    else
                    {
                        isDescendStore = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndelete_Click(object sender, EventArgs e)
        {
            int totalRowCount = grdcreditcard.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdcreditcard.Rows[i].FindControl("hdncreditcardid");
                CheckBox chk = (CheckBox)grdcreditcard.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    tb_CreditCardTypes tblcredit = null;
                    tblcredit = creditcomp.GetcreditcardType(Convert.ToInt16(hdn.Value));
                    tblcredit.Deleted = true;
                    creditcomp.Deletecredittypetype(tblcredit);
                }
            }
            grdcreditcard.DataBind();
        }
    }
}