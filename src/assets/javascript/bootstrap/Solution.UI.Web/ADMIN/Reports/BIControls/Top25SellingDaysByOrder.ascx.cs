using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Reports.BIControls
{
    public partial class Top25SellingDaysByOrder : System.Web.UI.UserControl
    {
        AdminComponent adminComponent = new AdminComponent();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateChart("ByYear");
            }
        }

        protected void btnmonth_Click(object sender, EventArgs e)
        {
            CreateChart("ByMonth");
        }

        protected void btnQuarter_Click(object sender, EventArgs e)
        {
            CreateChart("ByQuarterly");
        }

        protected void btnHalfyear_Click(object sender, EventArgs e)
        {
            CreateChart("ByHalfYear");
        }

        protected void btnYear_Click(object sender, EventArgs e)
        {
            CreateChart("ByYear");
        }

        public void CreateChart(string DBAction)
        {
            try
            {
                ChangeSelectedbtn(DBAction);
                DataSet ds = new DataSet();
                string strScript = string.Empty;
                ds = adminComponent.GedtTop25SellingDaysByOrder(DBAction);
                string str = string.Empty;
                decimal TotalRevenue = 0;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    str = str + "[['Order Date','Orders'],";
                    int cnt = ds.Tables[0].Rows.Count;
                    int i = 1;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TotalRevenue = TotalRevenue + Convert.ToDecimal(Convert.ToString(item["Orders"]));

                        str = str + "['" + item["OrderDate"].ToString() + "'," + item["Orders"].ToString() + "]";
                        if (i != cnt)
                        {
                            str = str + ",";
                        }
                        i++;
                    }
                    str = str + "]";
                }
                else
                {
                    str = str + "[['No Data','Orders'],";
                    str = str + "['No Data',1]";
                    str = str + "]";
                }

                strScript += "google.load('visualization', '1', { packages: ['corechart'] });";
                strScript += "google.setOnLoadCallback(Top25SellingDaysByOrderDiv);";
                strScript += "Sys.Application.add_load(Top25SellingDaysByOrderDiv);";
                strScript += "function Top25SellingDaysByOrderDiv() {";
                strScript += "var data = google.visualization.arrayToDataTable(" + str + ");";
                strScript += "var options = {title: 'Top 10 Selling Days by Order',pieHole: 0.4,is3D:true};";
                strScript += "var chart = new google.visualization.PieChart(document.getElementById('Top25SellingDaysByOrderDiv'));";
                strScript += "chart.draw(data, options);";
                strScript += "}";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "All Time Top 25 Products", strScript, true);
            }
            catch (Exception)
            {

            }
        }

        private void ChangeSelectedbtn(string btnid)
        {
            btnHalfyear.CssClass = "btn-unchecked";
            btnmonth.CssClass = "btn-unchecked";
            btnQuarter.CssClass = "btn-unchecked";
            btnYear.CssClass = "btn-unchecked";
            if (btnid == "ByHalfYear")
            {
                btnHalfyear.CssClass = "btn-checked";
            }
            else if (btnid == "ByMonth")
            {
                btnmonth.CssClass = "btn-checked";
            }
            else if (btnid == "ByQuarterly")
            {
                btnQuarter.CssClass = "btn-checked";
            }
            else if (btnid == "ByYear")
            {
                btnYear.CssClass = "btn-checked";
            }
        }
    }
}