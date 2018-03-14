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


namespace Solution.UI.Web.ADMIN.Configuration
{
    public partial class ShippingMethodList : BasePage
    {
        #region Declaration
        ShippingComponent objShippingMethod = new ShippingComponent();
        tb_ShippingMethods tblShippingMethod = new tb_ShippingMethods();
        public static bool isDescendName = false;
        public static bool isDescendstname = false;
        private static DataSet DS = new DataSet();
        string StoreID;
        int ShippingServiceId = 0;
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
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/search.gif";
                ibtnShowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/showall.png";
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                String strStatus = Convert.ToString(Request.QueryString["status"]);
                if (strStatus == "inserted")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Shipping Method inserted successfully.', 'Message');});", true);
                }
                else if (strStatus == "updated")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Shipping Method updated successfully.', 'Message');});", true);
                }
                bindstore();
            }
        }

        /// <summary>
        /// Bind Store 
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
                StoreID = ddlStore.SelectedValue.ToString();
                BindShippingService(StoreID);
            }
            else
            {
                ddlStore.Items.Insert(0, new ListItem("Select Store", "-1"));
                ddlShippingService.Items.Insert(0, new ListItem("All Shipping Service", "-1"));
            }
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
        }

        /// <summary>
        /// Sorting function For Grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    gvShippingMethods.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = false;
                    }
                    else if (lb.ID == "lbstname")
                    {
                        isDescendstname = false;
                    }
                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    gvShippingMethods.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = true;
                    }
                    else if (lb.ID == "lbstname")
                    {
                        isDescendstname = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Shipping Method Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvShippingMethods_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvShippingMethods.Rows.Count > 0)
            {
                data.Style["display"] = "block";
            }
            else
            {
                data.Style["display"] = "none";
            }
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
                if (isDescendstname == false)
                {
                    ImageButton lbstname = (ImageButton)e.Row.FindControl("lbstname");
                    lbstname.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbstname.AlternateText = "Ascending Order";
                    lbstname.ToolTip = "Ascending Order";
                    lbstname.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbstname = (ImageButton)e.Row.FindControl("lbstname");
                    lbstname.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbstname.AlternateText = "Descending Order";
                    lbstname.ToolTip = "Descending Order";
                    lbstname.CommandArgument = "ASC";
                }

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton _editLinkButton = (ImageButton)e.Row.FindControl("_editLinkButton");
                _editLinkButton.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }
        }

        /// <summary>
        ///  Go Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            gvShippingMethods.PageIndex = 0;
            gvShippingMethods.DataBind();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int totalRowCount = gvShippingMethods.Rows.Count;
            this.Response.Redirect("ShippingMethodList.aspx", false);
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)gvShippingMethods.Rows[i].FindControl("hdnShippingMethodID");
                CheckBox chk = (CheckBox)gvShippingMethods.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    tblShippingMethod = null;
                    tblShippingMethod = objShippingMethod.GetShippingMethod(Convert.ToInt16(hdn.Value));
                    tblShippingMethod.Deleted = true;
                    objShippingMethod.DeleteShippingMethod(tblShippingMethod);
                }
            }
        }

        /// <summary>
        ///  DeleAhow All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
            ddlStore.SelectedIndex = 0;
            ddlShippingService.SelectedIndex = 0;
            gvShippingMethods.PageIndex = 0;
            gvShippingMethods.DataBind();


        }

        /// <summary>
        /// Shipping Method Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvShippingMethods_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "edit")
            {
                try
                {
                    int ShippingMethodID = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("ShippingMethod.aspx?ShippingMethodID=" + ShippingMethodID);
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            StoreID = ddlStore.SelectedValue.ToString();
            BindShippingService(StoreID);

            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }

        }

        /// <summary>
        /// Bind Shipping Service
        /// </summary>
        /// <param name="storeID">string StoreID</param>
        protected void BindShippingService(string storeID)
        {
            ddlShippingService.Items.Clear();
            objShippingMethod = new ShippingComponent();

            DS = objShippingMethod.SelectShippingService(Convert.ToInt32(storeID));
            if (DS != null && DS.Tables[0] != null && DS.Tables[0].Rows.Count > 0)
            {
                ddlShippingService.DataSource = DS;
                ddlShippingService.DataTextField = "ShippingService";
                ddlShippingService.DataValueField = "ShippingServiceID";
                ddlShippingService.DataBind();
            }
            ddlShippingService.Items.Insert(0, new ListItem("All Shipping Service", "-1"));
            ddlShippingService.SelectedIndex = 0;
        }

        /// <summary>
        /// Shipping Service Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlShippingService_SelectedIndexChanged(object sender, EventArgs e)
        {

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

    }
}