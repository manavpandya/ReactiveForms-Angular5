using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;

namespace Solution.UI.Web
{
    public partial class Rewriter : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Commented
            Session["PageName"] = "Content Not Found";
            Context.Items["Original_Path"] = "/Rewriter.aspx";
            Response.StatusCode = 404;
            #endregion

            #region Redirect Logic

            #region comman Variable

            String Url = Request.RawUrl.ToString();
            int totallenght = Url.Length;

            String hostname = Request.Url.Host.ToString();
            String Oldurl;

            #endregion
            if (Url.ToLower().IndexOf("?") > -1)
            {

                Oldurl = Url.Substring(0, Url.IndexOf("?"));
            }
            else if (Url.Contains("="))
            {
                int Finalindex = Request.RawUrl.LastIndexOf("=");
                int totallength = Request.RawUrl.Length;
                int diff = totallenght - (Finalindex + 1);
                Oldurl = Url.Substring(Finalindex + 1, diff);
            }
            else
            {
                Oldurl = Request.RawUrl.ToString();
            }
            String newurl = Convert.ToString(CommonComponent.GetScalarCommonData("select NewUrl from tb_PageRedirect where OldUrl like '" + Oldurl + "'"));
            if (newurl != null && newurl != "")
            {
                Context.Items["Original_Path"] = "/Rewriter.aspx";
                Response.Clear();
                Response.Status = "301 Moved Permanently";
                Response.AddHeader("Location", newurl);
            }
            //else if (Oldurl != null && Oldurl != "" && Oldurl.ToString().IndexOf(".html") > -1)
            //{
            //    Context.Items["Original_Path"] = "/ProductSearchList.aspx";
            //    Response.Clear();
            //    Response.Status = "301 Moved Permanently";
            //    Response.AddHeader("Location", Oldurl);
            //}
            else
            {
                Session["PageName"] = "Content Not Found";
                Context.Items["Original_Path"] = "/Rewriter.aspx";
                Response.StatusCode = 404;
            }
            #endregion

        }
    }
}