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
    public partial class AllTimeTop3SellingMonth_ByRevenu : System.Web.UI.UserControl
    {
        AdminComponent adminComponent = new AdminComponent();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateChart("ByYear");
            }
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
                ds = adminComponent.GetTop3SellingMonth_ByRevenue(DBAction);
                string str = string.Empty;
                int MaxValue = 0;
                decimal TotalRevenue = 0;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    str = str + "[['Duration','Revenue','TotalOrder'],";
                    int cnt = ds.Tables[0].Rows.Count;
                    int i = 1;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TotalRevenue = TotalRevenue + Convert.ToDecimal(Convert.ToString(item["TotalRevenu"]));
                        if (MaxValue == 0)
                        {
                            MaxValue = Convert.ToInt32(item["TotalOrder"]);
                        }
                        str = str + "['" + item["Duration"].ToString() + "'," + item["TotalRevenu"].ToString() + "," + item["TotalOrder"].ToString() + "]";
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
                    str = str + "['No Data',0,0]";
                    str = str + "]";
                }

                MaxValue = MaxValue + 50;
                strScript += "google.load('visualization', '1', { packages: ['corechart'] });";
                strScript += "google.setOnLoadCallback(top3SellingMonthRevenue);";
                strScript += "Sys.Application.add_load(top3SellingMonthRevenue);";
                strScript += "function top3SellingMonthRevenue() {";
                strScript += "var data = google.visualization.arrayToDataTable(" + str + ");";
                strScript += "var options = {title: 'Top 3 Selling Month by Revenue', hAxis: {title: 'Total Revenue'},vAxis: {title: 'Total Order',minValue:'0',maxValue:'" + MaxValue + "'},bubble: {stroke:'#F1CA3A',fill:'#F1CA3A',textStyle: {fontSize: 11}}};";
                strScript += "var chart = new google.visualization.BubbleChart(document.getElementById('Top3SellingMonthRevenueDiv'));";
                strScript += "chart.draw(data, options);";
                strScript += "}";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "All Time Top 3 Month By Revenue", strScript, true);
            }
            catch (Exception)
            {

            }
        }

        private void ChangeSelectedbtn(string btnid)
        {
            btnHalfyear.CssClass = "btn-unchecked";
            btnYear.CssClass = "btn-unchecked";
            if (btnid == "ByHalfYear")
            {
                btnHalfyear.CssClass = "btn-checked";
            }
            else if (btnid == "ByYear")
            {
                btnYear.CssClass = "btn-checked";
            }
        }
    }
}