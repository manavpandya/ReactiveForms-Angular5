using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Solution.UI.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            ltCalcuation.Text = "";
            DataSet ds = new DataSet();
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
            ds = objSql.GetDs("EXEC usp_Product_Pricecalculator " + txtProductid.Text.ToString() + "," + txtWidth.Text.ToString() + "," + txtLength.Text.ToString() + "," + txtpanel.Text.ToString() + "");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltCalcuation.Text = "Price : $" + ds.Tables[0].Rows[0][0].ToString();
                ltCalcuation.Text += "<br />Yard : " + ds.Tables[0].Rows[0][1].ToString();
            }
        }
    }
}