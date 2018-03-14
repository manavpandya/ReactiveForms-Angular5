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
    public partial class ProductDataSummary : System.Web.UI.UserControl
    {
        AdminComponent adminComponent = new AdminComponent();
        decimal RevenueTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/searchgo.gif) no-repeat transparent; width: 32px; height: 23px; border:none;cursor:pointer;");
                BindStore();
                CreateChart("ByYear");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CreateChart("BySelect");
        }

        private void CreateChart(string DBAction)
        {
            if (DBAction == null || DBAction == "")
            {
                DBAction = "ByYear";
            }
            hdnProductSummary.Value = DBAction;
            try
            {

                int StoreId = Convert.ToInt32(Convert.ToString(ddlStore1.SelectedValue));
                ChangeSelectedbtn(DBAction);
                DateTime StartDate = System.DateTime.Now;
                DateTime EndDate = System.DateTime.Now;
                if (DBAction == "BySelect")
                {
                    StartDate = Convert.ToDateTime(txtFromDate.Text);
                    EndDate = Convert.ToDateTime(txtToDate.Text);
                }
                string VariantName = Convert.ToString(ddlVariant.SelectedItem.Text);

                DataSet ds = new DataSet();
                string strScript = string.Empty;
                int TotalOrder = 0;
                ds = adminComponent.GetProductSummaryByOrder(StoreId, DBAction, Convert.ToString(txtProductName.Text), VariantName);
                string str = string.Empty;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    str = str + "[['Duration','TotalOrder', { role: 'style' }, { role: 'annotation' }],";
                    int cnt = ds.Tables[0].Rows.Count;
                    int i = 1;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TotalOrder = TotalOrder + Convert.ToInt32(item["TotalOrder"]);
                        str = str + "['" + item["Duration"].ToString() + "'," + Convert.ToString(item["TotalOrder"]) + ",'#3366cc'," + Convert.ToString(item["TotalOrder"]) + "]";
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
                    str = str + "[['Duration','TotalOrder', { role: 'style' }, { role: 'annotation' }],";
                    str = str + "['No Data',0,'#3366cc',0]";
                    str = str + "]";
                }
                lblTotalOrder.Text = "<strong>(" + TotalOrder + ")</strong>";
                lblSalesChannel.Text = "Sales Channel";
                strScript += "google.load('visualization', '1', { packages: ['corechart'] });";
                strScript += "google.setOnLoadCallback(ProductSummaryOrderchart);";
                strScript += "Sys.Application.add_load(ProductSummaryOrderchart);";
                strScript += "function ProductSummaryOrderchart() {";
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
                    hAxis = "Current Year";
                }
                else if (DBAction == "ByQuarterly")
                {
                    hAxis = "Quarterly";
                }
                else if (DBAction == "ByHalfYear")
                {
                    hAxis = "Six Month of year";
                }
                else if (DBAction == "ByYear")
                {
                    hAxis = "Yearly";
                }
                strScript += "var options = {animation:{duration: 2000,easing: 'linear',}, title: '',vAxis: {title: 'Total Order', minValue: 0, titleTextStyle: {color: '#000000'}},hAxis:{title: '" + hAxis + "', titleTextStyle: {color: '#000000'}}};";
                strScript += "var chart = new google.visualization.ColumnChart(document.getElementById('ProductSummaryOrderchartdiv'));";
                strScript += "chart.draw(data, options);";
                strScript += "};";

                //// Product Price Summary ////

                DataSet dsPriceSummary = new DataSet();
                double TotalRevenu = 0;
                dsPriceSummary = adminComponent.GetProductSummaryByPrice(StoreId, Convert.ToString(txtProductName.Text), VariantName, DBAction);
                string strPriceSummary = string.Empty;
                if (dsPriceSummary != null && dsPriceSummary.Tables.Count > 0 && dsPriceSummary.Tables[0].Rows.Count > 0)
                {
                    strPriceSummary = strPriceSummary + "[['Duration','Price'],";
                    int cnt = dsPriceSummary.Tables[0].Rows.Count;
                    int i = 1;
                    //if (cnt > 0)
                    //{
                    //    TotalRevenu = Convert.ToDouble(Convert.ToString(dsPriceSummary.Tables[0].Rows[cnt - 1]["AverageRevenue"]));
                    //}
                    foreach (DataRow item in dsPriceSummary.Tables[0].Rows)
                    {


                        strPriceSummary = strPriceSummary + "['" + Convert.ToDateTime(item["Duration"]).ToString("MMM dd,yyyy") + "'," + Convert.ToString(item["Price"]) + "]";
                        if (i != cnt)
                        {
                            strPriceSummary = strPriceSummary + ",";
                        }
                        i++;
                    }
                    strPriceSummary = strPriceSummary + "]";
                }
                else
                {
                    strPriceSummary = strPriceSummary + "[['Duration','Price'],";
                    strPriceSummary = strPriceSummary + "['No Data',0]";
                    strPriceSummary = strPriceSummary + "]";
                }


                //strScript += "google.load('visualization', '1', { packages: ['corechart'] });";
                strScript += "google.setOnLoadCallback(ProductSummaryPrice);";
                strScript += "Sys.Application.add_load(ProductSummaryPrice);";
                strScript += "function ProductSummaryPrice() {";
                strScript += "var data = google.visualization.arrayToDataTable(" + strPriceSummary + ");";
                string hAxis1 = string.Empty;
                if (DBAction == "BySelect")
                {
                    hAxis1 = "Record between " + StartDate.Date.ToString("MM/dd/yyyy") + " to " + EndDate.Date.ToString("MM/dd/yyyy") + " date";
                }
                else if (DBAction == "ByDay")
                {
                    hAxis1 = "Today";
                }
                else if (DBAction == "ByWeek")
                {
                    hAxis1 = "Week of Currrent Month";
                }
                else if (DBAction == "ByMonth")
                {
                    hAxis1 = "Currrent Month";
                }
                else if (DBAction == "ByQuarterly")
                {
                    hAxis1 = "Quarter";
                }
                else if (DBAction == "ByHalfYear")
                {
                    hAxis1 = "Six Month of year";
                }
                else if (DBAction == "ByYear")
                {
                    hAxis1 = "Yearly";
                }

                strScript += "var options = {title: '',pointSize: 5,colors:['green','#48D56E'],vAxis: {title: 'Order Revenue', minValue: 0,format:'$####', titleTextStyle: {color: '#000000'}},hAxis:{title: '" + hAxis1 + "', titleTextStyle: {color: '#000000'}}};";
                strScript += "var chart = new google.visualization.AreaChart(document.getElementById('ProductSummaryPrice'));";
                strScript += "chart.draw(data, options);";
                strScript += "}";

                //// Product Inventory Report ////

                //DataSet dsInventorySummary = new DataSet();
                ////double TotalRevenu = 0;
                //dsInventorySummary = adminComponent.GetAllTimeAverageRevenue(DBAction, StartDate, EndDate);
                //string strInventorySummary = string.Empty;
                //if (dsInventorySummary != null && dsInventorySummary.Tables.Count > 0 && dsInventorySummary.Tables[0].Rows.Count > 0)
                //{
                //    strInventorySummary = strInventorySummary + "[['Duration','AverageRevenue'],";
                //    int cnt = dsPriceSummary.Tables[0].Rows.Count;
                //    int i = 1;
                //    //if (cnt > 0)
                //    //{
                //    //    TotalRevenu = Convert.ToDouble(Convert.ToString(dsInventorySummary.Tables[0].Rows[cnt - 1]["AverageRevenue"]));
                //    //}
                //    foreach (DataRow item in dsInventorySummary.Tables[0].Rows)
                //    {


                //        strInventorySummary = strInventorySummary + "['" + item["Duration"].ToString() + "'," + Convert.ToString(item["AverageRevenue"]) + "]";
                //        if (i != cnt)
                //        {
                //            strInventorySummary = strInventorySummary + ",";
                //        }
                //        i++;
                //    }
                //    strInventorySummary = strInventorySummary + "]";
                //}
                //else
                //{
                //    strInventorySummary = strInventorySummary + "[['Duration','Revenue'],";
                //    strInventorySummary = strInventorySummary + "['No Data',0]";
                //    strInventorySummary = strInventorySummary + "]";
                //}


                ////strScript += "google.load('visualization', '1', { packages: ['corechart'] });";
                //strScript += "google.setOnLoadCallback(ProductSummaryInventory);";
                //strScript += "Sys.Application.add_load(ProductSummaryInventory);";
                //strScript += "function ProductSummaryInventory() {";
                //strScript += "var data = google.visualization.arrayToDataTable(" + strInventorySummary + ");";
                //string hAxisInventory = string.Empty;
                //if (DBAction == "BySelect")
                //{
                //    hAxisInventory = "Record between " + StartDate.Date.ToString("MM/dd/yyyy") + " to " + EndDate.Date.ToString("MM/dd/yyyy") + " date";
                //}
                //else if (DBAction == "ByDay")
                //{
                //    hAxisInventory = "Today";
                //}
                //else if (DBAction == "ByWeek")
                //{
                //    hAxisInventory = "Week of Currrent Month";
                //}
                //else if (DBAction == "ByMonth")
                //{
                //    hAxisInventory = "Currrent Month";
                //}
                //else if (DBAction == "ByQuarterly")
                //{
                //    hAxisInventory = "Quarter";
                //}
                //else if (DBAction == "ByHalfYear")
                //{
                //    hAxisInventory = "Six Month of year";
                //}
                //else if (DBAction == "ByYear")
                //{
                //    hAxisInventory = "Yearly";
                //}

                //strScript += "var options = {title: '',pointSize: 5,colors:['green','#48D56E'],vAxis: {title: 'Order Revenue', minValue: 0, titleTextStyle: {color: '#000000'}},hAxis:{title: '" + hAxisInventory + "', titleTextStyle: {color: '#000000'}}};";
                //strScript += "var chart = new google.visualization.AreaChart(document.getElementById('ProductSummaryInventory'));";
                //strScript += "chart.draw(data, options);";
                //strScript += "}";

                //// Sales Channel Report ////

                DataSet dssc = new DataSet();
                dssc = adminComponent.GetProductSummaryBySalesChanel(DBAction, txtProductName.Text, Convert.ToString(ddlVariant.SelectedItem.Text));
                string strsc = string.Empty;
                strsc = " data.addRows([";
                if (dssc != null && dssc.Tables.Count > 0 && dssc.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in dssc.Tables[0].Rows)
                    {
                        strsc = strsc + "['" + item["StoreName"].ToString() + "', " + Convert.ToString(item["SoldQuantity"]) + ",  {v: " + Convert.ToString(item["TotalRevenue"]) + ", f: '$" + Convert.ToString(item["TotalRevenue"]) + "'}," + Convert.ToString(item["TotalOrder"]) + "],";
                    }
                }
                else
                {
                    strsc = strsc + "['No Data',0,0,0]";
                }
                strsc = strsc + "]);";

                strScript += "google.load('visualization', '1', { packages: ['table'] });";
                strScript += "google.setOnLoadCallback(ProductSummaryBySalesChannel);";
                strScript += "Sys.Application.add_load(ProductSummaryBySalesChannel);";
                strScript += "function ProductSummaryBySalesChannel() {";
                strScript += "var data = new google.visualization.DataTable();";
                strScript += " data.addColumn('string', 'StoreName');";
                strScript += " data.addColumn('number', 'SoldQuantity');";
                strScript += "data.addColumn('number', 'Total Revenue');";
                strScript += "data.addColumn('number', 'Total Order');";
                strScript += strsc;
                strScript += "var table = new google.visualization.Table(document.getElementById('ProductSummaryBySalesChannel'));";
                strScript += " table.draw(data, {showRowNumber: true});";
                strScript += "}";

                Random rd = new Random();
                ScriptManager.RegisterStartupScript(upnlProductDataSummary, upnlProductDataSummary.GetType(), "TotalOrderByProduct" + rd.Next(1000).ToString(), strScript, true);
            }
            catch (Exception e)
            {
                string str = e.ToString();
            }
        }

        private void BindVariant()
        {
            string search = txtProductName.Text;
            int StoreId = Convert.ToInt32(Convert.ToString(ddlStore1.SelectedValue));
            DataSet VariantDetail = adminComponent.GetVariantFromParent(StoreId, search);
            if (VariantDetail.Tables.Count > 0 && VariantDetail.Tables[0].Rows.Count > 0 && VariantDetail != null)
            {
                ddlVariant.Items.Clear();
                ddlVariant.DataSource = VariantDetail;
                ddlVariant.DataTextField = "VariantName";
                ddlVariant.DataValueField = "VariantID";
                ddlVariant.DataBind();
                ddlVariant.Items.Insert(0, new ListItem("Select Variant", "0"));
                ddlVariant.SelectedIndex = 0;
            }
            else
            {
                ddlVariant.Items.Clear();
                ddlVariant.Items.Insert(0, new ListItem("Select Variant", "0"));
                ddlVariant.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void BindStore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore1.DataSource = storeDetail;
                ddlStore1.DataTextField = "StoreName";
                ddlStore1.DataValueField = "StoreID";
                ddlStore1.DataBind();
                ddlStore1.SelectedIndex = 0;
            }
            else
            {
                ddlStore1.Items.Insert(0, new ListItem("All Stores", "0"));
                ddlStore1.SelectedIndex = 0;
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

        protected void ddlStore1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVariant();
            CreateChart(Convert.ToString(hdnProductSummary.Value));
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            BindVariant();
            CreateChart(Convert.ToString(hdnProductSummary.Value));
        }

        protected void ddlVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateChart(Convert.ToString(hdnProductSummary.Value));
        }
    }
}