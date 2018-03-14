using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using InfoSoftGlobal;

namespace Solution.UI.Web.ADMIN.Controls
{
    public partial class ChartBusinessTrend : System.Web.UI.UserControl
    {

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FillChart(ddlOption.SelectedValue.ToString());
        }


        /// <summary>
        /// Fill Charts with Data for Order and Amount
        /// </summary>
        /// <param name="Duration">String Duration</param>
        public void FillChart(String Duration)
        {
            try
            {
                int TempQty = 0;
                string YaxeName = "Quantity";
                string XaxeName = "";
                string numberPrefix = "";
                if (Duration == "Last24hours")
                {
                    XaxeName = "Hours";
                }
                else if (Duration == "Last7days" || Duration == "ThisMonth" || Duration == "LastMonth")
                {
                    XaxeName = "Days";
                }
                else if (Duration == "ThisYear" || Duration == "LastYear" || Duration == "Quarterly")
                {
                    XaxeName = "Months";
                }

                DataSet dsChart = new DataSet();
                dsChart = DashboardComponent.GetChartDetails(Duration);
                if (dsChart != null && dsChart.Tables.Count > 0 && dsChart.Tables[0].Rows.Count > 0)
                {

                    string[] RandColor = { "ec1f7b", "77113f", "0b0206", "e585e9", "e23be8", "87058c", "c98bf0", "a83bed", "4c047a", "61d3eb", "098eaa", "244d56", "0b9e42", "99a911", "e1ae23", "f58d54", "b24206", "ed6b59", "c5230d" };
                    Random random = new Random();
                    String XMLChart = String.Empty;
                    String XMLChartAmount = String.Empty;
                    for (int i = 0; i < dsChart.Tables[0].Rows.Count; i++)
                    {
                        String strDaycolor = "";
                        if (Duration == "Last7days" || Duration == "ThisMonth" || Duration == "LastMonth")
                        {
                            if (Convert.ToString(dsChart.Tables[0].Rows[i]["DayNames"]).Trim().ToLower() == "monday")
                            {
                                strDaycolor = "557fff"; // Blue
                            }
                            else if (Convert.ToString(dsChart.Tables[0].Rows[i]["DayNames"]).Trim().ToLower() == "tuesday")
                            {
                                strDaycolor = "e3e35a"; // Yellow
                            }
                            else if (Convert.ToString(dsChart.Tables[0].Rows[i]["DayNames"]).Trim().ToLower() == "wednesday")
                            {
                                strDaycolor = "cb8aeb"; // Purple
                            }
                            else if (Convert.ToString(dsChart.Tables[0].Rows[i]["DayNames"]).Trim().ToLower() == "thursday")
                            {
                                strDaycolor = "6cd545"; // Green
                            }
                            else if (Convert.ToString(dsChart.Tables[0].Rows[i]["DayNames"]).Trim().ToLower() == "friday")
                            {
                                strDaycolor = "ff7fff"; // Pink
                            }
                            else if (Convert.ToString(dsChart.Tables[0].Rows[i]["DayNames"]).Trim().ToLower() == "saturday")
                            {
                                strDaycolor = "f5f5dc"; // Beige
                            }
                            else if (Convert.ToString(dsChart.Tables[0].Rows[i]["DayNames"]).Trim().ToLower() == "sunday")
                            {
                                strDaycolor = "3c3c3c"; // Black
                            }
                            else
                            {
                                strDaycolor = RandColor[random.Next(19)];
                            }
                        }
                        else
                        {
                            strDaycolor = RandColor[random.Next(19)];
                        }


                        string strskul = dsChart.Tables[0].Rows[i]["OrderSKU"].ToString();
                        if (strskul.Length > 35) strskul = strskul.Substring(0, 35) + "...";
                        XMLChart += "<set  value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["TotalOrder"]), 2)) + "'" + " name='" + dsChart.Tables[0].Rows[i]["Day"] + "'" + " color='" + strDaycolor.ToString() + "'  hoverText='" + strskul + "' />";
                        strskul += ", " + dsChart.Tables[0].Rows[i]["TotalOrder"].ToString();
                        XMLChartAmount += "<set value='" + Convert.ToString(Math.Round(Convert.ToDecimal(dsChart.Tables[0].Rows[i]["OrderTotal"]), 2)) + "'" + " name='" + dsChart.Tables[0].Rows[i]["Day"] + "'" + " color='" + strDaycolor.ToString() + "'  hoverText='" + strskul + "'   />";
                        TempQty += Convert.ToInt32(dsChart.Tables[0].Rows[i]["TotalOrder"].ToString());
                    }
                    XMLChart = "-<graph caption='" + XaxeName + " wise Order-Quantity Chart' xAxisName='" + XaxeName + "'  yAxisName='" + YaxeName + "' decimalPrecision='0' " + numberPrefix + " formatNumberScale='0'>" + XMLChart + "</graph>";
                    ltrChartOrder.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_Column3D.swf", "", XMLChart, "myFirst", "800", "300", false);
                    ltrChartOrder.Text = ltrChartOrder.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");


                    YaxeName = "Amount";
                    XMLChartAmount = "-<graph caption='" + XaxeName + " wise Order-Amount Chart' xAxisName='" + XaxeName + "' numberPrefix='$' yAxisName='" + YaxeName + "' decimalPrecision='2' " + numberPrefix + " formatNumberScale='0' >" + XMLChartAmount + "</graph>";
                    ltrChartAmount.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/FCF_Column3D.swf", "", XMLChartAmount, "myFirst", "800", "300", false);
                    ltrChartAmount.Text = ltrChartAmount.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object>");

                }
                else
                {
                    ltrChartOrder.Text = "<div style=\"height:180px;width:100%; text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">No Order available in requested duration.</div>";
                    ltrChartAmount.Text = "<div style=\"height:180px;width:100%; text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">No Order available in requested duration.</div>";
                }
                if (dsChart != null && dsChart.Tables.Count > 1 && dsChart.Tables[1].Rows.Count > 0) //Revenue,Tax,Shipping,Quantity
                {
                    Decimal dc = Decimal.Zero;
                    if (dsChart.Tables[0] != null)
                        for (int cnt = 0; cnt < dsChart.Tables[0].Rows.Count; cnt++)
                        {
                            dc += (!string.IsNullOrEmpty(dsChart.Tables[0].Rows[cnt]["OrderTotal"].ToString())) ? Convert.ToDecimal(dsChart.Tables[0].Rows[cnt]["OrderTotal"].ToString()) : Decimal.Zero;
                        }
                    decimal Revenue = Convert.ToDecimal(Math.Round(Convert.ToDecimal(dsChart.Tables[1].Rows[0]["Revenue"].ToString()), 2));
                    lblRevenue.Text = Revenue.ToString("C2");
                    decimal authorized = Convert.ToDecimal(Math.Round(dc, 2));
                    lblAuthorized.Text = authorized.ToString("C2");
                    decimal Tax = Convert.ToDecimal(Math.Round(Convert.ToDecimal(dsChart.Tables[2].Rows[0]["Tax"].ToString()), 2));
                    lblTax.Text = Tax.ToString("C2");
                    decimal shipping = Convert.ToDecimal(Math.Round(Convert.ToDecimal(dsChart.Tables[2].Rows[0]["Shipping"].ToString()), 2));
                    lblShipping.Text = shipping.ToString("C2");
                    lblQuantity.Text = Convert.ToString(TempQty); //Convert.ToString(dsChart.Tables[2].Rows[0]["Quantity"]);// dsChart.Tables[1].Rows[0]["Quantity"].ToString();
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

        /// <summary>
        /// for Re-Filling Charts according to Selected Duration
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillChart(ddlOption.SelectedValue.ToString());
            ReCallJQuery();
        }
    }
}