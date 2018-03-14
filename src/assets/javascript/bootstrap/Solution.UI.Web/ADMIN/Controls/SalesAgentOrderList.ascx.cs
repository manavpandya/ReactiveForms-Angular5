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
    public partial class SalesAgentOrderList : System.Web.UI.UserControl
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SalesAgentListt();
        }

        /// <summary>
        /// Fill Sales Agent List 
        /// </summary>
        private void SalesAgentListt()
        {
            DataSet ds = new DataSet();
            //ds = CommonComponent.GetCommonDataSet("SELECT TOP 10 o.OrderNumber, o.OrderDate,a.FirstName+' '+a.LastName AS Name FROM dbo.tb_Order o INNER JOIN dbo.tb_Admin a ON o.SalesAgentID=a.AdminID WHERE o.IsPhoneOrder=1 ORDER BY OrderNumber desc");
            ds = DashboardComponent.getsalesagentorderlist();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdsalesagentlist.DataSource = ds.Tables[0];
                grdsalesagentlist.DataBind();
            }
            else
            {
                grdsalesagentlist.DataSource = null;
                grdsalesagentlist.DataBind();
            }
        }

    }
}