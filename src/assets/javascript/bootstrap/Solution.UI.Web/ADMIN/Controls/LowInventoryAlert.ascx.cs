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
    public partial class LowInventoryAlert : System.Web.UI.UserControl
    {
        public int storeid = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            FillLowInventoryAlert();
        }
        private void FillLowInventoryAlert()
        {


            DataSet dsTop5Items = new DataSet();
            ContentPlaceHolder objCon = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            if (objCon != null)
            {
                DropDownList ddls = (DropDownList)objCon.FindControl("ddlStore");
                Int32 ss = Convert.ToInt32(ddls.SelectedValue.ToString());

                storeid = ss;
                DataSet dsLowInventoryAlert = new DataSet();
                dsLowInventoryAlert = DashboardComponent.GetLowInventoryAlert(ss);

                if (dsLowInventoryAlert != null && dsLowInventoryAlert.Tables.Count > 0 && dsLowInventoryAlert.Tables[0].Rows.Count > 0)
                {
                    grdLowInventoryAlert.DataSource = dsLowInventoryAlert.Tables[0];
                    grdLowInventoryAlert.DataBind();
                }
                else
                {
                    grdLowInventoryAlert.DataSource = null;
                    grdLowInventoryAlert.DataBind();
                }
            }
        }
    }

}