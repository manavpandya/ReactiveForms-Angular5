using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.IO;

namespace Solution.UI.Web
{
    public partial class _63inchlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetproductDetail();
            }
        }
        public String GetIconImageCategory(String img)
        {
            try
            {
                String imagepath = String.Empty;
                imagepath = AppLogic.AppConfigs("ImagePathCategory") + "Icon/" + img;

                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") + imagepath))
                {
                    return AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
                }
                else
                {
                    return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
                }
                
            }
            catch
            {

            }
            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
        }
        public String GetmicroImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "micro/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "micro/image_not_available.jpg");
        }
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg");
        }
        private void GetproductDetail()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("SELECT Name,Sename,Imagename,CategoryId FROM tb_Category WHERE Storeid=1 and isnull(Active,0)=1 and Isnull(deleted,0)=0 and CategoryId in (SELECT CategoryId FROM tb_CategoryMapping WHERE ParentCategoryID in (SELECT categoryId FROM tb_Categorymapping WHERE ParentCategoryID=0 and  categoryId in (SELECT categoryId FROM tb_Category WHERE Storeid=1 and isnull(Active,0)=1 and Isnull(deleted,0)=0 and name like '%63Inch%'))) Order By DisplayOrder");
            string strhtml = "";



            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strhtml += "<div class=\"kt-row\">";
                    strhtml += "<div class=\"kt-cat-imgpro\">";
                    strhtml += "<a title=\"\" href=\"#\">";
                    strhtml += "<img title=\"\" alt=\"\" src=\"" + GetIconImageCategory(ds.Tables[0].Rows[i]["Imagename"].ToString()) + "\">";
                    strhtml += "</a>";
                    strhtml += "</div>";

                    strhtml += "<div class=\"kt-cat-content\">";
                    strhtml += "<div class=\"kt-cat-content-box\">";
                    strhtml += "<div class=\"kt-cat-content-title\"><a title=\"" + Server.HtmlEncode(ds.Tables[0].Rows[i]["Name"].ToString()) + "\" href=\"/" + ds.Tables[0].Rows[i]["Sename"].ToString() + ".html\">" + ds.Tables[0].Rows[i]["Name"].ToString() + "</a> </div>";
                    strhtml += "<div class=\"kt-cat-content-moreimg\">";


                    DataSet dsProduct = new DataSet();
                    dsProduct = CommonComponent.GetCommonDataSet("SELECT Name,ProductUrl,ProductId,ImageName FROM tb_Product WHERE Storeid=1 and isnull(Active,0)=1 and Isnull(deleted,0)=0 and ProductId in (SELECT ProductId FROM tb_ProductCategory WHERE CategoryId=" + ds.Tables[0].Rows[i]["CategoryId"].ToString() + ")");
                    if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                    {
                        strhtml += "<ul class=\"ulcatallimg\">";
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            strhtml += "<li>";
                            strhtml += "<a href=\"/" + ds.Tables[0].Rows[i]["ProductUrl"].ToString() + "\" title=\"" + Server.HtmlEncode(ds.Tables[0].Rows[i]["Name"].ToString()) + "\">";
                            strhtml += "<img src=\"" + GetmicroImageProduct(ds.Tables[0].Rows[i]["ImageName"].ToString()) + "\" alt=\"" + Server.HtmlEncode(ds.Tables[0].Rows[i]["Name"].ToString()) + "\" title=\"<div style='float:left;width:100%;'><a href='/" + ds.Tables[0].Rows[i]["ProductUrl"].ToString() + "' style='cursor:pointer' title=\"" + Server.HtmlEncode(ds.Tables[0].Rows[i]["Name"].ToString()) + "\"><img src='" + GetIconImageProduct(ds.Tables[0].Rows[i]["ImageName"].ToString()) + "' title=\"" + Server.HtmlEncode(ds.Tables[0].Rows[i]["Name"].ToString()) + "\" alt=\"" + Server.HtmlEncode(ds.Tables[0].Rows[i]["Name"].ToString()) + "\" /></a></div><div class='divtooltipover'><a href='/" + ds.Tables[0].Rows[i]["ProductUrl"].ToString() + "' style='cursor:pointer' title=\"" + Server.HtmlEncode(ds.Tables[0].Rows[i]["Name"].ToString()) + "\">" + ds.Tables[0].Rows[i]["Name"].ToString() + "</a></div>\" />";
                            strhtml += "</a>";
                            strhtml += "</li>";
                        }
                        strhtml += "</ul>";
                    }
                    strhtml += "</div>";
                    strhtml += "</div><img src=\"/images/sades-cat-box-shadow.jpg\" alt=\"\" title=\"\"></div></div>";
                    

                }

            }
            ltrproduct.Text = strhtml;


        }
    }
}