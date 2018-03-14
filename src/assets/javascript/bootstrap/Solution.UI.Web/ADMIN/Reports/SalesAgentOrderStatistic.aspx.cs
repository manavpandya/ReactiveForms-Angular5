using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class SalesAgentOrderStatistic : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtMailFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtMailTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                // btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                // btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");

                BindStore();
                GetSalesAgents();
                // GetEmail();
                GetOrderStats();
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            //GetEmail();
            GetOrderStats();
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void BindStore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grvsalesagentordrstats.PageIndex = 0;
            //GetEmail();
            GetOrderStats();
        }

        /// <summary>
        /// Gets the Sales Agents
        /// </summary>
        private void GetSalesAgents()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet(" SELECT AdminID,(FirstName+' '+LastName) AS Name FROM tb_admin WHERE AdminID IN( SELECT adminid FROM tb_Admin where Deleted=0)");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsalesagents.DataSource = ds;
                ddlsalesagents.DataTextField = "Name";
                ddlsalesagents.DataValueField = "AdminID";
                ddlsalesagents.DataBind();
            }
            else
            {
                ddlsalesagents.DataSource = null;
                ddlsalesagents.DataBind();
            }
            ddlsalesagents.Items.Insert(0, new ListItem("All", "0"));
        }

        /// <summary>
        /// Gets the Order Stats.
        /// </summary>
        private void GetOrderStats()
        {
            // string strSql = "SELECT tb_MailLog.*,tb_Store.Storename FROM tb_MailLog INNER JOIN tb_Store ON tb_Store.StoreID = tb_MailLog.StoreID WHERE ISNULL(tb_MailLog.Deleted,0) = 0 ";

            string strSql = "SELECT o.OrderNumber,o.OrderDate,o.StoreID, a.FirstName+' '+a.LastName AS NAME, o.OrderStatus, o.OrderSubtotal, o.OrderTotal,o.ShippingZip FROM dbo.tb_Order o INNER JOIN dbo.tb_Admin a ON o.SalesAgentID=a.AdminID";
            if (ddlStore.SelectedIndex > 0)
            {
                strSql += " AND o.Storeid=" + ddlStore.SelectedValue.ToString() + "";
            }
            if (ddlsalesagents.SelectedIndex > 0)
            {
                strSql += " AND o.SalesAgentID=" + ddlsalesagents.SelectedValue.ToString() + "";
            }
            if (txtMailFrom.Text.ToString() != "" && txtMailTo.Text.ToString() != "")
            {
                if (Convert.ToDateTime(txtMailTo.Text.ToString()) >= Convert.ToDateTime(txtMailFrom.Text.ToString()))
                {

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }
            else
            {
                if (txtMailFrom.Text.ToString() == "" && txtMailTo.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
                else if (txtMailTo.Text.ToString() == "" && txtMailFrom.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }

            }


            if (txtMailFrom.Text.ToString() != "")
            {
                strSql += " AND Convert(char(10),o.OrderDate,101) >= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }
            if (txtMailTo.Text.ToString() != "")
            {

                strSql += " AND Convert(char(10),o.OrderDate,101) <= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }
            strSql = strSql + " ORDER BY o.OrderNumber desc";
            DataSet dsMail = new DataSet();
            dsMail = CommonComponent.GetCommonDataSet(strSql);
            if (dsMail != null && dsMail.Tables.Count > 0 && dsMail.Tables[0].Rows.Count > 0)
            {
                grvsalesagentordrstats.DataSource = dsMail;
                grvsalesagentordrstats.DataBind();
                trdelete.Visible = true;
            }
            else
            {
                trdelete.Visible = false;
                grvsalesagentordrstats.DataSource = null;
                grvsalesagentordrstats.DataBind();
            }



        }

        /// <summary>
        ///  Sales Agent Order States Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvsalesagentordrstats_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvsalesagentordrstats.PageIndex = e.NewPageIndex;
            GetOrderStats();
        }

        Decimal total = Decimal.Zero;

        /// <summary>
        /// Sales Agent Order States Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grvsalesagentordrstats_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ordertotal"));
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblamount = (Label)e.Row.FindControl("lblTotal");
                lblamount.Text = total.ToString("C").Replace("(", "-").Replace(")", "");

            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged1(object sender, EventArgs e)
        {
            grvsalesagentordrstats.PageIndex = 0;
            GetOrderStats();
        }
    }
}