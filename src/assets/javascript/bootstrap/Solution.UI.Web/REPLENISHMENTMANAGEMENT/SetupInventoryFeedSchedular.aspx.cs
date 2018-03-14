using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.IO;
using Solution.Bussines.Components.Common;
using System.Threading;
using LumenWorks.Framework.IO.Csv;
using System.Data;
using Solution.Bussines.Components;

namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT
{
    public partial class SetupInventoryFeedSchedular : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("/Admin/login.aspx");
            }

            if (!IsPostBack)
            {
                BindStore();
                ShowExistSchedular();
                // BindHtml();
            }
        }

        private void BindStore()
        {
            GenerateInventoryFeedComponent objInv = new GenerateInventoryFeedComponent();
            DataSet dsStore = new DataSet();
            dsStore = objInv.GetSalesPartnerList();
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlstore.DataSource = dsStore.Tables[0];
                ddlstore.DataTextField = "StoreName";
                ddlstore.DataValueField = "RepStoreID";
                ddlstore.DataBind();
            }

            else
            {
                ddlstore.DataSource = null;
            }
            ddlstore.Items.Insert(0, new ListItem("Select Channel Partner", "0"));
        }


        private void ShowExistSchedular()
        {
            ltrgroupdata.Text = "";
            Literal1.Text = "";
            divcolumndata.InnerHtml = "";
            DataSet DsData = new DataSet();
            DsData = CommonComponent.GetCommonDataSet("select * from tb_FeedSchedular where storeid=" + ddlstore.SelectedValue.ToString() + " and isnull(deleted,0)=0 and isnull(active,0)=1");
            if (DsData != null && DsData.Tables.Count > 0 && DsData.Tables[0].Rows.Count > 0)
            {
                String Strhtml = "";
                String Strhtml1 = "";
                for (int i = 0; i < DsData.Tables[0].Rows.Count; i++)
                {




                    //Strhtml1 += "<div class=\"form-group\">";
                    //Strhtml1 += "<label class=\"col-md-3 control-label\">&nbsp;</label>";

                    if (DsData.Tables[0].Rows.Count - 1 == i)
                    {
                        Strhtml1 += "<div class=\"form-group\">";
                        Strhtml1 += "<label class=\"col-md-3 control-label\">&nbsp;</label>";
                        Strhtml1 += "<div class=\"col-md-6\">";
                        Strhtml1 += "<button type=\"button\" class=\"btn btn-orang\" id=\"btnmore_" + i.ToString() + "\" onclick=\"addmorecolumn(" + DsData.Tables[0].Rows.Count.ToString() + ");\"><i class=\"fa fa-plus\"></i>Add Scheduler</button>";
                        Strhtml1 += "</div>";
                        Strhtml1 += "</div>";
                    }
                    else
                    {
                        //Strhtml1 += "<button type=\"button\" class=\"btn btn-orang\" id=\"btnmore_" + i.ToString() + "\" onclick=\"addmorecolumn(##kau_new###);\"><i class=\"fa fa-plus\"></i>Add Scheduler</button>";
                    }




                    Strhtml += "  <div class=\"form-group\" id=\"div_" + i.ToString() + "\">";
                    Strhtml += "<div class=\"col-md-3 control-label\">&nbsp;</div>";
                    Strhtml += "<div class=\"col-md-6\">";

                    Strhtml += "<div class=\"panel fa-border\">";

                    Strhtml += "<div class=\"panel-heading\" >Scheduler <label id=\"scheduler_" + i.ToString() + "\">" + (i + 1).ToString() + "</label> <span class=\"tools pull-right\">";
                    //if (DsData.Tables[0].Rows.Count - 1 != i)
                    //{
                    Strhtml += "<button type=\"button\" id=\"btnremove_" + i.ToString() + "\"  onclick=\"removecolumn(" + i.ToString() + ");\" class=\"btn btn-xs btn-orang\">Remove</button>";
                    //}
                    Strhtml += "</span></div>";

                    Strhtml += "<div class=\"panel-body\" id=\"verify-pro\">";
                    Strhtml += "<div action=\"#\" class=\"form-horizontal\">";
                    Strhtml += "<div class=\"form-group\">";
                    Strhtml += "<label class=\"control-label col-md-3\">Time</label>";
                    Strhtml += "<div class=\"col-md-6\">";

                    Strhtml += "<div class=\"form-control-static\"><strong><input type=\"text\"  name=\"txttime_" + i.ToString() + "\" id=\"txttime_" + i.ToString() + "\" value=\"" + DsData.Tables[0].Rows[i]["SchedularTime"].ToString() + "\" /></strong> <br/><span> (eg: hh:mm:ss AM or PM) </span> </div>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "<div class=\"form-group\">";
                    Strhtml += "<label class=\"control-label col-md-3\">Days</label>";
                    Strhtml += "<div class=\"col-md-9 icheck minimal\">";
                    Strhtml += "<div class=\"row\">";
                    Strhtml += "<div class=\"radio single-row\">";
                    if (!string.IsNullOrEmpty(DsData.Tables[0].Rows[i]["Mon"].ToString()) && Convert.ToBoolean(DsData.Tables[0].Rows[i]["Mon"].ToString()) == true)
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" checked  id=\"chk_mon_" + i.ToString() + "\"  onchange=\"javascript:checkparent(this);\"  name=\"chk_mon_" + i.ToString() + "\">";
                    }
                    else
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\"  id=\"chk_mon_" + i.ToString() + "\"  onchange=\"javascript:checkparent(this);\"  name=\"chk_mon_" + i.ToString() + "\">";
                    }
                    // Strhtml += "<input tabindex=\"3\" type=\"checkbox\"  id=\"chk_mon_" + i.ToString() + "\"  onchange=\"javascript:checkparent(this);\"  name=\"chk_mon_" + i.ToString() + "\">";
                    Strhtml += "<label>Mon </label>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "<div class=\"row\">";
                    Strhtml += "<div class=\"radio single-row\">";
                    if (!string.IsNullOrEmpty(DsData.Tables[0].Rows[i]["Tues"].ToString()) && Convert.ToBoolean(DsData.Tables[0].Rows[i]["Tues"].ToString()) == true)
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" checked id=\"chk_tues_" + i.ToString() + "\"  onchange=\"javascript:checkparent(this);\" name=\"chk_tues_" + i.ToString() + "\">";
                    }
                    else
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_tues_" + i.ToString() + "\"  onchange=\"javascript:checkparent(this);\" name=\"chk_tues_" + i.ToString() + "\">";

                    }
                    Strhtml += "<label>Tues </label>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "<div class=\"row\">";
                    Strhtml += "<div class=\"radio single-row\">";
                    if (!string.IsNullOrEmpty(DsData.Tables[0].Rows[i]["Wed"].ToString()) && Convert.ToBoolean(DsData.Tables[0].Rows[i]["Wed"].ToString()) == true)
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" checked id=\"chk_wed_" + i.ToString() + "\" onchange=\"javascript:checkparent(this);\"  name=\"chk_wed_" + i.ToString() + "\">";
                    }
                    else
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_wed_" + i.ToString() + "\" onchange=\"javascript:checkparent(this);\"  name=\"chk_wed_" + i.ToString() + "\">";
                    }
                    Strhtml += "<label>Wed </label>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "<div class=\"row\">";
                    Strhtml += "<div class=\"radio single-row\">";
                    if (!string.IsNullOrEmpty(DsData.Tables[0].Rows[i]["Thur"].ToString()) && Convert.ToBoolean(DsData.Tables[0].Rows[i]["Thur"].ToString()) == true)
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" checked id=\"chk_thur_" + i.ToString() + "\" onchange=\"javascript:checkparent(this);\"  name=\"chk_thur_" + i.ToString() + "\">";
                    }
                    else
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_thur_" + i.ToString() + "\" onchange=\"javascript:checkparent(this);\"  name=\"chk_thur_" + i.ToString() + "\">";
                    }
                    Strhtml += "<label>Thu </label>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "<div class=\"row\">";
                    Strhtml += " <div class=\"radio single-row\">";
                    if (!string.IsNullOrEmpty(DsData.Tables[0].Rows[i]["Fri"].ToString()) && Convert.ToBoolean(DsData.Tables[0].Rows[i]["Fri"].ToString()) == true)
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" checked id=\"chk_fri_" + i.ToString() + "\" onchange=\"javascript:checkparent(this);\"  name=\"chk_fri_" + i.ToString() + "\">";
                    }
                    else
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_fri_" + i.ToString() + "\" onchange=\"javascript:checkparent(this);\"  name=\"chk_fri_" + i.ToString() + "\">";
                    }
                    Strhtml += "<label>Fri </label>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "<div class=\"row\">";
                    Strhtml += "<div class=\"radio single-row\">";
                    if (!string.IsNullOrEmpty(DsData.Tables[0].Rows[i]["Sat"].ToString()) && Convert.ToBoolean(DsData.Tables[0].Rows[i]["Sat"].ToString()) == true)
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" checked id=\"chk_sat_" + i.ToString() + "\" onchange=\"javascript:checkparent(this);\"  name=\"chk_sat_" + i.ToString() + "\">";
                    }
                    else
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_sat_" + i.ToString() + "\" onchange=\"javascript:checkparent(this);\"  name=\"chk_sat_" + i.ToString() + "\">";
                    }
                    Strhtml += "<label>Sat </label>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "<div class=\"row\">";
                    Strhtml += "<div class=\"radio single-row\">";
                    if (!string.IsNullOrEmpty(DsData.Tables[0].Rows[i]["Sun"].ToString()) && Convert.ToBoolean(DsData.Tables[0].Rows[i]["Sun"].ToString()) == true)
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" checked id=\"chk_sun_" + i.ToString() + "\" onchange=\"javascript:checkparent(this);\"  name=\"chk_sun_" + i.ToString() + "\"><input type=\"hidden\" id=\"hdn_" + i.ToString() + "\" name=\"hdn_" + i.ToString() + "\" value=\"0\" />";
                    }
                    else
                    {
                        Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_sun_" + i.ToString() + "\" onchange=\"javascript:checkparent(this);\"  name=\"chk_sun_" + i.ToString() + "\"><input type=\"hidden\" id=\"hdn_" + i.ToString() + "\" name=\"hdn_" + i.ToString() + "\" value=\"0\" />";
                    }
                    Strhtml += "<label>Sun </label>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    //Strhtml += "<div class=\"form-group\">";
                    //Strhtml += "<label class=\"control-label col-md-3\">";
                    //Strhtml += "<button type=\"button\" class=\"btn btn-orang\">Cancel</button>";
                    //Strhtml += "</label>";
                    //Strhtml += "<div class=\"col-md-6\">";
                    //Strhtml += "<div class=\"form-control-static\">";
                    //Strhtml += "<button type=\"button\" class=\"btn btn-orang\">Save</button>";
                    //Strhtml += "</div>";
                    //Strhtml += "</div>";
                    //Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                    Strhtml += "</div>";
                }
                Literal1.Text = Strhtml;
                ltrgroupdata.Text = Strhtml1;
                BindHtmlDiv();
            }
            else
            {
                BindHtml();

            }


        }

        private void BindHtml()
        {
            String Strhtml = "";
            String Strhtml1 = "";



            Strhtml1 += "<div class=\"form-group\">";
            Strhtml1 += "<label class=\"col-md-3 control-label\">&nbsp;</label>";
            Strhtml1 += "<div class=\"col-md-6\">";
            Strhtml1 += "<button type=\"button\" class=\"btn btn-orang\" id=\"btnmore_##kau###\" onclick=\"addmorecolumn(##kau_new###);\"><i class=\"fa fa-plus\"></i>Add Scheduler</button>";

            Strhtml1 += "</div>";
            Strhtml1 += "</div>";

            Strhtml += "  <div class=\"form-group\" id=\"div_##kau###\">";
            Strhtml += "<div class=\"col-md-3 control-label\">&nbsp;</div>";
            Strhtml += "<div class=\"col-md-6\">";

            Strhtml += "<div class=\"panel fa-border\">";

            Strhtml += "<div class=\"panel-heading\" >Scheduler <label id=\"scheduler_##kau###\">1</label> <span class=\"tools pull-right\">";
            Strhtml += "<button type=\"button\" id=\"btnremove_##kau###\"  onclick=\"removecolumn(##kau###);\" style=\"display:none;\" class=\"btn btn-xs btn-orang\">Remove</button>";
            Strhtml += "</span></div>";
            Strhtml += "<div class=\"panel-body\" id=\"verify-pro\">";
            Strhtml += "<div action=\"#\" class=\"form-horizontal\">";
            Strhtml += "<div class=\"form-group\">";
            Strhtml += "<label class=\"control-label col-md-3\">Time</label>";
            Strhtml += "<div class=\"col-md-6\">";
            Strhtml += "<div class=\"form-control-static\"><strong><input type=\"text\"  name=\"txttime_##kau###\" id=\"txttime_##kau###\" /></strong> <br/><span> (eg: hh:mm:ss AM or PM) </span></div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"form-group\">";
            Strhtml += "<label class=\"control-label col-md-3\">Days</label>";
            Strhtml += "<div class=\"col-md-9 icheck minimal\">";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\"  id=\"chk_mon_##kau###\"  onchange=\"javascript:checkparent(this);\"  name=\"chk_mon_##kau###\">";
            Strhtml += "<label>Mon </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_tues_##kau###\"  onchange=\"javascript:checkparent(this);\" name=\"chk_tues_##kau###\">";
            Strhtml += "<label>Tues </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_wed_##kau###\" onchange=\"javascript:checkparent(this);\"  name=\"chk_wed_##kau###\">";
            Strhtml += "<label>Wed </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_thur_##kau###\" onchange=\"javascript:checkparent(this);\"  name=\"chk_thur_##kau###\">";
            Strhtml += "<label>Thu </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += " <div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_fri_##kau###\" onchange=\"javascript:checkparent(this);\"  name=\"chk_fri_##kau###\">";
            Strhtml += "<label>Fri </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_sat_##kau###\" onchange=\"javascript:checkparent(this);\"  name=\"chk_sat_##kau###\">";
            Strhtml += "<label>Sat </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_sun_##kau###\" onchange=\"javascript:checkparent(this);\"  name=\"chk_sun_##kau###\"><input type=\"hidden\" id=\"hdn_##kau###\" name=\"hdn_##kau###\" value=\"0\" />";
            Strhtml += "<label>Sun </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            //Strhtml += "<div class=\"form-group\">";
            //Strhtml += "<label class=\"control-label col-md-3\">";
            //Strhtml += "<button type=\"button\" class=\"btn btn-orang\">Cancel</button>";
            //Strhtml += "</label>";
            //Strhtml += "<div class=\"col-md-6\">";
            //Strhtml += "<div class=\"form-control-static\">";
            //Strhtml += "<button type=\"button\" class=\"btn btn-orang\">Save</button>";
            //Strhtml += "</div>";
            //Strhtml += "</div>";
            //Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";

            //  ltrhtml.Text = Strhtml;

            Literal1.Text = Strhtml.Replace("##kau###", "0").Replace("##kau_new###", "1");
            ltrgroupdata.Text = Strhtml1.Replace("##kau###", "0").Replace("##kau_new###", "1");
            divcolumndata.InnerHtml = Strhtml.ToString() + Strhtml1.ToString();

            //    (function ($) {
            //    $(function () {
            //        $('txttime_0').timepicker();
            //    });
            //})(jQuery);
            // Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jQuery.noConflict();$('#txttime').datetimepicker({ampm: true,timeOnly: true,showSecond: true,});", true);
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$('#txttime_0').timepicker({ timeFormat: 'h:mm:ss p' });", true);
        }
        private void BindHtmlDiv()
        {
            String Strhtml = "";
            String Strhtml1 = "";



            Strhtml1 += "<div class=\"form-group\">";
            Strhtml1 += "<label class=\"col-md-3 control-label\">&nbsp;</label>";
            Strhtml1 += "<div class=\"col-md-6\">";
            Strhtml1 += "<button type=\"button\" class=\"btn btn-orang\" id=\"btnmore_##kau###\" onclick=\"addmorecolumn(##kau_new###);\"><i class=\"fa fa-plus\"></i>Add Scheduler</button>";

            Strhtml1 += "</div>";
            Strhtml1 += "</div>";

            Strhtml += "  <div class=\"form-group\" id=\"div_##kau###\">";
            Strhtml += "<div class=\"col-md-3 control-label\">&nbsp;</div>";
            Strhtml += "<div class=\"col-md-6\">";

            Strhtml += "<div class=\"panel fa-border\">";

            Strhtml += "<div class=\"panel-heading\" >Scheduler <label id=\"scheduler_##kau###\">1</label> <span class=\"tools pull-right\">";
            Strhtml += "<button type=\"button\" id=\"btnremove_##kau###\"  onclick=\"removecolumn(##kau###);\" style=\"display:none;\" class=\"btn btn-xs btn-orang\">Remove</button>";
            Strhtml += "</span></div>";
            Strhtml += "<div class=\"panel-body\" id=\"verify-pro\">";
            Strhtml += "<div action=\"#\" class=\"form-horizontal\">";
            Strhtml += "<div class=\"form-group\">";
            Strhtml += "<label class=\"control-label col-md-3\">Time</label>";
            Strhtml += "<div class=\"col-md-6\">";
            Strhtml += "<div class=\"form-control-static\"><strong><input type=\"text\"  name=\"txttime_##kau###\" id=\"txttime_##kau###\" /></strong> <br/><span> (eg: hh:mm:ss AM or PM) </span> </div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"form-group\">";
            Strhtml += "<label class=\"control-label col-md-3\">Days</label>";
            Strhtml += "<div class=\"col-md-9 icheck minimal\">";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\"  id=\"chk_mon_##kau###\"  onchange=\"javascript:checkparent(this);\"  name=\"chk_mon_##kau###\">";
            Strhtml += "<label>Mon </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_tues_##kau###\"  onchange=\"javascript:checkparent(this);\" name=\"chk_tues_##kau###\">";
            Strhtml += "<label>Tues </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_wed_##kau###\" onchange=\"javascript:checkparent(this);\"  name=\"chk_wed_##kau###\">";
            Strhtml += "<label>Wed </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_thur_##kau###\" onchange=\"javascript:checkparent(this);\"  name=\"chk_thur_##kau###\">";
            Strhtml += "<label>Thu </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += " <div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_fri_##kau###\" onchange=\"javascript:checkparent(this);\"  name=\"chk_fri_##kau###\">";
            Strhtml += "<label>Fri </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_sat_##kau###\" onchange=\"javascript:checkparent(this);\"  name=\"chk_sat_##kau###\">";
            Strhtml += "<label>Sat </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "<div class=\"row\">";
            Strhtml += "<div class=\"radio single-row\">";
            Strhtml += "<input tabindex=\"3\" type=\"checkbox\" id=\"chk_sun_##kau###\" onchange=\"javascript:checkparent(this);\"  name=\"chk_sun_##kau###\"><input type=\"hidden\" id=\"hdn_##kau###\" name=\"hdn_##kau###\" value=\"0\" />";
            Strhtml += "<label>Sun </label>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            //Strhtml += "<div class=\"form-group\">";
            //Strhtml += "<label class=\"control-label col-md-3\">";
            //Strhtml += "<button type=\"button\" class=\"btn btn-orang\">Cancel</button>";
            //Strhtml += "</label>";
            //Strhtml += "<div class=\"col-md-6\">";
            //Strhtml += "<div class=\"form-control-static\">";
            //Strhtml += "<button type=\"button\" class=\"btn btn-orang\">Save</button>";
            //Strhtml += "</div>";
            //Strhtml += "</div>";
            //Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";
            Strhtml += "</div>";

            //  ltrhtml.Text = Strhtml;


            divcolumndata.InnerHtml = Strhtml.ToString() + Strhtml1.ToString();

            //    (function ($) {
            //    $(function () {
            //        $('txttime_0').timepicker();
            //    });
            //})(jQuery);
            // Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jQuery.noConflict();$('#txttime').datetimepicker({ampm: true,timeOnly: true,showSecond: true,});", true);
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$('#txttime_0').timepicker({ timeFormat: 'h:mm:ss p' });", true);
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            Int32 flag = 0;
            string[] formkeys = Request.Form.AllKeys;
            DataTable dt = new DataTable();
            DataColumn col1 = new DataColumn("Storeid", typeof(int));
            dt.Columns.Add(col1);
            DataColumn col2 = new DataColumn("Mon", typeof(string));
            dt.Columns.Add(col2);
            DataColumn col3 = new DataColumn("Tues", typeof(string));
            dt.Columns.Add(col3);
            DataColumn col4 = new DataColumn("Wed", typeof(string));
            dt.Columns.Add(col4);
            DataColumn col5 = new DataColumn("Thur", typeof(string));
            dt.Columns.Add(col5);
            DataColumn col6 = new DataColumn("Fri", typeof(string));
            dt.Columns.Add(col6);
            DataColumn col7 = new DataColumn("Sat", typeof(string));
            dt.Columns.Add(col7);
            DataColumn col8 = new DataColumn("Sun", typeof(string));
            dt.Columns.Add(col8);
            DataColumn col9 = new DataColumn("SchedularTime", typeof(string));
            dt.Columns.Add(col9);
            Int32 Storeid = 0;
            Int32 cl = 0;
            Int32 d1 = 0;
            Int32 d2 = 0;
            Int32 d3 = 0;
            Int32 d4 = 0;
            Int32 d5 = 0;
            Int32 d6 = 0;
            Int32 d7 = 0;
            Int32 CurrentRowID = 0;
            Int32 OldRowID = -1;
            DataRow dr = null;
            String time = "";

            foreach (String s in formkeys)
            {

                if (cl == 0)
                {
                    dr = dt.NewRow();

                }
                if (s.ToLower().IndexOf("##kau###") <= -1)
                {
                    Storeid = Convert.ToInt32(ddlstore.SelectedValue.ToString());
                    if (s.ToLower().IndexOf("txttime_") > -1)
                    {
                         dr["SchedularTime"] = Request.Form[s].ToString();
                         time = Request.Form[s].ToString();
                  

                       
                       

                    }
                    else if (s.ToLower().IndexOf("chk_mon_") > -1)
                    {
                        if (Request.Form[s].ToString() == "on")
                        {
                            dr["Mon"] = "1";
                            d1 = 1;

                        }
                        else
                        {
                            dr["Mon"] = "0";
                            d1 = 0;
                        }


                    }
                    else if (s.ToLower().IndexOf("chk_tues") > -1)
                    {
                        if (Request.Form[s].ToString() == "on")
                        {
                            dr["Tues"] = "1";
                            d2 = 1;

                        }
                        else
                        {
                            dr["Tues"] = "0";
                        }


                    }
                    else if (s.ToLower().IndexOf("chk_wed_") > -1)
                    {
                        if (Request.Form[s].ToString() == "on")
                        {
                            dr["Wed"] = "1";
                            d3 = 1;

                        }
                        else
                        {
                            dr["Wed"] = "0";
                        }


                    }
                    else if (s.ToLower().IndexOf("chk_thur_") > -1)
                    {
                        if (Request.Form[s].ToString() == "on")
                        {
                            dr["Thur"] = "1";
                            d4 = 1;

                        }
                        else
                        {
                            dr["Thur"] = "0";
                        }


                    }
                    else if (s.ToLower().IndexOf("chk_fri_") > -1)
                    {
                        if (Request.Form[s].ToString() == "on")
                        {
                            dr["Fri"] = "1";
                            d5 = 1;

                        }
                        else
                        {
                            dr["Fri"] = "0";
                        }


                    }
                    else if (s.ToLower().IndexOf("chk_sat_") > -1)
                    {
                        if (Request.Form[s].ToString() == "on")
                        {
                            dr["Sat"] = "1";
                            d6 = 1;

                        }
                        else
                        {
                            dr["Sat"] = "0";
                        }


                    }
                    else if (s.ToLower().IndexOf("chk_sun_") > -1)
                    {
                        if (Request.Form[s].ToString() == "on")
                        {
                            dr["Sun"] = "1";
                            d7 = 1;

                        }
                        else
                        {
                            dr["Sun"] = "0";
                        }


                    }
                    else if (s.ToLower().IndexOf("hdn_") > -1)
                    {
                        CurrentRowID = Convert.ToInt32(s.ToString().ToLower().Replace("hdn_", ""));

                        cl = 1;

                    }



                    if (cl == 1 && CurrentRowID > OldRowID)
                    {

                        OldRowID = CurrentRowID;

                        if (flag==0)
                        {
                            CommonComponent.ExecuteCommonData("update tb_FeedSchedular SET  Active=0,UpdatedBy=" + Session["AdminID"].ToString() + ",UpdatedOn=getdate() WHERE Storeid=" + Storeid + " ");
                            flag = 1;
                        }
                        CommonComponent.ExecuteCommonData("Exec [usp_FeedSchedular] 1," + Storeid + "," + d1 + "," + d2 + "," + d3 + "," + d4 + "," + d5 + "," + d6 + "," + d7 + ",'" + time.ToString() + "'," + Session["AdminID"].ToString() + ",1,0");
                        cl = 0;
                        d1 = 0;
                        d2 = 0;
                        d3 = 0;
                        d4 = 0;
                        d5 = 0;
                        d6 = 0;
                        d7 = 0;
                        time = "";
                        //dt.Rows.Add(dr);
                    }

                    //  dt.AcceptChanges();
                }

            }
            ShowExistSchedular();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message", "alert('Schedular Save Successfully!!!');", true);


        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/REPLENISHMENTMANAGEMENT/Dashboard.aspx");
        }

        protected void ddlstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowExistSchedular();
        }
    }
}