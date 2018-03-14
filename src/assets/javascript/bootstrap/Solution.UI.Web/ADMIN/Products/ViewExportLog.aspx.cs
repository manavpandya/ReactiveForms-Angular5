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
    public partial class ViewExportLog : BasePage
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
            ds = CommonComponent.GetCommonDataSet("Exec GuiGetExportProduct  2,'" + txtsearch.Text.ToString().Replace("'", "''") + "'");
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
    }
}