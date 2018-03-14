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
    public partial class TopTenCustomers : System.Web.UI.UserControl
    {
        public int storeid = 1;
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Top10Customers();
        }

        /// <summary>
        /// Fill the Top 10 Customers By Profitability grid view
        /// </summary>
        private void Top10Customers()
        {
            DataSet dsCustomer = new DataSet();
            ContentPlaceHolder objCon = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            if (objCon != null)
            {
                DropDownList ddls = (DropDownList)objCon.FindControl("ddlStore");
                Int32 ss = Convert.ToInt32(ddls.SelectedValue.ToString());

                storeid = ss;
                dsCustomer = DashboardComponent.GetTop10Customer(ss);
                if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
                {
                    grdCustomerlist.DataSource = dsCustomer;
                    grdCustomerlist.DataBind();
                    tdviewReport.Visible = true;
                }
                else
                {
                    grdCustomerlist.DataSource = null;
                    grdCustomerlist.DataBind();
                    tdviewReport.Visible = false;
                }
            }
        }
    }
}