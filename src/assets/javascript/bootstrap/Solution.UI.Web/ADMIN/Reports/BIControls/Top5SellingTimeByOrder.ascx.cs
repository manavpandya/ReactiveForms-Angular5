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
    public partial class Top5SellingTimeByOrder : System.Web.UI.UserControl
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
                ds = adminComponent.GetAllTimeTop3SellingTime_ByOrder(DBAction, StartDate, EndDate);
                string str = string.Empty;
                int TotalCustomer = 0;
                int MaxValue = 0;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    str = str + "[['Duration','TotalOrder'],";
                    int cnt = ds.Tables[0].Rows.Count;
                    int i = 1;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TotalCustomer = TotalCustomer + Convert.ToInt32(Convert.ToString(item["TotalOrder"]));
                        if (MaxValue == 0)
                        {
                            MaxValue = TotalCustomer;
                        }
                        str = str + "['" + item["Duration"].ToString().Replace("'", "") + "'," + item["TotalOrder"].ToString() + "]";
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
                    str = str + "[['No Data','Revenue','TotalOrder'],";
                    str = str + "['No Data',0,1]";
                    str = str + "]";
                }
                MaxValue = MaxValue + 25;
                //ldlTotalTop25Products.Text = TotalCustomer.ToString();
                //ldlTotalTop25Products.ToolTip = "Total Quantity";

                strScript += "google.load('visualization', '1', { packages: ['corechart'] });";
                strScript += "google.setOnLoadCallback(top5timesOrdersdiv);";
                strScript += "Sys.Application.add_load(top5timesOrdersdiv);";
                strScript += "function top5timesOrdersdiv() {";
                strScript += "var data = google.visualization.arrayToDataTable(" + str + ");";
                strScript += "var options = {title: 'Top 3 Selling Time by Order',pieHole: 0.4,is3D:false};";


                //strScript += "var options = {title: 'Top 3 Selling Time by Order', hAxis: {title: 'Revenue'},vAxis: {title: 'Total Order',minValue:'0',maxValue:'" + MaxValue + "'},bubble: {stroke:'#F1CA3A',fill:'#F1CA3A',textStyle: {fontSize: 11}}};";


                strScript += "var chart = new google.visualization.PieChart(document.getElementById('top5timesOrdersdiv'));";
                strScript += "chart.draw(data, options);";
                strScript += "};";
                Random rd = new Random();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "AllTimetop25Products" + rd.Next(1000).ToString(), strScript, true);
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