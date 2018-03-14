using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.Data;

namespace Solution.UI.Web
{
    public partial class MoreImages : System.Web.UI.Page
    {
        public string strMoreImg = "";

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProductMoreImages();
            }
        }

        /// <summary>
        /// Displays the More Images for Product
        /// </summary>
        private void ProductMoreImages()
        {
            if (Request.QueryString["PID"] != null)
            {

                DataSet dsImage = new DataSet();
                dsImage = ProductComponent.GetproductImagename(Convert.ToInt32(Request.QueryString["PID"]));
                if (dsImage != null && dsImage.Tables.Count > 0 && dsImage.Tables[0].Rows.Count > 0)
                {
                    string productname = dsImage.Tables[0].Rows[0]["Name"].ToString();
                    if (!string.IsNullOrEmpty(dsImage.Tables[0].Rows[0]["ImageName"].ToString()))
                    {
                        //string productImagename = dsImage.Tables[0].Rows[0]["ImageName"].ToString();
                        //strMoreImg += "<li><a title=\"" + Server.HtmlEncode(productname) + "\" href=\"" + AppLogic.AppConfigs("ImagePathProduct") + "Large/" + productImagename + "\"><img id='first' src=\"" + AppLogic.AppConfigs("ImagePathProduct") + "micro/" + productImagename + "\" alt=\"" + Server.HtmlEncode(productname) + "\" width=\"72\"/></a></li>";
                        //ltimg.Text = "<img style=\"width:320px;\" src=\"" + AppLogic.AppConfigs("ImagePathProduct") + "Large/" + productImagename + "\" alt=\"" + Server.HtmlEncode(productname) + "\" />";
                        //string[] strname = { ".bmp", ".jpg", ".jpeg", ".gif", ".png" };
                        //for (int cnt = 1; cnt < 26; cnt++)
                        //{
                        //    foreach (string str in strname)
                        //    {
                        //        System.IO.FileInfo fl = new System.IO.FileInfo(productImagename);
                        //        string imglarge = AppLogic.AppConfigs("ImagePathProduct") + "Large/" + productImagename.Replace(fl.Extension, "") + "_" + cnt.ToString() + str.ToString();
                        //        string imgMicro = AppLogic.AppConfigs("ImagePathProduct") + "Micro/" + productImagename.Replace(fl.Extension, "") + "_" + cnt.ToString() + str.ToString();
                        //        if (System.IO.File.Exists(Server.MapPath(imglarge)) && System.IO.File.Exists(Server.MapPath(imgMicro)))
                        //        {
                        //            strMoreImg += "<li><a title=\"" + Server.HtmlEncode(productname) + "\" href=\"" + imglarge + "\"><img  src=\"" + imgMicro + "\" alt=\"" + Server.HtmlEncode(productname) + "\" width=\"72\"  /></a></li>";
                        //        }
                        //    }
                        //}


                        string productImagename = dsImage.Tables[0].Rows[0]["ImageName"].ToString();
                        if (Request.QueryString["img"] != null)
                        {
                            strMoreImg += "<li><a title=\"" + Server.HtmlEncode(productname) + "\" href=\"" + Request.QueryString["img"].ToString().Replace("medium", "large") + "\"></li>";
                        }
                        else
                        {
                            strMoreImg += "<li><a title=\"" + Server.HtmlEncode(productname) + "\" href=\"" + AppLogic.AppConfigs("ImagePathProduct") + "Large/" + productImagename + "\"></li>";
                        }
                    }
                }

            }
        }
    }
}