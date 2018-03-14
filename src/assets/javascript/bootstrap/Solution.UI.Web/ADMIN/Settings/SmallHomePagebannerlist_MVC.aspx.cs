using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using System.IO;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class SmallHomePagebannerlist_MVC : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindStore();
                // GetGrid();
                GetGroupGrid();
                if (Request.QueryString["autobanner"] != null && Request.QueryString["autobanner"].ToString() == "1")
                {
                    AppLogic.ApplicationStart();
                    getfinalhtml();
                }
            }
        }
        private void BindStore()
        {
            DataSet dsStore = StoreComponent.GetStoreList();
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = dsStore;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
        }


        private void getfinalhtml()
        {
            string Live_Contant_Server = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 ConfigValue from tb_AppConfig where ConfigName='Live_Contant_Server' and StoreID=1 and isnull(Deleted,0)=0"));
            if (String.IsNullOrEmpty(Live_Contant_Server))
            {
                Live_Contant_Server = AppLogic.AppConfigs("Live_Contant_Server").ToString();
            }
            string ImagePathHomePageBanner = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 ConfigValue from tb_AppConfig where ConfigName='ImagePathHomePageBannerMVC' and StoreID=1 and isnull(Deleted,0)=0"));
            if (String.IsNullOrEmpty(ImagePathHomePageBanner))
            {
                ImagePathHomePageBanner = AppLogic.AppConfigs("ImagePathHomePageBanner_MVC").ToString();
            }

            try
            {
                string strbanner = "";
                DataSet dsGroupCount = new DataSet();
                //BannerComponent objbanner = new BannerComponent();
                //objbanner.Mode = 1;
                dsGroupCount = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBannerSmall_MVC 1");// objbanner.GetHomePageRotatingBanner();
                strbanner = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT  top 1 Description  FROM  tb_Topic where Storeid=1 and TopicName='WHERETOSTART'"));

                //strbanner += "###BESTSELLERS###";

                if (dsGroupCount != null && dsGroupCount.Tables.Count > 0 && dsGroupCount.Tables[0].Rows.Count > 0)
                {
                    DataSet dsBanner = new DataSet();
                    // objbanner.Mode = 2;
                    dsBanner = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBannerSmall_MVC 2"); //objbanner.GetHomePageRotatingBanner();
                    if (dsBanner != null && dsBanner.Tables.Count > 0 && dsBanner.Tables[0].Rows.Count > 0)
                    {
                        Random rd = new Random();
                        // ltBanner.Text += "<div>";
                        for (int temp = 0; temp < dsGroupCount.Tables[0].Rows.Count; temp++)
                        {
                            if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 1)
                            {
                                DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + "");
                                if (dr.Length > 0)
                                {
                                    if (Convert.ToString(dr[0]["BannerText"]).Trim() == "")
                                    {
                                        //strbanner += "<section class=\"subbanner-sections wow fadeIn animated\" data-wow-duration=\"3s\" style=\"visibility: visible;-webkit-animation-duration: 3s; -moz-animation-duration: 3s; animation-duration: 3s;\">";
                                        strbanner += "<section data-wow-duration=\"3s\" class=\"subbanner-sections wow fadeIn animated\" style=\"visibility: visible;-webkit-animation-duration: 3s; -moz-animation-duration: 3s; animation-duration: 3s;\">";
                                        strbanner += " <div class=\"container\"><div class=\"row\"> <div class=\"col-sm-12 text-center width-full\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'  class=\"banner-images\"  /></a>";
                                        else
                                            strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                        strbanner += "</div>";
                                        strbanner += "</div>";
                                        strbanner += "</section>";
                                    }
                                    else
                                    {
                                        strbanner += "<section data-wow-duration=\"3s\" class=\"banner-sections wow fadeIn animated\" style=\"visibility: visible;-webkit-animation-duration: 3s; -moz-animation-duration: 3s; animation-duration: 3s;\">";
                                        strbanner += "<div class=\"banner-sections-img\">";

                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'  class=\"banner-images\"  /></a>";
                                        else
                                            strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                        strbanner += "<div class=\"container\">";

                                        if (Convert.ToString(dr[0]["BannerTextPosition"]) != "")
                                        {
                                            strbanner += dr[0]["BannerText"].ToString().Replace("banner-box-section banner-ct-right", "banner-box-section banner-ct-" + dr[0]["BannerTextPosition"].ToString().ToLower());
                                        }
                                        strbanner += "</div>";
                                        strbanner += "</section>";
                                    }
                                }
                            }
                            else if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 2)
                            {
                                int maincount = 0;
                                int subcount = 0;
                                DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=1");
                                maincount = dr.Length;

                                DataRow[] drsubcount = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                subcount = drsubcount.Length;

                                if (maincount > 0 || subcount > 0)
                                {
                                    strbanner += "<section data-wow-duration=\"3s\" class=\"subbanner-sections wow fadeIn animated\" style=\"visibility: visible;-webkit-animation-duration: 3s; -moz-animation-duration: 3s; animation-duration: 3s;\">";
                                    strbanner += "<div class=\"container\">";
                                    //strbanner += "<div class=\"margin-top-20 col-layout layout-2-col wow slideInUp animated\">";
                                    strbanner += "<div class=\"row\">";
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"col-sm-6 col-xs-12\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        {
                                            strbanner += "<div class=\"index-small-banner\">";
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                            strbanner += "</div>";

                                            strbanner += "<div class=\"index-small-banner-name\">";
                                            strbanner += "<a title=\"\" href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\">";
                                            strbanner += "<h6>" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "</h6>";
                                            strbanner += "<span>" + Server.HtmlEncode(Convert.ToString(dr[0]["BannerText"]).Trim()) + "</span>";
                                            strbanner += "</a>";
                                            strbanner += "</div>";
                                        }
                                        else
                                        {
                                            strbanner += "<div class=\"index-small-banner\">";
                                            strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                            strbanner += "</div>";

                                            strbanner += "<div class=\"index-small-banner-name\">";
                                            strbanner += "<a title=\"\" href=\"javascript:void(0);\">";
                                            strbanner += "<h6>" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "</h6>";
                                            strbanner += "<span>" + Server.HtmlEncode(Convert.ToString(dr[0]["BannerText"]).Trim()) + "</span>";
                                            strbanner += "</a>";
                                            strbanner += "</div>";
                                        }
                                        strbanner += "</div>";
                                    }

                                    dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"col-sm-6 col-xs-12\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        {
                                            strbanner += "<div class=\"index-small-banner\">";
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                            strbanner += "</div>";

                                            strbanner += "<div class=\"index-small-banner-name\">";
                                            strbanner += "<a title=\"\" href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\">";
                                            strbanner += "<h6>" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "</h6>";
                                            strbanner += "<span>" + Server.HtmlEncode(Convert.ToString(dr[0]["BannerText"]).Trim()) + "</span>";
                                            strbanner += "</a>";
                                            strbanner += "</div>";
                                        }
                                        else
                                        {
                                            strbanner += "<div class=\"index-small-banner\">";
                                            strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                            strbanner += "</div>";

                                            strbanner += "<div class=\"index-small-banner-name\">";
                                            strbanner += "<a title=\"\" href=\"javascript:void(0);\">";
                                            strbanner += "<h6>" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "</h6>";
                                            strbanner += "<span>" + Server.HtmlEncode(Convert.ToString(dr[0]["BannerText"]).Trim()) + "</span>";
                                            strbanner += "</a>";
                                            strbanner += "</div>";
                                        }
                                        strbanner += "</div>";
                                    }

                                    strbanner += "</div>";
                                    strbanner += "</div>";
                                    strbanner += "</section>";
                                }
                            }
                            else if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 3)
                            {
                                int maincount = 0;
                                int subcount = 0;

                                DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=1");
                                maincount = dr.Length;

                                DataRow[] drsubcount = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                subcount = drsubcount.Length;

                                if (maincount > 0 || subcount > 0)
                                {
                                    //strbanner += "<div class=\"margin-top-20 col-layout layout-3-col wow slideInUp animated\">";
                                    //strbanner += "<div class=\"row\">";
                                    strbanner += "<section data-wow-duration=\"3s\" class=\"subbanner-sections wow fadeIn animated\" style=\"visibility: visible;-webkit-animation-duration: 3s; -moz-animation-duration: 3s; animation-duration: 3s;\">";
                                    strbanner += "<div class=\"container\">";
                                    strbanner += " <div class=\"row\">";
                                    if (dr.Length > 0)
                                    {

                                        //inforloop
                                        strbanner += "<div class=\"col-sm-4 col-xs-12\">";
                                        strbanner += " <div class=\"index-small-banner\">";

                                        //  strbanner += "<div class=\"col-sm-4 col-xs-4 xs-mrg-b-20\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'class=\"mobibanner\"   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";

                                        strbanner += "</div>";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        {
                                            strbanner += "<div class=\"index-small-banner-name\"><a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + " <span>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</span></a></div>";
                                        }
                                        else
                                        {
                                            strbanner += "<div class=\"index-small-banner-name\"><a href=\"javascript:void(0);\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + " <span>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</span></a></div>";
                                        }

                                        strbanner += "</div>";
                                    }

                                    dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"col-sm-4 col-xs-12\">";
                                        strbanner += " <div class=\"index-small-banner\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "' class=\"mobibanner\"  /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        {
                                            strbanner += "<div class=\"index-small-banner-name\"><a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + " <span>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</span></a></div>";
                                        }
                                        else
                                        {
                                            strbanner += "<div class=\"index-small-banner-name\"><a href=\"javascript:void(0);\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + " <span>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</span></a></div>";
                                        }
                                        strbanner += "</div>";

                                        if (dr.Length > 1)
                                        {
                                            strbanner += "<div class=\"col-sm-4 col-xs-12\">";
                                            strbanner += " <div class=\"index-small-banner\">";
                                            if (Convert.ToString(dr[1]["BannerURL"]) != "" && Convert.ToString(dr[1]["StoreID"]) != "")
                                                strbanner += "<a href=\"" + Convert.ToString(dr[1]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "'  class=\"mobibanner\" /></a>";
                                            else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"])) + "' /></a>";
                                            strbanner += "</div>";
                                            if (Convert.ToString(dr[1]["BannerURL"]) != "" && Convert.ToString(dr[1]["StoreID"]) != "")
                                            {
                                                strbanner += "<div class=\"index-small-banner-name\"><a href=\"" + Convert.ToString(dr[1]["BannerURL"]) + "\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\">" + Convert.ToString(dr[1]["Title"]).Trim() + " <span>" + Convert.ToString(dr[1]["BannerText"]).Trim() + "</span></a></div>";
                                            }
                                            else
                                            {
                                                strbanner += "<div class=\"index-small-banner-name\"><a href=\"javascript:void(0);\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\">" + Convert.ToString(dr[1]["Title"]).Trim() + " <span>" + Convert.ToString(dr[1]["BannerText"]).Trim() + "</span></a></div>";
                                            }
                                            strbanner += "</div>";
                                        }
                                    }

                                    strbanner += "</div>";
                                    strbanner += "</div>";
                                    strbanner += "</section>";
                                }
                            }
                            else if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 4)
                            {
                                int maincount = 0;
                                int subcount = 0;
                                //if (temp == 0)
                                //{
                                //  //  ltBanner.Text += "<div class=\"item active\">";
                                //}
                                //else
                                //{
                                //  //  ltBanner.Text += "<div class=\"item\">";
                                //}
                                DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=1");
                                maincount = dr.Length;

                                DataRow[] drsubcount = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                subcount = drsubcount.Length;

                                //hiiiii

                                if (maincount > 0 || subcount > 0)
                                {
                                    //strbanner += "<div class=\"margin-top-20 col-layout layout-3-col wow slideInUp animated\">";
                                    //strbanner += "<div class=\"row\">";
                                    strbanner += "<section data-wow-duration=\"3s\" class=\"shop-by-room-section wow fadeIn\" style=\"visibility: visible;-webkit-animation-duration: 3s; -moz-animation-duration: 3s; animation-duration: 3s;\">";
                                    strbanner += "<div class=\"container\">";
                                    strbanner += " <div class=\"row\">";
                                    if (dr.Length > 0)
                                    {
                                        //inforloop
                                        strbanner += "<div class=\"col-md-3 col-sm-6 col-xs-6\">";
                                        strbanner += "<div class=\"shop-by-room-main-box\">";
                                        strbanner += " <div class=\"shop-by-room-list-box\">";
                                        strbanner += " <div class=\"shop-by-room-list-img\">";

                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'class=\"mobibanner\"   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";

                                        strbanner += "</div>";
                                        strbanner += "<div class=\"shop-by-room-list-desc\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        {
                                            strbanner += "<div class=\"shop-by-room-list-name\"><a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + "</a>";
                                            if (!string.IsNullOrEmpty(Convert.ToString(dr[0]["BannerText"]).Trim()))
                                            {
                                                strbanner += "<p>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</p>";
                                            }
                                            strbanner += "</div>";
                                        }
                                        else
                                        {
                                            strbanner += "<div class=\"shop-by-room-list-name\"><a href=\"javascript:void(0);\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + "</a>";
                                            if (!string.IsNullOrEmpty(Convert.ToString(dr[0]["BannerText"]).Trim()))
                                            {
                                                strbanner += "<p>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</p>";
                                            }
                                            strbanner += "</div>";
                                        }
                                        strbanner += "</div>";
                                        strbanner += "</div>";
                                        strbanner += "</div>";
                                        strbanner += "</div>";
                                    }

                                    dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"col-md-3 col-sm-6 col-xs-6\">";
                                        strbanner += "<div class=\"shop-by-room-main-box\">";
                                        strbanner += " <div class=\"shop-by-room-list-box\">";
                                        strbanner += " <div class=\"shop-by-room-list-img\">";

                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "' class=\"mobibanner\"  /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                        strbanner += "<div class=\"shop-by-room-list-desc\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        {
                                            strbanner += "<div class=\"shop-by-room-list-name\"><a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + "</a> <p>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</p></div>";
                                        }
                                        else
                                        {
                                            strbanner += "<div class=\"shop-by-room-list-name\"><a href=\"javascript:void(0);\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + "</a> <p>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</p></div>";
                                        }
                                        strbanner += "</div>";
                                        strbanner += "</div>";
                                        strbanner += "</div>";
                                        strbanner += "</div>";

                                        if (dr.Length > 1)
                                        {
                                            strbanner += "<div class=\"col-md-3 col-sm-6 col-xs-6\">";
                                            strbanner += "<div class=\"shop-by-room-main-box\">";
                                            strbanner += " <div class=\"shop-by-room-list-box\">";
                                            strbanner += " <div class=\"shop-by-room-list-img\">";
                                            if (Convert.ToString(dr[1]["BannerURL"]) != "" && Convert.ToString(dr[1]["StoreID"]) != "")
                                                strbanner += "<a href=\"" + Convert.ToString(dr[1]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "'  class=\"mobibanner\" /></a>";
                                            else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"])) + "' /></a>";
                                            strbanner += "</div>";
                                            strbanner += "<div class=\"shop-by-room-list-desc\">";
                                            if (Convert.ToString(dr[1]["BannerURL"]) != "" && Convert.ToString(dr[1]["StoreID"]) != "")
                                            {
                                                strbanner += "<div class=\"shop-by-room-list-name\"><a href=\"" + Convert.ToString(dr[1]["BannerURL"]) + "\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\">" + Convert.ToString(dr[1]["Title"]).Trim() + "</a> <p>" + Convert.ToString(dr[1]["BannerText"]).Trim() + "</p></div>";
                                            }
                                            else
                                            {
                                                strbanner += "<div class=\"shop-by-room-list-name\"><a href=\"javascript:void(0);\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\">" + Convert.ToString(dr[1]["Title"]).Trim() + "</a> <p>" + Convert.ToString(dr[1]["BannerText"]).Trim() + "</p></div>";
                                            }
                                            strbanner += "</div>";
                                            strbanner += "</div>";
                                            strbanner += "</div>";
                                            strbanner += "</div>";
                                        }
                                        if (dr.Length > 2)
                                        {
                                            strbanner += "<div class=\"col-md-3 col-sm-6 col-xs-6\">";
                                            strbanner += "<div class=\"shop-by-room-main-box\">";
                                            strbanner += " <div class=\"shop-by-room-list-box\">";
                                            strbanner += " <div class=\"shop-by-room-list-img\">";
                                            if (Convert.ToString(dr[2]["BannerURL"]) != "" && Convert.ToString(dr[2]["StoreID"]) != "")
                                                strbanner += "<a href=\"" + Convert.ToString(dr[2]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[2]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[2]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[2]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[2]["Title"]).Trim()) + "'  class=\"mobibanner\" /></a>";
                                            else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[2]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[2]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[2]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[2]["Title"])) + "' /></a>";
                                            strbanner += "</div>";
                                            strbanner += "<div class=\"shop-by-room-list-desc\">";
                                            if (Convert.ToString(dr[2]["BannerURL"]) != "" && Convert.ToString(dr[2]["StoreID"]) != "")
                                            {
                                                strbanner += "<div class=\"shop-by-room-list-name\"><a href=\"" + Convert.ToString(dr[2]["BannerURL"]) + "\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[2]["Title"]).Trim()) + "\">" + Convert.ToString(dr[2]["Title"]).Trim() + "</a><p>" + Convert.ToString(dr[2]["BannerText"]).Trim() + "</p></div>";
                                            }
                                            else
                                            {
                                                strbanner += "<div class=\"shop-by-room-list-name\"><a href=\"javascript:void(0);\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[2]["Title"]).Trim()) + "\">" + Convert.ToString(dr[2]["Title"]).Trim() + "</a><p>" + Convert.ToString(dr[2]["BannerText"]).Trim() + "</p></div>";
                                            }
                                            strbanner += "</div>";
                                            strbanner += "</div>";
                                            strbanner += "</div>";
                                            strbanner += "</div>";
                                        }
                                    }

                                    strbanner += "</div>";
                                    strbanner += "</div>";
                                    strbanner += "</section>";
                                }
                            }
                        }
                        //  ltBanner.Text += "</div>";
                    }
                }
                CommonComponent.ExecuteCommonData("update tb_topic set Description='" + strbanner.ToString().Replace("'", "''") + "' where TopicName='HomePageBanner_MVC' and StoreID=1");
            }
            catch { }
        }

        #region OLD HTML Generate
        /*
        private void getfinalhtml()
        {

            string Live_Contant_Server = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 ConfigValue from tb_AppConfig where ConfigName='Live_Contant_Server' and StoreID=1 and isnull(Deleted,0)=0"));
            if (String.IsNullOrEmpty(Live_Contant_Server))
            {
                Live_Contant_Server = AppLogic.AppConfigs("Live_Contant_Server").ToString();
            }


            string ImagePathHomePageBanner = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 ConfigValue from tb_AppConfig where ConfigName='ImagePathHomePageBannerMVC' and StoreID=1 and isnull(Deleted,0)=0"));
            if (String.IsNullOrEmpty(ImagePathHomePageBanner))
            {
                ImagePathHomePageBanner = AppLogic.AppConfigs("ImagePathHomePageBanner_MVC").ToString();
            }

            try
            {
                string strbanner = "";
                DataSet dsGroupCount = new DataSet();
                //BannerComponent objbanner = new BannerComponent();
                //objbanner.Mode = 1;
                dsGroupCount = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBannerSmall_MVC 1");// objbanner.GetHomePageRotatingBanner();
                strbanner = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT  top 1 Description  FROM  tb_Topic where Storeid=1 and TopicName='WHERETOSTART'"));
                if (dsGroupCount != null && dsGroupCount.Tables.Count > 0 && dsGroupCount.Tables[0].Rows.Count > 0)
                {

                    DataSet dsBanner = new DataSet();
                    // objbanner.Mode = 2;
                    dsBanner = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBannerSmall_MVC 2"); //objbanner.GetHomePageRotatingBanner();
                    if (dsBanner != null && dsBanner.Tables.Count > 0 && dsBanner.Tables[0].Rows.Count > 0)
                    {
                        Random rd = new Random();
                        // ltBanner.Text += "<div>";

                        for (int temp = 0; temp < dsGroupCount.Tables[0].Rows.Count; temp++)
                        {
                            if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 1)
                            {
                                DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + "");


                                if (dr.Length > 0)
                                {
                                    strbanner += "<section data-wow-duration=\"3s\" class=\"banner-sections wow fadeIn animated\" style=\"visibility: visible;-webkit-animation-duration: 3s; -moz-animation-duration: 3s; animation-duration: 3s;\">";
                                    strbanner += "<div class=\"banner-sections-img\">";


                                    if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'  class=\"banner-images\"  /></a>";
                                    else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                    strbanner += "</div>";
                                    strbanner += "<div class=\"container\">";

                                    if (Convert.ToString(dr[0]["BannerTextPosition"]) != "")
                                    {
                                        strbanner += dr[0]["BannerText"].ToString().Replace("banner-box-section banner-ct-right", "banner-box-section banner-ct-" + dr[0]["BannerTextPosition"].ToString().ToLower());

                                    }

                                    strbanner += "</div>";
                                    strbanner += "</section>";
                                }


                            }
                            else if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 2)
                            {


                                int maincount = 0;
                                int subcount = 0;
                                //if (temp == 0)
                                //{
                                //  //  ltBanner.Text += "<div class=\"item active\">";
                                //}
                                //else
                                //{
                                //  //  ltBanner.Text += "<div class=\"item\">";

                                //}
                                DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=1");
                                maincount = dr.Length;

                                DataRow[] drsubcount = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                subcount = drsubcount.Length;




                                if (maincount > 0 || subcount > 0)
                                {
                                    strbanner += "<div class=\"margin-top-20 col-layout layout-2-col wow slideInUp animated\">";
                                    strbanner += "<div class=\"row\">";
                                    if (dr.Length > 0)
                                    {

                                      
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                    }

                                    dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"col-sm-6 col-xs-6\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                    }

                                    strbanner += "</div>";
                                    strbanner += "</div>";
                                }
                            }
                            else if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 3)
                            {


                                int maincount = 0;
                                int subcount = 0;
                                //if (temp == 0)
                                //{
                                //  //  ltBanner.Text += "<div class=\"item active\">";
                                //}
                                //else
                                //{
                                //  //  ltBanner.Text += "<div class=\"item\">";

                                //}
                                DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=1");
                                maincount = dr.Length;

                                DataRow[] drsubcount = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                subcount = drsubcount.Length;

                                //hiiiii


                                if (maincount > 0 || subcount > 0)
                                {
                                    //strbanner += "<div class=\"margin-top-20 col-layout layout-3-col wow slideInUp animated\">";
                                    //strbanner += "<div class=\"row\">";
                                    strbanner += "<section data-wow-duration=\"3s\" class=\"m-t-30 subbanner-sections m-b-30 wow fadeIn animated\" style=\"visibility: visible;-webkit-animation-duration: 3s; -moz-animation-duration: 3s; animation-duration: 3s;\">";
                                    strbanner += "<div class=\"container\">";
                                    strbanner += " <div class=\"row\">";
                                    if (dr.Length > 0)
                                    {
                                      
                                        //inforloop
                                        strbanner += "<div class=\"col-sm-4 col-xs-12\">";
                                        strbanner += " <div class=\"index-small-banner\">";
                                    

                                      //  strbanner += "<div class=\"col-sm-4 col-xs-4 xs-mrg-b-20\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'class=\"mobibanner\"   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";

                                        strbanner += "</div>";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        {
                                            strbanner += "<div class=\"index-small-banner-name\"><a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + " <span>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</span></a></div>";
                                        }
                                        else
                                        {
                                            strbanner += "<div class=\"index-small-banner-name\"><a href=\"javascript:void(0);\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + " <span>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</span></a></div>";
                                        }
                                        
                                        strbanner += "</div>";
                                    }

                                    dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"col-sm-4 col-xs-12\">";
                                        strbanner += " <div class=\"index-small-banner\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "' class=\"mobibanner\"  /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        {
                                            strbanner += "<div class=\"index-small-banner-name\"><a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + " <span>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</span></a></div>";
                                        }
                                        else
                                        {
                                            strbanner += "<div class=\"index-small-banner-name\"><a href=\"javascript:void(0);\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + " <span>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</span></a></div>";
                                        }
                                        strbanner += "</div>";


                                        if (dr.Length > 1)
                                        {


                                            strbanner += "<div class=\"col-sm-4 col-xs-12\">";
                                            strbanner += " <div class=\"index-small-banner\">";
                                            if (Convert.ToString(dr[1]["BannerURL"]) != "" && Convert.ToString(dr[1]["StoreID"]) != "")
                                                strbanner += "<a href=\"" + Convert.ToString(dr[1]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "'  class=\"mobibanner\" /></a>";
                                            else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"])) + "' /></a>";
                                            strbanner += "</div>";
                                            if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            {
                                                strbanner += "<div class=\"index-small-banner-name\"><a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + " <span>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</span></a></div>";
                                            }
                                            else
                                            {
                                                strbanner += "<div class=\"index-small-banner-name\"><a href=\"javascript:void(0);\"  title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\">" + Convert.ToString(dr[0]["Title"]).Trim() + " <span>" + Convert.ToString(dr[0]["BannerText"]).Trim() + "</span></a></div>";
                                            }
                                            strbanner += "</div>";
                                        }
                                    }

                                    strbanner += "</div>";
                                    strbanner += "</div>";
                                    strbanner += "</section>";
                                }


                            }
                        }




                        //  ltBanner.Text += "</div>";
                    }



                }


                CommonComponent.ExecuteCommonData("update tb_topic set Description='" + strbanner.ToString().Replace("'", "''") + "' where TopicName='HomePageBanner_MVC' and StoreID=1");
            }
            catch { }
        }

        */
        #endregion

        private void GetGroupGrid()
        {
            HomeBannerComponent objbanner = new HomeBannerComponent();
            DataSet dsbanner = new DataSet();
            string strGrid = "";

            DataSet dsBannergroups = new DataSet();
            // dsBannergroups = CommonComponent.GetCommonDataSet("select HomeRotatorId,StoreID,BannerTypeId,isnull(IsActive,0) as IsActive,isnull(BannerID,0) as BannerID,isnull(DisplayOrder,0) as DisplayOrder from tb_RotatorHomebannersmall order by isnull(IsActive,0) desc,isnull(DisplayOrder,0) ASC");
            if (ddlstatus.SelectedValue.ToString() == "")
            {
                dsBannergroups = CommonComponent.GetCommonDataSet("select distinct tb_RotatorHomebannersmall_MVC.HomeRotatorId,tb_RotatorHomebannersmall_MVC.StoreID,tb_RotatorHomebannersmall_MVC.BannerTypeId,isnull(tb_RotatorHomebannersmall_MVC.IsActive,0) as IsActive,isnull(tb_RotatorHomebannersmall_MVC.BannerID,0) as BannerID,isnull(tb_RotatorHomebannersmall_MVC.DisplayOrder,0) as DisplayOrder, case when cast(isnull(enddate,getdate()) as date) >=cast(GETDATE() as date) and cast(isnull(startdate,getdate()) as date)  <= cast(GETDATE() as date) then 1 else 0 end as datecase,convert(char(10), isnull(startdate,getdate()),101) as startdate,convert(char(10), isnull(enddate,getdate()),101) as enddate from tb_RotatorHomebannersmall_MVC inner join tb_RotatorHomeBannerDetailsmall_MVC on tb_RotatorHomebannersmall_MVC.HomeRotatorId=tb_RotatorHomeBannerDetailsmall_MVC.HomeRotatorId order by datecase desc, isnull(tb_RotatorHomebannersmall_MVC.IsActive,0) desc,isnull(tb_RotatorHomebannersmall_MVC.DisplayOrder,0) ASC");
            }
            else if (ddlstatus.SelectedValue.ToString() == "1")
            {
                dsBannergroups = CommonComponent.GetCommonDataSet("select distinct tb_RotatorHomebannersmall_MVC.HomeRotatorId,tb_RotatorHomebannersmall_MVC.StoreID,tb_RotatorHomebannersmall_MVC.BannerTypeId,isnull(tb_RotatorHomebannersmall_MVC.IsActive,0) as IsActive,isnull(tb_RotatorHomebannersmall_MVC.BannerID,0) as BannerID,isnull(tb_RotatorHomebannersmall_MVC.DisplayOrder,0) as DisplayOrder, case when cast(isnull(enddate,getdate()) as date) >=cast(GETDATE() as date) and cast(isnull(startdate,getdate()) as date)  <= cast(GETDATE() as date) then 1 else 0 end as datecase,convert(char(10), isnull(startdate,getdate()),101) as startdate,convert(char(10), isnull(enddate,getdate()),101) as enddate from tb_RotatorHomebannersmall_MVC inner join tb_RotatorHomeBannerDetailsmall_MVC on tb_RotatorHomebannersmall_MVC.HomeRotatorId=tb_RotatorHomeBannerDetailsmall_MVC.HomeRotatorId where cast(isnull(enddate,getdate()) as date) >=cast(GETDATE() as date) and cast(isnull(startdate,getdate()) as date)  <= cast(GETDATE() as date) and isnull(IsActive,0)=1 order by datecase desc, isnull(tb_RotatorHomebannersmall_MVC.IsActive,0) desc,isnull(tb_RotatorHomebannersmall_MVC.DisplayOrder,0) ASC");
            }
            else if (ddlstatus.SelectedValue.ToString() == "0")
            {
                dsBannergroups = CommonComponent.GetCommonDataSet("select distinct tb_RotatorHomebannersmall_MVC.HomeRotatorId,tb_RotatorHomebannersmall_MVC.StoreID,tb_RotatorHomebannersmall_MVC.BannerTypeId,isnull(tb_RotatorHomebannersmall_MVC.IsActive,0) as IsActive,isnull(tb_RotatorHomebannersmall_MVC.BannerID,0) as BannerID,isnull(tb_RotatorHomebannersmall_MVC.DisplayOrder,0) as DisplayOrder, case when cast(isnull(enddate,getdate()) as date) >=cast(GETDATE() as date) and cast(isnull(startdate,getdate()) as date)  <= cast(GETDATE() as date) then 1 else 0 end as datecase,convert(char(10), isnull(startdate,getdate()),101) as startdate,convert(char(10), isnull(enddate,getdate()),101) as enddate from tb_RotatorHomebannersmall_MVC inner join tb_RotatorHomeBannerDetailsmall_MVC on tb_RotatorHomebannersmall_MVC.HomeRotatorId=tb_RotatorHomeBannerDetailsmall_MVC.HomeRotatorId where (cast(isnull(enddate,getdate()) as date) < cast(GETDATE() as date) or isnull(IsActive,0)=0)  order by datecase desc, isnull(tb_RotatorHomebannersmall_MVC.IsActive,0) desc,isnull(tb_RotatorHomebannersmall_MVC.DisplayOrder,0) ASC");
            }
            else if (ddlstatus.SelectedValue.ToString() == "2")
            {
                dsBannergroups = CommonComponent.GetCommonDataSet("select distinct tb_RotatorHomebannersmall_MVC.HomeRotatorId,tb_RotatorHomebannersmall_MVC.StoreID,tb_RotatorHomebannersmall_MVC.BannerTypeId,isnull(tb_RotatorHomebannersmall_MVC.IsActive,0) as IsActive,isnull(tb_RotatorHomebannersmall_MVC.BannerID,0) as BannerID,isnull(tb_RotatorHomebannersmall_MVC.DisplayOrder,0) as DisplayOrder, case when cast(isnull(enddate,getdate()) as date) >=cast(GETDATE() as date) and cast(isnull(startdate,getdate()) as date)  <= cast(GETDATE() as date) then 1 else 0 end as datecase,convert(char(10), isnull(startdate,getdate()),101) as startdate,convert(char(10), isnull(enddate,getdate()),101) as enddate from tb_RotatorHomebannersmall_MVC inner join tb_RotatorHomeBannerDetailsmall_MVC on tb_RotatorHomebannersmall_MVC.HomeRotatorId=tb_RotatorHomeBannerDetailsmall_MVC.HomeRotatorId where (cast(isnull(enddate,getdate()) as date) > cast(GETDATE() as date) and cast(isnull(StartDate,getdate()) as date) > cast(GETDATE() as date))  order by datecase desc, isnull(tb_RotatorHomebannersmall_MVC.IsActive,0) desc,isnull(tb_RotatorHomebannersmall_MVC.DisplayOrder,0) ASC");
            }

            if (dsBannergroups != null && dsBannergroups.Tables.Count > 0 && dsBannergroups.Tables[0].Rows.Count > 0)
            {
                for (int kk = 0; kk < dsBannergroups.Tables[0].Rows.Count; kk++)
                {
                    //  dsbanner = objbanner.GetHomeRotatorBanner(Convert.ToInt32(ddlStore.SelectedValue.ToString()), Convert.ToInt32(dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString()));
                    dsbanner = CommonComponent.GetCommonDataSet("EXEC usp_HomeRotatorBannerSmall_MVC " + Convert.ToInt32(dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString()) + "," + Convert.ToInt32(ddlStore.SelectedValue.ToString()) + "");
                    ltactive.Text = "";
                    if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
                    {
                        string path = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='ImagePathHomePageBannerMVC' AND storeid=" + ddlStore.SelectedValue.ToString() + " AND isnull(Deleted,0)=0"));
                        strGrid += "<fieldset class=\"fldset\" style=\"width: 98%;\" id=\"fieldset1\">";
                        if (Convert.ToBoolean(dsbanner.Tables[0].Rows[0]["IsActive"].ToString()))
                        {
                            strGrid += "<legend><span class=\"icheckbox_flat-green checked\" ></span>&nbsp;Group" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "</legend>";
                            strGrid += "<div style=\"width: 100%;height: 21px;\" class=\"legendright\"><div class=\"divfiledset1\"  id=\"legend1\"><a href=\"javascript:void(0);\" onclick=\"makeInActive(" + ddlStore.SelectedValue.ToString() + "," + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/groupInactive.png\" title=\"InActive Group\" border=\"0\" /></a></div></div>";
                            //strGrid += "<legend id=\"legend1\" class=\"legendright\"><a href=\"javascript:void(0);\" onclick=\"ShowModelCredit(" + ddlStore.SelectedValue.ToString() + ",1);\">Preview</a></legend>";
                            //strGrid += "<div style=\"width: 100%; height: 21px;\" class=\"legendright\"><div class=\"divfiledset\" style=\"display:none;\" id=\"legend1\"><a href=\"javascript:void(0);\" onclick=\"ShowModelCredit(" + ddlStore.SelectedValue.ToString() + ",1);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/preview.png\" title=\"preview\" border=\"0\" /></a>&nbsp;</div></div>";

                        }
                        else
                        {
                            strGrid += "<legend><span class=\"icheckbox_flat-green\" style=\"background-position: -66px 0pt;\"></span>&nbsp;Group" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "</legend>";
                            //strGrid += "<legend  id=\"legend1\" class=\"legendright\"><a href=\"javascript:void(0);\" onclick=\"ShowModelCredit(" + ddlStore.SelectedValue.ToString() + ",1);\">Preview</a>&nbsp;<a href=\"javascript:void(0);\" onclick=\"makeActive(" + ddlStore.SelectedValue.ToString() + ",1);\">Active</a></legend>";
                            //strGrid += "<div style=\"width: 100%;height: 21px;\" class=\"legendright\"><div class=\"divfiledset\" style=\"display:none;\" id=\"legend1\"><a href=\"javascript:void(0);\" onclick=\"ShowModelCredit(" + ddlStore.SelectedValue.ToString() + ",1);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/preview.png\" title=\"preview\" border=\"0\" /></a>&nbsp;<a href=\"javascript:void(0);\" onclick=\"makeActive(" + ddlStore.SelectedValue.ToString() + ",1);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/banneractive.png\" title=\"Active\" border=\"0\" /></a></div></div>";
                            strGrid += "<div style=\"width: 100%;height: 21px;\" class=\"legendright\"><div class=\"divfiledset1\"  id=\"legend1\"><a href=\"javascript:void(0);\" onclick=\"makeActive(" + ddlStore.SelectedValue.ToString() + "," + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/groupactive.png\" title=\"Active Group\" border=\"0\" /></a></div></div>";
                        }


                        if (dsBannergroups.Tables[0].Rows[kk]["BannerTypeId"].ToString() == "1")
                        {


                            strGrid += "<div id='Group'+" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + " style=\"float:left;width:100%;\">";
                            strGrid += "<table cellspacing=\"0\" style=\"width: 100%; border-collapse: collapse;\">";
                            strGrid += "<tr style=\"color: White; font-weight: normal;\">";
                            //strGrid += "<th align=\"center\" style=\"width: 20%;height: 36px;display:none;\" scope=\"col\">Display Order<br /><a onclick=\"getlayoutid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"updateorder1\" style=\"display:none;\" href=\"javascript:void(0);\">Update Order</a></th>";
                            if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString()) > Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 90%;\" scope=\"col\">Banner Image (Start Date: " + dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString() + ")</th>";
                            }
                            else if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 90%;\" scope=\"col\">Banner Image (End Date: " + dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString() + ")</th>";
                            }
                            else
                            {
                                strGrid += "<th align=\"left\" style=\"width: 90%;\" scope=\"col\">Banner Image</th>";
                            }

                            //strGrid += "<th align=\"left\" style=\"width: 20%;\" scope=\"col\">Banner Title</th>";
                            //strGrid += "<th align=\"left\" style=\"width: 20%;\" scope=\"col\">Banner Url</th>";
                            //strGrid += "<th align=\"center\" style=\"width: 10%;\" scope=\"col\">Status</th>";
                            strGrid += "<th align=\"center\" style=\"width: 10%;\" scope=\"col\">Action</th>";
                            strGrid += "</tr>";

                            for (int i = 0; i < 1; i++)
                            {
                                if ((i + 1) % 2 == 0)
                                {
                                    strGrid += "<tr  class=\"altrow\">";
                                }
                                else
                                {
                                    strGrid += "<tr  class=\"oddrow\">";
                                }

                                //strGrid += "<td align=\"Center\" style=\"display:none;\"></td>";
                                if (File.Exists(Server.MapPath(path.ToString() + "/" + dsbanner.Tables[0].Rows[i]["ImageName"].ToString())))
                                {
                                    Random rd = new Random();
                                    strGrid += "<td align=\"left\"><img width=\"95%\" src=\"" + path.ToString() + dsbanner.Tables[0].Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" /></td>";
                                }
                                else
                                {
                                    strGrid += "<td align=\"left\"></td>";
                                }
                                //strGrid += "<td align=\"left\">" + dsbanner.Tables[0].Rows[i]["Title"].ToString() + "</td>";
                                //strGrid += "<td align=\"left\">" + dsbanner.Tables[0].Rows[i]["BannerURL"].ToString() + "</td>";
                                //if (Convert.ToBoolean(dsbanner.Tables[0].Rows[i]["Active"].ToString()))
                                //{
                                //    strGrid += "<td align=\"center\"><img src=\"/admin/images/active.gif\"></td>";
                                //}
                                //else
                                //{
                                //    strGrid += "<td align=\"center\"><img src=\"/admin/images/in-active.gif\"></td>";
                                //}
                                strGrid += "<td align=\"center\" ><a href=\"SmallHomepageRotatorbanneruploadMVC.aspx?id=" + dsBannergroups.Tables[0].Rows[kk]["BannerTypeId"].ToString() + "&Storeid=" + ddlStore.SelectedValue.ToString() + "&hid=" + dsbanner.Tables[0].Rows[i]["BannerID"].ToString() + "\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/edit-icon.png\"></a><a onclick=\"Deleterecord(" + dsbanner.Tables[0].Rows[i]["BannerID"].ToString() + ");\" style=\"display:none;\" href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/delete-icon.png\"></a></td>";

                                strGrid += "</tr>";

                                strGrid += "<tr>";
                                strGrid += "<td colspan=\"2\">";
                                strGrid += "<table width=\"100%\">";
                                strGrid += "<tr>";
                                strGrid += "<td width=\"20%;\">Display Order";

                                strGrid += "</td>";
                                strGrid += "<td>";
                                strGrid += " <input type=\"text\" id=\"text1_" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\" onkeypress=\"return keyRestrict(event,'0123456789');\" name=\"text1_" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\" class=\"order-textfield\" style=\"width:40px;text-align:center;\" value=\"" + dsBannergroups.Tables[0].Rows[kk]["DisplayOrder"].ToString() + "\" />";
                                strGrid += " </td>";
                                strGrid += "<td>";
                                strGrid += " <a onclick=\"getlayoutid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"updateorder" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\"  href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/updateorder.png\" title=\"Update Group Order\" border=\"0\" /></a>&nbsp;&nbsp;&nbsp;&nbsp;<a onclick=\"return getdeleteid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"deletegroup" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\"  href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/deletegroup.png\" title=\"Delete Group\" border=\"0\" /></a>";
                                strGrid += " </td>";
                                strGrid += "</tr>";
                                strGrid += "</table>";
                                strGrid += " </td>";
                                strGrid += "</tr>";


                            }


                            strGrid += "</table>";
                            strGrid += "</div>";
                        }
                        else if (dsBannergroups.Tables[0].Rows[kk]["BannerTypeId"].ToString() == "2")
                        {


                            strGrid += "<div id='Group'+" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + " style=\"float:left;width:100%;\">";
                            strGrid += "<table cellspacing=\"0\" style=\"width: 100%; border-collapse: collapse;\">";
                            strGrid += "<tr style=\"color: White; font-weight: normal;\">";
                            //strGrid += "<th align=\"center\" style=\"width: 20%;height: 36px;display:none;\" scope=\"col\">Display Order<br /><a onclick=\"getlayoutid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"updateorder1\" style=\"display:none;\" href=\"javascript:void(0);\">Update Order</a></th>";
                            if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString()) > Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 45%;\" scope=\"col\">Banner Image1 (Start Date: " + dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString() + ")</th>";
                            }
                            else if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 45%;\" scope=\"col\">Banner Image1 (End Date: " + dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString() + ")</th>";
                            }
                            else
                            {
                                strGrid += "<th align=\"left\" style=\"width: 45%;\" scope=\"col\">Banner Image1</th>";
                            }
                            if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString()) > Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 45%;\" scope=\"col\">Banner Image2 (Start Date: " + dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString() + ")</th>";
                            }
                            else if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 45%;\" scope=\"col\">Banner Image2 (End Date: " + dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString() + ")</th>";
                            }
                            else
                            {
                                strGrid += "<th align=\"left\" style=\"width: 45%;\" scope=\"col\">Banner Image2</th>";
                            }

                            //strGrid += "<th align=\"left\" style=\"width: 20%;\" scope=\"col\">Banner Title</th>";
                            //strGrid += "<th align=\"left\" style=\"width: 20%;\" scope=\"col\">Banner Url</th>";
                            //strGrid += "<th align=\"center\" style=\"width: 10%;\" scope=\"col\">Status</th>";
                            strGrid += "<th align=\"center\" style=\"width: 10%;\" scope=\"col\">Action</th>";
                            strGrid += "</tr>";



                            for (int i = 0; i < 1; i++)
                            {
                                if ((i + 1) % 2 == 0)
                                {
                                    strGrid += "<tr  class=\"altrow\">";
                                }
                                else
                                {
                                    strGrid += "<tr  class=\"oddrow\">";
                                }

                                //strGrid += "<td align=\"Center\" style=\"display:none;\"></td>";
                                if (File.Exists(Server.MapPath(path.ToString() + "/" + dsbanner.Tables[0].Rows[i]["ImageName"].ToString())))
                                {
                                    Random rd = new Random();
                                    strGrid += "<td align=\"left\"><img width=\"95%\" src=\"" + path.ToString() + dsbanner.Tables[0].Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" /></td>";
                                }
                                else
                                {
                                    strGrid += "<td align=\"left\"></td>";
                                }

                                if (dsbanner.Tables[0].Rows.Count > (i + 1))
                                {
                                    for (int j = i + 1; j < dsbanner.Tables[0].Rows.Count; j++)
                                    {
                                        if (dsbanner.Tables[0].Rows[j]["ismain"].ToString() == "0")
                                        {
                                            if (File.Exists(Server.MapPath(path.ToString() + "/" + dsbanner.Tables[0].Rows[j]["ImageName"].ToString())))
                                            {
                                                Random rd = new Random();
                                                strGrid += "<td align=\"left\"><img width=\"95%\" src=\"" + path.ToString() + dsbanner.Tables[0].Rows[j]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" /></td>";
                                            }
                                            else
                                            {
                                                strGrid += "<td align=\"left\"></td>";
                                            }

                                            break;
                                        }

                                    }
                                }
                                else
                                {
                                    strGrid += "<td align=\"left\"></td>";
                                }


                                //strGrid += "<td align=\"left\">" + dsbanner.Tables[0].Rows[i]["Title"].ToString() + "</td>";
                                //strGrid += "<td align=\"left\">" + dsbanner.Tables[0].Rows[i]["BannerURL"].ToString() + "</td>";
                                //if (Convert.ToBoolean(dsbanner.Tables[0].Rows[i]["Active"].ToString()))
                                //{
                                //    strGrid += "<td align=\"center\"><img src=\"/admin/images/active.gif\"></td>";
                                //}
                                //else
                                //{
                                //    strGrid += "<td align=\"center\"><img src=\"/admin/images/in-active.gif\"></td>";
                                //}
                                strGrid += "<td align=\"center\" ><a href=\"SmallHomepageRotatorbanneruploadMVC.aspx?id=" + dsBannergroups.Tables[0].Rows[kk]["BannerTypeId"].ToString() + "&Storeid=" + ddlStore.SelectedValue.ToString() + "&hid=" + dsbanner.Tables[0].Rows[i]["BannerID"].ToString() + "\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/edit-icon.png\"></a><a onclick=\"Deleterecord(" + dsbanner.Tables[0].Rows[i]["BannerID"].ToString() + ");\" style=\"display:none;\" href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/delete-icon.png\"></a></td>";

                                strGrid += "</tr>";




                            }

                            strGrid += "<tr>";
                            strGrid += "<td colspan=\"2\">";
                            strGrid += "<table width=\"100%\">";
                            strGrid += "<tr>";
                            strGrid += "<td width=\"20%;\">Display Order";

                            strGrid += "</td>";
                            strGrid += "<td>";
                            strGrid += " <input type=\"text\" id=\"text1_" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\" onkeypress=\"return keyRestrict(event,'0123456789');\" name=\"text1_" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\" class=\"order-textfield\" style=\"width:40px;text-align:center;\" value=\"" + dsBannergroups.Tables[0].Rows[kk]["DisplayOrder"].ToString() + "\" />";
                            strGrid += " </td>";
                            strGrid += "<td>";
                            strGrid += " <a onclick=\"getlayoutid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"updateorder" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\"  href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/updateorder.png\" title=\"Update Group Order\" border=\"0\" /></a> &nbsp;&nbsp;&nbsp;&nbsp;<a onclick=\"return getdeleteid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"deletegroup" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\"  href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/deletegroup.png\" title=\"Delete Group\" border=\"0\" /></a>";
                            strGrid += " </td>";
                            strGrid += "</tr>";
                            strGrid += "</table>";
                            strGrid += " </td>";
                            strGrid += "</tr>";


                            strGrid += "</table>";
                            strGrid += "</div>";
                        }
                        else if (dsBannergroups.Tables[0].Rows[kk]["BannerTypeId"].ToString() == "3")
                        {


                            strGrid += "<div id='Group'+" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + " style=\"float:left;width:100%;\">";
                            strGrid += "<table cellspacing=\"0\" style=\"width: 100%; border-collapse: collapse;\">";
                            strGrid += "<tr style=\"color: White; font-weight: normal;\">";
                            //strGrid += "<th align=\"center\" style=\"width: 20%;height: 36px;display:none;\" scope=\"col\">Display Order<br /><a onclick=\"getlayoutid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"updateorder1\" style=\"display:none;\" href=\"javascript:void(0);\">Update Order</a></th>";

                            if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString()) > Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\">Banner Image1 (Start Date: " + dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString() + ")</th>";
                            }
                            else if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\">Banner Image1 (End Date: " + dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString() + ")</th>";
                            }
                            else
                            {
                                strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\">Banner Image1</th>";
                            }
                            if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString()) > Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\">Banner Image2 (Start Date: " + dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString() + ")</th>";
                            }
                            else if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\">Banner Image2 (End Date: " + dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString() + ")</th>";
                            }
                            else
                            {
                                strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\">Banner Image2</th>";
                            }
                            if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString()) > Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\">Banner Image3 (Start Date: " + dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString() + ")</th>";
                            }
                            else if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\">Banner Image3 (End Date: " + dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString() + ")</th>";
                            }
                            else
                            {
                                strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\">Banner Image3</th>";
                            }
                            //strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\"> Banner Image1</th>";
                            //strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\"> Banner Image2</th>";
                            //strGrid += "<th align=\"left\" style=\"width: 30%;\" scope=\"col\"> Banner Image3</th>";
                            //strGrid += "<th align=\"left\" style=\"width: 20%;\" scope=\"col\">Banner Title</th>";
                            //strGrid += "<th align=\"left\" style=\"width: 20%;\" scope=\"col\">Banner Url</th>";
                            //strGrid += "<th align=\"center\" style=\"width: 10%;\" scope=\"col\">Status</th>";
                            strGrid += "<th align=\"center\" style=\"width: 10%;\" scope=\"col\">Action</th>";
                            strGrid += "</tr>";



                            for (int i = 0; i < 1; i++)
                            {
                                if ((i + 1) % 3 == 0)
                                {
                                    strGrid += "<tr  class=\"altrow\">";
                                }
                                else
                                {
                                    strGrid += "<tr  class=\"oddrow\">";
                                }

                                //strGrid += "<td align=\"Center\" style=\"display:none;\"></td>";
                                if (File.Exists(Server.MapPath(path.ToString() + "/" + dsbanner.Tables[0].Rows[i]["ImageName"].ToString())))
                                {
                                    Random rd = new Random();
                                    strGrid += "<td align=\"left\"><img width=\"95%\" src=\"" + path.ToString() + dsbanner.Tables[0].Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" /></td>";
                                }
                                else
                                {
                                    strGrid += "<td align=\"left\"></td>";
                                }

                                if (dsbanner.Tables[0].Rows.Count > (i + 1))
                                {
                                    int fl = 0;
                                    for (int j = i + 1; j < dsbanner.Tables[0].Rows.Count; j++)
                                    {
                                        if (dsbanner.Tables[0].Rows[j]["ismain"].ToString() == "0")
                                        {
                                            if (File.Exists(Server.MapPath(path.ToString() + "/" + dsbanner.Tables[0].Rows[j]["ImageName"].ToString())))
                                            {
                                                Random rd = new Random();
                                                strGrid += "<td align=\"left\"><img width=\"95%\" src=\"" + path.ToString() + dsbanner.Tables[0].Rows[j]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" /></td>";
                                            }
                                            else
                                            {
                                                strGrid += "<td align=\"left\"></td>";
                                            }
                                            fl++;
                                            if (fl == 2)
                                            {


                                                break;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    strGrid += "<td align=\"left\"></td>";
                                }


                                //strGrid += "<td align=\"left\">" + dsbanner.Tables[0].Rows[i]["Title"].ToString() + "</td>";
                                //strGrid += "<td align=\"left\">" + dsbanner.Tables[0].Rows[i]["BannerURL"].ToString() + "</td>";
                                //if (Convert.ToBoolean(dsbanner.Tables[0].Rows[i]["Active"].ToString()))
                                //{
                                //    strGrid += "<td align=\"center\"><img src=\"/admin/images/active.gif\"></td>";
                                //}
                                //else
                                //{
                                //    strGrid += "<td align=\"center\"><img src=\"/admin/images/in-active.gif\"></td>";
                                //}
                                strGrid += "<td align=\"center\" ><a href=\"SmallHomepageRotatorbanneruploadMVC.aspx?id=" + dsBannergroups.Tables[0].Rows[kk]["BannerTypeId"].ToString() + "&Storeid=" + ddlStore.SelectedValue.ToString() + "&hid=" + dsbanner.Tables[0].Rows[i]["BannerID"].ToString() + "\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/edit-icon.png\"></a><a onclick=\"Deleterecord(" + dsbanner.Tables[0].Rows[i]["BannerID"].ToString() + ");\" style=\"display:none;\" href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/delete-icon.png\"></a></td>";

                                strGrid += "</tr>";
                            }

                            strGrid += "<tr>";
                            strGrid += "<td colspan=\"2\">";
                            strGrid += "<table width=\"100%\">";
                            strGrid += "<tr>";
                            strGrid += "<td width=\"20%;\">Display Order";

                            strGrid += "</td>";
                            strGrid += "<td>";
                            strGrid += " <input type=\"text\" id=\"text1_" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\" onkeypress=\"return keyRestrict(event,'0123456789');\" name=\"text1_" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\" class=\"order-textfield\" style=\"width:40px;text-align:center;\" value=\"" + dsBannergroups.Tables[0].Rows[kk]["DisplayOrder"].ToString() + "\" />";
                            strGrid += " </td>";
                            strGrid += "<td>";
                            strGrid += " <a onclick=\"getlayoutid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"updateorder" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\"  href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/updateorder.png\" title=\"Update Group Order\" border=\"0\" /></a> &nbsp;&nbsp;&nbsp;&nbsp;<a onclick=\"return getdeleteid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"deletegroup" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\"  href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/deletegroup.png\" title=\"Delete Group\" border=\"0\" /></a>";
                            strGrid += " </td>";
                            strGrid += "</tr>";
                            strGrid += "</table>";
                            strGrid += " </td>";
                            strGrid += "</tr>";


                            strGrid += "</table>";
                            strGrid += "</div>";
                        }
                        else if (dsBannergroups.Tables[0].Rows[kk]["BannerTypeId"].ToString() == "4")
                        {


                            strGrid += "<div id='Group'+" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + " style=\"float:left;width:100%;\">";
                            strGrid += "<table cellspacing=\"0\" style=\"width: 100%; border-collapse: collapse;\">";
                            strGrid += "<tr style=\"color: White; font-weight: normal;\">";
                            //strGrid += "<th align=\"center\" style=\"width: 20%;height: 36px;display:none;\" scope=\"col\">Display Order<br /><a onclick=\"getlayoutid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"updateorder1\" style=\"display:none;\" href=\"javascript:void(0);\">Update Order</a></th>";
                            if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString()) > Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image1 (Start Date: " + dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString() + ")</th>";
                            }
                            else if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image1 (End Date: " + dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString() + ")</th>";
                            }
                            else
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image1</th>";
                            }
                            if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString()) > Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image2 (Start Date: " + dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString() + ")</th>";
                            }
                            else if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image2 (End Date: " + dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString() + ")</th>";
                            }
                            else
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image2</th>";
                            }

                            if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString()) > Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image3 (Start Date: " + dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString() + ")</th>";
                            }
                            else if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image3 (End Date: " + dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString() + ")</th>";
                            }
                            else
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image3</th>";
                            }

                            if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString()) > Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image4 (Start Date: " + dsBannergroups.Tables[0].Rows[kk]["startdate"].ToString() + ")</th>";
                            }
                            else if (Convert.ToDateTime(dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image4 (End Date: " + dsBannergroups.Tables[0].Rows[kk]["enddate"].ToString() + ")</th>";
                            }
                            else
                            {
                                strGrid += "<th align=\"left\" style=\"width: 22%;\" scope=\"col\">Banner Image4</th>";
                            }

                            //strGrid += "<th align=\"left\" style=\"width: 20%;\" scope=\"col\">Banner Title</th>";
                            //strGrid += "<th align=\"left\" style=\"width: 20%;\" scope=\"col\">Banner Url</th>";
                            //strGrid += "<th align=\"center\" style=\"width: 10%;\" scope=\"col\">Status</th>";
                            strGrid += "<th align=\"center\" style=\"width: 10%;\" scope=\"col\">Action</th>";
                            strGrid += "</tr>";

                            string strsmallbanner = "";

                            for (int i = 0; i < 1; i++)
                            {
                                if ((i + 1) % 4 == 0)
                                {
                                    strGrid += "<tr  class=\"altrow\">";
                                }
                                else
                                {
                                    strGrid += "<tr  class=\"oddrow\">";
                                }

                                //strGrid += "<td align=\"Center\" style=\"display:none;\"></td>";
                                if (File.Exists(Server.MapPath(path.ToString() + "/" + dsbanner.Tables[0].Rows[i]["ImageName"].ToString())))
                                {
                                    Random rd = new Random();
                                    strGrid += "<td align=\"left\"><img width=\"95%\" src=\"" + path.ToString() + dsbanner.Tables[0].Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" /></td>";
                                }
                                else
                                {
                                    strGrid += "<td align=\"left\"></td>";
                                }

                                if (dsbanner.Tables[0].Rows.Count > (i + 1))
                                {
                                    for (int j = i + 1; j < dsbanner.Tables[0].Rows.Count; j++)
                                    {
                                        if (j < 4)
                                        {
                                            if (dsbanner.Tables[0].Rows[j]["ismain"].ToString() == "0")
                                            {
                                                if (File.Exists(Server.MapPath(path.ToString() + "/" + dsbanner.Tables[0].Rows[j]["ImageName"].ToString())))
                                                {
                                                    Random rd = new Random();
                                                    strGrid += "<td align=\"left\"><img width=\"95%\" src=\"" + path.ToString() + dsbanner.Tables[0].Rows[j]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" /></td>";
                                                }
                                                else
                                                {
                                                    strGrid += "<td align=\"left\"></td>";
                                                }

                                                //break;
                                            }
                                        }
                                        if (j >= 4)
                                        {
                                            strsmallbanner += "<tr class=\"altrow\">";
                                            if (dsbanner.Tables[0].Rows[j]["ismain"].ToString() == "0")
                                            {
                                                if (File.Exists(Server.MapPath(path.ToString() + "/" + dsbanner.Tables[0].Rows[j]["ImageName"].ToString())))
                                                {
                                                    Random rd = new Random();
                                                    strsmallbanner += "<td align=\"center\" colspan=\"5\"><img src=\"" + path.ToString() + dsbanner.Tables[0].Rows[j]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" /></td>";
                                                }
                                                else
                                                {
                                                    strsmallbanner += "<td align=\"center\" colspan=\"5\"></td>";
                                                }

                                                //break;
                                            }
                                            strsmallbanner += "</tr>";
                                        }

                                    }
                                }
                                else
                                {
                                    strGrid += "<td align=\"left\"></td>";
                                }


                                //strGrid += "<td align=\"left\">" + dsbanner.Tables[0].Rows[i]["Title"].ToString() + "</td>";
                                //strGrid += "<td align=\"left\">" + dsbanner.Tables[0].Rows[i]["BannerURL"].ToString() + "</td>";
                                //if (Convert.ToBoolean(dsbanner.Tables[0].Rows[i]["Active"].ToString()))
                                //{
                                //    strGrid += "<td align=\"center\"><img src=\"/admin/images/active.gif\"></td>";
                                //}
                                //else
                                //{
                                //    strGrid += "<td align=\"center\"><img src=\"/admin/images/in-active.gif\"></td>";
                                //}
                                strGrid += "<td align=\"center\" ><a href=\"SmallHomepageRotatorbanneruploadMVC.aspx?id=" + dsBannergroups.Tables[0].Rows[kk]["BannerTypeId"].ToString() + "&Storeid=" + ddlStore.SelectedValue.ToString() + "&hid=" + dsbanner.Tables[0].Rows[i]["BannerID"].ToString() + "\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/edit-icon.png\"></a><a onclick=\"Deleterecord(" + dsbanner.Tables[0].Rows[i]["BannerID"].ToString() + ");\" style=\"display:none;\" href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/delete-icon.png\"></a></td>";

                                strGrid += "</tr>";
                                strGrid += strsmallbanner;



                            }

                            strGrid += "<tr>";
                            strGrid += "<td colspan=\"5\">";
                            strGrid += "<table width=\"100%\">";
                            strGrid += "<tr>";
                            strGrid += "<td width=\"20%;\">Display Order";

                            strGrid += "</td>";
                            strGrid += "<td>";
                            strGrid += " <input type=\"text\" id=\"text1_" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\" onkeypress=\"return keyRestrict(event,'0123456789');\" name=\"text1_" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\" class=\"order-textfield\" style=\"width:40px;text-align:center;\" value=\"" + dsBannergroups.Tables[0].Rows[kk]["DisplayOrder"].ToString() + "\" />";
                            strGrid += " </td>";
                            strGrid += "<td>";
                            strGrid += " <a onclick=\"getlayoutid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"updateorder" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\"  href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/updateorder.png\" title=\"Update Group Order\" border=\"0\" /></a> &nbsp;&nbsp;&nbsp;&nbsp;<a onclick=\"return getdeleteid(" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + ");\" id=\"deletegroup" + dsBannergroups.Tables[0].Rows[kk]["HomeRotatorId"].ToString() + "\"  href=\"javascript:void(0);\"><img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/deletegroup.png\" title=\"Delete Group\" border=\"0\" /></a>";
                            strGrid += " </td>";
                            strGrid += "</tr>";
                            strGrid += "</table>";
                            strGrid += " </td>";
                            strGrid += "</tr>";


                            strGrid += "</table>";
                            strGrid += "</div>";
                        }

                        strGrid += "</fieldset>";

                    }
                }

            }

            if (strGrid == "")
            {
                ltactive.Text = "No Record(s) Found.";
            }
            else
            {
                ltactive.Text = strGrid;
            }








        }

        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetGrid();
            GetGroupGrid();

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            tb_RotatorHomeBannerDetail objRotatorhome = new tb_RotatorHomeBannerDetail();
            objRotatorhome.BannerID = Convert.ToInt32(hdnid.Value.ToString());
            //  objHome.DeleteBanner(objRotatorhome);
            CommonComponent.ExecuteCommonData("DELETE FROM tb_RotatorHomeBannerDetailSmall_MVC WHERE BannerID=" + objRotatorhome.BannerID + "");
            //GetGrid();
            GetGroupGrid();
            // BindBanner();
            getfinalhtml();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string[] strkeys = Request.Form.AllKeys;
            foreach (string strky in strkeys)
            {
                if (strky.ToString().ToLower().IndexOf("text1_" + hdnlayouid.Value.ToString()) > -1)
                {
                    if (Request.Form[strky] != null)
                    {
                        if (Request.Form[strky].ToString() != "")
                        {
                            CommonComponent.ExecuteCommonData("UPDATE tb_RotatorHomebannersmall_MVC SET DisplayOrder=" + Request.Form[strky].ToString() + " WHERE HomeRotatorId=" + hdnlayouid.Value.ToString() + "");
                        }
                        else
                        {
                            CommonComponent.ExecuteCommonData("UPDATE tb_RotatorHomebannersmall_MVC SET DisplayOrder=0 WHERE HomeRotatorId=" + hdnlayouid.Value.ToString() + "");
                        }
                    }
                }
            }
            // GetGrid();
            GetGroupGrid();
            // BindBanner();
            getfinalhtml();
        }
        protected void btnActive_Click(object sender, EventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            //    objHome.UpdateRotatebanner(Convert.ToInt32(ddlStore.SelectedValue.ToString()), Convert.ToInt32(hdnTypeid.Value.ToString()));

            CommonComponent.ExecuteCommonData("UPDATE tb_RotatorHomebannersmall_MVC SET IsActive=1 WHERE StoreId=" + Convert.ToInt32(ddlStore.SelectedValue.ToString()) + " AND HomeRotatorId=" + Convert.ToInt32(hdnTypeid.Value.ToString()) + "");
            string strHTML = "";
            HomeBannerComponent objbanner = new HomeBannerComponent();
            DataSet dsbanner = new DataSet();
            ViewState["ImagebannrpathSmall"] = CommonComponent.GetScalarCommonData("SELECT TOP 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='ImagePathHomePageBannerMVC' AND storeid=" + ddlStore.SelectedValue.ToString() + " AND isnull(Deleted,0)=0");
            // dsbanner = objbanner.GetHomeRotatorBanner(Convert.ToInt32(ddlStore.SelectedValue.ToString()), Convert.ToInt32(hdnTypeid.Value.ToString()));

            //GetGrid();
            GetGroupGrid();
            // BindBanner();
            getfinalhtml();
        }

        protected void btnInactive_Click(object sender, EventArgs e)
        {
            CommonComponent.ExecuteCommonData("UPDATE tb_RotatorHomebannersmall_MVC SET IsActive=0 WHERE StoreId=" + Convert.ToInt32(ddlStore.SelectedValue.ToString()) + " AND HomeRotatorId=" + Convert.ToInt32(hdnTypeid.Value.ToString()) + "");
            GetGroupGrid();
            // BindBanner();
            getfinalhtml();

        }

        protected void btnDeletegroup_Click(object sender, EventArgs e)
        {
            string[] strkeys = Request.Form.AllKeys;
            foreach (string strky in strkeys)
            {
                if (strky.ToString().ToLower().IndexOf("text1_" + hdnlayouid.Value.ToString()) > -1)
                {
                    if (Request.Form[strky] != null)
                    {
                        if (Request.Form[strky].ToString() != "")
                        {
                            CommonComponent.ExecuteCommonData("Delete from tb_RotatorHomeBannerDetailSmall_MVC WHERE HomeRotatorId=" + hdnlayouid.Value.ToString() + "");
                            CommonComponent.ExecuteCommonData("Delete from tb_RotatorHomebannersmall_MVC WHERE HomeRotatorId=" + hdnlayouid.Value.ToString() + "");
                        }

                    }
                }
            }
            // GetGrid();
            GetGroupGrid();
            // BindBanner();
            getfinalhtml();
        }

        protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetGroupGrid();
        }
    }
}