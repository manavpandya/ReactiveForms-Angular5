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
    public partial class LowInventory : System.Web.UI.UserControl
    {
        public int storeid = 1;
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FillTopTenLowInventory();
        }


        /// <summary>
        /// Fill Top ten Low Inventory in Gridview
        /// </summary>
        private void FillTopTenLowInventory()
        {
             DataSet dsTop5Items = new DataSet();
            ContentPlaceHolder objCon = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            if (objCon != null)
            {
                DropDownList ddls = (DropDownList)objCon.FindControl("ddlStore");
                Int32 ss = Convert.ToInt32(ddls.SelectedValue.ToString());

                storeid = ss;
                DataSet dsLowInventory = new DataSet();
                dsLowInventory = DashboardComponent.GetLowInventory(ss);

                if (dsLowInventory != null && dsLowInventory.Tables.Count > 0 && dsLowInventory.Tables[0].Rows.Count > 0)
                {
                    grdLowInventory.DataSource = dsLowInventory.Tables[0];
                    grdLowInventory.DataBind();
                }
                else
                {
                    grdLowInventory.DataSource = null;
                    grdLowInventory.DataBind();
                }
            }
        }
    }
}