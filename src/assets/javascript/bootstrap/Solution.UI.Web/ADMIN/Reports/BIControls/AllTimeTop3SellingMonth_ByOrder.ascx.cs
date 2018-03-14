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
    public partial class AllTimeTop3SellingMonth_ByOrder : System.Web.UI.UserControl
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
                int MaxValue = 0;
                string strScript = string.Empty;
                ds = adminComponent.GetTop3SellingMonth_ByOrder(DBAction);
                string str = string.Empty;
                int TotalOrder = 0;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    str = str + "[['Duration','Revenue','Total Order'],";
                    int cnt = ds.Tables[0].Rows.Count;
                    int i = 1;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TotalOrder = TotalOrder + Convert.ToInt32(Convert.ToString(item["TotalOrder"]));
                        if (MaxValue == 0)
                        {
                            MaxValue = TotalOrder;
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
                    str = str + "[['No Data','Revenue','Total Order'],";
                    str = str + "['No Data',0,0]";
                    str = str + "]";
                }
                MaxValue = MaxValue + 25;
                //ldlTop25SellingDays.Text = "$" + TotalRevenue.ToString();
                //ldlTop25SellingDays.ToolTip = "Total Revenue";
                strScript += "google.load('visualization', '1', { packages: ['corechart'] });";
                strScript += "google.setOnLoadCallback(top3SellingMonthOrder);";
                strScript += "Sys.Application.add_load(top3SellingMonthOrder);";
                strScript += "function top3SellingMonthOrder() {";
                strScript += "var data = google.visualization.arrayToDataTable(" + str + ");";
                //strScript += "var options = {title: 'Top 3 Selling Month By Order',pieHole: 0.4,is3D:false};";

                strScript += "var options = {title: 'Top 3 Selling Month by Order',hAxis: {title: 'Revenue'},vAxis: {title: 'Total Order',minValue:'0',maxValue:'" + MaxValue + "'},bubble: {textStyle: {fontSize: 11}}};";

                strScript += "var chart = new google.visualization.BubbleChart(document.getElementById('Top3SellingMonthOrderDiv'));";
                strScript += "chart.draw(data, options);";
                strScript += "}";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "All Time Top 3 Month By Order", strScript, true);
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