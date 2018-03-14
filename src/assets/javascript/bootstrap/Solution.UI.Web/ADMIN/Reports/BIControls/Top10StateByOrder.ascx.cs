﻿using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Reports.BIControls
{
    public partial class Top10StateByOrder : System.Web.UI.UserControl
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
                int TotalRevenu = 0;
                ds = adminComponent.GetTop10State(DBAction, StartDate, EndDate, "Order");
                string str = string.Empty;
                str = "[['Location', 'Parent', 'Market trade volume (size)', 'Market increase/decrease (color)'],";
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    str = str + "['Global',null,0,0],";
                    int cnt = ds.Tables[0].Rows.Count;
                    #region Bind Country
                    string countrystr = "";
                    //foreach (DataRow item in ds.Tables[0].Rows)
                    //{
                    //    if (countrystr == "")
                    //    {
                    //        countrystr = Convert.ToString(item["Country"]);
                    //        str = str + "['" + Convert.ToString(item["Country"]) + "','Global',0,0],";
                    //    }
                    //    else
                    //    {
                    //        string[] strpl = countrystr.Split(',');
                    //        bool check = false;
                    //        foreach (var items in strpl)
                    //        {
                    //            if (items == Convert.ToString(item["Country"]))
                    //            {
                    //                check = true;
                    //            }
                    //        }
                    //        countrystr = countrystr + "," + Convert.ToString(item["Country"]);
                    //        if (check == false)
                    //        {
                    //            str = str + "['" + Convert.ToString(item["Country"]) + "','Global',0,0],";
                    //        }
                    //    }
                    //}
                    #endregion
                    int bcnt = 0;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        bcnt += 1;
                        TotalRevenu = TotalRevenu + Convert.ToInt32(Convert.ToString(item["TotalOrder"]));
                        if (cnt == bcnt)
                        {
                            str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-2]";
                        }
                        else
                        {
                            if (bcnt == 1)
                            {
                                str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-45],";
                            }
                            else if (bcnt == 2)
                            {
                                str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-37],";
                            }
                            else if (bcnt == 3)
                            {
                                str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-29],";
                            }
                            else if (bcnt == 4)
                            {
                                str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-24],";
                            }
                            else if (bcnt == 5)
                            {
                                str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-19],";
                            }
                            else if (bcnt == 6)
                            {
                                str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-15],";
                            }
                            else if (bcnt == 7)
                            {
                                str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-12],";
                            }
                            else if (bcnt == 8)
                            {
                                str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-8],";
                            }
                            else if (bcnt == 9)
                            {
                                str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-5],";
                            }
                            else if (bcnt == 10)
                            {
                                str = str + "['" + item["State"].ToString() + "','Global'," + Convert.ToInt32(Convert.ToString(item["TotalOrder"])) + ",-1],";
                            }

                        }
                    }
                }
                else
                {
                    str = str + "['Global',null,0,0],";
                    str = str + "['No Data','Global',0,0]";
                }
                str = str + "]";
                strScript += "google.load('visualization', '1', { packages: ['treemap'] });";
                strScript += "google.setOnLoadCallback(top10statebyorder);";
                strScript += "Sys.Application.add_load(top10statebyorder);";
                strScript += "function top10statebyorder() {";
                strScript += "var data = google.visualization.arrayToDataTable(" + str + ");";
                strScript += "tree = new google.visualization.TreeMap(document.getElementById('top10statebyorder'));";
                strScript += "tree.draw(data, {minColor: '#f00',midColor: '#ddd',maxColor: '#0d0',headerHeight: 15,fontColor: 'black',showScale: true,generateTooltip: showFullTooltip});";
                strScript += "function showFullTooltip(row, size, value) {" +
                                "return '<div style=\"background:#fd9; padding:10px; border-style:solid;border-width:1px\">" +
                                "<div style=\"font-weight:bold;width:100%;float:left\">' + data.getValue(row,0) + '</div>" +
                                "Total Order : ' + data.getValue(row,2) + ' </div>';}";
                strScript += "};";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Top10statebyorder", strScript, true);
            }
            catch (Exception e)
            {
                string str = e.ToString();
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