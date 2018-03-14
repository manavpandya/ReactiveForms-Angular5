using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;


namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class SearchLogList : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSearchFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtSearchTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");

                BindStore();
                GetSearchLog();
            }
        }

        /// <summary>
        ///  Search Report Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvSearchReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvSearchReport.PageIndex = e.NewPageIndex;
            GetSearchLog();
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void BindStore()
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
            ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
            else
            {
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Get Search log By Store
        /// </summary>
        private void GetSearchLog()
        {
            try
            {
                string strSql = "SELECT tb_SearchLog.*,tb_Store.Storename FROM tb_SearchLog INNER JOIN tb_Store ON tb_Store.StoreID = tb_SearchLog.StoreID WHERE tb_SearchLog.SearchLogid <> 0 AND tb_SearchLog.SearchTerm NOT LIKE '%<script%'";
                if (ddlStore.SelectedIndex > 0)
                {
                    strSql += " AND tb_SearchLog.Storeid=" + ddlStore.SelectedValue.ToString() + "";
                }
                if (txtSearch.Text.ToString().ToLower() != "search keyword")
                {
                    strSql += " AND SearchTerm like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'";
                }
                if (txtSearchFrom.Text.ToString() != "" && txtSearchTo.Text.ToString() != "")
                {
                    if (Convert.ToDateTime(txtSearchTo.Text.ToString()) >= Convert.ToDateTime(txtSearchFrom.Text.ToString()))
                    {

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }
                }
                else
                {
                    if (txtSearchFrom.Text.ToString() == "" && txtSearchTo.Text.ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }
                    else if (txtSearchTo.Text.ToString() == "" && txtSearchFrom.Text.ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }

                }


                if (txtSearchFrom.Text.ToString() != "")
                {
                    strSql += " AND Convert(char(10),tb_SearchLog.CreatedON,101) >= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtSearchFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
                }
                if (txtSearchTo.Text.ToString() != "")
                {

                    strSql += " AND Convert(char(10),tb_SearchLog.CreatedON,101) <= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtSearchTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
                }

                strSql = strSql + " order by tb_SearchLog.CreatedON DESC";
                DataSet dsSearchLog = new DataSet();
                dsSearchLog = CommonComponent.GetCommonDataSet(strSql);
                if (dsSearchLog != null && dsSearchLog.Tables.Count > 0 && dsSearchLog.Tables[0].Rows.Count > 0)
                {
                    grvSearchReport.DataSource = dsSearchLog;
                    grvSearchReport.DataBind();
                    trdelete.Visible = true;
                }
                else
                {
                    trdelete.Visible = false;
                    grvSearchReport.DataSource = null;
                    grvSearchReport.DataBind();
                }
            }
            catch { }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grvSearchReport.PageIndex = 0;
            GetSearchLog();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            grvSearchReport.PageIndex = 0;
            txtSearch.Text = "Search Keyword";
            txtSearchFrom.Text = "";
            txtSearchTo.Text = "";
            GetSearchLog();
            txtSearchFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
            txtSearchTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));

        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gr in grvSearchReport.Rows)
            {
                Label lblSearchID = (Label)gr.FindControl("lblSearchID");
                CheckBox chkRecord = (CheckBox)gr.FindControl("chkSelect");
                if (chkRecord.Checked)
                {
                    CommonComponent.ExecuteCommonData("Delete from  tb_SearchLog WHERE SearchLogID=" + lblSearchID.Text.ToString() + "");
                }
            }
            grvSearchReport.PageIndex = 0;
            GetSearchLog();
        }
    }
}