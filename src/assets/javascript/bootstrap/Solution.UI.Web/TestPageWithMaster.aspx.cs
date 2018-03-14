using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Solution.UI.Web
{
    public partial class TestPageWithMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        protected void btnUpcupdate_Click(object sender, EventArgs e)
        {
            //Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
            //DataSet dsproduct = new DataSet();
            //dsproduct = objSql.GetDs("SELECT PRODUCTID,SKU FROM tb_Product WHERE isnull(Active,0)=1 AND isnull(deleted,0)=0 AND Storeid=" + txtId.Text.ToString() + "");
            //if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < dsproduct.Tables[0].Rows.Count; i++)
            //    {
            //        string strBarcode = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 UPC FROM tb_UPCMASTER WHERE isnull(SKU,'')='" + dsproduct.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "'"));
            //        if (strBarcode.Trim() == "")
            //        {
            //            strBarcode = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 UPC FROM tb_UPCMASTER WHERE isnull(SKU,'')=''"));
            //        }
            //        objSql.ExecuteNonQuery("UPDATE tb_Product SET UPC='" + strBarcode + "' WHERE SKU='" + dsproduct.Tables[0].Rows[i]["SKU"].ToString() + "'");
            //        objSql.ExecuteNonQuery("UPDATE tb_UPCMASTER SET SKU='" + dsproduct.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "' WHERE UPC = '" + strBarcode.ToString().Trim() + "'");
            //    }
            //}

            //Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
            //DataSet dsproduct = new DataSet();
            //Response.Write(Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 Isnull(FirstName,'') as FirstName FROM tb_Admin")));

        }
    }
}