using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web
{
    public partial class Videopopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["header"] != null)
                {
                    string vieopath = AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("VideoPath").ToString() + Request.QueryString["pid"].ToString() + "_1.mp4";
                    ltvide.Text = "<video width=\"100%\" height=\"340\" controls><source src=\"" + vieopath + "\" type=\"video/mp4\"></video>";
                }
                else if (Request.QueryString["product"] != null)
                {
                    string vieopath = AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("VideoPath").ToString() + Request.QueryString["pid"].ToString() + "_2.mp4";
                    ltvide.Text = "<video width=\"100%\" height=\"340\" controls><source src=\"" + vieopath + "\" type=\"video/mp4\"></video>";
                }
                else
                {
                    string vieopath = AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("VideoPath").ToString() + Request.QueryString["pid"].ToString() + ".mp4";
                    ltvide.Text = "<video width=\"100%\" height=\"340\" controls><source src=\"" + vieopath + "\" type=\"video/mp4\"></video>";
                }


                DataSet dsVideo = new DataSet();
                dsVideo = CommonComponent.GetCommonDataSet("SELECT ISNULL(VideoTitle,'') AS VideoTitle,ISNULL(Videodetail,'') AS Videodetail FROM tb_Product WHERE ProductId=" + Request.QueryString["pid"].ToString() + "");
                if (dsVideo != null && dsVideo.Tables.Count > 0 && dsVideo.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsVideo.Tables[0].Rows[0]["VideoTitle"].ToString()))
                    {
                        ltrVediodetail.Text = "<div class=\"readymade-detail-pt1-pro\">" + dsVideo.Tables[0].Rows[0]["VideoTitle"].ToString() + "</div>";
                    }
                    if (!string.IsNullOrEmpty(dsVideo.Tables[0].Rows[0]["Videodetail"].ToString()))
                    {
                        ltrVediodetail.Text += "<div class=\"static_content static-detail\" ><p>" + dsVideo.Tables[0].Rows[0]["Videodetail"].ToString() + "</p></div>";
                    }


                }
                else
                {
                    ltrVediodetail.Text = "";
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "window.parent.iframeAutoheight('frmdisplay1');", true);
            }
        }
    }
}