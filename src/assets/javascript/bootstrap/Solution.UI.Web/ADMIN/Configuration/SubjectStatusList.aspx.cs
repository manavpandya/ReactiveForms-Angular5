using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.UI.Web.ADMIN.Configuration
{
    public partial class SubjectStatusList : BasePage
    {
        #region Declarations

        public static bool isDescendName = false;
        public static bool isDescendTLISO = false;
        public static bool isDescendThLISO = false;

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
                isDescendTLISO = false;
                isDescendThLISO = false;
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Subject inserted successfully.', 'Message','');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Subject updated successfully.', 'Message','');});", true);
                    }
                }
                BindStatus();
                //SubjctStatusComponent.Filter = "";
            }

            Page.Form.DefaultButton = btnSearch.UniqueID;
        }


        public void BindStatus()
        {
            DataSet ds = CommonComponent.GetCommonDataSet("Select * from tb_ContactEmail where isnull(Deleted,0)=0");
            if(ds!=null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
            {
                grdSubjctStatus.DataSource = ds;
                grdSubjctStatus.DataBind();
            }
            else
            {
                grdSubjctStatus.DataSource = null;
                grdSubjctStatus.DataBind();
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
        /// SubjctStatus Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdSubjctStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteSubjctStatus")
            {
                try
                {
                   
                    int SubjctStatusId = Convert.ToInt32(e.CommandArgument);
                    CommonComponent.ExecuteCommonData("Update tb_ContactEmail set Deleted=1 where ID=" + SubjctStatusId + "");
                    BindStatus();
                }
                catch (Exception ex)
                { throw ex; }
            }
            else
            if (e.CommandName == "edit")
            {
                try
                {
                    int SubjctStatusId = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("SubjectStatus.aspx?SubjectStatusID=" + SubjctStatusId);
                }
                catch
                { }
            }
            else if (e.CommandName == "add")
            {
                try
                {
                    Response.Redirect("SubjectStatus.aspx");
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Refresh Gridview Function
        /// </summary>
        /// <param name="afterDelete">Boolean afterDelete</param>
        private void RefreshGrid(bool afterDelete = false)
        {
            //SubjctStatusComponent.AfterDelete = afterDelete;
            //grdSubjctStatus.DataBind();
        }

        /// <summary>
        /// Sorting Gridview Column by ACS or DESC Order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            //ImageButton lb = (ImageButton)sender;
            //if (lb != null)
            //{
            //    if (lb.CommandArgument == "ASC")
            //    {
            //        grdSubjctStatus.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
            //        lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
            //        if (lb.ID == "lbName")
            //        {
            //            isDescendName = false;
            //        }
            //        else if (lb.ID == "lbTLISO")
            //        {
            //            isDescendTLISO = false;
            //        }
            //        else
            //        {
            //            isDescendThLISO = false;
            //        }

            //        lb.AlternateText = "Descending Order";
            //        lb.ToolTip = "Descending Order";
            //        lb.CommandArgument = "DESC";
            //    }
            //    else if (lb.CommandArgument == "DESC")
            //    {
            //        grdSubjctStatus.Sort(lb.CommandName.ToString(), SortDirection.Descending);
            //        lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
            //        if (lb.ID == "lbName")
            //        {
            //            isDescendName = true;
            //        }
            //        else if (lb.ID == "lbTLISO")
            //        {
            //            isDescendTLISO = true;
            //        }
            //        else
            //        {
            //            isDescendThLISO = true;
            //        }
            //        lb.AlternateText = "Ascending Order";
            //        lb.ToolTip = "Ascending Order";
            //        lb.CommandArgument = "ASC";
            //    }
            //}
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
        //    SubjctStatusComponent.SubjctStatusID = 0;
        //    SubjctStatusComponent.Filter = txtSubjctStatus.Text.Trim();
        //    if (txtSubjctStatus.Text.ToString().Length > 0)
        //    {
        //        SubjctStatusComponent.NewFilter = true;
        //    }
        //    else
        //    {
        //        SubjctStatusComponent.NewFilter = false;
        //    }

        //    grdSubjctStatus.PageIndex = 0;
        //    grdSubjctStatus.DataBind();
        }

        /// <summary>
        /// SubjctStatus Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdSubjctStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
                Label LabelActive = (Label)e.Row.FindControl("LabelActive");
                if (LabelActive.Text.Trim() == "1" || LabelActive.Text.Trim() == "True")
                {
                    LabelActive.Text = "Active";
                    LabelActive.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    LabelActive.Text = "In-Active";
                    LabelActive.ForeColor = System.Drawing.Color.Red;
                }
            }
           
        }

        /// <summary>
        ///  Hidden Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnDelete_Click(object sender, EventArgs e)
        {
            int SubjctStatusId = Convert.ToInt32(hdnDelete.Value);
            CommonComponent.ExecuteCommonData("Update tb_ContactEmail set Deleted=1 where ID=" + SubjctStatusId + "");
            BindStatus();
            //SubjctStatusComponent cust = new SubjctStatusComponent();
            //tb_SubjctStatus tb_SubjctStatus = null;
            //int SubjctStatusId = Convert.ToInt32(hdnDelete.Value);
            //tb_SubjctStatus = cust.getSubjctStatus(SubjctStatusId);
            //tb_SubjctStatus.Deleted = true;
            //cust.UpdateSubjctStatus(tb_SubjctStatus);
            //if (SubjctStatusComponent.SubjctStatusID == 0)
            //    RefreshGrid(true);
            //else
            //    this.Response.Redirect("SubjctStatusList.aspx", false);
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            //txtSubjctStatus.Text = "";
            //SubjctStatusComponent.Filter = "";
            //grdSubjctStatus.PageIndex = 0;
            //grdSubjctStatus.DataBind();
        }
    }
}