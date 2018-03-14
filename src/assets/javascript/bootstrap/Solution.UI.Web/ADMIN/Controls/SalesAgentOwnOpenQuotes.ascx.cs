﻿using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Controls
{
    public partial class SalesAgentOwnOpenQuotes : System.Web.UI.UserControl
    {
        public int storeid = 1;
        public int loginid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SalesAgentQwnQuote();
        }
        /// <summary>
        /// Set Item Name for substring Item Name and display
        /// </summary>
        /// <param name="ItemName">String ItemName</param>
        /// <returns>Returns substring of Item Name </returns>
        public String SetItemName(String ItemName)
        {
            if (ItemName.Length > 25)
            {
                ItemName = ItemName.ToString().Trim().Substring(0, 25) + "...";
            }
            return ItemName;
        }

        private void SalesAgentQwnQuote()
        {
            DataSet dsTop5Items = new DataSet();
            ContentPlaceHolder objCon = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder1");
            if (objCon != null)
            {
                DropDownList ddls = (DropDownList)objCon.FindControl("ddlStore");
                Int32 ss = Convert.ToInt32(ddls.SelectedValue.ToString());

                storeid = ss;
                
               if(Session["AdminID"]!=null)
               {
                   loginid = Convert.ToInt32(Session["AdminID"].ToString());
               }
               dsTop5Items = DashboardComponent.GetTop10SalesAgentOwnOpenQuotes(ss,loginid);
                if (dsTop5Items != null && dsTop5Items.Tables.Count > 0 && dsTop5Items.Tables[0].Rows.Count > 0)
                {
                    grdSalesAgentQuotelist.DataSource = dsTop5Items.Tables[0];
                    grdSalesAgentQuotelist.DataBind();
                    tdviewReport.Visible = true;
                }
                else
                {
                    grdSalesAgentQuotelist.DataSource = null;
                    grdSalesAgentQuotelist.DataBind();
                    tdviewReport.Visible = false;
                }
            }
        }
    }
}