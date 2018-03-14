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

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductVariantColorList : BasePage
    {
        /// <summary>
        /// Admin Listing form Contains Admin related Code for Operations and Displaying Admin Data      
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
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product Color inserted successfully.', 'Message','');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product Color updated successfully.', 'Message','');});", true);
                    }
                }
                BindData("");
            }
        }

        protected void BindData(String SearchVal)
        {
            string strSearchVal = "";

            if (!string.IsNullOrEmpty(txtSearch.Text.ToString()))
            {
                strSearchVal = "Where ColorName like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'";
            }

            DataSet dsSearchpro = new DataSet();
            dsSearchpro = CommonComponent.GetCommonDataSet("Select ColorID,ColorName,IsNULL(ImagePath,'') as ImagePath,ISNULL(Active,0) as Active,CreatedOn from tb_ProductColor " + strSearchVal + "");
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
        /// Admin Gridview Row Command Event
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
                        CommonComponent.ExecuteCommonData("Delete tb_ProductColor where ColorID=" + SearchID + "");
                        BindData("");
                    }
                }
                catch (Exception ex)
                { throw ex; }
            }
            else if (e.CommandName == "edit")
            {
                try
                {
                    int SearchID = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("AddProductVariantColor.aspx?ColorID=" + SearchID);
                }
                catch
                { }
            }
            else if (e.CommandName == "add")
            {
                try
                {
                    Response.Redirect("AddProductVariantColor.aspx");
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
                Literal ltrImagePath = ((Literal)e.Row.FindControl("ltrImagePath"));
                Label lblImagePath = ((Label)e.Row.FindControl("lblImagePath"));
                if (!string.IsNullOrEmpty(lblImagePath.Text.ToString().Trim()))
                {
                    string ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/");
                    string StrPath = ProductTempPath + lblImagePath.Text.ToString();
                    if (File.Exists(Server.MapPath(StrPath)))
                    {
                        ltrImagePath.Text = "<img alt=\"\" src=\"" + StrPath.ToString() + "\">";
                    }
                }
            }
        }

        protected void grdSearchProductType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSearchProductType.PageIndex = e.NewPageIndex;
            BindData("");
        }
        protected void btnhdnDelete_Click(object sender, EventArgs e)
        {
            int gColorID = Convert.ToInt32(hdnColorDelete.Value);
            try
            {
                CommonComponent.ExecuteCommonData("Delete from tb_ProductColor where ColorID=" + gColorID + "");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Deleted Successfully.', 'Message');});", true);
                hdnColorDelete.Value = "0";
                BindData("");BindData("");
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while deleting record.', 'Message');});", true);
                return;
            }
        }
    }
}