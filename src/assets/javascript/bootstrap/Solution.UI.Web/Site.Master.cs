using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Text;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;
using System.Net.Mail;
using System.Configuration;
using System.IO;

namespace Solution.UI.Web
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public string MetaDescription;
        public string MetaKeywords;
        public string Title;
        public string strMeta = "";
        public string strProductId = "";
        protected string MenuData = string.Empty;
        CategoryComponent objCategorycomponent = new CategoryComponent();
        RedTag_CCTV_Ecomm_DBEntities ctxRedtag = new RedTag_CCTV_Ecomm_DBEntities();


        System.Text.StringBuilder strSEORemarketingCode = new StringBuilder(20000);
        System.Text.StringBuilder strSEOGoogleAnalyticCode = new StringBuilder(20000);
        System.Text.StringBuilder strSEOSpringMeterics = new StringBuilder(20000);
        System.Text.StringBuilder strSEOGooglePlus = new StringBuilder(20000);
        System.Text.StringBuilder strSEOSocialLannex = new StringBuilder(20000);
        System.Text.StringBuilder strSEOCretioRemarketingCode = new StringBuilder(20000);
        System.Text.StringBuilder strSEONewsletterCode = new StringBuilder(20000);
        System.Text.StringBuilder strSEOnextopiaCode = new StringBuilder(20000);
        System.Text.StringBuilder strSEOPayPalCode = new StringBuilder(20000);

        public string SEORemarketingCode = "";
        public string SEOGoogleAnalyticCode = "";
        public string SEOSpringMeterics = "";
        public string SEOGooglePlus = "";
        public string SEOSocialLannex = "";
        public string SEOCretioRemarketingCode = "";
        public string SEONewsletterCode = "";
        public string SEOnextopiaCode = "";
        public string SEOPayPalCode = "";
        public string jqueryscroll = "";
        public string jqueryscroll1 = "";
        public string calljavascript = "";
        public string klevuScript = "";



        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        ///  

        protected void Page_Load(object sender, EventArgs e)
        {



            string strfilePath = Request.RawUrl.ToLower().ToString();
            if (strfilePath.Contains("index.aspx") || Request.RawUrl.ToString() == "/")
                divShoppingCart.Attributes.Add("style", "display:block;");
            if (strfilePath.Contains("addtocart.aspx"))
                divShoppingCart.Attributes.Add("style", "display:none;");
            if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/index.aspx") > -1)
            {
                hidehomebanner.Visible = true;
                //hidehomerightbanner.Visible = true;
                //hideheaderfreeship.Visible = true;
                hidePhone.Visible = true;
                // hidePhoneshow.Visible = false;
                //divIndexRightSide.Visible = true;
                //divMainBanner.Attributes.Add("class", "main-banner");
                BindNewArrival();
            }
            else if (Request.RawUrl != null && Request.RawUrl.ToString().IndexOf("?") > -1 && Request.RawUrl.ToString().Substring(0, Request.RawUrl.ToString().IndexOf("?")) == "/")
            {
                hidehomebanner.Visible = true;
                //hidehomerightbanner.Visible = true;
                //hideheaderfreeship.Visible = true;
                hidePhone.Visible = true;
                //hidePhoneshow.Visible = false;
                //divIndexRightSide.Visible = true;
                //divMainBanner.Attributes.Add("class", "main-banner");
                BindNewArrival();
            }
            else if (Request.RawUrl.ToString() == "/")
            {
                hidehomebanner.Visible = true;
                //hidehomerightbanner.Visible = true;
                //hideheaderfreeship.Visible = true;
                hidePhone.Visible = true;
                //hidePhoneshow.Visible = false;
                //divIndexRightSide.Visible = true;
                //divMainBanner.Attributes.Add("class", "main-banner");
                BindNewArrival();

            }
            else
            {
                hidehomebanner.Visible = false;
                //hidehomerightbanner.Visible = false;
                //hideheaderfreeship.Visible = true;
                hidePhone.Visible = true;
                //hidePhoneshow.Visible = false;
            }
            //if ((Request.RawUrl.ToString().ToLower().Contains("/login.aspx")) || (Request.RawUrl.ToString().ToLower().Contains("/addtocart.aspx"))
            //    || (Request.RawUrl.ToString().ToLower().Contains("/addtocart.aspx")) || (Request.RawUrl.ToString().ToLower().Contains("/checkoutcommon.aspx"))
            //    || (Request.RawUrl.ToString().ToLower().Contains("/orderreceived.aspx")) || (Request.RawUrl.ToString().ToLower().Contains("/viewoldorders.aspx"))
            //    || (Request.RawUrl.ToLower().Contains("/orderdetails.aspx")) || (Request.RawUrl.ToString().ToLower().Contains("/viewrecentorders.aspx")))
            //{
            //    leftmenu.Visible = false;
            //}
            //else
            //{
            //    leftmenu.Visible = true;
            //}

            // jqueryscroll = "<script type=\"text/javascript\" src=\"/js/jquery.min.js\"></script>";
            if (Request.Url != null && Request.Url.ToString() != "/" && Request.Url.ToString().ToLower().IndexOf("index.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("subcategory.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("itempage.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("itempagerating.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("roman-itempage.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("productsearchlist.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("itempageratingnew.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("romanitempagerating.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("free-swatch.aspx") <= -1)
            {
                jqueryscroll = "<script type=\"text/javascript\" src=\"/js/jquery.min.js\"></script>";
            }
            if (Request.Url != null && (Request.Url.ToString() == "/" || Request.Url.ToString().ToLower().IndexOf("index.aspx") > -1))
            {
                // jqueryscroll1 = "<script type=\"text/javascript\" src=\"/js/jquery-homebanner.js\"></script><script type=\"text/javascript\" src=\"/js/homebannerslider.js\"></script>";
                // jqueryscroll1 += "<link rel=\"stylesheet\" type=\"text/css\" href=\"/css/bannerslider.css\">";

            }
            //if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("login.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("checkoutcommon.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("customerquotecheckout.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("addtocart.aspx") <= -1)
            //{
            //    klevuScript = "<script type=\"text/javascript\">var klevu_apiKey = 'klevu-13939350527231', klevu_analytics_key = 'klevu-139393549851193', searchTextBoxName = 'txtSearch', klevu_lang = 'en', klevu_result_top_margin = '', klevu_result_left_margin = ''; (function () { var ws = document.createElement('script'); ws.type = 'text/javascript'; ws.async = true; ws.src = 'http://box.klevu.com/klevu-js-v1/js/klevu-webstore.js'; var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ws, s); })();}</script>";
            //}


            AppConfig.StoreID = Convert.ToInt32(ConfigurationManager.AppSettings["GeneralStoreID"]);
            if (!Convert.ToBoolean(AppLogic.AppConfigs("IsStoreon")))
            {
                if (AppLogic.AppConfigs("StoreClosedMessage").ToString() != "")
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('" + AppLogic.AppConfigs("StoreClosedMessage").ToString() + "');window.location.href='/opps.html';", true);
                }
                else
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.location.href='/opps.html';", true);
            }

            if (!IsPostBack)
            {
                if (Request.Url != null && Request.RawUrl != null && Request.Url.ToString().ToLower().IndexOf("/index.aspx") > -1)
                {
                    this.form1.Action = Request.RawUrl;
                }


                //HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                //HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
                //HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //HttpContext.Current.Response.Cache.SetNoStore();

                txtSearch.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                SetBookMark();
                ViewState["RowUrl"] = Request.RawUrl.ToString();
                headerrotator();
                string strfreeshipping = Convert.ToString(CommonComponent.GetScalarCommonData("Select isnull(configvalue,'0') from tb_AppConfig WHERE Configname ='FreeShippingLimit' and isnull(Deleted,0)=0 and StoreId=1"));

                ltfreeamount.Text = String.Format("{0:0.00}", Convert.ToDecimal(strfreeshipping));
                GetRandomProductId();
                //BindFooterContentRight();
                //BindFooterContentBottom();
                AddSiteConfig();
                BindBanner();
                BindRightBanner();
                LoadTestimonial();
                if (Request.RawUrl != null && (Request.RawUrl.ToString() == "/" || Request.Url.ToString().ToLower().IndexOf("/index.aspx") > -1))
                {
                    GetHomeContent();
                }

                GetCustomerService();

                GetFreeshipping();

                //BindPattern();
                //BindFabric();
                //BindStyle();
                //BindColors();
                //BindHeader();
                //Session["IndexPriceValue"] = null;
                //Session["IndexFabricValue"] = null;
                //Session["IndexPatternValue"] = null;
                //Session["IndexStyleValue"] = null;
                //Session["IndexColorValue"] = null;
                //Session["IndexHeaderValue"] = null;
                //Session["IndexCustomValue"] = null;

                ////Spring Meterics
                strSEOSpringMeterics.Clear();
                strSEOSpringMeterics.AppendLine("<script type='text/javascript'>");
                strSEOSpringMeterics.AppendLine("var _springMetq = _springMetq || [];");
                strSEOSpringMeterics.AppendLine("_springMetq.push(['id', '3b8785e203']);");
                strSEOSpringMeterics.AppendLine("(");
                strSEOSpringMeterics.AppendLine("function(){");
                strSEOSpringMeterics.AppendLine("var s = document.createElement('script');");
                strSEOSpringMeterics.AppendLine("s.type = 'text/javascript';");
                strSEOSpringMeterics.AppendLine("s.async = true;");
                strSEOSpringMeterics.AppendLine("s.src = ('https:' == document.location.protocol ? 'https://d3rmnwi2tssrfx.cloudfront.net/a.js' : 'http://static.springmetrics.com/a.js');");
                strSEOSpringMeterics.AppendLine("var x = document.getElementsByTagName('script')[0];");
                strSEOSpringMeterics.AppendLine("x.parentNode.insertBefore(s, x);");
                strSEOSpringMeterics.AppendLine("}");
                strSEOSpringMeterics.AppendLine(")();");
                strSEOSpringMeterics.AppendLine("</script>");
                SEOSpringMeterics = strSEOSpringMeterics.ToString();
                ////end

                ////For Cretio Remarketing code
                strSEOCretioRemarketingCode.Clear();
                strSEOCretioRemarketingCode.AppendLine("<script type='text/javascript'>");
                strSEOCretioRemarketingCode.AppendLine("var CRITEO_CONF = [[{ pageType: 'home' }], [6853, 'pac', '', '010', [[7722358, 7722359]]]];");
                strSEOCretioRemarketingCode.AppendLine("if (typeof(CRITEO) != \"undefined\") { CRITEO.Load(false); }");
                strSEOCretioRemarketingCode.AppendLine("</script>");
                SEOCretioRemarketingCode = strSEOCretioRemarketingCode.ToString();
                ////end

                //Start
                //strSEOSocialLannex.Clear();
                //strSEOSocialLannex.AppendLine("<script type=\"text/javascript\">");
                //strSEOSocialLannex.AppendLine("var sa_page='1';(function() {function sa_async_load() { var sa = document.createElement('script');");
                //strSEOSocialLannex.AppendLine("sa.type = 'text/javascript';sa.async = true;sa.src = '//cdn.socialannex.com/partner/9910051/universal.js';");
                //strSEOSocialLannex.AppendLine("var sax = document.getElementsByTagName('script')[0];sax.parentNode.insertBefore(sa, sax); }");
                //strSEOSocialLannex.AppendLine("if(window.attachEvent) {window.attachEvent('onload', sa_async_load);}");
                //strSEOSocialLannex.AppendLine("else{window.addEventListener('load', sa_async_load,false);}})();");
                //strSEOSocialLannex.AppendLine("</script>");
                //SEOSocialLannex = strSEOSocialLannex.ToString();
                //end
                //    ////Google Analytic Code
                strSEORemarketingCode.Clear();
                strSEOGoogleAnalyticCode.AppendLine("<script type=\"text/javascript\">");
                strSEOGoogleAnalyticCode.AppendLine("var _gaq = _gaq || [];");
                strSEOGoogleAnalyticCode.AppendLine("_gaq.push(['_setAccount', 'UA-2756708-1']);");
                strSEOGoogleAnalyticCode.AppendLine("_gaq.push(['_trackPageview']);");
                strSEOGoogleAnalyticCode.AppendLine("");
                strSEOGoogleAnalyticCode.AppendLine("(function() {");
                strSEOGoogleAnalyticCode.AppendLine("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
                strSEOGoogleAnalyticCode.AppendLine("ga.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'stats.g.doubleclick.net/dc.js';");
                strSEOGoogleAnalyticCode.AppendLine("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
                strSEOGoogleAnalyticCode.AppendLine("})();");
                strSEOGoogleAnalyticCode.AppendLine("</script>");
                SEOGoogleAnalyticCode = strSEOGoogleAnalyticCode.ToString();
                //    ////end


                if ((Request.RawUrl.ToString() == "/") || (Request.RawUrl.ToString().ToLower().Contains("/index.aspx")))
                {
                    Session["HeaderCatid"] = null;
                    calljavascript = "<script type=\"text/javascript\" src=\"/js/responsiveslides.min.js\"></script><script type=\"text/javascript\">";
                    calljavascript += " $(function () {";
                    calljavascript += "$('#slider4').responsiveSlides({";
                    calljavascript += " auto: true,";
                    calljavascript += " pager: true,";
                    calljavascript += " nav: false,";
                    calljavascript += " speed: 500,";
                    calljavascript += " namespace: \"callbacks\"";
                    calljavascript += " });";
                    calljavascript += "});";
                    calljavascript += "</script>";

                    //calljavascript = "<script type=\"text/javascript\"> $(function(){";
                    //calljavascript += "$('#slides').slides({";
                    //calljavascript += "preload: true,";
                    //calljavascript += "preloadImage: 'img/loading.gif',";
                    //calljavascript += "play: 5000,";
                    //calljavascript += "pause: 2500,";
                    //calljavascript += "hoverPause: true";
                    //calljavascript += "});";
                    //calljavascript += "});</script>";

                    //calljavascript = "<script type=\"text/javascript\">";
                    //calljavascript += "pgwjs.checkNavbar();$(window).resize(function(){ pgwjs.checkNavbar(); });";
                    //calljavascript += "$('a[data-goto]').click(function() { pgwjs.goTo($(this).attr('data-goto')); });";
                    //calljavascript += "$('#mbmn').click(function(e) { pgwjs.displayMobileMenu(); e.stopPropagation(); });";
                    //calljavascript += "$(document).click(function() { pgwjs.disableMobileMenu(); });";
                    //calljavascript += "var $j = jQuery.noConflict(); $j(document).ready(function() { $j('.pgwSlider').pgwSlider();});</script>";


                }
                else if (Request.RawUrl.ToString().ToLower().Contains("/checkoutcommon.aspx"))
                {
                    Session["HeaderCatid"] = null;

                }
                else if (Request.Url.ToString().ToLower().Contains("/category.aspx") || Request.Url.ToString().ToLower().Contains("/subcategory.aspx") || Request.Url.ToString().ToLower().Contains("/romancategory.aspx"))
                {
                }
                else if (Request.RawUrl.ToString().ToLower().Contains("/checkoutcustomerquote.aspx"))
                {
                    Session["HeaderCatid"] = null;

                }
                else if (Request.Url.ToString().ToLower().Contains("itempage.aspx") || Request.Url.ToString().ToLower().Contains("roman-itempage.aspx"))
                {


                }
                else if (Request.RawUrl.ToString().ToLower().Contains("/productsearchlist.aspx"))
                {


                }
                else if (Request.RawUrl.ToString().ToLower().Contains("/search.aspx"))
                {
                    Session["HeaderCatid"] = null;

                }

                else
                {


                    Session["HeaderCatid"] = null;
                }
            }

            SetProductyofthemonth();
            BindHeaderLink();
            if (!IsPostBack)
            {
                BindCartDetails();
            }

            #region Login - Logout manage function

            if ((Session["IsAnonymous"] == null && Session["UserName"] != null && Session["CustID"] != null) || (Session["IsAnonymous"] != null && Session["IsAnonymous"].ToString().Trim() != "true" && Session["UserName"] != null && Session["CustID"] != null))
            {
                ltLogin.Visible = false;

                ltregister.InnerHtml = "My Account";
                MyAccount.Visible = true;
                ltregister.HRef = "/myaccount.aspx";

                ltregister.Title = "MyAccount";
                lkbLogout.Visible = true;
                //ulcontent.Attributes.Add("style", "width:505px;");
            }

            else if (Session["IsAnonymous"] != null && Session["IsAnonymous"].ToString().Trim() == "true")
            {
                ltLogin.Visible = true;
                ltregister.InnerHtml = "Register";
                ltregister.HRef = "/CreateAccount.aspx";
                ltregister.Title = "Register";
                lkbLogout.Visible = false;
                //ulcontent.Attributes.Add("style", "width:405px;");
            }
            else
            {
                ltLogin.Visible = true;
                ltregister.InnerHtml = "Register";
                ltregister.HRef = "/CreateAccount.aspx";
                ltregister.Title = "Register";
                lkbLogout.Visible = false;
                //ulcontent.Attributes.Add("style", "width:405px;");
            }

            #endregion
            if (Request.RawUrl != null && (Request.RawUrl.ToString() == "/" || Request.Url.ToString().ToLower().IndexOf("/index.aspx") > -1))
            {

            }
            else
            {
                if (Request.Url.ToString().ToLower().IndexOf("subcategory.aspx") > -1 || Request.Url.ToString().ToLower().IndexOf("category.aspx") > -1)
                {
                    string strDescription = "";
                    string name = "";
                    if (Request.QueryString["CatID"] != null)
                    {
                        DataSet dsname = new DataSet();
                        dsname = CommonComponent.GetCommonDataSet("SELECT Description,Name,isnull(canonicaldescription,'') as canonicaldescription FROM tb_Category WHERE CategoryID=" + Request.QueryString["CatID"].ToString() + "");
                        if (dsname != null && dsname.Tables.Count > 0 && dsname.Tables[0].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dsname.Tables[0].Rows[0]["canonicaldescription"].ToString()))
                            {
                                strDescription = Convert.ToString(dsname.Tables[0].Rows[0]["canonicaldescription"].ToString()) + "<br />";
                            }
                            
                            strDescription += Convert.ToString(dsname.Tables[0].Rows[0]["Description"].ToString());
                            name = Convert.ToString(dsname.Tables[0].Rows[0]["Name"].ToString());
                        }
                    }
                    if (!string.IsNullOrEmpty(strDescription))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidefooter", "$('#divhalfpricedrapes').html('<div class=\"footer-row2-bg\">" + strDescription.Replace("\r\n", "<br />").Replace(System.Environment.NewLine, "<br />") + "</div>');", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidefooter", "$('#divhalfpricedrapes').html('');$('#divhalfpricedrapes').css('display','none');", true);
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidefooter", "$('#divhalfpricedrapes').html('');$('#divhalfpricedrapes').css('display','none');", true);

                }
            }
            if (Request.RawUrl != null)
            {

                //http://<%=Request.Url.Authority %><%=Request.ApplicationPath.TrimEnd('/') %><%=Request.RawUrl  %>
                if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("itempage.aspx") > -1)
                {
                    if (Request.QueryString["PID"] != null)
                    {
                        string strurl = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT CanonicalUrl FROM tb_Product WHERE ProductId=" + Request.QueryString["PID"].ToString() + " and isnull(Isseocanonical,0)=1"));
                        if(!string.IsNullOrEmpty(strurl))
                        {
                            canonical.Href = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + strurl;
                        }
                        else
                        {
                            canonical.Visible = false;
                        }
                    }
                    else
                    {
                        canonical.Visible = false;
                    }


                    //Literal lt = (Literal)this.ContentPlaceHolder1.FindControl("ltBreadcrmbs");
                    //if (lt != null && lt.Text.ToString().ToLower().IndexOf("vintage-cotton-velvet-curtains.html") > -1)
                    //{
                    //    canonical.Href = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/vintage-cotton-velvet-curtains.html";
                    //}
                    //else
                    //{

                    //    try
                    //    {
                    //        if (lt != null && !string.IsNullOrEmpty(lt.Text.ToString()))
                    //        {
                    //            string[] strhref = System.Text.RegularExpressions.Regex.Split(lt.Text.ToString(), "href=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    //            string strconical = strhref[strhref.Length - 1];
                    //            strconical = strconical.Substring(1, strconical.IndexOf(".html") + 5);
                    //            strconical = strconical.Replace("\"", "").Replace(">", "");
                    //            strconical = strconical.Replace("'", "");
                    //            canonical.Href = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + strconical.ToString();
                    //        }
                    //        else
                    //        {
                    //            canonical.Href = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + Request.RawUrl.ToString();
                    //        }
                    //    }
                    //    catch
                    //    {
                    //        canonical.Href = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + Request.RawUrl.ToString();
                    //    }



                    //}
                }
                else
                {
                    canonical.Visible = false;
                    canonical.Href = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + Request.RawUrl.ToString();
                }

            }
            else
            {
                canonical.Visible = false;
                canonical.Href = Request.Url.ToString();
            }


        }

        private void GetRandomProductId()
        {
            DataSet dsProduct = new DataSet();
            dsProduct = CommonComponent.GetCommonDataSet("SELECT Top 1 ProductID FROM tb_Product WHERE isnull(active,0)=1 and Storeid=1 and isnull(deleted,0)=0 and ProductId in (SELECT ProductId FROM tb_ProductCategory)");
            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                strProductId = dsProduct.Tables[0].Rows[0]["ProductID"].ToString();
            }
            else
            {
                strProductId = "0";
            }

        }
        private void headerrotator()
        {

            DataSet dsRorator = new DataSet();
            dsRorator = CommonComponent.GetCommonDataSet("SELECT HeaderText FROM tb_HeaderText WHERE isnull(Active,0)=1 Order By isnull(DisplayOrder,0)");
            if (dsRorator != null && dsRorator.Tables.Count > 0 && dsRorator.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsRorator.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        ltrotator.Text += dsRorator.Tables[0].Rows[i]["HeaderText"].ToString();
                    }
                    ltrotator1.Text += "<div id=\"divheder" + i.ToString() + "\" style=\"display:none;\">" + dsRorator.Tables[0].Rows[i]["HeaderText"].ToString() + "</div>";
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "loadrotator", "$(document).ready(function () {setupRotator();});", true);
            }
        }
        private void GetFreeshipping()
        {
            DataSet dsTopic = new DataSet();
            dsTopic = TopicComponent.GetTopicList("freeshipping", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {

                lttitlefree.Text = Convert.ToString(dsTopic.Tables[0].Rows[0]["Title"].ToString().Replace("<span>", " ").Replace("</span>", " "));
                if (dsTopic.Tables[0].Rows[0]["Description"].ToString() == "")
                {
                    ltfreeshiping.Text = "<p style='padding-left:20px;'>Coming Soon...</p>";
                }
                else
                {
                    ltfreeshiping.Text = dsTopic.Tables[0].Rows[0]["Description"].ToString();
                }


            }
            dsTopic = TopicComponent.GetTopicList("tradepartnerprogram", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {

                lttradetitle.Text = Convert.ToString(dsTopic.Tables[0].Rows[0]["Title"].ToString().Replace("<span>", " ").Replace("</span>", " "));
                if (dsTopic.Tables[0].Rows[0]["Description"].ToString() == "")
                {
                    lttradedescription.Text = "<p style='padding-left:20px;'>Coming Soon...</p>";
                }
                else
                {
                    lttradedescription.Text = dsTopic.Tables[0].Rows[0]["Description"].ToString();
                }


            }
        }
        /// <summary>
        /// Set Product of the Month
        /// </summary>
        private void SetProductyofthemonth()
        {
            //ViewState["ProductOfTheMonth"] = AppLogic.AppConfigs("ProductOfTheMonth").ToString();
            //DataSet ds = CommonComponent.GetCommonDataSet("select * from tb_Product where productid = " + ViewState["ProductOfTheMonth"].ToString() + " and StoreID=" + AppLogic.AppConfigs("StoreID") + "and isnull(Deleted,0)=0 ");
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    ProductofMonth.HRef = "/" + ds.Tables[0].Rows[0]["MainCategory"].ToString() + "/" + ds.Tables[0].Rows[0]["SEName"].ToString() + "-" + ds.Tables[0].Rows[0]["ProductID"].ToString() + ".aspx";
            //}

        }

        /// <summary>
        /// Set Book Mark Method
        /// </summary>
        private void SetBookMark()
        {
            //ltBookMark.Text = "<a title=\"Bookmark This Site\" href=\"javascript:bookmarksite('" + AppLogic.AppConfigs("StoreName") + "','" + AppLogic.AppConfigs("LIVE_SERVER") + "');\">Bookmark This Site</a>";
        }

        /// <summary>
        ///  SubScriber Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSubscriber_Click(object sender, EventArgs e)
        {
            NewsSubscribtionEntity objsubscriber = new NewsSubscribtionEntity();
            //if (txtSubscriber.Text.Trim().Length > 0 && txtNewsZipCode.Text.Trim().Length > 0)
            if (txtSubscriber.Text.Trim().Length > 0)
            {
                Regex re = new Regex(@"^[a-zA-Z0-9][-\+\w\.]*@([a-zA-Z0-9][\w\-]*\.)+[a-zA-Z]{2,4}$");
                if (txtSubscriber.Text.Trim() == "" || txtSubscriber.Text.Trim() == "Enter your E-Mail Address")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgEmail", "alert('Please enter E-Mail Address.');", true);
                    txtSubscriber.Focus();

                }
                else if (!re.IsMatch(txtSubscriber.Text.Trim()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgValidEmail", "alert('Enter valid E-Mail Address.');", true);
                    txtSubscriber.Focus();
                }
                //else if (txtNewsZipCode.Text.Trim() == "" || txtNewsZipCode.Text.Trim() == "Enter Zip")
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgEmail", "alert('Please enter Zip code.');", true);
                //    txtNewsZipCode.Focus();

                //}

                else
                {
                    objsubscriber.Email = txtSubscriber.Text.ToString().Trim();
                    try
                    {
                        NewsSubscribtionComponent obj = new NewsSubscribtionComponent();

                        #region Mail Send For Subscribed Newsletter

                        DataSet DSNews = new DataSet();

                        DSNews = obj.GetNewsSubscribtionList(txtSubscriber.Text.ToString().Trim(), Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));

                        if (DSNews != null && DSNews.Tables.Count > 0 && DSNews.Tables[0].Rows.Count > 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgAlready", "alert('You have been already subscribed.');", true);
                            txtSubscriber.Text = "Enter your E-Mail Address";
                            //txtNewsZipCode.Text = "Enter Zip";
                            return;
                        }
                        else
                        {
                            NewsSubscribtionComponent counS = new NewsSubscribtionComponent();
                            tb_NewsSubscription tb_NewsSubscription = new Bussines.Entities.tb_NewsSubscription();
                            tb_NewsSubscription.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                            tb_NewsSubscription.Email = txtSubscriber.Text;
                            tb_NewsSubscription.CreatedOn = DateTime.Now;
                            Int32 isupdated = counS.CreateNewsSubscription(tb_NewsSubscription);

                            CustomerComponent objCustomer = new CustomerComponent();
                            DataSet dsMailTemplate = new DataSet();
                            dsMailTemplate = objCustomer.GetEmailTamplate("NewsSubscription", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                            {
                                String strBody = "";
                                String strSubject = "";
                                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();
                                strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                                strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###EMAILID###", Server.UrlEncode(Server.HtmlEncode(SecurityComponent.Encrypt(txtSubscriber.Text.Trim()))), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                                CommonOperations.SendMail(txtSubscriber.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgSuccess", "alert('You have been subscribed successfully.');", true);
                                txtSubscriber.Text = "Enter your E-Mail Address";
                                //txtNewsZipCode.Text = "Enter Zip";
                            }

                        #endregion
                        }

                    }
                    catch
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgSorry", "alert('Sorry...unable to send confirmation mail.');", true);
                        txtSubscriber.Text = "Enter your E-Mail Address";
                        // txtNewsZipCode.Text = "Enter Zip";
                    }
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            String strSearch = "Enter Search Text Here";
            if (txtSearch.Text.Trim().Length != 0 && txtSearch.Text.Trim() != strSearch)
                Response.Redirect("Search.aspx?SearchTerm=" + txtSearch.Text.Trim());
            else
            {
                txtSearch.Text = strSearch;
            }

        }


        /// <summary>
        /// Bind Dynamic Header Links
        /// </summary>
        public void BindHeaderLink()
        {

            string filePath = Request.Url.ToString().ToLower();
            litCommonHeader.Text = "";
            System.Text.StringBuilder strWriter = new System.Text.StringBuilder();
            //if (filePath.Contains("/itempage.aspx"))
            //{
            //item.Visible = true;
            index.Visible = false;
            int StoreID = 1;// Convert.ToInt32(AppLogic.AppConfigs("StoreID"));

            #region get parent menu
            Int32 subcategorycount = 1;
            var sQueryparentMenu1 = (from cat in ctxRedtag.tb_Category
                                     join catMap in ctxRedtag.tb_CategoryMapping
                                     on cat.CategoryID equals catMap.CategoryID
                                     where catMap.ParentCategoryID == 0 && cat.Active == true && cat.Deleted == false && cat.ShowOnHeader == true
                                     select new
                                     {
                                         cat.SEName,
                                         cat.Name,
                                         cat.ImageName,
                                         cat.ShortName,
                                         cat.CategoryID,
                                         cat.DisplayOrder,
                                         cat.Active,
                                         cat.tb_Store
                                     }).OrderBy(a => a.DisplayOrder);



            var sQueryparentMenu = (from a in sQueryparentMenu1
                                    where a.tb_Store.StoreID == StoreID
                                    select new
                                    {
                                        a.SEName,
                                        a.Name,
                                        a.ImageName,
                                        a.ShortName,
                                        a.CategoryID,
                                        a.DisplayOrder,
                                        a.Active,
                                    });


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Int32 ilink = 0;
            Int32 pcount = 1;
            Int32 iparent = 1;
            foreach (var parentMenu in sQueryparentMenu)
            {
                #region main parent menu

                if (Session["HeaderCatid"] != null && (Convert.ToInt32(Session["HeaderCatid"]) == Convert.ToInt32(parentMenu.CategoryID)))
                {
                    // sb.Append("<ul id='nav' class='top-level'>");
                    ilink++;
                    sb.Append("<li id=\"menu_link\" class=\"link-" + ilink.ToString() + "\">");
                    if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf(".aspx") <= -1 && Request.RawUrl.ToString() != "/")
                    {
                        // sb.Append("<a class='qmparent sales link-" + ilink + "' title='" + Server.HtmlEncode(parentMenu.Name) + "'  href='/" + parentMenu.SEName.ToString().Replace("amp;", "-").Replace("--", "-") + ".html' >");
                        sb.Append("<a onmouseover='hideshowdiv(" + subcategorycount.ToString() + "," + iparent.ToString() + ");' title='" + Server.HtmlEncode(parentMenu.Name) + "' class=\"selctedatag\"  href='/" + parentMenu.SEName.ToString().Replace("amp;", "-").Replace("--", "-") + ".html' >");
                    }
                    else if (Request.RawUrl != null && (Request.Url.ToString().ToLower().IndexOf("/itempage.aspx") > -1 || Request.Url.ToString().ToLower().IndexOf("/roman-itemPage.aspx") > -1) && (Request.RawUrl.ToString() != "/"))
                    {
                        // sb.Append("<a class='qmparent sales link-" + ilink + "' title='" + Server.HtmlEncode(parentMenu.Name) + "'  href='/" + parentMenu.SEName.ToString().Replace("amp;", "-").Replace("--", "-") + ".html' >");
                        sb.Append("<a onmouseover='hideshowdiv(" + subcategorycount.ToString() + "," + iparent.ToString() + ");' title='" + Server.HtmlEncode(parentMenu.Name) + "' class=\"selctedatag\" href='/" + parentMenu.SEName.ToString().Replace("amp;", "-").Replace("--", "-") + ".html' >");
                    }
                    else if (Request.RawUrl != null && (Request.RawUrl.ToLower().Contains("/productsearchlist.aspx")))
                    {
                        // sb.Append("<a class='qmparent sales link-" + ilink + "' title='" + Server.HtmlEncode(parentMenu.Name) + "' href='/" + parentMenu.SEName + ".html' >");
                        sb.Append("<a onmouseover='hideshowdiv(" + subcategorycount.ToString() + "," + iparent.ToString() + ");' title='" + Server.HtmlEncode(parentMenu.Name) + "' class=\"selctedatag\" href='/" + parentMenu.SEName + ".html' >");
                    }
                    else
                    {
                        // sb.Append("<a class='qmparent link-" + ilink + "' title='" + Server.HtmlEncode(parentMenu.Name) + "' href='/" + parentMenu.SEName + ".html' >");
                        if (parentMenu.SEName.ToString().ToLower() == "customitempage")
                        {
                            if (Request.RawUrl != null && Request.Url.ToString().ToLower().IndexOf("/customitempage.aspx") > -1)
                            {
                                sb.Append("<a onmouseover='hideshowdiv(" + subcategorycount.ToString() + "," + iparent.ToString() + ");' class=\"selctedatag\"  title='" + Server.HtmlEncode(parentMenu.Name) + "' href='/" + parentMenu.SEName + ".aspx' >");
                                
                            }
                            else
                            {
                                sb.Append("<a onmouseover='hideshowdiv(" + subcategorycount.ToString() + "," + iparent.ToString() + ");' title='" + Server.HtmlEncode(parentMenu.Name) + "' href='/" + parentMenu.SEName + ".aspx' >");
                            }

                        }
                        else
                        {
                            sb.Append("<a onmouseover='hideshowdiv(" + subcategorycount.ToString() + "," + iparent.ToString() + ");' title='" + Server.HtmlEncode(parentMenu.Name) + "' href='/" + parentMenu.SEName + ".html' >");
                        }


                        
                    }
                    sb.Append(parentMenu.ShortName);
                    sb.Append("</a>");
                    // sb.Append("</li>");
                }
                else
                {
                    //sb.Append("<ul id='nav' class='top-level'>");
                    ilink++;
                    sb.Append("<li id=\"menu_link\" class=\"link-" + ilink.ToString() + "\">");
                    // sb.Append("<a class='qmparent link-" + ilink + "' href='/" + parentMenu.SEName + ".html' >");
                    if (parentMenu.SEName.ToString().ToLower() == "customitempage")
                    {
                        sb.Append("<a onmouseover='hideshowdiv(" + subcategorycount.ToString() + "," + iparent.ToString() + ");' href='/" + parentMenu.SEName + ".aspx' >");
                    }
                    else
                    {
                        sb.Append("<a onmouseover='hideshowdiv(" + subcategorycount.ToString() + "," + iparent.ToString() + ");' href='/" + parentMenu.SEName + ".html' >");
                    }
                    
                    sb.Append(parentMenu.ShortName);
                    sb.Append("</a>");
                }



                #endregion
                string strsub = "";

                #region bind sub menu of parent menu

                var sQuerySubmenu = (from cat in ctxRedtag.tb_Category
                                     join catmap in ctxRedtag.tb_CategoryMapping
                                         on cat.CategoryID equals catmap.CategoryID
                                     where catmap.ParentCategoryID == parentMenu.CategoryID && cat.Active == true && cat.Deleted == false
                                     select cat).OrderBy(a => a.DisplayOrder);
                if (sQuerySubmenu.Count() > 0)
                {
                    Int32 CategoryCountwidth = sQuerySubmenu.Count();
                    if (CategoryCountwidth >= 4)
                    {
                        CategoryCountwidth = 448;
                    }
                    else
                    {
                        CategoryCountwidth = Convert.ToInt32(sQuerySubmenu.Count()) * 112;
                    }
                    if (pcount >= 6)
                    {
                        //sb.Append(" <ul class=\"menu-thumbs menu-thumbs-1 sub-link-" + pcount + "\" style=\"width:" + CategoryCountwidth.ToString() + "px !important;\">");
                        sb.Append(" <ul class=\"sub-menu\" id=\"menu_detail" + iparent.ToString() + "\">");
                    }
                    else
                    {
                        //sb.Append(" <ul class=\"menu-thumbs menu-thumbs-1\" style=\"width:" + CategoryCountwidth.ToString() + "px !important;\">");
                        sb.Append(" <ul class=\"sub-menu\" id=\"menu_detail" + iparent.ToString() + "\">");
                    }
                    string productlink = "";

                    bool chkmenu = false;
                    foreach (var subMenu in sQuerySubmenu)
                    {
                        if (parentMenu.Name.ToString().ToLower().IndexOf("roman") > -1)
                        {
                            productlink = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 ProductUrl FROM tb_Product WHERE isnull(active,0)=1 and Storeid=1 and isnull(deleted,0)=0 and ProductId in (SELECT ProductId FROM tb_ProductCategory WHERE categoryId =" + subMenu.CategoryID + ")"));
                        }
                        else
                        {
                            productlink = subMenu.SEName + ".html";

                        }
                        if (string.IsNullOrEmpty(productlink))
                        {
                            productlink = "/" + subMenu.SEName + ".html";
                        }
                        else
                        {
                            productlink = "/" + productlink;
                        }
                        if (Session["HeaderSubCatid"] != null && Convert.ToString(Session["HeaderSubCatid"].ToString()) == subMenu.CategoryID.ToString())
                        {
                            //sb.Append("<li><a class='qmactiveSub' href='/" + parentMenu.SEName + "/" + subMenu.SEName + "' title='" + subMenu.Name + "'>");
                            sb.Append("<li id=\"lisub" + subcategorycount.ToString() + "\"><a onmouseover=\"hideshowdiv(" + subcategorycount.ToString() + "," + iparent.ToString() + ");\"  href='" + productlink + "' title='" + subMenu.Name + "'>");
                        }
                        else
                        {
                            //sb.Append("<li><a  href='/" + parentMenu.SEName + "/" + subMenu.SEName + "' title='" + subMenu.Name + "'>");
                            sb.Append("<li id=\"lisub" + subcategorycount.ToString() + "\"><a  onmouseover=\"hideshowdiv(" + subcategorycount.ToString() + "," + iparent.ToString() + ");\" href='" + productlink + "' title='" + subMenu.Name + "'>");
                        }
                        //try
                        //{
                        //    string strmainPath = Convert.ToString(AppLogic.AppConfigs("ImagePathCategory").ToString());
                        //    if (!string.IsNullOrEmpty(subMenu.ImageName.ToString()))
                        //    {
                        //        Random rd = new Random();
                        //        string strimgName = strmainPath + "icon/" + subMenu.ImageName.ToString();
                        //        if (File.Exists(Server.MapPath(strimgName)))
                        //        {
                        //            strimgName = strimgName + "?" + rd.Next(1000).ToString();
                        //            sb.Append("<img height=\"105\" width=\"80\" title=\"" + Server.HtmlEncode(subMenu.Name.ToString()) + "\" alt=\"" + Server.HtmlEncode(subMenu.Name.ToString()) + "\" src=\"" + strimgName.ToString() + "\">");
                        //        }
                        //        else
                        //        {
                        //            // sb.Append("<img height=\"105\" width=\"80\" title=\"\" alt=\"\" src=\"" + strmainPath + "micro/image_not_available.jpg\" />");
                        //        }
                        //    }
                        //    else
                        //    {
                        //        //  sb.Append("<img height=\"105\" width=\"80\" title=\"\" alt=\"\" src=\"" + strmainPath + "micro/image_not_available.jpg\" />");
                        //    }
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                        //sb.Append(subMenu.Name);
                        sb.Append("" + SetNameForHeader(subMenu.Name) + "");
                        if (chkmenu == false)
                        {
                            sb.Append("</a>####menudata####</li>");
                            chkmenu = true;
                        }
                        else
                        {
                            sb.Append("</a></li>");
                        }

                        strsub += "<div class=\"menu-desc\" id=\"divsubcat" + subcategorycount.ToString() + "\" style=\"display:none;\">";
                        strsub += "<div class=\"menu-desc-left\"> <span>" + subMenu.Name.ToString() + "</span>";
                        strsub += "<p>" + System.Text.RegularExpressions.Regex.Replace(subMenu.Description.ToString().Trim(), @"<[^>]*>", String.Empty) + "</p>";
                        strsub += "</div>";
                        strsub += "<div class=\"menu-desc-right\"><a href='" + productlink + "'>";

                        try
                        {
                            string strmainPath = Convert.ToString(AppLogic.AppConfigs("ImagePathCategory").ToString());
                            if (!string.IsNullOrEmpty(subMenu.ImageName.ToString()))
                            {
                                Random rd = new Random();
                                string strimgName = strmainPath + "icon/" + subMenu.ImageName.ToString();
                                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") + strimgName))
                                {

                                    strimgName = strimgName;
                                    strsub += "<img  title=\"" + Server.HtmlEncode(subMenu.Name.ToString()) + "\" alt=\"" + Server.HtmlEncode(subMenu.Name.ToString()) + "\" src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + strimgName.ToString().ToLower() + "\">";
                                }
                                else
                                {
                                    // sb.Append("<img height=\"105\" width=\"80\" title=\"\" alt=\"\" src=\"" + strmainPath + "micro/image_not_available.jpg\" />");
                                }
                            }
                            else
                            {
                                //  sb.Append("<img height=\"105\" width=\"80\" title=\"\" alt=\"\" src=\"" + strmainPath + "micro/image_not_available.jpg\" />");
                            }
                        }
                        catch (Exception ex)
                        {

                        }


                        strsub += "</a></div>";
                        strsub += "</div>";


                        subcategorycount++;
                    }
                    sb.Append("</ul>");
                    sb.Replace("####menudata####", strsub);
                    //sb.Append("</li>");
                }

                sb.Append("</li>");
                //sb.Append("</li>");
                //   sb.Append("</ul>");
                pcount++;
                iparent++;
                #endregion
            }
            //sb.Append("<li>");
            //if (Request.RawUrl != null && Request.RawUrl.Contains("/SalesOutlet.aspx"))
            //{
            //    sb.Append("<li><a class='qmparent sales' style='float:right;' href='/SalesOutlet.aspx'>Sales Outlet</a> ");
            //}
            //else
            //{
            //    sb.Append("<li><a class='qmparent' style='float:right;' href='/SalesOutlet.aspx'>Sales Outlet</a> ");
            //}
            //sb.Append("</li>");
            MenuData = sb.ToString();


            #endregion

            //}
            //else
            //{
            //    item.Visible = false;
            //    index.Visible = true;
            //    DataSet dsHeaderlinks = new DataSet();
            //    dsHeaderlinks = HeaderlinkComponent.GetHeaderLinks("IndexPage", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            //    bool chk = false;
            //    int iCount = 0;
            //    if (dsHeaderlinks != null && dsHeaderlinks.Tables.Count > 0 && dsHeaderlinks.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dsHeaderlinks.Tables[0].Rows)
            //        {
            //            if (iCount % 8 == 0 && iCount != 0)
            //            {
            //                strWriter.Append("</ul>");
            //            }
            //            if (iCount % 8 == 0 || iCount == 0)
            //            {
            //                strWriter.Append("<ul>");
            //                chk = true;
            //            }

            //            iCount++;
            //            if (filePath.Contains(dr["PageURL"].ToString().ToLower()))
            //            {
            //                strWriter.Append("<li ><a class='active' alt='" + dr["PageName"] + "' title='" + dr["PageName"] + "' href='/" + dr["PageURL"].ToString().ToLower() + "'><span>" + dr["PageName"].ToString() + "</span></a></li>");
            //            }
            //            else
            //            {
            //                strWriter.Append("<li ><a  alt='" + dr["PageName"] + "' title='" + dr["PageName"] + "' href='/" + dr["PageURL"].ToString().ToLower() + "'><span>" + dr["PageName"].ToString() + "</span></a></li>");
            //            }
            //        }
            //        dsHeaderlinks.Dispose();
            //        if (chk)
            //        {
            //            strWriter.Append("</ul>");
            //        }
            //    }
            //}
            //litCommonHeader.Text = strWriter.ToString();
        }


        public String SetNameForHeader(String Name)
        {
            //if (Name.Length > 35)
            //    Name = Name.Substring(0, 35) + "...";
            return Name;
        }


        /// <summary>
        ///  Log Out Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lkbLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();
            ltLogin.Visible = true;
            ltregister.InnerHtml = "Register";
            ltregister.HRef = AppLogic.AppConfigs("LIVE_SERVER") + "/CreateAccount.aspx";
            ltregister.Title = "Register";
            lkbLogout.Visible = false;
            Response.Redirect("/Index.aspx", true);
            //Response.Redirect(AppLogic.AppConfigs("LIVE_SERVER"),true);
        }


        /// <summary>
        /// Bind Cart Details By Customer ID
        /// </summary>
        private void BindCartDetails()
        {
            DataSet dsCart = new DataSet();
            if (Session["CustID"] != null && Session["CustID"].ToString() != "")
            {
                dsCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));
                if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
                {
                    int items = Convert.ToInt32(dsCart.Tables[0].Rows[0]["TotalItems"].ToString());
                    //String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC) "));
                    Decimal SwatchQty = 0;
                    //if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
                    //{
                    SwatchQty = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                    //}
                    Decimal GrandSubTotal = Decimal.Zero;
                    for (int i = 0; i < dsCart.Tables[0].Rows.Count; i++)
                    {
                        Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["ProductId"].ToString() + " and ItemType='Swatch'"));
                        if (Isorderswatch == 1)
                        {
                            Decimal pp = 0;
                            //if (Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Discountprice"].ToString()) > Decimal.Zero)
                            //{
                            //    pp = Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Discountprice"].ToString()) * Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Qty"].ToString());
                            //}
                            //else
                            //{
                            pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + dsCart.Tables[0].Rows[i]["Qty"].ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                            //}

                            if (Convert.ToDecimal(pp) >= SwatchQty)
                            {


                                GrandSubTotal += Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) - Convert.ToDecimal(SwatchQty))));

                                SwatchQty = Decimal.Zero;

                            }
                            else
                            {
                                //Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                                if (SwatchQty > Decimal.Zero)
                                {
                                    GrandSubTotal += Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(0)));
                                    SwatchQty = SwatchQty - Convert.ToDecimal(pp);
                                }
                                else
                                {
                                    GrandSubTotal += Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(pp.ToString())));
                                }

                            }
                        }
                        else
                        {
                            GrandSubTotal += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["IndiSubTotal"].ToString());
                        }
                    }


                    decimal Price = Convert.ToDecimal(GrandSubTotal);
                    cartlink.InnerHtml = items > 1 ? "<p><span class='navQty'>(" + items.ToString("D2") + " items)</span><span class='navTotal'> $" + Math.Round(Price, 2) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='Cart' title='Cart' class='cart-icon'>" : "<p><span class='navQty'>(" + items.ToString("D2") + " item)</span><span class='navTotal'> $" + Math.Round(Price, 2) + "</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='' title='' class='cart-icon'>";
                    hiddenTotalItems.Value = items.ToString("D2");
                    if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("checkoutcommon.aspx") > -1)
                    {
                        cartlink.HRef = "/checkoutcommon.aspx";

                    }
                    else
                    {
                        cartlink.HRef = "javascript:void(0);";
                        cartlink.Attributes.Add("onclick", "ShowHideCart();");

                    }
                    Session["NoOfCartItems"] = dsCart.Tables[0].Rows[0]["TotalItems"].ToString();
                }
                else
                {
                    Session["NoOfCartItems"] = null;
                    cartlink.InnerHtml = "<p><span class='navQty'>(0 item)</span><span class='navTotal'> $0.00</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='Cart' title='Cart' class='cart-icon'>";
                    hiddenTotalItems.Value = "0.00";
                    cartlink.HRef = "javascript:void(0);";
                    cartlink.Attributes.Add("onclick", "ShowHideCart();");

                }
            }
            else
            {
                Session["NoOfCartItems"] = null;
                cartlink.InnerHtml = "<p><span class='navQty'>(0 item)</span><span class='navTotal'> $0.00</span></p><img src='/images/cart-icon.jpg' width='70' height='22' alt='Cart' title='Cart' class='cart-icon'>";
                hiddenTotalItems.Value = "0.00";
                cartlink.HRef = "javascript:void(0);";
                cartlink.Attributes.Add("onclick", "ShowHideCart();");


            }
        }


        #region SiteConfigSettings

        /// <summary>
        /// Site Configuration Details from AppConfig
        /// </summary>
        public void HeadTitle(string SiteSETitle, string SiteSEKeywords, string SiteSEDescription)
        {

            string Email = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(ConfigValue,'') from tb_appConfig where ConfigName='DC.Creator.Address' and StoreId=1"));

            Page.Title = SiteSETitle;

            System.Web.UI.HtmlControls.HtmlHead Head = (System.Web.UI.HtmlControls.HtmlHead)Page.Header;
            System.Web.UI.HtmlControls.HtmlMeta HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
            string strMetaDup = "";

            if (Request.Url.ToString().ToLower().IndexOf("subcategory.aspx") > -1)
            {
                string title = "";
                string keywords = "";
                string description = "";
                string ImgName = "";
                string strCatename = "";
                if (Request.QueryString["CatPID"] != null && Request.QueryString["CatPID"].ToString() != "0")
                {
                    strCatename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Name FROM tb_Category WHERE CategoryID=" + Request.QueryString["CatID"].ToString() + ""));
                    string strparentCatename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Name FROM tb_Category WHERE CategoryID=" + Request.QueryString["CatPID"].ToString() + ""));

                    title = Convert.ToString("Browse & Shop " + strCatename + " & Draperies – HalfPriceDrapes");
                    keywords = Convert.ToString("" + strCatename + ", " + strparentCatename + ", window treatments, curtains and drapes, window curtains, window draperies, custom draperies, curtain panels, sale on curtains, curtains and valances");
                    description = Convert.ToString("" + strCatename + ": Shop exclusive sets of " + strCatename + ", " + strparentCatename + " with best prices. Large discounts on " + strCatename + ".");

                    //HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    //HtmlMeta.Name = "Title";
                    //HtmlMeta.Content = Convert.ToString("Browse & Shop " + strCatename + " & Draperies – HalfPriceDrapes");
                    //Head.Controls.Add(HtmlMeta);

                    //HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    //HtmlMeta.Name = "Keywords";
                    //HtmlMeta.Content = Convert.ToString("" + strCatename + ", " + strparentCatename + ", window treatments, curtains and drapes, window curtains, window draperies, custom draperies, curtain panels, sale on curtains, curtains and valances");
                    //Head.Controls.Add(HtmlMeta);

                    //HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    //HtmlMeta.Name = "Description";
                    //HtmlMeta.Content = Convert.ToString("" + strCatename + ": Shop exclusive sets of " + strCatename + ", " + strparentCatename + " with best prices. Large discounts on " + strCatename + ".");
                    //Head.Controls.Add(HtmlMeta);


                }
                else
                {
                    strCatename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Name FROM tb_Category WHERE CategoryID=" + Request.QueryString["CatID"].ToString() + ""));
                    title = Convert.ToString("Savings on Royal " + strCatename.ToString() + " & Drapes for Window Treatment");
                    keywords = Convert.ToString("" + strCatename + " & draperies, window treatments, curtains, draperies, window curtains, window draperies, silk curtains, curtain panels, window hardware & accessories, sale on curtains, curtains and valances");
                    description = Convert.ToString("Browse royal variety of " + strCatename + " for window treatments. Get well designed " + strCatename + " & draperies with attractive prices. Order curtains now.");
                    //HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    //HtmlMeta.Name = "Title";
                    //HtmlMeta.Content = Convert.ToString("Savings on Royal " + strCatename.ToString() + " & Drapes for Window Treatment");
                    //Head.Controls.Add(HtmlMeta);

                    //HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    //HtmlMeta.Name = "Keywords";
                    //HtmlMeta.Content = Convert.ToString("" + strCatename + " & draperies, window treatments, curtains, draperies, window curtains, window draperies, silk curtains, curtain panels, window hardware & accessories, sale on curtains, curtains and valances");
                    //Head.Controls.Add(HtmlMeta);

                    //HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    //HtmlMeta.Name = "Description";
                    //HtmlMeta.Content = Convert.ToString("Browse royal variety of " + strCatename + " for window treatments. Get well designed " + strCatename + " & draperies with attractive prices. Order curtains now.");
                    //Head.Controls.Add(HtmlMeta);


                }

                DataSet dsProduct = new DataSet();
                dsProduct = CommonComponent.GetCommonDataSet("SELECT SETitle,SEKeywords,SEDescription,ImageName FROM tb_Category WHERE CategoryID=" + Request.QueryString["CatID"].ToString() + "");
                string strnewtitle = title;
                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                {
                    ImgName = dsProduct.Tables[0].Rows[0]["ImageName"].ToString();
                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "Title";

                    if (string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["SETitle"].ToString()))
                    {
                        HtmlMeta.Content = title;
                        Page.Title = title;
                        strnewtitle = title;
                    }
                    else
                    {
                        HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                        Page.Title = Convert.ToString(dsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                        strnewtitle = Convert.ToString(dsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                    }

                    Head.Controls.Add(HtmlMeta);

                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "Keywords";
                    if (string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["SEKeywords"].ToString()))
                    {
                        HtmlMeta.Content = keywords;
                    }
                    else
                    {
                        HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SEKeywords"].ToString());
                    }

                    Head.Controls.Add(HtmlMeta);

                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "Description";
                    if (string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["SEDescription"].ToString()))
                    {
                        HtmlMeta.Content = description;
                    }
                    else
                    {
                        HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SEDescription"].ToString());
                    }

                    Head.Controls.Add(HtmlMeta);



                }
                strMeta = "<meta property=\"og:title\" content=\"" + strCatename + "\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:type\" content=\"CATEGORY\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:url\" content=\"http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + Request.RawUrl.ToString() + "\" />" + System.Environment.NewLine;
                string strimagename = AppLogic.AppConfigs("ImagePathCategory");
                strMeta += "<meta property=\"og:image\" content=\"http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "" + strimagename + "icon/" + ImgName.ToString() + "\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:brand\" content=\"HalfPriceDrapes\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:site_name\" content=\"Halfpricedrapes.com\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:description\" content=\"Curtains And Drapes - Its All We Do! Most people assume that high-end luxury in curtains must come at a high price. Not so! Half Price Drapes has been committed to offering our clients the highest quality custom discount curtains and window treatments at the\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:email\" content=\"admin@halfpricedrapes.com\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:phone_number\" content=\"1-866-413-7273\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:latitude\" content=\"37.694633\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:longitude\" content=\"-121.799841\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:street-address\" content=\"1725 Rutan Drive\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:locality\" content=\"Livermore\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:region\" content=\"CA\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:postal-code\" content=\"94551\" />" + System.Environment.NewLine;
                strMeta += "<meta property=\"og:country-name\" content=\"USA\" />";
                Literal lt = new Literal();
                lt.Text = strMeta;
                Head.Controls.Add(lt);



                strMetaDup = "<link rel=\"schema.DC\" href=\"http://purl.org/dc/elements/1.1/ \" />" + System.Environment.NewLine;
                strMetaDup += "<META NAME=\"DC.Creator.Address\" CONTENT=\"" + Email + "\">" + System.Environment.NewLine;
                strMetaDup += "<META NAME=\"DC.Title\" CONTENT=\"" + strnewtitle + "\">" + System.Environment.NewLine;
                strMetaDup += "<META NAME=\"DC.Description\" CONTENT=\"" + HtmlMeta.Content + "\">" + System.Environment.NewLine;
                if (Request.RawUrl != null)
                {
                    strMetaDup += "<META NAME=\"DC.Identifier\" CONTENT=\"http://www.halfpricedrapes.com" + Request.RawUrl.ToString() + "\">" + System.Environment.NewLine;
                }

                strMetaDup += "<META NAME=\"DC.Language\" CONTENT=\"en\">" + System.Environment.NewLine;
                strMetaDup += "<META NAME=\"DC.Date.Created\" CONTENT=\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\">" + System.Environment.NewLine;
                strMetaDup += "<META NAME=\"DC.Type\" CONTENT=\"Half Price Drapes & Curtains\">" + System.Environment.NewLine;
                Literal ltDup = new Literal();
                ltDup.Text = strMetaDup;
                Head.Controls.Add(ltDup);


            }
            else if (Request.Url.ToString().ToLower().IndexOf("category.aspx") > -1 || Request.Url.ToString().ToLower().IndexOf("romancategory.aspx") > -1)
            {
                string strCatename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Name FROM tb_Category WHERE CategoryID=" + Request.QueryString["CatID"].ToString() + ""));
                string title = Convert.ToString("Savings on Royal " + strCatename.ToString() + " & Drapes for Window Treatment");
                string keywords = Convert.ToString("" + strCatename + " & draperies, window treatments, curtains, draperies, window curtains, window draperies, silk curtains, curtain panels, window hardware & accessories, sale on curtains, curtains and valances");
                string description = Convert.ToString("Browse royal variety of " + strCatename + " for window treatments. Get well designed " + strCatename + " & draperies with attractive prices. Order curtains now.");
                string ImgName = "";
                //HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                //HtmlMeta.Name = "Title";
                //HtmlMeta.Content = Convert.ToString("Savings on Royal " + strCatename.ToString() + " & Drapes for Window Treatment");
                //Head.Controls.Add(HtmlMeta);

                //HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                //HtmlMeta.Name = "Keywords";
                //HtmlMeta.Content = Convert.ToString("" + strCatename + " & draperies, window treatments, curtains, draperies, window curtains, window draperies, silk curtains, curtain panels, window hardware & accessories, sale on curtains, curtains and valances");
                //Head.Controls.Add(HtmlMeta);

                //HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                //HtmlMeta.Name = "Description";
                //HtmlMeta.Content = Convert.ToString("Browse royal variety of " + strCatename + " for window treatments. Get well designed " + strCatename + " & draperies with attractive prices. Order curtains now.");
                //Head.Controls.Add(HtmlMeta);

                DataSet dsProduct = new DataSet();
                dsProduct = CommonComponent.GetCommonDataSet("SELECT SETitle,SEKeywords,SEDescription,ImageName FROM tb_Category WHERE CategoryID=" + Request.QueryString["CatID"].ToString() + "");
                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                {
                    ImgName = dsProduct.Tables[0].Rows[0]["ImageName"].ToString();
                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "Title";
                    string strnewtitle = "";
                    if (string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["SETitle"].ToString()))
                    {
                        HtmlMeta.Content = title;
                        Page.Title = title;
                        strnewtitle = title;
                    }
                    else
                    {
                        HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                        Page.Title = Convert.ToString(dsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                        strnewtitle = Convert.ToString(dsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                    }
                    Head.Controls.Add(HtmlMeta);

                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "Keywords";
                    if (string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["SEKeywords"].ToString()))
                    {
                        HtmlMeta.Content = keywords;
                    }
                    else
                    {
                        HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SEKeywords"].ToString());
                    }
                    //HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SEKeywords"].ToString());
                    Head.Controls.Add(HtmlMeta);

                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "Description";
                    if (string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["SEDescription"].ToString()))
                    {
                        HtmlMeta.Content = description;
                    }
                    else
                    {
                        HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SEDescription"].ToString());
                    }
                    //HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SEDescription"].ToString());
                    Head.Controls.Add(HtmlMeta);

                    strMeta = "<meta property=\"og:title\" content=\"" + strCatename + "\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:type\" content=\"CATEGORY\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:url\" content=\"http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + Request.RawUrl.ToString() + "\" />" + System.Environment.NewLine;
                    string strimagename = AppLogic.AppConfigs("ImagePathCategory");
                    strMeta += "<meta property=\"og:image\" content=\"http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "" + strimagename + "icon/" + ImgName.ToString() + "\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:brand\" content=\"HalfPriceDrapes\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:site_name\" content=\"Halfpricedrapes.com\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:description\" content=\"Curtains And Drapes - Its All We Do! Most people assume that high-end luxury in curtains must come at a high price. Not so! Half Price Drapes has been committed to offering our clients the highest quality custom discount curtains and window treatments at the\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:email\" content=\"admin@halfpricedrapes.com\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:phone_number\" content=\"1-866-413-7273\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:latitude\" content=\"37.694633\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:longitude\" content=\"-121.799841\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:street-address\" content=\"1725 Rutan Drive\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:locality\" content=\"Livermore\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:region\" content=\"CA\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:postal-code\" content=\"94551\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:country-name\" content=\"USA\" />";
                    Literal lt = new Literal();
                    lt.Text = strMeta;
                    Head.Controls.Add(lt);


                    strMetaDup = "<link rel=\"schema.DC\" href=\"http://purl.org/dc/elements/1.1/ \" />" + System.Environment.NewLine;
                    strMetaDup += "<META NAME=\"DC.Creator.Address\" CONTENT=\"" + Email + "\">" + System.Environment.NewLine;
                    strMetaDup += "<META NAME=\"DC.Title\" CONTENT=\"" + strnewtitle + "\">" + System.Environment.NewLine;
                    strMetaDup += "<META NAME=\"DC.Description\" CONTENT=\"" + HtmlMeta.Content + "\">" + System.Environment.NewLine;
                    if (Request.RawUrl != null)
                    {
                        strMetaDup += "<META NAME=\"DC.Identifier\" CONTENT=\"http://www.halfpricedrapes.com" + Request.RawUrl.ToString() + "\">" + System.Environment.NewLine;
                    }

                    strMetaDup += "<META NAME=\"DC.Language\" CONTENT=\"en\">" + System.Environment.NewLine;
                    strMetaDup += "<META NAME=\"DC.Date.Created\" CONTENT=\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\">" + System.Environment.NewLine;
                    strMetaDup += "<META NAME=\"DC.Type\" CONTENT=\"Half Price Drapes & Curtains\">" + System.Environment.NewLine;
                    Literal ltDup = new Literal();
                    ltDup.Text = strMetaDup;
                    Head.Controls.Add(ltDup);

                }

            }
            else if (Request.Url.ToString().ToLower().IndexOf("itempage.aspx") > -1 || Request.Url.ToString().ToLower().IndexOf("itempagerating.aspx") > -1 || Request.Url.ToString().ToLower().IndexOf("roman-itempage.aspx") > -1 || Request.Url.ToString().ToLower().IndexOf("itempageratingnew.aspx") > -1 || Request.Url.ToString().ToLower().IndexOf("romanitempagerating.aspx") > -1)
            {
                string ProductName = "";
                string CategoryName = "";
                string ImgName = "";
                string title = "";
                string keywords = "";
                string description = "";
                string priceAmount = "0.00";
                DataSet dsProduct = new DataSet();
                if (Request.QueryString["PID"] != null)
                {

                    string cc = "0";
                    String SelectQuery = "";
                    SelectQuery = " SELECT  top 1 tb_ProductCategory.CategoryID,ParentCategoryID,tb_Product.Name,tb_Product.Imagename,case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end as saleprice FROM tb_ProductCategory " +
                    " INNER JOIN tb_Product ON tb_ProductCategory.ProductID =tb_Product.ProductID  " +
                    " INNER JOIN tb_Category ON tb_ProductCategory.CategoryID = tb_Category.CategoryID  " +
                    " INNER join tb_CategoryMapping On tb_Category.CategoryID= tb_CategoryMapping.CategoryID WHERE  tb_Category.StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " And  tb_Product.StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " And tb_Category.Deleted=0 and  (tb_ProductCategory.ProductID = " + Request.QueryString["PID"] + ") ";

                    DataSet dsCommon = new DataSet();
                    dsCommon = CommonComponent.GetCommonDataSet(SelectQuery);


                    dsProduct = CommonComponent.GetCommonDataSet("SELECT SETitle,SEKeywords,SEDescription,case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end as saleprice,isnull(Imagename,'') as Imagename, Name FROM tb_product WHERE ProductId=" + Request.QueryString["PID"].ToString() + "");

                    if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsCommon.Tables[0].Rows[0]["CategoryID"].ToString()))
                        {

                            cc = dsCommon.Tables[0].Rows[0]["CategoryID"].ToString();
                            CategoryName = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Name FROM tb_Category WHERE CategoryID=" + cc.ToString() + ""));
                            ProductName = dsCommon.Tables[0].Rows[0]["Name"].ToString();
                            ImgName = dsCommon.Tables[0].Rows[0]["Imagename"].ToString();
                            priceAmount = dsCommon.Tables[0].Rows[0]["saleprice"].ToString();
                        }
                    }

                    if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                    {
                        priceAmount = dsProduct.Tables[0].Rows[0]["saleprice"].ToString();
                        ProductName = dsProduct.Tables[0].Rows[0]["Name"].ToString();
                        ImgName = dsProduct.Tables[0].Rows[0]["Imagename"].ToString();
                    }

                    title = "Buy " + ProductName + " & Drapes - HalfPriceDrapes";
                    keywords = ProductName + ", " + CategoryName + ", window treatments, curtains and drapes, window curtains, window draperies, custom draperies, curtain panels, sale on curtains, curtains and valances";
                    description = "Buy " + ProductName + " with discounted prices. Large savings on " + CategoryName + " & drapes for window treatments by halfpricedrapes.com!";


                    strMeta = "<meta property=\"og:title\" content=\"" + ProductName + "\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:type\" content=\"PRODUCT\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:url\" content=\"http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + Request.RawUrl.ToString() + "\" />" + System.Environment.NewLine;
                    string strimagename = AppLogic.AppConfigs("ImagePathProduct");
                    strMeta += "<meta property=\"og:image\" content=\"http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "" + strimagename + "large/" + ImgName.ToString() + "\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:brand\" content=\"HalfPriceDrapes\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:price:amount\" content=\"" + string.Format("{0:0.00}", Convert.ToDecimal(priceAmount)) + "\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:price:currency\" content=\"USD\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:site_name\" content=\"Halfpricedrapes.com\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:description\" content=\"Curtains And Drapes - Its All We Do! Most people assume that high-end luxury in curtains must come at a high price. Not so! Half Price Drapes has been committed to offering our clients the highest quality custom discount curtains and window treatments at the\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:email\" content=\"admin@halfpricedrapes.com\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:phone_number\" content=\"1-866-413-7273\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:latitude\" content=\"37.694633\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:longitude\" content=\"-121.799841\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:street-address\" content=\"1725 Rutan Drive\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:locality\" content=\"Livermore\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:region\" content=\"CA\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:postal-code\" content=\"94551\" />" + System.Environment.NewLine;
                    strMeta += "<meta property=\"og:country-name\" content=\"USA\" />";
                    Literal lt = new Literal();
                    lt.Text = strMeta;
                    Head.Controls.Add(lt);





                }

                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                {
                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "Title";
                    string strnewtitle = "";
                    if (string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["SETitle"].ToString()))
                    {
                        HtmlMeta.Content = title;
                        Page.Title = title;
                        strnewtitle = title;
                    }
                    else
                    {
                        HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                        Page.Title = Convert.ToString(dsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                        strnewtitle = Convert.ToString(dsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                    }
                    //HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                    Head.Controls.Add(HtmlMeta);

                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "Keywords";
                    if (string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["SEKeywords"].ToString()))
                    {
                        HtmlMeta.Content = keywords;
                    }
                    else
                    {
                        HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SEKeywords"].ToString());
                    }
                    //HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SEKeywords"].ToString());
                    Head.Controls.Add(HtmlMeta);

                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "Description";
                    if (string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["SEDescription"].ToString()))
                    {
                        HtmlMeta.Content = description;
                    }
                    else
                    {
                        HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SEDescription"].ToString());
                    }
                    //HtmlMeta.Content = Convert.ToString(dsProduct.Tables[0].Rows[0]["SEDescription"].ToString());
                    Head.Controls.Add(HtmlMeta);


                    strMetaDup = "<link rel=\"schema.DC\" href=\"http://purl.org/dc/elements/1.1/ \" />" + System.Environment.NewLine;
                    strMetaDup += "<META NAME=\"DC.Creator.Address\" CONTENT=\"" + Email + "\">" + System.Environment.NewLine;
                    strMetaDup += "<META NAME=\"DC.Title\" CONTENT=\"" + strnewtitle + "\">" + System.Environment.NewLine;
                    strMetaDup += "<META NAME=\"DC.Description\" CONTENT=\"" + HtmlMeta.Content + "\">" + System.Environment.NewLine;
                    if (Request.RawUrl != null)
                    {
                        strMetaDup += "<META NAME=\"DC.Identifier\" CONTENT=\"http://www.halfpricedrapes.com" + Request.RawUrl.ToString() + "\">" + System.Environment.NewLine;
                    }

                    strMetaDup += "<META NAME=\"DC.Language\" CONTENT=\"en\">" + System.Environment.NewLine;
                    strMetaDup += "<META NAME=\"DC.Date.Created\" CONTENT=\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\">" + System.Environment.NewLine;
                    strMetaDup += "<META NAME=\"DC.Type\" CONTENT=\"Half Price Drapes & Curtains\">" + System.Environment.NewLine;
                    Literal ltDup = new Literal();
                    ltDup.Text = strMetaDup;
                    Head.Controls.Add(ltDup);

                }

            }
            else
            {
                if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/createaccount.aspx") > -1)
                {
                    SiteSETitle = "Create Account/Login - HalfPriceDrapes.com";
                    SiteSEKeywords = "create account";
                    SiteSEDescription = "Create account";
                    Page.Title = SiteSETitle;
                }
                else if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/login.aspx") > -1)
                {
                    SiteSETitle = "Login/Register - HalfPriceDrapes.com";
                    SiteSEKeywords = "Login, Register";
                    SiteSEDescription = "Login/Register - HalfPriceDrapes.com";
                    Page.Title = SiteSETitle;
                }
                else if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/free-swatch.html") > -1)
                {
                    SiteSETitle = "Free Swatches - HalfPriceDrapes.com";
                    SiteSEKeywords = "free swatches, swatches samples";
                    SiteSEDescription = "Free Swatch Samples – to help you elect the perfect window treatments from Half Price Drapes.";
                    Page.Title = SiteSETitle;
                }
                else if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/salesoutlet.aspx") > -1)
                {
                    SiteSETitle = "Sales Outlet - HalfPriceDrapes.com";
                    SiteSEKeywords = "sales outlet";
                    SiteSEDescription = "We have offers our customers some exceptional values on CLEARANCE items. All curtains, drapes, hardware & accessories that are offered are considered Final sales.";
                    Page.Title = SiteSETitle;
                }
                else if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/returnpolicy.html") > -1)
                {
                    SiteSETitle = "Return Policy - HalfPriceDrapes.com";
                    SiteSEKeywords = "return policy";
                    SiteSEDescription = "Free Shipping Back Policy - 100% satisfaction guarantee";
                    Page.Title = SiteSETitle;
                }
                else if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/fabricromancer.html") > -1)
                {
                    SiteSETitle = "Fabric Romancer - HalfPriceDrapes.com";
                    SiteSEKeywords = "fabirc romancer, mark scott";
                    SiteSEDescription = "Mark Scott, The Fabric Romancer";
                    Page.Title = SiteSETitle;
                }
                else if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/showroom.html") > -1)
                {
                    SiteSETitle = "Showroom: Appointment Preferred - HalfPriceDrapes.com";
                    SiteSEKeywords = "curtains showroom, drapes showroom";
                    SiteSEDescription = "Huge selection of quality curtains and drapes in our showroom.";
                    Page.Title = SiteSETitle;
                }
                else if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/ind.html") > -1)
                {
                    SiteSETitle = "SiteMap - HalfPriceDrapes.com";
                    SiteSEKeywords = "designer curtains, drapes, silk curtains, window treatments, home décor, window curtains, window hardware";
                    SiteSEDescription = "The roadmap of all products like draperies, window curtains, silk drapes, designer drapes, taffeta silk curtains, linen curtains & drapes, window hardware, etc.";
                    Page.Title = SiteSETitle;

                }
                else if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/onlineapplicationform.aspx") > -1)
                {
                    SiteSETitle = "Online Application Form – HalfPriceDrapes.com";
                    SiteSEKeywords = "online application form,  trade membership application";
                    SiteSEDescription = " Trade Membership Application Form - HalfPriceDrapes.com";
                    Page.Title = SiteSETitle;

                }
                else if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("/productsearchlist.aspx") > -1)
                {
                    if (Request.RawUrl.ToLower().IndexOf(".html") > -1)
                    {
                        string fullPath = Server.UrlDecode(Request.RawUrl.ToLower());
                        fullPath = fullPath = "/" + fullPath.Trim("/".ToCharArray());
                        string sename = fullPath;

                        sename = sename.Replace("/", "").Replace(".html", "");
                        DataSet dsSearchtype = CommonComponent.GetCommonDataSet("select isnull(SEKeywords,'') as SEKeywords,isnull(SEDescription,'') as SEDescription,isnull(SETitle,'') as SETitle from tb_ProductSearchType where Sename='" + sename.ToString().Trim() + "'");
                        if (dsSearchtype != null && dsSearchtype.Tables.Count > 0 && dsSearchtype.Tables[0].Rows.Count > 0)
                        {
                            SiteSETitle = dsSearchtype.Tables[0].Rows[0]["SETitle"].ToString();
                            SiteSEKeywords = dsSearchtype.Tables[0].Rows[0]["SEKeywords"].ToString();
                            SiteSEDescription = dsSearchtype.Tables[0].Rows[0]["SEDescription"].ToString();
                            Page.Title = SiteSETitle;
                        }


                    }

                }


                HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                HtmlMeta.Name = "Title";
                HtmlMeta.Content = Convert.ToString(SiteSETitle);
                Head.Controls.Add(HtmlMeta);

                HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                HtmlMeta.Name = "Keywords";
                HtmlMeta.Content = Convert.ToString(SiteSEKeywords);
                Head.Controls.Add(HtmlMeta);

                HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                HtmlMeta.Name = "Description";
                HtmlMeta.Content = Convert.ToString(SiteSEDescription);
                Head.Controls.Add(HtmlMeta);

                if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("/login.aspx") > -1 || Request.RawUrl.ToString().ToLower().IndexOf("/checkoutcommon.aspx") > -1 || Request.RawUrl.ToString().ToLower().IndexOf("/customerquotecheckout.aspx") > -1)
                {
                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "googlebot";
                    HtmlMeta.Content = Convert.ToString("noindex");
                    Head.Controls.Add(HtmlMeta);

                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "googlebot-news";
                    HtmlMeta.Content = Convert.ToString("nosnippet");
                    Head.Controls.Add(HtmlMeta);

                }


                strMetaDup = "<link rel=\"schema.DC\" href=\"http://purl.org/dc/elements/1.1/ \" />" + System.Environment.NewLine;
                strMetaDup += "<META NAME=\"DC.Creator.Address\" CONTENT=\"" + Email + "\">" + System.Environment.NewLine;
                strMetaDup += "<META NAME=\"DC.Title\" CONTENT=\"" + SiteSETitle + "\">" + System.Environment.NewLine;
                strMetaDup += "<META NAME=\"DC.Description\" CONTENT=\"" + HtmlMeta.Content + "\">" + System.Environment.NewLine;
                if (Request.RawUrl != null)
                {
                    strMetaDup += "<META NAME=\"DC.Identifier\" CONTENT=\"http://www.halfpricedrapes.com" + Request.RawUrl.ToString() + "\">" + System.Environment.NewLine;
                }

                strMetaDup += "<META NAME=\"DC.Language\" CONTENT=\"en\">" + System.Environment.NewLine;
                strMetaDup += "<META NAME=\"DC.Date.Created\" CONTENT=\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\">" + System.Environment.NewLine;
                strMetaDup += "<META NAME=\"DC.Type\" CONTENT=\"Half Price Drapes & Curtains\">" + System.Environment.NewLine;
                Literal ltDup = new Literal();
                ltDup.Text = strMetaDup;
                Head.Controls.Add(ltDup);


            }
            if (Request.Url.ToString().ToLower().IndexOf("index.aspx") > -1)
            {
                if (!String.IsNullOrEmpty(AppLogic.AppConfigs("GoogleSiteVerification")) && AppLogic.AppConfigs("GoogleSiteVerification") != "")
                {
                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "google-site-verification";
                    HtmlMeta.Content = AppLogic.AppConfigs("GoogleSiteVerification");
                    Head.Controls.Add(HtmlMeta);
                }

                //if (!String.IsNullOrEmpty(AppLogic.AppConfigs("MSValidate.01")) && AppLogic.AppConfigs("MSValidate.01") != "")
                //{
                //    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                //    HtmlMeta.Name = "msvalidate.01";
                //    HtmlMeta.Content = AppLogic.AppConfigs("MSValidate.01");
                //    Head.Controls.Add(HtmlMeta);
                //}

                if (!String.IsNullOrEmpty(AppLogic.AppConfigs("Y_Key")) && AppLogic.AppConfigs("Y_Key") != "")
                {
                    HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
                    HtmlMeta.Name = "y_key";
                    HtmlMeta.Content = AppLogic.AppConfigs("Y_Key");
                    Head.Controls.Add(HtmlMeta);
                }

            }

            HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
            HtmlMeta.Name = "googlebot";
            HtmlMeta.Content = "index, follow";
            Head.Controls.Add(HtmlMeta);

            HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
            HtmlMeta.Name = "revisit-after";
            HtmlMeta.Content = "3 Days";
            Head.Controls.Add(HtmlMeta);

            HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
            HtmlMeta.Name = "robots";
            HtmlMeta.Content = "all";
            Head.Controls.Add(HtmlMeta);

            HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
            HtmlMeta.Name = "robots";
            HtmlMeta.Content = "index, follow";
            Head.Controls.Add(HtmlMeta);

            HtmlMeta = new System.Web.UI.HtmlControls.HtmlMeta();
            HtmlMeta.Name = "author";
            HtmlMeta.Content = AppLogic.AppConfigs("StorePath");
            Head.Controls.Add(HtmlMeta);

        }

        /// <summary>
        /// Displays the Site Configuration Values
        /// </summary>
        private void AddSiteConfig()
        {
            if (Request.Url.ToString().ToLower().IndexOf("subcategory.aspx") > -1)
            {
            }
            if (Request.Url.ToString().ToLower().IndexOf("free-swatch.html") > -1)
            {
            }
            else if (Request.Url.ToString().ToLower().IndexOf("category.aspx") > -1 && Request.Url.ToString().ToLower().IndexOf("romancategory.aspx") <= -1)
            {
            }
            else if (Request.Url.ToString().ToLower().IndexOf("itempage.aspx") > -1)
            {
            }
            else if (Request.Url.ToString().ToLower().IndexOf("staticpage.aspx") > -1)
            {
            }
            else
            {
                HeadTitle(AppLogic.AppConfigs("SiteSETitle"), AppLogic.AppConfigs("SiteSEKeywords"), AppLogic.AppConfigs("SiteSEDescription"));
            }
        }

        /// <summary>
        /// Display  Banners
        /// </summary>
        //private void BindBanner()
        //{
        //    DataTable dtHomeBanner = new DataTable();
        //    DataTable dtHomeBannerDetail = new DataTable();
        //    dtHomeBanner = CommonComponent.GetCommonDataSet("SELECT dbo.tb_RotatorHomebanner.* FROM dbo.tb_RotatorHomebanner INNER JOIN dbo.tb_RotatorHomeBannerDetail ON dbo.tb_RotatorHomebanner.HomeRotatorId = dbo.tb_RotatorHomeBannerDetail.HomeRotatorId WHERE tb_RotatorHomebanner.StoreId=1 and isnull(tb_RotatorHomebanner.IsActive,0)=1").Tables[0];
        //    bool flgchk = false;
        //    bool flgleft = false;
        //    Int32 MainBannerCount = 1;
        //    if (dtHomeBanner != null && dtHomeBanner.Rows.Count > 0)
        //    {
        //        dtHomeBannerDetail = CommonComponent.GetCommonDataSet("SELECT  * FROM tb_RotatorHomeBannerDetail  WHERE HomeRotatorId=" + dtHomeBanner.Rows[0]["HomeRotatorId"].ToString() + " and StoreId=1 and isnull(Active,0)=1 and cast(StartDate as date) <= cast(getdate() as date) and cast(EndDate as date) >= cast(getdate() as date) Order By DisplayOrder").Tables[0];
        //        if (dtHomeBannerDetail != null && dtHomeBannerDetail.Rows.Count > 0)
        //        {
        //            flgchk = true;
        //            hidesmallBanner.InnerHtml = "<div id='small-banner-border'><div class=\"vector-line\"></div><div class=\"small-banner-main\">";
        //            Int32 BanenrTypeId = Convert.ToInt32(dtHomeBanner.Rows[0]["BannerTypeId"].ToString());
        //            Random rd = new Random();
        //            if (BanenrTypeId == 1)
        //            {

        //                ltBanner.Text += "<div class='callbacks_container'>";
        //                ltBanner.Text += "<ul class=\"rslides\" id=\"slider4\">";


        //                for (int i = 0; i < dtHomeBannerDetail.Rows.Count; i++)
        //                {

        //                    if (Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) != "" && Convert.ToString(dtHomeBannerDetail.Rows[i]["StoreID"]) != "")
        //                        ltBanner.Text += "<li><a href=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "'   /></a></li>";
        //                    else ltBanner.Text += "<li><a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"])) + "' /></a></li>";

        //                }
        //                ltBanner.Text += "</ul></div>";




        //                hidesmallBanner.Visible = false;

        //            }
        //            else
        //            {
        //                hidesmallBanner.Visible = true;

        //                ltBanner.Text += "<div class='callbacks_container'>";
        //                ltBanner.Text += "<ul class=\"rslides\" id=\"slider4\">";
        //                bool CheckBanner = false;

        //                Int32 iCount = 1;
        //                for (int i = 0; i < dtHomeBannerDetail.Rows.Count; i++)
        //                {

        //                    if (Convert.ToString(dtHomeBannerDetail.Rows[i]["IsMain"]) == "1")
        //                    {

        //                        if (Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) != "" && Convert.ToString(dtHomeBannerDetail.Rows[i]["StoreID"]) != "")
        //                            ltBanner.Text += "<li ><a href=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "\"><img  src='" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "' /></a></li>";
        //                        else ltBanner.Text += "<li><a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "\"><img  src='" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"])) + "' /></a></li>";
        //                    }
        //                    else
        //                    {
        //                        if (iCount <= 3)
        //                        {



        //                            if (flgleft == false)
        //                            {
        //                                if (Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) != "" && Convert.ToString(dtHomeBannerDetail.Rows[i]["StoreID"]) != "")
        //                                {
        //                                    hidesmallBanner.InnerHtml += "<div class=\"small-banner-new-" + iCount.ToString() + "\"><a href=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></a></div>";
        //                                }
        //                                else
        //                                {
        //                                    hidesmallBanner.InnerHtml += "<div class=\"small-banner-new-" + iCount.ToString() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></div>";
        //                                }
        //                                flgleft = true;
        //                                iCount++;
        //                            }
        //                            else
        //                            {
        //                                if (Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) != "" && Convert.ToString(dtHomeBannerDetail.Rows[i]["StoreID"]) != "")
        //                                {
        //                                    hidesmallBanner.InnerHtml += "<div class=\"small-banner-new-" + iCount.ToString() + "\"><a href=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></a></div>";
        //                                }
        //                                else
        //                                {
        //                                    hidesmallBanner.InnerHtml += "<div class=\"small-banner-new-" + iCount.ToString() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></div>";
        //                                }
        //                                iCount++;
        //                            }
        //                        }
        //                    }
        //                }


        //                ltBanner.Text += "</ul></div>";



        //            }

        //            hidesmallBanner.InnerHtml += "</div></div>";
        //        }
        //    }
        //    if (flgchk == false)
        //    {
        //        dtHomeBanner = HomeBannerComponent.GetHomeBanner(1).Tables[0]; //HomeBannerComponent.GetHomeBanner(Convert.ToInt32(AppLogic.AppConfigs("StoreID"))).Tables[0];

        //        if (dtHomeBanner.Rows.Count == 1)
        //        {

        //            ltBanner.Text += ("<div id='slider1' class='contentslide'><div class='contentdiv'><div class='index-banner-1'><div class='banner1'><img style='cursor:pointer;'  src='" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "home/" + dtHomeBanner.Rows[0]["BannerID"].ToString() + ".jpg' alt='" + Convert.ToString(dtHomeBanner.Rows[0]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dtHomeBanner.Rows[0]["Title"]).Trim()) + "'  onclick=\"javascript:location.href='" + Convert.ToString(dtHomeBanner.Rows[0]["BannerURL"]) + "';\" /></div></div></div></div>");
        //        }

        //    }



        //}
        private void BindBanner()
        {
            DataTable dtHomeBanner = new DataTable();
            DataTable dtHomeBannerDetail = new DataTable();
            dtHomeBanner = CommonComponent.GetCommonDataSet("SELECT dbo.tb_RotatorHomebanner.* FROM dbo.tb_RotatorHomebanner INNER JOIN dbo.tb_RotatorHomeBannerDetail ON dbo.tb_RotatorHomebanner.HomeRotatorId = dbo.tb_RotatorHomeBannerDetail.HomeRotatorId WHERE tb_RotatorHomebanner.StoreId=1 and isnull(tb_RotatorHomebanner.IsActive,0)=1").Tables[0];
            bool flgchk = false;
            bool flgleft = false;
            Int32 MainBannerCount = 1;
            if (dtHomeBanner != null && dtHomeBanner.Rows.Count > 0)
            {
                dtHomeBannerDetail = CommonComponent.GetCommonDataSet("SELECT  * FROM tb_RotatorHomeBannerDetail  WHERE HomeRotatorId=" + dtHomeBanner.Rows[0]["HomeRotatorId"].ToString() + " and StoreId=1 and isnull(Active,0)=1 and cast(StartDate as date) <= cast(getdate() as date) and cast(EndDate as date) >= cast(getdate() as date) Order By DisplayOrder").Tables[0];
                if (dtHomeBannerDetail != null && dtHomeBannerDetail.Rows.Count > 0)
                {
                    flgchk = true;
                    hidesmallBanner.InnerHtml = "<div id='small-banner-border'><div class=\"vector-line\"></div><div class=\"index-new-banner-main\">";
                    Int32 BanenrTypeId = Convert.ToInt32(dtHomeBanner.Rows[0]["BannerTypeId"].ToString());
                    Random rd = new Random();
                    if (BanenrTypeId == 1)
                    {

                        ltBanner.Text += "<div class='callbacks_container'>";
                        ltBanner.Text += "<ul class=\"rslides\" id=\"slider4\">";


                        for (int i = 0; i < dtHomeBannerDetail.Rows.Count; i++)
                        {

                            if (Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) != "" && Convert.ToString(dtHomeBannerDetail.Rows[i]["StoreID"]) != "")
                                ltBanner.Text += "<li><a href=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner") + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "'   /></a></li>";
                            else ltBanner.Text += "<li><a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner") + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"])) + "' /></a></li>";

                        }
                        ltBanner.Text += "</ul></div>";




                        hidesmallBanner.Visible = false;

                    }
                    else
                    {
                        hidesmallBanner.Visible = true;

                        ltBanner.Text += "<div class='callbacks_container'>";
                        ltBanner.Text += "<ul class=\"rslides\" id=\"slider4\">";
                        bool CheckBanner = false;

                        Int32 iCount = 1;
                        for (int i = 0; i < dtHomeBannerDetail.Rows.Count; i++)
                        {

                            if (Convert.ToString(dtHomeBannerDetail.Rows[i]["IsMain"]) == "1")
                            {

                                if (Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) != "" && Convert.ToString(dtHomeBannerDetail.Rows[i]["StoreID"]) != "")
                                    ltBanner.Text += "<li ><a href=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "\"><img  src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner") + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "' /></a></li>";
                                else ltBanner.Text += "<li><a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim()) + "\"><img  src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner") + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"])) + "' /></a></li>";
                            }
                            else
                            {
                                if (iCount <= 2)
                                {



                                    if (flgleft == false)
                                    {
                                        if (Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) != "" && Convert.ToString(dtHomeBannerDetail.Rows[i]["StoreID"]) != "")
                                        {
                                            hidesmallBanner.InnerHtml += "<div class=\"index-new-banner-new-" + iCount.ToString() + "\"><a href=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner") + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></a></div>";
                                        }
                                        else
                                        {
                                            hidesmallBanner.InnerHtml += "<div class=\"index-new-banner-new-" + iCount.ToString() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner") + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></div>";
                                        }
                                        flgleft = true;
                                        iCount++;
                                    }
                                    else
                                    {
                                        if (Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) != "" && Convert.ToString(dtHomeBannerDetail.Rows[i]["StoreID"]) != "")
                                        {
                                            hidesmallBanner.InnerHtml += "<div class=\"index-new-banner-new-" + iCount.ToString() + "\"><a href=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner") + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></a></div>";
                                        }
                                        else
                                        {
                                            hidesmallBanner.InnerHtml += "<div class=\"index-new-banner-new-" + iCount.ToString() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner") + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></div>";
                                        }
                                        iCount++;
                                    }


                                    if (iCount == 3)
                                    {
                                        DataSet dsBottomBanner = new DataSet();
                                        dsBottomBanner = CommonComponent.GetCommonDataSet("select top 1 * from tb_HomeBottomBannerDetail where isnull(Active,0)=1 and storeid=1 and cast(StartDate as date) <= cast(getdate() as date) and cast(EndDate as date) >= cast(getdate() as date)  order by DisplayOrder");
                                        if (dsBottomBanner != null && dsBottomBanner.Tables.Count > 0 && dsBottomBanner.Tables[0].Rows.Count > 0)
                                        {
                                            rd = new Random();
                                            for (int k = 0; k < dsBottomBanner.Tables[0].Rows.Count; k++)
                                            {

                                                if (Convert.ToString(dsBottomBanner.Tables[0].Rows[k]["BannerURL"]) != "" && Convert.ToString(dsBottomBanner.Tables[0].Rows[k]["StoreID"]) != "")
                                                {
                                                    hidesmallBanner.InnerHtml += "<div class=\"index-new-banner-new-" + iCount.ToString() + "\"><a href=\"" + Convert.ToString(dsBottomBanner.Tables[0].Rows[k]["BannerURL"]).Trim() + "\" title=\"" + Convert.ToString(dsBottomBanner.Tables[0].Rows[k]["Title"]).Trim() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBottomBanner") + "" + dsBottomBanner.Tables[0].Rows[k]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dsBottomBanner.Tables[0].Rows[k]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dsBottomBanner.Tables[0].Rows[k]["Title"]).Trim() + "\"></a></div>";
                                                }
                                                else
                                                {
                                                    hidesmallBanner.InnerHtml += "<div class=\"index-new-banner-new-" + iCount.ToString() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBottomBanner") + "" + dsBottomBanner.Tables[0].Rows[k]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dsBottomBanner.Tables[0].Rows[k]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dsBottomBanner.Tables[0].Rows[k]["Title"]).Trim() + "\"></div>";
                                                }

                                            }


                                        }

                                    }

                                }
                            }
                        }


                        ltBanner.Text += "</ul></div>";



                    }

                    hidesmallBanner.InnerHtml += "</div></div>";
                }
            }
            if (flgchk == false)
            {
                dtHomeBanner = HomeBannerComponent.GetHomeBanner(1).Tables[0]; //HomeBannerComponent.GetHomeBanner(Convert.ToInt32(AppLogic.AppConfigs("StoreID"))).Tables[0];

                if (dtHomeBanner.Rows.Count == 1)
                {

                    ltBanner.Text += ("<div id='slider1' class='contentslide'><div class='contentdiv'><div class='index-banner-1'><div class='banner1'><img style='cursor:pointer;'  src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner") + "Home/" + dtHomeBanner.Rows[0]["BannerID"].ToString() + ".jpg' alt='" + Convert.ToString(dtHomeBanner.Rows[0]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dtHomeBanner.Rows[0]["Title"]).Trim()) + "'  onclick=\"javascript:location.href='" + Convert.ToString(dtHomeBanner.Rows[0]["BannerURL"]) + "';\" /></div></div></div></div>");
                }

            }



        }

        #endregion

        /// <summary>
        /// Loads the Testimonial for Display
        /// </summary>
        private void LoadTestimonial()
        {

        }

        /// <summary>
        /// Add '...', if String length is more than 200 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 200 Length String </returns>
        public String SetName(String Name)
        {
            if (Name.Length > 200)
                Name = Name.Substring(0, 195) + "...";
            return Name;
        }

        /// <summary>
        /// Binds  Right Side Banner
        /// </summary>
        private void BindRightBanner()
        {
            ltrightBanner.Text = "";
            DataSet dsRightBanner = CommonComponent.GetCommonDataSet("select top 1 * from tb_HomeRightBannerDetail where isnull(Active,0)=1 and storeid=1 order by DisplayOrder");
            if (dsRightBanner != null && dsRightBanner.Tables.Count > 0 && dsRightBanner.Tables[0].Rows.Count > 0)
            {
                Random rd = new Random();
                for (int i = 0; i < dsRightBanner.Tables[0].Rows.Count; i++)
                {
                    //if (i == 0)
                    //{
                    //    ltrightBanner.Text += "<div class='banner-right-pt1'>";
                    //    if (Convert.ToString(dsRightBanner.Tables[0].Rows[i]["BannerURL"]) != "" && Convert.ToString(dsRightBanner.Tables[0].Rows[i]["StoreID"]) != "")
                    //        ltrightBanner.Text += "<a href=\"" + Convert.ToString(dsRightBanner.Tables[0].Rows[i]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim()) + "\"><img  src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathRightBanner") + "" + dsRightBanner.Tables[0].Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim()) + "' /></a>";
                    //    else
                    //        ltrightBanner.Text += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim()) + "\"><img  src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathRightBanner") + "" + dsRightBanner.Tables[0].Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim()) + "' /></a>";
                    //    ltrightBanner.Text += "</div>";
                    //}
                    //else
                    //{
                    // ltrightBanner.Text += "<div class='banner-right-pt2'>";
                    if (Convert.ToString(dsRightBanner.Tables[0].Rows[i]["BannerURL"]) != "" && Convert.ToString(dsRightBanner.Tables[0].Rows[i]["StoreID"]) != "")
                        ltrightBanner.Text += "<a href=\"" + Convert.ToString(dsRightBanner.Tables[0].Rows[i]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim()) + "\"><img  src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathRightBanner") + "" + dsRightBanner.Tables[0].Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim()) + "' /></a>";
                    else
                        ltrightBanner.Text += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim()) + "\"><img  src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathRightBanner") + "" + dsRightBanner.Tables[0].Rows[i]["ImageName"].ToString() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim() + "' Title='" + Server.HtmlEncode(Convert.ToString(dsRightBanner.Tables[0].Rows[i]["Title"]).Trim()) + "' /></a>";
                    // ltrightBanner.Text += "</div>";

                    //}

                }
            }

            //Int32 ImgID = Convert.ToInt32(AppLogic.AppConfigs("HotDealProduct").ToString());
            //string stritemurl = Convert.ToString(CommonComponent.GetScalarCommonData(""));
            //dvBannerright.InnerHtml = "<a href=\"" + stritemurl + "-" + ImgID.ToString() + ".aspx\"><img height='34' width='210' class='img-right' src=\"" + AppLogic.AppConfigs("ImagePathBanner") + "HotDeal/" + ImgID + ".jpg\" /></a>";
        }

        /// <summary>
        /// Binds the New Arrival Products
        /// </summary>
        private void BindNewArrival()
        {

            DataSet dsNewArrival = CommonComponent.GetCommonDataSet("select top 5 ProductID,Name,isnull(Price,0) as Price,isnull(SalePrice,0) as SalePrice, isnull(tb_Product.Description,'''') as Description, " +
                                                             " ImageName,isnull(DisplayOrder,0) as DisplayOrder,SKU, MainCategory, SEName,TagName,case when isnull(tb_Product.Tooltip,'''')='''' then tb_Product.Name else tb_Product.Tooltip end as Tooltip ,  " +
                                                             " isnull(tb_Product.Inventory,0) as Inventory,isnull(IsFreeEngraving,0) as IsFreeEngraving   from tb_Product   " +
                                                             " where isnull(Active,0)=1 and StoreID=" + AppLogic.AppConfigs("StoreID") + "and isnull(Deleted,0)=0 and  isnull(IsNewArrival,0)=1 and tb_Product.productid not in(select tb_Giftcardproduct.productid from tb_Giftcardproduct)");
            if (dsNewArrival != null && dsNewArrival.Tables.Count > 0 && dsNewArrival.Tables[0].Rows.Count > 0)
            {

                rptNewArrival.DataSource = dsNewArrival;
                rptNewArrival.DataBind();
            }

        }

        /// <summary>
        /// Replace the '"' and '\' which is comes in ProductName to "-"
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>return the ProductName with Replace the '"' and '\' which is comes in ProductName to "-" </returns>
        public String SetAttribute(String Name)
        {
            return Name.Replace('"', '-').Replace('\'', '-').ToString();
        }

        /// <summary>
        /// Add '...', if String length is more than 40 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 40 Length String </returns>
        public String SetNamenewarrival(String Name)
        {
            if (Name.Length > 40)
                Name = Name.Substring(0, 35) + "...";
            return Server.HtmlEncode(Name);
        }

        /// <summary>
        /// Get Icon Image for Category
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Image With Full Path</returns>
        public String GetIconImageCategory(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathCategory") + "Icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }

            return string.Concat(AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
        }

        /// <summary>
        /// Get Icon Image for Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Image With Full Path</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }

        private void GetHomeContent()
        {
            DataTable dtHomeContent = new DataTable();
            DataSet ds = new DataSet();

            ds = TopicComponent.GetTopicAccordingStoreID("Half Price <span>Drapes</span>", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltrhomecontent.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                ltrhomecontenthead.Text = ds.Tables[0].Rows[0]["Topicname"].ToString();
            }
            else
            {
                ltrhomecontent.Text = "";
                ltrhomecontenthead.Text = "";
            }
        }

        private void GetCustomerService()
        {
            DataTable dtHomeContent = new DataTable();
            DataSet ds = new DataSet();

            ds = TopicComponent.GetTopicAccordingStoreID("customerservice", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltrCusromerService.Text = ds.Tables[0].Rows[0]["Description"].ToString();
            }
            else
            {
                ltrCusromerService.Text = "";
            }
        }
        /// <summary>
        /// Initializes the <see cref="T:System.Web.UI.HtmlTextWriter" /> object and calls on the child controls of the <see cref="T:System.Web.UI.Page" /> to render.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> that receives the page content.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                System.IO.StringWriter stringWriter = new System.IO.StringWriter(stringBuilder);
                System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(stringWriter);
                base.Render(htmlWriter);
                string yourHtml = stringBuilder.ToString();//.Replace(stringBuilder.ToString().IndexOf("<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=") + ,""); // ***** Parse and Modify This *****
                yourHtml = yourHtml.Replace("</a> > <a", "</a><img class=\"breadcrumbs-bullet-icon\" title=\"\" alt=\"\" src=\"/images/spacer.png\"/><a");
                yourHtml = yourHtml.Replace("</a> > <span", "</a><img class=\"breadcrumbs-bullet-icon\" title=\"\" alt=\"\" src=\"/images/spacer.png\" /><span");

                yourHtml = yourHtml.Replace("</a>> <span", "</a><img class=\"breadcrumbs-bullet-icon\" title=\"\" alt=\"\" src=\"/images/spacer.png\" /><span");
                yourHtml = yourHtml.Replace("<link id=\"canonical\"", "<link");
                if (AppLogic.AppConfigBool("UseSSL"))
                {
                    if (Request.RawUrl != null && (Request.RawUrl.ToString().ToLower().IndexOf("/login.aspx") > -1 || Request.RawUrl.ToString().ToLower().IndexOf("/checkoutcommon.aspx") > -1 || Request.RawUrl.ToString().ToLower().IndexOf("/addtocart.aspx") > -1 || Request.RawUrl.ToString().ToLower().IndexOf("/customerquotecheckout.aspx") > -1 || Request.RawUrl.ToString().ToLower().IndexOf("/myaccount.aspx") > -1))
                    {
                        yourHtml = yourHtml.Replace("http://www.halfpricedrapes.us/Resources", "https://www.halfpricedrapes.us/Resources");
                        yourHtml = yourHtml.Replace("var klevu_apiKey", "//var klevu_apiKey");
                        yourHtml = yourHtml.Replace("http://box.klevu.com/klevu-js-v1/js/klevu-webstore.js", "");

                    }


                }
                if (Request.RawUrl != null && Request.RawUrl.ToString() != "/" && Request.RawUrl.ToString().ToLower().IndexOf("/index.aspx") <= -1)
                {
                    yourHtml = yourHtml.Replace("src=\"/js/jquery-homebanner.js?444\"", "");
                    yourHtml = yourHtml.Replace("src=\"/js/homebannerslider.js?333\"", "");
                    yourHtml = yourHtml.Replace("var $k = jQuery.noConflict();", "//var $k = jQuery.noConflict();");
                }
                if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("/itempage.aspx") > -1)
                {
                    yourHtml = yourHtml.Replace("forcesecure=1\" style=\"width: 1px; height: 1px;", "forcesecure=1\" style=\"width: 1px; height: 1px; left:0;");

                }

                writer.Write(yourHtml);
            }
            catch
            {
            }
        }
        //private void BindHeader()
        //{
        //    string StrPattern = "";
        //    DataSet dsPattern = new DataSet();
        //    dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 5 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
        //    StrPattern = "<div class=\"toggle1\">";
        //    StrPattern += "<ul id=\"mycarousel7\" class=\"jcarousel-skin-tango2\">";
        //    if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
        //    {
        //        Int32 icheck = 0;
        //        for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
        //        {
        //            string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
        //            string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
        //            string Imagename = Convert.ToString(dsPattern.Tables[0].Rows[i]["Imagename"]);

        //            string strImageName = "";
        //            String SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages");
        //            if (!Directory.Exists(Server.MapPath(SearchProductPath)))
        //                Directory.CreateDirectory(Server.MapPath(SearchProductPath));
        //            string strFilePath = Server.MapPath(SearchProductPath + Imagename);
        //            Random rnd = new Random();
        //            if (File.Exists(strFilePath))
        //            {
        //                strImageName = SearchProductPath + Imagename + "?" + rnd.Next(10000);
        //            }
        //            else
        //            {
        //                dsPattern.Tables[0].Rows.RemoveAt(i);
        //                i--;
        //                dsPattern.Tables[0].AcceptChanges();
        //                continue;
        //            }
        //            if (icheck == 0)
        //            {
        //                StrPattern += "<li><ul class=\"option-pro\">";
        //            }
        //            if (icheck % 3 == 0 && icheck > 2)
        //            {
        //                if (dsPattern.Tables[0].Rows.Count - 1 > i)
        //                {
        //                    StrPattern += "</ul></li><li><ul class=\"option-pro\">";
        //                }
        //            }
        //            icheck++;
        //            StrPattern += "<li class=\"header-pro-box\">";
        //            StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkHeader_" + SearchId + "\">";
        //            StrPattern += "<img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString() + "\"><span style=\"padding-left: 16px;\">" + SearchValue.ToString() + "</span></li>";
        //        }
        //        StrPattern += "</ul></li>";
        //    }
        //    StrPattern += "</ul>";
        //    StrPattern += "</div>";
        //    ltrHeader.Text = StrPattern.ToString();
        //}

        //private void BindPattern()
        //{
        //    string StrPattern = "";
        //    DataSet dsPattern = new DataSet();
        //    dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType=2 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 Order by ISNULL(DisplayOrder,999)");
        //    StrPattern = "<div class=\"toggle1\">";
        //    StrPattern += "<ul id=\"mycarousel3\" class=\"jcarousel-skin-tango2\">";
        //    if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                StrPattern += "<li><ul class=\"option-pro\">";
        //            }
        //            if (i % 8 == 0 && i > 7)
        //            {
        //                if (dsPattern.Tables[0].Rows.Count - 1 > i)
        //                {
        //                    StrPattern += "</ul></li><li><ul class=\"option-pro\">";
        //                }
        //            }

        //            string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
        //            string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
        //            StrPattern += "<li class=\"pattern-pro-box\">";
        //            StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkPattern_" + SearchId + "\">";
        //            StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
        //        }
        //        StrPattern += "</ul></li>";
        //    }
        //    StrPattern += "</ul>";
        //    StrPattern += "</div>";
        //    ltrPattern.Text = StrPattern.ToString();
        //}

        //private void BindFabric()
        //{
        //    string StrPattern = "";
        //    DataSet dsPattern = new DataSet();
        //    dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 3 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
        //    StrPattern = "<div class=\"toggle1\">";
        //    StrPattern += "<ul id=\"mycarousel4\" class=\"jcarousel-skin-tango2\">";
        //    if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                StrPattern += "<li><ul class=\"option-pro\">";
        //            }
        //            if (i % 8 == 0 && i > 7)
        //            {
        //                if (dsPattern.Tables[0].Rows.Count - 1 > i)
        //                {
        //                    StrPattern += "</ul></li><li><ul class=\"option-pro\">";
        //                }
        //            }

        //            string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
        //            string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
        //            StrPattern += "<li class=\"pattern-pro-box\">";
        //            StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkFabric_" + SearchId + "\">";
        //            StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
        //        }
        //        StrPattern += "</ul></li>";
        //    }
        //    StrPattern += "</ul>";
        //    StrPattern += "</div>";
        //    ltrFabric.Text = StrPattern.ToString();
        //}

        //private void BindStyle()
        //{
        //    string StrPattern = "";
        //    DataSet dsPattern = new DataSet();
        //    dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 4 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
        //    StrPattern = "<div class=\"toggle1\">";
        //    StrPattern += "<ul id=\"mycarousel5\" class=\"jcarousel-skin-tango2\">";
        //    if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                StrPattern += "<li><ul class=\"option-pro\">";
        //            }
        //            if (i % 8 == 0 && i > 7)
        //            {
        //                if (dsPattern.Tables[0].Rows.Count - 1 > i)
        //                {
        //                    StrPattern += "</ul></li><li><ul class=\"option-pro\">";
        //                }
        //            }

        //            string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
        //            string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
        //            StrPattern += "<li class=\"pattern-pro-box\">";
        //            StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkStyle_" + SearchId + "\">";
        //            StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
        //        }
        //        StrPattern += "</ul></li>";
        //    }
        //    StrPattern += "</ul>";
        //    StrPattern += "</div>";
        //    ltrStyle.Text = StrPattern.ToString();
        //}

        //private void BindColors()
        //{
        //    string StrPattern = "";
        //    DataSet dsPattern = new DataSet();
        //    dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType =1 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
        //    StrPattern = "<div class=\"toggle1\">";
        //    StrPattern += "<ul id=\"mycarousel2\" class=\"jcarousel-skin-tango0\">";
        //    if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
        //    {
        //        Int32 icheck = 0;
        //        for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
        //        {
        //            string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
        //            string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
        //            string Imagename = Convert.ToString(dsPattern.Tables[0].Rows[i]["Imagename"]);

        //            string strImageName = "";
        //            String SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages");
        //            if (!Directory.Exists(Server.MapPath(SearchProductPath)))
        //                Directory.CreateDirectory(Server.MapPath(SearchProductPath));
        //            string strFilePath = Server.MapPath(SearchProductPath + Imagename);
        //            Random rnd = new Random();
        //            if (File.Exists(strFilePath))
        //            {
        //                strImageName = SearchProductPath + Imagename + "?" + rnd.Next(10000);
        //            }
        //            else
        //            {
        //                dsPattern.Tables[0].Rows.RemoveAt(i);
        //                i--;
        //                dsPattern.Tables[0].AcceptChanges();
        //                continue;
        //            }
        //            if (icheck == 0)
        //            {
        //                StrPattern += "<li><ul class=\"option-pro-color\" style=\"width:92px !important;\">";
        //            }
        //            if (icheck % 10 == 0 && icheck > 9)
        //            {
        //                if (dsPattern.Tables[0].Rows.Count - 1 > i)
        //                {
        //                    StrPattern += "</ul></li><li><ul class=\"option-pro-color\" style=\"width:92px !important;\">";
        //                }
        //            }

        //            icheck++;
        //            StrPattern += "<li class=\"option-pro-box\"  style=\"padding-bottom:4px !important;\">";
        //            StrPattern += "<a href=\"javascript:void(0);\" onclick=\"ColorSelection('" + SearchValue.ToString() + "');\"><img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString() + "\"></a> </li>";
        //        }
        //        StrPattern += "</ul></li>";
        //    }
        //    StrPattern += "</ul>";
        //    StrPattern += "</div>";
        //    ltrColor.Text = StrPattern.ToString();
        //}

        //protected void btnIndexPriceGo_Click(object sender, ImageClickEventArgs e)
        //{
        //    Session["IndexPriceValue"] = null;
        //    Session["IndexFabricValue"] = null;
        //    Session["IndexPatternValue"] = null;
        //    Session["IndexStyleValue"] = null;
        //    Session["IndexColorValue"] = null;
        //    Session["IndexHeaderValue"] = null;
        //    Session["IndexCustomValue"] = null;

        //    if (!string.IsNullOrEmpty(txtFrom.Text.ToString().Trim()) || !string.IsNullOrEmpty(txtTo.Text.ToString().Trim()))
        //    {
        //        if (string.IsNullOrEmpty(txtFrom.Text.ToString().Trim()))
        //        {
        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Please Enter Valid Price.');document.getElementById('ContentPlaceHolder1_txtFrom').focus();", true);
        //            return;
        //        }
        //        if (string.IsNullOrEmpty(txtTo.Text.ToString().Trim()))
        //        {
        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Tovalcal", "alert('Please Enter Valid Price.');document.getElementById('ContentPlaceHolder1_txtTo').focus();", true);
        //            return;
        //        }
        //        decimal FromVal = Convert.ToDecimal(txtFrom.Text.Trim());
        //        decimal ToVal = Convert.ToDecimal(txtTo.Text.Trim());
        //        if (FromVal > ToVal)
        //        {
        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Low Price should be Less than High Price.');document.getElementById('ContentPlaceHolder1_txtFrom').focus();", true);
        //            return;
        //        }
        //        Session["IndexPriceValue"] = FromVal.ToString() + "-" + ToVal.ToString();
        //    }

        //    string[] strkey = Request.Form.AllKeys;

        //    foreach (string strkeynew in strkey)
        //    {
        //        if (strkeynew.ToString().ToLower().IndexOf("chkprice") > -1 && Session["IndexPriceValue"] == null)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexPriceValue"] += ChkValue[0];
        //        }
        //        if (strkeynew.ToString().ToLower().IndexOf("chkpattern") > -1)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexPatternValue"] += ChkValue[0] + ",";
        //        }
        //        if (strkeynew.ToString().ToLower().IndexOf("chkfabric") > -1)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexFabricValue"] += ChkValue[0] + ",";
        //        }
        //        if (strkeynew.ToString().ToLower().IndexOf("chkstyle") > -1)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexStyleValue"] += ChkValue[0] + ",";
        //        }
        //        if (strkeynew.ToString().ToLower().IndexOf("chkheader") > -1)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexHeaderValue"] += ChkValue[0] + ",";
        //        }
        //        if (strkeynew.ToString().ToLower().IndexOf("chkcustom") > -1)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexCustomValue"] += ChkValue[0].ToString();
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(hdnColorSelection.Value) && hdnColorSelection.Value != "")
        //    {
        //        Session["IndexColorValue"] = hdnColorSelection.Value.ToString();
        //    }
        //    hdnColorSelection.Value = "";

        //    if (Session["IndexPriceValue"] == null && Session["IndexFabricValue"] == null && Session["IndexPatternValue"] == null && Session["IndexStyleValue"] == null && Session["IndexColorValue"] == null && Session["IndexHeaderValue"] == null && Session["IndexCustomValue"] == null)
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Please Select Search Criteria.');", true);
        //        return;
        //    }
        //    else
        //    {
        //        Response.Redirect("/ProductSearchList.aspx");
        //    }
        //}
        //protected void btnIndexPriceGo1_Click(object sender, ImageClickEventArgs e)
        //{
        //    Session["IndexPriceValue"] = null;
        //    Session["IndexFabricValue"] = null;
        //    Session["IndexPatternValue"] = null;
        //    Session["IndexStyleValue"] = null;
        //    Session["IndexColorValue"] = null;
        //    Session["IndexHeaderValue"] = null;
        //    Session["IndexCustomValue"] = null;

        //    string[] strkey = Request.Form.AllKeys;

        //    foreach (string strkeynew in strkey)
        //    {
        //        if (strkeynew.ToString().ToLower().IndexOf("chkprice") > -1 && Session["IndexPriceValue"] == null)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexPriceValue"] += ChkValue[0];
        //        }
        //        if (strkeynew.ToString().ToLower().IndexOf("chkpattern") > -1)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexPatternValue"] += ChkValue[0] + ",";
        //        }
        //        if (strkeynew.ToString().ToLower().IndexOf("chkfabric") > -1)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexFabricValue"] += ChkValue[0] + ",";
        //        }
        //        if (strkeynew.ToString().ToLower().IndexOf("chkstyle") > -1)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexStyleValue"] += ChkValue[0] + ",";
        //        }
        //        if (strkeynew.ToString().ToLower().IndexOf("chkheader") > -1)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexHeaderValue"] += ChkValue[0] + ",";
        //        }
        //        if (strkeynew.ToString().ToLower().IndexOf("chkcustom") > -1)
        //        {
        //            string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
        //            if (ChkValue.Length > 0)
        //                Session["IndexCustomValue"] += ChkValue[0].ToString();
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(hdnColorSelection.Value) && hdnColorSelection.Value != "")
        //    {
        //        Session["IndexColorValue"] = hdnColorSelection.Value.ToString();
        //    }
        //    hdnColorSelection.Value = "";

        //    if (Session["IndexPriceValue"] == null && Session["IndexFabricValue"] == null && Session["IndexPatternValue"] == null && Session["IndexStyleValue"] == null && Session["IndexColorValue"] == null && Session["IndexHeaderValue"] == null && Session["IndexCustomValue"] == null)
        //    {
        //        Response.Redirect("/ProductSearchList.aspx");
        //    }
        //    else
        //    {
        //        Response.Redirect("/ProductSearchList.aspx");
        //    }
        //}



    }
}