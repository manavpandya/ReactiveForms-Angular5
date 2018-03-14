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
    public partial class AllTimeAverageRevenue : System.Web.UI.UserControl
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

        protected void btnToday_Click(object sender, EventArgs e)
        {
            CreateChart("ByDay");
        }

        protected void btnWeek_Click(object sender, EventArgs e)
        {
            CreateChart("ByWeek");
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CreateChart("BySelect");
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
            double TotalRevenu = 0;
            ds = adminComponent.GetAllTimeAverageRevenue(DBAction, StartDate, EndDate);
            string str = string.Empty;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                str = str + "[['Duration','AverageRevenue'],";
                int cnt = ds.Tables[0].Rows.Count;
                int i = 1;
                if (cnt > 0)
                {
                    TotalRevenu = Convert.ToDouble(Convert.ToString(ds.Tables[0].Rows[cnt - 1]["AverageRevenue"]));
                }
                foreach (DataRow item in ds.Tables[0].Rows)
                {


                    str = str + "['" + item["Duration"].ToString() + "'," + Convert.ToString(item["AverageRevenue"]) + "]";
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
                str = str + "[['Duration','Revenue'],";
                str = str + "['No Data',0]";
                str = str + "]";
            }


            lblRevenueAverage.Text = "$" + Math.Round(TotalRevenu, 2).ToString("#.##");
            lblRevenueAverage.ToolTip = "Total Revenue";

            strScript += "google.load('visualization', '1', { packages: ['corechart'] });";
            strScript += "google.setOnLoadCallback(drawChartavg);";
            strScript += "Sys.Application.add_load(drawChartavg);";
            strScript += "function drawChartavg() {";
            strScript += "var data = google.visualization.arrayToDataTable(" + str + ");";
            string hAxis = string.Empty;
            if (DBAction == "BySelect")
            {
                hAxis = "Record between " + StartDate.Date.ToString("MM/dd/yyyy") + " to " + EndDate.Date.ToString("MM/dd/yyyy") + " date";
            }
            else if (DBAction == "ByDay")
            {
                hAxis = "Today";
            }
            else if (DBAction == "ByWeek")
            {
                hAxis = "Week of Currrent Month";
            }
            else if (DBAction == "ByMonth")
            {
                hAxis = "Currrent Month";
            }
            else if (DBAction == "ByQuarterly")
            {
                hAxis = "Quarter";
            }
            else if (DBAction == "ByHalfYear")
            {
                hAxis = "Six Month of year";
            }
            else if (DBAction == "ByYear")
            {
                hAxis = "Yearly";
            }

            strScript += "var options = {title: 'All Time Average Revenue',pointSize: 5,colors:['green','#48D56E'],vAxis: {title: 'Order Revenue', minValue: 0,format:'$####', titleTextStyle: {color: '#000000'}},hAxis:{title: '" + hAxis + "', titleTextStyle: {color: '#000000'}}};";
            strScript += "var chart = new google.visualization.AreaChart(document.getElementById('AverageRevenuediv'));";
            strScript += "chart.draw(data, options);";
            strScript += "}";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Test1", strScript, true);
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
    }
}