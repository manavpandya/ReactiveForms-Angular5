using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using System.Web.UI.DataVisualization;
using System.Text;
using Solution.Bussines.Entities;
using System.IO;
namespace Solution.UI.Web.ADMIN
{
    public partial class MarketingDashboard : BasePage
    {
        Dictionary<DateTime, int> testData = new Dictionary<DateTime, int>();

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminType"] != null && Session["AdminType"].ToString() == "0")
            {
                Response.Redirect("/Admin/Dashboard.aspx");
            }

            if (!IsPostBack)
            {
                Session["CountryUs"] = null;
                BindStore();
              //  bindCountry();
                DashboardComponent.ControlStoreID = Convert.ToInt32(ddlStore.SelectedValue);
               // GetOrderlistWord();
            }
            else
            {
                //bindCountry();
                Session["CountryUs"] = ddlCountryList.SelectedValue.ToString();
                if (Session["CountryUs"] != null)
                {
                    try
                    {
                        ddlCountryList.SelectedValue = Session["CountryUs"].ToString();
                    }
                    catch
                    {
                        ddlCountryList.SelectedIndex = 0;
                    }
                }
                if (ddlCountryList.SelectedIndex == 0)
                {
                  //  GetOrderlistWord();
                }
                else
                {
                   // GetOrderlist();
                }
            }
            if (Session["AdminID"] != null)
            {
                tb_Admin admin = null;
                string[] Rights = null;
                AdminRightsComponent objAdminRightsComp = new AdminRightsComponent();
                admin = objAdminRightsComp.GetAdminByID(Convert.ToInt32(Session["AdminID"]));
                if (admin != null && admin.Rights != null)
                {
                    Rights = admin.Rights.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (Rights != null && (Rights.Contains("7") || Rights.Contains("5")))
                    {
                        spanDashboardSetting.Visible = false;
                    }
                    else
                    {
                        spanDashboardSetting.Visible = false;
                        //spanStore.Visible = false;
                    }
                    LoadUserControls();

                }
                else
                {
                    spanDashboardSetting.Visible = false;
                    spanStore.Visible = false;
                }
                AdminRightsComponent objAdminRight = new AdminRightsComponent();
                objAdminRight.GetAllPageRightsByAdminID(Convert.ToInt32(Session["AdminID"]));
            }
            else
            {
                Response.Redirect("/admin/login.aspx");
            }
            Page.MaintainScrollPositionOnPostBack = true;
        }

        /// <summary>
        /// Loads the User Controls
        /// </summary>
        private void LoadUserControls()
        {
            int sid=Convert.ToInt32(ddlStore.SelectedValue.ToString());
            divleftControls.InnerHtml = "";
            divCenterControls.InnerHtml = "";
            divRightControls.InnerHtml = "";
            DataTable tabSetting = new DataTable();
            tabSetting.Columns.Add("Position",typeof(String));
            tabSetting.Columns.Add("SectionName", typeof(String));
            tabSetting.Columns.Add("ControlName", typeof(String));
            tabSetting.Columns.Add("IsDisplay", typeof(String));
            tabSetting.Columns.Add("DisplayPosition", typeof(Int32));
            tabSetting.Columns.Add("StoreID", typeof(Int32));

            tabSetting.Rows.Add("Left", "Top 10 Best Selling Colors", "Top10BestSellingColors.ascx", "True", 1, sid);
            tabSetting.Rows.Add("Left", "Top 10 Best Patterns", "Top10BestPatterns.ascx", "True", 1, sid);
            tabSetting.Rows.Add("Center", "Low Inventory Alert", "LowInventoryAlert.ascx", "True", 1, sid);
            tabSetting.Rows.Add("Right", "New Arrivals", "NewlyArrivalProducts.ascx", "True", 1, sid);
            tabSetting.Rows.Add("Right", "Top 10 Coupon List", "Promotions.ascx", "True", 1, sid);
            tabSetting.Rows.Add("Center", "Sales Agent Own Sale", "SalesAgentOwnSales.ascx", "True", 1, sid);
            tabSetting.Rows.Add("Center", "Sales Agent Own Open Quotes", "SalesAgentOwnOpenQuotes.ascx", "True", 1, sid);
            tabSetting.Rows.Add("Right", "Shortcuts", "ShortcutList.ascx", "True", 1, sid);
            

            DataSet dsDSettings = new DataSet();
            dsDSettings.Tables.Add(tabSetting);

            //dsDSettings = DashboardComponent.GetAllDashboardSettings(Convert.ToInt32(Session["AdminID"]));

            if (dsDSettings != null && dsDSettings.Tables.Count > 0 && dsDSettings.Tables[0].Rows.Count > 0)
            {

                Literal ltrCommon = null;
                String strControlPathPrefix = "/Admin/Controls/";
                tdLeft.Visible = true;
                tdCenter.Visible = true;
                tdRight.Visible = true;
                tdLeftCenter.Visible = true;
                tdCenterRight.Visible = true;
                int LeftCount = 0;
                int CenterCount = 0;
                int RightCount = 0;

                #region Load Left Controls
                try
                {
                    DataRow[] drLeftControls = dsDSettings.Tables[0].Select("Position='Left'", "DisplayPosition ASC");

                    LeftCount = drLeftControls.Count();
                    if (drLeftControls.Count() > 0)
                    {
                        tdLeftCenter.Visible = true;
                        String[] strLeftControlsName = new String[drLeftControls.Count()];
                        for (int j = 0; j < drLeftControls.Count(); j++)
                        {
                            strLeftControlsName[j] = strControlPathPrefix + drLeftControls[j]["ControlName"].ToString();
                        }

                        ltrCommon = new Literal();

                        ltrCommon.Text = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">";
                        divleftControls.Controls.Add(ltrCommon);
                        ltrCommon.Dispose();
                        for (int li = 0; li < strLeftControlsName.Count(); li++)
                        {
                            if (File.Exists(Server.MapPath(strLeftControlsName[li].ToString())))
                            {
                                ltrCommon = new Literal();
                                ltrCommon.Text = "<tr><td>";
                                divleftControls.Controls.Add(ltrCommon);
                                ltrCommon.Dispose();
                                UserControl usrcntrl = (UserControl)Page.LoadControl(strLeftControlsName[li].ToString());
                                usrcntrl.EnableViewState = false;
                                divleftControls.Controls.Add(usrcntrl);
                                ltrCommon = new Literal();
                                ltrCommon.Text = "</td></tr>";
                                divleftControls.Controls.Add(ltrCommon);
                                ltrCommon.Dispose();
                            }
                        }
                        ltrCommon = new Literal();
                        ltrCommon.Text = "</table>";
                        divleftControls.Controls.Add(ltrCommon);
                        ltrCommon.Dispose();
                    }
                    else
                    {
                        tdLeft.Visible = false;
                        tdLeftCenter.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion

                #region Load Center Controls
                try
                {
                    DataRow[] drCenterControls = dsDSettings.Tables[0].Select("Position='Center'", "DisplayPosition ASC");

                    CenterCount = drCenterControls.Count();
                    if (drCenterControls.Count() > 0)
                    {
                        String[] strCenterControlsName = new String[drCenterControls.Count()];
                        for (int k = 0; k < drCenterControls.Count(); k++)
                        {
                            strCenterControlsName[k] = strControlPathPrefix + drCenterControls[k]["ControlName"].ToString();
                        }

                        ltrCommon = new Literal();
                        ltrCommon.Text = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">";
                        divCenterControls.Controls.Add(ltrCommon);
                        ltrCommon.Dispose();
                        for (int ci = 0; ci < strCenterControlsName.Count(); ci++)
                        {
                            if (File.Exists(Server.MapPath(strCenterControlsName[ci].ToString())))
                            {
                                ltrCommon = new Literal();
                                ltrCommon.Text = "<tr><td>";
                                divCenterControls.Controls.Add(ltrCommon);
                                ltrCommon.Dispose();
                                UserControl usrcntrl = (UserControl)Page.LoadControl(strCenterControlsName[ci].ToString());
                                usrcntrl.EnableViewState = false;
                                divCenterControls.Controls.Add(usrcntrl);
                                ltrCommon = new Literal();
                                ltrCommon.Text = "</td></tr>";
                                divCenterControls.Controls.Add(ltrCommon);
                                ltrCommon.Dispose();
                            }
                        }
                        ltrCommon = new Literal();
                        ltrCommon.Text = "</table>";
                        divCenterControls.Controls.Add(ltrCommon);
                        ltrCommon.Dispose();
                    }
                    else
                    {
                        tdCenter.Visible = false;
                        tdCenterRight.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                #endregion

                #region Load Right Controls

                try
                {
                    DataRow[] drRightControls = dsDSettings.Tables[0].Select("Position='Right'", "DisplayPosition ASC");
                    RightCount = drRightControls.Count();
                    if (drRightControls.Count() > 0)
                    {
                        tdRight.Visible = true;
                        String[] strRightControlsName = new String[drRightControls.Count()];
                        for (int l = 0; l < drRightControls.Count(); l++)
                        {
                            strRightControlsName[l] = strControlPathPrefix + drRightControls[l]["ControlName"].ToString();
                        }

                        ltrCommon = new Literal();
                        ltrCommon.Text = "<table cellspacing=\"0\" align=\"center\" cellpadding=\"0\" border=\"0\" width=\"100%\">";
                        divRightControls.Controls.Add(ltrCommon);
                        ltrCommon.Dispose();
                        for (int ri = 0; ri < strRightControlsName.Count(); ri++)
                        {
                            if (File.Exists(Server.MapPath(strRightControlsName[ri].ToString())))
                            {
                                ltrCommon = new Literal();
                                ltrCommon.Text = "<tr><td>";
                                divRightControls.Controls.Add(ltrCommon);
                                ltrCommon.Dispose();
                                UserControl usrcntrl = (UserControl)Page.LoadControl(strRightControlsName[ri].ToString());
                                usrcntrl.EnableViewState = false;
                                divRightControls.Controls.Add(usrcntrl);
                                ltrCommon = new Literal();
                                ltrCommon.Text = "</td></tr>";
                                divRightControls.Controls.Add(ltrCommon);
                                ltrCommon.Dispose();
                            }
                        }
                        ltrCommon = new Literal();
                        ltrCommon.Text = "</table>";
                        divRightControls.Controls.Add(ltrCommon);
                        ltrCommon.Dispose();
                    }
                    else
                    {
                        tdRight.Visible = false;
                        tdLeftCenter.Visible = false;
                        tdCenterRight.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion
                if (LeftCount > 0 && CenterCount > 0)
                {
                    tdLeftCenter.Visible = true;
                }
                if (CenterCount > 0 && RightCount > 0)
                {
                    tdCenterRight.Visible = true;
                }
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
            //ddlStore.SelectedIndex = 1;
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            DashboardComponent.ControlStoreID = Convert.ToInt32(ddlStore.SelectedValue);
            LoadUserControls();
        }
        protected void ddlCountryList_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Session["CountryUs"] = ddlCountryList.SelectedValue.ToString();

            ////Response.Redirect(Request.Url.ToString());

            //if (ddlCountryList.SelectedIndex == 0)
            //{
            //    GetOrderlistWord();
            //    // Response.Redirect("Dashboard.aspx?Storeid=" + Convert.ToInt32(ddlStore.SelectedValue));

            //    //Page.ClientScript.RegisterStartupScript(Page.GetType(),"mapload"," google.load('visualization', '1', {'packages': ['geochart']});google.setOnLoadCallback(drawRegionsMap);",true);

            //}
            //else
            //{
            //    GetOrderlist();

            //    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "", true);
            //    // Response.Redirect("Dashboard.aspx?Storeid=" + Convert.ToInt32(ddlStore.SelectedValue) + "&Country=" + ddlCountryList.SelectedValue + "");
            //}
            ScriptManager.RegisterStartupScript(this, this.GetType(), "postbackscript", "$('html, body').animate({ scrollTop: $('#countryorder').offset().top }, 'slow');", true);

        }
        private void bindCountry()
        {
            ddlCountryList.Items.Clear();
            ddlCountryList.Items.Insert(0, new ListItem("World", "World"));
            DataSet dsOrder = new DataSet();
            if (ddlStore.SelectedIndex == 0)
            {
                dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT Count(*) over(partition by ShippingCountry) as TotalCount,ShippingCountry FROM tb_order WHERE   isnull(Deleted,0)=0 AND ISNULL(ShippingCountry,'') <> ''");
            }
            else
            {
                dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT Count(*) over(partition by ShippingCountry) as TotalCount,ShippingCountry FROM tb_order WHERE Storeid=" + ddlStore.SelectedValue.ToString() + " AND isnull(Deleted,0)=0 AND ISNULL(ShippingCountry,'') <> ''");
            }

            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsOrder.Tables[0].Rows.Count; i++)
                {
                    ddlCountryList.Items.Insert((i + 1), new ListItem(dsOrder.Tables[0].Rows[i]["ShippingCountry"].ToString(), dsOrder.Tables[0].Rows[i]["ShippingCountry"].ToString()));
                }

            }
            if (Session["CountryUs"] == null)
            {
                ddlCountryList.SelectedIndex = 0;
            }
            else
            {
                ddlCountryList.SelectedValue = Session["CountryUs"].ToString();
            }

        }

        private void GetOrderlist()
        {
            string strScript = "";
            strScript += "google.load('visualization', '1', { 'packages': ['geochart'] });";
            strScript += "google.setOnLoadCallback(drawRegionsMap);";

            strScript += "function drawRegionsMap() {";
            strScript += "document.getElementById('OrderChart').innerHTML = '';";

            bool flg = false;

            DataSet dsOrder = new DataSet();
            if (ddlStore.SelectedIndex == 0)
            {
                dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT Count(*) over(partition by ShippingState) as TotalCount,ShippingState FROM tb_order WHERE   isnull(Deleted,0)=0 AND ISNULL(ShippingState,'') <> ''");
            }
            else
            {
                dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT Count(*) over(partition by ShippingState) as TotalCount,ShippingState FROM tb_order WHERE Storeid=" + ddlStore.SelectedValue.ToString() + " AND isnull(Deleted,0)=0 AND ISNULL(ShippingState,'') <> ''");
            }

            string strdata = "";
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                flg = true;
                strdata = "[['State', 'No of Orders'],";
                for (int i = 0; i < dsOrder.Tables[0].Rows.Count; i++)
                {
                    if (i == dsOrder.Tables[0].Rows.Count - 1)
                    {
                        strdata += " ['" + dsOrder.Tables[0].Rows[i]["ShippingState"].ToString() + "', " + dsOrder.Tables[0].Rows[i]["TotalCount"].ToString() + "]";
                    }
                    else
                    {
                        strdata += " ['" + dsOrder.Tables[0].Rows[i]["ShippingState"].ToString() + "', " + dsOrder.Tables[0].Rows[i]["TotalCount"].ToString() + "],";
                    }
                }
                strdata += "]";

            }

            strScript += "var data_orders = google.visualization.arrayToDataTable(" + strdata + ");";
            if (ddlStore.SelectedIndex == 0)
            {
                dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT COUNT( dbo.tb_Address.State) over(partition by dbo.tb_Address.State) as TotalCount,dbo.tb_Address.State FROM dbo.tb_Customer INNER JOIN dbo.tb_Address ON dbo.tb_Customer.CustomerID = dbo.tb_Address.CustomerID  WHERE dbo.tb_Address.AddressType=1 AND ISNULL(dbo.tb_Customer.Deleted,0)=0 AND ISNULL(dbo.tb_Customer.Deleted,0)=0 AND ISNULL(dbo.tb_Address.State,'') <> ''");
            }
            else
            {
                dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT COUNT( dbo.tb_Address.State) over(partition by dbo.tb_Address.State) as TotalCount,dbo.tb_Address.State FROM dbo.tb_Customer INNER JOIN dbo.tb_Address ON dbo.tb_Customer.CustomerID = dbo.tb_Address.CustomerID  WHERE dbo.tb_Address.AddressType=1 AND ISNULL(dbo.tb_Customer.Deleted,0)=0 and ISNULL(dbo.tb_Customer.StoreId,0)=" + ddlStore.SelectedValue.ToString() + " AND ISNULL(dbo.tb_Customer.Deleted,0)=0 AND ISNULL(dbo.tb_Address.State,'') <> ''");
            }
            strdata = "";
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                strdata = "[['State', 'No of Customers'],";
                for (int i = 0; i < dsOrder.Tables[0].Rows.Count; i++)
                {
                    if (i == dsOrder.Tables[0].Rows.Count - 1)
                    {
                        strdata += " ['" + dsOrder.Tables[0].Rows[i]["State"].ToString() + "', " + dsOrder.Tables[0].Rows[i]["TotalCount"].ToString() + "]";
                    }
                    else
                    {
                        strdata += " ['" + dsOrder.Tables[0].Rows[i]["State"].ToString() + "', " + dsOrder.Tables[0].Rows[i]["TotalCount"].ToString() + "],";
                    }
                }
                strdata += "]";

            }
            strScript += "document.getElementById('CustomerChart').innerHTML = '';";
            strScript += "var data_customers = google.visualization.arrayToDataTable(" + strdata + ");";

            // create chart object
            strScript += "var chart_orders = new google.visualization.GeoChart(";
            strScript += "document.getElementById('OrderChart')";
            strScript += ");";

            // create chart object
            strScript += "var chart_customers = new google.visualization.GeoChart(";
            strScript += "document.getElementById('CustomerChart')";
            strScript += ");";


            strScript += "var view_orders = new google.visualization.DataView(data_orders);";
            strScript += "view_orders.setColumns([0, 1]);";

            strScript += "var view_customers = new google.visualization.DataView(data_customers);";
            strScript += "view_customers.setColumns([0, 1]);";
            string strCountry = "";

            strCountry = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 TwoLetterISOCode FROM tb_Country WHERE Name='" + ddlCountryList.SelectedValue.ToString().Replace("'", "''") + "'"));

            // set options for this chart
            strScript += "var options_orders =";
            strScript += "{";
            strScript += "width: 'auto',";
            strScript += "region: '" + strCountry.ToString() + "',";
            strScript += "resolution: 'provinces',";
            if (Page.Theme.ToString().ToLower() == "gray")
            {
                strScript += "colorAxis: { colors: ['#dcdcdc', '#4B4B4B'] },";
            }
            else if (Page.Theme.ToString().ToLower() == "red")
            {
                strScript += "colorAxis: { colors: ['#ffe4b5', '#DC3912'] },";
            }
            else if (Page.Theme.ToString().ToLower() == "blue")
            {
                strScript += "colorAxis: { colors: ['#add8e6', '#3366CC'] },";
            }
            // strScript += "colorAxis: { colors: ['#fab32a', '#be7700'] },";
            strScript += "legend: 'none'";
            strScript += "};";
            strScript += "var options_customers =";
            strScript += "{";
            strScript += "width: 'auto',";
            strScript += "region: '" + strCountry.ToString() + "',";
            strScript += "resolution: 'provinces',";
            //strScript += "colorAxis: { colors: ['#fab32a', '#be7700'] },";
            if (Page.Theme.ToString().ToLower() == "gray")
            {
                strScript += "colorAxis: { colors: ['#dcdcdc', '#4B4B4B'] },";
            }
            else if (Page.Theme.ToString().ToLower() == "red")
            {
                strScript += "colorAxis: { colors: ['#ffe4b5', '#DC3912'] },";
            }
            else if (Page.Theme.ToString().ToLower() == "blue")
            {
                strScript += "colorAxis: { colors: ['#add8e6', '#3366CC'] },";
            }
            strScript += "legend: 'none'";
            strScript += "};";
            strScript += "chart_orders.draw(view_orders, options_orders);";
            strScript += "chart_customers.draw(view_customers, options_customers);";
            strScript += "$('#CustomerChart').css('display', 'none');";
            strScript += "};";
            if (flg)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgmapload" + strCountry.ToString().Replace(" ", "_"), strScript, true);
            }
        }
        private void GetOrderlistWord()
        {

            string strScript = "";
            bool flg = false;
            strScript += "google.load('visualization', '1', {'packages': ['geochart']});";
            strScript += "google.setOnLoadCallback(drawRegionsMap);";

            strScript += "function drawRegionsMap() {";

            strScript += "var data = google.visualization.arrayToDataTable([";
            DataSet dsOrder = new DataSet();
            if (ddlStore.SelectedIndex == 0)
            {
                dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT Count(*) over(partition by ShippingCountry) as TotalCount,ShippingCountry FROM tb_order WHERE   isnull(Deleted,0)=0 AND ISNULL(ShippingCountry,'') <> ''");
            }
            else
            {
                dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT Count(*) over(partition by ShippingCountry) as TotalCount,ShippingCountry FROM tb_order WHERE Storeid=" + ddlStore.SelectedValue.ToString() + " AND isnull(Deleted,0)=0 AND ISNULL(ShippingCountry,'') <> ''");
            }
            string strdata = "";
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                flg = true;
                strScript += "['Country', 'No of Orders'],";
                for (int i = 0; i < dsOrder.Tables[0].Rows.Count; i++)
                {
                    //ddlCountryList.Items.Insert((i + 1), new ListItem(dsOrder.Tables[0].Rows[i]["ShippingCountry"].ToString(), dsOrder.Tables[0].Rows[i]["ShippingCountry"].ToString()));
                    if (i == dsOrder.Tables[0].Rows.Count - 1)
                    {
                        strScript += " ['" + dsOrder.Tables[0].Rows[i]["ShippingCountry"].ToString() + "', " + dsOrder.Tables[0].Rows[i]["TotalCount"].ToString() + "]";

                    }
                    else
                    {
                        strScript += " ['" + dsOrder.Tables[0].Rows[i]["ShippingCountry"].ToString() + "', " + dsOrder.Tables[0].Rows[i]["TotalCount"].ToString() + "],";
                    }
                }


            }

            strScript += "]);";

            strScript += "var options = {";
            if (Page.Theme.ToString().ToLower() == "gray")
            {
                strScript += "colorAxis: { colors: ['#dcdcdc', '#4B4B4B'] }};";
            }
            else if (Page.Theme.ToString().ToLower() == "red")
            {
                strScript += "colorAxis: { colors: ['#ffe4b5', '#DC3912'] }};";
            }
            else if (Page.Theme.ToString().ToLower() == "blue")
            {
                strScript += "colorAxis: { colors: ['#add8e6', '#3366CC'] }};";
            }
            strScript += "var chart = new google.visualization.GeoChart(document.getElementById('OrderChart'));";


            strScript += "chart.draw(data, options);";


            strScript += "var dataCutomers = google.visualization.arrayToDataTable([";
            if (ddlStore.SelectedIndex == 0)
            {
                dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT COUNT( dbo.tb_Address.Country) over(partition by dbo.tb_Country.Name) as TotalCount,dbo.tb_Country.Name FROM dbo.tb_Customer INNER JOIN dbo.tb_Address ON dbo.tb_Customer.CustomerID = dbo.tb_Address.CustomerID INNER JOIN tb_Country On tb_Address.Country = tb_Country.CountryID  WHERE dbo.tb_Address.AddressType=1 AND ISNULL(dbo.tb_Customer.Deleted,0)=0 AND ISNULL(dbo.tb_Customer.Deleted,0)=0 AND ISNULL(dbo.tb_Address.Country,'') <> ''");
            }
            else
            {
                dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT COUNT( dbo.tb_Address.Country) over(partition by dbo.tb_Country.Name) as TotalCount,dbo.tb_Country.Name FROM dbo.tb_Customer INNER JOIN dbo.tb_Address ON dbo.tb_Customer.CustomerID = dbo.tb_Address.CustomerID INNER JOIN tb_Country On tb_Address.Country = tb_Country.CountryID  WHERE dbo.tb_Address.AddressType=1 AND ISNULL(dbo.tb_Customer.Deleted,0)=0 and ISNULL(dbo.tb_Customer.StoreId,0)=" + ddlStore.SelectedValue.ToString() + " AND ISNULL(dbo.tb_Customer.Deleted,0)=0 AND ISNULL(dbo.tb_Address.Country,'') <> ''");
            }
            strdata = "";

            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                strScript += "['Country', 'No of Customers'],";
                for (int i = 0; i < dsOrder.Tables[0].Rows.Count; i++)
                {
                    if (i == dsOrder.Tables[0].Rows.Count - 1)
                    {
                        strScript += " ['" + dsOrder.Tables[0].Rows[i]["Name"].ToString() + "', " + dsOrder.Tables[0].Rows[i]["TotalCount"].ToString() + "]";
                    }
                    else
                    {
                        strScript += " ['" + dsOrder.Tables[0].Rows[i]["Name"].ToString() + "', " + dsOrder.Tables[0].Rows[i]["TotalCount"].ToString() + "],";
                    }
                }

            }
            strScript += "]);";

            strScript += "var optionsCustomers = {";
            if (Page.Theme.ToString().ToLower() == "gray")
            {
                strScript += "colorAxis: { colors: ['#dcdcdc', '#4B4B4B'] }};";
            }
            else if (Page.Theme.ToString().ToLower() == "red")
            {
                strScript += "colorAxis: { colors: ['#ffe4b5', '#DC3912'] }};";
            }
            else if (Page.Theme.ToString().ToLower() == "blue")
            {
                strScript += "colorAxis: { colors: ['#add8e6', '#3366CC'] }};";
            }
            strScript += "var chartCustomers = new google.visualization.GeoChart(document.getElementById('CustomerChart'));";


            strScript += "chartCustomers.draw(dataCutomers, optionsCustomers);";

            strScript += "$('#CustomerChart').css('display', 'none');";
            strScript += "};";




            //if (ddlStore.SelectedIndex == 0)
            //{
            //    dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT COUNT( dbo.tb_Address.Country) over(partition by dbo.tb_Country.Name) as TotalCount,dbo.tb_Country.Name FROM dbo.tb_Customer INNER JOIN dbo.tb_Address ON dbo.tb_Customer.CustomerID = dbo.tb_Address.CustomerID INNER JOIN tb_Country On tb_Address.Country = tb_Country.CountryID  WHERE dbo.tb_Address.AddressType=1 AND ISNULL(dbo.tb_Customer.Deleted,0)=0 AND ISNULL(dbo.tb_Customer.Deleted,0)=0 AND ISNULL(dbo.tb_Address.Country,'') <> ''");
            //}
            //else
            //{
            //    dsOrder = CommonComponent.GetCommonDataSet("SELECT  DISTINCT COUNT( dbo.tb_Address.Country) over(partition by dbo.tb_Country.Name) as TotalCount,dbo.tb_Country.Name FROM dbo.tb_Customer INNER JOIN dbo.tb_Address ON dbo.tb_Customer.CustomerID = dbo.tb_Address.CustomerID INNER JOIN tb_Country On tb_Address.Country = tb_Country.CountryID  WHERE dbo.tb_Address.AddressType=1 AND ISNULL(dbo.tb_Customer.Deleted,0)=0 and ISNULL(dbo.tb_Customer.StoreId,0)=" + ddlStore.SelectedValue.ToString() + " AND ISNULL(dbo.tb_Customer.Deleted,0)=0 AND ISNULL(dbo.tb_Address.Country,'') <> ''");
            //}
            //strdata = "";
            //if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            //{
            //    strdata = "[['State', 'No of Customers'],";
            //    for (int i = 0; i < dsOrder.Tables[0].Rows.Count; i++)
            //    {
            //        if (i == dsOrder.Tables[0].Rows.Count - 1)
            //        {
            //            strdata += " ['" + dsOrder.Tables[0].Rows[i]["Name"].ToString() + "', " + dsOrder.Tables[0].Rows[i]["TotalCount"].ToString() + "]";
            //        }
            //        else
            //        {
            //            strdata += " ['" + dsOrder.Tables[0].Rows[i]["Name"].ToString() + "', " + dsOrder.Tables[0].Rows[i]["TotalCount"].ToString() + "],";
            //        }
            //    }
            //    strdata += "]";

            //}

            if (flg)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgmapload" + ddlCountryList.SelectedValue.ToString().Replace(" ", "_"), strScript, true);
            }
        }
    }
}