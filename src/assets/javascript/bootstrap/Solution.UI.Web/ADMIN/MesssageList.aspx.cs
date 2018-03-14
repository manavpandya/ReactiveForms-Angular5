using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Components;
using System.IO;

namespace Solution.UI.Web.ADMIN
{
    public partial class MesssageList : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string Body = "";
                string url = AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/Admin/MessageBox.aspx";
                WebRequest NewWebReq = WebRequest.Create(url);
                WebResponse newWebRes = NewWebReq.GetResponse();
                string format = newWebRes.ContentType;
                Stream ftprespstrm = newWebRes.GetResponseStream();
                StreamReader reader;
                reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
                string strbiody = reader.ReadToEnd().ToString();
                Response.Clear();
                Response.Write(strbiody.ToString());
                Response.End();
            }
        }
    }
}