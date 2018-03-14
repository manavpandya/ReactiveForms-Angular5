using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Controls
{
    public partial class SalesAgentOwnSales : System.Web.UI.UserControl
    {
        public int storeid = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            FillOrdersbyownsale();
        }
   

       /// <summary>
        /// Fill Top Ten Orders 
        /// </summary>
        private void FillOrdersbyownsale()
        {
            DataSet dsTopTenOrder = new DataSet();
            ContentPlaceHolder objCon = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            if (objCon != null)
            {
                DropDownList ddls = (DropDownList)objCon.FindControl("ddlStore");
                Int32 ss = Convert.ToInt32(ddls.SelectedValue.ToString());

                storeid = ss;
                
                int loginid = Convert.ToInt32(Session["AdminID"]);
                dsTopTenOrder = DashboardComponent.GetSalesAgentOwnSale(ss,loginid);
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
        }
}
}