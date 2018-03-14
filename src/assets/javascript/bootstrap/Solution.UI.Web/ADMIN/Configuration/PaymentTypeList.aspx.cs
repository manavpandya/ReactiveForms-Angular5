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
    public partial class PaymentTypeList : Solution.UI.Web.BasePage
    {
        #region Declaration

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
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Payment Option inserted successfully.', 'Message');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Payment Option updated successfully.', 'Message');});", true);
                    }
                }
            }
        }

        /// <summary>
        /// Method for set Image is active or not
        /// </summary>
        /// <param name="_Active">(bool _Active</param>
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
        /// Binds the Store Dropdown
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                drpstore.DataSource = Storelist;
                drpstore.DataTextField = "StoreName";
                drpstore.DataValueField = "StoreID";
            }
            else
            {
                drpstore.DataSource = null;
            }
            drpstore.DataBind();
            drpstore.Items.Insert(0, new ListItem("All Store", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                drpstore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdpayment.PageIndex = 0;
            grdpayment.DataBind();
        }

        /// <summary>
        ///  Payment Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void _paymentgridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdpayment.Rows.Count > 0)
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
        /// Payment Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void _paymentgridview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deletepayment")
            {
                PaymentComponent paycomp = new PaymentComponent();
                tb_Payment pay = null;
                int paymnentId = Convert.ToInt32(e.CommandArgument);
                pay = paycomp.getPaymentType(paymnentId);
                pay.Deleted = true;
                paycomp.Deletepaymenttype(pay);
                grdpayment.DataBind();
            }
            else if (e.CommandName == "edit")
            {
                int paymnentId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("PaymentType.aspx?PaymentID=" + paymnentId);
            }
        }

        /// <summary>
        ///  Grid view Sorting  Event
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
                    grdpayment.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
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
                    grdpayment.Sort(lb.CommandName.ToString(), SortDirection.Descending);
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
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void drpstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdpayment.PageIndex = 0;
            grdpayment.DataBind();
        }

        /// <summary>
        ///  ShowAll Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnshowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            drpstore.SelectedIndex = 0;
            grdpayment.PageIndex = 0;
            grdpayment.DataBind();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndelete_Click(object sender, EventArgs e)
        {
            PaymentComponent objpaycomp = new PaymentComponent();
            int totalRowCount = grdpayment.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdpayment.Rows[i].FindControl("hdnpaymentid");
                CheckBox chk = (CheckBox)grdpayment.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    tb_Payment objpay = null;
                    objpay = objpaycomp.getPaymentType(Convert.ToInt16(hdn.Value));
                    objpay.Deleted = true;
                    objpaycomp.Deletepaymenttype(objpay);
                }
            }
            grdpayment.DataBind();
        }
    }
}