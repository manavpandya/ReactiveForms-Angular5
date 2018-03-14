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

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class DynamicPagePropertyList : Solution.UI.Web.BasePage
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

                BindPageList();

                btndelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('SEO Details inserted successfully.', 'Message');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('SEO Details updated successfully.', 'Message');});", true);
                    }
                }
                int loginid = 0;
                if (Request.QueryString["loginid"] != "" && Request.QueryString["loginid"] != null)
                {
                    loginid = Convert.ToInt32(Request.QueryString["loginid"]);
                }
            }
        }


        private void BindPageList()
        {
            DataSet dsevents = new DataSet();

            dsevents = CommonComponent.GetCommonDataSet("select * from tb_DynamicPageProperty");


            if (dsevents != null && dsevents.Tables.Count > 0 && dsevents.Tables[0].Rows.Count > 0)
            {
                gridcoupon.DataSource = dsevents.Tables[0];
                gridcoupon.DataBind();
            }
            else
            {
                gridcoupon.DataSource = null;
                gridcoupon.DataBind();
            }
            int numVisible = 0;
            foreach (GridViewRow row in gridcoupon.Rows)
            {
                if (row.Visible == true)
                {
                    numVisible += 1;
                }
            }

            if (numVisible > 0)
            {
                trBottom.Visible = true;
            }
            else
            {
                trBottom.Visible = false;
                gridcoupon.DataSource = null;
                gridcoupon.DataBind();
            }
        }


        /// <summary>
        /// Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gridcoupon_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int couponid = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("AddDynamicPageProperty.aspx?ID=" + couponid + "");
            }
        }

        /// <summary>
        /// Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gridcoupon_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lnkedit = (ImageButton)e.Row.FindControl("lnkbtnedit");
                lnkedit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                System.Web.UI.HtmlControls.HtmlAnchor alinkdisplay = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("alinkdisplay");
            }


        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndelete_Click(object sender, EventArgs e)
        {
            int totalRowCount = gridcoupon.Rows.Count;
            for (int tt = 0; tt < totalRowCount; tt++)
            {
                HiddenField hdn = (HiddenField)gridcoupon.Rows[tt].FindControl("hdneventid");
                CheckBox chk = (CheckBox)gridcoupon.Rows[tt].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    CommonComponent.ExecuteCommonData("delete from tb_DynamicPageProperty where ID=" + hdn.Value.ToString() + "");
                }
            }
            BindPageList();
        }
    }
}