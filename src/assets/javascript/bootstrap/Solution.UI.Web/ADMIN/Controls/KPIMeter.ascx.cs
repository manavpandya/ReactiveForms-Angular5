using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using InfoSoftGlobal;
using System.Globalization;

namespace Solution.UI.Web.ADMIN.Controls
{
    public partial class KPIMeter : System.Web.UI.UserControl
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FillOption();
            FillView();
            FillKPIMeter(ddlOption.SelectedValue.ToString(), ddlOption.SelectedItem.Text.ToString());

            if (ddlView.SelectedIndex == 0)
            {
                ltrRevenue.Visible = true;
                ltrQuantity.Visible = false;
            }
            else
            {
                ltrRevenue.Visible = false;
                ltrQuantity.Visible = true;
            }
        }

        /// <summary>
        /// Fill Option function
        /// </summary>
        private void FillOption()
        {
            ddlOption.Items.Insert(0, new ListItem("Daily", "Daily"));
            ddlOption.Items.Insert(1, new ListItem("Weekly", "Weekly"));
            ddlOption.Items.Insert(2, new ListItem("This Month", "ThisMonth"));
            ddlOption.Items.Insert(3, new ListItem("Last Month", "LastMonth"));
            ddlOption.Items.Insert(4, new ListItem("Quarterly", "Quarterly"));
            ddlOption.Items.Insert(5, new ListItem("This Year", "ThisYear"));
            ddlOption.Items.Insert(6, new ListItem("Last Year", "LastYear"));
        }


        /// <summary>
        /// Fill Type of Chart display Function
        /// </summary>
        private void FillView()
        {
            ddlView.Items.Insert(0, new ListItem("Revenue", "Revenue"));
            ddlView.Items.Insert(1, new ListItem("Quantity", "Quantity"));
        }

        /// <summary>
        /// View Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlView.SelectedIndex == 0)
            {
                ltrRevenue.Visible = true;
                ltrQuantity.Visible = false;
            }
            else
            {
                ltrRevenue.Visible = false;
                ltrQuantity.Visible = true;
            }
        }

        /// <summary>
        /// Option Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillKPIMeter(ddlOption.SelectedValue.ToString(), ddlOption.SelectedItem.Text.ToString());
        }

        /// <summary>
        /// Calculate the Kilo formate of Decimal Amount
        /// </summary>
        /// <param name="num"></param>
        /// <returns>Returns the </returns>
        public string KiloFormat(decimal num)
        {
            if (num >= 100000000)
                return (num / 1000000).ToString("#,0.0M");

            if (num >= 10000000)
                return (num / 1000000).ToString("0.0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0.0K");

            if (num >= 10000)
                return (num / 1000).ToString("0.0.#") + "K";

            return num.ToString("#,0.00");
        }

        /// <summary>
        /// Fill KPI Meter function
        /// </summary>
        /// <param name="Duration">String Duration</param>
        /// <param name="Name">String Name</param>
        private void FillKPIMeter(String Duration, String Name)
        {
            DataSet dsKPIMeter = DashboardComponent.GetKPIMeter(Duration);

            if (dsKPIMeter != null && dsKPIMeter.Tables.Count > 0 && dsKPIMeter.Tables[0].Rows.Count > 0)
            {
                decimal Revenue = Convert.ToDecimal(dsKPIMeter.Tables[0].Rows[0]["Revenue"]);

                #region Calculatiton for dividing KPI Meter into 3 part
                decimal MaxValue = Convert.ToDecimal(dsKPIMeter.Tables[0].Rows[0]["MaxValue"]);

                decimal FirstPart = MaxValue / 3;
                decimal SecondPart = FirstPart + 1;
                decimal ThirdPart = SecondPart + FirstPart;
                decimal FourthPart = ThirdPart + 1;

                #endregion


                String strXML = "";
                strXML = "<chart upperLimit='" + MaxValue.ToString() + "' lowerLimit='0' majorTMNumber='6' majorTMHeight='8' showGaugeBorder='0' xAxisName='Revenue' manageValueOverlapping='1' autoAlignTickValues='0' formatNumberScale='1' numberPrefix='&nbsp;$' decimalPrecision='2' tickMarkDecimalPrecision='1' showPivotBorder='1' pivotBorderColor='000000' pivotBorderThickness='5' pivotFillMix='FFFFFF,000000' bgColor='FFFFFF' borderColor='f7f2ea' borderthickness='0' bgAlpha='100' gaugeOuterRadius='60' gaugeOriginX='100' gaugeOriginY='90' gaugeInnerRadius='2'>";

                // open dial XML element
                strXML += "<dials id='avgSRev'>";

                // add dial value (average revenue)
                strXML += "<dial value='" + Revenue.ToString() + "' borderAlpha='0' bgColor='000000' baseWidth='10' topWidth='1' radius='50'/>";

                // close dial XML element
                strXML += "</dials>";

                // add color range and annotation XML elements
                strXML += "<colorRange>";
                strXML += "<color minValue='0' maxValue='" + FirstPart.ToString("F2") + "' code='B41527'/>";
                strXML += "<color minValue='" + SecondPart.ToString("F2") + "' maxValue='" + ThirdPart.ToString("F2") + "' code='E48739'/>";
                strXML += "<color minValue='" + FourthPart.ToString("F2") + "' maxValue='" + MaxValue.ToString("F2") + "' code='399E38'/>";
                strXML += "</colorRange>";

                strXML += "<annotations><annotationGroup>";
                strXML += "<annotation type='circle' xPos='100' yPos='91' radius='62' startAngle='0' endAngle='180' fillPattern='linear' fillAsGradient='1' fillColor='dddddd,666666' fillAlpha='100,100' fillRatio='50,50' fillDegree='0' showBorder='1' borderColor='444444' borderThickness='2'/>";
                strXML += "<annotation type='circle' xPos='100' yPos='91' radius='55' startAngle='0' endAngle='180' fillPattern='linear' fillAsGradient='1' fillColor='666666,ffffff' fillAlpha='100,100' fillRatio='50,50' fillDegree='0'/>";
                strXML += "</annotationGroup></annotations>";

                // close chart XML element
                strXML += "</chart>";

                ltrRevenue.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/AngularGauge.swf", "", strXML, "myFirst", "220", "105", false);
                ltrRevenue.Text = ltrRevenue.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object><br/><div style=\"text-align:center;vertical-align:middle;font-family:Arial; color:#424242; font-size:12px\">" + Name + " Revenue : $" + KiloFormat(Convert.ToDecimal(Revenue.ToString("F2"))) + "</div>");
                ltrRevenue.Text = ltrRevenue.Text.Replace("borderColor='f7f2ea'", "borderColor='ffffff'");
            }
            else
            {
                ltrRevenue.Text = "<div style=\"text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">No Order available in requested duration.</div>";
            }

            if (dsKPIMeter != null && dsKPIMeter.Tables.Count > 0 && dsKPIMeter.Tables[1].Rows.Count > 0)
            {
                Int32 Quantity = Convert.ToInt32(dsKPIMeter.Tables[1].Rows[0]["Quantity"]);

                #region Calculatiton for dividing KPI Meter into 3 part
                Int32 MaxValue = Convert.ToInt32(dsKPIMeter.Tables[1].Rows[0]["MaxValue"]);
                if (MaxValue % 2 != 0)
                {
                    MaxValue = MaxValue + 1;
                }
                Int32 FirstPart = MaxValue / 3;
                Int32 SecondPart = FirstPart + 1;
                Int32 ThirdPart = SecondPart + FirstPart;
                Int32 FourthPart = ThirdPart + 1;

                String strXML = "";

                // open chat XML element
                strXML = "<chart upperLimit='" + MaxValue.ToString() + "' lowerLimit='0' majorTMNumber='6' majorTMHeight='8' showGaugeBorder='0' xAxisName='Revenue' manageValueOverlapping='1' autoAlignTickValues='0' formatNumberScale='1' numberPrefix='&nbsp;' decimalPrecision='2' tickMarkDecimalPrecision='1' showPivotBorder='1' pivotBorderColor='000000' pivotBorderThickness='5' pivotFillMix='FFFFFF,000000' bgColor='FFFFFF' borderColor='f7f2ea' borderthickness='0' bgAlpha='100' gaugeOuterRadius='60' gaugeOriginX='100' gaugeOriginY='90' gaugeInnerRadius='2'>";

                // add dial XMl element
                strXML += "<dials id='avgSQ'>";

                // add dial value- average sales quantity
                strXML += "<dial value='" + Quantity.ToString() + "' borderAlpha='0' bgColor='000000' baseWidth='10' topWidth='1' radius='50'/>";

                // close dial element
                strXML += "</dials>";

                // add color range and annotation XML elements
                strXML += "<colorRange>";
                strXML += "<color minValue='0' maxValue='" + FirstPart + "' code='B41527'/>";
                strXML += "<color minValue='" + SecondPart + "' maxValue='" + ThirdPart + "' code='E48739'/>";
                strXML += "<color minValue='" + FourthPart + "' maxValue='" + MaxValue + "' code='399E38'/>";
                strXML += "</colorRange>";

                strXML += "<annotations><annotationGroup>";
                strXML += "<annotation type='circle' xPos='100' yPos='91' radius='62' startAngle='0' endAngle='180' fillPattern='linear' fillAsGradient='1' fillColor='dddddd,666666' fillAlpha='100,100' fillRatio='50,50' fillDegree='0' showBorder='1' borderColor='444444' borderThickness='2'/>";
                strXML += "<annotation type='circle' xPos='100' yPos='91' radius='65' startAngle='0' endAngle='180' fillPattern='linear' fillAsGradient='1' fillColor='666666,ffffff' fillAlpha='100,100' fillRatio='50,50' fillDegree='0'/>";
                strXML += "</annotationGroup></annotations>";

                // close chart element
                strXML += "</chart>";

                ltrQuantity.Text = FusionCharts.RenderChartHTML("/Admin/FusionCharts/AngularGauge.swf", "", strXML, "myFirst", "220", "105", false);
                ltrQuantity.Text = ltrQuantity.Text.Replace("<embed", "<embed wmode='transparent' ").Replace("</object>", "<param name=\"wmode\" value=\"transparent\"> </object><br/><div style=\"text-align:center;vertical-align:middle;font-family:Arial; color:#424242; font-size:12px\">" + Name + " Sold Quantity : " + Quantity + "</div>");
                #endregion
            }
            else
            {
                ltrQuantity.Text = "<div style=\"text-align:center;vertical-align:middle;font-family:Arial; color:Maroon; font-size:12px\">No Order available in requested duration.</div>";
            }
        }
    }
}