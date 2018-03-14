using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;

namespace Solution.UI.Web.Controls
{
    public partial class Dealoftheday : System.Web.UI.UserControl
    {
        string browser = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Web.HttpBrowserCapabilities browser1 = Request.Browser;
            browser = browser1.Browser.ToString();
            if (!IsPostBack)
            {
                if (Request.QueryString["PID"] != null)
                {
                    string strProductid = Convert.ToString(Request.QueryString["PID"]);
                    int Productid = 0;
                    bool isNum = int.TryParse(strProductid, out Productid);
                    if (isNum)
                    {
                        ViewState["HotDealProduct"] = AppLogic.AppConfigs("HotDealProduct").ToString();
                        if (Convert.ToInt32(ViewState["HotDealProduct"]) == Productid)
                        {
                            Timer1.Enabled = true;
                        }

                    }
                }
            }

        }
        protected void Timer1_Tick(object sender, System.EventArgs e)
        {

            // lblTime.Text = hh.ToString("D2") + " Hrs : " + mini.ToString("D2") + " Mins : " + Second.ToString("D2") + " Sec";
            string date = "";
            TimeSpan ts1 = TimeSpan.Parse("24:00:00");

            if (browser.ToString().ToLower().IndexOf("firefox") > -1)
            {
                ts1 = TimeSpan.Parse("24:00:00");

            }
            else
            {
                //ts1 = TimeSpan.Parse("12:00:00 AM");
                ///  date = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date.AddDays(1))) + " 12:00:00 AM";
            }

            //TimeSpan ts1 = TimeSpan.Parse();
            TimeSpan ts = TimeSpan.Parse(DateTime.Now.TimeOfDay.ToString());
            TimeSpan ts2 = ts1.Subtract(ts);
            Int32 hh = ts2.Hours;
            Int32 mini = ts2.Minutes;
            Int32 Second = ts2.Seconds;
            lblTime.Text = hh.ToString("D2") + " Hrs : " + mini.ToString("D2") + " Mins : " + Second.ToString("D2") + " Sec";


        }
    }
}