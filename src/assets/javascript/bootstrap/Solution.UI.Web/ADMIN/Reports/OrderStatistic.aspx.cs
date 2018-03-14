using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using System.IO;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using InfoSoftGlobal;
using System.Data.SqlClient;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class OrderStatistic : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgSearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/searchgo.gif";
                BindStore();
                txtOrderFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date));
                txtOrderTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date));
                FillChart(ddlPeriod.SelectedValue.ToString());
                FillChartTax(ddlPeriod.SelectedValue.ToString());
                FillChartRefund(ddlPeriod.SelectedValue.ToString());
                FillMonthChart();
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
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillChart(ddlPeriod.SelectedValue.ToString());
            FillChartTax(ddlPeriod.SelectedValue.ToString());
            FillChartRefund(ddlPeriod.SelectedValue.ToString());
            if (hdnTabid.Value.ToString() == "1")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "OrderSubTabdisplay(" + hdnTabidOrders.Value.ToString() + ");Tabdisplay(" + hdnTabid.Value.ToString() + ");", true);
            }
            else if (hdnTabid.Value.ToString() == "2")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "OrderSubTabdisplayTax(" + hdnTabidOrders.Value.ToString() + ");Tabdisplay(" + hdnTabid.Value.ToString() + ");", true);
            }
            else if (hdnTabid.Value.ToString() == "3")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "OrderSubTabdisplayRefund(" + hdnTabidOrders.Value.ToString() + ");Tabdisplay(" + hdnTabid.Value.ToString() + ");", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "OrderSubTabdisplay(1);Tabdisplay(1);", true);
            }
            FillMonthChart();
        }

        /// <summary>
        /// Fill Charts with Data for Order and Amount
        /// </summary>
        /// <param name="Duration">String Duration</param>
        public void FillChart(String Duration)
        {
            try
            {
                string YaxeName = "Orders";
                string XaxeName = "";
                string numberPrefix = "";
                if (Duration == "Day")
                {
                    XaxeName = "Date";
                }
                else if (Duration == "Month")
                {
                    XaxeName = "Month";
                }
                else if (Duration == "Year")
                {
                    XaxeName = "Year";
                }

                DataSet dsChart = new DataSet();
                dsChart = OrderComponent.GetChartDetailsForOrderStatisticReport(Duration, Convert.ToInt32(ddlStore.SelectedValue), Convert.ToDateTime(txtOrderFrom.Text), Convert.ToDateTime(txtOrderTo.Text), "");
                if (dsChart != null && dsChart.Tables.Count > 0 && dsChart.Tables[0].Rows.Count > 0)
                {

                    string[] RandColor = { "ec1f7b", "77113f", "0b0206", "e585e9", "e23be8", "87058c", "c98bf0", "a83bed", "4c047a", "61d3eb", "098eaa", "244d56", "0b9e42", "99a911", "e1ae23", "f58d54", "b24206", "ed6b59", "c5230d" };
                    Random random = new Random();
                    String XMLChart = String.Empty;
                    String XMLChartAmount = String.Empty;
                    for (int i = 0; i < dsChart.Tables[0].Rows.Count; i++)
                    {
                        string stramt = dsChart.Tables[0].Rows[i]["Day"].ToString() + " ," + dsChart.Tables[0].Rows[i]["OrderTotal"].ToString();
                        string strorder = dsChart.Tables[0].Rows[i]["Day"].ToString() + " ," + dsChart.Tables[0].Rows[i]["TotalOrder"].ToString();
                        if (Duration == "Day")
                        {
                            XMLChart += "<set  value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["TotalOrder"]), 2)) + "'" + " name='" + String.Format("{0:MM/dd/yy}", Convert.ToDateTime(dsChart.Tables[0].Rows[i]["Day"])) + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + strorder.ToString() + "' />";
                            XMLChartAmount += "<set value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["OrderTotal"]), 2)) + "'" + " name='" + String.Format("{0:MM/dd/yy}", Convert.ToDateTime(dsChart.Tables[0].Rows[i]["Day"])) + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + stramt.ToString() + "'   />";
                        }
                        else
                        {
                            XMLChart += "<set  value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["TotalOrder"]), 2)) + "'" + " name='" + dsChart.Tables[0].Rows[i]["Day"] + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + strorder.ToString() + "' />";
                            XMLChartAmount += "<set value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["OrderTotal"]), 2)) + "'" + " name='" + dsChart.Tables[0].Rows[i]["Day"] + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + stramt.ToString() + "'   />";
                        }
                    }
                    XMLChart = "-<graph caption='" + XaxeName + " wise Order-Quantity Chart' xAxisName='" + XaxeName + "'  yAxisName='" + YaxeName + "' decimalPrecision='0' " + numberPrefix + " formatNumberScale='0'>" + XMLChart + "</graph>";
                    ltrChartOrder.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_Column3D.swf", "", XMLChart, "myFirst", "1000", "300", false);
                    ltrChartOrder.Text = ltrChartOrder.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");


                    YaxeName = "Amount";
                    XMLChartAmount = "-<graph caption='" + XaxeName + " wise Order-Amount Chart' xAxisName='" + XaxeName + "' numberPrefix='$' yAxisName='" + YaxeName + "' decimalPrecision='2' " + numberPrefix + " formatNumberScale='0' >" + XMLChartAmount + "</graph>";
                    ltrChartAmount.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_Column3D.swf", "", XMLChartAmount, "myFirst", "1000", "300", false);
                    ltrChartAmount.Text = ltrChartAmount.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");

                }
                else
                {
                    ltrChartOrder.Text = "<div style=\"height:180px;width:500px; text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">Order not available in requested duration.</div>";
                    ltrChartAmount.Text = "<div style=\"height:180px;width:500px; text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">Order not available in requested duration.</div>";
                }
            }
            catch { }
        }

        /// <summary>
        /// Fill Charts For TAX with Data for Order and Amount
        /// </summary>
        /// <param name="Duration">String Duration</param>
        public void FillChartTax(String Duration)
        {
            try
            {
                string YaxeName = "Orders";
                string XaxeName = "";
                string numberPrefix = "";
                if (Duration == "Day")
                {
                    XaxeName = "Date";
                }
                else if (Duration == "Month")
                {
                    XaxeName = "Days";
                }
                else if (Duration == "Year")
                {
                    XaxeName = "Months";
                }

                DataSet dsChart = new DataSet();
                dsChart = OrderComponent.GetChartDetailsForOrderStatisticReport(Duration, Convert.ToInt32(ddlStore.SelectedValue), Convert.ToDateTime(txtOrderFrom.Text), Convert.ToDateTime(txtOrderTo.Text), "CAPTURED");
                if (dsChart != null && dsChart.Tables.Count > 0 && dsChart.Tables[0].Rows.Count > 0)
                {

                    string[] RandColor = { "ec1f7b", "77113f", "0b0206", "e585e9", "e23be8", "87058c", "c98bf0", "a83bed", "4c047a", "61d3eb", "098eaa", "244d56", "0b9e42", "99a911", "e1ae23", "f58d54", "b24206", "ed6b59", "c5230d" };
                    Random random = new Random();
                    String XMLChart = String.Empty;
                    String XMLChartAmount = String.Empty;
                    for (int i = 0; i < dsChart.Tables[0].Rows.Count; i++)
                    {


                        string stramt = dsChart.Tables[0].Rows[i]["Day"].ToString() + " ," + dsChart.Tables[0].Rows[i]["OrderTotal"].ToString();
                        string strorder = dsChart.Tables[0].Rows[i]["Day"].ToString() + " ," + dsChart.Tables[0].Rows[i]["TotalOrder"].ToString();
                        XMLChart += "<set  value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["TotalOrder"]), 2)) + "'" + " name='" + dsChart.Tables[0].Rows[i]["Day"] + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + strorder.ToString() + "' />";
                        XMLChartAmount += "<set value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["OrderTotal"]), 2)) + "'" + " name='" + dsChart.Tables[0].Rows[i]["Day"] + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + stramt.ToString() + "'   />";
                    }


                    XMLChart = "-<graph caption='" + XaxeName + " wise Order-Quantity Chart' xAxisName='" + XaxeName + "'  yAxisName='" + YaxeName + "' decimalPrecision='0' " + numberPrefix + " formatNumberScale='0'>" + XMLChart + "</graph>";
                    ltrChartOrderTax.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_Column3D.swf", "", XMLChart, "myFirst", "1000", "300", false);
                    ltrChartOrderTax.Text = ltrChartOrderTax.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");


                    YaxeName = "Amount";
                    XMLChartAmount = "-<graph caption='" + XaxeName + " wise Order-Amount Chart' xAxisName='" + XaxeName + "' numberPrefix='$' yAxisName='" + YaxeName + "' decimalPrecision='2' " + numberPrefix + " formatNumberScale='0' >" + XMLChartAmount + "</graph>";
                    ltrChartAmountTax.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_Column3D.swf", "", XMLChartAmount, "myFirst", "1000", "300", false);
                    ltrChartAmountTax.Text = ltrChartAmountTax.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");


                }
                else
                {
                    ltrChartOrderTax.Text = "<div style=\"height:180px;width:500px; text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">Tax not  available in requested duration.</div>";
                    ltrChartAmountTax.Text = "<div style=\"height:180px;width:500px; text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">Tax not available in requested duration.</div>";
                }
            }
            catch { }
        }

        /// <summary>
        /// Fill Charts For Refund with Data for Order and Amount
        /// </summary>
        /// <param name="Duration">String Duration</param>
        public void FillChartRefund(String Duration)
        {
            try
            {
                string YaxeName = "Orders";
                string XaxeName = "";
                string numberPrefix = "";
                if (Duration == "Day")
                {
                    XaxeName = "Date";
                }
                else if (Duration == "Month")
                {
                    XaxeName = "Days";
                }
                else if (Duration == "Year")
                {
                    XaxeName = "Months";
                }

                DataSet dsChart = new DataSet();
                dsChart = OrderComponent.GetChartDetailsForOrderStatisticReport(Duration, Convert.ToInt32(ddlStore.SelectedValue), Convert.ToDateTime(txtOrderFrom.Text), Convert.ToDateTime(txtOrderTo.Text), "REFUNDED");
                if (dsChart != null && dsChart.Tables.Count > 0 && dsChart.Tables[0].Rows.Count > 0)
                {

                    string[] RandColor = { "ec1f7b", "77113f", "0b0206", "e585e9", "e23be8", "87058c", "c98bf0", "a83bed", "4c047a", "61d3eb", "098eaa", "244d56", "0b9e42", "99a911", "e1ae23", "f58d54", "b24206", "ed6b59", "c5230d" };
                    Random random = new Random();
                    String XMLChart = String.Empty;
                    String XMLChartAmount = String.Empty;
                    for (int i = 0; i < dsChart.Tables[0].Rows.Count; i++)
                    {
                        string stramt = dsChart.Tables[0].Rows[i]["Day"].ToString() + " ," + dsChart.Tables[0].Rows[i]["OrderTotal"].ToString();
                        string strorder = dsChart.Tables[0].Rows[i]["Day"].ToString() + " ," + dsChart.Tables[0].Rows[i]["TotalOrder"].ToString();
                        XMLChart += "<set  value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["TotalOrder"]), 2)) + "'" + " name='" + dsChart.Tables[0].Rows[i]["Day"] + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + strorder.ToString() + "' />";
                        XMLChartAmount += "<set value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["OrderTotal"]), 2)) + "'" + " name='" + dsChart.Tables[0].Rows[i]["Day"] + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + stramt.ToString() + "'   />";
                    }
                    XMLChart = "-<graph caption='" + XaxeName + " wise Order-Quantity Chart' xAxisName='" + XaxeName + "'  yAxisName='" + YaxeName + "' decimalPrecision='0' " + numberPrefix + " formatNumberScale='0'>" + XMLChart + "</graph>";
                    ltrChartOrderRefund.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_Column3D.swf", "", XMLChart, "myFirst", "1000", "300", false);
                    ltrChartOrderRefund.Text = ltrChartOrderRefund.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");


                    YaxeName = "Amount";
                    XMLChartAmount = "-<graph caption='" + XaxeName + " wise Order-Amount Chart' xAxisName='" + XaxeName + "' numberPrefix='$' yAxisName='" + YaxeName + "' decimalPrecision='2' " + numberPrefix + " formatNumberScale='0' >" + XMLChartAmount + "</graph>";
                    ltrChartAmountRefund.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_Column3D.swf", "", XMLChartAmount, "myFirst", "1000", "300", false);
                    ltrChartAmountRefund.Text = ltrChartAmountRefund.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");

                }
                else
                {
                    ltrChartOrderRefund.Text = "<div style=\"height:180px;width:500px; text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">Refund not available in requested duration.</div>";
                    ltrChartAmountRefund.Text = "<div style=\"height:180px;width:500px; text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">Refund not available in requested duration.</div>";
                }
            }
            catch { }
        }

        /// <summary>
        /// Fill Charts For Refund with Data for Order and Amount
        /// </summary>
        private void FillMonthChart()
        {

            String Series1Name = String.Empty;
            String Series2Name = String.Empty;
            String SelectFields = String.Empty;
            String GroupByFields = String.Empty;
            String OrderByFields = String.Empty;
            String DateFormat = String.Empty;
            String GroupByIncrement = String.Empty;

            if (ddlPeriod.SelectedItem.Text.ToString().ToLower() == "day")
            {
                SelectFields = "month(OrderDate) as [Month],datepart(\"dy\",OrderDate) as [Day], Year(OrderDate) as [Year]";
                GroupByFields = "month(OrderDate),Year(OrderDate), datepart(\"dy\",OrderDate)";
                OrderByFields = "month(OrderDate),Year(OrderDate) asc, datepart(\"dy\",OrderDate) asc";
                DateFormat = "mm-dd-yyyy";
                GroupByIncrement = "0";

            }
            else if (ddlPeriod.SelectedItem.Text.ToString().ToLower() == "month")
            {

                SelectFields = "month(OrderDate) as [Month], Year(OrderDate) as [Year]";
                GroupByFields = "Year(OrderDate), month(OrderDate)";
                OrderByFields = "Year(OrderDate) asc, month(OrderDate) asc";
                DateFormat = "mm-yyyy";
                GroupByIncrement = "2";
            }
            else if (ddlPeriod.SelectedItem.Text.ToString().ToLower() == "year")
            {
                SelectFields = "month(OrderDate) as [Month],Year(OrderDate) as [Year]";
                GroupByFields = "month(OrderDate),Year(OrderDate)";
                OrderByFields = "month(OrderDate),Year(OrderDate) asc";
                DateFormat = "yyyy";
                GroupByIncrement = "3";
            }

            String Sql = null;
            DataSet dsMonth = new DataSet();
            String Whareclause = "CONVERT(CHAR(10),Orderdate,101) >= CONVERT(CHAR(10)," + txtOrderFrom.Text.ToString() + ",101) and CONVERT(CHAR(10),Orderdate,101) <= CONVERT(CHAR(10)," + txtOrderTo.Text.ToString() + ",101)";

            Sql = "EXEC usp_Order_GetOrderByDate '" + txtOrderFrom.Text.ToString() + "','" + txtOrderTo.Text.ToString() + "','" + ddlPeriod.SelectedItem.Text.ToString() + "'," + ddlStore.SelectedValue.ToString() + "";

            try
            {
                SqlDataAdapter Adpt = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand();
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"].ToString());
                try
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = Sql;
                    cmd.CommandTimeout = 10000;
                    Adpt.SelectCommand = cmd;
                    Adpt.Fill(dsMonth);
                }
                catch (Exception ex)
                {

                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                finally
                {
                    if (conn != null)
                        if (conn.State == ConnectionState.Open) conn.Close();
                    cmd.Dispose();
                    Adpt.Dispose();

                }
               // dsMonth = CommonComponent.GetCommonDataSet(Sql);
                string strMonth = "";
                if (dsMonth != null && dsMonth.Tables.Count > 0 && dsMonth.Tables[0].Rows.Count > 0)
                {
                    string strName = "";
                    string strYear = "";
                    string strName1 = "";
                    string strYear1 = "";
                    for (int i = 0; i < dsMonth.Tables[0].Rows.Count; i++)
                    {
                        if (strName != dsMonth.Tables[0].Rows[i]["ordermonth"].ToString() || strYear != dsMonth.Tables[0].Rows[i]["orderyear"].ToString())
                        {
                            strMonth += "<h3><a href=\"#\">" + String.Format("{0:MMMMMMMMM - yyyy}", Convert.ToDateTime(dsMonth.Tables[0].Rows[i]["orderDate"].ToString())) + " (Total Orders : " + dsMonth.Tables[0].Rows[i]["totalorder1"].ToString() + " Total: " + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["Total1"].ToString()).ToString("C") + ")</a> </h3>";
                            DataRow[] dr = dsMonth.Tables[0].Select("ordermonth='" + dsMonth.Tables[0].Rows[i]["ordermonth"].ToString() + "' AND orderyear='" + dsMonth.Tables[0].Rows[i]["orderyear"].ToString() + "' ");
                            if (dr != null)
                            {
                                strMonth += "<div>";
                                strMonth += "<table cellspacing='0' cellpadding='0' border='0' class='order-table' style='font-family:Arial,sans-serif;font-size:12px;' width='100%' >";
                                Decimal SubTotal = Decimal.Zero;
                                Decimal Tax = Decimal.Zero;
                                Decimal Shipping = Decimal.Zero;
                                Decimal RefundAmount = Decimal.Zero;
                                Decimal Discount = Decimal.Zero;
                                Decimal Total = Decimal.Zero;
                                if (ddlPeriod.SelectedItem.Text.ToString().ToLower() == "year")
                                {
                                    strMonth += "<tr style=\"background: none repeat scroll 0 0 #e5e5e5;\"><th width='10%' align='center'>Month</th><th width='10%' align='center'>Total Orders</th><th align='center' width='12%'>Sub Total</th><th align='center' width='12%'>Tax</th><th align='center' width='12%'>Shipping</th><th align='center' width='12%'>Refund</th><th align='center' width='12%'>Discount</th><th align='center' width='8%'>Adj. Amount</th><th align='center' width='12%'>Total</th></tr>";
                                }
                                else
                                {
                                    strMonth += "<tr style=\"background: none repeat scroll 0 0 #e5e5e5;\"><th width='10%' align='center'>Day</th><th width='10%' align='center'>Total Orders</th><th align='center' width='12%'>Sub Total</th><th align='center' width='12%'>Tax</th><th align='center' width='12%'>Shipping</th><th align='center' width='12%'>Refund</th><th align='center' width='12%'>Discount</th><th align='center' width='8%'>Adj. Amount</th><th align='center' width='12%'>Total</th></tr>";
                                }
                                Int32 row = 1;

                                if (ddlPeriod.SelectedItem.Text.ToString().ToLower() == "year")
                                {
                                    foreach (DataRow dr1 in dr)
                                    {
                                        if (strName1 != dr1["ordermonth"].ToString() || strYear1 != dr1["orderyear"].ToString())
                                        {
                                            Total += Convert.ToDecimal(dr1["Total"].ToString());
                                            SubTotal += Convert.ToDecimal(dr1["SubTotal"].ToString());
                                            Tax += Convert.ToDecimal(dr1["Tax"].ToString());
                                            Shipping += Convert.ToDecimal(dr1["Shipping"].ToString());
                                            RefundAmount += Convert.ToDecimal(dr1["RefundAmount"].ToString());
                                            Discount += Convert.ToDecimal(dr1["Discount"].ToString());
                                            Total += Convert.ToDecimal(dr1["Total"].ToString());
                                            if (row % 2 == 0 && row != 1)
                                            {
                                                strMonth += "<tr style=\"background:#F0F0F0;\">";
                                            }
                                            else
                                            {
                                                strMonth += "<tr>";
                                            }
                                            strMonth += "<td align='left'>" + String.Format("{0:MMMMMMMMM - yyyy}", Convert.ToDateTime(dr1["orderDate"].ToString())) + "</td>";
                                            strMonth += "<td align='center'>" + dr1["totalorder"].ToString() + "</td>";
                                            strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["SubTotal"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                            strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["Tax"]).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                            strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["Shipping"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                            strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["RefundAmount"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                            strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["Discount"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                            strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["AdjustmentAmount"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                            strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["Total"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                            strMonth += "</tr>";

                                        }
                                        strName1 = dr1["ordermonth"].ToString();
                                        strYear1 = dr1["orderyear"].ToString();
                                        row++;
                                    }
                                    strMonth += "<tr>";
                                    strMonth += "<td><b>Total</b></td>";
                                    strMonth += "<td align='center'><b>" + dsMonth.Tables[0].Rows[i]["totalorder1"].ToString() + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["SubTotal1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["Tax1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["Shipping1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["RefundAmount1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(Discount).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["AdjustmentAmount1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["Total1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "</tr>";

                                }
                                else
                                {
                                    foreach (DataRow dr1 in dr)
                                    {
                                        Total += Convert.ToDecimal(dr1["Total"].ToString());
                                        SubTotal += Convert.ToDecimal(dr1["SubTotal"].ToString());
                                        Tax += Convert.ToDecimal(dr1["Tax"].ToString());
                                        Shipping += Convert.ToDecimal(dr1["Shipping"].ToString());
                                        RefundAmount += Convert.ToDecimal(dr1["RefundAmount"].ToString());
                                        Discount += Convert.ToDecimal(dr1["Discount"].ToString());
                                        Total += Convert.ToDecimal(dr1["Total"].ToString());
                                        if (row % 2 == 0 && row != 1)
                                        {
                                            strMonth += "<tr style=\"background:#F0F0F0;\">";
                                        }
                                        else
                                        {
                                            strMonth += "<tr>";
                                        }
                                        strMonth += "<td align='left'>" + dr1["orderDate"].ToString() + "</td>";
                                        strMonth += "<td align='center'>" + dr1["totalorder"].ToString() + "</td>";
                                        strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["SubTotal"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                        strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["Tax"]).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                        strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["Shipping"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                        strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["RefundAmount"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                        strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["Discount"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                        strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["AdjustmentAmount"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                        strMonth += "<td align='right'>" + Convert.ToDecimal(dr1["Total"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</td>";
                                        strMonth += "</tr>";

                                        row++;
                                    }
                                    strMonth += "<tr>";
                                    strMonth += "<td><b>Total</b></td>";
                                    strMonth += "<td align='center'><b>" + dsMonth.Tables[0].Rows[i]["totalorder1"].ToString() + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["SubTotal1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["Tax1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["Shipping1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["RefundAmount1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(Discount).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["AdjustmentAmount1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "<td align='right'><b>" + Convert.ToDecimal(dsMonth.Tables[0].Rows[i]["Total1"].ToString()).ToString("C").Replace("(", "-").Replace(")", "") + "</b></td>";
                                    strMonth += "</tr>";
                                }
                                strMonth += "</table>";
                                strMonth += "</div>";
                            }
                        }
                        strName = dsMonth.Tables[0].Rows[i]["ordermonth"].ToString();
                        strYear = dsMonth.Tables[0].Rows[i]["orderyear"].ToString();
                    }
                    ltrmonth.Text = strMonth.ToString();
                }
                else
                    ltrmonth.Text = "";
            }
            catch
            {

            }
        }
    }
}