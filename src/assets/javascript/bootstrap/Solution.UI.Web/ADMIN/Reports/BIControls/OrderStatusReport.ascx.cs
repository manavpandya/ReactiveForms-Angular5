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
    public partial class OrderStatusReport : System.Web.UI.UserControl
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
                ds = adminComponent.GetOrderStatus(DBAction, StartDate, EndDate);
                string str = string.Empty;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    str = str + "[['OrderStatus','Total Order',{ role: 'style' },{ role: 'annotation' }],";
                    int cnt = ds.Tables[0].Rows.Count;
                    int i = 1;

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        string OrderStatus = item["OrderStatus"].ToString().Replace("'", "");
                        if (OrderStatus != "" && OrderStatus != null)
                        {
                            string LineColor = "#3366CC";
                            switch (OrderStatus.ToLower())
                            {
                                case "canceled":
                                    LineColor = "#ff0000";
                                    break;

                                case "hold":
                                    LineColor = "#DE9D52";
                                    break;

                                case "new":
                                    LineColor = "#ff7f00";
                                    break;

                                case "partially shipped":
                                    LineColor = "#00aaff";
                                    break;

                                case "po generated":
                                    LineColor = "#5293DE";
                                    break;

                                case "shipped":
                                    LineColor = "#348934";
                                    break;
                            }
                            str = str + "['" + item["OrderStatus"].ToString().Replace("'", "") + "'," + item["Total"].ToString() + ",'" + LineColor + "'," + item["Total"].ToString() + "]";
                            if (i != cnt)
                            {
                                str = str + ",";
                            }
                            i++;
                        }
                    }
                    str = str + "]";
                }
                else
                {
                    str = str + "[['No Data','Quantity'],";
                    str = str + "['No Data',1]";
                    str = str + "]";
                }
                strScript += "google.load('visualization', '1', { packages: ['corechart'] });";
                strScript += "google.setOnLoadCallback(orderstatusdiv);";
                strScript += "Sys.Application.add_load(orderstatusdiv);";
                strScript += "function orderstatusdiv() {";
                strScript += "var data = google.visualization.arrayToDataTable(" + str + ");";
                strScript += "var options = {title: 'Order Status'};";
                strScript += "var chart = new google.visualization.BarChart(document.getElementById('orderstatusdiv'));";
                strScript += "chart.draw(data, options);";
                strScript += "};";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AllTimetop25Products", strScript, true);
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