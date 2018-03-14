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
    public partial class TopFiveItems : System.Web.UI.UserControl
    {
        public int storeid = 1;
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Top5ItemsBySales();
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
        private void Top5ItemsBySales()
        {
            DataSet dsTop5Items = new DataSet();
             ContentPlaceHolder objCon = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
             if (objCon != null)
             {
                 DropDownList ddls = (DropDownList)objCon.FindControl("ddlStore");
                 Int32 ss = Convert.ToInt32(ddls.SelectedValue.ToString());

                 storeid = ss;
                 dsTop5Items = DashboardComponent.GetTop5ItemsBySales(ss);
                 if (dsTop5Items != null && dsTop5Items.Tables.Count > 0 && dsTop5Items.Tables[0].Rows.Count > 0)
                 {
                     grdTopItemlist.DataSource = dsTop5Items.Tables[0];
                     grdTopItemlist.DataBind();
                     tdviewReport.Visible = true;
                 }
                 else
                 {
                     grdTopItemlist.DataSource = null;
                     grdTopItemlist.DataBind();
                     tdviewReport.Visible = false;
                 }
             }
        }
    }
}