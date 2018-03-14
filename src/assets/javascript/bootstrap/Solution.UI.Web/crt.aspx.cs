using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web
{
    public partial class crt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(true);
        }
    }
}