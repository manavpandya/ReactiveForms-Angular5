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

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class Reportyearcamparision : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgSearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/searchgo.gif";
                BindStore();
                txtOrderFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date));
                txtOrderTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date));
                FillChart(ddlPeriod.SelectedValue.ToString());
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
            //if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            //{
            //    ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            //    AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            //}
            //else
            //{
            AppConfig.StoreID = 1;
            ddlStore.SelectedIndex = 0;
            //}
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
                dsChart = CommonComponent.GetCommonDataSet("EXEC usp_Report_OrderStatistics_compare " + Convert.ToInt32(ddlStore.SelectedValue) + ",'" + Duration + "','" + Convert.ToDateTime(txtOrderFrom.Text) + "','" + Convert.ToDateTime(txtOrderTo.Text) + "',''");// OrderComponent.GetChartDetailsForOrderStatisticReport(Duration, Convert.ToInt32(ddlStore.SelectedValue), Convert.ToDateTime(txtOrderFrom.Text), Convert.ToDateTime(txtOrderTo.Text), "");
                if (dsChart != null && dsChart.Tables.Count > 0 && dsChart.Tables[0].Rows.Count > 0)
                {

                    string[] RandColor = { "61d3eb", "098eaa", "244d56", "0b9e42", "99a911", "e1ae23", "f58d54", "b24206", "ed6b59", "c5230d" };
                    string[] RandColor1 = { "ec1f7b", "77113f", "0b0206", "e585e9", "e23be8", "87058c", "c98bf0", "a83bed", "4c047a" };
                    Random random = new Random();
                    String XMLChart = String.Empty;
                    String XMLChartAmount = String.Empty;
                    //for (int i = 0; i < dsChart.Tables[0].Rows.Count; i++)
                    //{
                    //    string stramt = dsChart.Tables[0].Rows[i]["Day"].ToString() + " ," + dsChart.Tables[0].Rows[i]["OrderTotal"].ToString();
                    //    string strorder = dsChart.Tables[0].Rows[i]["Day"].ToString() + " ," + dsChart.Tables[0].Rows[i]["TotalOrder"].ToString();
                    //    if (Duration == "Day")
                    //    {
                    //        XMLChart += "<set  value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["TotalOrder"]), 2)) + "'" + " name='" + String.Format("{0:MM/dd/yy}", Convert.ToDateTime(dsChart.Tables[0].Rows[i]["Day"])) + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + strorder.ToString() + "' />";
                    //        XMLChartAmount += "<set value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["OrderTotal"]), 2)) + "'" + " name='" + String.Format("{0:MM/dd/yy}", Convert.ToDateTime(dsChart.Tables[0].Rows[i]["Day"])) + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + stramt.ToString() + "'   />";
                    //    }
                    //    else
                    //    {
                    //        XMLChart += "<set  value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["TotalOrder"]), 2)) + "'" + " name='" + dsChart.Tables[0].Rows[i]["Day"] + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + strorder.ToString() + "' />";
                    //        XMLChartAmount += "<set value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["OrderTotal"]), 2)) + "'" + " name='" + dsChart.Tables[0].Rows[i]["Day"] + "'" + " color='" + RandColor[random.Next(19)] + "'  hoverText='" + stramt.ToString() + "'   />";
                    //    }
                    //}



                    XMLChartAmount += "<categories  font='Arial' fontSize='11' fontColor='000000'>";
                    XMLChart += "<categories  font='Arial' fontSize='11' fontColor='000000'>";
                    string stt = ",";
                    string strmonth = "";
                    DataSet dsyear = new DataSet();
                    DataSet dsmonth = new DataSet();
                    if (Duration == "Month")
                    {



                        if (ddlStore.SelectedValue.ToString() != "0")
                        {
                            dsmonth = CommonComponent.GetCommonDataSet(@"SELECT DISTINCT month(Orderdate),DATENAME(month,Orderdate) FROM tb_Order WHERE  isnull(TransactionStatus,'') not IN ('Voided','CANCELED') and isnull(OrderStatus,'')<>'Canceled' and  StoreID=" + ddlStore.SelectedValue.ToString() + @" AND (isnull(TransactionStatus,'')='CAPTURED' OR isnull(TransactionStatus,'')='PARTIALLY REFUNDED') AND 
                              isnull(deleted,0) <> 1 and isnull(FraudedOn,'')='' and isnull(voidedon,'')='' and  orderdate is not   
                              NULL AND 
							  cast(OrderDate as date) >= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY('" + txtOrderFrom.Text.ToString() + @"')-1),'" + txtOrderFrom.Text.ToString() + @"'),101)) as date) and cast(OrderDate as date) <= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"'))),DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"')),101)) as date) Order By month(Orderdate) ASC");

                        }
                        else
                        {
                            dsmonth = CommonComponent.GetCommonDataSet(@"SELECT DISTINCT month(Orderdate),DATENAME(month,Orderdate) FROM tb_Order WHERE  isnull(TransactionStatus,'') not IN ('Voided','CANCELED') and isnull(OrderStatus,'')<>'Canceled' and (isnull(TransactionStatus,'')='CAPTURED' OR isnull(TransactionStatus,'')='PARTIALLY REFUNDED') AND 
                              isnull(deleted,0) <> 1 and isnull(FraudedOn,'')='' and isnull(voidedon,'')='' and  orderdate is not   
                              NULL AND 
							  cast(OrderDate as date) >= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY('" + txtOrderFrom.Text.ToString() + @"')-1),'" + txtOrderFrom.Text.ToString() + @"'),101)) as date) and cast(OrderDate as date) <= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"'))),DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"')),101)) as date) Order By month(Orderdate) ASC");

                        }
                        if (ddlStore.SelectedValue.ToString() != "0")
                        {
                            dsyear = CommonComponent.GetCommonDataSet(@"SELECT DISTINCT year(Orderdate) FROM tb_Order WHERE  isnull(TransactionStatus,'') not IN ('Voided','CANCELED') and isnull(OrderStatus,'')<>'Canceled' and  StoreID=" + ddlStore.SelectedValue.ToString() + @" and (isnull(TransactionStatus,'')='CAPTURED' OR isnull(TransactionStatus,'')='PARTIALLY REFUNDED') AND 
                              isnull(deleted,0) <> 1 and isnull(FraudedOn,'')='' and isnull(voidedon,'')='' and  orderdate is not   
                              NULL AND 
							  cast(OrderDate as date) >= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY('" + txtOrderFrom.Text.ToString() + @"')-1),'" + txtOrderFrom.Text.ToString() + @"'),101)) as date) and cast(OrderDate as date) <= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"'))),DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"')),101)) as date) Order By year(Orderdate) ASC");

                        }
                        else
                        {
                            dsyear = CommonComponent.GetCommonDataSet(@"SELECT DISTINCT year(Orderdate) FROM tb_Order WHERE  isnull(TransactionStatus,'') not IN ('Voided','CANCELED') and isnull(OrderStatus,'')<>'Canceled' and (isnull(TransactionStatus,'')='CAPTURED' OR isnull(TransactionStatus,'')='PARTIALLY REFUNDED') AND 
                              isnull(deleted,0) <> 1 and isnull(FraudedOn,'')='' and isnull(voidedon,'')='' and  orderdate is not   
                              NULL AND 
							  cast(OrderDate as date) >= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY('" + txtOrderFrom.Text.ToString() + @"')-1),'" + txtOrderFrom.Text.ToString() + @"'),101)) as date) and cast(OrderDate as date) <= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"'))),DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"')),101)) as date) Order By year(Orderdate) ASC");

                        }

                        for (int i = 0; i < dsmonth.Tables[0].Rows.Count; i++)
                        {
                            if (stt.ToLower().IndexOf("," + dsmonth.Tables[0].Rows[i][1].ToString().ToLower() + ",") <= -1)
                            {
                                stt += dsmonth.Tables[0].Rows[i][1].ToString() + ",";
                                XMLChartAmount += "<category name='" + dsmonth.Tables[0].Rows[i][1].ToString() + "' />";
                                XMLChart += "<category name='" + dsmonth.Tables[0].Rows[i][1].ToString() + "' />";
                            }

                        }
                        strmonth = "Month";



                    }
                    else if (Duration == "Year")
                    {

                        if (ddlStore.SelectedValue.ToString() != "0")
                        {
                            dsyear = CommonComponent.GetCommonDataSet(@"SELECT DISTINCT year(Orderdate) FROM tb_Order WHERE  isnull(TransactionStatus,'') not IN ('Voided','CANCELED') and isnull(OrderStatus,'')<>'Canceled' and  StoreID=" + ddlStore.SelectedValue.ToString() + @" and (isnull(TransactionStatus,'')='CAPTURED' OR isnull(TransactionStatus,'')='PARTIALLY REFUNDED') AND 
                              isnull(deleted,0) <> 1 and isnull(FraudedOn,'')='' and isnull(voidedon,'')='' and  orderdate is not   
                              NULL AND 
							  cast(OrderDate as date) >= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY('" + txtOrderFrom.Text.ToString() + @"')-1),'" + txtOrderFrom.Text.ToString() + @"'),101)) as date) and cast(OrderDate as date) <= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"'))),DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"')),101)) as date) Order By year(Orderdate) ASC");


                        }
                        else
                        {
                            dsyear = CommonComponent.GetCommonDataSet(@"SELECT DISTINCT year(Orderdate) FROM tb_Order WHERE  isnull(TransactionStatus,'') not IN ('Voided','CANCELED') and isnull(OrderStatus,'')<>'Canceled' and (isnull(TransactionStatus,'')='CAPTURED' OR isnull(TransactionStatus,'')='PARTIALLY REFUNDED') AND 
                              isnull(deleted,0) <> 1 and isnull(FraudedOn,'')='' and isnull(voidedon,'')='' and  orderdate is not   
                              NULL AND 
							  cast(OrderDate as date) >= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY('" + txtOrderFrom.Text.ToString() + @"')-1),'" + txtOrderFrom.Text.ToString() + @"'),101)) as date) and cast(OrderDate as date) <= cast((SELECT CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"'))),DATEADD(mm,1,'" + txtOrderTo.Text.ToString() + @"')),101)) as date) Order By year(Orderdate) ASC");

                        }

                        for (int i = 0; i < dsyear.Tables[0].Rows.Count; i++)
                        {
                            if (stt.ToLower().IndexOf("," + dsyear.Tables[0].Rows[i][0].ToString() + ",") <= -1)
                            {
                                stt += dsyear.Tables[0].Rows[i][0].ToString() + ",";
                                XMLChartAmount += "<category name='" + dsyear.Tables[0].Rows[i][0].ToString() + "' />";
                                XMLChart += "<category name='" + dsyear.Tables[0].Rows[i][0].ToString() + "' />";
                            }

                        }
                        strmonth = "Year";
                    }


                    //XMLChartAmount += "<category name='Software' />";
                    //XMLChartAmount += "<category name='Service' />";
                    XMLChartAmount += "</categories>";
                    XMLChart += "</categories>";

                    if (Duration == "Month")
                    {
                        for (int i = 0; i < dsyear.Tables[0].Rows.Count; i++)
                        {
                            XMLChartAmount += "<dataset seriesname='" + dsyear.Tables[0].Rows[i][0].ToString() + "' color='" + RandColor[random.Next(10)] + "'>";
                            XMLChart += "<dataset seriesname='" + dsyear.Tables[0].Rows[i][0].ToString() + "' color='" + RandColor[random.Next(10)] + "'>";
                            for (int j = 0; j < dsmonth.Tables[0].Rows.Count; j++)
                            {

                                DataRow[] dr = dsChart.Tables[0].Select("year=" + dsyear.Tables[0].Rows[i][0].ToString() + " and mname='" + dsmonth.Tables[0].Rows[j][1].ToString() + "'");
                                if (dr.Length > 0)
                                {
                                    XMLChartAmount += "<set value='" + dr[0]["OrderTotal"].ToString() + "' />";
                                    XMLChart += "<set value='" + dr[0]["TotalOrder"].ToString() + "' />";
                                }
                                else
                                {
                                    XMLChartAmount += "<set value='' />";
                                    XMLChart += "<set value='' />";
                                }
                            }
                            XMLChartAmount += "</dataset>";
                            XMLChart += "</dataset>";
                        }
                    }
                    else if (Duration == "Year")
                    {
                        XMLChartAmount += "<dataset seriesname='Year' color='" + RandColor[random.Next(10)] + "'>";
                        XMLChart += "<dataset seriesname='Year' color='" + RandColor[random.Next(10)] + "'>";
                        for (int i = 0; i < dsyear.Tables[0].Rows.Count; i++)
                        {



                            DataRow[] dr = dsChart.Tables[0].Select("Day=" + dsyear.Tables[0].Rows[i][0].ToString() + "");
                            if (dr.Length > 0)
                            {
                                XMLChartAmount += "<set value='" + dr[0]["OrderTotal"].ToString() + "' />";
                                XMLChart += "<set value='" + dr[0]["TotalOrder"].ToString() + "' />";
                            }
                            else
                            {
                                XMLChartAmount += "<set value='' />";
                                XMLChart += "<set value='' />";
                            }


                        }
                        XMLChartAmount += "</dataset>";
                        XMLChart += "</dataset>";
                    }



                    //XMLChartAmount += "<set value='84' />";
                    //XMLChartAmount += "<set value='207' />";
                    //XMLChartAmount += "<set value='116' />";

                    //XMLChartAmount += "<dataset seriesname='International' color='" + RandColor1[random.Next(9)] + "'>";
                    //XMLChartAmount += "<set value='116' />";
                    //XMLChartAmount += "<set value='237' />";
                    //XMLChartAmount += "<set value='83' />";
                    //XMLChartAmount += "</dataset>";

                    //XMLChart = XMLChartAmount;

                    // XMLChart = "-<graph caption='" + XaxeName + " wise Order-Quantity Chart' xAxisName='" + XaxeName + "'  yAxisName='" + YaxeName + "' decimalPrecision='0' " + numberPrefix + " formatNumberScale='0'>" + XMLChart + "</graph>";
                    XMLChart = "-<graph xaxisname='" + strmonth + "' yaxisname='Total Order' showbarshadow='0' hdivlinethickness='25'  canvasbordercolor='d7d7d7' hovercapbg='DEDEBE' hovercapborder='889E6D' rotateNames='0' animation='1' yAxisMaxValue='100' numdivlines='9' divLineColor='CCCCCC' divLineAlpha='80' decimalPrecision='0' showAlternateVGridColor='1' AlternateVGridAlpha='30' AlternateVGridColor='CCCCCC' caption='" + strmonth + " Order Comparison' subcaption=''>" + XMLChart + "</graph>";
                    ltrChartOrder.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_MSBar2D.swf", "", XMLChart, "myFirst", "1000", "600", false);
                    ltrChartOrder.Text = ltrChartOrder.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");


                    YaxeName = "Amount";
                    XMLChartAmount = "-<graph xaxisname='" + strmonth + "' yaxisname='Amount' hdivlinethickness='25' showbarshadow='0'  canvasbordercolor='d7d7d7' hovercapbg='DEDEBE' hovercapborder='889E6D' rotateNames='0' animation='1' yAxisMaxValue='100' numdivlines='9' divLineColor='CCCCCC' divLineAlpha='80' decimalPrecision='2' showAlternateVGridColor='1' AlternateVGridAlpha='30' AlternateVGridColor='CCCCCC' caption='" + strmonth + " Wise Amount Comparison' subcaption=''>" + XMLChartAmount + "</graph>";
                    // XMLChartAmount = "-<graph caption='" + XaxeName + " wise Order-Amount Chart' xAxisName='" + XaxeName + "' numberPrefix='$' yAxisName='" + YaxeName + "' decimalPrecision='2' " + numberPrefix + " formatNumberScale='0' >" + XMLChartAmount + "</graph>";
                    ltrChartAmount.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_MSBar2D.swf", "", XMLChartAmount, "myFirst", "1000", "600", false);
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
    }
}