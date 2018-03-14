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
    public partial class OrderRevenue_ByCountry : System.Web.UI.UserControl
    {
        AdminComponent adminComponent = new AdminComponent();
        CountryComponent countrycomponent = new CountryComponent();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/searchgo.gif) no-repeat transparent; width: 32px; height: 23px; border:none;cursor:pointer;");
                BindCountry();
                CreateChart("ByYear");
            }
        }

        private void BindCountry()
        {
            CountryComponent objCountry = new CountryComponent();
            DataSet storeDetail = objCountry.GetAllCountries();
            if (storeDetail.Tables[0].Rows.Count > 0 && storeDetail != null)
            {
                ddlCountry.DataSource = storeDetail;
                ddlCountry.DataTextField = "Name";
                ddlCountry.DataValueField = "CountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("All Country", "0"));
                ddlCountry.SelectedIndex = 0;
            }
            else
            {
                ddlCountry.Items.Insert(0, new ListItem("All Country", "0"));
                ddlCountry.SelectedIndex = 0;
            }

        }

        private void BindState(string Country, string DbAction)
        {
            //StateComponent objstate = new StateComponent();
            DataSet ds = new DataSet();
            ds = adminComponent.GetOrderRevenuByCountryState(DbAction, DateTime.Now, DateTime.Now, Country, "state");
            if (ds.Tables[0].Rows.Count > 0 && ds != null && Country.ToLower() != "all country")
            {
                ddlState.DataSource = ds;
                ddlState.DataTextField = "BillingState";
                ddlState.DataValueField = "BillingState";
                ddlState.DataBind();
            }
            ddlState.Items.Insert(0, new ListItem("State", "0"));
            ddlState.SelectedIndex = 0;
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
                string Country = Convert.ToString(ddlCountry.SelectedItem.Text);
                string State = Convert.ToString(ddlState.SelectedItem.Text);
                ds = adminComponent.GetOrderRevenuByCountryState(DBAction, StartDate, EndDate, Country, State);
                string str = string.Empty;
                int TotalCustomer = 0;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    str = str + "[['Duration','Total Order'],";
                    int cnt = ds.Tables[0].Rows.Count;
                    int i = 1;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TotalCustomer = TotalCustomer + Convert.ToInt32(Convert.ToString(item["TotalOrder"]));
                        if (Country.ToLower() == "all country")
                        {
                            str = str + "['" + item["country"].ToString() + "'," + item["TotalOrder"].ToString() + "]";
                        }
                        else
                        {
                            str = str + "['" + item["BillingState"].ToString() + "'," + item["TotalOrder"].ToString() + "]";
                        }
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
                    str = str + "[['Country','Order'],";
                    str = str + "['No Data',0]";
                    str = str + "]";
                }

                lblTotalOrders.Text = TotalCustomer.ToString();
                lblTotalOrders.ToolTip = "Total Orders";

                strScript += "google.load('visualization', '1', { packages: ['geochart'] });";
                strScript += "google.setOnLoadCallback(drawChartOrderRevenue);";
                strScript += "Sys.Application.add_load(drawChartOrderRevenue);";
                strScript += "function drawChartOrderRevenue() {";
                strScript += "var data = google.visualization.arrayToDataTable(" + str + ");";
                if (Country.ToLower() != "all country")
                {
                    string Code = countrycomponent.GetCountryCodeByName(ddlCountry.SelectedValue.ToString());
                    if (Code != null && Code != "")
                    {
                        strScript += "var options = {region:'" + Code + "',resolution:'provinces',};";
                    }
                    else
                    {
                        strScript += "var options = {};";
                    }
                }
                else
                {
                    strScript += "var options = {};";
                }
                strScript += "var chart = new google.visualization.GeoChart(document.getElementById('OrderRevenuechartdiv'));";
                strScript += "chart.draw(data, options);";
                strScript += "}";

                ScriptManager.RegisterStartupScript(upnlOrderRevenueCountry, upnlOrderRevenueCountry.GetType(), "Order Revenue", strScript, true);

            }
            catch
            {

            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateChart("ByYear");
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindState(Convert.ToString(ddlCountry.SelectedItem), "ByYear");
            CreateChart("ByYear");
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