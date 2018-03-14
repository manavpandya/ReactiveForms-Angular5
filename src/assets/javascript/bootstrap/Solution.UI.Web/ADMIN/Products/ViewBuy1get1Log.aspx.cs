using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ViewBuy1get1Log : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //  binddata();
            }

        }
        public void binddata()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("Exec [GuiGetExportProductBuy1Get1]  2,'" + txtsearch.Text.ToString().Replace("'", "''") + "'");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvhemminglog.DataSource = ds;
                gvhemminglog.DataBind();
            }
            else
            {
                gvhemminglog.DataSource = null;
                gvhemminglog.DataBind();
            }
        }

        protected void gvhemminglog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvhemminglog.PageIndex = e.NewPageIndex;
            binddata();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            binddata();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtsearch.Text = "";
            binddata();
        }

        protected void gvhemminglog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblbeforeBuy1Fromdate = (Label)e.Row.FindControl("lblbeforeBuy1Fromdate");
                Label lblbeforeBuy1Todate = (Label)e.Row.FindControl("lblbeforeBuy1Todate");
                Label lblBuy1Fromdate = (Label)e.Row.FindControl("lblBuy1Fromdate");
                Label lblBuy1Todate = (Label)e.Row.FindControl("lblBuy1Todate");
                Label lblbeforebuy1price=(Label)e.Row.FindControl("lblbeforebuy1price");
                Label lblafterbuy1price = (Label)e.Row.FindControl("lblafterbuy1price");

                lblbeforebuy1price.Text = string.Format("{0:0.00}", Convert.ToDecimal(lblbeforebuy1price.Text.ToString()));
                lblafterbuy1price.Text = string.Format("{0:0.00}", Convert.ToDecimal(lblafterbuy1price.Text.ToString()));

                if (!String.IsNullOrEmpty(lblbeforeBuy1Fromdate.Text) && (lblbeforeBuy1Fromdate.Text.ToString().IndexOf("01/01/0001") > -1 || lblbeforeBuy1Fromdate.Text.ToString().IndexOf("1/1/0001") > -1 || lblbeforeBuy1Fromdate.Text.ToString().IndexOf("01/01/1900") > -1 || lblbeforeBuy1Fromdate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblbeforeBuy1Fromdate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblbeforeBuy1Fromdate.Text))
                {
                    lblbeforeBuy1Fromdate.Text = (Convert.ToDateTime(lblbeforeBuy1Fromdate.Text)).ToString("MM/dd/yyyy");
                }

                if (!String.IsNullOrEmpty(lblbeforeBuy1Todate.Text) && (lblbeforeBuy1Todate.Text.ToString().IndexOf("01/01/0001") > -1 || lblbeforeBuy1Todate.Text.ToString().IndexOf("1/1/0001") > -1 || lblbeforeBuy1Todate.Text.ToString().IndexOf("01/01/1900") > -1 || lblbeforeBuy1Todate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblbeforeBuy1Todate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblbeforeBuy1Todate.Text))
                {
                    lblbeforeBuy1Todate.Text = (Convert.ToDateTime(lblbeforeBuy1Todate.Text)).ToString("MM/dd/yyyy");
                }


                if (!String.IsNullOrEmpty(lblBuy1Fromdate.Text) && (lblBuy1Fromdate.Text.ToString().IndexOf("01/01/0001") > -1 || lblBuy1Fromdate.Text.ToString().IndexOf("1/1/0001") > -1 || lblBuy1Fromdate.Text.ToString().IndexOf("01/01/1900") > -1 || lblBuy1Fromdate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblBuy1Fromdate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblBuy1Fromdate.Text))
                {
                    lblBuy1Fromdate.Text = (Convert.ToDateTime(lblBuy1Fromdate.Text)).ToString("MM/dd/yyyy");
                }

                if (!String.IsNullOrEmpty(lblBuy1Todate.Text) && (lblBuy1Todate.Text.ToString().IndexOf("01/01/0001") > -1 || lblBuy1Todate.Text.ToString().IndexOf("1/1/0001") > -1 || lblBuy1Todate.Text.ToString().IndexOf("01/01/1900") > -1 || lblBuy1Todate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblBuy1Todate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblBuy1Todate.Text))
                {
                    lblBuy1Todate.Text = (Convert.ToDateTime(lblBuy1Todate.Text)).ToString("MM/dd/yyyy");
                }

            }
        }
    }
}