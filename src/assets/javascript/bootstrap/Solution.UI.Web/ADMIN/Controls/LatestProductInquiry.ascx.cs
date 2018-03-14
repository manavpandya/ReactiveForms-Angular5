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
    public partial class LatestProductInquiry : System.Web.UI.UserControl
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            BindProIn();
        }

        /// <summary>
        /// Bind Product Inquiry
        /// </summary>
        private void BindProIn()
        {
            DataSet dsProductInquiry = new DataSet();
            dsProductInquiry = DashboardComponent.GetProductInquiry();
            if (dsProductInquiry != null && dsProductInquiry.Tables.Count > 0 && dsProductInquiry.Tables[0].Rows.Count > 0)
            {
                grdProductInquiry.DataSource = dsProductInquiry.Tables[0];
                grdProductInquiry.DataBind();
                tdviewReport.Visible = true;
            }
            else
            {
                grdProductInquiry.DataSource = null;
                grdProductInquiry.DataBind();
                tdviewReport.Visible = false;
            }
        }
    }
}