using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web.ADMIN.Controls
{
    public partial class AgentMonthlysales : System.Web.UI.UserControl
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FillMonths();
            ddlOption.SelectedValue = DateTime.Now.Month.ToString();
            FillagentwiseGrid(ddlOption.SelectedValue.ToString());
        }



        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlOption control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillagentwiseGrid(ddlOption.SelectedValue.ToString());
        }


        /// <summary>
        /// Fill agent wise the grid.
        /// </summary>
        /// <param name="Duration">string duration.</param>
        private void FillagentwiseGrid(String Duration)
        {
            DataSet ds = new DataSet();
            //ds = CommonComponent.GetCommonDataSet("SELECT SUM(o.OrderTotal) AS ordertotal ,COUNT(o.SalesAgentID) AS totalsales, o.SalesAgentID ,a.FirstName+' '+a.LastName AS Name FROM dbo.tb_Order o INNER JOIN dbo.tb_Admin a ON o.SalesAgentID=a.AdminID WHERE MONTH(o.OrderDate)='" + Duration + "' AND YEAR(o.OrderDate) = DATEPART(year,GETDATE()) and o.StoreID='"+DashboardComponent.ControlStoreID+"'  GROUP BY a.FirstName+' '+a.LastName, o.SalesAgentID ORDER BY SUM(o.OrderTotal) desc");
            ds = DashboardComponent.getagentmonthlysales(Duration);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdsalesagentlistmnthly.DataSource = ds.Tables[0];
                grdsalesagentlistmnthly.DataBind();
            }
            else
            {
                grdsalesagentlistmnthly.DataSource = null;
                grdsalesagentlistmnthly.DataBind();
            }
        }

        /// <summary>
        /// View Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Fill Months function
        /// </summary>
        private void FillMonths()
        {
            ddlOption.Items.Insert(0, new ListItem("January", "1"));
            ddlOption.Items.Insert(1, new ListItem("Febuary", "2"));
            ddlOption.Items.Insert(2, new ListItem("March", "3"));
            ddlOption.Items.Insert(3, new ListItem("April", "4"));
            ddlOption.Items.Insert(4, new ListItem("May", "5"));
            ddlOption.Items.Insert(5, new ListItem("June", "6"));
            ddlOption.Items.Insert(6, new ListItem("July", "7"));
            ddlOption.Items.Insert(7, new ListItem("August", "8"));
            ddlOption.Items.Insert(8, new ListItem("September", "9"));
            ddlOption.Items.Insert(9, new ListItem("October", "10"));
            ddlOption.Items.Insert(10, new ListItem("November", "11"));
            ddlOption.Items.Insert(11, new ListItem("December", "12"));
        }
    }
}