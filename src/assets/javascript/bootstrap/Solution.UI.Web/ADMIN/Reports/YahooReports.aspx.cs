using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;
using InfoSoftGlobal;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class YahooReports : BasePage
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
                imgGraph.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/graph.png";
                BindStore();
                DateTime FirstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                txtFromDate.Text = String.Format("{0:MM/dd/yyyy}", FirstDate);
                txtToDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date));

                DataTable dtPeriod = new DataTable();
                dtPeriod.Columns.Add("Period", typeof(String));

                DataRow dr = dtPeriod.NewRow();
                dr["Period"] = "Last 10 Days"; dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = "Last 30 Days"; dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = "Last 60 Days"; dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = "Last 90 Days"; dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = "Last 120 Days"; dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = "Last 180 Days"; dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = "Last 365 Days"; dtPeriod.Rows.Add(dr);

                dr = dtPeriod.NewRow(); dr["Period"] = DateTime.Now.AddMonths(-5).ToString("Y"); dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = DateTime.Now.AddMonths(-4).ToString("Y"); dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = DateTime.Now.AddMonths(-3).ToString("Y"); dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = DateTime.Now.AddMonths(-2).ToString("Y"); dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = DateTime.Now.AddMonths(-1).ToString("Y"); dtPeriod.Rows.Add(dr);
                dr = dtPeriod.NewRow(); dr["Period"] = "From"; dtPeriod.Rows.Add(dr);

                foreach (DataRow row in dtPeriod.Rows)
                {
                    ListItem item = new ListItem();
                    item.Text = row["Period"].ToString();
                    item.Value = row["Period"].ToString();
                    rblPeriod.Items.Add(item);
                    if (item.Value.ToLower() == "from")
                        rblPeriod.SelectedValue = "From";
                }
            }

            ltCriteria.Text = rblCriteria.SelectedItem.Text;
            ltPeriod.Text = rblPeriod.SelectedItem.Text;
            if (ltPeriod.Text.ToLower() == "from")
                ltPeriod.Text += " " + txtFromDate.Text + " to " + txtToDate.Text;
            imgGraph_Click(null, null);
        }

        /// <summary>
        ///  Graph Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgGraph_Click(object sender, ImageClickEventArgs e)
        {
            FillChart(Convert.ToInt32(rblCriteria.SelectedValue));
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
            //ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
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
        /// WriteFile For Writing Into File
        /// </summary>
        /// <param name="Text">String Text</param>
        /// <param name="FileName">String FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }

        /// <summary>
        ///  Spread Sheet Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lkSpreadsheet_Click(object sender, EventArgs e)
        {
            try
            {
                string strServer = AppLogic.AppConfigs("Live_Server").TrimEnd('/');

                DateTime Fromdate = new DateTime();
                DateTime ToDate = new DateTime();

                ToDate = DateTime.Now.Date;
                if (rblPeriod.SelectedValue.ToLower() == "last 10 days")
                    Fromdate = DateTime.Now.AddDays(-9).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 30 days")
                    Fromdate = DateTime.Now.AddDays(-29).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 60 days")
                    Fromdate = DateTime.Now.AddDays(-59).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 90 days")
                    Fromdate = DateTime.Now.AddDays(-89).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 120 days")
                    Fromdate = DateTime.Now.AddDays(-119).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 180 days")
                    Fromdate = DateTime.Now.AddDays(-179).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 365 days")
                    Fromdate = DateTime.Now.AddDays(-364).Date;

                else if (rblPeriod.SelectedValue.ToLower() == DateTime.Now.AddMonths(-5).ToString("Y").ToLower())
                {
                    Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-5);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-4).AddDays(-1);
                }
                else if (rblPeriod.SelectedValue.ToLower() == DateTime.Now.AddMonths(-4).ToString("Y").ToLower())
                {
                    Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-4);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-3).AddDays(-1);
                }

                else if (rblPeriod.SelectedValue.ToLower() == DateTime.Now.AddMonths(-3).ToString("Y").ToLower())
                {
                    Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-3);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-2).AddDays(-1);
                }
                else if (rblPeriod.SelectedValue.ToLower() == DateTime.Now.AddMonths(-2).ToString("Y").ToLower())
                {
                    Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-2);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).AddDays(-1);
                }
                else if (rblPeriod.SelectedValue.ToLower() == DateTime.Now.AddMonths(-1).ToString("Y").ToLower())
                {
                    Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                }


                else if (rblPeriod.SelectedValue.ToLower() == "from")
                {
                    try
                    {
                        Fromdate = Convert.ToDateTime(txtFromDate.Text.ToString());
                    }
                    catch
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid From Date.', 'Message', 'ContentPlaceHolder1_txtFromDate')", true);
                        return;
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(txtToDate.Text.ToString()))
                            ToDate = Convert.ToDateTime(txtToDate.Text.ToString());
                    }
                    catch
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid From Date.', 'Message', 'ContentPlaceHolder1_txtFromDate')", true);
                        return;
                    }
                    if (!string.IsNullOrEmpty(txtToDate.Text.ToString()) && Fromdate > ToDate)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid Date Range.', 'Message', 'ContentPlaceHolder1_txtFromDate')", true);
                        return;
                    }
                }

                CommonComponent clsCommon = new CommonComponent();
                DataSet Ds = new DataSet();
                // Ds = clsCommon.GetYahooReports(Convert.ToInt32(rblCriteria.SelectedValue), Int32.Parse(ddlStore.SelectedValue.ToString()), Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
                if (rblGraphType.SelectedValue == "Dailycycle") // Display Record(s) as Time Period wise
                {
                    Ds = DashboardComponent.GetChartDetailsforYahooDetails(Convert.ToInt32(ddlStore.SelectedValue), Fromdate, ToDate, Convert.ToInt32(rblCriteria.SelectedValue), "Time");
                }
                else
                {
                    Ds = DashboardComponent.GetChartDetailsforYahooDetails(Convert.ToInt32(ddlStore.SelectedValue), Fromdate, ToDate, Convert.ToInt32(rblCriteria.SelectedValue), "");
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {
                        object[] args = new object[2];
                        args[0] = Ds.Tables[0].Rows[i]["DisDate"].ToString();
                        args[1] = Ds.Tables[0].Rows[i]["NoOfOrder"].ToString();
                        sb.AppendLine(string.Format("{0},{1}", args));
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg11", "$(document).ready( function() {jAlert('No Data available in requested duration.', 'Message','');});", true);
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine(FullString);
                    String FileName = "";

                    DateTime dt = DateTime.Now;
                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(rblCriteria.SelectedValue) == 1)
                        FileName = "Customers_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(rblCriteria.SelectedValue) == 2)
                        FileName = "Number-of-Orders_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(rblCriteria.SelectedValue) == 3)
                        FileName = "Number-of-Gift-Certificate-Orders_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(rblCriteria.SelectedValue) == 4)
                        FileName = "Items-Sold_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(rblCriteria.SelectedValue) == 5)
                        FileName = "Revenue_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(rblCriteria.SelectedValue) == 6)
                        FileName = "Revenue-from-Gift-Certificate_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(rblCriteria.SelectedValue) == 7)
                        FileName = "Order-Customer_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(rblCriteria.SelectedValue) == 8)
                        FileName = "Revenue-Customer_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(rblCriteria.SelectedValue) == 9)
                        FileName = "Revenue-Order_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(rblCriteria.SelectedValue) == 10)
                        FileName = "Item-Order_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                    if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ReportExportPath"))))
                        Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ReportExportPath")));

                    try
                    {
                        String FilePath = Server.MapPath(AppLogic.AppConfigs("ReportExportPath") + FileName);
                        WriteFile(sb.ToString(), FilePath);
                        Response.ContentType = "text/csv";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                        Response.TransmitFile(FilePath);
                        Response.End();
                    }
                    catch { }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg12", "$(document).ready( function() {jAlert('No Data available in requested duration.', 'Message','');});", true);
                }
            }
            catch { }

        }

        /// <summary>
        /// Fills the Chart
        /// </summary>
        /// <param name="Mode">int Mode</param>
        public void FillChart(int Mode)
        {
            try
            {
                string YaxeName = "No Of Record(s)";
                string XaxeName = "Days";
                string numberPrefix = "";
                DateTime Fromdate = new DateTime();
                DateTime ToDate = new DateTime();

                ToDate = DateTime.Now.Date;
                if (rblPeriod.SelectedValue.ToLower() == "last 10 days")
                    Fromdate = DateTime.Now.AddDays(-9).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 30 days")
                    Fromdate = DateTime.Now.AddDays(-29).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 60 days")
                    Fromdate = DateTime.Now.AddDays(-59).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 90 days")
                    Fromdate = DateTime.Now.AddDays(-89).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 120 days")
                    Fromdate = DateTime.Now.AddDays(-119).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 180 days")
                    Fromdate = DateTime.Now.AddDays(-179).Date;

                else if (rblPeriod.SelectedValue.ToLower() == "last 365 days")
                    Fromdate = DateTime.Now.AddDays(-364).Date;

                else if (rblPeriod.SelectedValue.ToLower() == DateTime.Now.AddMonths(-5).ToString("Y").ToLower())
                {
                    Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-5);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-4).AddDays(-1);
                }
                else if (rblPeriod.SelectedValue.ToLower() == DateTime.Now.AddMonths(-4).ToString("Y").ToLower())
                {
                    Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-4);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-3).AddDays(-1);
                }

                else if (rblPeriod.SelectedValue.ToLower() == DateTime.Now.AddMonths(-3).ToString("Y").ToLower())
                {
                    Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-3);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-2).AddDays(-1);
                }
                else if (rblPeriod.SelectedValue.ToLower() == DateTime.Now.AddMonths(-2).ToString("Y").ToLower())
                {
                    Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-2);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).AddDays(-1);
                }
                else if (rblPeriod.SelectedValue.ToLower() == DateTime.Now.AddMonths(-1).ToString("Y").ToLower())
                {
                    Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                }

                else if (rblPeriod.SelectedValue.ToLower() == "from")
                {
                    try
                    {
                        Fromdate = Convert.ToDateTime(txtFromDate.Text.ToString());
                    }
                    catch
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid From Date.', 'Message', 'ContentPlaceHolder1_txtFromDate')", true);
                        return;
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(txtToDate.Text.ToString()))
                            ToDate = Convert.ToDateTime(txtToDate.Text.ToString());
                    }
                    catch
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid From Date.', 'Message', 'ContentPlaceHolder1_txtFromDate')", true);
                        return;
                    }
                    if (!string.IsNullOrEmpty(txtToDate.Text.ToString()) && Fromdate > ToDate)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid Date Range.', 'Message', 'ContentPlaceHolder1_txtFromDate')", true);
                        return;
                    }
                }
                DataSet dsChart = new DataSet();
                if (rblGraphType.SelectedValue == "Dailycycle") // Display Record(s) as Time Period wise
                {
                    dsChart = DashboardComponent.GetChartDetailsforYahooDetails(Convert.ToInt32(ddlStore.SelectedValue), Fromdate, ToDate, Mode, "Time");
                }
                else
                {
                    dsChart = DashboardComponent.GetChartDetailsforYahooDetails(Convert.ToInt32(ddlStore.SelectedValue), Fromdate, ToDate, Mode, "");
                }
                ltTotalNoOfOrders.Text = "";

                if (dsChart != null && dsChart.Tables.Count > 0 && dsChart.Tables[0].Rows.Count > 0)
                {
                    String XMLChart = String.Empty;
                    string Orderdate = "";
                    string NoOfOrder = "";
                    string chartdate = "";
                    Decimal TotNoOfRecords = 0;
                    int Rowcount = Convert.ToInt32(dsChart.Tables[0].Rows.Count);
                    for (int i = 0; i < dsChart.Tables[0].Rows.Count; i++)
                    {
                        string strskul = dsChart.Tables[0].Rows[i]["NoOfOrder"].ToString();
                        if (!string.IsNullOrEmpty(dsChart.Tables[0].Rows[i]["NoOfOrder"].ToString()))
                            TotNoOfRecords += Convert.ToDecimal(dsChart.Tables[0].Rows[i]["NoOfOrder"].ToString());

                        NoOfOrder += strskul + "|";
                        if (rblGraphType.SelectedValue == "Dailycycle") // Display Record(s) as Time Period wise
                        {
                            Orderdate += dsChart.Tables[0].Rows[i]["DisDate"].ToString() + "|";
                        }
                        else
                        {
                            DateTime DisDate = new DateTime();
                            DisDate = Convert.ToDateTime(dsChart.Tables[0].Rows[i]["DisDate"].ToString());
                            chartdate = DisDate.ToString("MMM") + " " + DisDate.ToString("dd");
                            Orderdate += chartdate + "|";
                        }
                    }
                    if (TotNoOfRecords.ToString().Contains("."))
                    {
                        if (Mode == 8)
                        {
                            TotNoOfRecords = Convert.ToDecimal(TotNoOfRecords / Convert.ToDecimal(Rowcount));
                            //ltTotalNoOfOrders.Text = "<b>" + TotNoOfRecords.ToString("c").Replace("$", "") + "</b>";
                        }
                        else if(Mode ==9)
                        {
                            TotNoOfRecords = Convert.ToDecimal(TotNoOfRecords / Convert.ToDecimal(Rowcount));
                        }
                        ltTotalNoOfOrders.Text = "<b>" + TotNoOfRecords.ToString("c").Replace("$", "") + "</b>";
                    }
                    else
                    {
                        ltTotalNoOfOrders.Text = "<b>" + TotNoOfRecords.ToString() + "</b>";
                    }
                    if (Orderdate.Length > 0)
                        Orderdate = Orderdate.Substring(0, Orderdate.Length - 1);

                    XMLChart = "-<chart compactDataMode='1' dataSeparator='|' caption='' subcaption='' axis='linear' numberPrefix='' formatNumberScale='0' allowPinMode='0' enableIconMouseCursors='0' dynamicAxis='1' palette='3'> " +
                    "<categories>" + Orderdate + "</categories> " +
                    "<dataset seriesName='Close'>" + NoOfOrder + "</dataset></chart>";

                    ltrChartOrder.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/ZoomLine.swf", "", XMLChart, "myFirst", "800", "300", false);
                    ltrChartOrder.Text = ltrChartOrder.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");
                }
                else
                {
                    ltrChartOrder.Text = "<div style=\"height:180px;width:100%; text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">No Data available in requested duration.</div>";
                    ltTotalNoOfOrders.Text = "<b>0</b>";
                }
            }
            catch { }
            ReCallJQuery();
        }

        /// <summary>
        /// Re-calling Js for working Tabs in correctly 
        /// </summary>
        private void ReCallJQuery()
        {
            string strjquery = @"$(document).ready(function(){
	        $('.menu > li').click(function(e){
		        switch(e.target.id){
		            case 'OrdersList':
				
		                $('#OrdersList').addClass('active');
		                $('#AmountList').removeClass('active');
				
				
		                $('div.orderchart').fadeIn();
		                $('div.amountchart').css('display', 'none');
				
			        break;

                    case 'AmountList':
				
                        $('#OrdersList').removeClass('active');
                        $('#AmountList').addClass('active');
				
				
                        $('div.amountchart').fadeIn();
				        $('div.orderchart').css('display', 'none');
				
			        break;
		        }
		
		        return false;
	            });
            });";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", strjquery, true);
        }
    }

}