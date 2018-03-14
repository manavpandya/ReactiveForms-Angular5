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
    public partial class ViewNewArrivalLog :BasePage
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
            ds = CommonComponent.GetCommonDataSet("Exec [GuiGetExportProductNewarrival]  2,'" + txtsearch.Text.ToString().Replace("'", "''") + "'");
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
                Label lblBeforeIsNewArrivalfromdate = (Label)e.Row.FindControl("lblBeforeIsNewArrivalfromdate");
                Label lblbeforeIsNewArrivaltodate = (Label)e.Row.FindControl("lblbeforeIsNewArrivaltodate");
                Label lblIsNewArrivalFromdate = (Label)e.Row.FindControl("lblIsNewArrivalFromdate");
                Label lblIsNewArrivalTodate = (Label)e.Row.FindControl("lblIsNewArrivalTodate");

                if (!String.IsNullOrEmpty(lblBeforeIsNewArrivalfromdate.Text) && (lblBeforeIsNewArrivalfromdate.Text.ToString().IndexOf("01/01/0001") > -1 || lblBeforeIsNewArrivalfromdate.Text.ToString().IndexOf("1/1/0001") > -1 || lblBeforeIsNewArrivalfromdate.Text.ToString().IndexOf("01/01/1900") > -1 || lblBeforeIsNewArrivalfromdate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblBeforeIsNewArrivalfromdate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblBeforeIsNewArrivalfromdate.Text))
                {
                    lblBeforeIsNewArrivalfromdate.Text = (Convert.ToDateTime(lblBeforeIsNewArrivalfromdate.Text)).ToString("MM/dd/yyyy");
                }

                if (!String.IsNullOrEmpty(lblbeforeIsNewArrivaltodate.Text) && (lblbeforeIsNewArrivaltodate.Text.ToString().IndexOf("01/01/0001") > -1 || lblbeforeIsNewArrivaltodate.Text.ToString().IndexOf("1/1/0001") > -1 || lblbeforeIsNewArrivaltodate.Text.ToString().IndexOf("01/01/1900") > -1 || lblbeforeIsNewArrivaltodate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblbeforeIsNewArrivaltodate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblbeforeIsNewArrivaltodate.Text))
                {
                    lblbeforeIsNewArrivaltodate.Text = (Convert.ToDateTime(lblbeforeIsNewArrivaltodate.Text)).ToString("MM/dd/yyyy");
                }


                if (!String.IsNullOrEmpty(lblIsNewArrivalFromdate.Text) && (lblIsNewArrivalFromdate.Text.ToString().IndexOf("01/01/0001") > -1 || lblIsNewArrivalFromdate.Text.ToString().IndexOf("1/1/0001") > -1 || lblIsNewArrivalFromdate.Text.ToString().IndexOf("01/01/1900") > -1 || lblIsNewArrivalFromdate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblIsNewArrivalFromdate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblIsNewArrivalFromdate.Text))
                {
                    lblIsNewArrivalFromdate.Text = (Convert.ToDateTime(lblIsNewArrivalFromdate.Text)).ToString("MM/dd/yyyy");
                }

                if (!String.IsNullOrEmpty(lblIsNewArrivalTodate.Text) && (lblIsNewArrivalTodate.Text.ToString().IndexOf("01/01/0001") > -1 || lblIsNewArrivalTodate.Text.ToString().IndexOf("1/1/0001") > -1 || lblIsNewArrivalTodate.Text.ToString().IndexOf("01/01/1900") > -1 || lblIsNewArrivalTodate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblIsNewArrivalTodate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblIsNewArrivalTodate.Text))
                {
                    lblIsNewArrivalTodate.Text = (Convert.ToDateTime(lblIsNewArrivalTodate.Text)).ToString("MM/dd/yyyy");
                }

            }
        }
    }
}