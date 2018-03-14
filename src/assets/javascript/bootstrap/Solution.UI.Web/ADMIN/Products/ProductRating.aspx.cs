using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Castle.Web.Controls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductRating : Solution.UI.Web.BasePage
    {
        #region Declaration

        StoreComponent stac = new StoreComponent();
        public static bool isDescendName = false;
        public static bool isDescendDate = false;
        ProductComponent procomp = new ProductComponent();
        tb_Rating tb_rating = new tb_Rating();

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
                if (Request.QueryString["status"] != null)
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Review inserted successfully.', 'Message','');});", true);
                    }
                }
                BindStore();
                btnapproverating.ImageUrl = "/App_Themes/" + Page.Theme + "/images/approve-rating.png";
                btnDisapproverating.ImageUrl = "/App_Themes/" + Page.Theme + "/images/disapprove-rating.png";
                
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
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void drpstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdProductReview.PageIndex = 0;
            grdProductReview.DataBind();
            if (grdProductReview.Rows.Count > 0)
            {
                if (ddlreviewstatus.SelectedValue.ToString() == "0")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlreviewstatus.SelectedValue.ToString() == "1")
                {
                    btnapproverating.Visible = false;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlreviewstatus.SelectedValue.ToString() == "-1")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = false;

                }
                trBottom.Visible = true;
            }
        }

        /// <summary>
        /// Review Status Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlreviewstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdProductReview.PageIndex = 0;
            grdProductReview.DataBind();
            if (grdProductReview.Rows.Count > 0)
            {
                if (ddlreviewstatus.SelectedValue.ToString() == "0")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlreviewstatus.SelectedValue.ToString() == "1")
                {
                    btnapproverating.Visible = false;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlreviewstatus.SelectedValue.ToString() == "-1")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = false;

                }
                trBottom.Visible = true;
            }
        }

        /// <summary>
        /// Product Review Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdProductReview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ddlreviewstatus.SelectedValue == "1")
            {
              //  grdProductReview.Columns[0].Visible = false;
                grdProductReview.Columns[6].Visible = false;
                grdProductReview.Columns[7].Visible = true;
              //  trBottom.Visible = false;
            }
            else if (ddlreviewstatus.SelectedValue == "0")
            {
               // grdProductReview.Columns[0].Visible = false;
                grdProductReview.Columns[6].Visible = true;
                grdProductReview.Columns[7].Visible = true;
                //if (grdProductReview.Rows.Count > 0)
                //   // trBottom.Visible = false;
                //else
                //   // trBottom.Visible = false;
            }
            else if (ddlreviewstatus.SelectedValue == "-1")
            {
             //   grdProductReview.Columns[0].Visible = false;
                grdProductReview.Columns[6].Visible = true;
                grdProductReview.Columns[7].Visible = false;
                //if (grdProductReview.Rows.Count > 0)
                //   // trBottom.Visible = false;
                //else
                //    //trBottom.Visible = false;
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
                if (isDescendDate == false)
                {
                    ImageButton lbDate = (ImageButton)e.Row.FindControl("lbDate");
                    lbDate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbDate.AlternateText = "Ascending Order";
                    lbDate.ToolTip = "Ascending Order";
                    lbDate.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbDate = (ImageButton)e.Row.FindControl("lbDate");
                    lbDate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbDate.AlternateText = "Descending Order";
                    lbDate.ToolTip = "Descending Order";
                    lbDate.CommandArgument = "ASC";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnapprove = (ImageButton)e.Row.FindControl("btnApprove");
                ImageButton btnUnApprove = (ImageButton)e.Row.FindControl("btnUnApprove");

                Rater rating = (Rater)e.Row.FindControl("Rater");
                btnapprove.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/approve.png";
                btnUnApprove.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/disapprove.png";
                rating.ImageOnUrl = "/App_Themes/" + Page.Theme + "/images/star-form.jpg";
                rating.ImageOffUrl = "/App_Themes/" + Page.Theme + "/images/star-form1.jpg";
            }
        }

        /// <summary>
        /// Grid view Sorting Event
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
                    grdProductReview.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = false;
                    }
                    else if (lb.ID == "lbDate")
                    {
                        isDescendDate = false;
                    }
                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdProductReview.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = true;
                    }
                    else if (lb.ID == "lbDate")
                    {
                        isDescendDate = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Product Review Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdProductReview_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        /// <summary>
        ///  Approve Rating Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnapproverating_Click(object sender, ImageClickEventArgs e)
        {
            int totalRowCount = grdProductReview.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdProductReview.Rows[i].FindControl("hdnratingid");
                CheckBox chk = (CheckBox)grdProductReview.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    int ratingid = Convert.ToInt32(hdn.Value);
                    tb_rating = procomp.GetRatingDetail(ratingid);
                    tb_rating.IsApproved = 1;
                    tb_rating.ApprovedDate = DateTime.Now;
                    procomp.UpdateReview(tb_rating);
                }
            }
            grdProductReview.DataBind();
            if (ddlreviewstatus.SelectedValue == "1")
            {

               // grdProductReview.Columns[0].Visible = false;
                grdProductReview.Columns[6].Visible = false;
                trBottom.Visible = false;
            }
            else
            {
               // grdProductReview.Columns[0].Visible = true;
                grdProductReview.Columns[6].Visible = true;
                if (grdProductReview.Rows.Count > 0)
                    trBottom.Visible = true;
                else
                    trBottom.Visible = false;
            }
           // grdProductReview.DataBind();
            if (grdProductReview.Rows.Count > 0)
            {
                if (ddlreviewstatus.SelectedValue.ToString() == "0")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlreviewstatus.SelectedValue.ToString() == "1")
                {
                    btnapproverating.Visible = false;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlreviewstatus.SelectedValue.ToString() == "-1")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = false;

                }
                trBottom.Visible = true;
            }
        }

        /// <summary>
        ///  Hidden Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnDelete_Click(object sender, EventArgs e)
        {
            btnhdnDelete.CommandName = hdnCommandName.Value;
            if (btnhdnDelete.CommandName == "Approve")
            {
                int ratingid = Convert.ToInt32(hdnDelete.Value);
                tb_rating = procomp.GetRatingDetail(ratingid);
                tb_rating.IsApproved = 1;
                tb_rating.ApprovedDate = DateTime.Now;
                procomp.UpdateReview(tb_rating);
                grdProductReview.DataBind();
            }
            if (btnhdnDelete.CommandName == "Disapprove")
            {
                int ratingid = Convert.ToInt32(hdnDelete.Value);
                tb_rating = procomp.GetRatingDetail(ratingid);
                tb_rating.IsApproved = -1;
                procomp.UpdateReview(tb_rating);
                CommonComponent.ExecuteCommonData("update tb_rating set DisapprovedDate='" + DateTime.Now + "' where ratingid=" + ratingid + "");
                grdProductReview.DataBind();
            }
            grdProductReview.DataBind();
        }

        protected void btnDisapproverating_Click(object sender, ImageClickEventArgs e)
        {
            int totalRowCount = grdProductReview.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdProductReview.Rows[i].FindControl("hdnratingid");
                CheckBox chk = (CheckBox)grdProductReview.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    int ratingid = Convert.ToInt32(hdn.Value);
                    tb_rating = procomp.GetRatingDetail(ratingid);
                    tb_rating.IsApproved = -1;
                  
                    procomp.UpdateReview(tb_rating);
                    CommonComponent.ExecuteCommonData("update tb_rating set DisapprovedDate='" + DateTime.Now + "' where ratingid=" + ratingid + "");
                }
            }
            grdProductReview.DataBind();
            if (ddlreviewstatus.SelectedValue == "1")
            {
               // grdProductReview.Columns[0].Visible = false;
                grdProductReview.Columns[6].Visible = false;
                trBottom.Visible = false;
            }
            else
            {
                //grdProductReview.Columns[0].Visible = true;
                grdProductReview.Columns[6].Visible = true;
                if (grdProductReview.Rows.Count > 0)
                    trBottom.Visible = true;
                else
                    trBottom.Visible = false;
            }
            //grdProductReview.DataBind();
            if (grdProductReview.Rows.Count > 0)
            {
                if (ddlreviewstatus.SelectedValue.ToString() == "0")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlreviewstatus.SelectedValue.ToString() == "1")
                {
                    btnapproverating.Visible = false;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlreviewstatus.SelectedValue.ToString() == "-1")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = false;

                }
                trBottom.Visible = true;
            }
        }

    }
}