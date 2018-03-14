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
    public partial class AllTimeRevenue : System.Web.UI.UserControl
    {
        AdminComponent adminComponent = new AdminComponent();
        decimal RevenueTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/searchgo.gif) no-repeat transparent; width: 32px; height: 23px; border:none;cursor:pointer;");
                CreateChart("ByYear");
                //CreateChart1();
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
            try
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
                ds = adminComponent.GetAllTimeRevenue(DBAction, StartDate, EndDate);
                string str = string.Empty;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    str = str + "[['Duration','Revenue', { role: 'style' }, { role: 'annotation' }],";
                    int cnt = ds.Tables[0].Rows.Count;
                    int i = 1;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TotalRevenu = TotalRevenu + Convert.ToDouble(Convert.ToString(item["Revenue"]));

                        str = str + "['" + item["Duration"].ToString() + "'," + Convert.ToString(item["Revenue"]) + ",'#3366cc'," + Convert.ToString(item["Revenue"]) + "]";
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
                    str = str + "[['Duration','Revenue', { role: 'style' }, { role: 'annotation' }],";
                    str = str + "['No Data',0,'#3366cc',0]";
                    str = str + "]";
                }

                ltalltimerevenue.Text = "";
                lblTotalRevenue.Text = "$" + Math.Round(TotalRevenu, 2).ToString("#.##");
                if (TotalRevenu == 0)
                {
                    lblTotalRevenue.Text = "$0.00";
                }
                lblTotalRevenue.ToolTip = "Total Revenue";

                strScript += "google.load('visualization', '1', { packages: ['corechart'] });";

                strScript += "google.setOnLoadCallback(drawChart);";
                strScript += "Sys.Application.add_load(drawChart);";
                strScript += "function drawChart() {";
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
                strScript += "var options = {animation:{duration: 2000,easing: 'linear',}, title: 'All Time Revenue',vAxis: {title: 'Order Revenue', minValue: 0,format:'$#####', titleTextStyle: {color: '#000000'}},hAxis:{title: '" + hAxis + "', titleTextStyle: {color: '#000000'}}};";
                strScript += "var chart = new google.visualization.ColumnChart(document.getElementById('chartdiv'));";
                strScript += "chart.draw(data, options);";
                strScript += "};";
                Random rd = new Random();
                ScriptManager.RegisterStartupScript(upnlAllTimeRevenue, upnlAllTimeRevenue.GetType(), "AllTimeRevenue" + rd.Next(1000).ToString(), strScript, true);
            }
            catch (Exception e)
            {
                string str = e.ToString();
            }
        }

        //public void CreateChart(string DBAction)
        //{
        //    try
        //    {
        //        ChangeSelectedbtn(DBAction);
        //        DateTime StartDate = System.DateTime.Now;
        //        DateTime EndDate = System.DateTime.Now;
        //        if (DBAction == "BySelect")
        //        {
        //            StartDate = Convert.ToDateTime(txtFromDate.Text);
        //            EndDate = Convert.ToDateTime(txtToDate.Text);
        //        }
        //        DataSet ds = new DataSet();
        //        string strScript = string.Empty;
        //        double TotalRevenu = 0;
        //        ds = adminComponent.GetAllTimeRevenue(DBAction, StartDate, EndDate);
        //        string str = string.Empty;
        //        string str1 = string.Empty;
        //        decimal MaxValue = 0;
        //        if (ds != null && ds.Tables[0].Rows.Count > 0)
        //        {

        //            str += "data.addColumn('string', 'N');";
        //            str += "data.addColumn('number', 'Revenue');";
        //            int cnt = ds.Tables[0].Rows.Count;
        //            int incr = 0;
        //            foreach (DataRow item in ds.Tables[0].Rows)
        //            {
        //                str += "data.addRow(['" + item["Duration"].ToString() + "', 0]);";
        //                str1 += "data.setValue(" + incr + ", 1, " + Convert.ToString(item["Revenue"]) + ");";
        //                if (Convert.ToDecimal(item["Revenue"]) > MaxValue)
        //                {
        //                    MaxValue = Convert.ToDecimal(item["Revenue"]);
        //                }
        //                incr = incr + 1;
        //            }
        //        }
        //        else
        //        {
        //            str = str + "[['Duration','Revenue', { role: 'style' }, { role: 'annotation' }],";
        //            str = str + "['No Data',0,'#3366cc',0]";
        //            str = str + "]";
        //        }

        //        ltalltimerevenue.Text = "";
        //        lblTotalRevenue.Text = "$" + Math.Round(TotalRevenu, 2).ToString("#.##");
        //        if (TotalRevenu == 0)
        //        {
        //            lblTotalRevenue.Text = "$0.00";
        //        }
        //        lblTotalRevenue.ToolTip = "Total Revenue";

        //        strScript += "google.load('visualization', '1', { packages: ['corechart'] });";

        //        strScript += "google.setOnLoadCallback(drawChart);";
        //        strScript += "Sys.Application.add_load(drawChart);";
        //        strScript += "function drawChart() {";
        //        strScript += "var data = new google.visualization.DataTable();";
        //        strScript += str;
        //        string hAxis = string.Empty;
        //        if (DBAction == "BySelect")
        //        {
        //            hAxis = "Record between " + StartDate.Date.ToString("MM/dd/yyyy") + " to " + EndDate.Date.ToString("MM/dd/yyyy") + " date";
        //        }
        //        else if (DBAction == "ByDay")
        //        {
        //            hAxis = "Today";
        //        }
        //        else if (DBAction == "ByWeek")
        //        {
        //            hAxis = "Week of Currrent Month";
        //        }
        //        else if (DBAction == "ByMonth")
        //        {
        //            hAxis = "Currrent Month";
        //        }
        //        else if (DBAction == "ByQuarterly")
        //        {
        //            hAxis = "Quarter";
        //        }
        //        else if (DBAction == "ByHalfYear")
        //        {
        //            hAxis = "Six Month of year";
        //        }
        //        else if (DBAction == "ByYear")
        //        {
        //            hAxis = "Yearly";
        //        }
        //        strScript += "var options = {title: 'All Time Revenue',animation: { duration: 1000, easing: 'out', },is3D: true,vAxis: {title: 'Order Revenue',minValue: 0, maxValue: " + MaxValue + ",format:'$#####', titleTextStyle: {color: '#000000'}},hAxis:{title: '" + hAxis + "', titleTextStyle: {color: '#000000'}}};";
        //        strScript += "var chart = new google.visualization.ColumnChart(document.getElementById('chartdiv'));";
        //        strScript += "chart.draw(data, options);";
        //        strScript += str1;
        //        strScript += "chart.draw(data, options);";
        //        strScript += "};";
        //        Random rd = new Random();
        //        ScriptManager.RegisterStartupScript(upnlAllTimeRevenue, upnlAllTimeRevenue.GetType(), "AllTimeRevenue" + rd.Next(1000).ToString(), strScript, true);
        //    }
        //    catch (Exception e)
        //    {
        //        string str = e.ToString();
        //    }
        //}
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