using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;


namespace Solution.UI.Web.ADMIN.Controls
{
    public partial class KeyPerformanceIndicator : System.Web.UI.UserControl
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            FillKeyPerformanceIndicator();
        }

        /// <summary>
        /// Key Performance Indicator List
        /// </summary>
        private void FillKeyPerformanceIndicator()
        {
            string strKeyPerformance = "";
            strKeyPerformance += "<tr class=\"even-row\">";
            strKeyPerformance += "<th width=\"19\" style=\"border:none;\">";
            strKeyPerformance += "</th>";
            strKeyPerformance += "<th align=\"left\" width=\"230\" style=\"border:none;\">";
            strKeyPerformance += "Indicator";
            strKeyPerformance += "</th>";

            strKeyPerformance += "<th align=\"left\" width=\"230\" style=\"border:none;\">";
            strKeyPerformance += "Period";
            strKeyPerformance += "</th>";
            strKeyPerformance += "<th align=\"right\" width=\"75\" style=\"border:none;\">";
            strKeyPerformance += "Current";
            strKeyPerformance += "</th>";
            strKeyPerformance += "<th align=\"right\" width=\"100\" style=\"border:none;\">";
            strKeyPerformance += "Previous";
            strKeyPerformance += "</th>";
            strKeyPerformance += "<th align=\"right\" width=\"98\" style=\"border:none;\">";
            strKeyPerformance += "Change";
            strKeyPerformance += "</th>";
            strKeyPerformance += "</tr>";

            DataSet dsKeyprefomance = new DataSet();
            dsKeyprefomance = DashboardComponent.GetAllKeyPerformanceIndicator();
            int row = 1;
            if (dsKeyprefomance != null && dsKeyprefomance.Tables.Count > 0 && dsKeyprefomance.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsKeyprefomance.Tables.Count; i++)
                {
                    string SectionName = Convert.ToString(dsKeyprefomance.Tables[i].Rows[0][0]);
                    if (SectionName.ToString().Trim().ToLower() == "packagecount")
                    {
                        strKeyPerformance += "<tr><td colspan='6' style='padding-left:1%'><table cellpadding='0' cellspacing='0' width='100%'><tr style='background-color:#B3B3B3;color:#4b4b4b;font-weight:bold;font-size:12px;line-height:10px;'><td>Shipment Delivery</td></tr></table></td></tr>";
                    }
                    if ((row % 2 == 0) && (i != 0))
                    {
                        strKeyPerformance += "<tr class=\"even-row\">";
                    }
                    else
                    {
                        strKeyPerformance += "<tr>";
                    }


                    strKeyPerformance += "<td align=\"center\" width=\"19\" valign=\"middle\">";
                    strKeyPerformance += "<img title=\"\" alt=\"\" src=\"/App_Themes/" + Page.Theme + "/icon/red-arrow.png\">";
                    strKeyPerformance += "</td>";
                    strKeyPerformance += "<td>";
                    if (SectionName.ToString().Trim().ToLower() == "packagecount")
                    {
                        strKeyPerformance += "# of Packages";
                    }
                    else if (SectionName.ToString().Trim().ToLower() == "shipmentamount")
                    {
                        strKeyPerformance += "$ of Shipment";
                    }
                    else
                    {
                        strKeyPerformance += SectionName;
                    }
                    strKeyPerformance += " </td>";
                    strKeyPerformance += "<td>";
                    if (dsKeyprefomance.Tables[i].Columns.Count == 3)
                    {
                        strKeyPerformance += "<span style=\"text-decoration:none;\">This Month</span> v/s <span style=\"text-decoration:none;\">Last Month</span>";
                    }
                    else
                    {
                        strKeyPerformance += "Today";
                    }

                    strKeyPerformance += "</td>";
                    if (dsKeyprefomance.Tables[i].Columns.Count == 3)
                    {
                        decimal PreviousValue = Convert.ToDecimal(dsKeyprefomance.Tables[i].Rows[0][1]);
                        decimal CurrentValue = Convert.ToDecimal(dsKeyprefomance.Tables[i].Rows[0][2]);

                        strKeyPerformance += "<td align=\"right\">";

                        if (SectionName.ToString().ToLower() == "newleads" || SectionName.ToString().ToLower() == "packagecount")
                        {
                            strKeyPerformance += CurrentValue.ToString();
                        }
                        else
                        {
                            strKeyPerformance += CurrentValue.ToString("C2");
                        }

                        strKeyPerformance += "</td>";
                        strKeyPerformance += "<td align=\"right\">";
                        if (SectionName.ToString().ToLower() == "newleads" || SectionName.ToString().ToLower() == "packagecount")
                        {
                            strKeyPerformance += PreviousValue.ToString();
                        }
                        else
                        {
                            strKeyPerformance += PreviousValue.ToString("C2");
                        }

                        strKeyPerformance += "</td>";


                        if (CurrentValue == PreviousValue)
                        {
                            strKeyPerformance += "<td align=\"right\" class=\"\">";
                            strKeyPerformance += "<strong>0%</strong>";
                        }
                        else if (CurrentValue > PreviousValue)
                        {
                            strKeyPerformance += "<td align=\"right\" class=\"complete\">";
                            strKeyPerformance += "<img class=\"img-left\" title=\"\" alt=\"\" src=\"/App_Themes/" + Page.Theme + "/icon/green-arrow.png\"><strong>" + CalculatePercentage(CurrentValue, PreviousValue) + "%</strong>";

                        }
                        else if (CurrentValue < PreviousValue)
                        {
                            strKeyPerformance += "<td align=\"right\" class=\"cancelled\">";
                            strKeyPerformance += "<img class=\"img-left\" title=\"\" alt=\"\" src=\"/App_Themes/" + Page.Theme + "/icon/red-arrow1.png\"><strong>" + CalculatePercentage(CurrentValue, PreviousValue) + "%</strong>";
                        }
                        strKeyPerformance += "</td>";
                    }
                    else
                    {
                        decimal CurrentValue = Convert.ToDecimal(dsKeyprefomance.Tables[i].Rows[0][1]);
                        strKeyPerformance += "<td align=\"right\">";
                        if (SectionName.ToString().ToLower() == "newleads" || SectionName.ToString().ToLower() == "packagecount")
                        {
                            strKeyPerformance += CurrentValue.ToString();
                        }
                        else
                        {
                            strKeyPerformance += CurrentValue.ToString("C2");
                        }

                        strKeyPerformance += "</td>";
                        strKeyPerformance += "<td align=\"right\">";
                        strKeyPerformance += "";
                        strKeyPerformance += "</td>";
                        strKeyPerformance += "<td align=\"right\" class=\"cancelled\">";
                        strKeyPerformance += "";
                        strKeyPerformance += "</td>";
                    }
                    strKeyPerformance += "</tr>";
                    row++;
                }
            }

            else
            {
                strKeyPerformance += "<tr><td align=\"center\" style=\"color:red\" colspan=\"7\">No Key Performance Indicators Founds </td></tr>";
            }
            ltrIndicator.Text = strKeyPerformance.ToString();
        }

        /// <summary>
        /// Calculate Percentage function
        /// </summary>
        /// <param name="CurrVal">decimal CurrVal</param>
        /// <param name="PreValue">decimal PreValue</param>
        /// <returns>Returns Calculated function</returns>
        private String CalculatePercentage(decimal CurrVal, decimal PreValue)
        {
            decimal TotalChange = 0;
            decimal diff = 0;

            if (CurrVal > PreValue)
            {
                diff = CurrVal - PreValue;
                TotalChange = (diff * 100) / CurrVal;
            }
            else
            {
                diff = PreValue - CurrVal;
                TotalChange = (diff * 100) / PreValue;
            }
            TotalChange = Math.Abs(TotalChange);
            TotalChange = Math.Round(TotalChange);
            return TotalChange.ToString();
        }
    }
}