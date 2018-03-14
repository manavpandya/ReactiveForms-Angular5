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
    public partial class ViewonsaleLog : BasePage
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
            ds = CommonComponent.GetCommonDataSet("Exec [GuiGetExportProductFinalsale]  2,'" + txtsearch.Text.ToString().Replace("'", "''") + "'");
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
                Label lblbeforeonsaleFromdate = (Label)e.Row.FindControl("lblbeforeonsaleFromdate");
                Label lblbeforeonsaleTodate = (Label)e.Row.FindControl("lblbeforeonsaleTodate");
                Label lblonsaleFromdate = (Label)e.Row.FindControl("lblonsaleFromdate");
                Label lblonsaleTodate = (Label)e.Row.FindControl("lblonsaleTodate");

                if (!String.IsNullOrEmpty(lblbeforeonsaleFromdate.Text) && (lblbeforeonsaleFromdate.Text.ToString().IndexOf("01/01/0001") > -1 || lblbeforeonsaleFromdate.Text.ToString().IndexOf("1/1/0001") > -1 || lblbeforeonsaleFromdate.Text.ToString().IndexOf("01/01/1900") > -1 || lblbeforeonsaleFromdate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblbeforeonsaleFromdate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblbeforeonsaleFromdate.Text))
                {
                    lblbeforeonsaleFromdate.Text = (Convert.ToDateTime(lblbeforeonsaleFromdate.Text)).ToString("MM/dd/yyyy");
                }

                if (!String.IsNullOrEmpty(lblbeforeonsaleTodate.Text) && (lblbeforeonsaleTodate.Text.ToString().IndexOf("01/01/0001") > -1 || lblbeforeonsaleTodate.Text.ToString().IndexOf("1/1/0001") > -1 || lblbeforeonsaleTodate.Text.ToString().IndexOf("01/01/1900") > -1 || lblbeforeonsaleTodate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblbeforeonsaleTodate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblbeforeonsaleTodate.Text))
                {
                    lblbeforeonsaleTodate.Text = (Convert.ToDateTime(lblbeforeonsaleTodate.Text)).ToString("MM/dd/yyyy");
                }


                if (!String.IsNullOrEmpty(lblonsaleFromdate.Text) && (lblonsaleFromdate.Text.ToString().IndexOf("01/01/0001") > -1 || lblonsaleFromdate.Text.ToString().IndexOf("1/1/0001") > -1 || lblonsaleFromdate.Text.ToString().IndexOf("01/01/1900") > -1 || lblonsaleFromdate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblonsaleFromdate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblonsaleFromdate.Text))
                {
                    lblonsaleFromdate.Text = (Convert.ToDateTime(lblonsaleFromdate.Text)).ToString("MM/dd/yyyy");
                }

                if (!String.IsNullOrEmpty(lblonsaleTodate.Text) && (lblonsaleTodate.Text.ToString().IndexOf("01/01/0001") > -1 || lblonsaleTodate.Text.ToString().IndexOf("1/1/0001") > -1 || lblonsaleTodate.Text.ToString().IndexOf("01/01/1900") > -1 || lblonsaleTodate.Text.ToString().IndexOf("1/1/1900") > -1))
                {
                    lblonsaleTodate.Text = "";
                }
                else if (!String.IsNullOrEmpty(lblonsaleTodate.Text))
                {
                    lblonsaleTodate.Text = (Convert.ToDateTime(lblonsaleTodate.Text)).ToString("MM/dd/yyyy");
                }

            }
        }
    }
}