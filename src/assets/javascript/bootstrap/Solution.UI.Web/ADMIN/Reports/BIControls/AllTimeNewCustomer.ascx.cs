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
    public partial class AllTimeNewCustomer : System.Web.UI.UserControl
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CreateChart("BySelect");
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
            ds = adminComponent.GetAllTimeNewCustomer(DBAction, StartDate, EndDate);
            string str = string.Empty;
            int TotalCustomer = 0;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                str = str + "[['Duration','Total Customer', { role: 'style' }, { role: 'annotation' }],";
                int cnt = ds.Tables[0].Rows.Count;
                int i = 1;
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    TotalCustomer = TotalCustomer + Convert.ToInt32(Convert.ToString(item["Customers"]));

                    str = str + "['" + item["Duration"].ToString() + "'," + item["Customers"].ToString() + ",'#5F9654'," + item["Customers"].ToString() + "]";
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
                str = str + "[['Duration','Total Customer', { role: 'style' }, { role: 'annotation' }],";
                str = str + "['No Data',0,'#5F9654',0]";
                str = str + "]";
            }

            ldlTotalCustomer.Text = TotalCustomer.ToString();
            ldlTotalCustomer.ToolTip = "Total Customer";

            strScript += "google.load('visualization', '1', { packages: ['corechart'] });";
            strScript += "google.setOnLoadCallback(drawChart2);";
            strScript += "Sys.Application.add_load(drawChart2);";
            strScript += "function drawChart2() {";
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

            strScript += "var options = {title: 'All Time Customers', colors:['#5F9654','#5F9654'],vAxis: {title: 'Total Customer', minValue: 0,titleTextStyle: {color: '#000000'}},hAxis:{title: '" + hAxis + "', titleTextStyle: {color: '#000000'}}};";
            strScript += "var chart = new google.visualization.ColumnChart(document.getElementById('NewCustomerchartdiv'));";
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