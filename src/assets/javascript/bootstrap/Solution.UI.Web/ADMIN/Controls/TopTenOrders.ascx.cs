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
    public partial class TopTenOrders : System.Web.UI.UserControl
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FillTopTenOrders();
        }

        /// <summary>
        /// Fill Top Ten Orders 
        /// </summary>
        private void FillTopTenOrders()
        {
            DataSet dsTopTenOrder = new DataSet();
            dsTopTenOrder = DashboardComponent.GetTopTenOrders();
            if (dsTopTenOrder != null && dsTopTenOrder.Tables.Count > 0 && dsTopTenOrder.Tables[0].Rows.Count > 0)
            {
                grdTopTenOrders.DataSource = dsTopTenOrder.Tables[0];
                grdTopTenOrders.DataBind();
            }
            else
            {
                grdTopTenOrders.DataSource = null;
                grdTopTenOrders.DataBind();
            }
        }


        /// <summary>
        /// Top Ten Orders Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdTopTenOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblOrderStatus = (Label)e.Row.FindControl("lblOrderStatus");

                if (lblOrderStatus != null)
                {
                    if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("pending") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#D3321C;font-size:11px;");
                    }
                    else if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("authorized") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#FF7F00;");
                    }
                    else if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("captured") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#348934;");
                    }
                    else if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("void") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#1A1AC4;");
                    }
                    else if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("refunded") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#00AAFF;");
                    }
                    else if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("canceled") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#FF0000;");
                    }
                    else
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#000;");
                    }
                }
            }
        }
    }
}