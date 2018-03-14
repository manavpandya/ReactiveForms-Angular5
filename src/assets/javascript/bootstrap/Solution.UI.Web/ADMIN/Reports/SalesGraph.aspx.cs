using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using System.IO;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class SalesGraph : BasePage
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
                Contents();
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
            ddlStore.SelectedIndex = 0;
        }


        /// <summary>
        /// Method Use for bind Order Graph result according to
        /// selected date range and display Result -> 
        /// according to day wise,month wise number
        /// of Order Generated.
        /// </summary>
        protected void Contents()
        {
            try
            {

                #region CreateChart
                string day = string.Empty;
                string month = string.Empty;
                string Field = string.Empty;

                DataSet dsChart = new DataSet();
                string F1 = "";
                string YaxeName = "";
                string WhereClause = string.Empty;
                YaxeName = "Order";
                string DateWise = string.Empty;
                string SqlStr = string.Empty;

                Int16 TotDays = Convert.ToInt16(RadOrderByDays.SelectedValue);
                if (TotDays == 1)
                {
                    string strSearch = "";
                    String StartDate = txtOrderFrom.Text.ToString().Trim();
                    String EndDate = txtOrderTo.Text.ToString().Trim();
                    if (txtOrderFrom.Text.ToString() != "" && txtOrderTo.Text.ToString() != "")
                    {
                        if (Convert.ToDateTime(txtOrderTo.Text.ToString()) >= Convert.ToDateTime(txtOrderFrom.Text.ToString()))
                        {

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select valid date.', 'Message');});", true);
                            return;
                        }
                    }
                    else
                    {
                        if (txtOrderFrom.Text.ToString() == "" && txtOrderTo.Text.ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select valid date.', 'Message');});", true);
                            return;
                        }
                        else if (txtOrderTo.Text.ToString() == "" && txtOrderFrom.Text.ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select valid date.', 'Message');});", true);
                            return;
                        }

                    }

                    if (txtOrderFrom.Text.ToString() != "")
                    {
                        strSearch += " AND Convert(char(10),orderdate,101) >= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtOrderFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
                    }
                    if (txtOrderTo.Text.ToString() != "")
                    {
                        strSearch += " AND Convert(char(10),orderdate,101) <= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtOrderTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";

                    }

                    if (ddlStore.SelectedIndex == 0)
                    {
                        SqlStr = "select sum(OrderTotal) as Total,convert(datetime, convert(varchar(20),OrderDate,106)) as 'OrderDate' from tb_order  where TransactionStatus in ('AUTHORIZED','CAPTURED') and deleted=0 " + strSearch +
                                        " group by convert(datetime, convert(varchar(20),OrderDate,106)) order by OrderDate asc";
                    }
                    else
                    {
                        SqlStr = "select sum(OrderTotal) as Total,convert(datetime, convert(varchar(20),OrderDate,106)) as 'OrderDate' from tb_order  where TransactionStatus in ('AUTHORIZED','CAPTURED') and deleted=0 and StoreID=" + Convert.ToInt32(ddlStore.SelectedValue) + " " + strSearch +
                                     " group by convert(datetime, convert(varchar(20),OrderDate,106)) order by OrderDate asc";
                    }
                }
                else
                {
                    TotDays *= -1;
                    if (ddlStore.SelectedIndex == 0)
                    {
                        SqlStr = "select sum(OrderTotal) as Total,convert(datetime, convert(varchar(20),OrderDate,106)) as 'OrderDate' from tb_order  where TransactionStatus in ('AUTHORIZED','CAPTURED') and deleted=0 and ( OrderDate>=dateadd(day," + TotDays + ",getdate()) and  " +
                                 "OrderDate <=getdate()) group by convert(datetime, convert(varchar(20),OrderDate,106)) order by OrderDate asc";
                    }
                    else
                    {
                        SqlStr = "select sum(OrderTotal) as Total,convert(datetime, convert(varchar(20),OrderDate,106)) as 'OrderDate' from tb_order  where TransactionStatus in ('AUTHORIZED','CAPTURED') and deleted=0 and StoreID=" + Convert.ToInt32(ddlStore.SelectedValue) + " and ( OrderDate>=dateadd(day," + TotDays + ",getdate()) and  " +
                                        "OrderDate <=getdate()) group by convert(datetime, convert(varchar(20),OrderDate,106)) order by OrderDate asc";
                    }
                }
                dsChart = CommonComponent.GetCommonDataSet(SqlStr);

                if (dsChart != null && dsChart.Tables.Count > 0 && dsChart.Tables[0].Rows.Count > 0)
                {
                    string[] RandColor = { "ec1f7b", "77113f", "0b0206", "e585e9", "e23be8", "87058c", "c98bf0", "a83bed", "4c047a", "61d3eb", "098eaa", "244d56", "0b9e42", "99a911", "e1ae23", "f58d54", "b24206", "ed6b59", "c5230d" };
                    Random random = new Random();
                    String XMLChart = String.Empty;
                    int RecCnt = dsChart.Tables[0].Rows.Count;
                    string ChartWidth = (RecCnt * 90).ToString();
                    if (Convert.ToInt32(ChartWidth) <= 90)
                    {
                        ChartWidth = "200";
                    }
                    for (int i = 0; i < RecCnt; i++)
                    {
                        XMLChart += "<set value='" + System.Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i][0].ToString()), 0) + "'" + " name='" + Convert.ToDateTime(dsChart.Tables[0].Rows[i][1]).ToShortDateString() + "'" + " hoverText='" + dsChart.Tables[0].Rows[i][1] + "' />";
                    }
                    XMLChart = "-<graph caption='Date-wise Order revenue Chart' subcaption='for " + ((RadOrderByDays.SelectedValue == "1") ? (DateWise + " Date range") : (RadOrderByDays.SelectedValue + " Days")) + "' xAxisName='Date' yAxisMinValue='0' yAxisName='Revenue in USD' decimalPrecision='0' formatNumberScale='0' numberPrefix='$' showNames='1' showValues='0' showAlternateHGridColor='1' AlternateHGridColor='ff5904' divLineColor='ff5904' divLineAlpha='20' alternateHGridAlpha='5'>" + XMLChart + "</graph>";
                    FCLiteral.Text = InfoSoftGlobal.FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_Line.swf", "", XMLChart, "myFirst", ChartWidth, "300", false);
                    FCLiteral.Text = FCLiteral.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");

                }
                else
                {
                    FCLiteral.Text = "Record Not Found.";

                }
            }
            catch (Exception ex)
            {

            }
                #endregion CreateChart

        }

        /// <summary>
        /// Method Use for CreateXMLFile Order Graph 
        /// </summary>
        private void CreateXMLFile()
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add("ChartReport");
            DataColumn dc = dt.Columns.Add("N", Type.GetType("System.Int32"));
            dt.Columns.Add("Day", Type.GetType("System.DateTime"));

            Stream stream = new MemoryStream();
            ds.WriteXml(stream);
        }

        /// <summary>
        /// Search button Click Event for 
        /// display Result value entered 
        /// by User for Order Graph.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            Contents();
            if (RadOrderByDays.SelectedIndex == 7)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "if (document.getElementById('ContentPlaceHolder1_datetd') != null) {document.getElementById('ContentPlaceHolder1_datetd').style.display = '';}", true);
            }

        }
    }
}