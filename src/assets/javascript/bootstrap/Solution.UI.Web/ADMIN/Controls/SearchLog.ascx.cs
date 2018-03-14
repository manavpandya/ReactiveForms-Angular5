using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;

namespace Solution.UI.Web.ADMIN.Controls
{
    public partial class SearchLog : System.Web.UI.UserControl
    {
        public int storeid = 1;
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FillSearchLog();
        }

        /// <summary>
        /// Set Item Name for substring Item Name and display
        /// </summary>
        /// <param name="ItemName">String ItemName</param>
        /// <returns>Returns substring of Item Name </returns>
        public String SetItemName(String ItemName)
        {
            if (ItemName.Length > 25)
            {
                ItemName = ItemName.ToString().Trim().Substring(0, 25) + "...";
            }
            return ItemName;
        }

        /// <summary>
        /// Fill Top 5 Items By Sales
        /// </summary>
        private void FillSearchLog()
        {
            DataSet dsSearchLog = new DataSet();
            ContentPlaceHolder objCon = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            if (objCon != null)
            {
                DropDownList ddls = (DropDownList)objCon.FindControl("ddlStore");
                Int32 ss = Convert.ToInt32(ddls.SelectedValue.ToString());

                storeid = ss;
                dsSearchLog = DashboardComponent.GetSearchLog(ss);
                if (dsSearchLog != null && dsSearchLog.Tables.Count > 0 && dsSearchLog.Tables[0].Rows.Count > 0)
                {
                    grdSearchLog.DataSource = dsSearchLog.Tables[0];
                    grdSearchLog.DataBind();
                    tdviewReport.Visible = true;
                }
                else
                {
                    grdSearchLog.DataSource = null;
                    grdSearchLog.DataBind();
                    tdviewReport.Visible = false;
                }
            }
        }
    }
}