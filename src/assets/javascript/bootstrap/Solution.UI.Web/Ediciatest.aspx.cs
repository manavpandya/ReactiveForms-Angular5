using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.ShippingMethods;
using System.Data;

namespace Solution.UI.Web
{
    public partial class Ediciatest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            EndiciaService obj = new EndiciaService();

            DataTable dt = new DataTable();
            string msg = "";
            dt = obj.EndiciaGetRates("02451", "US", 2.3, ref msg);
        }
    }
}