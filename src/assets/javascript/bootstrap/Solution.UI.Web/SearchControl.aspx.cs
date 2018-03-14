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
    public partial class SearchControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindPattern();
                BindFabric();
                BindStyle();
                BindColors();
                BindHeader();
                if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("productsearchlist.aspx") >= -1)
                {

                }
                else
                {
                    Session["IndexPriceValue"] = null;
                    Session["IndexFabricValue"] = null;
                    Session["IndexPatternValue"] = null;
                    Session["IndexStyleValue"] = null;
                    Session["IndexColorValue"] = null;
                    Session["IndexHeaderValue"] = null;
                    Session["IndexCustomValue"] = null;
                }

            }
        }
        private void BindHeader()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 5 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
            StrPattern = "<div class=\"toggle1\">";
            StrPattern += "<ul id=\"mycarousel7\" class=\"jcarousel-skin-tango2\">";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                Int32 icheck = 0;
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    string Imagename = Convert.ToString(dsPattern.Tables[0].Rows[i]["Imagename"]);

                    string strImageName = "";
                    String SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages");
                    if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                        Directory.CreateDirectory(Server.MapPath(SearchProductPath));
                    string strFilePath = Server.MapPath(SearchProductPath + Imagename);
                    Random rnd = new Random();
                    if (File.Exists(strFilePath))
                    {
                        strImageName = SearchProductPath + Imagename + "?" + rnd.Next(10000);
                    }
                    else
                    {
                        dsPattern.Tables[0].Rows.RemoveAt(i);
                        i--;
                        dsPattern.Tables[0].AcceptChanges();
                        continue;
                    }
                    if (icheck == 0)
                    {
                        StrPattern += "<li><ul class=\"option-pro\">";
                    }
                    if (icheck % 3 == 0 && icheck > 2)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro\">";
                        }
                    }
                    icheck++;
                    StrPattern += "<li class=\"header-pro-box\">";
                    StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkHeader_" + SearchId + "\">";
                    StrPattern += "<img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString() + "\"><span style=\"padding-left: 16px;\">" + SearchValue.ToString() + "</span></li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrHeader.Text = StrPattern.ToString();
        }

        private void BindPattern()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType=2 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 Order by ISNULL(DisplayOrder,999)");
            StrPattern = "<div class=\"toggle1\">";
            StrPattern += "<ul id=\"mycarousel3\" class=\"jcarousel-skin-tango2\">";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        StrPattern += "<li><ul class=\"option-pro\">";
                    }
                    if (i % 8 == 0 && i > 7)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro\">";
                        }
                    }

                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    StrPattern += "<li class=\"pattern-pro-box\">";
                    StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkPattern_" + SearchId + "\">";
                    StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrPattern.Text = StrPattern.ToString();
        }

        private void BindFabric()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 3 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
            StrPattern = "<div class=\"toggle1\">";
            StrPattern += "<ul id=\"mycarousel4\" class=\"jcarousel-skin-tango2\">";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        StrPattern += "<li><ul class=\"option-pro\">";
                    }
                    if (i % 8 == 0 && i > 7)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro\">";
                        }
                    }

                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    StrPattern += "<li class=\"pattern-pro-box\">";
                    StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkFabric_" + SearchId + "\">";
                    StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrFabric.Text = StrPattern.ToString();
        }

        private void BindStyle()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 4 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
            StrPattern = "<div class=\"toggle1\">";
            StrPattern += "<ul id=\"mycarousel5\" class=\"jcarousel-skin-tango2\">";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        StrPattern += "<li><ul class=\"option-pro\">";
                    }
                    if (i % 8 == 0 && i > 7)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro\">";
                        }
                    }

                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    StrPattern += "<li class=\"pattern-pro-box\">";
                    StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkStyle_" + SearchId + "\">";
                    StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrStyle.Text = StrPattern.ToString();
        }

        private void BindColors()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType =1 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
            StrPattern = "<div class=\"toggle1\">";
            StrPattern += "<ul id=\"mycarousel2\" class=\"jcarousel-skin-tango0\">";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                Int32 icheck = 0;
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    string Imagename = Convert.ToString(dsPattern.Tables[0].Rows[i]["Imagename"]);

                    string strImageName = "";
                    String SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages");
                    if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                        Directory.CreateDirectory(Server.MapPath(SearchProductPath));
                    string strFilePath = Server.MapPath(SearchProductPath + Imagename);
                    Random rnd = new Random();
                    if (File.Exists(strFilePath))
                    {
                        strImageName = SearchProductPath + Imagename + "?" + rnd.Next(10000);
                    }
                    else
                    {
                        dsPattern.Tables[0].Rows.RemoveAt(i);
                        i--;
                        dsPattern.Tables[0].AcceptChanges();
                        continue;
                    }
                    if (icheck == 0)
                    {
                        StrPattern += "<li><ul class=\"option-pro-color\" style=\"width:92px !important;\">";
                    }
                    if (icheck % 10 == 0 && icheck > 9)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro-color\" style=\"width:92px !important;\">";
                        }
                    }

                    icheck++;
                    StrPattern += "<li class=\"option-pro-box\"  style=\"padding-bottom:4px !important;\">";
                    StrPattern += "<a href=\"javascript:void(0);\" onclick=\"ColorSelection('" + SearchValue.ToString() + "');\"><img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString() + "\"></a> </li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrColor.Text = StrPattern.ToString();
        }

        protected void btnIndexPriceGo_Click(object sender, ImageClickEventArgs e)
        {
            Session["IndexPriceValue"] = null;
            Session["IndexFabricValue"] = null;
            Session["IndexPatternValue"] = null;
            Session["IndexStyleValue"] = null;
            Session["IndexColorValue"] = null;
            Session["IndexHeaderValue"] = null;
            Session["IndexCustomValue"] = null;

            if (!string.IsNullOrEmpty(txtFrom.Text.ToString().Trim()) || !string.IsNullOrEmpty(txtTo.Text.ToString().Trim()))
            {
                if (string.IsNullOrEmpty(txtFrom.Text.ToString().Trim()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Please Enter Valid Price.');document.getElementById('ContentPlaceHolder1_txtFrom').focus();", true);
                    return;
                }
                if (string.IsNullOrEmpty(txtTo.Text.ToString().Trim()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Tovalcal", "alert('Please Enter Valid Price.');document.getElementById('ContentPlaceHolder1_txtTo').focus();", true);
                    return;
                }
                decimal FromVal = Convert.ToDecimal(txtFrom.Text.Trim());
                decimal ToVal = Convert.ToDecimal(txtTo.Text.Trim());
                if (FromVal > ToVal)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Low Price should be Less than High Price.');document.getElementById('ContentPlaceHolder1_txtFrom').focus();", true);
                    return;
                }
                Session["IndexPriceValue"] = FromVal.ToString() + "-" + ToVal.ToString();
            }

            string[] strkey = Request.Form.AllKeys;

            foreach (string strkeynew in strkey)
            {
                if (strkeynew.ToString().ToLower().IndexOf("chkprice") > -1 && Session["IndexPriceValue"] == null)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexPriceValue"] += ChkValue[0];
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkpattern") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexPatternValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkfabric") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexFabricValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkstyle") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexStyleValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkheader") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexHeaderValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkcustom") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexCustomValue"] += ChkValue[0].ToString();
                }
            }

            if (!string.IsNullOrEmpty(hdnColorSelection.Value) && hdnColorSelection.Value != "")
            {
                Session["IndexColorValue"] = hdnColorSelection.Value.ToString();
            }
            hdnColorSelection.Value = "";

            if (Session["IndexPriceValue"] == null && Session["IndexFabricValue"] == null && Session["IndexPatternValue"] == null && Session["IndexStyleValue"] == null && Session["IndexColorValue"] == null && Session["IndexHeaderValue"] == null && Session["IndexCustomValue"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Please Select Search Criteria.');", true);
                return;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msglocation", "javascript:window.parent.location.href='/ProductSearchList.aspx';", true);
                // Response.Redirect("/ProductSearchList.aspx");
            }
        }
        protected void btnIndexPriceGo1_Click(object sender, ImageClickEventArgs e)
        {
            Session["IndexPriceValue"] = null;
            Session["IndexFabricValue"] = null;
            Session["IndexPatternValue"] = null;
            Session["IndexStyleValue"] = null;
            Session["IndexColorValue"] = null;
            Session["IndexHeaderValue"] = null;
            Session["IndexCustomValue"] = null;

            string[] strkey = Request.Form.AllKeys;

            foreach (string strkeynew in strkey)
            {
                if (strkeynew.ToString().ToLower().IndexOf("chkprice") > -1 && Session["IndexPriceValue"] == null)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexPriceValue"] += ChkValue[0];
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkpattern") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexPatternValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkfabric") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexFabricValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkstyle") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexStyleValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkheader") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexHeaderValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkcustom") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexCustomValue"] += ChkValue[0].ToString();
                }
            }

            if (!string.IsNullOrEmpty(hdnColorSelection.Value) && hdnColorSelection.Value != "")
            {
                Session["IndexColorValue"] = hdnColorSelection.Value.ToString();
            }
            hdnColorSelection.Value = "";

            if (Session["IndexPriceValue"] == null && Session["IndexFabricValue"] == null && Session["IndexPatternValue"] == null && Session["IndexStyleValue"] == null && Session["IndexColorValue"] == null && Session["IndexHeaderValue"] == null && Session["IndexCustomValue"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msglocation", "javascript:window.parent.location.href='/ProductSearchList.aspx';", true);
                // Response.Redirect("/ProductSearchList.aspx");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msglocation", "javascript:window.parent.location.href='/ProductSearchList.aspx';", true);
                // Response.Redirect("/ProductSearchList.aspx");
            }
        }
    }
}