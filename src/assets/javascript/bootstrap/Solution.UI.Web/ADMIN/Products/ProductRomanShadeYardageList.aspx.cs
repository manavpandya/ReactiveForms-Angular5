using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductRomanShadeYardageList : BasePage
    {
        /// <summary>
        /// Roman Shade Yardage Listing form Contains Product related Code for Operations and Displaying Roman Shade Yardage Data      
        /// <author>
        /// Kaushalam Team © Kaushalam Inc. 2012.
        /// </author>
        /// Version 1.0
        /// </summary>

        #region Declarations

        public static bool isDescendFName = false;
        public static bool isDescendLName = false;
        public static bool isDescendEmail = false;

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
                isDescendFName = false;
                isDescendLName = false;
                isDescendEmail = false;
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Roman Shade Yardage inserted successfully.', 'Message','');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Roman Shade Yardage updated successfully.', 'Message','');});", true);
                    }
                }
                BindStore();

                if (ddlStoreName.SelectedIndex == 0)
                {
                    AtagInsertProductyardage.HRef = "/Admin/Products/ProductRomanShadeYardage.aspx?StoreId=1";
                }
                else
                {
                    AtagInsertProductyardage.HRef = "/Admin/Products/ProductRomanShadeYardage.aspx?StoreId=" + ddlStoreName.SelectedValue;
                }
                BindData("");
            }
        }

        /// <summary>
        /// Bind store dropdown
        /// </summary>
        private void BindStore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail != null && storeDetail.Count > 0)
            {
                ddlStoreName.DataSource = storeDetail;
                ddlStoreName.DataTextField = "StoreName";
                ddlStoreName.DataValueField = "StoreID";
                ddlStoreName.DataBind();
            }
            ddlStoreName.Items.Insert(0, new ListItem("All", "0"));
            ddlStoreName.SelectedIndex = 0;
        }

        protected void BindData(String SearchVal)
        {
            string strSearchVal = "";
            if (ddlStoreName.SelectedIndex > 0)
            {
                strSearchVal = " Where StoreId = " + ddlStoreName.SelectedValue + " ";
            }
            if (!string.IsNullOrEmpty(txtSearch.Text.ToString()))
            {
                if (!string.IsNullOrEmpty(strSearchVal.Trim()))
                    strSearchVal += " and ShadeName like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'";
                else
                    strSearchVal += " Where ShadeName like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'";
            }

            DataSet dsSearchpro = new DataSet();
            dsSearchpro = CommonComponent.GetCommonDataSet("Select RomanShadeId,ISNULL(ShadeName,'') as ShadeName,ISNULL(StoreId,1) as StoreId,ISNULL(WidthStandardAllowance,0) as WidthStandardAllowance,ISNULL(LengthStandardAllowance,0) as LengthStandardAllowance,ISNULL(Active,1) as Active,Isnull(DisplayOrder,0) as DisplayOrder from tb_ProductRomanShadeYardage " +
                                                             "" + strSearchVal + " Order by ISNULL(DisplayOrder,999)");
            if (dsSearchpro != null && dsSearchpro.Tables.Count > 0 && dsSearchpro.Tables[0].Rows.Count > 0)
            {
                grdSearchProductType.DataSource = dsSearchpro.Tables[0];
                grdSearchProductType.DataBind();
            }
            else
            {
                grdSearchProductType.DataSource = null;
                grdSearchProductType.DataBind();
            }
        }

        /// <summary>
        /// Page OnInit Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// Admin Grid view Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdSearchProductType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteAdmin")
            {
                try
                {
                    int SearchID = Convert.ToInt32(e.CommandArgument);
                    if (SearchID > 0)
                    {
                        CommonComponent.ExecuteCommonData("Delete tb_ProductRomanShadeYardage where RomanShadeId=" + SearchID + "");
                        BindData("");
                    }
                }
                catch (Exception ex)
                { throw ex; }
            }
            else if (e.CommandName == "edit")
            {
                foreach (GridViewRow dr in grdSearchProductType.Rows)
                {
                    Label lblRomanShadeId = (Label)dr.FindControl("lblRomanShadeId");
                    Label lblStoreId = (Label)dr.FindControl("lblStoreId");
                    if (lblRomanShadeId.Text == e.CommandArgument.ToString())
                    {
                        try
                        {
                            int SearchID = Convert.ToInt32(e.CommandArgument);
                            if (string.IsNullOrEmpty(lblStoreId.Text.ToString()))
                            {
                                Response.Redirect("ProductRomanShadeYardage.aspx?RomanShadeId=" + SearchID + "&StoreId=1");
                            }
                            else
                            {
                                Response.Redirect("ProductRomanShadeYardage.aspx?RomanShadeId=" + SearchID + "&StoreId=" + lblStoreId.Text.ToString());
                            }
                        }
                        catch { }
                    }
                }

            }
            else if (e.CommandName == "add")
            {
                try
                {
                    Response.Redirect("ProductRomanShadeYardage.aspx");
                }
                catch
                { }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData("");
        }

        /// <summary>
        /// Method for set Image is active or not
        /// </summary>
        /// <param name="_Status">bool _Status</param>
        /// <returns>Returns the Image Path</returns>
        public string SetImage(Boolean _Status)
        {
            String _ReturnUrl = "";
            if (_Status == true)
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
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindData("");
        }

        /// <summary>
        /// Search product Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>

        protected void grdSearchProductType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
            }
        }

        protected void grdSearchProductType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSearchProductType.PageIndex = e.NewPageIndex;
            BindData("");
        }

        protected void ddlStoreName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData("");
            if (ddlStoreName.SelectedIndex == 0)
            {
                AtagInsertProductyardage.HRef = "/Admin/Products/ProductRomanShadeYardage.aspx?StoreId=1";
            }
            else
            {
                AtagInsertProductyardage.HRef = "/Admin/Products/ProductRomanShadeYardage.aspx?StoreId=" + ddlStoreName.SelectedValue;
            }
        }
    }
}