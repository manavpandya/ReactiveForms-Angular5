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
    public partial class RestrictedShippingState : BasePage
    {
        #region Declarations

        public static bool isDescendName = false;
        public static bool isDescendAbb = false;
        public Int32 insertdata = 0;
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
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
               
                StateComponent.Filter = "";
            }

            Page.Form.DefaultButton = btnSearch.UniqueID;
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

        #region Grid View Event

        /// <summary>
        /// State Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdState_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource.GetType() != typeof(GridView))
            {
                int stateid = Convert.ToInt32(e.CommandArgument);
                
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).NamingContainer);
                CheckBox ischecked = (CheckBox)row.FindControl("chkSelect");
               
                //if (e.CommandName == "DeleteState")
                //{
                //    try
                //    {
                //        StateComponent cust = new StateComponent();
                //        tb_State tb_State = null;
                //        int StateId = Convert.ToInt32(e.CommandArgument);
                //        tb_State = cust.getState(StateId);
                //        tb_State.Deleted = true;
                //        cust.UpdateState(tb_State);
                //        if (StateComponent.StateID == 0)
                //            RefreshGrid(true);
                //        else
                //            this.Response.Redirect("StateList.aspx", false);
                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }
                //}
                ////else
                if (e.CommandName == "edit")
                {
                    string statename = "";

                    try
                    {

                        insertdata = Convert.ToInt32(CommonComponent.GetScalarCommonData("select stateid from tb_stateshippingrestricted where stateid = " + stateid + ""));
                        if (ischecked.Checked == true)
                        {
                           
                            if (insertdata == stateid)
                            {
                               
                                statename = Convert.ToString(CommonComponent.GetScalarCommonData("select distinct isnull(Name,'') as Name  from tb_state where stateid = " + insertdata + ""));
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('" + statename + " State Resricted for shipping!', 'Message');});", true);
                                CommonComponent.GetScalarCommonData("delete  from  tb_stateshippingrestricted where stateid = " + insertdata + " ");
                                CommonComponent.GetScalarCommonData("insert into tb_stateshippingrestricted (stateid,isrestictedshipping) values (" + insertdata + ",'" + ischecked.Checked + "')");
                                ischecked.Checked = true;
                               
                            }
                            else
                            {
                                ischecked.Checked = true;
                                statename = Convert.ToString(CommonComponent.GetScalarCommonData("select distinct isnull(Name,'') as Name  from tb_state where stateid = " + stateid + ""));
                               
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('" + statename + " State Resricted for shipping!', 'Message');});", true);
                                CommonComponent.GetScalarCommonData("insert into tb_stateshippingrestricted (stateid,isrestictedshipping) values (" + stateid + ",'" + ischecked.Checked + "')");
                                
                            }
                         

                        }
                        else if (ischecked.Checked == false)
                        {
                            if (insertdata == stateid)
                            {
                                ischecked.Checked = false;
                                statename = Convert.ToString(CommonComponent.GetScalarCommonData("select distinct isnull(Name,'') as Name  from tb_state where stateid = " + stateid + ""));
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('" + statename + " State free for shipping!', 'Message');});", true);
                                CommonComponent.GetScalarCommonData("delete  from  tb_stateshippingrestricted where stateid = " + insertdata + " ");
                            }
                        }

                    }
                    catch { }
                 
                }
             
            }
        }

        /// <summary>
        ///  Refresh Gridview Function
        /// </summary>
        /// <param name="afterDelete">bool afterDelete</param>
        private void RefreshGrid(bool afterDelete = false)
        {
            StateComponent.AfterDelete = afterDelete;
            grdState.DataBind();
        }

        /// <summary>
        /// Sorting Gridview Column by ACS or DESC Order
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
                    grdState.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = false;
                    }
                    else if (lb.ID == "lbabb")
                    {
                        isDescendAbb = false;
                    }

                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdState.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = true;
                    }
                    else if (lb.ID == "lbabb")
                    {
                        isDescendAbb = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }
        #endregion

        /// <summary>
        /// Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            StateComponent.StateID = 0;
            StateComponent.Filter = txtState.Text.Trim();
            if (txtState.Text.ToString().Length > 0)
            {
                StateComponent.NewFilter = true;
            }
            else
            {
                StateComponent.NewFilter = false;
            }

            grdState.PageIndex = 0;
            grdState.DataBind();
        }

        /// <summary>
        /// State Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdState_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";

                HiddenField hdnStateID = (HiddenField)e.Row.FindControl("hdnStateID");
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                Int32 restricted = Convert.ToInt32(CommonComponent.GetScalarCommonData("select distinct s.stateid as stateid from tb_state s inner join tb_stateshippingrestricted ss on s.stateid = ss.stateid  where s.stateid = " + hdnStateID.Value.ToString() + ""));
                if (restricted > 0)
                {
                    chkSelect.Checked = true;
                }
                else
                    chkSelect.Checked = false;
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
                if (isDescendAbb == false)
                {
                    ImageButton lbabb = (ImageButton)e.Row.FindControl("lbabb");
                    lbabb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbabb.AlternateText = "Ascending Order";
                    lbabb.ToolTip = "Ascending Order";
                    lbabb.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbabb = (ImageButton)e.Row.FindControl("lbabb");
                    lbabb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbabb.AlternateText = "Descending Order";
                    lbabb.ToolTip = "Descending Order";
                    lbabb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Hidden Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnDelete_Click(object sender, EventArgs e)
        {
            StateComponent cust = new StateComponent();
            tb_State tb_State = null;
            int StateId = Convert.ToInt32(hdnDelete.Value);
            tb_State = cust.getState(StateId);
            tb_State.Deleted = true;
            cust.UpdateState(tb_State);
            if (StateComponent.StateID == 0)
                RefreshGrid(true);
            else
                this.Response.Redirect("StateList.aspx", false);
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtState.Text = "";
            StateComponent.Filter = "";
            grdState.PageIndex = 0;
            grdState.DataBind();
        }
    }
}