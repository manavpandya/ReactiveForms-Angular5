using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Data;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class PricequoteList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Getpricequote();
            }
        }

        protected void grpricequotelist_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grpricequotelist.PageIndex = e.NewPageIndex;
            Getpricequote();
        }

        protected void grpricequotelist_OnRowDatabound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblname = (Label)e.Row.FindControl("lblname");
                Label lblproductname = (Label)e.Row.FindControl("lblproductname");
                string Firstname = DataBinder.Eval(e.Row.DataItem, "Firstname").ToString();
                string Lastname = DataBinder.Eval(e.Row.DataItem, "Lastname").ToString();
                lblname.Text = Firstname + " " + Lastname;
                Label lblAssignname = (Label)e.Row.FindControl("lblAssignname");
                if (!string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "Assignname").ToString()))
                    lblAssignname.Text = DataBinder.Eval(e.Row.DataItem, "Assignname").ToString();

              lblproductname.Text=Convert.ToString(CommonComponent.GetScalarCommonData("select name from tb_product where productid=" + DataBinder.Eval(e.Row.DataItem, "Productid").ToString()));
            }
        }

        public void Getpricequote()
        {
            DataSet dspricequotelist = new DataSet();

            dspricequotelist = CommonComponent.GetCommonDataSet("select * from tb_pricequote order by CreatedOn desc");
            if (dspricequotelist != null && dspricequotelist.Tables[0].Rows.Count > 0)
            {
                grpricequotelist.DataSource = dspricequotelist;
                grpricequotelist.DataBind();

            }
            else
            {
                grpricequotelist.DataSource = null;
                grpricequotelist.DataBind();


            }


        }
    }
}