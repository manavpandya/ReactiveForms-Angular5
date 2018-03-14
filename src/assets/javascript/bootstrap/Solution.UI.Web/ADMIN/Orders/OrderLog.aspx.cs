using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class OrderLog : BasePage
    {
        OrderComponent objorder = new OrderComponent();
        DataSet dsorderlog = new DataSet();
        string OrderNumber = "";

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = Request.QueryString["ONo"];
            }
            if (!IsPostBack)
            {
                BindData(OrderNumber);
            }
        }


        /// <summary>
        /// Binds the Data for Order Log
        /// </summary>
        /// <param name="OrderNumber">string OrderNumber</param>
        public void BindData(string OrderNumber)
        {
            dsorderlog = objorder.GetOrderLog(OrderNumber);
            if (dsorderlog != null && dsorderlog.Tables.Count > 0 && dsorderlog.Tables[0].Rows.Count > 0)
            {
                grdOrderLog.DataSource = dsorderlog;
                grdOrderLog.DataBind();
            }
            else
            {
                grdOrderLog.DataSource = null;
                grdOrderLog.DataBind();
            }
        }
    }
}