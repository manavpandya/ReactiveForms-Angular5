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
    public partial class HeaderTextList : BasePage
    {
        #region Variable declaration
        public static bool isDescendHeaderName = false;
        public static bool isDescendStoreName = false;
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
                BindheaderText();
             
                btnDeleteHeader.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/delet.gif) no-repeat transparent; width: 58px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Header inserted successfully.', 'Message');});", true);

                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Header updated successfully.', 'Message');});", true);
                    }
                }

                //  bindstore();
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }


            grdHeader.DataBind();
            if (grdHeader.Rows.Count == 0)
                trBottom.Visible = false;
        }

        public void BindheaderText()
        {
            DataSet ds = CommonComponent.GetCommonDataSet("select * from tb_HeaderText where isnull(Deleted,0)=0");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdHeader.DataSource = ds;
                grdHeader.DataBind();
            }
            else
            {
                grdHeader.DataSource = null;
                grdHeader.DataBind();
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //grdHeader.PageIndex = 0;
            //grdHeader.DataBind();
            //if (grdHeader.Rows.Count == 0)
            //    trBottom.Visible = false;
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            //txtSearch.Text = "";
            //ddlStore.SelectedIndex = 0;
            //grdHeader.PageIndex = 0;
            //grdHeader.DataBind();
        }

        /// <summary>
        ///  Delete Header Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDeleteHeader_Click(object sender, EventArgs e)
        {
            HeaderComponent objHeaderComp = new HeaderComponent();
            tb_HeaderText Header = new tb_HeaderText();
            int totalRowCount = grdHeader.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdHeader.Rows[i].FindControl("hdnHeaderID");
                CheckBox chk = (CheckBox)grdHeader.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                  
                 
                    CommonComponent.ExecuteCommonData("Update  tb_HeaderText set Deleted=1 where HeaderID=" + hdn.Value + "");
                    BindheaderText();
                }
            }
           
            if (grdHeader.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        /// Bind Store dropdown
        /// </summary>
        private void bindstore()
        {
            
        }

        /// <summary>
        /// Sort records in ASC or DESC order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
          
        }

        /// <summary>
        /// Header Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdHeader_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int HeaderID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("HeaderText.aspx?HeaderID=" + HeaderID); //Edit Header
            }
        }

        /// <summary>
        /// Header Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdHeader_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (grdHeader.Rows.Count > 0)
            {
                trBottom.Visible = true;
            }
            else
            {
                trBottom.Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                Label lblActive = (Label)e.Row.FindControl("lblActive");
                if(lblActive.Text.Trim()=="1" || lblActive.Text.Trim()=="True")
                {
                    lblActive.Text = "Active";
                    lblActive.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblActive.Text = "In-Active";
                    lblActive.ForeColor = System.Drawing.Color.Red;
                }
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {

            }
        }
    }
}