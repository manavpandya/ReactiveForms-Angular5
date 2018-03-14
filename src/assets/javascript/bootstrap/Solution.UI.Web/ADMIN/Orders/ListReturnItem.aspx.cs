using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ListReturnItem : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnDeleteMultiple.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
            if (!IsPostBack)
            {
                bindstore();
                BindData();
            }
        }

        /// <summary>
        /// Binds the data of Returns Items List
        /// </summary>
        private void BindData()
        {
            RMAComponent objRMA = new RMAComponent();
            DataSet dsRma = new DataSet();
            string strlike = "";
            strlike = "";
            if (ddlStore.SelectedIndex == 0)
            {
                strlike = " WHERE CustomerName like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'";
                dsRma = objRMA.GetAllReturnItem(strlike);
            }
            else
            {
                strlike = " AND CustomerName like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'";
                dsRma = objRMA.GetAllReturnItem(ddlStore.SelectedValue.ToString(), strlike);
            }

            if (dsRma != null && dsRma.Tables.Count > 0 && dsRma.Tables[0].Rows.Count > 0)
            {

                grdReturnItemList.DataSource = dsRma;
                grdReturnItemList.DataBind();

            }
            else
            {
                grdReturnItemList.DataSource = null;
                grdReturnItemList.DataBind();

            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdReturnItemList.PageIndex = 0;
            BindData();
        }

        /// <summary>
        /// Bind All Stores in Drop down
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
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            try
            {
                if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) > 0)
                {
                    ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]);
                }
                else
                {
                    ddlStore.SelectedIndex = 0;
                }
            }
            catch
            {
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Card Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdReturnItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView Row = (DataRowView)e.Row.DataItem;

                HyperLink hlk = (HyperLink)e.Row.FindControl("hlEdit");
                if (hlk != null)
                {
                    hlk.NavigateUrl = hlk.NavigateUrl + "&StoreID=" + Row["StoreID"].ToString();
                }


                System.Web.UI.HtmlControls.HtmlAnchor lkOrderNumber = e.Row.FindControl("lkOrderNumber") as System.Web.UI.HtmlControls.HtmlAnchor;
                System.Web.UI.WebControls.HiddenField hdOrderNumber = e.Row.FindControl("hdOrderNumber") as System.Web.UI.WebControls.HiddenField;
                System.Web.UI.WebControls.HiddenField hdReturnType = e.Row.FindControl("hdReturnType") as System.Web.UI.WebControls.HiddenField;
                System.Web.UI.WebControls.HiddenField hdStoreName = e.Row.FindControl("hdStoreName") as System.Web.UI.WebControls.HiddenField;
                System.Web.UI.WebControls.HiddenField hdnisReturnrequest = e.Row.FindControl("hdnisReturnrequest") as System.Web.UI.WebControls.HiddenField;
                Label lblReturnStatus = (Label)e.Row.FindControl("lblReturnStatus");
                System.Web.UI.HtmlControls.HtmlImage imagstatus = e.Row.FindControl("imagstatus") as System.Web.UI.HtmlControls.HtmlImage;

                if (lkOrderNumber != null)
                {
                    if (hdnisReturnrequest.Value.ToString() == "True")
                    {
                        if (hdReturnType.Value.ToString().Trim().ToLower() == "rr")
                            lkOrderNumber.HRef = "/Admin/Orders/orders.aspx?id=" + hdOrderNumber.Value + "&refund=1";
                        else
                            lkOrderNumber.HRef = "/Admin/Orders/orders.aspx?id=" + hdOrderNumber.Value + "&return=1";
                    }
                    else
                    {
                        //if (hdReturnType.Value.ToString().Trim().ToLower() == "rr" && !(hdStoreName.Value.ToString().ToLower().Contains("ebay") || hdStoreName.Value.ToString().ToLower().Contains("amazon")))
                        //    lkOrderNumber.HRef = "/Admin/OrderManagement/orders.aspx?ONo=" + hdOrderNumber.Value + "";
                        //else
                        lkOrderNumber.HRef = "/Admin/Orders/orders.aspx?id=" + hdOrderNumber.Value + "&fromreturn=1";
                    }
                }
                if (lblReturnStatus != null)
                {
                    if (Convert.ToBoolean(lblReturnStatus.Text.ToString()))
                    {
                        imagstatus.Src = "/Admin/images/isActive.png";
                    }
                    else
                    {
                        imagstatus.Src = "/Admin/images/isInactive.png";
                    }
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            grdReturnItemList.PageIndex = 0;
            BindData();
        }

        /// <summary>
        ///  Search All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
            grdReturnItemList.PageIndex = 0;
            BindData();
        }

        /// <summary>
        ///  Delete Multiple Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDeleteMultiple_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (GridViewRow row in grdReturnItemList.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                if (chk.Checked)
                {
                    Label lblID = (Label)row.FindControl("lblID");
                    CommonComponent.ExecuteCommonData("Update tb_return set Deleted=1 where returnid=" + lblID.Text.ToString() + "");
                    count++;
                }
            }
            if (count > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "MsgDelete", "jAlert('Record(s) Deleted Successfully.', 'Message', '');", true);
                BindData();
            }
        }
    }
}
