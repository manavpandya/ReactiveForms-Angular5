using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;
using Solution.Bussines.Components;

namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT
{
    public partial class Webmethodcalling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetData(string SKU, String StoreId, Int32 IsActive)
        {
            if (StoreId.ToString() != "1")
            {
                string resp = string.Empty;
                CommonComponent.ExecuteCommonData("EXEC usp_Replenishment_productimport '" + SKU + "'," + StoreId + ", " + IsActive.ToString() + "," + HttpContext.Current.Session["AdminID"].ToString() + "");
            }
            return "";
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string RemoveSedhuler(string timestr, Int32 StoreId)
        {
            CommonComponent.ExecuteCommonData("update tb_FeedSchedular SET Active=0,deleted=1,UpdatedBy=" + HttpContext.Current.Session["AdminID"].ToString() + ",UpdatedOn=getdate() WHERE Storeid=" + StoreId + " AND SchedularTime='" + timestr.ToString() + "'");
            return "";
        }
    }
}