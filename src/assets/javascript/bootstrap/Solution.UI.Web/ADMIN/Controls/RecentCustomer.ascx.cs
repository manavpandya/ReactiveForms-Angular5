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
    public partial class RecentCustomer : System.Web.UI.UserControl
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FillRecentCustomer();
        }


        /// <summary>
        /// Fills the recent customer who place order Recently.
        /// </summary>
        private void FillRecentCustomer()
        {
            DataSet dsProduct = new DataSet();
            dsProduct = DashboardComponent.GetRecentCustomer();
            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                grdRecentCustomerlist.DataSource = dsProduct.Tables[0];
                grdRecentCustomerlist.DataBind();
                tdviewReport.Visible = true;
            }
            else
            {
                grdRecentCustomerlist.DataSource = null;
                grdRecentCustomerlist.DataBind();
                tdviewReport.Visible = false;
            }
        }
    }
}