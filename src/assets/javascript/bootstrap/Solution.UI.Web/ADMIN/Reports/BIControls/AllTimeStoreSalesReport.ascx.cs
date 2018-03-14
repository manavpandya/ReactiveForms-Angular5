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
    public partial class AllTimeStoreSalesReport : System.Web.UI.UserControl
    {
        AdminComponent adminComponent = new AdminComponent();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/searchgo.gif) no-repeat transparent; width: 32px; height: 23px; border:none;cursor:pointer;");
                CreateChart("ByYear");
            }
        }


        public void CreateChart(string DBAction)
        {
            ChangeSelectedbtn(DBAction);
            DateTime StartDate = System.DateTime.Now;
            DateTime EndDate = System.DateTime.Now;
            if (DBAction == "BySelect")
            {
                StartDate = Convert.ToDateTime(txtFromDate.Text);
                EndDate = Convert.ToDateTime(txtToDate.Text);
            }

            DataSet ds = new DataSet();
            string strScript = string.Empty;

            ds = adminComponent.GetStoreSalesReport(DBAction, StartDate, EndDate);
            string str = string.Empty;
            str = " data.addRows([";
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {

                    str = str + "['" + item["StoreName"].ToString() + "', " + Convert.ToString(item["TotalOrder"]) + ",  {v: " + Convert.ToString(item["TotalRevenue"]) + ", f: '$" + Convert.ToString(item["TotalRevenue"]) + "'}],";
                }
            }
            else
            {
                str = str + "['No Data',0,0]";
            }
            str = str + "]);";

            strScript += "google.load('visualization', '1', { packages: ['table'] });";
            strScript += "google.setOnLoadCallback(StoreSalesReportdrawChart);";
            strScript += "Sys.Application.add_load(StoreSalesReportdrawChart);";
            strScript += "function StoreSalesReportdrawChart() {";


            strScript += "var data = new google.visualization.DataTable();";

            strScript += " data.addColumn('string', 'Store Name');";
            strScript += "data.addColumn('number', 'Total Order');";
            strScript += "data.addColumn('number', 'Total Revenue');";


            strScript += str;

            strScript += "var table = new google.visualization.Table(document.getElementById('StoreSalesReportchartdiv'));";

            strScript += " table.draw(data, {showRowNumber: true});";

            strScript += "}";
            ScriptManager.RegisterStartupScript(upnlStoreSalesReport, upnlStoreSalesReport.GetType(), "Store Wise Sales Report", strScript, true);
        }
        private void ChangeSelectedbtn(string btnid)
        {
            btnHalfyear.CssClass = "btn-unchecked";
            btnmonth.CssClass = "btn-unchecked";
            btnQuarter.CssClass = "btn-unchecked";
            btnToday.CssClass = "btn-unchecked";
            btnWeek.CssClass = "btn-unchecked";
            btnYear.CssClass = "btn-unchecked";
            if (btnid != "BySelect")
            {
                txtFromDate.Text = "";
                txtToDate.Text = "";
            }
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
            else if (btnid == "ByDay")
            {
                btnToday.CssClass = "btn-checked";
            }
            else if (btnid == "ByWeek")
            {
                btnWeek.CssClass = "btn-checked";
            }
            else if (btnid == "ByYear")
            {
                btnYear.CssClass = "btn-checked";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CreateChart("BySelect");
        }

        protected void btnYear_Click(object sender, EventArgs e)
        {
            CreateChart("ByYear");
        }

        protected void btnHalfyear_Click(object sender, EventArgs e)
        {
            CreateChart("ByHalfYear");
        }

        protected void btnQuarter_Click(object sender, EventArgs e)
        {
            CreateChart("ByQuarterly");
        }

        protected void btnmonth_Click(object sender, EventArgs e)
        {
            CreateChart("ByMonth");
        }

        protected void btnWeek_Click(object sender, EventArgs e)
        {
            CreateChart("ByWeek");
        }

        protected void btnToday_Click(object sender, EventArgs e)
        {
            CreateChart("ByDay");
        }

    }
}